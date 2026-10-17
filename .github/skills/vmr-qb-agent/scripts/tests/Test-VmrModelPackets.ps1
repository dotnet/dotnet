[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot '..\daily\report.ps1')
$root = Join-Path ([IO.Path]::GetTempPath()) ('vmr-model-packet-' + [guid]::NewGuid().ToString('N'))
[void][IO.Directory]::CreateDirectory($root)
$out = Join-Path $root 'packets'
try {
    $inventory = Join-Path $root 'inventory.json'
    @{ value = @(
        @{ id = 1; result = 'failed'; sourceBranch = 'refs/heads/main'; reason = 'batchedCI'; queueTime = '2026-09-23T01:00:00Z'; finishTime = '2026-09-23T02:00:00Z'; vmrClassification = @{ kind = 'rolling'; complete = $true } }
    ) } | ConvertTo-Json -Depth 20 | Set-Content -LiteralPath $inventory
    $failures = Join-Path $root 'failure-tasks.json'
    $warningTasks = @(1..1000 | ForEach-Object {
        @{ id = "warning-$_"; recordType = 'Task'; task = 'Component Governance'; job = 'linux'; result = 'succeeded'; issues = @(@{ message = ('Warning context ' * 100) }) }
    })
    @{ builds = @(@{
        buildId = 1; organization = 'dnceng'
        tasks = @(@{ id = 'root'; recordType = 'Task'; task = 'Build'; job = 'linux'; result = 'failed'; issues = @(@{ message = 'MSBuild server failed to shut down: directory validation failed.' }) }) + $warningTasks
        cascadeSummaries = @()
    }) } | ConvertTo-Json -Depth 30 | Set-Content -LiteralPath $failures
    $logs = Join-Path $root 'root-logs.json'
    $rootLogPath = Join-Path $root 'root.log'
    @(
        'Initializing source-only toolset...'
        'MSBuild server failed to shut down: directory validation failed.'
        "##[error]Bash exited with code '1'."
    ) | Set-Content -LiteralPath $rootLogPath
    @{ logs = @(@{
        buildId = 1; task = 'Build'; job = 'linux'; result = 'failed'; evidenceKind = 'failure'; available = $true; required = $true
        path = $rootLogPath; logId = 5
        diagnosticExcerpts = @(@{ line = 3; text = "##[error]Bash exited with code '1'." })
        diagnosticExcerpt = "##[error]Bash exited with code '1'."
    }) } | ConvertTo-Json -Depth 20 | Set-Content -LiteralPath $logs
    $evidence = [pscustomobject]@{
        schemaVersion = 2; status = 'complete'; reportDate = '2026-09-23'
        windowStartUtc = '2026-09-22T13:00:00Z'; observationCutoffUtc = '2026-09-23T13:00:00Z'; collectedAtUtc = '2026-09-23T13:01:00Z'
        failureTasksPath = $failures; rootLogsPath = $logs; requiredErrors = @()
        pipelines = @(@{
            name = 'internal'; status = 'available'; inventoryPath = $inventory; activeInventoryPaths = @()
            requiredProblemBuildIds = @(1); counts = @{ completed = 1; succeeded = 0; failed = 1; canceled = 0; partiallySucceeded = 0 }
            error = $null; loginCommand = $null
        })
    }
    $issues = [pscustomobject]@{
        schemaVersion = 1; status = 'complete'; observedAtUtc = '2026-09-23T13:01:00Z'
        issues = @(); assigneeCandidates = @(); fixPullRequests = @(); errors = @()
    }
    $packet = New-QBModelEvidence $evidence $issues $out
    $index = Get-Content -LiteralPath $packet.pipelinePath -Raw | ConvertFrom-Json -Depth 50
    if ($index.pipelines[0].problemBuilds[0].id -ne 1 -or $index.pipelines[0].counts.failed -ne 1) {
        throw 'The compact packet lost required build IDs or inventory counts.'
    }
    $detailPath = $index.pipelines[0].problemBuilds[0].detailPath
    $detail = Get-Content -LiteralPath $detailPath -Raw | ConvertFrom-Json -Depth 50
    if ($detail.tasks.Count -ne 1 -or -not ($detail.logs[0].excerpts[0].text.Contains('directory validation failed'))) {
        throw 'The compact packet lost the initiating diagnostic or retained bulk warning noise.'
    }
    if ((Get-Item -LiteralPath $detailPath).Length -gt 12000 -or (Get-Item -LiteralPath $packet.pipelinePath).Length -gt 12000) {
        throw 'A single-build packet unexpectedly ballooned with raw warning/index data.'
    }
    if ((Get-Item -LiteralPath $failures).Length -lt 1000000) { throw 'The fixture did not exercise a large raw index.' }
    'PASS: compact packets retain exact counts, required builds, classification and diagnostic excerpts without ingesting the raw warning index.'
}
finally {
    foreach ($name in @('inventory.json', 'failure-tasks.json', 'root-logs.json', 'root.log')) {
        $path = Join-Path $root $name
        if (Test-Path -LiteralPath $path) { Remove-Item -LiteralPath $path }
    }
    foreach ($name in @('model-internal-1.json', 'model-pipeline-evidence.json', 'model-issue-evidence.json')) {
        $path = Join-Path $out $name
        if (Test-Path -LiteralPath $path) { Remove-Item -LiteralPath $path }
    }
    if (Test-Path -LiteralPath $out) { Remove-Item -LiteralPath $out }
    Remove-Item -LiteralPath $root
}

