# Copyright (c) Microsoft. All rights reserved.
# Test script for Test Platform.

[CmdletBinding()]
Param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("Debug", "Release")]
    [Alias("c")]
    [System.String] $Configuration = "Debug",

    [Parameter(Mandatory=$false)]
    [ValidateSet("win7-x64", "win-x86")]
    [Alias("r")]
    [System.String] $TargetRuntime = "win7-x64",

    [Parameter(Mandatory=$false)]
    [ValidateSet("net48", "net9.0")]
    [Alias("f")]
    [System.String] $TargetFramework,

    # Only test sources matching the pattern are run.
    # Use End2End to run E2E tests. Or to run any one assembly tests, use the
    # assembly name. E.g. test -p Microsoft.TestPlatform.CoreUtilities.UnitTests
    [Parameter(Mandatory=$false)]
    [Alias("p")]
    [System.String] $Pattern = "Unit",

    # Stop test run on first failure
    [Parameter(Mandatory=$false)]
    [Alias("ff")]
    [Switch] $FailFast = $false,

    [Parameter(Mandatory=$false)]
    [Switch] $Parallel = $true,

    # Only test cases matching the filter are run. This input is converted to
    # /testCaseFilter:<filter>
    [Parameter(Mandatory=$false)]
    [System.String] $Filter
)

function Get-DotNetPath
{
    $dotnetPath = Join-Path $env:TP_TOOLS_DIR "dotnet\dotnet.exe"
    if (-not (Test-Path $dotnetPath)) {
        Write-Error "Dotnet.exe not found at $dotnetPath. Did the dotnet cli installation succeed?"
    }

    return $dotnetPath
}

$ErrorActionPreference = "Stop"

#
# Variables
#
Write-Verbose "Setup environment variables."
$env:TP_ROOT_DIR = (Get-Item (Split-Path $MyInvocation.MyCommand.Path)).Parent.FullName
$env:TP_TOOLS_DIR = Join-Path $env:TP_ROOT_DIR "tools"
$env:TP_PACKAGES_DIR = Join-Path $env:TP_ROOT_DIR "packages"
$env:TP_OUT_DIR = Join-Path $env:TP_ROOT_DIR "artifacts"

Write-Verbose "Setup dotnet configuration."
# Add latest dotnet.exe directory to environment variable PATH to tests run on latest dotnet.
$env:PATH = "$(Split-Path $(Get-DotNetPath));$env:PATH"

# set the roots to use only the versions of dotnet that we installed
$env:DOTNET_ROOT = Join-Path $env:TP_TOOLS_DIR "dotnet"
# set the root for x86 runtimes as well
${env:DOTNET_ROOT(x86)} = "${env:DOTNET_ROOT}_x86"
# disable looking up other dotnets in programfiles
$env:DOTNET_MULTILEVEL_LOOKUP = 0

# Disable first run since we want to control all package sources
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 1

# list what we have set and what is available
"---- dotnet environment variables"
Get-ChildItem "Env:\dotnet_*"

"`n`n---- x64 dotnet"
& "$env:DOTNET_ROOT\dotnet.exe" --info

"`n`n---- x86 dotnet"
# avoid erroring out because we don't have the sdk for x86 that global.json requires
try {
    & "${env:DOTNET_ROOT(x86)}\dotnet.exe" --info 2> $null
} catch {}


# Dotnet build doesn't support --packages yet. See https://github.com/dotnet/cli/issues/2712
$env:NUGET_PACKAGES = $env:TP_PACKAGES_DIR

