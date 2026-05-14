// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class DownloadResourcePluginTests
    {
        private readonly Mock<IConnection> _connection;
        private readonly Mock<ICredentialService> _credentialService;
        private readonly Mock<IMessageDispatcher> _dispatcher;
        private readonly PackageSource _packageSource;
        private readonly Mock<IPlugin> _plugin;
        private readonly Mock<IWebProxy> _proxy;
        private readonly DownloadResourcePlugin _resource;
        private readonly Mock<IPluginMulticlientUtilities> _utilities;

        public DownloadResourcePluginTests()
        {
            _packageSource = new PackageSource("https://unit.test");
            _proxy = new Mock<IWebProxy>();
            _credentialService = new Mock<ICredentialService>();
            _dispatcher = new Mock<IMessageDispatcher>();
            _connection = new Mock<IConnection>();
            _plugin = new Mock<IPlugin>();
            _utilities = new Mock<IPluginMulticlientUtilities>();

            _dispatcher.SetupGet(x => x.RequestHandlers)
                .Returns(new RequestHandlers());

            _connection.SetupGet(x => x.MessageDispatcher)
                .Returns(_dispatcher.Object);

            _plugin.SetupGet(x => x.Connection)
                .Returns(_connection.Object);

            _resource = new DownloadResourcePlugin(
                _plugin.Object,
                _utilities.Object,
                _packageSource);
        }

        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DownloadResourcePlugin(
                    plugin: null,
                    utilities: _utilities.Object,
                    packageSource: _packageSource));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullPluginMulticlientUtilities()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DownloadResourcePlugin(
                    _plugin.Object,
                    utilities: null,
                    packageSource: _packageSource));

            Assert.Equal("utilities", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullPackageSource()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DownloadResourcePlugin(
                    _plugin.Object,
                    _utilities.Object,
                    packageSource: null));

            Assert.Equal("packageSource", exception.ParamName);
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_ThrowsForNullIdentity()
        {
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _resource.GetDownloadResourceResultAsync(
                        identity: null,
                        downloadContext: new PackageDownloadContext(sourceCacheContext),
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("identity", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_ThrowsForNullDownloadContext()
        {
            var resource = new DownloadResourcePlugin(
                _plugin.Object,
                _utilities.Object,
                _packageSource);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => resource.GetDownloadResourceResultAsync(
                        new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                        downloadContext: null,
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("downloadContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_ThrowsForNullLogger()
        {
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _resource.GetDownloadResourceResultAsync(
                        new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                        new PackageDownloadContext(sourceCacheContext),
                        globalPackagesFolder: "",
                        logger: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("logger", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_ThrowsIfCancelled()
        {
            using (var sourceCacheContext = new SourceCacheContext())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => _resource.GetDownloadResourceResultAsync(
                        new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                        new PackageDownloadContext(sourceCacheContext),
                        globalPackagesFolder: "",
                        logger: NullLogger.Instance,
                        cancellationToken: new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_ThrowsForErrorPrefetchPackageResponse()
        {
            _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<PrefetchPackageRequest, PrefetchPackageResponse>(
                   It.Is<MessageMethod>(m => m == MessageMethod.PrefetchPackage),
                   It.IsNotNull<PrefetchPackageRequest>(),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(new PrefetchPackageResponse(MessageResponseCode.Error));

            using (var sourceCacheContext = new SourceCacheContext())
            {
                await Assert.ThrowsAsync<PluginException>(
                    () => _resource.GetDownloadResourceResultAsync(
                        new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                        new PackageDownloadContext(sourceCacheContext),
                        globalPackagesFolder: null,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_ReturnsDownloadResourceResultForSuccessfulPrefetchPackageResponse()
        {
            _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<PrefetchPackageRequest, PrefetchPackageResponse>(
                   It.Is<MessageMethod>(m => m == MessageMethod.PrefetchPackage),
                   It.IsNotNull<PrefetchPackageRequest>(),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(new PrefetchPackageResponse(MessageResponseCode.Success));

            using (var sourceCacheContext = new SourceCacheContext())
            using (var result = await _resource.GetDownloadResourceResultAsync(
                new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                new PackageDownloadContext(sourceCacheContext),
                globalPackagesFolder: null,
                logger: NullLogger.Instance,
                cancellationToken: CancellationToken.None))
            {
                Assert.NotNull(result);
                Assert.Equal(DownloadResourceResultStatus.AvailableWithoutStream, result.Status);
                Assert.IsType<PluginPackageReader>(result.PackageReader);
                Assert.Null(result.PackageStream);
                Assert.Equal(_packageSource.Source, result.PackageSource);
            }
        }

        [Fact]
        public async Task GetDownloadResourceResultAsync_ReturnsDownloadResourceResultForNotFoundPrefetchPackageResponse()
        {
            _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<PrefetchPackageRequest, PrefetchPackageResponse>(
                   It.Is<MessageMethod>(m => m == MessageMethod.PrefetchPackage),
                   It.IsNotNull<PrefetchPackageRequest>(),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(new PrefetchPackageResponse(MessageResponseCode.NotFound));

            using (var sourceCacheContext = new SourceCacheContext())
            using (var result = await _resource.GetDownloadResourceResultAsync(
                new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                new PackageDownloadContext(sourceCacheContext),
                globalPackagesFolder: null,
                logger: NullLogger.Instance,
                cancellationToken: CancellationToken.None))
            {
                Assert.NotNull(result);
                Assert.Equal(DownloadResourceResultStatus.NotFound, result.Status);
                Assert.Null(result.PackageReader);
                Assert.Null(result.PackageStream);
                Assert.Null(result.PackageSource);
            }
        }
    }
}
