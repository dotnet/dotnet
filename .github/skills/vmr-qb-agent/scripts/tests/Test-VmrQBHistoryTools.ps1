[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\common\core.ps1')
. (Join-Path $PSScriptRoot '..\history\history.ps1')

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }
function Assert-Throws([scriptblock] $Body, [string] $Pattern) {
    try { & $Body | Out-Null } catch {
        if ($_.Exception.Message -notmatch $Pattern) { throw "wrong error: $($_.Exception.Message)" }
        return
    }
    throw "Expected an error matching: $Pattern"
}

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-history-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$env:VMR_QB_HOME = Join-Path $sandbox 'state'

function New-World {
    $dir = Join-Path $sandbox ([guid]::NewGuid().ToString('N')); [void][IO.Directory]::CreateDirectory($dir)
    $history = Join-Path $dir 'issue-history.json'
    Initialize-VmrIssueStore -Path $history | Out-Null
    Add-VmrIssueOccurrence -Path $history -Name 'Arcade SDK package unavailable' -ReportDate '2026-10-07' -BuildIds @('9100201') | Out-Null
    Add-VmrIssueOccurrence -Path $history -Name 'macOS socket/resource failure' -ReportDate '2026-09-25' -BuildIds @('9100202') | Out-Null
    Add-VmrIssueOccurrence -Path $history -Name 'macOS socket/resource failure' -ReportDate '2026-10-07' -BuildIds @('9100203') | Out-Null
    return @{ dir = $dir; history = $history; links = (Join-Path $dir 'reported-links.json'); local = (Join-Path $dir 'knowledge') }
}
function Update([hashtable] $World, [hashtable] $Arguments) {
    Set-VmrQBOperatorUpdate -HistoryPath $World.history -LinksPath $World.links -LocalKnowledgeDir $World.local @Arguments
}

Check 'Recording an owner statement updates history with the speaker, the owner and an audit trail' {
    $w = New-World
    $r = Update $w @{ Finding = 'Arcade SDK package unavailable'; Status = 'owned'; Owner = '@dotnet/dnceng'; SaidBy = 'Alex'; Note = 'Alex says it is a transient feed outage; he will check.'; On = '2026-10-07' }
    $h = Get-VmrIssueHistory -Path $w.history -Name 'Arcade SDK package unavailable'
    Assert ($h.issue.status -eq 'owned' -and $h.issue.owner_team -eq '@dotnet/dnceng') "status/owner wrong: $($h.issue.status) $($h.issue.owner_team)"
    $last = @($h.statusTrail)[-1]
    Assert ($last.source -match 'operator: Alex') "source should name the speaker: $($last.source)"
    Assert ($last.note -match 'transient feed outage') 'the words must be kept'
    Assert ($r.slug -eq 'arcade-sdk-package-unavailable') 'slug should be returned'
}

Check 'Nothing is written when anything fails validation' {
    $w = New-World
    $before = (Get-Content $w.history -Raw)
    $cases = @(
        @{ a = @{ Finding = 'Arcade SDK package unavailable'; Status = 'owned'; Note = 'short' };                                                     p = 'what was actually said' },
        @{ a = @{ Finding = 'Arcade SDK package unavailable'; Status = 'owned'; Note = 'Alex owns this one now.'; Tracker = 'http://not-https/1' }; p = 'https URL' },
        @{ a = @{ Finding = 'Arcade SDK package unavailable'; Status = 'owned'; Note = 'Alex owns this one now.'; Builds = @(9100201) };           p = '-Builds requires -Match' },
        @{ a = @{ Finding = 'Arcade SDK package unavailable'; Status = 'owned'; Note = 'Alex owns this one now.'; Builds = @(1); Match = '(' };    p = 'not a valid regex' },
        @{ a = @{ Finding = 'Arcade SDK package unavailable'; Status = 'owned'; Note = 'Alex owns this one now.'; Owner = '@dotnet/totally-made-up' };     p = 'not a team that exists' },
        @{ a = @{ Finding = 'Arcade SDK package unavailable'; Status = 'owned'; Note = 'Alex owns this one now.'; AddContext = $true };             p = '-AddContext requires -AppliesTo' },
        @{ a = @{ Finding = 'Arcade SDK package unavailab1e'; Status = 'owned'; Note = 'Alex owns this one now.' };                                  p = 'No finding named' }
    )
    foreach ($case in $cases) { Assert-Throws { Update $w $case.a } $case.p }
    Assert ((Get-Content $w.history -Raw) -eq $before) 'a failed validation must not touch history'
    Assert (-not (Test-Path $w.links)) 'a failed validation must not create the links file'
    Assert (-not (Test-Path $w.local)) 'a failed validation must not create local knowledge'
}

