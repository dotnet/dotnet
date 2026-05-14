// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Swix;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public class SwixPackageGroupTests : TestBase
    {
        private static readonly ITaskItem[] s_shortNames = new[]
        {
            new TaskItem("Microsoft.NET.Workload.").WithMetadata("Replacement", ""),
        };

        [WindowsOnlyTheory, MemberData(nameof(PackageGroupData))]
        public void ItGeneratesPackageGroupsForManifestPackages(string manifestPackageFilename, string destinationDirectory, Version msiVersion, ITaskItem[] shortNames,
            string expectedPackageId, Version expectedVersion, string expectedManifestDependency, string expectedFeatureBand)
        {
            string destinationBaseDirectory = Path.Combine(BaseIntermediateOutputPath, destinationDirectory);
            TaskItem manifestPackageItem = new(Path.Combine(TestAssetsPath, manifestPackageFilename));
            WorkloadManifestPackage manifestPackage = new(manifestPackageItem, destinationBaseDirectory, msiVersion, shortNames, null, isSxS: true);
            var packageGroup = new SwixPackageGroup(manifestPackage);
            var packageGroupItem = PackageGroupSwixProject.CreateProjectItem(packageGroup, BaseIntermediateOutputPath, BaseOutputPath,
                DefaultValues.PackageTypeManifestPackageGroup);

            // Verify package group expectations
            Assert.Equal(expectedPackageId, packageGroup.Name);
            Assert.Equal(expectedVersion, packageGroup.Version);

            // Verify the generate SWIX authoring
            string packageGroupSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(packageGroupItem.ItemSpec), "packageGroup.swr"));
            Assert.Contains(expectedManifestDependency, packageGroupSwr);
            Assert.Contains("vs.package.type=group", packageGroupSwr);

            // Verify the task item metadata
            Assert.Equal(expectedFeatureBand, packageGroupItem.GetMetadata(Metadata.SdkFeatureBand));
            Assert.Equal(DefaultValues.PackageTypeManifestPackageGroup, packageGroupItem.GetMetadata(Metadata.PackageType));
        }

        public static readonly IEnumerable<object[]> PackageGroupData = new List<object[]>
        {
            new object[] { "microsoft.net.workload.mono.toolchain.manifest-6.0.300.6.0.21.nupkg", "grp1", 
                new Version("1.2.3"), s_shortNames, "PackageGroup.Mono.ToolChain.Manifest-6.0.300", new Version("1.2.3"),
                "  vs.dependency id=Mono.ToolChain.Manifest-6.0.300.6.0.21", "6.0.300" },
            new object[] { "microsoft.net.workload.emscripten.net6.manifest-8.0.100-preview.6.8.0.0-preview.6.23326.2.nupkg", "grp2",
                new Version("1.2.3"), s_shortNames, "PackageGroup.Emscripten.net6.Manifest-8.0.100", new Version("1.2.3"),
                "  vs.dependency id=Emscripten.net6.Manifest-8.0.100-preview.6.8.0.0-preview.6.23326.2", "8.0.100-preview.6" },
        };
    }
}
