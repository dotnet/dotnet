[CmdletBinding()]
param(
    [switch]$restore,
    [switch]$build,
    [switch]$clean,
    [string]$configuration = "Debug",
    [Parameter(ValueFromRemainingArguments=$true)]
    [string[]]$RemainingArgs = @()
)

function Stop-WindowsDesktopProcesses {
    Write-Host "Cleaning up WindowsDesktop build processes..." -ForegroundColor Yellow
    
    $processes = Get-Process | Where-Object {
        $_.ProcessName -eq "dotnet" -and 
        $_.Path -like "*WindowsDesktop*"
    }
    
    if ($processes) {
        $processes | ForEach-Object {
            Write-Host "  Stopping process $($_.Id) ($($_.ProcessName))" -ForegroundColor Gray
            try {
                Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
            } catch {
                Write-Warning "Failed to stop process $($_.Id): $_"
            }
        }
        Start-Sleep -Seconds 2
    } else {
        Write-Host "  No WindowsDesktop processes found." -ForegroundColor Gray
    }
}



# Main execution
try {
    # Clean up any existing processes
    Stop-WindowsDesktopProcesses
    
    # Prepare arguments for Build.ps1
    $buildArguments = @()
    if ($restore) { $buildArguments += '-restore' }
    if ($build) { $buildArguments += '-build' }
    if ($clean) { $buildArguments += '-clean' }
    if ($configuration) { 
        $buildArguments += '-configuration'
        $buildArguments += $configuration
    }
    $buildArguments += $RemainingArgs
    
    Write-Host "Starting sequential build with single CPU..." -ForegroundColor Green
    Write-Host "Arguments: $($buildArguments -join ' ')" -ForegroundColor Gray
    
    # Run the build with sequential settings
    try {
        & "$PSScriptRoot\eng\common\Build.ps1" -nodeReuse $false @buildArguments "/maxcpucount:1"
        $exitCode = $LASTEXITCODE
    } catch {
        Write-Error "Build failed: $_"
        $exitCode = 1
    }
    
    Write-Host "`nBuild completed with exit code: $exitCode" -ForegroundColor $(if ($exitCode -eq 0) { 'Green' } else { 'Red' })
    
} finally {
    # Always clean up processes after build
    Write-Host "`nCleaning up processes after build..." -ForegroundColor Yellow
    Stop-WindowsDesktopProcesses
}

exit $exitCode