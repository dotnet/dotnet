// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginFindPackageByIdResourceProviderTests
    {
        private readonly PluginFindPackageByIdResourceProvider _provider;

        public PluginFindPackageByIdResourceProviderTests()
        {
            _provider = new PluginFindPackageByIdResourceProvider();

            HttpHandlerResourceV3.CredentialService = new Lazy<ICredentialService>(() => Mock.Of<ICredentialService>());
        }

        [Fact]
        public void Before_IsBeforeHttpFileSystemBasedFindPackageByIdResourceProvider()
        {
            Assert.Equal(new[] { nameof(HttpFileSystemBasedFindPackageByIdResourceProvider) }, _provider.Before);
        }

        [Fact]
        public void After_IsEmpty()
        {
            Assert.Empty(_provider.After);
        }

        [Fact]
        public void Name_IsPluginFindPackageByIdResourceProvider()
        {
            Assert.Equal(nameof(PluginFindPackageByIdResourceProvider), _provider.Name);
        }

        [Fact]
        public void ResourceType_IsFindPackageByIdResource()
        {
            Assert.Equal(typeof(FindPackageByIdResource), _provider.ResourceType);
        }

        [Fact]
        public async Task TryCreate_ThrowsForNullSource()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _provider.TryCreate(source: null, cancellationToken: CancellationToken.None));

            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public async Task TryCreate_ThrowsIfCancelled()
        {
            var sourceRepository = CreateSourceRepository(
                createPluginResource: true,
                createServiceIndexResourceV3: true,
                createHttpHandlerResource: true);

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _provider.TryCreate(sourceRepository, new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task TryCreate_ReturnsFalseIfNoPluginResource()
        {
            var sourceRepository = CreateSourceRepository(
               createPluginResource: false,
               createServiceIndexResourceV3: true,
               createHttpHandlerResource: true);

            var result = await _provider.TryCreate(sourceRepository, CancellationToken.None);

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task TryCreate_ReturnsFalseIfNoServiceIndexResourceV3()
        {
            var sourceRepository = CreateSourceRepository(
                createPluginResource: true,
                createServiceIndexResourceV3: false,
                createHttpHandlerResource: true);

            var result = await _provider.TryCreate(sourceRepository, CancellationToken.None);

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task TryCreate_ReturnsFalseIfNoHttpHandlerResource()
        {
            var sourceRepository = CreateSourceRepository(
                createPluginResource: true,
                createServiceIndexResourceV3: true,
                createHttpHandlerResource: false);

            var result = await _provider.TryCreate(sourceRepository, CancellationToken.None);

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task TryCreate_ReturnsPluginFindPackageByIdResource()
        {
            var sourceRepository = CreateSourceRepository(
                createPluginResource: true,
                createServiceIndexResourceV3: true,
                createHttpHandlerResource: true);

            var result = await _provider.TryCreate(sourceRepository, CancellationToken.None);

            Assert.True(result.Item1);
            Assert.IsType<PluginFindPackageByIdResource>(result.Item2);
        }

        private static SourceRepository CreateSourceRepository(
            bool createPluginResource,
            bool createServiceIndexResourceV3,
            bool createHttpHandlerResource)
        {
            var packageSource = new PackageSource(source: "https://unit.test");
            var providers = new INuGetResourceProvider[]
            {
                CreatePluginResourceProvider(createPluginResource),
                CreateServiceIndexResourceV3Provider(createServiceIndexResourceV3),
                CreateMockHttpHandlerResource(createHttpHandlerResource)
            };

            return new SourceRepository(packageSource, providers);
        }

        private static HttpHandlerResourceV3Provider CreateMockHttpHandlerResource(bool createResource)
        {
            var provider = new Mock<HttpHandlerResourceV3Provider>();

            provider.Setup(x => x.Name)
                .Returns(nameof(HttpHandlerResourceV3Provider));
            provider.Setup(x => x.ResourceType)
                .Returns(typeof(HttpHandlerResource));

            HttpHandlerResource resource = null;

            if (createResource)
            {
                resource = Mock.Of<HttpHandlerResource>();
            }

            var tryCreateResult = new Tuple<bool, INuGetResource>(resource != null, resource);

            provider.Setup(x => x.TryCreate(It.IsAny<SourceRepository>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(tryCreateResult));

            return provider.Object;
        }

        private static PluginResourceProvider CreatePluginResourceProvider(bool createResource)
        {
            var provider = new Mock<PluginResourceProvider>();

            provider.Setup(x => x.Name)
                .Returns(nameof(PluginResourceProvider));
            provider.Setup(x => x.ResourceType)
                .Returns(typeof(PluginResource));

            PluginResource pluginResource = null;

            if (createResource)
            {
                var plugin = new Mock<IPlugin>();
                var utilities = new Mock<IPluginMulticlientUtilities>();
                var connection = new Mock<IConnection>();
                var dispatcher = new Mock<IMessageDispatcher>();

                dispatcher.SetupGet(x => x.RequestHandlers)
                    .Returns(new RequestHandlers());

                connection.SetupGet(x => x.MessageDispatcher)
                    .Returns(dispatcher.Object);

                plugin.Setup(x => x.Connection)
                    .Returns(connection.Object);

                var creationResult = new PluginCreationResult(
                    plugin.Object,
                    utilities.Object,
                    new List<OperationClaim>() { OperationClaim.DownloadPackage });
                var packageSource = new PackageSource(source: "https://unit.test");

                pluginResource = new PluginResource(
                    new[] { creationResult },
                    packageSource,
                    Mock.Of<ICredentialService>());
            }

            var tryCreateResult = new Tuple<bool, INuGetResource>(pluginResource != null, pluginResource);

            provider.Setup(x => x.TryCreate(It.IsAny<SourceRepository>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(tryCreateResult));

            return provider.Object;
        }

        private static ServiceIndexResourceV3Provider CreateServiceIndexResourceV3Provider(bool createResource)
        {
            var provider = new Mock<ServiceIndexResourceV3Provider>();

            provider.Setup(x => x.Name)
                .Returns(nameof(ServiceIndexResourceV3Provider));
            provider.Setup(x => x.ResourceType)
                .Returns(typeof(ServiceIndexResourceV3));

            var serviceIndexResource = createResource
                ? new ServiceIndexResourceV3(JObject.Parse("{}"), DateTime.UtcNow) : null;
            var tryCreateResult = new Tuple<bool, INuGetResource>(serviceIndexResource != null, serviceIndexResource);

            provider.Setup(x => x.TryCreate(It.IsAny<SourceRepository>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(tryCreateResult));

            return provider.Object;
        }
    }
}
