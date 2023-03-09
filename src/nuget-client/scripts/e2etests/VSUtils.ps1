$VSInstallerProcessName = "VSIXInstaller"

. "$PSScriptRoot\Utils.ps1"

function Get-VisualStudioVersionRangeFromConfig
{
    $VsVersion = ((& dotnet msbuild "$PSScriptRoot\..\..\build\config.props" /restore:false "/ConsoleLoggerParameters:Verbosity=Minimal;NoSummary;ForceNoAlign" /nologo /target:GetVSTargetMajorVersion) | Out-String).Trim()
    Write-Host "config.props targets VS version $vsVersion"
    $VsVersionRange = "["+$VsVersion+".0,"+(1+$VsVersion)+".0)"
    return $VsVersionRange
}

function Get-LatestVSInstance
{
    param(
        [string]$VersionRange
    )

    $vswhere = "${Env:\ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"

    if (-not $VersionRange) {
        $VSInstanceData = & $vswhere -latest -prerelease -nologo -format json | ConvertFrom-Json
    }
    else {
        $VSInstanceData = & $vswhere -latest -prerelease -version "$VersionRange" -nologo -format json | ConvertFrom-Json
    }

    return $VSInstanceData
}

function Get-SpecificVSInstance
{
    param(
        [Parameter(Mandatory = $true)]
        [string]$instanceId
    )

    $vswhere = "${Env:\ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"

    $allInstances = & $vswhere -prerelease -nologo -format json | ConvertFrom-Json

    $specificInstance = $allInstances | Where-Object { $_.instanceId -eq $instanceId }

    if ($null -eq $specificInstance)
    {
        throw "Could not find VS instance $instanceId"
    }

    return $specificInstance
}

function GetVSFolderPath {
    param(
        [Parameter(Mandatory = $true)]
        [Object]$VsInstance
    )

    $ProgramFilesPath = ${env:ProgramFiles}

    $VS17PreviewRelativePath = "Microsoft Visual Studio\2022\Preview"

    if (Test-Path (Join-Path $ProgramFilesPath $VS17PreviewRelativePath)) {
        $VSFolderPath = Join-Path $ProgramFilesPath $VS17PreviewRelativePath
    }

    return $VSFolderPath
}

function LaunchVSAndWaitForDTE {
    param (
        [string]$ActivityLogFullPath,
        [Parameter(Mandatory = $true)]
        [Object]$VSInstance,
        [Parameter(Mandatory = $true)]
        $DTEReadyPollFrequencyInSecs,
        [Parameter(Mandatory = $true)]
        $NumberOfPolls
    )

    KillRunningInstancesOfVS $VSInstance

    if ($ActivityLogFullPath) {
        $process = LaunchVS -VSInstance $VSInstance -ActivityLogFullPath $ActivityLogFullPath
    }
    else {
        $process = LaunchVS -VSInstance $VSInstance
    }

    if (-not $process)
    {
        Write-Error "Unable to start VS process"
        return $null
    }

    $VSVersionString = $VsInstance.installationVersion
    $VSVersion = $VSVersionString.Substring(0, $VSVersionString.IndexOf("."))

    $dte2 = $null
    $count = 0
    Write-Host "Will wait for $NumberOfPolls times and $DTEReadyPollFrequencyInSecs seconds each time."

    # https://docs.microsoft.com/en-us/visualstudio/extensibility/launch-visual-studio-dte?view=vs-2019
    $exeVersion = $VSInstance.installationVersion
    $dteName = "VisualStudio.DTE." + $exeVersion.Substring(0, $exeVersion.IndexOf('.')) + ".0"
    Write-Host "Looking for: $dteName"

    while ($count -lt $NumberOfPolls) {
        # Wait for $VSLaunchWaitTimeInSecs secs for VS to load before getting the DTE COM object
        Write-Host "Waiting for $DTEReadyPollFrequencyInSecs seconds for DTE to become available"
        start-sleep $DTEReadyPollFrequencyInSecs

        $dte2 = GetDTE2 -dteName $dteName
        if ($dte2) {
            Write-Host 'Obtained DTE.'
            return $dte2
        }

        $count++
    }

    return $null
}

function KillRunningInstancesOfVS {
    param(
        [Parameter(Mandatory = $true)]
        [Object]$VSInstance
    )

    Get-Process | ForEach-Object {
        if (-not [string]::IsNullOrEmpty($_.Path)) {
            $processPath = $_.Path | Out-String
            if ($processPath.StartsWith($VSInstance.installationPath, [System.StringComparison]::OrdinalIgnoreCase)) {
                Write-Host $processPath
                Stop-Process $_ -ErrorAction SilentlyContinue -Force
                if ($_.HasExited) {
                    Write-Host "Killed process:" $_.Name
                }
            }
        }
    }
}

