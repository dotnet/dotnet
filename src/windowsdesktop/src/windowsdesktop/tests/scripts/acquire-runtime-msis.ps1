# Purpose: Acquire real runtime MSIs (host, hostfxr, runtime) and normalize names for bundle build.
param(
    [Parameter(Mandatory=$true)][string]$Architecture,
    [string]$Destination = (Join-Path (Get-Location) "artifacts/prereqs/$Architecture"),
    [string]$SourceDrop,
    [string]$RuntimeBuildZip
)

Write-Host "[acquire-runtime-msis] Architecture=$Architecture"
New-Item -Force -ItemType Directory -Path $Destination | Out-Null

if (-not $SourceDrop -and -not $RuntimeBuildZip) {
    Write-Host "Provide -SourceDrop or -RuntimeBuildZip"; exit 1
}

if ($SourceDrop) {
    if (-not (Test-Path $SourceDrop)) { Write-Host "SourceDrop not found: $SourceDrop"; exit 2 }
    Write-Host "Copying MSIs from $SourceDrop -> $Destination"
    Get-ChildItem -Path $SourceDrop -Filter "dotnet-*-win-$Architecture*.msi" | Copy-Item -Destination $Destination -Force
}
elseif ($RuntimeBuildZip) {
    if (-not (Test-Path $RuntimeBuildZip)) { Write-Host "RuntimeBuildZip not found: $RuntimeBuildZip"; exit 3 }
    Write-Host "Expanding $RuntimeBuildZip -> $Destination"
    Expand-Archive -Path $RuntimeBuildZip -DestinationPath $Destination -Force
}

function PickLatest([string]$pattern) {
    $candidates = Get-ChildItem -Path $Destination -Filter $pattern | Sort-Object Name -Descending
    if ($candidates.Count -gt 0) { return $candidates[0].FullName }
    return $null
}

$_host    = PickLatest "dotnet-host-*-win-$Architecture.msi"
$_hostfxr = PickLatest "dotnet-hostfxr-*-win-$Architecture.msi"
$_runtime = PickLatest "dotnet-runtime-*-win-$Architecture.msi"

if (-not ($_host -and $_hostfxr -and $_runtime)) {
    Write-Host "Missing required MSIs after acquisition. Present:"; Get-ChildItem $Destination -Filter "dotnet-*-win-$Architecture*.msi" | Select-Object Name
    exit 4
}

Copy-Item $_host    (Join-Path $Destination "dotnet-host-win-$Architecture.msi") -Force
Copy-Item $_hostfxr (Join-Path $Destination "dotnet-hostfxr-win-$Architecture.msi") -Force
Copy-Item $_runtime (Join-Path $Destination "dotnet-runtime-win-$Architecture.msi") -Force

"RuntimePrereqVersion=" + ([IO.Path]::GetFileNameWithoutExtension($_runtime)) | Out-File (Join-Path $Destination "resolved-runtime-version.txt") -Encoding UTF8

Write-Host "[acquire-runtime-msis] Final staged files:"; Get-ChildItem $Destination -Filter "dotnet-*-win-$Architecture.msi" | Select-Object Name
