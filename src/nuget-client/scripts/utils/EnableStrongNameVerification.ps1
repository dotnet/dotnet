. "$PSScriptRoot\StrongNameVerification.ps1"

$verificationEnablements = Get-StrongNameVerificationEnablements
$isVerificationEnablementRequired = $verificationEnablements.GetEnumerator().Where({ $_.Value -eq $True }, 'First').Count -gt 0

If ($isVerificationEnablementRequired)
{
    Write-Host "Strong name verification must be enabled for some assemblies."

    If (Test-IsAdmin)
    {
        ForEach ($pair In $verificationEnablements.GetEnumerator())
        {
            If ($pair.Value -eq $True)
            {
                Enable-StrongNameVerification -regKey $pair.Name
            }
        }
    }
    Else
    {
        Execute-ScriptAsAdmin -scriptFilePath $MyInvocation.MyCommand.Definition
    }
}
Else
{
    Write-Host "Strong name verification is already enabled for required assemblies."
}