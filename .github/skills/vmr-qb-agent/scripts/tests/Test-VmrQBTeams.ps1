[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\common\core.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert([bool] $Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Assert-Throws([scriptblock] $Body, [string] $Pattern) {
    try { & $Body | Out-Null } catch {
        if ($_.Exception.Message -notmatch $Pattern) { throw "wrong error: $($_.Exception.Message)" }
        return
    }
    throw "Expected an error matching: $Pattern"
}
function New-TempDir {
    $path = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-teams-' + [guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($path)
    return $path
}
function Write-Registry([string] $Dir, [string] $Name, $Value) {
    $path = Join-Path $Dir $Name
    $Value | ConvertTo-Json -Depth 20 | Set-Content -LiteralPath $path -Encoding utf8
    return $path
}

# Keep the QB's real local registry out of every test.
$sandbox = New-TempDir
$env:VMR_QB_HOME = $sandbox

Check 'The shipped registry loads and every alias, repo owner and category owner is a real team' {
    $routing = Get-VmrQBRouting
    Assert ($routing.Teams.Count -ge 17) "expected the 17 verified teams, got $($routing.Teams.Count)"
    foreach ($alias in $routing.HandleAliases.Keys) { Assert ($routing.Teams -contains $routing.HandleAliases[$alias]) "alias $alias -> unknown team" }
    foreach ($repo in $routing.RepoOwners.Keys) { Assert ($routing.Teams -contains $routing.RepoOwners[$repo]) "repo $repo -> unknown owner" }
    Assert ($null -eq $routing.PSObject.Properties['DefaultOwner']) 'there is no default owner'
}

Check 'Aliases are derived from the Guide: every Guide repo whose name is not a team resolves to its owner' {
    $routing = Get-VmrQBRouting
    $derived = 0
    foreach ($repo in $routing.RepoOwners.Keys) {
        $handle = "@dotnet/$repo"
        if ($routing.Teams -contains $handle -or $routing.PreferredRedirects.Contains($handle)) { Assert (-not $routing.HandleAliases.Contains($handle)) "$handle is a real team and must not be an alias"; continue }
        Assert ($routing.HandleAliases[$handle] -eq $routing.RepoOwners[$repo]) "$handle must resolve to $($routing.RepoOwners[$repo])"
        $derived++
    }
    Assert ($derived -ge 10) "expected most Guide repos to be derived aliases, got $derived"
    $shipped = Get-Content (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json') -Raw | ConvertFrom-Json
    Assert (-not ($shipped.PSObject.Properties.Name -contains 'handleAliases')) 'routing.json must not carry a hand-kept alias list'
}

Check 'Handles resolve: canonical, mistaken alias, person, plain text, and unknown team' {
    Assert ((Resolve-VmrQBOwnerTeam '@dotnet/prodconsvcs') -eq '@dotnet/prodconsvcs') 'canonical changed'
    Assert ((Resolve-VmrQBOwnerTeam '@DotNet/ProdConSvcs') -eq '@dotnet/prodconsvcs') 'case must not matter and the canonical spelling must come back'
    Assert ((Resolve-VmrQBOwnerTeam '@dotnet/runtime') -eq '@dotnet/dnr-codeflow') '@dotnet/runtime is not a team; it must map to dnr-codeflow'
    Assert ((Resolve-VmrQBOwnerTeam '@dotnet/sdk') -eq '@dotnet/dotnet-cli') '@dotnet/sdk must map to dotnet-cli'
    Assert ((Resolve-VmrQBOwnerTeam '@dotnet/source-build-assets') -eq '@dotnet/source-build') 'a repo name in the Guide table resolves to that repo''s owner'
    Assert ($null -eq (Resolve-VmrQBOwnerTeam '@dotnet/installer')) 'installer is not in the Guide table, so nothing maps it'
    Assert ((Resolve-VmrQBOwnerTeam '@dotnet/roslyn') -eq '@dotnet/roslyn-infrastructure') '@dotnet/roslyn is a real broad team that routing narrows to roslyn-infrastructure'
    Assert ((Resolve-VmrQBOwnerTeam '@333fred') -eq '@333fred') 'a person handle must pass through'
    Assert ((Resolve-VmrQBOwnerTeam 'First Responders') -eq 'First Responders') 'non-handle text must pass through untouched'
    Assert ($null -eq (Resolve-VmrQBOwnerTeam '@dotnet/totally-made-up')) 'an unknown team must return $null, never be guessed'
}

Check 'A bad registry is refused at load time rather than discovered during a run' {
    $dir = New-TempDir
    try {
        $good = Get-Content (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json') -Raw | ConvertFrom-Json -Depth 20
        $copy = { $good | ConvertTo-Json -Depth 20 | ConvertFrom-Json -Depth 20 }

        $bad = & $copy; $bad | Add-Member -NotePropertyName handleAliases -NotePropertyValue ([pscustomobject]@{ '@dotnet/ghost' = '@dotnet/not-a-team' })
        $path = Write-Registry $dir 'alias.json' $bad
        Assert-Throws { Get-VmrQBRouting -Strict -Path $path -LocalPath $null } 'alias @dotnet/ghost points at unknown team'

        # A registry that lists a team itself and also aliases it is a contradiction. (A team the Guide lists is different: the Guide wins; see below.)
        $bad = & $copy; $bad | Add-Member -NotePropertyName teams -NotePropertyValue @('@dotnet/example-team')
        $bad | Add-Member -NotePropertyName handleAliases -NotePropertyValue ([pscustomobject]@{ '@dotnet/example-team' = '@dotnet/msbuild' })
        $path = Write-Registry $dir 'both.json' $bad
        Assert-Throws { Get-VmrQBRouting -Strict -Path $path -LocalPath $null } 'also listed as a real team'

        $bad = & $copy; $bad.categories[0].owners = @('@dotnet/nonexistent')
        $path = Write-Registry $dir 'category.json' $bad
        Assert-Throws { Get-VmrQBRouting -Strict -Path $path -LocalPath $null } "category '.*' \((?:shipped|local) routing file\) names unknown team"

        $bad = & $copy; $bad.schemaVersion = 7
        $path = Write-Registry $dir 'schema.json' $bad
        Assert-Throws { Get-VmrQBRouting -Strict -Path $path -LocalPath $null } 'unsupported schemaVersion'

        $bad = & $copy; $bad | Add-Member -NotePropertyName teams -NotePropertyValue @('dotnet/missing-at-sign')
        $path = Write-Registry $dir 'handle.json' $bad
        Assert-Throws { Get-VmrQBRouting -Strict -Path $path -LocalPath $null } 'not an @dotnet/<slug> handle'
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}

function New-GuideFile([string] $Dir, [string[]] $ExtraRows = @()) {
    # The real guide's table plus extra rows, so every team the registry's aliases and categories name still exists.
    $real = [IO.File]::ReadAllLines((Join-Path (Get-VmrQBPackageRoot) 'docs\VMR-QB-Guide.md'))
    $start = [array]::FindIndex($real, [Predicate[string]] { param($l) $l -match '^##\s+Repo Ownership\b' })
    $rows = @(for ($i = $start + 1; $i -lt $real.Count; $i++) { if ($real[$i] -match '^#{1,6}\s') { break }; if ($real[$i].StartsWith('|')) { $real[$i] } })
    $path = Join-Path $Dir 'VMR-QB-Guide.md'
    (@('# Guide', '', '## Repo Ownership', '') + $rows + $ExtraRows + @('', '### Tips', '', '| not | a row |')) | Set-Content -LiteralPath $path -Encoding utf8
    return $path
}

Check 'Repo owners and real teams come from the Guide, not from the shipped registry' {
    $shipped = Get-Content (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json') -Raw | ConvertFrom-Json
    Assert (-not ($shipped.PSObject.Properties.Name -contains 'repoOwners')) 'routing.json must not carry a copy of the Guide''s repo owners'
    Assert (-not ($shipped.PSObject.Properties.Name -contains 'teams')) 'routing.json must not carry a copy of the Guide''s team list'
    $dir = New-TempDir
    try {
        $guide = New-GuideFile $dir @('| newrepo | [@dotnet/new-team](https://github.com/orgs/dotnet/teams/new-team) |')
        $routing = Get-VmrQBRouting -LocalPath $null -GuidePath $guide
        Assert ($routing.RepoOwners['arcade'] -eq '@dotnet/dnceng') 'the Guide row must be used'
        Assert (-not $routing.RepoOwners.Contains('General VMR infra')) 'the General VMR infra row is the default owner, not a repo'
        Assert ($routing.RepoOwners['newrepo'] -eq '@dotnet/new-team') 'a repo added to the Guide must route without editing the registry'
        Assert ($routing.Teams -contains '@dotnet/new-team') 'a team the Guide lists is known without listing it in the registry'
        Assert ($routing.Teams -contains '@dotnet/prodconsvcs') 'the General VMR infra team is a real team too'
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'A routing file can override the Guide, and a handle the Guide lists as a team is never an alias' {
    $dir = New-TempDir
    try {
        $guide = New-GuideFile $dir @('| runtime | [@dotnet/runtime](https://github.com/orgs/dotnet/teams/runtime) |')
        $local = Write-Registry $dir 'routing.local.json' ([ordered]@{ schemaVersion = 1; repoOwners = [ordered]@{ arcade = '@dotnet/msbuild' } })
        $routing = Get-VmrQBRouting -LocalPath $local -GuidePath $guide
        Assert ($routing.RepoOwners['arcade'] -eq '@dotnet/msbuild') 'a routing-file entry must override the Guide'
        Assert ($routing.RepoOwners['runtime'] -eq '@dotnet/runtime') 'the Guide row must be used'
        Assert (-not $routing.HandleAliases.Contains('@dotnet/runtime')) 'the Guide lists @dotnet/runtime as a team, so it must not resolve as a repo-name alias'
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}
Check 'Without a readable Guide the registry refuses to load, naming the file, instead of guessing owners' {
    $dir = New-TempDir
    try {
        Assert-Throws { Get-VmrQBRouting -Strict -LocalPath $null -GuidePath (Join-Path $dir 'missing.md') } 'Guide not found.*git pull'
        $broken = Join-Path $dir 'broken.md'; Set-Content -LiteralPath $broken -Value "# Guide`n## Something else`n"
        Assert-Throws { Get-VmrQBRouting -Strict -LocalPath $null -GuidePath $broken } 'Repo Ownership.*broken\.md'
    }
    finally { Remove-Item $dir -Recurse -Force -ErrorAction SilentlyContinue }
}
Check 'The first use makes your own local copy of routing.json once, stamped with where it came from, and never overwrites it' {
    $fresh = New-TempDir
    $env:VMR_QB_HOME = $fresh
    try {
        $local = Join-Path (Get-VmrQBPaths).LocalKnowledgeDir 'routing.local.json'
        Assert (-not (Test-Path -LiteralPath $local)) 'a fresh state folder has no local routing file'
        $routing = Get-VmrQBRouting
        Assert (Test-Path -LiteralPath $local) 'the first load must create the local copy'
        $copy = Get-Content -LiteralPath $local -Raw | ConvertFrom-Json
        $shipped = Get-Content (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json') -Raw | ConvertFrom-Json
        Assert ($copy.baseSha256 -eq (Get-VmrQBShippedRoutingHash)) 'the copy records the shipped file it came from'
        Assert (@($copy.categories).Count -eq @($shipped.categories).Count -and $copy.vmrTestsOwner -eq $shipped.vmrTestsOwner) 'it is a full copy'
        Assert ($routing.Categories.Count -eq @($shipped.categories).Count) 'a full copy merged over the shipped file changes nothing'
        Assert (-not (Test-VmrQBLocalRoutingBehind)) 'a fresh copy is not behind'

        # Your edits live in the copy, are used, and are never overwritten.
        $copy.categories[0].name = 'Edited by this QB'
        $copy | ConvertTo-Json -Depth 20 | Set-Content -LiteralPath $local -Encoding utf8
        Assert ((Get-VmrQBRouting).Categories[0].name -eq 'Edited by this QB') 'an edit in the local copy must be used'
        Assert (-not (Initialize-VmrQBLocalRouting)) 'an existing local file is left alone'
        Assert ((Get-Content -LiteralPath $local -Raw | ConvertFrom-Json).categories[0].name -eq 'Edited by this QB') 'the edit survives'

        # A shipped file that moved on since the copy is reported, not merged.
        $copy.baseSha256 = 'deadbeef'
        $copy | ConvertTo-Json -Depth 20 | Set-Content -LiteralPath $local -Encoding utf8
        Assert (Test-VmrQBLocalRoutingBehind) 'a copy made from older shipped content is behind'
        $handWritten = [ordered]@{ schemaVersion = 1; categories = @() }
        $handWritten | ConvertTo-Json | Set-Content -LiteralPath $local -Encoding utf8
        Assert (-not (Test-VmrQBLocalRoutingBehind)) 'a hand-written file has no base and is never behind'
    }
    finally { $env:VMR_QB_HOME = $sandbox; Remove-Item $fresh -Recurse -Force -ErrorAction SilentlyContinue }
}

Check 'Mistaken handles in free text are found with a suggestion; real teams are ignored' {
    $text = 'Ping @dotnet/runtime and @dotnet/prodconsvcs, not @dotnet/installer. Also @dotnet/nonsense.'
    $found = @(Find-VmrQBUnknownHandles -Text $text)
    Assert ($found.Count -eq 3) "expected 3 unknown handles, got $($found.Count): $($found.handle -join ', ')"
    Assert ((@($found | Where-Object handle -eq '@dotnet/runtime')[0]).suggestion -eq '@dotnet/dnr-codeflow') 'runtime suggestion wrong'
    Assert ($null -eq (@($found | Where-Object handle -eq '@dotnet/nonsense')[0]).suggestion) 'a truly unknown handle must have no suggestion'
    Assert (@(Find-VmrQBUnknownHandles -Text 'See @dotnet/dnceng.').Count -eq 0) 'a real team followed by a period was flagged'
}

Check 'A real-but-broad team is a redirect, not a 404, and the generated text says so truthfully' {
    $routing = Get-VmrQBRouting
    Assert ($routing.PreferredRedirects.Contains('@dotnet/roslyn')) 'roslyn must be a redirect'
    Assert (-not $routing.HandleAliases.Contains('@dotnet/roslyn')) 'roslyn exists on GitHub, so it must not be listed as a 404 alias'
    $kinds = @{}; foreach ($f in @(Find-VmrQBUnknownHandles -Text '@dotnet/roslyn @dotnet/runtime @dotnet/zzz')) { $kinds[$f.handle] = $f.kind }
    Assert ($kinds['@dotnet/roslyn'] -eq 'redirect' -and $kinds['@dotnet/runtime'] -eq 'nonexistent' -and $kinds['@dotnet/zzz'] -eq 'unknown') "kinds wrong: $($kinds.GetEnumerator() | ForEach-Object { $_.Key + '=' + $_.Value })"
    $block = Get-VmrQBOwnershipBlock
    Assert ($block -match 'real but broad.*@dotnet/roslyn -> @dotnet/roslyn-infrastructure') 'the block must name the redirect'
    $nonexistentLine = ($block -split "`n" | Where-Object { $_ -match 'are NOT teams|names are NOT teams' }) -join ' '
    Assert ($nonexistentLine -notmatch '@dotnet/roslyn \(use') 'the 404 sentence must not claim roslyn does not exist'
}

Check 'The generated ownership block carries every owner and the rules that stop mistakes' {
    $block = Get-VmrQBOwnershipBlock
    $routing = Get-VmrQBRouting
    foreach ($repo in $routing.RepoOwners.Keys) {
        Assert ($block.Contains("$repo $($routing.RepoOwners[$repo])")) "block is missing $repo"
    }
    Assert ($block -match '@dotnet/runtime \(use @dotnet/dnr-codeflow\)') 'block must warn that @dotnet/runtime is not a team'
    Assert ($block -match 'HELIX ISSUES ONLY') 'block must keep First Responders Helix-only'
    Assert ($block -match 'name no team: write "No identified owner"') 'block must say what to write when nothing maps'
    Assert ($block -notmatch 'general VMR infra') 'there is no default owner in the repo table'
}

Check 'Every routed team has a named contact, and every contact is a routed team' {
    $rosterPath = Get-VmrQBContactsPath
    if (-not (Test-Path -LiteralPath $rosterPath)) { Write-Host '  (no team roster; run ./common/Invoke-Contacts.ps1 to build it, so there is nothing to compare)' -ForegroundColor DarkGray; return }
    $contacts = (Get-Content $rosterPath -Raw | ConvertFrom-Json).contacts.PSObject.Properties.Name
    $teams = (Get-VmrQBRouting).Teams
    $missing = @($teams | Where-Object { $_ -notin $contacts })
    $extra = @($contacts | Where-Object { $_ -notin $teams })
    Assert ($missing.Count -eq 0) "teams without a contact: $($missing -join ', ')"
    Assert ($extra.Count -eq 0) "contacts for non-teams: $($extra -join ', ')"
}

Remove-Item $sandbox -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue
if ($failures) { Write-Host "$failures routing test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '9 offline routing tests. Temporary files only; no network, email, or external changes.' -ForegroundColor Green
