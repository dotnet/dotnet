// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Linq;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using WixToolset.Dtf.WindowsInstaller;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public class CreateVisualStudioWorkloadSetTests : TestBase
    {
        [WindowsOnlyFact]
        public void ItCanCreateWorkloadSets()
        {
            // Create intermediate outputs under %temp% to avoid path issues and make sure it's clean so we don't pick up
            // conflicting sources from previous runs.
            string testCaseDirectory = GetTestCaseDirectory();
            string baseIntermediateOutputPath = testCaseDirectory;

            if (Directory.Exists(baseIntermediateOutputPath))
            {
                Directory.Delete(baseIntermediateOutputPath, recursive: true);
            }

            ITaskItem[] workloadSetPackages =
            [
                new TaskItem(Path.Combine(TestAssetsPath, "microsoft.net.workloads.9.0.100.9.0.100-baseline.1.23464.1.nupkg"))
                .WithMetadata(Metadata.MsiVersion, "12.8.45")
            ];

            var buildEngine = new MockBuildEngine();

            CreateVisualStudioWorkloadSet createWorkloadSetTask = new CreateVisualStudioWorkloadSet()
            {
                BaseOutputPath = Path.Combine(testCaseDirectory, "msi"),
                BaseIntermediateOutputPath = baseIntermediateOutputPath,
                BuildEngine = buildEngine,
                WorkloadSetPackageFiles = workloadSetPackages,
                WixExe = ToolsetInfo.WixExePath,
                HeatExe = ToolsetInfo.HeatExePath,
                WixExtensions = WixExtensions,
            };

            Assert.True(createWorkloadSetTask.Execute(), buildEngine.BuildErrorEvents.Count > 0 ?
                buildEngine.BuildErrorEvents[0].Message : "Task failed. No error events");

            // Validate the arm64 installer.
            ITaskItem arm64Msi = createWorkloadSetTask.Msis.FirstOrDefault(i => i.GetMetadata(Metadata.Platform) == "arm64");
            Assert.NotNull(arm64Msi);
            ITaskItem x64Msi = createWorkloadSetTask.Msis.FirstOrDefault(i => i.GetMetadata(Metadata.Platform) == "x64");
            Assert.NotNull(x64Msi);

            var arm64MsiPath = arm64Msi.ItemSpec;
            var x64MsiPath = x64Msi.ItemSpec;

            // Process the summary information stream's template to extract the MSIs target platform.
            using SummaryInfo si = new(arm64MsiPath, enableWrite: false);
            Assert.Equal("Arm64;1033", si.Template);

            // Upgrades are not supported, but we do generated stable GUIDs based on various
            // properties including the target platform.
            string upgradeCode = MsiUtils.GetProperty(arm64MsiPath, MsiProperty.UpgradeCode);
            Assert.Equal("{A05B88DE-F40F-3C20-B6DA-719B8EED1D9F}", upgradeCode);
            // Make sure the x64 and arm64 MSIs have different UpgradeCode properties.
            string x64UpgradeCode = MsiUtils.GetProperty(x64MsiPath, MsiProperty.UpgradeCode);
            Assert.NotEqual(upgradeCode, x64UpgradeCode);

            // Verify the installation record and dependency provider registry entries.
            var registryKeys = MsiUtils.GetAllRegistryKeys(arm64MsiPath);
            string productCode = MsiUtils.GetProperty(arm64MsiPath, MsiProperty.ProductCode);
            string installationRecordKey = @"SOFTWARE\Microsoft\dotnet\InstalledWorkloadSets\arm64\9.0.100\9.0.100-baseline.1.23464.1";
            string dependencyProviderKey = @"Software\Classes\Installer\Dependencies\Microsoft.NET.Workload.Set,9.0.100,9.0.100-baseline.1.23464.1,arm64";

            // ProductCode and UpgradeCode values in the installation record should match the 
            // values from the Property table.
            ValidateInstallationRecord(registryKeys, installationRecordKey,
                "Microsoft.NET.Workload.Set,9.0.100,9.0.100-baseline.1.23464.1,arm64",
                productCode, upgradeCode, "12.8.45");
            ValidateDependencyProviderKey(registryKeys, dependencyProviderKey);

            // Verify the workloadset version directory and only look at the long name version.
            DirectoryRow versionDir = MsiUtils.GetAllDirectories(arm64MsiPath).FirstOrDefault(d => string.Equals(d.Directory, "WorkloadSetVersionDir"));
            Assert.NotNull(versionDir);
            Assert.Contains("|9.0.0.100-baseline.1.23464.1", versionDir.DefaultDir);

            // Verify that the workloadset.json exists.
            var files = MsiUtils.GetAllFiles(arm64MsiPath);
            files.Should().Contain(f => f.FileName.EndsWith("|workloadset.json"));

            // Verify the SWIX authoring for one of the workload set MSIs. 
            ITaskItem workloadSetSwixItem = createWorkloadSetTask.SwixProjects.FirstOrDefault(
                s => s.GetMetadata(Metadata.PackageType) == DefaultValues.PackageTypeMsiWorkloadSet);
            string msiSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(workloadSetSwixItem.ItemSpec), "msi.swr"));
            Assert.Contains("package name=Microsoft.NET.Workloads.9.0.100.9.0.100-baseline.1.23464.1", msiSwr);
            Assert.Contains("version=12.8.45", msiSwr);
            Assert.Contains("vs.package.type=msi", msiSwr);

            // Verify package group SWIX project
            ITaskItem workloadSetPackageGroupSwixItem = createWorkloadSetTask.SwixProjects.FirstOrDefault(
                s => s.GetMetadata(Metadata.PackageType).Equals(DefaultValues.PackageTypeWorkloadSetPackageGroup));
            string packageGroupSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(workloadSetPackageGroupSwixItem.ItemSpec), "packageGroup.swr"));
            Assert.Contains("package name=PackageGroup.NET.Workloads-9.0.100", packageGroupSwr);
            Assert.Contains("vs.dependency id=Microsoft.NET.Workloads.9.0.100.9.0.100-baseline.1.23464.1", packageGroupSwr);
        }
    }
}