# ---------------------------------------------------------------------------------------------
# Severity belongs to each occurrence of a diagnostic (its task failed AND its own job failed), never to
# whichever copy of the text happened to be listed first.
# ---------------------------------------------------------------------------------------------
function New-SeverityFixture([object[]] $Tasks, [string] $Root, [object[]] $PullRequests = @()) {
    [void][IO.Directory]::CreateDirectory($Root)
    $inventory = Join-Path $Root 'inventory.json'
    @{ value = @(@{ id = 1; result = 'failed'; sourceBranch = 'refs/heads/main'; vmrClassification = @{ kind = 'rolling'; complete = $true } }) } |
        ConvertTo-Json -Depth 20 | Set-Content -LiteralPath $inventory
    $tasksPath = Join-Path $Root 'failure-tasks.json'
    @{ builds = @(@{ buildId = 1; organization = 'dnceng'; tasks = $Tasks; cascadeSummaries = @() }) } | ConvertTo-Json -Depth 30 | Set-Content -LiteralPath $tasksPath
    $logsPath = Join-Path $Root 'root-logs.json'
    @{ logs = @() } | ConvertTo-Json | Set-Content -LiteralPath $logsPath
    $evidence = [pscustomobject]@{
        schemaVersion = 2; status = 'complete'; reportDate = '2026-01-15'; windowStartUtc = '2026-01-14T00:00:00Z'
        observationCutoffUtc = '2026-01-15T00:00:00Z'; collectedAtUtc = '2026-01-15T00:00:01Z'
        failureTasksPath = $tasksPath; rootLogsPath = $logsPath; requiredErrors = @()
        pipelines = @(@{ name = 'internal'; status = 'available'; inventoryPath = $inventory; activeInventoryPaths = @()
            requiredProblemBuildIds = @(1); counts = @{ completed = 1; succeeded = 0; failed = 1; canceled = 0; partiallySucceeded = 0 }; error = $null; loginCommand = $null })
    }
    $issues = [pscustomobject]@{ schemaVersion = 1; status = 'complete'; observedAtUtc = '2026-01-15T00:00:02Z'; issues = @(); assigneeCandidates = @(); fixPullRequests = @($PullRequests); errors = @() }
    $packet = New-QBModelEvidence $evidence $issues (Join-Path $Root 'packets')
    $script:LastPacket = $packet
    $index = Get-Content -LiteralPath $packet.pipelinePath -Raw | ConvertFrom-Json -Depth 50
    return $index.pipelines[0].problemBuilds[0]
}
$severityRoot = Join-Path ([IO.Path]::GetTempPath()) ('vmr-model-severity-' + [guid]::NewGuid().ToString('N'))
try {
    $jobOk = @{ id = 'job-ok'; recordType = 'Job'; task = 'Passing job'; job = 'Passing job'; jobId = 'job-ok'; result = 'succeededWithIssues'; issues = @() }
    $jobBad = @{ id = 'job-bad'; recordType = 'Job'; task = 'Failing job'; job = 'Failing job'; jobId = 'job-bad'; result = 'failed'; issues = @() }
    $message = @(@{ message = 'error MSB1234: Shared diagnostic failed' })
    $nonFatal = @{ id = 'nonfatal'; recordType = 'Task'; task = 'Non-fatal task'; job = 'Passing job'; jobId = 'job-ok'; result = 'failed'; issues = $message }
    $fatal = @{ id = 'fatal'; recordType = 'Task'; task = 'Fatal task'; job = 'Failing job'; jobId = 'job-bad'; result = 'failed'; issues = $message }

    foreach ($case in @(@{ name = 'non-fatal first'; tasks = @($jobOk, $nonFatal, $jobBad, $fatal) }, @{ name = 'fatal first'; tasks = @($jobBad, $fatal, $jobOk, $nonFatal) })) {
        $build = New-SeverityFixture -Tasks $case.tasks -Root (Join-Path $severityRoot ($case.name -replace '\W', '-'))
        $terminating = @($build.diagnostics | Where-Object severity -eq 'terminating')
        $nonTerminating = @($build.diagnostics | Where-Object severity -eq 'nonTerminating')
        if ($build.terminatingDiagnosticCount -ne 1 -or $terminating.Count -ne 1) { throw "[$($case.name)] the fatal occurrence was hidden: terminatingDiagnosticCount=$($build.terminatingDiagnosticCount)." }
        if (@($terminating[0].tasks) -notcontains 'Failing job / Fatal task' -or @($terminating[0].tasks) -contains 'Passing job / Non-fatal task') { throw "[$($case.name)] the terminating entry must list only the fatal task." }
        if ($nonTerminating.Count -ne 1 -or @($nonTerminating[0].tasks) -notcontains 'Passing job / Non-fatal task') { throw "[$($case.name)] the non-fatal occurrence must stay non-fatal and visible." }
    }

    # Several non-fatal copies plus one fatal copy: the counts stay separate.
    $second = @{ id = 'nonfatal-2'; recordType = 'Task'; task = 'Other non-fatal task'; job = 'Passing job'; jobId = 'job-ok'; result = 'failed'; issues = $message }
    $build = New-SeverityFixture -Tasks @($jobOk, $nonFatal, $second, $jobBad, $fatal) -Root (Join-Path $severityRoot 'repeated')
    $terminating = @($build.diagnostics | Where-Object severity -eq 'terminating'); $nonTerminating = @($build.diagnostics | Where-Object severity -eq 'nonTerminating')
    if ($terminating[0].occurrences -ne 1 -or $nonTerminating[0].occurrences -ne 2) { throw "Occurrence counts leaked between severities: terminating=$($terminating[0].occurrences) nonTerminating=$($nonTerminating[0].occurrences)." }

    # Two legs sharing a display name are told apart by the job record id, not by name.
    $leg1 = @{ id = 'leg-1'; recordType = 'Job'; task = 'Linux leg'; job = 'Linux leg'; jobId = 'leg-1'; result = 'succeededWithIssues'; issues = @() }
    $leg2 = @{ id = 'leg-2'; recordType = 'Job'; task = 'Linux leg'; job = 'Linux leg'; jobId = 'leg-2'; result = 'failed'; issues = @() }
    $task1 = @{ id = 't1'; recordType = 'Task'; task = 'Build'; job = 'Linux leg'; jobId = 'leg-1'; result = 'failed'; issues = $message }
    $task2 = @{ id = 't2'; recordType = 'Task'; task = 'Build'; job = 'Linux leg'; jobId = 'leg-2'; result = 'failed'; issues = $message }
    $build = New-SeverityFixture -Tasks @($leg1, $task1, $leg2, $task2) -Root (Join-Path $severityRoot 'same-name')
    if ($build.terminatingDiagnosticCount -ne 1 -or @($build.diagnostics).Count -ne 2) { throw "Legs that share a name were conflated: terminating=$($build.terminatingDiagnosticCount), entries=$(@($build.diagnostics).Count)." }
    # Referenced fix PRs reach the model under the collector's own field names: a merged PR stays merged, with its merge
    # time, target branch and the pointer to the retained API response; a closed-unmerged PR is not turned into a merged one.
    $mergedPr = [pscustomobject]@{
        repository = 'dotnet/example'; number = 42; title = 'Fix the thing'; state = 'closed'; url = 'https://github.com/dotnet/example/pull/42'
        merged = $true; mergedAtUtc = '2026-01-14T10:30:00Z'; baseRefName = 'release/10.0'; headRefName = 'topic'; author = 'someone'
        observedAtUtc = '2026-01-15T00:00:01Z'; metadataPath = 'C:\evidence\dotnet-example-pr-42.json'; bodyPath = 'C:\evidence\dotnet-example-pr-42.body.md'
        relationship = 'Referenced candidate fix PR only; applicability, branch inclusion and deployment are unverified.'
    }
    $closedPr = [pscustomobject]@{
        repository = 'dotnet/example'; number = 43; title = 'Abandoned'; state = 'closed'; url = 'https://github.com/dotnet/example/pull/43'
        merged = $false; mergedAtUtc = $null; baseRefName = 'main'; headRefName = 'old'; observedAtUtc = '2026-01-15T00:00:01Z'
        metadataPath = 'C:\evidence\dotnet-example-pr-43.json'
    }
    $build = New-SeverityFixture -Tasks @($jobBad, $fatal) -Root (Join-Path $severityRoot 'prs') -PullRequests @($mergedPr, $closedPr)
    $compact = Get-Content -LiteralPath $script:LastPacket.issuePath -Raw | ConvertFrom-Json -Depth 20
    $byNumber = @{}; foreach ($pr in @($compact.fixPullRequests)) { $byNumber[[int]$pr.number] = $pr }
    $m = $byNumber[42]; $c = $byNumber[43]
    if ($m.merged -ne $true -or ([datetime]$m.mergedAtUtc).ToUniversalTime().ToString('yyyy-MM-ddTHH:mm:ssZ') -ne '2026-01-14T10:30:00Z') { throw "A merged PR lost its merged flag or merge time: merged=$($m.merged) mergedAtUtc=$($m.mergedAtUtc)." }
    if ($m.baseRefName -ne 'release/10.0' -or $m.metadataPath -ne 'C:\evidence\dotnet-example-pr-42.json') { throw "A merged PR lost its target branch or its retained-metadata pointer: base=$($m.baseRefName) metadata=$($m.metadataPath)." }
    if ($c.merged -ne $false -or $null -ne $c.mergedAtUtc -or $c.state -ne 'closed') { throw 'A closed-unmerged PR must stay closed and not merged.' }
    if (@($compact.fixPullRequests[0].PSObject.Properties.Name) -contains 'mergedAt' -or @($compact.fixPullRequests[0].PSObject.Properties.Name) -contains 'path') { throw 'The projection must use the collector''s names (mergedAtUtc, metadataPath), not renamed or invented ones.' }
    'PASS: referenced fix PRs reach the model with merged status, merge time, target branch and the retained metadata location.'
    'PASS: diagnostic severity is decided per occurrence, in either input order, and legs that share a display name are separated by job id.'
}
finally { Remove-Item -LiteralPath $severityRoot -Recurse -Force -ErrorAction SilentlyContinue }
