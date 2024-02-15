[CmdletBinding()]
Param(
    [Parameter(Mandatory)]
    [ValidateSet("Debug", "Release")]
    [string] $configuration,

    [Parameter(Mandatory)]
    [string] $versionPrefix
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem

function Verify-Nuget-Packages {
    Write-Host "Starting Verify-Nuget-Packages."
    $expectedNumOfFiles = @{
        "Microsoft.CodeCoverage"                      = 59;
        "Microsoft.NET.Test.Sdk"                      = 16;
        "Microsoft.TestPlatform"                      = 608;
        "Microsoft.TestPlatform.Build"                = 21;
        "Microsoft.TestPlatform.CLI"                  = 473;
        "Microsoft.TestPlatform.Extensions.TrxLogger" = 35;
        "Microsoft.TestPlatform.ObjectModel"          = 93;
        "Microsoft.TestPlatform.AdapterUtilities"     = 34;
        "Microsoft.TestPlatform.Portable"             = 595;
        "Microsoft.TestPlatform.TestHost"             = 63;
        "Microsoft.TestPlatform.TranslationLayer"     = 123;
        "Microsoft.TestPlatform.Internal.Uwp"         = 39;
    }

    $packageDirectory = Resolve-Path "$PSScriptRoot/../artifacts/packages/$configuration"
    $tmpDirectory = Resolve-Path "$PSScriptRoot/../artifacts/tmp/$configuration"

    $pattern = "*.$versionPrefix*.nupkg"
    $nugetPackages = @(Get-ChildItem $packageDirectory -Filter $pattern -Recurse -File | Where-Object { $_.Name -notLike "*.symbols.nupkg"})

    if (0 -eq $nugetPackages.Length) {
        throw "No nuget packages matching $pattern were found in '$packageDirectory'."
    }

    $suffixes = @($nugetPackages -replace ".*?$([regex]::Escape($versionPrefix))(.*)\.nupkg", '$1' | Sort-Object -Unique)
    if (1 -lt $suffixes.Length) {
        Write-Host "There are two different suffixes matching the same version prefix: '$($suffixes -join "', '")'".

        $latestNuget = $nugetPackages |
            Where-Object { $_.Name -like "Microsoft.TestPlatform.ObjectModel.*" } |
            Sort-Object -Property LastWriteTime -Descending |
            Select-Object -First 1

        $suffix = $suffixes | Where { $latestNuget.Name.Contains("$versionPrefix$_.nupkg") }
        $version = "$versionPrefix$suffix"
        Write-Host "The most recently written Microsoft.TestPlatform.ObjectModel.* nuget, is $($latestNuget.Name), which has '$suffix' suffix. Selecting only packages with that suffix."

        $nugetPackages = $nugetPackages | Where-Object { $_.Name -like "*$version.nupkg" }
    }
    else {
        $suffix = $suffixes[0]
        $version = "$versionPrefix$suffix"
    }


    Write-Host "Found $(@($nugetPackages).Count) nuget packages:`n    $($nugetPackages.FullName -join "`n    ")"
    Write-Host "Unzipping NuGet packages."
    $unzipNugetPackageDirs = @()
    foreach ($nugetPackage in $nugetPackages) {
        $unzipNugetPackageDir = Join-Path $tmpDirectory $nugetPackage.BaseName
        $unzipNugetPackageDirs += $unzipNugetPackageDir

        if (Test-Path -Path $unzipNugetPackageDir) {
            Remove-Item -Force -Recurse $unzipNugetPackageDir
        }

        Unzip $nugetPackage.FullName $unzipNugetPackageDir
    }

    Write-Host "Verify NuGet packages files."
    $errors = @()
    foreach ($unzipNugetPackageDir in $unzipNugetPackageDirs) {
        try {
            $packageBaseName = (Get-Item $unzipNugetPackageDir).BaseName
            $packageKey = $packageBaseName.Replace([string]".$version", [string]"")
            Write-Host "Verifying package '$packageBaseName'."

            $actualNumOfFiles = (Get-ChildItem -Recurse -File -Path $unzipNugetPackageDir).Count
            if (-not $expectedNumOfFiles.ContainsKey($packageKey)) {
                $errors += "Package '$packageKey' is not present in file expectedNumOfFiles table. Is that package known?"
                continue
            }
            if ($expectedNumOfFiles[$packageKey] -ne $actualNumOfFiles) {
                $errors += "Number of files are not equal for '$packageBaseName', expected: $($expectedNumOfFiles[$packageKey]) actual: $actualNumOfFiles"
            }
        }
        finally {
            if ($null -ne $unzipNugetPackageDir -and (Test-Path $unzipNugetPackageDir)) {
                Remove-Item -Force -Recurse $unzipNugetPackageDir | Out-Null
            }
        }
    }

    if ($errors) {
        Write-Error "There are $($errors.Count) errors:`n$($errors -join "`n")"
    }

    Write-Host "Completed Verify-Nuget-Packages."
}

function Unzip {
    param([string]$zipfile, [string]$outpath)

    Write-Verbose "Unzipping '$zipfile' to '$outpath'."

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

Verify-Nuget-Packages
