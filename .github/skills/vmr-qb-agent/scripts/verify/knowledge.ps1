Set-StrictMode -Version Latest

# verify/knowledge.ps1 - The knowledge-pack check: schema, handles, ages, and (with -Online) live team/contact/issue state.
# Dot-sourced by Invoke-Verify.ps1 and the tests; it defines functions only and runs nothing.
. (Join-Path $PSScriptRoot '..\common\core.ps1')

# ---------------------------------------------------------------------------------------------
# checks/Knowledge.ps1
# ---------------------------------------------------------------------------------------------
# Is the knowledge pack still true?
#
# The pack (routing, plus any local triage-context) is a starting point, and facts in it
# go stale: handles get renamed, issues close, fixes merge. This check replaces "someone should
# re-verify that" with mechanical findings:
#   offline  schema, duplicate ids, handles used in docs/knowledge vs the routing registry, entry age
#   -Online  every team handle resolves, every contact is assignable, every linked issue/PR state agrees
#            with what the entry says about it
# Severity: fail (wrong or unusable), warn (probably stale, a person should look), info.


function New-VmrQBKnowledgeFinding {
    param([string] $Severity, [string] $Check, [string] $Subject, [string] $Message)
    [pscustomobject]@{ severity = $Severity; check = $Check; subject = $Subject; message = $Message }
}

function Get-VmrQBDefaultTeamLookup {
    return {
        param($slug)
        $output = & gh api "orgs/dotnet/teams/$slug" --jq '.slug' 2>&1 | Out-String
        if ($LASTEXITCODE -eq 0) { return $true }
        if ($output -match 'HTTP 404') { return $false }
        return $null
    }
}

function Get-VmrQBDefaultUserLookup {
    return {
        param($login)
        & gh api "repos/dotnet/dotnet/assignees/$login" 2>&1 | Out-Null
        if ($LASTEXITCODE -eq 0) { return $true }
        $text = & gh api "repos/dotnet/dotnet/assignees/$login" 2>&1 | Out-String
        if ($text -match 'HTTP 404') { return $false }
        return $null
    }
}

function Get-VmrQBDefaultIssueLookup {
    return {
        param($owner, $repo, $number, $kind)
        if ($kind -eq 'pull') {
            $json = & gh api "repos/$owner/$repo/pulls/$number" --jq '{state: .state, merged: .merged}' 2>&1 | Out-String
            if ($LASTEXITCODE -ne 0) { return $null }
            $item = $json | ConvertFrom-Json
            if ($item.merged) { return 'merged' }
            return [string]$item.state
        }
        $state = & gh api "repos/$owner/$repo/issues/$number" --jq '.state' 2>&1 | Out-String
        if ($LASTEXITCODE -ne 0) { return $null }
        return $state.Trim()
    }
}

# A line that names a non-team handle only to warn against it ("not @dotnet/sdk", "404", "use X instead")
# is documentation of the mistake, not an instruction to ping it.
$script:VmrQBNegationPattern = [regex]'(?i)\b(?:not|never|no such|does not exist|doesn''t exist|nonexistent|non-existent|404|mistaken|invalid|instead of|rather than|is not a team|are not teams|there is no|use @dotnet|use `@dotnet)\b|\(use\s'

