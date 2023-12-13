<#
.SYNOPSIS
Configures NuGet.Client build environment. Detects and initializes
VS build toolsets. Configuration settings are stored at configure.json file.

.PARAMETER CleanCache
Cleans NuGet packages cache before build

.PARAMETER Force
Switch to force installation of required tools.

.PARAMETER Test
Indicates the Tests need to be run. Downloads the Test cli when tests are needed to run.

.EXAMPLE
.\configure.ps1 -cc -v
Clean repo build environment configuration

.EXAMPLE
.\configure.ps1 -v
Incremental install of build tools
#>
[CmdletBinding(SupportsShouldProcess=$True)]
Param (
    [Alias('cc')]
    [switch]$CleanCache,
    [Alias('f')]
    [switch]$Force,
    [switch]$RunTest,
    [switch]$SkipDotnetInfo,
    [switch]$ProcDump
)

$ErrorActionPreference = 'Stop'

. "$PSScriptRoot\build\common.ps1"

Trace-Log "Configuring NuGet.Client build environment"

if ($env:CI -eq "true") {
    [Environment]::SetEnvironmentVariable("NUGET_PACKAGES", $null, "Machine")
} else {
    $Env:NUGET_PACKAGES=""
}

$BuildErrors = @()

if ($ProcDump -eq $true -Or $env:CI -eq "true")
{
    Invoke-BuildStep 'Configuring Process Dump Collection' {
    
        Install-ProcDump
    } -ev +BuildErrors
}

Invoke-BuildStep 'Configuring git repo' {
    Update-SubModules -Force:$Force
} -ev +BuildErrors

Invoke-BuildStep 'Installing .NET CLI' {
    Install-DotnetCLI -Force:$Force -SkipDotnetInfo:$SkipDotnetInfo
} -ev +BuildErrors

Invoke-BuildStep 'Cleaning package cache' {
    Clear-PackageCache
} -skip:(-not $CleanCache) -ev +BuildErrors

if ($BuildErrors) {
    $ErrorLines = $BuildErrors | %{ ">>> $($_.Exception.Message)" }
    Write-Error "Build's completed with $($BuildErrors.Count) error(s):`r`n$($ErrorLines -join "`r`n")" -ErrorAction Stop
}
