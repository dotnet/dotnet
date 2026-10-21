[CmdletBinding()]
param()

# Offline smoke test of the evidence collector (daily/evidence.ps1): the four transports it ends in are replaced by a fixture, so no
# network, no sign-in and no change outside this test's own folder under local/ (git-ignored; removed at the end).
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot '..\daily\evidence.ps1')

function Assert-True { param([bool] $Condition, [string] $Message) if (-not $Condition) { throw "Assertion failed: $Message" } }
function Assert-Equal {
    param([AllowNull()][object] $Actual, [AllowNull()][object] $Expected, [string] $Message)
    if ($Actual -cne $Expected) { throw "Assertion failed: $Message (expected '$Expected', got '$Actual')" }
}
function Invoke-TestCase { param([string] $Name, [scriptblock] $Body) & $Body; $script:passed++; "PASS: $Name" }
function Read-TestJson { param([string] $Path) ConvertFrom-Json -InputObject ([IO.File]::ReadAllText($Path)) -Depth 100 -NoEnumerate }
function Test-TreeContains {
    param([string] $Directory, [string] $Pattern)
    @(Get-ChildItem -LiteralPath $Directory -Recurse -File | Where-Object { [IO.File]::ReadAllText($_.FullName) -match $Pattern }).Count -gt 0
}

function New-TestBuild {
    param([long] $Id, [string] $Branch = 'refs/heads/main', [string] $Result = 'succeeded')
    [pscustomobject]@{
        id = $Id; sourceBranch = $Branch; result = $Result; status = 'completed'; finishTime = '2026-09-23T12:00:00Z'
        queueTime = '2026-09-22T14:00:00Z'; definition = @{ id = 1 }; untouchedRawField = @{ nested = 'raw fixture' }
    }
}
function New-TestTask {
    param([string] $Id, [string] $Name = 'Build', [string] $Result = 'failed', [int] $LogId = 2, [string] $Message = 'Fixture failure.')
    [pscustomobject]@{
        id = $Id; name = $Name; type = 'Task'; parentId = 'job'; result = $Result
        log = @{ id = $LogId }; issues = @(@{ type = $(if ($Result -eq 'succeededWithIssues') { 'warning' } else { 'error' }); message = $Message })
    }
}
function New-TestFixture {
    param([hashtable] $Flags = @{})
    $internal = @(1001..1003 | ForEach-Object { New-TestBuild $_ }) + @(
        (New-TestBuild 1100 -Result failed), (New-TestBuild 1101 -Result canceled -Branch 'refs/heads/release/10.0'),
        (New-TestBuild 1102 -Result partiallySucceeded -Branch 'refs/heads/internal/release/10.0'),
        (New-TestBuild 1190 -Result failed -Branch 'refs/heads/experiment'))
    $public = @(2001, 2002 | ForEach-Object { New-TestBuild $_ }) + @(
        (New-TestBuild 2100 -Result failed), (New-TestBuild 2101 -Result canceled -Branch 'refs/pull/41/merge'),
        (New-TestBuild 2190 -Result failed -Branch 'refs/pull/42/merge'))
    if ($Flags['unknownResult']) { $internal[0].PSObject.Properties.Remove('result') }
    if ($Flags['empty']) { $internal = @(); $public = @() }
    @{ flags = $Flags; internal = $internal; public = $public; tokenCalls = 0; calls = [Collections.Generic.List[string]]::new() }
}

