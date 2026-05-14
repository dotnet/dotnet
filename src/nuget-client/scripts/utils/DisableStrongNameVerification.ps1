param ([switch] $skipNoOpMessage)

. "$PSScriptRoot\StrongNameVerification.ps1"

$verificationEnablements = Get-StrongNameVerificationEnablements
$isVerificationDisablementRequired = $verificationEnablements.GetEnumerator().Where({ $_.Value -eq $False }, 'First').Count -gt 0

If ($isVerificationDisablementRequired)
{
    Write-Host "Strong name verification must be disabled for some assemblies."

    If (Test-IsAdmin)
    {
        ForEach ($pair In $verificationEnablements.GetEnumerator())
        {
            If ($pair.Value -eq $False)
            {
                Disable-StrongNameVerification -regKey $pair.Name
            }
        }
    }
    Else
    {
        Execute-ScriptAsAdmin -scriptFilePath $MyInvocation.MyCommand.Definition
    }
}
ElseIf (-Not $skipNoOpMessage)
{
    Write-Host "Strong name verification is already disabled for required assemblies."
}