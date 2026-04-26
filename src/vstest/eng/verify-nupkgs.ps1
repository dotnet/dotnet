[CmdletBinding()]
Param(
    [Parameter(Mandatory)]
    [ValidateSet("Debug", "Release")]
    [string] $configuration,

    [Parameter(Mandatory)]
    [string] $versionPrefix,

    [Parameter(Mandatory)]
    [string] $currentBranch
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem

$isCI = $env:TF_BUILD -eq 'true' -or $env:CI -eq 'true'
$expectedCountsFile = Join-Path $PSScriptRoot "expected-nupkg-file-counts.json"

# Import binding redirect verification.
. "$PSScriptRoot/verify-binding-redirects.ps1"

function Verify-Nuget-Packages {
    Write-Host "Starting Verify-Nuget-Packages."
    $expectedNumOfFiles = @{}
    $json = Get-Content $expectedCountsFile -Raw | ConvertFrom-Json
    foreach ($property in $json.PSObject.Properties) {
        $expectedNumOfFiles[$property.Name] = [int]$property.Value
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
    
    # In source build we don't build this.
    $vsix = Get-Item "$PSScriptRoot/../artifacts/VSSetup/$configuration/Insertion/Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI.vsix" -ErrorAction Ignore
    if ($vsix) {
        $nugetPackages += $vsix
    }

    # Extract into the same location that IntegrationTestBuild.cs expects
    # (artifacts/tmp/{Config}/extractedPackages/{PackageFileName}), so integration
    # tests can reuse these extractions via the .cache marker files written below.
    $extractedPackagesDir = Join-Path $tmpDirectory "extractedPackages"
    New-Item -ItemType Directory -Path $extractedPackagesDir -Force | Out-Null
    Write-Host "Unzipping NuGet packages to '$extractedPackagesDir'."
    $unzipNugetPackageDirs = @()
    foreach ($nugetPackage in $nugetPackages) {
        $unzipNugetPackageDir = Join-Path $extractedPackagesDir $nugetPackage.Name
        $unzipNugetPackageDirs += $unzipNugetPackageDir

        if (Test-Path -Path $unzipNugetPackageDir) {
            Remove-Item -Force -Recurse $unzipNugetPackageDir
        }

        Unzip $nugetPackage.FullName $unzipNugetPackageDir
    }

    Write-Host "Verify NuGet packages files."
    $errors = @()
    $actualCounts = @{}
    $hasCountMismatch = $false
    foreach ($unzipNugetPackageDir in $unzipNugetPackageDirs) {
        # Directory is named after the package file (e.g. "Microsoft.TestPlatform.18.6.0-dev.nupkg"),
        # so strip the extension to get the base name for key derivation.
        $packageBaseName = [System.IO.Path]::GetFileNameWithoutExtension((Get-Item $unzipNugetPackageDir).Name)
        $packageKey = $packageBaseName.Replace([string]".$version", [string]"")
        Write-Host "Verifying package '$packageBaseName'."

        $actualNumOfFiles = (Get-ChildItem -Recurse -File -Path $unzipNugetPackageDir | Where-Object { $_.Name -ne '.signature.p7s' }).Count
        $actualCounts[$packageKey] = $actualNumOfFiles

        if (-not $expectedNumOfFiles.ContainsKey($packageKey)) {
            $errors += "Package '$packageKey' is not present in '$expectedCountsFile'. Add it to the expected counts file."
            $hasCountMismatch = $true
            continue
        }
        if ($expectedNumOfFiles[$packageKey] -ne $actualNumOfFiles) {
            $errors += "Number of files are not equal for '$packageBaseName', expected: $($expectedNumOfFiles[$packageKey]) actual: $actualNumOfFiles"
            $hasCountMismatch = $true
        }

        if ($packageKey -eq "Microsoft.TestPlatform") {
            Verify-Version -nugetDir $unzipNugetPackageDir -errors $errors
        }
    }

    if ($hasCountMismatch) {
        if ($isCI) {
            $errors += ""
            $errors += "To fix this, run the following command locally after building and packing:"
            $errors += "  .\build.cmd -c $configuration"
            $errors += "This will rebuild, pack, and auto-update '$expectedCountsFile' with the correct values."
            $errors += "Then commit the updated file."
            Write-Error "There are file count mismatches:`n$($errors -join "`n")"
        }
        else {
            Write-Host ""
            Write-Host "File count mismatches detected. Updating '$expectedCountsFile'." -ForegroundColor Yellow
            foreach ($err in $errors) {
                Write-Host "  $err" -ForegroundColor Yellow
            }

            # Build the updated counts: start from expected, overlay with actual
            $updatedCounts = [ordered]@{}
            foreach ($key in ($expectedNumOfFiles.Keys + $actualCounts.Keys) | Sort-Object -Unique) {
                if ($actualCounts.ContainsKey($key)) {
                    $updatedCounts[$key] = $actualCounts[$key]
                }
                else {
                    $updatedCounts[$key] = $expectedNumOfFiles[$key]
                }
            }

            # Write clean JSON (2-space indent, no BOM, no trailing spaces).
            $lines = @("{")
            $keys = @($updatedCounts.Keys)
            for ($i = 0; $i -lt $keys.Count; $i++) {
                $comma = if ($i -lt $keys.Count - 1) { "," } else { "" }
                $lines += "  ""$($keys[$i])"": $($updatedCounts[$keys[$i]])$comma"
            }
            $lines += "}"
            [System.IO.File]::WriteAllText($expectedCountsFile, ($lines -join "`n") + "`n", [System.Text.UTF8Encoding]::new($false))
            Write-Host "Updated '$expectedCountsFile'. Please commit the changes." -ForegroundColor Green
        }
    }
    elseif ($errors) {
        Write-Error "There are $($errors.Count) errors:`n$($errors -join "`n")"
    }

    # Write .cache marker files so IntegrationTestBuild.cs detects these
    # extractions and skips re-extracting the same packages during test init.
    Write-Host "Writing cache markers for extracted packages."
    foreach ($nugetPackage in $nugetPackages) {
        $unzipNugetPackageDir = Join-Path $extractedPackagesDir $nugetPackage.Name
        $cacheMarkerPath = Join-Path $unzipNugetPackageDir ($nugetPackage.Name + ".cache")
        $lastWriteTimeUtc = $nugetPackage.LastWriteTimeUtc.ToString("o", [System.Globalization.CultureInfo]::InvariantCulture)
        [System.IO.File]::WriteAllText($cacheMarkerPath, $lastWriteTimeUtc)
    }

    Write-Host "Completed Verify-Nuget-Packages."
    $unzipNugetPackageDirs
}

function Unzip {
    param([string]$zipfile, [string]$outpath)

    Write-Verbose "Unzipping '$zipfile' to '$outpath'."

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

function Match-VersionAgainstBranch {
    param ([string]$vsTestVersion, [string]$branchName,  [string[]]$errors)

    # Output useful info.
    Write-Host "VSTest Product Version: `"$vsTestVersion`""
    Write-Host "Current Branch: `"$branchName`""

    $versionIsRTM = $vsTestVersion -match "^\d+\.\d+\.\d+$"
    $versionIsRelease = $vsTestVersion -match "^\d+\.\d+\.\d+\-release\-\d{8}\-\d{2}$"
    $versionIsPreview = $vsTestVersion -match "^\d+\.\d+\.\d+\-preview\-\d{8}\-\d{2}$"

    $isReleaseBranch = $branchName -like "rel/*"
    $isPreviewBranch = $branchName -like "main"

    if (!$isReleaseBranch -and !$isPreviewBranch) {
        Write-Host "Skipping check since branch is neither `"release`" nor `"preview`""
        return
    }

    Write-Host "Matching branch against product version ... "
    if ($isReleaseBranch -and !$versionIsRTM -and !$versionIsRelease) {
        $errors += "Release version `"$vsTestVersion`" should either be RTM, or contain a `"release`" suffix."
    }
    if ($isPreviewBranch -and !$versionIsPreview) {
        $errors += "Preview version `"$vsTestVersion`" should contain a `"preview`" suffix."
    }
}

function Verify-Version {
    param ([string]$nugetDir, [string[]] $errors)

    $vsTestExe = "$nugetDir/tools/net462/Common7/IDE/Extensions/TestPlatform/vstest.console.exe"
    $vsTestProductVersion = (Get-Item $vsTestExe).VersionInfo.ProductVersion

    Match-VersionAgainstBranch -vsTestVersion $vsTestProductVersion -branchName $currentBranch -errors $errors
}

function Verify-NugetPackageExe {
    param(
        [Parameter(Mandatory)]
        [ValidateSet("Debug", "Release")]
        [string] $configuration,
        $UnzipNugetPackages
    )


    $exclusions = @{
        "CodeCoverage\CodeCoverage.exe"                = "x86"
        "Dynamic Code Coverage Tools\CodeCoverage.exe" = "x86"
        "amd64\CodeCoverage.exe"                       = "x64"

        "IntelliTrace.exe"                             = "x86"
        "ProcessSnapshotCleanup.exe"                   = "x86-64"
        "TDEnvCleanup.exe"                             = "x86"

        "TestPlatform\SettingsMigrator.exe"            = "x86"

        "dump\DumpMinitool.exe"                        = "x86-64"

        "VideoRecorder\VSTestVideoRecorder.exe"        = "x86"
    }

    $errs = @()
    $exes = $UnzipNugetPackages | Get-ChildItem -Filter *.exe -Recurse -Force
    if (0 -eq @($exes).Length) {
        throw "No exe files were found."
    }

    # use wow programfiles because they always point to x64 programfiles where VS is installed
    $dumpBin =  Get-ChildItem -Recurse -Force -Filter dumpbin.exe -path "$env:ProgramW6432\Microsoft Visual Studio\2022\Enterprise" | Select-Object -First 1
    if (-not $dumpBin) {
        throw "Did not find dumpbin.exe in '$env:ProgramW6432\Microsoft Visual Studio\2022\Enterprise'."
    }

    $corFlags = Get-ChildItem -Recurse -Force -Filter CorFlags.exe -path "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows" | Select-Object -First 1
    if (-not $corFlags) {
        throw "Did not find CorFlags.exe in '${env:ProgramFiles(x86)}\Microsoft SDKs\Windows'."
    }

    $exes | ForEach-Object {
        $m = & $dumpBin /headers $_.FullName | Select-String "machine \((.*)\)"
        if (-not $m.Matches.Success) {
            $err = "Did not find the platform of the exe $fullName)."
        }

        $platform = $m.Matches.Groups[1].Value
        $fullName = $_.FullName
        $name = $_.Name

        if ("x86" -eq $platform) {
            $corFlagsOutput = & $corFlags $fullName
            # this is an native x86 exe or a .net x86 that requires of prefers 32bit
            $platform = if ($corFlagsOutput -like "*does not have a valid managed header*" -or $corFlagsOutput -like "*32BITREQ  : 1*" -or $corFlagsOutput -like "*32BITPREF : 1*") {
                # this is an native x86 exe or a .net x86 that requires of prefers 32bit
                "x86" } else {
                # this is a x86 executable that is built as AnyCpu and does not prefer 32-bit so it will run as x64 on 64-bit system.
                "x86-64" }
        }

        if (($pair = $exclusions.GetEnumerator() | Where-Object { $fullName -like "*$($_.Name)" })) {
            if (1 -lt $($pair).Count) {
                $err = "Too many paths matched the query, only one match is allowed. Matches: $($pair.Name)"
                $errs += $err
                Write-Host -ForegroundColor Red Error: $err
            }

            if ($platform -ne $pair.Value) {
                $err = "$fullName must have architecture $($pair.Value), but it was $platform."
                $errs += $err
                Write-Host -ForegroundColor Red Error: $err
            }
        }
        elseif ("x86" -eq $platform) {
            if ($name -notlike "*x86*") {
                $err = "$fullName has architecture $platform, and must contain x86 in the name of the executable."
                $errs += $err
                Write-Host -ForegroundColor Red Error: $err
            }
        }
        elseif ($platform -in  "x64", "x86-64") {
            if ($name -like "*x86*" -or $name -like "*arm64*") {
                $err = "$fullName has architecture $platform, and must NOT contain x86 or arm64 in the name of the executable."
                $errs += $err
                Write-Host -ForegroundColor Red Error: $err
            }
        }
        elseif ("arm64" -eq $platform) {
            if ($name -notlike "*arm64*") {
                $err = "$fullName has architecture $platform, and must contain arm64 in the name of the executable."
                $errs += $err
                Write-Host -ForegroundColor Red Error: $err
            }
        }
        else {
            $err = "$fullName has unknown architecture $platform."
            $errs += $err
            Write-Host -ForegroundColor Red $err
        }

        "Success: $name is $platform - $fullName"
    }

    if ($errs) {
        throw "Fail!:`n$($errs -join "`n")"
    }
}

function Verify-NugetPackageVersion {
    param(
        [Parameter(Mandatory)]
        [ValidateSet("Debug", "Release")]
        [string] $configuration,
        $UnzipNugetPackages
    )

    # look for vstest.console.dll because unified build for .NET does not produce vstest.console.exe
    $exes = $UnzipNugetPackages | Get-ChildItem -Filter vstest.console.dll -Recurse -Force
    if (0 -eq @($exes).Length) {
        throw "No vstest.console.dll files were found."
    }

    $exes | ForEach-Object {
        if ($_.VersionInfo.ProductVersion.Contains("+")) {
            throw "$_ contains '+' in the ProductVersion $($_.VersionInfo.ProductVersion), this breaks DTAAgent in AzDO."
        }
        else {
            "$_ version $($_.VersionInfo.ProductVersion) is ok."
        }
    }

}


$unzipNugetPackages = Verify-Nuget-Packages
Start-sleep -Seconds 10
# skipped, it is hard to find the right dumpbin.exe and corflags tools on server
# Verify-NugetPackageExe -configuration $configuration -UnzipNugetPackages $unzipNugetPackages
Verify-NugetPackageVersion -configuration $configuration -UnzipNugetPackages $unzipNugetPackages

Write-Host "`nVerifying binding redirects..."
Verify-BindingRedirects -PackageDirs $unzipNugetPackages -Configuration $configuration
