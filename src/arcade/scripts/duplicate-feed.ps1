<#
.SYNOPSIS
Copies all packages from one feed to another in azure devops

.PARAMETER SourceFeedUri
Source feed to copy from

.PARAMETER TargetFeedUri
Target feed to copy to

.PARAMETER NugetPath
Path to nuget.exe to use for pushes

.PARAMETER AzDOPat
PAT used to make VSTS modifications.  If UseKeyVault is passed, may be omitted

.PARAMETER UseKeyVault
If passed, looks up secrets in keyvault

#>

param (
    [Parameter(Mandatory=$true)]
    [string]$SourceFeedUri,
    [Parameter(Mandatory=$true)]
    [string]$TargetFeedUri,
    [Parameter(Mandatory=$true)]
    [string]$NugetPath,
    [string]$AzDOPat,
    [switch]$UseKeyVault
)

function Parse-Feed-Uri($inputFeedUri) {
    if (-not $($inputFeedUri -match "https://pkgs.dev.azure.com/(?<account>[a-zA-Z0-9]+)/(?<visibility>[a-zA-Z0-9-]+/)?_packaging/(?<feed>.+)/nuget/v3/index.json")) {
        Write-Error "$inputFeedUri is not in the expected format."
        exit
    }
    return @{
        account = $Matches["account"];
        visibility = $Matches["visibility"];
        feed = $Matches["feed"];
    }
}

function Get-Package-List($vstsAuthHeader, $account, $visibility, $feed) {
    # Get the list of packages from the source and target
    $listOfPackages = @()
    try {
        $packageListUri = "https://feeds.dev.azure.com/$account/${visibility}_apis/packaging/Feeds/$feed/packages?api-version=5.1-preview.1"
        Write-Host "Looking up packages on feed at: $packageListUri"
        $result = Invoke-WebRequest -Headers $vstsAuthHeader $packageListUri
        $resultJson = $result | ConvertFrom-Json
        Write-Host "Feed $SourceFeedUri has $($resultJson.count) packages"
        foreach ($package in $resultJson.value) {
            $versionsResult = Invoke-WebRequest -Headers $vstsAuthHeader $package._links.versions.href
            $versionsResultJson = $versionsResult | ConvertFrom-Json
            foreach ($version in $versionsResultJson.value) {
                if (-not $version.isDeleted) {
                    $listOfPackages += @{ name = $package.name; version = $version.version }
                }
            }
        }
    } catch {
        Write-Error $_
        exit
    }
    $listOfPackages
}

if ($UseKeyVault) {
    try {
        Write-Output "Obtaining required secrets from keyvault"
        $AzDOPat = $(Get-AzKeyVaultSecret -VaultName 'EngKeyVault' -Name 'dn-bot-dnceng-artifact-feeds-rw' -ErrorAction Stop).SecretValueText
    }
    catch {
        Write-Error $_.Exception.Message
        Write-Error "Failed to gather required credentials from EngKeyVault.  Consider passing them in directly."
        exit
    }
} else {
    if (!$AzDOPat) {
        Write-Error "If not using key vault to find secrets, please provide AzDOPat, GitHubPat and MaestroSecret"
        exit
    }
}

# Set powershell to use TLS12 so that we don't error when talking to GitHub.
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

$base64authinfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(":$AzDOPat"))
$vstsAuthHeader = @{"Authorization"="Basic $base64authinfo"}

$sourceFeedInfo = Parse-Feed-Uri $SourceFeedUri
$targetFeedInfo = Parse-Feed-Uri $TargetFeedUri

$listOfSourcePackages = Get-Package-List $vstsAuthHeader $sourceFeedInfo.account $sourceFeedInfo.visibility $sourceFeedInfo.feed
$listOfTargetPackages = Get-Package-List $vstsAuthHeader $targetFeedInfo.account $targetFeedInfo.visibility $targetFeedInfo.feed

$targetPackageSet = @{}
foreach ($package in $listOfTargetPackages) {
    $targetPackageSet.Add("$($package.name)@$($package.version)", $true)
}

# Download all packages
$downloadRootBase = $([System.IO.Path]::GetTempPath())
$downloadRoot = Join-Path -Path $downloadRootBase -ChildPath $([System.IO.Path]::GetRandomFileName())
New-Item -Path $downloadRoot -ItemType Directory | Out-Null

foreach ($packageToCopy in $listOfSourcePackages) {
    if ($targetPackageSet.Contains("$($packageToCopy.name)@$($packageToCopy.version)")) {
        Write-Host "Skipping (already exists): $($packageToCopy.name) @ $($packageToCopy.version)"
        continue
    } else {
        Write-Host "Copying: $($packageToCopy.name) @ $($packageToCopy.version)"
    }
    
    try {
        $packageContentUrl = "https://pkgs.dev.azure.com/$($sourceFeedInfo.account)/$($sourceFeedInfo.visibility)_apis/packaging/feeds/$($sourceFeedInfo.feed)/nuget/packages/$($packageToCopy.name)/versions/$($packageToCopy.version)/content";
        Write-Host "Downloading package $($packageToCopy.name) @ $($packageToCopy.version) from $packageContentUrl"
        $localPackagePath = Join-Path -Path $downloadRoot -ChildPath "$($packageToCopy.name).$($packageToCopy.version).nupkg"
        Invoke-WebRequest -Headers $vstsAuthHeader $packageContentUrl -OutFile $localPackagePath
        & $NugetPath push -Source $TargetFeedUri -ApiKey AzureDevOps $localPackagePath -SkipDuplicate
    } catch {
        Write-Error $_
        exit
    }
}
