[CmdletBinding()]
param(
  [Parameter(Mandatory=$true)]
  [String]$filePath,
  [Parameter(Mandatory=$true)]
  [String]$outputPath,
  [Parameter(Mandatory=$true)]
  [String]$darcPath,
  [Parameter(Mandatory=$true)]
  [String]$dotnetGitHubToken,
  [Parameter(Mandatory=$true)]
  [String]$nugetGitHubToken,
  [Parameter(Mandatory=$true)]
  [String]$microsoftGitHubToken,
  [Parameter(Mandatory=$true)]
  [String]$azdevPat,
  [Parameter(Mandatory=$true)]
  [String[]]$includedRepositories,
  [Parameter(Mandatory=$false)]
  [String]$assetFilter = ".*",
  [Switch]$nonShipping = $false
)

$ErrorActionPreference = 'Stop'

Import-Module "$PSScriptRoot/VmrGitHubAuthentication.psm1" -Force

$jsonContent = Get-Content -Path $filePath -Raw | ConvertFrom-Json
$includedReposList = $includedRepositories | ForEach-Object { $_.Trim() }
$tokensByOwner = @{
  dotnet = $dotnetGitHubToken
  nuget = $nugetGitHubToken
  microsoft = $microsoftGitHubToken
}

foreach ($includedRepo in $includedReposList) {
  $repo = $jsonContent.repositories | Where-Object { $_.path -eq $includedRepo } | Select-Object -First 1
  if (-not $repo) {
    throw "Repository '$includedRepo' was not found in the source manifest."
  }

  $barId = $repo.barId
  $repoName = $repo.path
  $path = "$outputPath$($repoName)"
  $githubToken = Get-VmrGitHubToken -RemoteUri $repo.remoteUri -TokensByOwner $tokensByOwner
  $darcArguments = @(
    'gather-drop'
    '--id', $barId
    '--skip-existing'
    '--use-azure-credential-for-blobs'
    '--output-dir', $path
    '--github-pat', $githubToken
    '--azdev-pat', $azdevPat
    '--asset-filter', $assetFilter
    '--verbose'
    '--ci'
    '--include-released'
  )
  if ($nonShipping) {
    $darcArguments += '--non-shipping'
  }

  Write-Output "Gathering drop for $repoName"
  & $darcPath @darcArguments
  if ($LASTEXITCODE -ne 0) {
    throw "Gathering the drop for '$repoName' failed with exit code $LASTEXITCODE."
  }
}