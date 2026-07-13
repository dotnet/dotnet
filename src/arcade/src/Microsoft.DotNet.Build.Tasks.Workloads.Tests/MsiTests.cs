// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.DotNet.Releases;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Microsoft.DotNet.Build.Tasks.Workloads.Swix;
using Microsoft.NET.Sdk.WorkloadManifestReader;
using WixToolset.Dtf.WindowsInstaller;
using Xunit;
using static Microsoft.DotNet.Build.Tasks.Workloads.Msi.WorkloadManifestMsi;

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
            WorkloadManifestMsi msi = new(pkg, platform, new MockBuildEngine(), WixToolsetConfig,
                outputPath, isSxS: allowSideBySideInstalls);

            return msi.Build(Path.Combine(outputPath, "msi"));
        }

        [WindowsOnlyFact]
        public void ItCanBuildSideBySideManifestMsis()
        {
            using var fixture = new MsiTestFixture();

            // Build 6.0.200 manifest for version 6.0.3
            ITaskItem msi603 = BuildManifestMsi(fixture.OutputPath, Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.3.nupkg"));
            string msiPath603 = msi603.GetMetadata(Metadata.FullPath);

            // Build 6.0.200 manifest for version 6.0.4
            ITaskItem msi604 = BuildManifestMsi(fixture.OutputPath, Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.4.nupkg"));
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
        }

        [WindowsOnlyTheory]
        [InlineData(true, null, "Microsoft.NET.Workload.Mono.ToolChain,6.0.200,6.0.3,x64")]
        [InlineData(true, null, "Microsoft.NET.Workload.Mono.ToolChain,6.0.200,6.0.3,x86", "x86")]
        [InlineData(false, "{E4761192-882D-38E9-A3F4-14B6C4AD12BD}", "Microsoft.NET.Workload.Mono.ToolChain,6.0.200,x64")]
        [InlineData(false, "{239D1181-C3CE-3E9E-91FE-3A645B0077B2}", "Microsoft.NET.Workload.Mono.ToolChain,6.0.200,arm64", "arm64")]
        public void ItCanBuildAManifestMsi(bool allowSideBySideInstalls, string expectedUpgradeCode,
            string expectedProviderKeyName, string platform = "x64")
        {
            using var fixture = new MsiTestFixture(true);

            ITaskItem msi = BuildManifestMsi(fixture.OutputPath,
                Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.3.nupkg"),
                platform: platform,
                allowSideBySideInstalls: allowSideBySideInstalls);

            msi.GetMetadata(Metadata.PackageType).Should().Be("manifest", "because we're building a manifest MSI");

            string msiPath = msi.GetMetadata(Metadata.FullPath);

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
                // We can technically remove the Upgrade table by setting Package@UpgradeStrategy="none", but even
                // for SxS installs we still write some information to the JSON manifest that the CLI uses and the
                // absence of the table might have unforseen consequences with VS authoring and SWIX toolset.

                // The versioned manifest directory is required to support SxS installs.
                MsiUtils.GetAllDirectories(msiPath).Select(d => d.Directory).Should().Contain("ManifestVersionDir",
                    "because the manifest MSI supports major upgrades");
            }

            Assert.Equal("1.2.3", MsiUtils.GetProperty(msiPath, MsiProperty.ProductVersion));
            // The same ProviderKey is used across different versions when upgrades are supported,
            // but for SxS installs, the package version is included to differentiate it.
            Assert.Equal(expectedProviderKeyName, MsiUtils.GetProviderKeyName(msiPath));

            // Process the summary information stream's template to extract the MSIs target platform.
            ValidateSummaryInformation(msiPath, platform);

            // Verify the installation record and dependency provider registry entries
            var registryKeys = MsiUtils.GetAllRegistryKeys(msiPath);
            string expectedProductCode = MsiUtils.GetProperty(msiPath, MsiProperty.ProductCode);
            string installationRecordKey = $@"SOFTWARE\Microsoft\dotnet\InstalledManifests\{platform}\Microsoft.NET.Workload.Mono.ToolChain.Manifest-6.0.200\6.0.3";
            string dependencyProviderKey = @"Software\Classes\Installer\Dependencies\" + expectedProviderKeyName;

            ValidateInstallationRecord(registryKeys, installationRecordKey,
                expectedProviderKeyName,
                expectedProductCode, upgradeCode, "1.2.3");
            ValidateDependencyProviderKey(registryKeys, dependencyProviderKey);

            var customActions = MsiUtils.GetCustomActions(msiPath);
            ValidateDotNetHomeCustomActions(customActions, platform);

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
            using var fixture = new MsiTestFixture(true);

            TaskItem packageItem = new(Path.Combine(TestAssetsPath, "microsoft.net.workload.emscripten.manifest-6.0.200.6.0.4.nupkg"));
            WorkloadManifestPackage manifestPackage = new(packageItem, fixture.PackagePath, new Version("1.2.3"));
            // Parse the manifest to extract information related to workload packs so we can extract a specific pack.
            WorkloadManifest manifest = manifestPackage.GetManifest();
            WorkloadPackId packId = new("Microsoft.NET.Runtime.Emscripten.Python");
            WorkloadPack pack = manifest.Packs[packId];

            var sourcePackages = WorkloadPackPackage.GetSourcePackages(TestAssetsPath, pack);
            var sourcePackageInfo = sourcePackages.FirstOrDefault();
            var workloadPackPackage = WorkloadPackPackage.Create(pack, sourcePackageInfo.sourcePackage, sourcePackageInfo.platforms, fixture.PackagePath, null, null);
            workloadPackPackage.Extract();

            var workloadPackMsi = new WorkloadPackMsi(workloadPackPackage, "x64", new MockBuildEngine(),
                WixToolsetConfig, fixture.OutputPath);

            // Build the MSI and verify its contents
            var msiItem = workloadPackMsi.Build(fixture.MsiPath);
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
            if (string.IsNullOrEmpty(MSBuildExePath))
                return; // machine doesn't have native toolchain

            using var fixture = new MsiTestFixture(true);

            string packagePath = Path.Combine(TestAssetsPath, "microsoft.ios.templates.15.2.302-preview.14.122.nupkg");

            WorkloadPack p = new(new WorkloadPackId("Microsoft.iOS.Templates"), "15.2.302-preview.14.122", WorkloadPackKind.Template, null);
            TemplatePackPackage pkg = new(p, packagePath, ["x64"], fixture.PackagePath);
            pkg.Extract();
            var buildEngine = new MockBuildEngine();
            WorkloadPackMsi msi = new(pkg, "x64", buildEngine, WixToolsetConfig, fixture.OutputPath, createWixPack: true);
            ITaskItem msiItem = msi.Build(fixture.MsiPath);

            msiItem.GetMetadata(Metadata.PackageType).Should().Be("pack", "because we're building a template pack MSI");

            // Just do basic sanity checks. Validating the contents of the wixpack is outside the scope of this test.
            string wixPack = msiItem.GetMetadata(Metadata.WixPack);
            wixPack.Should().NotBeEmpty("because we're generating a wixpack for signing");
            File.Exists(wixPack).Should().BeTrue("because the wixpack path should point to an existing file");

            string msiPath = msiItem.GetMetadata(Metadata.FullPath);

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

            // Verify the installation record and dependency provider registry entries
            var registryKeys = MsiUtils.GetAllRegistryKeys(msiPath);
            string expectedProductCode = MsiUtils.GetProperty(msiPath, MsiProperty.ProductCode);
            string installationRecordKey = @"SOFTWARE\Microsoft\dotnet\InstalledPacks\x64\Microsoft.iOS.Templates\15.2.302-preview.14.122";
            string dependencyProviderKey = @"Software\Classes\Installer\Dependencies\Microsoft.iOS.Templates,15.2.302-preview.14.122,x64";

            ValidateInstallationRecord(registryKeys, installationRecordKey,
                "Microsoft.iOS.Templates,15.2.302-preview.14.122,x64", expectedProductCode, upgradeCode, "15.2.302.0");
            ValidateDependencyProviderKey(registryKeys, dependencyProviderKey);

            // Generate a SWIX project and build it.
            MsiSwixProject swixProject = new(msiItem, fixture.OutputPath, fixture.OutputPath,
                ReleaseVersion.Parse("10.0.100"), chip: null, machineArch: msiItem.GetMetadata(Metadata.Platform));

            string swixProj = swixProject.Create();
            // Output path for the SWIX JSON manifest
            string swixManifestOutputPath = Path.Combine(fixture.OutputPath, "swix");
            string swixManifestPath = Path.Combine(swixManifestOutputPath, Path.GetFileNameWithoutExtension(swixProj) + ".json");

            BuildSwixProject(swixProj, swixManifestOutputPath);

            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            var json = File.ReadAllText(swixManifestPath);
            var doc = JsonSerializer.Deserialize<JsonElement>(json, options);

            var package = doc.GetProperty("packages")
                .EnumerateArray()
                .First();

            // WiX 4 introduced a breaking change where the provider key table was renamed. Older versions of
            // SWIX would fail to read the provider key from the MSI and the property would be missing. 
            package.TryGetProperty("providerKey", out JsonElement providerKey)
                .Should().BeTrue("the package should contain a 'providerKey' property");
            providerKey.GetString()
                .Should().Be("Microsoft.iOS.Templates,15.2.302-preview.14.122,x64");
        }

        [WindowsOnlyFact]
        public void ItCanBuildAWorkPackGroupMsi()
        {
            using var fixture = new MsiTestFixture(true);

            string packageSource = Path.Combine(TestAssetsPath, "wasm");

            TaskItem packageItem = new(Path.Combine(packageSource, "microsoft.net.workload.mono.toolchain.current.manifest-10.0.100.10.0.100.nupkg"));
            WorkloadManifestPackage manifestPackage = new(packageItem, fixture.PackagePath, new Version("1.2.3"));
            // Parse the manifest to extract information related to workload packs so we can extract a specific
            // workload.
            WorkloadManifest manifest = manifestPackage.GetManifest();
            WorkloadId workloadId = new("wasm-tools");
            WorkloadDefinition workload = (WorkloadDefinition)manifest.Workloads[workloadId];

            string packGroupId = null;
            WorkloadPackGroupJson packGroupJson = null;

            packGroupId = WorkloadPackGroupPackage.GetPackGroupId(workload.Id);
            packGroupJson = new WorkloadPackGroupJson()
            {
                GroupPackageId = packGroupId,
                GroupPackageVersion = manifestPackage.PackageVersion.ToString()
            };

            List<WorkloadPackPackage> workloadPackPackages = [];

            foreach (WorkloadPackId packId in workload.Packs)
            {
                WorkloadPack pack = manifest.Packs[packId];

                packGroupJson.Packs.Add(new WorkloadPackJson()
                {
                    PackId = pack.Id,
                    PackVersion = pack.Version
                });

                string sourcePackage = WorkloadPackPackage.GetSourcePackage(packageSource, pack, "x64");

                if (!string.IsNullOrWhiteSpace(sourcePackage))
                {
                    workloadPackPackages.Add(WorkloadPackPackage.Create(pack, sourcePackage, ["x64"],
                        fixture.PackagePath, null, null));
                }
            }

            var groupPackage = new WorkloadPackGroupPackage(workload.Id, manifestPackage);
            groupPackage.Packs.AddRange(workloadPackPackages);
            groupPackage.ManifestsPerPlatform["x64"] = new([manifestPackage]);

            var buildEngine = new MockBuildEngine();

            workloadPackPackages.ForEach(p => p.Extract());

            WorkloadPackGroupMsi msi = new(groupPackage, "x64", buildEngine, WixToolsetConfig, fixture.OutputPath);

            ITaskItem msiItem = msi.Build(fixture.MsiPath);

            msiItem.GetMetadata(Metadata.PackageType).Should().Be("pack-group", "because we're building a workload pack group MSI");

            string msiPath = msiItem.GetMetadata(Metadata.FullPath);

            // UpgradeCode is predictable/stable for pack MSIs since they are seeded using the package identity (ID & version).
            string upgradeCode = MsiUtils.GetProperty(msiPath, MsiProperty.UpgradeCode);
            Assert.Equal("{31FEB1C7-842A-33EE-8267-2E46C6DCF3D4}", upgradeCode);

            // Verify the installation record and dependency provider registry entries
            var registryKeys = MsiUtils.GetAllRegistryKeys(msiPath);
            string expectedProductCode = MsiUtils.GetProperty(msiPath, MsiProperty.ProductCode);
            string installationRecordKey = @"SOFTWARE\Microsoft\dotnet\InstalledPackGroups\x64\wasm.tools.WorkloadPacks\10.0.100";
            string dependencyProviderKey = @"Software\Classes\Installer\Dependencies\wasm.tools.WorkloadPacks,10.0.100,x64";

            ValidateInstallationRecord(registryKeys, installationRecordKey,
                "wasm.tools.WorkloadPacks,10.0.100,x64", expectedProductCode, upgradeCode, "1.2.3");
            ValidateDependencyProviderKey(registryKeys, dependencyProviderKey);

            // Pack groups carry additional keys for each pack in the group.
            ValidatePackGroupInstallRecordKeys(registryKeys,
                @"SOFTWARE\Microsoft\dotnet\InstalledPackGroups\x64\wasm.tools.WorkloadPacks\10.0.100\Microsoft.NET.Runtime.WebAssembly.Sdk\10.0.0",
                @"SOFTWARE\Microsoft\dotnet\InstalledPackGroups\x64\wasm.tools.WorkloadPacks\10.0.100\Microsoft.NET.Sdk.WebAssembly.Pack\10.0.0",
                @"SOFTWARE\Microsoft\dotnet\InstalledPackGroups\x64\wasm.tools.WorkloadPacks\10.0.100\Microsoft.NETCore.App.Runtime.Mono.browser-wasm\10.0.0",
                @"SOFTWARE\Microsoft\dotnet\InstalledPackGroups\x64\wasm.tools.WorkloadPacks\10.0.100\Microsoft.NETCore.App.Runtime.AOT.win-x64.Cross.browser-wasm\10.0.0");
        }
    }
}
