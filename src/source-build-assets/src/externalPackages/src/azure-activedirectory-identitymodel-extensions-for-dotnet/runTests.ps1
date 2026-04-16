param([string]$buildType="Debug", [string]$dotnetDir="c:\Program Files\dotnet", [string]$root=$PSScriptRoot, [string]$failBuildOnTest="YES")

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
Write-Host "buildType:       " $buildType;
Write-Host "dotnetDir:       " $dotnetDir
Write-Host "root:            " $root;
Write-Host "failBuildOnTest: " $failBuildOnTest;
Write-Host "slnFile:         " $slnFile;

[xml]$buildConfiguration = Get-Content $PSScriptRoot\buildConfiguration.xml
$dotnetexe = "$dotnetDir\dotnet.exe";
$startTime = Get-Date
Write-Host "Start Time:     " $startTime
Write-Host "PSScriptRoot:   " $PSScriptRoot;
Write-Host "dotnetexe:      " $dotnetexe;

$ErrorActionPreference = "Stop"

$testProjects = $buildConfiguration.SelectNodes("root/projects/test/project")
foreach ($testProject in $testProjects)
{
    if ($testProject.test -eq "YES")
    {
        WriteSectionHeader("Test");

        $name = $testProject.name;
        Write-Host ">>> Set-Location $root\test\$name"
        pushd
        Set-Location $root\test\$name
        Write-Host ">>> Start-Process -Wait -PassThru -NoNewWindow $dotnetexe 'test $name.csproj' --filter category!=nonwindowstests --no-build --no-restore -nodereuse:false -v n -c $buildType"
        $p = Start-Process -Wait -PassThru -NoNewWindow $dotnetexe "test $name.csproj --filter category!=nonwindowstests --no-build --no-restore -nodereuse:false -v n -c $buildType"

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

        popd

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
