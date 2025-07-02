param(
    [string]$buildType="Debug",
    [string]$dotnetDir="c:\Program Files\dotnet",
    [string]$root=$PSScriptRoot,
    [string]$slnFile="wilson.sln")

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

function CreateArtifactsFolder($folder)
{
    if (Test-Path($folder))
    {
        Write-Host ">>> Remove-Item -Recurse -Force $folder"
        Remove-Item  -Recurse -Force $folder
    }

    Write-Host ">>> mkdir $path | Out-Null"
    mkdir $folder | Out-Null
}

################################################# Functions ############################################################

WriteSectionHeader("pack.ps1 - parameters");
Write-Host "buildType:       " $buildType;
Write-Host "dotnetDir:       " $dotnetDir
Write-Host "root:            " $root;
Write-Host "slnFile:         " $slnFile;

[xml]$buildConfiguration = Get-Content $PSScriptRoot\buildConfiguration.xml
$artifactsRoot = "$root\artifacts";
$dotnetexe = "$dotnetDir\dotnet.exe";
$nugetVersion = $buildConfiguration.SelectSingleNode("root/nugetVersion").InnerText;
$releaseVersion = [string]$buildConfiguration.SelectSingleNode("root/release").InnerText;
$nugetPreview = $buildConfiguration.SelectSingleNode("root/nugetPreview").InnerText;

$startTime = Get-Date
Write-Host "Start Time:     " $startTime
Write-Host "PSScriptRoot:   " $PSScriptRoot;
Write-Host "artifactsRoot:  " $artifactsRoot;
Write-Host "dotnetexe:      " $dotnetexe;
Write-Host "nugetVersion:   " $nugetVersion;
Write-Host "releaseVersion: " $releaseVersion;
Write-Host "nugetPreview:   " $nugetPreview;

CreateArtifactsFolder($artifactsRoot);

foreach ($project in $buildConfiguration.SelectNodes("root/projects/src/project"))
{
    $name = $project.name
    $projectPath = [System.IO.Path]::Combine($root, "src", $name, "$name.csproj")

    Write-Host ">>> dotnet pack --no-build --no-restore -nodereuse:false -c $buildType -o $artifactsRoot -v q -s $projectPath"
    dotnet pack --no-build --no-restore -nodereuse:false -c $buildType -o $artifactsRoot -v q -s $projectPath

    if($LASTEXITCODE -ne 0)
    {
        throw "Error occurred while packaging project '$projectPath'!"
    }

    Write-Host "Packaging for project '$projectPath' completed successfully."
}

Write-Host "============================"
Write-Host ""
$time = Get-Date
Write-Host "Start Time:    " ($startTime);
Write-Host "End Time:      " ($time);
Write-Host "Time to pack: " ($time - $startTime);
Write-Host ""
Write-Host "============================";
