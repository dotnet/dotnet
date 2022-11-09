// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class ConsolidatePackageFeedTests
    {
        private readonly TestLogger _logger;
        private readonly IPackageMetadataProvider _packageMetadataProvider;

        public ConsolidatePackageFeedTests(ITestOutputHelper testOutputHelper)
        {
            _logger = new TestLogger(testOutputHelper);

            var _metadataResource = Mock.Of<PackageMetadataResource>();
            INuGetResourceProvider provider = FeedTestUtils.CreateTestResourceProvider(_metadataResource);
            var packageSource = new Configuration.PackageSource("http://fake-source");
            var source = new SourceRepository(source: packageSource, providers: new[] { provider });

            _packageMetadataProvider = new MultiSourcePackageMetadataProvider(sourceRepositories: new[] { source }, optionalLocalRepository: null, optionalGlobalLocalRepositories: null, logger: _logger);
        }

        [Fact]
        public void ConsolidatePackageFeed_NullInstalledPackages_Throws()
        {
            // Arrange, Act and Assert
            Assert.Throws<ArgumentNullException>(() => new ConsolidatePackageFeed(installedPackages: null, _packageMetadataProvider, _logger));
        }

        [Fact]
        public async Task ContinueSearchAsync_MultipleInstalledVersions_ReturnsLatest()
        {
            // Arrange
            var installed = new[]
            {
                new PackageCollectionItem("FakePackage", new NuGetVersion("1.0.0"), installedReferences: null),
                new PackageCollectionItem("FakePackage", new NuGetVersion("2.0.0"), installedReferences: null),
                new PackageCollectionItem("FakePackage", new NuGetVersion("3.0.0"), installedReferences: null),
            };

            var feed = new ConsolidatePackageFeed(installed, _packageMetadataProvider, _logger);
            var token = FeedTestUtils.CreateInitialToken();

            // Act
            SearchResult<IPackageSearchMetadata> packageSearchMetadatas = await feed.ContinueSearchAsync(token, CancellationToken.None);

            // Assert
            Assert.Collection(packageSearchMetadatas,
                psm => Assert.Equal(new PackageIdentity("FakePackage", NuGetVersion.Parse("3.0.0")), psm.Identity));
        }

        [Fact]
        public async Task ContinueSearchAsync_EmptyInstalledVersions_ReturnsEmptySearchResults()
        {
            // Arrange
            var feed = new ConsolidatePackageFeed(Enumerable.Empty<PackageCollectionItem>(), _packageMetadataProvider, _logger);
            var token = FeedTestUtils.CreateInitialToken();

            // Act
            SearchResult<IPackageSearchMetadata> packageSearchMetadatas = await feed.ContinueSearchAsync(token, CancellationToken.None);

            // Assert
            Assert.Empty(packageSearchMetadatas);
        }
    }
}
