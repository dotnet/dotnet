<#
.SYNOPSIS
Run a set of performance tests with a given client and a solution.

Both packages.config and PackageReference styles are supported, but mixed projects are not handled very well.

The scenarios are sequential as follows:

1. Clean restore - no http cache & other local caches, no files in the global package folder, absolutely everything gets downloaded and extracted.
1. Cold restore - There is only an http cache. This tells us more about the installation/extraction time. Potentially we might see some extra http calls depending on the project graph.
1. Force restore - The http cache & global packages folder are full. This usually means that there are no package downloads or installations happening. Since most tests are running 
1. NoOp restore

.PARAMETER nugetClientFilePath
The NuGet Client file path. Supported are dotnet.exe and NuGet.exe

.PARAMETER solutionFilePath
The solution file path. The entry point on which restore is called. It could be a project file as well.

.PARAMETER resultsFilePath
The results file path. This is an exact path to a file.

.PARAMETER logsFolderPath
The logs folder path. The default is a temp directory that gets cleaned up after the script has completed.

.PARAMETER nugetFoldersPath
The temp folder for all the nuget assets. This includes the location of the global packages folder, http, plugins cache & temp location 

.PARAMETER iterationCount
How many times to run each test. The default is 3

.PARAMETER isPackagesConfig
Specifies whether the solution is packages-config.

.PARAMETER skipWarmup
When running the tests, a warmup run is performed. Use this parameter to skip it.

.PARAMETER skipCleanRestores
Skips clean restores. 

.PARAMETER skipColdRestores
Skips cold restores

.PARAMETER skipForceRestores
Skips force restores

.PARAMETER skipNoOpRestores
Skips no-op restore.

.PARAMETER staticGraphRestore
Uses static graph restore if applicable for the client.

.EXAMPLE
.\RunPerformanceTests.ps1 -nugetClientFilePath "C:\Program Files\dotnet\dotnet.exe" -solutionFilePath F:\NuGet.Client\NuGet.sln -resultsFilePath results.csv
#>
Param(
    [Parameter(Mandatory = $True)]
    [string] $nugetClientFilePath,
    [Parameter(Mandatory = $True)]
    [string] $solutionFilePath,
    [Parameter(Mandatory = $True)]
    [string] $resultsFilePath,
    [string] $logsFolderPath,
    [string] $nugetFoldersPath,
    [int] $iterationCount = 3,
    [switch] $isPackagesConfig,
    [switch] $skipWarmup,
    [switch] $skipCleanRestores,
    [switch] $skipColdRestores,
    [switch] $skipForceRestores,
    [switch] $skipNoOpRestores,
    [switch] $staticGraphRestore
)

. "$PSScriptRoot\PerformanceTestUtilities.ps1"

Function CreateNugetClientArguments(
    [string] $solutionFilePath,
    [string] $nugetClientFilePath,
    [string] $resultsFilePath,
    [string] $logsFolderPath,
    [string] $solutionName,
    [string] $testRunId,
    [string] $scenarioName,
    [string[]] $enabledSwitches)
{
    $arguments = @{
        solutionFilePath = $solutionFilePath
        nugetClientFilePath = $nugetClientFilePath
        resultsFilePath = $resultsFilePath
        logsFolderPath = $logsFolderPath
        scenarioName = $scenarioName
        solutionName = $solutionName
        testRunId = $testRunId
    }

    If ($enabledSwitches -ne $Null)
    {
        ForEach ($enabledSwitch In $enabledSwitches.GetEnumerator())
        {
            $arguments[$enabledSwitch] = $True
        }
    }

    Return $arguments
}

