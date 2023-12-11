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

MSBuild "$PSScriptRoot/eng/tools/init-build.proj" `
        /bl:"$PSScriptRoot/artifacts/log/Debug/BuildXPlatTasks_$LogDateStamp.binlog" `
        /flp:LogFile="$PSScriptRoot/artifacts/logs/BuildXPlatTasks_$LogDateStamp.log" `
        /t:PrepareOfflineLocalTools `
        /p:DotNetBuildVertical=true

MSBuild "$PSScriptRoot/build.proj" `
    /bl:"$PSScriptRoot/artifacts/log/Debug/Build_$LogDateStamp.binlog" `
    /flp:"LogFile=$PSScriptRoot/artifacts/logs/Build_$LogDateStamp.log" `
    /flp:v=detailed `
    /p:DotNetBuildVertical=true