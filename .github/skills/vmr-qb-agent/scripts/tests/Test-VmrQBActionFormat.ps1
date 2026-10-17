[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$root = Split-Path -Parent $PSScriptRoot
# The report contract and the prompts are libraries; only the runner's own state-bound functions are lifted below.
. (Join-Path $root 'daily\contract.ps1')
. (Join-Path $root 'daily\prompts.ps1')
$tokens = $null
$errors = $null
$ast = [System.Management.Automation.Language.Parser]::ParseFile(
    (Join-Path $root 'daily\Invoke-Daily.ps1'), [ref]$tokens, [ref]$errors)
if ($errors.Count) { throw ($errors.Message -join '; ') }

# Load functions only: do not invoke the monitor, its entry audit, or Outlook.
foreach ($name in @('Get-QBTeamContacts', 'Get-QBTranscriptModel', 'Test-QBPinnedModelRejected', 'Invoke-QBModelRun', 'Complete-QBReportDocument', 'New-Report')) {
    $definition = $ast.Find({
        param($node)
        $node -is [System.Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq $name
    }, $false)
    if ($null -eq $definition) { throw "Missing function: $name" }
    . ([scriptblock]::Create($definition.Extent.Text))
}

function Assert-Throws {
    param([scriptblock] $Action, [string] $Expected)
    try { & $Action | Out-Null }
    catch {
        if ($_.Exception.Message -notmatch $Expected) { throw }
        return
    }
    throw "Expected an error matching: $Expected"
}

$Date = '2026-09-19'
$stamp = 'fixture'
$ownershipBlock = 'Fixture routing only; no external actions.'
$buildUrl = 'https://dev.azure.com/dnceng/internal/_build/results?buildId=11'
$build = "[Build 11]($buildUrl)"
$pr = '[dotnet/dotnet#1](https://github.com/dotnet/dotnet/pull/1)'
$subscription = '[WindowsDesktop subscription](https://maestro.dot.net/subscriptions?search=fixture)'
$description = 'Please check the build failure. EMSDK signing failed on the servicing build.'
$where = 'Teams - Servicing chat'
$topAction = "1. $build - Ask @dotnet/dnr-codeflow to check EMSDK signing. Where: $where."
$cell = "Where: $where.<br>Copy/paste: `"@dotnet/dnr-codeflow, ${description}: $build`""
$header = '| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |'
$report = @"
# VMR QB Daily Overall Report - $Date

## Top actions for the QB today
$topAction

## Summary
$header
|---|---|---|---|---|---|
| EMSDK signing | $build | Y | None found | $cell | None found |
"@

Assert-QBOverallReport -Content $report
Assert-QBOverallReport -Content (ConvertTo-QBActionFormat $report.Replace('Copy/paste: "', 'Copy/paste: `"').Replace('" |', '"` |'))
Assert-QBOverallReport -Content ($report -replace '\r?\n', "`n")
Assert-QBOverallReport -Content ($report -replace '\r?\n', "`r`n")
Assert-QBOverallReport -Content $report.Replace($cell, $cell.Replace("$build`"", "$build.`""))


$thirtyWords = 'Please check the build failure. ' + ((1..25 | ForEach-Object { "word$_" }) -join ' ')
Assert-QBOverallReport -Content $report.Replace($description, $thirtyWords)
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($description, "$thirtyWords extra") } '31.*maximum is 30'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($description, 'Please correct the signing configuration.') } 'brief check request'
foreach ($directive in @(
    'Signing failed; correct the signing configuration.',
    'Signing failed; please rerun the build.',
    'Signing failed; verify rollout of the fix.',
    'Signing failed; update the SDK.',
    'Signing failed; backport the fix.',
    'Signing failed; can you investigate the cause?',
    'The PR is blocked; approve it.',
    'The PR is blocked; merge it.',
    'The fix needs to be applied.',
    'Signing failed. Rerun.'
)) {
    Assert-Throws { Assert-QBOverallReport -Content $report.Replace($description, "Please check the build failure. $directive") } 'not remediation instructions'
}
foreach ($observation in @(
    "Restore failed with 'A task was canceled'.",
    'Package update failed; the initiating cause is unknown.',
    'Compiler exited with an error; low disk was observed but the cause is unconfirmed.',
    'Retry failed with the same error.',
    'Signing failed. Rerun pending.',
    'Change validation rejected reserved version-file edits and removal of two VMR submodules.',
    'Restore authentication failed on a private feed.',
    'Merge validation rejected the PR.'
)) {
    Assert-QBOverallReport -Content $report.Replace($description, "Please check the build failure. $observation")
}
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($topAction, '1. Raise all failures with the team.') } 'start with an evidence link'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($topAction, "$topAction`n   - Also investigate another failure.") } 'start with an evidence link'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($where, 'Teams - some channel') } 'exact Teams/GitHub destination'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($cell, 'Ping the team about the problem.') } 'needs Where:'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($cell, "Where: $where.<br>Copy/paste: @dotnet/dnr-codeflow, ${description}: $build") } 'quoted message'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($cell, "Where: $where.<br>Copy/paste: `"@dotnet/dnr-codeflow, ${description}: <paste link>`"") } 'quoted message'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($cell, $cell.Replace($buildUrl, 'https://dev.azure.com/dnceng/internal/_build/results?buildId=99')) } 'evidence link from that row'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($cell, $cell.Replace($build, "$build; $pr")) } 'link absent from its evidence/fix columns'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($buildUrl, 'file:///C:/reports/build-health.md') } 'HTTPS web URLs'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($cell, $cell.Replace($buildUrl, '..\build-health.md')) } 'HTTPS web URLs'
$monitoringGap = $report.Replace($buildUrl, 'https://dev.azure.com/dnceng/internal/_build').
    Replace($description, 'Please check this issue. Internal pipeline health is unknown because authenticated access was unavailable.').
    Replace('EMSDK signing |', 'Internal monitoring unavailable (no inventory) |')
