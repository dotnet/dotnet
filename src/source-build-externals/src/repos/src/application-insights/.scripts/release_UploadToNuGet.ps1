Param(
   [Parameter(Mandatory=$true,HelpMessage="Enter NuGet API Key:")]
   [string]$nugetApiKey
) 

# SUMMARY
# This script will upload nupkg packages to a NuGet repository.
# This script expects to be in the same directory as the nupkg packages.
# This script will attempt to download the latest Nuget.exe. 

# DEVELOPER NOTES
# This script is provided as a backup in the event that our release automation is unavailable.

# Get the latest Nuget.exe from here:
$nugetPath = ([System.IO.Path]::Combine($PSScriptRoot, '.\NuGet.exe'));

if (!(Test-Path $nugetPath)) {

    Write-Host "Nuget.exe not found. Attempting download...";
    Write-Host "Start time:" (Get-Date -Format G);
    $downloadNugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";
    (New-Object System.Net.WebClient).DownloadFile($downloadNugetUrl, $nugetPath);
    Write-Host "Finish time:" (Get-Date -Format G);
    
    if (!(Test-Path $nugetPath)) {
        throw "Error: Nuget.exe not found! Please download latest from: https://www.nuget.org/downloads";
    }
}

#$nugetSourceUrl = 'https://www.myget.org/F/applicationinsights/api/v2/package'
#$nugetSourceUrl = 'https://www.nuget.org/api/v2/package'
$nugetSourceUrl = 'https://api.nuget.org/v3/index.json';

Get-ChildItem $PSScriptRoot -Filter *.nupkg |
ForEach-Object {
    if (!$_.Name.Contains('.symbols.')) {
        Write-Host $_.Name
        & $nugetPath push $_.FullName -ApiKey $nugetApiKey -Source $nugetSourceUrl
    }
}
