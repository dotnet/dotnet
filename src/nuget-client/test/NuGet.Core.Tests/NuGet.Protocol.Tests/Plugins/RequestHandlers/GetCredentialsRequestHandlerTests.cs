// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetCredentialsRequestHandlerTests
    {
        private readonly PackageSource _packageSource = new PackageSource("https://unit.test");

        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new GetCredentialsRequestHandler(
                    plugin: null,
                    proxy: Mock.Of<IWebProxy>(),
                    credentialService: Mock.Of<ICredentialService>()));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void CancellationToken_IsNone()
        {
            using (var provider = CreateDefaultRequestHandler())
            {
                Assert.Equal(CancellationToken.None, provider.CancellationToken);
            }
        }

        [Fact]
        public void Dispose_DisposesDisposables()
        {
            var plugin = new Mock<IPlugin>(MockBehavior.Strict);

            plugin.Setup(x => x.Dispose());

            var provider = new GetCredentialsRequestHandler(
                plugin.Object,
                Mock.Of<IWebProxy>(),
                Mock.Of<ICredentialService>());

            provider.Dispose();

            plugin.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            var plugin = new Mock<IPlugin>(MockBehavior.Strict);

            plugin.Setup(x => x.Dispose());

            var provider = new GetCredentialsRequestHandler(
                plugin.Object,
                Mock.Of<IWebProxy>(),
                Mock.Of<ICredentialService>());

            provider.Dispose();
            provider.Dispose();

            plugin.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void AddOrUpdateSourceRepository_ThrowsForNullSourceRepository()
        {
            using (var provider = CreateDefaultRequestHandler())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => provider.AddOrUpdateSourceRepository(sourceRepository: null));

                Assert.Equal("sourceRepository", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullConnection()
        {
            using (var provider = CreateDefaultRequestHandler())
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
            using (var provider = CreateDefaultRequestHandler())
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
            using (var provider = CreateDefaultRequestHandler())
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
            using (var provider = CreateDefaultRequestHandler())
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
            using (var provider = CreateDefaultRequestHandler())
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest("\\unit\test", HttpStatusCode.Unauthorized));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.NotFound
                            && r.Username == null && r.Password == null),
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
            var credentialService = new Mock<ICredentialService>(MockBehavior.Strict);

            credentialService.Setup(x => x.GetCredentialsAsync(
                    It.IsNotNull<Uri>(),
                    It.IsNotNull<IWebProxy>(),
                    It.Is<CredentialRequestType>(c => c == CredentialRequestType.Unauthorized),
                    It.IsNotNull<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((ICredentials)null);

            var plugin = CreateMockPlugin();

            using (var provider = new GetCredentialsRequestHandler(
                plugin,
                Mock.Of<IWebProxy>(),
                credentialService.Object))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest("https://unit.test", HttpStatusCode.Unauthorized));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.NotFound
                            && r.Username == null && r.Password == null),
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
        public async Task HandleResponseAsync_ReturnsPackageSourceCredentialsFromPackageSourceIfValid()
        {
            _packageSource.Credentials = new PackageSourceCredential(
                _packageSource.Source,
                username: "a",
                passwordText: "b",
                isPasswordClearText: true,
                validAuthenticationTypesText: null);

            using (var provider = CreateDefaultRequestHandler())
            {
                var sourceRepository = new SourceRepository(_packageSource, Enumerable.Empty<INuGetResourceProvider>());

                provider.AddOrUpdateSourceRepository(sourceRepository);

                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest(_packageSource.Source, HttpStatusCode.Unauthorized));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.Success
                            && r.Username == "a" && r.Password == "b"),
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
        public async Task HandleResponseAsync_ReturnsPackageSourceCredentialsFromCredentialServiceIfFound()
        {
            var networkCredential = new NetworkCredential(userName: "a", password: "b");
            var credentialService = new Mock<ICredentialService>();
            var credentials = new Mock<ICredentials>();
            var proxy = Mock.Of<IWebProxy>();

            credentials.Setup(x => x.GetCredential(
                    It.Is<Uri>(u => u == _packageSource.SourceUri),
                    It.Is<string>(a => a == null)))
                .Returns(networkCredential);

            credentialService.Setup(x => x.GetCredentialsAsync(
                    It.Is<Uri>(uri => uri == _packageSource.SourceUri),
                    It.Is<IWebProxy>(p => p == proxy),
                    It.Is<CredentialRequestType>(t => t == CredentialRequestType.Unauthorized),
                    It.IsNotNull<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(credentials.Object));

            var plugin = CreateMockPlugin();

            using (var provider = new GetCredentialsRequestHandler(plugin, proxy, credentialService.Object))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest(_packageSource.Source, HttpStatusCode.Unauthorized));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.Success
                            && r.Username == "a" && r.Password == "b"),
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
        public async Task HandleResponseAsync_ReturnsNullPackageSourceCredentialsIfPackageSourceCredentialsAreInvalidAndCredentialServiceIsNull()
        {
            var plugin = CreateMockPlugin();

            using (var provider = new GetCredentialsRequestHandler(
                plugin,
                Mock.Of<IWebProxy>(),
                credentialService: null))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest(_packageSource.Source, HttpStatusCode.Unauthorized));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.NotFound
                            && r.Username == null && r.Password == null),
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
        public async Task HandleResponseAsync_ReturnsNullPackageSourceCredentialsIfNoCredentials()
        {
            var credentialService = new Mock<ICredentialService>();
            var credentials = new Mock<ICredentials>();
            var proxy = Mock.Of<IWebProxy>();

            credentials.Setup(x => x.GetCredential(
                    It.Is<Uri>(u => u == _packageSource.SourceUri),
                    It.Is<string>(a => a == null)))
                .Returns<NetworkCredential>(null);

            credentialService.Setup(x => x.GetCredentialsAsync(
                    It.Is<Uri>(uri => uri == _packageSource.SourceUri),
                    It.Is<IWebProxy>(p => p == proxy),
                    It.Is<CredentialRequestType>(t => t == CredentialRequestType.Unauthorized),
                    It.IsNotNull<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(credentials.Object));

            var plugin = CreateMockPlugin();

            using (var provider = new GetCredentialsRequestHandler(plugin, proxy, credentialService.Object))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest(_packageSource.Source, HttpStatusCode.Unauthorized));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.NotFound
                            && r.Username == null && r.Password == null),
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
        public async Task HandleResponseAsync_ReturnsProxyCredentialsFromCredentialServiceIfFound()
        {
            var proxyUri = new Uri("https://proxy.unit.test");
            var networkCredential = new NetworkCredential(userName: "a", password: "b");
            var credentialService = new Mock<ICredentialService>();
            var credentials = new Mock<ICredentials>();
            var proxy = new Mock<IWebProxy>();

            proxy.Setup(x => x.GetProxy(It.IsAny<Uri>()))
                .Returns(proxyUri);

            credentials.Setup(x => x.GetCredential(
                    It.Is<Uri>(u => u == proxyUri),
                    It.Is<string>(a => a == "Basic")))
                .Returns(networkCredential);

            credentialService.Setup(x => x.GetCredentialsAsync(
                    It.Is<Uri>(u => u == _packageSource.SourceUri),
                    It.Is<IWebProxy>(p => p == proxy.Object),
                    It.Is<CredentialRequestType>(t => t == CredentialRequestType.Proxy),
                    It.IsNotNull<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(credentials.Object));

            var plugin = CreateMockPlugin();

            using (var provider = new GetCredentialsRequestHandler(plugin, proxy.Object, credentialService.Object))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest(_packageSource.Source, HttpStatusCode.ProxyAuthenticationRequired));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.Success
                            && r.Username == "a" && r.Password == "b"),
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
        public async Task HandleResponseAsync_ReturnsNullProxyCredentialsIfCredentialServiceIsNull()
        {
            var plugin = CreateMockPlugin();

            using (var provider = new GetCredentialsRequestHandler(
                plugin,
                Mock.Of<IWebProxy>(),
                credentialService: null))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest(_packageSource.Source, HttpStatusCode.ProxyAuthenticationRequired));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.NotFound
                            && r.Username == null && r.Password == null),
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
        public async Task HandleResponseAsync_ReturnsNullProxyCredentialsIfNoCredentials()
        {
            var credentialService = new Mock<ICredentialService>();
            var credentials = new Mock<ICredentials>();
            var proxy = Mock.Of<IWebProxy>();

            credentials.Setup(x => x.GetCredential(
                    It.Is<Uri>(u => u == _packageSource.SourceUri),
                    It.Is<string>(a => a == null)))
                .Returns<NetworkCredential>(null);

            credentialService.Setup(x => x.GetCredentialsAsync(
                    It.Is<Uri>(uri => uri == _packageSource.SourceUri),
                    It.Is<IWebProxy>(p => p == proxy),
                    It.Is<CredentialRequestType>(t => t == CredentialRequestType.Proxy),
                    It.IsNotNull<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(credentials.Object));

            var plugin = CreateMockPlugin();

            using (var provider = new GetCredentialsRequestHandler(plugin, proxy, credentialService.Object))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest(_packageSource.Source, HttpStatusCode.ProxyAuthenticationRequired));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.NotFound
                            && r.Username == null && r.Password == null),
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
        public async Task HandleResponseAsync_ReturnsNullProxyCredentialsIfNoProxy()
        {
            var plugin = CreateMockPlugin();

            using (var provider = new GetCredentialsRequestHandler(
                plugin,
                proxy: null,
                credentialService: Mock.Of<ICredentialService>()))
            {
                var request = CreateRequest(
                    MessageType.Request,
                    new GetCredentialsRequest(_packageSource.Source, HttpStatusCode.ProxyAuthenticationRequired));
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.Is<Message>(r => r == request),
                        It.Is<GetCredentialsResponse>(r => r.ResponseCode == MessageResponseCode.NotFound
                            && r.Username == null && r.Password == null),
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

        private GetCredentialsRequestHandler CreateDefaultRequestHandler()
        {
            var plugin = CreateMockPlugin();

            return new GetCredentialsRequestHandler(plugin, Mock.Of<IWebProxy>(), Mock.Of<ICredentialService>());
        }

        private static IPlugin CreateMockPlugin()
        {
            var plugin = new Mock<IPlugin>();

            plugin.SetupGet(x => x.Connection)
                .Returns(Mock.Of<IConnection>());

            return plugin.Object;
        }

        private static Message CreateRequest(MessageType type, GetCredentialsRequest payload = null)
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
