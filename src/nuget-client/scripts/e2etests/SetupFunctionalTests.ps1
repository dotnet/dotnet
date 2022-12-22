param (
    [ValidateSet("17.0")]
    [string]$VSVersion = "17.0")

 . "$PSScriptRoot\Utils.ps1"
 . "$PSScriptRoot\VSUtils.ps1"

function EnableWindowsDeveloperMode()
{
    $windowsDeveloperModeKey = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock"
    $success = SetRegistryKey $windowsDeveloperModeKey "AllowDevelopmentWithoutDevLicense" 1 -NeedAdmin
    if (!($success))
    {
        exit 1
    }

    Write-Host -ForegroundColor Cyan 'Windows Developer mode has been enabled.'
}

function DisableTextTemplateSecurityWarning([string]$VSVersion)
{
    $registryKey = Join-Path "HKCU:\SOFTWARE\Microsoft\VisualStudio" $VSVersion
    $registryKey = Join-Path $registryKey "ApplicationPrivateSettings\Microsoft\VisualStudio\TextTemplating\VSHost\OrchestratorOptionsAutomation"

    $success = SetRegistryKey $registryKey "ShowWarningDialog" 1*System.Boolean*False
    if (!($success))
    {
        exit 1
    }

    $registryKey = Join-Path "HKCU:\SOFTWARE\Microsoft\VisualStudio" $VSVersion
    $registryKey = Join-Path $registryKey "DSLTools"

    $success = SetRegistryKey $registryKey "ShowWarningDialog" False
    if (!($success))
    {
        exit 1
    }

    $message = 'Disabled security message for Text Templates. To re-enable,' `
    + 'Go to Visual Studio -> Tools -> Options -> Text Templating -> Show Security Message. Change it to True.'

    Write-Host -ForegroundColor Cyan $Message
}

Function SuppressNuGetUI([Parameter(Mandatory = $True)] [string] $registryValueName)
{
    $success = SetRegistryKey -RegKey 'HKCU:\Software\NuGet' -RegName $registryValueName -ExpectedValue '1'

    If (!$success)
    {
        Exit 1
    }
}

function  Set-VSINSTALLDIR
{
    # E2E CI agents should only have a single version of VS installed
    $VSInstance = Get-LatestVSInstance
    $installationPath = $VSInstance.installationPath
    Write-Host "Setting $$env:VSINSTALLDIR = $installationPath"
    $env:VSINSTALLDIR = $installationPath
    Write-Host "##vso[task.setvariable variable=VSINSTALLDIR]$installationPath"
}

trap
{
    Write-Host $_.Exception -ForegroundColor Red
    exit 1
}


$NuGetRoot = Split-Path $PSScriptRoot -Parent

$NuGetTestsPSM1 = Join-Path $NuGetRoot "NuGet.Tests.psm1"

Write-Host "NuGetTestsPSM1 variable value is:" $NuGetTestsPSM1

Write-Host 'If successful, this script needs to be run only once on your machine!'

Write-Host 'Setting the environment variable needed to load NuGet.Tests powershell module ' `
'for running functional tests...'

[Environment]::SetEnvironmentVariable("NuGetFunctionalTestPath", $NuGetTestsPSM1, "User")
Write-Host -ForegroundColor Cyan 'You can now call Run-Test from any instance of Visual Studio ' `
'as soon as you open Package Manager Console!'

Write-Host
Write-Host 'Trying to set some registry keys to avoid dialog boxes popping during the functional test run...'

DisableTextTemplateSecurityWarning $VSVersion
SuppressNuGetUI -registryValueName 'DoNotShowPreviewWindow'
SuppressNuGetUI -registryValueName 'SuppressUILegalDisclaimer'

#EnableWindowsDeveloperMode
Set-VSINSTALLDIR

Write-Host 'THE END!'