Assert-QBOverallReport -Content $monitoringGap
Assert-Throws { Assert-QBOverallReport -Content $report.Replace('Existing fix PR (status)', 'Fix') } 'required six-column'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($cell, "$cell | extra") } 'six columns'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($topAction, 'No QB action required.') } 'contains outreach messages'

$githubReport = $report.Replace($build, $pr).Replace($where, "GitHub - $pr")
Assert-QBOverallReport -Content $githubReport
Assert-QBOverallReport -Content $report.Replace('@dotnet/dnr-codeflow,', '@dotnet/dnr-codeflow, @dotnet/dnceng,')
$firstResponders = $report.Replace('@dotnet/dnr-codeflow,', 'First Responders,').Replace($where, 'Teams - First Responders')
Assert-Throws { Assert-QBOverallReport -Content $firstResponders } 'never mentions Helix'
Assert-QBOverallReport -Content $firstResponders.Replace('EMSDK signing', 'Helix work item')
Assert-QBOverallReport -Content $report.Replace($topAction, "$topAction`n   - $build - Preserve the signing log for @dotnet/dnr-codeflow. Where: $where.")
Assert-QBOverallReport -Content $report.Replace($build, $subscription).Replace('EMSDK signing |', 'WindowsDesktop (no PR/build exists) |')
$fix = '[example-org/example-repo#102](https://github.com/example-org/example-repo/pull/102)'
Assert-QBOverallReport -Content $report.Replace('| None found | Where:', "| $fix | Where:").Replace($cell, $cell.Replace($build, "$build; $fix"))
$secondAction = "2. $pr - Ask @dotnet/dnceng for required review on this PR. Where: GitHub - $pr."
$secondCell = "Where: GitHub - $pr.<br>Copy/paste: `"@dotnet/dnceng, Please check this issue. CI is green, but review is required: $pr`""
$multiIssue = $report.Replace($topAction, "$topAction`n$secondAction") +
    "`n| Required PR review | $pr | N | N/A | $secondCell | None found |"
Assert-QBOverallReport -Content $multiIssue

$waitingCell = 'Where: None - monitor only.<br>Action: Owner reports a transient restore failure; rerun pending.'
$waiting = $report.Replace($topAction, 'No QB action required.').Replace($cell, $waitingCell)
Assert-QBOverallReport -Content $waiting
Assert-Throws { Assert-QBOverallReport -Content $waiting.Replace($waitingCell, 'Where: None - monitor only.<br>Copy/paste: "Please rerun."') } 'without a ping'
Assert-Throws { Assert-QBOverallReport -Content $waiting.Replace('rerun pending.', 'rerun the build.') } 'not remediation instructions'

