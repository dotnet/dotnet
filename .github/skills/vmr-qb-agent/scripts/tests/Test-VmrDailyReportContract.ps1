[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot '..\daily\report.ps1')
. (Join-Path $PSScriptRoot '..\history\history.ps1')

function Assert-Throws {
    param([scriptblock] $Action, [string] $Expected)
    try { & $Action | Out-Null }
    catch {
        if ($_.Exception.Message -notmatch $Expected) { throw }
        return
    }
    throw "Expected error: $Expected"
}

function Copy-Value([object] $Value) { return $Value | ConvertTo-Json -Depth 100 | ConvertFrom-Json -Depth 100 }
function Write-Json([string] $Path, [object] $Value) {
    ConvertTo-Json -InputObject $Value -Depth 100 | Set-Content -LiteralPath $Path -Encoding utf8
}

# Windows repo builds redirect inner output to a log file, so the timeline often retains only the
# MSB3073 wrapper. That used to be dropped outright, leaving a finding with no evidence and a vague
# summary like "'roslyn' failed during build". The wrapper must now name repo, exit code and log.
$wrapperWindows = 'repo-projects\Directory.Build.targets(675,5): error MSB3073: The command "D:\a\_work\1\s\src\roslyn\build.cmd -restore -pack -publish -ci > D:\a\_work\1\s\artifacts\log\Release\roslyn\roslyn.log 2>&1" exited with code 1. [D:\a\_work\1\s\repo-projects\roslyn.proj]'
$summarised = ConvertTo-WrapperDiagnostic $wrapperWindows
foreach ($expected in @("'roslyn' repo build exited", 'with code 1', 'no inner compiler or restore diagnostic', 'artifacts\log\Release\roslyn\roslyn.log')) {
    if ($summarised -notmatch [regex]::Escape($expected)) {
        throw "Wrapper diagnostic lost '$expected': $summarised"
    }
}
$wrapperNpm = ConvertTo-WrapperDiagnostic 'repo-projects/Directory.Build.targets(709,5): error MSB3073: The command "/__w/1/s/src/emsdk/artifacts/obj/build/node/linux-x64/bin/npm ci" exited with code 127.'
if ($wrapperNpm -notmatch "'emsdk' repo build exited with code 127") {
    throw "Wrapper diagnostic did not identify emsdk/127: $wrapperNpm"
}
if ($null -ne (ConvertTo-WrapperDiagnostic 'error CS0234: The type or namespace name ''Public'' does not exist')) {
    throw 'Real compiler diagnostics must not be rewritten as wrapper summaries'
}
if ($null -ne (ConvertTo-WrapperDiagnostic '')) { throw 'Empty wrapper text must yield null' }

# Get-DiagnosticSummary is a nested helper, so lift it out by AST the same way the main-runner
# functions are lifted below.
$contractAst = [System.Management.Automation.Language.Parser]::ParseFile(
    (Join-Path $PSScriptRoot '..\daily\report.ps1'), [ref]$null, [ref]$null)
