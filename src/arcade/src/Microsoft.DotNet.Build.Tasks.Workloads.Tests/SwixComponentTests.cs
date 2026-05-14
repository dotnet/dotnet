// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.DotNet.Releases;
using Microsoft.DotNet.Build.Tasks.Workloads.Swix;
using Microsoft.NET.Sdk.WorkloadManifestReader;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public class SwixComponentTests : TestBase
    {
        public string RandomPath => Path.Combine(AppContext.BaseDirectory, "obj", Path.GetFileNameWithoutExtension(Path.GetTempFileName()));

        [WindowsOnlyFact]
        public void ItAssignsDefaultValues()
        {
            WorkloadManifest manifest = Create("WorkloadManifest.json");
            WorkloadDefinition workload = (WorkloadDefinition)manifest.Workloads.FirstOrDefault().Value;
            SwixComponent component = SwixComponent.Create(new ReleaseVersion("6.0.300"), workload, manifest, packGroupId: null);

            ComponentSwixProject project = new(component, BaseIntermediateOutputPath, BaseOutputPath);
            string swixProj = project.Create();

            string componentSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.swr"));
            Assert.Contains("package name=microsoft.net.sdk.blazorwebassembly.aot", componentSwr);
            Assert.Contains("version=1.0.0", componentSwr);

            string componentResSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.res.swr"));
            Assert.Contains(@"title=""Blazor WebAssembly AOT workload""", componentResSwr);
            Assert.Contains(@"description=""Blazor WebAssembly AOT workload""", componentResSwr);
            Assert.Contains(@"category="".NET""", componentResSwr);
        }

        [WindowsOnlyFact]
        public void ItCanAdvertiseComponents()
        {
            ITaskItem[] componentResources = new[]
            {
                new TaskItem("microsoft-net-sdk-blazorwebassembly-aot").WithMetadata(Metadata.Version, "4.5.6")
                .WithMetadata(Metadata.Description, "A long wordy description about Blazor.")
                .WithMetadata(Metadata.Category, "WebAssembly")
                .WithMetadata(Metadata.AdvertisePackage, "true")
            };

            WorkloadManifest manifest = Create("WorkloadManifest.json");
            WorkloadDefinition workload = (WorkloadDefinition)manifest.Workloads.FirstOrDefault().Value;
            SwixComponent component = SwixComponent.Create(new ReleaseVersion("6.0.300"), workload, manifest, packGroupId: null,
                componentResources);

            ComponentSwixProject project = new(component, BaseIntermediateOutputPath, BaseOutputPath);
            string swixProj = project.Create();

            string componentSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.swr"));
            Assert.Contains("package name=microsoft.net.sdk.blazorwebassembly.aot", componentSwr);
            Assert.Contains("version=4.5.6", componentSwr);
            Assert.Contains("isAdvertisedPackage=yes", componentSwr);

            string componentResSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.res.swr"));
            Assert.Contains(@"title=""Blazor WebAssembly AOT workload""", componentResSwr);
            Assert.Contains(@"description=""A long wordy description about Blazor.""", componentResSwr);
            Assert.Contains(@"category=""WebAssembly""", componentResSwr);
        }

        [WindowsOnlyFact]
        public void ItPrefersComponentResourcesOverDefaults()
        {
            ITaskItem[] componentResources = new[] 
            {
                new TaskItem("microsoft-net-sdk-blazorwebassembly-aot").WithMetadata(Metadata.Version, "4.5.6")
                .WithMetadata(Metadata.Description, "A long wordy description about Blazor.")
                .WithMetadata(Metadata.Category, "WebAssembly")
            };

            WorkloadManifest manifest = Create("WorkloadManifest.json");
            WorkloadDefinition workload = (WorkloadDefinition)manifest.Workloads.FirstOrDefault().Value;
            SwixComponent component = SwixComponent.Create(new ReleaseVersion("6.0.300"), workload, manifest, packGroupId: null,
                componentResources);

            ComponentSwixProject project = new(component, BaseIntermediateOutputPath, BaseOutputPath);
            string swixProj = project.Create();

            string componentSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.swr"));
            Assert.Contains("package name=microsoft.net.sdk.blazorwebassembly.aot", componentSwr);
            Assert.Contains("version=4.5.6", componentSwr);
            Assert.Contains("isAdvertisedPackage=no", componentSwr);

            string componentResSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.res.swr"));
            Assert.Contains(@"title=""Blazor WebAssembly AOT workload""", componentResSwr);
            Assert.Contains(@"description=""A long wordy description about Blazor.""", componentResSwr);
            Assert.Contains(@"category=""WebAssembly""", componentResSwr);
        }

        [WindowsOnlyFact]
        public void ItShortensComponentIds()
        {
            ITaskItem[] shortNames = new TaskItem[]
            {
                new TaskItem("Microsoft.NET.Runtime", new Dictionary<string, string> { { Metadata.Replacement, "MSFT" } })
            };

            WorkloadManifest manifest = Create("WorkloadManifest.json");
            WorkloadDefinition workload = (WorkloadDefinition)manifest.Workloads.FirstOrDefault().Value;
            SwixComponent component = SwixComponent.Create(new ReleaseVersion("6.0.300"), workload, manifest, packGroupId: null, shortNames: shortNames);

            ComponentSwixProject project = new(component, BaseIntermediateOutputPath, BaseOutputPath);
            string swixProj = project.Create();

            string componentSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.swr"));
            Assert.Contains("vs.dependency id=MSFT.MonoAOTCompiler.Task.6.0.0-preview.4.21201.1", componentSwr);
        }

        [WindowsOnlyFact]
        public void ItIgnoresNonApplicableDepedencies()
        {
            WorkloadManifest manifest = Create("AbstractWorkloadsNonWindowsPacks.json");
            WorkloadDefinition workload = (WorkloadDefinition)manifest.Workloads.FirstOrDefault().Value;
            SwixComponent component = SwixComponent.Create(new ReleaseVersion("6.0.300"), workload, manifest, packGroupId: null, null, null);

            ComponentSwixProject project = new(component, BaseIntermediateOutputPath, BaseOutputPath);
            string swixProj = project.Create();

            string componentSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.swr"));
            Assert.Contains(@"package name=microsoft.net.runtime.ios", componentSwr);
            Assert.DoesNotContain(@"vs.dependency id=Microsoft.NETCore.App.Runtime.AOT.Cross.ios-arm", componentSwr);
            Assert.DoesNotContain(@"vs dependency id=Microsoft.NETCore.App.Runtime.AOT.Cross.ios-arm64", componentSwr);
            Assert.DoesNotContain(@"vs dependency id=Microsoft.NETCore.App.Runtime.AOT.Cross.iossimulator-arm64", componentSwr);
            Assert.DoesNotContain(@"vs dependency id=Microsoft.NETCore.App.Runtime.AOT.Cross.iossimulator-x64", componentSwr);
            Assert.DoesNotContain(@"vs dependency id=Microsoft.NETCore.App.Runtime.AOT.Cross.iossimulator-x86", componentSwr);
            Assert.Contains(@"vs.dependency id=runtimes.ios", componentSwr);
        }

        [WindowsOnlyFact]
        public void ItCanOverrideDefaultValues()
        {
            WorkloadManifest manifest = Create("WorkloadManifest.json");
            WorkloadDefinition workload = (WorkloadDefinition)manifest.Workloads.FirstOrDefault().Value;

            ITaskItem[] componentResources = new ITaskItem[]
            {
                new TaskItem("microsoft-net-sdk-blazorwebassembly-aot", new Dictionary<string, string> {
                    { "Title", "AOT" },
                    { "Description", "A long wordy description." },
                    { "Category", "Compilers, build tools, and runtimes" }
                })
            };

            SwixComponent component = SwixComponent.Create(new ReleaseVersion("6.0.300"), workload, manifest, packGroupId: null, componentResources);
            ComponentSwixProject project = new(component, BaseIntermediateOutputPath, BaseOutputPath);
            string swixProj = project.Create();

            string componentResSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.res.swr"));

            Assert.Contains(@"title=""AOT""", componentResSwr);
            Assert.Contains(@"description=""A long wordy description.""", componentResSwr);
            Assert.Contains(@"category=""Compilers, build tools, and runtimes""", componentResSwr);
        }

        [Fact]
        public void ItCreatesComponentsWhenWorkloadsDoNotIncludePacks()
        {
            WorkloadManifest manifest = Create("mauiWorkloadManifest.json");
            WorkloadDefinition workload = (WorkloadDefinition)manifest.Workloads.FirstOrDefault().Value;
            SwixComponent component = SwixComponent.Create(new ReleaseVersion("7.0.100"), workload, manifest, packGroupId: null);
            ComponentSwixProject project = new(component, BaseIntermediateOutputPath, BaseOutputPath);
            string swixProj = project.Create();

            string componentSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.swr"));
            Assert.Contains(@"vs.dependency id=maui.mobile", componentSwr);
            Assert.Contains(@"vs.dependency id=maui.desktop", componentSwr);
        }

        [Fact]
        public void ItCreatesDependenciesForPackGroup()
        {
            WorkloadManifest manifest = Create("WorkloadManifest.json");
            WorkloadDefinition workload = (WorkloadDefinition)manifest.Workloads.FirstOrDefault().Value;
            var packGroupId = "microsoft.net.sdk.blazorwebassembly.aot.WorkloadPacks";
            SwixComponent component = SwixComponent.Create(new ReleaseVersion("7.0.100"), workload, manifest, packGroupId: packGroupId);
            ComponentSwixProject project = new(component, BaseIntermediateOutputPath, BaseOutputPath);
            string swixProj = project.Create();

            string componentSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "component.swr"));

            //  Should have only one dependency, use string.Split to check how many times vs.dependency occurs in swr
            Assert.Equal(2, componentSwr.Split(new[] { "vs.dependency" }, StringSplitOptions.None).Length);
            Assert.Contains($"vs.dependency id={packGroupId}", componentSwr);
        }

        private static WorkloadManifest Create(string filename)
        {
            return WorkloadManifestReader.ReadWorkloadManifest(Path.GetFileNameWithoutExtension(filename),
                File.OpenRead(Path.Combine(TestAssetsPath, filename)), filename);
        }
    }
}
