// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using Xunit.Abstractions;
using Xunit;
using NuGet.Test.Utility;
using NuGet.Protocol.Core.Types;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.DataAI.NuGetRecommender.Contracts;
using NuGet.Common;
using NuGet.VisualStudio.Internal.Contracts;
using FluentAssertions;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public class RecommenderPackageFeedTests : MockedVSCollectionTests
    {
        private readonly TestLogger _logger;
        private readonly Mock<IVsNuGetPackageRecommender> _mockRecommender;

        public RecommenderPackageFeedTests(GlobalServiceProvider globalServiceProvider, ITestOutputHelper testOutputHelper)
        : base(globalServiceProvider)

        {
            _mockRecommender = new Mock<IVsNuGetPackageRecommender>();
            _mockRecommender.Setup(x => x.GetVersionInfo()).Returns(() => new Dictionary<string, string>
            {
                { "Model", "1" },
                { "Vsix", "1" }
            });
            globalServiceProvider.AddService(typeof(SVsNuGetRecommenderService), _mockRecommender.Object);
            _logger = new TestLogger(testOutputHelper);
        }

        [Fact]
        public void Constructor_WithNullArgument_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var feed = new RecommenderPackageFeed(
                    decoratedFeed: null,
                    sourceRepositories: It.IsAny<IEnumerable<SourceRepository>>(),
                    installedPackages: It.IsAny<PackageCollection>(),
                    transitivePackages: It.IsAny<PackageCollection>(),
                    targetFrameworks: It.IsAny<IReadOnlyCollection<string>>(),
                    metadataProvider: It.IsAny<IPackageMetadataProvider>(),
                    logger: _logger);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var feed = new RecommenderPackageFeed(
                    decoratedFeed: It.IsAny<IPackageFeed>(),
                    sourceRepositories: null,
                    installedPackages: It.IsAny<PackageCollection>(),
                    transitivePackages: It.IsAny<PackageCollection>(),
                    targetFrameworks: It.IsAny<IReadOnlyCollection<string>>(),
                    metadataProvider: It.IsAny<IPackageMetadataProvider>(),
                    logger: _logger);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var feed = new RecommenderPackageFeed(
                    decoratedFeed: It.IsAny<IPackageFeed>(),
                    sourceRepositories: It.IsAny<IEnumerable<SourceRepository>>(),
                    installedPackages: null,
                    transitivePackages: It.IsAny<PackageCollection>(),
                    targetFrameworks: It.IsAny<IReadOnlyCollection<string>>(),
                    metadataProvider: It.IsAny<IPackageMetadataProvider>(),
                    logger: _logger);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var feed = new RecommenderPackageFeed(
                    decoratedFeed: It.IsAny<IPackageFeed>(),
                    sourceRepositories: It.IsAny<IEnumerable<SourceRepository>>(),
                    installedPackages: It.IsAny<PackageCollection>(),
                    transitivePackages: null,
                    targetFrameworks: It.IsAny<IReadOnlyCollection<string>>(),
                    metadataProvider: It.IsAny<IPackageMetadataProvider>(),
                    logger: _logger);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var feed = new RecommenderPackageFeed(
                    decoratedFeed: It.IsAny<IPackageFeed>(),
                    sourceRepositories: It.IsAny<IEnumerable<SourceRepository>>(),
                    installedPackages: It.IsAny<PackageCollection>(),
                    transitivePackages: It.IsAny<PackageCollection>(),
                    targetFrameworks: null,
                    metadataProvider: It.IsAny<IPackageMetadataProvider>(),
                    logger: _logger);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var feed = new RecommenderPackageFeed(
                    decoratedFeed: It.IsAny<IPackageFeed>(),
                    sourceRepositories: It.IsAny<IEnumerable<SourceRepository>>(),
                    installedPackages: It.IsAny<PackageCollection>(),
                    transitivePackages: It.IsAny<PackageCollection>(),
                    targetFrameworks: It.IsAny<IReadOnlyCollection<string>>(),
                    metadataProvider: null,
                    logger: _logger);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var feed = new RecommenderPackageFeed(
                    decoratedFeed: It.IsAny<IPackageFeed>(),
                    sourceRepositories: It.IsAny<IEnumerable<SourceRepository>>(),
                    installedPackages: It.IsAny<PackageCollection>(),
                    transitivePackages: It.IsAny<PackageCollection>(),
                    targetFrameworks: It.IsAny<IReadOnlyCollection<string>>(),
                    metadataProvider: It.IsAny<IPackageMetadataProvider>(),
                    logger: null);
            });
        }

        [Fact]
        public async Task SearchAsync_WhenNoRecommendedPackages_ReturnsPackagesFromDecoratedFeedAsync()
        {
            // Arrange
            var sourceRepository = new SourceRepository(new PackageSource("http://fake-source"), [FeedTestUtils.CreateTestResourceProvider(Mock.Of<PackageMetadataResource>())]);
            var packageMetadataProvider = new MultiSourcePackageMetadataProvider(sourceRepositories: [sourceRepository], optionalLocalRepository: null, optionalGlobalLocalRepositories: null, logger: _logger);

            var decoratedFeed = new Mock<IPackageFeed>();
            var feed = new RecommenderPackageFeed(
                decoratedFeed.Object,
                sourceRepositories: [sourceRepository],
                installedPackages: new PackageCollection([]),
                transitivePackages: new PackageCollection([]),
                targetFrameworks: [string.Empty],
                metadataProvider: packageMetadataProvider,
                logger: _logger);

            var expectedPackages = new List<IPackageSearchMetadata>()
        {
            PackageSearchMetadataBuilder
                .FromIdentity(new PackageIdentity("packageA", new NuGetVersion("1.0.0")))
                .Build(),
            PackageSearchMetadataBuilder
                .FromIdentity(new PackageIdentity("packageB", new NuGetVersion("2.0.0")))
                .Build(),
            PackageSearchMetadataBuilder
                .FromIdentity(new PackageIdentity("packageC", new NuGetVersion("3.0.0")))
                .Build()
        };

            decoratedFeed.Setup(f => f.SearchAsync(
                It.IsAny<string>(),
                It.IsAny<SearchFilter>(),
                It.IsAny<CancellationToken>()))
             .ReturnsAsync(new SearchResult<IPackageSearchMetadata> { Items = expectedPackages });

            // Act
            var actualPackages = await feed.SearchAsync(string.Empty, It.IsAny<SearchFilter>(), It.IsAny<CancellationToken>());

            // Assert
            Assert.Equal(expectedPackages, actualPackages.Items);
        }

        [Theory]
        [InlineData("packageB")]
        [InlineData("packageb")]
        public async Task SearchAsync_WithRecommenderPackages_MergesMetadata(string recommendedPackage)
        {
            // Arrange
            var metadataResource = new Mock<MetadataResource>();
            metadataResource.Setup(e => e.GetLatestVersions(It.IsAny<IEnumerable<string>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<SourceCacheContext>(), It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([new KeyValuePair<string, NuGetVersion>("packageb", new NuGetVersion(2, 0, 0))]);

            var packageMetadataResource = Mock.Of<PackageMetadataResource>();

            var sourceRepository = new SourceRepository(new PackageSource(NuGetConstants.V3FeedUrl), [FeedTestUtils.CreateTestResourceProvider(metadataResource.Object), FeedTestUtils.CreateTestResourceProvider(packageMetadataResource)]);
            var packageMetadataProvider = new MultiSourcePackageMetadataProvider(sourceRepositories: [sourceRepository], optionalLocalRepository: null, optionalGlobalLocalRepositories: null, logger: _logger);

            var decoratedFeed = new Mock<IPackageFeed>();
            var feed = new RecommenderPackageFeed(
                decoratedFeed.Object,
                sourceRepositories: [sourceRepository],
                installedPackages: new PackageCollection([]),
                transitivePackages: new PackageCollection([]),
                targetFrameworks: [string.Empty],
                metadataProvider: packageMetadataProvider,
                logger: _logger);


            var packageA = PackageSearchMetadataBuilder
                .FromIdentity(new PackageIdentity("packageA", new NuGetVersion("1.0.0")))
                .Build();

            var packageB = PackageSearchMetadataBuilder
                .FromIdentity(new PackageIdentity("packageB", new NuGetVersion("2.0.0")))
                .Build();

            var packageC = PackageSearchMetadataBuilder
                .FromIdentity(new PackageIdentity("packageC", new NuGetVersion("3.0.0")))
                .Build();

            var expectedPackages = new List<IPackageSearchMetadata>()
            {
                new RecommendedPackageSearchMetadata(PackageSearchMetadataBuilder
                                                    .FromIdentity(new PackageIdentity(recommendedPackage, new NuGetVersion("2.0.0")))
                                                    .Build(),
                                                    ("1", "1")),
                packageA,
                packageC,
            };

            _mockRecommender.Setup(e => e.GetRecommendedPackageIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([recommendedPackage]);

            decoratedFeed.Setup(f => f.SearchAsync(
                It.IsAny<string>(),
                It.IsAny<SearchFilter>(),
                It.IsAny<CancellationToken>()))
             .ReturnsAsync(new SearchResult<IPackageSearchMetadata> { Items = [packageA, packageB, packageC] });

            // Act
            var actualPackages = await feed.SearchAsync(string.Empty, new SearchFilter(includePrerelease: false), It.IsAny<CancellationToken>());

            // Assert
            actualPackages.Items.Should().BeEquivalentTo(expectedPackages);
        }
    }
}
