#requires -version 5

param(
  [Parameter(Mandatory = $true)]
  [ValidateNotNullOrEmpty()]
  [string]
  $rootSuffix,

  [Parameter(Mandatory = $true)]
  [ValidateNotNullOrEmpty()]
  [string]
  $devenvExePath
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 1

$success=$false
for($i=0; $i -le 3; $i++)
{
  & $devenvExePath /rootsuffix $rootSuffix /updateConfiguration
  if(Test-Path -Path $env:LocalAppData\Microsoft\VisualStudio\17.0*RoslynDev)
  {
    Write-Host "The hive 'RoslynDev' exists"
    $success=$true
    break
  }
}

if($success -eq $false){
  throw "Failed to create hive"
}
