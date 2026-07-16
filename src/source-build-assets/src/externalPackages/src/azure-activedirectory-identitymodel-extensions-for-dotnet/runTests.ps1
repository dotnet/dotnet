param(
    [string]$buildType="Debug",
    [string]$dotnetDir="c:\Program Files\dotnet",
    [string]$root=$PSScriptRoot,
    [string]$failBuildOnTest="YES",
    [bool]$runningInCI=$false)

################################################# Functions ############################################################

function WriteSectionHeader($sectionName)
{
    $startTime = Get-Date -DisplayHint Time
    Write-Host ""
    Write-Host "============================"
    Write-Host $sectionName
    Write-Host "Start Time:     "  $startTime
    Write-Host ""
}

function WriteSectionFooter($sectionName)
{
    $startTime = Get-Date -DisplayHint Time
    Write-Host ""
    Write-Host "End Time:     "  $startTime
    Write-Host $sectionName
    Write-Host "============================"
    Write-Host ""
}


################################################# Functions ############################################################

WriteSectionHeader("runTests.ps1");

if (Test-Path $env:TargetNetNext)
{
    Write-Host "using TargetNetNext from env:"  $env:TargetNetNext
}
else 
{
    Write-Host "using TargetNetNext as false"
    $env:TargetNetNext = "False"
}


Write-Host "buildType:       " $buildType;
Write-Host "dotnetDir:       " $dotnetDir
Write-Host "root:            " $root;
Write-Host "failBuildOnTest: " $failBuildOnTest;
Write-Host "slnFile:         " $slnFile;
Write-Host "runningInCI:     " $runningInCI;

$runSettingsPath = $PSScriptRoot + "\build\CodeCoverage.runsettings"
[xml]$buildConfiguration = Get-Content $PSScriptRoot\buildConfiguration.xml
$dotnetexe = "$dotnetDir\dotnet.exe";
$startTime = Get-Date

Write-Host "Start Time:     " $startTime
Write-Host "PSScriptRoot:   " $PSScriptRoot;
Write-Host "dotnetexe:      " $dotnetexe;

$ErrorActionPreference = "Stop"

$tempToUse = $env:TEMP;

if ($runningInCI) {
    # Temp dir used in ADO
    $tempToUse = "C:\__w\_temp";
}

$testProjects = $buildConfiguration.SelectNodes("root/projects/test/project")
foreach ($testProject in $testProjects)
{
    if ($testProject.test -eq "YES")
    {
        WriteSectionHeader("Test");

        $name = $testProject.name;
        Write-Host ">>> Set-Location $root\test\$name"
        Push-Location
        Set-Location $root\test\$name
        Write-Host ">>> Start-Process -Wait -PassThru -NoNewWindow $dotnetexe 'test $name.csproj' --filter category!=nonwindowstests --no-build --no-restore -nodereuse:false -v n -c $buildType --collect ""Code Coverage"" --settings ""$runSettingsPath"" --logger trx --results-directory ""$tempToUse"""
        $p = Start-Process -Wait -PassThru -NoNewWindow $dotnetexe "test $name.csproj --filter category!=nonwindowstests --no-build --no-restore -nodereuse:false -v n -c $buildType --collect ""Code Coverage"" --settings ""$runSettingsPath"" --logger trx --results-directory ""$tempToUse"""

        if($p.ExitCode -ne 0)
        {
            if (!$testExitCode)
            {
                $failedTestProjects = "$name"
            }
            else
            {
                $failedTestProjects = "$failedTestProjects, $name"
            }
        }
        $testExitCode = $p.ExitCode + $testExitCode

        Pop-Location

        WriteSectionFooter("End Test");
    }
}

if($testExitCode -ne 0)
{
    WriteSectionHeader("==== Test Failures ====");
    Write-Host "Failed test projects: $failedTestProjects" -foregroundcolor "DarkRed"
    WriteSectionFooter("==== End Test Failures ====");
    if($failBuildOnTest -ne "NO")
    {
        throw "Exiting test run."
    }
}

Write-Host "============================"
Write-Host ""
$time = Get-Date
Write-Host "Start Time:    " ($startTime);
Write-Host "End Time:      " ($time);
Write-Host "Time to runtests: " ($time - $startTime);
Write-Host ""
Write-Host "============================";
