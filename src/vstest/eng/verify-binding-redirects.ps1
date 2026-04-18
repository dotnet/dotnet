$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Verifies that binding redirects in .exe.config files match the actual assembly
# versions of the DLLs shipped in the same package directory.

function Verify-BindingRedirects {
    param(
        [Parameter(Mandatory)]
        [string[]]$PackageDirs
    )

    $errors = @()

    # Step 1: Parse ALL config files and collect unique DLL paths we need to check
    $redirectEntries = @() # { ConfigFile, AssemblyName, ExpectedVersion, DllPath }

    foreach ($packageDir in $PackageDirs) {
        if (-not (Test-Path $packageDir)) { continue }

        $configFiles = @(Get-ChildItem -Recurse $packageDir -Filter "*.config" | Where-Object {
            $_.Extension -eq ".config" -and (Get-Content $_.FullName -Raw) -match "bindingRedirect"
        })

        foreach ($configFile in $configFiles) {
            [xml]$config = Get-Content $configFile.FullName

            $xmlNsMgr = New-Object System.Xml.XmlNamespaceManager($config.NameTable)
            $xmlNsMgr.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1")

            $dependentAssemblies = $config.SelectNodes("//asm:dependentAssembly", $xmlNsMgr)
            if ($null -eq $dependentAssemblies -or $dependentAssemblies.Count -eq 0) { continue }

            $configDir = Split-Path $configFile.FullName
            $searchDirs = @($configDir)

            $probingNodes = $config.SelectNodes("//asm:probing", $xmlNsMgr)
            foreach ($probing in $probingNodes) {
                $privatePath = $probing.GetAttribute("privatePath")
                if ($privatePath) {
                    foreach ($subPath in $privatePath -split ";") {
                        $probingDir = Join-Path $configDir $subPath.Trim()
                        if (Test-Path $probingDir) { $searchDirs += $probingDir }
                    }
                }
            }

            foreach ($dep in $dependentAssemblies) {
                $identityNode = $dep.SelectSingleNode("asm:assemblyIdentity", $xmlNsMgr)
                $redirectNode = $dep.SelectSingleNode("asm:bindingRedirect", $xmlNsMgr)
                if ($null -eq $identityNode -or $null -eq $redirectNode) { continue }

                $assemblyName = $identityNode.GetAttribute("name")
                $expectedVersion = $redirectNode.GetAttribute("newVersion")

                $dllPath = $null
                foreach ($dir in $searchDirs) {
                    $candidate = Join-Path $dir "$assemblyName.dll"
                    if (Test-Path $candidate) { $dllPath = $candidate; break }
                }

                if ($dllPath) {
                    $redirectEntries += [PSCustomObject]@{
                        ConfigFile = $configFile.Name
                        AssemblyName = $assemblyName
                        ExpectedVersion = $expectedVersion
                        DllPath = $dllPath
                    }
                } else {
                    Write-Host "  INFO: $assemblyName referenced in $($configFile.Name) not found in package (may be runtime-provided)"
                }
            }
        }
    }

    if ($redirectEntries.Count -eq 0) {
        Write-Error "No binding redirect entries with matching DLLs found."
    }

    # Step 2: Get unique DLL paths, get versions in ONE subprocess using relative paths
    $uniqueDlls = @($redirectEntries | Select-Object -ExpandProperty DllPath -Unique)

    Write-Host "Checking assembly versions for $($uniqueDlls.Count) DLLs referenced in binding redirects..."

    # Write paths to a temp file to avoid command line length limits
    $tempFile = [IO.Path]::ChangeExtension([IO.Path]::GetTempFileName(), ".ps1");
    $files = $uniqueDlls -join '", "'

    $command = '
        $uniqueDlls = @( "##FILES##" )
        $uniqueDlls | ForEach-Object {
            try {
                $asm = [System.Reflection.Assembly]::LoadFile($_)
                $n = $asm.GetName()
                Write-Output "$_|$($n.Name)|$($n.Version)"
            } catch {
                Write-Output "$_|ERROR|0.0.0.0"
            }
    }
    ' -replace '##FILEs##', $files
    Set-Content -Path $tempFile -Value $command
    # Run in child process we are loadig dlls to get the assembly names
    $versionOutput = & pwsh -NoProfile -File $tempFile
    Remove-Item $tempFile -ErrorAction SilentlyContinue

    # Step 3: Build lookup
    $versionMap = @{}
    foreach ($line in $versionOutput) {
        if ($line -and $line.Contains("|")) {
            $parts = $line -split '\|', 3
            $versionMap[$parts[0]] = [PSCustomObject]@{ Name = $parts[1]; Version = $parts[2] }
        }
    }

    # Step 4: Compare
    foreach ($entry in $redirectEntries) {
        $info = $versionMap[$entry.DllPath]
        if (-not $info -or $info.Name -eq "ERROR") {
            Write-Error "Could not read assembly version for $($entry.AssemblyName)"
            continue
        }

        $normalizedExpected = Normalize-Version $entry.ExpectedVersion
        $normalizedActual = Normalize-Version $info.Version

        if ($normalizedExpected -ne $normalizedActual) {
            $msg = "MISMATCH: $($entry.AssemblyName) in $($entry.ConfigFile): redirect says $($entry.ExpectedVersion) but DLL is $($info.Version)"
            $errors += $msg
        }
    }

    $errors = @($errors | Sort-Object -Unique)
    
    if ($errors.Count -eq 0) {
        Write-Host "All binding redirects match their DLL versions."
    }
    else {
        Write-Error "Found $($errors.Count) binding redirect mismatches: $($errors -join "`n")"
    }
}

function Normalize-Version {
    param([string]$v)
    try {
        $ver = [Version]$v.Trim()
        $rev = if ($ver.Revision -lt 0) { 0 } else { $ver.Revision }
        $bld = if ($ver.Build   -lt 0) { 0 } else { $ver.Build }
        return "$($ver.Major).$($ver.Minor).$bld.$rev"
    } catch {
        return $v.Trim()
    }
}
