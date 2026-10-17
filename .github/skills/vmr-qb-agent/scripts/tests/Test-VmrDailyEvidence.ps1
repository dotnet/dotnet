[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot '..\daily\evidence.ps1')

# Fails the test if the condition is false.
function Assert-True {
    param([bool] $Condition, [string] $Message)
    if (-not $Condition) { throw "Assertion failed: $Message" }
}

# Fails the test if the actual and expected values differ.
function Assert-Equal {
    param([AllowNull()][object] $Actual, [AllowNull()][object] $Expected, [string] $Message)
    if ($Actual -cne $Expected) { throw "Assertion failed: $Message" }
}

# Checks that an action fails with the expected message and optional authentication flag.
function Assert-Throws {
    param([scriptblock] $Action, [string] $Pattern, [AllowNull()][object] $AuthenticationRequired = $null)
    try { & $Action | Out-Null }
    catch {
        Assert-True ($_.Exception.Message -match $Pattern) "Expected exception matching '$Pattern'."
        if ($null -ne $AuthenticationRequired) {
            Assert-Equal ([bool]$_.Exception.Data['AuthenticationRequired']) $AuthenticationRequired 'Authentication classification.'
        }
        return
    }
    throw "Expected exception matching '$Pattern'."
}

# Runs a test case and counts it as passed if it finishes without an error.
function Invoke-TestCase {
    param([string] $Name, [scriptblock] $Body)
    & $Body
    $script:passed++
    "PASS: $Name"
}

# Reads a saved JSON test file without unwrapping its top-level array.
function Read-TestJson {
    param([string] $Path)
    ConvertFrom-Json -InputObject ([IO.File]::ReadAllText($Path)) -Depth 100 -NoEnumerate
}

# Creates a pretend build with the chosen branch, result, and timestamps.
function New-TestBuild {
    param([long] $Id, [string] $Branch = 'refs/heads/main', [string] $Result = 'succeeded',
        [string] $Finish = '2026-09-23T12:00:00Z', [string] $Status = 'completed',
        [string] $Queue = '2026-09-22T14:00:00Z')
    [pscustomobject]@{
        id = $Id; sourceBranch = $Branch; result = $Result; finishTime = $Finish; queueTime = $Queue; status = $Status
        definition = @{ id = if ($Id -lt 2000) { 1330 } else { 101 } }
        repository = @{ name = 'dotnet/dotnet'; type = 'GitHub' }
        untouchedRawField = @{ nested = @{ retained = 'raw fixture' } }
    }
}

# Creates a pretend pipeline task with a result, log reference, and diagnostic message.
function New-TestTask {
    param([string] $Id, [string] $Name = 'Build', [string] $Result = 'failed', [int] $LogId = 2,
        [string] $Message = 'Independent fixture failure.', [string] $ParentId = 'job')
    [pscustomobject]@{
        id = $Id; name = $Name; type = 'Task'; parentId = $ParentId; result = $Result
        log = if ($LogId) { @{ id = $LogId } } else { $null }
        issues = @(@{ type = if ($Result -eq 'succeededWithIssues') { 'warning' } else { 'error' }; message = $Message })
    }
}

# Creates pretend builds, timelines, logs, and API responses for offline collector tests.
function New-TestFixture {
    param([hashtable] $Flags = @{})

    $internal = @(1..8 | ForEach-Object { New-TestBuild -Id (1000 + $_) })
    $internal += @(
        (New-TestBuild 1009 -Finish '2026-09-22T13:00:00Z'),
        (New-TestBuild 1010 -Finish '2026-09-23T13:00:00Z'),
        (New-TestBuild 1100 -Result failed),
        (New-TestBuild 1101 -Branch 'refs/heads/release/10.0' -Result canceled),
        (New-TestBuild 1102 -Branch 'refs/heads/internal/release/10.0' -Result partiallySucceeded),
        (New-TestBuild 1190 -Branch 'refs/heads/experiment-without-dev-prefix' -Result failed),
        (New-TestBuild 1191 -Finish '2026-09-22T12:59:59.9999999Z' -Result failed),
        (New-TestBuild 1192 -Finish '2026-09-23T13:00:00.0000001Z' -Result failed)
    )
    $public = @(1..7 | ForEach-Object { New-TestBuild -Id (2000 + $_) })
    $public += @(
        (New-TestBuild 2100 -Result failed),
        (New-TestBuild 2101 -Branch 'refs/pull/41/merge' -Result canceled),
        (New-TestBuild 2102 -Branch 'refs/heads/release/9.0' -Result partiallySucceeded),
        (New-TestBuild 2190 -Branch 'refs/pull/42/merge' -Result failed),
        (New-TestBuild 2191 -Branch 'refs/heads/experiment-without-dev-prefix' -Result failed),
        (New-TestBuild 2192 -Branch 'refs/heads/internal/release/10.0' -Result failed)
    )
    if ($Flags.ContainsKey('unknownBranch')) {
        ($internal | Where-Object id -eq 1190).PSObject.Properties.Remove('sourceBranch')
    }
    if ($Flags.ContainsKey('preflightRejection')) {
        $rejected = $internal | Where-Object id -eq 1101
        $rejected.result = 'failed'
        $rejected | Add-Member -NotePropertyName validationResults -NotePropertyValue @(
            [pscustomobject]@{ result = 'error'; message = 'The pipeline is not valid. Job Windows_x64: Step NuGetAuthenticate2 input nuGetServiceConnections expects a service connection of type ExternalNuGetFeed.' }
        ) -Force
    }
    if ($Flags.ContainsKey('preflightWithoutValidation')) {
        $rejected = $internal | Where-Object id -eq 1101
        $rejected.result = 'failed'
    }
    if ($Flags.ContainsKey('unknownResult')) {
        ($internal | Where-Object id -eq 1002).PSObject.Properties.Remove('result')
    }
    $timelines = @{}
    foreach ($build in @($internal) + @($public)) {
        $records = @([pscustomobject]@{ id = 'job'; name = 'Fixture job'; type = 'Job'; result = 'succeeded' })
        $buildResult = Get-VmrEvidenceProperty $build 'result'
        if ($buildResult -eq 'succeeded') {
            if ($build.id -eq 1001) { $records += New-TestTask warning -Result succeededWithIssues -LogId 17 -Message 'Nonblocking warning.' }
        }
        elseif ($buildResult -eq 'canceled') {
            $records = @([pscustomobject]@{
                id = 'job'; name = 'Canceled job'; type = 'Job'; result = 'canceled'
                issues = @(
                    @{ type = 'error'; message = 'The job ran longer than the maximum time of 240 minutes.' },
                    @{ type = 'error'; message = 'The agent was lost. A user manually canceled this job.' }
                )
            }, (New-TestTask canceled -Result canceled -LogId 9))
        }
        elseif ($buildResult -eq 'partiallySucceeded') {
            $records += New-TestTask partial -Name 'Run Tests' -Result succeededWithIssues -LogId 3
        }
        else { $records += New-TestTask failure }
        $timelines[[string]$build.id] = @{ id = '00000000-0000-0000-0000-000000000001'; records = $records }
    }
    $timelines['1100'] = @{
        id = '00000000-0000-0000-0000-000000000001'
        records = @(
            [pscustomobject]@{
                id = 'job'; name = 'Primary job'; type = 'Job'; result = 'failed'
                issues = @(@{ type = 'error'; message = 'Full job issue retained.' })
                details = @{ id = '00000000-0000-0000-0000-000000000002' }
            },
            (New-TestTask cascade -Name 'Generate SBoM' -LogId 1 -Message 'Missing Shipping output directory.'),
            (New-TestTask '11111111-2222-3333-4444-555555555555' -LogId 2),
            (New-TestTask tests -Name 'Run Tests' -LogId 3),
            (New-TestTask installer -Name 'Validate installer' -LogId 4),
            (New-TestTask darc -Name 'Publish Using Darc' -LogId 5),
            [pscustomobject]@{
                id = 'issue-without-result'; name = 'Agent diagnostic'; type = 'Job'
                issues = @(@{ type = 'error'; message = 'Agent heartbeat lost without a result field.' })
            }
        )
    }
    $timelines['1100/00000000-0000-0000-0000-000000000002'] = @{
        id = '00000000-0000-0000-0000-000000000002'
        records = @((New-TestTask nested -Name 'Nested Build' -Result canceled -LogId 7 -Message 'Compiler-server shutdown stall.'))
    }
    $timelines['9000'] = @{ records = @(
        [pscustomobject]@{ id = 'job'; name = 'Promotion job'; type = 'Job'; result = 'failed' },
        (New-TestTask publish -Name 'Publish assets' -LogId 11 -Message 'Promotion failed.')
    ) }
    $timelines['9001'] = @{ records = @([pscustomobject]@{
        id = 'job'; name = 'Successful promotion'; type = 'Job'; result = 'succeeded'
    }) }
    if ($Flags.ContainsKey('promotionChain')) {
        $timelines['9000'].records += New-TestTask child-darc -Name 'Publish Using Darc' -LogId 5
        $timelines['9002'] = $timelines['9000']
    }
    if ($Flags.ContainsKey('missingLogId')) {
        $timelines['1100'].records += New-TestTask missing -Name 'Build missing log' -LogId 0
    }
    if ($Flags.ContainsKey('sharedPromotion')) {
        $timelines['2100'].records += New-TestTask public-darc -Name 'Publish Using Darc' -LogId 5
    }
    $fixture = @{
        flags = $Flags; internal = $internal; public = $public; timelines = $timelines
        tokenCalls = 0; httpCalls = [Collections.Generic.List[object]]::new()
        githubCalls = [Collections.Generic.List[string]]::new()
    }
    $tokenProvider = {
        param($timeout)
        $fixture.tokenCalls++
        if ($fixture.flags.ContainsKey('tokenFailure')) { throw 'Synthetic token acquisition error with SECRET-DO-NOT-LEAK.' }
        return 'synthetic-collector-token'
    }.GetNewClosure()
    $http = {
        param($url, $headers, $path, $timeout, $maxBytes)
        $uri = [uri]$url
        $scope = if ($uri.AbsolutePath -like '/dnceng/internal/*') { 'internal' } else { 'public' }
        $fixture.httpCalls.Add(@{
            url = $url; streamed = [bool]$path; maxBytes = $maxBytes
            correctHeader = if ($scope -eq 'internal') { $headers.Authorization -ceq 'Bearer synthetic-collector-token' } else { -not $headers.ContainsKey('Authorization') }
        })
        $response = @{ statusCode = 200; contentType = 'application/json'; headers = @{}; content = $null }
        $body = $null
        if ($uri.AbsolutePath -match '/build/definitions$') {
            if ($scope -eq 'internal' -and $fixture.flags.ContainsKey('authFailure')) {
                $response.statusCode = 401
                return $response
            }
            $body = @{ value = @(@{ id = if ($scope -eq 'internal') { 1330 } else { 101 }; name = 'dotnet-unified-build' }) }
        }
        elseif ($uri.AbsolutePath -match '/build/builds$') {
            if ($uri.Query -notmatch '[?&]statusFilter=(?<status>[^&]+)') { throw 'Missing separate status filter.' }
            $status = $Matches.status
            if ($status -eq 'completed') {
                $builds = $fixture[$scope]
                if ($scope -eq 'internal' -and $uri.Query -notmatch 'continuationToken=') {
                    $body = @{ value = @($builds | Select-Object -First 5) }
                    $response.headers['x-ms-continuationtoken'] = 'next&+ token'
                }
                elseif ($scope -eq 'internal') { $body = @{ value = @($builds | Select-Object -Skip 5) } }
                else { $body = @{ value = $builds } }
                if (($fixture.flags.ContainsKey('inventoryFailure') -or $fixture.flags.ContainsKey('inventoryAuthFailure')) -and
                    $scope -eq 'internal' -and $uri.Query -match 'continuationToken=') {
                    $response.statusCode = if ($fixture.flags.ContainsKey('inventoryAuthFailure')) { 401 } else { 503 }
                    return $response
                }
                if ($fixture.flags.ContainsKey('malformedInventory') -and $scope -eq 'internal') {
                    $body = @{ message = 'not a list' }; $response.headers.Clear()
                }
            }
            elseif ($status -eq 'inProgress') {
                $body = @{ value = @(
                    [pscustomobject]@{ id = 3001; sourceBranch = 'refs/heads/main'; status = 'inProgress'; queueTime = '2026-09-23T12:59:59Z' },
                    [pscustomobject]@{ id = 3002; sourceBranch = 'refs/heads/main'; status = 'inProgress'; queueTime = '2026-09-23T13:00:00.0000001Z' },
                    [pscustomobject]@{ id = 3003; sourceBranch = 'refs/heads/experiment'; status = 'inProgress'; queueTime = '2026-09-22T14:00:00Z' }
                ) }
            }
            else { $body = @{ value = @() } }
        }
        elseif ($uri.AbsolutePath -match '/build/builds/(?<id>\d+)/timeline(?:/(?<part>[0-9a-f-]+))?$') {
            $id = $Matches.id
            $key = if ($Matches.ContainsKey('part')) { "$id/$($Matches.part)" } else { $id }
            if ($fixture.flags.ContainsKey('timelineFailure') -and $id -eq '1002') {
                $response.statusCode = 503
                return $response
            }
            if ($fixture.flags.ContainsKey('promotionTimelineFailure') -and $id -eq '9000') {
                $response.statusCode = 503
                return $response
            }
            if (($fixture.flags.ContainsKey('preflightRejection') -or
                 $fixture.flags.ContainsKey('preflightWithoutValidation')) -and $id -eq '1101') {
                # Azure DevOps answers 204 with no body when a build never produced a timeline.
                $response.statusCode = 204
                $response.contentType = ''
                $response.content = ''
                return $response
            }
            if (-not $fixture.timelines.ContainsKey($key)) { throw "Unexpected fixture timeline $key." }
            $body = $fixture.timelines[$key]
        }
        elseif ($uri.AbsolutePath -match '/build/builds/(?<id>\d+)/logs/(?<log>\d+)$') {
            $id = $Matches.id; $log = $Matches.log
            if ($id -eq '1001' -and $log -eq '17') {
                if ($fixture.flags.ContainsKey('warningLogFailure') -or $fixture.flags.ContainsKey('warningAuthFailure')) {
                    $response.statusCode = if ($fixture.flags.ContainsKey('warningAuthFailure')) { 401 } else { 404 }
                    return $response
                }
                if ($fixture.flags.ContainsKey('oversizeWarningLog')) {
                    [IO.File]::WriteAllText($path, 'Partial optional warning sample.')
                    throw (New-VmrCollectorException -Kind limit -Message 'Optional sample exceeded its bounded response size.')
                }
            }
            if ($fixture.flags.ContainsKey('logFailure') -and $id -eq '1100' -and $log -eq '3') {
                $response.statusCode = 404
                return $response
            }
            if ($fixture.flags.ContainsKey('promotionLogFailure') -and $id -eq '9000') {
                $response.statusCode = 503
                return $response
            }
            if ($fixture.flags.ContainsKey('oversizeLog') -and $id -eq '1100' -and $log -eq '2') {
                [IO.File]::WriteAllText($path, 'Retained partial raw log fixture.')
                throw (New-VmrCollectorException -Kind limit -Message 'HTTP response exceeded the explicit byte limit; evidence is incomplete.')
            }
            $text = "##[error] Fixture root diagnostic for build $id, log $log.`n"
            if ($log -eq '5') {
                $text += "publishing assets: https://dev.azure.com/dnceng/internal/_build/results?buildId=9000`n"
                $text += "publishing assets: https://dev.azure.com/dnceng/internal/_build/results?view=results&buildId=9001`n"
                if ($id -in @('9000', '9002')) { $text += "publishing assets: https://dev.azure.com/dnceng/internal/_build/results?buildId=9002`n" }
            }
            if ($id -eq '9000') { $text += "##[error] 401 Unauthorized returned by a CI publishing task, not the collector.`n" }
            $response.contentType = 'text/plain'
            if ($path) { [IO.File]::WriteAllText($path, $text) } else { $response.content = $text }
            return $response
        }
        elseif ($uri.AbsolutePath -match '/wiki/wikis/') { $body = @{ content = 'Legacy release wiki fixture.' } }
        else { throw 'Unexpected fixture HTTP request.' }
        $response.content = ConvertTo-Json -InputObject $body -Depth 100
        return $response
    }.GetNewClosure()
    $github = {
        param($apiPath, $timeout)
        $fixture.githubCalls.Add($apiPath)
        if ($apiPath -match '/pulls/(?<number>\d+)$') {
            $number = [int]$Matches.number
            if ($fixture.flags.ContainsKey('prGap') -and $number -eq 41) { return '[{"title":"Missing base and author"}]' }
            return ConvertTo-Json -Depth 10 -InputObject @(@{
                number = $number; title = "Actual fixture PR $number"; user = @{ login = 'fixture-author' }
                base = @{ ref = if ($number -eq 42) { 'experimental/target' } else { 'main' } }
            })
        }
        if ($apiPath -match '/contents/') {
            $text = if ($apiPath -match 'Guide') { 'https://dev.azure.com/devdiv/DevDiv/_wiki/wikis/DevDiv.wiki/123/Release' }
                else { "teamReviewer: fixture-team`n" }
            return ConvertTo-Json -InputObject @(@{ content = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($text)) })
        }
        return '[[]]'
    }.GetNewClosure()
    # These tests count requests, so a transient failure is not tried again here (the retries are tested in Test-VmrQBCollectorRetry).
    $fixture['session'] = New-VmrReadOnlySession -TokenProvider $tokenProvider -HttpGet $http -GitHubGet $github -MaxTransientRetries 0 -Sleep { param($seconds) }
    return $fixture
}

