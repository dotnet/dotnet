// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class InstalledPackageFeedTests
    {
        private readonly MultiSourcePackageMetadataProvider _metadataProvider;
        private readonly PackageMetadataResource _metadataResource;

        public InstalledPackageFeedTests()
        {
            // dependencies and data
            _metadataResource = Mock.Of<PackageMetadataResource>();

            INuGetResourceProvider provider = FeedTestUtils.CreateTestResourceProvider(_metadataResource);
            var logger = new TestLogger();
            var packageSource = new Configuration.PackageSource("http://fake-source");
            var source = new SourceRepository(packageSource, new[] { provider });

            // target
            _metadataProvider = new MultiSourcePackageMetadataProvider(
                new[] { source },
                optionalLocalRepository: null,
                optionalGlobalLocalRepositories: null,
                logger: logger);
        }

        [Fact]
        public async Task GetPackagesWithUpdatesAsync_WithHttpCache()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", new NuGetVersion("1.0.0"), null);

            var packageIdentity = new PackageIdentity("FakePackage", new NuGetVersion("1.0.0"));

            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.0");

            var _target = new InstalledPackageFeed(new[] { testPackageIdentity }, _metadataProvider);

            // Act
            var package = await _target.GetPackageMetadataAsync(
                packageIdentity, false, CancellationToken.None);

            Assert.NotNull(package);
            Assert.Equal("1.0.0", package.Identity.Version.ToString());
            var allVersions = await package.GetVersionsAsync();
            Assert.NotEmpty(allVersions);
            Assert.Equal(
                new[] { "2.0.0", "1.0.0", "0.0.1" },
                allVersions.Select(v => v.Version.ToString()).ToArray());

            SetupRemotePackageMetadata("FakePackage", "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

            package = await _target.GetPackageMetadataAsync(
                packageIdentity, false, CancellationToken.None);

            Assert.NotNull(package);
            Assert.Equal("1.0.0", package.Identity.Version.ToString());

            allVersions = await package.GetVersionsAsync();
            Assert.NotEmpty(allVersions);
            Assert.Equal(
                new[] { "2.0.1", "2.0.0", "1.0.1", "1.0.0", "0.0.1" },
                allVersions.Select(v => v.Version.ToString()).ToArray());
        }

        [Fact]
        public async Task GetPackageMetadataAsync_WithCancellationToken_ThrowsAsync()
        {
            // Arrange
            var testPackageIdentity = new PackageCollectionItem("FakePackage", new NuGetVersion("1.0.0"), installedReferences: null);
            var _target = new InstalledPackageFeed(new[] { testPackageIdentity }, _metadataProvider);

            using CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel();

            // Act and Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _target.GetPackageMetadataAsync(It.IsAny<PackageIdentity>(), It.IsAny<bool>(), cts.Token));
        }

        [Theory]
        [InlineData(new string[] { }, "", 0)]
        [InlineData(new[] { "Gamma", "Beta", "Alfa", "Delta" }, "", 4)]
        [InlineData(new[] { "Gamma", "Beta", "Alfa", "Delta" }, "g", 1)]
        [InlineData(new[] { "Gamma", "Beta", "Alfa", "Delta" }, "#@", 0)]
        [InlineData(new[] { "Gamma", "Beta", "Alfa", "Delta" }, "alFA", 1)]
        [InlineData(new[] { "Gamma", "Beta", "Alfa", "Delta" }, "pkg1", 0)]
        [InlineData(new[] { "Beta", "Alfa", "Delta", "Gamma" }, "ta", 2)]
        [InlineData(new[] { "Beta", "Alfa", "Delta", "Gamma" }, "g", 1)]
        [InlineData(new[] { "q", "z", "hi" }, "z", 1)]
        public async Task SearchAsync_WithInstalledPackages_AlwaysSortedResultsAsync(string[] installedPkgs, string query, int expectedResultsCount)
        {
            // Arrange
            var installedCollection = installedPkgs
                .Select(p => new PackageCollectionItem(p, new NuGetVersion("0.0.1"), installedReferences: null));
            var _target = new InstalledPackageFeed(installedCollection, _metadataProvider);

            // Act
            SearchResult<IPackageSearchMetadata> results = await _target.SearchAsync(query, new SearchFilter(includePrerelease: false), CancellationToken.None);

            // Assert
            var idComparer = Comparer<IPackageSearchMetadata>.Create((a, b) => a.Identity.Id.CompareTo(b.Identity.Id));
            Assert.Equal(results.Items.Count, results.RawItemsCount);
            results.Should().BeInAscendingOrder(idComparer);
            results.Should().HaveCount(expectedResultsCount);
        }

        [Theory]
        [InlineData(new object[] { new string[] { } })]
        [InlineData(new object[] { new[] { "one", "zero", "four", "nine" } })]
        [InlineData(new object[] { new[] { "Z", "A", "mypkg", "newpkg" } })]
        [InlineData(new object[] { new[] { "triangle", "square" } })]
        public async Task GetMetadataForPackagesAndSortAsync_WithTestData_AlwaysSortedByPackageIdAsync(string[] packageIds)
        {
            // Arrange
            PackageCollectionItem[] feedCollection = packageIds.Select(pkgId => new PackageCollectionItem(pkgId, new NuGetVersion("0.0.1"), installedReferences: null)).ToArray();
            var _target = new InstalledPackageFeed(feedCollection, _metadataProvider);

            // Act
            IPackageSearchMetadata[] result = await _target.GetMetadataForPackagesAndSortAsync(feedCollection, includePrerelease: It.IsAny<bool>(), CancellationToken.None);

            // Assert
            var idComparer = Comparer<IPackageSearchMetadata>.Create((a, b) => a.Identity.Id.CompareTo(b.Identity.Id));
            result.Should().BeInAscendingOrder(idComparer);
        }

        [Fact]
        public void PerformLookup_EmptyElements_ReturnsEmpty()
        {
            // Arrange
            FeedSearchContinuationToken token = FeedTestUtils.CreateInitialToken();

            // Act
            PackageIdentity[] result = InstalledPackageFeed.PerformLookup(Enumerable.Empty<PackageIdentity>(), token);

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("", 5)]
        [InlineData("one", 1)]
        [InlineData("largeQuery", 0)]
        public void PerformLookup_WithSampleData_Succeeds(string query, int expectedResultsCount)
        {
            // Arrange
            string[] packageIds = new string[] { "id", "package", "nuget", "sample", "one" };
            PackageIdentity[] ids = packageIds.Select(id => new PackageIdentity(id, new NuGetVersion("1.0.0"))).ToArray();
            var token = new FeedSearchContinuationToken()
            {
                SearchString = query,
            };

            // Act
            PackageIdentity[] result = InstalledPackageFeed.PerformLookup(ids, token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResultsCount, result.Length);
        }

        [Theory]
        [InlineData(new object[] { new string[] { } })]
        [InlineData(new object[] { new[] { "1", "2", "3" } })]
        [InlineData(new object[] { new[] { "id", "package", "nuget", "sample", "test" } })]
        public void CreateResults_WithSampleData_ResultAndCollectionAreEqual(string[] packageIds)
        {
            // Arrange
            IPackageSearchMetadata[] meta = packageIds
                .Select(id => PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity(id, new NuGetVersion("1.0.0"))).Build())
                .ToArray();

            // Act
            SearchResult<IPackageSearchMetadata> result = InstalledPackageFeed.CreateResult(meta);

            // Assert
            Assert.Equal(result, meta);
            Assert.NotNull(result.SourceSearchStatus);
            Assert.NotNull(result.SourceSearchStatus["Installed"]);
        }

        private void SetupRemotePackageMetadata(string id, params string[] versions)
        {
            var metadata = versions
                .Select(v => PackageSearchMetadataBuilder
                    .FromIdentity(new PackageIdentity(id, new NuGetVersion(v)))
                    .Build());

            Mock.Get(_metadataResource)
                .Setup(x => x.GetMetadataAsync(id, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(metadata));
        }
    }
}
