<#
This script is used as a payload of Helix jobs that execute Android workloads through XHarness on Windows systems.

The purpose of this script is to time-constrain user commands (command.ps1) so that we have time at the end of the
work item to process XHarness telemetry.
#>

param (
    [Parameter(Mandatory)]
    [string]$app,
    [Parameter(Mandatory)]
    [string]$timeout,
    [Parameter(Mandatory)]
    [int]$command_timeout, # in seconds
    [Parameter()]
    [string]$package_name = $null,
    [Parameter()]
    [int]$expected_exit_code = 0,
    [Parameter()]
    [string]$device_output_path = $null,
    [Parameter()]
    [string]$instrumentation = $null
)

$ErrorActionPreference="Continue"

# Act out the actual commands
# We have to time constrain them to create buffer for the end of this script
$psinfo = [System.Diagnostics.ProcessStartInfo]::new()
$psinfo.FileName = "powershell"
$psinfo.Arguments = " -ExecutionPolicy ByPass -NoProfile -File `"$PSScriptRoot\command.ps1`" -output_directory `"$Env:HELIX_WORKITEM_UPLOAD_ROOT`" -app `"$app`" -timeout `"$timeout`" -package_name `"$package_name`" -expected_exit_code `"$expected_exit_code`" -device_output_path `"$device_output_path`" -instrumentation `"$instrumentation`""
$psinfo.RedirectStandardError = $false
$psinfo.RedirectStandardOutput = $false
$psinfo.UseShellExecute = $false

$process = [System.Diagnostics.Process]::new()
$process.StartInfo = $psinfo
$process.Start()

Wait-Process -InputObject $process -TimeOut $command_timeout -ErrorVariable ev -ErrorAction SilentlyContinue

if ($ev) {
    Stop-Process -InputObject $process -Force
    $process.WaitForExit()
    [Console]::Out.Flush()
    Write-Output "User command timed out after $command_timeout seconds!"

    Write-Output "Removing installed apps after unsuccessful run"
    $packages = dotnet exec $Env:XHARNESS_CLI_PATH android adb -- shell pm list packages net.dot
    $split_packages = $packages.split(':')
    For ($i = 1; $i -lt $split_packages.Length; $i += 2) {
        Write-Output "    Uninstalling $($split_packages[$i])"
        dotnet exec $Env:XHARNESS_CLI_PATH android adb -- uninstall $split_packages[$i]
    }

    exit -3
} else {
    Write-Output "User command ended with $($process.ExitCode)"
    exit $process.ExitCode
}
