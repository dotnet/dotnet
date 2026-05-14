<#
.SYNOPSIS
Updates the NuGet License file with the latest SPDX License list.

.DESCRIPTION
Downloads the SPDX License list, builds and runs the GenerateLicenseList tool and later cleans up the check out SPDX license list data.

#>

function DownloadFileFromUrl {
param
(
    [Parameter(Mandatory=$True)]
    [string]$BaseUrl,
    [Parameter(Mandatory=$True)]
    [string]$Directory, 
    [Parameter(Mandatory=$True)]
    [string]$FileName
)
    $FilePath = [System.IO.Path]::Combine($Directory, $FileName)
    $FullUrl =  "$BaseUrl$FileName"

    If(!(Test-Path $Directory))
    {
        & New-Item -ItemType Directory -Force -Path $directory > $null
    }
    Write-Host "Downloading $FullUrl to $FilePath"

    Invoke-WebRequest -Uri $FullUrl -UseBasicParsing -Outfile $FilePath
}


try {
    $licenseListDirectory = $([System.IO.Path]::Combine($env:TEMP, "NuGet", "licenseList"))
    $licenseListBaseUrl = "https://raw.githubusercontent.com/spdx/license-list-data/master/json/"
    $licenseFileName = "licenses.json"
    $exceptionsFileName = "exceptions.json"

    DownloadFileFromUrl -BaseUrl $licenseListBaseUrl -Directory $licenseListDirectory -FileName $licenseFileName
    DownloadFileFromUrl -BaseUrl $licenseListBaseUrl -Directory $licenseListDirectory -FileName $exceptionsFileName

    $targetFile = $([System.IO.Path]::GetFullPath($([System.IO.Path]::Combine($PSScriptRoot, "..", "..", "src\NuGet.Core\NuGet.Packaging\Licenses\NuGetLicenseData.cs"))))
    $generateLicenseList = $([System.IO.Path]::GetFullPath($([System.IO.Path]::Combine($PSScriptRoot, "..", "..", "test\TestExtensions\GenerateLicenseList\GenerateLicenseList.csproj"))))
    
    Write-Host "Generating the license list. Target file: $targetFile"

    $licenseFile =  [System.IO.Path]::Combine($licenseListDirectory, $licenseFileName)
    $exceptionsFile = [System.IO.Path]::Combine($licenseListDirectory, $exceptionsFileName)

    dotnet run --project $generateLicenseList $licenseFile $exceptionsFile $targetFile
}
catch 
{
    Write-Host $_ -ForegroundColor "red"
}
finally 
{
    Write-Host "Cleaning up."
    Write-Host "Removing $licenseListDirectory"
    Remove-Item -Force -Recurse $licenseListDirectory
}

