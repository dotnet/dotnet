[CmdletBinding(PositionalBinding=$false)]
param (
  [string][Alias('c')]$configuration = "Release",
  [string][Alias('v')]$verbosity = "minimal",
  [switch]$ci,
  [switch]$prepareMachine,
  [string]$msbuildEngine = $null,
  [switch][Alias('bl')]$binaryLog,
  [switch][Alias('nobl')]$excludeCIBinarylog,
  [switch][Alias('pb')]$productBuild,
  [switch]$fromVMR,
  [bool]$nodeReuse = $true,
  [Parameter(ValueFromRemainingArguments = $true)][string[]]$properties
)

. $PSScriptRoot\..\common\tools.ps1

function Build {
  $bl = if ($binaryLog) { '/bl:' + (Join-Path $LogDir 'Build.binlog') } else { '' }

  MSBuild "$PSScriptRoot\dotnet-build.proj" `
    $bl `
    /p:Configuration=$configuration `
    /p:RepoRoot=$RepoRoot `
    /p:DotNetBuild=$productBuild `
    /p:DotNetBuildFromVMR=$fromVMR `
    @properties
}

try {
  if ($ci) {
    if (-not $excludeCIBinarylog) {
      $binaryLog = $true
    }
    $nodeReuse = $false
  }

  Build
}
catch {
  Write-Host $_.ScriptStackTrace
  Write-PipelineTelemetryError -Category 'InitializeToolset' -Message $_
  ExitWithExitCode 1
}

ExitWithExitCode 0