Check 'A known-mistaken handle is corrected to the real team, with a warning, never silently' {
    $w = New-World
    $warnings = @()
    Update $w @{ Finding = 'macOS socket/resource failure'; Status = 'routed'; Owner = '@dotnet/runtime'; Note = 'Sent to the runtime owner for a look.'; WarningAction = 'SilentlyContinue'; WarningVariable = 'warnings' } | Out-Null
    $h = Get-VmrIssueHistory -Path $w.history -Name 'macOS socket/resource failure'
    Assert ($h.issue.owner_team -eq '@dotnet/dnr-codeflow') "the corrected owner must be stored, got $($h.issue.owner_team)"
}

Check 'A typo suggests close matches instead of silently creating a new finding' {
    $w = New-World
    $msg = $null
    try { Update $w @{ Finding = 'macOS socket failure'; Status = 'routed'; Note = 'Sent to the msbuild team as a bug.' } } catch { $msg = $_.Exception.Message }
    Assert ($msg -match 'No finding named') 'must refuse'
    Assert ($msg -match "macOS socket/resource failure") "must suggest the real name: $msg"
    Update $w @{ Finding = 'Brand new thing'; Status = 'open'; Note = 'A genuinely new finding to track.'; Create = $true } | Out-Null
    Assert ($null -ne (Get-VmrIssueHistory -Path $w.history -Name 'Brand new thing')) '-Create must create it'
}

Check 'Builds and a tracker are written to the QB links file, scoped by -Match, preserving earlier entries' {
    $w = New-World
    [ordered]@{ '$schema' = 'x'; readme = @('keep me'); reports = @([ordered]@{ note = 'earlier'; match = 'NuGet'; buildIds = @(1) }) } | ConvertTo-Json -Depth 6 | Set-Content $w.links -Encoding utf8
    Update $w @{ Finding = 'macOS socket/resource failure'; Status = 'routed'; Owner = '@dotnet/msbuild'; Note = 'Sent to the msbuild team as a bug.'; Builds = @(9100203, 9100202); Match = 'socket|buffer space'; Tracker = 'https://github.com/dotnet/msbuild/issues/12345' } | Out-Null
    $doc = Get-Content $w.links -Raw | ConvertFrom-Json
    Assert ($doc.readme -contains 'keep me') 'existing readme must be preserved'
    Assert (@($doc.reports).Count -eq 3) "expected earlier + note + tracker link, got $(@($doc.reports).Count)"
    Assert ($doc.reports[0].note -eq 'earlier') 'earlier entries must keep their place'
    $note = @($doc.reports | Where-Object { $_.PSObject.Properties['note'] -and $_.note -match 'msbuild team' })[0]
    Assert ($note.match -eq 'socket|buffer space' -and (@($note.buildIds) -contains 9100203)) 'the note must be scoped to its builds and category'
    $link = @($doc.reports | Where-Object { $_.PSObject.Properties['url'] })[0]
    Assert ($link.label -eq 'dotnet/msbuild#12345' -and $link.url -match 'issues/12345') "tracker link wrong: $($link | ConvertTo-Json -Compress)"
    $h = Get-VmrIssueHistory -Path $w.history -Name 'macOS socket/resource failure'
    Assert ($h.issue.tracker_url -eq 'https://github.com/dotnet/msbuild/issues/12345') 'the tracker must also be in history'
}

Check '-AddContext writes a local triage entry the model can read, and re-running replaces it' {
    $w = New-World
    $args1 = @{ Finding = 'macOS socket/resource failure'; Status = 'routed'; Owner = '@dotnet/msbuild'; Note = "Sent to the msbuild team as a bug. Link not recorded."; AddContext = $true; AppliesTo = "macOS legs where an MSBuild node exits with 'No buffer space available'."; SaidBy = 'operator'; On = '2026-10-07' }
    $r = Update $w $args1
    $path = Join-Path $w.local 'triage-context.local.json'
    Assert (Test-Path $path) 'local context not written'
    $doc = Get-Content $path -Raw | ConvertFrom-Json
    Assert (@($doc.entries).Count -eq 1 -and $doc.entries[0].id -eq 'macos-socket-resource-failure-2026-10-07') "entry id wrong: $($doc.entries[0].id)"
    Assert ($doc.entries[0].action -match 'already reported') 'the action must tell the model what to do'
    Assert ($doc.entries[0].verification -match 'Not independently verified') 'the entry must say it is an unverified statement'
    foreach ($field in 'title', 'operatorStatus', 'operatorNote', 'applicability', 'action', 'verification') { Assert ([string]$doc.entries[0].$field) "missing $field" }
    Update $w $args1 | Out-Null
    Assert (@((Get-Content $path -Raw | ConvertFrom-Json).entries).Count -eq 1) 're-running the same update must replace, not duplicate'
}

