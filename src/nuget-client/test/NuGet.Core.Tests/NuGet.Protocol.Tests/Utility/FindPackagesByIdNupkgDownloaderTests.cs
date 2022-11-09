// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class FindPackagesByIdNupkgDownloaderTests
    {
        [Theory]
        [InlineData(HttpStatusCode.NoContent, 1)]
        [InlineData(HttpStatusCode.NotFound, 1)]
        [InlineData(HttpStatusCode.InternalServerError, EnhancedHttpRetryHelper.DefaultEnabled ? EnhancedHttpRetryHelper.DefaultRetryCount : 3)]
        public async Task CopyNupkgToStreamAsync_DoesNothingWithDestinationStreamWhenNupkgIsNotFoundAsync(
            HttpStatusCode statusCode,
            int expectedRequests)
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var tc = await TestContext.CreateAsync(testDirectory);
                tc.StatusCode = statusCode;

                // Act
                var copied = await tc.Target.CopyNupkgToStreamAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    tc.DestinationStream,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.False(copied);
                var actualContent = tc.DestinationStream.ToArray();
                Assert.Empty(actualContent);
                Assert.Equal(expectedRequests, tc.RequestCount);
            }
        }

        [Theory]
        [InlineData(HttpStatusCode.NoContent, 1)]
        [InlineData(HttpStatusCode.NotFound, 1)]
        [InlineData(HttpStatusCode.InternalServerError, EnhancedHttpRetryHelper.DefaultEnabled ? EnhancedHttpRetryHelper.DefaultRetryCount : 3)]
        public async Task GetNuspecReaderFromNupkgAsync_ThrowsWhenNupkgIsNotFoundAsync(
            HttpStatusCode statusCode,
            int expectedRequests)
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var tc = await TestContext.CreateAsync(testDirectory);
                tc.StatusCode = statusCode;

                // Act & Assert
                var exception = await Assert.ThrowsAsync<PackageNotFoundProtocolException>(
                    () => tc.Target.GetNuspecReaderFromNupkgAsync(
                        tc.Identity,
                        tc.NupkgUrl,
                        cacheContext,
                        tc.Logger,
                        CancellationToken.None));
                Assert.Contains(tc.Identity.Id, exception.Message);
                Assert.Equal(expectedRequests, tc.RequestCount);
            }
        }

        [Fact]
        public async Task GetNuspecReaderFromNupkgAsync_GetsNuspecReaderAsync()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var tc = await TestContext.CreateAsync(testDirectory);

                // Act
                var nuspecReader = await tc.Target.GetNuspecReaderFromNupkgAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.NotNull(nuspecReader);
                Assert.Equal(tc.Identity.Id, nuspecReader.GetId());
                Assert.Equal(tc.Identity.Version.ToFullString(), nuspecReader.GetVersion().ToFullString());
                Assert.Equal(1, tc.RequestCount);
            }
        }

        [Fact]
        public async Task GetNuspecReaderFromNupkgAsync_DoesNotWriteCacheFileWithDirectDownloadAsync()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var tc = await TestContext.CreateAsync(testDirectory);
                cacheContext.DirectDownload = true;

                // Act
                // This should not write to the disk cache.
                var nuspecReaderA = await tc.Target.GetNuspecReaderFromNupkgAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);

                // This also should not write to the disk cache but the .nuspec should be cached in memory.
                var nuspecReaderB = await tc.Target.GetNuspecReaderFromNupkgAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.Equal(tc.Identity, nuspecReaderA.GetIdentity());
                Assert.Equal(tc.Identity, nuspecReaderB.GetIdentity());
                Assert.Equal(1, tc.RequestCount);
                Assert.False(Directory.Exists(tc.HttpCacheDirectory));
            }
        }

        [Fact]
        public async Task CopyNupkgToStreamAsync_CopiesNupkgToDestinationStreamAsync()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var tc = await TestContext.CreateAsync(testDirectory);

                // Act
                var copied = await tc.Target.CopyNupkgToStreamAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    tc.DestinationStream,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.True(copied);
                var actualContent = tc.DestinationStream.ToArray();
                Assert.Equal(tc.ExpectedContent, actualContent);
                Assert.Equal(1, tc.RequestCount);
            }
        }

        [Fact]
        public async Task CopyNupkgToStreamAsync_RemembersCacheFileLocationWithoutDirectDownloadAsync()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var tc = await TestContext.CreateAsync(testDirectory);

                // Act
                // This should record the cache entry in memory.
                var copiedA = await tc.Target.CopyNupkgToStreamAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    tc.DestinationStream,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);
                var actualContentA = tc.DestinationStream.ToArray();
                tc.DestinationStream.SetLength(0);

                // This should find that in-memory cache entry.
                var copiedB = await tc.Target.CopyNupkgToStreamAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    tc.DestinationStream,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);
                var actualContentB = tc.DestinationStream.ToArray();

                // Assert
                Assert.True(copiedA);
                Assert.True(copiedB);
                Assert.Equal(tc.ExpectedContent, actualContentA);
                Assert.Equal(tc.ExpectedContent, actualContentB);
                Assert.Equal(1, tc.RequestCount);
            }
        }

        [Fact]
        public async Task CopyNupkgToStreamAsync_DoesNotWriteCacheFileWithDirectDownloadAsync()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var tc = await TestContext.CreateAsync(testDirectory);
                cacheContext.DirectDownload = true;

                // Act
                // This should not write to the disk cache.
                var copiedA = await tc.Target.CopyNupkgToStreamAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    tc.DestinationStream,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);
                var actualContentA = tc.DestinationStream.ToArray();
                tc.DestinationStream.SetLength(0);

                // This also should not write to the disk cache.
                var copiedB = await tc.Target.CopyNupkgToStreamAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    tc.DestinationStream,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);
                var actualContentB = tc.DestinationStream.ToArray();

                // Assert
                Assert.True(copiedA);
                Assert.True(copiedB);
                Assert.Equal(tc.ExpectedContent, actualContentA);
                Assert.Equal(tc.ExpectedContent, actualContentB);
                Assert.Equal(2, tc.RequestCount);
                Assert.False(Directory.Exists(tc.HttpCacheDirectory));
            }
        }

        [Fact]
        public async Task CopyNupkgToStreamAsync_DirectDownloadPopulatesInMemoryCacheAsync()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
                var tc = await TestContext.CreateAsync(testDirectory);

                // Populate the disk cache.
                await tc.Target.CopyNupkgToStreamAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    new MemoryStream(),
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);

                tc.Initialize();

                // Act
                // This should find an entry on the disk cache.
                cacheContext.DirectDownload = true;
                var copiedA = await tc.Target.CopyNupkgToStreamAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    tc.DestinationStream,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);
                var actualContentA = tc.DestinationStream.ToArray();
                tc.DestinationStream.SetLength(0);

                // This should find an cache entry in memory.
                cacheContext.DirectDownload = false;
                var copiedB = await tc.Target.CopyNupkgToStreamAsync(
                    tc.Identity,
                    tc.NupkgUrl,
                    tc.DestinationStream,
                    cacheContext,
                    tc.Logger,
                    CancellationToken.None);
                var actualContentB = tc.DestinationStream.ToArray();

                // Assert
                Assert.True(copiedA);
                Assert.True(copiedB);
                Assert.Equal(tc.ExpectedContent, actualContentA);
                Assert.Equal(tc.ExpectedContent, actualContentB);
                Assert.Equal(0, tc.RequestCount);
                Assert.Equal(1, tc.HttpSource.CacheHits);
                Assert.Equal(0, tc.HttpSource.CacheMisses);
            }
        }

        private class TestContext
        {
            public static async Task<TestContext> CreateAsync(TestDirectory testDirectory)
            {
                var identity = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0-Beta"));

                var packageDirectory = Path.Combine(testDirectory, "packages");
                var package = await SimpleTestPackageUtility.CreateFullPackageAsync(
                    packageDirectory,
                    identity.Id,
                    identity.Version.ToString());

                var expectedContent = File.ReadAllBytes(package.FullName);

                return new TestContext(testDirectory, identity, expectedContent);
            }

            private TestContext(TestDirectory testDirectory, PackageIdentity identity, byte[] expectedContent)
            {
                TestDirectory = testDirectory;
                Identity = identity;
                ExpectedContent = expectedContent;

                PackageSource = new PackageSource("http://foo/index.json");
                NupkgUrl = "http://foo/package.nupkg";
                RequestCount = 0;
                HttpCacheDirectory = Path.Combine(testDirectory, "httpCache");
                StatusCode = HttpStatusCode.OK;

                Logger = new TestLogger();
                DestinationStream = new MemoryStream();

                Initialize();
            }

            public void Initialize()
            {
                HttpSource = new TestHttpSource(
                    PackageSource,
                    new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>()
                    {
                        {
                            NupkgUrl,
                            request =>
                            {
                                RequestCount++;

                                return Task.FromResult(new HttpResponseMessage
                                {
                                    StatusCode = StatusCode,
                                    Content = new ByteArrayContent(ExpectedContent)
                                });
                            }
                        }
                    })
                {
                    HttpCacheDirectory = HttpCacheDirectory,
                    DisableCaching = false
                };

                RequestCount = 0;

                Target = new FindPackagesByIdNupkgDownloader(HttpSource);
            }

            public byte[] ExpectedContent { get; }
            public PackageIdentity Identity { get; }
            public TestLogger Logger { get; }
            public TestDirectory TestDirectory { get; }
            public MemoryStream DestinationStream { get; }
            public string NupkgUrl { get; }
            public int RequestCount { get; private set; }
            public string HttpCacheDirectory { get; }
            public PackageSource PackageSource { get; }
            public TestHttpSource HttpSource { get; private set; }
            public HttpStatusCode StatusCode { get; set; }
            public FindPackagesByIdNupkgDownloader Target { get; private set; }
        }

    }
}
