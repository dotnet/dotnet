#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Logs Windows diagnostic information for build troubleshooting.

.DESCRIPTION
    This script logs detailed Windows environment information including:
    - OS details and version
    - Windows registry information (product, edition, build)
    - Recent Windows updates
    - Tar version information
#>

Write-Host "=== WINDOWS BUILD ENVIRONMENT DIAGNOSTICS ===" -ForegroundColor Cyan
Write-Host ""

# OS Information
Write-Host "OS: $([System.Runtime.InteropServices.RuntimeInformation]::OSDescription)"
Write-Host "OS Architecture: $([System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture)"
Write-Host "OS Version: $([System.Environment]::OSVersion)"
Write-Host "PowerShell Version: $($PSVersionTable.PSVersion)"
Write-Host ""

# Windows-specific information
if ($IsWindows -or $env:OS -eq "Windows_NT") {
    try {
        $buildInfo = Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion" -ErrorAction SilentlyContinue

        if ($buildInfo.ProductName) { Write-Host "Windows Product: $($buildInfo.ProductName)" }
        if ($buildInfo.EditionID) { Write-Host "Windows Edition: $($buildInfo.EditionID)" }
        if ($buildInfo.CurrentBuild) { Write-Host "Windows Build: $($buildInfo.CurrentBuild)" }
        if ($buildInfo.ReleaseId) { Write-Host "Windows Release: $($buildInfo.ReleaseId)" }
        if ($buildInfo.DisplayVersion) { Write-Host "Windows Display Version: $($buildInfo.DisplayVersion)" }

        # Detailed build information for patch level identification
        if ($buildInfo.CurrentBuild -and $buildInfo.UBR) {
            Write-Host "Full Build Version: $($buildInfo.CurrentBuild).$($buildInfo.UBR)" -ForegroundColor Green
        }
        if ($buildInfo.BuildBranch) { Write-Host "Build Branch: $($buildInfo.BuildBranch)" }
        if ($buildInfo.BuildLab) { Write-Host "Build Lab: $($buildInfo.BuildLab)" }
        if ($buildInfo.BuildLabEx) { Write-Host "Build Lab Ex: $($buildInfo.BuildLabEx)" }
        if ($buildInfo.UBR) { Write-Host "UBR (Update Build Revision): $($buildInfo.UBR)" -ForegroundColor Cyan }

        Write-Host ""

        # Check installed Windows updates
        Write-Host "Recent Windows Updates:" -ForegroundColor Yellow
        try {
            $recentUpdates = Get-HotFix | Sort-Object InstalledOn -Descending | Select-Object -First 5
            if ($recentUpdates) {
                $recentUpdates | ForEach-Object {
                    Write-Host "  $($_.HotFixID) - $($_.Description) - $($_.InstalledOn)" -ForegroundColor White
                }
            } else {
                Write-Host "  No hotfix information available" -ForegroundColor Gray
            }
        }
        catch {
            Write-Host "  Could not retrieve hotfix information: $_" -ForegroundColor Yellow
        }
    }
    catch {
        Write-Host "Could not retrieve Windows registry information: $_" -ForegroundColor Yellow
    }
}

# Display tar version and location
Write-Host ""
Write-Host "Tar information:" -ForegroundColor Yellow
try {
    $tarCommand = Get-Command tar -ErrorAction SilentlyContinue
    if ($tarCommand) {
        Write-Host "  Path: $($tarCommand.Source)" -ForegroundColor Green
    } else {
        Write-Host "  Tar command not found in PATH" -ForegroundColor Red
    }
}
catch {
    Write-Host "  Could not retrieve tar location: $_" -ForegroundColor Yellow
}

try {
    $tarVersion = & tar --version 2>&1 | Select-Object -First 1
    Write-Host "  Version: $tarVersion"
}
catch {
    Write-Host "  Could not retrieve tar version: $_" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== END WINDOWS BUILD ENVIRONMENT DIAGNOSTICS ===" -ForegroundColor Cyan
Write-Host ""
