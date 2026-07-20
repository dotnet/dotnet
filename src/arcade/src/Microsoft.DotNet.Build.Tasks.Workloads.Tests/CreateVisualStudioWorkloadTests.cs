// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using WixToolset.Dtf.WindowsInstaller;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    [Collection("Workload Creation")]
    public class CreateVisualStudioWorkloadTests : TestBase
    {
        [SkipOnCI(reason: "This test builds the full WASM workload.")]
        [WindowsOnlyFact]
        public void ItCreatesPackGroups()
        {
            string packageSource = Path.Combine(TestAssetsPath, "wasm");
            // Create intermediate outputs under %temp% to avoid path issues and make sure it's clean so we don't pick up
            // conflicting sources from previous runs.
            string testCaseDirectory = GetTestCaseDirectory();
            string baseIntermediateOutputPath = testCaseDirectory;

            ITaskItem[] manifestsPackages =
            {
                new TaskItem(Path.Combine(packageSource, "microsoft.net.workload.mono.toolchain.current.manifest-10.0.100.10.0.100.nupkg"))
                .WithMetadata(Metadata.MsiVersion, "10.0.456")
            };

            IBuildEngine buildEngine = new MockBuildEngine();
            CreateVisualStudioWorkload createWorkloadTask = new CreateVisualStudioWorkload()
            {
                AllowMissingPacks = true,
                BaseOutputPath = Path.Combine(testCaseDirectory, "bin"),
                BaseIntermediateOutputPath = baseIntermediateOutputPath,
                BuildEngine = buildEngine,
                ComponentResources = Array.Empty<ITaskItem>(),
                CreateWorkloadPackGroups = true,
                DisableParallelPackageGroupProcessing = false,
                IsOutOfSupportInVisualStudio = false,
                ManifestMsiVersion = null,
                PackageSource = packageSource,
                ShortNames = Array.Empty<ITaskItem>(),
                WixExe = ToolsetInfo.WixExePath,
                HeatExe = ToolsetInfo.HeatExePath,
                WixExtensions = WixExtensions,
                WorkloadManifestPackageFiles = manifestsPackages
            };

            bool result = createWorkloadTask.Execute();
            Assert.True(result);

            // Verify that the Visual Studio workload components reference workload pack groups.
            string componentSwr = File.ReadAllText(
                Path.Combine(Path.GetDirectoryName(
                    createWorkloadTask.SwixProjects.FirstOrDefault(
                        i => i.ItemSpec.Contains("wasm.tools.10.0.swixproj")).ItemSpec), "component.swr"));
            Assert.Contains("vs.dependency id=wasm.tools.WorkloadPacks", componentSwr);

            // Manifest installers should contain additional JSON files describing pack groups.
            ITaskItem manifestMsi = createWorkloadTask.Msis.First(m => m.GetMetadata(Metadata.PackageType) == "manifest");
            MsiUtils.GetAllFiles(manifestMsi.ItemSpec).Should().Contain(f => f.FileName.EndsWith("WorkloadPackGroups.json"));

            // Verify the package group JSON and ensure there are no duplicates.
            var json = File.ReadAllText(Path.Combine(manifestMsi.GetMetadata(Metadata.SourcePath), "json", "WorkloadPackGroups.json"));
            var groupIds = JsonSerializer.Deserialize<JsonElement>(json).EnumerateArray();
            groupIds.Count().Should().Be(groupIds.Distinct().Count(), "because there should be no duplicate pack group IDs");

            // Verify that the workload component contains a reference to a workload pack group package.
            string iosComponentSwr = File.ReadAllText(
                Path.Combine(Path.GetDirectoryName(
                    createWorkloadTask.SwixProjects.FirstOrDefault(
                        i => i.ItemSpec.Contains("microsoft.net.runtime.ios.10.0.swixproj")).ItemSpec), "component.swr"));
            iosComponentSwr.Should().Contain("vs.dependency id=microsoft.net.runtime.ios.WorkloadPacks");
        }

        [WindowsOnlyFact]
        public void ItCanCreateWorkloads()
        {
            // Create intermediate outputs under %temp% to avoid path issues and make sure it's clean so we don't pick up
            // conflicting sources from previous runs.
            string testCaseDirectory = GetTestCaseDirectory();
            string baseIntermediateOutputPath = testCaseDirectory;

            ITaskItem[] manifestsPackages =
            [
                new TaskItem(Path.Combine(TestAssetsPath, "microsoft.net.workload.emscripten.manifest-6.0.200.6.0.4.nupkg"))
                .WithMetadata(Metadata.MsiVersion, "6.33.28")
            ];

            ITaskItem[] componentResources =
            [
                new TaskItem("microsoft-net-sdk-emscripten")
                .WithMetadata(Metadata.Title, ".NET WebAssembly Build Tools (Emscripten)")
                .WithMetadata(Metadata.Description, "Build tools for WebAssembly ahead-of-time (AoT) compilation and native linking.")
                .WithMetadata(Metadata.Version, "5.6.7.8")
            ];

            ITaskItem[] shortNames =
            [
                new TaskItem("Microsoft.NET.Workload.Emscripten").WithMetadata("Replacement", "Emscripten"),
                new TaskItem("microsoft.netcore.app.runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("Microsoft.NETCore.App.Runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("microsoft.net.runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("Microsoft.NET.Runtime").WithMetadata("Replacement", "Microsoft")
            ];

            IBuildEngine buildEngine = new MockBuildEngine();

            CreateVisualStudioWorkload createWorkloadTask = new CreateVisualStudioWorkload()
            {
                AllowMissingPacks = true,
                BaseOutputPath = Path.Combine(testCaseDirectory, "bin"),
                BaseIntermediateOutputPath = baseIntermediateOutputPath,
                BuildEngine = buildEngine,
                ComponentResources = componentResources,
                ManifestMsiVersion = null,
                PackageSource = TestAssetsPath,
                ShortNames = shortNames,
                WixExe = ToolsetInfo.WixExePath,
                HeatExe = ToolsetInfo.HeatExePath,
                WixExtensions = WixExtensions,
                WorkloadManifestPackageFiles = manifestsPackages,
                IsOutOfSupportInVisualStudio = true
            };

            bool result = createWorkloadTask.Execute();

            Assert.True(result);
            ITaskItem manifestMsiItem = createWorkloadTask.Msis.Where(m => m.ItemSpec.ToLowerInvariant().Contains("d96ba8044ad35589f97716ecbf2732fb-x64.msi")).FirstOrDefault();
            Assert.NotNull(manifestMsiItem);

            // Spot check one of the manifest MSIs. We have additional tests that cover MSI generation.
            // The UpgradeCode is predictable/stable for manifest MSIs since they are upgradable withing an SDK feature band,
            Assert.Equal("{C4F269D9-6B65-36C5-9556-75B78EFE9EDA}", MsiUtils.GetProperty(manifestMsiItem.ItemSpec, MsiProperty.UpgradeCode));
            // The version should match the value passed to the build task. For actual builds like dotnet/runtiem, this value would
            // be generated.
            Assert.Equal("6.33.28", MsiUtils.GetProperty(manifestMsiItem.ItemSpec, MsiProperty.ProductVersion));
            Assert.Equal("Microsoft.NET.Workload.Emscripten,6.0.200,x64", MsiUtils.GetProviderKeyName(manifestMsiItem.ItemSpec));

            // Process the template in the summary information stream. This is the only way to verify the intended platform
            // of the MSI itself.
            using SummaryInfo si = new(manifestMsiItem.ItemSpec, enableWrite: false);
            Assert.Equal("x64;1033", si.Template);

            // Verify the SWIX authoring for the component representing the workload in VS. The first should be a standard
            // component. There should also be a second preview component.
            string componentSwr = File.ReadAllText(
                Path.Combine(Path.GetDirectoryName(
                    createWorkloadTask.SwixProjects.FirstOrDefault(
                        i => i.ItemSpec.Contains("microsoft.net.sdk.emscripten.5.6.swixproj")).ItemSpec), "component.swr"));
            Assert.Contains("package name=microsoft.net.sdk.emscripten", componentSwr);
            string previewComponentSwr = File.ReadAllText(
                Path.Combine(Path.GetDirectoryName(
                    createWorkloadTask.SwixProjects.FirstOrDefault(
                        i => i.ItemSpec.Contains("microsoft.net.sdk.emscripten.pre.5.6.swixproj")).ItemSpec), "component.swr"));
            Assert.Contains("package name=microsoft.net.sdk.emscripten.pre", previewComponentSwr);

            // Emscripten is an abstract workload so it should be a component group.
            Assert.Contains("vs.package.type=component", componentSwr);
            Assert.Contains("vs.package.outOfSupport=yes", componentSwr);
            Assert.Contains("isUiGroup=yes", componentSwr);
            Assert.Contains("version=5.6.7.8", componentSwr);

            Assert.Contains("vs.package.type=component", previewComponentSwr);
            Assert.Contains("isUiGroup=yes", previewComponentSwr);
            Assert.Contains("version=5.6.7.8", previewComponentSwr);

            // Verify pack dependencies. These should map to MSI packages. The VS package IDs should be the non-aliased
            // pack IDs and version from the workload manifest. The actual VS packages will point to the MSIs generated from the
            // aliased workload pack packages. 
            Assert.Contains("vs.dependency id=Microsoft.Emscripten.Node.6.0.4", componentSwr);
            Assert.Contains("vs.dependency id=Microsoft.Emscripten.Python.6.0.4", componentSwr);
            Assert.Contains("vs.dependency id=Microsoft.Emscripten.Sdk.6.0.4", componentSwr);

            // Pack dependencies for preview components should be identical to the non-preview component.
            Assert.Contains("vs.dependency id=Microsoft.Emscripten.Node.6.0.4", previewComponentSwr);
            Assert.Contains("vs.dependency id=Microsoft.Emscripten.Python.6.0.4", previewComponentSwr);
            Assert.Contains("vs.dependency id=Microsoft.Emscripten.Sdk.6.0.4", previewComponentSwr);

            // Verify the SWIX authoring for the VS package wrapping the manifest MSI
            string manifestMsiSwr = File.ReadAllText(
                Path.Combine(Path.GetDirectoryName(
                    createWorkloadTask.SwixProjects.FirstOrDefault(
                        i => i.GetMetadata(Metadata.PackageType) == DefaultValues.PackageTypeMsiManifest).ItemSpec), "msi.swr"));
            Assert.Contains("package name=Emscripten.Manifest-6.0.200", manifestMsiSwr);
            Assert.Contains("vs.package.type=msi", manifestMsiSwr);
            Assert.Contains("vs.package.chip=", manifestMsiSwr);
            Assert.DoesNotContain("vs.package.machineArch", manifestMsiSwr);
            Assert.DoesNotContain("vs.package.outOfSupport", manifestMsiSwr);

            // There should be no SWIX projects generated targeting arm64 when VS does not support it.
            createWorkloadTask.SwixProjects.Where(s => s.GetMetadata(Metadata.Platform) == "arm64").Should().BeEmpty();

            // Verify the SWIX authoring for one of the workload pack MSIs. Packs get assigned random sub-folders so we
            // need to filter out the SWIX project output items the task produced.
            ITaskItem pythonPackSwixItem = createWorkloadTask.SwixProjects.FirstOrDefault(s =>
                s.GetMetadata(Metadata.PackageType) == DefaultValues.PackageTypeMsiPack &&
                s.GetMetadata(Metadata.Platform) == "x64" &&
                s.GetMetadata(Metadata.SwixPackageId).Contains("Python"));
            string packMsiSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(pythonPackSwixItem.ItemSpec), "msi.swr"));
            Assert.Contains("package name=Microsoft.Emscripten.Python.6.0.4", packMsiSwr);
            Assert.Contains("vs.package.chip=x64", packMsiSwr);
            Assert.Contains("vs.package.outOfSupport=yes", packMsiSwr);
            Assert.DoesNotContain("vs.package.machineArch", packMsiSwr);

            string m = File.ReadAllText(
                Path.Combine(Path.GetDirectoryName(
                    createWorkloadTask.SwixProjects.FirstOrDefault(
                        i => i.GetMetadata(Metadata.PackageType) == DefaultValues.PackageTypeMsiManifest).ItemSpec), "msi.swr"));

            // Verify the swix project items for components. The project files names always contain the major.minor suffix, so we'll end up
            // with microsoft.net.sdk.emscripten.5.6.swixproj and microsoft.net.sdk.emscripten.pre.5.6.swixproj
            IEnumerable<ITaskItem> swixComponentProjects = createWorkloadTask.SwixProjects.Where(s => s.GetMetadata(Metadata.PackageType).Equals(DefaultValues.PackageTypeComponent));
            Assert.All(swixComponentProjects, c => Assert.True(c.ItemSpec.Contains(".pre.") && c.GetMetadata(Metadata.IsPreview) == "true" ||
                !c.ItemSpec.Contains(".pre.") && c.GetMetadata(Metadata.IsPreview) == "false"));
        }

        [WindowsOnlyFact]
        public void ItCanCreateWorkloadsThatSupportArm64InVisualStudio()
        {
            // Create intermediate outputs under %temp% to avoid path issues and make sure it's clean so we don't pick up
            // conflicting sources from previous runs.
            string testCaseDirectory = GetTestCaseDirectory();
            string baseIntermediateOutputPath = testCaseDirectory;

            ITaskItem[] manifestsPackages =
            [
                new TaskItem(Path.Combine(TestBase.TestAssetsPath, "microsoft.net.workload.emscripten.manifest-6.0.200.6.0.4.nupkg"))
                .WithMetadata(Metadata.MsiVersion, "6.33.28")
                .WithMetadata(Metadata.SupportsMachineArch, "true")
            ];

            ITaskItem[] componentResources =
            [
                new TaskItem("microsoft-net-sdk-emscripten")
                .WithMetadata(Metadata.Title, ".NET WebAssembly Build Tools (Emscripten)")
                .WithMetadata(Metadata.Description, "Build tools for WebAssembly ahead-of-time (AoT) compilation and native linking.")
                .WithMetadata(Metadata.Version, "5.6.7.8")
            ];

            ITaskItem[] shortNames =
            [
                new TaskItem("Microsoft.NET.Workload.Emscripten").WithMetadata("Replacement", "Emscripten"),
                new TaskItem("microsoft.netcore.app.runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("Microsoft.NETCore.App.Runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("microsoft.net.runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("Microsoft.NET.Runtime").WithMetadata("Replacement", "Microsoft")
            ];

            IBuildEngine buildEngine = new MockBuildEngine();

            CreateVisualStudioWorkload createWorkloadTask = new CreateVisualStudioWorkload()
            {
                AllowMissingPacks = true,
                BaseOutputPath = Path.Combine(testCaseDirectory, "bin"),
                BaseIntermediateOutputPath = baseIntermediateOutputPath,
                BuildEngine = buildEngine,
                ComponentResources = componentResources,
                ManifestMsiVersion = null,
                PackageSource = TestAssetsPath,
                ShortNames = shortNames,
                WixExe = ToolsetInfo.WixExePath,
                HeatExe = ToolsetInfo.HeatExePath,
                WixExtensions = WixExtensions,
                WorkloadManifestPackageFiles = manifestsPackages,
            };

            bool result = createWorkloadTask.Execute();

            Assert.True(result);
            ITaskItem manifestMsiItem = createWorkloadTask.Msis.Where(m => m.ItemSpec.ToLowerInvariant().Contains("d96ba8044ad35589f97716ecbf2732fb-arm64.msi")).FirstOrDefault();
            Assert.NotNull(manifestMsiItem);

            // Spot check one of the manifest MSIs. We have additional tests that cover MSI generation.
            // The UpgradeCode is predictable/stable for manifest MSIs since they are upgradable withing an SDK feature band,
            Assert.Equal("{CBA7CF4A-F3C9-3B75-8F1F-0D08AF7CD7BE}", MsiUtils.GetProperty(manifestMsiItem.ItemSpec, MsiProperty.UpgradeCode));
            // The version should match the value passed to the build task. For actual builds like dotnet/runtiem, this value would
            // be generated.
            Assert.Equal("6.33.28", MsiUtils.GetProperty(manifestMsiItem.ItemSpec, MsiProperty.ProductVersion));
            Assert.Equal("Microsoft.NET.Workload.Emscripten,6.0.200,arm64", MsiUtils.GetProviderKeyName(manifestMsiItem.ItemSpec));

            // Process the template in the summary information stream. This is the only way to verify the intended platform
            // of the MSI itself.
            using SummaryInfo si = new(manifestMsiItem.ItemSpec, enableWrite: false);
            Assert.Equal("Arm64;1033", si.Template);

            // Verify the SWIX authoring for the component representing the workload in VS.
            string componentSwr = File.ReadAllText(
                Path.Combine(Path.GetDirectoryName(
                    createWorkloadTask.SwixProjects.FirstOrDefault(
                        i => i.ItemSpec.Contains("microsoft.net.sdk.emscripten.5.6.swixproj")).ItemSpec), "component.swr"));
            Assert.Contains("package name=microsoft.net.sdk.emscripten", componentSwr);

            // Emscripten is an abstract workload so it should be a component group.
            Assert.Contains("vs.package.type=component", componentSwr);
            Assert.Contains("isUiGroup=yes", componentSwr);
            Assert.Contains("version=5.6.7.8", componentSwr);
            // Default setting should be off
            Assert.Contains("vs.package.outOfSupport=no", componentSwr);

            // Verify pack dependencies. These should map to MSI packages. The VS package IDs should be the non-aliased
            // pack IDs and version from the workload manifest. The actual VS packages will point to the MSIs generated from the
            // aliased workload pack packages. 
            Assert.Contains("vs.dependency id=Microsoft.Emscripten.Node.6.0.4", componentSwr);
            Assert.Contains("vs.dependency id=Microsoft.Emscripten.Python.6.0.4", componentSwr);
            Assert.Contains("vs.dependency id=Microsoft.Emscripten.Sdk.6.0.4", componentSwr);

            // Verify the SWIX authoring for the VS package wrapping the manifest MSI
            string manifestMsiSwr = File.ReadAllText(
                Path.Combine(Path.GetDirectoryName(
                    createWorkloadTask.SwixProjects.FirstOrDefault(
                        i => i.GetMetadata(Metadata.PackageType) == DefaultValues.PackageTypeMsiManifest).ItemSpec), "msi.swr"));
            Assert.Contains("package name=Emscripten.Manifest-6.0.200", manifestMsiSwr);
            Assert.Contains("vs.package.type=msi", manifestMsiSwr);
            Assert.DoesNotContain("vs.package.chip", manifestMsiSwr);
            Assert.Contains("vs.package.machineArch=", manifestMsiSwr);
        }
    }
}
