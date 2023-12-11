[CmdletBinding(PositionalBinding=$false)]
Param(
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)


. $PSScriptRoot\eng\common\tools.ps1

# Initialize toolset

$LogDateStamp = Get-Date -Format "MMddHHmmss"

InitializeToolset

MSBuild "$PSScriptRoot/build.proj" `
    /bl:"$PSScriptRoot/artifacts/log/Debug/Build_$LogDateStamp.binlog" `
    /flp:"LogFile=$PSScriptRoot/artifacts/logs/Build_$LogDateStamp.log" `
    /flp:v=detailed `
    /p:DotNetBuildVertical=true