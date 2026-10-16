Set-StrictMode -Version Latest

# verify/publish.ps1 - The publish-readiness gate: no identity, binaries or local state in the shipped tree.
# Dot-sourced by Invoke-Verify.ps1 and the tests; it defines functions only and runs nothing.
. (Join-Path $PSScriptRoot '..\common\core.ps1')

# ---------------------------------------------------------------------------------------------
# checks/Publish.ps1
# ---------------------------------------------------------------------------------------------
# Is this tree safe to copy into a public repository?
#
# Everything here is a mechanical check that was previously "someone should look at that":
#   * no binaries (dotnet/dotnet runs DetectBinaries and prep-source-build removes unlisted ones)
#   * no identity or secrets (an email address, a user profile path, a token)
#   * no per-QB state (config, history, reports) beside the scripts, and none under local/ that Git tracks or has staged
#     (an ignore rule does not protect a file the index already holds)
#   * every script or file a document points at actually exists
#   * every script parses, every SKILL.md has valid front matter
#   * a size budget, and an inventory of internal links so a human can decide if they are publishable
# Findings have a severity: fail (do not publish), warn (a person should decide), info (inventory).

$script:VmrQBPublishExampleMailboxes = @('you', 'qb', 'next.qb', 'first.last', 'someone', 'user', 'name', 'example', 'recipient', 'sender', 'noreply')
$script:VmrQBPublishBinaryExtensions = @('.dll', '.exe', '.so', '.dylib', '.pdb', '.nupkg', '.snupkg', '.zip', '.7z', '.gz', '.tar', '.db', '.sqlite', '.sqlite3', '.bin', '.obj', '.lib', '.a', '.jar', '.class', '.pfx', '.cer', '.p12')
$script:VmrQBPublishStateNames = @('qb-config.json', 'qb-config.local.json', 'routing.local.json', 'issue-history.json', 'issue-history.db', 'reported-links.json', 'run-context.json')
$script:VmrQBPublishStateDirectories = @('reports', 'logs', 'artifacts', 'run')

function New-VmrQBPublishFinding {
    param([string] $Severity, [string] $Check, [string] $Path, [string] $Message, [int] $Line = 0)
    [pscustomobject]@{ severity = $Severity; check = $Check; path = $Path; line = $Line; message = $Message }
}

function Test-VmrQBFileIsBinary {
    param([Parameter(Mandatory)][string] $Path)
    $stream = [IO.File]::OpenRead($Path)
    try {
        $buffer = [byte[]]::new([Math]::Min(8192, [Math]::Max(1, $stream.Length)))
        $read = $stream.Read($buffer, 0, $buffer.Length)
        if ($read -ge 2 -and (($buffer[0] -eq 0xFF -and $buffer[1] -eq 0xFE) -or ($buffer[0] -eq 0xFE -and $buffer[1] -eq 0xFF))) { return $false }
        for ($i = 0; $i -lt $read; $i++) { if ($buffer[$i] -eq 0) { return $true } }
        return $false
    }
    finally { $stream.Dispose() }
}

function Get-VmrQBGitIndexedLocalState {
    <#
    .SYNOPSIS
    The files under <folder>/local that Git tracks or has staged, for each given top-level folder of Root.
    .DESCRIPTION
    An ignore rule keeps new files out of Git; it does nothing for a file the index already holds (tracked, or force-added). So an
    ignored local/ is not proof that nothing private would be published: this asks Git what it would commit. It returns nothing
    when Git cannot answer (it is not installed, or Root is not inside a work tree): there is no index to leak through.
    #>
    param([Parameter(Mandatory)][string] $Root, [AllowEmptyCollection()][string[]] $Folder = @())
    if (-not $Folder.Count -or -not (Get-Command git -ErrorAction SilentlyContinue)) { return @() }
    $null = & git -C $Root rev-parse --show-toplevel 2>$null
    if ($LASTEXITCODE -ne 0) { return @() }
    $prefix = ([string](& git -C $Root rev-parse --show-prefix 2>$null)).Trim()
    $found = [Collections.Generic.List[object]]::new()
    foreach ($name in $Folder) {
        $listed = @(& git -c core.quotepath=false --literal-pathspecs -C $Root ls-files --cached -- "$name/local" 2>$null)
        if ($LASTEXITCODE -eq 0 -and $listed.Count) {
            $found.Add([pscustomobject]@{ folder = $name; files = $listed; repoPath = "$prefix$name/local" })
        }
    }
    return $found.ToArray()
}

