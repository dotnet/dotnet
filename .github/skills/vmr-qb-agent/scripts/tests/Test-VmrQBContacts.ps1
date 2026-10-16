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
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-contacts-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$env:VMR_QB_HOME = $sandbox
$script:out = Join-Path $sandbox 'knowledge\team-contacts.json'
$script:now = [datetime]'2026-10-07'

$guideText = @'
# Guide
## Overview
| Repo | Owner |
|------|-------|
| not-this-table | [@dotnet/overview-team](https://github.com/orgs/dotnet/teams/overview-team) |

## Repo Ownership

Ping the team.

| Repo | Owner |
|------|-------|
| arcade | [@dotnet/dnceng](https://github.com/orgs/dotnet/teams/dnceng) |
| fsharp | [@dotnet/fsharp](https://github.com/orgs/dotnet/teams/fsharp) |
| razor | [@dotnet/roslyn-infrastructure](https://github.com/orgs/dotnet/teams/roslyn-infrastructure) |
| roslyn | [@dotnet/roslyn-infrastructure](https://github.com/orgs/dotnet/teams/roslyn-infrastructure) |
| *General VMR infra* | @dotnet/prodconsvcs |

### Tips

| Other | [@dotnet/after-section](https://github.com/orgs/dotnet/teams/after-section) |
'@
$script:guidePath = Join-Path $sandbox 'guide.md'
[IO.File]::WriteAllText($script:guidePath, $guideText, [Text.UTF8Encoding]::new($false))

$script:calls = [Collections.Generic.List[string]]::new()
$script:roster = @{}
$script:failTeams = @{}
$script:activity = @{}
$script:names = @{}
function Reset-World {
    $script:calls.Clear()
    $script:failTeams = @{}
    $script:roster = @{
        'dnceng/maintainer' = @('alice', 'bob', 'dependabot[bot]')
        'fsharp/maintainer' = @('dave', 'carol')
        'roslyn-infrastructure/maintainer' = @('erin')
        'prodconsvcs/maintainer' = @()
        'prodconsvcs/member' = @('frank', 'gina')
    }
    $script:activity = @{ alice = '2026-10-01'; bob = '2026-10-05'; carol = '2026-09-01'; dave = '2026-09-01'; erin = '2026-10-06'; gina = '2026-08-01' }
    $script:names = @{ bob = 'Bob Builder'; erin = 'Erin E'; gina = 'Gina G' }
    Remove-Item -LiteralPath $script:out -Force -ErrorAction SilentlyContinue
}
$script:maintainerStub = {
    param($org, $slug, $role)
    $script:calls.Add("$slug/$role")
    if ($script:failTeams.ContainsKey($slug)) { return [pscustomobject]@{ Status = $script:failTeams[$slug]; Logins = @(); Message = "HTTP stub failure for $slug" } }
    $key = "$slug/$role"
    [pscustomobject]@{ Status = 'ok'; Logins = @($(if ($script:roster.ContainsKey($key)) { $script:roster[$key] } else { @() })); Message = '' }
}
$script:activityStub = { param($login) $value = $script:activity[$login]; if ($value) { [datetime]$value } else { $null } }
$script:profileStub = { param($login) $script:names[$login] }
function Invoke-Update([hashtable] $Extra = @{}) {
    $script:calls.Clear()
    $arguments = @{ GuidePath = $script:guidePath; OutputPath = $script:out; ExistingPath = $script:out; Today = $script:now
        MaintainerLookup = $script:maintainerStub; ProfileLookup = $script:profileStub; ActivityLookup = $script:activityStub }
    foreach ($key in $Extra.Keys) { $arguments[$key] = $Extra[$key] }
    Update-VmrQBContacts @arguments
}

Check 'The guide parser reads the Repo Ownership table: team link or bare handle, ignoring other sections' {
    $rows = @(ConvertFrom-VmrQBGuideOwnership -Text $guideText)
    Assert ($rows.Count -eq 5) "expected 5 rows, got $($rows.Count)"
    Assert (@($rows | ForEach-Object Team | Select-Object -Unique).Count -eq 4) 'two repos share roslyn-infrastructure'
    Assert ($rows[0].Url -eq 'https://github.com/orgs/dotnet/teams/dnceng') 'the team page URL comes from the link'
    Assert ($rows[4].Repo -eq 'General VMR infra' -and $rows[4].Team -eq '@dotnet/prodconsvcs') 'italics are stripped and a bare handle works'
    Assert (@($rows.Team) -notcontains '@dotnet/overview-team' -and @($rows.Team) -notcontains '@dotnet/after-section') 'other tables must not leak in'
}

Check 'A guide without the section or table fails loudly instead of yielding an empty roster' {
    $threw = $false; try { ConvertFrom-VmrQBGuideOwnership -Text "# Guide`n## Something else`n" | Out-Null } catch { $threw = $_.Exception.Message -match 'Repo Ownership' }
    Assert $threw 'a missing section must throw'
    $threw = $false; try { ConvertFrom-VmrQBGuideOwnership -Text "## Repo Ownership`nno table here`n" | Out-Null } catch { $threw = $_.Exception.Message -match 'no table rows' }
    Assert $threw 'a section without rows must throw'
}

Check 'The real guide, when present, lists exactly the teams routing.json routes to' {
    $guide = Join-Path (Get-VmrQBPackageRoot) 'docs\VMR-QB-Guide.md'
    if (-not (Test-Path -LiteralPath $guide)) { Write-Host '  (no docs/VMR-QB-Guide.md beside this skill; skipped)' -ForegroundColor DarkGray; return }
    $teams = @(ConvertFrom-VmrQBGuideOwnership -Text ([IO.File]::ReadAllText($guide)) | ForEach-Object Team | Select-Object -Unique | Sort-Object)
    $routed = @((Get-VmrQBRouting).Teams | Sort-Object)
    Assert (($teams -join ',') -eq ($routed -join ',')) "guide and routing.json disagree. Guide only: $(@($teams | Where-Object { $_ -notin $routed }) -join ', '). Routing only: $(@($routed | Where-Object { $_ -notin $teams }) -join ', ')"
}

Check 'Build: the most recently active human maintainer is chosen; bots are ignored; ties go to the lower login' {
    Reset-World
    $result = Invoke-Update
    Assert ($result.Built.Count -eq 4 -and $result.Missing.Count -eq 0) "built $($result.Built.Count), missing $($result.Missing.Count)"
    $dnceng = $result.Entries['@dotnet/dnceng']
    Assert ($dnceng.github -eq 'bob' -and $dnceng.teams -eq 'Bob Builder') 'bob is the most recent'
    Assert (@($dnceng.candidates | ForEach-Object { $_.github }) -join ',' -eq 'bob,alice') "candidates should be ranked and exclude the bot: $(@($dnceng.candidates | ForEach-Object { $_.github }) -join ',')"
    Assert ($dnceng.candidates[0].name -eq 'Bob Builder' -and $dnceng.candidates[0].lastActive -eq '2026-10-05' -and $dnceng.candidates[1].name -eq 'alice') 'each candidate carries a name (login when the profile has none) and last activity'
    Assert ($dnceng.basis -eq 'team maintainer, most recently active of 2') "basis: $($dnceng.basis)"
    Assert ($dnceng.source -eq 'github-maintainers' -and $dnceng.verifiedAt -eq '2026-10-07') 'provenance must be recorded'
    Assert ($dnceng.teamUrl -eq 'https://github.com/orgs/dotnet/teams/dnceng') 'the team page is recorded'
    $fsharp = $result.Entries['@dotnet/fsharp']
    Assert ($fsharp.github -eq 'carol') 'a tie is broken by login, so the choice is repeatable'
    Assert ($fsharp.teams -eq 'carol') 'with no profile name the login is the display name'
    Assert ($result.Entries['@dotnet/roslyn-infrastructure'].basis -eq 'team maintainer') 'a lone maintainer needs no ranking note'
}

Check 'Build: a team with no maintainers falls back to members and says so' {
    Reset-World
    $entry = (Invoke-Update).Entries['@dotnet/prodconsvcs']
    Assert ($entry.github -eq 'gina') 'gina has activity, frank has none'
    Assert ($entry.basis -match 'GitHub lists no maintainer') "basis: $($entry.basis)"
}

Check 'The roster is written once, as LF JSON the readers understand' {
    Reset-World
    Invoke-Update | Out-Null
    Assert (Test-Path -LiteralPath $script:out) 'the roster must be written to the state directory'
    $text = [IO.File]::ReadAllText($script:out)
    Assert (-not $text.Contains("`r`n")) 'LF line endings'
    $contacts = Read-VmrQBContacts -Path $script:out
    Assert ($contacts.Count -eq 4 -and $contacts['@dotnet/dnceng'].github -eq 'bob') 'readers must see github/teams/basis'
    Assert ((Get-VmrQBContactsPath) -eq $script:out) 'the state-directory roster is the one in use'
}

Check 'Reuse: a second run makes no GitHub calls and writes nothing' {
    Reset-World
    Invoke-Update | Out-Null
    $stamp = (Get-Item -LiteralPath $script:out).LastWriteTimeUtc
    $again = Invoke-Update
    Assert ($script:calls.Count -eq 0) "unexpected lookups: $($script:calls -join ', ')"
    Assert ($again.Reused.Count -eq 4 -and $again.Built.Count -eq 0 -and -not $again.Written) 'everything must be reused'
    Assert ((Get-Item -LiteralPath $script:out).LastWriteTimeUtc -eq $stamp) 'an unchanged roster must not be rewritten'
}

Check 'Reuse never expires by age; only -Refresh re-queries GitHub' {
    Reset-World
    Invoke-Update | Out-Null
    Assert (-not (Test-VmrQBContactsStale -Path $script:out -GuidePath $script:guidePath)) 'a roster that covers the guide is not stale'
    $later = Invoke-Update @{ Today = $script:now.AddDays(400) }
    Assert ($later.Reused.Count -eq 4 -and $later.Built.Count -eq 0 -and $script:calls.Count -eq 0) 'a year later nothing is refetched without -Refresh'
    Assert ((Invoke-Update @{ Refresh = $true }).Built.Count -eq 4) '-Refresh rebuilds every generated entry'
}

Check 'A pulled guide that adds a team makes the roster stale, and only that team is looked up' {
    Reset-World
    Invoke-Update | Out-Null
    $newer = Join-Path $sandbox 'guide-newer.md'
    [IO.File]::WriteAllText($newer, $guideText.Replace('| roslyn |', '| newrepo | [@dotnet/new-team](https://github.com/orgs/dotnet/teams/new-team) |' + "`n" + '| roslyn |'), [Text.UTF8Encoding]::new($false))
    Assert (Test-VmrQBContactsStale -Path $script:out -GuidePath $newer) 'a team the roster lacks means stale'
    $script:roster['new-team/maintainer'] = @('nina'); $script:activity['nina'] = '2026-10-02'
    $result = Invoke-Update @{ GuidePath = $newer }
    Assert ($result.Built.Count -eq 1 -and $result.Built[0] -eq '@dotnet/new-team' -and $result.Reused.Count -eq 4) "built: $($result.Built -join ', '); reused: $($result.Reused.Count)"
    Assert (@($script:calls | Where-Object { $_ -notmatch '^new-team/' }).Count -eq 0) "only the new team may be queried: $($script:calls -join ', ')"
}

Check 'Stale check: a missing roster is stale; a hand-written roster that covers the guide never goes stale' {
    Reset-World
    Assert (Test-VmrQBContactsStale -Path $script:out -GuidePath $script:guidePath) 'no roster means build one'
    $dir = Split-Path -Parent $script:out; [void][IO.Directory]::CreateDirectory($dir)
    $contacts = [ordered]@{}
    foreach ($team in '@dotnet/dnceng', '@dotnet/fsharp', '@dotnet/roslyn-infrastructure', '@dotnet/prodconsvcs') { $contacts[$team] = [ordered]@{ github = 't'; teams = 'T'; basis = 'me' } }
    [ordered]@{ contacts = $contacts } | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $script:out -Encoding utf8
    Assert (-not (Test-VmrQBContactsStale -Path $script:out -GuidePath $script:guidePath)) 'a hand-written roster that covers the guide is not stale'
}
Check 'An entry a person wrote is kept; -Force replaces it' {
    Reset-World
    [void][IO.Directory]::CreateDirectory((Split-Path -Parent $script:out))
    '{"contacts":{"@dotnet/fsharp":{"github":"example-login-one","teams":"First Example","basis":"operator-specified contact"}}}' | Set-Content -LiteralPath $script:out -Encoding utf8
    $kept = Invoke-Update
    Assert ($kept.Entries['@dotnet/fsharp'].github -eq 'example-login-one' -and $kept.Preserved -contains '@dotnet/fsharp') 'the person''s choice must win'
    Assert ($script:calls -notcontains 'fsharp/maintainer') 'GitHub must not even be asked about it'
    Assert ($kept.Built.Count -eq 3) 'the other teams are still built'
    $forced = Invoke-Update @{ Force = $true }
    Assert ($forced.Entries['@dotnet/fsharp'].github -eq 'carol') '-Force rebuilds from GitHub'
}

Check 'A first-generation roster entry (basis "team maintainer", no source) is rebuilt from GitHub; other bases are kept' {
    Reset-World
    [void][IO.Directory]::CreateDirectory((Split-Path -Parent $script:out))
    '{"contacts":{"@dotnet/dnceng":{"github":"old","teams":"Old","basis":"team maintainer"},"@dotnet/fsharp":{"github":"example-login-one","teams":"First Example","basis":"operator-specified contact"}}}' | Set-Content -LiteralPath $script:out -Encoding utf8
    Assert (Test-VmrQBContactsStale -Path $script:out -GuidePath $script:guidePath) 'a legacy derived entry has no verification date, so the roster is stale'
    $result = Invoke-Update
    Assert ($result.Entries['@dotnet/dnceng'].github -eq 'bob' -and $result.Entries['@dotnet/dnceng'].source -eq 'github-maintainers') 'the derived entry is rebuilt and gains provenance'
    Assert ($result.Entries['@dotnet/fsharp'].github -eq 'example-login-one') 'a deliberate choice survives the migration'
}

Check 'A contact for a team the guide no longer lists survives only if a person wrote it' {
    Reset-World
    [void][IO.Directory]::CreateDirectory((Split-Path -Parent $script:out))
    @'
{"contacts":{
 "@dotnet/by-hand":{"github":"h","teams":"H","basis":"me"},
 "@dotnet/gone":{"github":"g","teams":"G","basis":"team maintainer","source":"github-maintainers","verifiedAt":"2026-10-06"}}}
'@ | Set-Content -LiteralPath $script:out -Encoding utf8
    $result = Invoke-Update
    Assert ($result.Entries.Contains('@dotnet/by-hand')) 'a hand-written contact is kept'
    Assert (-not $result.Entries.Contains('@dotnet/gone')) 'a generated contact for a removed team is dropped'
}

Check 'Failures: an existing entry is kept, a new team is reported missing, a sign-in problem is surfaced verbatim' {
    Reset-World
    Invoke-Update | Out-Null
    $script:failTeams = @{ dnceng = 'error' }
    $kept = Invoke-Update @{ Refresh = $true }
    Assert ($kept.Entries['@dotnet/dnceng'].github -eq 'bob' -and @($kept.Problems | Where-Object { $_ -match 'dnceng kept as it was' }).Count -eq 1) 'a failed refresh must not erase a good contact'
    Reset-World
    $script:failTeams = @{ fsharp = 'notfound' }
    $result = Invoke-Update
    Assert ($result.Missing -contains '@dotnet/fsharp' -and -not $result.Entries.Contains('@dotnet/fsharp')) 'a team GitHub cannot show has no contact'
    Assert (-not $result.AuthProblem) 'a 404 is not a sign-in problem'
    Reset-World
    $script:failTeams = @{ prodconsvcs = 'auth' }
    $result = Invoke-Update
    Assert ($result.AuthProblem -match 'HTTP stub failure for prodconsvcs') 'the sign-in message must be kept verbatim'
    Assert ($result.Missing -contains '@dotnet/prodconsvcs') 'and the team is reported missing'
}

Check 'Only bot accounts, or nobody, is a missing contact rather than a bot ping' {
    Reset-World
    $script:roster['fsharp/maintainer'] = @('copilot-bot', 'x[bot]'); $script:roster['fsharp/member'] = @('y[bot]')
    $result = Invoke-Update
    Assert ($result.Missing -contains '@dotnet/fsharp') 'bots are not contacts'
}

Check 'Drift between the guide and routing.json is reported both ways' {
    Reset-World
    $withGhost = $guideText -replace '\| \*General VMR infra\*', "| ghost | [@dotnet/ghost-team](https://github.com/orgs/dotnet/teams/ghost-team) |`n| *General VMR infra*"
    $ghostGuide = Join-Path $sandbox 'ghost-guide.md'
    [IO.File]::WriteAllText($ghostGuide, $withGhost, [Text.UTF8Encoding]::new($false))
    $script:roster['ghost-team/maintainer'] = @('zed')
    $result = Invoke-Update @{ GuidePath = $ghostGuide }
    Assert (@($result.Drift | Where-Object { $_ -match '@dotnet/ghost-team is in the guide but not in routing\.json' }).Count -eq 1) 'a guide-only team must be flagged'
    Assert (@($result.Drift | Where-Object { $_ -match 'is in routing\.json but not in the guide' }).Count -ge 1) 'a routing-only team must be flagged'
}

Check '-Preview reports the plan and writes nothing' {
    Reset-World
    $result = Invoke-Update @{ Preview = $true }
    Assert ($result.Built.Count -eq 4 -and $result.Changed -and -not $result.Written) 'preview builds the answer without writing it'
    Assert (-not (Test-Path -LiteralPath $script:out)) 'no file may appear'
}

Check 'The roster lives only in the local folder: there is no shipped fallback' {
    Reset-World
    $paths = Get-VmrQBPaths
    $expected = Join-Path $paths.LocalKnowledgeDir 'team-contacts.json'
    Assert ((Get-VmrQBContactsPath) -eq $expected -and (Get-VmrQBContactsPath -ForWrite) -eq $expected) 'always the local path'
    Assert (-not $expected.StartsWith($paths.KnowledgeRoot)) 'never inside the shipped knowledge folder'
    Remove-Item -LiteralPath $expected -Force -ErrorAction SilentlyContinue
    Assert (@((Read-VmrQBContacts -Path (Get-VmrQBContactsPath)).Keys).Count -eq 0) 'no local roster means an empty one, whatever else is on disk'
}
Remove-Item $sandbox -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue
if ($failures) { Write-Host "$failures contact roster test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '17 offline contact-roster tests. Stubbed GitHub lookups and temporary files only; no network, email, or external changes.' -ForegroundColor Green
