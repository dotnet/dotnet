[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot '..\verify\knowledge.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-knowledge-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$env:VMR_QB_HOME = $sandbox
$shippedRouting = Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json'
$today = [datetime]'2026-10-07'

function New-Pack {
    param([object[]] $Entries, [switch] $NoContacts)
    $dir = Join-Path $sandbox ([guid]::NewGuid().ToString('N'))
    [void][IO.Directory]::CreateDirectory($dir)
    Copy-Item $shippedRouting (Join-Path $dir 'routing.json')
    [ordered]@{ schemaVersion = 1; entries = @($Entries) } | ConvertTo-Json -Depth 10 | Set-Content (Join-Path $dir 'triage-context.json') -Encoding utf8
    if (-not $NoContacts) {
        $contacts = [ordered]@{}
        foreach ($team in (Get-VmrQBRouting).Teams) { $contacts[$team] = [ordered]@{ github = 'someone'; teams = 'Some One'; basis = 'test' } }
        [ordered]@{ contacts = $contacts } | ConvertTo-Json -Depth 10 | Set-Content (Join-Path $dir 'team-contacts.json') -Encoding utf8
    }
    return $dir
}
function New-Entry {
    param([string] $Id = 'good-entry', [hashtable] $Override = @{})
    $entry = [ordered]@{
        id = $Id; title = 'A fine entry'; operatorStatus = 'Verified 2026-10-05.'; operatorNote = 'Something was assessed.'
        applicability = 'When the thing happens.'; action = 'Monitor only.'; links = @(); verification = 'Read the log.'
    }
    foreach ($key in $Override.Keys) { $entry[$key] = $Override[$key] }
    return [pscustomobject]$entry
}
function Run([string] $Pack, [hashtable] $Extra = @{}) {
    @(Invoke-VmrQBKnowledgeCheck -KnowledgeRoot $Pack -DocumentRoots @() -Today $today @Extra)
}

Check 'A well-formed pack has no failures or warnings' {
    $f = Run (New-Pack @(New-Entry))
    $bad = @($f | Where-Object { $_.severity -in 'fail', 'warn' })
    Assert ($bad.Count -eq 0) "unexpected: $(($bad | ForEach-Object { "$($_.check):$($_.message)" }) -join ' | ')"
}

Check 'A missing triage context (git-ignored local data) is information, not a failure' {
    $pack = New-Pack @()
    Remove-Item (Join-Path $pack 'triage-context.json') -Force
    $f = Run $pack
    $bad = @($f | Where-Object { $_.severity -in 'fail', 'warn' })
    Assert ($bad.Count -eq 0) "a fresh clone has no triage context and must not fail: $(($bad | ForEach-Object { "$($_.check):$($_.message)" }) -join ' | ')"
    Assert (@($f | Where-Object { $_.severity -eq 'info' -and $_.subject -eq 'triage-context.json' }).Count -eq 1) 'the missing context must be reported as info (absent triage)'
}

Check 'A missing contact roster (git-ignored local data) is information, not a failure' {
    $f = Run (New-Pack @(New-Entry) -NoContacts)
    $bad = @($f | Where-Object { $_.severity -in 'fail', 'warn' })
    Assert ($bad.Count -eq 0) "a fresh clone has no roster and must not fail: $(($bad | ForEach-Object { "$($_.check):$($_.message)" }) -join ' | ')"
    Assert (@($f | Where-Object { $_.check -eq 'contact' -and $_.severity -eq 'info' -and $_.subject -eq 'team-contacts.json' }).Count -eq 1) 'the missing roster must be reported as info'
}

Check 'Missing fields, duplicate ids and non-https links are failures' {
    $pack = New-Pack @((New-Entry), (New-Entry -Override @{ verification = '' }), (New-Entry -Id 'other' -Override @{ links = @('http://insecure.example/x') }))
    $f = Run $pack | Where-Object severity -eq 'fail'
    Assert (@($f | Where-Object { $_.message -match "Missing 'verification'" }).Count -eq 1) 'a missing verification field must fail'
    Assert (@($f | Where-Object { $_.message -match 'Duplicate entry id' }).Count -eq 1) 'a duplicate id must fail'
    Assert (@($f | Where-Object { $_.message -match 'not an https URL' }).Count -eq 1) 'a non-https link must fail'
}

Check 'A bad routing registry stops the check with a clear failure' {
    $pack = New-Pack @(New-Entry)
    $bad = Get-Content (Join-Path $pack 'routing.json') -Raw | ConvertFrom-Json -Depth 20
    # A repoOwners entry in a routing file overrides the Guide's table, so it can also be wrong.
    $bad | Add-Member -NotePropertyName repoOwners -NotePropertyValue ([pscustomobject]@{ runtime = '@dotnet/not-a-team' })
    $bad | ConvertTo-Json -Depth 20 | Set-Content (Join-Path $pack 'routing.json') -Encoding utf8
    $f = Run $pack
    Assert (@($f | Where-Object { $_.check -eq 'schema' -and $_.message -match 'unknown team' }).Count -eq 1) 'the registry problem must be reported'
}

Check 'Age: ISO dates and month names are read, stale entries warn, undated entries are only info' {
    $old = New-Entry -Id 'old' -Override @{ operatorStatus = 'Verified 2026-01-02.'; operatorNote = 'n'; verification = 'v' }
    $named = New-Entry -Id 'named' -Override @{ operatorStatus = 'Alex confirmed September 29 that this is fine.'; operatorNote = 'n'; verification = 'v' }
    $undated = New-Entry -Id 'undated' -Override @{ operatorStatus = 'No dates here.'; operatorNote = 'n'; verification = 'v' }
    $f = Run (New-Pack @($old, $named, $undated))
    Assert (@($f | Where-Object { $_.subject -eq 'old' -and $_.severity -eq 'warn' -and $_.message -match '2026-01-02' }).Count -eq 1) 'the old entry must warn with its date'
    Assert (@($f | Where-Object { $_.subject -eq 'named' -and $_.severity -eq 'warn' }).Count -eq 0) 'September 29 is 8 days old on 2026-10-07 and must not warn'
    Assert (@($f | Where-Object { $_.subject -eq 'undated' -and $_.severity -eq 'info' }).Count -eq 1) 'an undated entry is informational'
}

Check 'A non-team handle is flagged only when it is an instruction, not when it is a warning' {
    $pack = New-Pack @(New-Entry)
    $docs = Join-Path $sandbox 'docs'; [void][IO.Directory]::CreateDirectory($docs)
    @'
| SDK / CLI | `@dotnet/dotnet-cli` | **not** `@dotnet/sdk` - that handle does not exist |
| Runtime | `@dotnet/runtime` | crossgen2 is a runtime component, **not** roslyn |
Ping @dotnet/installer for this.
Use @dotnet/dnr-codeflow (there is no @dotnet/emsdk team: 404).
'@ | Set-Content (Join-Path $docs 'SKILL.md') -Encoding utf8
    $f = @(Invoke-VmrQBKnowledgeCheck -KnowledgeRoot $pack -DocumentRoots @($docs) -Today $today | Where-Object check -eq 'handle')
    $lines = @($f | ForEach-Object { [int](($_.subject -split ':')[-1]) } | Sort-Object)
    Assert (($lines -join ',') -eq '2,3') "expected lines 2 and 3 flagged (an instruction with a distant 'not', and a bare instruction); got: $($lines -join ',')"
    Assert (@($f | Where-Object { $_.message -match 'Use @dotnet/dnr-codeflow' }).Count -eq 1) 'the runtime finding must suggest the real team'
}

Check 'A real-but-broad team in a document is a warning with the narrower owner, not a failure' {
    $pack = New-Pack @(New-Entry)
    $docs = Join-Path $sandbox 'docs2'; [void][IO.Directory]::CreateDirectory($docs)
    'Ping @dotnet/roslyn for compiler failures.' | Set-Content (Join-Path $docs 'SKILL.md') -Encoding utf8
    $f = @(Invoke-VmrQBKnowledgeCheck -KnowledgeRoot $pack -DocumentRoots @($docs) -Today $today | Where-Object check -eq 'handle')
    Assert ($f.Count -eq 1 -and $f[0].severity -eq 'warn') "expected one warning, got: $($f | ForEach-Object { $_.severity + ':' + $_.message })"
    Assert ($f[0].message -match 'roslyn-infrastructure') 'the warning must name the preferred owner'
}

Check 'Aliases that chain are refused' {
    $pack = New-Pack @(New-Entry)
    [ordered]@{ groups = @([ordered]@{ canonical = 'A'; aliases = @('B') }, [ordered]@{ canonical = 'B'; aliases = @('C') }) } | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $pack 'issue-aliases.json') -Encoding utf8
    $f = Run $pack | Where-Object check -eq 'alias'
    Assert ($f.Count -eq 1 -and $f[0].subject -eq 'B') 'B is both an alias and a canonical name'
}