function Invoke-VmrQBPublishReadiness {
    <#
    .SYNOPSIS
    Scans a skills tree and returns findings (severity fail | warn | info).
    .PARAMETER Root
    The directory that will be copied, normally <repo>/.github/skills.
    .PARAMETER SkillFolder
    Only scan these top-level folders under Root. Inside dotnet/dotnet the skills tree also holds other
    teams' skills, which are not this gate's business.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Root,
        [int] $MaxFileKB = 256,
        [int] $MaxTotalKB = 1536,
        [string[]] $SkillFolder
    )
    if (-not (Test-Path -LiteralPath $Root -PathType Container)) { throw "Publish root not found: $Root" }
    $Root = (Resolve-Path -LiteralPath $Root).Path
    $findings = [Collections.Generic.List[object]]::new()
    $files = @(Get-ChildItem -LiteralPath $Root -Recurse -File -Force)
    if ($SkillFolder) {
        $files = @($files | Where-Object { ($_.FullName.Substring($Root.Length).TrimStart('\', '/') -split '[\\/]')[0] -in $SkillFolder })
        # Fail closed: a requested folder that is absent or empty must never read as "nothing to publish, all clear".
        foreach ($requested in $SkillFolder) {
            $scanned = @($files | Where-Object { ($_.FullName.Substring($Root.Length).TrimStart('\', '/') -split '[\\/]')[0] -eq $requested }).Count
            if ($scanned -eq 0) {
                $findings.Add((New-VmrQBPublishFinding 'fail' 'scope' $requested "No files were scanned under '$requested' in $Root. The folder is missing or the scan root is wrong, so nothing was checked."))
            }
        }
    }
    $rel = { param($file) $file.FullName.Substring($Root.Length).TrimStart('\', '/') }
    $topFolders = @($files | ForEach-Object { ((& $rel $_) -split '[\\/]')[0] } | Select-Object -Unique)

    # --- local data: one folder per skill, <skill>/local/, which must be git-ignored ---------------------------
    # Everything one QB creates for their own sessions lives there (config, history, reports, roster, findings).
    # An ignored local/ is not published, so it is not scanned; an unignored one is a blocker.
    $skillFolders = @($files | ForEach-Object { (& $rel $_) -replace '\\', '/' } | Where-Object { $_ -match '^[^/]+/local/' } | ForEach-Object { ($_ -split '/')[0] } | Select-Object -Unique)
    foreach ($skillFolder in $skillFolders) {
        $ignoreFile = Join-Path (Join-Path $Root $skillFolder) '.gitignore'
        $listed = (Test-Path -LiteralPath $ignoreFile) -and @(@([IO.File]::ReadAllLines($ignoreFile) | ForEach-Object { $_.Trim() }) | Where-Object { $_ -in '/local/', '/local' }).Count -gt 0
        if ($listed) { $files = @($files | Where-Object { ((& $rel $_) -replace '\\', '/') -notmatch "^$([regex]::Escape($skillFolder))/local/" }) }
        else { $findings.Add((New-VmrQBPublishFinding 'fail' 'state' "$skillFolder/local" "This folder holds one QB's own data (identity, history, reports, roster) and is not git-ignored. Add '/local/' to $skillFolder/.gitignore."))}
    }
    # Ignoring local/ is not enough when Git already tracks or has staged something in it (a force-add, or a file added before the
    # rule existed): the next commit would publish it, whatever .gitignore says.
    foreach ($tracked in @(Get-VmrQBGitIndexedLocalState -Root $Root -Folder $topFolders)) {
        $shown = (@($tracked.files | Select-Object -First 3)) -join ', '
        $more = if ($tracked.files.Count -gt 3) { " and $($tracked.files.Count - 3) more" } else { '' }
        $findings.Add((New-VmrQBPublishFinding 'fail' 'state' "$($tracked.folder)/local" "Git tracks or has staged $($tracked.files.Count) file(s) under $($tracked.folder)/local/ ($shown$more). An ignore rule does not protect a file the index already holds, so one QB's private data would be published. Stop tracking them and keep your copies: git rm -r --cached -- $($tracked.repoPath)  (add -f if Git refuses)."))
    }
    # Files that belong in local/ but sit in the shipped folders. Moving them is the fix; ignoring them is not enough.
    $belongsInLocal = @{
        'knowledge/issue-history.seed.json'  = "The history seed is one QB's exported triage history."
        'knowledge/team-contacts.json'       = 'The contact roster names people.'
        'knowledge/triage-context.json'      = 'Verified findings come from one QB''s rotations and go stale; each QB records their own.'
        'knowledge/routing.local.json'       = 'Your routing copy holds your own incident notes and edits.'
        'knowledge/issue-aliases.json'       = 'Alias groups are finding names from one QB''s reports; they drift.'
        'references/guide-update-draft.md'   = 'A working draft for the maintainers'' guide update, not skill content.'
    }
    foreach ($misplaced in @($files)) {
        $parts = @(((& $rel $misplaced) -replace '\\', '/') -split '/')
        if ($parts.Count -lt 3) { continue }
        $inside = ($parts[1..($parts.Count - 1)]) -join '/'
        if ($belongsInLocal.ContainsKey($inside)) {
            $findings.Add((New-VmrQBPublishFinding 'fail' 'state' (& $rel $misplaced) "$($belongsInLocal[$inside]) Move it into $($parts[0])/local/, which is git-ignored."))
        }
    }
    # --- the shipped qb-config.template.json is a blank copy source: it must never carry an address ---------------
    foreach ($template in @($files | Where-Object { $_.Name -eq 'qb-config.template.json' })) {
        $relative = & $rel $template
        if (@($relative -split '[\\/]').Count -ne 2) { continue }
        try {
            $config = Get-Content -LiteralPath $template.FullName -Raw | ConvertFrom-Json
            if (-not [string]::IsNullOrWhiteSpace([string](Get-VmrQBField $config 'recipient'))) {
                $findings.Add((New-VmrQBPublishFinding 'fail' 'identity' $relative "The public template has a non-empty 'recipient'. It must stay blank; users copy it to local/qb-config.local.json and add their own address there."))
            }
        }
        catch { $findings.Add((New-VmrQBPublishFinding 'fail' 'schema' $relative "The template is not valid JSON: $($_.Exception.Message)")) }
    }
    # --- binaries ------------------------------------------------------------------------------
    foreach ($file in $files) {
        if ($file.Extension.ToLowerInvariant() -in $script:VmrQBPublishBinaryExtensions -or (Test-VmrQBFileIsBinary -Path $file.FullName)) {
            $findings.Add((New-VmrQBPublishFinding 'fail' 'binary' (& $rel $file) 'Binary file. dotnet/dotnet runs DetectBinaries and prep-source-build removes unlisted binaries; ship text only.'))
        }
    }

    # --- per-QB state beside the scripts -------------------------------------------------------
    foreach ($file in $files) {
        $relative = & $rel $file
        $segments = $relative -split '[\\/]'
        $reason = $null
        if ($file.Name -in $script:VmrQBPublishStateNames) { $reason = "$($file.Name) is one QB's state" }
        elseif ($file.Name -match '\.(lock|tmp|bak)$' -or $file.Name -match '^issue-history\.json\.') { $reason = 'lock/temp/backup file' }
        elseif (@($segments | Select-Object -SkipLast 1 | Where-Object { $_ -in $script:VmrQBPublishStateDirectories }).Count -gt 0 -and $segments[0] -notin @('references') ) {
            $directory = @($segments | Select-Object -SkipLast 1 | Where-Object { $_ -in $script:VmrQBPublishStateDirectories })[0]
            if ($relative -notmatch '(^|[\\/])tests[\\/]fixtures[\\/]') { $reason = "'$directory' is a runtime output folder" }
        }
        if ($reason) { $findings.Add((New-VmrQBPublishFinding 'fail' 'state' $relative "State belongs in the skill's git-ignored local/ folder ($reason).")) }
    }

    # --- text scans: identity, secrets, paths, links -------------------------------------------
    $emailPattern = [regex]'(?<![\w.%+-])([A-Za-z0-9._%+-]+)@([A-Za-z0-9-]+(?:\.[A-Za-z0-9-]+)*\.[A-Za-z]{2,})'
    $secretPatterns = @(
        @{ name = 'GitHub token'; pattern = [regex]'\b(?:ghp|gho|ghu|ghs|ghr)_[A-Za-z0-9]{30,}\b|\bgithub_pat_[A-Za-z0-9_]{40,}\b' },
        @{ name = 'JWT / bearer token'; pattern = [regex]'\beyJ[A-Za-z0-9_-]{15,}\.[A-Za-z0-9_-]{15,}\.[A-Za-z0-9_-]{5,}' },
        @{ name = 'private key'; pattern = [regex]'-----BEGIN [A-Z ]*PRIVATE KEY-----' },
        @{ name = 'storage/connection secret'; pattern = [regex]'(?i)\b(?:AccountKey|SharedAccessKey|Password|Pwd)\s*=\s*[A-Za-z0-9+/=_-]{12,}' },
        @{ name = 'SAS signature'; pattern = [regex]'[?&]sig=[A-Za-z0-9%+/=]{20,}' }
    )
    $userPathPattern = [regex]'(?i)\b[A-Z]:\\Users\\(?!Public\b|Default\b|<|%|\$|cloudtest\b|\.\.\.|name\b|user\b|you\b|\*)[A-Za-z0-9._-]+|/(?:home|Users)/(?!runner\b|vsts\b|<|name\b|user\b|you\b)[a-z0-9._-]+/'
    $machinePathPattern = [regex]'(?i)\b[A-Z]:\\(?:projects|src|repos|git|work)\\[A-Za-z0-9._-]+'
    # A path into a Copilot CLI session folder is private to one person's machine and session. (The per-user plugin folder is a normal install location, so it is not matched.)
    $sessionPathPattern = [regex]'(?i)\.copilot[\\/]+session-state|session-state[\\/]+[0-9a-f]{8}-[0-9a-f]{4}'
    $teamsLinkPattern = [regex]'(?i)teams\.microsoft\.com/l/|@thread\.(?:skype|tacv2|v2)\b'
    $internalPatterns = [ordered]@{
        'internal Azure DevOps links (dnceng/internal, mseng, devdiv)' = [regex]'(?i)dev\.azure\.com/(?:dnceng/internal|mseng|devdiv)\b'
        'SharePoint / OneDrive'      = [regex]'(?i)\b[\w-]+\.sharepoint\.com|onedrive\.live\.com|\b[\w-]+-my\.sharepoint\.com'
        'corp hostnames'             = [regex]'(?i)\b[\w-]+\.(?:corp|redmond)\.microsoft\.com\b'
    }
    $internalCounts = @{}
    $totalBytes = 0L
    foreach ($file in $files) {
        $totalBytes += $file.Length
        $relative = & $rel $file
        $kb = [Math]::Ceiling($file.Length / 1KB)
        if ($kb -gt $MaxFileKB) {
            $findings.Add((New-VmrQBPublishFinding 'warn' 'size' $relative "$kb KB exceeds the $MaxFileKB KB per-file budget. A reviewer should decide whether this belongs in the repository."))
        }
        if ($file.Extension.ToLowerInvariant() -in $script:VmrQBPublishBinaryExtensions -or (Test-VmrQBFileIsBinary -Path $file.FullName)) { continue }
        $text = [IO.File]::ReadAllText($file.FullName)
        $lines = $text -split "`r?`n"
        # editorconfig asks for a final newline; a file that mixes CRLF and LF makes every later edit a noisy diff.
        if ($text.Contains("`r`n") -and [regex]::IsMatch($text, '(?<!\r)\n')) {
            $findings.Add((New-VmrQBPublishFinding 'warn' 'format' $relative 'Mixes CRLF and LF line endings. Normalize it to one (the rest of the tree uses LF).'))
        }
        if ($text.Length -gt 0 -and -not $text.EndsWith("`n")) {
            $findings.Add((New-VmrQBPublishFinding 'warn' 'format' $relative 'No newline at the end of the file.'))
        }

        for ($i = 0; $i -lt $lines.Count; $i++) {
            $line = $lines[$i]
            foreach ($m in $emailPattern.Matches($line)) {
                $local = $m.Groups[1].Value.ToLowerInvariant(); $domain = $m.Groups[2].Value.ToLowerInvariant()
                if ($local -in $script:VmrQBPublishExampleMailboxes -or $domain -in @('example.com', 'example.co.uk', 'example.org', 'users.noreply.github.com', 'noreply.github.com')) { continue }
                # A Teams conversation id (19:<id>@thread.<suffix>) is not a mailbox; the Teams-permalink check owns it.
                if ($domain -match '^thread\.(?:skype|tacv2|v2)$') { continue }
                # An @-prefixed handle such as @dotnet/team is not an address; the regex needs a dotted domain, so none match here.
                $findings.Add((New-VmrQBPublishFinding 'fail' 'identity' $relative "Email address '$($m.Value)'. Identity belongs only in your git-ignored local/qb-config.local.json." ($i + 1)))
            }
            foreach ($secret in $secretPatterns) {
                if ($secret.pattern.IsMatch($line)) { $findings.Add((New-VmrQBPublishFinding 'fail' 'secret' $relative "Looks like a $($secret.name). Remove it and rotate it." ($i + 1))) }
            }
            if ($userPathPattern.IsMatch($line)) {
                $findings.Add((New-VmrQBPublishFinding 'fail' 'identity' $relative "A personal profile path ('$($userPathPattern.Match($line).Value)') identifies a person and a machine." ($i + 1)))
            }
            if ($teamsLinkPattern.IsMatch($line)) {
                $findings.Add((New-VmrQBPublishFinding 'fail' 'internal-link' $relative 'Teams message permalink / conversation id. It identifies a private internal chat; keep it in the QB-local reported-links.json, never in a published file.' ($i + 1)))
            }
            if ($sessionPathPattern.IsMatch($line)) {
                $findings.Add((New-VmrQBPublishFinding 'fail' 'identity' $relative "A path into a private Copilot session folder ('$($sessionPathPattern.Match($line).Value)') exists only on one machine." ($i + 1)))
            }
            if ($machinePathPattern.IsMatch($line)) {
                $findings.Add((New-VmrQBPublishFinding 'warn' 'path' $relative "Machine-specific path '$($machinePathPattern.Match($line).Value)'. Use a neutral example." ($i + 1)))
            }
            foreach ($name in $internalPatterns.Keys) {
                $hits = $internalPatterns[$name].Matches($line).Count
                if ($hits) { $internalCounts[$name] = [int]$(if ($internalCounts.ContainsKey($name)) { $internalCounts[$name] } else { 0 }) + $hits }
            }
        }
    }
    if (($totalBytes / 1KB) -gt $MaxTotalKB) {
        $findings.Add((New-VmrQBPublishFinding 'warn' 'size' '(tree)' "Total $([Math]::Ceiling($totalBytes / 1KB)) KB exceeds the $MaxTotalKB KB budget; the existing project skills total about 80 KB."))
    }
    foreach ($name in $internalCounts.Keys) {
        $findings.Add((New-VmrQBPublishFinding 'info' 'internal-links' '(tree)' "$($internalCounts[$name]) reference(s): $name. dotnet/dotnet is public; confirm these may be published."))
    }

    # --- scripts parse; front matter; documents point at things that exist ---------------------
    foreach ($file in @($files | Where-Object Extension -eq '.ps1')) {
        $tokens = $null; $parseErrors = $null
        [void][Management.Automation.Language.Parser]::ParseFile($file.FullName, [ref]$tokens, [ref]$parseErrors)
        foreach ($err in @($parseErrors)) {
            $findings.Add((New-VmrQBPublishFinding 'fail' 'parse' (& $rel $file) "PowerShell parse error: $($err.Message)" $err.Extent.StartLineNumber))
        }
    }
    $names = @($files | ForEach-Object Name)
    foreach ($skill in @($files | Where-Object { $_.Name -eq 'SKILL.md' })) {
        $relative = & $rel $skill
        $content = [IO.File]::ReadAllText($skill.FullName)
        $front = [regex]::Match($content, '(?s)\A---\r?\n(.*?)\r?\n---\r?\n')
        if (-not $front.Success) { $findings.Add((New-VmrQBPublishFinding 'fail' 'frontmatter' $relative 'SKILL.md must start with a --- front-matter block.')); continue }
        $fields = @{}
        $frontLines = @($front.Groups[1].Value -split '\r?\n')
        for ($lineIndex = 0; $lineIndex -lt $frontLines.Count; $lineIndex++) {
            $field = [regex]::Match($frontLines[$lineIndex], '^(?<k>[A-Za-z_-]+):\s*(?<v>.*?)\s*$')
            if (-not $field.Success) { continue }
            $value = $field.Groups['v'].Value
            # A YAML block scalar (description: >- or |): the indented lines that follow are the value, joined by spaces (folded) or newlines (literal).
            if ($value -match '^[>|][+-]?$') {
                $block = [Collections.Generic.List[string]]::new()
                while ($lineIndex + 1 -lt $frontLines.Count -and $frontLines[$lineIndex + 1] -match '^(\s+\S.*|\s*)$') { $lineIndex++; if ($frontLines[$lineIndex].Trim()) { $block.Add($frontLines[$lineIndex].Trim()) } }
                $value = ($block -join $(if ($value.StartsWith('>')) { ' ' } else { "`n" })).Trim()
            }
            if ($value) { $fields[$field.Groups['k'].Value] = $value }
        }
        $folder = Split-Path -Leaf (Split-Path -Parent $skill.FullName)
        if (-not $fields.ContainsKey('name')) { $findings.Add((New-VmrQBPublishFinding 'fail' 'frontmatter' $relative "front matter has no 'name'.")) }
        elseif ($fields['name'].Trim('"''') -ne $folder) { $findings.Add((New-VmrQBPublishFinding 'fail' 'frontmatter' $relative "name '$($fields['name'])' must equal the folder name '$folder'.")) }
        if (-not $fields.ContainsKey('description') -or $fields['description'].Length -lt 40) { $findings.Add((New-VmrQBPublishFinding 'fail' 'frontmatter' $relative "front matter needs a meaningful 'description' (it is what triggers the skill).")) }
        elseif ($fields['description'].Length -gt 1024) { $findings.Add((New-VmrQBPublishFinding 'warn' 'frontmatter' $relative "description is $($fields['description'].Length) chars; keep it under 1024.")) }
    }
    $skillRootOf = {
        param($file)
        $directory = Split-Path -Parent $file.FullName
        while ($directory) {
            if (Test-Path -LiteralPath (Join-Path $directory 'SKILL.md')) { return $directory }
            $directory = Split-Path -Parent $directory
        }
        return $null
    }
    foreach ($doc in @($files | Where-Object Extension -eq '.md')) {
        $relative = & $rel $doc
        $lines = [IO.File]::ReadAllText($doc.FullName) -split "`r?`n"
        $docSkillRoot = & $skillRootOf $doc
        for ($i = 0; $i -lt $lines.Count; $i++) {
            # A script named in a document: bare (Do-Thing.ps1), directly after ./ (./Do-Thing.ps1) or with folders
            # (./daily/Invoke-Daily.ps1). The name must exist somewhere in this tree, and a path-qualified one must also resolve
            # from the scripts folder (the guides say to run from there), the skill root or the document's own folder.
            foreach ($m in [regex]::Matches($lines[$i], '(?<![\w])(?<path>(?:\.{1,2}[\\/])?(?:[A-Za-z0-9_-]+[\\/])*)(?<name>[A-Za-z][A-Za-z0-9_-]*\.ps1)\b')) {
                $name = $m.Groups['name'].Value
                $path = $m.Groups['path'].Value
                if ($name -notin $names) {
                    $findings.Add((New-VmrQBPublishFinding 'fail' 'dangling-reference' $relative "References '$($path)$name', which does not exist in this tree." ($i + 1)))
                    continue
                }
                if ($path -and $path -notmatch '^[A-Za-z]:' ) {
                    $candidates = @(Join-Path (Split-Path -Parent $doc.FullName) "$path$name")
                    if ($docSkillRoot) {
                        $candidates += Join-Path $docSkillRoot "$path$name"
                        $candidates += Join-Path (Join-Path $docSkillRoot 'scripts') "$path$name"
                        # A dependency drawn relative to one of the workflow folders (../common/core.ps1 from scripts/daily).
                        foreach ($workflow in @(Get-ChildItem -LiteralPath (Join-Path $docSkillRoot 'scripts') -Directory -ErrorAction SilentlyContinue)) { $candidates += Join-Path $workflow.FullName "$path$name" }
                    }
                    if (-not @($candidates | Where-Object { Test-Path -LiteralPath $_ -PathType Leaf }).Count) {
                        $findings.Add((New-VmrQBPublishFinding 'fail' 'dangling-reference' $relative "References '$($path)$name', but no such path exists (the script is elsewhere in the tree)." ($i + 1)))
                    }
                }
            }
            foreach ($m in [regex]::Matches($lines[$i], '\]\((?<target>[^)\s#]+)(?:#[^)]*)?\)')) {
                $target = $m.Groups['target'].Value
                if ($target -match '^(?:[a-z][a-z0-9+.-]*:|mailto:|#)') { continue }
                if ($target -match '^<[^>]+>$') { continue }   # a documented placeholder such as [<name>](<url>), not a file link
                $resolved = Join-Path (Split-Path -Parent $doc.FullName) $target
                if (-not (Test-Path -LiteralPath $resolved)) {
                    $findings.Add((New-VmrQBPublishFinding 'fail' 'dangling-reference' $relative "Link target '$target' does not exist." ($i + 1)))
                }
            }
        }
    }
    return $findings.ToArray()
}
