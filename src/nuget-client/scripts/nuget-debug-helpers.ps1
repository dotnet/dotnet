$NuGetClientRoot= Resolve-Path $(Join-Path $PSScriptRoot "..\")

$Configuration = "Debug"
$NETFramework = "net472"
$NETStandard = "netstandard2.0"
$NETCoreApp = "net9.0"

<#
Auto bootstraps NuGet for debugging the targets. This includes both restore and pack and is the recommended way to test things :)

.EXAMPLE
    This runs restore on the NuGet.sln. The args here are equivalent to the args you would pass to an MSBuild invocation.
    C:\PS> Invoke-NuGetCustom /t:restore NuGet.sln
#>
Function Invoke-NuGetCustom()
{
    $packDllPath = Join-Path $NuGetClientRoot "artifacts\NuGet.Build.Tasks.Pack\bin\$Configuration\$NETFramework\NuGet.Build.Tasks.Pack.dll"
    $packTargetsPath = Join-Path $NuGetClientRoot "src\NuGet.Core\NuGet.Build.Tasks\NuGet.Build.Tasks.Pack.targets"
    $restoreDllPath = Join-Path $NuGetClientRoot "artifacts\NuGet.Build.Tasks\bin\$Configuration\$NETFramework\NuGet.Build.Tasks.dll"
    $nugetRestoreTargetsPath = Join-Path $NuGetClientRoot "src\NuGet.Core\NuGet.Build.Tasks\NuGet.targets"
    $nugetPropsPath = Join-Path $NuGetClientRoot "src\NuGet.Core\NuGet.Build.Tasks\NuGet.props"
    $consoleExePath = Join-Path $NuGetClientRoot "artifacts\NuGet.Build.Tasks.Console\bin\$Configuration\$NETFramework\NuGet.Build.Tasks.Console.exe"
    Write-Host "msbuild /p:NuGetRestoreTargets=$nugetRestoreTargetsPath /p:NuGetPropsFile=$nugetPropsPath /p:RestoreTaskAssemblyFile=$restoreDllPath /p:NuGetBuildTasksPackTargets=$packTargetsPath /p:NuGetConsoleProcessFileName=$consoleExePath /p:ImportNuGetBuildTasksPackTargetsFromSdk=true /p:NuGetPackTaskAssemblyFile=$packDllPath $($args[0..$args.Count])"
    & msbuild /p:NuGetRestoreTargets=$nugetRestoreTargetsPath /p:NuGetPropsFile=$nugetPropsPath /p:RestoreTaskAssemblyFile=$restoreDllPath /p:NuGetBuildTasksPackTargets=$packTargetsPath /p:NuGetConsoleProcessFileName=$consoleExePath /p:ImportNuGetBuildTasksPackTargetsFromSdk=true /p:NuGetPackTaskAssemblyFile=$packDllPath $args[0..$args.Count]
}

