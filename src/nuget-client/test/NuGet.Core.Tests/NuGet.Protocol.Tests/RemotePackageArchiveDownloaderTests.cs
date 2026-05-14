// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class RemotePackageArchiveDownloaderTests
    {
        private static readonly PackageIdentity _packageIdentity;

        static RemotePackageArchiveDownloaderTests()
        {
            _packageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));
        }

        [Fact]
        public void Constructor_ThrowsForNullFindPackageByIdResource()
        {
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new RemotePackageArchiveDownloader(
                        null,
                        resource: null,
                        packageIdentity: _packageIdentity,
                        cacheContext: sourceCacheContext,
                        logger: NullLogger.Instance));

                Assert.Equal("resource", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_ThrowsForNullPackageIdentity()
        {
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new RemotePackageArchiveDownloader(
                        null,
                        Mock.Of<FindPackageByIdResource>(),
                        packageIdentity: null,
                        cacheContext: sourceCacheContext,
                        logger: NullLogger.Instance));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_ThrowsForNullSourceCacheContext()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new RemotePackageArchiveDownloader(
                    null,
                    Mock.Of<FindPackageByIdResource>(),
                    _packageIdentity,
                    cacheContext: null,
                    logger: NullLogger.Instance));

            Assert.Equal("cacheContext", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullLogger()
        {
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new RemotePackageArchiveDownloader(
                        null,
                        Mock.Of<FindPackageByIdResource>(),
                        _packageIdentity,
                        sourceCacheContext,
                        logger: null));

                Assert.Equal("logger", exception.ParamName);
            }

        }

        [Fact]
        public async Task Constructor_InitializesPropertiesAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                test.Resource.Setup(x => x.CopyNupkgToStreamAsync(
                        It.IsNotNull<string>(),
                        It.IsNotNull<NuGetVersion>(),
                        It.IsNotNull<Stream>(),
                        It.IsNotNull<SourceCacheContext>(),
                        It.IsNotNull<ILogger>(),
                        It.IsAny<CancellationToken>()))
                    .Callback<string, NuGetVersion, Stream, SourceCacheContext, ILogger, CancellationToken>(async
                        (id, version, stream, cacheContext, logger, cancellationToken) =>
                        {
                            var remoteDirectoryPath = Path.Combine(test.TestDirectory.Path, "remote");

                            Directory.CreateDirectory(remoteDirectoryPath);

                            var packageContext = new SimpleTestPackageContext()
                            {
                                Id = test.PackageIdentity.Id,
                                Version = test.PackageIdentity.Version.ToNormalizedString()
                            };

                            packageContext.AddFile($"lib/net45/{test.PackageIdentity.Id}.dll");

                            await SimpleTestPackageUtility.CreatePackagesAsync(remoteDirectoryPath, packageContext);

                            var sourcePackageFilePath = Path.Combine(
                                remoteDirectoryPath,
                                $"{test.PackageIdentity.Id}.{test.PackageIdentity.Version.ToNormalizedString()}.nupkg");

                            using (var remoteStream = File.OpenRead(sourcePackageFilePath))
                            {
                                remoteStream.CopyTo(stream);
                            }
                        })
                    .ReturnsAsync(true);

                var destinationFilePath = Path.Combine(test.TestDirectory.Path, "a");

                await test.Downloader.CopyNupkgFileToAsync(
                    destinationFilePath,
                    CancellationToken.None);

                Assert.IsType<PackageArchiveReader>(test.Downloader.ContentReader);
                Assert.IsType<PackageArchiveReader>(test.Downloader.CoreReader);
            }
        }

        [Fact]
        public async Task Dispose_IsIdempotentAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                test.Downloader.Dispose();
                test.Downloader.Dispose();
            }
        }

        [Fact]
        public async Task ContentReader_ThrowsIfCopyNupkgFileToAsyncNotCalledFirstAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                Assert.Throws<InvalidOperationException>(() => test.Downloader.ContentReader);
            }
        }

        [Fact]
        public async Task ContentReader_ThrowsIfDisposedAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                test.Downloader.Dispose();

                var exception = Assert.Throws<ObjectDisposedException>(() => test.Downloader.ContentReader);

                Assert.Equal(nameof(RemotePackageArchiveDownloader), exception.ObjectName);
            }
        }

        [Fact]
        public async Task CoreReader_ThrowsIfCopyNupkgFileToAsyncNotCalledFirstAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                Assert.Throws<InvalidOperationException>(() => test.Downloader.CoreReader);
            }
        }

        [Fact]
        public async Task CoreReader_ThrowsIfDisposedAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                test.Downloader.Dispose();

                var exception = Assert.Throws<ObjectDisposedException>(() => test.Downloader.CoreReader);

                Assert.Equal(nameof(RemotePackageArchiveDownloader), exception.ObjectName);
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ThrowsIfDisposedAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                test.Downloader.Dispose();

                var exception = await Assert.ThrowsAsync<ObjectDisposedException>(
                    () => test.Downloader.CopyNupkgFileToAsync(
                        destinationFilePath: "a",
                        cancellationToken: CancellationToken.None));

                Assert.Equal(nameof(RemotePackageArchiveDownloader), exception.ObjectName);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CopyNupkgFileToAsync_ThrowsForNullOrEmptyDestinationFilePathAsync(string destinationFilePath)
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => test.Downloader.CopyNupkgFileToAsync(
                        destinationFilePath,
                        CancellationToken.None));

                Assert.Equal("destinationFilePath", exception.ParamName);
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ThrowsIfCancelledAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Downloader.CopyNupkgFileToAsync(
                        destinationFilePath: "a",
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ReturnsFalseIfExceptionHandledAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                test.Resource.Setup(x => x.CopyNupkgToStreamAsync(
                        It.IsNotNull<string>(),
                        It.IsNotNull<NuGetVersion>(),
                        It.IsNotNull<Stream>(),
                        It.IsNotNull<SourceCacheContext>(),
                        It.IsNotNull<ILogger>(),
                        It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new FatalProtocolException("simulated failure"));

                var destinationFilePath = Path.Combine(test.TestDirectory.Path, "a");

                test.Downloader.SetExceptionHandler(exception => TaskResult.True);

                var wasCopied = await test.Downloader.CopyNupkgFileToAsync(
                    destinationFilePath,
                    CancellationToken.None);

                Assert.False(wasCopied);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CopyNupkgFileToAsync_ReturnsResultFromFindPackageByIdResourceAsync(bool expectedResult)
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                test.Resource.Setup(x => x.CopyNupkgToStreamAsync(
                        It.IsNotNull<string>(),
                        It.IsNotNull<NuGetVersion>(),
                        It.IsNotNull<Stream>(),
                        It.IsNotNull<SourceCacheContext>(),
                        It.IsNotNull<ILogger>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedResult);

                var destinationFilePath = Path.Combine(test.TestDirectory.Path, "a");

                var actualResult = await test.Downloader.CopyNupkgFileToAsync(
                    destinationFilePath,
                    CancellationToken.None);

                Assert.Equal(expectedResult, actualResult);
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_RespectsThrottleAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            using (var throttle = new SemaphoreSlim(initialCount: 0, maxCount: 1))
            using (var copyEvent = new ManualResetEventSlim())
            {
                test.Resource.Setup(x => x.CopyNupkgToStreamAsync(
                        It.IsNotNull<string>(),
                        It.IsNotNull<NuGetVersion>(),
                        It.IsNotNull<Stream>(),
                        It.IsNotNull<SourceCacheContext>(),
                        It.IsNotNull<ILogger>(),
                        It.IsAny<CancellationToken>()))
                    .Callback<string, NuGetVersion, Stream, SourceCacheContext, ILogger, CancellationToken>(
                        (id, version, destination, sourceCacheContext, logger, cancellationToken) =>
                        {
                            copyEvent.Set();
                        })
                    .ReturnsAsync(true);

                var destinationFilePath = Path.Combine(test.TestDirectory.Path, "a");

                test.Downloader.SetThrottle(throttle);

                var wasCopied = false;

                var copyTask = Task.Run(async () =>
                {
                    wasCopied = await test.Downloader.CopyNupkgFileToAsync(
                        destinationFilePath,
                        CancellationToken.None);
                });

                await Task.Delay(100);

                Assert.False(copyEvent.IsSet);

                throttle.Release();

                await copyTask;

                Assert.True(copyEvent.IsSet);
                Assert.True(wasCopied);
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ReleasesThrottleOnExceptionAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            using (var throttle = new SemaphoreSlim(initialCount: 1, maxCount: 1))
            {
                test.Resource.Setup(x => x.CopyNupkgToStreamAsync(
                        It.IsNotNull<string>(),
                        It.IsNotNull<NuGetVersion>(),
                        It.IsNotNull<Stream>(),
                        It.IsNotNull<SourceCacheContext>(),
                        It.IsNotNull<ILogger>(),
                        It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new FatalProtocolException("simulated failure"));

                var destinationFilePath = Path.Combine(test.TestDirectory.Path, "a");

                test.Downloader.SetThrottle(throttle);

                var copyTask = Task.Run(async () =>
                {
                    try
                    {
                        await test.Downloader.CopyNupkgFileToAsync(
                            destinationFilePath,
                            CancellationToken.None);
                    }
                    catch (Exception)
                    {
                    }
                });

                await copyTask;

                Assert.Equal(1, throttle.CurrentCount);
            }
        }

        [Fact]
        public async Task GetPackageHashAsync_ThrowsIfDisposedAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                test.Downloader.Dispose();

                var exception = await Assert.ThrowsAsync<ObjectDisposedException>(
                    () => test.Downloader.GetPackageHashAsync(
                        hashAlgorithm: "SHA512",
                        cancellationToken: CancellationToken.None));

                Assert.Equal(nameof(RemotePackageArchiveDownloader), exception.ObjectName);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetPackageHashAsync_ThrowsForNullOrEmptyHashAlgorithmAsync(string hashAlgorithm)
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => test.Downloader.GetPackageHashAsync(
                        hashAlgorithm,
                        CancellationToken.None));

                Assert.Equal("hashAlgorithm", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetPackageHashAsync_ThrowsIfCancelledAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Downloader.GetPackageHashAsync(
                        hashAlgorithm: "a",
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetPackageHashAsync_ReturnsPackageHashAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                var destinationFilePath = Path.Combine(test.TestDirectory.Path, "a");

                test.Resource.Setup(x => x.CopyNupkgToStreamAsync(
                        It.IsNotNull<string>(),
                        It.IsNotNull<NuGetVersion>(),
                        It.IsNotNull<Stream>(),
                        It.IsNotNull<SourceCacheContext>(),
                        It.IsNotNull<ILogger>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

                await test.Downloader.CopyNupkgFileToAsync(
                    destinationFilePath,
                    CancellationToken.None);

                var actualResult = await test.Downloader.GetPackageHashAsync(
                    hashAlgorithm: "SHA512",
                    cancellationToken: CancellationToken.None);

                Assert.Equal("z4PhNX7vuL3xVChQ1m2AB9Yg5AULVxXcg/SpIdNs6c5H0NE8XYXysP+DGNKHfuwvY7kxvUdBeoGlODJ6+SfaPg==", actualResult);
            }
        }

        [Fact]
        public async Task SetExceptionHandler_ThrowsForNullHandlerAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Downloader.SetExceptionHandler(handleExceptionAsync: null));

                Assert.Equal("handleExceptionAsync", exception.ParamName);
            }
        }

        [Fact]
        public async Task SetThrottle_AcceptsNullThrottleAsync()
        {
            using (var test = await RemotePackageArchiveDownloaderTest.CreateAsync())
            {
                test.Downloader.SetThrottle(throttle: null);
            }
        }

        private sealed class RemotePackageArchiveDownloaderTest : IDisposable
        {
            internal RemotePackageArchiveDownloader Downloader { get; }
            internal PackageIdentity PackageIdentity { get; }
            internal Mock<FindPackageByIdResource> Resource { get; }
            internal SourceCacheContext SourceCacheContext { get; }
            internal TestDirectory TestDirectory { get; }

            private RemotePackageArchiveDownloaderTest(
                TestDirectory testDirectory,
                PackageIdentity packageIdentity,
                Mock<FindPackageByIdResource> resource,
                SourceCacheContext sourceCacheContext,
                RemotePackageArchiveDownloader downloader)
            {
                TestDirectory = testDirectory;
                PackageIdentity = packageIdentity;
                Resource = resource;
                SourceCacheContext = sourceCacheContext;
                Downloader = downloader;
            }

            public void Dispose()
            {
                Downloader.Dispose();
                SourceCacheContext.Dispose();
                TestDirectory.Dispose();

                GC.SuppressFinalize(this);
            }

            internal static async Task<RemotePackageArchiveDownloaderTest> CreateAsync()
            {
                var testDirectory = TestDirectory.Create();
                var sourceCacheContext = new SourceCacheContext();
                var packageContext = new SimpleTestPackageContext()
                {
                    Id = _packageIdentity.Id,
                    Version = _packageIdentity.Version.ToNormalizedString()
                };

                packageContext.AddFile($"lib/net45/{_packageIdentity.Id}.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory.Path, packageContext);

                var packageFilePath = Path.Combine(
                    testDirectory.Path,
                    $"{_packageIdentity.Id}.{_packageIdentity.Version.ToNormalizedString()}.nupkg");

                var resource = new Mock<FindPackageByIdResource>(MockBehavior.Strict);

                var downloader = new RemotePackageArchiveDownloader(
                    testDirectory.Path,
                    resource.Object,
                    _packageIdentity,
                    sourceCacheContext,
                    NullLogger.Instance);

                return new RemotePackageArchiveDownloaderTest(
                    testDirectory,
                    _packageIdentity,
                    resource,
                    sourceCacheContext,
                    downloader);
            }
        }
    }
}
