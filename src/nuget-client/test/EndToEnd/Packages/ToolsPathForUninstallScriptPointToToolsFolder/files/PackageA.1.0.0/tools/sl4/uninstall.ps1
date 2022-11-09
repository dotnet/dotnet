param($rootPath, $toolsPath, $package) 

$scriptPath = $script:MyInvocation.MyCommand.Path

Write-Host "uninstall is running"

$path = Join-Path $toolsPath "uninstall.ps1"

if ($scriptPath -ne $path)
{
    throw "'$toolsPath' value is wrong."
}