Check '-WhatIf previews and changes nothing' {
    $w = New-World
    $before = (Get-Content $w.history -Raw)
    $r = Update $w @{ Finding = 'Arcade SDK package unavailable'; Status = 'owned'; Owner = '@dotnet/dnceng'; Note = 'Alex owns this one now.'; Builds = @(9100201); Match = 'Arcade'; WhatIf = $true }
    Assert ($r.status -eq 'owned' -and $r.store) 'the plan should still be returned'
    Assert ((Get-Content $w.history -Raw) -eq $before) 'history changed under -WhatIf'
    Assert (-not (Test-Path $w.links)) 'links written under -WhatIf'
}

Check 'An operator-recorded owner survives a later model-written report (sticky provenance)' {
    $w = New-World
    Update $w @{ Finding = 'macOS socket/resource failure'; Status = 'routed'; Owner = '@dotnet/msbuild'; Note = 'Sent to the msbuild team as a bug.' } | Out-Null
    Add-VmrIssueOccurrence -Path $w.history -Name 'macOS socket/resource failure' -ReportDate '2026-10-08' -BuildIds @('9100204') -OwnerTeam '@dotnet/prodconsvcs' -TrackerUrl 'https://github.com/dotnet/dotnet/issues/1' | Out-Null
    $h = Get-VmrIssueHistory -Path $w.history -Name 'macOS socket/resource failure'
    Assert ($h.issue.owner_team -eq '@dotnet/msbuild') "the operator's owner was overwritten by a report: $($h.issue.owner_team)"
    Assert ($h.issue.occurrences -eq 3) 'the new occurrence must still be recorded'
}

Check 'Local triage context merges over the shipped one by id, and is a no-op without a local file' {
    $dir = Join-Path $sandbox 'merge'; [void][IO.Directory]::CreateDirectory($dir)
    $shipped = Join-Path $dir 'shipped.json'
    [ordered]@{ schemaVersion = 1; entries = @([ordered]@{ id = 'a'; title = 'shipped a' }, [ordered]@{ id = 'b'; title = 'shipped b' }) } | ConvertTo-Json -Depth 5 | Set-Content $shipped -Encoding utf8
    Assert ((Merge-VmrQBTriageContext -ShippedPath $shipped -LocalPath (Join-Path $dir 'none.json') -OutputPath (Join-Path $dir 'out.json')) -eq $shipped) 'with no local file the shipped path must be returned'
    $local = Join-Path $dir 'local.json'
    [ordered]@{ schemaVersion = 1; entries = @([ordered]@{ id = 'b'; title = 'local b' }, [ordered]@{ id = 'c'; title = 'local c' }) } | ConvertTo-Json -Depth 5 | Set-Content $local -Encoding utf8
    $out = Merge-VmrQBTriageContext -ShippedPath $shipped -LocalPath $local -OutputPath (Join-Path $dir 'out.json')
    $merged = (Get-Content $out -Raw | ConvertFrom-Json).entries
    Assert ((@($merged | ForEach-Object id) | Sort-Object) -join ',' -eq 'a,b,c') 'union by id expected'
    Assert ((@($merged | Where-Object id -eq 'b')[0]).title -eq 'local b') 'local must win on the same id'
    Assert (@(Get-ChildItem $dir -Filter '*.tmp').Count -eq 0) 'no temp files may be left behind'
}

Check 'Search finds a build in reverse, requires every word, resolves owner aliases, and demands a filter' {
    $w = New-World
    Update $w @{ Finding = 'macOS socket/resource failure'; Status = 'routed'; Owner = '@dotnet/dnr-codeflow'; Note = 'Routed to the runtime owner for a look.' } | Out-Null
    $byBuild = @(Search-VmrIssueHistory -Path $w.history -BuildId 9100201)
    Assert ($byBuild.Count -eq 1 -and $byBuild[0].name -eq 'Arcade SDK package unavailable' -and $byBuild[0].matchedOn -match '2026-10-07') "build lookup wrong: $($byBuild | ConvertTo-Json -Compress)"
    Assert (@(Search-VmrIssueHistory -Path $w.history -BuildId 1).Count -eq 0) 'an unseen build must return nothing'
    Assert (@(Search-VmrIssueHistory -Path $w.history -Text 'socket resource').Count -eq 1) 'both words are in the name'
    Assert (@(Search-VmrIssueHistory -Path $w.history -Text 'socket arcade').Count -eq 0) 'every word must match'
    Assert (@(Search-VmrIssueHistory -Path $w.history -Owner '@dotnet/runtime').Count -eq 1) '@dotnet/runtime must resolve to its real owner before matching'
    Assert (@(Search-VmrIssueHistory -Path $w.history -Status routed).Count -eq 1) 'status filter'
    Assert-Throws { Search-VmrIssueHistory -Path $w.history } 'at least one of'
}

