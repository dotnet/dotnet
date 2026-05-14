<#
.SYNOPSIS
Replaces all occurances of build version string in project.json and other files

.PARAMETER NewVersion
New version string to set

.PARAMETER OldVersion
Optional. Old version string to replace.
Will use version from .teamcity.properties file if not specified.

.PARAMETER NuGetRoot
Optional. NuGet client repository root.

.PARAMETER Force
Optional switch to force replacing text when new and old versions are the same

.EXAMPLE
UpdateNuGetVersion.ps1 3.5.1 -Verbose
#>
[CmdletBinding(SupportsShouldProcess=$True)]
Param (
    [Parameter(Mandatory=$true, Position=0)]
    [Alias('version')]
    [string]$NewVersion,
    [Parameter(Mandatory=$false, Position=1)]
    [string]$OldVersion,
    [Parameter(Mandatory=$false, Position=2)]
    [string]$NuGetRoot,
    [switch]$Force)

. "$PSScriptRoot\..\common.ps1"

if (-not $NuGetRoot -and (Test-Path Env:\NuGetRoot)) {
    $NuGetRoot = $env:NuGetRoot
}

if (-not $NuGetRoot) {
    $NuGetRoot = Join-Path $PSScriptRoot '..\..\' -Resolve
}

if (-not $OldVersion -and (Test-Path "$NuGetRoot\.teamcity.properties")) {
    $properties = ReadPropertiesFile "$NuGetRoot\.teamcity.properties"
    $OldVersion = $properties['ReleaseProductVersion']
}

if (-not $OldVersion) {
    throw "OLD version string can't be found"
}

if ($OldVersion -eq $NewVersion -and -not $Force) {
    Write-Output "NO-OP [$OldVersion == $NewVersion]"
    exit 0
}

Write-Output "Updating NuGet version [$OldVersion => $NewVersion]"

$SourcesLocation = Join-Path $NuGetRoot src -Resolve
Get-ChildItem $SourcesLocation -Recurse -Filter project.json |
    %{ $_.FullName } |
    ReplaceTextInFiles -old $OldVersion -new $NewVersion -ef '*"System.*'

$TestsLocation = Join-Path $NuGetRoot test -Resolve
Get-ChildItem $TestsLocation -Recurse -Filter project.json |
    %{ $_.FullName } |
    ReplaceTextInFiles -old $OldVersion -new $NewVersion -ef '*"System.*'

$miscFiles = @(
    "src\NuGet.Clients\NuGet.Tools\NuGetPackage.cs",
    "src\NuGet.Clients\NuGet.CommandLine\NuGet.CommandLine.nuspec",
    "build\common.props",
    "build\common.ps1",
    ".teamcity.properties"
)

$miscFiles | %{ Join-Path $NuGetRoot $_ -Resolve } |
    ReplaceTextInFiles -old $OldVersion -new $NewVersion