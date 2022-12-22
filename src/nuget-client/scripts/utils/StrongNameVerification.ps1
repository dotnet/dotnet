Set-Variable MicrosoftPublicKeyToken -option Constant -value 'b03f5f7f11d50a3a'
Set-Variable NuGetPublicKeyToken -option Constant -value '31bf3856ad364e35'

Function Get-StrongNameVerificationEnablements()
{
    $publicKeyTokens = @($MicrosoftPublicKeyToken, $NuGetPublicKeyToken)
    $verificationEnablements = @{ }

    ForEach ($publicKeyToken In $publicKeyTokens)
    {
        $regKey = "HKLM:SOFTWARE\Microsoft\StrongName\Verification\*,$publicKeyToken"

        $verificationEnablements[$regKey] = Test-Path $regKey

        If (Test-Path "HKLM:SOFTWARE\Wow6432Node")
        {
            $regKey = "HKLM:SOFTWARE\Wow6432Node\Microsoft\StrongName\Verification\*,$publicKeyToken"

            $verificationEnablements[$regKey] = Test-Path $regKey
        }
    }

    Return $verificationEnablements
}

Function Enable-StrongNameVerification([Parameter(Mandatory = $True)] [string] $regKey)
{
    If (Test-Path $regKey)
    {
        Write-Host "Enabling .NET strong name verification with $regKey."

        Remove-Item -Path $regKey -Force | Out-Null
    }
}

Function Disable-StrongNameVerification([Parameter(Mandatory = $True)] [string] $regKey)
{
    If (-Not (Test-Path $regKey))
    {
        Write-Host "Disabling .NET strong name verification with $regKey."

        New-Item -Path (Split-Path $regKey) -Name (Split-Path -Leaf $regKey) -Force | Out-Null
    }
}

Function Test-IsAdmin()
{
    [Security.Principal.WindowsPrincipal] $currentUser = [Security.Principal.WindowsIdentity]::GetCurrent()

    Return $currentUser.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

Function Execute-ScriptAsAdmin([Parameter(Mandatory = $True)] [string] $scriptFilePath)
{
    Write-Host "The current process is not elevated.  Launching new process elevated..."

    $arguments = '-ExecutionPolicy Bypass -NoLogo -NoProfile -File "{0}"' -f $scriptFilePath
    $location = Get-Location

    $process = Start-Process powershell.exe -Verb RunAs -WorkingDirectory $location -PassThru -Wait -ArgumentList $arguments

    If ($process.ExitCode -ne 0)
    {
        Throw [System.Exception]::new("Process exited with code $($process.ExitCode) after executing $scriptFilePath.");
    }
}