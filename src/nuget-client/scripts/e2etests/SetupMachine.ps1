. "$PSScriptRoot\Utils.ps1"
. "$PSScriptRoot\NuGetFunctionalTestUtils.ps1"

$success = IsAdminPrompt

if ($success -eq $false)
{
    $errorMessage = 'ERROR: Please re-run this script as an Administrator! ' +
    'Updating registry require admin privileges.'

    Write-Error $errorMessage
    exit 1
}

# Already checked if the prompt is an admin prompt
DisableCrashDialog