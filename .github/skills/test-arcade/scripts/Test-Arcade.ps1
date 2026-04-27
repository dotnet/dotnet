<#
.SYNOPSIS
    Builds the Arcade SDK from a local checkout, configures a local NuGet feed
    with the build artifacts, and builds a test repository against them.
    Optionally runs Signing Validation (SignCheck) on the test repo output.

.DESCRIPTION
    Prerequisites:
      - PowerShell 7+ (pwsh)
      - dotnet CLI (for building and configuring the local NuGet feed)
      - Network access to Azure DevOps package feeds

.EXAMPLE
    pwsh ./Test-Arcade.ps1 -Arcade /path/to/arcade -TestRepo /path/to/test-repo
    pwsh ./Test-Arcade.ps1 -Arcade /path/to/arcade -TestRepo /path/to/test-repo -CleanFeed
    pwsh ./Test-Arcade.ps1 -Arcade /path/to/arcade -TestRepo /path/to/test-repo -SignCheck
    pwsh ./Test-Arcade.ps1 -Arcade /path/to/arcade -TestRepo /path/to/test-repo -SignCheck -SignCheckDir path/to/files
    pwsh ./Test-Arcade.ps1 -Arcade /path/to/arcade -TestRepo /path/to/test-repo -SkipArcadeBuild -SignCheck
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, HelpMessage = "Path to the local dotnet/arcade checkout")]
    [string]$Arcade,

    [Parameter(Mandatory = $true, HelpMessage = "Path to the Arcade-consuming repo to test against")]
    [string]$TestRepo,

    [switch]$CleanFeed,

    [switch]$SignCheck,

    [ValidateScript({ Test-Path $_ -PathType Container })]
    [string]$SignCheckDir,

    [switch]$SkipArcadeBuild
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if ($SignCheckDir) {
    $SignCheck = $true
}

# ─── Resolve paths ───────────────────────────────────────────────────────────
$ScriptDir = Split-Path -Parent $PSCommandPath
$SkillDir = Split-Path -Parent $ScriptDir
$FeedPath = Join-Path ([System.IO.Path]::GetTempPath()) 'arcade-local-feed'

$ArcadePath = Resolve-Path -Path $Arcade -ErrorAction SilentlyContinue
$TestPath = Resolve-Path -Path $TestRepo -ErrorAction SilentlyContinue

if (-not $ArcadePath -or -not (Test-Path $ArcadePath -PathType Container)) {
    Write-Error "Arcade directory not found at: $Arcade"
}
if (-not $TestPath -or -not (Test-Path $TestPath -PathType Container)) {
    Write-Error "Test repo directory not found at: $TestRepo"
}

$ArcadePath = $ArcadePath.Path
$TestPath = $TestPath.Path

Write-Host "Building and testing Arcade with the following parameters:"
Write-Host "Arcade Repo Path:       $ArcadePath"
Write-Host "Test Repo Path:         $TestPath"
Write-Host "Package Feed Path:      $FeedPath"
if ($SignCheck) {
    $signMsg = "enabled"
    if ($SignCheckDir) { $signMsg += " (dir: $SignCheckDir)" }
    Write-Host "Signing Validation:     $signMsg"
}

# ─── Platform helpers ────────────────────────────────────────────────────────
function Get-BuildScript([string]$RepoPath) {
    if ($IsWindows) {
        $cmd = Join-Path $RepoPath 'Build.cmd'
        if (Test-Path $cmd) { return $cmd }
    }
    $sh = Join-Path $RepoPath 'build.sh'
    if (Test-Path $sh) { return $sh }
    Write-Error "No build script found in $RepoPath"
}

function Get-SdkTaskScript([string]$RepoPath) {
    if ($IsWindows) {
        $ps1 = Join-Path $RepoPath 'eng/common/sdk-task.ps1'
        if (Test-Path $ps1) { return $ps1 }
    }
    $sh = Join-Path $RepoPath 'eng/common/sdk-task.sh'
    if (Test-Path $sh) { return $sh }
    Write-Error "No sdk-task script found in $RepoPath"
}

function Invoke-BuildScript([string]$Script, [string[]]$Arguments) {
    if ($Script.EndsWith('.cmd')) {
        & cmd /c "$Script $($Arguments -join ' ')"
    } elseif ($Script.EndsWith('.ps1')) {
        & pwsh -NoProfile -File $Script @Arguments
    } else {
        & bash $Script @Arguments
    }
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build script failed with exit code $LASTEXITCODE"
    }
}

# ─── Reset repos ─────────────────────────────────────────────────────────────
function Reset-Repo([string]$RepoPath) {
    foreach ($dir in '.packages', 'artifacts') {
        $target = Join-Path $RepoPath $dir
        if (Test-Path $target) {
            Remove-Item -Recurse -Force $target
        }
    }
}