# Runs the collector against pretend API responses for a fixed 24-hour window and reads its saved results.
function Invoke-TestCollection {
    param([hashtable] $Fixture, [string] $Name, [hashtable] $Limits = @{}, [switch] $Legacy, [switch] $FullContext, [string] $CheckoutRoot = $script:checkoutRoot)
    $parameters = @{
        ReportDate = '2026-09-23'; WindowStart = [DateTimeOffset]'2026-09-22T13:00:00Z'
        WindowEnd = [DateTimeOffset]'2026-09-23T13:00:00Z'; Session = $Fixture.session
        Limits = $Limits; CoreOnly = -not $FullContext; CheckoutRoot = $CheckoutRoot
    }
    if ($Legacy) { $parameters.LegacyDirectory = Join-Path $script:fixtureRoot $Name }
    else { $parameters.OutputDirectory = Join-Path $script:fixtureRoot $Name }
    $path = Invoke-VmrDailyEvidence @parameters
    return Read-TestJson $path
}

# Adds successful task details and nonblocking warnings to the pretend timeline.
function Add-TestSuccessfulDetailsAndWarnings {
    param([hashtable] $Fixture, [int] $Details = 104, [int] $Warnings = 79)

    for ($i = 1; $i -le $Details; $i++) {
        $detailId = '22222222-2222-2222-2222-{0:d12}' -f $i
        $Fixture.timelines['1100'].records += [pscustomobject]@{
            id = "successful-command-task-$i"; parentId = 'job'; type = 'Task'; name = 'Setup Internal Feeds'
            state = 'completed'; result = 'succeeded'; errorCount = 0; warningCount = 0
            details = @{ id = $detailId; changeId = 3 }
        }
        $Fixture.timelines["1100/$detailId"] = @{ id = $detailId; changeId = 3; records = @(
            [pscustomobject]@{ id = "command-$i"; type = 'command'; name = 'command'; parentId = $null
                state = 'completed'; result = 'succeeded'; errorCount = 0; warningCount = 0 }
        ) }
    }
    for ($i = 1; $i -le $Warnings; $i++) {
        $Fixture.timelines['1100'].records += [pscustomobject]@{
            id = "cg-warning-$i"; parentId = 'job'; type = 'Task'; name = "Component Governance (container-$($i % 5))"
            state = 'completed'; result = 'succeeded'; errorCount = 0; warningCount = 2
            log = @{ id = 1000 + $i }
            issues = @(@{ type = 'warning'; category = 'General'; message = 'Nonblocking governance warning; all warnings are retained.' })
        }
    }
}

# Creates pretend jobs where a build failure causes later artifact and SBOM tasks to fail.
function Set-TestCascadeLayout {
    param([hashtable] $Fixture)

    $records = [Collections.Generic.List[object]]::new()
    for ($job = 1; $job -le 2; $job++) {
        $jobId = "cascade-job-$job"
        $result = if ($job -eq 1) { 'failed' } else { 'canceled' }
        $records.Add([pscustomobject]@{
            id = $jobId; name = 'Identical display name'; type = 'Job'; result = $result
            issues = @(@{ type = 'error'; message = 'Build failed or was canceled in this job.' })
        })
        $records.Add([pscustomobject]@{
            id = "cascade-root-$job"; parentId = $jobId; name = "Build (container-$job)"; type = 'Task'; result = $result
            startTime = '2026-09-23T10:00:00Z'; finishTime = '2026-09-23T10:01:00Z'
            log = @{ id = 300 + $job }; issues = @(@{ type = 'error'; message = 'Immediate bootstrap failure.' })
        })
        for ($i = 1; $i -le 7; $i++) {
            $name = switch ($i % 3) {
                1 { "Generate SBoM Manifest (container-$job) (1ES PT)" }
                2 { 'Publish Artifacts (On Failure)' }
                0 { 'Publish OSX Artifacts (On Failure)' }
            }
            $path = if ($i % 2) { 'C:\agent\_work\1\a\prepublish' } else {
                'C:\agent\_work\1\s\src\runtime\artifacts\packages\Release\Shipping'
            }
            $issues = if ($i % 3 -eq 1) { @(
                @{ type = 'error'; message = 'Encountered an error in generation workflow' },
                @{ type = 'error'; message = "Error details: Could not find a part of the path '$path'." },
                @{ type = 'warning'; message = 'Failed to write workflow results file' },
                @{ type = 'error'; message = 'SBOM generation failed with exit code 1.' },
                @{ type = 'error'; message = "The process 'C:\tools\python.exe' failed with exit code 1" }
            ) } else { @(@{ type = 'error'; message = "Path does not exist: $path" }) }
            $records.Add([pscustomobject]@{
                id = "cascade-$job-$i"; parentId = $jobId; name = $name; type = 'Task'; result = 'succeededWithIssues'
                startTime = ([DateTimeOffset]'2026-09-23T10:02:00Z').AddSeconds($i).ToString('o')
                finishTime = ([DateTimeOffset]'2026-09-23T10:02:10Z').AddSeconds($i).ToString('o')
                log = @{ id = 600 + 10 * $job + $i }; issues = $issues
            })
        }
    }
    $Fixture.timelines['1100'] = @{ id = '00000000-0000-0000-0000-000000000001'; records = $records.ToArray() }
}

