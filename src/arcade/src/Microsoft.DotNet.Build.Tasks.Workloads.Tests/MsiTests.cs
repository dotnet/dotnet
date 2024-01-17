// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Microsoft.NET.Sdk.WorkloadManifestReader;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    [Collection("6.0.200 Toolchain manifest tests")]
    public class MsiTests : TestBase
    {
        private static ITaskItem BuildManifestMsi(string path, string msiVersion = "1.2.3", string platform = "x64", string msiOutputPath = null)
        {
            TaskItem packageItem = new(path);
            WorkloadManifestPackage pkg = new(packageItem, PackageRootDirectory, new Version(msiVersion));
            pkg.Extract();
            WorkloadManifestMsi msi = new(pkg, platform, new MockBuildEngine(), WixToolsetPath, BaseIntermediateOutputPath,
                isSxS: true);
            return string.IsNullOrWhiteSpace(msiOutputPath) ? msi.Build(MsiOutputPath) : msi.Build(msiOutputPath);
        }

        [WindowsOnlyFact]
        public void WorkloadManifestsIncludeInstallationRecords()
        {
            ITaskItem msi603 = BuildManifestMsi(Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.3.nupkg"), 
                msiOutputPath: Path.Combine(MsiOutputPath, "mrec"));
            string msiPath603 = msi603.GetMetadata(Metadata.FullPath);

            MsiUtils.GetAllRegistryKeys(msiPath603).Should().Contain(r =>
              r.Key == @"SOFTWARE\Microsoft\dotnet\InstalledManifests\x64\Microsoft.NET.Workload.Mono.ToolChain.Manifest-6.0.200\6.0.3"
            );
        }

        [WindowsOnlyFact]
        public void ItCanBuildSideBySideManifestMsis()
        {
            string PackageRootDirectory = Path.Combine(BaseIntermediateOutputPath, "pkg");

            // Build 6.0.200 manifest for version 6.0.3
            ITaskItem msi603 = BuildManifestMsi(Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.3.nupkg"));
            string msiPath603 = msi603.GetMetadata(Metadata.FullPath);

            // Build 6.0.200 manifest for version 6.0.4
            ITaskItem msi604 = BuildManifestMsi(Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.4.nupkg"));
            string msiPath604 = msi604.GetMetadata(Metadata.FullPath);

            // For upgradable MSIs, the 6.0.4 and 6.0.3 copies of the package would have generated the same
            // upgrade code to ensure upgrades along the manifest feature band. For SxS they should be different.
            Assert.NotEqual(MsiUtils.GetProperty(msiPath603, MsiProperty.UpgradeCode), MsiUtils.GetProperty(msiPath604, MsiProperty.UpgradeCode));

            // Provider keys for SxS MSIs should be different while upgrade related MSIs should have stable provider keys.
            Assert.Equal("Microsoft.NET.Workload.Mono.ToolChain,6.0.200,6.0.3,x64", MsiUtils.GetProviderKeyName(msiPath603));
            Assert.Equal("Microsoft.NET.Workload.Mono.ToolChain,6.0.200,6.0.4,x64", MsiUtils.GetProviderKeyName(msiPath604));

            // WiX populates the DefaultDir column using "short name | long name" pairs.
            MsiUtils.GetAllDirectories(msiPath603).Should().Contain(d =>
                d.Directory == "ManifestVersionDir" &&
                d.DirectoryParent == "ManifestIdDir" &&
                d.DefaultDir.EndsWith("|6.0.3"));
            MsiUtils.GetAllDirectories(msiPath604).Should().Contain(d =>
                d.Directory == "ManifestVersionDir" &&
                d.DirectoryParent == "ManifestIdDir" &&
                d.DefaultDir.EndsWith("|6.0.4"));

            // Generated MSI should return the path where the .wixobj files are located so
            // WiX packs can be created for post-build signing.
            Assert.NotNull(msi603.GetMetadata(Metadata.WixObj));
            Assert.NotNull(msi604.GetMetadata(Metadata.WixObj));
        }

        [WindowsOnlyFact]
        public void ItCanBuildAManifestMsi()
        {
            string PackageRootDirectory = Path.Combine(BaseIntermediateOutputPath, "pkg");
            TaskItem packageItem = new(Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.3.nupkg"));
            WorkloadManifestPackage pkg = new(packageItem, PackageRootDirectory, new Version("1.2.3"));
            pkg.Extract();
            WorkloadManifestMsi msi = new(pkg, "x64", new MockBuildEngine(), WixToolsetPath, BaseIntermediateOutputPath);

            ITaskItem item = msi.Build(MsiOutputPath);

            string msiPath = item.GetMetadata(Metadata.FullPath);

            // Process the summary information stream's template to extract the MSIs target platform.
            using SummaryInfo si = new(msiPath, enableWrite: false);

            // UpgradeCode is predictable/stable for manifest MSIs.
            Assert.Equal("{E4761192-882D-38E9-A3F4-14B6C4AD12BD}", MsiUtils.GetProperty(msiPath, MsiProperty.UpgradeCode));
            Assert.Equal("1.2.3", MsiUtils.GetProperty(msiPath, MsiProperty.ProductVersion));
            Assert.Equal("Microsoft.NET.Workload.Mono.ToolChain,6.0.200,x64", MsiUtils.GetProviderKeyName(msiPath));
            Assert.Equal("x64;1033", si.Template);

            // There should be no version directory present if the old upgrade model is used.
            MsiUtils.GetAllDirectories(msiPath).Select(d => d.Directory).Should().NotContain("ManifestVersionDir");

            // Generated MSI should return the path where the .wixobj files are located so
            // WiX packs can be created for post-build signing.
            Assert.NotNull(item.GetMetadata(Metadata.WixObj));
        }

        [WindowsOnlyFact]
        public void ItCanBuildATemplatePackMsi()
        {
            string PackageRootDirectory = Path.Combine(BaseIntermediateOutputPath, "pkg");
            string packagePath = Path.Combine(TestAssetsPath, "microsoft.ios.templates.15.2.302-preview.14.122.nupkg");

            WorkloadPack p = new(new WorkloadPackId("Microsoft.iOS.Templates"), "15.2.302-preview.14.122", WorkloadPackKind.Template, null);
            TemplatePackPackage pkg = new(p, packagePath, new[] { "x64" }, PackageRootDirectory);
            pkg.Extract();
            var buildEngine = new MockBuildEngine();
            WorkloadPackMsi msi = new(pkg, "x64", buildEngine, WixToolsetPath, BaseIntermediateOutputPath);
            ITaskItem item = msi.Build(MsiOutputPath);

            string msiPath = item.GetMetadata(Metadata.FullPath);

            // Process the summary information stream's template to extract the MSIs target platform.
            using SummaryInfo si = new(msiPath, enableWrite: false);

            // UpgradeCode is predictable/stable for pack MSIs since they are seeded using the package identity (ID & version).
            Assert.Equal("{EC4D6B34-C9DE-3984-97FD-B7AC96FA536A}", MsiUtils.GetProperty(msiPath, MsiProperty.UpgradeCode));
            // The version is set using the package major.minor.patch
            Assert.Equal("15.2.302.0", MsiUtils.GetProperty(msiPath, MsiProperty.ProductVersion));
            Assert.Equal("Microsoft.iOS.Templates,15.2.302-preview.14.122,x64", MsiUtils.GetProviderKeyName(msiPath));
            Assert.Equal("x64;1033", si.Template);

            // Template packs should pull in the raw nupkg. We can verify by query the File table. There should
            // only be a single file.
            FileRow fileRow = MsiUtils.GetAllFiles(msiPath).FirstOrDefault();
            Assert.Contains("microsoft.ios.templates.15.2.302-preview.14.122.nupk", fileRow.FileName);

            // Generated MSI should return the path where the .wixobj files are located so
            // WiX packs can be created for post-build signing.
            Assert.NotNull(item.GetMetadata(Metadata.WixObj));
        }
    }
}
