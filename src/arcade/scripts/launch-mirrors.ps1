<#
.SYNOPSIS
Launches the merge and/or code mirror given a list of repos and branches

.PARAMETER GitHubRepoName
Repository to mirror

.PARAMETER GitHubBranch
Branch to mirror

.PARAMETER BranchAndRepoFile
Json file describing repos and branches to mirror. File should be a json
list like:

[
  {
    "repo": "dotnet/aspnetcore",
    "branch": "release/3.1"
  },
  {
    "repo": "dotnet/aspnetcore-tooling",
    "branch": "release/3.1"
  }
]

.PARAMETER NoInternalMergeMirror
Do not launch the internal branch mirror (foobranch -> internal/foobranch)

.PARAMETER NoPublicMirror
Do not launch the public mirror (foobranch -> foobranch)

.PARAMETER AzDOPat
PAT used to make AzDO modifications.  If UseKeyVault is passed, may be omitted

.PARAMETER UseKeyVault
If passed, looks up secrets in keyvault

#>

param (
    [string]$GitHubRepoName,
    [string]$GitHubBranch,
    [string]$BranchAndRepoFile,
    [switch]$NoInternalMergeMirror,
    [switch]$NoPublicMirror,
    [string]$AzDOPat,
    [switch]$UseKeyVault
)

function LaunchMirrorBuild {
    param ($repo, $branch, $AzDOInstance, $definitionId, $mirrorType)
    try {
        $body = @{
            definition = @{
                id = $definitionId
            }
            parameters = $(ConvertTo-Json @{BranchToMirror=$branch; GithubRepo=$repo})
        }
        $bodyStr = ConvertTo-Json $body
        $uri = "${AzDOInstance}/_apis/build/builds?api-version=5.1"
        Write-Host "Launching $mirrorType build for $repo @ $branch"
        $queueResponse = Invoke-WebRequest -Method Post -ContentType "application/json" -Headers $AzDOAuthHeader -Uri "${AzDOInstance}/_apis/build/builds?api-version=5.1" -Body $bodyStr | ConvertFrom-Json
        $buildId = $queueResponse.id
        Write-Host "Launched $AzDOInstance/_build/results?buildId=$buildId"
    }
    catch {
        Write-Error "Failed to launch build"
        Write-Error "$_.Exception"
        exit
    }
}

$AzDOInternalInstance = "https://dev.azure.com/dnceng/internal"  

# If UseKeyVault is set, grab keys from keyvault
if ($UseKeyVault) {
    try {
        Write-Output "Obtaining required secrets from keyvault"
        $AzDOPat = $(Get-AzKeyVaultSecret -VaultName 'EngKeyVault' -Name 'dn-bot-all-build-queue' -ErrorAction Stop).SecretValueText
    }
    catch {
        Write-Error $_.Exception.Message
        Write-Error "Failed to gather required credentials from EngKeyVault.  Consider passing them in directly."
        exit
    }
} else {
    if (!$AzDOPat) {
        Write-Error "If not using key vault to find secrets, please provide AzDOPat"
        exit
    }
}

# Set powershell to use TLS12 so that we don't error when talking to GitHub.
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

$base64authinfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(":$AzDOPat"))
$AzDOAuthHeader = @{"Authorization"="Basic $base64authinfo"}

$repoList = $null

if ($GitHubRepoName -and $GitHubBranch) {
    $repoList = @(@{repo=$GitHubRepoName;branch=$GitHubBranch})
}

# Read the json file
if ($BranchAndRepoFile) {
    $repoList = Get-Content $BranchAndRepoFile | ConvertFrom-Json 
}

foreach ($repoAndBranch in $repoList) {
    $repo = $repoAndBranch.repo
    $branch = $repoAndBranch.branch
    
    

    if (!$NoInternalMergeMirror) {
        LaunchMirrorBuild $repo $branch $AzDOInternalInstance 240 "internal branch merge"
    }
    
    if (!$NoPublicMirror) {
        LaunchMirrorBuild $repo $branch $AzDOInternalInstance 16 "public branch mirror"
    }
}