function Test-VmrQBHandleMentions {
    <#
    .SYNOPSIS
    Finds @dotnet/<x> mentions that are not real teams and are not merely being warned against.
    #>
    param([Parameter(Mandatory)][string] $Path, [string] $RelativeName, [object] $Routing)
    $routing = if ($Routing) { $Routing } else { Get-VmrQBRouting }
    $findings = [Collections.Generic.List[object]]::new()
    $lines = [IO.File]::ReadAllText($Path) -split "`r?`n"
    $inGenerated = $false
    for ($i = 0; $i -lt $lines.Count; $i++) {
        # A block generated from routing.json lists the bad handles on purpose, and ./verify/Invoke-Verify.ps1
        # already proves it matches the registry.
        if ($lines[$i] -match '<!-- BEGIN GENERATED:') { $inGenerated = $true; continue }
        if ($lines[$i] -match '<!-- END GENERATED:') { $inGenerated = $false; continue }
        if ($inGenerated) { continue }
        foreach ($unknown in @(Find-VmrQBUnknownHandles -Text $lines[$i] -Routing $routing)) {
            # The warning must sit right next to the handle. A "not" three columns away in a table row
            # says nothing about this handle (the triage skill's routing table had exactly that).
            $from = [Math]::Max(0, $unknown.index - 24)
            $window = $lines[$i].Substring($from, [Math]::Min($lines[$i].Length - $from, ($unknown.index - $from) + $unknown.handle.Length + 40))
            $warned = $script:VmrQBNegationPattern.IsMatch($window)
            $hint = if ($unknown.suggestion) { " Use $($unknown.suggestion)." } else { '' }
            if ($warned) { continue }
            if ($unknown.kind -eq 'redirect') {
                $findings.Add((New-VmrQBKnowledgeFinding 'warn' 'handle' "$RelativeName`:$($i + 1)" "'$($unknown.handle)' is a real but broad team; QB routing prefers the narrower owner.$hint"))
            }
            else {
                $findings.Add((New-VmrQBKnowledgeFinding 'fail' 'handle' "$RelativeName`:$($i + 1)" "'$($unknown.handle)' is not a routable team (it is not a team in the Guide's Repo Ownership table, nor a routing target) and the line does not just warn against it.$hint"))
            }
        }
    }
    return $findings.ToArray()
}

