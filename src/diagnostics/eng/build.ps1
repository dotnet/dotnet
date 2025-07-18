[CmdletBinding(PositionalBinding=$false)]
Param(
    [ValidateSet("x86","x64","arm","arm64")][string][Alias('a', "platform")]$architecture = [System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture.ToString().ToLowerInvariant(),
    [ValidateSet("Debug","Release")][string][Alias('c')] $configuration = "Debug",
    [string][Alias('v')] $verbosity = "minimal",
    [switch][Alias('t')] $test,
    [switch] $installruntimes,
    [switch] $privatebuild,
    [switch] $ci,
    [switch][Alias('bl')]$binaryLog,
    [switch] $skipmanaged,
    [switch] $skipnative,
    [switch] $bundletools,
    [switch] $useCdac,
    [ValidatePattern("(default|\d+\.\d+.\d+(-[a-z0-9\.]+)?)")][string] $dotnetruntimeversion = 'default',
    [ValidatePattern("(default|\d+\.\d+.\d+(-[a-z0-9\.]+)?)")][string] $dotnetruntimedownloadversion= 'default',
    [string] $runtimesourcefeed = '',
    [string] $runtimesourcefeedkey = '',
    [string] $liveRuntimeDir = '',
    [Parameter(ValueFromRemainingArguments=$true)][String[]] $remainingargs
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$crossbuild = $false
if (($architecture -eq "arm") -or ($architecture -eq "arm64")) {
    $processor = @([System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture.ToString().ToLowerInvariant())
    if ($architecture -ne $processor) {
        $crossbuild = $true
    }
}

switch ($configuration.ToLower()) {
    { $_ -eq "debug" } { $configuration = "Debug" }
    { $_ -eq "release" } { $configuration = "Release" }
}

$reporoot = Join-Path $PSScriptRoot ".."
$engroot = Join-Path $reporoot "eng"
$artifactsdir = Join-Path $reporoot "artifacts"
$os = "Windows_NT"
$logdir = Join-Path $artifactsdir "log"
$logdir = Join-Path $logdir Windows_NT.$architecture.$configuration

$bl = if ($binaryLog) { '-binaryLog' } else { '' }

if ($ci) {
    $remainingargs = "-ci " + $remainingargs
}

if ($bundletools) {
    $remainingargs = "/p:BundleTools=true " + $remainingargs
    $remainingargs = '/bl:"$logdir\BundleTools.binlog" ' + $remainingargs
    $remainingargs = '-noBl ' + $remainingargs
    $skipnative = $True
    $test = $False
}

# Build native components
if (-not $skipnative) {
    Invoke-Expression "& `"$engroot\Build-Native.cmd`" -architecture $architecture -configuration $configuration -verbosity $verbosity $remainingargs"
    if ($lastExitCode -ne 0) {
        exit $lastExitCode
    }
}

# Install sdk for building, restore and build managed components.
if (-not $skipmanaged) {
    Invoke-Expression "& `"$engroot\common\build.ps1`" -configuration $configuration -verbosity $verbosity $bl /p:TargetOS=$os /p:TargetArch=$architecture /p:TestArchitectures=$architecture $remainingargs"

    if ($lastExitCode -ne 0) {
        exit $lastExitCode
    }
}

if ($installruntimes -or $privatebuild) {
    $privatebuildtesting = "false"
    if ($privatebuild) {
        $privatebuildtesting = "true"
    }
    Remove-Item -Force -Recurse -ErrorAction SilentlyContinue "$reporoot\.dotnet-test"
    & "$engroot\common\msbuild.ps1" `
      $engroot\InstallRuntimes.proj `
      -verbosity $verbosity `
      /t:InstallTestRuntimes `
      /bl:$logdir\InstallRuntimes.binlog `
      /p:PrivateBuildTesting=$privatebuildtesting `
      /p:TargetOS=$os `
      /p:TargetArch=$architecture `
      /p:TestArchitectures=$architecture `
      /p:LiveRuntimeDir="$liveRuntimeDir"
}

# Run the xunit tests
if ($test) {
    if (-not $crossbuild) {
        if ($useCdac) {
            $env:SOS_TEST_CDAC="true"
        }
        & "$engroot\common\build.ps1" `
          -test `
          -configuration $configuration `
          -verbosity $verbosity `
          -ci:$ci `
          /bl:$logdir\Test.binlog `
          /p:TargetOS=$os `
          /p:TargetArch=$architecture `
          /p:TestArchitectures=$architecture `
          /p:DotnetRuntimeVersion="$dotnetruntimeversion" `
          /p:DotnetRuntimeDownloadVersion="$dotnetruntimedownloadversion" `
          /p:RuntimeSourceFeed="$runtimesourcefeed" `
          /p:RuntimeSourceFeedKey="$runtimesourcefeedkey" `
          /p:LiveRuntimeDir="$liveRuntimeDir" 

        if ($lastExitCode -ne 0) {
            exit $lastExitCode
        }
    }
}
