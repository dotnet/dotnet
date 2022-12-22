# Contains all the utility methods used by the performance tests.

# The format of the URL is assumed to be https://github.com/NuGet/NuGet.Client.git. The result would be NuGet-Client-git
function GenerateNameFromGitUrl([string]$gitUrl)
{
    return $gitUrl.Substring($($gitUrl.LastIndexOf('/') + 1)).Replace('.','-')
}

# Appends the log time in front of the log statement with the color specified. 
function Log([string]$logStatement, [string]$color)
{
    if([string]::IsNullOrEmpty($color))
    {
        Write-Host "$($(Get-Date).ToString()): $logStatement"
    }
    else
    { 
        Write-Host "$($(Get-Date).ToString()): $logStatement" -ForegroundColor $color
    }
}

# Given a relative path, gets the absolute path from the current directory
function GetAbsolutePath([string]$Path)
{
    $Path = [System.IO.Path]::Combine((pwd).Path, $Path);
    $Path = [System.IO.Path]::GetFullPath($Path);
    return $Path;
}

# Writes the content to the given path. Creates the folder structure if needed
function OutFileWithCreateFolders([string]$path, [string]$content)
{
    $folder = [System.IO.Path]::GetDirectoryName($path)
    If(!(Test-Path $folder))
    {
        & New-Item -ItemType Directory -Force -Path $folder > $null
    }
    Add-Content -Path $path -Value $content
}

# Gets a list of all the files recursively in the given folder
Function GetFiles(
    [Parameter(Mandatory = $True)]
    [string] $folderPath,
    [string] $pattern)
{
    If (Test-Path $folderPath)
    {
        $files = Get-ChildItem -Path $folderPath -Filter $pattern -Recurse -File

        Return $files
    }

    Return $Null
}

# Gets a list of all the nupkgs recursively in the given folder
Function GetPackageFiles(
    [Parameter(Mandatory = $True)]
    [string] $folderPath)
{
    Return GetFiles $folderPath "*.nupkg"
}

Function GetFilesInfo([System.IO.FileInfo[]] $files)
{
    If ($files -eq $Null)
    {
        $count = 0
        $totalSizeInMB = 0
    }
    Else
    {
        $count = $files.Count
        $totalSizeInMB = ($files | Measure-Object -Property Length -Sum).Sum / 1000000
    }

    Return @{
        Count = $count
        TotalSizeInMB = $totalSizeInMB
    }
}

# Determines if the client is dotnet.exe by checking the path.
function GetClientName([string]$nugetClient)
{
    return [System.IO.Path]::GetFileName($nugetClient)
}

function IsClientDotnetExe([string]$nugetClient)
{
    return $nugetClient.EndsWith("dotnet.exe")
}

function IsClientMSBuildExe([string]$nugetClient)
{
    return $nugetClient.EndsWith("MSBuild.exe", "CurrentCultureIgnoreCase")
}

# Downloads the repository at the given path.
Function DownloadRepository([string] $repository, [string] $commitHash, [string] $sourceFolderPath)
{
    If (Test-Path $sourceFolderPath)
    {
        Log "Skipping the cloning of $repository as $sourceFolderPath is not empty" -color "Yellow"
    }
    Else
    {
        git clone $repository $sourceFolderPath
        git -C $sourceFolderPath checkout $commitHash
    }
}

# Find the appropriate solution file for the repository. Looks for a solution file matching the repo name, 
# if not it takes the first available sln file in the repo. 
Function GetSolutionFilePath([string] $repository, [string] $sourceFolderPath)
{
    $gitRepoName = $repository.Substring($($repository.LastIndexOf('/') + 1))
    $potentialSolutionFilePath = [System.IO.Path]::Combine($sourceFolderPath, "$($gitRepoName.Substring(0, $gitRepoName.Length - 4)).sln")

    If (Test-Path $potentialSolutionFilePath)
    {
        $solutionFilePath = $potentialSolutionFilePath
    }
    Else
    {
        $possibleSln = Get-ChildItem $sourceFolderPath *.sln
        If ($possibleSln.Length -eq 0)
        {
            Log "No solution files found in $sourceFolderPath" "red"
        }
        Else
        {
            $solutionFilePath = $possibleSln[0] | Select-Object -f 1 | Select-Object -ExpandProperty FullName
        }
    }

    Return $solutionFilePath
}