function Invoke-VmrQBKnowledgeCheck {
    [CmdletBinding()]
    param(
        [string] $KnowledgeRoot = (Get-VmrQBPaths).KnowledgeRoot,
        # The roster is local data. Defaults to the one in use (Get-VmrQBContactsPath), or to
        # <KnowledgeRoot>/team-contacts.json when a KnowledgeRoot is given explicitly.
        [string] $ContactsPath,
        # Documents whose handle mentions are checked (SKILL.md and references/*.md).
        [string[]] $DocumentRoots = @((Get-VmrQBPaths).SkillRoot),
        [switch] $Online,
        [scriptblock] $TeamLookup = (Get-VmrQBDefaultTeamLookup),
        [scriptblock] $UserLookup = (Get-VmrQBDefaultUserLookup),
        [scriptblock] $IssueLookup = (Get-VmrQBDefaultIssueLookup),
        [int] $MaxAgeDays = 30,
        [datetime] $Today = (Get-Date)
    )
    $findings = [Collections.Generic.List[object]]::new()
    $add = { param($f) $findings.Add($f) }
    if (-not $ContactsPath) {
        $ContactsPath = if ($PSBoundParameters.ContainsKey('KnowledgeRoot')) { Join-Path $KnowledgeRoot 'team-contacts.json' } else { Get-VmrQBContactsPath }
    }

    # --- routing registry: loading validates it -------------------------------------------------
    try { $routing = Get-VmrQBRouting -Path (Join-Path $KnowledgeRoot 'routing.json') -LocalPath $null -Strict }
    catch { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' 'routing.json' $_.Exception.Message); return $findings.ToArray() }

    # --- your routing copy: the registry the daily round actually loads ----------------------------
    # The shipped registry is published; the daily round uses it together with local routing.
    # Problems in local routing produce a warning and a run note; the round continues with usable routing.
    # An explicit -KnowledgeRoot checks only that folder, without local routing.
    if (-not $PSBoundParameters.ContainsKey('KnowledgeRoot')) {
        $localRouting = (Get-VmrQBPaths).LocalRoutingPath
        if (Test-Path -LiteralPath $localRouting -PathType Leaf) {
            try { [void](Get-VmrQBRouting -Strict) }
            catch { & $add (New-VmrQBKnowledgeFinding 'warn' 'local-routing' 'routing.local.json' "Your local routing copy ($localRouting) has a problem: $($_.Exception.Message) The daily round carries on without the bad entry and lists it in its run notes. ./common/Invoke-Routing.ps1 diff shows how it differs from the shipped file; moving the file aside makes a fresh copy.") }
        }
    }

    # --- triage-context -------------------------------------------------------------------------
    # Verified findings come from one QB's rotations, so they are local data (local/knowledge, git-ignored) and absent from a fresh clone.
    $triagePath = if ($PSBoundParameters.ContainsKey('KnowledgeRoot')) { Join-Path $KnowledgeRoot 'triage-context.json' } else { (Get-VmrQBPaths).LocalContextPath }
    $entries = @()
    if (-not (Test-Path -LiteralPath $triagePath)) {
        & $add (New-VmrQBKnowledgeFinding 'info' 'schema' 'triage-context.json' 'No verified findings recorded yet. ./history/Invoke-Record.ps1 -AddContext stores them in local/knowledge (git-ignored); the skill ships none.')
    }
    else {
        try {
            $triage = Get-Content -LiteralPath $triagePath -Raw | ConvertFrom-Json -Depth 20
            if ([int](Get-VmrQBField $triage 'schemaVersion' 0) -ne 1) { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' 'triage-context.json' 'schemaVersion must be 1.') }
            $entries = @(Get-VmrQBField $triage 'entries' @())
        }
        catch { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' 'triage-context.json' "Not valid JSON: $($_.Exception.Message)") }
    }
    $ids = @{}
    foreach ($entry in $entries) {
        $id = [string](Get-VmrQBField $entry 'id')
        if (-not $id) { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' 'triage-context.json' 'An entry has no id.'); continue }
        if ($ids.ContainsKey($id)) { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' $id 'Duplicate entry id.') }
        $ids[$id] = $true
        foreach ($field in 'title', 'operatorStatus', 'operatorNote', 'applicability', 'action', 'verification') {
            if ([string]::IsNullOrWhiteSpace([string](Get-VmrQBField $entry $field))) { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' $id "Missing '$field'. Every entry must say how it was verified and when it applies.") }
        }
        foreach ($link in @(Get-VmrQBField $entry 'links' @())) {
            if (([string]$link) -notmatch '^https://') { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' $id "Link '$link' is not an https URL.") }
        }
        # Age: the most recent date the entry states about itself, in its status, note or verification
        # text. Both 2026-10-02 and "September 29" are understood (a month name takes the current year,
        # or the previous one if that would be in the future).
        $own = (([string](Get-VmrQBField $entry 'operatorStatus')), ([string](Get-VmrQBField $entry 'operatorNote')), ([string](Get-VmrQBField $entry 'verification'))) -join ' '
        $dates = [Collections.Generic.List[datetime]]::new()
        foreach ($m in [regex]::Matches($own, '\b(20\d\d-\d\d-\d\d)\b')) { $dates.Add([datetime]::ParseExact($m.Value, 'yyyy-MM-dd', [Globalization.CultureInfo]::InvariantCulture)) }
        foreach ($m in [regex]::Matches($own, '\b(January|February|March|April|May|June|July|August|September|October|November|December)\s+(\d{1,2})\b')) {
            $month = [datetime]::ParseExact($m.Groups[1].Value, 'MMMM', [Globalization.CultureInfo]::InvariantCulture).Month
            $candidate = [datetime]::new($Today.Year, $month, [int]$m.Groups[2].Value)
            if ($candidate -gt $Today.Date) { $candidate = $candidate.AddYears(-1) }
            $dates.Add($candidate)
        }
        if ($dates.Count) {
            $latest = ($dates | Sort-Object -Descending | Select-Object -First 1)
            $age = [int]($Today.Date - $latest).TotalDays
            if ($age -gt $MaxAgeDays) { & $add (New-VmrQBKnowledgeFinding 'warn' 'age' $id "Last dated $($latest.ToString('yyyy-MM-dd')), $age days ago (limit $MaxAgeDays). Re-verify it, then add a dated line to operatorStatus.") }
        }
        else { & $add (New-VmrQBKnowledgeFinding 'info' 'age' $id 'No date anywhere in the entry, so its age cannot be checked. Add one when you next verify it.') }
    }

    # --- contacts and aliases ------------------------------------------------------------------
    $contacts = $null
    # The roster names people, so it is local data (built by ./common/Invoke-Contacts.ps1) and absent from a fresh clone.
    if (-not (Test-Path -LiteralPath $ContactsPath)) {
        & $add (New-VmrQBKnowledgeFinding 'info' 'contact' 'team-contacts.json' 'No team roster yet (local/knowledge/team-contacts.json), so reports ping teams without naming a person. Run ./common/Invoke-Contacts.ps1 to build it from the guide and the GitHub teams.')
    }
    else {
        try {
            $contacts = (Get-Content -LiteralPath $ContactsPath -Raw | ConvertFrom-Json -Depth 20).contacts
            foreach ($property in $contacts.PSObject.Properties) {
                foreach ($field in 'github', 'teams', 'basis') {
                    if ([string]::IsNullOrWhiteSpace([string](Get-VmrQBField $property.Value $field))) { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' $property.Name "Contact is missing '$field'.") }
                }
            }
        }
        catch { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' 'team-contacts.json' "Not readable: $($_.Exception.Message)") }
    }
    foreach ($team in $routing.Teams) {
        if ($null -ne $contacts -and -not $contacts.PSObject.Properties[$team]) { & $add (New-VmrQBKnowledgeFinding 'warn' 'contact' $team 'Routed team has no named contact; reports will ping the team alone.') }
    }
    $aliasPath = if ($PSBoundParameters.ContainsKey('KnowledgeRoot')) { Join-Path $KnowledgeRoot 'issue-aliases.json' } else { (Get-VmrQBPaths).AliasesPath }
    if (Test-Path -LiteralPath $aliasPath) {
        try {
            $groups = @((Get-Content -LiteralPath $aliasPath -Raw | ConvertFrom-Json -Depth 20).groups)
            $canonicals = @($groups | ForEach-Object { $_.canonical })
            foreach ($group in $groups) {
                foreach ($alias in @($group.aliases)) {
                    if ($alias -in $canonicals) { & $add (New-VmrQBKnowledgeFinding 'fail' 'alias' $alias "Alias is also a canonical name in another group; that would chain.") }
                }
            }
        }
        catch { & $add (New-VmrQBKnowledgeFinding 'fail' 'schema' 'issue-aliases.json' "Not readable: $($_.Exception.Message)") }
    }

    # --- handles mentioned in knowledge and documents ------------------------------------------
    $scan = [Collections.Generic.List[object]]::new()
    $scan.Add(@{ path = $triagePath; name = 'triage-context.json' })
    $scan.Add(@{ path = $ContactsPath; name = 'team-contacts.json' })
    foreach ($root in $DocumentRoots) {
        if (-not (Test-Path -LiteralPath $root)) { continue }
        $resolvedRoot = (Resolve-Path -LiteralPath $root).Path
        foreach ($file in @(Get-ChildItem -LiteralPath $resolvedRoot -Filter '*.md' -File)) { $scan.Add(@{ path = $file.FullName; name = (Join-Path (Split-Path -Leaf $resolvedRoot) $file.Name) }) }
        $references = Join-Path $resolvedRoot 'references'
        if (Test-Path -LiteralPath $references) {
            foreach ($file in @(Get-ChildItem -LiteralPath $references -Filter '*.md' -File)) { $scan.Add(@{ path = $file.FullName; name = (Join-Path (Split-Path -Leaf $resolvedRoot) "references\$($file.Name)") }) }
        }
    }
    foreach ($item in $scan) {
        if (Test-Path -LiteralPath $item.path) { foreach ($f in @(Test-VmrQBHandleMentions -Path $item.path -RelativeName $item.name -Routing $routing)) { & $add $f } }
    }

    # --- live verification ---------------------------------------------------------------------
    if ($Online) {
        foreach ($team in $routing.Teams) {
            $exists = & $TeamLookup ($team -replace '^@dotnet/', '')
            if ($exists -eq $false) { & $add (New-VmrQBKnowledgeFinding 'fail' 'team' $team 'GitHub returns 404 for this team. Pinging it delivers to nobody.') }
            elseif ($null -eq $exists) { & $add (New-VmrQBKnowledgeFinding 'warn' 'team' $team 'Could not verify this team (no network, no auth, or SSO not authorized for the dotnet org).') }
        }
        foreach ($alias in $routing.HandleAliases.Keys) {
            $exists = & $TeamLookup ($alias -replace '^@dotnet/', '')
            if ($exists -eq $true) { & $add (New-VmrQBKnowledgeFinding 'warn' 'alias' $alias "This repo-name handle resolves as a real team on GitHub, but the Guide's Repo Ownership table does not list it. The Guide's table may be stale.") }
        }
        if ($null -ne $contacts) {
            foreach ($property in $contacts.PSObject.Properties) {
                $assignable = & $UserLookup ([string]$property.Value.github)
                if ($assignable -eq $false) { & $add (New-VmrQBKnowledgeFinding 'fail' 'contact' $property.Name "GitHub login '$($property.Value.github)' is not assignable on dotnet/dotnet.") }
                elseif ($null -eq $assignable) { & $add (New-VmrQBKnowledgeFinding 'warn' 'contact' $property.Name "Could not verify login '$($property.Value.github)'.") }
            }
        }
        $stateCache = @{}
        foreach ($entry in $entries) {
            $claims = ([string](Get-VmrQBField $entry 'operatorStatus')) + ' ' + ([string](Get-VmrQBField $entry 'operatorNote'))
            $urls = @(@(Get-VmrQBField $entry 'links' @()) + @([regex]::Matches($claims, 'https://github\.com/[^\s)"]+') | ForEach-Object Value)) | Select-Object -Unique
            foreach ($url in $urls) {
                $m = [regex]::Match([string]$url, '^https://github\.com/(?<o>[^/]+)/(?<r>[^/]+)/(?<k>issues|pull)/(?<n>\d+)')
                if (-not $m.Success) { continue }
                $key = $m.Value
                if (-not $stateCache.ContainsKey($key)) { $stateCache[$key] = & $IssueLookup $m.Groups['o'].Value $m.Groups['r'].Value $m.Groups['n'].Value $m.Groups['k'].Value }
                $live = $stateCache[$key]
                if ($null -eq $live) { & $add (New-VmrQBKnowledgeFinding 'warn' 'link' $entry.id "Could not read $key."); continue }
                # Does the entry's own text say something about this number that the live state contradicts?
                $number = $m.Groups['n'].Value
                $said = [regex]::Matches($claims, "(?i)(?:#|/)$number\b[^.;]{0,80}?\b(open|closed|merged)\b|\b(open|closed|merged)\b[^.;]{0,60}?(?:#|/)$number\b") |
                    ForEach-Object { ($_.Groups[1].Value + $_.Groups[2].Value).ToLowerInvariant() } | Select-Object -Unique
                foreach ($claim in @($said)) {
                    $agrees = ($claim -eq $live) -or ($claim -eq 'closed' -and $live -eq 'merged')
                    if (-not $agrees) { & $add (New-VmrQBKnowledgeFinding 'warn' 'link' $entry.id "Text says $key is $claim but it is $live now. The entry may be stale.") }
                }
            }
        }
    }
    return $findings.ToArray()
}
