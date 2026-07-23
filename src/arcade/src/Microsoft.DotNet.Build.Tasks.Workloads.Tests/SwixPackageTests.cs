// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Deployment.DotNet.Releases;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Microsoft.DotNet.Build.Tasks.Workloads.Swix;
using Microsoft.NET.Sdk.WorkloadManifestReader;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    [Collection("SWIX Package")]
    public class SwixPackageTests : TestBase
    {
        [WindowsOnlyFact]
        public void ItThrowsIfPackageRelativePathExceedsLimit()
        {
            TaskItem msiItem = new TaskItem("Microsoft.NET.Workload.Mono.ToolChain.Manifest-6.0.100.6.0.0-preview.7.21377.12-x64.msi");
            msiItem.SetMetadata(Metadata.SwixPackageId, "Microsoft.NET.Workload.Mono.ToolChain.Manifest-6.0.100");
            msiItem.SetMetadata(Metadata.Version, "6.0.0.0");
            msiItem.SetMetadata(Metadata.Platform, "x64");

            Exception e = Assert.Throws<Exception>(() =>
            {
                MsiSwixProject swixProject = new(msiItem, GetTestCaseDirectory(), BaseOutputPath,
                    new ReleaseVersion("6.0.100"),
                    chip: "x64", machineArch: "x64", productArch: "neutral");
            });

            Assert.Equal(@"Relative package path exceeds the maximum length (182): Microsoft.NET.Workload.Mono.ToolChain.Manifest-6.0.100,version=6.0.0.0,chip=x64,productarch=neutral,machinearch=x64\Microsoft.NET.Workload.Mono.ToolChain.Manifest-6.0.100.6.0.0-preview.7.21377.12-x64.msi.", e.Message);
        }

        [WindowsOnlyFact]
        public void ItOnlyIncludesDefinedPropertiesForMsiPackages()
        {
            string packageVersion = "16.0.527";
            string outputDirectory = GetTestCaseDirectory();
            string pkgDirectory = Path.Combine(outputDirectory, "pkg");
            string msiDirectory = Path.Combine(outputDirectory, "msi");
            string packagePath = Path.Combine(TestAssetsPath, $"microsoft.ios.templates.{packageVersion}.nupkg");

            WorkloadPack p = new(new WorkloadPackId("Microsoft.iOS.Templates"), packageVersion, WorkloadPackKind.Template, null);
            TemplatePackPackage pkg = new(p, packagePath, ["x64"], pkgDirectory);
            pkg.Extract();
            WorkloadPackMsi msi = new(pkg, "x64", new MockBuildEngine(), WixToolsetConfig, outputDirectory);

            ITaskItem msiItem = msi.Build(msiDirectory);
            msiItem.SetMetadata(Metadata.Platform, "x64");

            Assert.Equal($"Microsoft.iOS.Templates.{packageVersion}", msiItem.GetMetadata(Metadata.SwixPackageId));

            MsiSwixProject swixProject = new(msiItem, outputDirectory, BaseOutputPath,
                new ReleaseVersion("6.0.100"),
                chip: msiItem.GetMetadata(Metadata.Platform),
                machineArch: DefaultValues.x64);
            string swixProj = swixProject.Create();
            string msiSwr = File.ReadAllText(Path.Combine(Path.GetDirectoryName(swixProj), "msi.swr"));

            Assert.DoesNotContain("vs.package.productArch", msiSwr);
            Assert.Contains("vs.package.chip=x64", msiSwr);
            Assert.Contains("vs.package.machineArch=x64", msiSwr);
        }
    }
}
