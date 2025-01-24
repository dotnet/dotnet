[CmdletBinding()]
Param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("Debug", "Release")]
    [Alias("c")]
    [System.String] $Configuration = "Release",

    [Parameter(Mandatory=$false)]
    [ValidateSet("win7-x64", "win-x86")]
    [Alias("r")]
    [System.String] $TargetRuntime = "win7-x64",

    [Parameter(Mandatory=$false)]
    [ValidateSet("net9.0", "net462")]
    [Alias("f")]
    [System.String] $TargetFramework,

    [Parameter(Mandatory=$false)]
    [ValidateSet("Discovery", "Execution")]
    [Alias("a")]
    [System.String] $DefaultAction = "Both",

    [Parameter(Mandatory=$false)]
    [ValidateSet("csv")]
    [Alias("e")]
    [System.String] $ExportResults = $null
    )


#
# Variables
#
Write-Verbose "Setup environment variables."
$env:TP_ROOT_DIR = (Get-Item (Split-Path $MyInvocation.MyCommand.Path)).Parent.Parent.FullName
$env:TP_TOOLS_DIR = Join-Path $env:TP_ROOT_DIR "tools"
$env:TP_PACKAGES_DIR = Join-Path $env:TP_ROOT_DIR "packages"
$env:TP_OUT_DIR = Join-Path $env:TP_ROOT_DIR "artifacts"

#
# Test configuration
#
$TPT_TargetFrameworkFullCLR = "net462"
$TPT_TargetFramework6Core = "net6.0"
Write-Verbose "Setup build configuration."
$Script:TPT_Configuration = $Configuration
$Script:TPT_SourceFolders =  @(Join-Path $env:TP_ROOT_DIR "test\TestAssets")
$Script:TPT_TargetFrameworks =@($TPT_TargetFramework6Core, $TPT_TargetFrameworkFullCLR)
$Script:TPT_TargetFramework = $TargetFramework
$Script:TPT_TargetRuntime = $TargetRuntime
$Script:TPT_Pattern = $Pattern
$Script:TPT_Parallel = $Parallel
$Script:TPT_TestResultsDir = Join-Path $env:TP_ROOT_DIR "TestResults"
$Script:TPT_DefaultTrxFileName = "TrxLogResults.trx"
$Script:TPT_ErrorMsgColor = "Red"
$Script:TPT_PayLoads=New-Object System.Collections.ArrayList
$Script:TPT_PerfIterations = 10
$Script:TPT_Results = New-Object System.Collections.ArrayList
$Script:TPT_DependencyProps = [xml] (Get-Content $env:TP_ROOT_DIR\eng\Versions.props)

$ResultObject = New-Object PSObject
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name Runner -Value ""
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name Action -Value ""
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name Adapter -Value ""
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name AdapterVersion -Value ""
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name RunnerVersion -Value ""
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name ElapsedTime -Value ""
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name PayLoad -Value ""
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name Goal -Value ""
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name Delta -Value ""
Add-Member -InputObject $ResultObject -MemberType NoteProperty -Name Status -Value ""

# Capture error state in any step globally to modify return code
$Script:ScriptFailed = $false

#
#Import module Benchmark
#
function Invoke-InstallBenchmarkModule
{
    Write-Host "Installing NuGet Package provider"
    Install-PackageProvider -Name NuGet
    Write-Host "Saving BenchMark module into tools folder"
    Save-Module -Name Benchmark -Path $env:TP_TOOLS_DIR
    Write-Host "Setting InstallationPolicy to Trusted for PSGallery Repository"
    Set-PSRepository -Name "PSGallery" -InstallationPolicy Trusted
    Write-Host "Installing Benchmark module"
    Install-Module -Name Benchmark -Force
}

#
# Helper functions
#
function Get-PackageDirectory($framework, $targetRuntime)
{
    return $(Join-Path $env:TP_OUT_DIR "$($Script:TPT_Configuration)\$($framework)\$($targetRuntime)")
}

