<#
.SYNOPSIS
Validates the result of the localization process

.DESCRIPTION
Runs NuGetValidator.exe over localized artifact binaries to count validation mismatchs between binaries and localization inputs

.PARAMETER RepoRoot
Path to NuGet.Client repo root folder

.PARAMETER OutputLogsBasePath
Path to logs output folder

.PARAMETER BuildRTM
true/false depending on whether nupkgs are being with or without the release labels.

.PARAMETER ValidateVsix
Flag to verify VSIX artifact. Otherwise, verifies binaries under $RepoRoot\artifacts folder (default)

.PARAMETER TmpPath
Path to a temporary folder to extract the VSIX artifact.
#>
param
(
    [Parameter(Mandatory=$True)]
    [string]$RepoRoot,

    [Parameter(Mandatory=$True)]
    [string]$OutputLogsBasePath,

    [Parameter(Mandatory=$True)]
    [string]$BuildRTM,

    [switch]$ValidateVsix,

    [string]$TmpPath = $Env:TEMP
)

if ($BuildRTM -eq 'false')
{
    $NuGetValidator = [System.IO.Path]::Combine($RepoRoot, 'packages', 'nugetvalidator', '2.0.5', 'tools', 'NuGetValidator.exe')
    $LocalizationRepository = [System.IO.Path]::Combine($RepoRoot, 'submodules', 'NuGet.Build.Localization', 'localize', 'comments', '15')

    if ($ValidateVsix)
    {
        $VsixLocation = [System.IO.Path]::Combine($RepoRoot, 'artifacts', 'VS15', 'NuGet.Tools.vsix')
        $VsixExtractLocation = [System.IO.Path]::Combine($TmpPath, 'extractedVsix')
        $VsixLogOutputDir = [System.IO.Path]::Combine($OutputLogsBasePath, 'LocalizationValidation', 'vsix')

        Write-Host "Validating NuGet.Tools.Vsix localization..."
        Write-Host "Running: $NuGetValidator localization --vsix --vsix-path $VsixLocation --vsix-extract-path $VsixExtractLocation --output-path $VsixLogOutputDir --comments-path $LocalizationRepository"
        & $NuGetValidator localization --vsix --vsix-path $VsixLocation --vsix-extract-path $VsixExtractLocation --output-path $VsixLogOutputDir --comments-path $LocalizationRepository
    }
    else
    {
        $ArtifactsLocation = [System.IO.Path]::Combine($RepoRoot, 'artifacts')
        $ArtifactsLogOutputDir = [System.IO.Path]::Combine($OutputLogsBasePath, 'LocalizationValidation', 'artifacts')

        Write-Host "Validating NuGet.Client repository localization..."
        Write-Host "Running: $NuGetValidator localization --artifacts-path $ArtifactsLocation --output-path $ArtifactsLogOutputDir --comments-path $LocalizationRepository --filter-paths-containing net45"
        & $NuGetValidator localization --artifacts-path $ArtifactsLocation --output-path $ArtifactsLogOutputDir --comments-path $LocalizationRepository  --filter-paths-containing net45
    }

    # return the exit code from the validator
    exit $LASTEXITCODE
}
