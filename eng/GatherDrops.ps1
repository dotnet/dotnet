[CmdletBinding()]
param(
  [Parameter(Mandatory=$true)]
  [String]$filePath,
  [Parameter(Mandatory=$true)]
  [String]$outputPath,
  [Parameter(Mandatory=$true)]
  [String]$darcPath,
  [Parameter(Mandatory=$true)]
  [String]$githubPat,
  [Parameter(Mandatory=$true)]
  [String]$azdevPat,
  [Parameter(Mandatory=$true)]
  [String]$includedRepositories,
  [Parameter(Mandatory=$false)]
  [String]$assetFilter = ".*",
  [Switch]$nonShipping = $false
)

$jsonContent = Get-Content -Path $filePath -Raw | ConvertFrom-Json
$includedReposList = $includedRepositories -split ',' | ForEach-Object { $_.Trim() }

foreach ($includedRepo in $includedReposList) {
  $repo = $jsonContent.repositories | Where-Object { $_.path -eq $includedRepo } | Select-Object -First 1
  if (-not $repo) {
    Write-Warning "Repository '$includedRepo' not found in JSON. Skipping."
    continue
  }

  $barId = $repo.barId
  $repoName = $repo.path
  $path = "$outputPath$($repoName)"
  $darcCommand = "$darcPath gather-drop --id $barId --skip-existing --continue-on-error --use-azure-credential-for-blobs --output-dir $path --github-pat $githubPat --azdev-pat $azdevPat --asset-filter $assetFilter --verbose --ci --include-released $(if ($nonShipping) { '--non-shipping' })"
  Write-Output "Gathering drop for $repoName"
  Invoke-Expression $darcCommand
}
exit 0