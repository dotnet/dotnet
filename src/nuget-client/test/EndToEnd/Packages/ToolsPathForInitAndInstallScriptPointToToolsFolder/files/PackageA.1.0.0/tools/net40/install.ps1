param($rootPath, $toolsPath, $package) 

$scriptPath = $script:MyInvocation.MyCommand.Path

$path = Join-Path $toolsPath "install.ps1"

if ($scriptPath -ne $path)
{
    throw "'$toolsPath' value is wrong."
}