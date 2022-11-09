<#
.SYNOPSIS
Sets a secret in one vault to be the same as another. Temporary workaround for issues in secret
layout. Assumes that the Keyvaults are in the same subscription and that the current context
is set to that subscription.

.PARAMETER SourceVaultName
Vault where the source secret is located

.PARAMETER SourceSecretName
Name of source secret

.PARAMETER DestinationVaultName
Vault where the destination secret is located

.PARAMETER DestinationSecretName
Name of destination secret. If not specified, the source secret name is used.

#>

param (
    [Parameter(Mandatory=$true)]
    [string]$SourceVaultName,
    [Parameter(Mandatory=$true)]
    [string]$SourceSecretName,
    [Parameter(Mandatory=$true)]
    [string]$DestinationVaultName,
    [string]$DestinationSecretName
)

if (!$DestinationSecretName) {
    $DestinationSecretName = $SourceSecretName
}

$sourceSecretInfo = Get-AzKeyVaultSecret -VaultName $SourceVaultName -Name $SourceSecretName
$destinationSecretInfo = Get-AzKeyVaultSecret -VaultName $DestinationVaultName -Name $DestinationSecretName

if (!$sourceSecretInfo) {
    Write-Error "Could not find source secret '$SourceSecretName' in '$SourceVaultName'"
    return
}

if (!$destinationSecretInfo) {
    Write-Error "Could not find destination secret '$DestinationSecretName' in '$DestinationVaultName'"
    return
}

if ($($sourceSecretInfo.SecretValue | ConvertFrom-SecureString -AsPlainText) -eq $($destinationSecretInfo.SecretValue | ConvertFrom-SecureString -AsPlainText)) {
    Write-Host "Secrets are already the same, not updating."
    return
}

$newDestinationSecret = Set-AzKeyVaultSecret -VaultName $DestinationVaultName -Name $DestinationSecretName -SecretValue $sourceSecretInfo.SecretValue -Expires $sourceSecretInfo.Expires -Tag $sourceSecretInfo.Tags -ContentType $sourceSecretInfo.ContextType

if ($($sourceSecretInfo.SecretValue | ConvertFrom-SecureString -AsPlainText) -ne $($newDestinationSecret.SecretValue | ConvertFrom-SecureString -AsPlainText)) {
    Write-Error "Destination secret was not correctly updated"
    return
} else {
    Write-Host "Destination secret '$DestinationSecretName' in '$DestinationVaultName' is now the same as '$SourceSecretName' in '$SourceVaultName'"
}