function Get-PerfConfigurations
{
    $configPath = Join-Path $env:TP_ROOT_DIR "scripts\perf\perfconfig.csv"
    if(Test-Path $configPath)
    {
        $fileContent = Import-Csv -Path $configPath -Delimiter ";"
        foreach($row in $fileContent)
        {
            $row.runners = $row.runners.Trim("(").Trim(")").Split(",")
            $row.discoverygoal = $row.discoverygoal.Trim("(").Trim(")").Split(",")
            $row.executiongoal = $row.executiongoal.Trim("(").Trim(")").Split(",")
            $Script:TPT_PayLoads.Add($row) > $null
        }
    }
    else
    {
        Write-Error "Unable to find the configuartion file: $configPath"
    }
}

function Get-TestAdapterPath($testadapter)
{
    if($testadapter -eq "MsTest")
    {
        return "$env:TP_PACKAGES_DIR\mstest.testadapter\$($Script:TPT_DependencyProps.Project.PropertyGroup.MSTestTestAdapterVersion)\build\_common"
    }
    if($testadapter -eq "xUnit")
    {
        return "$env:TP_PACKAGES_DIR\xunit.runner.visualstudio\$($Script:TPT_DependencyProps.Project.PropertyGroup.XUnitAdapterVersion)\build\_common"
    }
    if($testadapter -eq "nUnit")
    {
        return "$env:TP_PACKAGES_DIR\nunit3testadapter\$($Script:TPT_DependencyProps.Project.PropertyGroup.NUnit3AdapterVersion)\build\net35"
    }
}

function Get-AdapterVersion($testadapter)
{
    if($testadapter -eq "MsTest")
    {
        return "$($Script:TPT_DependencyProps.Project.PropertyGroup.MSTestTestAdapterVersion)"
    }
    if($testadapter -eq "xUnit")
    {
        return "$($Script:TPT_DependencyProps.Project.PropertyGroup.XUnitAdapterVersion)"
    }
    if($testadapter -eq "nUnit")
    {
        return "$($Script:TPT_DependencyProps.Project.PropertyGroup.NUnit3AdapterVersion)"
    }
}

function Get-ConsoleRunnerPath($runner, $targetFrameWork)
{
    if($runner -eq "vstest.console")
    {
        if($targetFrameWork -eq $TPT_TargetFramework6Core)
        {
            $vstestConsoleFileName = "vstest.console.dll"
            $targetRunTime = ""
            $vstestConsolePath = Join-Path (Get-PackageDirectory $TPT_TargetFramework6Core $targetRuntime) $vstestConsoleFileName
        } else {
            $vstestConsoleFileName = "vstest.console.exe"
            $targetRunTime = $Script:TPT_TargetRuntime
            $vstestConsolePath = Join-Path (Get-PackageDirectory $TPT_TargetFrameworkFullCLR $targetRuntime) $vstestConsoleFileName
        }
        return $vstestConsolePath;
    }
    if($runner -eq "xunit.runner.console")
    {
        return "$env:TP_PACKAGES_DIR\xunit.runner.console\$($Script:TPT_DependencyProps.Project.PropertyGroup.XUnitConsoleRunnerVersion)\tools\net452\xunit.console.exe"
    }
    if($runner -eq "nunit.consolerunner")
    {
        return "$env:TP_PACKAGES_DIR\nunit.consolerunner\$($Script:TPT_DependencyProps.Project.PropertyGroup.NUnitConsoleRunnerVersion)\tools\nunit3-console.exe"
    }
}

function Write-Log ([string] $message, $messageColor = "Green")
{
    if ($message)
    {
        Write-Host "... $message" -ForegroundColor $messageColor
    }
}

function Get-ProductVersion($filePath)
{
    return [System.Diagnostics.FileVersionInfo]::GetVersionInfo($filePath).ProductVersion
}

function Set-CommonProperties($result, $payload)
{
    $result.PayLoad = [System.IO.Path]::GetFileName($($payload.containerPath))
    $result.Runner = $($payload.currentRunner)
    $result.RunnerVersion = "$($payload.currentRunnerVersion)"
    $result.Adapter = if($($payload.currentAdapter) -ne $null -and $($payload.currentAdapter) -ne "") {$($payload.currentAdapter)} else {""}
    $result.AdapterVersion = if($($payload.currentAdapter) -ne $null -and $($payload.currentAdapter) -ne "") {(Get-AdapterVersion($result.Adapter))} else {""}
    $result.Delta = [math]::Round(($result.Goal - $result.ElapsedTime), 2)
    $result.Status = If($result.Delta -lt 0) {"FAIL"} Else {"PASS"}
    $result.Delta = [math]::Round((($result.Delta*100)/[int]$result.ElapsedTime),2)
    $result.Delta = [string]$result.Delta+"%"
}

