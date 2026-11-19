// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.DotNet.Releases;
using Microsoft.NET.Sdk.WorkloadManifestReader;
using NuGet.Versioning;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests;

[Collection("6.0.200 Toolchain manifest tests")]
public class PackageTests : TestBase
{
    [WindowsOnlyTheory]
    [InlineData("11.0.100", null, "x86,x64,arm64")]
    [InlineData("12.0.100-alpha.1", null, "x64,arm64")]
    [InlineData("12.0.200", null, "x64,arm64")]
    [InlineData("13.0.100", null, "x64,arm64")]
    [InlineData("11.0.100", "any", "x86,x64,arm64")]
    [InlineData("12.0.100", "any", "x64,arm64")]
    [InlineData("12.0.100", "win", "x64,arm64")]
    public void ItSelectsInstallerPlatformsForNeutralPacks(string sdkFeatureBand, string aliasRid, string expectedPlatforms)
    {
        WorkloadPack pack = new(new WorkloadPackId("Example.Templates"), "1.0.0", WorkloadPackKind.Template,
            aliasRid == null ? null : new Dictionary<string, WorkloadPackId> { [aliasRid] = new("Example.Templates") });
        string[] platforms = WorkloadPackageBase.GetSupportedPlatforms(new ReleaseVersion(sdkFeatureBand));

        var sourcePackages = WorkloadPackPackage.GetSourcePackages(TestAssetsPath, pack, platforms).ToArray();

        var sourcePackage = Assert.Single(sourcePackages);
        Assert.Equal(Path.Combine(TestAssetsPath, "Example.Templates.1.0.0.nupkg"), sourcePackage.sourcePackage);
        Assert.Equal(expectedPlatforms.Split(','), sourcePackage.platforms);
    }

    [WindowsOnlyTheory]
    [InlineData("11.0.100", "x86,x64,arm64")]
    [InlineData("12.0.100-alpha.1", "x64,arm64")]
    public void ItFiltersAliasedPackInstallersBySdkHost(string sdkFeatureBand, string expectedPlatforms)
    {
        WorkloadPack pack = new(new WorkloadPackId("Example.Tool"), "1.0.0", WorkloadPackKind.Tool,
            new Dictionary<string, WorkloadPackId>
            {
                ["win-x86"] = new("Example.Tool.win-x86"),
                ["win-x64"] = new("Example.Tool.win-x64"),
                ["win-arm64"] = new("Example.Tool.win-arm64"),
                ["linux-x64"] = new("Example.Tool.linux-x64")
            });
        string[] platforms = WorkloadPackageBase.GetSupportedPlatforms(new ReleaseVersion(sdkFeatureBand));

        var sourcePackages = WorkloadPackPackage.GetSourcePackages(TestAssetsPath, pack, platforms).ToArray();

        Assert.Equal(expectedPlatforms.Split(','), sourcePackages.SelectMany(p => p.platforms));
        Assert.Equal(expectedPlatforms.Split(',').Select(p => Path.Combine(TestAssetsPath, $"Example.Tool.win-{p}.1.0.0.nupkg")),
            sourcePackages.Select(p => p.sourcePackage));
    }

    [WindowsOnlyFact]
    public void ItSkipsPacksAvailableOnlyForAnUnsupportedSdkHost()
    {
        WorkloadPack pack = new(new WorkloadPackId("Example.Tool"), "1.0.0", WorkloadPackKind.Tool,
            new Dictionary<string, WorkloadPackId> { ["win-x86"] = new("Example.Tool.win-x86") });
        string[] platforms = WorkloadPackageBase.GetSupportedPlatforms(new ReleaseVersion("12.0.100"));

        Assert.Empty(WorkloadPackPackage.GetSourcePackages(TestAssetsPath, pack, platforms));
    }

    [WindowsOnlyFact]
    public void ItCanReadAManifestPackage()
    {
        string PackageRootDirectory = Path.Combine(BaseIntermediateOutputPath, "pkg");

        TaskItem manifestPackageItem = new(Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.300.6.0.22.nupkg"));
        WorkloadManifestPackage p = new(manifestPackageItem, PackageRootDirectory, new Version("1.2.3"));

        ReleaseVersion expectedFeatureBand = new("6.0.300");

        Assert.Equal("Microsoft.NET.Workload.Mono.ToolChain", p.ManifestId);
        Assert.Equal(expectedFeatureBand, p.SdkFeatureBand);
    }

    [WindowsOnlyTheory]
    [InlineData("Microsoft.NET.Workload.Emscripten.net6.Manifest-8.0.100-alpha.1", WorkloadManifestPackage.ManifestSeparator, "8.0.100-alpha.1")]
    [InlineData("Microsoft.NET.Workload.Emscripten.Manifest-8.0.100-alpha.1.23062.6", WorkloadManifestPackage.ManifestSeparator, "8.0.100-alpha.1.23062.6")]
    [InlineData("Microsoft.NET.Workloads.8.0.100-preview.7.23376.3", WorkloadSetPackage.SdkFeatureBandSeparator, "8.0.100-preview.7.23376.3")]
    [InlineData("Microsoft.NET.Workloads.8.0.100", WorkloadSetPackage.SdkFeatureBandSeparator, "8.0.100")]
    public static void ItExtractsTheSdkVersionFromThePackageId(string packageId, string separator, string expectedVersion)
    {
        string actualSdkVersion = WorkloadPackageBase.GetSdkVersion(packageId, separator);

        Assert.Equal(expectedVersion, actualSdkVersion);
    }

    [WindowsOnlyFact]
    public void ItThrowsIfTheMsiVersionIsInvalid()
    {
        string PackageRootDirectory = Path.Combine(BaseIntermediateOutputPath, "wls-pkg");

        ITaskItem workloadSetPackageItem = new TaskItem(Path.Combine(TestAssetsPath, "microsoft.net.workloads.9.0.100.9.0.100-baseline.1.23464.1.nupkg"));

        Assert.Throws<ArgumentOutOfRangeException>(() => { WorkloadSetPackage p = new(workloadSetPackageItem, PackageRootDirectory, new Version("256.12.3")); });
    }

    [WindowsOnlyTheory]
    [InlineData("8.0.200", "8.200.0", "8.0.200")]
    [InlineData("8.0.200", "8.201.0", "8.0.201")]
    [InlineData("8.0.200", "8.203.1", "8.0.203.1")]
    [InlineData("9.0.100-preview.2", "9.100.0-preview.2.3.4.5.6.7.8", "9.0.100-preview.2.3.4.5.6.7.8")]
    [InlineData("8.0.200", "8.201.1-preview", "8.0.201.1-preview")]
    [InlineData("8.0.200", "8.201.1-preview.2", "8.0.201.1-preview.2")]
    [InlineData("8.0.200", "8.201.0-servicing.23015", "8.0.201-servicing.23015")]
    public void ItCanComputeWorkloadSetVersion(string sdkFeatureBand, string packageVersion, string expectedWorkloadSetVerion)
    {
        string workloadSetVersion = WorkloadSetPackage.GetWorkloadSetVersion(new ReleaseVersion(sdkFeatureBand), new NuGetVersion(packageVersion));

        Assert.Equal(expectedWorkloadSetVerion, workloadSetVersion);
    }
}