Try
{
    ##### Script logic #####

    If (!(Test-Path $solutionFilePath))
    {
        Log "$solutionFilePath does not exist!" "Red"
        Exit 1
    }

    If (!(Test-Path $nugetClientFilePath))
    {
        Log "$nugetClientFilePath does not exist!" "Red"
        Exit 1
    }

    $nugetClientFilePath = GetAbsolutePath $nugetClientFilePath
    $solutionFilePath = GetAbsolutePath $solutionFilePath
    $resultsFilePath = GetAbsolutePath $resultsFilePath
    $isClientDotnetExe = IsClientDotnetExe $nugetClientFilePath

    
    If ($isPackagesConfig)
    {
        If ($isClientDotnetExe)
        {
            Log "dotnet.exe does not support packages.config restore." "Red"

            Exit 1
        }

        Log "Restores are expected to be packages.config restores."

        If (!$skipForceRestores)
        {
            Log "Force restore is not supported with packages.config.  Skipping force restores." "Yellow"
            $skipForceRestores = $True
        }
    }

    If (![string]::IsNullOrEmpty($logsFolderPath))
    {
        $logsFolderPath = GetAbsolutePath $logsFolderPath

        If ([System.IO.Path]::GetDirectoryName($resultsFilePath).StartsWith($logsFolderPath))
        {
            Log "$resultsFilePath cannot be under $logsFolderPath" "red"
            Exit 1
        }
    }

    If ([string]::IsNullOrEmpty($nugetFoldersPath))
    {
        $default = GetDefaultNuGetTestFolder
        $nugetFoldersPath = GetNuGetFoldersPath $default
    }

    LogDotNetSdkInfo

    if(Test-Path $resultsFilePath)
    {
        Log "The results file $resultsFilePath already exists. The test results of this run will be appended to the same file." "yellow"
    }

    # Setup the NuGet folders - This includes global packages folder/http/plugin caches
    SetupNuGetFolders $nugetClientFilePath $nugetFoldersPath

    $processorInfo = GetProcessorInfo

    Log "Measuring restore for $solutionFilePath by $nugetClientFilePath" "Green"

    $solutionName = [System.IO.Path]::GetFileNameWithoutExtension($solutionFilePath)
    $testRunId = [System.DateTime]::UtcNow.ToString("O")

    If (!$skipWarmup)
    {
        Log "Running 1x warmup restore"
        $enabledSwitches = @("cleanGlobalPackagesFolder", "cleanHttpCache", "cleanPluginsCache", "killMSBuildAndDotnetExeProcess")
        If ($isPackagesConfig)
        {
            $enabledSwitches += "isPackagesConfig"
        }
        If ($staticGraphRestore)
        {
            $enabledSwitches += "staticGraphRestore"
        }
        $arguments = CreateNugetClientArguments $solutionFilePath $nugetClientFilePath $resultsFilePath $logsFolderPath $solutionName $testRunId "warmup" -enabledSwitches $enabledSwitches
        RunRestore @arguments
    }

    If (!$skipCleanRestores)
    {
        Log "Running $($iterationCount)x clean restores"
        $enabledSwitches = @("cleanGlobalPackagesFolder", "cleanHttpCache", "cleanPluginsCache", "killMSBuildAndDotnetExeProcess")
        If ($isPackagesConfig)
        {
            $enabledSwitches += "isPackagesConfig"
        }
        If ($staticGraphRestore)
        {
            $enabledSwitches += "staticGraphRestore"
        }
        $arguments = CreateNugetClientArguments $solutionFilePath $nugetClientFilePath $resultsFilePath $logsFolderPath $solutionName $testRunId "arctic" -enabledSwitches $enabledSwitches
        1..$iterationCount | % { RunRestore @arguments }
    }

    If (!$skipColdRestores)
    {
        Log "Running $($iterationCount)x without a global packages folder"
        $enabledSwitches = @("cleanGlobalPackagesFolder", "killMSBuildAndDotnetExeProcess")
        If ($isPackagesConfig)
        {
            $enabledSwitches += "isPackagesConfig"
        }
        If ($staticGraphRestore)
        {
            $enabledSwitches += "staticGraphRestore"
        }
        $arguments = CreateNugetClientArguments $solutionFilePath $nugetClientFilePath $resultsFilePath $logsFolderPath $solutionName $testRunId "cold" -enabledSwitches $enabledSwitches
        1..$iterationCount | % { RunRestore @arguments }
    }

    If (!$skipForceRestores)
    {
        Log "Running $($iterationCount)x force restores"
        $enabledSwitches = @("force")
        If ($staticGraphRestore)
        {
            $enabledSwitches += "staticGraphRestore"
        }
        $arguments = CreateNugetClientArguments $solutionFilePath $nugetClientFilePath $resultsFilePath $logsFolderPath $solutionName $testRunId "force" -enabledSwitches $enabledSwitches
        1..$iterationCount | % { RunRestore @arguments }
    }

    If (!$skipNoOpRestores)
    {
        Log "Running $($iterationCount)x no-op restores"
        If ($staticGraphRestore)
        {
            $arguments = CreateNugetClientArguments $solutionFilePath $nugetClientFilePath $resultsFilePath $logsFolderPath $solutionName $testRunId "noop" -enabledSwitches @("staticGraphRestore")
        }
        Else
        {
            $arguments = CreateNugetClientArguments $solutionFilePath $nugetClientFilePath $resultsFilePath $logsFolderPath $solutionName $testRunId "noop"
        }
        1..$iterationCount | % { RunRestore @arguments }
    }

    Log "Completed the performance measurements for $solutionFilePath.  Results are in $resultsFilePath." "green"
}
Finally
{
    CleanNuGetFolders $nugetClientFilePath $nugetFoldersPath
}