function LaunchVS {
    param(
        [Parameter(Mandatory = $true)]
        [Object]$VSInstance,
        [string]$ActivityLogFullPath
    )

    $env:__VSDisableStartWindow=1
    $env:__VSDisableNewProjectCreationExperience=1

    $VSPath = $VSInstance.productPath
    Write-Host 'Starting ' $VSPath
    if ($ActivityLogFullPath) {
        $process = start-process $VSPath -ArgumentList "/log $ActivityLogFullPath" -PassThru
    }
    else {
        $process = start-process $VSPath -PassThru
    }
    return $process
}

function GetDTE2 {
    param(
        [Parameter(Mandatory = $true)]
        [string]$dteName
    )

    Try {
        $dte2 = [System.Runtime.InteropServices.Marshal]::GetActiveObject($dteName)
        return $dte2
    }
    Catch {
        return $null
    }
}

function ExecuteCommand {
    param(
        [Parameter(Mandatory = $true)]
        $dte2,
        [Parameter(Mandatory = $true)]
        [string]$command,
        [AllowEmptyString()]
        [Parameter(Mandatory = $true)]
        [string]$args,
        [Parameter(Mandatory = $true)]
        [string]$message,
        $waitTime = 0
    )

    Write-Host $message
    $success = $false
    $numberOfTries = 0
    do {
        try {
            $numberOfTries++
            Write-Host "Attempt # $numberOfTries "
            if ($args) {
                Write-Host 'Executing command ' $command ' with arguments: ' $args
                $dte2.ExecuteCommand($command, $args)
                # In examples like loading of a tool window, the window is not fully loaded when ExecuteCommand returns
                # So, the caller can choose to wait if needed. Not passing will result in no wait
                start-sleep $waitTime
                $success = $true
            }
            else {
                Write-Host 'Executing command ' $command ' without arguments'
                $dte2.ExecuteCommand($command)
                start-sleep $waitTime
                $success = $true
            }
        }
        catch {
            Write-Host "$command threw an exception: $PSItem"
            Write-Host "Will wait for $waitTime seconds and retry"
            $success = $false
            start-sleep $waitTime
        }
    }
    until (($success -eq $true) -or ($numberOfTries -gt 2))
}

function GetVSIXInstallerPath {
    param(
        [Parameter(Mandatory = $true)]
        [Object]$VSInstance
    )

    $VSIXInstallerPath = Get-ChildItem -Recurse $VSInstance.installationPath -filter "$VSInstallerProcessName.exe"

    return $VSIXInstallerPath.FullName
}

function GetMEFCachePath {
    param(
        [Parameter(Mandatory = $true)]
        [Object]$VSInstance
    )

    $cachePath = $env:localappdata
    $exeVersion = $VSInstance.installationVersion
    $vsVersion = $exeVersion.Substring(0, $exeVersion.IndexOf('.'))
    @( "Microsoft", "VisualStudio", "$vsVersion.0_$($VSInstance.instanceId)", "ComponentModelCache" ) | % { $cachePath = Join-Path $cachePath $_ }

    return $cachePath
}

function Update-Configuration(
        [Parameter(Mandatory = $true)]
        [Object] $vsInstance) {

    Write-Host "Updating configuration for $($vsInstance.productPath)"

    Start-Process -FilePath $vsInstance.productPath -ArgumentList '/updateConfiguration' -Wait
}

