[CmdletBinding(PositionalBinding=$false)]
param(
    [string[]]$Architectures = @('x64', 'x86', 'arm64'),
    [string]$Configuration,
    [string[]]$MsbuildArgs = @(),
    [switch]$VerboseLogging,
    [switch]$CI
)

$ErrorActionPreference = 'Stop'
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..' '..' '..')
$msbuildScript = Join-Path $repoRoot 'eng' 'common' 'msbuild.ps1'
$bundleProject = Join-Path $repoRoot 'src' 'windowsdesktop' 'src' 'bundle' 'bundle.wixproj'

if (-not (Test-Path $msbuildScript)) {
    throw "Unable to locate msbuild.ps1 at '$msbuildScript'"
}

if (-not (Test-Path $bundleProject)) {
    throw "Unable to locate bundle project at '$bundleProject'"
}

$architectures = $Architectures | Where-Object { $_ -and $_.Trim() } | ForEach-Object { $_.Trim().ToLowerInvariant() } | Select-Object -Unique
if (-not $architectures) {
    throw 'At least one architecture must be supplied.'
}

foreach ($arch in $architectures) {
    Write-Host "[restore-runtime-prereqs] Staging runtime MSIs for architecture '$arch'"

    $msbuildArguments = @('-restore', '/t:RestoreRuntimePrereqs', "/p:TargetArchitecture=$arch")
    if ($Configuration) {
        $msbuildArguments += "/p:Configuration=$Configuration"
    }
    if ($VerboseLogging) {
        $msbuildArguments += '/v:normal'
    }
    if ($MsbuildArgs) {
        $msbuildArguments += $MsbuildArgs
    }
    $msbuildArguments += $bundleProject

    & $msbuildScript -warnAsError:$false -nodeReuse:$false -ci:$CI @msbuildArguments
    if ($LASTEXITCODE -ne 0) {
        throw "RestoreRuntimePrereqs failed for architecture '$arch'"
    }

    $prereqDir = Join-Path $repoRoot "artifacts/prereqs/$arch"
    if (Test-Path $prereqDir) {
        Write-Host "[restore-runtime-prereqs] Contents of ${prereqDir}:" -ForegroundColor Cyan
        $msis = Get-ChildItem -Path $prereqDir -Filter "dotnet-*-win-$arch*.msi" -ErrorAction SilentlyContinue | Sort-Object Name
        if ($msis) {
            $msis | ForEach-Object { Write-Host "  $_" }
        }
        else {
            Write-Host "  (no runtime MSI files were found in the staging directory)"
        }

        $resolved = Join-Path $prereqDir 'resolved-runtime-version.txt'
        if (Test-Path $resolved) {
            Write-Host "  $(Get-Content $resolved -ErrorAction SilentlyContinue)"
        }
    }
}
