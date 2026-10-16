[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# The round never stops for something stale, missing or damaged in the QB's own files: each scenario runs the real daily script in a
# child process (existing reports, no email, a rehearsal root, so no collection, model, network or Outlook) with one local input
# broken, and expects the report anyway: exit 0, the problem and its fix in the notes at the top of the overall report and in the
# run log, and the broken file left exactly as it was. An unexpected crash is the only thing that exits 1, with a run log that says
# where, and a run for a day another run holds exits 3 without touching anything.
$scripts = Split-Path -Parent $PSScriptRoot
. (Join-Path $scripts 'common\core.ps1')
. (Join-Path $scripts 'daily\notes.ps1')
. (Join-Path $scripts 'daily\workspace.ps1')
$daily = Join-Path $scripts 'daily\Invoke-Daily.ps1'
$pwsh = (Get-Process -Id $PID).Path
$date = '2026-10-09'

$failures = 0
function Check([string] $Name, [scriptblock] $Body) {
    try { & $Body; Write-Host "PASS: $Name" -ForegroundColor Green }
    catch { $script:failures++; Write-Host "FAIL: $Name -> $($_.Exception.Message)" -ForegroundColor Red }
}
function Assert($Condition, [string] $Message) { if (-not $Condition) { throw $Message } }

$sandbox = Join-Path ([IO.Path]::GetTempPath()) ('vmrqb-safety-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($sandbox)
$originalHome = $env:VMR_QB_HOME

function New-Overall {
    $link = '[501](https://dev.azure.com/dnceng/internal/_build/results?buildId=501)'
    $where = 'Teams - Unified Build Technical and General Discussions'
    $header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
    return (@("# VMR QB Daily Overall Report - $date", '', '## Top actions for the QB today', '',
            "1. $link - **BH #1:** Fixture finding failed. Where: $where.", '', '## Summary', '', $header, '|---|---|---|---|---|---|',
            "| BH #1 - Fixture finding | $link | Y | None found | Where: $where.<br>Copy/paste: `"@dotnet/dnceng, Please check the build failure. Fixture finding failed: $link`" | None found |") -join "`n") + "`n"
}
function New-Case([string] $Name, [switch] $NoReports) {
    $root = Join-Path $sandbox $Name
    $case = [pscustomobject]@{ State = (Join-Path $root 'state'); Rehearsal = (Join-Path $root 'rehearsal') }
    foreach ($dir in @($case.State, (Join-Path $case.Rehearsal 'reports'))) { [void][IO.Directory]::CreateDirectory($dir) }
    if (-not $NoReports) {
        $reports = Join-Path $case.Rehearsal 'reports'
        [IO.File]::WriteAllText((Join-Path $reports "build-health-$date.md"), "# Build Health Monitoring - $date`n")
        [IO.File]::WriteAllText((Join-Path $reports "code-flow-$date.md"), "# Code Flow Monitoring - $date`n")
        [IO.File]::WriteAllText((Join-Path $reports "overall-summary-$date.md"), (New-Overall))
    }
    return $case
}
function Invoke-Round([object] $Case, [string[]] $Mode = @('-UseExistingReports')) {
    $env:VMR_QB_HOME = $Case.State
    $output = & $pwsh -NoProfile -NonInteractive -File $daily -Date $date @Mode -SkipEmail -OutputRoot $Case.Rehearsal 2>&1 | Out-String
    $exit = $LASTEXITCODE
    $overallPath = Join-Path $Case.Rehearsal "reports\overall-summary-$date.md"
    $logs = Join-Path $Case.Rehearsal 'logs'
    $runLog = @(Get-ChildItem -LiteralPath $logs -Filter "run-$date-*.json" -ErrorAction SilentlyContinue | Sort-Object LastWriteTimeUtc) | Select-Object -Last 1
    return [pscustomobject]@{
        Exit = $exit; Output = $output
        Overall = $(if (Test-Path -LiteralPath $overallPath) { [IO.File]::ReadAllText($overallPath) } else { '' })
        Log = $(if ($runLog) { Get-Content -LiteralPath $runLog.FullName -Raw | ConvertFrom-Json } else { $null })
    }
}
function Get-Notes([object] $Result, [string] $Source) { return @(@($Result.Log.notes) | Where-Object { $_.source -eq $Source }) }
function Get-Hash([string] $Path) { return (Get-FileHash -LiteralPath $Path).Hash }
function Assert-Delivered([object] $Result, [string] $What) {
    Assert ($Result.Exit -eq 0) "$What must exit 0, got $($Result.Exit): $($Result.Output)"
    Assert ($null -ne $Result.Log -and $Result.Log.delivered -eq $true) "$What must be delivered: $($Result.Output)"
    Assert ($Result.Overall -match 'VMR-QB-RUN-NOTES' -and $Result.Overall -match 'Run notes \(\d+\)') "${What}: the overall report must open with the run notes"
    Assert ($Result.Output -match 'Issue history updated') "${What}: the report was still recorded in the history: $($Result.Output)"
}

try {
    $baseline = New-Case 'baseline'
    $clean = Invoke-Round $baseline
    Check 'With nothing broken the round delivers, and nothing it later breaks is mentioned in the notes' {
        Assert ($clean.Exit -eq 0 -and $clean.Log.delivered -eq $true) "the baseline must deliver: exit $($clean.Exit) $($clean.Output)"
        foreach ($source in @('config', 'history', 'owners', 'run')) { Assert (@(Get-Notes $clean $source).Count -eq 0) "no $source note when nothing is wrong" }
        Assert (@(@($clean.Log.notes) | Where-Object { $_.message -match 'triage context|issue history|routing|time zone' }).Count -eq 0) 'no note about the local inputs'
        Assert ((Remove-QBRunNotesBlock -Content $clean.Overall).TrimEnd() -ceq (New-Overall).TrimEnd()) 'taking the notes block out leaves the report exactly as it was written'
    }

    Check 'A config that is not valid JSON costs only the email: the report is delivered, the problem and its fix are on top, and the file is left alone' {
        $case = New-Case 'config-json'
        $config = Join-Path $case.State 'qb-config.local.json'; Set-Content -LiteralPath $config -Value '{ nope'
        $before = Get-Hash $config
        $result = Invoke-Round $case
        Assert-Delivered $result 'a bad config'
        $notes = Get-Notes $result 'config'
        Assert ($notes.Count -eq 1 -and $notes[0].severity -eq 'first' -and $notes[0].message -match 'not valid JSON' -and $notes[0].message -match 'To get email') 'one note that says what is wrong and how to get email'
        Assert ($result.Overall -match 'not valid JSON') 'it is at the top of the overall report'
        $body = Join-Path $case.Rehearsal "logs\email-body-$date.html"
        Assert ((Test-Path -LiteralPath $body) -and ([IO.File]::ReadAllText($body) -match 'Run notes \(\d+\)') -and ([IO.File]::ReadAllText($body) -match 'not valid JSON')) 'and in the email body that would have been sent'
        Assert ((Get-Hash $config) -eq $before) 'the config file is untouched'
    }

    Check 'A recipient that is not an address and a time zone this machine does not know are notes, not stops' {
        $case = New-Case 'config-fields'
        Set-Content -LiteralPath (Join-Path $case.State 'qb-config.local.json') -Value '{ "recipient": "nope", "timeZone": "Mars/Phobos" }'
        $result = Invoke-Round $case
        Assert-Delivered $result 'a bad recipient and time zone'
        $notes = @(Get-Notes $result 'config')
        Assert (@($notes | Where-Object { $_.message -match "'recipient'.*not an email address" }).Count -eq 1) 'the recipient problem'
        Assert (@($notes | Where-Object { $_.message -match "time zone 'Mars/Phobos' is not known" -and $_.message -match 'qb-config\.local\.json' }).Count -eq 1) 'the time zone problem, naming the file to fix'
    }

    Check 'A corrupt issue history is noted and left as it is; the report is delivered and the history of the rehearsal is the only thing that was tried' {
        $case = New-Case 'history'
        $history = Join-Path $case.State 'issue-history.json'; Set-Content -LiteralPath $history -Value '{ nope'
        $before = Get-Hash $history
        $result = Invoke-Round $case
        Assert ($result.Exit -eq 0 -and $result.Log.delivered -eq $true) "must deliver: $($result.Output)"
        Assert ($result.Overall -match 'VMR-QB-RUN-NOTES') 'the notes are on top'
        Assert (@(@($result.Log.notes) | Where-Object { $_.message -match 'issue history' }).Count -ge 1) "a note about the history: $(($result.Log.notes | ForEach-Object message) -join ' | ')"
        Assert ($result.Output -notmatch 'RUN FAILED') 'no crash'
        Assert ((Get-Hash $history) -eq $before) 'the damaged file is left for the QB'
    }

    Check 'A corrupt local triage context is skipped with a note and left as it is' {
        $case = New-Case 'triage'
        $local = Join-Path $case.State 'knowledge'; [void][IO.Directory]::CreateDirectory($local)
        $context = Join-Path $local 'triage-context.local.json'; Set-Content -LiteralPath $context -Value '{ nope'
        $before = Get-Hash $context
        $result = Invoke-Round $case
        Assert-Delivered $result 'a corrupt triage context'
        Assert (@(@($result.Log.notes) | Where-Object { $_.message -match 'triage context.*could not be used' -and $_.fix -match 'Fix or move' }).Count -eq 1) "one note, with the fix: $((@($result.Log.notes) | ForEach-Object message) -join ' | ')"
        Assert ((Get-Hash $context) -eq $before) 'the file is untouched'
    }

    Check 'A local routing category that names a team the Guide no longer lists is listed in the notes and never stops the round' {
        $case = New-Case 'routing'
        $first = Invoke-Round $case
        Assert ($first.Exit -eq 0) "the first round makes the local routing copy: $($first.Output)"
        $local = Join-Path $case.State 'knowledge\routing.local.json'
        Assert (Test-Path -LiteralPath $local) 'the first round made the routing copy once'
        $document = Get-Content -LiteralPath $local -Raw | ConvertFrom-Json -Depth 50
        $document.categories = @($document.categories) + @([pscustomobject]@{ id = 'gone-category'; name = 'A category that left'; owners = @('@dotnet/gone-team-zzz') })
        [IO.File]::WriteAllText($local, ((ConvertTo-Json -InputObject $document -Depth 50) + "`n"), [Text.UTF8Encoding]::new($false))
        $before = Get-Hash $local
        $case2 = [pscustomobject]@{ State = $case.State; Rehearsal = (Join-Path $sandbox 'routing\rehearsal-2') }
        $reports = Join-Path $case2.Rehearsal 'reports'; [void][IO.Directory]::CreateDirectory($reports)
        foreach ($name in @('build-health', 'code-flow', 'overall-summary')) { Copy-Item -LiteralPath (Join-Path $case.Rehearsal "reports\$name-$date.md") -Destination $reports }
        $overallFile = Join-Path $reports "overall-summary-$date.md"
        [IO.File]::WriteAllText($overallFile, (Remove-QBRunNotesBlock -Content ([IO.File]::ReadAllText($overallFile))))
        $result = Invoke-Round $case2
        Assert-Delivered $result 'a stale routing category'
        $owners = @(Get-Notes $result 'owners')
        Assert ($owners.Count -ge 1 -and (($owners | ForEach-Object message) -join ' ') -match 'gone-team-zzz') "the owner note names the team: $(($owners | ForEach-Object message) -join ' | ')"
        Assert ((Get-Hash $local) -eq $before) 'the routing file is never overwritten'
    }

    Check 'A saved report that is missing or has another day''s title never stops the round: the others are delivered, the digest stands in for a missing overall report, and the notes say why' {
        $case = New-Case 'missing-report'
        Remove-Item -LiteralPath (Join-Path $case.Rehearsal "reports\code-flow-$date.md")
        $result = Invoke-Round $case
        Assert ($result.Exit -eq 0 -and $result.Log.delivered -eq $true) "a missing code-flow report must not stop the round: exit $($result.Exit) $($result.Output)"
        Assert ($result.Output -notmatch 'RUN FAILED') "no crash: $($result.Output)"
        $codeFlow = @($result.Log.reports | Where-Object name -eq 'code-flow')
        Assert ($codeFlow.Count -eq 1 -and $codeFlow[0].status -eq 'failed' -and $codeFlow[0].error -match 'code-flow report is missing') 'the log names the missing report'
        Assert (@($result.Log.reports | Where-Object { $_.status -eq 'succeeded' }).Count -eq 2) 'the other two are delivered'
        Assert (@(@($result.Log.notes) | Where-Object { $_.source -eq 'report' -and $_.severity -eq 'first' -and $_.message -match 'code-flow report was not generated' }).Count -eq 1) 'a highlighted note says so'
        Assert ($result.Overall -match 'code-flow report was not generated') 'and it is at the top of the overall report'

        $noOverall = New-Case 'missing-overall'
        Remove-Item -LiteralPath (Join-Path $noOverall.Rehearsal "reports\overall-summary-$date.md")
        $result = Invoke-Round $noOverall
        Assert ($result.Exit -eq 0 -and $result.Log.delivered -eq $true -and $result.Output -notmatch 'RUN FAILED') "a missing overall report must still deliver (the digest): exit $($result.Exit) $($result.Output)"
        Assert (@($result.Log.reports | Where-Object { $_.name -eq 'overall-summary' -and $_.status -eq 'failed' }).Count -eq 1 -and $result.Log.digest) 'the overall report is a failed result and the digest stands in'

        $wrongTitle = New-Case 'wrong-title'
        [IO.File]::WriteAllText((Join-Path $wrongTitle.Rehearsal "reports\code-flow-$date.md"), "# Code Flow Monitoring - 2000-01-01`n")
        $result = Invoke-Round $wrongTitle
        Assert-Delivered $result 'a report with another day''s title'
        Assert (@(@($result.Log.reports | Where-Object status -eq 'succeeded')).Count -eq 3) 'all three are delivered'
        Assert (@(@($result.Log.notes) | Where-Object { $_.source -eq 'report-check' -and $_.severity -eq 'first' -and $_.message -match 'no title line' }).Count -eq 1) 'a highlighted note says the title is wrong'
    }

    Check 'An overall report whose actions break the format contract is delivered with every problem highlighted at the top, each as its own note' {
        $case = New-Case 'format-problems'
        $overallFile = Join-Path $case.Rehearsal "reports\overall-summary-$date.md"
        $absent = '[#9](https://github.com/dotnet/dotnet/pull/9)'
        $bad = (New-Overall).Replace('Fixture finding failed: [501]', "Fixture finding failed: $absent, [501]") + "| BH #2 - Second finding | [502](https://dev.azure.com/dnceng/internal/_build/results?buildId=502) | Y | None found | Ping the team about the problem. | None found |`n"
        [IO.File]::WriteAllText($overallFile, $bad)
        $result = Invoke-Round $case
        Assert ($result.Exit -eq 0 -and $result.Log.delivered -eq $true) "must deliver: $($result.Output)"
        $checks = @(Get-Notes $result 'report-check')
        Assert (@($checks | Where-Object { $_.message -match 'contains a link absent from its evidence/fix columns' -and $_.severity -eq 'first' }).Count -eq 1) "the absent link is a highlighted note: $(($checks | ForEach-Object message) -join ' | ')"
        Assert (@($checks | Where-Object { $_.message -match "Second finding.*needs Where:" }).Count -eq 1) 'and so is the second row, not hidden by the first'
        Assert ($result.Overall -match 'link absent from its evidence/fix columns' -and $result.Overall -match 'Second finding') 'both are at the top of the saved report'
        Assert ($result.Overall.Contains($absent)) 'the report is saved as it was written'
    }

    Check 'An unexpected crash is the only failure: exit 1, a failed run log that names the stage, and the message on the console' {
        $case = New-Case 'crash' -NoReports
        $result = Invoke-Round $case -Mode @('-UseCollectedEvidence')
        Assert ($result.Exit -eq 1) "exit 1, got $($result.Exit)"
        Assert ($result.Output -match 'RUN FAILED at collection: -UseCollectedEvidence requires an existing dated run-context manifest') "the console says where: $($result.Output)"
        Assert ($null -ne $result.Log -and $result.Log.delivered -eq $false -and $result.Log.runStatus -eq 'failed' -and $result.Log.stage -eq 'collection' -and $result.Log.error -match 'run-context manifest') 'the run log says the same'
        Assert (@(@($result.Log.notes) | Where-Object { $_.source -eq 'run' -and $_.severity -eq 'first' -and $_.message -match "stopped unexpectedly at 'collection'" }).Count -eq 1) 'and carries a first-severity note'
    }

    Check 'A run for a day another run is holding exits 3 and changes nothing' {
        $case = New-Case 'busy'
        $logs = Join-Path $case.Rehearsal 'logs'; [void][IO.Directory]::CreateDirectory($logs)
        $holder = $null
        $claim = Enter-QBDailyClaim -LogDirectory $logs -Date $date -Holder ([ref]$holder)
        Assert ($null -ne $claim) 'the test holds the claim'
        try {
            $overall = Join-Path $case.Rehearsal "reports\overall-summary-$date.md"
            $before = Get-Hash $overall
            $result = Invoke-Round $case
            Assert ($result.Exit -eq 3) "exit 3, got $($result.Exit): $($result.Output)"
            Assert ($result.Output -match 'Another VMR QB run for 2026-10-09 is already in progress') 'it says who holds the day'
            Assert ((Get-Hash $overall) -eq $before -and $null -eq $result.Log) 'nothing was changed and no run log was written'
        }
        finally { $claim.Dispose() }
    }
}
finally {
    if ($null -eq $originalHome) { Remove-Item Env:\VMR_QB_HOME -ErrorAction SilentlyContinue } else { $env:VMR_QB_HOME = $originalHome }
    Remove-Item -LiteralPath $sandbox -Recurse -Force -ErrorAction SilentlyContinue
}

if ($failures) { Write-Host "$failures run-safety test(s) failed." -ForegroundColor Red; exit 1 }
Write-Host 'All run-safety tests passed.' -ForegroundColor Green