# --- an external support destination: no GitHub team, not a Teams channel (for example 1ES support) ----------------
$supportLink = '[1ES support](https://aka.ms/1esptdocs)'
$supportWhere = "Support - $supportLink"
$supportTop = "1. $build - 1ES Pipeline Template task failed. Where: $supportWhere."
$supportCell = "Where: $supportWhere.<br>Copy/paste: `"1ES support, ${description}: $build`""
$supportReport = $report.Replace($topAction, $supportTop).Replace($cell, $supportCell)
Assert-QBOverallReport -Content $supportReport
Assert-QBOverallReport -Content $supportReport -TeamContacts @{ '@dotnet/dnceng' = [pscustomobject]@{ github = 'someone'; teams = 'Some One' } }
Assert-Throws { Assert-QBOverallReport -Content $supportReport.Replace('https://aka.ms/1esptdocs', 'https://example.com/support') } 'exact Teams/GitHub destination'
Assert-Throws { Assert-QBOverallReport -Content $supportReport.Replace('Support - [1ES support]', 'Support - [Some other support]') } 'exact Teams/GitHub destination'
Assert-Throws { Assert-QBOverallReport -Content $report.Replace($cell, "Where: $supportWhere.<br>Copy/paste: `"Mystery support, ${description}: $build`"").Replace($topAction, $supportTop) } 'quoted message'

