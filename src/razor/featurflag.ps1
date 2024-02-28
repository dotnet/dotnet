[CmdletBinding(PositionalBinding=$false)]
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

$flagBase = "FeatureFlags\Razor\LSP"
$slashIndex = $flag.LastIndexOf("\")

if ($slashIndex -ge 0) {
    $flagBase = $flag.Substring(0, $slashIndex)
    $flag = $flag.Substring($slashIndex + 1) #+1 to trim the \
}

if ($set -and $get) {
    throw "Use only one of set or get"
}

if (-not ($set -or $get)) {
    throw "Specify one of -set or -get"
}

if ($set)
{
  if ($enable -and $disable) {
    throw "Use only one of -enable or -disable"
  }

  if (-not ($enable -or $disable)) {
    throw "Specify one of -enable or -disable"
  }
}

. (Join-Path $PSScriptRoot "eng" "common" "tools.ps1")

$vsInfo = LocateVisualStudio
if ($null -eq $vsInfo) {
  throw "Unable to locate required Visual Studio installation"
}

$vsDir = $vsInfo.installationPath.TrimEnd("\")
$vsRegEdit = Join-Path (Join-Path (Join-Path $vsDir 'Common7') 'IDE') 'VsRegEdit.exe'

$value = $enable ? 1 : 0

if ($set) {
  &$vsRegEdit set "$vsDir" $hive HKCU $flagBase $flag dword $value
}
else {
  &$vsRegEdit read "$vsDir" $hive HKCU $flagBase $flag dword
}
 