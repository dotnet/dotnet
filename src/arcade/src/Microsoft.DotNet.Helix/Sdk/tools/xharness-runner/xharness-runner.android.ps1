<#
This script is used as a payload of Helix jobs that execute Android workloads through XHarness on Windows systems.
This file is a header of script that gets populated with user's custom commands.

This script is separate as it is executed with a timeout in its own process.
#>

param (
    [Parameter(Mandatory)]
    [string]$output_directory,
    [Parameter(Mandatory)]
    [string]$app,
    [Parameter(Mandatory)]
    [string]$timeout,
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

# The xharness alias
function xharness() {
    dotnet exec $Env:XHARNESS_CLI_PATH @args
}

dotnet exec $Env:XHARNESS_CLI_PATH android adb -- shell settings put global verifier_verify_adb_installs 0
dotnet exec $Env:XHARNESS_CLI_PATH android adb -- shell settings put global package_verifier_enable 0

# User can call this when they detect a problem they think is caused by the infrastructure
function report_infrastructure_failure($message) {
    Write-Output "Infrastructural problem reported by the user, requesting retry+reboot: $message"

    New-Item -Path "$Env:HELIX_WORKITEM_ROOT" -Name ".retry" -ItemType "file" -Force
    New-Item -Path "$Env:HELIX_WORKITEM_ROOT" -Name ".reboot" -ItemType "file" -Force

    $message -replace "['\\]" | Out-File -FilePath "$Env:HELIX_WORKITEM_ROOT\.retry"
    $message -replace "['\\]" | Out-File -FilePath "$Env:HELIX_WORKITEM_ROOT\.reboot"
}

# User commands begin here

<#%%USER COMMANDS%%#>

# User commands end here

exit $LASTEXITCODE