# --- no owner can be identified: nobody to ping, but the QB still has to route it --------------------------------------------
$noOwnerWhere = 'None - owner not identified'
$noOwnerTop = "1. $build - Compilation failed and no owning repo is named in the retained log. Where: $noOwnerWhere."
$noOwnerCell = "Where: $noOwnerWhere.<br>Action: No identified owner. Compilation failed in the retained log and no owning repository is named."
$noOwner = $report.Replace($topAction, $noOwnerTop).Replace($cell, $noOwnerCell)
Assert-QBOverallReport -Content $noOwner
Assert-Throws { Assert-QBOverallReport -Content $noOwner.Replace($topAction, 'No QB action required.').Replace($noOwnerTop, 'No QB action required.') } 'contains outreach messages'
Assert-Throws { Assert-QBOverallReport -Content $noOwner.Replace($noOwnerCell, "Where: $noOwnerWhere.<br>Copy/paste: `"@dotnet/dnceng, ${description}: $build`"") } 'no identified owner'
Assert-Throws { Assert-QBOverallReport -Content $noOwner.Replace('No identified owner. Compilation', 'Compilation') } 'must say'
Assert-Throws { Assert-QBOverallReport -Content $noOwner.Replace('Compilation failed in the retained log', 'Compilation failed; @dotnet/dnceng may know') } 'must not ping anyone'
Assert-Throws { Assert-QBOverallReport -Content $noOwner.Replace('no owning repository is named.', 'please find the owner.') } 'factual status'
$known = Get-KnownFixContext -Path (Join-Path $PSScriptRoot 'fixtures\triage-context.fixture.json')

# --- named contact beside every team ping -----------------------------------
$contacts = @{
    '@dotnet/fsharp'       = [pscustomobject]@{ github = 'example-login-one';    teams = 'First Example' }
    '@dotnet/dnr-codeflow' = [pscustomobject]@{ github = 'example-login-two'; teams = 'Second Example' }
}
# No roster supplied -> no enforcement, so a missing roster can never block a report.
Assert-QBOverallReport -Content $report
# Teams destination must carry the display name.
Assert-Throws { Assert-QBOverallReport -Content $report -TeamContacts $contacts } 'without naming its contact'
$withTeamsContact = $report.Replace('@dotnet/dnr-codeflow,', '@dotnet/dnr-codeflow @Second Example,')
Assert-QBOverallReport -Content $withTeamsContact -TeamContacts $contacts
# The GitHub login is the wrong form for a Teams destination.
Assert-Throws { Assert-QBOverallReport -Content $report.Replace('@dotnet/dnr-codeflow,', '@dotnet/dnr-codeflow @example-login-two,') -TeamContacts $contacts } 'without naming its contact'
# GitHub destination must carry the login instead.
$githubRow = $report.Replace($build, $pr).Replace($where, "GitHub - $pr")
Assert-Throws { Assert-QBOverallReport -Content $githubRow -TeamContacts $contacts } 'without naming its contact'
Assert-QBOverallReport -Content $githubRow.Replace('@dotnet/dnr-codeflow,', '@dotnet/dnr-codeflow @example-login-two,') -TeamContacts $contacts
# A team with no roster entry is still pingable on its own.
Assert-QBOverallReport -Content $report.Replace('@dotnet/dnr-codeflow,', '@dotnet/prodconsvcs,') -TeamContacts $contacts
# The 30-word budget still excludes owners, so naming a person costs nothing.
$thirtyWithContact = $withTeamsContact.Replace($description, $thirtyWords)
Assert-QBOverallReport -Content $thirtyWithContact -TeamContacts $contacts
# A bare URL at the end of the quoted message is turned into a Markdown link by the script, so the row keeps its place.
$bareReport = $report.Replace($cell, "Where: $where.<br>Copy/paste: `"@dotnet/dnr-codeflow, ${description}: $buildUrl`"")
Assert-Throws { Assert-QBOverallReport -Content $bareReport } 'needs a quoted message'
$linkedReport = ConvertTo-QBActionFormat -Content $bareReport
Assert-QBOverallReport -Content $linkedReport
if (-not $linkedReport.Contains("${description}: [11]($buildUrl)`"")) { throw 'The bare build URL was not turned into a Markdown link labelled with its build id.' }
if ((ConvertTo-QBActionFormat -Content $linkedReport) -cne $linkedReport) { throw 'Turning bare URLs into links must be idempotent.' }
if ((ConvertTo-QBActionFormat -Content $report) -cne (ConvertTo-QBActionFormat -Content (ConvertTo-QBActionFormat -Content $report))) { throw 'An existing Markdown link must not be wrapped again.' }
$barePullAndIssue = $report.Replace($cell, "Where: $where.<br>Action: See https://github.com/dotnet/dotnet/pull/1, https://github.com/dotnet/dotnet/issues/2. and [kept](https://github.com/dotnet/dotnet/pull/3)")
$formattedBare = ConvertTo-QBActionFormat -Content $barePullAndIssue
foreach ($expected in @('[#1](https://github.com/dotnet/dotnet/pull/1),', '[#2](https://github.com/dotnet/dotnet/issues/2).', '[kept](https://github.com/dotnet/dotnet/pull/3)')) {
    if (-not $formattedBare.Contains($expected)) { throw "Bare URL formatting lost or doubled: $expected" }
}
if ($formattedBare.Contains('[[')) { throw 'A link label was wrapped inside another link.' }

$prompt = Get-OverallPrompt -Date $Date -OwnershipBlock $ownershipBlock -BuildHealthPath 'build-fixture.md' -CodeFlowPath 'flow-fixture.md' -KnownFixContext $known
foreach ($required in @('Start EVERY action and sub-bullet', 'One issue and one next step', 'AT MOST 30 words',
    'WindowsDesktop and WPF', 'Copy/paste:', 'Servicing chat', 'Preview/RC chat', 'owner-assessed transient',
    'Please check the build failure.', 'Please check this issue.', 'state ONLY what failed',
    'quoted action message reports only the failure/status',
    'All report hyperlinks must be HTTPS', 'https://dev.azure.com/dnceng/internal/_build',
    'https://github.com/example-org/example-repo/pull/102', 'Where: Support - [', 'Where: None - owner not identified', 'No identified owner',
    'BEFORE the destination', 'Nothing may follow the Where', 'Top actions holds ONLY actions', 'Never write a "Step 3" or "Step 4" heading')) {
    if (-not $prompt.Contains($required)) { throw "Prompt lost required guidance: $required" }
}

$html = ConvertTo-QBHtmlBody -Date $Date -RunStatus complete -Results @() -Attachments @('overall-fixture.md') -OverallMarkdown $report
if ($html -notmatch '<table style=' -or $html -notmatch '<br\s*/?>Copy/paste:' -or
    $html -notmatch 'href="https://dev.azure.com/dnceng/internal/_build/results\?buildId=11"') {
    throw 'HTML email lost the action table, line break, or clickable evidence.'
}
if (-not [Net.WebUtility]::HtmlDecode($html).Contains('Copy/paste: "@dotnet/dnr-codeflow,')) {
    throw 'HTML email lost the quoted copy-ready message.'
}

$fixtureRoot = Join-Path ([IO.Path]::GetTempPath()) ('vmr-qb-action-test-' + [guid]::NewGuid().ToString('N'))
$logDir = $fixtureRoot
$outputPath = Join-Path $fixtureRoot 'overall.md'
$specialistPath = Join-Path $fixtureRoot 'specialist.md'
$invalidPath = Join-Path $fixtureRoot "copilot-overall-summary-$Date-$stamp.jsonl.invalid.md"
$reuseDirectory = Join-Path $fixtureRoot 'reports'
$reuseBuild = Join-Path $reuseDirectory "build-health-$Date.md"
$reuseFlow = Join-Path $reuseDirectory "code-flow-$Date.md"
$reuseSummary = Join-Path $reuseDirectory "overall-summary-$Date.md"
[void](New-Item -ItemType Directory -Path $fixtureRoot)
$runWorkspace = [pscustomobject]@{ Path = $fixtureRoot }
# New-Report reads these two optional files; the fixture directory holds neither, so no history or operator links apply.
$paths = [pscustomobject]@{
    HistoryPath       = Join-Path $fixtureRoot 'issue-history.json'
    ReportedLinksPath = Join-Path $fixtureRoot 'reported-links.json'
}
function Invoke-CopilotReport {
    param($Prompt, $TranscriptPath, $ErrorPath, [switch] $Synthesis)
    return $script:generatedContent
}
try {
    $script:generatedContent = $report
    $success = New-Report -Name overall-summary -Prompt fixture -OutputPath $outputPath -Synthesis 6>$null
    if ($success.status -ne 'succeeded') { throw "Valid summary was rejected: $($success.error)" }
    $saved = Get-Content -LiteralPath $outputPath -Raw

    $script:generatedContent = $report.Replace($description, "$thirtyWords extra")
    # A report that fails a check is never withheld: it is saved, and the finding is a run note.
    Clear-QBRunNotes
    $flagged = New-Report -Name overall-summary -Prompt fixture -OutputPath $outputPath -Synthesis 6>$null
    if ($flagged.status -ne 'succeeded' -or $flagged.path -ne $outputPath) { throw 'A report that fails a check must still be saved and delivered.' }
    if (@(Get-QBRunNotes | Where-Object { $_.message -match 'maximum is 30' }).Count -ne 1) { throw 'The failed check must be a run note.' }
    if ((Get-Content -LiteralPath $outputPath -Raw) -ceq $saved) { throw 'The flagged report was not saved.' }

    $script:generatedContent = '# Specialist fixture: existing format is unchanged.'
    $specialist = New-Report -Name build-health -Prompt fixture -OutputPath $specialistPath 6>$null
    if ($specialist.status -ne 'succeeded') { throw 'The overall format check changed specialist-report behavior.' }

    [void](New-Item -ItemType Directory -Path $reuseDirectory)
    Set-Content -LiteralPath $reuseBuild -Value "# Build Health Monitoring - $Date"
    Set-Content -LiteralPath $reuseFlow -Value "# Code Flow Monitoring - $Date"
    Set-Content -LiteralPath $reuseSummary -Value $report
    $existing = @(Get-QBExistingReports -Root $fixtureRoot -Date $Date)
    if ($existing.Count -ne 3 -or $existing[2].path -ne $reuseSummary) {
        throw 'Existing-report recovery did not retain the three expected attachments.'
    }
    Set-Content -LiteralPath $reuseSummary -Value $report.Replace($description, "$thirtyWords extra")
    Clear-QBRunNotes
    if (@(Get-QBExistingReports -Root $fixtureRoot -Date $Date).Count -ne 3) { throw 'Reuse must still return the three reports when a check fails.' }
    if (@(Get-QBRunNotes | Where-Object { $_.message -match 'maximum is 30' }).Count -ne 1) { throw 'Reuse must record the failed check as a run note.' }
    Set-Content -LiteralPath $reuseSummary -Value $report
    # A check never costs a report on reuse either: a report with another day's title is kept, with a note that says so.
    Set-Content -LiteralPath $reuseFlow -Value '# Code Flow Monitoring - 2000-01-01'
    Clear-QBRunNotes
    $wrongTitle = @(Get-QBExistingReports -Root $fixtureRoot -Date $Date)
    if ($wrongTitle.Count -ne 3 -or @($wrongTitle | Where-Object status -ne 'succeeded').Count) { throw 'A report with the wrong dated title must still be reused.' }
    if (@(Get-QBRunNotes | Where-Object { $_.source -eq 'report-check' -and $_.message -match 'no title line' }).Count -ne 1) { throw 'The wrong title must be one run note.' }
    # A report that is not there is a failed result with its reason; the others are still returned and delivered.
    Remove-Item -LiteralPath $reuseFlow
    $missing = @(Get-QBExistingReports -Root $fixtureRoot -Date $Date)
    if ($missing.Count -ne 3 -or $missing[1].status -ne 'failed' -or $missing[1].error -notmatch 'code-flow report is missing' -or
        $missing[0].status -ne 'succeeded' -or $missing[2].status -ne 'succeeded') { throw 'A missing report must be a failed result while the other two are still reused.' }

    # The failure that lost a whole overall report in production: an action that links something its own row does not list.
    $script:generatedContent = $report.Replace($cell, $cell.Replace($build, "$build; $pr"))
    Clear-QBRunNotes
    $absent = New-Report -Name overall-summary -Prompt fixture -OutputPath $outputPath -Synthesis 6>$null
    if ($absent.status -ne 'succeeded' -or $absent.path -ne $outputPath) { throw 'A link absent from its row must not fail the report.' }
    $absentNotes = @(Get-QBRunNotes | Where-Object { $_.source -eq 'report-check' -and $_.message -match 'link absent from its evidence/fix columns' })
    if ($absentNotes.Count -ne 1 -or $absentNotes[0].severity -ne 'first') { throw 'The absent link must be one highlighted run note.' }
    if (-not (Get-Content -LiteralPath $outputPath -Raw).Contains($pr)) { throw 'The report must be saved as the model wrote it.' }

    # Every problem is its own note, so one bad row never hides another.
    $bothBad = $script:generatedContent + "`n| Second finding | $build | Y | None found | Ping the team about the problem. | None found |"
    $formatFindings = @(Invoke-QBOverallReportGate -Content $bothBad -Date $Date | Where-Object { $_.check -eq 'format' })
    if ($formatFindings.Count -ne 2) { throw "Two bad rows must be two findings, got $($formatFindings.Count): $(($formatFindings | ForEach-Object message) -join ' | ')" }
    $objectiveFindings = @(Invoke-QBOverallReportGate -Content ($report.Replace($cell, $cell.Replace($build, "$build; $pr")).Replace('@dotnet/dnr-codeflow', '@dotnet/totally-fake-team')) -Date $Date | Where-Object severity -eq 'fail')
    if (@($objectiveFindings | Where-Object { $_.check -eq 'format' -and $_.message -match 'link absent' }).Count -ne 1) { throw 'A handle problem must not hide a format problem.' }
    if (@($objectiveFindings | Where-Object { $_.message -match 'Report check failed' }).Count) { throw 'The objective checks must not be reported twice, once inside a composite message.' }
}
finally {
    foreach ($path in @($outputPath, $specialistPath, $reuseBuild, $reuseFlow, $reuseSummary, $invalidPath)) {
        if (Test-Path -LiteralPath $path) { Remove-Item -LiteralPath $path }
    }
    if (Test-Path -LiteralPath $reuseDirectory) { Remove-Item -LiteralPath $reuseDirectory }
    Remove-Item -LiteralPath $fixtureRoot
}

Write-Output 'PASS: factual check messages, no remediation directives, link-first actions, destinations, 30-word boundary, waiting items, HTML email, and save-time validation.'
