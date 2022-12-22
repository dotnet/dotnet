// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginFindPackageByIdResourceTests
    {
        private readonly Mock<ICredentialService> _credentialService;
        private readonly PackageSource _packageSource;
        private readonly Mock<IPlugin> _plugin;
        private readonly Mock<IWebProxy> _proxy;
        private readonly Mock<IPluginMulticlientUtilities> _utilities;

        public PluginFindPackageByIdResourceTests()
        {
            _packageSource = new PackageSource("https://unit.test");
            _proxy = new Mock<IWebProxy>();
            _credentialService = new Mock<ICredentialService>();
            _plugin = new Mock<IPlugin>();
            _utilities = new Mock<IPluginMulticlientUtilities>();

            HttpHandlerResourceV3.CredentialService = new Lazy<ICredentialService>(() => Mock.Of<ICredentialService>());
        }

        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginFindPackageByIdResource(
                    plugin: null,
                    utilities: _utilities.Object,
                    packageSource: _packageSource));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullPluginMulticlientUtilities()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginFindPackageByIdResource(
                    _plugin.Object,
                    utilities: null,
                    packageSource: _packageSource));

            Assert.Equal("utilities", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullPackageSource()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginFindPackageByIdResource(
                    _plugin.Object,
                    _utilities.Object,
                    packageSource: null));

            Assert.Equal("packageSource", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetAllVersionsAsync_ThrowsForNullOrEmptyIdAsync(string id)
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => test.Resource.GetAllVersionsAsync(
                        id,
                        test.SourceCacheContext,
                        NullLogger.Instance,
                        CancellationToken.None));

                Assert.Equal("id", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetAllVersionsAsync_ThrowsForNullSourceCacheContextAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Resource.GetAllVersionsAsync(
                        test.PackageIdentity.Id,
                        cacheContext: null,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("cacheContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetAllVersionsAsync_ThrowsForNullLoggerAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Resource.GetAllVersionsAsync(
                        test.PackageIdentity.Id,
                        test.SourceCacheContext,
                        logger: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("logger", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetAllVersionsAsync_ThrowIfCancelledAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Resource.GetAllVersionsAsync(
                        test.PackageIdentity.Id,
                        test.SourceCacheContext,
                        NullLogger.Instance,
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetAllVersionsAsync_ThrowsForErrorAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync(MessageResponseCode.Error))
            {
                await Assert.ThrowsAsync<FatalProtocolException>(
                    () => test.Resource.GetAllVersionsAsync(
                        test.PackageIdentity.Id,
                        test.SourceCacheContext,
                        NullLogger.Instance,
                        CancellationToken.None));
            }
        }

        [Fact]
        public async Task GetAllVersionsAsync_ReturnsEmptyEnumerableIfPackageIdNotFoundAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync(MessageResponseCode.NotFound))
            {
                var versions = await test.Resource.GetAllVersionsAsync(
                    test.PackageIdentity.Id,
                    test.SourceCacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.Empty(versions);
            }
        }

        [Fact]
        public async Task GetAllVersionsAsync_ReturnsAllVersionsAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync(MessageResponseCode.Success))
            {
                var versions = await test.Resource.GetAllVersionsAsync(
                    test.PackageIdentity.Id,
                    test.SourceCacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.Equal(new[]
                    {
                        NuGetVersion.Parse("1.0.0"),
                        NuGetVersion.Parse("2.0.0"),
                        NuGetVersion.Parse("3.0.0"),
                    }, versions);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetDependencyInfoAsync_ThrowsForNullOrEmptyIdAsync(string id)
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => test.Resource.GetDependencyInfoAsync(
                        id,
                        NuGetVersion.Parse("1.0.0"),
                        test.SourceCacheContext,
                        NullLogger.Instance,
                        CancellationToken.None));

                Assert.Equal("id", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDependencyInfoAsync_ThrowsForNullVersionAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Resource.GetDependencyInfoAsync(
                        test.PackageIdentity.Id,
                        version: null,
                        cacheContext: null,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("version", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDependencyInfoAsync_ThrowsForNullSourceCacheContextAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Resource.GetDependencyInfoAsync(
                        test.PackageIdentity.Id,
                        test.PackageIdentity.Version,
                        cacheContext: null,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("cacheContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDependencyInfoAsync_ThrowsForNullLoggerAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Resource.GetDependencyInfoAsync(
                        test.PackageIdentity.Id,
                        test.PackageIdentity.Version,
                        test.SourceCacheContext,
                        logger: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("logger", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetDependencyInfoAsync_ThrowIfCancelledAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Resource.GetDependencyInfoAsync(
                        test.PackageIdentity.Id,
                        test.PackageIdentity.Version,
                        test.SourceCacheContext,
                        NullLogger.Instance,
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetDependencyInfoAsync_ReturnsNullIfPackageNotFoundAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync(MessageResponseCode.NotFound))
            {
                var dependencyInfo = await test.Resource.GetDependencyInfoAsync(
                    test.PackageIdentity.Id,
                    test.PackageIdentity.Version,
                    test.SourceCacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.Null(dependencyInfo);
            }
        }

        [Fact]
        public async Task GetDependencyInfoAsync_GetsOriginalIdentityAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync(MessageResponseCode.Success))
            {
                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<PrefetchPackageRequest, PrefetchPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.PrefetchPackage),
                        It.IsNotNull<PrefetchPackageRequest>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new PrefetchPackageResponse(MessageResponseCode.Success));

                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetFilesInPackageRequest, GetFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetFilesInPackage),
                        It.IsNotNull<GetFilesInPackageRequest>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetFilesInPackageResponse(MessageResponseCode.Success, new[] { $"{test.PackageIdentity.Id}.nuspec" }));

                string tempNuspecFilePath = null;

                test.Connection.Setup(x => x.SendRequestAndReceiveResponseAsync<CopyFilesInPackageRequest, CopyFilesInPackageResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.CopyFilesInPackage),
                        It.IsNotNull<CopyFilesInPackageRequest>(),
                        It.IsAny<CancellationToken>()))
                   .Callback<MessageMethod, CopyFilesInPackageRequest, CancellationToken>(
                        (method, request, cancellationToken) =>
                        {
                            tempNuspecFilePath = Path.Combine(
                                request.DestinationFolderPath,
                                $"{test.PackageIdentity.Id}.nuspec");

                            File.WriteAllText(
                                tempNuspecFilePath,
                                $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                <package>
                                    <metadata>
                                        <id>{test.PackageIdentity.Id}</id>
                                        <version>{test.PackageIdentity.Version.ToNormalizedString()}</version>
                                        <title />
                                        <frameworkAssemblies>
                                            <frameworkAssembly assemblyName=""System.Runtime"" />
                                        </frameworkAssemblies>
                                    </metadata>
                                </package>");

                        })
                    .ReturnsAsync(() => new CopyFilesInPackageResponse(MessageResponseCode.Success, new[] { tempNuspecFilePath }));

                var dependencyInfo = await test.Resource.GetDependencyInfoAsync(
                    test.PackageIdentity.Id.ToUpper(),
                    test.PackageIdentity.Version,
                    test.SourceCacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.NotNull(dependencyInfo);
                Assert.Equal(test.PackageIdentity, dependencyInfo.PackageIdentity);
            }
        }

        [Fact]
        public async Task CopyNupkgToStreamAsync_ThrowsAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<NotSupportedException>(
                    () => test.Resource.CopyNupkgToStreamAsync(
                        test.PackageIdentity.Id,
                        test.PackageIdentity.Version,
                        Stream.Null,
                        test.SourceCacheContext,
                        NullLogger.Instance,
                        CancellationToken.None));
            }
        }

        [Fact]
        public async Task GetPackageDownloaderAsync_ThrowsForNullPackageIdentityAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Resource.GetPackageDownloaderAsync(
                        packageIdentity: null,
                        cacheContext: test.SourceCacheContext,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("packageIdentity", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetPackageDownloaderAsync_ThrowsForNullSourceCacheContextAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Resource.GetPackageDownloaderAsync(
                        test.PackageIdentity,
                        cacheContext: null,
                        logger: NullLogger.Instance,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("cacheContext", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetPackageDownloaderAsync_ThrowsForNullLoggerAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Resource.GetPackageDownloaderAsync(
                        test.PackageIdentity,
                        test.SourceCacheContext,
                        logger: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("logger", exception.ParamName);
            }
        }

        [Fact]
        public async Task GetPackageDownloaderAsync_ThrowsIfCancelledAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Resource.GetPackageDownloaderAsync(
                        test.PackageIdentity,
                        test.SourceCacheContext,
                        NullLogger.Instance,
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task GetPackageDownloaderAsync_ReturnsNullIfPackageNotFoundAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync(MessageResponseCode.NotFound))
            {
                var downloader = await test.Resource.GetPackageDownloaderAsync(
                    new PackageIdentity(id: "b", version: NuGetVersion.Parse("1.0.0")),
                    test.SourceCacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.IsType<PluginPackageDownloader>(downloader);
            }
        }

        [Fact]
        public async Task GetPackageDownloaderAsync_ReturnsPackageDownloaderIfPackageFoundAsync()
        {
            using (var test = await PluginFindPackageByIdResourceTest.CreateAsync())
            {
                var downloader = await test.Resource.GetPackageDownloaderAsync(
                    test.PackageIdentity,
                    test.SourceCacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.IsType<PluginPackageDownloader>(downloader);
            }
        }

        private sealed class PluginFindPackageByIdResourceTest : IDisposable
        {
            internal Mock<IConnection> Connection { get; }
            internal FileInfo Package { get; }
            internal PackageIdentity PackageIdentity { get; }
            internal PluginFindPackageByIdResource Resource { get; }
            internal SourceCacheContext SourceCacheContext { get; }
            internal TestDirectory TestDirectory { get; }

            private PluginFindPackageByIdResourceTest(
                PluginFindPackageByIdResource resource,
                FileInfo package,
                PackageIdentity packageIdentity,
                Mock<IConnection> connection,
                SourceCacheContext sourceCacheContext,
                TestDirectory testDirectory)
            {
                Resource = resource;
                Package = package;
                PackageIdentity = packageIdentity;
                Connection = connection;
                SourceCacheContext = sourceCacheContext;
                TestDirectory = testDirectory;
            }

            public void Dispose()
            {
                SourceCacheContext.Dispose();
                TestDirectory.Dispose();

                GC.SuppressFinalize(this);
            }

            internal static async Task<PluginFindPackageByIdResourceTest> CreateAsync(
                MessageResponseCode responseCode = MessageResponseCode.Error)
            {
                var packageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));
                var testDirectory = TestDirectory.Create();
                var packageSource = new PackageSource("http://unit.test");
                var package = await SimpleTestPackageUtility.CreateFullPackageAsync(
                    testDirectory.Path,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());
                var packageBytes = File.ReadAllBytes(package.FullName);
                var plugin = new Mock<IPlugin>();
                var dispatcher = new Mock<IMessageDispatcher>();
                var connection = new Mock<IConnection>();

                dispatcher.SetupGet(x => x.RequestHandlers)
                    .Returns(new RequestHandlers());

                connection.SetupGet(x => x.MessageDispatcher)
                    .Returns(dispatcher.Object);

                var versions = responseCode == MessageResponseCode.Success ? new[] { "1.0.0", "2.0.0", "3.0.0" } : null;
                var response = new GetPackageVersionsResponse(responseCode, versions);

                connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetPackageVersionsRequest, GetPackageVersionsResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetPackageVersions),
                        It.Is<GetPackageVersionsRequest>(
                            c => string.Equals(c.PackageId, packageIdentity.Id, StringComparison.OrdinalIgnoreCase) &&
                            c.PackageSourceRepository == packageSource.Source),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

                plugin.SetupGet(x => x.Connection)
                    .Returns(connection.Object);

                var utilities = new Mock<IPluginMulticlientUtilities>();
                var credentialService = new Mock<ICredentialService>();
                var credentialsProvider = new GetCredentialsRequestHandler(
                    plugin.Object,
                    proxy: null,
                    credentialService: credentialService.Object);

                var resource = new PluginFindPackageByIdResource(
                    plugin.Object,
                    utilities.Object,
                    packageSource);

                return new PluginFindPackageByIdResourceTest(
                    resource,
                    package,
                    packageIdentity,
                    connection,
                    new SourceCacheContext(),
                    testDirectory);
            }
        }
    }
}