Check 'Duplicate finder flags renamed findings, respects branch versions, and skips aliased pairs' {
    $dir = Join-Path $sandbox ([guid]::NewGuid().ToString('N')); [void][IO.Directory]::CreateDirectory($dir)
    $h = Join-Path $dir 'issue-history.json'; Initialize-VmrIssueStore -Path $h | Out-Null
    Add-VmrIssueOccurrence -Path $h -Name 'macOS MSBuild socket-buffer exhaustion' -ReportDate '2026-09-17' -BuildIds @('100', '101') | Out-Null
    Add-VmrIssueOccurrence -Path $h -Name 'macOS MSBuild socket exhaustion' -ReportDate '2026-09-18' -BuildIds @('101', '102') | Out-Null
    Add-VmrIssueOccurrence -Path $h -Name 'macOS MSBuild socket exhaustion' -ReportDate '2026-09-19' -BuildIds @('103') | Out-Null
    Add-VmrIssueOccurrence -Path $h -Name 'SDK release/10.0.1xx flow blocked' -ReportDate '2026-09-18' -BuildIds @('200') | Out-Null
    Add-VmrIssueOccurrence -Path $h -Name 'SDK release/11.0.1xx flow blocked' -ReportDate '2026-09-18' -BuildIds @('201') | Out-Null
    Add-VmrIssueOccurrence -Path $h -Name 'Unrelated disk space exhaustion on Windows' -ReportDate '2026-09-18' -BuildIds @('300') | Out-Null
    $pairs = @(Find-VmrIssueDuplicates -Path $h)
    Assert ($pairs.Count -eq 1) "expected exactly the macOS pair, got $($pairs.Count): $(($pairs | ForEach-Object { $_.keep + ' <- ' + $_.fold }) -join ' | ')"
    Assert ($pairs[0].keep -eq 'macOS MSBuild socket exhaustion' -and $pairs[0].fold -eq 'macOS MSBuild socket-buffer exhaustion') 'the thicker history must be kept'
    Assert ($pairs[0].sharedBuilds -eq 1) 'shared build 101 must be counted'
    Assert ($pairs[0].suggestedCommand -match "^Register-VmrIssueAlias -Path '.*' -Alias 'macOS MSBuild socket-buffer exhaustion' -CanonicalName 'macOS MSBuild socket exhaustion'$") "command wrong: $($pairs[0].suggestedCommand)"
    Assert ($pairs[0].suggestedCli -ceq "./history/Invoke-History.ps1 alias -Alias 'macOS MSBuild socket-buffer exhaustion' -Canonical 'macOS MSBuild socket exhaustion'") "cli wrong: $($pairs[0].suggestedCli)"
    Register-VmrIssueAlias -Path $h -Alias 'macOS MSBuild socket-buffer exhaustion' -CanonicalName 'macOS MSBuild socket exhaustion' | Out-Null
    Assert (@(Find-VmrIssueDuplicates -Path $h).Count -eq 0) 'once aliased, the pair must not be suggested again'
}

Check 'Duplicate finder vetoes different versions, opposite polarity and negation, but still folds plain respellings' {
    $dir = Join-Path $sandbox ([guid]::NewGuid().ToString('N')); [void][IO.Directory]::CreateDirectory($dir)
    $h = Join-Path $dir 'issue-history.json'; Initialize-VmrIssueStore -Path $h | Out-Null
    $names = @(
        'OmniSharp v2 scenario initialization HTTP 404', 'OmniSharp v1.40.0 scenario initialization HTTP 404',
        'Node dependency flow remains red', 'Green Node dependency flow awaiting review',
        'Open re-bootstrap PRs from dotnet-bot', 'No open re-bootstrap PRs from dotnet-bot',
        'Crossgen2 startup Text file busy', 'Crossgen2 startup failed with Text file busy')
    $day = 10
    foreach ($name in $names) { Add-VmrIssueOccurrence -Path $h -Name $name -ReportDate "2026-09-$day" -BuildIds @("$day") | Out-Null; $day++ }
    $pairs = @(Find-VmrIssueDuplicates -Path $h -Threshold 0.5)
    $text = ($pairs | ForEach-Object { $_.keep + ' <- ' + $_.fold }) -join ' | '
    Assert ($pairs.Count -eq 1) "only the Crossgen2 respelling may be suggested, got $($pairs.Count): $text"
    Assert ($pairs[0].keep -match 'Crossgen2' -and $pairs[0].fold -match 'Crossgen2') "wrong pair suggested: $text"
}

