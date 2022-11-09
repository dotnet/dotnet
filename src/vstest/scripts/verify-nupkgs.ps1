Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip
{
    param([string]$zipfile, [string]$outpath)

    Write-VerboseLog "Unzip $zipfile to $outpath."

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

function Verify-Nuget-Packages($packageDirectory, $version)
{
    Write-Log "Starting Verify-Nuget-Packages."
    $expectedNumOfFiles = @{
        "Microsoft.CodeCoverage" = 58;
        "Microsoft.NET.Test.Sdk" = 16;
        "Microsoft.TestPlatform" = 603;
        "Microsoft.TestPlatform.Build" = 21;
        "Microsoft.TestPlatform.CLI" = 479;
        "Microsoft.TestPlatform.Extensions.TrxLogger" = 35;
        "Microsoft.TestPlatform.ObjectModel" = 93;
        "Microsoft.TestPlatform.AdapterUtilities" = 34;
        "Microsoft.TestPlatform.Portable" = 597;
        "Microsoft.TestPlatform.TestHost" = 63;
        "Microsoft.TestPlatform.TranslationLayer" = 123;
        "Microsoft.TestPlatform.Internal.Uwp" = 46;
    }

    $nugetPackages = Get-ChildItem -Filter "*$version*.nupkg" $packageDirectory | % { $_.FullName }

    Write-VerboseLog "Unzip NuGet packages."
    $unzipNugetPackageDirs =  New-Object System.Collections.Generic.List[System.Object]
    foreach($nugetPackage in $nugetPackages)
    {
        $unzipNugetPackageDir = $(Join-Path $packageDirectory $(Get-Item $nugetPackage).BaseName)
        $unzipNugetPackageDirs.Add($unzipNugetPackageDir)

        if(Test-Path -Path $unzipNugetPackageDir)
        {
            Remove-Item -Force -Recurse $unzipNugetPackageDir
        }

        Unzip $nugetPackage $unzipNugetPackageDir
    }

    Write-VerboseLog "Verify NuGet packages files."
    $errors = @()
    foreach($unzipNugetPackageDir in $unzipNugetPackageDirs)
    {
        try {
            $actualNumOfFiles = (Get-ChildItem -Recurse -File -Path $unzipNugetPackageDir).Count
            $versionLen = $version.Length + 1  # +1 for dot
            $packageKey = (Get-Item $unzipNugetPackageDir).BaseName -replace ".{$versionLen}$"
            Write-VerboseLog "verifying package $packageKey."

            if ($expectedNumOfFiles[$packageKey] -ne $actualNumOfFiles) {
                $errors += "Number of files are not equal $unzipNugetPackageDir, expected: $($expectedNumOfFiles[$packageKey]) actual: $actualNumOfFiles"
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

    Write-Log "Completed Verify-Nuget-Packages."
}
