. $PSScriptRoot\common\tools.ps1

$vsInfo = LocateVisualStudio
if ($null -eq $vsInfo) {
  throw "Unable to locate required Visual Studio installation"
}

$vsDir = $vsInfo.installationPath.TrimEnd("\")

$vsRegEdit = Join-Path (Join-Path (Join-Path $vsDir 'Common7') 'IDE') 'VSRegEdit.exe'

$hive = "RoslynDev"
&$vsRegEdit set "$vsDir" $hive HKCU "Roslyn\Internal\OnOff\Features" OOP64Bit dword 0