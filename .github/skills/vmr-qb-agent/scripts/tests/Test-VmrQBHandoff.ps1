[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\history\history.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-handoff-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
function Use-State([string] $Name) {
    $dir = Join-Path $sandbox $Name
    [void][IO.Directory]::CreateDirectory($dir)
    $env:VMR_QB_HOME = $dir
    return Get-VmrQBPaths -Create
}
function Write-Json([string] $Path, $Value) {
    [void][IO.Directory]::CreateDirectory((Split-Path -Parent $Path))
    [IO.File]::WriteAllText($Path, (ConvertTo-Json -InputObject $Value -Depth 20), [Text.UTF8Encoding]::new($false))
}
function New-FullState([string] $Name) {
    $paths = Use-State $Name
    Initialize-VmrIssueStore -Path $paths.HistoryPath | Out-Null
    Add-VmrIssueOccurrence -Path $paths.HistoryPath -Name 'Example failure one' -ReportDate '2026-10-01' -BuildIds @('9000001') -OwnerTeam '@dotnet/dnceng' -Source 'daily-run' | Out-Null
    Add-VmrIssueOccurrence -Path $paths.HistoryPath -Name 'Example failure two' -ReportDate '2026-10-02' -BuildIds @('9000002') -Source 'daily-run' | Out-Null
    Write-Json $paths.LocalContextPath ([ordered]@{ schemaVersion = 1; entries = @([ordered]@{ id = 'ctx-a'; title = 'A'; operatorNote = 'from handoff' }) })
    Write-Json $paths.AliasesPath ([ordered]@{ groups = @([ordered]@{ canonical = 'Example failure one'; aliases = @('Example failure 1') }) })
    Write-Json $paths.ReportedLinksPath ([ordered]@{ reports = @([ordered]@{ note = 'reported once'; match = 'Example'; buildIds = @(9000001) }) })
    Write-Json (Get-VmrQBContactsPath -ForWrite) ([ordered]@{ contacts = [ordered]@{
                '@dotnet/example-team' = [ordered]@{ github = 'picked-by-hand'; teams = 'Picked By Hand'; basis = 'operator-specified contact' }
                '@dotnet/example-generated' = [ordered]@{ github = 'generated'; teams = 'Generated'; basis = 'team maintainer'; source = 'github-maintainers'; verifiedAt = '2026-10-01' } } })
    return $paths
}

Check 'Export writes one JSON file holding history, context, aliases, links and only hand-picked contacts' {
    $a = New-FullState 'a'
    $result = Export-VmrQBHandoff -OutputPath (Join-Path $sandbox 'bundle.json')
    Assert ($result.issues -eq 2 -and $result.occurrences -eq 2 -and $result.contextEntries -eq 1 -and $result.aliasGroups -eq 1 -and $result.reportedLinks -eq 1) "counts: $($result | ConvertTo-Json -Compress)"
    Assert ($result.contacts -eq 1) 'a generated contact is rebuilt from GitHub and must not travel; only the hand-picked one does'
    $text = [IO.File]::ReadAllText((Join-Path $sandbox 'bundle.json'))
    Assert (-not $text.Contains("`r`n")) 'LF line endings'
    $bundle = $text | ConvertFrom-Json -Depth 50
    Assert ($bundle.schema -eq 'vmr-qb-handoff' -and $bundle.schemaVersion -eq 1) 'schema stamp'
    Assert (@($bundle.contacts.PSObject.Properties.Name) -contains '@dotnet/example-team' -and @($bundle.contacts.PSObject.Properties.Name) -notcontains '@dotnet/example-generated') 'contacts'
}

Check 'The default output path is in the local folder (handoff/), which is git-ignored' {
    $paths = Use-State 'a'
    $result = Export-VmrQBHandoff -Now ([datetime]'2026-10-07')
    Assert ($result.path -eq (Join-Path $paths.HandoffDir 'vmr-qb-handoff-2026-10-07.json')) "path: $($result.path)"
    Assert ($result.path.StartsWith($paths.StateRoot)) 'inside the state directory'
}

Check 'Import into a fresh state restores everything, and a second import changes nothing' {
    $b = Use-State 'b'
    $first = Import-VmrQBHandoff -InputPath (Join-Path $sandbox 'bundle.json')
    Assert ($first.history.issuesAfter -eq 2 -and $first.contextAdded -eq 1 -and $first.aliasGroupsAdded -eq 1 -and $first.reportedLinksAdded -eq 1 -and $first.contactsAdded -eq 1) "first: $($first | ConvertTo-Json -Depth 4 -Compress)"
    Assert ((Read-VmrQBJsonOrNull $b.LocalContextPath).entries[0].id -eq 'ctx-a') 'context file written'
    Assert ((Read-VmrQBJsonOrNull $b.AliasesPath).groups[0].canonical -eq 'Example failure one') 'aliases file written'
    Assert ((Read-VmrQBContacts -Path (Get-VmrQBContactsPath))['@dotnet/example-team'].github -eq 'picked-by-hand') 'hand-picked contact written'
    $before = (Get-Item $b.HistoryPath).LastWriteTimeUtc
    $second = Import-VmrQBHandoff -InputPath (Join-Path $sandbox 'bundle.json')
    Assert ($second.contextAdded -eq 0 -and $second.aliasGroupsAdded -eq 0 -and $second.reportedLinksAdded -eq 0 -and $second.contactsAdded -eq 0) "second: $($second | ConvertTo-Json -Depth 4 -Compress)"
    Assert ($second.history.issuesBefore -eq 2 -and $second.history.issuesAfter -eq 2) 'history unchanged'
    Assert ((Get-Item $b.HistoryPath).LastWriteTimeUtc -eq $before) 'an unchanged history is not rewritten'
}

Check 'What the importing QB already recorded wins; only new entries and new aliases arrive' {
    $c = Use-State 'c'
    Write-Json $c.LocalContextPath ([ordered]@{ schemaVersion = 1; entries = @([ordered]@{ id = 'ctx-a'; title = 'A'; operatorNote = 'mine' }) })
    Write-Json $c.AliasesPath ([ordered]@{ groups = @([ordered]@{ canonical = 'Example failure one'; aliases = @('Local alias') }) })
    Write-Json (Get-VmrQBContactsPath -ForWrite) ([ordered]@{ contacts = [ordered]@{ '@dotnet/example-team' = [ordered]@{ github = 'mine'; teams = 'Mine'; basis = 'me' } } })
    $result = Import-VmrQBHandoff -InputPath (Join-Path $sandbox 'bundle.json')
    Assert ($result.contextAdded -eq 0 -and (Read-VmrQBJsonOrNull $c.LocalContextPath).entries[0].operatorNote -eq 'mine') 'a context entry with the same id is not replaced'
    $group = (Read-VmrQBJsonOrNull $c.AliasesPath).groups | Where-Object canonical -eq 'Example failure one'
    Assert (@($group.aliases) -contains 'Local alias' -and @($group.aliases) -contains 'Example failure 1') 'alias groups with one canonical name merge their aliases'
    Assert ((Read-VmrQBContacts -Path (Get-VmrQBContactsPath))['@dotnet/example-team'].github -eq 'mine') 'my contact for a team is kept'
}

Check '-Preview reports the plan and writes nothing' {
    $d = Use-State 'd'
    $result = Import-VmrQBHandoff -InputPath (Join-Path $sandbox 'bundle.json') -Preview
    Assert ($result.preview -and $result.history.issuesAfter -eq 2 -and $result.contextAdded -eq 1) 'the preview must describe the import'
    Assert (-not (Test-Path -LiteralPath $d.HistoryPath) -and -not (Test-Path -LiteralPath $d.LocalContextPath) -and -not (Test-Path -LiteralPath $d.AliasesPath) -and -not (Test-Path -LiteralPath $d.ReportedLinksPath)) 'nothing may be created'
}

Check 'A plain history export (the old handoff format) imports as history only' {
    $e = Use-State 'e'
    $plain = Join-Path $sandbox 'plain.json'
    $source = Use-State 'a'
    Export-VmrIssueHistory -Path $source.HistoryPath -OutputPath $plain | Out-Null
    $e = Use-State 'e2'
    $result = Import-VmrQBHandoff -InputPath $plain
    Assert ($result.history.issuesAfter -eq 2 -and $result.contextAdded -eq 0 -and $result.contactsAdded -eq 0) 'history arrives, nothing else is invented'
}

Check 'A bundle from a newer skill is refused, and a missing file is a clear error' {
    $newer = Join-Path $sandbox 'newer.json'
    Write-Json $newer ([ordered]@{ schema = 'vmr-qb-handoff'; schemaVersion = 99 })
    $threw = $false; try { Import-VmrQBHandoff -InputPath $newer | Out-Null } catch { $threw = $_.Exception.Message -match 'newer than this skill' }
    Assert $threw 'a newer schema must be refused with an instruction'
    $threw = $false; try { Import-VmrQBHandoff -InputPath (Join-Path $sandbox 'missing.json') | Out-Null } catch { $threw = $_.Exception.Message -match 'not found' }
    Assert $threw 'a missing file must say so'
}

Check 'Replay turns the six-column report tables into occurrences, is idempotent, and honours -Date' {
    $f = Use-State 'f'
    Initialize-VmrIssueStore -Path $f.HistoryPath | Out-Null
    [void][IO.Directory]::CreateDirectory($f.ReportsDir)
    $header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
    foreach ($day in '2026-10-01', '2026-10-02') {
        $lines = @('# Report', '', $header, '|---|---|---|---|---|---|',
            "| BH #1 - Example failure for $day | [9000001](https://dev.azure.com/example/_build/results?buildId=9000001) | Y | None found | Where: GitHub - x<br>Copy/paste: `"@dotnet/dnceng please check`" | None found |")
        [IO.File]::WriteAllText((Join-Path $f.ReportsDir "overall-summary-$day.md"), ($lines -join "`n") + "`n", [Text.UTF8Encoding]::new($false))
    }
    $all = Update-VmrIssueHistory
    Assert ($all.imported -eq 2 -and $all.reports -eq 2 -and $all.issues -eq 2) "replayed: $($all | ConvertTo-Json -Depth 3 -Compress)"
    $again = Update-VmrIssueHistory
    Assert ($again.issues -eq 2 -and @((Get-VmrIssueHistory -Path $f.HistoryPath -Limit 100)).Count -eq 2) 'a second replay adds nothing'
    $g = Use-State 'g'
    Initialize-VmrIssueStore -Path $g.HistoryPath | Out-Null
    [void][IO.Directory]::CreateDirectory($g.ReportsDir)
    Copy-Item (Join-Path $f.ReportsDir '*.md') $g.ReportsDir
    $one = Update-VmrIssueHistory -Date '2026-10-02'
    Assert ($one.imported -eq 1 -and $one.reports -eq 1) '-Date must limit the replay to that report'
}

Check 'Aliases fold from the local file; repair reports what it did; no alias list ships' {
    $h = Use-State 'h'
    Initialize-VmrIssueStore -Path $h.HistoryPath | Out-Null
    Add-VmrIssueOccurrence -Path $h.HistoryPath -Name 'Example failure one' -ReportDate '2026-10-01' -BuildIds @('9000001') -Source 'daily-run' | Out-Null
    Add-VmrIssueOccurrence -Path $h.HistoryPath -Name 'Example failure 1' -ReportDate '2026-10-02' -BuildIds @('9000001') -Source 'daily-run' | Out-Null
    Write-Json $h.AliasesPath ([ordered]@{ groups = @([ordered]@{ canonical = 'Example failure one'; aliases = @('Example failure 1') }) })
    Assert (@(Get-VmrQBAliasFiles).Count -ge 1 -and (@(Get-VmrQBAliasFiles) -contains $h.AliasesPath)) 'the local alias file is used'
    $result = Repair-VmrIssueHistory
    Assert ($result.aliasGroups -ge 1 -and $result.unknownOwners -eq 0) "repair: $($result | ConvertTo-Json -Depth 3 -Compress)"
    $issue = Get-VmrIssueHistory -Path $h.HistoryPath -Name 'Example failure one'
    Assert ($issue.issue.occurrences -eq 2) 'the alias was folded into its canonical issue'
    $threw = $false; try { Set-VmrIssueAliases -AliasFile (Join-Path $sandbox 'nope.json') | Out-Null } catch { $threw = $_.Exception.Message -match 'not found' }
    Assert $threw 'an explicit alias file that does not exist is an error'
    Assert (-not (Test-Path -LiteralPath (Join-Path (Get-VmrQBPaths).KnowledgeRoot 'issue-aliases.json'))) 'the skill must not ship an alias list'
}

Check './history/Invoke-History.ps1 alias folds one name into another in the store, and a handoff carries it' {
    $j = Use-State 'j'
    Initialize-VmrIssueStore -Path $j.HistoryPath | Out-Null
    Add-VmrIssueOccurrence -Path $j.HistoryPath -Name 'Example failure one' -ReportDate '2026-10-01' -BuildIds @('9000001') -Source 'daily-run' | Out-Null
    Add-VmrIssueOccurrence -Path $j.HistoryPath -Name 'Example failure uno' -ReportDate '2026-10-02' -BuildIds @('9000001') -Source 'daily-run' | Out-Null
    $qb = Join-Path $PSScriptRoot '..\history\Invoke-History.ps1'
    $null = & pwsh -NoProfile -File $qb alias -Alias 'Example failure uno' -Canonical 'Example failure one' 2>&1
    Assert ($LASTEXITCODE -eq 0) 'the command succeeds'
    Assert ((Get-VmrIssueHistory -Path $j.HistoryPath -Name 'Example failure one').issue.occurrences -eq 2) 'both days now belong to the canonical issue'
    $bundle = Join-Path $sandbox 'alias-bundle.json'
    Export-VmrQBHandoff -OutputPath $bundle | Out-Null
    $k = Use-State 'k'
    Import-VmrQBHandoff -InputPath $bundle | Out-Null
    Assert ((Get-VmrIssueHistory -Path $k.HistoryPath -Name 'Example failure uno').issue.name -eq 'Example failure one') 'the alias survives the handoff, so no alias file is needed'
    $threw = $false; $out = & pwsh -NoProfile -File $qb alias -Alias 'only one' 2>&1 | Out-String; Assert ($out -match 'needs both') 'one of the two names alone is refused'
}

Check 'Invoke-History.ps1 without an action says what is missing'  {
    $qb = Join-Path $PSScriptRoot '..\history\Invoke-History.ps1'
    $out = & pwsh -NoProfile -File $qb 2>&1 | Out-String
    Assert ($out -match 'Action') 'history without an action says what is missing'
    Assert ($LASTEXITCODE -ne 0) 'and it exits non-zero'
}
Check 'Invoke-History.ps1: search and export work end to end against a throwaway state directory' {
    $i = Use-State 'i'
    Initialize-VmrIssueStore -Path $i.HistoryPath | Out-Null
    Add-VmrIssueOccurrence -Path $i.HistoryPath -Name 'Example socket failure' -ReportDate '2026-10-01' -BuildIds @('9000001') -Source 'daily-run' | Out-Null
    $qb = Join-Path $PSScriptRoot '..\history\Invoke-History.ps1'
    $found = & pwsh -NoProfile -File $qb search -Text socket -Json 2>&1 | Out-String | ConvertFrom-Json
    Assert (@($found).Count -eq 1 -and $found[0].name -eq 'Example socket failure') 'search finds it'
    $out = Join-Path $sandbox 'cli-export.json'
    $null = & pwsh -NoProfile -File $qb export -OutputPath $out 2>&1
    Assert ($LASTEXITCODE -eq 0 -and (Test-Path -LiteralPath $out)) 'export writes the handoff file'
    Assert ((Get-Content $out -Raw | ConvertFrom-Json).schema -eq 'vmr-qb-handoff') 'and it is a handoff bundle'
}

Check 'Every Invoke-History.ps1 action that takes -Json speaks JSON, and a list with nothing in it is [] rather than nothing' {
    $state = Join-Path $sandbox 'json-cli'
    $qb = Join-Path $PSScriptRoot '..\history\Invoke-History.ps1'
    $saved = $env:VMR_QB_HOME
    $env:VMR_QB_HOME = $state
    try {
        $empty = (& pwsh -NoProfile -File $qb search -Text 'nothing-matches-this' -Json 2>&1 | Out-String).Trim()
        Assert ($LASTEXITCODE -eq 0 -and $empty -eq '[]') "search on a fresh store must print []: '$empty'"
        $none = (& pwsh -NoProfile -File $qb duplicates -Json 2>&1 | Out-String).Trim()
        Assert ($LASTEXITCODE -eq 0 -and $none -eq '[]') "duplicates with none must print []: '$none'"
        $one = & pwsh -NoProfile -File $qb search -Text 'x' -Json 2>&1 | Out-String | ConvertFrom-Json
        Assert (@($one).Count -eq 0) 'the empty list parses as an empty list'
        $env:VMR_QB_HOME = $state
        $paths = Get-VmrQBPaths -Create
        Initialize-VmrIssueStore -Path $paths.HistoryPath | Out-Null
        Add-VmrIssueOccurrence -Path $paths.HistoryPath -Name 'Only finding' -ReportDate '2026-10-01' -BuildIds @('9000001') | Out-Null
        $found = @(& pwsh -NoProfile -File $qb search -Text 'only' -Json 2>&1 | Out-String | ConvertFrom-Json)
        Assert ($found.Count -eq 1 -and $found[0].name -eq 'Only finding') 'a single hit is still a one-item list'
        $replay = & pwsh -NoProfile -File $qb replay -Date '2026-10-01' -Json 2>&1 | Out-String
        Assert ($LASTEXITCODE -eq 0 -and $null -ne ($replay | ConvertFrom-Json)) "replay -Json must be JSON: $replay"
        $bundle = Join-Path $sandbox 'json-cli-bundle.json'
        $export = & pwsh -NoProfile -File $qb export -OutputPath $bundle -Json 2>&1 | Out-String
        Assert ($LASTEXITCODE -eq 0 -and ($export | ConvertFrom-Json).path -eq $bundle) "export -Json must be JSON: $export"
        $import = & pwsh -NoProfile -File $qb import -InputPath $bundle -Preview -Json 2>&1 | Out-String
        Assert ($LASTEXITCODE -eq 0 -and ($import | ConvertFrom-Json).preview -eq $true) "import -Json must be JSON: $import"
    }
    finally { $env:VMR_QB_HOME = $saved }
}
function Get-RoutingFor([object] $Paths) { return (Get-Content -LiteralPath $Paths.LocalRoutingPath -Raw | ConvertFrom-Json -Depth 20) }
function Save-RoutingFor([object] $Paths, $Document) { [IO.File]::WriteAllText($Paths.LocalRoutingPath, (ConvertTo-Json -InputObject $Document -Depth 20), [Text.UTF8Encoding]::new($false)) }

Check 'A handoff carries only the sender''s own routing input, versioned, with no stamp, path or setting' {
    $a = New-FullState 'route-a'
    [void](Get-VmrQBRouting)
    Set-VmrQBLocalRoutingEntry -Section categories -Key 'sender-cat' -Value ([pscustomobject]@{ id = 'sender-cat'; name = 'Sender category'; ownerRepos = @('runtime'); owners = @('@dotnet/dnceng'); note = 'A note the sender wrote.' })
    Set-VmrQBLocalRoutingEntry -Section channelRules -Key 'Servicing chat' -Value 'The sender''s servicing rule.'
    $result = Export-VmrQBHandoff -OutputPath (Join-Path $sandbox 'route-bundle.json')
    Assert ($result.routingChanges -eq 2) "expected the two edits to travel, got $($result.routingChanges)"
    $text = [IO.File]::ReadAllText((Join-Path $sandbox 'route-bundle.json'))
    $bundle = $text | ConvertFrom-Json -Depth 50
    Assert ($bundle.routing.schemaVersion -eq 1) 'the routing section is versioned'
    $keys = @($bundle.routing.changes | ForEach-Object { "$($_.section)/$($_.key)" })
    Assert ($keys -contains 'categories/sender-cat' -and $keys -contains 'channelRules/Servicing chat') "the edits must travel: $($keys -join ', ')"
    Assert (@($bundle.routing.changes).Count -eq 2) 'the rest of the copy is the shipped file and must not travel'
    Assert ($text -notmatch 'baseSha256|shippedSha256|"(?:recipient|sender)"|[A-Za-z]:\\\\') 'no stamp, setting or file path may travel'
    $rule = @($bundle.routing.changes | Where-Object key -eq 'Servicing chat')[0]
    Assert ($null -ne $rule.base -and $rule.base -ne $rule.value) 'a change to a shipped entry carries the shipped entry it started from'
}

Check 'Importing routing adds new entries, keeps yours on conflict, refuses what would not load, previews exactly, and is idempotent' {
    $a = New-FullState 'route-from'
    [void](Get-VmrQBRouting)
    Set-VmrQBLocalRoutingEntry -Section categories -Key 'sender-cat' -Value ([pscustomobject]@{ id = 'sender-cat'; name = 'Sender category'; owners = @('@dotnet/dnceng') })
    Set-VmrQBLocalRoutingEntry -Section channelRules -Key 'Servicing chat' -Value 'The sender''s rule.'
    Set-VmrQBLocalRoutingEntry -Section channelRules -Key 'Preview/RC chat' -Value 'The sender''s preview rule.'
    $bundlePath = Join-Path $sandbox 'route-import.json'
    [void](Export-VmrQBHandoff -OutputPath $bundlePath)
    # A hand-edited bundle can carry things that must not be imported.
    $bundle = Get-Content -LiteralPath $bundlePath -Raw | ConvertFrom-Json -Depth 50
    $bundle.routing.changes = @($bundle.routing.changes) + @(
        [pscustomobject]@{ section = 'categories'; key = 'ghost'; value = [pscustomobject]@{ id = 'ghost'; name = 'Ghost'; owners = @('@dotnet/not-a-team') }; base = $null },
        [pscustomobject]@{ section = 'categories'; key = 'leaky'; value = [pscustomobject]@{ id = 'leaky'; name = 'Leaky'; note = ('Ask someone@example.com or see ' + 'C:\' + 'Users\' + 'someone\notes.') }; base = $null })
    [IO.File]::WriteAllText($bundlePath, (ConvertTo-Json -InputObject $bundle -Depth 50), [Text.UTF8Encoding]::new($false))

    $b = New-FullState 'route-to'
    [void](Get-VmrQBRouting)
    Set-VmrQBLocalRoutingEntry -Section channelRules -Key 'Servicing chat' -Value 'The importer''s own rule.'
    $before = (Get-FileHash -LiteralPath $b.LocalRoutingPath).Hash
    $preview = Import-VmrQBHandoff -InputPath $bundlePath -Preview
    Assert ((Get-FileHash -LiteralPath $b.LocalRoutingPath).Hash -eq $before) '-Preview must write nothing to the routing copy'
    $result = Import-VmrQBHandoff -InputPath $bundlePath
    $by = @{}; foreach ($r in $result.routing.results) { $by["$($r.Section)/$($r.Key)"] = $r }
    Assert ($by['categories/sender-cat'].Result -eq 'added') 'a new category must be added'
    Assert ($by['channelRules/Servicing chat'].Result -eq 'conflict') 'the importer''s own edit wins'
    Assert ($by['channelRules/Preview/RC chat'].Result -eq 'replaced') 'an entry the importer never changed takes the sender''s edit when both started from the same shipped entry'
    Assert ($by['categories/ghost'].Result -eq 'invalid' -and $by['categories/ghost'].Detail -match 'unknown team') 'an entry that would not load here is refused with the reason'
    Assert ($by['categories/leaky'].Result -eq 'invalid' -and $by['categories/leaky'].Detail -match 'email address or a file path') 'personal data in an entry is refused'
    Assert (($preview.routing.results | ForEach-Object { "$($_.Section)/$($_.Key)=$($_.Result)" }) -join ',' -eq (($result.routing.results | ForEach-Object { "$($_.Section)/$($_.Key)=$($_.Result)" }) -join ',')) '-Preview must predict exactly what the import does'
    $routing = Get-VmrQBRouting
    Assert ($routing.ChannelRules['Servicing chat'] -eq 'The importer''s own rule.') 'on conflict the importer''s rule is kept'
    Assert ($routing.ChannelRules['Preview/RC chat'] -eq 'The sender''s preview rule.') 'the untouched entry took the sender''s edit'
    Assert (@($routing.Categories | Where-Object id -eq 'sender-cat').Count -eq 1 -and @($routing.Categories | Where-Object id -in @('ghost', 'leaky')).Count -eq 0) 'only the safe category was added'
    Assert ((Format-QBHandoffSummary $result) -match 'routing: 1 added, 1 replaced, 1 kept as conflicts, 2 refused') "the summary must say what happened: $(Format-QBHandoffSummary $result)"
    $afterFirst = (Get-FileHash -LiteralPath $b.LocalRoutingPath).Hash
    $again = Import-VmrQBHandoff -InputPath $bundlePath
    Assert ((Get-FileHash -LiteralPath $b.LocalRoutingPath).Hash -eq $afterFirst) 'a repeated import must not change the routing copy'
    Assert (@($again.routing.results | Where-Object Result -in @('added', 'replaced')).Count -eq 0) 'a repeated import adds and replaces nothing'
}

Check 'An old bundle, a bundle with no routing, a newer routing version and a first-time importer are all handled' {
    $old = New-FullState 'route-old'
    $bundlePath = Join-Path $sandbox 'route-old.json'
    [void](Export-VmrQBHandoff -OutputPath $bundlePath)
    $bundle = Get-Content -LiteralPath $bundlePath -Raw | ConvertFrom-Json -Depth 50
    $bundle.PSObject.Properties.Remove('routing')
    [IO.File]::WriteAllText($bundlePath, (ConvertTo-Json -InputObject $bundle -Depth 50), [Text.UTF8Encoding]::new($false))
    $fresh = Use-State 'route-fresh'
    $result = Import-VmrQBHandoff -InputPath $bundlePath
    Assert ($null -eq $result.routing) 'an old bundle has no routing result'
    # The owner-repair step reads routing, so the copy may be made (first use); it must still be an untouched copy of the shipped file.
    if (Test-Path -LiteralPath $fresh.LocalRoutingPath) {
        $shippedDoc = Get-Content -LiteralPath (Join-Path $fresh.KnowledgeRoot 'routing.json') -Raw | ConvertFrom-Json -Depth 20
        $copyDoc = Get-Content -LiteralPath $fresh.LocalRoutingPath -Raw | ConvertFrom-Json -Depth 20
        Assert (@(Get-VmrQBRoutingDiff -Local $copyDoc -Shipped $shippedDoc -Base $null).Count -eq 0) 'importing without routing must not change the routing copy'
    }
    Assert ((Format-QBHandoffSummary $result) -notmatch 'routing') 'the summary says nothing about routing'

    $withNewer = [pscustomobject]@{ schema = 'vmr-qb-handoff'; schemaVersion = 1; routing = [pscustomobject]@{ schemaVersion = 2; changes = @([pscustomobject]@{ section = 'categories'; key = 'x'; value = [pscustomobject]@{ id = 'x'; name = 'X' } }) } }
    $newerPath = Join-Path $sandbox 'route-newer.json'
    [IO.File]::WriteAllText($newerPath, (ConvertTo-Json -InputObject $withNewer -Depth 20), [Text.UTF8Encoding]::new($false))
    $newer = Import-VmrQBHandoff -InputPath $newerPath
    Assert ($newer.routing.skipped -match 'version 2' -and @($newer.routing.results).Count -eq 0) 'a newer routing version is skipped, not guessed at'
    Assert ((Format-QBHandoffSummary $newer) -match 'routing skipped') 'and the summary says so'

    # A first-time importer has no local copy yet: the import makes it once (copy-once) and then applies the change.
    $sender = Use-State 'route-sender'
    Initialize-VmrIssueStore -Path $sender.HistoryPath | Out-Null
    [void](Get-VmrQBRouting)
    Set-VmrQBLocalRoutingEntry -Section categories -Key 'first-cat' -Value ([pscustomobject]@{ id = 'first-cat'; name = 'First'; owners = @('@dotnet/dnceng') })
    $firstPath = Join-Path $sandbox 'route-first.json'
    [void](Export-VmrQBHandoff -OutputPath $firstPath)
    $receiver = Use-State 'route-receiver'
    Assert (-not (Test-Path -LiteralPath $receiver.LocalRoutingPath)) 'the receiver starts with no routing copy'
    $firstPreview = Import-VmrQBHandoff -InputPath $firstPath -Preview
    Assert (-not (Test-Path -LiteralPath $receiver.LocalRoutingPath)) '-Preview must not even create the copy'
    Assert ($firstPreview.routing.results[0].Result -eq 'added') 'the preview already says it would add the category'
    $first = Import-VmrQBHandoff -InputPath $firstPath
    Assert ((Test-Path -LiteralPath $receiver.LocalRoutingPath) -and (Test-Path -LiteralPath $receiver.RoutingBasePath)) 'the copy and its base snapshot are made once'
    Assert ($first.routing.results[0].Result -eq 'added' -and @((Get-VmrQBRouting).Categories | Where-Object id -eq 'first-cat').Count -eq 1) 'and the sender''s category is active'
}

Check 'A malformed local document stops the import before anything changes' {
    $a = New-FullState 'route-bad-src'
    $bundlePath = Join-Path $sandbox 'route-bad.json'
    [void](Export-VmrQBHandoff -OutputPath $bundlePath)
    $b = Use-State 'route-bad-dst'
    Write-Json $b.LocalContextPath ([ordered]@{ schemaVersion = 1; entries = @() })
    [IO.File]::WriteAllText($b.ReportedLinksPath, '{ "reports": [ nope')
    $threw = $null
    try { Import-VmrQBHandoff -InputPath $bundlePath | Out-Null } catch { $threw = $_.Exception.Message }
    Assert ($threw -match 'reported links file .*not valid JSON.*Nothing was imported') "expected a clear refusal: $threw"
    Assert (-not (Test-Path -LiteralPath $b.HistoryPath)) 'the history must not have been merged first'
}
Remove-Item $sandbox -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue
if ($failures) { Write-Host "$failures handoff test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '16 offline handoff and history-command tests. Temporary state directories only; no network, email, or external changes.' -ForegroundColor Green