# The four transports, replaced after evidence.ps1 is loaded (a function defined later in the same scope wins).
function Start-Sleep { param($Seconds) }
function Get-VmrAzureCliAccessToken {
    param([int] $TimeoutSeconds)
    $script:fixture.tokenCalls++
    if ($script:fixture.flags['tokenFailure']) { throw 'Synthetic token error with SECRET-DO-NOT-LEAK.' }
    return 'synthetic-collector-token'
}
function Invoke-VmrEvidenceNativeCapture {
    param($Command, $TimeoutSeconds)
    if ($Command -notmatch 'repos/dotnet/dotnet/pulls/(\d+)') { throw 'Unexpected native command.' }
    $pr = @{ number = [int]$Matches[1]; title = "Actual fixture PR $($Matches[1])"; user = @{ login = 'fixture-author' }
        base = @{ ref = $(if ($Matches[1] -eq '42') { 'experimental/target' } else { 'main' }) }; author_association = 'CONTRIBUTOR' }
    return @{ exitCode = 0; stderr = ''; stdout = (ConvertTo-Json -InputObject @($pr) -Depth 10) }
}
function Invoke-VmrEvidenceHttpGet {
    param($Uri, $Headers, $OutputPath, $TimeoutSeconds, $MaxBytes)
    $f = $script:fixture
    $u = [uri]$Uri
    $scope = if ($u.AbsolutePath -like '/dnceng/internal/*') { 'internal' } else { 'public' }
    $f.calls.Add($u.PathAndQuery)
    $response = @{ statusCode = 200; contentType = 'application/json'; headers = @{}; content = $null }
    if ($scope -eq 'internal') {
        if ($Headers.Authorization -cne 'Bearer synthetic-collector-token') { throw 'An internal request carried no bearer token.' }
        if ($f.flags['authFailure']) { $response.statusCode = 401; return $response }
    }
    elseif ($Headers.ContainsKey('Authorization')) { throw 'A public request must not carry a token.' }
    if ($u.AbsolutePath -match '/build/definitions$') {
        $body = @{ value = @(@{ id = 1; name = 'dotnet-unified-build' }) }
    }
    elseif ($u.AbsolutePath -match '/build/builds$') {
        if ($u.Query -notmatch 'statusFilter=completed') {
            $body = @{ value = @(
                [pscustomobject]@{ id = 3001; sourceBranch = 'refs/heads/main'; status = 'inProgress'; queueTime = '2026-09-23T12:59:59Z' },
                [pscustomobject]@{ id = 3002; sourceBranch = 'refs/heads/main'; status = 'inProgress'; queueTime = '2026-09-23T13:00:01Z' },
                [pscustomobject]@{ id = 3003; sourceBranch = 'refs/heads/experiment'; status = 'inProgress'; queueTime = '2026-09-22T14:00:00Z' }) }
        }
        else {
            $builds = $f[$scope]
            $paged = $scope -eq 'internal' -and $builds.Count -gt 3
            $second = $u.Query -match 'continuationToken='
            if ($paged -and $second -and $f.flags['inventoryFailure']) { $response.statusCode = 503; return $response }
            $body = @{ value = @(if ($paged -and $second) { $builds | Select-Object -Skip 3 } elseif ($paged) { $builds | Select-Object -First 3 } else { $builds }) }
            if ($paged -and -not $second) { $response.headers['x-ms-continuationtoken'] = 'next&+ token' }
            if ($f.flags['malformedInventory'] -and $scope -eq 'internal') { $body = @{ message = 'not a list' }; $response.headers.Clear() }
        }
    }
    elseif ($u.AbsolutePath -match '/build/builds/(?<id>\d+)/timeline$') {
        $id = [int]$Matches.id
        if ($id -in @(1001, 1002, 1003, 2001, 2002)) { throw "The timeline of the successful build $id must not be read." }
        $records = @([pscustomobject]@{ id = 'job'; name = 'Fixture job'; type = 'Job'; result = 'failed' })
        $records += if ($id -eq 9000) { New-TestTask publish -Name 'Publish assets' -LogId 11 -Message 'Promotion failed.' } else { New-TestTask build -LogId 2 }
        if ($id -eq 1100) {
            $records += (New-TestTask darc -Name 'Publish Using Darc' -LogId 5), (New-TestTask warn -Name 'Component Governance' -Result succeededWithIssues -LogId 17)
        }
        $body = @{ id = '00000000-0000-0000-0000-000000000001'; records = $records }
    }
    elseif ($u.AbsolutePath -match '/build/builds/(?<id>\d+)/logs/(?<log>\d+)$') {
        if ($f.flags['logFailure'] -and $Matches.id -eq '1100' -and $Matches.log -eq '2') { $response.statusCode = 404; return $response }
        $text = "##[error] Fixture root diagnostic for build $($Matches.id), log $($Matches.log).`n"
        if ($Matches.log -eq '5') { $text += "publishing assets: https://dev.azure.com/dnceng/internal/_build/results?view=results&buildId=9000`n" }
        $response.contentType = 'text/plain'
        [IO.File]::WriteAllText($OutputPath, $text)
        return $response
    }
    else { throw "Unexpected fixture request $($u.AbsolutePath)." }
    $response.content = ConvertTo-Json -InputObject $body -Depth 50
    return $response
}

