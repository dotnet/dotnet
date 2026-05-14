function CleanTempFolder()
{
    if (Test-Path $env:temp)
    {
        Write-Host 'Deleting temp folder'
        Get-ChildItem $env:temp | Remove-Item -Recurse -ErrorAction SilentlyContinue
        Write-Host 'Done.'
    }
}

function IsAdminPrompt()
{
    If (-NOT ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(`
        [Security.Principal.WindowsBuiltInRole] "Administrator"))
    {
        return $false
    }

    return $true
}

function GetRegistryKey
{
    param (
    [Parameter(Mandatory=$true)]
    [string]$RegKey,
    [Parameter(Mandatory=$true)]
    [string]$RegName)

    if (!(Test-Path -Path $RegKey -PathType Container))
    {
        return $null
    }

    $props = Get-ItemProperty -Path $RegKey
    if (!($props) -or ($props.Length -eq 0))
    {
        return $null
    }

    $name = Get-Member -InputObject $props -Name $RegName
    if (!($name))
    {
        return $null
    }

    $value = Get-ItemPropertyValue -Path $RegKey -Name $RegName
    return $value
}

function SetRegistryKey
{
    param (
    [Parameter(Mandatory=$true)]
    [string]$RegKey,
    [Parameter(Mandatory=$true)]
    [string]$RegName,
    [Parameter(Mandatory=$true)]
    $ExpectedValue,
    [switch]$NeedAdmin)

    $currentValue = GetRegistryKey $RegKey $RegName

    $fullPath = Join-Path $RegKey $RegName

    if (-not $currentValue) {
        Write-Host "$fullPath is not already set"
    } else {
        Write-Host "Current value of $fullPath is '$currentValue'"
    }
    if ($currentValue -eq $ExpectedValue)
    {
        Write-Host -ForegroundColor Cyan "Registry settings for $fullPath is already '$ExpectedValue'."
        return $true
    }
    else
    {
        if ($NeedAdmin -and !(IsAdminPrompt))
        {
            return $false
        }

        If (!(Test-Path $RegKey))
        {
            New-Item -Path $RegKey -Force | Out-Null
        }

        New-ItemProperty -Path $RegKey -Name $RegName -Value $ExpectedValue -Force | Out-Null
        Write-Host "$fullPath set to '$ExpectedValue'"
    }

    return $true
}


function DisableCrashDialog()
{
    # Set the registry key to prevent the 'Not Responding' window from showing up on a crash
    $result1 = SetRegistryKey "HKLM:\SOFTWARE\Microsoft\Windows\Windows Error Reporting" "ForceQueue" 1 -NeedAdmin
    $result2 = SetRegistryKey "HKLM:\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Consent" "DefaultConsent" 1 -NeedAdmin

    If (($result1 -eq $false) -or ($result2 -eq $false))
    {
        $warningMessage = 'WARNING: Please re-run this script as an Administrator! Setting registry keys need admin privileges.'

        Write-Host -ForegroundColor Yellow $warningMessage
        exit 1
    }

    Write-Host -ForegroundColor Cyan 'Windows crash dialog has been disabled'
}

function WaitForProcessExit
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string]$ProcessName,
        [Parameter(Mandatory=$true)]
        [int]$TimeoutInSeconds
    )

    try
    {
        Wait-Process -Name "$ProcessName" -Timeout $TimeoutInSeconds -ErrorAction Stop
    }
    catch [System.Management.Automation.ActionPreferenceStopException]
    {
        if ($_.Exception -is [System.TimeoutException])
        {
            throw;
        }

        # Otherwise, the process could not be found.
    }
}