function UpdateVSInstaller {
    param(
        [string]$VSVersion,
        [Parameter(Mandatory = $true)]
        [int]$ProcessExitTimeoutInSeconds
    )

    # The public Preview channel is intentional since the --update command will update the installer to the latest public preview version.
    # It matches the channel of VS installed on CI
    $vsBootstrapperUrl = "https://aka.ms/vs/$VSVersion/pre/vs_enterprise.exe"

    $tempdir = [System.IO.Path]::GetTempPath()
    $VSBootstrapperPath =  "$tempdir" + "vs_enterprise.exe"
    if (Test-Path $VSBootstrapperPath)
    {
        Remove-Item $VSBootstrapperPath
    }

    Write-Host "Downloading [$VSBootstrapperUrl]`nSaving at [$VSBootstrapperPath]"
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Invoke-WebRequest -Uri $VSBootstrapperUrl -OutFile $VSBootstrapperPath

    Write-Host "Updating the locally installed VS Installer"
    $args = "--update --quiet --wait"
    Write-Host """$VSBootstrapperPath"" $args"
    $p = Start-Process -FilePath "$VSBootstrapperPath" -Wait -PassThru -ArgumentList $args

    if ($p.ExitCode -ne 0) {
        Write-Error "Error updating VS installer. Exit code $($p.ExitCode)"
        return $false
    }
    else {
        return $true
    }
}

function ResumeVSInstall {
    param(
        [Parameter(Mandatory = $true)]
        [Object]$VSInstance,
        [Parameter(Mandatory = $true)]
        [int]$ProcessExitTimeoutInSeconds
    )

    $ProgramFilesPath = ${env:ProgramFiles}
    if (Test-Path ${env:ProgramFiles(x86)}) {
        $ProgramFilesPath = ${env:ProgramFiles(x86)}
    }
    $VSInstallerPath = "$ProgramFilesPath\Microsoft Visual Studio\Installer\vs_installer.exe"
    $VSFolderPath = $VSInstance.installationPath

    Write-Host 'Resuming any incomplete install'
    $args = "resume --installPath ""$VSFolderPath"" -q"
    Write-Host """$VSInstallerPath"" $args"
    $p = Start-Process "$VSInstallerPath" -Wait -PassThru -NoNewWindow -ArgumentList $args

    if ($p.ExitCode -ne 0) {
        if ($p.ExitCode -eq 1)
        {
            $vsExeVersion = $VSInstance.installationVersion
            $VSVersion = $vsExeVersion.Substring(0, $vsExeVersion.IndexOf('.'))

            Write-Host "VS installer appears to need updating. Updating VS installer."
            $resumeResult = UpdateVSInstaller $VSVersion $ProcessExitTimeoutInSeconds
            if ( $resumeResult -eq $true) {
                Write-Host """$VSIXInstallerPath"" $args"
                $p = start-process "$VSIXInstallerPath" -Wait -PassThru -NoNewWindow -ArgumentList $args
            }
        }

        if ($p.ExitCode -ne 0)
        {
            Write-Error "Error resuming VS installer. Exit code $($p.ExitCode)"
            return $false
        }
    }
    else {
        return $true
    }
}

function DowngradeVSIX {
    param(
        [Parameter(Mandatory = $true)]
        [Object]$VSInstance,
        [Parameter(Mandatory = $true)]
        [int]$ProcessExitTimeoutInSeconds
    )

    $VSIXInstallerPath = GetVSIXInstallerPath $VSInstance

    Write-Host 'Downgrading VSIX...'
    $args = "/q /a /d:NuGet.72c5d240-f742-48d4-a0f1-7016671e405b /instanceIds:$($VSInstance.instanceId)"
    Write-Host """$VSIXInstallerPath"" $args"
    $p = start-process "$VSIXInstallerPath" -Wait -PassThru -NoNewWindow -ArgumentList $args

    if ($p.ExitCode -ne 0) {
        if ($p.ExitCode -eq -2146233079)
        {
            Write-Host "Previous VSIX install appears not to have completed. Resuming VS install."
            $exeVersion = $VSInstance.installationVersion
            $VSVersion = $exeVersion.Substring(0, $exeVersion.IndexOf("."))
            $resumeResult = ResumeVSInstall $VSVersion $ProcessExitTimeoutInSeconds
            if ( $resumeResult -eq $true) {
                Write-Host """$VSIXInstallerPath"" $args"
                $p = start-process "$VSIXInstallerPath" -Wait -PassThru -NoNewWindow -ArgumentList $args
            }
        }

        if ($p.ExitCode -eq 2001) {
            Write-Host "This VS2017 version does not support downgrade. Moving on to installing the VSIX! Exit code: $($p.ExitCode)"
            return $true
        }
        elseif ($p.ExitCode -ne 0) {
            Write-Error "Error downgrading the VSIX! Exit code: $($p.ExitCode)"
            return $false
        }
    }

    WaitForProcessExit -ProcessName $VSInstallerProcessName -TimeoutInSeconds $ProcessExitTimeoutInSeconds
    Write-Host "VSIX has been downgraded successfully."

    return $true
}


function InstallVSIX {
    param(
        [Parameter(Mandatory = $true)]
        [string]$vsixpath,
        [Parameter(Mandatory = $true)]
        [Object]$VSInstance,
        [Parameter(Mandatory = $true)]
        [int]$ProcessExitTimeoutInSeconds
    )

    $VSIXInstallerPath = GetVSIXInstallerPath $VSInstance

    Write-Host "Installing VSIX from $vsixpath..."
    $args = "/q /a $vsixpath"
    Write-Host """$VSIXInstallerPath"" $args"
    $p = start-process "$VSIXInstallerPath" -Wait -PassThru -NoNewWindow -ArgumentList $args

    if ($p.ExitCode -ne 0) {
        Write-Error "Error installing the VSIX! Exit code:  $($p.ExitCode)"
        return $false
    }

    WaitForProcessExit -ProcessName $VSInstallerProcessName -TimeoutInSeconds $ProcessExitTimeoutInSeconds
    Write-Host "VSIX has been installed successfully."

    return $true
}


function ClearMEFCache {
    param(
        [Parameter(Mandatory = $true)]
        [Object]$VSInstance
    )

    $mefCachePath = GetMEFCachePath $VSInstance

    Write-Host "rm -r $mefCachePath..."
    rm -r $mefCachePath
    Write-Host "Done clearing MEF cache..."
}
