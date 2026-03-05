// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Microsoft.NET.Sdk.WorkloadManifestReader;
using WixToolset.Dtf.WindowsInstaller;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    [Collection("6.0.200 Toolchain manifest tests")]
    public class MsiTests : TestBase
    {
        /// <summary>
        /// Helper method to build a manifest MSI from a given manifest package.
        /// </summary>
        /// <param name="outputPath">The file system path of the output directory used for creating the WiX source and MSI.</param>
        /// <param name="manifestPackagePath">The file system path of the NuGet package containing the workload manifest.</param>
        /// <param name="msiVersion">The version of the MSI to create.</param>
        /// <param name="platform">The platform for the MSI.</param>
        /// <param name="allowSideBySideInstalls">Whether MSIs should allow side-by-side installations instead of major upgrades.</param>
        /// <returns>A task item with metadata for the generated MSI.</returns>
        private static ITaskItem BuildManifestMsi(string outputPath, string manifestPackagePath, string msiVersion = "1.2.3", string platform = "x64",
            bool allowSideBySideInstalls = true)
        {
            Directory.CreateDirectory(outputPath);
            TaskItem packageItem = new(manifestPackagePath);
            WorkloadManifestPackage pkg = new(packageItem, Path.Combine(outputPath, "pkg"),
                new Version(msiVersion));
            pkg.Extract();
            WorkloadManifestMsi msi = new(pkg, platform, new MockBuildEngine(), WixToolsetPath,
                outputPath, isSxS: allowSideBySideInstalls);

            return msi.Build(Path.Combine(outputPath, "msi"));
        }

        [WindowsOnlyFact]
        public void ItCanBuildSideBySideManifestMsis()
        {
            string outputDirectory = GetTestCaseDirectory();

            // Build 6.0.200 manifest for version 6.0.3
            ITaskItem msi603 = BuildManifestMsi(outputDirectory, Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.3.nupkg"));
            string msiPath603 = msi603.GetMetadata(Metadata.FullPath);

            // Build 6.0.200 manifest for version 6.0.4
            ITaskItem msi604 = BuildManifestMsi(outputDirectory, Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.4.nupkg"));
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

        [WindowsOnlyTheory]
        [InlineData(true, null, "Microsoft.NET.Workload.Mono.ToolChain,6.0.200,6.0.3,x64")]
        [InlineData(false, "{E4761192-882D-38E9-A3F4-14B6C4AD12BD}", "Microsoft.NET.Workload.Mono.ToolChain,6.0.200,x64")]
        public void ItCanBuildAManifestMsi2(bool allowSideBySideInstalls, string expectedUpgradeCode,
            string expectedProviderKeyName)
        {
            string outputDirectory = GetTestCaseDirectory();
            string wixpackOutputDirectory = Path.Combine(outputDirectory, "wixpack");

            ITaskItem msi = BuildManifestMsi(outputDirectory,
                Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.3.nupkg"),
                allowSideBySideInstalls: allowSideBySideInstalls);

            string msiPath = msi.GetMetadata(Metadata.FullPath);

            // Process the summary information stream's template to extract the MSIs target platform.
            using SummaryInfo si = new(msiPath, enableWrite: false);

            string upgradeCode = MsiUtils.GetProperty(msiPath, MsiProperty.UpgradeCode);

            if (!allowSideBySideInstalls)
            {
                // UpgradeCode is predictable/stable for manifest MSIs that support major upgrades.
                Assert.Equal(expectedUpgradeCode, upgradeCode);

                // Upgrade table should contain two rows, one of which is only used to detect downgrades.
                var relatedProducts = MsiUtils.GetRelatedProducts(msiPath);
                ValidatedRelatedProduct(relatedProducts, $"{expectedUpgradeCode:B}", null, "1.2.3", 1, "WIX_UPGRADE_DETECTED");
                ValidatedRelatedProduct(relatedProducts, $"{expectedUpgradeCode:B}", "1.2.3", null, 2, "WIX_DOWNGRADE_DETECTED");

                // There should be no version directory present if the old upgrade model is used.
                MsiUtils.GetAllDirectories(msiPath).Select(d => d.Directory).Should().NotContain("ManifestVersionDir",
                    "because the manifest MSI supports major upgrades");
            }
            else
            {
                Assert.False(MsiUtils.HasTable(msiPath, "Upgrade"));

                // The versioned manifest directory is required to support SxS installs.
                MsiUtils.GetAllDirectories(msiPath).Select(d => d.Directory).Should().Contain("ManifestVersionDir",
                    "because the manifest MSI supports major upgrades");
            }

            Assert.Equal("1.2.3", MsiUtils.GetProperty(msiPath, MsiProperty.ProductVersion));
            // The same ProviderKey is used across different versions when upgrades are supported,
            // but for SxS installs, the package version is included to differentiate it.
            Assert.Equal(expectedProviderKeyName, MsiUtils.GetProviderKeyName(msiPath));
            Assert.Equal("x64;1033", si.Template);

            // Verify the installation record and dependency provider registry entries
            var registryKeys = MsiUtils.GetAllRegistryKeys(msiPath);
            string expectedProductCode = MsiUtils.GetProperty(msiPath, MsiProperty.ProductCode);
            string installationRecordKey = @"SOFTWARE\Microsoft\dotnet\InstalledManifests\x64\Microsoft.NET.Workload.Mono.ToolChain.Manifest-6.0.200\6.0.3";
            string dependencyProviderKey = @"Software\Classes\Installer\Dependencies\" + expectedProviderKeyName;

            ValidateInstallationRecord(registryKeys, installationRecordKey,
                expectedProviderKeyName,
                expectedProductCode, upgradeCode, "1.2.3");
            ValidateDependencyProviderKey(registryKeys, dependencyProviderKey);

            // The File table should contain the workload manifest and targets. There may be additional
            // localized content for the manifests. Their presence is neither required nor critical to
            // how workloads functions.
            var files = MsiUtils.GetAllFiles(msiPath);
            files.Should().Contain(f => f.FileName.EndsWith("WorkloadManifest.json"));
            files.Should().Contain(f => f.FileName.EndsWith("WorkloadManifest.targets"));
        }

        [WindowsOnlyFact]
        public void ItCanBuildWorkloadSdkPackMsi()
        {
            string outputDirectory = GetTestCaseDirectory();
            string packageContentsDirectory = Path.Combine(outputDirectory, "pkg");
            string msiOutputDirectory = Path.Combine(outputDirectory, "msi");

            TaskItem packageItem = new(Path.Combine(TestAssetsPath, "microsoft.net.workload.emscripten.manifest-6.0.200.6.0.4.nupkg"));
            WorkloadManifestPackage manifestPackage = new(packageItem, packageContentsDirectory, new Version("1.2.3"));
            // Parse the manifest to extract information related to workload packs so we can extract a specific pack.
            WorkloadManifest manifest = manifestPackage.GetManifest();
            WorkloadPackId packId = new("Microsoft.NET.Runtime.Emscripten.Python");
            WorkloadPack pack = manifest.Packs[packId];

            var sourcePackages = WorkloadPackPackage.GetSourcePackages(TestAssetsPath, pack);
            var sourcePackageInfo = sourcePackages.FirstOrDefault();
            var workloadPackPackage = WorkloadPackPackage.Create(pack, sourcePackageInfo.sourcePackage, sourcePackageInfo.platforms, packageContentsDirectory, null, null);
            workloadPackPackage.Extract();

            var workloadPackMsi = new WorkloadPackMsi(workloadPackPackage, "x64", new MockBuildEngine(),
                WixToolsetPath, outputDirectory);

            // Build the MSI and verify its contents
            var msiItem = workloadPackMsi.Build(msiOutputDirectory);
            string msiPath = msiItem.GetMetadata(Metadata.FullPath);

            // Process the summary information stream's template to extract the MSIs target platform.
            using SummaryInfo si = new(msiPath, enableWrite: false);
            Assert.Equal("x64;1033", si.Template);

            // Verify pack directories
            var directories = MsiUtils.GetAllDirectories(msiPath);
            directories.Select(d => d.Directory).Should().Contain("PackageDir", "because it's an SDK pack");
            directories.Select(d => d.Directory).Should().Contain("InstallDir", "because it's a workload pack");

            // UpgradeCode is predictable/stable for pack MSIs since they are seeded using the package identity (ID & version).
            string upgradeCode = MsiUtils.GetProperty(msiPath, MsiProperty.UpgradeCode);
            Assert.Equal("{BDE8712D-9BD7-3692-9C2A-C518208967D6}", upgradeCode);

            // Verify the installation record and dependency provider registry entries
            var registryKeys = MsiUtils.GetAllRegistryKeys(msiPath);
            string expectedProductCode = MsiUtils.GetProperty(msiPath, MsiProperty.ProductCode);
            string installationRecordKey = @"SOFTWARE\Microsoft\dotnet\InstalledPacks\x64\Microsoft.NET.Runtime.Emscripten.2.0.23.Python.win-x64\6.0.4";
            string dependencyProviderKey = @"Software\Classes\Installer\Dependencies\Microsoft.NET.Runtime.Emscripten.2.0.23.Python.win-x64,6.0.4,x64";

            ValidateInstallationRecord(registryKeys, installationRecordKey,
                "Microsoft.NET.Runtime.Emscripten.2.0.23.Python.win-x64,6.0.4,x64",
                expectedProductCode, upgradeCode, "6.0.4.0");
            ValidateDependencyProviderKey(registryKeys, dependencyProviderKey);
        }

        [WindowsOnlyFact]
        public void ItCanBuildATemplatePackMsi()
        {
            string outputDirectory = GetTestCaseDirectory();
            string pkgDirectory = Path.Combine(outputDirectory, "pkg");
            string msiDirectory = Path.Combine(outputDirectory, "msi");

            string packagePath = Path.Combine(TestAssetsPath, "microsoft.ios.templates.15.2.302-preview.14.122.nupkg");

            WorkloadPack p = new(new WorkloadPackId("Microsoft.iOS.Templates"), "15.2.302-preview.14.122", WorkloadPackKind.Template, null);
            TemplatePackPackage pkg = new(p, packagePath, new[] { "x64" }, pkgDirectory);
            pkg.Extract();
            var buildEngine = new MockBuildEngine();
            WorkloadPackMsi msi = new(pkg, "x64", buildEngine, WixToolsetPath, outputDirectory);
            ITaskItem item = msi.Build(msiDirectory);

            string msiPath = item.GetMetadata(Metadata.FullPath);

            // Process the summary information stream's template to extract the MSIs target platform.
            using SummaryInfo si = new(msiPath, enableWrite: false);

            // UpgradeCode is predictable/stable for pack MSIs since they are seeded using the package identity (ID & version).
            string upgradeCode = MsiUtils.GetProperty(msiPath, MsiProperty.UpgradeCode);
            Assert.Equal("{EC4D6B34-C9DE-3984-97FD-B7AC96FA536A}", upgradeCode);
            // The version is set using the package major.minor.patch
            Assert.Equal("15.2.302.0", MsiUtils.GetProperty(msiPath, MsiProperty.ProductVersion));
            Assert.Equal("Microsoft.iOS.Templates,15.2.302-preview.14.122,x64", MsiUtils.GetProviderKeyName(msiPath));
            Assert.Equal("x64;1033", si.Template);

            // Template packs should pull in the raw nupkg. We can verify this by querying the File table. There should
            // only be a single file.
            FileRow fileRow = MsiUtils.GetAllFiles(msiPath).FirstOrDefault();
            Assert.Contains("microsoft.ios.templates.15.2.302-preview.14.122.nupkg", fileRow.FileName);

            // Verify that the generated component GUID for the template pack is stable. This value
            // should only change if component's keypath changes. The check also checks the foreign key reference
            // from the file row into the component table.
            MsiUtils.GetAllComponents(msiPath).Should().Contain(c =>
                c.ComponentId == "{98827ECA-69A2-5300-A75E-F1A251EB17F9}" &&
                c.Component == fileRow.Component_);

            // Generated MSI should return the path where the .wixobj files are located so
            // WiX packs can be created for post-build signing.
            Assert.NotNull(item.GetMetadata(Metadata.WixObj));

            // Verify the installation record and dependency provider registry entries
            var registryKeys = MsiUtils.GetAllRegistryKeys(msiPath);
            string expectedProductCode = MsiUtils.GetProperty(msiPath, MsiProperty.ProductCode);
            string installationRecordKey = @"SOFTWARE\Microsoft\dotnet\InstalledPacks\x64\Microsoft.iOS.Templates\15.2.302-preview.14.122";
            string dependencyProviderKey = @"Software\Classes\Installer\Dependencies\Microsoft.iOS.Templates,15.2.302-preview.14.122,x64";

            ValidateInstallationRecord(registryKeys, installationRecordKey,
                "Microsoft.iOS.Templates,15.2.302-preview.14.122,x64", expectedProductCode, upgradeCode, "15.2.302.0");
            ValidateDependencyProviderKey(registryKeys, dependencyProviderKey);
        }
    }
}
