[CmdletBinding(PositionalBinding = $false)]
param(
  [string]$flag = $null,
  [switch]$enable,
  [switch]$disable,
  [string]$hive = "RoslynDev",
  [switch]$set,
  [switch]$get
)

if ($null -eq $flag -or '' -eq $flag) {
  throw "Specify a -flag to set"
}

if ($flag.EndsWith("\")) {
  throw "Provided flag '$flag' ends with '\', which is not valid"
}

if ($set -and $get) {
  throw "Use only one of set or get"
}

if (-not ($set -or $get)) {
  throw "Specify one of -set or -get"
}

if ($set) {
  if ($enable -and $disable) {
    throw "Use only one of -enable or -disable"
  }

  if (-not ($enable -or $disable)) {
    throw "Specify one of -enable or -disable"
  }
}

$value = 0

if ($enable) {
  $value = 1
}

$slashIndex = $flag.LastIndexOf("\")

if ($slashIndex -ge 0) {
  $flagBase = $flag.Substring(0, $slashIndex)
  $flag = $flag.Substring($slashIndex + 1) #+1 to trim the \
}

if ($flag.IndexOf('.') -ge 0) {
  Write-Host "Replacing . in $flag with \"
  $flag = $flag.Replace(".", "\")
  Write-Host "New value for flag: $flag"
}

if (-not ($flag -like "FeatureFlags*"))
{
  Write-Host "FeatureFlags was not present, modifying $flag"
  $flag = "FeatureFlags\" + $flag
  Write-Host "New value for flag: $flag"
}

if ($set) {
  Write-Host "Attempting to modify '$flag' to '$value'"
}

$engPath = Join-Path $PSScriptRoot ".."
$commonPath = Join-Path $engPath "common"
$toolScript = Join-Path $commonPath "tools.ps1"

Write-Host "Executing '$toolScript'"
. $toolScript

$vsInfo = LocateVisualStudio
if ($null -eq $vsInfo) {
  throw "Unable to locate required Visual Studio installation"
}

Write-Host "Running VsRegEdit"
$vsDir = $vsInfo.installationPath.TrimEnd("\")
$vsRegEdit = Join-Path (Join-Path (Join-Path $vsDir 'Common7') 'IDE') 'VsRegEdit.exe'

Write-Host "Current value:"
&$vsRegEdit read "$vsDir" $hive HKCU $flag VALUE dword

if ($set) {
  Write-Host "Running $vsRegEdit set `"$vsDir`" $hive HKCU $flag VALUE dword $value"
  &$vsRegEdit set "$vsDir" $hive HKCU $flag VALUE dword $value
}
