<#
.SYNOPSIS
Script to tag NuGet.Client and NuGet.Build.Localization every time an insertion is made into VS.

.DESCRIPTION
Uses the Personal Access Token of NuGetLurker to automate the tagging process.

.PARAMETER PersonalAccessToken
PersonalAccessToken of the NuGetLurker account

.PARAMETER VsTargetBranch
The VS Branch that the NuGet build is being inserted into.

.PARAMETER BuildOutputPath
The path to root artifacts.
#>

[CmdletBinding()]
param
(
    [Parameter(Mandatory=$True)]
    [string]$PersonalAccessToken,
    [Parameter(Mandatory=$True)]
    [string]$VsTargetBranch,
    [Parameter(Mandatory=$True)]
    [string]$ArtifactsDirectory
)

# Set security protocol to tls1.2 for Invoke-RestMethod powershell cmdlet
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# These environment variables are set on the VSTS Release Definition agents.
$Commit = ${env:BUILD_SOURCEVERSION}

$BuildInfoJsonFile = [System.IO.Path]::Combine($ArtifactsDirectory, 'BuildInfo','buildinfo.json')
$NuGetExePath = [System.IO.Path]::Combine($ArtifactsDirectory, 'VS15', "NuGet.exe")
Write-Host $NuGetExePath

$AttemptNum = ${env:RELEASE_ATTEMPTNUMBER}
$ProductVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($NuGetExePath).ProductVersion
$index = $ProductVersion.LastIndexOf('+')
if($index -ne '-1')
{
    $ProductVersion = $ProductVersion.Substring(0,$index).Trim()
}

$Date = Get-Date
$Message = "Insert $ProductVersion into $VsTargetBranch on $Date"
$buildInfoJson = (Get-Content $BuildInfoJsonFile -Raw) | ConvertFrom-Json
$LocRepoCommitHash = $buildInfoJson.LocalizationRepositoryCommitHash
$TagName = $buildInfoJson.BuildNumber

Function Tag-GitCommit {
    param(
        [string]$NuGetRepository,
        [string]$PersonalAccessToken,
        [string]$CommitHash,
        [string]$TagName,
        [string]$TagMessage
    )

$Token = $PersonalAccessToken
$Base64Token = [System.Convert]::ToBase64String([char[]]$Token)

$Headers= @{
    Authorization='Basic {0}' -f $Base64Token;
}

try {
    $Body = @{
        tag = $TagName;
        object = $CommitHash;
        type = 'commit';
        message= $TagMessage;
        } | ConvertTo-Json;

        Write-Host $Body

    $tagObject = "refs/tags/$TagName"
    $r1 = Invoke-RestMethod -Headers $Headers -Method Post -Uri "https://api.github.com/repos/NuGet/$NuGetRepository/git/tags" -Body $Body
    Write-Host $r1
}
catch {
    # The above would fail if the tag already existed, in which case we would append the attempt number to the tag name to make it unique
    Write-Host "Tagging failed, appending attempt number...."
    $TagName = "$TagName-$AttemptNum"
    $Body = @{
        tag = $TagName;
        object = $CommitHash;
        type = 'commit';
        message= $TagMessage;
        } | ConvertTo-Json;

        Write-Host $Body

    $tagObject = "refs/tags/$TagName"
    $r1 = Invoke-RestMethod -Headers $Headers -Method Post -Uri "https://api.github.com/repos/NuGet/$NuGetRepository/git/tags" -Body $Body
    Write-Host $r1
}


$Body2 = @{
ref = $tagObject;
sha = $r1.sha;
} | ConvertTo-Json;

Write-Host $Body2

$r2 = Invoke-RestMethod -Headers $Headers -Method Post -Uri "https://api.github.com/repos/NuGet/$NuGetRepository/git/refs" -Body $Body2

Write-Host $r2

}

Tag-GitCommit -NuGetRepository 'NuGet.Client' -PersonalAccessToken $PersonalAccessToken -CommitHash $Commit -TagName $TagName -TagMessage $Message

Tag-GitCommit -NuGetRepository 'NuGet.Build.Localization' -PersonalAccessToken $PersonalAccessToken -CommitHash $LocRepoCommitHash -TagName $TagName -TagMessage $Message
