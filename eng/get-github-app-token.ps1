[CmdletBinding()]
param(
  [Parameter(Mandatory=$true)] [string] $KeyVaultName,
  [Parameter(Mandatory=$true)] [string] $AppSecretName,
  [Parameter(Mandatory=$true)] [string] $InstallationOwner,
  [Parameter(Mandatory=$true)] [string[]] $RepositoryNames,
  [Parameter(Mandatory=$true)] [string] $OutputVariableName
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $true

Import-Module "$PSScriptRoot/GitHubAppToken.psm1" -Force

function Get-KeyVaultSecretValue {
  param([Parameter(Mandatory=$true)] [string] $Name)

  $escapedSecretName = [Uri]::EscapeDataString($Name)
  $secretUri = "https://$KeyVaultName.vault.azure.net/secrets/$escapedSecretName`?api-version=7.4"
  try {
    $response = Invoke-RestMethod `
      -Uri $secretUri `
      -Headers @{ Authorization = "Bearer $keyVaultAccessToken" } `
      -Method Get
  }
  catch {
    throw "Failed to read secret '$Name' from Key Vault '$KeyVaultName': $_"
  }

  if ([string]::IsNullOrWhiteSpace($response.value)) {
    throw "Secret '$Name' in Key Vault '$KeyVaultName' is empty."
  }

  $response.value
}

$previousNativeCommandErrorPreference = $PSNativeCommandUseErrorActionPreference
try {
  $PSNativeCommandUseErrorActionPreference = $false
  $keyVaultAccessToken = az account get-access-token `
    --resource https://vault.azure.net `
    --query accessToken `
    --output tsv `
    --only-show-errors
  $tokenExitCode = $LASTEXITCODE
}
finally {
  $PSNativeCommandUseErrorActionPreference = $previousNativeCommandErrorPreference
}
if ($tokenExitCode -ne 0 -or [string]::IsNullOrWhiteSpace($keyVaultAccessToken)) {
  throw 'Failed to acquire an Azure Key Vault access token.'
}

$appId = Get-KeyVaultSecretValue "$AppSecretName-app-id"
$privateKey = Get-KeyVaultSecretValue "$AppSecretName-app-private-key"

try {
  $jwt = New-GitHubAppJwt -AppId $appId -PrivateKeyPem $privateKey
  $tokenResponse = Get-GitHubAppInstallationToken `
    -Jwt $jwt `
    -InstallationOwner $InstallationOwner `
    -RepositoryNames $RepositoryNames
}
finally {
  $privateKey = $null
}

if ([string]::IsNullOrWhiteSpace($tokenResponse.token)) {
  throw "GitHub did not return an installation token for '$InstallationOwner'."
}

Write-Host "Got an installation token for '$InstallationOwner' that expires at $($tokenResponse.expires_at)."
Write-Host "##vso[task.setvariable variable=$OutputVariableName;issecret=true]$($tokenResponse.token)"