if ($SkipArcadeBuild) {
    Write-Host "Skipping Arcade build (-SkipArcadeBuild)..."
    Reset-Repo $TestPath
} else {
    Reset-Repo $ArcadePath
    Reset-Repo $TestPath

    # ─── Reset feed (only if -CleanFeed) ─────────────────────────────────────
    if ($CleanFeed) {
        Write-Host "Cleaning feed directory..."
        if (Test-Path $FeedPath) {
            Remove-Item -Recurse -Force $FeedPath
        }
    }
    if (-not (Test-Path $FeedPath)) {
        New-Item -ItemType Directory -Path $FeedPath -Force | Out-Null
    }

    # ─── Build Arcade ────────────────────────────────────────────────────────
    $futureDate = (Get-Date).AddYears(5).ToString('yyyyMMdd')
    $OfficialBuildId = "$futureDate.1"
    Write-Host "Building Arcade (OfficialBuildId=$OfficialBuildId)..."

    $buildScript = Get-BuildScript $ArcadePath
    $configFlag = if ($buildScript.EndsWith('.cmd')) { '-configuration' } else { '--configuration' }
    $packFlag = if ($buildScript.EndsWith('.cmd')) { '-pack' } else { '--pack' }
    Invoke-BuildScript $buildScript @($configFlag, 'Release', $packFlag, "/p:OfficialBuildId=$OfficialBuildId")

    # ─── Configure local NuGet feed ──────────────────────────────────────────
    Write-Host "Configuring local NuGet feed..."
    $ArcadePackagesPath = Join-Path $ArcadePath 'artifacts/packages/Release/NonShipping'

    $nupkgs = Get-ChildItem -Path $ArcadePackagesPath -Filter '*.nupkg' -ErrorAction SilentlyContinue
    if (-not $nupkgs -or $nupkgs.Count -eq 0) {
        Write-Error "No .nupkg packages found in $ArcadePackagesPath. The Arcade pack step may have failed."
    }

    # Validate the Arcade SDK package specifically
    $arcadeSdkPkg = $nupkgs | Where-Object { $_.Name -like 'Microsoft.DotNet.Arcade.Sdk.*.nupkg' } | Select-Object -First 1
    if (-not $arcadeSdkPkg) {
        Write-Error "Microsoft.DotNet.Arcade.Sdk package not found in $ArcadePackagesPath"
    }

    foreach ($nupkg in $nupkgs) {
        & dotnet nuget push $nupkg.FullName --source $FeedPath --skip-duplicate
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Failed to push $($nupkg.Name) to local feed"
        }
    }

    & dotnet nuget locals all --clear
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "Failed to clear NuGet caches — continuing."
    }

    # Add the local feed to the test repo's NuGet.config using --configfile
    $nugetConfig = Join-Path $TestPath 'NuGet.config'
    $feedName = 'ArcadeLocalFeed'

    if (Test-Path $nugetConfig) {
        $existingSources = & dotnet nuget list source --configfile $nugetConfig 2>&1
        if ($existingSources -match $feedName) {
            & dotnet nuget remove source $feedName --configfile $nugetConfig 2>$null
        }
    }

    & dotnet nuget add source $FeedPath --name $feedName --configfile $nugetConfig
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to add local feed source to $nugetConfig"
    }

    # ─── Update global.json ──────────────────────────────────────────────────
    Write-Host "Updating global.json..."

    # Extract version from the Arcade SDK package filename
    $arcadeVersion = $arcadeSdkPkg.Name -replace '^Microsoft\.DotNet\.Arcade\.Sdk\.', '' -replace '\.nupkg$', ''

    if (-not $arcadeVersion) {
        Write-Error "Could not extract Arcade version from $($arcadeSdkPkg.Name)"
    }

    $globalJsonPath = Join-Path $TestPath 'global.json'
    $globalJson = Get-Content $globalJsonPath -Raw | ConvertFrom-Json

    $sdks = $globalJson.'msbuild-sdks'
    if ($null -eq $sdks) {
        Write-Error "global.json does not contain an 'msbuild-sdks' section"
    }

    # Only update keys that already exist in the file
    foreach ($key in @('Microsoft.DotNet.Arcade.Sdk', 'Microsoft.DotNet.Helix.Sdk')) {
        if ($null -ne $sdks.$key) {
            $sdks.$key = $arcadeVersion
        }
    }

    $globalJson | ConvertTo-Json -Depth 10 | Set-Content $globalJsonPath -Encoding utf8NoBOM
    Write-Host "Pinned Arcade SDK version: $arcadeVersion"
}

# ─── Build Test Repo ─────────────────────────────────────────────────────────
Write-Host "Building test repo..."
$testBuildScript = Get-BuildScript $TestPath
Invoke-BuildScript $testBuildScript @()

# ─── Run Signing Validation ──────────────────────────────────────────────────
if ($SignCheck) {
    if (-not $SignCheckDir) {
        foreach ($cfg in 'Debug', 'Release') {
            $candidate = Join-Path $TestPath "artifacts/packages/$cfg/NonShipping"
            if (Test-Path $candidate -PathType Container) {
                $SignCheckDir = $candidate
                break
            }
        }
        if (-not $SignCheckDir) {
            Write-Error "No artifacts/packages/<config>/NonShipping directory found. Build the test repo with --pack or specify -SignCheckDir."
        }
    }

    if (-not (Test-Path $SignCheckDir -PathType Container)) {
        Write-Error "SignCheck directory does not exist: $SignCheckDir"
    }

    Write-Host "Running Signing Validation on: $SignCheckDir"
    $signCheckExclusions = Join-Path $TestPath 'eng/SignCheckExclusionsFile.txt'
    $extraArgs = @()
    if (Test-Path $signCheckExclusions) {
        Write-Host "Using exclusions file:    $signCheckExclusions"
        $extraArgs += "/p:SignCheckExclusionsFile=$signCheckExclusions"
    }
    $extraArgs += "/p:RestoreAdditionalProjectSources=$FeedPath"

    $sdkTaskScript = Get-SdkTaskScript $TestPath
    $taskFlag = if ($sdkTaskScript.EndsWith('.ps1')) { '-task' } else { '--task' }
    $restoreFlag = if ($sdkTaskScript.EndsWith('.ps1')) { '-restore' } else { '--restore' }
    Push-Location $TestPath
    try {
        Invoke-BuildScript $sdkTaskScript (@($taskFlag, 'SigningValidation', $restoreFlag, "/p:PackageBasePath=$SignCheckDir") + $extraArgs)
    } finally {
        Pop-Location
    }
}
