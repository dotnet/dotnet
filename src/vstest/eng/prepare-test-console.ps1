param (
    [ValidateSet("Debug", "Release")]
    [string] $Configuration = "Release",
    [string] $RepoRoot = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = "Stop"
$RepoRoot = [System.IO.Path]::GetFullPath($RepoRoot)
$artifacts = Join-Path $RepoRoot "artifacts"
$packageDirectory = [System.IO.Path]::Combine($artifacts, "packages", $Configuration, "Shipping")
$packages = @(Get-ChildItem -LiteralPath $packageDirectory -Filter "Microsoft.TestPlatform.CLI.*.nupkg" -File |
    Where-Object { $_.Name -notlike "*.symbols.nupkg" })
if ($packages.Count -ne 1) {
    throw "Expected one Microsoft.TestPlatform.CLI package in $packageDirectory, found $($packages.Count). Cannot select the console under test."
}

$destination = [System.IO.Path]::Combine($artifacts, "tmp", $Configuration, "localConsole")
if (Test-Path -LiteralPath $destination) {
    Remove-Item -LiteralPath $destination -Recurse -Force
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory($packages[0].FullName, $destination)
$contentDirectory = [System.IO.Path]::Combine($destination, "contentFiles", "any")
$consoles = @(Get-ChildItem -LiteralPath $contentDirectory -Recurse -Filter "vstest.console.dll" -File)
if ($consoles.Count -ne 1) {
    throw "Expected one vstest.console.dll under $contentDirectory, found $($consoles.Count)."
}

$console = $consoles[0]
$binLayout = [System.IO.Path]::Combine($artifacts, "bin", "vstest.console", $Configuration, $console.Directory.Name)
if (-not (Test-Path -LiteralPath $binLayout -PathType Container)) {
    throw "No build output for $($console.Directory.Name) at $binLayout. Cannot complete the console layout."
}

# The package supplies Extensions and TestHostNetFramework. The local build supplies
# the runnable dependency layout for this operating system.
Copy-Item -Path (Join-Path $binLayout "*") -Destination $console.Directory.FullName -Recurse -Force
$dotnetName = if ($env:OS -eq "Windows_NT") { "dotnet.exe" } else { "dotnet" }
$dotnet = [System.IO.Path]::Combine($RepoRoot, ".dotnet", $dotnetName)
if (-not (Test-Path -LiteralPath $dotnet -PathType Leaf)) {
    throw "The repository's dotnet executable was not found at $dotnet."
}

# This checks the layout, not authentication or serializer state in later processes.
$helpOutput = & $dotnet exec $console.FullName --help 2>&1
if ($LASTEXITCODE -ne 0) {
    throw "The console at $($console.FullName) did not start (exit $LASTEXITCODE): $($helpOutput -join [Environment]::NewLine)"
}

Write-Host "package : $($packages[0].Name)"
Write-Host "console : $($console.FullName)"
Write-Host "##vso[task.setvariable variable=_LocalVSTestConsolePath]$($console.FullName)"