#
# Test configuration
#
$TPT_TargetFrameworkNet462 = "net462"
$TPT_TargetFrameworkNet48 = "net48"
$TPT_TargetFrameworkNet80 = "net8.0"
$TPT_TargetFrameworkNet90 = "net9.0"
Write-Verbose "Setup build configuration."
$Script:TPT_Configuration = $Configuration
$Script:TPT_SourceFolders =  @("test")
$Script:TPT_TargetFrameworks =@($TPT_TargetFrameworkNet48, $TPT_TargetFrameworkNet90)
$Script:TPT_TargetFramework = $TargetFramework
$Script:TPT_TargetRuntime = $TargetRuntime
$Script:TPT_SkipProjects = @("_none_");
$Script:TPT_Pattern = $Pattern
$Script:TPT_TestFilter = $Filter
$Script:TPT_FailFast = $FailFast
$Script:TPT_Parallel = $Parallel
$Script:TPT_TestResultsDir = Join-Path $env:TP_ROOT_DIR "TestResults"
$Script:TPT_DefaultTrxFileName = "TrxLogResults.trx"
$Script:TPT_ErrorMsgColor = "Red"
$Script:TPT_RunSettingsFile = Join-Path (Get-Item (Split-Path $MyInvocation.MyCommand.Path)) "vstest-codecoverage.runsettings"
$Script:TPT_NSTraceDataCollectorPath = Join-Path $env:TP_OUT_DIR "$Script:TPT_Configuration\Microsoft.CodeCoverage"

#
# Capture error state in any step globally to modify return code
$Script:ScriptFailed = $false

function Write-Log ([string] $message, $messageColor = "Green")
{
    if ($message)
    {
        Write-Host "... $message" -ForegroundColor $messageColor
    }
}

function Write-VerboseLog([string] $message)
{
    Write-Verbose $message
}

function Print-FailedTests($TrxFilePath)
{
    if(![System.IO.File]::Exists($TrxFilePath))
    {
      Write-Log "TrxFile: $TrxFilePath doesn't exists"
      return
    }

    $xdoc = [xml] (get-content $TrxFilePath)
    $FailedTestCaseDetailsDict = @{}
    # Get failed testcase data from UnitTestResult tag.
    $xdoc.TestRun.Results.UnitTestResult | ? { $_.GetAttribute("outcome") -eq "Failed" } | % {
        $FailedTestCaseDetailsDict.Add($_.testId, @{"Message" = $_.Output.ErrorInfo.Message; "StackTrace" = $_.Output.ErrorInfo.StackTrace; "StdOut"=$_.Output.StdOut});
    }

    if ($FailedTestCaseDetailsDict.Count -ne 0)
    {
        Write-Log ".. . Failed tests:" $Script:TPT_ErrorMsgColor
        # Print failed test details.
        $count = 1
        $nl = [Environment]::NewLine
        $xdoc.TestRun.TestDefinitions.UnitTest |?{$FailedTestCaseDetailsDict.ContainsKey($_.id)} | %{
            Write-Log (".. .. . $count. " + "$($_.TestMethod.className).$($_.TestMethod.name)") $Script:TPT_ErrorMsgColor
            Write-Log (".. .. .. .ErrorMessage: $nl" + $FailedTestCaseDetailsDict[$_.id]["Message"]) $Script:TPT_ErrorMsgColor
            Write-Log (".. .. .. .StackTrace: $nl" + $FailedTestCaseDetailsDict[$_.id]["StackTrace"]) $Script:TPT_ErrorMsgColor
            Write-Log (".. .. .. .StdOut: $nl" + $FailedTestCaseDetailsDict[$_.id]["StdOut"]) $Script:TPT_ErrorMsgColor
            $count++
        }

        Set-ScriptFailed
        if ($Script:TPT_FailFast)
        {
            Write-Log ".. Stop execution since fail fast is enabled."
            continue
        }
    }
}