function Invoke-TestCollection {
    param([hashtable] $Flags, [string] $Name)
    $script:fixture = New-TestFixture $Flags
    $path = Invoke-VmrDailyEvidence -ReportDate '2026-09-23' -WindowStart ([DateTimeOffset]'2026-09-22T13:00:00Z') `
        -WindowEnd ([DateTimeOffset]'2026-09-23T13:00:00Z') -OutputDirectory (Join-Path $script:root $Name)
    return Read-TestJson $path
}
function Get-TestPipeline { param([object] $Index, [string] $Name) $Index.pipelines | Where-Object name -eq $Name }

$script:passed = 0
$script:root = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot "..\..\local\evidence-test-$([Guid]::NewGuid().ToString('N'))"))
try {
    [void][IO.Directory]::CreateDirectory((Join-Path $script:root 'baseline'))
    [IO.File]::WriteAllText((Join-Path $script:root 'baseline\keep.txt'), 'another stage wrote this')
    $baseline = Invoke-TestCollection @{} 'baseline'
    $baselineCalls = @($script:fixture.calls)
    Invoke-TestCase 'A complete run: schema-2 index, scoped counts, problem builds only, an existing output folder is not refused' {
        Assert-Equal $baseline.schemaVersion 2 'schema version'
        Assert-Equal $baseline.status 'complete' ('status: ' + (($baseline.requiredErrors | ForEach-Object message) -join '; '))
        Assert-Equal (($baseline.pipelines | ForEach-Object name) -join ',') 'public,internal' 'ordered pipeline records'
        foreach ($field in 'reportDate', 'windowStartUtc', 'observationCutoffUtc', 'collectedAtUtc', 'failureTasksPath', 'rootLogsPath', 'sources', 'requiredErrors') {
            Assert-True ($null -ne $baseline.PSObject.Properties[$field]) "index field $field"
        }
        $internal = Get-TestPipeline $baseline internal
        $public = Get-TestPipeline $baseline public
        foreach ($field in 'status', 'inventoryPath', 'activeInventoryPaths', 'requiredProblemBuildIds', 'counts', 'error', 'loginCommand') {
            Assert-True ($null -ne $internal.PSObject.Properties[$field]) "pipeline field $field"
        }
        Assert-Equal (($internal.counts.PSObject.Properties | ForEach-Object { $_.Value }) -join ',') '6,3,1,1,1' 'internal counts across two inventory pages, experiment left out'
        Assert-Equal (($public.counts.PSObject.Properties | ForEach-Object { $_.Value }) -join ',') '4,2,1,1,0' 'public counts, the PR into an experimental branch left out'
        Assert-Equal ($internal.requiredProblemBuildIds -join ',') '1100,1101,1102' 'internal problem builds'
        Assert-Equal ($public.requiredProblemBuildIds -join ',') '2100,2101' 'public problem builds'
        $pr = ((Read-TestJson $public.inventoryPath).value | Where-Object id -eq 2101).vmrClassification.pullRequest
        Assert-True ($pr.baseRefName -eq 'main' -and $pr.author -eq 'fixture-author' -and $pr.title -eq 'Actual fixture PR 41') 'PR metadata from GitHub'
        Assert-Equal @($baselineCalls | Where-Object { $_ -match '/timeline' }).Count 6 'a timeline for each problem build and the promotion, none for successes'
        $active = Read-TestJson $internal.activeInventoryPaths[0]
        Assert-True ($active.value.Count -eq 1 -and $active.value[0].id -eq 3001) 'only the eligible active build'
        Assert-True ([IO.File]::Exists((Join-Path $script:root 'baseline\keep.txt')) -and -not [IO.File]::Exists((Join-Path $script:root 'baseline\.collector-run'))) 'no claim, nothing removed'
    }
    Invoke-TestCase 'The failing task logs are read whole, a warning is not a failure, and a Darc log leads to its promotion build' {
        $logs = (Read-TestJson $baseline.rootLogsPath).logs
        $build = $logs | Where-Object { $_.actualBuildId -eq 1100 -and $_.logId -eq 2 }
        Assert-True ($build.available -and [IO.File]::Exists($build.path) -and $build.diagnosticExcerpt -match '^L1: ##\[error\] Fixture root diagnostic') 'the failing Build log and its excerpt'
        Assert-True ($build.diagnosticExcerpts -is [array] -and $build.diagnosticExcerpts.Count -eq 1) 'a single excerpt is still an array'
        Assert-True ($build.recordType -eq 'Task' -and $build.result -eq 'failed' -and $build.evidenceKind -eq 'failure' -and $build.organization -eq 'dnceng') 'record and origin of the log'
        Assert-True (@($logs | Where-Object logId -eq 17).Count -eq 0 -and @($baselineCalls | Where-Object { $_ -match '/logs/17' }).Count -eq 0) 'a warning-only task has no log read'
        $child = $logs | Where-Object { $_.actualBuildId -eq 9000 }
        Assert-True ($child.buildId -eq 1100 -and $child.promotionBuildId -eq 9000 -and $child.parentBuildId -eq 1100) 'the promotion is filed under its parent'
        Assert-Equal (Read-TestJson $baseline.failureTasksPath).promotions.Count 1 'the promotion build is listed'
        $timeline = $baseline.sources | Where-Object apiPath -eq 'build/builds/1100/timeline'
        Assert-True ($timeline.organization -eq 'dnceng' -and $timeline.status -eq 'available' -and [IO.File]::Exists($timeline.path)) 'the root timeline is a source'
        Assert-True (-not (Test-TreeContains (Join-Path $script:root 'baseline') 'synthetic-collector-token')) 'the token is not in any artifact'
    }
    Invoke-TestCase 'Internal sign-in fails closed with its fix while public stays available, and no token or token error is written' {
        foreach ($flag in 'authFailure', 'tokenFailure') {
            $index = Invoke-TestCollection @{ $flag = $true } $flag
            $internal = Get-TestPipeline $index internal
            Assert-Equal $internal.status 'authentication_required' "$flag status"
            Assert-Equal $internal.loginCommand 'az login --allow-no-subscriptions' "$flag fix"
            Assert-True ($null -eq $internal.counts.completed -and $null -eq $internal.inventoryPath) "$flag leaves the counts unknown, not zero"
            Assert-True ($internal.error -match 'Microsoft work account') "$flag names the account"
            Assert-Equal $script:fixture.tokenCalls $(if ($flag -eq 'authFailure') { 2 } else { 1 }) "${flag}: a 401 gets one fresh token, a missing token is not asked for again"
            Assert-Equal (Get-TestPipeline $index public).status 'available' "${flag}: public is separate and never substituted"
            Assert-Equal $index.status 'partial' "$flag index status"
            Assert-True (-not (Test-TreeContains (Join-Path $script:root $flag) 'SECRET-DO-NOT-LEAK|synthetic-collector-token')) "$flag leaks nothing"
        }
    }
    Invoke-TestCase 'An unread or unplaceable inventory leaves counts unknown (never zero) and the pipeline partial' {
        foreach ($flag in 'inventoryFailure', 'malformedInventory', 'unknownResult') {
            $index = Invoke-TestCollection @{ $flag = $true } $flag
            $internal = Get-TestPipeline $index internal
            Assert-Equal $index.status 'partial' "$flag index status"
            Assert-Equal $internal.status 'partial' "$flag pipeline status"
            foreach ($count in $internal.counts.PSObject.Properties) { Assert-True ($null -eq $count.Value) "$flag count $($count.Name) is null" }
            Assert-True ($null -eq $internal.inventoryPath -and [IO.File]::Exists($internal.partialInventoryPath)) "$flag keeps only a provisional inventory"
            if ($flag -eq 'inventoryFailure') {
                Assert-Equal @($script:fixture.calls | Where-Object { $_ -match 'continuationToken=' }).Count 3 'a HTTP 503 is tried three times, then it is an error'
            }
        }
    }
    Invoke-TestCase 'A log that cannot be read is a gap, not a healthy build' {
        $index = Invoke-TestCollection @{ logFailure = $true } 'logFailure'
        $internal = Get-TestPipeline $index internal
        Assert-True ($index.status -eq 'partial' -and $internal.status -eq 'partial' -and $internal.counts.completed -eq 6) 'partial evidence with exact counts'
        $log = (Read-TestJson $index.rootLogsPath).logs | Where-Object { $_.actualBuildId -eq 1100 -and $_.logId -eq 2 }
        Assert-True (-not $log.available -and $log.error -match 'HTTP 404' -and $null -eq $log.path) 'the unread log is listed as unavailable'
    }
    Invoke-TestCase 'Proven empty inventories give real zeros' {
        $index = Invoke-TestCollection @{ empty = $true } 'empty'
        Assert-Equal $index.status 'complete' 'complete empty inventory'
        foreach ($pipeline in $index.pipelines) {
            Assert-True ($pipeline.counts.completed -eq 0 -and $pipeline.requiredProblemBuildIds.Count -eq 0 -and [IO.File]::Exists($pipeline.inventoryPath)) "$($pipeline.name) zeros and an inventory file"
        }
        Assert-True ((Read-TestJson $index.failureTasksPath).builds.Count -eq 0 -and (Read-TestJson $index.rootLogsPath).logs.Count -eq 0) 'empty indexes'
    }
    "PASS: $script:passed offline collector tests. No network, sign-in, report, email or change outside local/."
}
finally {
    if ([IO.Directory]::Exists($script:root)) { [IO.Directory]::Delete($script:root, $true) }
}
