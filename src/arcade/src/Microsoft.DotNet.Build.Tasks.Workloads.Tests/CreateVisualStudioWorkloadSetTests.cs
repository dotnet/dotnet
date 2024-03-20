// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public class CreateVisualStudioWorkloadSetTests : TestBase
    {
        [WindowsOnlyFact]
        public static void ItCanCreateWorkloadSets()
        {
            // Create intermediate outputs under %temp% to avoid path issues and make sure it's clean so we don't pick up
            // conflicting sources from previous runs.
            string baseIntermediateOutputPath = Path.Combine(Path.GetTempPath(), "WLS");

            if (Directory.Exists(baseIntermediateOutputPath))
            {
                Directory.Delete(baseIntermediateOutputPath, recursive: true);
            }

            ITaskItem[] workloadSetPackages = new[]
            {
                new TaskItem(Path.Combine(TestAssetsPath, "microsoft.net.workloads.9.0.100.9.0.100-baseline.1.23464.1.nupkg"))
                .WithMetadata(Metadata.MsiVersion, "12.8.45")
            };

            IBuildEngine buildEngine = new MockBuildEngine();

            CreateVisualStudioWorkloadSet createWorkloadSetTask = new CreateVisualStudioWorkloadSet()
            {
                BaseOutputPath = BaseOutputPath,
                BaseIntermediateOutputPath = baseIntermediateOutputPath,
                BuildEngine = buildEngine,
                WixToolsetPath = WixToolsetPath,
                WorkloadSetPackageFiles = workloadSetPackages
            };

            Assert.True(createWorkloadSetTask.Execute());

            // Spot check the x64 generated MSI.
            ITaskItem msi = createWorkloadSetTask.Msis.Where(i => i.GetMetadata(Metadata.Platform) == "x64").FirstOrDefault();
            Assert.NotNull(msi);

            // Verify the workload set records the CLI will use.
            MsiUtils.GetAllRegistryKeys(msi.ItemSpec).Should().Contain(r =>
                r.Root == 2 &&
                r.Key == @"SOFTWARE\Microsoft\dotnet\InstalledWorkloadSets\x64\9.0.100\9.0.100-baseline.1.23464.1" &&
                r.Name == "ProductVersion" &&
                r.Value == "12.8.45");

            // Workload sets are SxS. Verify that we don't have an Upgrade table.
            Assert.False(MsiUtils.HasTable(msi.ItemSpec, "Upgrade"));

            // Verify the workloadset version directory and only look at the long name version.
            DirectoryRow versionDir = MsiUtils.GetAllDirectories(msi.ItemSpec).FirstOrDefault(d => string.Equals(d.Directory, "WorkloadSetVersionDir"));
            Assert.NotNull(versionDir);
            Assert.Contains("|9.0.0.100-baseline.1.23464.1", versionDir.DefaultDir);

            // Verify the SWIX authoring for one of the workload set MSIs. 
            ITaskItem workloadSetSwixItem = createWorkloadSetTask.SwixProjects.Where(s => s.ItemSpec.Contains(@"Microsoft.NET.Workloads.9.0.100.9.0.100-baseline.1.23464.1\x64")).FirstOrDefault();
            Assert.Equal(DefaultValues.PackageTypeMsiWorkloadSet, workloadSetSwixItem.GetMetadata(Metadata.PackageType));

            string msiSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(workloadSetSwixItem.ItemSpec), "msi.swr"));
            Assert.Contains("package name=Microsoft.NET.Workloads.9.0.100.9.0.100-baseline.1.23464.1", msiSwr);
            Assert.Contains("version=12.8.45", msiSwr);
            Assert.DoesNotContain("vs.package.chip=x64", msiSwr);
            Assert.Contains("vs.package.machineArch=x64", msiSwr);
            Assert.Contains("vs.package.type=msi", msiSwr);

            // Verify package group SWIX project
            ITaskItem workloadSetPackageGroupSwixItem = createWorkloadSetTask.SwixProjects.Where(
                s => s.GetMetadata(Metadata.PackageType).Equals(DefaultValues.PackageTypeWorkloadSetPackageGroup)).
                FirstOrDefault();
            string packageGroupSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(workloadSetPackageGroupSwixItem.ItemSpec), "packageGroup.swr"));
            Assert.Contains("package name=PackageGroup.NET.Workloads-9.0.100", packageGroupSwr);
            Assert.Contains("vs.dependency id=Microsoft.NET.Workloads.9.0.100.9.0.100-baseline.1.23464.1", packageGroupSwr);
        }
    }
}
