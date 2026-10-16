<#
.SYNOPSIS
Offline replay of a retained internal run: timeline completeness, problem records, warning counts and cascades.
.DESCRIPTION
With -EvidenceDirectory a retained run you supply is replayed: a directory holding sources.json, the internal inventory and
each build's retained timeline files. Without it, a small synthetic retained run is generated in a temporary
directory, so the test needs no live artifacts.
#>
[CmdletBinding()]
param(
    [string] $EvidenceDirectory,
    [string] $SummaryPath
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot '..\daily\evidence.ps1')

function Assert-RetainedEvidence {
    param([bool] $Condition, [string] $Message)
    if (-not $Condition) { throw "Retained-evidence regression failed: $Message" }
}

function New-SyntheticRetainedRun {
    <#
    Builds a small retained internal run with the shapes the collector retains: a primary timeline per build,
    successful Tasks that reference (but never need) command-detail timelines, warning-only Tasks, and Job-level
    missing-input cascades that follow a failed or canceled Build step. The four cascade builds (ids 9003-9006) are the
    ones the cascade-omission guard below checks.
    #>
    param([Parameter(Mandatory)][string] $Directory)

    [void][IO.Directory]::CreateDirectory($Directory)
    $encoding = [Text.UTF8Encoding]::new($false)
    function Save-FixtureJson([string] $Path, [object] $Value) {
        [IO.File]::WriteAllText($Path, (ConvertTo-Json -InputObject $Value -Depth 20), $encoding)
    }
    function New-Record([string] $Id, [string] $Type, [string] $Name, [AllowNull()][object] $ParentId, [string] $Result, [hashtable] $Extra = @{}) {
        $record = [ordered]@{ id = $Id; type = $Type; name = $Name; parentId = $ParentId; state = 'completed'
            result = $Result; errorCount = 0; warningCount = 0 }
        foreach ($key in $Extra.Keys) { $record[$key] = $Extra[$key] }
        return $record
    }
    function New-DetailReference([int] $Number) { return @{ id = ('22222222-2222-2222-2222-{0:d12}' -f $Number); changeId = 3 } }

    $mixed = [Collections.Generic.List[object]]::new()
    $mixed.Add((New-Record 'stage-1' 'Stage' 'Build' $null 'failed'))
    $mixed.Add((New-Record 'job-1' 'Job' 'Linux_x64' 'stage-1' 'failed' @{ errorCount = 1; log = @{ id = 10 }
        issues = @(@{ type = 'error'; message = 'Job failed because a Task failed.' }) }))
    $mixed.Add((New-Record 'task-build' 'Task' 'Build' 'job-1' 'failed' @{ errorCount = 1; log = @{ id = 12 }
        startTime = '2026-09-23T10:00:00Z'; finishTime = '2026-09-23T10:01:00Z'
        issues = @(@{ type = 'error'; message = 'error CS0000: Synthetic compiler failure.' }) }))
    for ($i = 1; $i -le 3; $i++) {
        $mixed.Add((New-Record "task-warning-$i" 'Task' 'Component Governance' 'job-1' 'succeeded' @{ warningCount = 2
            log = @{ id = 20 + $i }; details = (New-DetailReference $i)
            issues = @(@{ type = 'warning'; message = 'Nonblocking governance warning.' }) }))
    }
    $mixed.Add((New-Record 'task-with-issues' 'Task' 'Publish' 'job-1' 'succeededWithIssues' @{ warningCount = 1; log = @{ id = 30 }
        issues = @(@{ type = 'warning'; message = 'Nonblocking publish warning.' }) }))
    for ($i = 1; $i -le 5; $i++) {
        $mixed.Add((New-Record "task-ok-$i" 'Task' "Setup step $i" 'job-1' 'succeeded' @{ details = (New-DetailReference (10 + $i)) }))
    }

    $clean = [Collections.Generic.List[object]]::new()
    $clean.Add((New-Record 'stage-1' 'Stage' 'Build' $null 'succeeded'))
    $clean.Add((New-Record 'job-1' 'Job' 'Windows_x64' 'stage-1' 'succeeded'))
    for ($i = 1; $i -le 2; $i++) {
        $clean.Add((New-Record "task-warning-$i" 'Task' 'Component Governance' 'job-1' 'succeeded' @{ warningCount = 1
            log = @{ id = 40 + $i }; details = (New-DetailReference (20 + $i))
            issues = @(@{ type = 'warning'; message = 'Nonblocking governance warning.' }) }))
    }
    for ($i = 1; $i -le 3; $i++) { $clean.Add((New-Record "task-ok-$i" 'Task' "Setup step $i" 'job-1' 'succeeded')) }

    function New-CascadeRecords([int] $Jobs, [int] $PerJob, [string] $RootResult) {
        $records = [Collections.Generic.List[object]]::new()
        $records.Add((New-Record 'stage-1' 'Stage' 'Build' $null $RootResult))
        for ($job = 1; $job -le $Jobs; $job++) {
            $jobId = "cascade-job-$job"
            $records.Add((New-Record $jobId 'Job' "Linux_x64 leg $job" 'stage-1' $RootResult @{
                issues = @(@{ type = 'error'; message = 'Build failed or was canceled in this job.' }) }))
            $records.Add((New-Record "cascade-root-$job" 'Task' "Build (container-$job)" $jobId $RootResult @{
                startTime = '2026-09-23T10:00:00Z'; finishTime = '2026-09-23T10:01:00Z'; log = @{ id = 300 + $job }
                issues = @(@{ type = 'error'; message = 'Immediate bootstrap failure.' }) }))
            for ($i = 1; $i -le $PerJob; $i++) {
                $name = switch ($i % 3) {
                    1 { "Generate SBoM Manifest (container-$job) (1ES PT)" }
                    2 { 'Publish Artifacts (On Failure)' }
                    0 { 'Publish OSX Artifacts (On Failure)' }
                }
                $path = if ($i % 2) { 'C:\agent\_work\1\a\prepublish' } else { 'C:\agent\_work\1\s\src\runtime\artifacts\packages\Release\Shipping' }
                $issues = if ($i % 3 -eq 1) { @(
                    @{ type = 'error'; message = 'Encountered an error in generation workflow' },
                    @{ type = 'error'; message = "Error details: Could not find a part of the path '$path'." },
                    @{ type = 'warning'; message = 'Failed to write workflow results file' },
                    @{ type = 'error'; message = 'SBOM generation failed with exit code 1.' },
                    @{ type = 'error'; message = "The process 'C:\tools\python.exe' failed with exit code 1" }
                ) } else { @(@{ type = 'error'; message = "Path does not exist: $path" }) }
                $records.Add((New-Record "cascade-$job-$i" 'Task' $name $jobId 'succeededWithIssues' @{
                    startTime = ([DateTimeOffset]'2026-09-23T10:02:00Z').AddSeconds($i).ToString('o')
                    finishTime = ([DateTimeOffset]'2026-09-23T10:02:10Z').AddSeconds($i).ToString('o')
                    log = @{ id = 600 + 10 * $job + $i }; issues = $issues }))
            }
        }
        return $records
    }

    $builds = @(
        @{ id = 9001; result = 'failed'; records = $mixed }
        @{ id = 9002; result = 'succeeded'; records = $clean }
        @{ id = 9003; result = 'failed'; records = (New-CascadeRecords -Jobs 2 -PerJob 7 -RootResult 'failed') }
        @{ id = 9004; result = 'failed'; records = (New-CascadeRecords -Jobs 3 -PerJob 6 -RootResult 'failed') }
        @{ id = 9005; result = 'canceled'; records = (New-CascadeRecords -Jobs 1 -PerJob 14 -RootResult 'canceled') }
        @{ id = 9006; result = 'canceled'; records = (New-CascadeRecords -Jobs 2 -PerJob 9 -RootResult 'canceled') }
    )
    foreach ($build in $builds) {
        Save-FixtureJson (Join-Path $Directory "internal-build-$($build.id)-timeline.json") @{
            id = [Guid]::NewGuid().ToString(); records = @($build.records) }
    }
    $inventory = Join-Path $Directory 'inventory.json'
    Save-FixtureJson $inventory @{ value = @($builds | ForEach-Object { @{ id = $_.id; result = $_.result } }) }
    Save-FixtureJson (Join-Path $Directory 'sources.json') @{
        pipelines = @(@{ name = 'internal'; inventoryPath = $inventory; counts = @{ completed = $builds.Count }
            requiredProblemBuildIds = @($builds | Where-Object { $_.result -ne 'succeeded' } | ForEach-Object { $_.id }) })
    }
    return $Directory
}

