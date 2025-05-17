
# The VMR orchestrator passes a number of stnadar
param (
  [string][Alias('c')]$configuration = "Release",
  [string][Alias('v')]$verbosity = "minimal",
  [switch]$ci,
  [switch]$bl,
  [switch][Alias('pb')]$productBuild
  [Parameter(ValueFromRemainingArguments = $true)]
  [string[]]$AdditionalArgs
)

# This will exec a process using the console and return it's exit code.
# This will not throw when the process fails.
# Returns process exit code.
function Exec-Process([string]$command, [string]$commandArgs) {
    $startInfo = New-Object System.Diagnostics.ProcessStartInfo
    $startInfo.FileName = $command
    $startInfo.Arguments = $commandArgs
    Write-Host $command
    WRite-Host $commandArgs
    $startInfo.UseShellExecute = $false
    $startInfo.WorkingDirectory = Get-Location
  
    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = $startInfo
    $process.Start() | Out-Null
  
    $finished = $false
    try {
        while (-not $process.WaitForExit(100)) {
            # Non-blocking loop done to allow ctr-c interrupts
        }
  
        $finished = $true
        return $global:LASTEXITCODE = $process.ExitCode
    }
    finally {
        # If we didn't finish then an error occurred or the user hit ctrl-c.  Either
        # way kill the process
        if (-not $finished) {
            $process.Kill()
        }
    }
}

$dotnet = Join-Path $env:DOTNET_PATH dotnet.exe
$repoRoot = Resolve-Path "$PSScriptRoot/../../"
$binLog = Join-Path $repoRoot "artifacts/log/$configuration/Build.binlog"
$dotnetTool = "msbuild"
$nugetPackagesRoot = Join-Path $repoRoot "artifacts/.packages/"
$dotnetArguments = @()

# Environment variables
$env:NUGET_PACKAGES=$nugetPackagesRoot

# MSBuild arguments
# Add the dotnet tool...
$dotnetArguments += $dotnetTool
# Add the verbosity flag
$dotnetArguments += "-v:$verbosity"
# Then project file...
$dotnetArguments += "$PSScriptRoot/dotnet-build.proj"
# Then remaining arguments.
$dotnetArguments += "/p:Configuration=$configuration"
$dotnetArguments += "/p:DotNetBuild=$productBuild"
$dotnetArguments += "/p:RepoRoot=$repoRoot"
if ($bl){
    $dotnetArguments += "/bl:${binLog}"
}
if ($ci) {
    $dotnetArguments += "/p:ContinuousIntegrationBuild=true"
}

# Then any pass-through arguments.
$dotnetArguments += $AdditionalArgs

try {
    $exitCode = Exec-Process $dotnet "$dotnetArguments"
    if ($exitCode -ne 0) {
        exit $exitCode
    }
}
catch {
    exit 1
}