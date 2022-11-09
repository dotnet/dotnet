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
    [string]$BuildNumber
)

Function Get-Version {
    param(
        [string]$buildNumber
    )
        Write-Host "Evaluating the new VSIX Version : $buildNumber"
        # The major version is NuGetMajorVersion + 11, to match VS's number.
        # The new minor version is: 4.0.0 => 40000, 4.11.5 => 41105.
        # This assumes we only get to NuGet major/minor/patch 99 at worst, otherwise the logic breaks.
        # The final version for NuGet 4.0.0, build number 3128 would be 15.0.40000.3128
        $parsedVersion = [System.Version]::Parse($buildNumber)
        $major = $parsedVersion.Major + 11
        $patchVersion = $parsedVersion.Major * 10000 + $parsedVersion.Minor * 100 + $parsedVersion.Build
        $finalVersion = "$major.0.$patchVersion.$($parsedVersion.Revision)"

        Write-Host "The new VSIX Version is: $finalVersion"
        return $finalVersion
}

Function Update-VsixVersion {
    param(
        [string]$buildNumber,
        [string]$manifestName,
        [string]$repositoryPath
    )
    $vsixManifest = Join-Path "$repositoryPath\src\NuGet.Clients\NuGet.VisualStudio.Client" $manifestName

    Write-Host "Updating the VSIX version in manifest $vsixManifest"

    [xml]$xml = get-content $vsixManifest
    $root = $xml.PackageManifest

    # Reading the current version from the manifest
    $oldVersion = $root.Metadata.Identity.Version
    # Evaluate the new version
    $newVersion = Get-Version $buildNumber
    Write-Host "Updating the VSIX version [$oldVersion] => [$newVersion]"
    Write-Host "##vso[task.setvariable variable=VsixBuildNumber;]$newVersion"
    # setting the revision to the new version
    $root.Metadata.Identity.Version = "$newVersion"

    $xml.Save($vsixManifest)

    Write-Host "Updated the VSIX version [$oldVersion] => [$($root.Metadata.Identity.Version)]"
}

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

Function Get-LocBranchExists {
    param(
        [Parameter(Mandatory = $true)]
        [string]$branchName
    )

    Write-Host "Looking for branch '$branchName' in NuGet.Build.Localization"
    $lsRemoteOpts = 'ls-remote', '--exit-code', 'origin', "refs/heads/$branchName"
    $branchExists = & git -C $NuGetLocalization $lsRemoteOpts
    return $LASTEXITCODE -eq 0
}

$isRTMBuild = [boolean]::Parse($BuildRTM)

Set-RtmLabel -isRTMBuild $isRTMBuild

# Disable strong name verification of common public keys so that scenarios like building the VSIX or running unit tests
# will not fail because of strong name verification errors.
. "$PSScriptRoot\..\utils\DisableStrongNameVerification.ps1"

$regKeyFileSystem = "HKLM:SYSTEM\CurrentControlSet\Control\FileSystem"
$enableLongPathSupport = "LongPathsEnabled"

$Submodules = Join-Path $RepositoryPath submodules -Resolve

# NuGet.Build.Localization repository set-up
$NuGetLocalization = Join-Path $Submodules NuGet.Build.Localization -Resolve

# Check if there is a localization branch associated with this branch repo
if (Get-LocBranchExists $BranchName)
{
    $NuGetLocalizationRepoBranch = $BranchName
}
else
{
    if ($BranchName -like "*-MSRC") {
        $currentNuGetBranch = $BranchName -replace "-MSRC$", ""
        if (Get-LocBranchExists $currentNuGetBranch) {
            $NuGetLocalizationRepoBranch = $currentNuGetBranch
        }
        else
        {
            $NuGetLocalizationRepoBranch = "dev"
        }
    }
    else {
        $NuGetLocalizationRepoBranch = 'dev'
    }
}
Write-Host "NuGet.Build.Localization Branch: $NuGetLocalizationRepoBranch"

# update submodule NuGet.Build.Localization
$updateOpts = 'switch', '-d', "origin/$NuGetLocalizationRepoBranch", "-q"
Write-Host "git update NuGet.Build.Localization at $NuGetLocalization"
& git -C $NuGetLocalization $updateOpts 2>&1 | Write-Host
# Get the commit of the localization repository that will be used for this build.
$LocalizationRepoCommitHash = & git -C $NuGetLocalization log --pretty=format:'%H' -n 1

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
    $VsTargetBranch = & dotnet msbuild $RepositoryPath\build\config.props /v:m /nologo /t:GetVsTargetBranch
    $NuGetSdkVsVersion = & dotnet msbuild $RepositoryPath\build\config.props /v:m /nologo /t:GetNuGetSdkVsSemanticVersion
    $VsTargetChannel = & dotnet msbuild $RepositoryPath\build\config.props /v:m /nologo /t:GetVsTargetChannel
    $VsTargetMajorVersion = & dotnet msbuild $RepositoryPath\build\config.props /v:m /nologo /t:GetVsTargetMajorVersion
    Write-Host "VS target branch: $VsTargetBranch"
    $jsonRepresentation = @{
        BuildNumber = $newBuildCounter
        CommitHash = $CommitHash
        BuildBranch = $BranchName
        LocalizationRepositoryBranch = $NuGetLocalizationRepoBranch
        LocalizationRepositoryCommitHash = $LocalizationRepoCommitHash
        VsTargetBranch = $VsTargetBranch.Trim()
        VsTargetChannel = $VstargetChannel.Trim()
        VsTargetMajorVersion = $VsTargetMajorVersion.Trim()
        NuGetSdkVsVersion = $NuGetSdkVsVersion.Trim()
    }

    # First create the file locally so that we can laster publish it as a build artifact from a local source file instead of a remote source file.
    $localBuildInfoJsonFilePath = [System.IO.Path]::Combine("$RepositoryPath\artifacts", 'buildinfo.json')

    New-Item $localBuildInfoJsonFilePath -Force | Out-Null
    $jsonRepresentation | ConvertTo-Json | Set-Content $localBuildInfoJsonFilePath

    Update-VsixVersion -manifestName source.extension.vsixmanifest -buildNumber $BuildNumber -RepositoryPath $RepositoryPath
}