$syntheticRoot = $null
if (-not $EvidenceDirectory) {
    $syntheticRoot = Join-Path ([IO.Path]::GetTempPath()) "vmr-retained-fixture-$([Guid]::NewGuid().ToString('N'))"
    try { $EvidenceDirectory = New-SyntheticRetainedRun -Directory $syntheticRoot }
    catch { Remove-Item -LiteralPath $syntheticRoot -Recurse -Force -ErrorAction SilentlyContinue; throw }
}

if (-not [IO.Path]::IsPathFullyQualified($EvidenceDirectory) -or -not [IO.Directory]::Exists($EvidenceDirectory)) {
    throw 'EvidenceDirectory must be the absolute path of an existing retained run.'
}
if ($SummaryPath -and (-not [IO.Path]::IsPathFullyQualified($SummaryPath) -or [IO.File]::Exists($SummaryPath))) {
    throw 'SummaryPath must be absolute and unused; retained evidence is never overwritten.'
}
$index = ConvertFrom-Json -InputObject ([IO.File]::ReadAllText((Join-Path $EvidenceDirectory 'sources.json'))) -Depth 100
$internal = $index.pipelines | Where-Object name -eq internal
$inventory = ConvertFrom-Json -InputObject ([IO.File]::ReadAllText($internal.inventoryPath)) -Depth 100
$temporary = Join-Path ([IO.Path]::GetTempPath()) "vmr-retained-evidence-$([Guid]::NewGuid().ToString('N'))"
$directory = Initialize-VmrEvidenceDirectory -OutputDirectory $temporary
$requests = [Collections.Generic.List[string]]::new()
$http = {
    param($url, $headers, $path, $timeout, $maxBytes)
    $uri = [uri]$url
    if ($uri.AbsolutePath -notmatch '^/dnceng/internal/_apis/build/builds/(?<id>\d+)/timeline(?:/(?<part>[0-9a-f-]+))?$') {
        throw (New-VmrCollectorException -Message 'Offline replay refused a non-timeline request.')
    }
    $suffix = if ($Matches.ContainsKey('part')) { "-$($Matches.part)" } else { '' }
    $file = Join-Path $EvidenceDirectory "internal-build-$($Matches.id)-timeline$suffix.json"
    if (-not [IO.File]::Exists($file)) {
        throw (New-VmrCollectorException -Message 'A required timeline part is not retained; offline replay cannot fetch it.')
    }
    $requests.Add($uri.AbsolutePath)
    return @{ statusCode = 200; headers = @{}; contentType = 'application/json'; content = [IO.File]::ReadAllText($file) }
}.GetNewClosure()
$session = New-VmrReadOnlySession -HttpGet $http -TokenProvider { param($timeout) 'offline-retained-fixture' } `
    -GitHubGet { throw 'Offline replay never queries GitHub.' }
$state = @{
    directory = $directory; session = $session
    limits = @{ maxTimelinePartsPerBuild = 16 }
    sources = [Collections.Generic.List[object]]::new()
    errors = [Collections.Generic.List[object]]::new()
    optionalErrors = [Collections.Generic.List[object]]::new()
    timelineCache = @{}
}
$rows = [Collections.Generic.List[object]]::new()
$retainedDetails = [Collections.Generic.List[object]]::new()
try {
    foreach ($build in $inventory.value) {
        $rootPath = Join-Path $EvidenceDirectory "internal-build-$($build.id)-timeline.json"
        $root = ConvertFrom-Json -InputObject ([IO.File]::ReadAllText($rootPath)) -Depth 100
        $before = $requests.Count
        $timeline = Get-VmrFullEvidenceTimeline -State $state -Scope internal -Organization dnceng -Project internal `
            -BuildId $build.id -Prefix "internal-build-$($build.id)"
        Assert-RetainedEvidence ($timeline.status -eq 'available') "Build $($build.id) has a genuine unresolved timeline gap."
        $expected = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
        foreach ($record in $root.records) { [void]$expected.Add([string]$record.id) }
        $actual = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
        foreach ($record in $timeline.records) { [void]$actual.Add([string]$record.id) }
        Assert-RetainedEvidence ($expected.SetEquals($actual)) "Build $($build.id) lost or duplicated primary pipeline records."
        Assert-RetainedEvidence ($timeline.coverage.missingParentIds.Count -eq 0) "Build $($build.id) has a missing Job/Task ancestor."
        Assert-RetainedEvidence ($requests.Count - $before -eq 1) "Build $($build.id) unnecessarily expanded successful Task command details."
        $tasks = @(Get-VmrEvidenceProblemTasks $timeline.records)
        $steps = @(Get-VmrEvidenceProblemTasks $timeline.records -IncludeBuildSteps |
            Where-Object { $_.recordType -eq 'Task' -and $_.task -match '^Build(?:\s+\(.+\))?$' })
        $selection = Get-VmrRootLogSelection -Tasks $tasks -MaxLogs 12 -Required ($build.result -ne 'succeeded') -MaxWarningLogs 1 `
            -BuildSteps $steps -FullTimelineAvailable
        $represented = @(@($selection.selected) + @($selection.omitted) + @($selection.cascades) | ForEach-Object { $_.task.id })
        foreach ($task in $tasks | Where-Object recordType -eq Task) {
            $diagnostics = Get-VmrRecordDiagnostics $task
            if ($diagnostics.failure) {
                Assert-RetainedEvidence ($task.id -in $represented) "Build $($build.id) lost a failed/canceled/error Task."
            }
            elseif ($task.result -eq 'succeeded') {
                Assert-RetainedEvidence (@($selection.omitted | Where-Object { $_.task.id -eq $task.id -and $_.required }).Count -eq 0) `
                    "Build $($build.id) classified a successful warning as a missing root log."
            }
        }
        foreach ($cascade in $selection.cascades) {
            $rootStep = $steps | Where-Object id -eq $cascade.cascade.rootRecordId
            Assert-RetainedEvidence ($rootStep.result -in @('failed','canceled') -and
                $rootStep.jobId -eq $cascade.task.jobId -and -not $cascade.required -and
                (Get-VmrCascadeTimestamp $rootStep.finishTime) -le (Get-VmrCascadeTimestamp $cascade.task.startTime)) `
                "Build $($build.id) summarized a cascade without same-job preceding root evidence."
        }
        $missingLogs = @($selection.selected | Where-Object {
            $_.required -and -not [IO.File]::Exists((Join-Path $EvidenceDirectory "internal-build-$($build.id)-log-$($_.task.log.id).log"))
        } | ForEach-Object { @{ recordId = $_.task.id; task = $_.task.task; logId = $_.task.log.id } })
        $rows.Add(@{
            buildId = $build.id; result = $build.result; timelineStatus = $timeline.status
            primaryRecords = $root.records.Count; retainedRecords = $timeline.records.Count
            jobs = $timeline.coverage.retainedJobCount; tasks = $timeline.coverage.retainedTaskCount
            optionalTaskDetails = $timeline.coverage.optionalTaskDetails.Count
            problemRecords = $tasks.Count
            failedRecords = @($tasks | Where-Object result -eq failed).Count
            succeededWarningTasks = @($tasks | Where-Object { $_.recordType -eq 'Task' -and $_.result -eq 'succeeded' -and -not (Get-VmrRecordDiagnostics $_).failure }).Count
            warningTaskCount = $selection.warningSummary.taskCount; taskWarningCount = $selection.warningSummary.taskWarningCount
            warningSamples = $selection.warningSelected.Count; expectedUnsampledWarnings = $selection.warningSummary.logSampling.notSampled
            selectedFailureLogs = $selection.selected.Count
            summarizedCascades = $selection.cascades.Count
            cascadeSummaries = @($selection.cascades | ForEach-Object { @{ recordId = $_.task.id; task = $_.task.task; cascade = $_.cascade } })
            genuineRequiredLogOmissions = @($selection.omitted | Where-Object required).Count
            requiredLogsNotRetained = $missingLogs
        })
        foreach ($file in [IO.Directory]::EnumerateFiles($EvidenceDirectory, "internal-build-$($build.id)-timeline-*.json")) {
            $detail = ConvertFrom-Json -InputObject ([IO.File]::ReadAllText($file)) -Depth 100
            foreach ($record in $detail.records) {
                $retainedDetails.Add(@{ type = $record.type; result = $record.result })
            }
        }
        $state.timelineCache.Clear()
    }
    $affected = @($rows | Where-Object { $_.buildId -in @(9003,9004,9005,9006) })
    if ($affected.Count) {
        Assert-RetainedEvidence (@($affected | Where-Object genuineRequiredLogOmissions -gt 0).Count -eq 0) `
            'Cascade-heavy builds must not report false required-log omissions under the unchanged 12-log cap.'
    }
    $summary = @{
        offline = $true; networkRequests = 0; internalBuilds = $rows.Count
        completedInventoryCount = $internal.counts.completed; requiredProblemBuildIds = $internal.requiredProblemBuildIds
        completePrimaryTimelines = @($rows | Where-Object timelineStatus -eq available).Count
        replayedTimelineRequests = $requests.Count
        primaryRecordsPreserved = [long](($rows | ForEach-Object primaryRecords | Measure-Object -Sum).Sum)
        optionalSuccessfulTaskDetails = [long](($rows | ForEach-Object optionalTaskDetails | Measure-Object -Sum).Sum)
        succeededWarningTasksPreserved = [long](($rows | ForEach-Object succeededWarningTasks | Measure-Object -Sum).Sum)
        warningSamplesPlanned = [long](($rows | ForEach-Object warningSamples | Measure-Object -Sum).Sum)
        falseWarningRootLimitErrors = 0; requiredTimelineErrors = $state.errors.Count
        summarizedMissingInputCascades = [long](($rows | ForEach-Object summarizedCascades | Measure-Object -Sum).Sum)
        originalRootLogCap = 12; targetedCascadeBuilds = $affected.Count
        retainedDetailShapes = @($retainedDetails | Group-Object type, result | ForEach-Object { @{ shape = $_.Name; count = $_.Count } })
        genuineRequiredLogOmissions = [long](($rows | ForEach-Object genuineRequiredLogOmissions | Measure-Object -Sum).Sum)
        requiredLogFilesNotRetained = @($rows | ForEach-Object requiredLogsNotRetained).Count
        builds = $rows.ToArray()
    }
    if ($SummaryPath) { Write-VmrEvidenceJson -Path $SummaryPath -Value $summary }
    $console = @{}
    foreach ($key in $summary.Keys) { if ($key -ne 'builds') { $console[$key] = $summary[$key] } }
    ConvertTo-Json -InputObject $console -Depth 8
    "PASS: all $($rows.Count) $(if ($syntheticRoot) { 'synthetic' } else { 'saved' }) internal primary timelines, problem records and warning counts replayed without network access."
}
finally {
    $session.tokens.Clear()
    $session.authenticationFailures.Clear()
    if ([IO.Directory]::Exists($temporary)) { [IO.Directory]::Delete($temporary, $true) }
    if ($syntheticRoot -and [IO.Directory]::Exists($syntheticRoot)) { [IO.Directory]::Delete($syntheticRoot, $true) }
}
