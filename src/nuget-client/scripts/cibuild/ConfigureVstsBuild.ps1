<#
.SYNOPSIS
Sets build variables during a CI build dynamically.

.DESCRIPTION
This script is used to dynamically set some build variables during CI build.
Specifically, this script determines the build number of the artifacts,
also it sets the $(NupkgOutputDir) based on whether $(BuildRTM) is true or false.

.PARAMETER BuildRTM
True/false depending on whether nupkgs are being with or without the release labels.

.PARAMETER RepositoryPath
The path to the root of the NuGet.Client repo

.PARAMETER BranchName
The name of the branch being built

.PARAMETER CommitHash
The commit hash being built

.PARAMETER BuildNumber
The build number of the current build

.PARAMETER BuildInfoDirectory
Optional directory path to write buildinfo.json to. When not provided, defaults to the repository's artifacts directory.
#>

param
(
    [Parameter(Mandatory=$True)]
    [string]$BuildRTM,
    [Parameter(Mandatory=$true)]
    [string]$RepositoryPath,
    [Parameter(Mandatory=$true)]
    [string]$BranchName,
    [Parameter(Mandatory=$true)]
    [string]$CommitHash,
    [Parameter(Mandatory=$true)]
    [string]$BuildNumber,
    [Parameter(Mandatory=$false)]
    [string]$BuildInfoDirectory
)

Function Set-RtmLabel {
    param(
        [Parameter(Mandatory = $true)]
        [boolean]$isRTMBuild
    )

    if ($isRTMBuild -eq $true) {
        $label = "RTM"
    } else {
        $label = "NonRTM"
    }

    Write-Host "RTM Label: $label"
    Write-Host "##vso[task.setvariable variable=RtmLabel;]$label"
}

$isRTMBuild = [boolean]::Parse($BuildRTM)

Set-RtmLabel -isRTMBuild $isRTMBuild

# Disable strong name verification of common public keys so that scenarios like building the VSIX or running unit tests
# will not fail because of strong name verification errors.
. "$PSScriptRoot\..\utils\DisableStrongNameVerification.ps1"

$regKeyFileSystem = "HKLM:SYSTEM\CurrentControlSet\Control\FileSystem"
$enableLongPathSupport = "LongPathsEnabled"

if (-not (Test-Path $regKeyFileSystem))
{
    Write-Host "Enabling long path support on the build machine"
    Set-ItemProperty -Path $regKeyFileSystem -Name $enableLongPathSupport -Value 1
}

if ($BuildRTM -eq $true)
{
    Write-Host "##vso[task.setvariable variable=VsixPublishDir;]VS15-RTM"
}
else
{
    Write-Host "##vso[task.setvariable variable=VsixPublishDir;]VS15"
    $newBuildCounter = $BuildNumber
    $VsTargetBranch = ((& dotnet msbuild $RepositoryPath\build\config.props /restore:false "/ConsoleLoggerParameters:Verbosity=Minimal;NoSummary;ForceNoAlign" /nologo /target:GetVsTargetBranch) | Out-String).Trim()
    $NuGetSdkVsVersion = ((& dotnet msbuild $RepositoryPath\build\config.props /restore:false "/ConsoleLoggerParameters:Verbosity=Minimal;NoSummary;ForceNoAlign" /nologo /target:GetNuGetSdkVsSemanticVersion) | Out-String).Trim()
    $VsTargetChannel = ((& dotnet msbuild $RepositoryPath\build\config.props /restore:false "/ConsoleLoggerParameters:Verbosity=Minimal;NoSummary;ForceNoAlign" /nologo /target:GetVsTargetChannel) | Out-String).Trim()
    $VsTargetMajorVersion = ((& dotnet msbuild $RepositoryPath\build\config.props /restore:false "/ConsoleLoggerParameters:Verbosity=Minimal;NoSummary;ForceNoAlign" /nologo /target:GetVsTargetMajorVersion) | Out-String).Trim()
    $GetNuGetVsVersion = ((& dotnet msbuild $RepositoryPath\build\config.props /restore:false "/ConsoleLoggerParameters:Verbosity=Minimal;NoSummary;ForceNoAlign" /nologo /target:GetNuGetVsVersion) | Out-String).Trim()

    Write-Host "VS target branch: $VsTargetBranch"
    $jsonRepresentation = @{
        BuildNumber = $newBuildCounter
        CommitHash = $CommitHash
        BuildBranch = $BranchName
        VsTargetBranch = $VsTargetBranch
        VsTargetChannel = $VstargetChannel
        VsTargetMajorVersion = $VsTargetMajorVersion
        NuGetSdkVsVersion = $NuGetSdkVsVersion
        NuGetVsVersion = $GetNuGetVsVersion
    }

    if (-not [string]::IsNullOrWhiteSpace($BuildInfoDirectory))
    {
        $buildInfoDirectoryPath = $BuildInfoDirectory
    }
    else
    {
        $buildInfoDirectoryPath = Join-Path $RepositoryPath 'artifacts'
    }

    New-Item -Path $buildInfoDirectoryPath -ItemType Directory -Force | Out-Null
    $localBuildInfoJsonFilePath = Join-Path $buildInfoDirectoryPath 'buildinfo.json'

    New-Item $localBuildInfoJsonFilePath -Force | Out-Null
    $jsonRepresentation | ConvertTo-Json | Set-Content $localBuildInfoJsonFilePath
    Write-Host "Created $localBuildInfoJsonFilePath"
}