Check 'Owner repair rewrites 404 handles, keeps provenance, leaves unknown handles alone, and is a no-op when clean' {
    $dir = Join-Path $sandbox ([guid]::NewGuid().ToString('N')); [void][IO.Directory]::CreateDirectory($dir)
    $h = Join-Path $dir 'issue-history.json'; Initialize-VmrIssueStore -Path $h | Out-Null
    Add-VmrIssueOccurrence -Path $h -Name 'Crossgen2 hang' -ReportDate '2026-09-18' -BuildIds @('1') | Out-Null
    Add-VmrIssueOccurrence -Path $h -Name 'Installer drift' -ReportDate '2026-09-18' -BuildIds @('2') | Out-Null
    Add-VmrIssueOccurrence -Path $h -Name 'Mystery owner' -ReportDate '2026-09-18' -BuildIds @('3') | Out-Null
    # Plant what an older export carries: handles that were valid text then but are not teams.
    $raw = Get-Content -LiteralPath $h -Raw | ConvertFrom-Json -AsHashtable
    foreach ($issue in $raw.issues) {
        if ($issue.slug -eq 'crossgen2-hang') { $issue.owner_team = '@dotnet/runtime'; $issue.owner_source = 'operator' }
        if ($issue.slug -eq 'installer-drift') { $issue.owner_team = '@dotnet/source-build-assets'; $issue.owner_source = 'daily-run' }
        if ($issue.slug -eq 'mystery-owner') { $issue.owner_team = '@dotnet/not-a-real-team'; $issue.owner_source = 'daily-run' }
    }
    [IO.File]::WriteAllText($h, ($raw | ConvertTo-Json -Depth 20), [Text.UTF8Encoding]::new($false))
    $preview = @(Repair-VmrIssueOwners -Path $h -Preview)
    Assert (@($preview | Where-Object result -eq 'fixed').Count -eq 2) "preview must find the two fixable handles, got $(($preview | ForEach-Object { $_.slug + ':' + $_.result }) -join ',')"
    Assert ((Search-VmrIssueHistory -Path $h -Text 'crossgen2 hang' | Select-Object -First 1).owner_team -eq '@dotnet/runtime') 'preview must not change anything'
    $fixed = @(Repair-VmrIssueOwners -Path $h)
    $byslug = @{}; foreach ($row in $fixed) { $byslug[$row.slug] = $row }
    Assert ($byslug['crossgen2-hang'].to -eq '@dotnet/dnr-codeflow') "runtime must map to the registry's runtime owner, got $($byslug['crossgen2-hang'].to)"
    Assert ($byslug['installer-drift'].to -eq '@dotnet/source-build') 'a repo-name handle must map to its Guide owner, source-build'
    Assert ($byslug['mystery-owner'].result -eq 'unknown') 'an unregistered handle is reported, never guessed'
    $after = Read-VmrIssueStore -Path $h
    $issues = @{}; foreach ($i in $after.issues) { $issues[$i.slug] = $i }
    Assert ($issues['crossgen2-hang'].owner_source -eq 'operator' -and $issues['installer-drift'].owner_source -eq 'daily-run') 'provenance must survive the repair'
    Assert ($issues['mystery-owner'].owner_team -eq '@dotnet/not-a-real-team') 'an unknown handle must be left as it was'
    $before = (Get-Item -LiteralPath $h).LastWriteTimeUtc
    Start-Sleep -Milliseconds 30
    $again = @(Repair-VmrIssueOwners -Path $h)
    Assert (@($again | Where-Object result -eq 'fixed').Count -eq 0) 'second run has nothing to fix'
    Assert ((Get-Item -LiteralPath $h).LastWriteTimeUtc -eq $before) 'a no-op repair must not rewrite the store'
}
Check 'Report replay ignores sentence-ending periods after team handles and remains idempotent' {
    $w = New-World
    $report = Join-Path $w.dir 'overall-summary-2026-10-13.md'
    $content = @'
| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |
| --- | --- | --- | --- | --- | --- |
| BH #- - Public rolling main | [1](https://dev.azure.com/dnceng-public/public/_build/results?buildId=1) | ? | None found | Where: None - monitor only.<br>Action: The owner is @dotnet/prodconsvcs. | None found |
| BH #1 - Synthetic build failure | [2](https://dev.azure.com/dnceng/internal/_build/results?buildId=2) | Y | None found | Where: Teams.<br>Copy/paste: "@dotnet/msbuild, Please check this issue." | None found |
'@
    [IO.File]::WriteAllText($report, $content, [Text.UTF8Encoding]::new($false))
    $warnings = @()
    Update-VmrIssueHistory -Date '2026-10-13' -Database $w.history -ReportDirectory $w.dir -WarningVariable warnings | Out-Null
    Assert ($warnings.Count -eq 0) "valid team handles must not produce warnings: $warnings"
    $public = Get-VmrIssueHistory -Path $w.history -Name 'Public rolling main'
    Assert ($public.issue.owner_team -eq '@dotnet/prodconsvcs') 'the sentence period must not become part of the owner handle'
    $synthetic = Get-VmrIssueHistory -Path $w.history -Name 'Synthetic build failure'
    Assert ($synthetic.issue.owner_team -eq '@dotnet/msbuild') 'a comma-delimited handle must continue to work'
    $before = Get-Content -LiteralPath $w.history -Raw
    Update-VmrIssueHistory -Date '2026-10-13' -Database $w.history -ReportDirectory $w.dir | Out-Null
    Assert ((Get-Content -LiteralPath $w.history -Raw) -ceq $before) 'replaying the same report must not change history'
}

