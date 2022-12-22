// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Commands;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class SelfUpdaterTests
    {
        [SkipMono(Skip = "Mono has issues if the MockServer has anything else running in the same process https://github.com/NuGet/Home/issues/8594")]
        public async Task SelfUpdater_WithV3Server_WithUnlistedPackages_IgnoresUnlistedPackagesAsync()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var tc = new TestContext(testDirectory);
                using (var mockServer = new FileSystemBackedV3MockServer(tc.SourceDirectory))
                {
                    var expectedPackage = GetNuGetCommandLinePackage(tc, "6.0.0", isExpected: true);
                    // This package is unlisted, so we expect 6.0.0 to be chosen.
                    var unlistedPackage = GetNuGetCommandLinePackage(tc, "6.5.0", isExpected: false);

                    await SimpleTestPackageUtility.CreatePackagesAsync(tc.SourceDirectory,
                        expectedPackage,
                        unlistedPackage
                        );
                    mockServer.UnlistedPackages.Add(unlistedPackage.Identity);
                    mockServer.Start();

                    // Act
                    await tc.Target.UpdateSelfFromVersionAsync(
                        tc.Target.AssemblyLocation,
                        prerelease: false,
                        currentVersion: new NuGetVersion("5.5.0"),
                        new Configuration.PackageSource(mockServer.ServiceIndexUri),
                        CancellationToken.None);

                    // Assert
                    tc.VerifyReplacedState(replaced: true);
                }
            }
        }

        [Fact]
        public async Task SelfUpdater_PackageWithoutNuGetExe_FailsAsync()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var currentVersion = new NuGetVersion("5.5.0");
                var tc = new TestContext(testDirectory);
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    tc.SourceDirectory,
                    new SimpleTestPackageContext("NuGet.CommandLine", "6.0.0"));

                // Act & Assert
                await Assert.ThrowsAsync<CommandException>(() =>
                    tc.Target.UpdateSelfFromVersionAsync(
                           tc.Target.AssemblyLocation,
                           prerelease: false,
                           currentVersion,
                           new Configuration.PackageSource(tc.SourceDirectory),
                           CancellationToken.None));
                tc.VerifyReplacedState(replaced: false);
            }
        }

        [SkipMono(Skip = "Mono has issues if the MockServer has anything else running in the same process https://github.com/NuGet/Home/issues/8594")]
        public async Task SelfUpdater_WithV3Server_SucceedsAsync()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var tc = new TestContext(testDirectory);
                using (var mockServer = new FileSystemBackedV3MockServer(tc.SourceDirectory))
                {
                    await SimpleTestPackageUtility.CreatePackagesAsync(tc.SourceDirectory,
                        GetNuGetCommandLinePackage(tc, "6.0.0", isExpected: false),
                        GetNuGetCommandLinePackage(tc, "6.5.0", isExpected: true)
                        );
                    mockServer.Start();

                    // Act
                    await tc.Target.UpdateSelfFromVersionAsync(
                        tc.Target.AssemblyLocation,
                        prerelease: false,
                        currentVersion: new NuGetVersion("5.5.0"),
                        new Configuration.PackageSource(mockServer.ServiceIndexUri),
                        CancellationToken.None);

                    // Assert
                    tc.VerifyReplacedState(replaced: true);
                }
            }
        }

        [Theory]
        [InlineData("1.1.1", true, false)]
        [InlineData("1.1.1", false, false)]
        [InlineData("1.1.1-beta", true, false)]
        [InlineData("1.1.1-beta", false, false)]
        [InlineData("99.99.99", true, true)]
        [InlineData("99.99.99", false, true)]
        [InlineData("99.99.99-beta", true, true)]
        [InlineData("99.99.99-beta", false, false)]
        public async Task SelfUpdater_WithArbitraryVersions_SucceedsAsync(string version, bool prerelease, bool replaced)
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var tc = new TestContext(testDirectory);
                await SimpleTestPackageUtility.CreatePackagesAsync(tc.SourceDirectory, GetNuGetCommandLinePackage(tc, version, replaced));

                // Act
                var currentVersion = new NuGetVersion(5, 5, 0);
                await tc.Target.UpdateSelfFromVersionAsync(
                    tc.Target.AssemblyLocation,
                    prerelease,
                    currentVersion,
                    new Configuration.PackageSource(tc.SourceDirectory),
                    CancellationToken.None);

                // Assert
                tc.VerifyReplacedState(replaced);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SelfUpdater_WithNewerStableAndPrereleaseVersions_PrereleaseSwitchIsRespectedAsync(bool prerelease)
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var tc = new TestContext(testDirectory);
                var latestPrereleaseVersion = "5.0.0-preview1";
                var latestStableVersion = "4.6.0";

                var package100 = GetNuGetCommandLinePackage(tc, "1.0.0", false);
                var package450 = GetNuGetCommandLinePackage(tc, "4.5.0", false);
                var package460 = GetNuGetCommandLinePackage(tc, latestStableVersion, !prerelease);
                var package500preview = GetNuGetCommandLinePackage(tc, latestPrereleaseVersion, prerelease);

                await SimpleTestPackageUtility.CreatePackagesAsync(tc.SourceDirectory, package100, package450, package460, package500preview);

                // Act
                var currentVersion = new NuGetVersion(4, 0, 0);
                await tc.Target.UpdateSelfFromVersionAsync(
                    tc.Target.AssemblyLocation,
                    prerelease,
                    currentVersion,
                    new Configuration.PackageSource(tc.SourceDirectory),
                    CancellationToken.None);

                // Assert
                tc.VerifyReplacedState(replaced: true);
            }
        }

        private static SimpleTestPackageContext GetNuGetCommandLinePackage(TestContext tc, string version, bool isExpected)
        {
            var package = new SimpleTestPackageContext("NuGet.CommandLine", version);
            package.Files.Clear();
            if (isExpected)
            {
                package.Files.Add(new System.Collections.Generic.KeyValuePair<string, byte[]>(@"tools/NuGet.exe", tc.NewContent));
            }
            else
            {
                package.Files.Add(new System.Collections.Generic.KeyValuePair<string, byte[]>(@"tools/NuGet.exe", tc.WrongContent));
            }
            return package;
        }

        private class TestContext
        {
            public TestContext(TestDirectory directory)
            {
                Directory = directory;
                Console = new Mock<IConsole>();
                OriginalContent = new byte[] { 0 };
                NewContent = new byte[] { 1 };
                WrongContent = new byte[] { 2 };

                Target = new SelfUpdater(Console.Object)
                {
                    AssemblyLocation = Path.Combine(Directory, "nuget.exe")
                };

                SourceDirectory = Path.Combine(Directory, "source");
                File.WriteAllBytes(Target.AssemblyLocation, OriginalContent);
            }

            public Mock<IConsole> Console { get; }
            public SelfUpdater Target { get; }
            public TestDirectory Directory { get; }
            public byte[] NewContent { get; set; }
            public byte[] OriginalContent { get; }
            public byte[] WrongContent { get; set; }
            public string SourceDirectory { get; }

            public void VerifyReplacedState(bool replaced)
            {
                Assert.True(File.Exists(Target.AssemblyLocation), "nuget.exe should still exist.");
                var actualContent = File.ReadAllBytes(Target.AssemblyLocation);

                Assert.Equal(replaced ? NewContent : OriginalContent, actualContent);
            }
        }
    }
}
