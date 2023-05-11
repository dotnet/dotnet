<#
.SYNOPSIS
Script to post status of tests for the commit to GitHub
https://developer.github.com/v3/repos/statuses/

.DESCRIPTION
Uses the Personal Access Token of NuGetLurker to post status of tests and build to GitHub.
#>
# Set security protocol to tls1.2 for Invoke-RestMethod powershell cmdlet
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

Function Update-GitCommitStatus {
    param(
        [Parameter(Mandatory = $True)]
        [string]$PersonalAccessToken,
        [Parameter(Mandatory = $True)]
        [ValidateSet( "Build_and_UnitTest_NonRTM", "Build_and_UnitTest_RTM", "Unit Tests On Mac", "Functional Tests On Mac", "Mono Tests On Mac", "Unit Tests On Linux", "Functional Tests On Linux", "Windows FunctionalTests IsDesktop", "Windows FunctionalTests IsCore", "Windows CrossFrameworkTests", "End_To_End_Tests_On_Windows Part1", "End_To_End_Tests_On_Windows Part2", "Apex Tests On Windows", "Rebuild")]
        [string]$TestName,
        [Parameter(Mandatory = $True)]
        [ValidateSet( "pending", "success", "error", "failure")]
        [string]$Status,
        [Parameter(Mandatory = $True)]
        [string]$CommitSha,
        [Parameter(Mandatory = $True)]
        [string]$TargetUrl,
        [Parameter(Mandatory = $True)]
        [string]$Description
    )

    $Token = $PersonalAccessToken
    $Base64Token = [System.Convert]::ToBase64String([char[]]$Token)

    $Headers = @{
        Authorization = 'Basic {0}' -f $Base64Token;
    }

    $Body = @{
        state      = $Status;
        context    = $TestName;
        target_url = $TargetUrl;
        description = $Description
    } | ConvertTo-Json;

    Write-Host $Body

    try {
        # Post status of tests and build to GitHub.
        $r1 = Invoke-RestMethod -Headers $Headers -Method Post -Uri "https://api.github.com/repos/nuget/nuget.client/statuses/$CommitSha" -Body $Body
        Write-Host $r1
    }
    catch {
        $StatusCode = $PSItem.Exception.Response.StatusCode.Value__
        $exceptionMessage = $PSItem.Exception.Message

        # If branch name ends with "-MSRC" and the post statuscode is 404 (not found), it's acceptable.
        if ($env:BUILD_SOURCEBRANCHNAME.toUpper().endsWith("-MSRC") -and ($StatusCode -eq "404") )
        {
            Write-Host "[Info] : The commit hash could not be found on github."
        }
        else
        {
            throw $exceptionMessage
        }
    }
}

Function InitializeAllTestsToPending {
    param(
        [Parameter(Mandatory = $True)]
        [string]$PersonalAccessToken,
        [Parameter(Mandatory = $True)]
        [string]$CommitSha
    )

    Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Build_and_UnitTest_NonRTM" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
    Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Build_and_UnitTest_RTM" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
    if($env:RunFunctionalTestsOnWindows -eq "true")
    {
        # Setup individual states for the matrixing of jobs in "Functional Tests On Windows".
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Windows FunctionalTests IsDesktop" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Windows FunctionalTests IsCore" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
    }
    else
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Windows FunctionalTests IsDesktop" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Windows FunctionalTests IsCore" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
    }
    if($env:RunCrossFrameworkTestsOnWindows -eq "true")
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Windows CrossFrameworkTests" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
    }
    else
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Windows CrossFrameworkTests" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
    }
    if($env:RunTestsOnMac -eq "true")
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Unit Tests On Mac" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Functional Tests On Mac" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Mono Tests On Mac" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
    }
    else
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Unit Tests On Mac" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Functional Tests On Mac" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Mono Tests On Mac" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
    }
    if($env:RunTestsOnLinux -eq "true")
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Unit Tests On Linux" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Functional Tests On Linux" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
    }
    else
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Unit Tests On Linux" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Functional Tests On Linux" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
    }
    if($env:RunEndToEndTests -eq "true")
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "End_To_End_Tests_On_Windows Part1" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "End_To_End_Tests_On_Windows Part2" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
    }
    else
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "End_To_End_Tests_On_Windows Part1" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "End_To_End_Tests_On_Windows Part2" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
    }
    if($env:RunApexTests -eq "true")
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Apex Tests On Windows" -Status "pending" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "in progress"
    }
    else
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Apex Tests On Windows" -Status "success" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description "skipped"
    }
}