Check 'The Invoke-Record.ps1 entry script succeeds end to end with -Create -AddContext, and -WhatIf writes nothing and says so' {
    $entry = Join-Path $PSScriptRoot '..\history\Invoke-Record.ps1'
    $state = Join-Path $sandbox 'record-cli'
    $saved = $env:VMR_QB_HOME
    $env:VMR_QB_HOME = $state
    try {
        $common = @('-Finding', 'Synthetic entry finding', '-Status', 'owned', '-Note', 'The owner confirmed a synthetic failure.', '-Owner', '@dotnet/dnceng', '-Create', '-AddContext', '-AppliesTo', 'Only synthetic offline input.')
        $preview = & pwsh -NoProfile -NonInteractive -File $entry @common -WhatIf 2>&1 | Out-String
        Assert ($LASTEXITCODE -eq 0) "-WhatIf must succeed: $preview"
        Assert ($preview -match 'Would record' -and $preview -notmatch "(?m)^Recorded ") "-WhatIf must not claim it recorded anything: $preview"
        Assert (-not (Test-Path -LiteralPath (Join-Path $state 'issue-history.json'))) '-WhatIf must not create the history'
        Assert (-not (Test-Path -LiteralPath (Join-Path $state 'knowledge\triage-context.local.json'))) '-WhatIf must not write the context'

        $real = & pwsh -NoProfile -NonInteractive -File $entry @common 2>&1 | Out-String
        Assert ($LASTEXITCODE -eq 0) "the real update must exit 0 (it used to fail on .Contains after writing): $real"
        Assert ($real -match "(?m)^Recorded 'owned'" -and $real -match 'context:') "the summary must report the recorded context: $real"
        $env:VMR_QB_HOME = $state
        Assert ((Get-VmrIssueHistory -Path (Join-Path $state 'issue-history.json') -Name 'Synthetic entry finding').issue.status -eq 'owned') 'history must hold the update'
        $context = Get-Content -LiteralPath (Join-Path $state 'knowledge\triage-context.local.json') -Raw | ConvertFrom-Json
        Assert (@($context.entries).Count -eq 1) 'the context must hold exactly one entry'
    }
    finally { $env:VMR_QB_HOME = $saved }
}

function Get-StateFingerprint([hashtable] $World) {
    # Every file under the world's folder, by name and content hash: "nothing was written" means this is unchanged.
    # Lock files are empty coordination files, not data.
    return (@(Get-ChildItem -LiteralPath $World.dir -Recurse -File -Force | Where-Object Extension -ne '.lock' | Sort-Object FullName | ForEach-Object { "$($_.FullName.Substring($World.dir.Length))=$((Get-FileHash -LiteralPath $_.FullName).Hash)" }) -join ';')
}
$goodNote = @{ Finding = 'Arcade SDK package unavailable'; Status = 'owned'; Note = 'Alex owns this one now.'; On = '2026-10-07' }

