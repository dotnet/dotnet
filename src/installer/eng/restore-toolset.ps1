function InitializeCustomSDKToolset {    
  if (-not $restore) {
    return
  }

  # The following frameworks and tools are used only for testing.
  # Do not attempt to install them in source build.
  if ($env:DotNetBuildFromSource -eq "true") {
    return
  }

  # InstallDotNetSharedFramework "1.0.5"
  # InstallDotNetSharedFramework "1.1.2"
  # InstallDotNetSharedFramework "2.1.0"

  CreateBuildEnvScript
}

function CreateBuildEnvScript()
{
  InitializeBuildTool | Out-Null # Make sure DOTNET_INSTALL_DIR is set

  Create-Directory $ArtifactsDir
  $scriptPath = Join-Path $ArtifactsDir "core-sdk-build-env.bat"
  $scriptContents = @"
@echo off
title Core SDK Build ($RepoRoot)
set DOTNET_MULTILEVEL_LOOKUP=0

set PATH=$env:DOTNET_INSTALL_DIR;%PATH%
set NUGET_PACKAGES=$env:NUGET_PACKAGES

set DOTNET_ROOT=$env:DOTNET_INSTALL_DIR
"@

  Out-File -FilePath $scriptPath -InputObject $scriptContents -Encoding ASCII
}

function InstallDotNetSharedFramework([string]$version) {
  $dotnetRoot = $env:DOTNET_INSTALL_DIR
  $fxDir = Join-Path $dotnetRoot "shared\Microsoft.NETCore.App\$version"

  if (!(Test-Path $fxDir)) {
    $installScript = GetDotNetInstallScript $dotnetRoot
    & $installScript -Version $version -InstallDir $dotnetRoot -Runtime "dotnet"

    if($lastExitCode -ne 0) {
      throw "Failed to install shared Framework $version to '$dotnetRoot' (exit code '$lastExitCode')."
    }
  }
}

InitializeCustomSDKToolset