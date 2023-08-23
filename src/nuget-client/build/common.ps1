### Constants ###
$NuGetClientRoot = Split-Path -Path $PSScriptRoot -Parent
$CLIRoot = Join-Path $NuGetClientRoot cli
$Artifacts = Join-Path $NuGetClientRoot artifacts
$Nupkgs = Join-Path $Artifacts nupkgs
$ConfigureJson = Join-Path $Artifacts configure.json
$BuiltInVsWhereExe = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
$VSVersion = $env:VisualStudioVersion
$DotNetExe = Join-Path $CLIRoot 'dotnet.exe'

Set-Alias dotnet $DotNetExe

Function Read-PackageSources {
    param($NuGetConfig)
    $xml = New-Object xml
    $xml.Load($NuGetConfig)
    $xml.SelectNodes('/configuration/packageSources/add') | `
        ? { $_.key -ne "BuildFeed" } | `
        % { $_.value }
}
$PackageSources = Read-PackageSources (Join-Path $NuGetClientRoot 'NuGet.Config')

$OrigBgColor = $host.ui.rawui.BackgroundColor
$OrigFgColor = $host.ui.rawui.ForegroundColor

# MSBUILD has a nasty habit of leaving the foreground color red
Function Reset-Colors {
    $host.ui.rawui.BackgroundColor = $OrigBgColor
    $host.ui.rawui.ForegroundColor = $OrigFgColor
}

function Format-TeamCityMessage([string]$Text) {
    $Text.Replace("|", "||").Replace("'", "|'").Replace("[", "|[").Replace("]", "|]").Replace("`n", "|n").Replace("`r", "|r")
}

Function Trace-Log($TraceMessage = '') {
    Write-Host "[$(Trace-Time)]`t$TraceMessage" -ForegroundColor Cyan
}

Function Verbose-Log($VerboseMessage) {
    Write-Verbose "[$(Trace-Time)]`t$VerboseMessage"
}

Function Error-Log {
    param(
        [string]$ErrorMessage,
        [switch]$Fatal)
    if (-not $Fatal) {
        Write-Error "[$(Trace-Time)]`t$ErrorMessage"
    }
    else {
        Write-Error "[$(Trace-Time)]`t[FATAL] $ErrorMessage" -ErrorAction Stop
    }
}

Function Warning-Log($WarningMessage) {
    Write-Warning "[$(Trace-Time)]`t$WarningMessage"
}

Function Trace-Time() {
    $currentTime = Get-Date
    $lastTime = $Global:LastTraceTime
    $Global:LastTraceTime = $currentTime
    "{0:HH:mm:ss} +{1:F0}" -f $currentTime, ($currentTime - $lastTime).TotalSeconds
}

$Global:LastTraceTime = Get-Date

Function Format-ElapsedTime($ElapsedTime) {
    '{0:D2}:{1:D2}:{2:D2}' -f $ElapsedTime.Hours, $ElapsedTime.Minutes, $ElapsedTime.Seconds
}

Function Invoke-BuildStep {
    [CmdletBinding()]
    [Alias('ibs')]
    param(
        [Parameter(Mandatory = $True)]
        [string]$BuildStep,
        [Parameter(Mandatory = $True)]
        [ScriptBlock]$Expression,
        [Parameter(Mandatory = $False)]
        [Alias('args')]
        [Object[]]$Arguments,
        [Alias('skip')]
        [switch]$SkipExecution,
        [switch]$Critical
    )
    if (-not $SkipExecution) {
        if ($env:TEAMCITY_VERSION) {
            Write-Output "##teamcity[blockOpened name='$BuildStep']"
        }

        Trace-Log "[BEGIN] $BuildStep"
        $sw = [Diagnostics.Stopwatch]::StartNew()
        $completed = $false
        $PwdBefore = $PWD

        try {
            if (-not $Arguments) {
                Invoke-Command $Expression -ErrorVariable err
            }
            else {
                Invoke-Command $Expression -ArgumentList $Arguments -ErrorVariable err
            }
            $completed = $true
        }
        finally {
            $sw.Stop()
            Reset-Colors
            if ($PWD -ne $PwdBefore) {
                cd $PwdBefore
            }
            if (-not $err -and $completed) {
                Trace-Log "[DONE +$(Format-ElapsedTime $sw.Elapsed)] $BuildStep"
            }
            elseif (-not $err) {
                Trace-Log "[STOPPED +$(Format-ElapsedTime $sw.Elapsed)] $BuildStep"
            }
            else {
                Trace-Log "[FAILED +$(Format-ElapsedTime $sw.Elapsed)] $BuildStep"
            }
        }
    }
    else {
        Warning-Log "[SKIP] $BuildStep"
    }
}

