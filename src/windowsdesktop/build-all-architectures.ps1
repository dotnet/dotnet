#!/usr/bin/env pwsh

Write-Host "Building Windows Desktop Runtime for all architectures..."

$architectures = @("x86", "x64", "arm64")
$failed = @()

foreach ($arch in $architectures) {
    Write-Host "Building for architecture: $arch" -ForegroundColor Green
    
    # Build the main projects with specific architecture
    $result = & dotnet build Build.proj -c Release -p:Platform=$arch -p:TargetArchitecture=$arch
    
    if ($LASTEXITCODE -ne 0) {
        $failed += $arch
        Write-Host "Failed to build $arch architecture" -ForegroundColor Red
    } else {
        Write-Host "Successfully built $arch architecture" -ForegroundColor Green
    }
}

if ($failed.Count -gt 0) {
    Write-Host "Failed architectures: $($failed -join ', ')" -ForegroundColor Red
    exit 1
} else {
    Write-Host "All architectures built successfully!" -ForegroundColor Green
    
    # Show the results
    Write-Host "`nGenerated installers:" -ForegroundColor Yellow
    Get-ChildItem -Path artifacts -Recurse -Filter "windowsdesktop-runtime-*-win-*.exe" -ErrorAction SilentlyContinue | Select-Object Name, FullName
}