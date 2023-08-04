<# .SYNOPSIS
    Powershell build module for xUnit.net. Defines useful helper functions for
    creating Powershell-based build scripts. Copyright (C) .NET Foundation.
    License: Apache 2.0 (https://github.com/xunit/xunit/blob/master/license.txt)
#>

Set-StrictMode -Version 2

if ($args.Count -eq 0) {
    $nugetVersion = "3.5.0"
} else {
    $nugetVersion = $args[0]
}

$nugetExe = [System.IO.Path]::Combine($home, ".nuget", "cli", $nugetVersion, "nuget.exe")

function _build_step([string] $message) {
    Write-Host -ForegroundColor White $("==> " + $message + " <==")
    Write-Host ""
}

function _dotnet([string] $command, [string] $message = "") {
    _exec ("dotnet " + $command) $message
}

function _download_nuget() {
    $cliVersionPath = split-path $nugetExe -Parent
    if ((test-path $cliVersionPath) -eq $false) {
        New-Item -Type Directory -Path $cliVersionPath | out-null
    }

    if ((test-path $nugetExe) -eq $false) {
        _build_step ("Downloading NuGet version " + $nugetVersion)
            Invoke-WebRequest ("https://dist.nuget.org/win-x86-commandline/v" + $nugetVersion + "/nuget.exe") -OutFile $nugetExe
    }
}

function _exec([string] $command, [string] $message = "") {
    if ($message -eq "") {
        $message = $command
    }
    Write-Host -ForegroundColor DarkGray ("EXEC: " + $message)
    Write-Host ""
    Invoke-Expression $command
    Write-Host ""

    if ($LASTEXITCODE -ne 0) {
        exit 1
    }
}

function _fatal([string] $message) {
    Write-Host -ForegroundColor Red ("Error: " + $message)
    exit -1
}

function _mkdir([string] $path) {
    if ((test-path $path) -eq $false) {
        New-Item -Type directory -Path $path | out-null
    }
}

function _msbuild([string] $project, [string] $configuration, [string] $target = "build", [string] $verbosity = "minimal", [string] $message = "", [string] $binlogFile = "") {
    $cmd = "msbuild " + $project + " /t:" + $target + " /p:Configuration=" + $configuration + " /v:" + $verbosity + " /m /nologo"
    if ($binlogFile -ne "") {
        $cmd = $cmd + " /bl:" + $binlogFile
    }
    _exec $cmd $message
}

function _nuget_pack {
    [cmdletbinding()]
    Param(
        [System.IO.FileInfo[]][Parameter(ValueFromPipeLine=$True)] $nuspecFiles,
        [string] $outputFolder,
        [string] $configuration
    )
    Process {
        _download_nuget

        $nuspecFiles | ForEach-Object {
            _exec ('& "' + $nugetExe + '" pack ' + $_.FullName + ' -NonInteractive -NoPackageAnalysis -OutputDirectory "' + $outputFolder + '" -Properties Configuration=' + $configuration)
        }
    }
}

function _nuget_push {
    Param(
        [System.IO.FileInfo[]][Parameter(ValueFromPipeLine=$True)] $nupkgFiles,
        [string] $source,
        [string] $apiKey
    )
    Process {
        _download_nuget

        $nupkgFiles | ForEach-Object {
            $cmd = '& "' + $nugetExe + '" push "' + $_.FullName + '" -Source ' + $source + ' -NonInteractive -ApiKey ' + $apiKey
            $message = $cmd.Replace($env:MyGetApiKey, "[redacted]")
            _exec $cmd $message
        }
    }
}

function _replace {
    [cmdletbinding()]
    Param(
        [System.IO.FileInfo[]][Parameter(ValueFromPipeLine=$True)] $files,
        [regex] $match,
        [string] $replacement
    )
    Process {
        $files | ForEach-Object {
            $content = Get-Content -raw $_.FullName
            $content = $match.Replace($content, $replacement)
            Set-Content $_.FullName $content -Encoding UTF8 -NoNewline
        }
    }
}

function _require([string] $command, [string] $message) {
    if ((get-command $command -ErrorAction SilentlyContinue) -eq $null) {
        _fatal $message
    }
}

function _verify_version([string]$version, [string]$minVersion, [string]$appName) {
    $dashIndex = $version.IndexOf('-')
    if ($dashIndex -gt -1) {
        $version = $version.Substring(0, $dashIndex)
    }

    if ([version]$version -lt [version]$minVersion) {
        _fatal ("Unsupported " + $appName + " version '$version' (must be '$minVersion' or later).")
    }
}

function _verify_dotnetsdk_version([string]$minVersion) {
    _verify_version (& dotnet --version) $minVersion ".NET SDK"
}

function _verify_msbuild_version([string]$minVersion) {
    _verify_version (& msbuild /nologo /ver) $minVersion "MSBuild"
}

Export-ModuleMember -Function * -Variable nugetExe
