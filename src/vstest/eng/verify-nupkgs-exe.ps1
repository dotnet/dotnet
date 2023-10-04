$exclusions = @{
    "CodeCoverage\CodeCoverage.exe"                = "x86"
    "Dynamic Code Coverage Tools\CodeCoverage.exe" = "x86"
    "amd64\CodeCoverage.exe"                       = "x64"

    "IntelliTrace.exe"                             = "x86"
    "ProcessSnapshotCleanup.exe"                   = "x86-64"
    "TDEnvCleanup.exe"                             = "x86"

    "TestPlatform\SettingsMigrator.exe"            = "x86"

    "dump\DumpMinitool.exe"                        = "x86-64"

    "QTAgent32.exe"                                = "x86"
    "QTAgent32_35.exe"                             = "x86"
    "QTAgent32_40.exe"                             = "x86"
    "QTDCAgent32.exe"                              = "x86"

    "V1\VSTestVideoRecorder.exe"                   = "x86"
    "VideoRecorder\VSTestVideoRecorder.exe"        = "x86"
}

$errs = @()
Get-ChildItem  S:\p\vstest3\artifacts\packages\Debug\Shipping -Filter *.exe -Recurse -Force | ForEach-Object {
    $m = & "C:\Program Files\Microsoft Visual Studio\2022\IntPreview\VC\Tools\MSVC\14.38.32919\bin\HostX86\x86\dumpbin.exe" /headers $_.FullName | Select-String "machine \((.*)\)"
    if (-not $m.Matches.Success) {
        $err = "Did not find the platform of the exe $fullName)."
    }

    $platform = $m.Matches.Groups[1].Value
    $fullName = $_.FullName
    $name = $_.Name

    if ("x86" -eq $platform) { 
        $corFlags = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\CorFlags.exe"
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