function Measure-DiscoveryTime($commandtorun, $payload)
{
    if(($DefaultAction -eq "Both") -or ($DefaultAction -eq "Discovery"))
    {
        Write-Log "Discovering Tests in $($payload.containerPath) using $($payload.currentRunner)"
        $result = Get-TimeTaken $commandtorun
        $result.Action = "Discovery"
        $result.Goal = $payload.discoverygoal[$payload.runners.IndexOf("`"$($payload.currentRunner)`"")]
        Set-CommonProperties $result $payload
        $Script:TPT_Results.Add($result) > $null
    }
}

function Measure-ExecutionTime($commandtorun, $payload)
{
    if(($DefaultAction -eq "Both") -or ($DefaultAction -eq "Execution"))
    {
        Write-Log "Executing Tests in $($payload.containerPath) using $($payload.currentRunner)"
        $result = Get-TimeTaken $commandtorun
        $result.Action = "Execution"
        $result.Goal = $payload.executiongoal[$payload.runners.IndexOf("`"$($payload.currentRunner)`"")]
        Set-CommonProperties $result $payload
        $Script:TPT_Results.Add($result) > $null
    }
}

function Get-TimeTaken($commandtorun)
{
    $op = Measure-These -Count $Script:TPT_PerfIterations -ScriptBlock $commandtorun
    $timetaken = [double]$op.'Average (ms)'

    $obj = $ResultObject | Select-Object *
    $obj.ElapsedTime = [math]::Round($timetaken, 2)
    return $obj
}

function Get-SystemInfo
{
    $osInfo = @{"MachineName"="";"MachineType"="";"LogicalCores"="";"Processor"="";"OSName"="";"OSVersion"="";"RAMSize"="";}
    $sysinfo = Get-WmiObject -Class Win32_Processor -ComputerName . | Select-Object -Property SystemName, Name, NumberOfLogicalProcessors
    $osInfo.MachineName = $sysinfo.SystemName
    $osInfo.Processor = $sysinfo.Name
    $osInfo.LogicalCores = $sysinfo.NumberOfLogicalProcessors
    $sysinfo = Get-WmiObject -Class Win32_ComputerSystem -ComputerName . | Select-Object -Property SystemType
    $osInfo.MachineType = $sysinfo.SystemType
    $sysinfo = Get-WmiObject -Class Win32_OperatingSystem -ComputerName . | Select-Object -Property Caption, Version, OSArchitecture
    $osInfo.OSName=$sysinfo.Caption
    $osInfo.OSVersion=$sysinfo.Version
    $sysinfo = Get-CimInstance CIM_ComputerSystem | Select-Object -Property TotalPhysicalMemory, Domain
    $osInfo.RAMSize=$sysinfo.TotalPhysicalMemory/1GB
    $osInfo.RAMSize = "$($osInfo.RAMSize)"+"GB"
    $osInfo.MachineName += "."+$sysinfo.Domain
    return New-Object -TypeName psobject -Property $osInfo
}

function Invoke-PerformanceTests
{
    foreach ($src in $Script:TPT_SourceFolders)
    {
        $assets = Get-ChildItem -Recurse -Path $src -Include *.csproj
            foreach($payload in $Script:TPT_PayLoads)
            {
                foreach($asset in $assets)
                {
                    if($payload.payload -eq $asset.Directory.Name)
                    {
                        $testContainerName = $payload.payload
                        $testOutputPath = Join-Path $asset.Directory.FullName "bin/$($Script:TPT_Configuration)/{0}"
                        $testContainerPath = Join-Path $testOutputPath "$($testContainerName).dll"
                         foreach($fx in $Script:TPT_TargetFrameworks)
                        {
                            if ($Script:TPT_TargetFramework -ne "" -and $fx -ne $Script:TPT_TargetFramework)
                            {
                                # Write-Log "Skipped framework $fx based on user setting."
                                continue;
                            }

                            $testContainer = [System.String]::Format($testContainerPath, $fx)

                            if (Test-Path $testContainer)
                            {
                                $adapter = $payload.adapter
                                $testAdapterPath = Get-TestAdapterPath($adapter)
                                if(Test-Path $testAdapterPath)
                                {
                                    foreach($runner in $payload.runners)
                                    {
                                        if($runner -eq $null)
                                        {
                                            return
                                        }
                                        $runner = $runner -replace '"', ""
                                        $runnerPath = Get-ConsoleRunnerPath($runner)
                                        if(($runnerPath -ne $null) -and (Test-Path $runnerPath))
                                        {
                                            $payload | Add-Member containerPath $testContainer -Force
                                            $payload | Add-Member adapterPath $testAdapterPath -Force
                                            $payload | Add-Member currentRunner $runner -Force
                                            $payload | Add-Member currentAdapter $null -Force
                                            $payload | Add-Member currentAdapterVersion $null -Force

                                            if($runner -eq "vstest.console")
                                            {
                                                $payload | Add-Member currentRunnerVersion (Get-ProductVersion($runnerPath)) -Force
                                                $payload.currentAdapter = $adapter

                                                Measure-DiscoveryTime {&$runnerPath $testContainer --listtests --testadapterpath:$testAdapterPath} $payload
                                                Measure-ExecutionTime {&$runnerPath $testContainer --testadapterpath:$testAdapterPath} $payload
                                            }
                                            elseif($runner -eq "nunit.consolerunner")
                                            {
                                                $payload | Add-Member currentRunnerVersion "$($Script:TPT_DependencyProps.Project.PropertyGroup.NUnitConsoleRunnerVersion)" -Force
                                                Measure-DiscoveryTime {&$runnerPath $testContainer --explore --inprocess} $payload
                                                Measure-ExecutionTime {&$runnerPath $testContainer --inprocess} $payload
                                            }
                                            elseif($runner -eq "xunit.runner.console")
                                            {
                                                $payload | Add-Member currentRunnerVersion "$($Script:TPT_DependencyProps.Project.PropertyGroup.XUnitConsoleRunnerVersion)" -Force
                                                Measure-DiscoveryTime {&$runnerPath $testContainer -class foo} $payload
                                                Measure-ExecutionTime {&$runnerPath $testContainer} $payload
                                            }
                                        }
                                        else {
                                            Write-Log "Specified runner $runner doesn't exist at $runnerPath"
                                        }
                                    }

                                }
                                else
                                {
                                    Write-Log "Unable to find $testAdapterPath, Did you restore ?"
                                }
                            }
                            else
                            {
                                Write-Log "Unable to find $testContainer, Did you build the Test Assets?"
                            }
                        }
                        break
                    }
                }
            }
    }
}

#
# Displaying the results in table format
#
function Invoke-DisplayResults
{
    try {
        $currentColor = $Host.UI.RawUI.ForegroundColor
        $Host.UI.RawUI.ForegroundColor = "Green"
        $osDetails = Get-SystemInfo
        "`n"
        "Machine Configuration"
        $osDetails | Format-List 'MachineName', 'OSName', 'OSVersion', 'MachineType' , 'Processor', 'LogicalCores', 'RAMSize'

        if($DefaultAction -eq "Both" -or $DefaultAction -eq "Discovery")
        {
            $Script:TPT_Results | Where-Object {$_.Action -like "Discovery"} | Format-Table 'Runner', 'Adapter', 'Action', 'ElapsedTime', 'Goal', 'Delta', 'Status', 'PayLoad', 'RunnerVersion', 'AdapterVersion' -AutoSize
        }

        if($DefaultAction -eq "Both" -or $DefaultAction -eq "Execution")
        {
            $Script:TPT_Results | Where-Object {$_.Action -like "Execution"} | Format-Table 'Runner', 'Adapter', 'Action', 'ElapsedTime', 'Goal', 'Delta', 'Status', 'PayLoad', 'RunnerVersion', 'AdapterVersion' -AutoSize
        }
    }
    finally {
        $Host.UI.RawUI.ForegroundColor = $currentColor
    }

    if($ExportResults -ne $null -and $ExportResults -eq "csv")
    {
        $Script:TPT_Results | Export-Csv -Path "PerformanceResults.csv" -Force -NoTypeInformation
        "Exported results to PerformanceResults.csv"
    }
}

Get-PerfConfigurations

if (-not (Get-Module -Name "Benchmark")) {
    Invoke-InstallBenchmarkModule
}
else
{
    Write-Log "Benchmark module is already installed"
}

Invoke-PerformanceTests
Invoke-DisplayResults