# Given a repository and a hash, checks out the revision in the given source directory. The return is a solution file if found. 
Function SetupGitRepository([string] $repository, [string] $commitHash, [string] $sourceFolderPath)
{
    Log "Setting up $repository into $sourceFolderPath"
    DownloadRepository $repository $commitHash $sourceFolderPath
    $solutionFilePath = GetSolutionFilePath $repository $sourceFolderPath
    Log "Completed the repository setup. The solution file is $solutionFilePath" -color "Green"

    Return $solutionFilePath
}

# runs locals clear all with the given client
# If the client is msbuild.exe, the locals clear all *will* be run with dotnet.exe
Function LocalsClearAll([string] $nugetClientFilePath)
{
    $nugetClientFilePath = GetAbsolutePath $nugetClientFilePath
    If ($(IsClientDotnetExe $nugetClientFilePath))
    {
        . $nugetClientFilePath nuget locals -c all *>>$null
    }
    Elseif($(IsClientMSBuildExe $nugetClientFilePath))
    {
        . dotnet.exe nuget locals -c all *>>$null
    }
    Else
    {
        . $nugetClientFilePath locals -clear all -Verbosity quiet
    }
}

# Gets the client version
Function GetClientVersion([string] $nugetClientFilePath)
{
    $nugetClientFilePath = GetAbsolutePath $nugetClientFilePath

    If (IsClientDotnetExe $nugetClientFilePath)
    {
        $version = . $nugetClientFilePath --version
    }
    ElseIf($(IsClientMSBuildExe $nugetClientFilePath))
    {
        $clientDir = Split-Path -Path $nugetClientFilePath
        $nugetClientPath = Resolve-Path (Join-Path -Path $clientDir -ChildPath "../../../Common7/IDE/CommonExtensions/Microsoft/NuGet/NuGet.Build.Tasks.dll")
        $versionInfo = Get-ChildItem $nugetClientPath | % versioninfo | Select-Object FileVersion
        Return $(($versionInfo -split '\n')[0]).TrimStart("@{").TrimEnd('}').Substring("FileVersion=".Length)
    }
    Else
    {
        $output = . $nugetClientFilePath
        $version = $(($output -split '\n')[0]).Substring("NuGet Version: ".Length)
    }

    Return $version
}

# Gets the default test folder
Function GetDefaultNuGetTestFolder()
{
    Return $Env:UserProfile
}

# Gets the NuGet folders path where all of the discardable data from the tests will be put.
Function GetNuGetFoldersPath([string] $testFoldersPath)
{
    $nugetFoldersPath = [System.IO.Path]::Combine($testFoldersPath, "np")
    return GetAbsolutePath $nugetFoldersPath
}

# Sets up the global packages folder, http cache and plugin caches and cleans them before starting.
Function SetupNuGetFolders([string] $nugetClientFilePath, [string] $nugetFoldersPath)
{
    $Env:NUGET_PACKAGES = [System.IO.Path]::Combine($nugetFoldersPath, "gpf")
    $Env:NUGET_HTTP_CACHE_PATH = [System.IO.Path]::Combine($nugetFoldersPath, "hcp")
    $Env:NUGET_PLUGINS_CACHE_PATH = [System.IO.Path]::Combine($nugetFoldersPath, "pcp")

    # This environment variable is not recognized by any NuGet client.
    $Env:NUGET_SOLUTION_PACKAGES_FOLDER_PATH = [System.IO.Path]::Combine($nugetFoldersPath, "sp")
    $Env:DOTNET_MULTILEVEL_LOOKUP=0

    LocalsClearAll $nugetClientFilePath
}

