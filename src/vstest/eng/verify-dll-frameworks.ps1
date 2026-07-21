# Verifies that every managed DLL in shipped packages has the expected target
# framework.  Native DLLs are excluded by name.
#
# In CI:     validates and fails with instructions to run locally.
# Locally:   auto-fixes the expected data file with the correct values.
#
# This script is dot-sourced from verify-nupkgs.ps1.

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$script:isCI = $env:TF_BUILD -eq 'true' -or $env:CI -eq 'true'
$script:ExpectedFrameworksFile = Join-Path $PSScriptRoot "expected-dll-frameworks.json"

# Reads the TargetFrameworkAttribute value from a managed DLL by searching the
# assembly bytes for the well-known TFM identifier strings embedded by the
# compiler.  The match is validated against the custom-attribute blob format
# (0x01 0x00 prolog, followed by a compressed-uint string length) to avoid
# false positives from string resources or code constants.
#
# Returns:
#   - The TFM string (e.g. ".NETCoreApp,Version=v8.0") for managed DLLs
#   - ""    for managed DLLs that lack a TargetFrameworkAttribute
#   - $null for native (unmanaged) DLLs
function Get-DllTargetFramework {
    param([string]$DllPath)

    # Detect managed vs native.  GetAssemblyName reads PE metadata without
    # loading the assembly.  It throws BadImageFormatException for native DLLs.
    try {
        $null = [System.Reflection.AssemblyName]::GetAssemblyName($DllPath)
    }
    catch [System.BadImageFormatException] {
        return $null
    }

    # Read the raw bytes and interpret them as ASCII.  The TFM string is always
    # pure ASCII, and non-ASCII bytes become '?' which won't affect the search.
    $bytes = [System.IO.File]::ReadAllBytes($DllPath)
    $text = [System.Text.Encoding]::ASCII.GetString($bytes)

    $familyMap = [ordered]@{
        '.NETCoreApp,Version=v'   = 'net'
        '.NETFramework,Version=v' = 'netframework'
        '.NETStandard,Version=v'  = 'netstandard'
    }

    foreach ($prefix in $familyMap.Keys) {
        $startIdx = 0
        while ($true) {
            $idx = $text.IndexOf($prefix, $startIdx, [System.StringComparison]::Ordinal)
            if ($idx -lt 0) { break }

            $end = $idx + $prefix.Length
            while ($end -lt $text.Length -and ([char]::IsDigit($text[$end]) -or $text[$end] -eq '.')) {
                $end++
            }
            $tfm = $text.Substring($idx, $end - $idx).TrimEnd('.')
            $tfmLength = $tfm.Length

            # Validate custom-attribute blob structure.  The TargetFrameworkAttribute
            # constructor argument is serialized as:
            #   0x01 0x00           (prolog)
            #   <compressed-uint>   (string byte length, 1 byte for lengths <= 127)
            #   <UTF-8 bytes>       (the TFM string itself)
            #
            # Checking the prolog and length byte eliminates false positives from
            # string resources or code constants that happen to contain TFM text.
            if ($idx -ge 3 -and
                $bytes[$idx - 3] -eq 0x01 -and
                $bytes[$idx - 2] -eq 0x00 -and
                $bytes[$idx - 1] -eq $tfmLength) {
                return $familyMap[$prefix]
            }

            $startIdx = $end
        }
    }

    # Managed assembly without TargetFrameworkAttribute — older assemblies
    # built before the attribute became standard (e.g. BCL compat shims).
    return "none"
}

# Detects a satellite (localized resources) assembly that has been misnamed as a
# plain '<name>.dll' instead of '<name>.resources.dll'.  The CLR satellite loader
# only probes '<culture>\<name>.resources.dll', so a misnamed satellite is never
# found and its localized strings silently fall back to the neutral language.
# This guards against reintroducing the packaging bug where a hand-written
# PackagePath dropped the '.resources' segment from the file name.
#
# Returns the culture name (e.g. 'cs') when the DLL is a misnamed satellite,
# otherwise $null.
function Get-MisnamedSatelliteCulture {
    param([string]$DllPath)

    try {
        $an = [System.Reflection.AssemblyName]::GetAssemblyName($DllPath)
    }
    catch {
        # Native or unreadable — target-framework detection handles those.
        return $null
    }

    if (-not [string]::IsNullOrEmpty($an.CultureName) -or $an.Name -like '*.resources') {
        return $an.CultureName
    }

    return $null
}

