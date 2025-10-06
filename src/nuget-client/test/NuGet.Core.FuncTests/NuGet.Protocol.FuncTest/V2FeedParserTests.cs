// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.FuncTest
{
    public class V2FeedParserTests
    {
        [Fact]
        public async Task V2FeedParser_DownloadFromInvalidUrl()
        {
            // Arrange
            var randomName = Guid.NewGuid().ToString();
            var repo = Repository.Factory.GetCoreV3(TestSources.NuGetV2Uri);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, TestSources.NuGetV2Uri);

            // Act 
            using (var packagesFolder = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                Exception ex = await Assert.ThrowsAsync<FatalProtocolException>(
                    async () => await parser.DownloadFromUrl(
                        new PackageIdentity("not-found", new NuGetVersion("6.2.0")),
                        new Uri($"https://www.{randomName}.org/api/v2/"),
                        new PackageDownloadContext(cacheContext),
                        packagesFolder,
                        NullLogger.Instance,
                        CancellationToken.None));

                // Assert
                Assert.NotNull(ex);
                Assert.Equal($"Error downloading 'not-found.6.2.0' from 'https://www.{randomName}.org/api/v2/'.", ex.Message);
            }
        }

        [Fact]
        public async Task V2FeedParser_DownloadFromUrlInvalidId()
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(TestSources.NuGetV2Uri);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, TestSources.NuGetV2Uri);

            // Act 
            using (var packagesFolder = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var actual = await parser.DownloadFromUrl(
                    new PackageIdentity("not-found", new NuGetVersion("6.2.0")),
                    new Uri($@"{TestSources.NuGetV2Uri}/package/not-found/6.2.0"),
                    new PackageDownloadContext(cacheContext),
                    packagesFolder,
                    NullLogger.Instance,
                    CancellationToken.None);

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(DownloadResourceResultStatus.NotFound, actual.Status);
            }
        }

        [Fact]
        public async Task V2FeedParser_DownloadFromIdentity()
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(TestSources.NuGetV2Uri);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, TestSources.NuGetV2Uri);

            // Act & Assert
            using (var packagesFolder = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            using (var downloadResult = await parser.DownloadFromIdentity(
                new PackageIdentity("WindowsAzure.Storage", new NuGetVersion("6.2.0")),
                new PackageDownloadContext(cacheContext),
                packagesFolder,
                cacheContext,
                NullLogger.Instance,
                CancellationToken.None))
            {
                var packageReader = downloadResult.PackageReader;
                var files = packageReader.GetFiles();

                Assert.Equal(13, files.Count());
            }
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.ProGet, TestSources.Klondike, TestSources.Artifactory, TestSources.MyGet)]
        public async Task V2FeedParser_NormalizedVersion(string packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(packageSource);
            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource);

            // Act
            var package = await parser.GetPackage(new PackageIdentity("owin", new NuGetVersion("1.0")), NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal("Owin", package.Id);
            Assert.Equal("1.0", package.Version.ToString());
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.ProGet, TestSources.Klondike, TestSources.Artifactory, TestSources.MyGet)]
        public async Task V2FeedParser_DownloadFromIdentityFromDifferentServer(string packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(packageSource);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource);

            // Act & Assert
            using (var packagesFolder = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                using (var downloadResult = await parser.DownloadFromIdentity(
                    new PackageIdentity("newtonsoft.json", new NuGetVersion("8.0.3")),
                    new PackageDownloadContext(cacheContext),
                    packagesFolder,
                    cacheContext,
                    NullLogger.Instance,
                    CancellationToken.None))
                {
                    var packageReader = downloadResult.PackageReader;
                    var files = packageReader.GetFiles();

                    Assert.Equal(15, files.Count());
                }
            }
        }

        [PackageSourceTheory]
        // ProGet does not support seach portable framework, it will return empty packages
        [PackageSourceData(TestSources.Klondike, TestSources.Artifactory, TestSources.MyGet)]
        public async Task V2FeedParser_SearchWithPortableFramework(string packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(packageSource);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource);

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = new string[] { "portable-net45+win8" }
            };

            // Act
            var packages = await parser.Search("nunit", searchFilter, 0, 1, NullLogger.Instance, CancellationToken.None);
            var package = packages.FirstOrDefault();

            // Assert
            Assert.Equal("NUnit", package.Id);
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.ProGet, TestSources.Klondike, TestSources.Artifactory, TestSources.MyGet)]
        public async Task V2FeedParser_Search(string packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(packageSource);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource);

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = new string[] { "net45" }
            };

            // Act
            var packages = await parser.Search("nunit", searchFilter, 0, 1, NullLogger.Instance, CancellationToken.None);
            var package = packages.FirstOrDefault();

            // Assert
            Assert.Equal("NUnit", package.Id);
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.ProGet, TestSources.Klondike, TestSources.Artifactory, TestSources.MyGet)]
        public async Task V2FeedParser_SearchWithPrerelease(string packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(packageSource);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource);

            var searchFilter = new SearchFilter(includePrerelease: true)
            {
                SupportedFrameworks = new string[] { "net" }
            };

            // Act
            var packages = await parser.Search("entityframework", searchFilter, 0, 3, NullLogger.Instance, CancellationToken.None);
            var package = packages.FirstOrDefault(p => p.Id == "EntityFramework" && p.Version.ToString() == "7.0.0-beta4");

            // Assert
            Assert.NotNull(package);
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.NuGetServer, TestSources.VSTS)]
        public async Task V2FeedParser_CredentialNormalizedVersion(PackageSource packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV2(packageSource);
            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource.Source);

            // Act
            var package = await parser.GetPackage(new PackageIdentity("owin", new NuGetVersion("1.0")), NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal("Owin", package.Id);
            Assert.Equal("1.0", package.Version.ToString());
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.NuGetServer, TestSources.VSTS)]
        public async Task V2FeedParser_DownloadFromIdentityFromDifferentCredentialServer(PackageSource packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV2(packageSource);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource.Source);

            // Act & Assert
            using (var packagesFolder = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                using (var downloadResult = await parser.DownloadFromIdentity(
                    new PackageIdentity("newtonsoft.json", new NuGetVersion("8.0.3")),
                    new PackageDownloadContext(cacheContext),
                    packagesFolder,
                    cacheContext,
                    NullLogger.Instance,
                    CancellationToken.None))
                {
                    var packageReader = downloadResult.PackageReader;
                    var files = packageReader.GetFiles();

                    Assert.Equal(15, files.Count());
                }
            }
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.NuGetServer, TestSources.VSTS)]
        public async Task V2FeedParser_SearchWithPortableFrameworkFromCredentialServer(PackageSource packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV2(packageSource);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource.Source);

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = new string[] { "portable-net45+win8" }
            };

            // Act
            var packages = await parser.Search("nunit", searchFilter, 0, 1, NullLogger.Instance, CancellationToken.None);
            var package = packages.FirstOrDefault();

            // Assert
            Assert.Equal("NUnit", package.Id);
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.NuGetServer, TestSources.VSTS)]
        public async Task V2FeedParser_SearchFromCredentialServer(PackageSource packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV2(packageSource);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource.Source);

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = new string[] { "net45" }
            };

            // Act
            var packages = await parser.Search("nunit", searchFilter, 0, 1, NullLogger.Instance, CancellationToken.None);
            var package = packages.FirstOrDefault();

            // Assert
            Assert.Equal("NUnit", package.Id);
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.NuGetServer, TestSources.VSTS)]
        public async Task V2FeedParser_SearchWithPrereleaseCredentialServer(PackageSource packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV2(packageSource);

            var httpSource = HttpSource.Create(repo);

            var parser = new V2FeedParser(httpSource, packageSource.Source);

            var searchFilter = new SearchFilter(includePrerelease: true)
            {
                SupportedFrameworks = new string[] { "net" }
            };

            // Act
            var packages = await parser.Search("entityframework", searchFilter, 0, 3, NullLogger.Instance, CancellationToken.None);
            var package = packages.FirstOrDefault(p => p.Id == "EntityFramework" && p.Version.ToString() == "7.0.0-beta4");

            // Assert
            Assert.NotNull(package);
        }
    }
}
