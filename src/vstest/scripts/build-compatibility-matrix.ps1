
param (
    [VerifyNotNull]
    [string] $RootPath,
    [VerifyNotNull]
    [string] $TestAssetsPath,
    [VerifyNotNull]
    [string] $TestArtifactsPath,
    [VerifyNotNull]
    [string] $PackagesPath,
    [VerifySet("Debug", "Release")]
    [string] $Configuration,
    [VerifyNotNull]
    [string] $DotnetExe,
    [switch] $Ci,
    [switch] $LocalizedBuild,
    [VerifyNotNull]
    [string] $NugetExeVersion
)

. $PSScriptRoot\common.lib.ps1

Write-Log "Invoke-CompatibilityTestAssetsBuild: Start test assets build."
$timer = Start-Timer
$generated = Join-Path (Split-Path -Path $TestAssetsPath) -ChildPath "GeneratedTestAssets"
$generatedSln = Join-Path $generated "CompatibilityTestAssets.sln"

# Figure out if the versions or the projects to build changed, and if they did not
# and the solution is already in place just build it.
# Otherwise delete everything and regenerate and re-build.
$dependenciesPath = "$RootPath\scripts\build\TestPlatform.Dependencies.props"
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
    "$RootPath\test\TestAssets\MSTestProject1\MSTestProject1.csproj"
    "$RootPath\test\TestAssets\MSTestProject2\MSTestProject2.csproj"
    # Don't use this one, it does not use the variables for mstest and test sdk.
    # "$RootPath\test\TestAssets\SimpleTestProject2\SimpleTestProject2.csproj"
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
        Invoke-Exe $dotnetExe -Arguments "build $RootPath\test\TestAssets\Tools\Tools.csproj --configuration $Configuration -v:minimal -p:CIBuild=$Ci -p:LocalizedBuild=$LocalizedBuild -p:NETTestSdkVersion=$netTestSdkVersion"
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
        Invoke-Exe $dotnetExe -Arguments "build $generatedSln --configuration $Configuration -v:minimal -p:CIBuild=$Ci -p:LocalizedBuild=$LocalizedBuild"
        $rebuild = $false
    }
}

if ($rebuild) {
    if (Test-Path $generated) {
        Remove-Item $generated -Recurse -Force
    }

    New-Item -ItemType Directory -Force -Path $generated | Out-Null

    Write-Log ".. .. Generate: Source: $generatedSln"
    $nugetExe = Join-Path $PackagesPath -ChildPath "Nuget.CommandLine" | Join-Path -ChildPath $NugetExeVersion | Join-Path -ChildPath "tools\NuGet.exe"
    $nugetConfigSource = Join-Path $TestAssetsPath "NuGet.config"
    $nugetConfig = Join-Path $generated "NuGet.config"

    Invoke-Exe $dotnetExe -Arguments "new sln --name CompatibilityTestAssets --output ""$generated"""

    Write-Log ".. .. Build: Source: $generatedSln"
    try {
        $projectsToAdd = @()
        $nugetConfigSource = Join-Path $TestAssetsPath "NuGet.config"
        $nugetConfig = Join-Path $generated "NuGet.config"

        Copy-Item -Path $nugetConfigSource -Destination $nugetConfig

        Write-Log ".. .. Build: Source: $generatedSln -- add NuGet source"
        Invoke-Exe -IgnoreExitCode 1 $nugetExe -Arguments "sources add -Name ""locally-built-testplatform-packages"" -Source $TestArtifactsPath\packages\ -ConfigFile ""$nugetConfig"""

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
        Invoke-Exe $dotnetExe -Arguments "build $generatedSln --configuration $Configuration -v:minimal -p:CIBuild=$Ci -p:LocalizedBuild=$LocalizedBuild"
        $cacheIdText | Set-Content "$generated/checksum.json" -NoNewline
    }
    finally {
        Write-Log ".. .. Build: Source: $TestAssetsPath_Solution -- remove NuGet source"
        Invoke-Exe -IgnoreExitCode 1 $nugetExe -Arguments "sources remove -Name ""locally-built-testplatform-packages"" -ConfigFile ""$nugetConfig"""
    }
}
Write-Log ".. .. Build: Complete."
Write-Log "Invoke-CompatibilityTestAssetsBuild: Complete. {$(Get-ElapsedTime($timer))}"


