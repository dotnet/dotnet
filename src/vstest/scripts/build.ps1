# Copyright (c) Microsoft. All rights reserved.
# Build script for Test Platform.

[CmdletBinding()]
Param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [Alias("c")]
    [System.String] $Configuration = "Debug",

    [Parameter(Mandatory = $false)]
    [Alias("r")]
    [System.String] $TargetRuntime = "win7-x64",

    # Versioning scheme = Major(15).Minor(RTW, Updates).SubUpdates(preview4, preview5, RC etc)
    # E.g. VS 2017 Update 1 Preview will have version 15.1.1
    [Parameter(Mandatory = $false)]
    [Alias("v")]
    [System.String] $Version, # Will set this later by reading TestPlatform.Settings.targets file.

    [Parameter(Mandatory = $false)]
    [Alias("vs")]
    [System.String] $VersionSuffix = "dev",

    [Parameter(Mandatory = $false)]
    [Alias("bn")]
    [System.String] $BuildNumber = "20991231-99",

    [Parameter(Mandatory = $false)]
    [Alias("ff")]
    [System.Boolean] $FailFast = $true,

    [Parameter(Mandatory = $false)]
    [Alias("noloc")]
    [Switch] $DisableLocalizedBuild,

    [Parameter(Mandatory = $false)]
    [Alias("ci")]
    [Switch] $CIBuild,

    [Parameter(Mandatory = $false)]
    [Alias("pt")]
    [Switch] $PublishTestArtifacts,

    # Build specific projects
    [Parameter(Mandatory = $false)]
    [Alias("p")]
    [System.String[]] $ProjectNamePatterns = @(),

    [Alias("f")]
    [Switch] $Force,

    [Alias("s")]
    [ValidateSet("InstallDotnet", "Restore", "UpdateLocalization", "Build", "Publish", "Pack", "Manifest", "PrepareAcceptanceTests")]
    [String[]] $Steps = @("InstallDotnet", "Restore", "UpdateLocalization", "Build", "Publish", "Pack", "Manifest", "PrepareAcceptanceTests")
)

$ErrorActionPreference = 'Stop'
$ErrorView = 'Normal'

. $PSScriptRoot\common.lib.ps1

# Set Version from scripts/build/TestPlatform.Settings.targets, when we are running locally and not providing the version as the parameter
# or when the build is done directly in VS
if ([string]::IsNullOrWhiteSpace($Version)) {
    $Version = ([xml](Get-Content $env:TP_ROOT_DIR\scripts\build\TestPlatform.Settings.targets)).Project.PropertyGroup[0].TPVersionPrefix |
    ForEach-Object { $_.Trim() } |
    Select-Object -First 1

    Write-Verbose "Version was not provided using version '$Version' from TestPlatform.Settings.targets"
}


#
# Build configuration
#
Write-Verbose "Setup build configuration."
$TPB_TestAssets = Join-Path $env:TP_ROOT_DIR "test\TestAssets\"
$TPB_Solution = Join-Path $env:TP_ROOT_DIR "TestPlatform.sln"
$TPB_TestAssets_Solution = Join-Path $TPB_TestAssets "TestAssets.sln"
$TPB_TestAssets_CILAssets = Join-Path $TPB_TestAssets "CILProject\CILProject.proj"
$TPB_TargetFramework462 = "net462"
$TPB_TargetFramework472 = "net472"
$TPB_TargetFramework48 = "net48"
$TPB_TargetFrameworkCore31 = "netcoreapp3.1"
$TPB_TargetFrameworkNS20 = "netstandard2.0"
$TPB_Configuration = $Configuration
$TPB_TargetRuntime = $TargetRuntime
$TPB_X64_Runtime = "win7-x64"
$TPB_X86_Runtime = "win7-x86"
$TPB_ARM64_Runtime = "win10-arm64"

# Version suffix is empty for RTM release
$TPB_Version = if ($VersionSuffix -ne '') { $Version + "-" + $VersionSuffix } else { $Version }
$TPB_CIBuild = $CIBuild
$TPB_PublishTests = $PublishTestArtifacts
$TPB_LocalizedBuild = !$DisableLocalizedBuild
$TPB_PackageOutDir = Join-Path $env:TP_OUT_DIR $TPB_Configuration\packages
$TPB_SourceBuildPackageOutDir = Join-Path $TPB_PackageOutDir "source-build"

$language = @("cs", "de", "es", "fr", "it", "ja", "ko", "pl", "pt-BR", "ru", "tr", "zh-Hans", "zh-Hant")

. "$($CurrentScriptDir.FullName)\verify-nupkgs.ps1"

# Update the version in the dependencies props to be the TPB_version version, this is not ideal but because changing how this is resolved would
# mean that we need to change the whole build process this is a solution with the least amount of impact, that does not require us to keep track of
# the version in multiple places
$dependenciesPath = "$env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props"
$dependencies = Get-Content -Raw -Encoding UTF8 $dependenciesPath
$updatedDependencies = $dependencies -replace "<NETTestSdkVersion>.*?</NETTestSdkVersion>", "<NETTestSdkVersion>$TPB_Version</NETTestSdkVersion>"
# PS7 considers utf8 to not have BOM, and utf8BOM needs to be used instead, while earlier versions use BOM with utf8 encoding
$encoding = if ($PSVersionTable.PSVersion.Major -ge 7) { "utf8BOM" } else { "utf8" }
$updatedDependencies | Set-Content -Encoding $encoding $dependenciesPath -NoNewline

$attachVsPath = "$env:TP_ROOT_DIR\src\AttachVS\bin\Debug\net472"

if ($env:PATH -notlike "*$attachVsPath") {
    Write-Log "Adding AttachVS to PATH"
    $env:PATH = "$attachVsPath;$env:PATH"
}