Check 'Online: unreachable teams fail, unverifiable ones warn, a revived mistaken handle warns' {
    $pack = New-Pack @(New-Entry)
    $team = { param($slug) switch ($slug) { 'dnceng' { $false } 'msbuild' { $null } 'sdk' { $true } default { $true -and ($slug -notin 'runtime', 'installer', 'efcore', 'emsdk', 'aspnetcore', 'arcade', 'roslyn', 'razor', 'nuget-client', 'vstest', 'winforms', 'wpf', 'windowsdesktop', 'templating', 'deployment-tools', 'command-line-api', 'source-build-assets') } } }
    $f = Run $pack @{ Online = $true; TeamLookup = $team; UserLookup = { param($l) $true }; IssueLookup = { param($o, $r, $n, $k) 'open' } }
    Assert (@($f | Where-Object { $_.check -eq 'team' -and $_.severity -eq 'fail' -and $_.subject -eq '@dotnet/dnceng' }).Count -eq 1) 'a 404 team must fail'
    Assert (@($f | Where-Object { $_.check -eq 'team' -and $_.severity -eq 'warn' -and $_.subject -eq '@dotnet/msbuild' }).Count -eq 1) 'an unverifiable team must only warn'
    Assert (@($f | Where-Object { $_.check -eq 'alias' -and $_.subject -eq '@dotnet/sdk' }).Count -eq 1) '@dotnet/sdk resolving means the alias is stale'
}

