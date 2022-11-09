<#
.SYNOPSIS
Build and run unit-tests and functional tests.

.PARAMETER Configuration
Build configuration (debug by default)

.PARAMETER ReleaseLabel
Release label to use for package and assemblies versioning (zlocal by default)

.PARAMETER BuildNumber
Build number to use for package and assemblies versioning (auto-generated if not provided)

.PARAMETER SkipCore
Skips running NuGet.Core.Tests and NuGet.Core.FuncTests

.PARAMETER CI
Indicates the build script is invoked from CI

.NOTES
Some of the functional tests including NuGet server ecosystem will fail outside of Microsoft corpnet.

.EXAMPLE
.\runTests.ps1 -Verbose
Running full test suite

.EXAMPLE
.\runTests.ps1 -sut
Running functional tests only

.EXAMPLE
.\runTests.ps1 -sft -s14 -s15
Running core unit tests only
#>
[CmdletBinding()]
param (
    [ValidateSet('debug', 'release')]
    [Alias('c')]
    [string]$Configuration,
    [ValidatePattern('^(beta|final|preview|rc|release|rtm|xprivate|zlocal)([0-9]*)$')]
    [Alias('l')]
    [string]$ReleaseLabel = 'zlocal',
    [Alias('n')]
    [int]$BuildNumber,
    [Alias('sb')]
    [switch]$SkipBuild,
    [Alias('sc')]
    [switch]$SkipCore,
    [switch]$CI
)

. "$PSScriptRoot\build\common.ps1"

if (-not $Configuration) {
    $Configuration = switch ($CI.IsPresent) {
        $True   { 'Release' } # CI build is Release by default
        $False  { 'Debug' } # Local builds are Debug by default
    }
}

Write-Host ("`r`n" * 3)
Trace-Log ('=' * 60)

$startTime = [DateTime]::UtcNow
if (-not $BuildNumber) {
    $BuildNumber = Get-BuildNumber
}

Invoke-BuildStep 'Installing .NET CLI for tests' {
        Install-DotnetCLI -Force:$Force
    } -ev +BuildErrors

Trace-Log "Test suite run #$BuildNumber started at $startTime"

Test-BuildEnvironment -CI:$CI

if (-not $VSToolsetInstalled) {
    Warning-Log "The build is requested, but no toolset is available"
    exit 1
}

$BuildErrors = @()

Invoke-BuildStep 'Cleaning package cache' {
        Clear-PackageCache
    } `
    -skip:(-not $CI) `
    -ev +BuildErrors

Invoke-BuildStep 'Running /t:RestoreVS' {

    & $MSBuildExe build\build.proj /t:RestoreVS /p:Configuration=$Configuration /p:ReleaseLabel=$ReleaseLabel /p:BuildNumber=$BuildNumber /v:m /m:1

    if (-not $?)
    {
        Write-Error "Restore failed!"
        exit 1
    }
} `
-ev +BuildErrors



Invoke-BuildStep 'Running /t:CoreFuncTests' {

    & $MSBuildExe build\build.proj /t:CoreFuncTests /p:Configuration=$Configuration /p:ReleaseLabel=$ReleaseLabel /p:BuildNumber=$BuildNumber /v:m /m:1

    if (-not $?)
    {
        Write-Error "CoreFuncTests failed!"
        exit 1
    }
} `
-ev +BuildErrors

Invoke-BuildStep 'Cleaning package cache' {
        Clear-PackageCache
    } `
    -skip:(-not $CI) `
    -ev +BuildErrors

Trace-Log ('-' * 60)

## Calculating Build time
$endTime = [DateTime]::UtcNow
Trace-Log "Test suite run has completed at $endTime"
Trace-Log "Time elapsed $(Format-ElapsedTime ($endTime - $startTime))"

Trace-Log ('=' * 60)

if ($BuildErrors) {
    $ErrorLines = $BuildErrors | %{ ">>> $($_.Exception.Message)" }
    Write-Error "Build's completed with $($BuildErrors.Count) error(s):`r`n$($ErrorLines -join "`r`n")" -ErrorAction Stop
}


Write-Host ("`r`n" * 3)
