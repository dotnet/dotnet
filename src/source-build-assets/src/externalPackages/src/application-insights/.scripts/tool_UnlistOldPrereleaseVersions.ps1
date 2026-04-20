Param(
    [Parameter(Mandatory=$true,HelpMessage="Api Key")]
    [string]
    $nugetApiKey
) 

# SUMMARY
# This script will lookup the version of the latest stable release of a specified package.
# This script will then attempt to unlist (nuget cli delete) any prerelease versions older than the latest stable.

# https://docs.microsoft.com/powershell/module/packagemanagement/find-package?view=powershell-7.2
# https://docs.microsoft.com/nuget/reference/cli-reference/cli-ref-delete

# WARNING
# The Delete/Unlist api has a quota of 250/hour
# https://docs.microsoft.com/en-us/nuget/api/rate-limits

$nugetSourceUrl = 'https://api.nuget.org/v3/index.json'

$packageNames = @("Microsoft.ApplicationInsights",
    "Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel",
    "Microsoft.AspNet.ApplicationInsights.HostingStartup",
    "Microsoft.ApplicationInsights.DependencyCollector",
    "Microsoft.ApplicationInsights.EventCounterCollector",
    "Microsoft.ApplicationInsights.PerfCounterCollector",
    "Microsoft.ApplicationInsights.WindowsServer",
    "Microsoft.ApplicationInsights.Web",
    "Microsoft.ApplicationInsights.AspNetCore",
    "Microsoft.ApplicationInsights.WorkerService",
    "Microsoft.Extensions.Logging.ApplicationInsights",
    "Microsoft.ApplicationInsights.NLogTarget",
    "Microsoft.ApplicationInsights.Log4NetAppender",
    "Microsoft.ApplicationInsights.TraceListener",
    "Microsoft.ApplicationInsights.DiagnosticSourceListener",
    "Microsoft.ApplicationInsights.EtwCollector",
    "Microsoft.ApplicationInsights.EventSourceListener"
)


# Get the latest Nuget.exe from here:
$nugetPath = ([System.IO.Path]::Combine($PSScriptRoot, '.\NuGet.exe'));

Write-Host "Searching for Nuget.exe..."
if (Test-Path $nugetPath) {
    Write-Host "Nuget.exe found."
}
else {
    Write-Host "Nuget.exe not found. Attempting download...";
    $downloadNugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";
    (New-Object System.Net.WebClient).DownloadFile($downloadNugetUrl, $nugetPath);
    
    if (!(Test-Path $nugetPath)) {
        throw "Error: Failed to download! Please download latest from: https://www.nuget.org/downloads";
    }
}

$counter = 0;

foreach ($name in $packageNames)
{
    # Find the latest release
    Write-Host
    Write-Host "Querying for latest version of $($name)..."
    $latest = Find-Package -Name $name -Source $nugetSourceUrl
    Write-Host $latest.Name $latest.Version

    if ($null -eq $latest)
    {
        continue;
    }

    # Find all versions older than latest. This filters out any newer prerelease versions.
    Write-Host
    Write-Host "Querying for all versions..."
    $packagesList = Find-Package -Name $latest.Name -Source $nugetSourceUrl -AllVersions -MaximumVersion $latest.Version -AllowPrereleaseVersions

    foreach ($package in $packagesList)
    {
        # Prerelease versions will contain a hyphen ('-').
        if ($package.Version.IndexOf('-') -gt -1)
        {
            Write-Host $package.Version " PreRelease"

            Write-Host "Unlist: $($package.Name) $($package.Version)"
            
            & $nugetPath delete $package.Name $package.Version -ApiKey $nugetApiKey -Source $nugetSourceUrl -NonInteractive

            $counter++
        }
        else {
            Write-Host $package.Version;
        }
    }
}

Write-Host "Unlisted $($counter) packages."
Write-Host "Done."
