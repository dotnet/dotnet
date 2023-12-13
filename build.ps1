[CmdletBinding(PositionalBinding=$false)]
Param(
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)


. $PSScriptRoot\eng\common\tools.ps1

# Initialize toolset

$LogDateStamp = Get-Date -Format "MMddHHmmss"

InitializeToolset

# Build the init-tools project so that we get the xplat tasks, specifically
# the msbuild SDK resolver required to force the rest of the repos to use the right arcade SDK.

# Set the NUGET_PACKAGES dir so that we don't accidentally pull some packages from the global location,
# They should be pulled from the local feeds.
$env:NUGET_PACKAGES="$PSScriptRoot/.packages"

try {
  $nodeReuse=$false

  MSBuild "$PSScriptRoot/build.proj" `
      /bl:"$PSScriptRoot/artifacts/log/Debug/Build_$LogDateStamp.binlog" `
      /flp:"LogFile=$PSScriptRoot/artifacts/logs/Build_$LogDateStamp.log" `
      /flp:v=detailed `
      /p:DotNetBuildVertical=true
}
catch {
  Write-Host $_.ScriptStackTrace
  Write-PipelineTelemetryError -Category 'Build' -Message $_
  ExitWithExitCode 1
}