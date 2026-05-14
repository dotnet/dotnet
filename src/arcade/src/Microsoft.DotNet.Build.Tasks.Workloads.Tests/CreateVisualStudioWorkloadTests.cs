// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    [Collection("Workload Creation")]
    public class CreateVisualStudioWorkloadTests : TestBase
    {
        [WindowsOnlyFact]
        public static void ItCanCreateWorkloads()
        {
            // Create intermediate outputs under %temp% to avoid path issues and make sure it's clean so we don't pick up
            // conflicting sources from previous runs.
            string baseIntermediateOutputPath = Path.Combine(Path.GetTempPath(), "WL");

            if (Directory.Exists(baseIntermediateOutputPath))
            {
                Directory.Delete(baseIntermediateOutputPath, recursive: true);
            }

            ITaskItem[] manifestsPackages = new[]
            {
                new TaskItem(Path.Combine(TestBase.TestAssetsPath, "microsoft.net.workload.emscripten.manifest-6.0.200.6.0.4.nupkg"))
                .WithMetadata(Metadata.MsiVersion, "6.33.28")
            };

            ITaskItem[] componentResources = new[]
            {
                new TaskItem("microsoft-net-sdk-emscripten")
                .WithMetadata(Metadata.Title, ".NET WebAssembly Build Tools (Emscripten)")
                .WithMetadata(Metadata.Description, "Build tools for WebAssembly ahead-of-time (AoT) compilation and native linking.")
                .WithMetadata(Metadata.Version, "5.6.7.8")
            };

            ITaskItem[] shortNames = new[]
            {
                new TaskItem("Microsoft.NET.Workload.Emscripten").WithMetadata("Replacement", "Emscripten"),
                new TaskItem("microsoft.netcore.app.runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("Microsoft.NETCore.App.Runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("microsoft.net.runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("Microsoft.NET.Runtime").WithMetadata("Replacement", "Microsoft")
            };

            IBuildEngine buildEngine = new MockBuildEngine();

            CreateVisualStudioWorkload createWorkloadTask = new CreateVisualStudioWorkload()
            {
                AllowMissingPacks = true,
                BaseOutputPath = TestBase.BaseOutputPath,
                BaseIntermediateOutputPath = baseIntermediateOutputPath,
                BuildEngine = buildEngine,
                ComponentResources = componentResources,
                ManifestMsiVersion = null,
                PackageSource = TestBase.TestAssetsPath,
                ShortNames = shortNames,
                WixToolsetPath = TestBase.WixToolsetPath,
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
            string manifestMsiSwr = File.ReadAllText(Path.Combine(baseIntermediateOutputPath, "src", "swix", "6.0.200", "Emscripten.Manifest-6.0.200", "x64", "msi.swr"));
            Assert.Contains("package name=Emscripten.Manifest-6.0.200", manifestMsiSwr);
            Assert.Contains("vs.package.type=msi", manifestMsiSwr);
            Assert.Contains("vs.package.chip=x64", manifestMsiSwr);
            Assert.DoesNotContain("vs.package.machineArch", manifestMsiSwr);
            Assert.DoesNotContain("vs.package.outOfSupport", manifestMsiSwr);

            // Verify that no arm64 MSI authoring for VS. EMSDK doesn't define RIDs for arm64, but manifests always generate
            // arm64 MSIs for the CLI based installs so we should not see that.
            string swixRootDirectory = Path.Combine(baseIntermediateOutputPath, "src", "swix", "6.0.200");
            IEnumerable<string> arm64Directories = Directory.EnumerateDirectories(swixRootDirectory, "arm64", SearchOption.AllDirectories);
            Assert.DoesNotContain(arm64Directories, s => s.Contains("arm64"));

            // Verify the SWIX authoring for one of the workload pack MSIs. Packs get assigned random sub-folders so we
            // need to filter out the SWIX project output items the task produced.
            ITaskItem pythonPackSwixItem = createWorkloadTask.SwixProjects.Where(s => s.ItemSpec.Contains(@"Microsoft.Emscripten.Python.6.0.4\x64")).FirstOrDefault();
            string packMsiSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(pythonPackSwixItem.ItemSpec), "msi.swr"));
            Assert.Contains("package name=Microsoft.Emscripten.Python.6.0.4", packMsiSwr);
            Assert.Contains("vs.package.chip=x64", packMsiSwr);
            Assert.Contains("vs.package.outOfSupport=yes", packMsiSwr);
            Assert.DoesNotContain("vs.package.machineArch", packMsiSwr);

            // Verify the swix project items for components. The project files names always contain the major.minor suffix, so we'll end up
            // with microsoft.net.sdk.emscripten.5.6.swixproj and microsoft.net.sdk.emscripten.pre.5.6.swixproj
            IEnumerable<ITaskItem> swixComponentProjects = createWorkloadTask.SwixProjects.Where(s => s.GetMetadata(Metadata.PackageType).Equals(DefaultValues.PackageTypeComponent));
            Assert.All(swixComponentProjects, c => Assert.True(c.ItemSpec.Contains(".pre.") && c.GetMetadata(Metadata.IsPreview) == "true" ||
                !c.ItemSpec.Contains(".pre.") && c.GetMetadata(Metadata.IsPreview) == "false"));
        }

        [WindowsOnlyFact]
        public static void ItCanCreateWorkloadsThatSupportArm64InVisualStudio()
        {
            // Create intermediate outputs under %temp% to avoid path issues and make sure it's clean so we don't pick up
            // conflicting sources from previous runs.
            string baseIntermediateOutputPath = Path.Combine(Path.GetTempPath(), "WLa64");

            if (Directory.Exists(baseIntermediateOutputPath))
            {
                Directory.Delete(baseIntermediateOutputPath, recursive: true);
            }

            ITaskItem[] manifestsPackages = new[]
            {
                new TaskItem(Path.Combine(TestBase.TestAssetsPath, "microsoft.net.workload.emscripten.manifest-6.0.200.6.0.4.nupkg"))
                .WithMetadata(Metadata.MsiVersion, "6.33.28")
                .WithMetadata(Metadata.SupportsMachineArch, "true")
            };

            ITaskItem[] componentResources = new[]
            {
                new TaskItem("microsoft-net-sdk-emscripten")
                .WithMetadata(Metadata.Title, ".NET WebAssembly Build Tools (Emscripten)")
                .WithMetadata(Metadata.Description, "Build tools for WebAssembly ahead-of-time (AoT) compilation and native linking.")
                .WithMetadata(Metadata.Version, "5.6.7.8")
            };

            ITaskItem[] shortNames = new[]
            {
                new TaskItem("Microsoft.NET.Workload.Emscripten").WithMetadata("Replacement", "Emscripten"),
                new TaskItem("microsoft.netcore.app.runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("Microsoft.NETCore.App.Runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("microsoft.net.runtime").WithMetadata("Replacement", "Microsoft"),
                new TaskItem("Microsoft.NET.Runtime").WithMetadata("Replacement", "Microsoft")
            };

            IBuildEngine buildEngine = new MockBuildEngine();

            CreateVisualStudioWorkload createWorkloadTask = new CreateVisualStudioWorkload()
            {
                AllowMissingPacks = true,
                BaseOutputPath = TestBase.BaseOutputPath,
                BaseIntermediateOutputPath = baseIntermediateOutputPath,
                BuildEngine = buildEngine,
                ComponentResources = componentResources,
                ManifestMsiVersion = null,
                PackageSource = TestBase.TestAssetsPath,
                ShortNames = shortNames,
                WixToolsetPath = TestBase.WixToolsetPath,
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
            string manifestMsiSwr = File.ReadAllText(Path.Combine(baseIntermediateOutputPath, "src", "swix", "6.0.200", "Emscripten.Manifest-6.0.200", "arm64", "msi.swr"));
            Assert.Contains("package name=Emscripten.Manifest-6.0.200", manifestMsiSwr);
            Assert.Contains("vs.package.type=msi", manifestMsiSwr);
            Assert.DoesNotContain("vs.package.chip", manifestMsiSwr);
            Assert.Contains("vs.package.machineArch=arm64", manifestMsiSwr);
        }
    }
}