Function Update-Submodules {
    [CmdletBinding()]
    param(
        [switch]$Force
    )
    $Submodules = Join-Path $NuGetClientRoot submodules -Resolve
    $GitAttributes = gci $Submodules\* -Filter '.gitattributes' -r -ea Ignore
    if ($Force -or -not $GitAttributes) {
        $opts = 'submodule', 'update'
        $opts += '--init'
        if (-not $VerbosePreference) {
            $opts += '--quiet'
        }

        Trace-Log 'Updating and initializing submodules'
        Trace-Log "git $opts"
        & git $opts 2>&1
    }
}

Function Install-DotnetCLI {
    [CmdletBinding()]
    param(
        [switch]$Force,
        [switch]$SkipDotnetInfo
    )

    $DotNetInstall = Join-Path $CLIRoot 'dotnet-install.ps1'

    #If "-force" is specified, or dotnet.exe under cli folder doesn't exist, create cli folder and download dotnet-install.ps1 into cli folder.
    if ($Force -or -not (Test-Path $DotNetExe)) {
        Trace-Log "Downloading .NET CLI install script"

        New-Item -ItemType Directory -Force -Path $CLIRoot | Out-Null

        Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile $DotNetInstall
    }

    if (-not ([string]::IsNullOrEmpty($env:DOTNET_SDK_VERSIONS))) {
        Trace-Log "Using environment variable DOTNET_SDK_VERSIONS instead of DotNetSdkVersions.txt.  Value: '$env:DOTNET_SDK_VERSIONS'"
        $CliBranchList = $env:DOTNET_SDK_VERSIONS -Split ";"
    } else {
        $CliBranchList = (Get-Content -Path "$NuGetClientRoot\build\DotNetSdkVersions.txt")
    }

    ForEach ($CliBranch in $CliBranchList) {
        $CliBranch = $CliBranch.trim()
        if ($CliBranch.StartsWith("#") -or $CliBranch.Equals("")) {
            continue
        }

        if ([Environment]::Is64BitOperatingSystem) {
            $arch = "x64";
        }
        else {
            $arch = "x86";
        }

        Trace-Log "$DotNetInstall $CliBranch -InstallDir $CLIRoot -Architecture $arch -NoPath"
 
        & powershell $DotNetInstall $CliBranch -InstallDir $CLIRoot -Architecture $arch -NoPath
        if ($LASTEXITCODE -ne 0)
        {
            throw "dotnet-install.ps1 exited with non-zero exit code"
        }
    }
    
    if (-not (Test-Path $DotNetExe)) {
        Error-Log "Unable to find dotnet.exe. The CLI install may have failed." -Fatal
    }

    if ($SkipDotnetInfo -ne $true) {
        # Display build info
        & $DotNetExe --info
        if ($LASTEXITCODE -ne 0)
        {
            throw "dotnet --info exited with non-zero exit code"
        }
    }
    
    if ($env:CI -eq "true") {
        Write-Host "##vso[task.setvariable variable=DOTNET_ROOT;isOutput=false;issecret=false;]$CLIRoot"
        Write-Host "##vso[task.setvariable variable=DOTNET_MULTILEVEL_LOOKUP;isOutput=false;issecret=false;]0"
        Write-Host "##vso[task.prependpath]$CLIRoot"
    } else {
        $env:DOTNET_ROOT=$CLIRoot
        $env:DOTNET_MULTILEVEL_LOOKUP=0
        if (-not $env:path.Contains($CLIRoot)) {
            $env:path = $CLIRoot + ";" + $env:path
        }
    }
}