function Verify-DllFrameworks {
    param(
        [Parameter(Mandatory)]
        [string[]] $PackageDirs,

        [Parameter(Mandatory)]
        [ValidateSet("Debug", "Release")]
        [string] $Configuration,

        [Parameter(Mandatory)]
        [string] $Version
    )

    Write-Host "Starting Verify-DllFrameworks."

    # --- Load expected data ------------------------------------------------
    $expectedNativeExcludes = @()
    $expectedFrameworks = [ordered]@{}

    if (Test-Path $script:ExpectedFrameworksFile) {
        $json = Get-Content $script:ExpectedFrameworksFile -Raw | ConvertFrom-Json
        $expectedNativeExcludes = @($json.nativeExcludes)
        foreach ($pkgProp in $json.frameworks.PSObject.Properties) {
            $expectedFrameworks[$pkgProp.Name] = [ordered]@{}
            foreach ($dllProp in $pkgProp.Value.PSObject.Properties) {
                $expectedFrameworks[$pkgProp.Name][$dllProp.Name] = $dllProp.Value
            }
        }
    }

    $nativeSet = [System.Collections.Generic.HashSet[string]]::new(
        [string[]]$expectedNativeExcludes,
        [System.StringComparer]::OrdinalIgnoreCase
    )

    # --- Scan packages and build actual data -------------------------------
    $actualFrameworks = [ordered]@{}
    $actualNatives = [System.Collections.Generic.HashSet[string]]::new(
        [System.StringComparer]::OrdinalIgnoreCase
    )
    $errors = @()
    $misnamedSatellites = @()

    foreach ($pkgDir in $PackageDirs) {
        $pkgItem = Get-Item $pkgDir
        $baseName = [System.IO.Path]::GetFileNameWithoutExtension($pkgItem.Name)
        $packageKey = $baseName.Replace([string]".$Version", [string]"")

        Write-Host "  Scanning '$packageKey'..."

        $dlls = @(Get-ChildItem -Path $pkgDir -Filter *.dll -Recurse -File)
        $pkgMap = [ordered]@{}

        foreach ($dll in $dlls) {
            $relativePath = $dll.FullName.Substring($pkgDir.Length).TrimStart('\', '/') -replace '\\', '/'
            $dllName = $dll.Name

            if ($nativeSet.Contains($dllName)) {
                [void]$actualNatives.Add($dllName)
                continue
            }

            # Resource assemblies don't carry a meaningful TFM — skip them.
            if ($dllName -like '*.resources.dll') {
                continue
            }

            # Guard: a satellite misnamed as '<name>.dll' (dropping '.resources')
            # is invisible to the CLR satellite loader, so its localized resources
            # silently never load.  Catch it before the TFM scan, where it would
            # otherwise masquerade as a neutral assembly with no TargetFramework.
            $misnamedCulture = Get-MisnamedSatelliteCulture -DllPath $dll.FullName
            if ($null -ne $misnamedCulture) {
                $expectedName = "$([System.IO.Path]::GetFileNameWithoutExtension($dllName)).resources.dll"
                $misnamedSatellites += "[$packageKey] '$relativePath' is a '$misnamedCulture' satellite but is named '$dllName' instead of '$expectedName'."
                continue
            }

            $tfm = Get-DllTargetFramework -DllPath $dll.FullName

            if ($null -eq $tfm) {
                # Unmanaged DLL not in the exclude list — record it so the
                # auto-fix adds it, but also report the discrepancy.
                [void]$actualNatives.Add($dllName)
                $errors += "Native DLL '$dllName' (in $packageKey/$relativePath) is not in the nativeExcludes list."
                continue
            }

            $pkgMap[$relativePath] = $tfm
        }

        if ($pkgMap.Count -gt 0) {
            $actualFrameworks[$packageKey] = $pkgMap
        }
    }

    # --- Guard: misnamed satellites fail immediately -----------------------
    # This is a structural packaging error (localized resources won't load),
    # not data drift the auto-fixer should silently absorb into the baseline,
    # so fail fast in both CI and local runs.
    if ($misnamedSatellites.Count -gt 0) {
        $satMsg = "Misnamed satellite assemblies detected - localized resources will not load:`n"
        $satMsg += ($misnamedSatellites -join "`n")
        $satMsg += "`n`nA satellite assembly must ship as '<name>.resources.dll' inside its '<culture>' folder. "
        $satMsg += "Fix the Pack/PackagePath metadata for the file(s) above so the '.resources' suffix is preserved."
        Write-Error $satMsg
    }

    # --- Compare -----------------------------------------------------------
    $hasMismatch = $false

    foreach ($pkg in $actualFrameworks.Keys) {
        if (-not $expectedFrameworks.Contains($pkg)) {
            $errors += "Package '$pkg' is not present in '$($script:ExpectedFrameworksFile)'. Run locally to add it."
            $hasMismatch = $true
            continue
        }

        foreach ($path in $actualFrameworks[$pkg].Keys) {
            $actualTfm = $actualFrameworks[$pkg][$path]

            if (-not $expectedFrameworks[$pkg].Contains($path)) {
                $errors += "[$pkg] New DLL '$path' with TFM '$actualTfm'."
                $hasMismatch = $true
                continue
            }

            $expectedTfm = $expectedFrameworks[$pkg][$path]
            if ($actualTfm -cne $expectedTfm) {
                $errors += "[$pkg] TFM changed for '$path': expected '$expectedTfm', actual '$actualTfm'."
                $hasMismatch = $true
            }
        }

        # Detect removals.
        foreach ($path in $expectedFrameworks[$pkg].Keys) {
            if (-not $actualFrameworks[$pkg].Contains($path)) {
                $errors += "[$pkg] DLL removed: '$path' (was '$($expectedFrameworks[$pkg][$path])')."
                $hasMismatch = $true
            }
        }
    }

    # Packages that were expected but are now absent.
    # In VMR/source-build, not all packages are built, so we only warn (don't error)
    # when expected packages are missing from the actual set.
    foreach ($pkg in $expectedFrameworks.Keys) {
        if (-not $actualFrameworks.Contains($pkg)) {
            Write-Host "  Package '$pkg' is in the expected file but was not found in actual packages. Skipping." -ForegroundColor Yellow
        }
    }

    # Compare native exclude lists.
    # Only flag new natives not in expected. Missing natives are expected
    # when not all packages are built (e.g. VMR/source-build).
    $newNatives = @($actualNatives | Where-Object { $_ -notin $expectedNativeExcludes })
    $nativesChanged = $newNatives.Count -gt 0

    # --- Report / fix ------------------------------------------------------
    if (-not $hasMismatch -and -not $nativesChanged -and $errors.Count -eq 0) {
        Write-Host "All DLL target frameworks match expectations."
        Write-Host "Completed Verify-DllFrameworks."
        return
    }

    if ($script:isCI) {
        foreach ($err in $errors) {
            Write-Host "  $err" -ForegroundColor Red
        }
        if ($nativesChanged) {
            Write-Host "  Native exclude list differs from actual." -ForegroundColor Red
        }

        $message = "DLL target framework mismatches detected:`n"
        $message += ($errors -join "`n")
        $message += "`n`nTo fix this, run the following command locally after building and packing:`n"
        $message += "  .\build.cmd -c $Configuration`n"
        $message += "This will rebuild, pack, and auto-update '$($script:ExpectedFrameworksFile)' with the correct values.`n"
        $message += "Then commit the updated file."
        Write-Error $message
    }
    else {
        Write-Host ""
        Write-Host "DLL framework mismatches detected. Updating '$($script:ExpectedFrameworksFile)'." -ForegroundColor Yellow
        foreach ($err in $errors) {
            Write-Host "  $err" -ForegroundColor Yellow
        }

        Write-DllFrameworksJson -NativeExcludes @($actualNatives | Sort-Object) -Frameworks $actualFrameworks
        Write-Host "Updated '$($script:ExpectedFrameworksFile)'. Please commit the changes." -ForegroundColor Green
    }

    Write-Host "Completed Verify-DllFrameworks."
}

function Write-DllFrameworksJson {
    param(
        [string[]] $NativeExcludes,
        [System.Collections.Specialized.OrderedDictionary] $Frameworks
    )

    $sb = [System.Text.StringBuilder]::new()
    [void]$sb.AppendLine('{')

    # nativeExcludes
    [void]$sb.AppendLine('  "nativeExcludes": [')
    for ($i = 0; $i -lt $NativeExcludes.Count; $i++) {
        $comma = if ($i -lt $NativeExcludes.Count - 1) { ',' } else { '' }
        [void]$sb.AppendLine("    `"$($NativeExcludes[$i])`"$comma")
    }
    [void]$sb.AppendLine('  ],')

    # frameworks — packages sorted by name, DLL paths sorted within each package
    [void]$sb.AppendLine('  "frameworks": {')
    $pkgKeys = @($Frameworks.Keys | Sort-Object)
    for ($p = 0; $p -lt $pkgKeys.Count; $p++) {
        $pkgKey = $pkgKeys[$p]
        [void]$sb.AppendLine("    `"$pkgKey`": {")

        $dllKeys = @($Frameworks[$pkgKey].Keys | Sort-Object)
        for ($d = 0; $d -lt $dllKeys.Count; $d++) {
            $dllKey = $dllKeys[$d]
            $tfm = $Frameworks[$pkgKey][$dllKey]
            $comma = if ($d -lt $dllKeys.Count - 1) { ',' } else { '' }
            [void]$sb.AppendLine("      `"$dllKey`": `"$tfm`"$comma")
        }

        $pkgComma = if ($p -lt $pkgKeys.Count - 1) { ',' } else { '' }
        [void]$sb.AppendLine("    }$pkgComma")
    }
    [void]$sb.AppendLine('  }')

    [void]$sb.Append('}')

    [System.IO.File]::WriteAllText(
        $script:ExpectedFrameworksFile,
        ($sb.ToString() + "`n"),
        [System.Text.UTF8Encoding]::new($false)
    )
}
