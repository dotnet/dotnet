
# SUMMARY
# This script will kill iis express processes.
# This is used with our functional tests.


Write-Host "Cleaning up iisexpress"

$s = Get-Process -Name iisexpress -ErrorAction SilentlyContinue

if($s -ne $null)
{
    $s | Stop-Process
}
else
{
    Write-Host "IISExpress.exe not found"
}

$s = Get-Process -Name iisexpresstray -ErrorAction SilentlyContinue

if($s -ne $null)
{
    $s | Stop-Process
}
else
{
    Write-Host "iisexpresstray.exe not found"
}

Write-Host "Cleaning up iisexpresstray completed"

