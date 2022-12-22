// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using Microsoft.DotNet.Tools.Tests.Utilities;
using Xunit;

namespace Microsoft.DotNet.Tools.Tests
{
    public class VersionUpdaterTests
    {
        private static void AssertPackagesEqual(byte[] expected, byte[] actual)
        {
            // Compare parts of the packages.
            // The zip archive contains file time stamps hence comparing raw bits directly is impractical.

            (string name, byte[] blob)[] GetPackageParts(byte[] packageBytes)
            {
                using (var package = new ZipArchive(new MemoryStream(packageBytes), ZipArchiveMode.Read))
                {
                    return package.Entries.Select(e =>
                    {
                        using (var s = e.Open())
                        {
                            var m = new MemoryStream();
                            s.CopyTo(m);
                            return (e.FullName, m.ToArray());
                        }
                    }).ToArray();
                }
            }

            var expectedParts = GetPackageParts(expected);
            var actualParts = GetPackageParts(actual);

            Assert.Equal(expectedParts.Length, actualParts.Length);
            for (int i = 0; i < expectedParts.Length; i++)
            {
                Assert.Equal(expectedParts[i].name, actualParts[i].name);
                AssertEx.Equal(expectedParts[i].blob, actualParts[i].blob);

                // all parts of test packages are XML documents, test that they can be loaded:
                XDocument.Load(new MemoryStream(actualParts[i].blob));
            }
        }

