// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;
using static NuGet.Test.Utility.V3PackageSearchMetadataFixture;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class MultiSourcePackageMetadataProviderTests
    {
        public class LocalProviderTests : SourceRepositoryCreator
        {
            private readonly MultiSourcePackageMetadataProvider _target;
            private readonly SourceRepository _localSource;
            private readonly SourceRepository _globalSource;
            private readonly PackageMetadataResource _localMetadataResource;
            private readonly PackageMetadataResource _globalMetadataResource;

            public LocalProviderTests()
            {
                _localMetadataResource = Mock.Of<PackageMetadataResource>();
                _localSource = SetupSourceRepository(_localMetadataResource);

                _globalMetadataResource = Mock.Of<PackageMetadataResource>();
                _globalSource = SetupSourceRepository(_globalMetadataResource);

                _target = new MultiSourcePackageMetadataProvider(
                    new[] { _source },
                    optionalLocalRepository: _localSource,
                    optionalGlobalLocalRepositories: new[] { _globalSource },
                    logger: _logger);
            }

            [Fact]
            public async Task GetLocalPackageMetadataAsync_WhenGlobalSourceHasPackage_WithoutDeprecationMetadata()
            {
                // Arrange
                var emptyTestMetadata = PackageSearchMetadataBuilder.FromIdentity(TestPackageIdentity).Build();

                Mock.Get(_globalMetadataResource)
                    .Setup(x => x.GetMetadataAsync(TestPackageIdentity.Id, true, true, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { emptyTestMetadata });

                Mock.Get(_metadataResource)
                    .Setup(x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { emptyTestMetadata });

                // Act
                var metadata = await _target.GetLocalPackageMetadataAsync(
                    TestPackageIdentity,
                    includePrerelease: true,
                    cancellationToken: CancellationToken.None);

                // Assert
                Mock.Get(_metadataResource).Verify(
                    x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()),
                    Times.Once);

                Assert.Equal(new[] { "1.0.0" }, (await metadata.GetVersionsAsync()).Select(v => v.Version.ToString()).OrderBy(v => v));
                Assert.Null(await metadata.GetDeprecationMetadataAsync());
            }

            [Fact]
            public async Task GetLocalPackageMetadataAsync_WhenGlobalSourceHasPackage_WithoutVulnerabilityMetadata()
            {
                // Arrange
                var emptyTestMetadata = PackageSearchMetadataBuilder.FromIdentity(TestPackageIdentity).Build();

                Mock.Get(_globalMetadataResource)
                    .Setup(x => x.GetMetadataAsync(TestPackageIdentity.Id, true, true, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { emptyTestMetadata });

                Mock.Get(_metadataResource)
                    .Setup(x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { emptyTestMetadata });

                // Act
                var metadata = await _target.GetLocalPackageMetadataAsync(
                    TestPackageIdentity,
                    includePrerelease: true,
                    cancellationToken: CancellationToken.None);

                // Assert
                Mock.Get(_metadataResource).Verify(
                    x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()),
                    Times.Once);

                Assert.Equal(new[] { "1.0.0" }, (await metadata.GetVersionsAsync()).Select(v => v.Version.ToString()).OrderBy(v => v));
                Assert.Null(metadata.Vulnerabilities);
            }

            [Fact]
            public async Task GetLocalPackageMetadataAsync_WhenMultipleSourcesHavePackage_WithVulnerabilityMetadata()
            {
                // Arrange
                var vulnerabilities1 = new PackageVulnerabilityMetadata[]
                {
                    new PackageVulnerabilityMetadata() { AdvisoryUrl = new Uri("https://example/advisory/1"), Severity = 2 },
                    new PackageVulnerabilityMetadata() { AdvisoryUrl = new Uri("https://example/advisory/2"), Severity = 1 }
                };

                var vulnerabilities2 = new PackageVulnerabilityMetadata[]
                {
                    new PackageVulnerabilityMetadata() { AdvisoryUrl = new Uri("https://example/advisory/3"), Severity = 0 },
                    new PackageVulnerabilityMetadata() { AdvisoryUrl = new Uri("https://example/advisory/4"), Severity = 1 }
                };

                IPackageSearchMetadata metadata1 = new MockPackageSearchMetadata() { Identity = TestPackageIdentity, Vulnerabilities = vulnerabilities1 };
                IPackageSearchMetadata metadata2 = new MockPackageSearchMetadata() { Identity = TestPackageIdentity, Vulnerabilities = vulnerabilities2 };

                Mock.Get(_globalMetadataResource)
                    .Setup(x => x.GetMetadataAsync(TestPackageIdentity.Id, true, true, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { metadata1 });

                Mock.Get(_metadataResource)
                    .Setup(x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { metadata2 });

                // Act
                var metadata = await _target.GetLocalPackageMetadataAsync(
                    TestPackageIdentity,
                    includePrerelease: true,
                    cancellationToken: CancellationToken.None);

                // Assert
                Mock.Get(_metadataResource).Verify(
                    x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()),
                    Times.Once);

                Assert.NotNull(metadata.Vulnerabilities);
                Assert.Collection(metadata.Vulnerabilities,
                    item =>
                    {
                        Assert.Equal(item.AdvisoryUrl, new Uri("https://example/advisory/1"));
                        Assert.Equal(item.Severity, 2);
                    },
                    item =>
                    {
                        Assert.Equal(item.AdvisoryUrl, new Uri("https://example/advisory/2"));
                        Assert.Equal(item.Severity, 1);
                    });
            }

            [Fact]
            public async Task GetPackageMetadataListAsync_WithMultipleSources_UnifiesVersions()
            {
                // Arrange
                var testPackageId = "FakePackage";
                SetupRemotePackageMetadata(testPackageId, "1.0.0", "2.0.0", "2.0.1", "1.0.1", "2.0.0", "1.0.0", "1.0.1");

                // Act
                var packages = await _target.GetPackageMetadataListAsync(
                    testPackageId,
                    includePrerelease: true,
                    includeUnlisted: false,
                    cancellationToken: CancellationToken.None);

                // Assert
                Assert.NotEmpty(packages);

                var actualVersions = packages.Select(p => p.Identity.Version.ToString()).ToArray();
                Assert.Equal(
                    new[] { "1.0.0", "2.0.0", "2.0.1", "1.0.1" },
                    actualVersions);
            }

            [Fact]
            public async Task GetLocalPackageMetadataAsync_WhenLocalSourceHasPackage_CombinesMetadata()
            {
                // Arrange
                Mock.Get(_localMetadataResource)
                    .Setup(x => x.GetMetadataAsync(TestPackageIdentity.Id, true, true, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { PackageSearchMetadataBuilder.FromIdentity(TestPackageIdentity).Build() });

                var expectedVersionStrings = new[] { "1.0.0", "2.0.0" };
                var deprecationMetadata = new PackageDeprecationMetadata();
                Mock.Get(_metadataResource)
                    .Setup(x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(
                        new[]
                        {
                            PackageSearchMetadataBuilder
                                .FromIdentity(TestPackageIdentity)
                                .WithDeprecation(new AsyncLazy<PackageDeprecationMetadata>(() => Task.FromResult(deprecationMetadata)))
                                .Build(),

                            PackageSearchMetadataBuilder
                                .FromIdentity(new PackageIdentity(TestPackageIdentity.Id, new NuGetVersion("2.0.0")))
                                .Build()
                        });

                // Act
                var metadata = await _target.GetLocalPackageMetadataAsync(
                    TestPackageIdentity,
                    includePrerelease: true,
                    cancellationToken: CancellationToken.None);

                // Assert
                Mock.Get(_metadataResource).Verify(
                    x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()),
                    Times.Once);

                Assert.Equal(expectedVersionStrings, (await metadata.GetVersionsAsync()).Select(v => v.Version.ToString()).OrderBy(v => v));
                Assert.Same(deprecationMetadata, await metadata.GetDeprecationMetadataAsync());
            }

            [Fact]
            public async Task GetOnlyLocalPackageMetadataAsync_WithLocalSource_SucceedsAsync()
            {
                Mock.Get(_localMetadataResource)
                    .Setup(x => x.GetMetadataAsync(TestPackageIdentity.Id, true, true, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { PackageSearchMetadataBuilder.FromIdentity(TestPackageIdentity).Build() });

                IPackageSearchMetadata packageSearchMetadata = await _target.GetOnlyLocalPackageMetadataAsync(TestPackageIdentity, CancellationToken.None);

                Assert.NotNull(packageSearchMetadata);
            }
        }

        public class NoLocalProviderTests : SourceRepositoryCreator
        {
            private readonly MultiSourcePackageMetadataProvider _target;

            public NoLocalProviderTests()
            {
                _target = new MultiSourcePackageMetadataProvider(
                    new[] { _source },
                    optionalLocalRepository: null,
                    optionalGlobalLocalRepositories: null,
                    logger: _logger);
            }

            [Fact]
            public async Task GetLatestPackageMetadataAsync_Always_SendsASingleRequestPerSource()
            {
                // Arrange
                var testProject = SetupProject(TestPackageIdentity, allowedVersions: null);

                // Act
                await _target.GetLatestPackageMetadataAsync(
                        TestPackageIdentity,
                        testProject,
                        includePrerelease: true,
                        cancellationToken: CancellationToken.None);

                // Assert
                Mock.Get(_metadataResource).Verify(
                    x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task GetPackageMetadataAsync_Always_SendsASingleRequestPerSource()
            {
                // Act
                await _target.GetPackageMetadataAsync(
                    TestPackageIdentity,
                    includePrerelease: true,
                    cancellationToken: CancellationToken.None);

                // Assert
                Mock.Get(_metadataResource).Verify(
                    x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task GetPackageMetadataListAsync_Always_SendsASingleRequestPerSource()
            {
                // Act
                await _target.GetPackageMetadataListAsync(
                    TestPackageIdentity.Id,
                    includePrerelease: true,
                    includeUnlisted: false,
                    cancellationToken: CancellationToken.None);

                // Assert
                Mock.Get(_metadataResource).Verify(
                    x => x.GetMetadataAsync(TestPackageIdentity.Id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task GetLatestPackageMetadataAsync_WithAllVersions_RetrievesLatestVersion()
            {
                // Arrange
                var testProject = SetupProject(TestPackageIdentity, allowedVersions: null);
                SetupRemotePackageMetadata(TestPackageIdentity.Id, "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

                // Act
                var latest = await _target.GetLatestPackageMetadataAsync(
                    TestPackageIdentity,
                    testProject,
                    includePrerelease: true,
                    cancellationToken: CancellationToken.None);

                // Assert
                Assert.NotNull(latest);
                Assert.Equal("2.0.1", latest.Identity.Version.ToString());

                var actualVersions = await latest.GetVersionsAsync();
                Assert.NotEmpty(actualVersions);
                Assert.Equal(
                    new[] { "2.0.1", "2.0.0", "1.0.1", "1.0.0", "0.0.1" },
                    actualVersions.Select(v => v.Version.ToString()).ToArray());
            }

            [Fact]
            public async Task GetPackageMetadataForIdentityAsync_WithoutVersions()
            {
                // Arrange
                var testProject = SetupProject(TestPackageIdentity, "[1,2)");
                SetupRemotePackageMetadata(TestPackageIdentity.Id, "0.0.1", "1.0.0", "1.12", "2.0.1", "2.0.0", "1.0.1");

                // Act
                var specificVersion = await _target.GetPackageMetadataForIdentityAsync(
                    new PackageIdentity(TestPackageIdentity.Id, new NuGetVersion("1.12")),
                    cancellationToken: CancellationToken.None);

                // Assert
                Assert.NotNull(specificVersion);
                Assert.Equal("1.12", specificVersion.Identity.Version.ToString());

                var actualVersions = await specificVersion.GetVersionsAsync();
                Assert.Equal(new[] { "1.12" }, actualVersions.Select(v => v.Version.ToString()).ToArray());
            }

            [Fact]
            public async Task GetLatestPackageMetadataAsync_WithAllowedVersions_RetrievesLatestVersion()
            {
                // Arrange
                var testProject = SetupProject(TestPackageIdentity, "[1,2)");
                SetupRemotePackageMetadata(TestPackageIdentity.Id, "0.0.1", "1.0.0", "2.0.1", "2.0.0", "1.0.1");

                // Act
                var latest = await _target.GetLatestPackageMetadataAsync(
                    TestPackageIdentity,
                    testProject,
                    includePrerelease: true,
                    cancellationToken: CancellationToken.None);

                // Assert
                Assert.NotNull(latest);
                Assert.Equal("1.0.1", latest.Identity.Version.ToString());

                var actualVersions = await latest.GetVersionsAsync();
                Assert.NotEmpty(actualVersions);
                Assert.Equal(
                    new[] { "2.0.1", "2.0.0", "1.0.1", "1.0.0", "0.0.1" },
                    actualVersions.Select(v => v.Version.ToString()).ToArray());
            }

            [Fact]
            public async Task GetPackageMetadataListAsync_WithMultipleSources_UnifiesVersions()
            {
                // Arrange
                var testPackageId = "FakePackage";
                SetupRemotePackageMetadata(testPackageId, "1.0.0", "2.0.0", "2.0.1", "1.0.1", "2.0.0", "1.0.0", "1.0.1");

                // Act
                var packages = await _target.GetPackageMetadataListAsync(
                    testPackageId,
                    includePrerelease: true,
                    includeUnlisted: false,
                    cancellationToken: CancellationToken.None);

                // Assert
                Assert.NotEmpty(packages);

                var actualVersions = packages.Select(p => p.Identity.Version.ToString()).ToArray();
                Assert.Equal(
                    new[] { "1.0.0", "2.0.0", "2.0.1", "1.0.1" },
                    actualVersions);
            }

            [Fact]
            public async Task GetLatestPackageMetadataAsync_CancellationThrows()
            {
                // Arrange
                var testProject = SetupProject(TestPackageIdentity, allowedVersions: null);

                CancellationToken token = new CancellationToken(canceled: true);

                // Act
                Task task() => _target.GetLatestPackageMetadataAsync(
                    TestPackageIdentity,
                    testProject,
                    includePrerelease: true,
                    cancellationToken: token);

                await Assert.ThrowsAsync<OperationCanceledException>(task);
            }

            [Fact]
            public async Task GetPackageMetadataAsync_CancellationThrows()
            {
                // Arrange
                CancellationToken token = new CancellationToken(canceled: true);

                // Act
                Task task() => _target.GetPackageMetadataAsync(
                    TestPackageIdentity,
                    includePrerelease: true,
                    cancellationToken: token);

                await Assert.ThrowsAsync<OperationCanceledException>(task);
            }

            [Fact]
            public async Task GetPackageMetadataListAsync_CancellationThrows()
            {
                // Arrange
                CancellationToken token = new CancellationToken(canceled: true);

                // Act
                Task task() => _target.GetPackageMetadataListAsync(
                    TestPackageIdentity.Id,
                    includePrerelease: true,
                    includeUnlisted: true,
                    cancellationToken: token);

                await Assert.ThrowsAsync<OperationCanceledException>(task);
            }

            [Fact]
            public async Task GetLocalPackageMetadataAsync_CancellationThrows()
            {
                // Arrange
                CancellationToken token = new CancellationToken(canceled: true);

                // Act
                //Note: Private method MultiSourcePackageMetadataProvider.FetchAndMergeVersionsAndDeprecationMetadataAsync
                // is called within this method, but this test does not enter into that logic.
                Task task() => _target.GetLocalPackageMetadataAsync(
                    TestPackageIdentity,
                    includePrerelease: true,
                    cancellationToken: token);

                await Assert.ThrowsAsync<OperationCanceledException>(task);
            }

            [Fact]
            public async Task GetMetadataTaskSafeAsync_CancellationThrows()
            {
                // Arrange
                CancellationToken token = new CancellationToken(canceled: true);

                // Act
                Task<IPackageSearchMetadata> task() => _target.GetPackageMetadataAsync(
                   TestPackageIdentity,
                   includePrerelease: true,
                   cancellationToken: token);

                Task safeTask() => _target.GetMetadataTaskSafeAsync(() => task());

                await Assert.ThrowsAsync<OperationCanceledException>(safeTask);
            }
        }
    }
}