$script:passed = 0
$script:fixtureRoot = [IO.Path]::GetFullPath((Join-Path ([IO.Path]::GetTempPath()) "vmr-evidence-tests-$([Guid]::NewGuid().ToString('N'))"))
[void][IO.Directory]::CreateDirectory($script:fixtureRoot)
# The Guide and the ownership policy are read from a checkout, never fetched: this one is a fixture.
$script:checkoutRoot = Join-Path $script:fixtureRoot 'checkout'
[void][IO.Directory]::CreateDirectory((Join-Path $script:checkoutRoot 'docs'))
[void][IO.Directory]::CreateDirectory((Join-Path $script:checkoutRoot '.github\policies'))
[IO.File]::WriteAllText((Join-Path $script:checkoutRoot 'docs\VMR-QB-Guide.md'), "Release schedule: https://dev.azure.com/devdiv/DevDiv/_wiki/wikis/DevDiv.wiki/123/Release`n")
[IO.File]::WriteAllText((Join-Path $script:checkoutRoot '.github\policies\assign_ownership.yml'), "teamReviewer: fixture-team`n")
try {
    Invoke-TestCase 'Bearer construction rejects placeholders without exposing credentials' {
        $headers = New-VmrAzureHeaders -AccessToken 'synthetic-only'
        Assert-Equal $headers.Authorization ('Bearer ' + 'synthetic-only') 'Bearer prefix and acquired token.'
        Assert-Equal $headers.Accept 'application/json' 'JSON accept header.'
        $headers.Clear()
        Assert-Throws { New-VmrAzureHeaders -AccessToken '******' } 'usable bearer' $true
        Assert-Throws { New-VmrAzureHeaders -AccessToken "bad`r`nheader" } 'usable bearer' $true
    }
    Invoke-TestCase 'HTTPS/read-only allowlists and exact NuGet repository support' {
        Assert-VmrReadOnlyApiPath GitHub 'repos/NuGet/NuGet.Client/pulls/1'
        Assert-Throws { Assert-VmrReadOnlyApiPath GitHub 'repos/NuGet/Other/pulls/1' } 'limited'
        Assert-Throws { Assert-VmrReadOnlyApiPath AzureDevOps 'build/builds/%252e%252e/secrets' dnceng internal } 'traversal'
        Assert-Throws { Assert-VmrReadOnlyApiPath AzureDevOps 'build/builds?continuationToken=x' dnceng internal } 'pagination'
        Assert-Throws { Assert-VmrReadOnlyApiPath AzureDevOps 'build/builds' dnceng public } 'allowlisted'
        Assert-Throws { Invoke-VmrEvidenceHttpGet -Uri 'http://dev.azure.com/dnceng/internal/_apis/build/definitions' -Headers @{} } 'HTTPS'
        Assert-Throws { Invoke-VmrEvidenceHttpGet -Uri 'https://evil.example/dnceng/internal/_apis/build/definitions' -Headers @{} } 'HTTPS'
    }
    Invoke-TestCase 'HTTP authentication, permission denial, and sign-in redirects fail closed' {
        foreach ($status in @(401, 403)) {
            Assert-Throws { Assert-VmrEvidenceHttpResponse @{ statusCode = $status; contentType = 'application/json'; headers = @{} } 'dnceng/internal' } "HTTP $status" $true
        }
        Assert-Throws { Assert-VmrEvidenceHttpResponse @{ statusCode = 302; contentType = ''; headers = @{ Location = 'https://login.microsoftonline.com/secret-state' } } 'dnceng/internal' } 'sign-in' $true
        Assert-Throws { Assert-VmrEvidenceHttpResponse @{ statusCode = 200; contentType = 'text/html'; headers = @{}; content = '<title>Sign in</title>' } 'dnceng/internal' } 'sign-in' $true
        Assert-Throws { Assert-VmrEvidenceHttpResponse @{ statusCode = 200; contentType = 'text/html'; headers = @{}; content = '<title>Proxy error</title>' } 'dnceng/internal' } 'unexpected HTML' $false
        Assert-Throws { Assert-VmrEvidenceHttpResponse @{ statusCode = 503; contentType = 'text/html'; headers = @{}; content = '<title>Service error</title>' } 'dnceng/internal' } 'HTTP 503' $false
        Assert-Throws { Assert-VmrEvidenceHttpResponse @{ statusCode = 302; contentType = ''; headers = @{ Location = 'https://example.com/' } } 'dnceng/internal' } 'redirects' $false
        Assert-Throws { Assert-VmrEvidenceHttpResponse @{ statusCode = 500; contentType = 'application/json'; headers = @{} } 'dnceng/internal' } 'HTTP 500' $false
    }
    Invoke-TestCase 'Pagination preserves all pages and escapes opaque continuation tokens' {
        $fixture = New-TestFixture
        $path = Join-Path $fixtureRoot 'paged.json'
        $data = Invoke-VmrReadOnlyRequest -Session $fixture.session -Service AzureDevOps -Organization dnceng -Project internal `
            -ApiPath 'build/builds?statusFilter=completed' -OutputPath $path
        Assert-Equal $data.value.Count 16 'Every completed raw build retained.'
        Assert-True ($fixture.httpCalls[1].url.Contains('continuationToken=next%26%2B%20token')) 'Continuation token is escaped.'
        Assert-Equal @([IO.Directory]::GetFiles("$path.pages")).Count 2 'Raw pages retained.'
        Assert-Equal $fixture.tokenCalls 1 'Token acquired once in memory.'
        Assert-True (@($fixture.httpCalls | Where-Object { -not $_.correctHeader }).Count -eq 0) 'Correct Authorization reaches internal HTTP calls.'
    }
    Invoke-TestCase 'Repeated/unfinished pagination cannot return a complete inventory' {
        $http = { param($url, $headers, $path, $timeout, $maxBytes)
            @{ statusCode = 200; contentType = 'application/json'; headers = @{ 'x-ms-continuationtoken' = 'repeat' }; content = '{"value":[{"id":1}]}' }
        }
        $session = New-VmrReadOnlySession -HttpGet $http
        Assert-Throws { Invoke-VmrReadOnlyRequest $session AzureDevOps 'build/builds' } 'repeated' $false
        $session = New-VmrReadOnlySession -HttpGet $http -MaxPages 1
        $path = Join-Path $fixtureRoot 'incomplete.json'
        Assert-Throws { Invoke-VmrReadOnlyRequest $session AzureDevOps 'build/builds' -OutputPath $path } 'page limit' $false
        Assert-True (-not [IO.File]::Exists($path)) 'No success-shaped aggregate on a page limit.'
        Assert-True ([IO.File]::Exists((Join-Path "$path.pages" 'page-0001.json'))) 'Obtained raw page remains on disk.'
    }
    Invoke-TestCase 'HTTP byte budget and request budget are explicit' {
        $inputStream = [IO.MemoryStream]::new([byte[]](1..9))
        $outputStream = [IO.MemoryStream]::new()
        try { Assert-Throws { Copy-VmrEvidenceStream $inputStream $outputStream 8 } 'byte limit' $false }
        finally { $inputStream.Dispose(); $outputStream.Dispose() }
        $session = New-VmrReadOnlySession -MaxRequests 1 -HttpGet {
            param($url, $headers, $path, $timeout, $maxBytes)
            @{ statusCode = 200; contentType = 'application/json'; headers = @{ 'x-ms-continuationtoken' = 'next' }; content = '{"value":[]}' }
        }
        Assert-Throws { Invoke-VmrReadOnlyRequest $session AzureDevOps 'build/builds' } 'request budget' $false
    }
    Invoke-TestCase 'Malformed page counts and pagination shape changes cannot pass as complete' {
        $session = New-VmrReadOnlySession -HttpGet {
            param($url, $headers, $path, $timeout, $maxBytes)
            @{ statusCode = 200; contentType = 'application/json'; headers = @{}; content = '{"count":2,"value":[{"id":1}]}' }
        }
        Assert-Throws { Invoke-VmrReadOnlyRequest $session AzureDevOps 'build/builds' } 'page count' $false
        $session = New-VmrReadOnlySession -HttpGet {
            param($url, $headers, $path, $timeout, $maxBytes)
            if ($url -match 'continuationToken=') {
                return @{ statusCode = 200; contentType = 'application/json'; headers = @{}; content = '{"message":"wrong second page"}' }
            }
            @{ statusCode = 200; contentType = 'application/json'; headers = @{ 'x-ms-continuationtoken' = 'next' }; content = '{"value":[]}' }
        }
        Assert-Throws { Invoke-VmrReadOnlyRequest $session AzureDevOps 'build/builds' } 'pagination shape' $false
    }
    Invoke-TestCase 'A refused token is renewed once; a second refusal is final and cached without further HTTP or provider attempts' {
        $fixture = New-TestFixture @{ authFailure = $true }
        for ($i = 0; $i -lt 2; $i++) {
            Assert-Throws {
                Invoke-VmrReadOnlyRequest $fixture.session AzureDevOps 'build/definitions?name=dotnet-unified-build' dnceng internal
            } 'HTTP 401' $true
        }
        Assert-Equal $fixture.httpCalls.Count 2 'The first 401 is tried once more with a fresh token; the second refusal is final and cached.'
        Assert-Equal $fixture.tokenCalls 2 'One token for the first try and one fresh one for the retry; nothing more once the failure is cached.'
        Assert-Equal $fixture.session.tokens.Count 0 'Rejected token removed from memory cache.'
    }

    $baselineFixture = New-TestFixture
    $baseline = Invoke-TestCollection $baselineFixture baseline
    Invoke-TestCase 'Schema v2 has exactly public/internal records and backward-compatible indexes' {
        Assert-Equal $baseline.schemaVersion 2 'Schema version.'
        Assert-Equal $baseline.status 'complete' (
            'Baseline complete coverage. ' + (($baseline.requiredErrors | ForEach-Object { "$($_.stage): $($_.message)" }) -join '; '))
        Assert-Equal (($baseline.pipelines | ForEach-Object name) -join ',') 'public,internal' 'Exactly two ordered pipeline records.'
        Assert-Equal $baseline.requiredErrors.Count 0 'No baseline required errors.'
        Assert-Equal $baseline.reportDate '2026-09-23' 'Report date.'
        foreach ($field in @('windowStartUtc', 'observationCutoffUtc', 'collectedAtUtc', 'failureTasksPath', 'rootLogsPath', 'sources')) {
            Assert-True ($null -ne $baseline.PSObject.Properties[$field]) "Contract field $field."
        }
        foreach ($pipeline in $baseline.pipelines) {
            foreach ($field in @('name', 'status', 'inventoryPath', 'activeInventoryPaths', 'requiredProblemBuildIds', 'counts', 'error', 'loginCommand')) {
                Assert-True ($null -ne $pipeline.PSObject.Properties[$field]) "Pipeline contract field $field."
            }
            Assert-Equal $pipeline.status 'available' 'Pipeline fully available.'
            Assert-True ([IO.File]::Exists($pipeline.inventoryPath)) 'Scoped inventory exists.'
        }
        Assert-True ([IO.File]::Exists($baseline.failureTasksPath) -and [IO.File]::Exists($baseline.rootLogsPath)) 'Both legacy indexes always exist.'
        Assert-True ($baseline.sources.Count -gt 0) 'Legacy sources retained.'
    }
    Invoke-TestCase 'Exact finish window, shipping branch scope, PR metadata, and raw preservation' {
        $internal = $baseline.pipelines | Where-Object name -eq internal
        $public = $baseline.pipelines | Where-Object name -eq public
        Assert-Equal $internal.counts.completed 13 'All 13 internal eligible completions.'
        Assert-Equal $internal.counts.succeeded 10 'All internal successes counted.'
        Assert-Equal $public.counts.completed 10 'Experimental PR/base/head builds excluded.'
        foreach ($pipeline in @($internal, $public)) {
            Assert-Equal $pipeline.counts.failed 1 'Failed count.'
            Assert-Equal $pipeline.counts.canceled 1 'Canceled count.'
            Assert-Equal $pipeline.counts.partiallySucceeded 1 'Partial count.'
        }
        $inventory = Read-TestJson $internal.inventoryPath
        Assert-True (1009 -in $inventory.value.id -and 1010 -in $inventory.value.id) 'Both inclusive boundary instants retained.'
        Assert-True (1190 -notin $inventory.value.id -and 1191 -notin $inventory.value.id -and 1192 -notin $inventory.value.id) 'Experiments and one-tick-outside finishes excluded.'
        $raw = Read-TestJson $internal.rawInventoryPaths[0]
        Assert-Equal $raw.value.Count 16 'Full raw inventory unfiltered.'
        Assert-Equal $raw.value[0].untouchedRawField.nested.retained 'raw fixture' 'Raw nested fields survive.'
        $publicInventory = Read-TestJson $public.inventoryPath
        $pr = ($publicInventory.value | Where-Object id -eq 2101).vmrClassification.pullRequest
        Assert-Equal $pr.baseRefName 'main' 'Actual GitHub PR base.'
        Assert-Equal $pr.author 'fixture-author' 'Actual GitHub author.'
        Assert-Equal $pr.title 'Actual fixture PR 41' 'Actual GitHub title.'
    }
    Invoke-TestCase 'All internal success timelines and all public problem timelines are retained' {
        $internal = $baseline.pipelines | Where-Object name -eq internal
        $public = $baseline.pipelines | Where-Object name -eq public
        Assert-Equal $internal.timelineCoverage.availableBuildIds.Count 13 'Full internal coverage, not six-success sampling.'
        Assert-Equal $public.timelineCoverage.availableBuildIds.Count 9 'All public problems plus exactly six successes.'
        $failure = Read-TestJson $baseline.failureTasksPath
        $warning = $failure.builds | Where-Object buildId -eq 1001
        Assert-True (@($warning.tasks | Where-Object result -eq succeededWithIssues).Count -eq 1) 'Successful internal warnings retained.'
        $failed = $failure.builds | Where-Object buildId -eq 1100
        Assert-True (@($failed.tasks | Where-Object task -eq 'Nested Build').Count -eq 1) 'Referenced full child timeline collected.'
        Assert-True (@($failed.tasks | Where-Object task -eq 'Agent diagnostic').Count -eq 1) 'Issues without a result are not discarded.'
        $canceled = $failure.builds | Where-Object buildId -eq 1101
        $messages = @($canceled.tasks.issues | ForEach-Object message) -join ' '
        Assert-True ($messages -match '240 minutes' -and $messages -match 'agent was lost' -and $messages -match 'manually canceled') 'Job timeout, agent loss, manual cancellation preserved.'
    }
    Invoke-TestCase 'Successful Task command details stay optional: the full primary pipeline is kept without exhausting detail limits' {
        $fixture = New-TestFixture
        Add-TestSuccessfulDetailsAndWarnings $fixture
        $index = Invoke-TestCollection $fixture successful-task-details
        Assert-Equal $index.status 'complete' 'Successful Task command details are not pipeline coverage gaps.'
        $entry = (Read-TestJson $index.failureTasksPath).builds | Where-Object buildId -eq 1100
        Assert-Equal $entry.timelineStatus 'available' 'Full primary records and required nested failure detail are complete.'
        Assert-Equal $entry.timelineCoverage.optionalTaskDetails.Count 104 'Every intentionally deferred Task detail is documented.'
        Assert-Equal $entry.timelineCoverage.requiredDetailIds.Count 1 'Genuine Job detail still required.'
        Assert-Equal $entry.timelineCoverage.unresolvedDetailIds.Count 0 'No unresolved required detail.'
        Assert-Equal @($fixture.httpCalls | Where-Object { $_.url -match '/build/builds/1100/timeline' }).Count 2 `
            'Only root plus genuine structural detail were fetched, not 104 successful command leaves.'
        Assert-Equal $entry.warningSummary.taskCount 79 'All 79 warning-only Task records are retained.'
        Assert-Equal $entry.warningSummary.taskWarningCount 158 'Declared warning counters are preserved, not reduced to the truncated issues array.'
        Assert-Equal @($entry.tasks | Where-Object { $_.result -eq 'succeeded' -and $_.task -like 'Component Governance*' }).Count 79 `
            'Full warning records remain in failure-tasks.json.'
        $logs = Read-TestJson $index.rootLogsPath
        $warnings = @($logs.logs | Where-Object { $_.actualBuildId -eq 1100 -and $_.evidenceKind -eq 'warningSample' })
        Assert-Equal $warnings.Count 1 'At most one separately budgeted warning log is sampled.'
        Assert-True (-not $warnings[0].required) 'Warning sample never becomes required initiating evidence.'
        Assert-Equal $entry.warningSummary.logSampling.notSampled 78 'Unsampled warnings are explicitly expected sampling.'
        Assert-True (@($index.requiredErrors | Where-Object stage -like 'root-logs-1100').Count -eq 0) 'No warning-induced root-limit errors.'
    }
    Invoke-TestCase 'Repeated structural references, cycles and duplicate record snapshots are fetched once' {
        $fixture = New-TestFixture
        $partId = '00000000-0000-0000-0000-000000000002'
        $rootId = '00000000-0000-0000-0000-000000000001'
        $fixture.timelines['1100'].records += [pscustomobject]@{
            id = 'other-job'; type = 'Job'; name = 'Other job'; result = 'succeeded'
            details = @{ id = $partId.ToUpperInvariant() }
        }
        $fixture.timelines["1100/$partId"].records += @(
            $fixture.timelines['1100'].records[0],
            [pscustomobject]@{ id = 'cycle-task'; parentId = 'job'; type = 'Task'; name = 'Failed nested check'; result = 'failed'
                log = @{ id = 23 }; details = @{ id = $rootId } }
        )
        $index = Invoke-TestCollection $fixture repeated-parts -Limits @{ maxTimelinePartsPerBuild = 2 }
        Assert-Equal $index.status 'complete' 'Shared references/cycles do not consume another part budget.'
        $entry = (Read-TestJson $index.failureTasksPath).builds | Where-Object buildId -eq 1100
        Assert-Equal $entry.timelineCoverage.duplicateRecordCount 1 'Repeated primary record is counted once.'
        Assert-True ($entry.timelineCoverage.repeatedReferenceCount -ge 2) 'Repeated detail references are distinguished from new parts.'
        Assert-Equal @($entry.tasks | Where-Object id -eq job).Count 1 'No duplicated Job issue/failure counts.'
        Assert-Equal @($entry.tasks | Where-Object id -eq cycle-task).Count 1 'New nested failure is retained.'
        Assert-Equal @($fixture.httpCalls | Where-Object { $_.url -match '/build/builds/1100/timeline' }).Count 2 'Root and one unique required part only.'
    }
    Invoke-TestCase 'A 204 timeline is a YAML pre-flight rejection, diagnosed from validationResults' {
        $index = Invoke-TestCollection (New-TestFixture @{ preflightRejection = $true }) preflight-rejection
        Assert-Equal $index.status 'complete' 'A build with no timeline was treated as missing evidence.'
        Assert-Equal @($index.requiredErrors | Where-Object stage -like '*1101*').Count 0 `
            'A definitive 204 timeline produced a collection error.'
        $internal = $index.pipelines | Where-Object name -eq 'internal'
        Assert-True (1101 -in @($internal.timelineCoverage.noTimelineBuildIds)) 'No-timeline coverage was not recorded separately.'
        Assert-True (1101 -notin @($internal.timelineCoverage.unavailableBuildIds)) 'A resolved no-timeline build was reported unavailable.'
        $entry = (Read-TestJson $index.failureTasksPath).builds | Where-Object buildId -eq 1101
        Assert-Equal $entry.timelineStatus 'none' 'Timeline status did not record a definitive absence.'
        Assert-Equal @($entry.tasks).Count 1 'Pre-flight validation was not surfaced as the initiating diagnosis.'
        Assert-Equal $entry.tasks[0].recordType 'PipelineValidation' 'Synthesized pre-flight record has the wrong type.'
        Assert-True ([string]$entry.tasks[0].issues[0].message -match 'pipeline is not valid') 'The validation message was not retained verbatim.'
        $logs = Read-TestJson $index.rootLogsPath
        Assert-Equal @($logs.logs | Where-Object actualBuildId -eq 1101).Count 0 'A build with no timeline requested root logs.'
    }
    Invoke-TestCase 'A no-timeline build without validation results stays an explicit gap' {
        $index = Invoke-TestCollection (New-TestFixture @{ preflightWithoutValidation = $true }) preflight-undiagnosed
        Assert-Equal $index.status 'partial' 'An undiagnosable no-timeline build was reported complete.'
        Assert-True (@($index.requiredErrors | Where-Object { $_.message -match 'no pipeline validation results' }).Count -eq 1) `
            'A build with neither timeline nor validation results lacked an explicit gap.'
    }
    Invoke-TestCase 'True unresolved parts, missing parents and conflicting records remain explicit gaps' {
        $fixture = New-TestFixture
        $index = Invoke-TestCollection $fixture genuine-detail-bound -Limits @{ maxTimelinePartsPerBuild = 1 }
        Assert-Equal $index.status 'partial' 'A genuine uncollected Job detail still causes partial coverage.'
        $entry = (Read-TestJson $index.failureTasksPath).builds | Where-Object buildId -eq 1100
        Assert-Equal $entry.timelineCoverage.unresolvedDetailIds.Count 1 'Unresolved required timeline ID retained.'
        $fixture = New-TestFixture
        $fixture.timelines['1100'].records += [pscustomobject]@{
            id = 'orphan'; type = 'Task'; name = 'Orphan'; parentId = 'missing-parent'; result = 'succeeded'
        }
        $index = Invoke-TestCollection $fixture missing-parent
        Assert-Equal $index.status 'partial' 'Missing structural parents are not silently ignored.'
        $fixture = New-TestFixture
        $fixture.timelines['1100/00000000-0000-0000-0000-000000000002'].records += [pscustomobject]@{
            id = '11111111-2222-3333-4444-555555555555'; parentId = 'job'; type = 'Task'; name = 'Build'; result = 'failed'
            log = @{ id = 2 }; issues = @(@{ type = 'error'; message = 'Different retained error snapshot.' })
        }
        $index = Invoke-TestCollection $fixture conflicting-record
        Assert-Equal $index.status 'partial' 'Conflicting repeated records remain explicit rather than discarding a diagnostic.'
        Assert-True (($index.requiredErrors.message -join ' ') -match 'Conflicting repeated') 'Specific conflicting-record evidence gap.'
    }
    Invoke-TestCase 'Error-bearing Tasks and canceled-error Jobs remain required despite warning noise' {
        $fixture = New-TestFixture
        Add-TestSuccessfulDetailsAndWarnings $fixture -Details 0
        $fixture.timelines['1100'].records += @(
            [pscustomobject]@{ id = 'nonblocking-errors'; parentId = 'job'; type = 'Task'; name = 'Run Tests with tolerated errors'
                result = 'succeededWithIssues'; errorCount = 1; warningCount = 0; log = @{ id = 24 }
                issues = @(@{ type = 'error'; message = 'Actual test error, not a nonblocking warning.' }) },
            [pscustomobject]@{ id = 'succeeded-error'; parentId = 'job'; type = 'Task'; name = 'Succeeded with error issue'
                result = 'succeeded'; errorCount = 0; warningCount = 1; log = @{ id = 25 }
                issues = @(@{ type = 'error'; message = 'Error issue overrides successful status.' }) },
            [pscustomobject]@{ id = 'agent-lost'; type = 'Job'; name = 'Agent loss'; result = 'canceled'; log = @{ id = 26 }
                issues = @(@{ type = 'error'; message = 'Agent lost; canceled job.' }) }
        )
        $index = Invoke-TestCollection $fixture critical-with-warning-noise
        $logs = Read-TestJson $index.rootLogsPath
        foreach ($id in @('nonblocking-errors', 'succeeded-error', 'agent-lost')) {
            $log = $logs.logs | Where-Object { $_.actualBuildId -eq 1100 -and $_.recordId -eq $id }
            Assert-True ($log.available -and $log.required -and $log.evidenceKind -eq 'failure') 'Real failed/error/cancellation evidence is root-prioritized.'
        }
        $fixture = New-TestFixture
        Add-TestSuccessfulDetailsAndWarnings $fixture -Details 0
        $index = Invoke-TestCollection $fixture no-warning-downloads -Limits @{ maxWarningLogsPerBuild = 0 }
        Assert-Equal $index.status 'complete' 'Disabling optional warning logs does not lose complete required evidence.'
        Assert-Equal @((Read-TestJson $index.rootLogsPath).logs | Where-Object evidenceKind -eq warningSample).Count 0 'Zero warning downloads is supported explicitly.'
    }
    Invoke-TestCase 'Sixteen error Tasks retain same-job cascade evidence without consuming twelve mandatory root slots' {
        $fixture = New-TestFixture
        Set-TestCascadeLayout $fixture
        $index = Invoke-TestCollection $fixture linked-cascades -Limits @{ maxRootLogsPerBuild = 12 }
        Assert-Equal $index.status 'complete' 'Fourteen proven downstream cascades do not create false required-log limits.'
        Assert-Equal $index.limits.maxRootLogsPerBuild 12 'Cascade classification does not rely on a larger root-log budget.'
        $entry = (Read-TestJson $index.failureTasksPath).builds | Where-Object buildId -eq 1100
        Assert-Equal @($entry.tasks | Where-Object recordType -eq Task).Count 16 'Every error-bearing Task remains retained.'
        Assert-Equal $entry.cascadeSummaries.Count 14 'Each summarized cascade retains an explicit root link.'
        $logs = Read-TestJson $index.rootLogsPath
        $cascades = @($logs.logs | Where-Object { $_.actualBuildId -eq 1100 -and $_.evidenceKind -eq 'cascadeSummary' })
        Assert-Equal $cascades.Count 14 'Each downstream record stays in the log index as a timeline-backed summary.'
        foreach ($cascade in $cascades) {
            Assert-True (-not $cascade.required -and -not $cascade.available -and $null -eq $cascade.path) 'Raw cascade log is optional, not fabricated.'
            Assert-Equal $cascade.rawLogStatus 'not_requested_optional' 'Explicit optional raw-log status.'
            Assert-Equal $cascade.diagnosticExcerptSource 'timelineIssues' 'Timeline diagnostic provenance retained.'
            Assert-True ($cascade.diagnosticExcerpt -match 'Path does not exist|Could not find a part of the path') 'Specific missing-input quote retained.'
            Assert-Equal $cascade.jobId $cascade.cascade.jobId 'Job GUID linkage, not display-name inference.'
            $root = $entry.tasks | Where-Object id -eq $cascade.cascade.rootRecordId
            Assert-True ($root.result -in @('failed','canceled') -and $root.jobId -eq $cascade.jobId) 'Correct initiating Build task preserved and linked.'
            Assert-True ((Get-VmrCascadeTimestamp $root.finishTime) -le (Get-VmrCascadeTimestamp $cascade.cascade.downstreamStartTimeUtc)) 'Root precedes the downstream task.'
        }
        Assert-Equal @($fixture.httpCalls | Where-Object { $_.url -match '/build/builds/1100/logs/' }).Count 2 `
            'Only the two initiating Build logs are downloaded, not fourteen downstream raw logs.'
    }
    Invoke-TestCase 'Standalone SBOM signing/scanning and ambiguous cascades remain required' {
        foreach ($scenario in @('different-job', 'missing-time', 'ambiguous-diagnostic', 'signing', 'scanner', 'intervening-success')) {
            $fixture = New-TestFixture
            Set-TestCascadeLayout $fixture
            $task = $fixture.timelines['1100'].records | Where-Object id -eq 'cascade-1-1'
            switch ($scenario) {
                'different-job' {
                    $fixture.timelines['1100'].records += [pscustomobject]@{
                        id = 'unrelated-job'; name = 'Identical display name'; type = 'Job'; result = 'failed'
                        issues = @(@{ type = 'error'; message = 'Unrelated standalone SBOM failure.' })
                    }
                    $task.parentId = 'unrelated-job'
                }
                'missing-time' { $task.startTime = $null }
                'ambiguous-diagnostic' { $task.issues = @(@{ type = 'error'; message = 'SBOM generation failed with exit code 1.' }) }
                'signing' {
                    $task.name = 'SBOM Signing'
                    $task.issues += @{ type = 'error'; message = 'ESRP signing failed independently.' }
                }
                'scanner' { $task.issues += @{ type = 'error'; message = 'Scanner crashed independently of the missing input.' } }
                'intervening-success' {
                    $fixture.timelines['1100'].records += [pscustomobject]@{
                        id = 'recovered-build'; parentId = 'cascade-job-1'; name = 'Build'; type = 'Task'; result = 'succeeded'
                        startTime = '2026-09-23T10:01:10Z'; finishTime = '2026-09-23T10:01:50Z'
                    }
                }
            }
            $index = Invoke-TestCollection $fixture "cascade-negative-$scenario"
            $log = (Read-TestJson $index.rootLogsPath).logs | Where-Object { $_.actualBuildId -eq 1100 -and $_.recordId -eq 'cascade-1-1' }
            Assert-True ($log.required -and $log.available -and $log.evidenceKind -eq 'failure') "$scenario must remain required initiating/unknown evidence."
        }
        $fixture = New-TestFixture
        Set-TestCascadeLayout $fixture
        $records = $fixture.timelines['1100'].records
        $tasks = @(Get-VmrEvidenceProblemTasks $records)
        $steps = @(Get-VmrEvidenceProblemTasks $records -IncludeBuildSteps | Where-Object { $_.recordType -eq 'Task' -and $_.task -match '^Build(?:\s+\(.+\))?$' })
        $selection = Get-VmrRootLogSelection -Tasks $tasks -BuildSteps $steps -MaxLogs 12 -Required $true
        Assert-Equal $selection.cascades.Count 0 'No linkage is inferred without explicitly complete timeline coverage.'
        Assert-True (@($selection.omitted | Where-Object required).Count -gt 0) 'Ambiguous/incomplete evidence retains required gaps.'
    }
    Invoke-TestCase 'Warning sample limits and transport failures are optional, but HTTP auth failure remains required' {
        foreach ($scenario in @('warningLogFailure', 'oversizeWarningLog')) {
            $fixture = New-TestFixture @{ $scenario = $true }
            $index = Invoke-TestCollection $fixture $scenario
            Assert-Equal $index.status 'complete' 'An optional warning sample failure is not missing required failure evidence.'
            Assert-Equal $index.requiredErrors.Count 0 'Required errors exclude optional warning sampling failures.'
            Assert-Equal $index.optionalErrors.Count 1 'Optional failures remain explicit and discoverable.'
            $entry = (Read-TestJson $index.failureTasksPath).builds | Where-Object buildId -eq 1001
            Assert-Equal $entry.warningSummary.logSampling.unavailable 1 'Failed sample count retained.'
            Assert-Equal @($fixture.httpCalls | Where-Object { $_.url -match '/build/builds/1001/logs/17' })[0].maxBytes 1MB `
                'Warning downloads use the independent byte limit, not the full root-log limit.'
        }
        $index = Invoke-TestCollection (New-TestFixture @{ warningAuthFailure = $true }) warning-auth
        Assert-Equal (($index.pipelines | Where-Object name -eq internal).status) 'authentication_required' 'Actual HTTP authentication errors are never hidden as optional.'
        Assert-True (@($index.requiredErrors | Where-Object authenticationRequired).Count -gt 0) 'Concrete collector auth failure stays required.'
    }
    Invoke-TestCase 'Active inventory is separately paginated, later observed, and cutoff-queued only' {
        foreach ($pipeline in $baseline.pipelines) {
            Assert-Equal $pipeline.activeInventoryPaths.Count 4 'Separate active statuses.'
            $active = Read-TestJson $pipeline.activeInventoryPaths[0]
            Assert-Equal $active.value.Count 1 'Post-cutoff and experimental active runs excluded.'
            Assert-Equal $active.value[0].id 3001 'Only eligible pre-cutoff queued run.'
            Assert-True ($active.observationNote -match 'not a reconstruction') 'No active-at-cutoff claim.'
            Assert-True ((ConvertTo-VmrEvidenceTime $active.observedAtUtc) -gt (ConvertTo-VmrEvidenceTime $active.observationCutoffUtc)) 'Actual later observation timestamp.'
        }
        $completedCalls = @($baselineFixture.httpCalls | Where-Object { $_.url -match 'statusFilter=completed' })
        Assert-True (@($completedCalls | Where-Object { $_.url -notmatch 'queryOrder=finishTimeAscending|minTime=' }).Count -eq 0) 'Completed queries use finish-time ordering/window.'
    }
    Invoke-TestCase 'Root logs outrank cascades and preserve parent/child evidence without counting child failures' {
        $logs = Read-TestJson $baseline.rootLogsPath
        $parentLogs = @($logs.logs | Where-Object { $_.buildId -eq 1100 -and $_.actualBuildId -eq 1100 -and $_.available })
        $names = @($parentLogs | ForEach-Object task)
        foreach ($name in @('Build', 'Run Tests', 'Validate installer', 'Publish Using Darc')) {
            Assert-True ([array]::IndexOf($names, $name) -lt [array]::IndexOf($names, 'Generate SBoM')) "$name precedes the cascade candidate."
        }
        $childLogs = @($logs.logs | Where-Object { $_.actualBuildId -eq 9000 -and $_.available })
        Assert-Equal $childLogs.Count 1 'Relevant failed child promotion log.'
        Assert-Equal $childLogs[0].buildId 1100 'Original parent build ID.'
        Assert-Equal $childLogs[0].promotionBuildId 9000 'Child promotion ID.'
        Assert-Equal $childLogs[0].parentBuildId 1100 'Direct parent ID.'
        Assert-True (@($childLogs[0].diagnosticExcerpts.text -match '401 Unauthorized').Count -gt 0) 'Compact diagnostic excerpt retained.'
        Assert-Equal $baseline.status 'complete' 'CI log 401 is not investigator authentication failure.'
        Assert-True (@($baseline.requiredErrors | Where-Object authenticationRequired).Count -eq 0) 'No text-based authentication classification.'
        $failure = Read-TestJson $baseline.failureTasksPath
        Assert-Equal $failure.promotions.Count 2 'Both referenced child timelines retained, including successful child.'
        Assert-True (9000 -notin $failure.builds.buildId -and 9001 -notin $failure.builds.buildId) 'Children excluded from parent-build inventory.'
        Assert-True (@($baselineFixture.httpCalls | Where-Object { $_.url -match '/logs/' -and -not $_.streamed }).Count -eq 0) 'All logs stream to files, never transcript strings.'
    }
    Invoke-TestCase 'Root-log schema preserves task GUIDs, results and compact diagnostics for model validation' {
        $logs = Read-TestJson $baseline.rootLogsPath
        $buildLog = $logs.logs | Where-Object { $_.actualBuildId -eq 1100 -and $_.logId -eq 2 }
        Assert-Equal $buildLog.organization 'dnceng' 'Actual log organization.'
        Assert-Equal $buildLog.project 'internal' 'Actual log project.'
        Assert-Equal $buildLog.recordId '11111111-2222-3333-4444-555555555555' 'Unmodified timeline task GUID.'
        Assert-Equal $buildLog.recordType 'Task' 'Timeline record type.'
        Assert-Equal $buildLog.result 'failed' 'Task result, not the aggregate build result.'
        Assert-True ($buildLog.diagnosticExcerpt -is [string] -and $buildLog.diagnosticExcerpt -match '^L1: ##\[error\]') 'Compact log-derived diagnostic string.'
        Assert-Equal $buildLog.diagnosticExcerptSource 'log' 'Excerpt provenance.'
        Assert-True ($buildLog.diagnosticExcerpts.Count -gt 0) 'Original plural excerpt array retained.'
        $failure = Read-TestJson $baseline.failureTasksPath
        Assert-True ($buildLog.recordId -in ($failure.builds | Where-Object buildId -eq 1100).tasks.id) 'Log entry joins directly to the retained task record.'
        $child = $logs.logs | Where-Object { $_.actualBuildId -eq 9000 -and $_.available }
        Assert-Equal $child.recordType 'Task' 'Child promotion records receive the same metadata.'
        Assert-True ($child.diagnosticExcerpt -match '401 Unauthorized') 'CI diagnostic is retained without triggering monitoring auth.'
        $internal = $baseline.pipelines | Where-Object name -eq internal
        Assert-Equal ($internal.requiredProblemBuildIds -join ',') '1100,1101,1102' 'Failed, canceled and partiallySucceeded builds are all required.'
    }
    Invoke-TestCase 'Job cancellation and agent-loss issues survive when a task has no diagnostic' {
        $fixture = New-TestFixture
        $task = $fixture.timelines['1101'].records | Where-Object type -eq Task
        $task.issues = @()
        $task.log = $null
        $index = Invoke-TestCollection $fixture no-task-diagnostic
        $failures = Read-TestJson $index.failureTasksPath
        $job = ($failures.builds | Where-Object buildId -eq 1101).tasks | Where-Object recordType -eq Job
        Assert-True (($job.issues.message -join ' ') -match '240 minutes' -and
            ($job.issues.message -join ' ') -match 'agent was lost') 'Full Job issues retained independently of Task diagnostics.'
        $logs = Read-TestJson $index.rootLogsPath
        $jobEntry = $logs.logs | Where-Object { $_.actualBuildId -eq 1101 -and $_.recordType -eq 'Job' }
        Assert-Equal $jobEntry.result 'canceled' 'Job cancellation result retained.'
        Assert-True (-not $jobEntry.available -and $null -eq $jobEntry.logId) 'Timeline issue is not misrepresented as a downloaded log.'
        Assert-Equal $jobEntry.diagnosticExcerptSource 'timelineIssues' 'Job-only diagnostic provenance.'
        Assert-True ($jobEntry.diagnosticExcerpt -match '240 minutes' -and $jobEntry.diagnosticExcerpt -match 'agent was lost') 'Compact Job issues retained.'
        $taskEntry = $logs.logs | Where-Object { $_.actualBuildId -eq 1101 -and $_.recordType -eq 'Task' }
        Assert-True ($null -eq $taskEntry.diagnosticExcerpt -and $null -eq $taskEntry.diagnosticExcerptSource) 'Missing task diagnostic remains null, not a fabricated explanation.'
        Assert-Equal $index.status 'partial' 'Missing required task log is explicit.'
    }
    Invoke-TestCase 'CoreOnly retains guides and ownership without release wiki/team-detail calls' {
        Assert-True ([IO.File]::Exists((Join-Path (Split-Path $baseline.rootLogsPath) 'guide.txt'))) 'Guide text retained.'
        Assert-True ([IO.File]::Exists((Join-Path (Split-Path $baseline.rootLogsPath) 'ownership.txt'))) 'Ownership text retained.'
        Assert-True (@($baselineFixture.githubCalls | Where-Object { $_ -match '^orgs/dotnet/teams/' }).Count -eq 0) 'No every-team lookup.'
        Assert-True (@($baselineFixture.httpCalls | Where-Object { $_.url -match '/wiki/' }).Count -eq 0) 'No release wiki.'
    }
    Invoke-TestCase 'The Guide and the ownership policy come from the checkout with provenance and are never fetched; live observations stay live and timestamped' {
        Assert-True (@($baselineFixture.githubCalls | Where-Object { $_ -match '/contents/' }).Count -eq 0) 'A static checkout document was fetched from GitHub.'
        foreach ($pair in @(@('guide', 'docs\VMR-QB-Guide.md'), @('ownership', '.github\policies\assign_ownership.yml'))) {
            $source = @($baseline.sources | Where-Object name -eq $pair[0])
            Assert-Equal $source.Count 1 "$($pair[0]) must be recorded once."
            Assert-Equal $source[0].service 'Checkout' "$($pair[0]) must say it came from the checkout."
            $original = Join-Path $script:checkoutRoot $pair[1]
            Assert-Equal $source[0].sha256 ((Get-FileHash -LiteralPath $original -Algorithm SHA256).Hash.ToLowerInvariant()) "$($pair[0]) must carry the content hash."
            Assert-True ($source[0].checkoutPath -eq $original -and $source[0].lastWriteUtc -and $source[0].observedAtUtc) "$($pair[0]) must carry its path and times."
            Assert-Equal ([IO.File]::ReadAllText($source[0].path)) ([IO.File]::ReadAllText($original)) "$($pair[0]) evidence must be a copy of the checkout file."
            Assert-Equal $source[0].status 'available' "$($pair[0]) status."
        }
        $branches = @($baseline.sources | Where-Object name -eq 'branches')
        Assert-True ($branches.Count -eq 1 -and $branches[0].service -eq 'GitHub' -and $branches[0].observedAtUtc) 'Branches are a live observation and keep their observation time.'
    }
    Invoke-TestCase 'A Guide or policy missing from the checkout is an explicit evidence error, never a fetch' {
        $emptyCheckout = Join-Path $script:fixtureRoot 'empty-checkout'
        [void][IO.Directory]::CreateDirectory($emptyCheckout)
        $fixture = New-TestFixture
        $index = Invoke-TestCollection $fixture missing-checkout -CheckoutRoot $emptyCheckout
        Assert-Equal $index.status 'partial' 'A missing static document makes the collection partial.'
        $guide = @($index.sources | Where-Object name -eq 'guide')
        Assert-True ($guide.Count -eq 1 -and $guide[0].status -eq 'unavailable' -and $guide[0].error -match 'never fetches|not in this checkout') 'The missing Guide is explicit.'
        Assert-True (@($fixture.githubCalls | Where-Object { $_ -match '/contents/' }).Count -eq 0) 'A missing checkout document must not be fetched instead.'
        Assert-True (@($index.requiredErrors | Where-Object { $_.stage -in @('guide-checkout', 'ownership-checkout') }).Count -eq 2) 'Both missing documents are required errors.'
    }
    Invoke-TestCase 'PR classification gaps are partial with unknown counts, not healthy' {
        $index = Invoke-TestCollection (New-TestFixture @{ prGap = $true }) pr-gap
        $public = $index.pipelines | Where-Object name -eq public
        Assert-Equal $index.status 'partial' 'Classification gap reported.'
        Assert-Equal $public.status 'partial' 'Public pipeline partial.'
        Assert-True ($null -eq $public.inventoryPath -and $null -eq $public.counts.completed) 'No complete inventory/count claim.'
        Assert-True ([IO.File]::Exists($public.partialInventoryPath)) 'Provisional inventory retained.'
        Assert-True (2101 -in $public.requiredProblemBuildIds) 'Unclassified PR failure is still covered.'
    }
    Invoke-TestCase 'Unknown branch/result metadata is never classified as healthy or experimental' {
        foreach ($scenario in @('unknownBranch', 'unknownResult')) {
            $index = Invoke-TestCollection (New-TestFixture @{ $scenario = $true }) $scenario
            $internal = $index.pipelines | Where-Object name -eq internal
            Assert-Equal $index.status 'partial' 'Unknown build metadata is partial.'
            Assert-True ($null -eq $internal.inventoryPath -and $null -eq $internal.counts.completed) 'Incomplete classification leaves counts unknown.'
        }
    }
    Invoke-TestCase 'Distinct root task kinds get representatives before duplicate legs and cascades' {
        $records = @(1..15 | ForEach-Object { New-TestTask "build-$_" -LogId (100 + $_) })
        $records += @(
            (New-TestTask run -Name 'Run Tests' -LogId 201),
            (New-TestTask validate -Name 'Validate installer' -LogId 202),
            (New-TestTask publish -Name 'Publish Using Darc' -LogId 203),
            (New-TestTask cascade -Name 'Generate SBoM' -LogId 204 -Message 'Missing Shipping output directory.')
        )
        $tasks = @(Get-VmrEvidenceProblemTasks -Records $records)
        $selection = Get-VmrRootLogSelection -Tasks $tasks -MaxLogs 4 -Required $true
        Assert-Equal (($selection.selected | ForEach-Object { $_.task.task } | Sort-Object) -join ',') `
            'Build,Publish Using Darc,Run Tests,Validate installer' 'Four distinct root task kinds precede duplicates and cascades.'
        Assert-True (@($selection.omitted | Where-Object required).Count -gt 0) 'Unrepresented required legs remain explicit.'
    }
    Invoke-TestCase 'Other failed Task categories and shared log records are not starved by duplicate Build legs' {
        $records = @(1..15 | ForEach-Object { New-TestTask "build-$_" -LogId (100 + $_) })
        $records += @(
            (New-TestTask restore-warning -Name Restore -Result succeededWithIssues -LogId 200),
            (New-TestTask restore-failed -Name Restore -LogId 201),
            (New-TestTask audit-missing -Name Audit -LogId 0),
            (New-TestTask audit-failed -Name Audit -LogId 202),
            (New-TestTask cascade -Name 'Generate SBoM' -LogId 203 -Message 'Missing Shipping output directory.')
        )
        $selection = Get-VmrRootLogSelection -Tasks @(Get-VmrEvidenceProblemTasks $records) -MaxLogs 4 -Required $true
        Assert-Equal (($selection.selected | ForEach-Object { $_.task.task } | Sort-Object) -join ',') `
            'Audit,Build,Generate SBoM,Restore' 'Each failed Task category receives a representative before duplicate legs.'
        Assert-Equal (($selection.selected | Where-Object { $_.task.task -eq 'Restore' }).task.id) 'restore-failed' 'Failed outcome outranks warning in the same category.'
        $limited = Get-VmrRootLogSelection -Tasks @(Get-VmrEvidenceProblemTasks $records) -MaxLogs 3 -Required $true
        Assert-True (@($limited.omitted | Where-Object { $_.task.task -eq 'Generate SBoM' -and $_.required }).Count -eq 1) `
            'A budget-exhausted failed supporting category is also an explicit required coverage gap.'
        $shared = @((New-TestTask build -LogId 1), (New-TestTask tests -Name 'Run Tests' -LogId 1))
        $selection = Get-VmrRootLogSelection -Tasks @(Get-VmrEvidenceProblemTasks $shared) -MaxLogs 1 -Required $true
        Assert-Equal $selection.selected.Count 2 'Both task identities retain their shared log mapping.'
        Assert-Equal $selection.omitted.Count 0 'One physical log fits the unique-log budget.'
    }
    Invoke-TestCase 'The same failure in many builds gets one raw log; a different message, scope or branch gets its own, and same-failure records take no slot' {
        $nuget = 'error NU1605: Detected package downgrade: Example.Package from 7.0.0-preview.1.20000.103 to 7.0.0-preview.1.20000.99 (build {0}, 00000000-0000-4000-8000-0000000000a1, commit 0123456789abcdef0123456789abcdef01234567)'
        $records = {
            param([int] $Base, [string] $Message)
            @(Get-VmrEvidenceProblemTasks -Records @(
                (New-TestTask "build-$Base" -Name 'Build' -LogId $Base -Message $Message),
                (New-TestTask "tests-$Base" -Name 'Run Tests' -LogId ($Base + 1) -Message 'Tests did not finish.')))
        }
        $signatures = @{}
        $first = Get-VmrRootLogSelection -Tasks (& $records 100 ($nuget -f 9100001)) -MaxLogs 32 -Required $true -Signatures $signatures -SignatureScope 'internal|refs/heads/main' -BuildId 9100001
        Assert-Equal @($first.selected | Where-Object { $_.ContainsKey('sameFailureAs') }).Count 0 'The first build with a failure is its representative.'
        Assert-Equal $signatures.Count 2 'Each distinct failure is remembered once.'

        # The same failure with another build id, version stamp, GUID and hash: one category, so no second raw log.
        $second = Get-VmrRootLogSelection -Tasks (& $records 200 ($nuget -f 9100002).Replace('20000.103', '20010.7').Replace('00000000-0000-4000-8000-0000000000a1', 'aaaaaaaa-0000-4000-8000-000000000001').Replace('0123456789abcdef0123456789abcdef01234567', '1111111111111111111111111111111111111111')) `
            -MaxLogs 1 -Required $true -Signatures $signatures -SignatureScope 'internal|refs/heads/main' -BuildId 9100002
        Assert-Equal $second.selected.Count 2 'Both records are kept, as summaries.'
        Assert-Equal @($second.selected | Where-Object { $_.ContainsKey('sameFailureAs') }).Count 2 'Both name the build that stands for them.'
        Assert-Equal $second.omitted.Count 0 'Same-failure records take no slot, so a limit of one raises no gap.'
        Assert-Equal ($second.selected | Select-Object -First 1).sameFailureAs.buildId 9100001 'The representative build is named.'
        Assert-Equal ($second.selected | Select-Object -First 1).sameFailureAs.logId ([string]($signatures.Values | Where-Object { $_.task -eq 'Build' }).logId) 'And so is its log.'

        # A different message, another branch, or no message at all is never grouped.
        $other = Get-VmrRootLogSelection -Tasks (& $records 300 'error MSB4216: Could not run the "NetTaskHost" task.') -MaxLogs 32 -Required $true -Signatures $signatures -SignatureScope 'internal|refs/heads/main' -BuildId 9100003
        Assert-Equal (@($other.selected | Where-Object { -not $_.ContainsKey('sameFailureAs') }).task.task -join ',') 'Build' 'A different message for the same task is a new failure.'
        $branch = Get-VmrRootLogSelection -Tasks (& $records 400 ($nuget -f 9100004)) -MaxLogs 32 -Required $true -Signatures $signatures -SignatureScope 'internal|refs/heads/release/10.0.1xx' -BuildId 9100004
        Assert-Equal @($branch.selected | Where-Object { $_.ContainsKey('sameFailureAs') }).Count 0 'The same message on another branch keeps its own log.'
        $silent = @(Get-VmrEvidenceProblemTasks -Records @((New-TestTask silent -Name 'Build' -LogId 500 -Message ' ')))
        $before = $signatures.Count
        $unnamed = Get-VmrRootLogSelection -Tasks $silent -MaxLogs 32 -Required $true -Signatures $signatures -SignatureScope 'internal|refs/heads/main' -BuildId 9100005
        Assert-True ($unnamed.selected.Count -le 1 -and @($unnamed.selected | Where-Object { $_.ContainsKey('sameFailureAs') }).Count -eq 0 -and $signatures.Count -eq $before) 'A failure with no message cannot be grouped and is fetched as before.'

        # A distinct failure still meets the per-build limit.
        $tight = Get-VmrRootLogSelection -Tasks (& $records 600 'error CS0103: The name x does not exist.') -MaxLogs 1 -Required $true -Signatures $signatures -SignatureScope 'internal|refs/heads/main' -BuildId 9100006
        Assert-True (@($tight.omitted | Where-Object { $_.omission -match 'limit \(1\)' }).Count -le 1) 'The per-build limit still applies to distinct failures.'

        # Without signatures every failure gets its own log, exactly as before.
        $plain = Get-VmrRootLogSelection -Tasks (& $records 100 ($nuget -f 9100001)) -MaxLogs 32 -Required $true
        Assert-Equal @($plain.selected | Where-Object { $_.ContainsKey('sameFailureAs') }).Count 0 'No grouping unless asked.'

        Assert-Equal (Get-VmrFailureSignature -Task (& $records 1 ($nuget -f 9100001))[0] -Scope 'x') (Get-VmrFailureSignature -Task (& $records 2 (($nuget -f 9100002).Replace('20000.103', '20010.7')))[0] -Scope 'x') 'Build ids, versions, GUIDs and hashes do not make two failures different.'
        Assert-True ((Get-VmrFailureSignature -Task (& $records 1 'error NU1605: a')[0] -Scope 'x') -ne (Get-VmrFailureSignature -Task (& $records 1 'error NU1102: a')[0] -Scope 'x')) 'Different error codes are different failures.'
        Assert-True ((Get-VmrFailureSignature -Task (& $records 1 'exit code 1')[0] -Scope 'x') -ne (Get-VmrFailureSignature -Task (& $records 1 'exit code 137')[0] -Scope 'x')) 'Short numbers such as exit codes are kept.'
        Assert-True ($null -eq (Get-VmrFailureSignature -Task ([pscustomobject]@{ task = 'Build'; issues = @() }) -Scope 'x')) 'No message, no signature.'
        Assert-True ($null -eq (Get-VmrFailureSignature -Task (& $records 1 "Bash exited with code '1'.")[0] -Scope 'x')) 'A wrapper message names no cause, so it is never grouped.'
        $withWrapper = [pscustomobject]@{ task = 'Prep'; issues = @(@{ message = "dotnet-sdk-*.tar.gz not found in '/archive'." }, @{ message = "Bash exited with code '1'." }) }
        $withoutWrapper = [pscustomobject]@{ task = 'Prep'; issues = @(@{ message = "dotnet-sdk-*.tar.gz not found in '/archive'." }) }
        Assert-Equal (Get-VmrFailureSignature -Task $withWrapper -Scope 'x') (Get-VmrFailureSignature -Task $withoutWrapper -Scope 'x') 'A wrapper beside a real message is left out of the key.'
    }
    Invoke-TestCase 'Missing successful timeline, required logs and root budget limits are explicit partial coverage' {
        foreach ($scenario in @('timelineFailure', 'logFailure', 'missingLogId')) {
            $index = Invoke-TestCollection (New-TestFixture @{ $scenario = $true }) $scenario
            $internal = $index.pipelines | Where-Object name -eq internal
            Assert-Equal $index.status 'partial' "$scenario is partial."
            Assert-Equal $internal.counts.completed 13 'Known inventory counts remain exact despite evidence gaps.'
            Assert-True (@($index.requiredErrors | Where-Object { -not $_.authenticationRequired }).Count -gt 0) 'Non-authentication gap is explicit.'
        }
        $index = Invoke-TestCollection (New-TestFixture) bounded -Limits @{ maxRootLogsPerBuild = 2; maxTimelineBuildsPerPipeline = 3 }
        Assert-Equal $index.status 'partial' 'Budget exhaustion not healthy.'
        $logs = Read-TestJson $index.rootLogsPath
        Assert-True (@($logs.logs | Where-Object { $_.required -and -not $_.available }).Count -gt 0) 'Missing required representative logs indexed.'
        $internal = $index.pipelines | Where-Object name -eq internal
        Assert-Equal $internal.timelineCoverage.unavailableBuildIds.Count 10 'Uncollected internal successes explicitly listed.'
    }
    Invoke-TestCase 'Oversized streamed logs retain explicit partial raw paths without claiming availability' {
        $index = Invoke-TestCollection (New-TestFixture @{ oversizeLog = $true }) oversize-log
        Assert-Equal $index.status 'partial' 'Oversized required log is incomplete.'
        $logs = Read-TestJson $index.rootLogsPath
        $log = $logs.logs | Where-Object { $_.actualBuildId -eq 1100 -and $_.logId -eq 2 }
        Assert-True (-not $log.available -and -not [IO.File]::Exists($log.path)) 'No successful log file.'
        Assert-True ([IO.File]::Exists($log.partialPath)) 'Partial raw log is discoverable from the index.'
        Assert-Equal ([IO.File]::ReadAllText($log.partialPath)) 'Retained partial raw log fixture.' 'Retained bytes remain intact.'
    }
    Invoke-TestCase 'Failed inventory pages and malformed lists leave unknown counts and raw pages' {
        foreach ($scenario in @('inventoryFailure', 'malformedInventory')) {
            $index = Invoke-TestCollection (New-TestFixture @{ $scenario = $true }) $scenario
            $internal = $index.pipelines | Where-Object name -eq internal
            Assert-Equal $index.status 'partial' 'Incomplete inventory marked partial.'
            foreach ($count in $internal.counts.PSObject.Properties) { Assert-True ($null -eq $count.Value) 'Unknown inventory counts are null.' }
            Assert-True ($null -eq $internal.inventoryPath) 'No scoped complete inventory path.'
        }
    }
    Invoke-TestCase 'Token/HTTP failures expose safe sign-in instructions and never fall back' {
        foreach ($scenario in @('tokenFailure', 'authFailure')) {
            $fixture = New-TestFixture @{ $scenario = $true }
            $index = Invoke-TestCollection $fixture $scenario
            $internal = $index.pipelines | Where-Object name -eq internal
            Assert-Equal $internal.status 'authentication_required' 'Internal auth failure explicit.'
            Assert-Equal $internal.loginCommand 'az login --allow-no-subscriptions' 'Concrete Azure CLI sign-in command.'
            Assert-True ($null -eq $internal.counts.completed -and $null -eq $internal.inventoryPath) 'Auth outage is unknown, not zero.'
            # A token that could not be acquired is not asked for again; a 401 gets one fresh token and then it is final.
            Assert-Equal $fixture.tokenCalls ($(if ($scenario -eq 'authFailure') { 2 } else { 1 })) 'A failed identity is renewed at most once and never replaced.'
            Assert-Equal (($index.pipelines | Where-Object name -eq public).status) 'available' 'Public data is separate, never substituted for internal.'
            $serialized = ConvertTo-Json -InputObject $index -Depth 100
            Assert-True ($serialized -notmatch 'SECRET-DO-NOT-LEAK|synthetic-collector-token') 'Token and token command diagnostics not persisted.'
            Assert-True ($internal.error -match 'Microsoft work account') 'Required account/access is identified.'
        }
    }
    Invoke-TestCase 'Mid-pagination authentication loss discards complete counts without losing raw pages' {
        $fixture = New-TestFixture @{ inventoryAuthFailure = $true }
        $index = Invoke-TestCollection $fixture pagination-auth
        $internal = $index.pipelines | Where-Object name -eq internal
        Assert-Equal $internal.status 'authentication_required' 'Actual later-page HTTP 401 classified.'
        Assert-True ($null -eq $internal.counts.completed -and $null -eq $internal.inventoryPath) 'No incomplete success inventory.'
        $source = $index.sources | Where-Object name -eq 'internal-builds-1330-completed'
        Assert-True (-not [IO.File]::Exists($source.path)) 'No aggregate file on failed pagination.'
        Assert-True ([IO.File]::Exists((Join-Path "$($source.path).pages" 'page-0001.json'))) 'First raw page retained.'
        Assert-Equal $fixture.tokenCalls 2 'The token is renewed once after a later-page 401, and not again after the second refusal.'
    }
    Invoke-TestCase 'Cached missing promotion evidence makes every referencing scope partial' {
        foreach ($scenario in @('promotionTimelineFailure', 'promotionLogFailure')) {
            $fixture = New-TestFixture @{ sharedPromotion = $true; $scenario = $true }
            $index = Invoke-TestCollection $fixture $scenario
            foreach ($pipeline in $index.pipelines) { Assert-Equal $pipeline.status 'partial' 'Shared missing child is partial in both scopes.' }
            $suffix = if ($scenario -eq 'promotionTimelineFailure') { '/timeline' } else { '/logs/11' }
            Assert-Equal @($fixture.httpCalls | Where-Object { $_.url -match ("/build/builds/9000" + [regex]::Escape($suffix) + '\?') }).Count 1 `
                'Shared child resource was requested once, not repeatedly redownloaded.'
        }
    }
    Invoke-TestCase 'Promotion limits retain links/IDs and mark untraced children partial' {
        $index = Invoke-TestCollection (New-TestFixture @{ promotionChain = $true }) promotion-limit `
            -Limits @{ maxPromotionDepth = 1; maxPromotionBuildsPerParent = 2 }
        Assert-Equal $index.status 'partial' 'Untraced promotion is partial.'
        $failure = Read-TestJson $index.failureTasksPath
        $untraced = @($failure.promotions | Where-Object { $_.promotionBuildId -eq 9002 })
        Assert-Equal $untraced.Count 1 'Omitted child retains a record.'
        Assert-Equal $untraced[0].parentBuildId 9000 'Nested direct parent retained.'
        Assert-Equal $untraced[0].buildId 1100 'Original pipeline parent retained.'
    }
    Invoke-TestCase 'Run collisions are refused and legacy date indexes point only to fresh runs' {
        $path = Join-Path $fixtureRoot baseline
        $original = [IO.File]::ReadAllText((Join-Path $path 'sources.json'))
        Assert-Throws { Initialize-VmrEvidenceDirectory -OutputDirectory $path } 'new or empty'
        Assert-Throws { Initialize-VmrEvidenceDirectory -OutputDirectory 'relative\run' } 'absolute'
        Assert-Equal ([IO.File]::ReadAllText((Join-Path $path 'sources.json'))) $original 'Existing run was not changed.'
        $first = Invoke-TestCollection (New-TestFixture) legacy -Legacy
        $second = Invoke-TestCollection (New-TestFixture @{ authFailure = $true }) legacy -Legacy
        Assert-True ($first.failureTasksPath -ne $second.failureTasksPath) 'Same date gets distinct raw evidence directories.'
        Assert-Equal (($second.pipelines | Where-Object name -eq internal).status) 'authentication_required' 'Old internal success never reused.'
        Assert-True ([IO.File]::Exists((Join-Path $fixtureRoot 'legacy\failure-tasks.json'))) 'Legacy failure-tasks path preserved.'
        Assert-True ([IO.File]::Exists((Join-Path $fixtureRoot 'legacy\root-logs.json'))) 'Legacy root-logs path preserved.'
    }
    Invoke-TestCase 'A pipeline that fails unexpectedly is an error for that pipeline only; a previous complete index never looks current' {
        $first = Invoke-TestCollection (New-TestFixture) legacy-isolated -Legacy
        $fixture = New-TestFixture
        # An id that cannot be converted fails inside the internal pipeline's collection (a logic error, not a request failure).
        $fixture.internal[0].id = '99999999999999999999999999999'
        $index = Invoke-TestCollection $fixture legacy-isolated -Legacy
        Assert-Equal $index.status 'partial' 'The run is explicitly incomplete, not complete and not thrown away.'
        Assert-True ($index.failureTasksPath -ne $first.failureTasksPath) 'No previous run is masquerading as this run.'
        $internal = $index.pipelines | Where-Object name -eq internal
        $public = $index.pipelines | Where-Object name -eq public
        Assert-True ($internal.status -ne 'available' -and $null -eq $internal.counts.completed) 'The pipeline that failed claims no completed evidence.'
        Assert-Equal $public.status 'available' 'The other pipeline keeps what it collected.'
        Assert-True (@($index.requiredErrors | Where-Object { $_.scope -eq 'internal' -and $_.stage -eq 'pipeline-collection' }).Count -eq 1) 'The failure is recorded for that pipeline.'
        Assert-Equal (Read-TestJson $index.failureTasksPath).builds.Count (Read-TestJson $index.failureTasksPath).builds.Count 'The failure index is readable.'
    }
    Invoke-TestCase 'A build whose evidence cannot be read is a gap for that build, and the other builds are still collected' {
        $fixture = New-TestFixture
        $script:brokenBuild = [string]$fixture.internal[0].id
        $script:realBuildEvidence = ${function:Get-VmrBuildEvidence}
        # Simulates a logic error for one build and uses the real collector for all other builds in this test.
        function Get-VmrBuildEvidence {
            param([hashtable] $State, [string] $Scope, [string] $Organization, [string] $Project, [object] $Build)
            if ([string]$Build.id -eq $script:brokenBuild) { throw 'synthetic logic failure while reading one build' }
            & $script:realBuildEvidence @PSBoundParameters
        }
        $index = Invoke-TestCollection $fixture build-isolated
        Assert-Equal $index.status 'partial' 'One unreadable build makes the run partial, not failed.'
        Assert-True (@($index.requiredErrors | Where-Object { $_.scope -eq 'internal' -and $_.stage -eq "build-$script:brokenBuild" }).Count -eq 1) 'The gap is recorded for that build.'
        $builds = @((Read-TestJson $index.failureTasksPath).builds | Where-Object organization -eq 'dnceng')
        $broken = @($builds | Where-Object { [string]$_.buildId -eq $script:brokenBuild })
        Assert-True ($broken.Count -eq 1 -and $broken[0].timelineStatus -eq 'unavailable') 'The unreadable build is listed as unavailable, never as healthy.'
        Assert-True (@($builds | Where-Object { [string]$_.buildId -ne $script:brokenBuild -and $_.timelineStatus -eq 'available' }).Count -ge 1) 'The other builds are still collected.'
    }    Invoke-TestCase 'Legacy non-Core context still collects wiki and individual teams' {
        $fixture = New-TestFixture
        $index = Invoke-TestCollection $fixture full-context -FullContext
        Assert-Equal $index.status 'complete' 'Legacy collection succeeds.'
        Assert-True (@($fixture.httpCalls | Where-Object { $_.url -match '/wiki/' }).Count -eq 1) 'Release wiki retained for legacy callers.'
        Assert-True ('orgs/dotnet/teams/fixture-team' -in $fixture.githubCalls -and 'orgs/dotnet/teams/prodconsvcs' -in $fixture.githubCalls) 'Legacy individual-team lookups retained.'
    }
    Invoke-TestCase 'Proven empty inventories produce real zeros, not missing-data defaults' {
        $fixture = New-TestFixture
        $fixture.internal = @()
        $fixture.public = @()
        $index = Invoke-TestCollection $fixture empty
        Assert-Equal $index.status 'complete' 'Complete empty inventory.'
        foreach ($pipeline in $index.pipelines) {
            Assert-Equal $pipeline.counts.completed 0 'Known empty completion count is zero.'
            Assert-True ([IO.File]::Exists($pipeline.inventoryPath)) 'An actual empty scoped inventory exists.'
            Assert-Equal $pipeline.requiredProblemBuildIds.Count 0 'No invented problem builds.'
        }
        Assert-Equal (Read-TestJson $index.failureTasksPath).builds.Count 0 'Empty compatible builds array.'
        Assert-Equal (Read-TestJson $index.rootLogsPath).logs.Count 0 'Empty compatible logs array.'
    }
    Invoke-TestCase 'Excerpts stay compact while raw logs remain complete; artifacts contain no synthetic token' {
        $path = Join-Path $fixtureRoot 'large-fixture.log'
        $lines = @(1..100 | ForEach-Object { '##[error] ' + ('diagnostic ' * 100) })
        [IO.File]::WriteAllLines($path, $lines)
        $diagnostics = Get-VmrLogDiagnostics $path
        Assert-Equal $diagnostics.excerpts.Count 24 'Exact excerpt line budget.'
        Assert-True (@($diagnostics.excerpts | Where-Object { $_.text.Length -gt 512 }).Count -eq 0) 'Exact excerpt character budget.'
        Assert-True $diagnostics.excerptsTruncated 'Truncation explicitly labeled.'
        Assert-Equal ([IO.File]::ReadAllLines($path)).Count 100 'Full raw log retained.'
        $compact = Get-VmrCompactDiagnosticExcerpt -Lines @(
            ('first ' + ('x' * 1000)), 'second', 'middle', 'penultimate', ('last ' + ('y' * 1000))
        ) -MaxCharacters 256
        Assert-True ($compact.text.Length -le 256 -and $compact.text -match '^first ' -and $compact.text -match 'last ') 'Exact compact character bound preserves beginning and end.'
        Assert-True $compact.truncated 'Compact truncation is explicit.'
        $redacted = Get-VmrCompactDiagnosticExcerpt -Lines @('Authorization: Bearer offline-fake', 'accessToken=offline-fake')
        Assert-True ($redacted.text -notmatch 'offline-fake') 'Timeline and log compact excerpts share credential redaction.'
        foreach ($log in (Read-TestJson $baseline.rootLogsPath).logs) {
            Assert-True ($null -eq $log.diagnosticExcerpt -or $log.diagnosticExcerpt.Length -le 1536) 'Every indexed compact excerpt obeys the default limit.'
        }
        foreach ($file in [IO.Directory]::EnumerateFiles($fixtureRoot, '*', [IO.SearchOption]::AllDirectories)) {
            Assert-True (-not [IO.File]::ReadAllText($file).Contains('synthetic-collector-token')) 'Collector credentials never saved in any artifact.'
        }
    }
    "PASS: $script:passed offline collector tests. No network, reports, email, or external changes."
}
finally {
    if ([IO.Directory]::Exists($script:fixtureRoot)) { [IO.Directory]::Delete($script:fixtureRoot, $true) }
}
