// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Microsoft.NET.Sdk.WorkloadManifestReader;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    [Collection("6.0.200 Toolchain manifest tests")]
    public class MsiTests : TestBase
    {
        [WindowsOnlyFact]        
        public void ItCanBuildAManifestMsi()
        {
            string PackageRootDirectory = Path.Combine(BaseIntermediateOutputPath, "pkg");
            TaskItem packageItem = new(Path.Combine(TestAssetsPath, "microsoft.net.workload.mono.toolchain.manifest-6.0.200.6.0.3.nupkg"));
            WorkloadManifestPackage pkg = new(packageItem, PackageRootDirectory, new Version("1.2.3"));
            pkg.Extract();
            WorkloadManifestMsi msi = new(pkg, "x64", new MockBuildEngine(), WixToolsetPath, BaseIntermediateOutputPath);

            ITaskItem item = msi.Build(MsiOutputPath);

            string msiPath = item.GetMetadata(Metadata.FullPath);

            // Process the summary information stream's template to extract the MSIs target platform.
            using SummaryInfo si = new(msiPath, enableWrite: false);

            // UpgradeCode is predictable/stable for manifest MSIs.
            Assert.Equal("{E4761192-882D-38E9-A3F4-14B6C4AD12BD}", MsiUtils.GetProperty(msiPath, MsiProperty.UpgradeCode));
            Assert.Equal("1.2.3", MsiUtils.GetProperty(msiPath, MsiProperty.ProductVersion));
            Assert.Equal("Microsoft.NET.Workload.Mono.ToolChain,6.0.200,x64", MsiUtils.GetProviderKeyName(msiPath));
            Assert.Equal("x64;1033", si.Template);

            // Generated MSI should return the path where the .wixobj files are located so
            // WiX packs can be created for post-build signing.
            Assert.NotNull(item.GetMetadata(Metadata.WixObj));
        }

        [WindowsOnlyFact]
        public void ItCanBuildATemplatePackMsi()
        {
            string PackageRootDirectory = Path.Combine(BaseIntermediateOutputPath, "pkg");
            string packagePath = Path.Combine(TestAssetsPath, "microsoft.ios.templates.15.2.302-preview.14.122.nupkg");

            WorkloadPack p = new(new WorkloadPackId("Microsoft.iOS.Templates"), "15.2.302-preview.14.122", WorkloadPackKind.Template, null);
            TemplatePackPackage pkg = new(p, packagePath, new[] {"x64"}, PackageRootDirectory);
            pkg.Extract();
            WorkloadPackMsi msi = new(pkg, "x64", new MockBuildEngine(), WixToolsetPath, BaseIntermediateOutputPath);

            ITaskItem item = msi.Build(MsiOutputPath);

            string msiPath = item.GetMetadata(Metadata.FullPath);

            // Process the summary information stream's template to extract the MSIs target platform.
            using SummaryInfo si = new(msiPath, enableWrite: false);

            // UpgradeCode is predictable/stable for pack MSIs since they are seeded using the package identity (ID & version).
            Assert.Equal("{EC4D6B34-C9DE-3984-97FD-B7AC96FA536A}", MsiUtils.GetProperty(msiPath, MsiProperty.UpgradeCode));
            // The version is set using the package major.minor.patch
            Assert.Equal("15.2.302.0", MsiUtils.GetProperty(msiPath, MsiProperty.ProductVersion));
            Assert.Equal("Microsoft.iOS.Templates,15.2.302-preview.14.122,x64", MsiUtils.GetProviderKeyName(msiPath));
            Assert.Equal("x64;1033", si.Template);

            // Template packs should pull in the raw nupkg. We can verify by query the File table. There should
            // only be a single file.
            FileRow fileRow = MsiUtils.GetAllFiles(msiPath).FirstOrDefault();
            Assert.Contains("microsoft.ios.templates.15.2.302-preview.14.122.nupk", fileRow.FileName);

            // Generated MSI should return the path where the .wixobj files are located so
            // WiX packs can be created for post-build signing.
            Assert.NotNull(item.GetMetadata(Metadata.WixObj));
        }
    }
}
