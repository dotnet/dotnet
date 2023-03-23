<#
.SYNOPSIS
For a series of known or specified channels, lists out what would be installed for a given quality level and channel.

.PARAMETER Quality
Optional quality(s) to check

.PARAMETER Channel
Optional channel(s) to check. Otherwise checks a bunch of known channels.

.PARAMETER Component
Optional channel(s) to check. Otherwise checks a bunch of known components.

#>

param (
    [CmdletBinding()]

    [ValidateSet("preview", "ga", "daily", "signed", "validated")]
    [Parameter()]
    [string[]]$Quality = @("preview", "ga", "daily", "signed", "validated"),

    [ValidateSet("6.0", "6.0.1xx", "6.0.3xx", "6.0.4xx", "7.0", "7.0.1xx", "7.0.2xx", "8.0", "8.0.1xx")]
    [Parameter()]
    [string[]]$Channel = @("6.0", "6.0.1xx", "6.0.3xx", "6.0.4xx", "7.0", "7.0.1xx", "7.0.2xx", "8.0", "8.0.1xx"),

    [ValidateSet("runtime", "windowsdesktop", "sdk", "aspnetcore")]
    [Parameter()]
    [string[]]$Component = @("runtime", "windowsdesktop", "sdk", "aspnetcore"),

    [Parameter()]
    [switch]$Json
)

$queries = New-Object System.Collections.ArrayList
foreach ($ch in $Channel) {
    foreach ($q in $Quality) {
        foreach ($co in $Component) {
            # Filter out ones we know will be wrong
            if ($co -ne "sdk" -and $ch -like '*xx*') {
                continue
            }
            $queries += @{channel = $ch; quality = $q; component = $co }
        }
    }
}

Write-Verbose "Querying $($queries.Count) combinations"

$productVersions = @()

$productVersions += $queries | ForEach-Object -Parallel {
    function DoLookup($channel, $quality, $component) {
        $akaMSPrefix = "https://aka.ms/dotnet"

        $versionStringUrl = ""
        if ($quality -eq "ga") {
            $versionStringUrl = "$akaMSPrefix/$channel/$component-productVersion.txt"
        }
        else {
            $versionStringUrl = "$akaMSPrefix/$channel/$quality/$component-productVersion.txt"
        }

        try {
            $response = Invoke-WebRequest $versionStringUrl

            # It's difficult to tell whether the aka.ms link 404s or not, since it redirects in case of
            # a not found.
            if ($response.StatusCode -ne 200 -or
                $response.Headers.'Content-Type' -eq 'text/html') {
                return "N/A"
            }

            return [System.Text.Encoding]::ASCII.GetString($response.Content).Trim()
        }
        catch {
            return 'N/A'
        }
    }

    $channel = $_.channel
    $quality = $_.quality
    $component = $_.component

    @{channel = $channel; quality = $quality; component = $component; version = $(DoLookup $channel $quality $component) }
}

$validVersions = $productVersions | Where-Object { $_.version -ne 'N/A' }

if ($Json) {
    $validVersions | ConvertTo-Json
}
else {
    $validVersions | ForEach-Object {
        $formatStr = "{0, -10}{1, -10}{2, -16}{3, -10}" -f $_.channel, $_.quality, $_.component, $_.version
        Write-Host $formatStr
    }
}