function SetCommitStatusForTestResult {
    param(
        [Parameter(Mandatory = $True)]
        [string]$PersonalAccessToken,
        [Parameter(Mandatory = $True)]
        [string]$TestName,
        [Parameter(Mandatory = $True)]
        [string]$CommitSha,
        [Parameter(Mandatory = $True)]
        [string]$VstsPersonalAccessToken
    )
    $testRun = Get-TestRun -TestName "NuGet.Client $TestName" -PersonalAccessToken $VstsPersonalAccessToken
    $url = $testRun[0]
    $failures = $testRun[1]
    if ($env:AGENT_JOBSTATUS -eq "Succeeded" -or $env:AGENT_JOBSTATUS -eq "SucceededWithIssues") {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName $TestName -Status "success" -CommitSha $CommitSha -TargetUrl $url -Description $env:AGENT_JOBSTATUS
    }
    else {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName $TestName -Status "error" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description $env:AGENT_JOBSTATUS
    }

    # If the build gets cancelled or fails when the unit tests are running , we also need to call the github api to update status
    # for mac, apex and e2e tests as they only run when the unit tests phase succeeds (or partially succeeds). If we don't do this,
    # the status for those tests will forever be in pending state.
    if(($env:AGENT_JOBSTATUS -eq "Failed" -or $env:AGENT_JOBSTATUS -eq "Canceled") -and $TestName -eq "Build_and_UnitTest_NonRTM")
    {
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Tests On Mac" -Status "failure" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description $env:AGENT_JOBSTATUS
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "End_To_End_Tests_On_Windows Part1" -Status "failure" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description $env:AGENT_JOBSTATUS
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "End_To_End_Tests_On_Windows Part2" -Status "failure" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description $env:AGENT_JOBSTATUS
        Update-GitCommitStatus -PersonalAccessToken $PersonalAccessToken -TestName "Apex Tests On Windows" -Status "failure" -CommitSha $CommitSha -TargetUrl $env:BUILDURL -Description $env:AGENT_JOBSTATUS
    }
}

function Get-TestRun {
    param(
        [Parameter(Mandatory = $True)]
        [string]$TestName,
        [Parameter(Mandatory = $True)]
        [string]$PersonalAccessToken
    )
    $url = "$env:VSTSTESTRUNSRESTAPI$env:BUILD_BUILDID"
    Write-Host $url
    $Token = ":$PersonalAccessToken"
    $Base64Token = [System.Convert]::ToBase64String([char[]]$Token)

    $Headers = @{
        Authorization = 'Basic {0}' -f $Base64Token;
    }

    $testRuns = Invoke-RestMethod -Uri $url -Method GET -Headers $Headers
    Write-Host $testRuns
    $matchingRun = $testRuns.value | where { $_.name -ieq $TestName }
    if(-not $matchingRun)
    {
        $testUrl = $env:BUILDURL
    }
    else
    {
        $testUrl = $env:VSTSTESTRUNURL -f $matchingRun.id
    }
    $failedTests = $matchingRun.unanalyzedTests
    return $testUrl,$failedTests
}

function CheckVstsPersonalAccessToken {
    param(
        [Parameter(Mandatory = $True)]
        [string]$VstsPersonalAccessToken
    )
    $url = "$env:VSTSTESTRUNSRESTAPI$env:BUILD_BUILDID"
    Write-Host "Checking $url"
    $Token = ":$VstsPersonalAccessToken"
    $Base64Token = [System.Convert]::ToBase64String([char[]]$Token)

    $Headers = @{
        Authorization = 'Basic {0}' -f $Base64Token;
    }

    try {
        # Basic Parsing prevents the need for Internet Explorer availability.
        $response = Invoke-WebRequest -Uri $url -Method GET -Headers $Headers -UseBasicParsing

        $StatusCode = $response.StatusCode

        if ($StatusCode -ne 200)
        {
            throw "The remote server returned HTTP status code $StatusCode"
        }

        Write-Host 'VstsPersonalAccessToken Valid! (HTTP Status Code: ' $StatusCode ')'
    }
    catch {
        $exceptionMessage = 'Invalid HTTP Status Code for VstsPersonalAccessToken ! ' + $PSItem.Exception.Message
        throw $exceptionMessage
    }
}
