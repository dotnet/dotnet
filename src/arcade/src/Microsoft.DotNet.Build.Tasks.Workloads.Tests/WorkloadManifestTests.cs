// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Deployment.DotNet.Releases;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public class WorkloadManifestTests : TestBase
    {
        [WindowsOnlyTheory]
        [InlineData("Microsoft.NET.Workload.Emscripten.net6.Manifest-8.0.100-alpha.1", "8.0.100-alpha.1")]
        [InlineData("Microsoft.NET.Workload.Emscripten.Manifest-8.0.100-alpha.1.23062.6", "8.0.100-alpha.1.23062.6")]
        public static void ItExtractsTheSdkVersionFromTheManifestPackageId(string packageId, string expectedVersion)
        {
            string actualSdkVersion = WorkloadManifestPackage.GetSdkVersion(packageId);

            Assert.Equal(expectedVersion, actualSdkVersion);
        }

        [WindowsOnlyTheory]
        [InlineData("8.0.100-alpha.1", "8.0.100-alpha.1")]
        [InlineData("8.0.100-preview.1", "8.0.100-preview.1")]
        [InlineData("8.0.100-dev.1", "8.0.100")]
        [InlineData("7.0.203", "7.0.200")]
        [InlineData("8.0.100-alpha.1.23062.6", "8.0.100-alpha.1")]
        [InlineData("8.0.101-alpha.1.23062.6", "8.0.100-alpha.1")]
        public static void ItConvertsTheManifestSdkVersionToAFeatureBandVersion(string sdkVersion, string expectedVersion)
        {
            ReleaseVersion actualFeatureBandVersion = WorkloadManifestPackage.GetSdkFeatureBandVersion(sdkVersion);

            Assert.Equal(expectedVersion, $"{actualFeatureBandVersion}");
        }
    }
}
