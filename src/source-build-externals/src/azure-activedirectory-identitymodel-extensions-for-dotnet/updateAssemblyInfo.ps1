param([string]$root=$PSScriptRoot,
      [string]$packageType="preview")

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
    Write-Host $sectionName
    Write-Host "End Time:     "  $startTime		
    Write-Host "============================"
    Write-Host ""
}

################################################# Functions ############################################################

WriteSectionHeader("updateAssemblyInfo.ps1");
Write-Host "root:           " $root;
Write-Host "PSScriptRoot:   " $PSScriptRoot;

$date = Get-Date
$dateTimeStamp = ($date.ToString("yy")-19).ToString() + $date.ToString("MMddHHmmss")
[xml]$buildConfiguration = Get-Content $PSScriptRoot\buildConfiguration.xml

$assemblyVersion = [string]$buildConfiguration.SelectSingleNode("root/assemblyVersion").InnerText
$assemblyFileVersion = $assemblyVersion + "." + ($date.ToString("yy")-19).ToString() + $date.ToString("MMdd")
$assemblyInformationalVersion = $assemblyVersion + "." + $dateTimeStamp + "." + (git rev-parse HEAD)
Write-Host "assemblyVersion: "  $assemblyVersion
Write-Host "assemblyFileVersion: " $assemblyFileVersion
Write-Host "assemblyInformationalVersion: "  $assemblyInformationalVersion

$nugetSuffix = [string]$buildConfiguration.SelectSingleNode("root/nugetSuffix").InnerText
if ( $packageType -eq "release")
{
    $versionSuffix = ""
}
else
{
    $versionSuffix = $nugetSuffix + "-" + $dateTimeStamp
}

Write-Host "nugetSuffix: " $nugetSuffix
Write-Host "versionSuffix: " $versionSuffix

$versionPath = $PSScriptRoot + "/build/version.props"
$version = Get-Content $versionPath
$version = $version -replace "<VersionPrefix>(.*)</VersionPrefix>", "<VersionPrefix>$assemblyVersion</VersionPrefix>"
$version = $version -replace "<VersionSuffix>(.*)</VersionSuffix>", "<VersionSuffix>$versionSuffix</VersionSuffix>"
Set-Content $versionPath $version

foreach($project in $buildConfiguration.SelectNodes("root/projects/src/project"))
{
    $name = $project.name
    $assemblyInfoPath = "$root\src\$name\Properties\AssemblyInfo.cs"
    Write-Host "assemblyInfoPath: " $assemblyInfoPath

    $assemblyInfo = Get-Content $assemblyInfoPath
    $assemblyInfo = $assemblyInfo -replace "AssemblyVersion(.*)", "AssemblyVersion(""$assemblyVersion"")]"
    $assemblyInfo = $assemblyInfo -replace "AssemblyFileVersion(.*)", "AssemblyFileVersion(""$assemblyFileVersion"")]"
    $assemblyInfo = $assemblyInfo -replace "AssemblyInformationalVersion(.*)", "AssemblyInformationalVersion(""$assemblyInformationalVersion"")]"
    Set-Content $assemblyInfoPath $assemblyInfo
}

WriteSectionFooter("updateAssemblyInfo.ps1")
