
# The VMR orchestrator passes a number of stnadar
param (
  [string][Alias('c')]$configuration = "Release",
  [string][Alias('v')]$verbosity = "minimal",
  [switch]$ci,
  [switch][Alias('bl')]$binaryLog,
  [switch][Alias('pb')]$productBuild,
  [switch]$fromVMR,
  [bool]$warnAsError = $true,
  [bool]$nodeReuse = $true,
  [Parameter(ValueFromRemainingArguments = $true)][string[]]$properties
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
$nugetPackagesRoot = Join-Path $repoRoot "artifacts/.packages/"

# Environment variables
$env:NUGET_PACKAGES=$nugetPackagesRoot

# MSBuild arguments
$dotnetArguments = @()
$dotnetArguments += "$PSScriptRoot/dotnet-build.proj"
$dotnetArguments += "/p:RepoRoot=$repoRoot"
$dotnetArguments += "/p:Configuration=$configuration"
$dotnetArguments += "/p:DotNetBuild=$productBuild"
$dotnetArguments += "/p:DotNetBuildFromVMR=$fromVMR"

$bl = if ($binaryLog) { "/bl:" + (Join-Path $repoRoot "artifacts/log/$configuration/Build.binlog") } else { '' }
if ($ci) {
    $nodeReuse = $false
}

$cmdArgs = "$dotnet msbuild /m /nologo /clp:Summary /v:$verbosity /nr:$nodeReuse /p:ContinuousIntegrationBuild=$ci"

if ($warnAsError) {
    $cmdArgs += ' /warnaserror /p:TreatWarningsAsErrors=true'
}
else {
    $cmdArgs += ' /p:TreatWarningsAsErrors=false'
}

try {
    $exitCode = Exec-Process $cmdArgs $bl "$dotnetArguments" "$properties"
    if ($exitCode -ne 0) {
        exit $exitCode
    }
}
catch {
    exit 1
}