Check 'A malformed links or context file stops the update before history changes, with the file named' {
    foreach ($case in @(
            @{ file = 'links';   args = @{ Builds = @(9100201); Match = 'Arcade' };      message = 'reported links file .*not valid JSON' },
            @{ file = 'context'; args = @{ AddContext = $true; AppliesTo = 'synthetic' }; message = 'triage context file .*not valid JSON' })) {
        $w = New-World
        $target = if ($case.file -eq 'links') { $w.links } else { Join-Path $w.local 'triage-context.local.json' }
        [void][IO.Directory]::CreateDirectory((Split-Path -Parent $target))
        [IO.File]::WriteAllText($target, '{ "reports": [ not json')
        $before = Get-StateFingerprint $w
        $arguments = $goodNote.Clone(); foreach ($key in $case.args.Keys) { $arguments[$key] = $case.args[$key] }
        Assert-Throws { Update $w $arguments } $case.message
        Assert ((Get-StateFingerprint $w) -ceq $before) "the $($case.file) failure changed state: history must not have been written first"
        Assert-Throws { Update $w ($arguments + @{ WhatIf = $true }) } $case.message
        $shape = @('[]', '{"reports": 5, "entries": 5}')
        foreach ($body in $shape) {
            [IO.File]::WriteAllText($target, $body)
            $before = Get-StateFingerprint $w
            Assert-Throws { Update $w $arguments } 'must hold a JSON object|not a list'
            Assert ((Get-StateFingerprint $w) -ceq $before) "a $body $($case.file) file changed state"
        }
    }
}

Check 'An unwritable location stops the update before history changes and leaves no staged file' {
    $w = New-World
    # The folder that should hold the links file is a plain file, so nothing can be created there.
    [IO.File]::WriteAllText((Join-Path $w.dir 'blocked'), 'x')
    $world = @{ dir = $w.dir; history = $w.history; links = (Join-Path $w.dir 'blocked\reported-links.json'); local = $w.local }
    $before = Get-StateFingerprint $w
    Assert-Throws { Update $world ($goodNote + @{ Builds = @(9100201); Match = 'Arcade' }) } 'Cannot write the reported links file'
    Assert ((Get-StateFingerprint $w) -ceq $before) 'history must be untouched when the links location is unwritable'
    $knowledgeBlocked = @{ dir = $w.dir; history = $w.history; links = $w.links; local = (Join-Path $w.dir 'blocked') }
    Assert-Throws { Update $knowledgeBlocked ($goodNote + @{ AddContext = $true; AppliesTo = 'synthetic' }) } 'Cannot create the local knowledge folder'
    Assert ((Get-StateFingerprint $w) -ceq $before) 'history must be untouched when the context folder is unwritable'
}

Check 'A failure after history was written is reported as a partial update that names what was and was not done' {
    $w = New-World
    # Another process has the existing context file open without allowing a replace: it reads fine, so validation and
    # staging succeed, but the final move cannot replace it.
    $context = Join-Path $w.local 'triage-context.local.json'
    [void][IO.Directory]::CreateDirectory($w.local)
    [IO.File]::WriteAllText($context, '{"schemaVersion":1,"entries":[]}')
    $holder = [IO.File]::Open($context, [IO.FileMode]::Open, [IO.FileAccess]::Read, [IO.FileShare]::Read)
    $message = $null
    try { Update $w ($goodNote + @{ Builds = @(9100201); Match = 'Arcade'; AddContext = $true; AppliesTo = 'synthetic' }) | Out-Null } catch { $message = $_.Exception.Message }
    finally { $holder.Dispose() }
    Assert ($message -match 'Only part of this update was recorded') "expected a partial-update error: $message"
    Assert ($message -match 'Done: the issue history, the reported links' -and $message -match 'Not done: the triage context') "the error must say what was and was not done: $message"
    Assert ($message -match 'Do not run the command again') 'the error must say how to recover'
    $saved = [regex]::Match($message, 'saved at (?<p>\S+?);').Groups['p'].Value
    Assert ($saved -and (Test-Path -LiteralPath $saved -PathType Leaf)) "the unmoved content must be kept at the path the error names: $saved"
    Assert ((Get-VmrIssueHistory -Path $w.history -Name 'Arcade SDK package unavailable').issue.status -eq 'owned') 'history was recorded and stays recorded'
    Assert (@((Get-Content -LiteralPath $w.links -Raw | ConvertFrom-Json).reports).Count -eq 1) 'the links entry was recorded'
}