function Invoke-Test
{
    $timer = Start-Timer
    $dotNetPath = Get-DotNetPath

    Write-Log "Invoke-Test: Start test."

    foreach ($src in $Script:TPT_SourceFolders)
    {
        Write-Log ".. Test: Computing sources"
        Get-ChildItem -Recurse -Path $src -Include *.csproj | Where-Object { $_.FullName -inotmatch "TestAssets" } | ForEach-Object {
            $testContainerName = $_.BaseName
            $testOutputPath = Join-Path $_.Directory.FullName "bin/$($Script:TPT_Configuration)/{0}"
            $testContainerPath = Join-Path $testOutputPath "$($testContainerName).dll"

            $skip = "False"

            foreach ($project in $Script:TPT_SkipProjects)
            {
               if($_.Name.Contains($project))
               {
                   $skip="True"
                   break
               }
            }

            if ($skip -eq "True")
            {
                 Write-Log ".. . $testContainerName is in skipped test list."
            }
            elseif (!($testContainerName -match $Script:TPT_Pattern))
            {
                 Write-Log ".. . $testContainerName doesn't match test container pattern '$($Script:TPT_Pattern)'. Skipped from run."
            }
            else
            {
                Write-Log ".. . $testContainerName test container found. ($testContainerPath)"
                $testContainers += ,"$testContainerPath"
            }
        }

        # Invoke test for each project since we want a custom output path
        foreach ($fx in $Script:TPT_TargetFrameworks)
        {
            Write-Log ".. Start run ($fx)"
            if ($Script:TPT_TargetFramework -ne "" -and $fx -ne $Script:TPT_TargetFramework)
            {
                Write-Log ".. . Skipped framework based on user setting."
                continue;
            }

            # Tests are only built for x86 at the moment, though we don't have architecture requirement
            if (-not [System.String]::IsNullOrEmpty($TPT_TestFilter))
            {
                $testFilter = "/testCaseFilter:`"$TPT_TestFilter`""
            }

            if($fx -eq $TPT_TargetFrameworkNet90)
            {
                $vstestConsoleFileName = "vstest.console.dll"
                $targetRunTime = ""
                $vstestConsolePath = Join-Path (Get-PackageDirectory $TPT_TargetFrameworkNet80 $targetRuntime) $vstestConsoleFileName
            }
            else
            {
                $vstestConsoleFileName = "vstest.console.exe"
                $targetRunTime = $Script:TPT_TargetRuntime
                $vstestConsolePath = Join-Path (Get-PackageDirectory $TPT_TargetFrameworkNet462 $targetRuntime) $vstestConsoleFileName
            }

            if (!(Test-Path $vstestConsolePath))
            {
                Write-Log "Unable to find $vstestConsoleFileName at $vstestConsolePath. Did you run build.cmd?"
                Write-Error "Test aborted."
            }

            if ($TPT_Parallel)
            {
                # Fill in the framework in test containers
                $testContainerSet = $testContainers | % {
                    $testContainerPath = [System.String]::Format($_, $fx)
                    if (Test-Path $testContainerPath)
                    {
                        $testContainerPath
                    }
                }
                $trxLogFileName  =  [System.String]::Format("Parallel_{0}_{1}_{2}", $TPT_Pattern, $fx, $Script:TPT_DefaultTrxFileName)

                # Remove already existed trx file name as due to which warning will get generated and since we are expecting result in a particular format, that will break
                $fullTrxFilePath = Join-Path $Script:TPT_TestResultsDir $trxLogFileName
                if([System.IO.File]::Exists($fullTrxFilePath))
                {
                    Remove-Item $fullTrxFilePath
                }

                Set-TestEnvironment
                if($fx -eq $TPT_TargetFrameworkNet48)
                {

                    Write-Verbose "$vstestConsolePath $testContainerSet /parallel /logger:`"trx;LogFileName=$trxLogFileName`" $testFilter $ConsoleLogger"
                    & $vstestConsolePath $testContainerSet /parallel /logger:"trx;LogFileName=$trxLogFileName" $testFilter $ConsoleLogger
                }
                else
                {

                    Write-Verbose "$dotNetPath $vstestConsolePath $testContainerSet /parallel /logger:`"trx;LogFileName=$trxLogFileName`" $testFilter /settings:$Script:TPT_RunSettingsFile $ConsoleLogger /testadapterpath:$Script:TPT_NSTraceDataCollectorPath"
                    & $dotNetPath $vstestConsolePath $testContainerSet /parallel /logger:"trx;LogFileName=$trxLogFileName" $testFilter /settings:"$Script:TPT_RunSettingsFile" $ConsoleLogger /testadapterpath:"$Script:TPT_NSTraceDataCollectorPath"
                }

                Reset-TestEnvironment
                Print-FailedTests (Join-Path $Script:TPT_TestResultsDir $trxLogFileName)
            }
            else
            {
                $testContainers |  % {
                    # Fill in the framework in test containers
                    $testContainer = [System.String]::Format($_, $fx)

                    if (-not (Test-Path $testContainer))
                    {
                        # Test project may not targetting all frameworks. Example: Microsoft.TestPlatform.Build.UnitTests won't target net451.
                        return
                    }

                    $trxLogFileName =  [System.String]::Format("{0}_{1}_{2}", ($(Get-ChildItem $testContainer).Name), $fx, $Script:TPT_DefaultTrxFileName)

                    # Remove already existed trx file name as due to which warning will get generated and since we are expecting result in a particular format, that will break
                    $fullTrxFilePath = Join-Path $Script:TPT_TestResultsDir $trxLogFileName
                    if([System.IO.File]::Exists($fullTrxFilePath))
                    {
                        Remove-Item $fullTrxFilePath
                    }

                    Write-Log ".. Container: $testContainer"

                    Set-TestEnvironment

                    if($fx -eq $TPT_TargetFrameworkNet48)
                    {

                        Write-Verbose "$vstestConsolePath $testContainer /logger:`"trx;LogFileName=$trxLogFileName`" $ConsoleLogger $testFilter"
                        & $vstestConsolePath $testContainer /logger:"trx;LogFileName=$trxLogFileName" $ConsoleLogger $testFilter
                    }
                    else
                    {
                        Write-Verbose "$dotNetPath $vstestConsolePath $testContainer /logger:`"trx;LogFileName=$trxLogFileName`" $ConsoleLogger $testFilter"
                        & $dotNetPath $vstestConsolePath $testContainer /logger:"trx;LogFileName=$trxLogFileName" $ConsoleLogger $testFilter
                    }

                    Reset-TestEnvironment
                    Print-FailedTests (Join-Path $Script:TPT_TestResultsDir $trxLogFileName)
                }
            }

            Write-Log ".. Test: Complete ($fx)."
        }
    }

    Write-Log "Invoke-Test: Complete. {$(Get-ElapsedTime($timer))}"
}

#
# Helper functions
#
function Get-PackageDirectory($framework, $targetRuntime)
{
    return $(Join-Path $env:TP_OUT_DIR "$($Script:TPT_Configuration)\$($framework)\$($targetRuntime)")
}

function Start-Timer
{
    return [System.Diagnostics.Stopwatch]::StartNew()
}

function Get-ElapsedTime([System.Diagnostics.Stopwatch] $timer)
{
    $timer.Stop()
    return $timer.Elapsed
}

function Set-ScriptFailed
{
    $Script:ScriptFailed = $true
}

function Set-TestEnvironment
{
    $env:TPT_TargetFramework = $Script:TPT_TargetFramework
    $env:TPT_TargetRuntime = $Script:TPT_TargetRuntime
}

function Reset-TestEnvironment
{
    $env:TPT_TargetFramework = $null
    $env:TPT_TargetRuntime = $null
}

# Execute build
$timer = Start-Timer
Write-Log "Test started: args = '$args'"
Write-Log "Test platform environment variables: "
Get-ChildItem env: | Where-Object -FilterScript { $_.Name.StartsWith("TP_") } | Format-Table

Write-Log "Test run configuration: "
Get-Variable | Where-Object -FilterScript { $_.Name.StartsWith("TPT_") } | Format-Table

$ConsoleLogger = if ($VerbosePreference -eq "Continue") {'/logger:"console;verbosity=detailed"'} else {'/logger:"console;verbosity=minimal"'}

Invoke-Test

Write-Log "Test complete. {$(Get-ElapsedTime($timer))}"

if ($Script:ScriptFailed) { Exit 1 } else { Exit 0 }
