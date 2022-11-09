#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

[CmdletBinding(PositionalBinding=$false)]
param(
    [string]$Configuration="Debug",
    [string]$Architecture="x64",
    [switch]$Sign=$false,
    [switch]$PgoInstrument,
    [bool]$WarnAsError=$true,
    [Parameter(ValueFromRemainingArguments=$true)][String[]]$ExtraParameters
)

$RepoRoot = "$PSScriptRoot"

$Parameters = "/p:Architecture=$Architecture"
$Parameters = "$Parameters -configuration $Configuration"

if ($PgoInstrument) {
  $Parameters = "$Parameters /p:PgoInstrument=true"
}

if ($Sign) {
  $Parameters = "$Parameters -sign /p:SignCoreSdk=true"

  # Workaround https://github.com/dotnet/arcade/issues/1776
  $WarnAsError = $false
}

$Parameters = "$Parameters -WarnAsError `$$WarnAsError"

try {
    $ExpressionToInvoke = "$RepoRoot\eng\common\build.ps1 -restore -build $Parameters $ExtraParameters"
    Write-Host "Invoking expression: $ExpressionToInvoke"
    Invoke-Expression $ExpressionToInvoke
}
catch {
 Write-Error $_
 Write-Error $_.ScriptStackTrace
 throw "Failed to build"
}

if($LASTEXITCODE -ne 0) { throw "Failed to build" }
