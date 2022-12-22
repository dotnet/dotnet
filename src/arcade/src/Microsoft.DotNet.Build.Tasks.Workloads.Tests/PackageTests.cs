// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.DotNet.Build.Tasks.Workloads;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.DotNet.Releases;
using System.IO;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    [Collection("6.0.200 Toolchain manifest tests")]
    public class PackageTests : TestBase
    {
        [WindowsOnlyFact]
        public void ItCanReadAManifestPackage()
        {
            string PackageRootDirectory = Path.Combine(BaseIntermediateOutputPath, "pkg");

            TaskItem manifestPackageItem = new(Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.3.nupkg"));
            WorkloadManifestPackage p = new(manifestPackageItem, PackageRootDirectory, new Version("1.2.3"));

            ReleaseVersion expectedFeatureBand = new("6.0.200");

            Assert.Equal("Microsoft.NET.Workload.Mono.ToolChain", p.ManifestId);
            Assert.Equal(expectedFeatureBand, p.SdkFeatureBand);
        }
    }
}
