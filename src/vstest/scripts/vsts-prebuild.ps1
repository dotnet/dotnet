# Sets variables which are used across the build tasks.

param (
  [Parameter(Mandatory)]
  [string] $BuildSuffix,
  [Parameter(Mandatory)]
  [string] $IsRtmBuild,
  [Parameter(Mandatory)]
  $Branch,
  [Parameter(Mandatory)]
  [string] $ForcePreviewOnRelBranch
)

$TP_ROOT_DIR = (Get-Item (Split-Path $MyInvocation.MyCommand.Path)).Parent.FullName

# Set Version from scripts/build/TestPlatform.Settings.targets
$TpVersion = [string](([xml](Get-Content $TP_ROOT_DIR\scripts\build\TestPlatform.Settings.targets)).Project.PropertyGroup.TPVersionPrefix)
$buildPrefix = $TpVersion.Trim()

if ($IsRtmBuild.ToLower() -eq "false")
{
  if ($null -ne $Branch -and $Branch -like "refs/heads/rel/*" -and "false" -eq $ForcePreviewOnRelBranch)
  {
    $BuildSuffix = $BuildSuffix -replace "preview", "release"
  }

  $packageVersion = $buildPrefix+"-"+$buildSuffix
}
else
{
  $packageVersion = $buildPrefix
  $buildSuffix = [string]::Empty
}

Write-Host "##vso[task.setvariable variable=BuildVersionPrefix;]$buildPrefix"
Write-Host "##vso[task.setvariable variable=BuildVersionSuffix;]$buildSuffix"
Write-Host "##vso[task.setvariable variable=PackageVersion;]$packageVersion"

# Set Newtonsoft.Json version to consume in  CI build "Package: TestPlatform SDK" task.
# "Nuget.exe pack" required JsonNetVersion property for creating nuget package.

$JsonNetVersion = ([xml](Get-Content $TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.JsonNetVersion
Write-Host "##vso[task.setvariable variable=JsonNetVersion;]$JsonNetVersion"

$microsoftFakesVersion = ([xml](Get-Content $TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.MicrosoftFakesVersion
$FakesPackageDir = Join-Path $TP_ROOT_DIR "packages\Microsoft.QualityTools.Testing.Fakes.TestRunnerHarness\$microsoftFakesVersion\contentFiles"
Write-Host "##vso[task.setvariable variable=FakesPackageDir;]$FakesPackageDir"