<#
Auto bootstraps NuGet for debugging the restore targets only (this doesn't include the pack targets!)
.EXAMPLE
    This runs restore on the NuGet.sln. The args here are equivalent to the args you would pass to an MSBuild invocation.
    C:\PS> Invoke-NuGetCustom /t:restore NuGet.sln
#>
Function Invoke-NuGetRestoreCustom()
{
    $restoreDllPath = Join-Path $NuGetClientRoot "artifacts\NuGet.Build.Tasks\bin\$Configuration\$NETFramework\NuGet.Build.Tasks.dll"
    $nugetRestoreTargetsPath = Join-Path $NuGetClientRoot "src\NuGet.Core\NuGet.Build.Tasks\NuGet.targets"
    $nugetPropsPath = Join-Path $NuGetClientRoot "src\NuGet.Core\NuGet.Build.Tasks\NuGet.props"
    $consoleExePath = Join-Path $NuGetClientRoot "artifacts\NuGet.Build.Tasks.Console\bin\$Configuration\$NETFramework\NuGet.Build.Console.exe"
    Write-Host "msbuild /p:NuGetRestoreTargets=$nugetRestoreTargetsPath /p:NuGetPropsFile=$nugetPropsPath /p:RestoreTaskAssemblyFile=$restoreDllPath /p:NuGetConsoleProcessFileName=$consoleExePath $($args[0..$args.Count])"
    & msbuild /p:NuGetRestoreTargets=$nugetRestoreTargetsPath /p:NuGetPropsFile=$nugetPropsPath /p:RestoreTaskAssemblyFile=$restoreDllPath /p:NuGetConsoleProcessFileName=$consoleExePath $args[0..$args.Count]
}

<#
Auto bootstraps NuGet for debugging the pack targets only (this doesn't include the restore targets!)
.EXAMPLE
    This runs pack on the NuGet.sln. The args here are equivalent to the args you would pass to an MSBuild invocation.
    C:\PS> Invoke-NuGetPackCustom /t:pack NuGet.sln
#>
Function Invoke-NuGetPackCustom()
{
    $packDllPath = Join-Path $NuGetClientRoot "artifacts\NuGet.Build.Tasks.Pack\bin\$Configuration\$NETFramework\NuGet.Build.Tasks.Pack.dll"
    $packTargetsPath = Join-Path $NuGetClientRoot "src\NuGet.Core\NuGet.Build.Tasks\NuGet.Build.Tasks.Pack.targets"
    Write-Host "msbuild /p:NuGetBuildTasksPackTargets=$packTargetsPath /p:ImportNuGetBuildTasksPackTargetsFromSdk=true /p:NuGetPackTaskAssemblyFile=$packDllPath $($args[0..$args.Count])"
    & msbuild /p:NuGetBuildTasksPackTargets=$packTargetsPath /p:ImportNuGetBuildTasksPackTargetsFromSdk=true /p:NuGetPackTaskAssemblyFile=$packDllPath $args[0..$args.Count]
}

<#
Patches the necessary NuGet assemblies in dotnet.exe.
.EXAMPLE
    C:\PS> Add-NuGetToCLI -sdkLocation D:\dotnet\sdk\5.0.0-preview4\
#>
Function Add-NuGetToCLI {
    param
    (
        [string]$sdkLocation,
        [switch]$createSdkLocation
    )

    if ( (-Not $createSdkLocation) -And (-Not (Test-Path $sdkLocation)))
    {
        Write-Error "The SDK path $sdkLocation does not exist!"
        return;
    }

    if($createSdkLocation)
    {
        if(-Not (Test-Path $sdkLocation))
        {
            New-Item $sdkLocation -ItemType Directory
        }
    }
    $sdk_path = $sdkLocation

    $locFolders = @('cs', 'de', 'es', 'fr', 'it', 'ja', 'ko', 'pl', 'pt-BR', 'ru', 'tr', 'zh-Hans', 'zh-Hant')

    $nugetXplatArtifactsPath = [System.IO.Path]::Combine($NuGetClientRoot, 'artifacts', 'NuGet.CommandLine.XPlat', 'bin', $Configuration, $NETCoreApp)
    $nugetBuildTasks = [System.IO.Path]::Combine($NuGetClientRoot, 'artifacts', 'NuGet.Build.Tasks', 'bin', $Configuration, $NETCoreApp, 'NuGet.Build.Tasks.dll')
    $nugetBuildTasksConsole = [System.IO.Path]::Combine($NuGetClientRoot, 'artifacts', 'NuGet.Build.Tasks.Console', 'bin', $Configuration, $NETCoreApp, 'NuGet.Build.Tasks.Console.dll')
    $nugetTargets = [System.IO.Path]::Combine($NuGetClientRoot, 'src', 'NuGet.Core', 'NuGet.Build.Tasks', 'NuGet.targets')
    $nugetExTargets = [System.IO.Path]::Combine($NuGetClientRoot, 'src', 'NuGet.Core', 'NuGet.Build.Tasks', 'NuGet.RestoreEx.targets')
    $nugetPackCoreTasks = [System.IO.Path]::Combine($NuGetClientRoot, 'artifacts', 'NuGet.Build.Tasks.Pack', 'bin', $Configuration, $NETCoreApp,"NuGet.Build.Tasks.Pack.dll")
    $nugetPackFrameworkTasks = [System.IO.Path]::Combine($NuGetClientRoot, 'artifacts', 'NuGet.Build.Tasks.Pack', 'bin', $Configuration, $NETFramework, "NuGet.Build.Tasks.Pack.dll")
    $nugetPackTargets = [System.IO.Path]::Combine($NuGetClientRoot, 'src', 'NuGet.Core', 'NuGet.Build.Tasks', 'NuGet.Build.Tasks.Pack.targets')
    $msbuildSdkResolverTasks = [System.IO.Path]::Combine($NuGetClientRoot, 'artifacts', 'Microsoft.Build.NuGetSdkResolver', 'bin', $Configuration, $NETCoreApp, 'Microsoft.Build.NuGetSdkResolver.dll')


    if (-Not (Test-Path $nugetXplatArtifactsPath)) {
        Write-Error "$nugetXplatArtifactsPath not found!"
        return;
    }

    if (-Not (Test-Path $nugetBuildTasks)) {
        Write-Error "$nugetBuildTasks not found!"
        return;
    }

    if (-Not (Test-Path $nugetBuildTasksConsole)) {
        Write-Error "$nugetBuildTasksConsole not found!"
        return;
    }

    if (-Not (Test-Path $nugetTargets)) {
        Write-Error "$nugetTargets not found!"
        return;
    }

    if (-Not (Test-Path $nugetExTargets)) {
        Write-Error "$nugetExTargets not found!"
        return;
    }

    if (-Not (Test-Path $nugetPackCoreTasks)) {
        Write-Error "$nugetPackCoreTasks not found!"
        return;
    }

    if (-Not (Test-Path $nugetPackFrameworkTasks)) {
        Write-Error "$nugetPackFrameworkTasks not found!"
        return;
    }

    if (-Not (Test-Path $nugetPackTargets)) {
        Write-Error "$nugetPackTargets not found!"
        return;
    }

    if (-Not (Test-Path $msbuildSdkResolverTasks)) {
        Write-Error "$msbuildSdkResolverTasks not found!"
        return;
    }

    Write-Host
    Write-Host "Source commandline path - $nugetXplatArtifactsPath"
    Write-Host "Destination sdk path - $sdk_path"
    Write-Host

    ## Copy the xplat artifacts

    Write-Debug "Artifacts: $nugetXplatArtifactsPath"
    Get-ChildItem -Recurse $nugetXplatArtifactsPath -Filter NuGet*.dll |
        Foreach-Object {
            $currDir = $_.Directory.BaseName
            Write-Debug "Parent $currDir" 
            if ($locFolders -contains $currDir)
            {
                $new_position = "$($sdk_path)\$($currDir)\$($_.BaseName )$($_.Extension )"
            }
            else
            {
                $new_position = "$($sdk_path)\$($_.BaseName )$($_.Extension )"
            }

            Write-Debug "Moving $($_.FullName) to - $($new_position)"
            Write-Host "Moving to - $($new_position)"
            Copy-Item $_.FullName $new_position
        }

    ## Copy the  restore artifacts

    $buildTasksDest = "$($sdk_path)\NuGet.Build.Tasks.dll"
    Write-Host "Moving to - $($buildTasksDest)"
    Copy-Item $nugetBuildTasks $buildTasksDest

    $buildTasksConsoleDest = "$($sdk_path)\NuGet.Build.Tasks.Console.dll"
    Write-Host "Moving to - $($buildTasksConsoleDest)"
    Copy-Item $nugetBuildTasksConsole $buildTasksConsoleDest

    $nugetTargetsDest = "$($sdk_path)\NuGet.targets"
    Write-Host "Moving to - $($nugetTargetsDest)"
    Copy-Item $nugetTargets $nugetTargetsDest

    $nugetRestoreExTargetsDest = "$($sdk_path)\NuGet.RestoreEx.targets"
    Write-Host "Moving to - $($nugetRestoreExTargetsDest)"
    Copy-Item $nugetExTargets $nugetRestoreExTargetsDest

    ## Copy the pack artifacts.

    $nugetPackTargetsDest = "$($sdk_path)\NuGet.Build.Tasks.Pack.targets"
    Write-Host "Moving to - $($nugetPackTargetsDest)"
    Copy-Item $nugetPackTargets $nugetPackTargetsDest

    $packTasksCoreDest = "$($sdk_path)\NuGet.Build.Tasks.Pack.dll"
    Write-Host "Moving to - $($packTasksCoreDest)"
    Copy-Item $nugetPackCoreTasks $packTasksCoreDest

    $packTasksFrameworkDest = "$($sdk_path)\Sdks\Microsoft.NET.Sdk\tools\net472\NuGet.Build.Tasks.Pack.dll"
    Write-Host "Moving to - $($packTasksFrameworkDest)"
    Copy-Item $nugetPackFrameworkTasks $packTasksFrameworkDest

    ## Copy the resolver

    $msbuildSdkResolverTasksDest = "$($sdk_path)\Microsoft.Build.NuGetSdkResolver.dll"
    Write-Host "Moving to - $($msbuildSdkResolverTasksDest)"
    Copy-Item $msbuildSdkResolverTasks $msbuildSdkResolverTasksDest
}

<#
Downgrades the NuGet extension in all Visual Studio instances on the machine.
Assumes the VSIXInstaller.exe is on the path.
#>
Function DowngradeNuGetVsix()
{
    & VSIXInstaller.exe /d:NuGet.72c5d240-f742-48d4-a0f1-7016671e405b
}

<#
Installs the NuGet extension build in the local repo.
#>
Function InstallCustomNuGetVsix()
{
    & VSIXInstaller.exe $(Join-Path $NuGetClientRoot "artifacts\VS15\NuGet.Tools.vsix" )
}

<#
Enable Visual Studio telemetry file logging.
#>
Function EnableVisualStudioTelemetryFileLogging()
{
    $command = "reg add HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\Telemetry\Channels /v fileLogger /t reg_dword /f /d 00000001"
    Invoke-Expression $command
}

<#
Disable Visual Studio telemetry file logging
#>
Function DisableVisualStudioTelemetryFileLogging()
{
    $command = "reg delete HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\Telemetry\Channels /v fileLogger /f"
    Invoke-Expression $command
}

<#
Isolate the restore output folders. When run, this redirects all NuGet folders and caches to the directory passed in as $RootDirectory.
The root folder *must* exist.
.EXAMPLE
    C:\PS> IsolateRestore -sdkLocation D:\source\NuGet.Client
.EXAMPLE
    C:\PS> IsolateRestore -sdkLocation .
#>
Function IsolateRestore {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$RootDirectory
    )
    $RootDirectory = Resolve-Path $RootDirectory
    Write-Host "The root directory is $RootDirectory"
    $gpf = Join-Path $RootDirectory -ChildPath "gpf"
    $httpCache = Join-Path $RootDirectory -ChildPath "httpCache"
    $pluginLogs = Join-Path $RootDirectory -ChildPath "plugin-logs"

    Write-Host "Setting the global packages folder to $gpf"
    $env:NUGET_PACKAGES=$gpf
    Write-Host "Setting the http cache to $httpCache"
    $env:NUGET_HTTP_CACHE_PATH=$httpCache
    Write-Host "Enabling the plugins loggings"
    $Env:NUGET_PLUGIN_ENABLE_LOG='true'
    Write-Host "Setting the plugin log directory to $pluginLogs"
    $Env:NUGET_PLUGIN_LOG_DIRECTORY_PATH=$pluginLogs
    Write-Host "Creating the plugin logs path."
    New-Item -ItemType directory -Path $pluginLogs
    # https://docs.microsoft.com/en-us/nuget/consume-packages/managing-the-global-packages-and-cache-folders
    # https://github.com/NuGet/Home/wiki/Plugin-Diagnostic-Logging
}
