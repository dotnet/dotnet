// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetServiceIndexRequestHandlerTests
    {
        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new GetServiceIndexRequestHandler(plugin: null));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void CancellationToken_IsNone()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                Assert.Equal(CancellationToken.None, provider.CancellationToken);
            }
        }

        [Fact]
        public void Dispose_DisposesDisposables()
        {
            var plugin = new Mock<IPlugin>(MockBehavior.Strict);

            plugin.Setup(x => x.Dispose());

            var provider = new GetServiceIndexRequestHandler(plugin.Object);

            provider.Dispose();

            plugin.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            var plugin = new Mock<IPlugin>(MockBehavior.Strict);

            plugin.Setup(x => x.Dispose());

            var provider = new GetServiceIndexRequestHandler(plugin.Object);

            provider.Dispose();
            provider.Dispose();

            plugin.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void AddOrUpdateSourceRepository_ThrowsForNullSourceRepository()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => provider.AddOrUpdateSourceRepository(sourceRepository: null));

                Assert.Equal("sourceRepository", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullConnection()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                var request = CreateRequest(MessageType.Request);

                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => provider.HandleResponseAsync(
                        connection: null,
                        request: request,
                        responseHandler: Mock.Of<IResponseHandler>(),
                        cancellationToken: CancellationToken.None));

                Assert.Equal("connection", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullRequest()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => provider.HandleResponseAsync(
                        Mock.Of<IConnection>(),
                        request: null,
                        responseHandler: Mock.Of<IResponseHandler>(),
                        cancellationToken: CancellationToken.None));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullResponseHandler()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                var request = CreateRequest(MessageType.Request);

                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => provider.HandleResponseAsync(
                        Mock.Of<IConnection>(),
                        request,
                        responseHandler: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("responseHandler", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsIfCancelled()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                var request = CreateRequest(MessageType.Request);

                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => provider.HandleResponseAsync(
                        Mock.Of<IConnection>(),
                        request,
                        Mock.Of<IResponseHandler>(),
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ReturnsNotFoundForNonHttpPackageSource()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetServiceIndexRequest("\\unit\test"));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetServiceIndexResponse>(r => r.ResponseCode == MessageResponseCode.NotFound),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await provider.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    request,
                    responseHandler.Object,
                    CancellationToken.None);

                responseHandler.Verify();
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ReturnsNotFoundIfPackageSourceNotFound()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetServiceIndexRequest("https://unit.test"));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetServiceIndexResponse>(r => r.ResponseCode == MessageResponseCode.NotFound),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await provider.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    request,
                    responseHandler.Object,
                    CancellationToken.None);

                responseHandler.Verify();
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ReturnsNotFoundSourceRepositoryReturnsNullServiceIndex()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                var packageSource = new PackageSource("https://unit.test");
                var sourceRepository = new SourceRepository(packageSource, Enumerable.Empty<INuGetResourceProvider>());

                provider.AddOrUpdateSourceRepository(sourceRepository);

                var request = CreateRequest(
                    MessageType.Request,
                    new GetServiceIndexRequest(packageSource.Source));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetServiceIndexResponse>(r => r.ResponseCode == MessageResponseCode.NotFound),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await provider.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    request,
                    responseHandler.Object,
                    CancellationToken.None);

                responseHandler.Verify();
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ReturnsSuccessIfServiceIndexIsFound()
        {
            using (var provider = new GetServiceIndexRequestHandler(Mock.Of<IPlugin>()))
            {
                var packageSource = new PackageSource("https://unit.test");
                var serviceIndex = JObject.Parse("{}");
                var serviceIndexResource = new ServiceIndexResourceV3(serviceIndex, DateTime.UtcNow);
                var serviceIndexResourceProvider = new Mock<INuGetResourceProvider>();

                serviceIndexResourceProvider.SetupGet(x => x.ResourceType)
                    .Returns(typeof(ServiceIndexResourceV3));

                serviceIndexResourceProvider.SetupGet(x => x.Name)
                    .Returns(nameof(ServiceIndexResourceV3Provider));

                serviceIndexResourceProvider.Setup(x => x.TryCreate(
                        It.IsNotNull<SourceRepository>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Tuple<bool, INuGetResource>(true, serviceIndexResource));

                var sourceRepository = new SourceRepository(
                    packageSource,
                    new INuGetResourceProvider[] { serviceIndexResourceProvider.Object });

                provider.AddOrUpdateSourceRepository(sourceRepository);

                var request = CreateRequest(
                    MessageType.Request,
                    new GetServiceIndexRequest(packageSource.Source));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetServiceIndexResponse>(r => r.ResponseCode == MessageResponseCode.Success
                            && r.ServiceIndex.ToString(Formatting.None) == serviceIndex.ToString(Formatting.None)),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await provider.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    request,
                    responseHandler.Object,
                    CancellationToken.None);

                responseHandler.Verify();
            }
        }

        private static Message CreateRequest(MessageType type, GetServiceIndexRequest payload = null)
        {
            if (payload == null)
            {
                return new Message(
                    requestId: "a",
                    type: type,
                    method: MessageMethod.GetCredentials,
                    payload: null);
            }

            return MessageUtilities.Create(
                requestId: "a",
                type: MessageType.Request,
                method: MessageMethod.GetCredentials,
                payload: payload);
        }
    }
}
