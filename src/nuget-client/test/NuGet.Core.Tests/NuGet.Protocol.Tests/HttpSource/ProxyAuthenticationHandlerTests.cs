// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Configuration;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class ProxyAuthenticationHandlerTests
    {
        private static readonly Uri ProxyAddress = new Uri("http://127.0.0.1:8888/");

        [Fact]
        public async Task SendAsync_WithUnauthenticatedProxy_PassesThru()
        {
            var defaultClientHandler = GetDefaultClientHandler();

            var service = Mock.Of<ICredentialService>();
            var handler = new ProxyAuthenticationHandler(defaultClientHandler, service, ProxyCache.Instance)
            {
                InnerHandler = GetLambdaHandler(HttpStatusCode.OK)
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_WithMissingCredentials_Returns407()
        {
            var defaultClientHandler = GetDefaultClientHandler();

            var service = Mock.Of<ICredentialService>();
            var handler = new ProxyAuthenticationHandler(defaultClientHandler, service, ProxyCache.Instance)
            {
                InnerHandler = GetLambdaHandler(HttpStatusCode.ProxyAuthenticationRequired)
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ProxyAuthenticationRequired, response.StatusCode);

            Mock.Get(service)
                .Verify(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once());
        }

        [Fact]
        public async Task SendAsync_WithAcquiredCredentials_RetriesRequest()
        {
            var defaultClientHandler = GetDefaultClientHandler();

            var service = Mock.Of<ICredentialService>();
            Mock.Get(service)
                .Setup(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));

            var handler = new ProxyAuthenticationHandler(defaultClientHandler, service, ProxyCache.Instance);

            var responses = new Queue<HttpStatusCode>(
                new[] { HttpStatusCode.ProxyAuthenticationRequired, HttpStatusCode.OK });
            var innerHandler = new LambdaMessageHandler(
                _ => new HttpResponseMessage(responses.Dequeue()));
            handler.InnerHandler = innerHandler;

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Mock.Get(service)
                .Verify(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task SendAsync_WhenCancelledDuringAcquiringCredentials_Throws()
        {
            // Arrange
            var defaultClientHandler = GetDefaultClientHandler();

            var cts = new CancellationTokenSource();

            var service = Mock.Of<ICredentialService>();
            Mock.Get(service)
                .Setup(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException())
                .Callback(() => cts.Cancel());

            var handler = new ProxyAuthenticationHandler(defaultClientHandler, service, ProxyCache.Instance);

            var responses = new Queue<HttpStatusCode>(
                new[] { HttpStatusCode.ProxyAuthenticationRequired, HttpStatusCode.OK });
            var innerHandler = new LambdaMessageHandler(
                _ => new HttpResponseMessage(responses.Dequeue()));
            handler.InnerHandler = innerHandler;

            // Act
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => SendAsync(handler, cancellationToken: cts.Token));

            Mock.Get(service)
                .Verify(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task SendAsync_WithWrongCredentials_StopsRetryingAfter3Times()
        {
            var defaultClientHandler = GetDefaultClientHandler();

            var service = Mock.Of<ICredentialService>();
            Mock.Get(service)
                .Setup(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));

            var handler = new ProxyAuthenticationHandler(defaultClientHandler, service, ProxyCache.Instance);

            int retryCount = 0;
            var innerHandler = new LambdaMessageHandler(
                _ => { retryCount++; return new HttpResponseMessage(HttpStatusCode.ProxyAuthenticationRequired); });
            handler.InnerHandler = innerHandler;

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ProxyAuthenticationRequired, response.StatusCode);

            Assert.Equal(ProxyAuthenticationHandler.MaxAuthRetries, retryCount);

            Mock.Get(service)
                .Verify(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Exactly(2));
        }

        [Fact]
        public async Task SendAsync_WhenCredentialServiceThrows_Returns407()
        {
            var defaultClientHandler = GetDefaultClientHandler();

            var service = Mock.Of<ICredentialService>();
            Mock.Get(service)
                .Setup(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Throws(new InvalidOperationException("Credential service failed acquiring credentials"));

            var handler = new ProxyAuthenticationHandler(defaultClientHandler, service, ProxyCache.Instance)
            {
                InnerHandler = GetLambdaHandler(HttpStatusCode.ProxyAuthenticationRequired)
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ProxyAuthenticationRequired, response.StatusCode);

            Mock.Get(service)
                .Verify(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once());
        }

        [Fact]
        public async Task SendAsync_RetryWithClonedRequest()
        {
            var defaultClientHandler = GetDefaultClientHandler();

            var service = Mock.Of<ICredentialService>();
            Mock.Get(service)
                .Setup(
                    x => x.GetCredentialsAsync(
                        ProxyAddress,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Proxy,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));

            var requests = 0;
            var handler = new ProxyAuthenticationHandler(defaultClientHandler, service, ProxyCache.Instance)
            {
                InnerHandler = new LambdaMessageHandler(
                    request =>
                    {
                        Assert.Null(request.Headers.Authorization);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "TEST");
                        requests++;
                        return new HttpResponseMessage(HttpStatusCode.ProxyAuthenticationRequired);
                    })
            };

            var response = await SendAsync(handler);

            Assert.True(requests > 1, "No retries");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ProxyAuthenticationRequired, response.StatusCode);
        }

        private static HttpClientHandler GetDefaultClientHandler()
        {
            var proxy = new TestProxy(ProxyAddress);
            return new HttpClientHandler { Proxy = proxy };
        }

        private static LambdaMessageHandler GetLambdaHandler(HttpStatusCode statusCode)
        {
            return new LambdaMessageHandler(
                _ => new HttpResponseMessage(statusCode));
        }

        private static async Task<HttpResponseMessage> SendAsync(
            HttpMessageHandler handler,
            HttpRequestMessage request = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var client = new HttpClient(handler))
            {
                return await client.SendAsync(request ?? new HttpRequestMessage(HttpMethod.Get, "http://foo"), cancellationToken);
            }
        }
    }
}