Check 'Updates are serialised: a held lock stops a second update cleanly, and parallel recordings lose nothing' {
    $w = New-World
    $savedTimeout = $script:VmrIssueStoreLockTimeoutSeconds
    $script:VmrIssueStoreLockTimeoutSeconds = 1
    [void][IO.Directory]::CreateDirectory($w.local)
    $lock = Enter-VmrIssueStoreLock -FullPath (Join-Path $w.local 'operator-update')
    try {
        $before = Get-StateFingerprint $w
        Assert-Throws { Update $w ($goodNote + @{ Builds = @(9100201); Match = 'Arcade' }) } 'Timed out'
        Assert ((Get-StateFingerprint $w) -ceq ($before)) 'a refused update must change nothing'
    }
    finally { $lock.Dispose(); $script:VmrIssueStoreLockTimeoutSeconds = $savedTimeout }

    $entry = Join-Path $PSScriptRoot '..\history\Invoke-Record.ps1'
    $state = Join-Path $sandbox 'parallel-state'
    # Start-Process joins its argument list without quoting, so every value with a space is quoted here.
    $jobs = foreach ($n in 1..4) {
        Start-Process -FilePath (Get-Process -Id $PID).Path -PassThru -WindowStyle Hidden -ArgumentList @('-NoProfile', '-NonInteractive', '-File', "`"$entry`"",
            '-Finding', "`"Parallel finding $n`"", '-Status', 'owned', '-Note', "`"Owner statement number $n for a parallel recording.`"", '-Create',
            '-Builds', "$(5000 + $n)", '-Match', "`"Parallel finding $n`"", '-AddContext', '-AppliesTo', 'synthetic', '-AllowUnknownOwner') -Environment @{ VMR_QB_HOME = $state }
    }
    $jobs | Wait-Process -Timeout 120
    Assert (@($jobs | Where-Object ExitCode -ne 0).Count -eq 0) "a parallel recording failed: exit codes $(($jobs | ForEach-Object ExitCode) -join ',')"
    $links = Get-Content -LiteralPath (Join-Path $state 'reported-links.json') -Raw | ConvertFrom-Json
    $context = Get-Content -LiteralPath (Join-Path $state 'knowledge\triage-context.local.json') -Raw | ConvertFrom-Json
    Assert (@($links.reports).Count -eq 4) "expected 4 link entries, found $(@($links.reports).Count): a recording was lost"
    Assert (@($context.entries).Count -eq 4) "expected 4 context entries, found $(@($context.entries).Count): a recording was lost"
    Assert (@(Get-ChildItem -LiteralPath $state -Recurse -File -Filter '*.tmp').Count -eq 0) 'no staged file may be left behind'
}

Check 'The local routing copy is created once; a real failure is an error, and only a concurrent first use is tolerated' {
    $shipped = Join-Path (Get-VmrQBPaths).KnowledgeRoot 'routing.json'
    $dir = Join-Path $sandbox 'routing-init'
    [void][IO.Directory]::CreateDirectory($dir)
    # A parent that is a file: nothing can be created, and that must be loud rather than "false".
    [IO.File]::WriteAllText((Join-Path $dir 'parent-is-a-file'), 'x')
    Assert-Throws { Initialize-VmrQBLocalRouting -LocalPath (Join-Path $dir 'parent-is-a-file\routing.local.json') -ShippedPath $shipped } 'Could not create your local routing copy'
    # An existing copy is never touched.
    $existing = Join-Path $dir 'existing.json'
    [IO.File]::WriteAllText($existing, '{"schemaVersion":1,"mine":true}')
    Assert ((Initialize-VmrQBLocalRouting -LocalPath $existing -ShippedPath $shipped) -eq $false) 'an existing file means false'
    Assert (([IO.File]::ReadAllText($existing)) -ceq '{"schemaVersion":1,"mine":true}') 'an existing copy must never be rewritten'
    # Six processes starting together: exactly one creates the copy and the rest see it, with no error.
    $target = Join-Path $dir 'race\routing.local.json'
    $core = Join-Path $PSScriptRoot '..\common\core.ps1'
    $barrier = [Threading.Barrier]::new(6)
    $results = @(1..6 | ForEach-Object -ThrottleLimit 6 -Parallel {
            . $using:core
            ($using:barrier).SignalAndWait()
            try { [string](Initialize-VmrQBLocalRouting -LocalPath $using:target -ShippedPath $using:shipped) } catch { "ERROR: $($_.Exception.Message)" }
        })
    Assert (@($results | Where-Object { $_ -like 'ERROR*' }).Count -eq 0) "a concurrent first use must not be an error: $($results -join ' | ')"
    Assert (@($results | Where-Object { $_ -eq 'True' }).Count -eq 1) "exactly one process creates the copy: $($results -join ',')"
    Assert (Test-Path -LiteralPath $target -PathType Leaf) 'the copy must exist'
    Assert (@(Get-ChildItem -LiteralPath (Split-Path -Parent $target) -Filter '*.tmp').Count -eq 0) 'no temp file may remain'
}
Remove-Item $sandbox -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue
if ($failures) { Write-Host "$failures history-tool test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host '19 offline history-tool tests. Temporary stores only; no network, email, or external changes.' -ForegroundColor Green
