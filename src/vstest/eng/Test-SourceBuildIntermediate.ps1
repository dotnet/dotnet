param([string] $DotNet = "$PSScriptRoot\..\.dotnet\dotnet.exe")

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$repo = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$version = '1.0.0-intermediate-test.1'
$root = Join-Path $repo ('artifacts\tmp\intermediate-test-' + [guid]::NewGuid().ToString('N'))
$cache = Join-Path $root 'tool-packages'
$shipping = Join-Path $root 'source-built\packages\Release\Shipping'
$compat = Join-Path $root 'compat'
$fixture = Join-Path $compat 'fixture'
$null = New-Item -ItemType Directory -Path $shipping, $compat, $fixture

try {
    $sdk = (Get-Content -LiteralPath (Join-Path $repo 'global.json') -Raw | ConvertFrom-Json).sdk.version
    [IO.File]::WriteAllText((Join-Path $compat 'global.json'), @"
{"sdk":{"version":"$sdk","rollForward":"disable","allowPrerelease":true},"msbuild-sdks":{"Microsoft.DotNet.Arcade.Sdk":"9.0.0-beta.25164.2"}}
"@)
    $env:DOTNET_ROOT = Split-Path $DotNet
    $env:NUGET_PACKAGES = $cache
    Push-Location $compat
    try {
        if ((& $DotNet --version).Trim() -ne $sdk) {
            throw "The exact VSTest SDK $sdk was not selected."
        }
    & $DotNet restore (Join-Path $PSScriptRoot 'SourceBuildIntermediateTools.csproj') `
        -p:ImportDirectoryBuildProps=false -p:ImportDirectoryBuildTargets=false -p:ImportDirectoryPackagesProps=false `
        --packages $cache `
        --source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json `
        --source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-public/nuget/v3/index.json
    if ($LASTEXITCODE -ne 0) {
        throw 'Could not restore the versioned compatibility tooling.'
    }

    $fixtureProject = Join-Path $fixture 'Probe.csproj'
    [IO.File]::WriteAllText($fixtureProject, @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <IsPackable>true</IsPackable>
    <ImportDirectoryBuildProps>false</ImportDirectoryBuildProps>
    <ImportDirectoryBuildTargets>false</ImportDirectoryBuildTargets>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <Authors>LocalProbe</Authors>
  </PropertyGroup>
</Project>
"@)
    [IO.File]::WriteAllText((Join-Path $fixture 'Probe.cs'), @"
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SourceBuildProbe;

public sealed class Probe { }
"@)
    foreach ($id in @('Microsoft.TestPlatform.Build', 'Microsoft.TestPlatform.CLI')) {
        & $DotNet pack $fixtureProject -c Release "-p:PackageId=$id" "-p:Version=$version" `
            -p:ImportDirectoryBuildProps=false -p:ImportDirectoryBuildTargets=false `
            -p:PackageIcon= -p:UseSharedCompilation=false -o $shipping
        if ($LASTEXITCODE -ne 0) {
            throw "Could not build the valid local $id package."
        }
    }
    foreach ($id in @('Microsoft.TestPlatform.Build', 'Microsoft.TestPlatform.CLI')) {
        if (-not (Test-Path -LiteralPath (Join-Path $shipping "$id.$version.nupkg"))) {
            throw "The versioned $id package was not produced by the fixture build."
        }
    }
    }
    finally {
        Pop-Location
    }

    $arcade = Join-Path $cache 'microsoft.dotnet.arcade.sdk\9.0.0-beta.25164.2\tools'
    $shared = @(
        "-p:RepoRoot=$repo\",
        '-p:ArcadeSdkVersion=9.0.0-beta.25164.2',
        "-p:NuGetPackageRoot=$cache\",
        '-p:Configuration=Release',
        '-p:UseInnerClone=false',
        '-p:ArcadeBuildFromSource=true',
        '-p:DotNetBuildSourceOnly=true',
        '-p:SourceBuildManagedOnly=true',
        '-p:GitHubRepositoryName=vstest',
        '-p:OfficialBuildId=20260929.99',
        "-p:Version=$version",
        "-p:RepositoryCommit=$(git -C $repo rev-parse HEAD)",
        '-p:RepositoryUrl=https://dev.azure.com/dnceng/internal/_git/microsoft-vstest',
        '-p:RepositoryType=git',
        '-p:PackageProjectUrl=https://github.com/microsoft/vstest',
        '-p:PackageDescription=Source-built VSTest packages for downstream SDK builds',
        "-p:ArtifactsDir=$compat\artifacts\",
        "-p:ArtifactsObjDir=$compat\artifacts\obj\",
        "-p:SourceBuildOutputDir=$fixture\",
        "-p:CurrentRepoSourceBuildPackageCache=$root\source-built\package-cache\",
        "-p:CurrentRepoSourceBuildArtifactsPackagesDir=$root\source-built\packages\Release\",
        "-p:SourceBuildSelfPrebuiltReportDir=$compat\prebuilt-report\"
    )
    Push-Location $compat
    try {
        & $DotNet msbuild (Join-Path $arcade 'SourceBuild\AfterSourceBuild.proj') `
            -nologo -nr:false -t:AfterSourceBuild `
            -p:MicrosoftDotNetSourceBuildTasksVersion=9.0.0-beta.25164.2 @shared
        if ($LASTEXITCODE -ne 0) {
            throw 'The original Arcade generator did not produce an intermediate.'
        }
        $itemJson = & $DotNet msbuild (Join-Path $arcade 'Publish.proj') `
            -nologo -nr:false -t:BeforePublish `
            -p:MicrosoftDotNetBuildTasksFeedVersion=9.0.0-beta.25164.2 `
            -p:DotNetPublishUsingPipelines=true -p:PublishToSymbolServer=false `
            -p:AutoGenerateSymbolPackages=false -p:AllowEmptySignPostBuildList=true `
            -p:PushToLocalStorage=true -getItem:PackagesToPublish,ExistingSymbolPackages,ItemsToPushToBlobFeed @shared
        if ($LASTEXITCODE -ne 0) {
            throw 'The legacy publishing item evaluation failed without executing publication.'
        }
    }
    finally {
        Pop-Location
    }

    $package = Join-Path $compat "artifacts\packages\Release\NonShipping\Microsoft.SourceBuild.Intermediate.vstest.$version.nupkg"
    if (-not (Test-Path -LiteralPath $package)) {
        throw 'No version-matched, nonshipping intermediate was built.'
    }
    $items = ($itemJson -join [Environment]::NewLine) | ConvertFrom-Json
    if (@($items.Items.PackagesToPublish).Count -ne 1 -or
        @($items.Items.ItemsToPushToBlobFeed).Count -ne 1 -or
        @($items.Items.ExistingSymbolPackages).Count -ne 0 -or
        $items.Items.PackagesToPublish[0].Identity -ne $package -or
        $items.Items.PackagesToPublish[0].IsShipping -ne 'false') {
        throw 'Publishing would stage anything other than the one nonshipping intermediate.'
    }

    $zip = [IO.Compression.ZipFile]::OpenRead($package)
    try {
        $entries = @($zip.Entries | ForEach-Object FullName)
        foreach ($entry in @(
            "artifacts/Microsoft.TestPlatform.Build.$version.nupkg",
            "artifacts/Microsoft.TestPlatform.CLI.$version.nupkg",
            'RepoManifest.xml',
            'prebuilt-report/annotated-usage.xml',
            'prebuilt-report/prebuilt-usage.xml',
            'prebuilt-report/baseline-comparison.xml',
            'prebuilt-report/generated-new-baseline.xml'
        )) {
            if ($entry -notin $entries) {
                throw "Intermediate lacks $entry."
            }
        }
        Write-Host "Intermediate contains both valid version-matched packages, the repo manifest, and four generated reports."
    }
    finally {
        $zip.Dispose()
    }
}
finally {
    Remove-Item -LiteralPath $root -Recurse -Force
}
