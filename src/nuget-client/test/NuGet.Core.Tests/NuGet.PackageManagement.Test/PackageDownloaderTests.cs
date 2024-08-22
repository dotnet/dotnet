// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement
{
    public class PackageDownloaderTests
    {
        private static readonly string _jQuery182ContentHash = "uhcB1DuO8O6WW6wWe7SDn0Rz4vZZPqNJHld10yrtG9Z/l4HiTHBhncn2GWAzF7Yv6hoNC/+kAM/6WMsrIdThWA==";

        [Fact]
        public async Task GetDownloadResourceResultAsync_Sources_ThrowsForNullSources()
        {
            using (var test = new PackageDownloaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        sources: null,
                        packageIdentity: test.PackageIdentity,
                        downloadContext: test.Context,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: CancellationToken.None));

                Assert.Equal("sources", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Sources_ThrowsForNullPackageIdentity()
        {
            using (var test = new PackageDownloaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        Enumerable.Empty<SourceRepository>(),
                        packageIdentity: null,
                        downloadContext: test.Context,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: CancellationToken.None));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Sources_ThrowsForNullDownloadContext()
        {
            using (var test = new PackageDownloaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        Enumerable.Empty<SourceRepository>(),
                        test.PackageIdentity,
                        downloadContext: null,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: CancellationToken.None));

                Assert.Equal("downloadContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Sources_ThrowsForNullLogger()
        {
            using (var test = new PackageDownloaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        Enumerable.Empty<SourceRepository>(),
                        test.PackageIdentity,
                        test.Context,
                        globalPackagesFolder: "",
                        logger: null,
                        token: CancellationToken.None));

                Assert.Equal("logger", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Sources_ThrowsIfCancelled()
        {
            using (var test = new PackageDownloaderTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        Enumerable.Empty<SourceRepository>(),
                        test.PackageIdentity,
                        test.Context,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Source_ThrowsForNullSourceRepository()
        {
            using (var test = new PackageDownloaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        sourceRepository: null,
                        packageIdentity: test.PackageIdentity,
                        downloadContext: test.Context,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: CancellationToken.None));

                Assert.Equal("sourceRepository", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Source_ThrowsForNullPackageIdentity()
        {
            using (var test = new PackageDownloaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        test.SourceRepository,
                        packageIdentity: null,
                        downloadContext: test.Context,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: CancellationToken.None));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Source_ThrowsForNullDownloadContext()
        {
            using (var test = new PackageDownloaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        test.SourceRepository,
                        test.PackageIdentity,
                        downloadContext: null,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: CancellationToken.None));

                Assert.Equal("downloadContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Source_ThrowsForNullLogger()
        {
            using (var test = new PackageDownloaderTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        test.SourceRepository,
                        test.PackageIdentity,
                        test.Context,
                        globalPackagesFolder: "",
                        logger: null,
                        token: CancellationToken.None));

                Assert.Equal("logger", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Source_ThrowsIfCancelled()
        {
            using (var test = new PackageDownloaderTest())
            {
                var resourceProvider = new Mock<INuGetResourceProvider>();

                resourceProvider.SetupGet(x => x.Name)
                    .Returns(nameof(DownloadResource) + "Provider");
                resourceProvider.SetupGet(x => x.ResourceType)
                    .Returns(typeof(DownloadResource));

                resourceProvider.Setup(x => x.TryCreate(
                        It.IsNotNull<SourceRepository>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Tuple<bool, INuGetResource>(true, Mock.Of<DownloadResource>()));

                var sourceRepository = new SourceRepository(
                    test.SourceRepository.PackageSource,
                    new[] { resourceProvider.Object });

                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        sourceRepository,
                        test.PackageIdentity,
                        test.Context,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_Source_ThrowsIfNoDownloadResource()
        {
            using (var test = new PackageDownloaderTest())
            {
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        test.SourceRepository,
                        test.PackageIdentity,
                        test.Context,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: CancellationToken.None));
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_V2_ThrowIfPackageDoesNotExist()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();
            var v2sourceRepository = sourceRepositoryProvider.GetRepositories().First();
            var packageId = Guid.NewGuid().ToString();
            var packageIdentity = new PackageIdentity(packageId, new NuGetVersion("1.0.0"));

            // Act
            Exception exception = null;
            try
            {
                using (var cacheContext = new SourceCacheContext())
                using (var packagesDirectory = TestDirectory.Create())
                {
                    await PackageDownloader.GetDownloadResourceResultAsync(
                        v2sourceRepository,
                        packageIdentity,
                        new PackageDownloadContext(cacheContext),
                        packagesDirectory,
                        NullLogger.Instance,
                        CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_V3_ThrowIfPackageDoesNotExist()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            var v3sourceRepository = sourceRepositoryProvider.GetRepositories().First();
            var packageId = Guid.NewGuid().ToString();
            var packageIdentity = new PackageIdentity(packageId, new NuGetVersion("1.0.0"));

            // Act
            Exception exception = null;
            try
            {
                using (var cacheContext = new SourceCacheContext())
                using (var packagesDirectory = TestDirectory.Create())
                {
                    await PackageDownloader.GetDownloadResourceResultAsync(
                        v3sourceRepository,
                        packageIdentity,
                        new PackageDownloadContext(cacheContext),
                        packagesDirectory,
                        NullLogger.Instance,
                        CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_V2_DownloadsPackage()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();
            var v2sourceRepository = sourceRepositoryProvider.GetRepositories().First();
            var packageIdentity = new PackageIdentity("jQuery", new NuGetVersion("1.8.2"));

            // Act
            using (var cacheContext = new SourceCacheContext())
            using (var packagesDirectory = TestDirectory.Create())
            using (var downloadResult = await PackageDownloader.GetDownloadResourceResultAsync(
                v2sourceRepository,
                packageIdentity,
                new PackageDownloadContext(cacheContext),
                packagesDirectory,
                NullLogger.Instance,
                CancellationToken.None))
            {
                var targetPackageStream = downloadResult.PackageStream;

                using (var packageArchiveReader = new PackageArchiveReader(targetPackageStream))
                {
                    var contentHash = packageArchiveReader.GetContentHash(CancellationToken.None);

                    // Assert
                    Assert.Equal(_jQuery182ContentHash, contentHash);
                    Assert.True(targetPackageStream.CanSeek);
                }
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_V3_DownloadsPackage()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            var v3sourceRepository = sourceRepositoryProvider.GetRepositories().First();
            var packageIdentity = new PackageIdentity("jQuery", new NuGetVersion("1.8.2"));

            // Act
            using (var cacheContext = new SourceCacheContext())
            using (var packagesDirectory = TestDirectory.Create())
            using (var downloadResult = await PackageDownloader.GetDownloadResourceResultAsync(
                v3sourceRepository,
                packageIdentity,
                new PackageDownloadContext(cacheContext),
                packagesDirectory,
                NullLogger.Instance,
                CancellationToken.None))
            {
                var targetPackageStream = downloadResult.PackageStream;

                using (var packageArchiveReader = new PackageArchiveReader(targetPackageStream))
                {
                    var contentHash = packageArchiveReader.GetContentHash(CancellationToken.None);

                    // Assert
                    Assert.Equal(_jQuery182ContentHash, contentHash);
                    Assert.True(targetPackageStream.CanSeek);
                }
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_WithDirectDownloadAndV2Source_SkipsGlobalPackagesFolder()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV2OnlySourceRepositoryProvider();

            // Act & Assert
            await VerifyDirectDownloadSkipsGlobalPackagesFolderAsync(sourceRepositoryProvider);
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_WithDirectDownloadAndV3Source_SkipsGlobalPackagesFolder()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();

            // Act & Assert
            await VerifyDirectDownloadSkipsGlobalPackagesFolderAsync(sourceRepositoryProvider);
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_MultipleSources_DownloadsPackage()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new[]
            {
                new PackageSource("https://www.myget.org/F/aspnetvnext/api/v2/"),
                TestSourceRepositoryUtility.V3PackageSource,
                new PackageSource("http://unit.test"),
            });

            var packageIdentity = new PackageIdentity("jQuery", new NuGetVersion("1.8.2"));

            // Act
            using (var cacheContext = new SourceCacheContext())
            using (var packagesDirectory = TestDirectory.Create())
            using (var downloadResult = await PackageDownloader.GetDownloadResourceResultAsync(
                sourceRepositoryProvider.GetRepositories(),
                packageIdentity,
                new PackageDownloadContext(cacheContext),
                packagesDirectory,
                NullLogger.Instance,
                CancellationToken.None))
            {
                var targetPackageStream = downloadResult.PackageStream;

                // Assert
                Assert.True(targetPackageStream.CanSeek);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_MultipleSources_PackageNotFound()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new[]
            {
                new PackageSource("https://www.myget.org/F/aspnetvnext/api/v2/"),
                new PackageSource("http://unit.test"),
            });

            var packageIdentity = new PackageIdentity("jQuery", new NuGetVersion("1.8.2"));

            using (var packagesDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                await Assert.ThrowsAsync<FatalProtocolException>(
                    async () => await PackageDownloader.GetDownloadResourceResultAsync(
                        sourceRepositoryProvider.GetRepositories(),
                        packageIdentity,
                        new PackageDownloadContext(cacheContext),
                        packagesDirectory,
                        NullLogger.Instance,
                        CancellationToken.None));
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_MultipleSources_PackageDownloadedWhenFoundInMultipleSources()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new[]
            {
                TestSourceRepositoryUtility.V3PackageSource,
                TestSourceRepositoryUtility.V3PackageSource,
                new PackageSource("http://unit.test"),
                TestSourceRepositoryUtility.V2PackageSource,
                TestSourceRepositoryUtility.V2PackageSource,
            });

            var packageIdentity = new PackageIdentity("jQuery", new NuGetVersion("1.8.2"));

            // Act
            using (var cacheContext = new SourceCacheContext())
            using (var packagesDirectory = TestDirectory.Create())
            using (var downloadResult = await PackageDownloader.GetDownloadResourceResultAsync(
                sourceRepositoryProvider.GetRepositories(),
                packageIdentity,
                new PackageDownloadContext(cacheContext),
                packagesDirectory,
                NullLogger.Instance,
                CancellationToken.None))
            {
                var targetPackageStream = downloadResult.PackageStream;

                // Assert
                Assert.True(targetPackageStream.CanSeek);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_WithSourceMappingFound_PackageDownloaded()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new[]
            {
                TestSourceRepositoryUtility.V3PackageSource,
                TestSourceRepositoryUtility.V2PackageSource,
            });

            string packageId = "jQuery.Validation";
            string packagePatterns = $"{TestSourceRepositoryUtility.V3PackageSource.Name},jQuery.*|{TestSourceRepositoryUtility.V2PackageSource.Name},jQuery.* ";
            var packageIdentity = new PackageIdentity(packageId, new NuGetVersion("1.19.5"));
            PackageSourceMapping packageSourceMapping = PackageSourceMappingUtility.GetPackageSourceMapping(packagePatterns);

            // Act
            using (var cacheContext = new SourceCacheContext())
            using (var packagesDirectory = TestDirectory.Create())
            using (var downloadResult = await PackageDownloader.GetDownloadResourceResultAsync(
                sourceRepositoryProvider.GetRepositories(),
                packageIdentity,
                new PackageDownloadContext(cacheContext, directDownloadDirectory: null, directDownload: false, packageSourceMapping),
                packagesDirectory,
                NullLogger.Instance,
                CancellationToken.None))
            {
                var targetPackageStream = downloadResult.PackageStream;

                // Assert
                Assert.True(targetPackageStream.CanSeek);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_WithSourceMappingNotFound_PackageNotFound()
        {
            // Arrange
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(new[]
            {
                TestSourceRepositoryUtility.V3PackageSource,
                TestSourceRepositoryUtility.V2PackageSource,
            });

            string mappedPackageId = "jQuery";
            string notMappedPackageId = "jQuery.Validation";
            string notMappedPackageVersion = "1.19.5";
            string packagePatterns = $"{TestSourceRepositoryUtility.V3PackageSource.Name},{mappedPackageId}|{TestSourceRepositoryUtility.V2PackageSource.Name},{mappedPackageId}";
            var notFoundPackageIdentity = new PackageIdentity(notMappedPackageId, new NuGetVersion(notMappedPackageVersion));
            PackageSourceMapping packageSourceMapping = PackageSourceMappingUtility.GetPackageSourceMapping(packagePatterns);

            // Act
            using var cacheContext = new SourceCacheContext();
            using var packagesDirectory = TestDirectory.Create();
            var exception = await Assert.ThrowsAsync<FatalProtocolException>(
                () => PackageDownloader.GetDownloadResourceResultAsync(
                    sourceRepositoryProvider.GetRepositories(),
                    notFoundPackageIdentity,
                    new PackageDownloadContext(cacheContext, directDownloadDirectory: null, directDownload: false, packageSourceMapping),
                    packagesDirectory,
                    NullLogger.Instance,
                    CancellationToken.None));

            Assert.Contains($"Unable to find version '{notMappedPackageVersion}' of package '{notMappedPackageId}'.", exception.Message);
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_MultipleSources_IncludesTaskStatusInException()
        {
            using (var test = new PackageDownloaderTest())
            {
                var resourceProvider = new Mock<INuGetResourceProvider>();
                var resource = new Mock<DownloadResource>();

                resourceProvider.SetupGet(x => x.Name)
                    .Returns(nameof(DownloadResource) + "Provider");
                resourceProvider.SetupGet(x => x.ResourceType)
                    .Returns(typeof(DownloadResource));
                resourceProvider.Setup(x => x.TryCreate(
                        It.IsNotNull<SourceRepository>(),
                        It.IsAny<CancellationToken>()))
                    .Throws(new OperationCanceledException());

                var sourceRepositories = new[]
                {
                    new SourceRepository(test.SourceRepository.PackageSource, new[] { resourceProvider.Object })
                };

                var exception = await Assert.ThrowsAsync<FatalProtocolException>(
                    () => PackageDownloader.GetDownloadResourceResultAsync(
                        sourceRepositories,
                        test.PackageIdentity,
                        test.Context,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        token: CancellationToken.None));

                Assert.Contains(": Canceled", exception.Message);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_SupportsDownloadResultWithoutPackageStream()
        {
            using (var test = new PackageDownloaderTest())
            using (var stream = new MemoryStream())
            using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create))
            using (var packageReader = new PackageArchiveReader(zipArchive))
            {
                var resourceProvider = new Mock<INuGetResourceProvider>();
                var resource = new Mock<DownloadResource>();
                var expectedResult = new DownloadResourceResult(
                    packageReader,
                    test.SourceRepository.PackageSource.Source);

                resource.Setup(x => x.GetDownloadResourceResultAsync(
                        It.IsNotNull<PackageIdentity>(),
                        It.IsNotNull<PackageDownloadContext>(),
                        It.IsAny<string>(),
                        It.IsNotNull<ILogger>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedResult);

                resourceProvider.SetupGet(x => x.Name)
                    .Returns(nameof(DownloadResource) + "Provider");
                resourceProvider.SetupGet(x => x.ResourceType)
                    .Returns(typeof(DownloadResource));
                resourceProvider.Setup(x => x.TryCreate(
                        It.IsNotNull<SourceRepository>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Tuple<bool, INuGetResource>(true, resource.Object));

                var sourceRepository = new SourceRepository(
                    test.SourceRepository.PackageSource,
                    new[] { resourceProvider.Object });

                var actualResult = await PackageDownloader.GetDownloadResourceResultAsync(
                    sourceRepository,
                    test.PackageIdentity,
                    test.Context,
                    globalPackagesFolder: "",
                    logger: NullLogger.Instance,
                    token: CancellationToken.None);

                Assert.Equal(DownloadResourceResultStatus.AvailableWithoutStream, actualResult.Status);
                Assert.Same(expectedResult, actualResult);
            }
        }

        private static async Task VerifyDirectDownloadSkipsGlobalPackagesFolderAsync(
            SourceRepositoryProvider sourceRepositoryProvider)
        {
            // Arrange
            var sourceRepository = sourceRepositoryProvider.GetRepositories().First();
            var packageIdentity = new PackageIdentity("jQuery", new NuGetVersion("1.8.2"));

            using (var packagesDirectory = TestDirectory.Create())
            using (var directDownloadDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var downloadContext = new PackageDownloadContext(
                    cacheContext,
                    directDownloadDirectory,
                    directDownload: true);

                // Act
                using (var downloadResult = await PackageDownloader.GetDownloadResourceResultAsync(
                    sourceRepository,
                    packageIdentity,
                    downloadContext,
                    packagesDirectory,
                    NullLogger.Instance,
                    CancellationToken.None))
                {
                    var targetPackageStream = downloadResult.PackageStream;

                    using (var packageArchiveReader = new PackageArchiveReader(targetPackageStream))
                    {
                        var contentHash = packageArchiveReader.GetContentHash(CancellationToken.None);

                        // Assert
                        Assert.Equal(_jQuery182ContentHash, contentHash);
                        Assert.True(targetPackageStream.CanSeek);
                    }
                }

                // Verify that the direct download directory is empty. The package should be downloaded to a temporary
                // file opened with DeleteOnClose.
                Assert.Equal(0, Directory.EnumerateFileSystemEntries(directDownloadDirectory).Count());

                // Verify that the package was not cached in the Global Packages Folder
                var globalPackage = GlobalPackagesFolderUtility.GetPackage(packageIdentity, packagesDirectory);
                Assert.Null(globalPackage);
            }
        }

        private sealed class PackageDownloaderTest : IDisposable
        {
            private readonly SourceCacheContext _sourceCacheContext;

            internal PackageDownloadContext Context { get; }
            internal PackageIdentity PackageIdentity { get; }
            internal SourceRepository SourceRepository { get; }

            internal PackageDownloaderTest()
            {
                _sourceCacheContext = new SourceCacheContext();
                Context = new PackageDownloadContext(_sourceCacheContext);
                PackageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));
                SourceRepository = new SourceRepository(
                    new PackageSource("https://unit.test"),
                    Enumerable.Empty<INuGetResourceProvider>());
            }

            public void Dispose()
            {
                _sourceCacheContext.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