foreach ($helper in @('Short-Text', 'Get-DiagnosticSeverity', 'Get-DiagnosticSummary')) {
    $helperAst = $contractAst.Find({
        param($node)
        $node -is [System.Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq $helper
    }, $true)
    if ($null -eq $helperAst) { throw "Missing nested helper $helper" }
    . ([scriptblock]::Create($helperAst.Extent.Text))
}

# The wrapper must be kept only when its job retained nothing better. A job that also has a real
# diagnostic must still drop the wrapper, otherwise every failure gains a duplicate noise row.
$wrapperTasks = @(
    [pscustomobject]@{ job = 'Windows_x64'; task = 'Build'; result = 'failed'; issues = @($wrapperWindows) }
    [pscustomobject]@{ job = 'OSX_x64'; task = 'Build'; result = 'failed'; issues = @(
        'repo-projects/Directory.Build.targets(675,5): error MSB3073: The command "/s/src/sdk/eng/common/build.sh --restore" exited with code 1.'
        'tool_projectdata.csproj : error NU1109: Detected package downgrade: NuGet.Versioning from 8.0.0-rc.48205 to centrally defined 8.0.0-rc.303.'
    ) }
)
$wrapperRecords = @(
    [pscustomobject]@{ recordType = 'Job'; task = 'Windows_x64'; result = 'failed' }
    [pscustomobject]@{ recordType = 'Job'; task = 'OSX_x64'; result = 'failed' }
)
$wrapperDiagnostics = @((Get-DiagnosticSummary -Tasks $wrapperTasks -Logs @() -AllRecords $wrapperRecords).diagnostics)
$allQuotes = ($wrapperDiagnostics | ForEach-Object quote) -join ' | '
$windowsRows = @($wrapperDiagnostics | Where-Object { (@($_.tasks) -join ',') -match 'Windows_x64' })
if ($windowsRows.Count -ne 1 -or $windowsRows[0].quote -notmatch 'MSB3073.*src\\roslyn\\build.cmd') {
    throw "Wrapper-only job lost its evidence: $allQuotes"
}
if (-not $wrapperWindows.Contains($windowsRows[0].quote)) {
    throw "Wrapper quote must remain a verbatim substring of the retained diagnostic: $($windowsRows[0].quote)"
}
if ($windowsRows[0].explanation -notmatch 'inner log not collected') {
    throw "Wrapper explanation omitted the uncollected inner log: $($windowsRows[0].explanation)"
}
$osxRows = @($wrapperDiagnostics | Where-Object { (@($_.tasks) -join ',') -match 'OSX_x64' })
if (@($osxRows | Where-Object { $_.quote -match 'repo build exited with code' }).Count -ne 0) {
    throw "Job with a real diagnostic must not keep a synthesized wrapper: $allQuotes"
}
if (@($osxRows | Where-Object { $_.quote -match 'NU1109|package downgrade' }).Count -ne 1) {
    throw "Real NU1109 diagnostic was lost: $allQuotes"
}

$invalidFlagTasks = @(
    [pscustomobject]@{ job = 'Windows_x64'; task = 'Build'; result = 'failed'; issues = @($wrapperWindows, 'Invalid argument: -officialSkipTests') }
)
$invalidFlagRows = @((Get-DiagnosticSummary -Tasks $invalidFlagTasks -Logs @() -AllRecords $wrapperRecords).diagnostics)
if ($invalidFlagRows.Count -ne 1 -or $invalidFlagRows[0].quote -ne 'Invalid argument: -officialSkipTests') {
    throw 'The initiating invalid-argument diagnostic must replace its outer build wrapper.'
}

$temporary = Join-Path ([IO.Path]::GetTempPath()) ('vmr-daily-contract-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($temporary)
$date = '2026-09-24'
try {
    $inventoryPath = Join-Path $temporary 'inventory.json'
    $rootLog = Join-Path $temporary 'root.log'
    $logsPath = Join-Path $temporary 'root-logs.json'
    $timelinePath = Join-Path $temporary 'timeline.json'
    Set-Content -LiteralPath $rootLog -Value 'MSBuild server failed to shut down: directory validation failed.'
    Write-Json $inventoryPath @(
        @{ id = 1; sourceBranch = 'refs/heads/main'; result = 'succeeded'; finishTime = '2026-09-24T01:00:00Z' },
        @{ id = 2; sourceBranch = 'refs/heads/internal/release/10.0.1xx'; result = 'failed'; finishTime = '2026-09-24T02:00:00Z' },
        @{ id = 3; sourceBranch = 'refs/heads/release/11.0.1xx'; result = 'canceled'; finishTime = '2026-09-24T03:00:00Z' }
    )
    Write-Json $logsPath @{ logs = @(@{ buildId = 2; path = $rootLog; available = $true }) }
    Write-Json $timelinePath @{ records = @(@{ issues = @(@{ message = 'The build was canceled by its requester.' }) }) }
    $failureTasksPath = Join-Path $temporary 'failure-tasks.json'
    Write-Json $failureTasksPath @{ builds = @() }
    $evidence = [pscustomobject]@{
        schemaVersion = 2; status = 'complete'; reportDate = $date
        windowStartUtc = '2026-09-23T13:00:00Z'; observationCutoffUtc = '2026-09-24T13:00:00Z'
        rootLogsPath = $logsPath; failureTasksPath = $failureTasksPath; requiredErrors = @()
        sources = @(@{
            organization = 'dnceng'; apiPath = 'build/builds/3/timeline'; status = 'available'; path = $timelinePath
        })
        pipelines = @(@{
            name = 'internal'; status = 'available'; inventoryPath = $inventoryPath
            requiredProblemBuildIds = @(2, 3)
            counts = @{ completed = 3; succeeded = 1; failed = 1; canceled = 1; partiallySucceeded = 0 }
            error = $null; loginCommand = $null
        })
    }
    $findings = [pscustomobject]@{
        schemaVersion = 1; reportDate = $date; sourceStatus = 'available'
        builds = @(
            @{ buildId = 2; roots = @(@{
                id = 'shared-memory'; description = 'Source-build bootstrap failed with a shared-memory error.'
                quote = 'MSBuild server failed to shut down'; ownerTeams = @('@dotnet/source-build', '@dotnet/msbuild')
            }) },
            @{ buildId = 3; roots = @(@{
                id = 'manual-cancel'; description = 'Requester canceled the build.'
                quote = 'The build was canceled by its requester.'; ownerTeams = @('@dotnet/prodconsvcs')
            }) }
        )
    }
    Assert-QBInternalFindings -Findings $findings -Evidence $evidence -Date $date
    $savedDiagnostic = Get-Content -LiteralPath $rootLog -Raw
    try {
        Set-Content -LiteralPath $rootLog -Value $wrapperWindows
        $wrapperFindings = Copy-Value $findings
        $wrapperFindings.builds[0].roots[0].quote = $windowsRows[0].quote
        $wrapperFindings.builds[0].roots[0].description = $windowsRows[0].explanation
        Assert-QBInternalFindings -Findings $wrapperFindings -Evidence $evidence -Date $date
        $wrapperFindings.builds[0].roots[0].quote = $windowsRows[0].explanation
        Assert-Throws { Assert-QBInternalFindings $wrapperFindings $evidence $date } 'retained evidence'
    }
    finally { Set-Content -LiteralPath $rootLog -Value $savedDiagnostic }
    $bad = Copy-Value $findings; $bad.builds = @($bad.builds[0])
    Assert-Throws { Assert-QBInternalFindings $bad $evidence $date } 'exactly the scoped problem builds'
    $bad = Copy-Value $findings; $bad.builds[0].roots[0].quote = 'Not an error from this build'
    Assert-Throws { Assert-QBInternalFindings $bad $evidence $date } 'retained evidence'
    $bad = Copy-Value $findings; $bad.sourceStatus = 'authentication_required'
    Assert-Throws { Assert-QBInternalFindings $bad $evidence $date } 'contradict'
    $bad = Copy-Value $findings; $bad.builds[0].roots[0].ownerTeams = @()
    # Never fail because of an owner: a finding with none is shown as unidentified, and a note says so.
    Clear-QBRunNotes
    Assert-QBInternalFindings $bad $evidence $date
    if (@($bad.builds[0].roots[0].ownerTeams) -notcontains 'No identified owner') { throw 'A finding with no owner must be shown as unidentified, not rejected.' }
    if (@(Get-QBRunNotes | Where-Object { $_.message -match 'had no owner' }).Count -ne 1) { throw 'The missing owner must be a run note.' }
    $rendered = ConvertTo-QBInternalSection $evidence $findings
    foreach ($expected in @('| Rolling | 3 | 1 | 1 | 0 | 1 | 33.3% |', 'buildId=2', 'buildId=3', '### internal/release/*', '### main + release/*',
        'Window (UTC): 2026-09-23T13:00:00.000Z through 2026-09-24T13:00:00.000Z')) {
        if (-not $rendered.Contains($expected)) { throw "Rendered internal report missing $expected" }
    }
    $json = $findings | ConvertTo-Json -Depth 20
    $document = "Report`n<!-- VMR-INTERNAL-FINDINGS`n$json`nVMR-INTERNAL-FINDINGS -->`n"
    $section = Read-QBStructuredSection $document 'VMR-INTERNAL-FINDINGS'
    if ($section.Content.Trim() -ne 'Report' -or $section.Data.builds.Count -ne 2) { throw 'Structured payload extraction lost content.' }
    Assert-Throws { Read-QBStructuredSection ($document + $document) 'VMR-INTERNAL-FINDINGS' } 'exactly one'

    $issues = [pscustomobject]@{
        schemaVersion = 1; status = 'complete'; errors = @()
        issues = @(@{ repository = 'dotnet/dotnet'; number = 101; state = 'open'; url = 'https://github.com/dotnet/dotnet/issues/101' })
        assigneeCandidates = @(@{
            login = 'triager'; assignable = $true; evidenceUrl = 'https://github.com/dotnet/dotnet/issues/101'
        })
    }
    $plan = [pscustomobject]@{
        schemaVersion = 1; reportDate = $date
        actions = @(
            @{ findingId = 'shared-memory'; buildIds = @(2); decision = 'existing_issue'
               issueUrl = 'https://github.com/dotnet/dotnet/issues/101'; reason = 'Exact tracked diagnostic.'
               assignee = 'triager'; assigneeBasis = 'Existing issue triager; selectable in this repository.'
               comment = $null },
            @{ findingId = 'manual-cancel'; buildIds = @(3); decision = 'monitor_only'; reason = 'Requester canceled.' }
        )
    }
    Assert-QBDailyActions $plan $findings $issues $date
    $bad = Copy-Value $plan; $bad.actions = @($bad.actions[0])
    Assert-Throws { Assert-QBDailyActions $bad $findings $issues $date } 'exactly once'
    $bad = Copy-Value $plan; $bad.actions[0].issueUrl = 'https://github.com/dotnet/dotnet/issues/99999'
    Assert-Throws { Assert-QBDailyActions $bad $findings $issues $date } 'collected issue evidence'
    $bad = Copy-Value $plan; $bad.actions[0].assignee = 'unverified-user'
    Assert-Throws { Assert-QBDailyActions $bad $findings $issues $date } 'verified as selectable'
    $bad = Copy-Value $plan; $bad.actions[0].decision = 'needs_triage'; $bad.actions[0].issueUrl = $null
    Clear-QBRunNotes
    Assert-QBDailyActions $bad $findings $issues $date
    if (@(Get-QBRunNotes | Where-Object { $_.source -eq 'action-plan' -and $_.message -match 'no conditional copy-ready title/body' }).Count -ne 1) { throw 'A missing conditional draft must be one note, not a rejection of the whole plan.' }
    Clear-QBRunNotes
    $new = Copy-Value $plan
    $new.actions[0] = [pscustomobject]@{
        findingId = 'shared-memory'; buildIds = @(2); decision = 'new_issue'
        title = '[Operational Issue]: shared-memory bootstrap failure'
        body = "### Error`nExact diagnostic.`n### Links`nhttps://dev.azure.com/dnceng/internal/_build/results?buildId=2`n### Root Cause`nUnknown.`n### Blocking`nNot established.`n### Affected Releases`n.NET 10"
        searchBasis = 'Checked collected open/closed issues and relevant comments.'
        reason = 'No exact match in bounded evidence.'; assignee = $null
    }
    Assert-QBDailyActions $new $findings $issues $date
    $partial = Copy-Value $issues; $partial.status = 'partial'
    Assert-Throws { Assert-QBDailyActions $new $findings $partial $date } 'incomplete'
    $draft = ConvertTo-QBDailyActionSection $new
    if (-not $draft.Contains('Copy-ready body') -or -not $draft.Contains('shared-memory bootstrap failure')) {
        throw 'New issue draft was not included in rendered output.'
    }
    $conditional = Copy-Value $new
    $conditional.actions[0].decision = 'needs_triage'
    Assert-QBDailyActions $conditional $findings $issues $date
    if (-not (ConvertTo-QBDailyActionSection $conditional).Contains('Conditional draft only')) {
        throw 'An uncertain issue proposal was not visibly qualified.'
    }
    $bad = Copy-Value $new; $bad.actions[0].decision = 'monitor_only'
    Assert-Throws { Assert-QBDailyActions $bad $findings $issues $date } 'duplicate new-issue drafts'

    # An already-tracked finding must never be drafted as new. The model only sees today's evidence,
    # so a tracker recorded on an earlier day (or an IcM GitHub cannot see) is enforced here.
    Assert-Throws {
        Assert-QBDailyActions $new $findings $issues $date @{ 'shared-memory' = 'https://github.com/dotnet/example/issues/102' }
    } 'already tracked by'
    # Matching on the drafted title must work too, since findingId wording drifts between days.
    Assert-Throws {
        Assert-QBDailyActions $new $findings $issues $date @{ '[Operational Issue]: shared-memory bootstrap failure' = 'an operator-recorded owned status' }
    } 'already tracked by'
    # Unrelated trackers must not block an otherwise valid plan.
    Assert-QBDailyActions $new $findings $issues $date @{ 'some-other-finding' = 'https://github.com/dotnet/dotnet/issues/1' }
    # existing_issue is the correct decision and must stay allowed even when tracked.
    Assert-QBDailyActions $plan $findings $issues $date @{ 'shared-memory' = 'https://github.com/dotnet/example/issues/102' }
    $missing = New-QBUnavailableEvidence $date ([DateTimeOffset]'2026-09-24T13:00:00Z') 'AUTH_ERROR: sign-in required.'
    $alerts = @(Get-QBCoverageAlerts $missing $issues)
    if (-not ($alerts -join ' ').Contains('az login --allow-no-subscriptions')) { throw 'Authentication alert has no sign-in command.' }
    $unavailable = [pscustomobject]@{ schemaVersion = 1; reportDate = $date; sourceStatus = 'authentication_required'; builds = @() }
    Assert-QBInternalFindings $unavailable $missing $date
    $gap = ConvertTo-QBInternalSection $missing $unavailable
    if (-not $gap.Contains('unavailable, not healthy') -or $gap.Contains('| Rolling |')) { throw 'Authentication gap fabricated a pass rate.' }
    if (@(Get-QBCoverageAlerts $evidence $issues).Count -ne 0) { throw 'Complete evidence incorrectly generated an alert.' }

    $root = Split-Path -Parent $PSScriptRoot
    $tokens = $null; $errors = $null
    $ast = [System.Management.Automation.Language.Parser]::ParseFile(
        (Join-Path $root 'daily\Invoke-Daily.ps1'), [ref]$tokens, [ref]$errors)
    if ($errors.Count) { throw ($errors.Message -join '; ') }
    . (Join-Path $root 'daily\contract.ps1')
    foreach ($name in @('New-Report', 'Get-QBTranscriptModel', 'Get-QBTeamContacts', 'Test-QBPinnedModelRejected', 'Invoke-QBModelRun', 'Complete-QBReportDocument')) {
        $fn = $ast.Find({
            param($node)
            $node -is [System.Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq $name
        }, $false)
        if ($null -eq $fn) { throw "Missing main-runner function $name" }
        . ([scriptblock]::Create($fn.Extent.Text))
    }
    $Date = $date
    $stamp = 'fixture'
    $logDir = $temporary
    $runWorkspace = [pscustomobject]@{ Path = $temporary }
    # New-Report reads these two optional files; the fixture directory holds neither, so no history or operator links apply.
    $paths = [pscustomobject]@{
        HistoryPath       = Join-Path $temporary 'issue-history.json'
        ReportedLinksPath = Join-Path $temporary 'reported-links.json'
    }
    $script:generationCalls = 0
    function Invoke-CopilotReport {
        param($Prompt, $TranscriptPath, $ErrorPath, [switch] $Synthesis, [switch] $FormattingOnly, $DataDirectory)
        $script:generationCalls++
        return $script:fixtureAnswer
    }
    $script:fixtureAnswer = $document -replace '^Report', "# Build Health Monitoring - $Date"
    $buildOutput = Join-Path $temporary 'build-health.md'
    $generated = New-Report -Name build-health -Prompt fixture -OutputPath $buildOutput -Evidence $evidence 6>$null
    if ($generated.status -ne 'succeeded' -or -not (Test-Path -LiteralPath "$buildOutput.data.json")) {
        throw "Internal build-health integration failed: $($generated.error)"
    }
    $body = Get-Content -LiteralPath $buildOutput -Raw
    if (-not $body.Contains('## Internal pipeline health (verified snapshot)') -or $body.Contains('VMR-INTERNAL-FINDINGS')) {
        throw 'Build-health report did not render the verified internal payload cleanly.'
    }
    $partialEvidence = Copy-Value $evidence
    $partialEvidence.status = 'partial'
    $partialEvidence.pipelines[0].status = 'partial'
    $partialReport = New-Report -Name build-health -Prompt fixture -OutputPath $buildOutput -Evidence $partialEvidence 6>$null 3>$null
    if ($partialReport.status -ne 'succeeded' -or $partialReport.payload.sourceStatus -ne 'partial') {
        throw 'Collector status did not remain authoritative for otherwise valid partial analysis.'
    }
    if (@(Get-QBCoverageAlerts $partialEvidence $issues).Count -eq 0) {
        throw 'Normalizing generated metadata hid an incomplete collector.'
    }
    $planJson = $plan | ConvertTo-Json -Depth 30
    $script:fixtureAnswer = @"
# VMR QB Daily Overall Report - $date
## Top actions for the QB today
1. [Build 2](https://dev.azure.com/dnceng/internal/_build/results?buildId=2) - Shared-memory bootstrap failed. Where: Teams - Unified Build Technical and General Discussions.
## Summary
| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |
|---|---|---|---|---|---|
| Bootstrap | [Build 2](https://dev.azure.com/dnceng/internal/_build/results?buildId=2) | N | None found | Where: Teams - Unified Build Technical and General Discussions.<br>Copy/paste: "@dotnet/source-build, Please check the build failure. Source-build bootstrap failed with shared-memory errors: [Build 2](https://dev.azure.com/dnceng/internal/_build/results?buildId=2)" | None found |
<!-- VMR-QB-ACTIONS
$planJson
VMR-QB-ACTIONS -->
"@
    $overallOutput = Join-Path $temporary 'overall.md'
    $generated = New-Report -Name overall-summary -Prompt fixture -OutputPath $overallOutput -Synthesis `
        -Evidence $evidence -IssueEvidence $issues -InternalFindings $findings 6>$null
    if ($generated.status -ne 'succeeded') { throw "Overall action integration failed: $($generated.error)" }
    $body = Get-Content -LiteralPath $overallOutput -Raw
    if (-not $body.Contains('## Internal issue tracking and action drafts') -or $body.Contains('VMR-QB-ACTIONS')) {
        throw 'Overall report did not render the action supplement cleanly.'
    }
    $html = ConvertTo-QBHtmlBody -Date $Date -RunStatus partial -Results @() -Attachments @() `
        -OverallMarkdown ($body + "`n" + (ConvertTo-QBDailyActionSection $new)) -CoverageAlerts $alerts
    if (-not $html.Contains('az login --allow-no-subscriptions') -or
        -not $html.Contains('Internal issue tracking and action drafts') -or $html -notmatch '<table style=') {
        throw 'HTML email omitted the real auth alert or internal action supplement.'
    }
    if (-not $html.Contains('Copy-ready body') -or -not $html.Contains('shared-memory bootstrap failure')) {
        throw 'HTML email lost the proposed issue title/body.'
    }
    $linkInventory = Join-Path $temporary 'public-link-inventory.json'
    Write-Json $linkInventory @{ value = @(@{
        id = 2; sourceBranch = 'refs/pull/1/merge'
        vmrClassification = @{ pullRequest = @{ number = 1; url = 'https://github.com/dotnet/dotnet/pull/1' } }
    }) }
    $linkEvidence = [pscustomobject]@{ pipelines = @(@{ inventoryPath = $linkInventory }) }
    $linked = $script:fixtureAnswer.Replace(
        'shared-memory errors: [Build 2](https://dev.azure.com/dnceng/internal/_build/results?buildId=2)',
        'shared-memory errors: [PR #1](https://github.com/dotnet/dotnet/pull/1)')
    Assert-Throws { Assert-QBOverallReport $linked } 'evidence link from that row'
    $canonical = ConvertTo-QBActionFormat -Content $linked -Evidence $linkEvidence
    Assert-QBOverallReport $canonical
    if (-not $canonical.Contains(', [PR #1](https://github.com/dotnet/dotnet/pull/1) | N |')) {
        throw 'Verified build-to-PR relationship was not added to the evidence cell.'
    }
    $unrelated = $linked.Replace('https://github.com/dotnet/dotnet/pull/1', 'https://github.com/dotnet/dotnet/pull/999')
    Assert-Throws { Assert-QBOverallReport (ConvertTo-QBActionFormat -Content $unrelated -Evidence $linkEvidence) } 'evidence link from that row'
    $script:generationCalls = 0
    $script:fixtureAnswer = 'Invalid document with no structured findings.'
    # A document with no verifiable findings is still saved, after exactly one repair attempt, with a note that says so.
    Clear-QBRunNotes
    $flagged = New-Report -Name build-health -Prompt fixture -OutputPath $buildOutput -Evidence $evidence 6>$null 3>$null
    if ($flagged.status -ne 'succeeded' -or $script:generationCalls -ne 2) {
        throw 'A malformed report must be saved after exactly one repair attempt.'
    }
    if (@(Get-QBRunNotes | Where-Object { $_.message -match 'could not be verified' }).Count -lt 1) { throw 'The unverified findings must be a run note.' }

    # A YAML pre-flight rejection has no timeline and no logs; its only retained evidence
    # is the synthesized pre-flight validation task in failure-tasks.json.
    $preflight = Copy-Value $evidence
    $preflight.pipelines[0].requiredProblemBuildIds = @(2, 3, 4)
    Write-Json $inventoryPath @(
        @{ id = 1; sourceBranch = 'refs/heads/main'; result = 'succeeded'; finishTime = '2026-09-24T01:00:00Z' },
        @{ id = 2; sourceBranch = 'refs/heads/internal/release/10.0.1xx'; result = 'failed'; finishTime = '2026-09-24T02:00:00Z' },
        @{ id = 3; sourceBranch = 'refs/heads/release/11.0.1xx'; result = 'canceled'; finishTime = '2026-09-24T03:00:00Z' },
        @{ id = 4; sourceBranch = 'refs/heads/release/10.0.1xx'; result = 'failed'; finishTime = '2026-09-24T04:00:00Z' }
    )
    Write-Json $failureTasksPath @{ builds = @(@{
        buildId = 4; organization = 'dnceng'; timelineStatus = 'none'
        tasks = @(@{ recordType = 'PipelineValidation'; task = 'Pipeline validation'; result = 'failed'
            issues = @(@{ type = 'error'; message = 'The pipeline is not valid. Job Windows_x64: Step NuGetAuthenticate2 input nuGetServiceConnections expects a service connection of type ExternalNuGetFeed.' }) })
    }) }
    $preflightFindings = Copy-Value $findings
    $preflightFindings.builds += @{ buildId = 4; roots = @(@{
        id = 'nuget-auth-connection-type'; description = 'The pipeline was rejected before any job ran.'
        quote = 'Step NuGetAuthenticate2 input nuGetServiceConnections expects a service connection of type ExternalNuGetFeed'
        ownerTeams = @('@dotnet/dnceng')
    }) }
    Assert-QBInternalFindings -Findings $preflightFindings -Evidence $preflight -Date $date

    # Never fail because of an owner: every finding without one is shown as unidentified, each with its own note.
    $unrouted = Copy-Value $preflightFindings
    $unrouted.builds[0].roots[0].ownerTeams = @()
    $unrouted.builds[2].roots[0].ownerTeams = @()
    Clear-QBRunNotes
    Assert-QBInternalFindings -Findings $unrouted -Evidence $preflight -Date $date
        if (@(Get-QBRunNotes | Where-Object { $_.message -match 'had no owner' }).Count -ne 2) { throw 'Each finding without an owner must get its own note.' }

    # There is no default team: a finding that maps to none names "No identified owner" and passes, with or without a roster.
    $noOwner = Copy-Value $preflightFindings
    $noOwner.builds[2].roots[0].ownerTeams = @('No identified owner')
    Assert-QBInternalFindings -Findings $noOwner -Evidence $preflight -Date $date
    Assert-QBInternalFindings -Findings $noOwner -Evidence $preflight -Date $date -KnownOwnerTeams @('@dotnet/source-build', '@dotnet/msbuild', '@dotnet/dnceng', '@dotnet/prodconsvcs')

    # An external support destination (for example 1ES support) is a valid owner too, with or without a roster.
    $supportOwner = Copy-Value $preflightFindings
    $supportOwner.builds[2].roots[0].ownerTeams = @('1ES support')
    Assert-QBInternalFindings -Findings $supportOwner -Evidence $preflight -Date $date
    Assert-QBInternalFindings -Findings $supportOwner -Evidence $preflight -Date $date -KnownOwnerTeams @('@dotnet/source-build', '@dotnet/msbuild', '@dotnet/dnceng', '@dotnet/prodconsvcs')
    $mixedSupportCase = Copy-Value $preflightFindings
    $mixedSupportCase.builds[2].roots[0].ownerTeams = @('1ES SUPPORT')
    Assert-QBInternalFindings -Findings $mixedSupportCase -Evidence $preflight -Date $date -KnownOwnerTeams @('@dotnet/source-build', '@dotnet/msbuild', '@dotnet/dnceng', '@dotnet/prodconsvcs')
    if ($mixedSupportCase.builds[2].roots[0].ownerTeams -cne '1ES support') { throw 'A support name must be normalized to its registered spelling.' }
    $madeUpSupport = Copy-Value $preflightFindings
    $madeUpSupport.builds[2].roots[0].ownerTeams = @('Imaginary support')
    # A name that is neither a team nor a support destination does not fail the report: it is kept as written and a note says so.
    Clear-QBRunNotes
    Assert-QBInternalFindings -Findings $madeUpSupport -Evidence $preflight -Date $date -KnownOwnerTeams @('@dotnet/source-build', '@dotnet/msbuild', '@dotnet/dnceng', '@dotnet/prodconsvcs')
    if (@(Get-QBRunNotes | Where-Object { $_.message -match 'Imaginary support' -and $_.message -match 'kept as written' }).Count -ne 1) { throw 'An unknown owner must be noted, not rejected.' }
    if ($madeUpSupport.builds[2].roots[0].ownerTeams -cne 'Imaginary support') { throw 'An unknown owner must be kept as written.' }

    # An invented handle cannot be pinged, so it must be rejected rather than shipped.
    $knownTeams = @('@dotnet/source-build', '@dotnet/msbuild', '@dotnet/dnceng', '@dotnet/prodconsvcs')
    Assert-QBInternalFindings -Findings $preflightFindings -Evidence $preflight -Date $date -KnownOwnerTeams $knownTeams

    # Regression: Get-Variable -ValueOnly emits an array as ONE object, so @() around it
    # yields a single "System.Object[]" entry and every real owner looks invented.
    # The roster must survive the exact resolution New-Report performs.
    $script:QBKnownOwnerTeams = $knownTeams
    $resolvedVariable = Get-Variable -Name QBKnownOwnerTeams -Scope Script -ErrorAction SilentlyContinue
    $resolved = if ($null -ne $resolvedVariable) { @($resolvedVariable.Value) } else { @() }
    if ($resolved.Count -ne $knownTeams.Count) { throw "Roster resolution lost entries: $($resolved.Count) of $($knownTeams.Count)." }
    if ('@dotnet/source-build' -notin $resolved) { throw 'Roster resolution dropped a real team handle.' }
    Assert-QBInternalFindings -Findings $preflightFindings -Evidence $preflight -Date $date -KnownOwnerTeams $resolved

    # A malformed roster must skip the check, never reject every real owner.
    Assert-QBInternalFindings -Findings $preflightFindings -Evidence $preflight -Date $date -KnownOwnerTeams @('System.Object[]') 3>$null

    # A repo name mistaken for a handle (@dotnet/source-build-assets, @dotnet/efcore) auto-corrects to
    # the real owning team rather than failing the whole report.
    $invented = Copy-Value $preflightFindings
    $invented.builds[2].roots[0].ownerTeams = @('@dotnet/source-build-assets')
    Assert-QBInternalFindings -Findings $invented -Evidence $preflight -Date $date -KnownOwnerTeams $knownTeams 3>$null
    if ($invented.builds[2].roots[0].ownerTeams -notcontains '@dotnet/source-build') {
        throw 'A repo-name handle was not auto-corrected to its owning team.'
    }
    $efcore = Copy-Value $preflightFindings
    $efcore.builds[2].roots[0].ownerTeams = @('@dotnet/efcore')
    # Never fail because of an owner: a handle with no close team is kept as written, each with its own note.
    Clear-QBRunNotes
    Assert-QBInternalFindings -Findings $efcore -Evidence $preflight -Date $date -KnownOwnerTeams $knownTeams
    if (@(Get-QBRunNotes | Where-Object { $_.message -match '@dotnet/efcore' -and $_.message -match 'kept as written' }).Count -ne 1) { throw 'An unmappable handle must be noted, not rejected.' }
    $multi = Copy-Value $preflightFindings
    $multi.builds[0].roots[0].ownerTeams = @('@dotnet/not-a-team')
    $multi.builds[2].roots[0].ownerTeams = @('@dotnet/made-up-team')
    Clear-QBRunNotes
    Assert-QBInternalFindings -Findings $multi -Evidence $preflight -Date $date -KnownOwnerTeams $knownTeams
    if (@(Get-QBRunNotes | Where-Object { $_.message -match 'not-a-team|made-up-team' }).Count -ne 2) { throw 'Every unmappable handle must get its own note.' }
    # A handle that is close to a real team is matched to it instead.
    $close = Copy-Value $preflightFindings
    $close.builds[2].roots[0].ownerTeams = @('@dotnet/msbuild-team')
    Clear-QBRunNotes
    Assert-QBInternalFindings -Findings $close -Evidence $preflight -Date $date -KnownOwnerTeams $knownTeams
    if ($close.builds[2].roots[0].ownerTeams -cne '@dotnet/msbuild' -or @(Get-QBRunNotes | Where-Object { $_.message -match 'closest team @dotnet/msbuild' }).Count -ne 1) { throw 'A close handle must be matched to the closest team, with a note.' }

    # A finding named after its hosting test misroutes the escalation: an installer leg that
    # fails NETSDK1045 is a target-framework failure, not a "dnx regression".
    $misnamedFinding = Copy-Value $preflightFindings
    $misnamedFinding.builds[2].roots[0].id = 'linux-installer-dnx-scenario-failure'
    $misnamedFinding.builds[2].roots[0].description = 'Linux installer dnx scenario regression.'
    $misnamedFinding.builds[2].roots[0].quote = 'error NETSDK1045: The current .NET SDK does not support targeting .NET 12.0'
    Write-Json $failureTasksPath @{ builds = @(@{
        buildId = 4; organization = 'dnceng'; timelineStatus = 'none'
        tasks = @(@{ recordType = 'PipelineValidation'; task = 'Pipeline validation'; result = 'failed'
            issues = @(@{ type = 'error'; message = 'error NETSDK1045: The current .NET SDK does not support targeting .NET 12.0' }) })
    }) }
    Assert-Throws { Assert-QBInternalFindings -Findings $misnamedFinding -Evidence $preflight -Date $date -KnownOwnerTeams $knownTeams } 'not a dnx defect'

    # A correctly named finding may still mention dnx in its description to explain what the
    # failure is NOT; that must not trip the guard.
    $correctlyNamed = Copy-Value $misnamedFinding
    $correctlyNamed.builds[2].roots[0].id = 'netsdk1045-target-framework-failure'
    $correctlyNamed.builds[2].roots[0].description = 'Templates target net12.0; the dnx usage text in the dump is console noise, not the failure.'
    Assert-QBInternalFindings -Findings $correctlyNamed -Evidence $preflight -Date $date -KnownOwnerTeams $knownTeams

    # A bare test identity names the host, not the failure, and cannot establish a root cause.
    $testIdentity = Copy-Value $preflightFindings
    $testIdentity.builds[2].roots[0].quote = 'Microsoft.DotNet.Installer.Tests.LinuxInstallerTests.RpmScenarioTest(repo: "x", tag: "y")'
    Write-Json $failureTasksPath @{ builds = @(@{
        buildId = 4; organization = 'dnceng'; timelineStatus = 'none'
        tasks = @(@{ recordType = 'PipelineValidation'; task = 'Pipeline validation'; result = 'failed'
            issues = @(@{ type = 'error'; message = 'Microsoft.DotNet.Installer.Tests.LinuxInstallerTests.RpmScenarioTest(repo: "x", tag: "y")' }) })
    }) }
    Assert-Throws { Assert-QBInternalFindings -Findings $testIdentity -Evidence $preflight -Date $date -KnownOwnerTeams $knownTeams } 'test identity, not a diagnostic'

    Write-Json $failureTasksPath @{ builds = @() }
    Write-Json $inventoryPath @(
        @{ id = 1; sourceBranch = 'refs/heads/main'; result = 'succeeded'; finishTime = '2026-09-24T01:00:00Z' },
        @{ id = 2; sourceBranch = 'refs/heads/internal/release/10.0.1xx'; result = 'failed'; finishTime = '2026-09-24T02:00:00Z' },
        @{ id = 3; sourceBranch = 'refs/heads/release/11.0.1xx'; result = 'canceled'; finishTime = '2026-09-24T03:00:00Z' }
    )
}
finally {
        foreach ($file in @($inventoryPath, $rootLog, $logsPath, $timelinePath, $failureTasksPath,
        (Join-Path $temporary 'build-health.md'), (Join-Path $temporary 'build-health.md.data.json'),
        (Join-Path $temporary 'overall.md'), (Join-Path $temporary 'overall.md.data.json'),
        (Join-Path $temporary 'public-link-inventory.json'),
        (Join-Path $temporary "copilot-build-health-$date-fixture.jsonl.invalid.md"))) {
        if (Test-Path -LiteralPath $file) { Remove-Item -LiteralPath $file }
    }
    Remove-Item -LiteralPath $temporary
}

# Format differences between models must not cost a report. The structured comment may be on one line or on its own lines,
# and a Teams display name may carry non-ASCII letters (Levé, Přemek Vysoký).
. (Join-Path $PSScriptRoot '..\daily\contract.ps1')
$compactPlan = '{"schemaVersion":1,"reportDate":"2026-10-09","actions":[],"coverageGap":"none"}'
foreach ($shape in @(
        @{ n = 'one line'; text = "Report`n<!-- VMR-QB-ACTIONS $compactPlan VMR-QB-ACTIONS -->`n" },
        @{ n = 'own lines'; text = "Report`n<!-- VMR-QB-ACTIONS`n$compactPlan`nVMR-QB-ACTIONS -->`n" },
        @{ n = 'extra spacing and CRLF'; text = "Report`n<!--  VMR-QB-ACTIONS`r`n  $compactPlan  `r`n  VMR-QB-ACTIONS  -->`r`n" })) {
    $section = Read-QBStructuredSection $shape.text 'VMR-QB-ACTIONS'
    if ($section.Content.Trim() -ne 'Report' -or $section.Data.reportDate -ne '2026-10-09') { throw "The structured comment ($($shape.n)) was not read." }
}
Assert-Throws { Read-QBStructuredSection "Report that only mentions VMR-QB-ACTIONS in prose`n" 'VMR-QB-ACTIONS' } 'exactly one'
Assert-Throws { Read-QBStructuredSection "<!-- VMR-QB-ACTIONS $compactPlan VMR-QB-ACTIONS -->`n<!-- VMR-QB-ACTIONS $compactPlan VMR-QB-ACTIONS -->" 'VMR-QB-ACTIONS' } 'exactly one'
Assert-Throws { Read-QBStructuredSection '<!-- VMR-QB-ACTIONS { nope VMR-QB-ACTIONS -->' 'VMR-QB-ACTIONS' } 'Invalid VMR-QB-ACTIONS JSON'
Assert-Throws { Read-QBStructuredSection '<!-- VMR-QB-ACTIONS {"schemaVersion":2} VMR-QB-ACTIONS -->' 'VMR-QB-ACTIONS' } 'Unsupported'

$overallLink = '[Build 2](https://dev.azure.com/dnceng/internal/_build/results?buildId=2)'
function New-OverallWith([string] $Owners) {
    return @"
# VMR QB Daily Overall Report - 2026-10-09
## Top actions for the QB today
1. $overallLink - Restore could not find the pinned package. Where: Teams - Unified Build Technical and General Discussions.
## Summary
| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |
|---|---|---|---|---|---|
| Restore | $overallLink | N | None found | Where: Teams - Unified Build Technical and General Discussions.<br>Copy/paste: "$Owners, Please check the build failure. Pinned package version not found: $overallLink" | None found |
"@
}
$unicodeContacts = @{
    '@dotnet/source-build' = [pscustomobject]@{ teams = 'Amaury Levé'; github = 'amaury' }
    '@dotnet/prodconsvcs'  = [pscustomobject]@{ teams = 'Přemek Vysoký'; github = 'premek' }
}
Assert-QBOverallReport -Content (New-OverallWith '@dotnet/source-build @Amaury Levé') -TeamContacts $unicodeContacts
Assert-QBOverallReport -Content (New-OverallWith '@dotnet/prodconsvcs @Přemek Vysoký') -TeamContacts $unicodeContacts
Assert-QBOverallReport -Content (New-OverallWith '@dotnet/source-build @Matt Thalman') -TeamContacts @{}
Assert-Throws { Assert-QBOverallReport -Content (New-OverallWith '@dotnet/source-build @Amaury Levé') -TeamContacts @{ '@dotnet/source-build' = [pscustomobject]@{ teams = 'Someone Else'; github = 'x' } } } 'without naming its contact'
Assert-QBOverallReport -Content (New-OverallWith '@dotnet/source-build @Amaury Levé, @dotnet/prodconsvcs @Přemek Vysoký') -TeamContacts $unicodeContacts
Assert-Throws { Assert-QBOverallReport -Content (New-OverallWith '@dotnet/source-build @Amaury Levé Please check') -TeamContacts @{} } 'needs a quoted message'
'PASS: The structured comment may be on one line; display names with non-ASCII letters are valid contacts'

'PASS: internal completeness, evidence quotes, branch tables, action tracking/drafts, assignee verification, and authentication alerts.'
