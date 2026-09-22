// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Utility;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.XPlat.FuncTest
{
    [Collection(XPlatCollection.Name)]
    public class AddPackageCommandUtilityTests
    {
        [Fact]
        public void EvaluateSources_GivenConfigWithCredentials_ReturnsPackageSourceWithCredentials()
        {
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                List<PackageSource> source = new List<PackageSource>() { new PackageSource("https://contoso.org/v3/index.json"), new PackageSource("b") };

                var nugetConfigFileName = "NuGet.Config";
                var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
                            <configuration>
                                <packageSources>
                                    <add key=""Contoso"" value=""https://contoso.org/v3/index.json"" />
                                    <add key=""b"" value =""b"" />
                                </packageSources>
                                <packageSourceCredentials>
                                    <Contoso>
                                        <add key=""Username"" value=""user @contoso.com"" />
                                        <add key=""Password"" value=""..."" />
                                    </Contoso>
                                </packageSourceCredentials>
                            </configuration>";

                var configPath = Path.Combine(mockBaseDirectory, nugetConfigFileName);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigFileName, mockBaseDirectory, config);
                var settingsLoadContext = new SettingsLoadingContext();

                var settings = Settings.LoadImmutableSettingsGivenConfigPaths(new string[] { configPath }, settingsLoadContext);
                var result = AddPackageCommandUtility.EvaluateSources(source, settings.GetConfigFilePaths());

                // Asert
                Assert.Equal(2, result.Count);
                Assert.NotNull(result[0].Credentials);
            }
        }

        public static readonly List<object[]> GetLatesVersionFromSourcesData
            = new List<object[]>
            {
                    new object[] { new string[] { "0.0.5;0.9.0;1.0.0-preview.3;PackageX" }, new string[] { "0.0.5;0.9.0;PackageX" }, "1.0.0-preview.3", true, "PackageX" },
                    new object[] { new string[] { "0.0.5;0.9.0;1.0.0-preview.3;PackageX" }, new string[] { }, "1.0.0-preview.3", true, "PackageX" },
                    new object[] { new string[] { "0.0.5;0.9.0;1.0.0-preview.3;PackageX" }, new string[] { }, "0.9.0", false, "PackageX" },
                    new object[] { new string[] { "0.0.5;0.9.0;PackageX" }, new string[] { }, "0.9.0", true, "PackageX" },
                    new object[] { new string[] { "0.0.5;0.9.0;1.0.0-preview.3;PackageX", "0.0.5;0.9.0;2.0.0-preview.4;PackageY" },
                        new string[] { "0.0.5;0.9.0;PackageX" }, "1.0.0-preview.3", true, "PackageX" },
                    new object[] { new string[] { "0.0.5;0.9.0;1.0.0-preview.3;PackageX", "0.0.5;0.9.0;2.0.0-preview.4;PackageY" }, new string[] { "0.0.5;0.9.0;PackageX" }, "2.0.0-preview.4", true, "PackageY" },
            };

        [Theory]
        [MemberData(nameof(GetLatesVersionFromSourcesData))]
        public async Task GetLatesVersionFromSources_Success(string[] sourceA, string[] sourceB, string expectedVersion, bool prerelease, string package)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arange
                string sourceAPath = await GetSourceWithPackages(sourceA, testDirectory, "SourceA");
                string sourceBPath = await GetSourceWithPackages(sourceB, testDirectory, "SourceB");

                var sources = new PackageSource[] { new PackageSource(sourceAPath), new PackageSource(sourceBPath) };

                // Act
                var logger = NullLogger.Instance;
                var result = await AddPackageCommandUtility.GetLatestVersionFromSourcesAsync(sources, logger, package, prerelease, ignoreMinPublishAge: false, CancellationToken.None);

                //Asert
                Assert.Equal(new NuGetVersion(expectedVersion), result);
            }
        }

        public static readonly List<object[]> GetLatesVersionFromSourcesData_Error
            = new List<object[]>
            {
                    new object[] { new string[] { "0.0.5;0.9.0;1.0.0-preview.3;PackageX", "0.0.5;0.9.0;2.0.0-preview.4;PackageY" },
                        new string[] { "0.0.5;0.9.0;PackageX" }, true, "PackageZ" },
            };

        [Theory]
        [MemberData(nameof(GetLatesVersionFromSourcesData_Error))]
        public async Task GetLatesVersionFromSources_Error(string[] sourceA, string[] sourceB, bool prerelease, string package)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var sourceAPath = await GetSourceWithPackages(sourceA, testDirectory, "SourceA");
                var sourceBPath = await GetSourceWithPackages(sourceB, testDirectory, "SourceB");

                var sources = new PackageSource[] { new PackageSource(sourceAPath), new PackageSource(sourceBPath) };

                // Act
                var logger = NullLogger.Instance;
                var result = await AddPackageCommandUtility.GetLatestVersionFromSourcesAsync(sources, logger, package, prerelease, ignoreMinPublishAge: false, CancellationToken.None);

                // Assert
                Assert.Null(result);
            }
        }

        private static async Task<string> GetSourceWithPackages(string[] source, TestDirectory testDirectory, string path)
        {
            var sourcePath = Path.Combine(testDirectory.Path, path);
            for (var i = 0; i < source.Length; i++)
            {
                var packageInfo = source[i].Split(';');
                var packages = new SimpleTestPackageContext[packageInfo.Length - 1];
                for (var j = 0; j < packageInfo.Length - 1; j++)
                {
                    packages[j] = new SimpleTestPackageContext(packageInfo.Last(), packageInfo[j]);
                }
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(sourcePath, packages);

            }
            return sourcePath;
        }

        [Fact]
        public async Task GetLatestVersionFromSources_WithMoreSourcesThanProcessorCount()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var processors = Environment.ProcessorCount * 2;
                var sources = new PackageSource[processors];
                var packages = new SimpleTestPackageContext[processors];
                for (var i = 0; i < processors; i++)
                {
                    var sourcePath = Path.Combine(testDirectory.Path, "Source" + i.ToString());
                    var packageX = new SimpleTestPackageContext("packageX", "1.0." + i.ToString());
                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(sourcePath, packageX);
                    sources[i] = new PackageSource(sourcePath);
                    packages[i] = packageX;
                }

                // Act
                var logger = NullLogger.Instance;
                var result = await AddPackageCommandUtility.GetLatestVersionFromSourcesAsync(sources, logger, packages.Last().Id, false, ignoreMinPublishAge: false, CancellationToken.None);

                // Assert
                Assert.Equal(packages.Last().Identity.Version, result);
            }
        }

        [Fact]
        public async Task GetLatestVersionFromSources_WithMinPublishAge_ReturnsLatestEligibleVersion()
        {
            using var testDirectory = TestDirectory.Create();
            const string packageId = "PackageX";
            var oldPackage = new PackageIdentity(packageId, new NuGetVersion("1.0.0"));
            var newPackage = new PackageIdentity(packageId, new NuGetVersion("2.0.0"));
            string sourcePath = Path.Combine(testDirectory.Path, "Source");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(sourcePath, oldPackage, newPackage);
            SetPackagePublishedDate(sourcePath, oldPackage, DateTimeOffset.UtcNow.AddDays(-2));
            SetPackagePublishedDate(sourcePath, newPackage, DateTimeOffset.UtcNow);
            var sources = new[]
            {
                new PackageSource(sourcePath) { MinPublishAge = TimeSpan.FromDays(1) }
            };

            NuGetVersion result = await AddPackageCommandUtility.GetLatestVersionFromSourcesAsync(
                sources,
                NullLogger.Instance,
                packageId,
                prerelease: false,
                ignoreMinPublishAge: false,
                CancellationToken.None);

            Assert.Equal(oldPackage.Version, result);
        }

        [Fact]
        public async Task GetLatestVersionFromSources_WhenVersionIsEligibleFromAnySource_ReturnsVersion()
        {
            using var testDirectory = TestDirectory.Create();
            var package = new PackageIdentity("PackageX", new NuGetVersion("2.0.0"));
            string coolingSourcePath = Path.Combine(testDirectory.Path, "CoolingSource");
            string eligibleSourcePath = Path.Combine(testDirectory.Path, "EligibleSource");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(coolingSourcePath, package);
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(eligibleSourcePath, package);
            SetPackagePublishedDate(coolingSourcePath, package, DateTimeOffset.UtcNow);
            SetPackagePublishedDate(eligibleSourcePath, package, DateTimeOffset.UtcNow.AddDays(-2));
            var sources = new[]
            {
                new PackageSource(coolingSourcePath) { MinPublishAge = TimeSpan.FromDays(1) },
                new PackageSource(eligibleSourcePath) { MinPublishAge = TimeSpan.FromDays(1) }
            };

            NuGetVersion result = await AddPackageCommandUtility.GetLatestVersionFromSourcesAsync(
                sources,
                NullLogger.Instance,
                package.Id,
                prerelease: false,
                ignoreMinPublishAge: false,
                CancellationToken.None);

            Assert.Equal(package.Version, result);
        }

        [Fact]
        public async Task GetLatestVersionFromSources_WhenAllVersionsAreWithinMinPublishAge_ReturnsNull()
        {
            using var testDirectory = TestDirectory.Create();
            var package = new PackageIdentity("PackageX", new NuGetVersion("1.0.0"));
            string sourcePath = Path.Combine(testDirectory.Path, "Source");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(sourcePath, package);
            SetPackagePublishedDate(sourcePath, package, DateTimeOffset.UtcNow);
            var sources = new[]
            {
                new PackageSource(sourcePath) { MinPublishAge = TimeSpan.FromDays(1) }
            };

            NuGetVersion result = await AddPackageCommandUtility.GetLatestVersionFromSourcesAsync(
                sources,
                NullLogger.Instance,
                package.Id,
                prerelease: false,
                ignoreMinPublishAge: false,
                CancellationToken.None);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(false, "1.0.0")]
        [InlineData(true, "2.0.0")]
        public async Task GetLatestVersionAsync_WithConfiguredMinPublishAge_ReturnsExpectedVersion(
            bool addPackageException,
            string expectedVersion)
        {
            using var testDirectory = TestDirectory.Create();
            const string packageId = "PackageX";
            var oldPackage = new PackageIdentity(packageId, new NuGetVersion("1.0.0"));
            var newPackage = new PackageIdentity(packageId, new NuGetVersion("2.0.0"));
            string sourcePath = Path.Combine(testDirectory.Path, "Source");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(sourcePath, oldPackage, newPackage);
            SetPackagePublishedDate(sourcePath, oldPackage, DateTimeOffset.UtcNow.AddDays(-2));
            SetPackagePublishedDate(sourcePath, newPackage, DateTimeOffset.UtcNow);
            const string configFileName = "NuGet.Config";
            string configPath = Path.Combine(testDirectory.Path, configFileName);
            string exceptionSection = addPackageException
                ? $"""
                  <minPublishAgeExceptions>
                    <package pattern="{packageId}" />
                  </minPublishAgeExceptions>
                  """
                : string.Empty;
            SettingsTestUtils.CreateConfigurationFile(
                configFileName,
                testDirectory,
                $"""
                <configuration>
                  <packageSources>
                    <clear />
                    <add key="Source" value="{sourcePath}" minPublishAgeHours="24" />
                  </packageSources>
                  {exceptionSection}
                </configuration>
                """);
            var packageSpec = new PackageSpec
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    Sources = new[] { new PackageSource(sourcePath) },
                    ConfigFilePaths = new[] { configPath }
                }
            };

            NuGetVersion result = await AddPackageReferenceCommandRunner.GetLatestVersionAsync(
                packageSpec,
                packageId,
                NullLogger.Instance,
                prerelease: false);

            Assert.Equal(new NuGetVersion(expectedVersion), result);
        }

        private static void SetPackagePublishedDate(string sourcePath, PackageIdentity package, DateTimeOffset published)
        {
            var pathResolver = new VersionFolderPathResolver(sourcePath);
            File.SetLastWriteTimeUtc(pathResolver.GetPackageFilePath(package.Id, package.Version), published.UtcDateTime);
        }
    }
}
