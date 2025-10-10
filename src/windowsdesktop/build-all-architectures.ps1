#!/usr/bin/env pwsh

param(
    [string]$target = "Build",
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$args
)

Write-Host "Building Windows Desktop Runtime for all architectures..."

$architectures = @("x86", "x64", "arm64")
$failed = @()

if ($target -eq "pack") {
    $target = "Build" # The 'Pack' target is part of the 'Build' target in Build.proj
}

$extraArgs = $args -join " "

foreach ($arch in $architectures) {
    Write-Host "Building for architecture: $arch" -ForegroundColor Green
    
    # Build the main projects with specific architecture
    $command = "dotnet build Build.proj -t:$target -c Release -p:Platform=$arch -p:TargetArchitecture=$arch $extraArgs"
    Write-Host "Executing: $command"
    Invoke-Expression -Command $command
    
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