        // As part of repacking, certain files are updated and rewritten. When this occurs line endings
        // change to match the platform that is executing. The reference packages that we use to validate
        // the SemVer tests were built on Windows which makes these test only valid for Windows.
        //
        // This can be removed when https://github.com/dotnet/corefx/issues/39931 is fixed. 
        [WindowsOnlyFact(Skip = "https://github.com/dotnet/arcade/issues/3794")]
        public void TestPackagesSemVer1()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            string a_daily, b_daily, c_daily, d_daily;
            File.WriteAllBytes(a_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameA), TestResources.DailyBuildPackages.TestPackageA);
            File.WriteAllBytes(b_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameB), TestResources.DailyBuildPackages.TestPackageB);
            File.WriteAllBytes(c_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameC), TestResources.DailyBuildPackages.TestPackageC);
            File.WriteAllBytes(d_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameD), TestResources.DailyBuildPackages.TestPackageD);

            var a_pre = Path.Combine(dir, TestResources.PreReleasePackages.NameA);
            var b_pre = Path.Combine(dir, TestResources.PreReleasePackages.NameB);
            var c_pre = Path.Combine(dir, TestResources.PreReleasePackages.NameC);
            var d_pre = Path.Combine(dir, TestResources.PreReleasePackages.NameD);

            var a_rel = Path.Combine(dir, TestResources.ReleasePackages.NameA);
            var b_rel = Path.Combine(dir, TestResources.ReleasePackages.NameB);
            var c_rel = Path.Combine(dir, TestResources.ReleasePackages.NameC);
            var d_rel = Path.Combine(dir, TestResources.ReleasePackages.NameD);

            NuGetVersionUpdater.Run(new[] { a_daily, b_daily, c_daily, d_daily }, dir, VersionTranslation.Release, exactVersions: false);
            NuGetVersionUpdater.Run(new[] { a_daily, b_daily, c_daily, d_daily }, dir, VersionTranslation.PreRelease, exactVersions: false);

            AssertPackagesEqual(TestResources.ReleasePackages.TestPackageA, File.ReadAllBytes(a_rel));
            AssertPackagesEqual(TestResources.ReleasePackages.TestPackageB, File.ReadAllBytes(b_rel));
            AssertPackagesEqual(TestResources.ReleasePackages.TestPackageC, File.ReadAllBytes(c_rel));
            AssertPackagesEqual(TestResources.ReleasePackages.TestPackageD, File.ReadAllBytes(d_rel));

            AssertPackagesEqual(TestResources.PreReleasePackages.TestPackageA, File.ReadAllBytes(a_pre));
            AssertPackagesEqual(TestResources.PreReleasePackages.TestPackageB, File.ReadAllBytes(b_pre));
            AssertPackagesEqual(TestResources.PreReleasePackages.TestPackageC, File.ReadAllBytes(c_pre));
            AssertPackagesEqual(TestResources.PreReleasePackages.TestPackageD, File.ReadAllBytes(d_pre));

            Directory.Delete(dir, recursive: true);
        }

        [WindowsOnlyFact(Skip = "https://github.com/dotnet/arcade/issues/3794")]
        public void TestPackagesSemVer2()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            string e_daily, f_daily;
            File.WriteAllBytes(e_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameE), TestResources.DailyBuildPackages.TestPackageE);
            File.WriteAllBytes(f_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameF), TestResources.DailyBuildPackages.TestPackageF);

            var e_pre = Path.Combine(dir, TestResources.PreReleasePackages.NameE);
            var f_pre = Path.Combine(dir, TestResources.PreReleasePackages.NameF);

            var e_rel = Path.Combine(dir, TestResources.ReleasePackages.NameE);
            var f_rel = Path.Combine(dir, TestResources.ReleasePackages.NameF);

            NuGetVersionUpdater.Run(new[] { e_daily, f_daily }, dir, VersionTranslation.Release, exactVersions: true);
            NuGetVersionUpdater.Run(new[] { e_daily, f_daily }, dir, VersionTranslation.PreRelease, exactVersions: true);

            AssertPackagesEqual(TestResources.ReleasePackages.TestPackageE, File.ReadAllBytes(e_rel));
            AssertPackagesEqual(TestResources.ReleasePackages.TestPackageF, File.ReadAllBytes(f_rel));

            AssertPackagesEqual(TestResources.PreReleasePackages.TestPackageE, File.ReadAllBytes(e_pre));
            AssertPackagesEqual(TestResources.PreReleasePackages.TestPackageF, File.ReadAllBytes(f_pre));

            Directory.Delete(dir, recursive: true);
        }

        [WindowsOnlyFact(Skip = "https://github.com/dotnet/arcade/issues/3794")]
        public void TestValidation()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            string a_daily, b_daily, c_daily;
            File.WriteAllBytes(a_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameA), TestResources.DailyBuildPackages.TestPackageA);
            File.WriteAllBytes(b_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameB), TestResources.DailyBuildPackages.TestPackageB);
            File.WriteAllBytes(c_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameC), TestResources.DailyBuildPackages.TestPackageC);

            var e1 = Assert.Throws<InvalidOperationException>(() => NuGetVersionUpdater.Run(new[] { c_daily }, outDirectoryOpt: null, VersionTranslation.Release, exactVersions: false));
            AssertEx.AreEqual("Package 'TestPackageC' depends on a pre-release package 'TestPackageB, [1.0.0-beta-12345-01]'", e1.Message);

            var e2 = Assert.Throws<AggregateException>(() => NuGetVersionUpdater.Run(new[] { a_daily }, outDirectoryOpt: null, VersionTranslation.Release, exactVersions: false));
            AssertEx.Equal(new[]
            {
                "System.InvalidOperationException: Package 'TestPackageA' depends on a pre-release package 'TestPackageB, 1.0.0-beta-12345-01'",
                "System.InvalidOperationException: Package 'TestPackageA' depends on a pre-release package 'TestPackageC, (, 1.0.0-beta-12345-01]'",
                "System.InvalidOperationException: Package 'TestPackageA' depends on a pre-release package 'TestPackageC, 1.0.0-beta-12345-01'"
            }, e2.InnerExceptions.Select(i => i.ToString()));

            var e3 = Assert.Throws<AggregateException>(() => NuGetVersionUpdater.Run(new[] { a_daily, b_daily }, outDirectoryOpt: null, VersionTranslation.Release, exactVersions: false));
            AssertEx.Equal(new[]
            {
                "System.InvalidOperationException: Package 'TestPackageA' depends on a pre-release package 'TestPackageC, (, 1.0.0-beta-12345-01]'",
                "System.InvalidOperationException: Package 'TestPackageA' depends on a pre-release package 'TestPackageC, 1.0.0-beta-12345-01'"
            }, e3.InnerExceptions.Select(i => i.ToString()));

            var e4 = Assert.Throws<AggregateException>(() => NuGetVersionUpdater.Run(new[] { a_daily, c_daily }, outDirectoryOpt: null, VersionTranslation.Release, exactVersions: false));
            AssertEx.Equal(new[]
            {
                "System.InvalidOperationException: Package 'TestPackageA' depends on a pre-release package 'TestPackageB, 1.0.0-beta-12345-01'",
                "System.InvalidOperationException: Package 'TestPackageC' depends on a pre-release package 'TestPackageB, [1.0.0-beta-12345-01]'"
            }, e4.InnerExceptions.Select(i => i.ToString()));

            Directory.Delete(dir, recursive: true);
        }

        [WindowsOnlyFact(Skip = "https://github.com/dotnet/arcade/issues/3794")]
        public void TestDotnetToolValidation()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var outputDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            string dotnet_tool;
            File.WriteAllBytes(dotnet_tool = Path.Combine(dir, TestResources.MiscPackages.NameDotnetTool), TestResources.MiscPackages.DotnetTool);
            string normal_package_b_daily;
            File.WriteAllBytes(normal_package_b_daily = Path.Combine(dir, TestResources.DailyBuildPackages.NameB), TestResources.DailyBuildPackages.TestPackageB);

            NuGetVersionUpdater.Run(new[] { dotnet_tool, normal_package_b_daily }, outDirectoryOpt: outputDir, VersionTranslation.Release, exactVersions: false);

            // Only contain normal package. dotnet tool package is skipped
            Assert.Single(Directory.EnumerateFiles(outputDir), fullPath => Path.GetFileNameWithoutExtension(fullPath) == "TestPackageB.1.0.0");

            Directory.Delete(dir, recursive: true);
            Directory.Delete(outputDir, recursive: true);
        }
    }
}