# VsixUtil gets regularly eaten by antivirus or something. Remove the package dir if it gets broken
# so nuget restores it correctly.
$vsSdkBuildToolsVersion = ([xml](Get-Content $env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.VSSdkBuildToolsVersion
$vsixUtilDir = "$env:TP_ROOT_DIR\packages\microsoft.vssdk.buildtools"
if ((Test-Path $vsixUtilDir) -and -not (Test-Path "$vsixUtilDir\$vsSdkBuildToolsVersion\tools\vssdk\bin\VsixUtil.exe")) {
    Remove-Item -Recurse -Force $vsixUtilDir
}

# Procdump gets regularly eaten by antivirus or something. Remove the package dir if it gets broken
# so nuget restores it correctly.
$procdumpDir = "$env:TP_ROOT_DIR\packages\procdump"
if ((Test-Path $procdumpDir) -and (Test-Path "$procdumpDir\0.0.1\bin") -and 2 -ne @(Get-Item "$procdumpDir\0.0.1\bin").Length) {
    Remove-Item -Recurse -Force $procdumpDir
}

function Invoke-Build {
    $timer = Start-Timer
    Write-Log "Invoke-Build: Start build."
    $dotnetExe = Get-DotNetPath

    Write-Log ".. .. Build: Source: $TPB_Solution"
    Invoke-Exe $dotnetExe -Arguments "build $TPB_Solution --configuration $TPB_Configuration -v:minimal -p:Version=$TPB_Version -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild -bl:""$env:TP_OUT_DIR\log\$Configuration\TestPlatform.binlog"""
    Write-Log ".. .. Build: Complete."

    Write-Log ".. .. Build: Source: $TPB_TestAssets_CILAssets"
    Invoke-Exe $dotnetExe -Arguments "build $TPB_TestAssets_CILAssets --configuration $TPB_Configuration -v:minimal -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild -bl:""$env:TP_OUT_DIR\log\$Configuration\CILAssets.binlog"""
    Write-Log ".. .. Build: Complete."
    Write-Log "Invoke-Build: Complete. {$(Get-ElapsedTime($timer))}"
}

function Invoke-TestAssetsBuild {
    $timer = Start-Timer
    Write-Log "Invoke-TestAssetsBuild: Start test assets build."
    $dotnetExe = Get-DotNetPath
    $nugetExe = Join-Path $env:TP_PACKAGES_DIR -ChildPath "Nuget.CommandLine" | Join-Path -ChildPath $env:NUGET_EXE_Version | Join-Path -ChildPath "tools\NuGet.exe"
    $nugetConfig = Join-Path $TPB_TestAssets "NuGet.config"

    Write-Log ".. .. Build: Source: $TPB_TestAssets_Solution"
    try {
        Write-Log ".. .. Build: Source: $TPB_TestAssets_Solution -- add NuGet source"
        Invoke-Exe -IgnoreExitCode 1 $nugetExe -Arguments "sources add -Name ""locally-built-testplatform-packages"" -Source $env:TP_TESTARTIFACTS\packages\ -ConfigFile ""$nugetConfig"""
        Invoke-Exe $dotnetExe -Arguments "build $TPB_TestAssets_Solution --configuration $TPB_Configuration -v:minimal -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild -bl:""$env:TP_OUT_DIR\log\$Configuration\TestAssets.binlog"""
    }
    finally {
        Write-Log ".. .. Build: Source: $TPB_TestAssets_Solution -- remove NuGet source"
        Invoke-Exe -IgnoreExitCode 1 $nugetExe -Arguments "sources remove -Name ""locally-built-testplatform-packages"" -ConfigFile ""$nugetConfig"""
    }
    Write-Log ".. .. Build: Complete."
    Write-Log "Invoke-TestAssetsBuild: Complete. {$(Get-ElapsedTime($timer))}"
}

function Invoke-CompatibilityTestAssetsBuild {
    # Compatibility matrix build.
    $dotnetExe = Get-DotNetPath
    Write-Log "Invoke-CompatibilityTestAssetsBuild: Start test assets build."
    $timer = Start-Timer
    $generated = Join-Path (Split-Path -Path $TPB_TestAssets) -ChildPath "GeneratedTestAssets"
    $generatedSln = Join-Path $generated "CompatibilityTestAssets.sln"

    # Figure out if the versions or the projects to build changed, and if they did not
    # and the solution is already in place just build it.
    # Otherwise delete everything and regenerate and re-build.
    $dependenciesPath = "$env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props"
    $dependenciesXml = [xml](Get-Content -Raw -Encoding UTF8 $dependenciesPath)

    $cacheId = [ordered]@{ }

    # Restore previous versions of TestPlatform (for vstest.console.exe), and TestPlatform.CLI (for vstest.console.dll).
    # These properties are coming from TestPlatform.Dependencies.props.
    $vstestConsoleVersionProperties = @(
        "VSTestConsoleLatestVersion"
        "VSTestConsoleLatestPreviewVersion"
        "VSTestConsoleLatestStableVersion"
        "VSTestConsoleRecentStableVersion"
        "VSTestConsoleMostDownloadedVersion"
        "VSTestConsolePreviousStableVersion"
        "VSTestConsoleLegacyStableVersion"
    )

    # Build with multiple versions of MSTest. The projects are directly in the root.
    # The folder structure in VS is not echoed in the TestAssets directory.
    $projects = @(
        "$env:TP_ROOT_DIR\test\TestAssets\MSTestProject1\MSTestProject1.csproj"
        "$env:TP_ROOT_DIR\test\TestAssets\MSTestProject2\MSTestProject2.csproj"
        # Don't use this one, it does not use the variables for mstest and test sdk.
        # "$env:TP_ROOT_DIR\test\TestAssets\SimpleTestProject2\SimpleTestProject2.csproj"
    )

    $msTestVersionProperties = @(
        "MSTestFrameworkLatestPreviewVersion"
        "MSTestFrameworkLatestStableVersion"
        "MSTestFrameworkRecentStableVersion"
        "MSTestFrameworkMostDownloadedVersion"
        "MSTestFrameworkPreviousStableVersion"
        "MSTestFrameworkLegacyStableVersion"
    )

    # We use the same version properties for NET.Test.Sdk as for VSTestConsole, for now.
    foreach ($sdkPropertyName in $vstestConsoleVersionProperties) {
        if ("VSTestConsoleLatestVersion" -eq $sdkPropertyName) {
            # NETTestSdkVersion has the version of the locally built package.
            $netTestSdkVersion = $dependenciesXml.Project.PropertyGroup."NETTestSdkVersion"
        }
        else {
            $netTestSdkVersion = $dependenciesXml.Project.PropertyGroup.$sdkPropertyName
        }

        if (-not $netTestSdkVersion) {
            throw "NetTestSdkVersion for $sdkPropertyName is empty."
        }

        $cacheId[$sdkPropertyName] = $netTestSdkVersion

            # We don't use the results of this build anywhere, we just use them to restore the packages to nuget cache
            # because using nuget.exe install errors out in various weird ways.
            Invoke-Exe $dotnetExe -Arguments "build $env:TP_ROOT_DIR\test\TestAssets\Tools\Tools.csproj --configuration $TPB_Configuration -v:minimal -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild -p:NETTestSdkVersion=$netTestSdkVersion"
    }

    foreach ($propertyName in $msTestVersionProperties) {
        $mstestVersion = $dependenciesXml.Project.PropertyGroup.$propertyName

        if (-not $mstestVersion) {
            throw "MSTestVersion for $propertyName is empty."
        }

        $cacheId[$propertyName] = $mstestVersion
    }

    $cacheId["projects"] = $projects

    $cacheIdText = $cacheId | ConvertTo-Json

    $currentCacheId = if (Test-Path "$generated/checksum.json") { Get-Content "$generated/checksum.json" -Raw }

    $rebuild = $true
    if ($cacheIdText -eq $currentCacheId) {
        if (Test-Path $generatedSln) {
            Write-Log ".. .. Build: Source: $generatedSln, cache is up to date, just building the solution."
            Invoke-Exe $dotnetExe -Arguments "build $generatedSln --configuration $TPB_Configuration -v:minimal -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild"
            $rebuild = $false
        }
    }

    if ($rebuild) {
        if (Test-Path $generated) {
            Remove-Item $generated -Recurse -Force
        }

        New-Item -ItemType Directory -Force -Path $generated | Out-Null

        Write-Log ".. .. Generate: Source: $generatedSln"
        $nugetExe = Join-Path $env:TP_PACKAGES_DIR -ChildPath "Nuget.CommandLine" | Join-Path -ChildPath $env:NUGET_EXE_Version | Join-Path -ChildPath "tools\NuGet.exe"
        $nugetConfigSource = Join-Path $TPB_TestAssets "NuGet.config"
        $nugetConfig = Join-Path $generated "NuGet.config"

        Invoke-Exe $dotnetExe -Arguments "new sln --name CompatibilityTestAssets --output ""$generated"""

        Write-Log ".. .. Build: Source: $generatedSln"
        try {
            $projectsToAdd = @()
            $nugetConfigSource = Join-Path $TPB_TestAssets "NuGet.config"
            $nugetConfig = Join-Path $generated "NuGet.config"

            Copy-Item -Path $nugetConfigSource -Destination $nugetConfig

            Write-Log ".. .. Build: Source: $generatedSln -- add NuGet source"
            Invoke-Exe -IgnoreExitCode 1 $nugetExe -Arguments "sources add -Name ""locally-built-testplatform-packages"" -Source $env:TP_TESTARTIFACTS\packages\ -ConfigFile ""$nugetConfig"""

            Write-Log ".. .. Build: Source: $generatedSln -- generate solution"
            foreach ($project in $projects) {
                $projectName = Split-Path -Path $project -Leaf
                $projectBaseName = [IO.Path]::GetFileNameWithoutExtension($projectName)
                $projectDir = Split-Path -Path $project
                $projectItems = Get-ChildItem $projectDir | Where-Object { $_.Name -notin "bin", "obj" } | ForEach-Object { if ($_.PsIsContainer) { Get-ChildItem $_ -Recurse -File } else { $_ } }

                Write-Log ".. .. .. Project $project has $($projectItems.Count) project items."
                # We use the same version properties for NET.Test.Sdk as for VSTestConsole, for now.
                foreach ($sdkPropertyName in $vstestConsoleVersionProperties) {
                    if ("VSTestConsoleLatestVersion" -eq $sdkPropertyName) {
                        # NETTestSdkVersion has the version of the locally built package.
                        $netTestSdkVersion = $dependenciesXml.Project.PropertyGroup."NETTestSdkVersion"
                    }
                    else {
                        $netTestSdkVersion = $dependenciesXml.Project.PropertyGroup.$sdkPropertyName
                    }

                    if (-not $netTestSdkVersion) {
                        throw "NetTestSdkVersion for $sdkPropertyName is empty."
                    }

                    $dirNetTestSdkVersion = $netTestSdkVersion -replace "\[|\]"
                    $dirNetTestSdkPropertyName = $sdkPropertyName -replace "Framework" -replace "Version" -replace "VSTestConsole", "NETTestSdk"

                    foreach ($propertyName in $msTestVersionProperties) {
                        $mstestVersion = $dependenciesXml.Project.PropertyGroup.$propertyName

                        if (-not $mstestVersion) {
                            throw "MSTestVersion for $propertyName is empty."
                        }

                        $dirMSTestVersion = $mstestVersion -replace "\[|\]"
                        $dirMSTestPropertyName = $propertyName -replace "Framework" -replace "Version"

                        # Do not make this a folder structure, it will break the relative reference to scripts\build\TestAssets.props that we have in the project,
                        # because the relative path will be different.
                        #
                        # It would be nice to use fully descriptive name but it is too long, hash the versions instead.
                        # $compatibilityProjectDir = "$generated/$projectBaseName--$dirNetTestSdkPropertyName-$dirNetTestSdkVersion--$dirMSTestPropertyName-$dirMSTestVersion"
                        $versions = "$dirNetTestSdkPropertyName-$dirNetTestSdkVersion--$dirMSTestPropertyName-$dirMSTestVersion"
                        $hash = Get-Hash $versions
                        Write-Host Hashed $versions to $hash
                        $projectShortName = "$projectBaseName--" + $hash
                        $compatibilityProjectDir = "$generated/$projectShortName"

                        if (Test-path $compatibilityProjectDir) {
                            throw "Path '$compatibilityProjectDir' does not exist"
                        }
                        New-Item -ItemType Directory -Path $compatibilityProjectDir | Out-Null
                        $compatibilityProjectDir = Resolve-Path $compatibilityProjectDir
                        foreach ($projectItem in $projectItems) {
                            $relativePath = ($projectItem.FullName -replace [regex]::Escape($projectDir)).TrimStart("\")
                            $fullPath = Join-Path $compatibilityProjectDir $relativePath
                            try {
                                Copy-Item -Path $projectItem.FullName -Destination $fullPath -Verbose
                            }
                            catch {
                                # can throw on wrong path, this makes the error more verbose
                                throw "$_, Source: '$($projectItem.FullName)', Destination: '$fullPath'"
                            }
                        }

                        $compatibilityCsproj = Get-ChildItem -Path $compatibilityProjectDir -Filter *.csproj
                        if (-not $compatibilityCsproj) {
                            throw "No .csproj files found in directory $compatibilityProjectDir."
                        }

                        $compatibilityCsproj = $compatibilityCsproj.FullName
                        $csprojContent = (Get-Content $compatibilityCsproj -Encoding UTF8) `
                            -replace "\$\(MSTestFrameworkVersion\)", $mstestVersion `
                            -replace "\$\(MSTestAdapterVersion\)", $mstestVersion `
                            -replace "\$\(NETTestSdkVersion\)", $netTestSdkVersion
                        $csprojContent | Set-Content -Encoding UTF8 -Path $compatibilityCsproj -Force

                        $uniqueCsprojName = Join-Path $compatibilityProjectDir "$projectShortName.csproj"
                        Rename-Item $compatibilityCsproj $uniqueCsprojName
                        $projectsToAdd += $uniqueCsprojName

                        Write-Log ".. .. .. Generated: $uniqueCsprojName"
                    }
                }
            }

            Write-Log ".. .. .. Building: generatedSln"
            Invoke-Exe $dotnetExe -Arguments "sln $generatedSln add $projectsToAdd"
            Invoke-Exe $dotnetExe -Arguments "build $generatedSln --configuration $TPB_Configuration -v:minimal -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild"
            $cacheIdText | Set-Content "$generated/checksum.json" -NoNewline
        }
        finally {
            Write-Log ".. .. Build: Source: $TPB_TestAssets_Solution -- remove NuGet source"
            Invoke-Exe -IgnoreExitCode 1 $nugetExe -Arguments "sources remove -Name ""locally-built-testplatform-packages"" -ConfigFile ""$nugetConfig"""
        }
    }
    Write-Log ".. .. Build: Complete."
    Write-Log "Invoke-CompatibilityTestAssetsBuild: Complete. {$(Get-ElapsedTime($timer))}"
}

function Copy-PackageIntoStaticDirectory {
    # packages are published into folder based on configuration, but
    # nuget does not understand that, and does not support wildcards in paths
    # in order to be able to use the produced packages for acceptance tests we
    # need to put them in folder that is not changing it's name based on config
    $tpPackagesPath = "$env:TP_OUT_DIR\$TPB_Configuration\packages\"
    $tpPackagesDestination = "$env:TP_TESTARTIFACTS"
    New-Item -ItemType Directory -Force $tpPackagesDestination | Out-Null
    Copy-Item $tpPackagesPath $tpPackagesDestination -Force -Filter *.nupkg -Verbose -Recurse
}

function Publish-PatchedDotnet {
    Write-Log "Publish-PatchedDotnet: Copy local dotnet installation to testArtifacts"
    $dotnetPath = "$env:TP_TOOLS_DIR\dotnet\"

    $dotnetTestArtifactsPath = "$env:TP_TESTARTIFACTS\dotnet\"

    if (Test-Path $dotnetTestArtifactsPath) {
        Remove-Item -Force -Recurse $dotnetTestArtifactsPath
    }

    $dotnetTestArtifactsSdkPath = "$env:TP_TESTARTIFACTS\dotnet\sdk\$env:DOTNET_CLI_VERSION\"
    Copy-Item $dotnetPath $dotnetTestArtifactsPath -Force -Recurse

    Write-Log "Publish-PatchedDotnet: Copy VSTest task artifacts to local dotnet installation to allow `dotnet test` to run with it"
    $buildArtifactsPath = "$env:TP_ROOT_DIR\src\Microsoft.TestPlatform.Build\bin\$TPB_Configuration\$TPB_TargetFrameworkNS20\*"
    Copy-Item $buildArtifactsPath $dotnetTestArtifactsSdkPath -Force
}

function Publish-Package {
    $timer = Start-Timer
    Write-Log "Publish-Package: Started."
    $net462PackageDir = Get-FullCLR462PackageDirectory
    $netstandard20PackageDir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\$TPB_TargetFrameworkNS20");
    $coreCLR31PackageDir = Get-CoreCLR31PackageDirectory
    $coreClrNetFrameworkTestHostDir = Join-Path $coreCLR31PackageDir "TestHostNetFramework"
    $packageProject = Join-Path $env:TP_PACKAGE_PROJ_DIR "package\package.csproj"
    $testHostProject = Join-Path $env:TP_ROOT_DIR "src\testhost\testhost.csproj"
    $testHostx86Project = Join-Path $env:TP_ROOT_DIR "src\testhost.x86\testhost.x86.csproj"
    $testHostarm64Project = Join-Path $env:TP_ROOT_DIR "src\testhost.arm64\testhost.arm64.csproj"

    $testhostFullPackageDir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Microsoft.TestPlatform.TestHost\$TPB_TargetFramework462\$TPB_TargetRuntime")
    $testhostCore31PackageDir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Microsoft.TestPlatform.TestHost\$TPB_TargetFrameworkCore31")
    $testhostCore31PackageX64Dir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Microsoft.TestPlatform.TestHost\$TPB_TargetFrameworkCore31\$TPB_X64_Runtime")
    $testhostCore31PackageX86Dir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Microsoft.TestPlatform.TestHost\$TPB_TargetFrameworkCore31\$TPB_X86_Runtime")
    $testhostCore31PackageARM64Dir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Microsoft.TestPlatform.TestHost\$TPB_TargetFrameworkCore31\$TPB_ARM64_Runtime")
    $testhostCore31PackageTempX64Dir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\publishTemp\Microsoft.TestPlatform.TestHost\$TPB_TargetFrameworkCore31\$TPB_X64_Runtime")
    $testhostCore31PackageTempX86Dir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\publishTemp\Microsoft.TestPlatform.TestHost\$TPB_TargetFrameworkCore31\$TPB_X86_Runtime")
    $testhostCore31PackageTempARM64Dir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\publishTemp\Microsoft.TestPlatform.TestHost\$TPB_TargetFrameworkCore31\$TPB_ARM64_Runtime")

    $vstestConsoleProject = Join-Path $env:TP_ROOT_DIR "src\vstest.console\vstest.console.csproj"
    $settingsMigratorProject = Join-Path $env:TP_ROOT_DIR "src\SettingsMigrator\SettingsMigrator.csproj"
    $dataCollectorProject = Join-Path $env:TP_ROOT_DIR "src\datacollector\datacollector.csproj"

    Write-Log "Package: Publish src\package\package\package.csproj"
    Publish-PackageInternal $packageProject $TPB_TargetFramework462 $net462PackageDir
    Publish-PackageInternal $packageProject $TPB_TargetFrameworkCore31 $coreCLR31PackageDir


    ################################################################################
    # Publish vstest.console and datacollector exclusively because *.config/*.deps.json file is not getting publish when we are publishing aforementioned project through dependency.
    Write-Log "Package: Publish src\vstest.console\vstest.console.csproj"

    # We build vstest.console.arm64.exe before building vstest.console.exe and we put it in the same folder, so they end up shipping together.
    Publish-PackageWithRuntimeInternal $vstestConsoleProject $TPB_TargetFramework462 $TPB_ARM64_Runtime false $net462PackageDir
    Publish-PackageWithRuntimeInternal $vstestConsoleProject $TPB_TargetFramework462 $TPB_X64_Runtime false $net462PackageDir
    Publish-PackageInternal $vstestConsoleProject $TPB_TargetFrameworkCore31 $coreCLR31PackageDir

    Write-Log "Package: Publish src\SettingsMigrator\SettingsMigrator.csproj"
    Publish-PackageInternal $settingsMigratorProject $TPB_TargetFramework462 $net462PackageDir

    Write-Log "Package: Publish src\datacollector\datacollector.csproj"
    # We build datacollector.arm64.exe before building datacollector.exe and we put it in the same folder, so they end up shipping together.
    Publish-PackageWithRuntimeInternal $dataCollectorProject $TPB_TargetFramework472 $TPB_ARM64_Runtime false $net462PackageDir
    Publish-PackageWithRuntimeInternal $dataCollectorProject $TPB_TargetFramework472 $TPB_X64_Runtime false $net462PackageDir
    Publish-PackageInternal $dataCollectorProject $TPB_TargetFrameworkCore31 $coreCLR31PackageDir

    ################################################################################
    # Publish testhost
    Write-Log "Package: Publish testhost\testhost.csproj"
    Publish-PackageInternal $testHostProject $TPB_TargetFramework462 $testhostFullPackageDir
    Publish-PackageInternal $testHostProject $TPB_TargetFrameworkCore31 $testhostCore31PackageDir
    Publish-PackageWithRuntimeInternal $testHostProject $TPB_TargetFrameworkCore31 $TPB_X64_Runtime false $testhostCore31PackageTempX64Dir

    Write-Log "Package: Publish testhost.x86\testhost.x86.csproj"
    Publish-PackageInternal $testHostx86Project $TPB_TargetFramework462 $testhostFullPackageDir
    Publish-PackageWithRuntimeInternal $testHostx86Project $TPB_TargetFrameworkCore31 $TPB_X86_Runtime false $testhostCore31PackageTempX86Dir

    Write-Log "Package: Publish testhost.arm64\testhost.arm64.csproj"
    Publish-PackageInternal $testHostarm64Project $TPB_TargetFramework462 $testhostFullPackageDir
    Publish-PackageWithRuntimeInternal $testHostarm64Project $TPB_TargetFrameworkCore31 $TPB_ARM64_Runtime false $testhostCore31PackageTempARM64Dir

    # Copy the .NET multitarget testhost exes to destination folder (except for net462 which is the default)
    foreach ($tfm in "net47;net471;net472;net48" -split ";") {
        # testhost
        Copy-Item "$(Split-Path $testHostProject)\bin\$TPB_Configuration\$tfm\$TPB_X64_Runtime\testhost.$tfm.exe" $testhostFullPackageDir\testhost.$tfm.exe -Force
        Copy-Item "$(Split-Path $testHostProject)\bin\$TPB_Configuration\$tfm\$TPB_X64_Runtime\testhost.$tfm.pdb" $testhostFullPackageDir\testhost.$tfm.pdb -Force
        Copy-Item "$(Split-Path $testHostProject)\bin\$TPB_Configuration\$tfm\$TPB_X64_Runtime\testhost.$tfm.exe.config" $testhostFullPackageDir\testhost.$tfm.exe.config -Force
        # testhost.x86
        Copy-Item "$(Split-Path $testHostx86Project)\bin\$TPB_Configuration\$tfm\$TPB_X86_Runtime\testhost.$tfm.x86.exe" $testhostFullPackageDir\testhost.$tfm.x86.exe -Force
        Copy-Item "$(Split-Path $testHostx86Project)\bin\$TPB_Configuration\$tfm\$TPB_X86_Runtime\testhost.$tfm.x86.pdb" $testhostFullPackageDir\testhost.$tfm.x86.pdb -Force
        Copy-Item "$(Split-Path $testHostx86Project)\bin\$TPB_Configuration\$tfm\$TPB_X86_Runtime\testhost.$tfm.x86.exe.config" $testhostFullPackageDir\testhost.$tfm.x86.exe.config -Force
        # testhost.arm64
        Copy-Item "$(Split-Path $testHostarm64Project)\bin\$TPB_Configuration\$tfm\$TPB_ARM64_Runtime\testhost.$tfm.arm64.exe" $testhostFullPackageDir\testhost.$tfm.arm64.exe -Force
        Copy-Item "$(Split-Path $testHostarm64Project)\bin\$TPB_Configuration\$tfm\$TPB_ARM64_Runtime\testhost.$tfm.arm64.pdb" $testhostFullPackageDir\testhost.$tfm.arm64.pdb -Force
        Copy-Item "$(Split-Path $testHostarm64Project)\bin\$TPB_Configuration\$tfm\$TPB_ARM64_Runtime\testhost.$tfm.arm64.exe.config" $testhostFullPackageDir\testhost.$tfm.arm64.exe.config -Force
    }

    # Copy the .NET core x86, x64 and arm64 testhost exes from tempPublish to required folder
    New-Item -ItemType directory -Path $testhostCore31PackageX64Dir -Force | Out-Null
    Copy-Item $testhostCore31PackageTempX64Dir\testhost* $testhostCore31PackageX64Dir -Force -Recurse
    Copy-Item $testhostCore31PackageTempX64Dir\Microsoft.TestPlatform.PlatformAbstractions.dll $testhostCore31PackageX64Dir -Force

    New-Item -ItemType directory -Path $testhostCore31PackageX86Dir -Force | Out-Null
    Copy-Item $testhostCore31PackageTempX86Dir\testhost.x86* $testhostCore31PackageX86Dir -Force -Recurse
    Copy-Item $testhostCore31PackageTempX86Dir\Microsoft.TestPlatform.PlatformAbstractions.dll $testhostCore31PackageX86Dir -Force

    New-Item -ItemType directory -Path $testhostCore31PackageARM64Dir -Force | Out-Null
    Copy-Item $testhostCore31PackageTempARM64Dir\testhost.arm64* $testhostCore31PackageARM64Dir -Force -Recurse
    Copy-Item $testhostCore31PackageTempARM64Dir\Microsoft.TestPlatform.PlatformAbstractions.dll $testhostCore31PackageARM64Dir -Force

    # Copy over the Full CLR built testhost package assemblies to the Core CLR and Full CLR package folder.
    New-Item -ItemType directory -Path $coreClrNetFrameworkTestHostDir -Force | Out-Null
    Copy-Item $testhostFullPackageDir\* $coreClrNetFrameworkTestHostDir -Force -Recurse

    # Copy over the Full CLR built datacollector package assemblies to the Core CLR package folder along with testhost
    Publish-PackageWithRuntimeInternal $dataCollectorProject $TPB_TargetFramework472 $TPB_ARM64_Runtime false $coreClrNetFrameworkTestHostDir
    Publish-PackageWithRuntimeInternal $dataCollectorProject $TPB_TargetFramework472 $TPB_X64_Runtime false $coreClrNetFrameworkTestHostDir

    New-Item -ItemType directory -Path $net462PackageDir -Force | Out-Null
    Copy-Item $testhostFullPackageDir\* $net462PackageDir -Force -Recurse

    ################################################################################
    # Publish Microsoft.TestPlatform.ObjectModel
    Copy-Bulk -root (Join-Path $env:TP_ROOT_DIR "src\Microsoft.TestPlatform.ObjectModel\bin\$TPB_Configuration") `
        -files @{
        $TPB_TargetFramework462    = $net462PackageDir              # net462
        $TPB_TargetFrameworkCore31 = $coreCLR31PackageDir           # netcoreapp3.1
        $TPB_TargetFrameworkNS20   = $netstandard20PackageDir       # netstandard2_0
    }

    ################################################################################
    # Publish Microsoft.TestPlatform.PlatformAbstractions
    Copy-Bulk -root (Join-Path $env:TP_ROOT_DIR "src\Microsoft.TestPlatform.PlatformAbstractions\bin\$TPB_Configuration") `
        -files @{
        $TPB_TargetFramework462    = $net462PackageDir             # net462
        $TPB_TargetFrameworkCore31 = $coreCLR31PackageDir          # netcoreapp3.1
        $TPB_TargetFrameworkNS20   = $netstandard20PackageDir      # netstandard2_0
    }

    ################################################################################
    # Publish Microsoft.TestPlatform.CoreUtilities
    Copy-Bulk -root (Join-Path $env:TP_ROOT_DIR "src\Microsoft.TestPlatform.CoreUtilities\bin\$TPB_Configuration") `
        -files @{
        $TPB_TargetFramework462    = $net462PackageDir              # net462
        $TPB_TargetFrameworkNS20   = $netstandard20PackageDir       # netstandard2_0
    }

    ################################################################################
    # Publish Microsoft.TestPlatform.AdapterUtilities
    Copy-Bulk -root (Join-Path $env:TP_ROOT_DIR "src\Microsoft.TestPlatform.AdapterUtilities\bin\$TPB_Configuration") `
        -files @{
        "$TPB_TargetFramework462/any"   = $net462PackageDir             # net462
        $TPB_TargetFrameworkNS20        = $netstandard20PackageDir      # netstandard2_0
    }

    ################################################################################
    # Publish Microsoft.TestPlatform.CrossPlatEngine
    Copy-Bulk -root (Join-Path $env:TP_ROOT_DIR "src\Microsoft.TestPlatform.CrossPlatEngine\bin\$TPB_Configuration") `
        -files @{
        $TPB_TargetFrameworkNS20 = $netstandard20PackageDir       # netstandard2_0
    }

    ################################################################################
    # Publish msdia
    $testPlatformMsDiaVersion = ([xml](Get-Content $env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.TestPlatformMSDiaVersion
    $comComponentsDirectory = Join-Path $env:TP_PACKAGES_DIR "Microsoft.Internal.Dia\$testPlatformMsDiaVersion\tools\net451"
    Copy-Item -Recurse $comComponentsDirectory\* $testhostCore31PackageDir -Force
    Copy-Item -Recurse $comComponentsDirectory\* $testhostFullPackageDir -Force
    Copy-Item -Recurse $comComponentsDirectory\* $coreClrNetFrameworkTestHostDir -Force

    # Copy over the logger assemblies to the Extensions folder.
    $extensions_Dir = "Extensions"
    $fullCLRExtensionsDir = Join-Path $net462PackageDir $extensions_Dir
    $coreCLRExtensionsDir = Join-Path $coreCLR31PackageDir $extensions_Dir

    # Create an extensions directory.
    New-Item -ItemType directory -Path $fullCLRExtensionsDir -Force | Out-Null
    New-Item -ItemType directory -Path $coreCLRExtensionsDir -Force | Out-Null

    # If there are some dependencies for the logger assemblies, those need to be moved too.
    # Ideally we should just be publishing the loggers to the Extensions folder.
    $loggers = @(
        "Microsoft.VisualStudio.TestPlatform.Extensions.Trx.TestLogger.dll", "Microsoft.VisualStudio.TestPlatform.Extensions.Trx.TestLogger.pdb",
        "Microsoft.VisualStudio.TestPlatform.Extensions.Html.TestLogger.dll", "Microsoft.VisualStudio.TestPlatform.Extensions.Html.TestLogger.pdb"
    )

    foreach ($file in $loggers) {
        Write-Verbose "Move-Item $net462PackageDir\$file $fullCLRExtensionsDir -Force"
        Move-Item $net462PackageDir\$file $fullCLRExtensionsDir -Force

        Write-Verbose "Move-Item $coreCLR31PackageDir\$file $coreCLRExtensionsDir -Force"
        Move-Item $coreCLR31PackageDir\$file $coreCLRExtensionsDir -Force
    }

    # Move logger resource dlls
    if ($TPB_LocalizedBuild) {
        Move-Loc-Files $net462PackageDir $fullCLRExtensionsDir "Microsoft.VisualStudio.TestPlatform.Extensions.Trx.TestLogger.resources.dll"
        Move-Loc-Files $coreCLR31PackageDir $coreCLRExtensionsDir "Microsoft.VisualStudio.TestPlatform.Extensions.Trx.TestLogger.resources.dll"
        Move-Loc-Files $net462PackageDir $fullCLRExtensionsDir "Microsoft.VisualStudio.TestPlatform.Extensions.Html.TestLogger.resources.dll"
        Move-Loc-Files $coreCLR31PackageDir $coreCLRExtensionsDir "Microsoft.VisualStudio.TestPlatform.Extensions.Html.TestLogger.resources.dll"
    }

    # Copy Blame Datacollector to Extensions folder.
    $TPB_TargetFrameworkStandard = "netstandard2.0"
    $blameDataCollector = Join-Path $env:TP_ROOT_DIR "src\Microsoft.TestPlatform.Extensions.BlameDataCollector\bin\$TPB_Configuration"
    $blameDataCollectorNetFull = Join-Path $blameDataCollector $TPB_TargetFramework472
    $blameDataCollectorNetStandard = Join-Path $blameDataCollector $TPB_TargetFrameworkStandard
    New-Item -ItemType Directory "$fullCLRExtensionsDir/dump" -Force | Out-Null
    Copy-Item $blameDataCollectorNetFull\Microsoft.TestPlatform.Extensions.BlameDataCollector.dll $fullCLRExtensionsDir -Force
    Copy-Item $blameDataCollectorNetFull\Microsoft.TestPlatform.Extensions.BlameDataCollector.pdb $fullCLRExtensionsDir -Force
    Copy-Item $blameDataCollectorNetFull\DumpMinitool.exe "$fullCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetFull\DumpMinitool.pdb "$fullCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetFull\DumpMinitool.exe.config "$fullCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetFull\DumpMinitool.x86.exe "$fullCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetFull\DumpMinitool.x86.pdb "$fullCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetFull\DumpMinitool.x86.exe.config "$fullCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetFull\DumpMinitool.arm64.exe "$fullCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetFull\DumpMinitool.arm64.pdb "$fullCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetFull\DumpMinitool.arm64.exe.config "$fullCLRExtensionsDir/dump" -Force

    New-Item -ItemType Directory "$coreCLRExtensionsDir/dump" -Force | Out-Null
    Copy-Item $blameDataCollectorNetStandard\Microsoft.TestPlatform.Extensions.BlameDataCollector.dll $coreCLRExtensionsDir -Force
    Copy-Item $blameDataCollectorNetStandard\Microsoft.TestPlatform.Extensions.BlameDataCollector.pdb $coreCLRExtensionsDir -Force
    Copy-Item $blameDataCollectorNetStandard\DumpMinitool.exe "$coreCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetStandard\DumpMinitool.pdb "$coreCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetStandard\DumpMinitool.exe.config "$coreCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetStandard\DumpMinitool.x86.exe "$coreCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetStandard\DumpMinitool.x86.pdb "$coreCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetStandard\DumpMinitool.x86.exe.config "$coreCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetStandard\DumpMinitool.arm64.exe "$coreCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetStandard\DumpMinitool.arm64.pdb "$coreCLRExtensionsDir/dump" -Force
    Copy-Item $blameDataCollectorNetStandard\DumpMinitool.arm64.exe.config "$coreCLRExtensionsDir/dump" -Force

    # we use this to dump processes on netcore
    Copy-Item $blameDataCollectorNetStandard\Microsoft.Diagnostics.NETCore.Client.dll $coreCLRExtensionsDir -Force

    # $null = New-Item -Force "$fullCLRExtensionsDir\procdump" -ItemType Directory
    # $null = New-Item -Force "$coreCLRExtensionsDir\procdump" -ItemType Directory
    # Copy-Item $blameDataCollectorNetFull\procdump.exe $fullCLRExtensionsDir\procdump -Force
    # Copy-Item $blameDataCollectorNetFull\procdump64.exe $fullCLRExtensionsDir\procdump -Force
    # Copy-Item $blameDataCollectorNetStandard\procdump.exe $coreCLRExtensionsDir\procdump -Force
    # Copy-Item $blameDataCollectorNetStandard\procdump64.exe $coreCLRExtensionsDir\procdump -Force
    # Copy-Item $blameDataCollectorNetStandard\procdump $coreCLRExtensionsDir\procdump -Force

    # Copy blame data collector resource dlls
    if ($TPB_LocalizedBuild) {
        Copy-Loc-Files $blameDataCollectorNetFull $fullCLRExtensionsDir "Microsoft.TestPlatform.Extensions.BlameDataCollector.resources.dll"
        Copy-Loc-Files $blameDataCollectorNetStandard $coreCLRExtensionsDir "Microsoft.TestPlatform.Extensions.BlameDataCollector.resources.dll"
    }

    # Copy Event Log Datacollector to Extensions folder.
    $eventLogDataCollector = Join-Path $env:TP_ROOT_DIR "src\DataCollectors\Microsoft.TestPlatform.Extensions.EventLogCollector\bin\$TPB_Configuration"
    $eventLogDataCollectorNetFull = Join-Path $eventLogDataCollector $TPB_TargetFramework462
    Copy-Item $eventLogDataCollectorNetFull\Microsoft.TestPlatform.Extensions.EventLogCollector.dll $fullCLRExtensionsDir -Force
    Copy-Item $eventLogDataCollectorNetFull\Microsoft.TestPlatform.Extensions.EventLogCollector.pdb $fullCLRExtensionsDir -Force
    Copy-Item $eventLogDataCollectorNetFull\Microsoft.TestPlatform.Extensions.EventLogCollector.dll $coreCLRExtensionsDir -Force
    Copy-Item $eventLogDataCollectorNetFull\Microsoft.TestPlatform.Extensions.EventLogCollector.pdb $coreCLRExtensionsDir -Force

    # Copy EventLogCollector resource dlls
    if ($TPB_LocalizedBuild) {
        Copy-Loc-Files $eventLogDataCollectorNetFull $fullCLRExtensionsDir "Microsoft.TestPlatform.Extensions.EventLogCollector.resources.dll"
        Copy-Loc-Files $eventLogDataCollectorNetFull $coreCLRExtensionsDir "Microsoft.TestPlatform.Extensions.EventLogCollector.resources.dll"
    }

    # Copy Microsoft.CodeCoverage.IO dlls
    $codeCoverageExternalsVersion = ([xml](Get-Content $env:TP_ROOT_DIR\eng\Versions.props)).Project.PropertyGroup.MicrosoftInternalCodeCoverageVersion
    $codeCoverageIOPackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.codecoverage.io\$codeCoverageExternalsVersion\lib\$TPB_TargetFrameworkStandard"
    Copy-Item $codeCoverageIOPackagesDir\Microsoft.CodeCoverage.IO.dll $coreCLR31PackageDir -Force
    if ($TPB_LocalizedBuild) {
        Copy-Loc-Files $codeCoverageIOPackagesDir $coreCLR31PackageDir "Microsoft.CodeCoverage.IO.resources.dll"
    }

    # If there are some dependencies for the TestHostRuntimeProvider assemblies, those need to be moved too.
    $runtimeproviders = @("Microsoft.TestPlatform.TestHostRuntimeProvider.dll", "Microsoft.TestPlatform.TestHostRuntimeProvider.pdb")
    foreach ($file in $runtimeproviders) {
        Write-Verbose "Move-Item $net462PackageDir\$file $fullCLRExtensionsDir -Force"
        Move-Item $net462PackageDir\$file $fullCLRExtensionsDir -Force

        Write-Verbose "Move-Item $coreCLR31PackageDir\$file $coreCLRExtensionsDir -Force"
        Move-Item $coreCLR31PackageDir\$file $coreCLRExtensionsDir -Force
    }

    # Move TestHostRuntimeProvider resource dlls
    if ($TPB_LocalizedBuild) {
        Move-Loc-Files $net462PackageDir $fullCLRExtensionsDir "Microsoft.TestPlatform.TestHostRuntimeProvider.resources.dll"
        Move-Loc-Files $coreCLR31PackageDir $coreCLRExtensionsDir "Microsoft.TestPlatform.TestHostRuntimeProvider.resources.dll"
    }

    # Copy dependency of Microsoft.TestPlatform.TestHostRuntimeProvider
    $newtonsoftJsonVersion = ([xml](Get-Content $env:TP_ROOT_DIR\eng\Versions.props)).Project.PropertyGroup.NewtonsoftJsonVersion
    $newtonsoft = Join-Path $env:TP_PACKAGES_DIR "newtonsoft.json\$newtonsoftJsonVersion\lib\net45\Newtonsoft.Json.dll"
    Write-Verbose "Copy-Item $newtonsoft $net462PackageDir -Force"
    Copy-Item $newtonsoft $net462PackageDir -Force

    $newtonsoft = Join-Path $env:TP_PACKAGES_DIR "newtonsoft.json\$newtonsoftJsonVersion\lib\netstandard2.0\Newtonsoft.Json.dll"
    Write-Verbose "Copy-Item $newtonsoft $coreCLR31PackageDir -Force"
    Copy-Item $newtonsoft $coreCLR31PackageDir -Force

    # Copy .NET Standard CPP Test adapter
    $fullClrNetTestHostDir = "$net462PackageDir\TestHostNet"
    New-Item $fullClrNetTestHostDir -ItemType Directory -Force | Out-Null

    $testPlatformRemoteExternalsVersion = ([xml](Get-Content "$env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props")).Project.PropertyGroup.TestPlatformRemoteExternalsVersion
    $testPlatformRemoteExternalsSourceDirectory = Join-Path $env:TP_PACKAGES_DIR "Microsoft.Internal.TestPlatform.Remote\$testPlatformRemoteExternalsVersion\tools\netstandard\Extensions\*"
    Copy-Item $testPlatformRemoteExternalsSourceDirectory $coreCLR31PackageDir -Force -Recurse
    Copy-Item $testPlatformRemoteExternalsSourceDirectory $fullClrNetTestHostDir -Force -Recurse

    # Copy standalone testhost
    $standaloneTesthost = Join-Path $env:TP_ROOT_DIR "temp\testhost\*"
    Copy-Item $standaloneTesthost $coreCLR31PackageDir -Force
    Copy-Item $testhostCore31PackageDir\testhost.dll $coreCLR31PackageDir -Force
    Copy-Item $testhostCore31PackageDir\testhost.pdb $coreCLR31PackageDir -Force

    Get-Item "$testhostCore31PackageDir\*" |
    Where-Object { $_.Name -notin ("x64", "x86", "win7-x64", "win7-x86", "testhost.deps.json", "testhost.runtimeconfig.json") } |
    Copy-Item -Recurse -Destination $fullClrNetTestHostDir -Force
    Copy-Item $standaloneTesthost $fullClrNetTestHostDir -Force

    # For libraries that are externally published, copy the output into artifacts. These will be signed and packaged independently.
    Copy-PackageItems "Microsoft.TestPlatform.Build"

    # Copy IntelliTrace components.
    $testPlatformExternalsVersion = ([xml](Get-Content $env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.TestPlatformExternalsVersion
    $intellitraceSourceDirectory = Join-Path $env:TP_PACKAGES_DIR "Microsoft.Internal.Intellitrace\$testPlatformExternalsVersion\tools\net451"
    $intellitraceTargetDirectory = Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Intellitrace"

    if (-not (Test-Path $intellitraceTargetDirectory)) {
        New-Item $intellitraceTargetDirectory -Type Directory -Force | Out-Null
    }

    Copy-Item -Recurse $intellitraceSourceDirectory\* $intellitraceTargetDirectory -Force

    # Copy IntelliTrace Extensions components.
    $intellitraceExtensionsSourceDirectory = Join-Path $env:TP_PACKAGES_DIR "Microsoft.Internal.Intellitrace.Extensions\$testPlatformExternalsVersion\tools\net451"

    if (-not (Test-Path $intellitraceExtensionsSourceDirectory)) {
        New-Item $intellitraceExtensionsSourceDirectory -Type Directory -Force | Out-Null
    }

    Copy-Item -Recurse $intellitraceExtensionsSourceDirectory\* $intellitraceTargetDirectory -Force

    # Copy Microsoft.VisualStudio.Telemetry APIs
    $testPlatformDirectory = Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Intellitrace\Common7\IDE\Extensions\TestPlatform"

    if (-not (Test-Path $testPlatformDirectory)) {
        New-Item $testPlatformDirectory -Type Directory -Force | Out-Null
    }

    $visualStudioTelemetryVersion = ([xml](Get-Content $env:TP_ROOT_DIR\eng\Versions.props)).Project.PropertyGroup.MicrosoftVisualStudioTelemetryVersion
    $visualStudioRemoteControlVersion = ([xml](Get-Content $env:TP_ROOT_DIR\eng\Versions.props)).Project.PropertyGroup.MicrosoftVisualStudioRemoteControlVersion
    $visualStudioUtilitiesInternalVersion = ([xml](Get-Content $env:TP_ROOT_DIR\eng\Versions.props)).Project.PropertyGroup.MicrosoftVisualStudioUtilitiesInternalVersion
    $visualStudioTelemetryDirectory = Join-Path $env:TP_PACKAGES_DIR "Microsoft.VisualStudio.Telemetry\$visualStudioTelemetryVersion\lib\net45"
    $visualStudioRemoteControl = Join-Path $env:TP_PACKAGES_DIR "Microsoft.VisualStudio.RemoteControl\$visualStudioRemoteControlVersion\lib\net45"
    $visualStudioUtilitiesDirectory = Join-Path $env:TP_PACKAGES_DIR "Microsoft.VisualStudio.Utilities.Internal\$visualStudioUtilitiesInternalVersion\lib\net45"

    Copy-Item "$visualStudioTelemetryDirectory\Microsoft.VisualStudio.Telemetry.dll" $testPlatformDirectory -Force
    Copy-Item "$visualStudioRemoteControl\Microsoft.VisualStudio.RemoteControl.dll" $testPlatformDirectory -Force
    Copy-Item "$visualStudioUtilitiesDirectory\Microsoft.VisualStudio.Utilities.Internal.dll" $testPlatformDirectory -Force

    Copy-CodeCoverage-Package-Artifacts

    Write-Log "Publish-Package: Complete. {$(Get-ElapsedTime($timer))}"
}

function Publish-Tests {
    if ($TPB_PublishTests) {
        Write-Log "Publish-Tests: Started."

        # Adding only Perf project for now
        $fullCLRTestDir = Join-Path $env:TP_TESTARTIFACTS "$TPB_Configuration\$TPB_TargetFramework462"
        $fullCLRPerfTestAssetDir = Join-Path $env:TP_TESTARTIFACTS "$TPB_Configuration\$TPB_TargetFramework462\TestAssets\PerfAssets"

        $mstest10kPerfProjectDir = Join-Path $fullCLRPerfTestAssetDir "MSTest10kPassing"
        $mstest10kPerfProject = Join-Path $env:TP_ROOT_DIR "test\TestAssets\PerfAssets\MSTest10kPassing"
        Publish-PackageInternal $mstest10kPerfProject $TPB_TargetFramework462 $mstest10kPerfProjectDir

        $nunittest10kPerfProjectDir = Join-Path $fullCLRPerfTestAssetDir "NUnit10kPassing"
        $nunittest10kPerfProject = Join-Path $env:TP_ROOT_DIR "test\TestAssets\PerfAssets\NUnit10kPassing"
        Publish-PackageInternal $nunittest10kPerfProject $TPB_TargetFramework462 $nunittest10kPerfProjectDir

        $xunittest10kPerfProjectDir = Join-Path $fullCLRPerfTestAssetDir "XUnit10kPassing"
        $xunittest10kPerfProject = Join-Path $env:TP_ROOT_DIR "test\TestAssets\PerfAssets\XUnit10kPassing"
        Publish-PackageInternal $xunittest10kPerfProject $TPB_TargetFramework462 $xunittest10kPerfProjectDir

        $testPerfProject = Join-Path $env:TP_ROOT_DIR "test\Microsoft.TestPlatform.PerformanceTests"
        Publish-PackageInternal $testPerfProject $TPB_TargetFramework48 $fullCLRTestDir
    }
}

function Publish-PackageInternal($packagename, $framework, $output) {
    $dotnetExe = Get-DotNetPath
    Invoke-Exe $dotnetExe -Arguments "publish $packagename --configuration $TPB_Configuration --framework $framework --output $output -v:minimal -p:Version=$TPB_Version -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild"
}

function Publish-PackageWithRuntimeInternal($packagename, $framework, $runtime, $selfcontained, $output) {
    $dotnetExe = Get-DotNetPath
    Invoke-Exe $dotnetExe -Arguments "publish $packagename --configuration $TPB_Configuration --framework $framework --runtime $runtime --self-contained $selfcontained --output $output -v:minimal -p:Version=$TPB_Version -p:CIBuild=$TPB_CIBuild -p:LocalizedBuild=$TPB_LocalizedBuild"
}

function Copy-Loc-Files($sourceDir, $destinationDir, $dllName) {
    foreach ($lang in $language) {
        $dllToCopy = Join-Path $sourceDir\$lang $dllName
        $destinationFolder = Join-Path $destinationDir $lang
        if (-not (Test-Path $destinationFolder)) {
            New-Item $destinationFolder -Type Directory -Force | Out-Null
        }
        Copy-Item $dllToCopy $destinationFolder -Force
    }
}

function Move-Loc-Files($sourceDir, $destinationDir, $dllName) {
    foreach ($lang in $language) {
        $dllToCopy = Join-Path $sourceDir\$lang $dllName
        $destinationFolder = Join-Path $destinationDir $lang
        if (-not (Test-Path $destinationFolder)) {
            New-Item $destinationFolder -Type Directory -Force | Out-Null
        }
        Move-Item $dllToCopy $destinationFolder -Force
    }
}

function Publish-VsixPackage {
    Write-Log "Publish-VsixPackage: Started."
    $timer = Start-Timer

    $packageDir = Get-FullCLR462PackageDirectory
    $extensionsPackageDir = Join-Path $packageDir "Extensions"
    $testImpactComComponentsDir = Join-Path $extensionsPackageDir "TestImpact"
    $legacyTestImpactComComponentsDir = Join-Path $extensionsPackageDir "V1\TestImpact"

    $testPlatformExternalsVersion = ([xml](Get-Content $env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.TestPlatformExternalsVersion
    $testPlatformMsDiaVersion = ([xml](Get-Content $env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.TestPlatformMSDiaVersion
    $codeCoverageExternalsVersion = ([xml](Get-Content $env:TP_ROOT_DIR\eng\Versions.props)).Project.PropertyGroup.MicrosoftInternalCodeCoverageVersion
    $interopExternalsVersion = ([xml](Get-Content $env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.InteropExternalsVersion

    # Copy Microsoft.VisualStudio.IO to root
    $codeCoverageIOPackageDirectory = Join-Path $env:TP_PACKAGES_DIR "Microsoft.CodeCoverage.IO\$codeCoverageExternalsVersion\lib\netstandard2.0"
    Copy-Item $codeCoverageIOPackageDirectory\Microsoft.CodeCoverage.IO.dll $packageDir -Force
    if ($TPB_LocalizedBuild) {
        Copy-Loc-Files $codeCoverageIOPackageDirectory $packageDir "Microsoft.CodeCoverage.IO.resources.dll"
    }

    # Copy legacy dependencies
    $legacyDir = Join-Path $env:TP_PACKAGES_DIR "Microsoft.Internal.TestPlatform.Extensions\$testPlatformExternalsVersion\tools\net451"
    Copy-Item -Recurse $legacyDir\* $packageDir -Force

    # Copy Microsoft.VisualStudio.ArchitectureTools.PEReader to Extensions
    Copy-Item $legacyDir\Microsoft.VisualStudio.ArchitectureTools.PEReader.dll $extensionsPackageDir -Force

    # Copy QtAgent Related depedencies
    $legacyDir = Join-Path $env:TP_PACKAGES_DIR "Microsoft.VisualStudio.QualityTools\$testPlatformExternalsVersion\tools\net451"
    Copy-Item -Recurse $legacyDir\* $packageDir -Force

    # Copy Legacy data collectors Related depedencies
    $legacyDir = Join-Path $env:TP_PACKAGES_DIR "Microsoft.VisualStudio.QualityTools.DataCollectors\$testPlatformExternalsVersion\tools\net451"
    Copy-Item -Recurse $legacyDir\* $packageDir -Force

    # Copy CUIT Related depedencies
    $legacyDir = Join-Path $env:TP_PACKAGES_DIR "Microsoft.VisualStudio.CUIT\$testPlatformExternalsVersion\tools\net451"
    Copy-Item -Recurse $legacyDir\* $packageDir -Force

    # Copy Interop depedencies
    $legacyDir = Join-Path $env:TP_PACKAGES_DIR "Microsoft.VisualStudio.Interop\$interopExternalsVersion\lib\net45"
    Copy-Item -Recurse $legacyDir\* $packageDir -Force

    # Copy COM Components and their manifests over
    $comComponentsDirectory = Join-Path $env:TP_PACKAGES_DIR "Microsoft.Internal.Dia\$testPlatformMsDiaVersion\tools\net451"
    Copy-Item -Recurse $comComponentsDirectory\* $packageDir -Force

    # Copy COM Components and their manifests over to Extensions Test Impact directory
    $comComponentsDirectoryTIA = Join-Path $env:TP_PACKAGES_DIR "Microsoft.Internal.Dia\$testPlatformMsDiaVersion\tools\net451"
    if (-not (Test-Path $testImpactComComponentsDir)) {
        New-Item $testImpactComComponentsDir -Type Directory -Force | Out-Null
    }
    Copy-Item -Recurse $comComponentsDirectoryTIA\* $testImpactComComponentsDir -Force

    if (-not (Test-Path $legacyTestImpactComComponentsDir)) {
        New-Item $legacyTestImpactComComponentsDir -Type Directory -Force | Out-Null
    }
    Copy-Item -Recurse $comComponentsDirectoryTIA\* $legacyTestImpactComComponentsDir -Force

    Copy-Item (Join-Path $env:TP_PACKAGE_PROJ_DIR "ThirdPartyNotices.txt") $packageDir -Force

    Write-Log "Publish-VsixPackage: Complete. {$(Get-ElapsedTime($timer))}"
}

function Create-VsixPackage {
    Write-Log "Create-VsixPackage: Started."
    $timer = Start-Timer

    Write-Verbose "Locating MSBuild install path..."
    $msbuildPath = Locate-MSBuildPath

    $vsixSourceDir = Join-Path $env:TP_ROOT_DIR "src\package\VSIXProject"
    $vsixProjectDir = Join-Path $env:TP_OUT_DIR "$TPB_Configuration\VSIX"

    # Create vsix only when msbuild is installed.
    if (![string]::IsNullOrEmpty($msbuildPath)) {
        # Copy the vsix project to artifacts directory to modify manifest
        New-Item $vsixProjectDir -Type Directory -Force
        Copy-Item -Recurse $vsixSourceDir\* $vsixProjectDir -Force

        # Update version of VSIX
        Update-VsixVersion $vsixProjectDir

        # Build vsix project to get TestPlatform.vsix
        Invoke-Exe $msbuildPath -Arguments """$vsixProjectDir\TestPlatform.csproj"" -p:Configuration=$Configuration"
    }
    else {
        throw ".. Create-VsixPackage: Cannot generate vsix as msbuild.exe not found at '$msbuildPath'."
    }

    Write-Log "Create-VsixPackage: Complete. {$(Get-ElapsedTime($timer))}"
}

function Create-NugetPackages {
    $timer = Start-Timer

    Write-Log "Create-NugetPackages: Started."
    $stagingDir = Join-Path $env:TP_OUT_DIR $TPB_Configuration
    $packageOutputDir = $TPB_PackageOutDir

    Copy-Item (Join-Path $env:TP_PACKAGE_PROJ_DIR "Icon.png") $stagingDir -Force

    # Packages folder should not be cleared on CI.
    # Artifacts from source-build are downloaded into this directory before the build starts, and this would remove them.
    if (-not $TPB_CIBuild) {
        # Remove all locally built nuget packages before we start creating them
        # we are leaving them in the folder after uzipping them for easier review.
        if (Test-Path $packageOutputDir) {
            Remove-Item $packageOutputDir -Recurse -Force
        }
    }

    if (-not (Test-Path $packageOutputDir)) {
        New-Item $packageOutputDir -Type directory -Force
    }

    $tpNuspecDir = Join-Path $env:TP_PACKAGE_PROJ_DIR "nuspec"

    # Copy over the nuspecs to the staging directory
    $nuspecFiles = @(
        "Microsoft.CodeCoverage.nuspec",
        "Microsoft.NET.Test.Sdk.nuspec",
        "Microsoft.TestPlatform.AdapterUtilities.nuspec",
        "Microsoft.TestPlatform.nuspec",
        "Microsoft.TestPlatform.Portable.nuspec",
        "TestPlatform.Extensions.TrxLogger.nuspec",
        "TestPlatform.ObjectModel.nuspec",
        "TestPlatform.TestHost.nuspec",
        "TestPlatform.TranslationLayer.nuspec"
        "TestPlatform.Internal.Uwp.nuspec"
    )

    $projectFiles = @(
        "Microsoft.TestPlatform.CLI.csproj",
        "Microsoft.TestPlatform.Build.csproj"
    )

    $dependencies = @(
        "TestPlatform.Build.nuspec",
        "TestPlatform.CLI.nuspec",

        ## .target and .props Files
        "Microsoft.NET.Test.Sdk.props",
        "Microsoft.CodeCoverage.props",
        "Microsoft.CodeCoverage.targets",

        ## Content Directories
        "netcoreapp",
        "netfx"
    )

    # Nuget pack analysis emits warnings if binaries are packaged as content. It is intentional for the below packages.
    $skipAnalysis = @(
        "TestPlatform.CLI.nuspec",
        "Microsoft.TestPlatform.CLI.csproj"
    )


    foreach ($item in $nuspecFiles + $projectFiles + $dependencies) {
        Copy-Item $tpNuspecDir\$item $stagingDir -Force -Recurse
    }

    # Copy empty and third patry notice file
    Copy-Item $tpNuspecDir\"_._" $stagingDir -Force
    Copy-Item $tpNuspecDir\..\"ThirdPartyNotices.txt" $stagingDir -Force
    Copy-Item $tpNuspecDir\..\"ThirdPartyNoticesCodeCoverage.txt" $stagingDir -Force

    # Copy licenses folder
    Copy-Item (Join-Path $env:TP_PACKAGE_PROJ_DIR "licenses") $stagingDir -Force -Recurse

    $testhostCore31PackageDir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Microsoft.TestPlatform.TestHost\$TPB_TargetFrameworkCore31")
    Copy-Item $tpNuspecDir\"Microsoft.TestPlatform.TestHost.NetCore.props" $testhostCore31PackageDir\Microsoft.TestPlatform.TestHost.props -Force

    # Call nuget pack on these components.
    $nugetExe = Join-Path $env:TP_PACKAGES_DIR -ChildPath "Nuget.CommandLine" | Join-Path -ChildPath $env:NUGET_EXE_Version | Join-Path -ChildPath "tools\NuGet.exe"
    $dotnetExe = Get-DotNetPath

    # Pass Newtonsoft.Json version to nuget pack to keep the version consistent across all nuget packages.
    $JsonNetVersion = ([xml](Get-Content $env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.JsonNetVersion

    # Additional external dependency folders
    $microsoftFakesVersion = ([xml](Get-Content $env:TP_ROOT_DIR\scripts\build\TestPlatform.Dependencies.props)).Project.PropertyGroup.MicrosoftFakesVersion
    $FakesPackageDir = Join-Path $env:TP_PACKAGES_DIR "Microsoft.QualityTools.Testing.Fakes.TestRunnerHarness\$microsoftFakesVersion\contentFiles"

    # package them from stagingDir
    foreach ($file in $nuspecFiles) {
        $additionalArgs = ""
        if ($skipAnalysis -contains $file) {
            $additionalArgs = "-NoPackageAnalysis"
        }

        Invoke-Exe $nugetExe -Arguments "pack $stagingDir\$file -OutputDirectory $packageOutputDir -Version $TPB_Version -Properties Version=$TPB_Version;JsonNetVersion=$JsonNetVersion;Runtime=$TPB_TargetRuntime;NetCoreTargetFramework=$TPB_TargetFrameworkCore31;FakesPackageDir=$FakesPackageDir;NetStandard20Framework=$TPB_TargetFrameworkNS20;BranchName=$TPB_BRANCH;CommitId=$TPB_COMMIT $additionalArgs"
    }

    foreach ($file in $projectFiles) {
        $additionalArgs = ""
        if ($skipAnalysis -contains $file) {
            $additionalArgs = "-NoPackageAnalysis"
        }

        Write-Host "Attempting to build package from '$file'."
        Invoke-Exe $dotnetExe -Arguments "restore $stagingDir\$file" -CaptureOutput | Out-Null
        Invoke-Exe $dotnetExe -Arguments "pack --no-build  $stagingDir\$file -o $packageOutputDir -p:Version=$TPB_Version -p:BranchName=`"$TPB_BRANCH`" -p:CommitId=`"$TPB_COMMIT`" /bl:pack_$file.binlog"
    }

    # Verifies that expected number of files gets shipped in nuget packages.
    # Few nuspec uses wildcard characters.
    Verify-Nuget-Packages $packageOutputDir $TPB_Version

    Write-Log "Create-NugetPackages: Complete. {$(Get-ElapsedTime($timer))}"
}

function Copy-CodeCoverage-Package-Artifacts {
    # Copy TraceDataCollector to Microsoft.CodeCoverage folder.
    $codeCoverageExternalsVersion = ([xml](Get-Content $env:TP_ROOT_DIR\eng\Versions.props)).Project.PropertyGroup.MicrosoftInternalCodeCoverageVersion
    $traceDataCollectorPackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.visualstudio.tracedatacollector\$codeCoverageExternalsVersion\lib\$TPB_TargetFrameworkNS20"
    $traceDataCollectorPackagesNetFxDir = Join-Path $env:TP_PACKAGES_DIR "Microsoft.VisualStudio.TraceDataCollector\$codeCoverageExternalsVersion\lib\$TPB_TargetFramework472"
    $internalCodeCoveragePackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.internal.codecoverage\$codeCoverageExternalsVersion\contentFiles\any\any\"
    $codeCoverageCorePackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.codecoverage.core\$codeCoverageExternalsVersion\lib\$TPB_TargetFrameworkNS20"
    $codeCoverageInterprocessPackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.codecoverage.interprocess\$codeCoverageExternalsVersion\lib\$TPB_TargetFrameworkNS20"
    $codeCoverageInstrumentationPackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.codecoverage.instrumentation\$codeCoverageExternalsVersion\lib\$TPB_TargetFrameworkNS20"
    $codeCoverageImUbuntuPackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.internal.codecoverage\$codeCoverageExternalsVersion\contentFiles\any\any\InstrumentationEngine\ubuntu"
    $codeCoverageImAlpinePackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.internal.codecoverage\$codeCoverageExternalsVersion\contentFiles\any\any\InstrumentationEngine\alpine"
    $codeCoverageImMacosPackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.internal.codecoverage\$codeCoverageExternalsVersion\contentFiles\any\any\InstrumentationEngine\macos"
    $codeCoverageTelemetryPackagesDir = Join-Path $env:TP_PACKAGES_DIR "microsoft.codecoverage.telemetry\$codeCoverageExternalsVersion\lib\$TPB_TargetFrameworkNS20"

    $microsoftCodeCoveragePackageDir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Microsoft.CodeCoverage\")
    $microsoftCodeCoverageExtensionsPackageDir = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\Microsoft.CodeCoverage.Extensions\")

    New-Item -ItemType directory -Path $microsoftCodeCoveragePackageDir -Force | Out-Null
    New-Item -ItemType directory -Path $microsoftCodeCoverageExtensionsPackageDir -Force | Out-Null

    Copy-Item $traceDataCollectorPackagesDir\Microsoft.VisualStudio.TraceDataCollector.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $traceDataCollectorPackagesDir\Microsoft.VisualStudio.TraceDataCollector.pdb $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageCorePackagesDir\Microsoft.CodeCoverage.Core.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInterprocessPackagesDir\Microsoft.CodeCoverage.Interprocess.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInstrumentationPackagesDir\Microsoft.CodeCoverage.Instrumentation.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInstrumentationPackagesDir\Mono.Cecil.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInstrumentationPackagesDir\Mono.Cecil.Pdb.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInstrumentationPackagesDir\Mono.Cecil.Rocks.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInstrumentationPackagesDir\Microsoft.DiaSymReader.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInstrumentationPackagesDir\Microsoft.DiaSymReader.Native.x86.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInstrumentationPackagesDir\Microsoft.DiaSymReader.Native.amd64.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInstrumentationPackagesDir\Microsoft.DiaSymReader.Native.arm64.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageInstrumentationPackagesDir\Microsoft.DiaSymReader.Native.arm.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $codeCoverageTelemetryPackagesDir\Microsoft.CodeCoverage.Telemetry.dll $microsoftCodeCoveragePackageDir -Force
    Copy-Item $internalCodeCoveragePackagesDir\CodeCoverage $microsoftCodeCoveragePackageDir -Force -Recurse
    Copy-Item $internalCodeCoveragePackagesDir\InstrumentationEngine $microsoftCodeCoveragePackageDir -Force -Recurse
    Copy-Item $internalCodeCoveragePackagesDir\Shim $microsoftCodeCoveragePackageDir -Force -Recurse

    Copy-Item $traceDataCollectorPackagesNetFxDir\Microsoft.VisualStudio.TraceDataCollector.dll $microsoftCodeCoverageExtensionsPackageDir -Force

    New-Item -ItemType directory -Path $microsoftCodeCoveragePackageDir\InstrumentationEngine\ubuntu\ -Force | Out-Null
    Copy-Item $codeCoverageImUbuntuPackagesDir\x64 $microsoftCodeCoveragePackageDir\InstrumentationEngine\ubuntu\ -Force -Recurse
    New-Item -ItemType directory -Path $microsoftCodeCoveragePackageDir\InstrumentationEngine\alpine\ -Force | Out-Null
    Copy-Item $codeCoverageImAlpinePackagesDir\x64 $microsoftCodeCoveragePackageDir\InstrumentationEngine\alpine\ -Force -Recurse
    New-Item -ItemType directory -Path $microsoftCodeCoveragePackageDir\InstrumentationEngine\macos\ -Force | Out-Null
    Copy-Item $codeCoverageImMacosPackagesDir\x64 $microsoftCodeCoveragePackageDir\InstrumentationEngine\macos\ -Force -Recurse

    # Copy TraceDataCollector resource dlls
    if ($TPB_LocalizedBuild) {
        Copy-Loc-Files $traceDataCollectorPackagesDir $microsoftCodeCoveragePackageDir "Microsoft.VisualStudio.TraceDataCollector.resources.dll"
        Copy-Loc-Files $traceDataCollectorPackagesNetFxDir $microsoftCodeCoverageExtensionsPackageDir "Microsoft.VisualStudio.TraceDataCollector.resources.dll"
    }
}

function Copy-PackageItems($packageName) {
    # Packages published separately are copied into their own artifacts directory
    # E.g. src\Microsoft.TestPlatform.ObjectModel\bin\Debug\net462\* is copied
    # to artifacts\Debug\Microsoft.TestPlatform.ObjectModel\net462
    $binariesDirectory = [System.IO.Path]::Combine($env:TP_ROOT_DIR, "src", "$packageName", "bin", "$TPB_Configuration")
    $binariesDirectory = $(Join-Path $binariesDirectory "*")
    $publishDirectory = $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\$packageName")
    Write-Log "Copy-PackageItems: Package: $packageName"
    Write-Verbose "Create $publishDirectory"
    New-Item -ItemType directory -Path $publishDirectory -Force | Out-Null

    Write-Log "Copy binaries for package '$packageName' from '$binariesDirectory' to '$publishDirectory'"
    Copy-Item -Path $binariesDirectory -Destination $publishDirectory -Recurse -Force
}

function Update-LocalizedResources {
    $timer = Start-Timer
    $dotnetExe = Get-DotNetPath

    Write-Log ".. Update-LocalizedResources: Started: $TPB_Solution"
    if (!$TPB_LocalizedBuild) {
        Write-Log ".. Update-LocalizedResources: Skipped based on user setting."
        return
    }

    $localizationProject = Join-Path $env:TP_PACKAGE_PROJ_DIR "Localize\Localize.proj"
    Invoke-Exe $dotnetExe -Arguments "msbuild $localizationProject -m -nologo -v:minimal -t:Localize -p:LocalizeResources=true -nodeReuse:False"
    Write-Log ".. Update-LocalizedResources: Complete. {$(Get-ElapsedTime($timer))}"
}

#
# Helper functions
#
function Get-DotNetPath {
    $dotnetPath = Join-Path $env:TP_TOOLS_DIR "dotnet\dotnet.exe"
    if (-not (Test-Path $dotnetPath)) {
        Write-Error "Dotnet.exe not found at $dotnetPath. Did the dotnet cli installation succeed?"
    }

    return $dotnetPath
}

function Get-FullCLR462PackageDirectory {
    return $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\$TPB_TargetFramework462\$TPB_TargetRuntime")
}

function Get-CoreCLR31PackageDirectory {
    return $(Join-Path $env:TP_OUT_DIR "$TPB_Configuration\$TPB_TargetFrameworkCore31")
}

function Locate-MSBuildPath {
    $vsInstallPath = Locate-VsInstallPath
    $msbuildPath = Get-ChildItem (Join-Path -path $vsInstallPath -childPath "MSBuild\*\Bin\MSBuild.exe")

    Write-Verbose "found msbuild : '$($msbuildPath -join "','")'"
    $msBuild = $msBuildPath | Select-Object -First 1

    Write-Verbose "msbuildPath is : '$($msbuildPath -join "','")'"
    if ($null -eq $msBuild -or 0 -eq $msBuild.Count) {
        throw "MSBuild not found."
    }

    return $msBuild.FullName
}

function Locate-VsInstallPath {
    $vswhere = Join-Path -path $env:TP_PACKAGES_DIR -ChildPath "vswhere\$env:VSWHERE_VERSION\tools\vswhere.exe"
    if (!(Test-Path -path $vswhere)) {
        throw "Unable to locate vswhere in path '$vswhere'."
    }

    Write-Verbose "Using '$vswhere' to locate VS installation path."

    $requiredPackageIds = @("Microsoft.Component.MSBuild", "Microsoft.Net.Component.4.6.TargetingPack", "Microsoft.VisualStudio.Component.VSSDK")
    Write-Verbose "VSInstallation requirements : $requiredPackageIds"

    Try {
        if ($TPB_CIBuild) {
            $vsInstallPath = Invoke-Exe $vswhere -CaptureOutput -Arguments "-version (15.0 -products * -requires $requiredPackageIds -property installationPath"
        }
        else {
            # Allow using pre release versions of VS for dev builds
            $vsInstallPath = Invoke-Exe $vswhere -CaptureOutput -Arguments "-version (15.0 -prerelease -products * -requires $requiredPackageIds -property installationPath"
        }
    }
    Catch [System.Management.Automation.MethodInvocationException] {
        throw "Failed to find VS installation with requirements: $requiredPackageIds"
    }

    if ($null -eq $vsInstallPath -or 0 -eq @($vsInstallPath).Count) {
        throw "Failed to find VS installation with requirements: $requiredPackageIds"
    }
    else {
        Write-Verbose "Found VS installation with requirements '$($requiredPackageIds -join "','")'  : '$($vsInstallPath -join "','")'."
    }

    $vsPath = $vsInstallPath | Select-Object -First 1
    Write-Verbose "VSInstallPath is : $vsPath"
    return $vsPath
}

function Update-VsixVersion($vsixProjectDir) {
    Write-Log "Update-VsixVersion: Started."
    $vsixVersion = $Version

    # Build number comes in the form 20170111-01(yyyymmdd-buildNoOfThatDay)
    # So Version of the vsix will be 15.1.0.2017011101
    $vsixVersionSuffix = $BuildNumber.Split("-");
    if ($vsixVersionSuffix.Length -ige 2) {
        $vsixVersion = "$vsixVersion.$($vsixVersionSuffix[0])$($vsixVersionSuffix[1])"
    }

    $manifestContentWithVersion = Get-Content "$vsixProjectDir\source.extension.vsixmanifest" -raw | ForEach-Object { $_.ToString().Replace("`$version`$", "$vsixVersion") }
    Set-Content -path "$vsixProjectDir\source.extension.vsixmanifest" -value $manifestContentWithVersion

    Write-Log "Update-VsixVersion: Completed."
}

function Generate-Manifest ($PackageFolder) {
    $packagesFolderName = [System.IO.Path]::GetFileName($PackageFolder)
    Write-Log "Generate-Manifest ($packagesFolderName): Started."

    $generateManifestPath = Join-Path $env:TP_ROOT_DIR "scripts\build\GenerateManifest.proj"
    $msbuildPath = Locate-MSBuildPath

    Invoke-Exe $msbuildPath -Arguments "$generateManifestPath /t:PublishToBuildAssetRegistry /p:PackagesToPublishPattern=$PackageFolder\*.nupkg /p:BUILD_BUILDNUMBER=$BuildNumber /p:PackagesPath=""$PackageFolder"" /p:Configuration=$TPB_Configuration /bl:""$env:TP_OUT_DIR\log\$Configuration\manifest-generation-$packagesFolderName.binlog"""

    Write-Log "Generate-Manifest ($packagesFolderName): Completed."
}

function Build-SpecificProjects {
    Write-Log "Build-SpecificProjects: Started for pattern: $ProjectNamePatterns"
    # FrameworksAndOutDirs format ("<target_framework>", "<output_dir>").
    $FrameworksAndOutDirs = (
        ("net462", "net462\win7-x64"),
        # REVIEW ME: Why do we copy netstandard2.0 into netcorecorapp2.1?
        ("netstandard2.0", "netstandard2.0"),
        ("netcoreapp3.1", "netcoreapp3.1")
    )

    $dotnetPath = Get-DotNetPath

    # Get projects to build.
    Get-ChildItem -Recurse -Path $env:TP_ROOT_DIR -Include *.csproj | ForEach-Object {
        foreach ($ProjectNamePattern in $ProjectNamePatterns) {
            if ($_.FullName -match $ProjectNamePattern) {
                $ProjectsToBuild += , "$_"
            }
        }
    }

    if ( $null -eq $ProjectsToBuild) {
        Write-Error "No csproj name match for given pattern: $ProjectNamePatterns"
    }

    # Build Projects.
    foreach ($ProjectToBuild in $ProjectsToBuild) {
        Write-Log "Building Project $ProjectToBuild"
        # Restore and Build
        $output = Invoke-Exe $dotnetPath -Arguments "restore $ProjectToBuild"
        PrintAndExit-OnError $output
        $output = Invoke-Exe $dotnetPath -Arguments "build $ProjectToBuild"
        PrintAndExit-OnError $output

        if (-Not ($ProjectToBuild.FullName -contains "$($env:TP_ROOT_DIR)$([IO.Path]::DirectorySeparatorChar)src")) {
            # Don't copy artifacts for non src folders.
            continue;
        }

        # Copy artifacts
        $ProjectDir = [System.IO.Path]::GetDirectoryName($ProjectToBuild)
        foreach ($FrameworkAndOutDir in $FrameworksAndOutDirs) {
            $fromDir = $([System.IO.Path]::Combine($ProjectDir, "bin", $TPB_Configuration, $FrameworkAndOutDir[0]))
            $toDir = $([System.IO.Path]::Combine($env:TP_OUT_DIR, $TPB_Configuration, $FrameworkAndOutDir[1]))
            if ( Test-Path $fromDir) {
                Write-Log "Copying artifacts from $fromDir to $toDir"
                Get-ChildItem $fromDir | ForEach-Object {
                    if (-not ($_.PSIsContainer)) {
                        Copy-Item $_.FullName $toDir
                    }
                }
            }
        }
    }
}

if ($ProjectNamePatterns.Count -ne 0) {
    # Build Specific projects.
    Build-SpecificProjects
    Exit
}

# Execute build
$timer = Start-Timer
Write-Log "Build started: args = '$args'"
Write-Log "Test platform environment variables: "
Get-ChildItem env: | Where-Object -FilterScript { $_.Name.StartsWith("TP_") } | Format-Table
Write-Log "Test platform build variables: "
Get-Variable | Where-Object -FilterScript { $_.Name.StartsWith("TPB_") } | Format-Table

if ($Force -or $Steps -contains "InstallDotnet") {
    Install-DotNetCli
}

if ($Force -or $Steps -contains "Restore") {
    Clear-Package
    Restore-Package
}

if ($Force -or $Steps -contains "UpdateLocalization") {
    Update-LocalizedResources
}

if ($Force -or $Steps -contains "Build") {
    Invoke-Build
}

if ($Force -or $Steps -contains "Publish") {
    Publish-Package
    Publish-VsixPackage
}

if ($Force -or $Steps -contains "Pack") {
    Create-VsixPackage
    Create-NugetPackages
}

if ($Force -or $Steps -contains "Manifest") {
    Generate-Manifest -PackageFolder $TPB_PackageOutDir
    if (Test-Path $TPB_SourceBuildPackageOutDir)
    {
        Generate-Manifest -PackageFolder $TPB_SourceBuildPackageOutDir
    }
    Copy-PackageIntoStaticDirectory
}

if ($Force -or $Steps -contains "PrepareAcceptanceTests") {
    Publish-PatchedDotnet
    Invoke-TestAssetsBuild
    Invoke-CompatibilityTestAssetsBuild
    Publish-Tests
}

if ($Script:ScriptFailed) {
    Write-Log "Build failed. {$(Get-ElapsedTime($timer))}" -Level "Error"
    Exit 1
}
else {
    Write-Log "Build succeeded. {$(Get-ElapsedTime($timer))}"
    Exit 0
}