Check 'Online: contacts that are not assignable fail' {
    $pack = New-Pack @(New-Entry)
    $f = Run $pack @{ Online = $true; TeamLookup = { param($s) $true }; UserLookup = { param($l) $false }; IssueLookup = { param($o, $r, $n, $k) 'open' } }
    Assert (@($f | Where-Object { $_.check -eq 'contact' -and $_.severity -eq 'fail' }).Count -ge 17) 'every non-assignable contact must fail'
}

Check 'Online: an entry that says OPEN about an issue that is closed or merged is flagged; agreement is not' {
    $stale = New-Entry -Id 'stale' -Override @{
        operatorStatus = 'Fix dotnet/dotnet#101 is OPEN and UNASSIGNED.'; links = @('https://github.com/dotnet/dotnet/issues/101')
        operatorNote = 'The cited fix dotnet/example#102 is MERGED.'; verification = 'v' }
    $stale.links += 'https://github.com/dotnet/example/pull/102'
    $lookup = { param($o, $r, $n, $k) if ($n -eq '101') { 'closed' } else { 'merged' } }
    $f = Run (New-Pack @($stale)) @{ Online = $true; TeamLookup = { param($s) $true -and $s -notin 'runtime', 'installer', 'efcore', 'emsdk', 'aspnetcore', 'arcade', 'roslyn', 'razor', 'nuget-client', 'vstest', 'winforms', 'wpf', 'windowsdesktop', 'templating', 'deployment-tools', 'command-line-api', 'source-build-assets', 'sdk' }; UserLookup = { param($l) $true }; IssueLookup = $lookup }
    $link = @($f | Where-Object { $_.check -eq 'link' })
    Assert ($link.Count -eq 1) "expected exactly the OPEN-vs-closed contradiction, got $($link.Count): $($link.message -join ' | ')"
    Assert ($link[0].message -match '101' -and $link[0].message -match 'open' -and $link[0].message -match 'closed') $link[0].message
}

Remove-Item $sandbox -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue
if ($failures) { Write-Host "$failures knowledge-check test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '12 offline knowledge-check tests. Stubbed lookups and temporary files only; no network, email, or external changes.' -ForegroundColor Green