# Cleanup the nuget folders and delete the nuget folders path.
# This should only be invoked by the the performance tests
Function CleanNuGetFolders([string] $nugetClientFilePath, [string] $nugetFoldersPath)
{
    Log "Cleanup up the NuGet folders - global packages folder, http/plugins caches. Client: $nugetClientFilePath. Folders: $nugetFoldersPath"

    LocalsClearAll $nugetClientFilePath

    Remove-Item $nugetFoldersPath -Recurse -Force -ErrorAction Ignore

    [Environment]::SetEnvironmentVariable("NUGET_PACKAGES", $Null)
    [Environment]::SetEnvironmentVariable("NUGET_HTTP_CACHE_PATH", $Null)
    [Environment]::SetEnvironmentVariable("NUGET_PLUGINS_CACHE_PATH", $Null)
    [Environment]::SetEnvironmentVariable("NUGET_SOLUTION_PACKAGES_FOLDER_PATH", $Null)
    [Environment]::SetEnvironmentVariable("NUGET_FOLDERS_PATH", $Null)
    [Environment]::SetEnvironmentVariable("DOTNET_MULTILEVEL_LOOKUP", $Null)
}

# Given a repository, a client and directories for the results/logs, runs the configured performance tests.
Function RunPerformanceTestsOnGitRepository(
    [string] $nugetClientFilePath,
    [string] $sourceRootFolderPath,
    [string] $testCaseName,
    [string] $repoUrl,
    [string] $commitHash,
    [string] $resultsFilePath,
    [string] $nugetFoldersPath,
    [string] $logsFolderPath,
    [int] $iterationCount,
    [switch] $staticGraphRestore)
{
    $solutionFilePath = SetupGitRepository -repository $repoUrl -commitHash $commitHash -sourceFolderPath $([System.IO.Path]::Combine($sourceRootFolderPath, $testCaseName))
    SetupNuGetFolders $nugetClientFilePath $nugetFoldersPath
    . "$PSScriptRoot\RunPerformanceTests.ps1" `
        -nugetClientFilePath $nugetClientFilePath `
        -solutionFilePath $solutionFilePath `
        -resultsFilePath $resultsFilePath `
        -logsFolderPath $logsFolderPath `
        -nugetFoldersPath $nugetFoldersPath `
        -iterationCount $iterationCount `
        -staticGraphRestore:$staticGraphRestore
}

Function GetProcessorInfo()
{
    $processorInfo = Get-WmiObject Win32_processor

    Return @{
        Name = $processorInfo | Select-Object -ExpandProperty Name
        NumberOfCores = $processorInfo | Select-Object -ExpandProperty NumberOfCores
        NumberOfLogicalProcessors = $processorInfo | Select-Object -ExpandProperty NumberOfLogicalProcessors
    }
}

Function LogDotNetSdkInfo()
{
    Try
    {
        $currentVersion = dotnet --version
        $currentSdk = dotnet --list-sdks | Where { $_.StartsWith("$currentVersion ") } | Select -First 1

        Log "Using .NET Core SDK $currentSdk."
    }
    Catch [System.Management.Automation.CommandNotFoundException]
    {
        Log ".NET Core SDK not found." -Color "Yellow"
    }
}

# Note:  System.TimeSpan rounds to the nearest millisecond.
Function ParseElapsedTime(
    [Parameter(Mandatory = $True)]
    [decimal] $value,
    [Parameter(Mandatory = $True)]
    [string] $unit)
{
    Switch ($unit)
    {
        "ms" { Return [System.TimeSpan]::FromMilliseconds($value) }
        "sec" { Return [System.TimeSpan]::FromSeconds($value) }
        "min" { Return [System.TimeSpan]::FromMinutes($value) }
        Default { throw "Unsupported unit of time:  $unit" }
    }
}

Function ExtractRestoreElapsedTime(
    [Parameter(Mandatory = $True)]
    [string[]] $lines)
{
    # All packages listed in packages.config are already installed.
    $prefix = "Restore completed in "

    $lines = $lines | Where { $_.IndexOf($prefix) -gt -1 }

    ForEach ($line In $lines)
    {
        $index = $line.IndexOf($prefix)

        $parts = $line.Substring($index + $prefix.Length).Split(' ', [System.StringSplitOptions]::RemoveEmptyEntries)

        $value = [System.Double]::Parse($parts[0])
        $unit = $parts[1]

        $temp = ParseElapsedTime $value $unit

        If ($elapsedTime -eq $Null -Or $elapsedTime -lt $temp)
        {
            $elapsedTime = $temp
        }
    }

    Return $elapsedTime
}

# Plugins cache is only available in 4.8+. We need to be careful when using that switch for older clients because it may blow up.
# The logs location is optional
Function RunRestore(
    [string] $solutionFilePath,
    [string] $nugetClientFilePath,
    [string] $resultsFile,
    [string] $logsFolderPath,
    [string] $scenarioName,
    [string] $solutionName,
    [string] $testRunId,
    [switch] $isPackagesConfig,
    [switch] $cleanGlobalPackagesFolder,
    [switch] $cleanHttpCache,
    [switch] $cleanPluginsCache,
    [switch] $killMsBuildAndDotnetExeProcesses,
    [switch] $force,
    [switch] $staticGraphRestore)
{
    $isClientDotnetExe = IsClientDotnetExe $nugetClientFilePath
    $isClientMSBuild = IsClientMSBuildExe $nugetClientFilePath

    If ($isClientDotnetExe -And $isPackagesConfig)
    {
        Log "dotnet.exe does not support packages.config restore." "Red"

        Return
    }

    Log "Running $nugetClientFilePath restore with cleanGlobalPackagesFolder:$cleanGlobalPackagesFolder cleanHttpCache:$cleanHttpCache cleanPluginsCache:$cleanPluginsCache killMsBuildAndDotnetExeProcesses:$killMsBuildAndDotnetExeProcesses force:$force"

    $solutionPackagesFolderPath = $Env:NUGET_SOLUTION_PACKAGES_FOLDER_PATH

    # Cleanup if necessary
    If ($cleanGlobalPackagesFolder -Or $cleanHttpCache -Or $cleanPluginsCache)
    {
        If ($cleanGlobalPackagesFolder -And $cleanHttpCache -And $cleanPluginsCache)
        {
            $localsArguments = "all"
        }
        ElseIf ($cleanGlobalPackagesFolder -And $cleanHttpCache)
        {
            $localsArguments = "http-cache global-packages"
        }
        ElseIf ($cleanGlobalPackagesFolder)
        {
            $localsArguments = "global-packages"
        }
        ElseIf ($cleanHttpCache)
        {
            $localsArguments = "http-cache"
        }
        Else
        {
            Log "Too risky to invoke a locals clear with the specified parameters." "yellow"
        }

        If ($isClientDotnetExe)
        {
            . $nugetClientFilePath nuget locals -c $localsArguments *>>$null
        }
        ElseIf($isClientMSBuild)
        {
            . dotnet.exe nuget locals -c $localsArguments *>>$null
        }
        Else
        {
            . $nugetClientFilePath locals -clear $localsArguments -Verbosity quiet
        }

        If ($isPackagesConfig -And ($cleanGlobalPackagesFolder -Or $cleanHttpCache))
        {
            Remove-Item $solutionPackagesFolderPath -Recurse -Force -ErrorAction Ignore > $Null
            mkdir $solutionPackagesFolderPath > $Null
        }
    }

    if($killMsBuildAndDotnetExeProcesses)
    {
        Stop-Process -name msbuild*,dotnet* -Force
    }

    $arguments = [System.Collections.Generic.List[string]]::new()

    If ($isClientMSBuild)
    {
        $arguments.Add("/t:restore")
    }
    Else 
    {
        $arguments.Add("restore")
    }
    $arguments.Add($solutionFilePath)

    If ($isPackagesConfig)
    {
        If ($isClientDotnetExe)
        {
            $arguments.Add("--packages")
        }
        Else
        {
            $arguments.Add("-PackagesDirectory")
        }

        $arguments.Add($Env:NUGET_SOLUTION_PACKAGES_FOLDER_PATH)
    }

    If ($force)
    {
        If ($isClientDotnetExe)
        {
            $arguments.Add("--force")
        }
        ElseIf($isClientMSBuild)
        {
            $arguments.Add("/p:RestoreForce=true")
        }
        Else
        {
            $arguments.Add("-Force")
        }
    }

    If (!$isClientDotnetExe -And !$isClientMSBuild)
    {
        $arguments.Add("-NonInteractive")
    }
    
    If($isClientDotnetExe -Or $isClientMSBuild)
    {   
        If ($staticGraphRestore)
        {
            $staticGraphOutputValue = "true"
            $arguments.Add("/p:RestoreUseStaticGraphEvaluation=true")
        }
        Else 
        {
            $staticGraphOutputValue = "false"
        }
    }
    Else 
    {
        $staticGraphOutputValue = "N/A"
    }

    $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

    $logs = . $nugetClientFilePath $arguments | Out-String
    if($LASTEXITCODE -ne 0)
    { 
        throw "The command `"$nugetClientFilePath $arguments`" finished with exit code $LASTEXITCODE.`n" + $logs
    }

    $totalTime = $stopwatch.Elapsed.TotalSeconds
    $restoreCoreTime = ExtractRestoreElapsedTime $logs

    If ($restoreCoreTime -ne $Null)
    {
        $restoreCoreTime = $restoreCoreTime.TotalSeconds
    }

    if(![string]::IsNullOrEmpty($logsFolderPath))
    {
        $logFile = [System.IO.Path]::Combine($logsFolderPath, "restoreLog-$([System.IO.Path]::GetFileNameWithoutExtension($solutionFilePath))-$(get-date -f yyyyMMddTHHmmssffff).txt")
        OutFileWithCreateFolders $logFile $logs
    }

    $folderPath = $Env:NUGET_PACKAGES
    $globalPackagesFolderNupkgFilesInfo = GetFilesInfo(GetPackageFiles $folderPath)
    $globalPackagesFolderFilesInfo = GetFilesInfo(GetFiles $folderPath)

    $folderPath = $Env:NUGET_HTTP_CACHE_PATH
    $httpCacheFilesInfo = GetFilesInfo(GetFiles $folderPath)

    $folderPath = $Env:NUGET_PLUGINS_CACHE_PATH
    $pluginsCacheFilesInfo = GetFilesInfo(GetFiles $folderPath)

    $clientName = GetClientName $nugetClientFilePath
    $clientVersion = GetClientVersion $nugetClientFilePath

    If (!(Test-Path $resultsFilePath))
    {
        $columnHeaders = "Client Name,Client Version,Solution Name,Test Run ID,Scenario Name,Total Time (seconds),Core Restore Time (seconds),Force,Static Graph," + `
            "Global Packages Folder .nupkg Count,Global Packages Folder .nupkg Size (MB),Global Packages Folder File Count,Global Packages Folder File Size (MB),Clean Global Packages Folder," + `
            "HTTP Cache File Count,HTTP Cache File Size (MB),Clean HTTP Cache,Plugins Cache File Count,Plugins Cache File Size (MB),Clean Plugins Cache,Kill MSBuild and dotnet Processes," + `
            "Processor Name,Processor Physical Core Count,Processor Logical Core Count"

        OutFileWithCreateFolders $resultsFilePath $columnHeaders
    }

    $data = "$clientName,$clientVersion,$solutionName,$testRunId,$scenarioName,$totalTime,$restoreCoreTime,$force,$staticGraphOutputValue," + `
        "$($globalPackagesFolderNupkgFilesInfo.Count),$($globalPackagesFolderNupkgFilesInfo.TotalSizeInMB),$($globalPackagesFolderFilesInfo.Count),$($globalPackagesFolderFilesInfo.TotalSizeInMB),$cleanGlobalPackagesFolder," + `
        "$($httpCacheFilesInfo.Count),$($httpCacheFilesInfo.TotalSizeInMB),$cleanHttpCache,$($pluginsCacheFilesInfo.Count),$($pluginsCacheFilesInfo.TotalSizeInMB),$cleanPluginsCache,$killMsBuildAndDotnetExeProcesses," + `
        "$($processorInfo.Name),$($processorInfo.NumberOfCores),$($processorInfo.NumberOfLogicalProcessors)"

    Add-Content -Path $resultsFilePath -Value $data

    Log "Finished measuring."
}