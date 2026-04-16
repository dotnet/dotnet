<#
.SYNOPSIS
    Updates SourceRevisionId and FileVersionRevision in an external package .proj file.
.DESCRIPTION
    After adding or updating a git submodule for an external component, run this script
    to automatically set the correct SourceRevisionId (from the submodule commit) and
    FileVersionRevision (from the Microsoft-shipped NuGet package) in the component's
    .proj file.

    The script honors the repo's NuGet.config when downloading packages.
.PARAMETER ComponentName
    The name of the external component (matching the submodule directory name).
    Example: azure-activedirectory-identitymodel-extensions-for-dotnet
.EXAMPLE
    ./update-external-metadata.ps1 azure-activedirectory-identitymodel-extensions-for-dotnet
#>
param(
    [Parameter(Mandatory=$true, Position=0)]
    [string]$ComponentName
)

$ErrorActionPreference = "Stop"

$repoRoot = & git rev-parse --show-toplevel
$repoRoot = $repoRoot.Trim()

$dotnetTool = Join-Path $repoRoot ".dotnet/dotnet"
if (!(Test-Path $dotnetTool)) {
    $dotnetTool = "dotnet"
}

$projectPath = Join-Path $repoRoot "eng/tools/UpdateExternalMetadata/UpdateExternalMetadata.csproj"

& $dotnetTool run --project $projectPath -- $ComponentName
exit $LASTEXITCODE
