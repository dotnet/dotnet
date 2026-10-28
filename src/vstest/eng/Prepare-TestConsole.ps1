[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug'
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$packageDirectory = Join-Path $repoRoot "artifacts\packages\$Configuration\Shipping"
$packages = @(Get-ChildItem -LiteralPath $packageDirectory -Filter 'Microsoft.TestPlatform.CLI.*.nupkg' -File |
    Where-Object { $_.Name -notlike '*.symbols.nupkg' })
if ($packages.Count -ne 1) {
    throw "Expected one Microsoft.TestPlatform.CLI package in '$packageDirectory', found $($packages.Count). Build and pack this configuration first."
}

$destination = Join-Path $repoRoot "artifacts\tmp\$Configuration\localConsole"
if (Test-Path -LiteralPath $destination) {
    Remove-Item -LiteralPath $destination -Recurse -Force
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory($packages[0].FullName, $destination)
$consoles = @(Get-ChildItem -LiteralPath (Join-Path $destination 'contentFiles\any') -Recurse -Filter 'vstest.console.dll' -File)
if ($consoles.Count -ne 1) {
    throw "Expected one vstest.console.dll under '$destination\contentFiles\any', found $($consoles.Count)."
}

$console = $consoles[0]
$binLayout = Join-Path $repoRoot "artifacts\bin\vstest.console\$Configuration\$($console.Directory.Name)"
if (-not (Test-Path -LiteralPath $binLayout)) {
    throw "Build output not found at '$binLayout'; cannot complete the console layout."
}

# The package supplies Extensions and TestHostNetFramework; build output supplies runtime assets.
Copy-Item -Path (Join-Path $binLayout '*') -Destination $console.Directory.FullName -Recurse -Force
$dotnet = Join-Path $repoRoot '.dotnet\dotnet.exe'
if (-not (Test-Path -LiteralPath $dotnet)) {
    throw "Repository dotnet not found at '$dotnet'. Restore the pinned toolchain first."
}

$startupOutput = & $dotnet exec $console.FullName --help 2>&1
if ($LASTEXITCODE -ne 0) {
    throw "The console at '$($console.FullName)' did not start (exit $LASTEXITCODE): $($startupOutput -join [Environment]::NewLine)"
}

Write-Host "package : $($packages[0].Name)"
Write-Host "console : $($console.FullName)"
if ($env:TF_BUILD -eq 'True') {
    Write-Host "##vso[task.setvariable variable=_LocalVSTestConsolePath]$($console.FullName)"
}

$console.FullName
