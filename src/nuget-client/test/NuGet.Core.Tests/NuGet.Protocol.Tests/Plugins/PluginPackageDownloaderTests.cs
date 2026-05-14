// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginPackageDownloaderTests
    {
        private static readonly PackageIdentity _packageIdentity;
        private static readonly string _packageSourceRepository;

        static PluginPackageDownloaderTests()
        {
            _packageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));
            _packageSourceRepository = "https://unit.test";
        }

        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            using (var packageReader = new PluginPackageReader(
                Mock.Of<IPlugin>(),
                _packageIdentity,
                _packageSourceRepository))
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new PluginPackageDownloader(
                        plugin: null,
                        packageIdentity: _packageIdentity,
                        packageReader: packageReader,
                        packageSourceRepository: _packageSourceRepository));

                Assert.Equal("plugin", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_ThrowsForNullPackageIdentity()
        {
            using (var packageReader = new PluginPackageReader(
                Mock.Of<IPlugin>(),
                _packageIdentity,
                _packageSourceRepository))
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new PluginPackageDownloader(
                        Mock.Of<IPlugin>(),
                        packageIdentity: null,
                        packageReader: packageReader,
                        packageSourceRepository: _packageSourceRepository));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_ThrowsForNullPackageReader()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginPackageDownloader(
                    Mock.Of<IPlugin>(),
                    _packageIdentity,
                    packageReader: null,
                    packageSourceRepository: _packageSourceRepository));

            Assert.Equal("packageReader", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            using (var packageReader = new PluginPackageReader(
                Mock.Of<IPlugin>(),
                _packageIdentity,
                _packageSourceRepository))
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new PluginPackageDownloader(
                        Mock.Of<IPlugin>(),
                        _packageIdentity,
                        packageReader,
                        packageSourceRepository));

                Assert.Equal("packageSourceRepository", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                Assert.Same(test.PackageReader, test.Downloader.ContentReader);
                Assert.Same(test.PackageReader, test.Downloader.CoreReader);
            }
        }

        [Fact]
        public void Dispose_DisposesDisposables()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Downloader.Dispose();

                // PluginPackageDownloader.Dispose() calls both IPlugin.Dispose() and
                // PluginPackageReader.Dispose().  The latter call IPlugin.Dispose()
                // on its own IPlugin instance.
                test.Plugin.Verify(x => x.Dispose(), Times.Exactly(2));
            }
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Downloader.Dispose();
                test.Downloader.Dispose();

                test.Plugin.Verify(x => x.Dispose(), Times.Exactly(2));
            }
        }

        [Fact]
        public void ContentReader_ThrowsIfDisposed()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Downloader.Dispose();

                var exception = Assert.Throws<ObjectDisposedException>(() => test.Downloader.ContentReader);

                Assert.Equal(nameof(PluginPackageDownloader), exception.ObjectName);
            }
        }

        [Fact]
        public void CoreReader_ThrowsIfDisposed()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Downloader.Dispose();

                var exception = Assert.Throws<ObjectDisposedException>(() => test.Downloader.CoreReader);

                Assert.Equal(nameof(PluginPackageDownloader), exception.ObjectName);
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ThrowsIfDisposed()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Downloader.Dispose();

                var exception = await Assert.ThrowsAsync<ObjectDisposedException>(
                    () => test.Downloader.CopyNupkgFileToAsync(
                        destinationFilePath: "a",
                        cancellationToken: CancellationToken.None));

                Assert.Equal(nameof(PluginPackageDownloader), exception.ObjectName);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CopyNupkgFileToAsync_ThrowsForNullOrEmptyDestinationFilePath(string destinationFilePath)
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => test.Downloader.CopyNupkgFileToAsync(destinationFilePath, CancellationToken.None));

                Assert.Equal("destinationFilePath", exception.ParamName);
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ThrowsIfCancelled()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Downloader.CopyNupkgFileToAsync(
                        destinationFilePath: "a",
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ReturnsFalseIfExceptionHandled()
        {
            var destinationFilePath = "a";
            var connection = new Mock<IConnection>(MockBehavior.Strict);

            connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyNupkgFileRequest, CopyNupkgFileResponse>(
                    It.Is<MessageMethod>(m => m == MessageMethod.CopyNupkgFile),
                    It.Is<CopyNupkgFileRequest>(c => c.PackageId == _packageIdentity.Id &&
                        c.PackageVersion == _packageIdentity.Version.ToNormalizedString() &&
                        c.PackageSourceRepository == _packageSourceRepository &&
                        c.DestinationFilePath == destinationFilePath),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("simulated failure"));

            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Plugin.SetupGet(x => x.Connection)
                    .Returns(connection.Object);

                test.Downloader.SetExceptionHandler(exception => TaskResult.True);

                var wasCopied = await test.Downloader.CopyNupkgFileToAsync(
                    destinationFilePath,
                    CancellationToken.None);

                Assert.False(wasCopied);
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ReturnsFalseIfNupkgFileNotCopied()
        {
            var destinationFilePath = "a";
            var connection = new Mock<IConnection>(MockBehavior.Strict);

            connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyNupkgFileRequest, CopyNupkgFileResponse>(
                    It.Is<MessageMethod>(m => m == MessageMethod.CopyNupkgFile),
                    It.Is<CopyNupkgFileRequest>(c => c.PackageId == _packageIdentity.Id &&
                        c.PackageVersion == _packageIdentity.Version.ToNormalizedString() &&
                        c.PackageSourceRepository == _packageSourceRepository &&
                        c.DestinationFilePath == destinationFilePath),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CopyNupkgFileResponse(MessageResponseCode.Error));

            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Plugin.SetupGet(x => x.Connection)
                    .Returns(connection.Object);

                var wasCopied = await test.Downloader.CopyNupkgFileToAsync(
                    destinationFilePath,
                    CancellationToken.None);

                Assert.False(wasCopied);
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ReturnsTrueIfNupkgFileCopied()
        {
            var destinationFilePath = "a";
            var connection = new Mock<IConnection>(MockBehavior.Strict);

            connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyNupkgFileRequest, CopyNupkgFileResponse>(
                    It.Is<MessageMethod>(m => m == MessageMethod.CopyNupkgFile),
                    It.Is<CopyNupkgFileRequest>(c => c.PackageId == _packageIdentity.Id &&
                        c.PackageVersion == _packageIdentity.Version.ToNormalizedString() &&
                        c.PackageSourceRepository == _packageSourceRepository &&
                        c.DestinationFilePath == destinationFilePath),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CopyNupkgFileResponse(MessageResponseCode.Success));

            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Plugin.SetupGet(x => x.Connection)
                    .Returns(connection.Object);

                var wasCopied = await test.Downloader.CopyNupkgFileToAsync(
                    destinationFilePath,
                    CancellationToken.None);

                Assert.True(wasCopied);
            }
        }

        [Fact]
        public async Task CopyNupkgFileToAsync_ReturnsFalseIfPackageDownloadMarkerFileCreated()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var destinationFilePath = Path.Combine(testDirectory.Path, _packageIdentity.Id);
                var connection = new Mock<IConnection>(MockBehavior.Strict);

                connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyNupkgFileRequest, CopyNupkgFileResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyNupkgFile),
                        It.Is<CopyNupkgFileRequest>(c => c.PackageId == _packageIdentity.Id &&
                            c.PackageVersion == _packageIdentity.Version.ToNormalizedString() &&
                            c.PackageSourceRepository == _packageSourceRepository &&
                            c.DestinationFilePath == destinationFilePath),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new CopyNupkgFileResponse(MessageResponseCode.NotFound));

                using (var test = PluginPackageDownloaderTest.Create())
                {
                    test.Plugin.SetupGet(x => x.Connection)
                        .Returns(connection.Object);

                    var wasCopied = await test.Downloader.CopyNupkgFileToAsync(
                        destinationFilePath,
                        CancellationToken.None);

                    var markerFilePath = Path.Combine(testDirectory.Path, $"{_packageIdentity.Id}.packagedownload.marker");

                    Assert.False(wasCopied);
                    Assert.True(File.Exists(markerFilePath));
                }
            }
        }

        [Fact]
        public async Task GetPackageHashAsync_ThrowsIfDisposed()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Downloader.Dispose();

                var exception = await Assert.ThrowsAsync<ObjectDisposedException>(
                    () => test.Downloader.GetPackageHashAsync(
                        hashAlgorithm: "a",
                        cancellationToken: CancellationToken.None));

                Assert.Equal(nameof(PluginPackageDownloader), exception.ObjectName);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetPackageHashAsync_ThrowsForNullOrEmptyDestinationFilePath(string hashAlgorithm)
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => test.Downloader.GetPackageHashAsync(hashAlgorithm, CancellationToken.None));

                Assert.Equal("hashAlgorithm", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetPackageHashAsync_ThrowsIfCancelled()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Downloader.GetPackageHashAsync(
                        hashAlgorithm: "SHA512",
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Theory]
        [InlineData(MessageResponseCode.Error)]
        [InlineData(MessageResponseCode.NotFound)]
        public async Task GetPackageHashAsync_ReturnsNullForNonSuccess(MessageResponseCode responseCode)
        {
            var hashAlgorithm = "a";
            var response = new GetPackageHashResponse(responseCode, hash: null);
            var connection = new Mock<IConnection>(MockBehavior.Strict);

            connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetPackageHashRequest, GetPackageHashResponse>(
                    It.Is<MessageMethod>(m => m == MessageMethod.GetPackageHash),
                    It.Is<GetPackageHashRequest>(c => c.PackageId == _packageIdentity.Id &&
                        c.PackageVersion == _packageIdentity.Version.ToNormalizedString() &&
                        c.PackageSourceRepository == _packageSourceRepository &&
                        c.HashAlgorithm == hashAlgorithm),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Plugin.SetupGet(x => x.Connection)
                    .Returns(connection.Object);

                var packageHash = await test.Downloader.GetPackageHashAsync(
                    hashAlgorithm,
                    CancellationToken.None);

                Assert.Null(packageHash);
            }
        }

        [Fact]
        public async Task GetPackageHashAsync_ReturnsPackageHashForSuccess()
        {
            var hashAlgorithm = "a";
            var response = new GetPackageHashResponse(MessageResponseCode.Success, hash: "b");
            var connection = new Mock<IConnection>(MockBehavior.Strict);

            connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetPackageHashRequest, GetPackageHashResponse>(
                    It.Is<MessageMethod>(m => m == MessageMethod.GetPackageHash),
                    It.Is<GetPackageHashRequest>(c => c.PackageId == _packageIdentity.Id &&
                        c.PackageVersion == _packageIdentity.Version.ToNormalizedString() &&
                        c.PackageSourceRepository == _packageSourceRepository &&
                        c.HashAlgorithm == hashAlgorithm),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            using (var test = PluginPackageDownloaderTest.Create())
            {
                test.Plugin.SetupGet(x => x.Connection)
                    .Returns(connection.Object);

                var packageHash = await test.Downloader.GetPackageHashAsync(
                    hashAlgorithm,
                    CancellationToken.None);

                Assert.Equal(response.Hash, packageHash);
            }
        }

        [Fact]
        public void SetExceptionHandler_ThrowsForNullHandler()
        {
            using (var test = PluginPackageDownloaderTest.Create())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Downloader.SetExceptionHandler(handleExceptionAsync: null));

                Assert.Equal("handleExceptionAsync", exception.ParamName);
            }
        }

        private sealed class PluginPackageDownloaderTest : IDisposable
        {
            internal PluginPackageDownloader Downloader { get; }
            internal PackageIdentity PackageIdentity { get; }
            internal PluginPackageReader PackageReader { get; }
            internal Mock<IPlugin> Plugin { get; }

            private PluginPackageDownloaderTest(
                PackageIdentity packageIdentity,
                Mock<IPlugin> plugin,
                PluginPackageReader packageReader,
                PluginPackageDownloader downloader)
            {
                PackageIdentity = packageIdentity;
                Plugin = plugin;
                PackageReader = packageReader;
                Downloader = downloader;
            }

            public void Dispose()
            {
                Downloader.Dispose();

                GC.SuppressFinalize(this);
            }

            internal static PluginPackageDownloaderTest Create()
            {
                var plugin = new Mock<IPlugin>(MockBehavior.Strict);

                plugin.Setup(x => x.Dispose());

                var packageReader = new PluginPackageReader(plugin.Object, _packageIdentity, _packageSourceRepository);
                var downloader = new PluginPackageDownloader(
                    plugin.Object,
                    _packageIdentity,
                    packageReader,
                    _packageSourceRepository);

                return new PluginPackageDownloaderTest(
                    _packageIdentity,
                    plugin,
                    packageReader,
                    downloader);
            }
        }
    }
}
