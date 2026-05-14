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
  if(Test-Path -Path $env:LocalAppData\Microsoft\VisualStudio\18.0*RoslynDev)
  {
    Write-Host "The hive 'RoslynDev' exists"
    $success=$true
    break
  }
}

if($success -eq $false)
{
  throw "Failed to create hive"
}

$vsDir = Split-Path -Parent $devenvExePath

$vsRegEdit = Join-Path $vsDir 'VsRegEdit.exe'

&$vsRegEdit set "$vsDir" RoslynDev HKLM "Profile" DisableFirstLaunchDialog dword 1

Write-Host "-- VS Info --"
$isolationIni = Join-Path $vsDir 'devenv.isolation.ini'
Get-Content $isolationIni | Write-Host
Write-Host "-- /VS Info --"