Function Get-LatestVisualStudioRoot {
    if (Test-Path $BuiltInVsWhereExe) {
        $installationPath = & $BuiltInVsWhereExe -latest -prerelease -property installationPath
        $installationVersion = & $BuiltInVsWhereExe -latest -prerelease -property installationVersion
        Verbose-Log "Found Visual Studio at '$installationPath' version '$installationVersion' with '$BuiltInVsWhereExe'"
        # Set the fallback version
        $majorVersion = "$installationVersion".Split('.')[0]
        $script:FallbackVSVersion = "$majorVersion.0"

        return $installationPath
    }

    Error-Log "Could not find a compatible Visual Studio Version because $BuiltInVsWhereExe does not exist" -Fatal
}

<#
.DESCRIPTION
Finds a suitable VSVersion based on the environment configuration,
if $VSVersion is set, that means we're running in a developer command prompt so we prefer that.
otherwise we pick the latest Visual Studio version available on the machine.
#>
Function Get-VSVersion() {
    if (-not $VSVersion) {
        if (-not $script:FallbackVSVersion) {
            Verbose-Log "No fallback VS Version set yet. This means that we are running outside of a developer command prompt scope."
            $_ = Get-LatestVisualStudioRoot
        }
        Verbose-Log "Using the fallback VS version '$script:FallbackVSVersion'"
        $VSVersion = $script:FallbackVSVersion
    }
    return $VSVersion
}

Function Get-MSBuildExe {

    # If there's a msbuild.exe on the path, use it.
    if ($null -ne (Get-Command "msbuild.exe" -ErrorAction Ignore))
    {
        return "msbuild.exe"
    }

    # Otherwise, use VSWhere.exe to find the latest MSBuild.exe.
    $MSBuildExe = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -requires Microsoft.Component.MSBuild -find MSBuild\**\bin\MSBuild.exe

    if (Test-Path $MSBuildExe) {
        Verbose-Log "Found MSBuild.exe at `"$MSBuildExe`""
        return $MSBuildExe
    }
    else {
        Error-Log 'Could not find MSBuild.exe' -Fatal
    }
}

Function Test-BuildEnvironment {
    $Installed = (Test-Path $DotNetExe)
    if (-not $Installed) {
        Error-Log 'Build environment is not configured. Please run configure.ps1 first.' -Fatal
    }
}

Function Install-ProcDump {
    [CmdletBinding()]
    param()
    if ($Env:OS -eq "Windows_NT")
    {
        Trace-Log "Downloading ProcDump..."
        
        $ProcDumpZip = Join-Path $env:TEMP 'ProcDump.zip'
        $TestDir = Join-Path $NuGetClientRoot '.test'
        $ProcDumpDir = Join-Path $TestDir 'ProcDump'

        Invoke-WebRequest 'https://download.sysinternals.com/files/Procdump.zip' -OutFile $ProcDumpZip
        if (Test-Path $ProcDumpDir) {
            Remove-Item $ProcDumpDir -Recurse -Force | Out-Null
        }
        New-Item $ProcDumpDir -ItemType Directory -Force | Out-Null
        Expand-Archive $ProcDumpZip -DestinationPath $ProcDumpDir

        if ($env:CI -eq "true") {
            Write-Host "##vso[task.setvariable variable=PROCDUMP_PATH;isOutput=false;issecret=false;]$ProcDumpDir"
        } else {
            $env:PROCDUMP_PATH=$ProcDumpDir
        }
    }
}

Function Clear-PackageCache {
    [CmdletBinding()]
    param()
    Trace-Log 'Cleaning local caches'

    & dotnet nuget locals all --clear
}

Function Clear-Artifacts {
    [CmdletBinding()]
    param()
    if ( Test-Path $Artifacts) {
        Trace-Log 'Cleaning the Artifacts folder'
        Remove-Item $Artifacts\* -Recurse -Force -Exclude 'configure.json'
    }
}

Function Clear-Nupkgs {
    [CmdletBinding()]
    param()
    if (Test-Path $Nupkgs) {
        Trace-Log 'Cleaning nupkgs folder'
        Remove-Item $Nupkgs\*.nupkg -Force
    }
}
