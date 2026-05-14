param (
    [Parameter(Mandatory=$true)]
    [string]$PMCCommand,
    [Parameter(Mandatory=$true)]
    [int]$PMCLaunchWaitTimeInSecs,
    [Parameter(Mandatory=$true)]
    [int]$EachTestTimoutInSecs,
    [Parameter(Mandatory=$true)]
    [string]$NuGetDropPath,
    [Parameter(Mandatory=$true)]
    [string]$FuncTestRoot,
    [Parameter(Mandatory=$true)]
    [string]$RunCounter)

. "$PSScriptRoot\Utils.ps1"
. "$PSScriptRoot\VSUtils.ps1"
. "$PSScriptRoot\NuGetFunctionalTestUtils.ps1"

if ($env:CI -eq "true")
{
    $VSInstance = Get-LatestVSInstance
} else {
    $VSInstance = Get-LatestVSInstance -VersionRange (Get-VisualStudioVersionRangeFromConfig)
}

trap
{
    Write-Host "RunFunctionalTests.ps1 threw an exception: " -ForegroundColor Red
    Write-Error ($_.Exception | Format-List -Force | Out-String) -ErrorAction Continue
    Write-Error ($_.InvocationInfo | Format-List -Force | Out-String) -ErrorAction Continue
    KillRunningInstancesOfVS $VsInstance
    exit 1
}

$NuGetTestPath = Join-Path $FuncTestRoot "EndToEnd"

Write-Host 'NuGetTestPath is ' $NuGetTestPath

Write-Host 'Before starting the functional tests, force delete all the Results.html under the tests folder'
(Get-ChildItem $NuGetTestPath -Recurse Results.html) | Remove-Item -Force

CleanTempFolder


$dte2 = LaunchVSAndWaitForDTE -VSInstance $VSInstance -DTEReadyPollFrequencyInSecs 6 -NumberOfPolls 50
if (-not $dte2) {
    Write-Host 'Do the kill VS, Launch VS and wait for DTE one more time'
    $dte2 = LaunchVSAndWaitForDTE -VSInstance $VSInstance -DTEReadyPollFrequencyInSecs 6 -NumberOfPolls 50 -ActivityLogFullPath $env:ActivityLogFullPath
    if (-not $dte2) {
        Write-Error "Could not obtain DTE after waiting $NumberOfPolls * $DTEReadyPollFrequencyInSecs = " $NumberOfPolls * $DTEReadyPollFrequencyInSecs " secs"
        exit 1
    }
}

Write-Host "Launching the Package Manager Console inside VS and waiting for $PMCLaunchWaitTimeInSecs seconds"
ExecuteCommand $dte2 "View.PackageManagerConsole" $null "Opening NuGet Package Manager Console" $PMCLaunchWaitTimeInSecs

Write-Host "Set the execution policy on the process to be Bypass and wait for a second. This operation is very fast"
ExecuteCommand $dte2 "View.PackageManagerConsole" "Set-ExecutionPolicy Bypass -Scope Process -Force" "Running command: 'Set-ExecutionPolicy Bypass -Scope Process -Force' ..." 1

Write-Host "Remove any NuGet.Tests module that may have been loaded already and wait for a second. This operation is very fast"
ExecuteCommand $dte2 "View.PackageManagerConsole" "Get-Module NuGet.Tests | Remove-Module" "Running command: 'Get-Module NuGet.Tests | Remove-Module' ..." 1

$NuGetTestsModulePath = Join-Path $PSScriptRoot "NuGet.Tests.psd1"
if (-not (Test-Path $NuGetTestsModulePath))
{
    $NuGetTestsModulePath = Join-Path $NuGetTestPath "NuGet.Tests.psd1"
}

Write-Host "Import NuGet.Tests module from $NuGetTestPath and wait for 5 seconds."
ExecuteCommand $dte2 "View.PackageManagerConsole" "Import-Module $NuGetTestsModulePath" "Running command: 'Import-Module $NuGetTestsModulePath' ..." 5

Write-Host "Executing the provided Package manager console command: ""$PMCCommand"""
ExecuteCommand $dte2 "View.PackageManagerConsole" $PMCCommand "Running command: $PMCCommand ..."

Write-Host "Starting functional tests with command '$PMCCommand'"
RealTimeLogResults $NuGetTestPath $EachTestTimoutInSecs

KillRunningInstancesOfVS $VSInstance



Write-Host -ForegroundColor Cyan "THE END!"
