# Purpose: Create placeholder (non-valid) MSI files so the bundle build exercises gating logic.
param(
    [Parameter(Mandatory=$true)][string]$Architecture,
    [string]$Destination = (Join-Path (Get-Location) "artifacts/prereqs/$Architecture"),
    [string]$Version = "0.0.0-dummy"
)

Write-Host "[make-dummy-runtime-msis] Architecture=$Architecture Version=$Version"
New-Item -Force -ItemType Directory -Path $Destination | Out-Null

$files = @(
    "dotnet-host-win-$Architecture.msi",
    "dotnet-hostfxr-win-$Architecture.msi",
    "dotnet-runtime-win-$Architecture.msi"
)

foreach ($f in $files) {
    $path = Join-Path $Destination $f
    "Dummy MSI placeholder for $f version $Version" | Out-File $path -Encoding ASCII
}

"RuntimePrereqVersion=$Version" | Out-File (Join-Path $Destination "resolved-runtime-version.txt") -Encoding UTF8

Write-Host "[make-dummy-runtime-msis] Created placeholder files:"; Get-ChildItem $Destination | Select-Object Name, Length
