[CmdletBinding(PositionalBinding=$false)]
Param(
  [string][Alias('c')]$configuration = "Release",
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)

. $PSScriptRoot\eng\common\tools.ps1

# Initialize toolset
$LogDateStamp = Get-Date -Format "MMddHHmmss"
InitializeToolset
$nodeReuse=$false

# Set the NUGET_PACKAGES dir so that we don't accidentally pull some packages from the global location,
# They should be pulled from the local feeds.
$env:NUGET_PACKAGES="$PSScriptRoot/.packages"

try {
  MSBuild "$PSScriptRoot\build.proj" `
      /bl:"$PSScriptRoot\artifacts\log\$configuration\Build_$LogDateStamp.binlog" `
      /flp:"LogFile=$PSScriptRoot\artifacts\log\$configuration\Build_$LogDateStamp.log" `
      /flp:v=detailed `
      --tl:off `
      /p:Configuration=$configuration `
      @properties
}
catch {
  Write-Host $_.ScriptStackTrace
  Write-PipelineTelemetryError -Category 'Build' -Message $_
  ExitWithExitCode 1
}