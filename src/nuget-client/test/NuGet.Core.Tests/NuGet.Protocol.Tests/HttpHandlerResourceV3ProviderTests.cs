// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Server;
using NuGet.Test.Utility;
using Org.BouncyCastle.Asn1.X509;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class HttpHandlerResourceV3ProviderTests
    {
        private readonly string _testPackageSourceURL = "https://contoso.test/v3/index.json";

#if IS_DESKTOP
        [PlatformFact(Platform.Windows)]
        public async Task DefaultMaxHttpRequestsPerSourceIsForwardedToV3HttpClientHandler_SuccessAsync()
        {
            // Arrange
            var packageSource = new PackageSource(_testPackageSourceURL);
            var sourceRepository = new SourceRepository(packageSource, new List<INuGetResourceProvider>() { new HttpSourceResourceProvider(), new HttpHandlerResourceV3Provider() });

            // HttpSourceResourceProvider updates PackageSource.MaxHttpRequestsPerSource value for .NET Framework code paths
            // HttpSource constructor accepts a delegate that creates HttpHandlerResource and it stores the delegate in a private variable.
            // Hence used discard to ignore the return value and fetched HttpHandlerResource from the source repository to verify behavior. 
            _ = await sourceRepository.GetResourceAsync<HttpSourceResource>(CancellationToken.None);

            // Act
            HttpHandlerResource httpHandlerResource = await sourceRepository.GetResourceAsync<HttpHandlerResource>(CancellationToken.None);

            // Assert
            Assert.NotNull(httpHandlerResource);
            Assert.Equal(64, httpHandlerResource.ClientHandler.MaxConnectionsPerServer);
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData(128)]
        [InlineData(256)]
        public async Task PackageSourceMaxHttpRequestsPerSourceIsForwardedToV3HttpClientHandler_SuccessAsync(int maxHttpRequestsPerSource)
        {
            // Arrange
            var packageSource = new PackageSource(_testPackageSourceURL) { MaxHttpRequestsPerSource = maxHttpRequestsPerSource };
            var sourceRepository = new SourceRepository(packageSource, new List<INuGetResourceProvider>() { new HttpSourceResourceProvider(), new HttpHandlerResourceV3Provider() });

            // HttpSourceResourceProvider updates PackageSource.MaxHttpRequestsPerSource value for .NET Framework code paths
            // HttpSource constructor accepts a delegate that creates HttpHandlerResource and it stores the delegate in a private variable.
            // Hence used discard to ignore the return value and fetched HttpHandlerResource from the source repository to verify behavior.
            _ = await sourceRepository.GetResourceAsync<HttpSourceResource>(CancellationToken.None);

            // Act            
            HttpHandlerResource httpHandlerResource = await sourceRepository.GetResourceAsync<HttpHandlerResource>(CancellationToken.None);

            // Assert
            Assert.NotNull(httpHandlerResource);
            Assert.Equal(maxHttpRequestsPerSource, httpHandlerResource.ClientHandler.MaxConnectionsPerServer);
        }
#elif IS_CORECLR

        [Theory]
        [InlineData(64)]
        [InlineData(128)]
        [InlineData(2)]
        public async Task PackageSourceMaxHttpRequestsPerSourceIsNotForwardedToV3HttpClientHandler_SuccessAsync(int maxHttpRequestsPerSource)
        {
            // Arrange
            var packageSource = new PackageSource(_testPackageSourceURL) { MaxHttpRequestsPerSource = maxHttpRequestsPerSource };
            var sourceRepository = new SourceRepository(packageSource, new[] { new HttpHandlerResourceV3Provider() });

            // HttpSourceResourceProvider updates PackageSource.MaxHttpRequestsPerSource value for .NET Framework code paths
            // HttpSource constructor accepts a delegate that creates HttpHandlerResource and it stores the delegate in a private variable.
            // Hence used discard to ignore the return value and fetched HttpHandlerResource from the source repository to verify behavior.
            _ = await sourceRepository.GetResourceAsync<HttpSourceResource>(CancellationToken.None);

            // Act            
            HttpHandlerResource httpHandlerResource = await sourceRepository.GetResourceAsync<HttpHandlerResource>(CancellationToken.None);

            // Assert
            Assert.NotNull(httpHandlerResource);
            Assert.NotEqual(maxHttpRequestsPerSource, httpHandlerResource.ClientHandler.MaxConnectionsPerServer);
        }
#endif

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task TryCreate_HandlerContainsServerWarningMiddleware(bool hasProxy)
        {
            // Arrange
            Mock<IProxyCache> proxyCache = new();
            IWebProxy webProxy = hasProxy ? new Mock<IWebProxy>().Object : null;
            proxyCache.Setup(pc => pc.GetProxy(It.IsAny<Uri>())).Returns(webProxy);

            PackageSource packageSource = new("https://nuget.test/v2/api", "source");
            SourceRepository sourceRepository = new(packageSource, Array.Empty<INuGetResourceProvider>());

            HttpHandlerResourceV3Provider target = new(proxyCache.Object);

            // Act
            var result = await target.TryCreate(sourceRepository, CancellationToken.None);

            // Assert
            result.Item1.Should().BeTrue();
            HttpHandlerResourceV3 resource = (HttpHandlerResourceV3)result.Item2;
            resource.Should().NotBeNull();
            IEnumerable<DelegatingHandler> delegatingHandlers = GetDelegatingHandlers(resource.MessageHandler);
            delegatingHandlers.Any(h => h is ServerWarningLogHandler).Should().BeTrue();

            static IEnumerable<DelegatingHandler> GetDelegatingHandlers(HttpMessageHandler handler)
            {
                DelegatingHandler delegatingHandler = handler as DelegatingHandler;
                while (delegatingHandler != null)
                {
                    yield return delegatingHandler;
                    delegatingHandler = delegatingHandler.InnerHandler as DelegatingHandler;
                }
            }
        }

        [Fact]
        public async Task TryCreate_WhenCertificateValidationIsDisabled_HttpClientHandlerServerCertificateCustomValidationCallbackShouldNotBeNull()
        {
            // Arrange
            Mock<IProxyCache> proxyCache = new();
            proxyCache.Setup(pc => pc.GetProxy(It.IsAny<Uri>())).Returns((IWebProxy)null);
            PackageSource packageSource = new(_testPackageSourceURL, "source")
            {
                DisableTLSCertificateValidation = true
            };
            SourceRepository sourceRepository = new(packageSource, Array.Empty<INuGetResourceProvider>());
            HttpHandlerResourceV3Provider target = new(proxyCache.Object);

            // Act
            var result = await target.TryCreate(sourceRepository, CancellationToken.None);
            HttpHandlerResourceV3 resource = (HttpHandlerResourceV3)result.Item2;
            HttpClientHandler clientHandler = resource.ClientHandler;

            // Assert
            clientHandler.ServerCertificateCustomValidationCallback.Should().NotBeNull();
        }

        [Fact]
        public async Task Invoke_WhenCertificateValidationIsDisabled_HttpClientHandlerServerCertificateCustomValidationCallbackReturnsTrue()
        {
            // Arrange
            Mock<IProxyCache> proxyCache = new();
            proxyCache.Setup(pc => pc.GetProxy(It.IsAny<Uri>())).Returns((IWebProxy)null);
            PackageSource packageSource = new(_testPackageSourceURL, "source")
            {
                DisableTLSCertificateValidation = true
            };
            SourceRepository sourceRepository = new(packageSource, Array.Empty<INuGetResourceProvider>());
            HttpHandlerResourceV3Provider target = new(proxyCache.Object);
            var result = await target.TryCreate(sourceRepository, CancellationToken.None);
            HttpHandlerResourceV3 resource = (HttpHandlerResourceV3)result.Item2;
            HttpClientHandler clientHandler = resource.ClientHandler;

            // Act
            var callbackResult = clientHandler.ServerCertificateCustomValidationCallback.Invoke(null, null, null, SslPolicyErrors.RemoteCertificateChainErrors
                & SslPolicyErrors.RemoteCertificateNameMismatch
                & SslPolicyErrors.RemoteCertificateNotAvailable
                & SslPolicyErrors.None);

            // Assert
            callbackResult.Should().BeTrue();
        }

        [Fact]
        public async Task TryCreate_WhenCertificateValidationIsEnabled_HttpClientHandlerServerCertificateCustomValidationCallbackShouldBeNull()
        {
            // Arrange
            Mock<IProxyCache> proxyCache = new();
            proxyCache.Setup(pc => pc.GetProxy(It.IsAny<Uri>())).Returns((IWebProxy)null);
            PackageSource packageSource = new(_testPackageSourceURL, "source")
            {
                DisableTLSCertificateValidation = false
            };
            SourceRepository sourceRepository = new(packageSource, Array.Empty<INuGetResourceProvider>());
            HttpHandlerResourceV3Provider target = new(proxyCache.Object);

            // Act
            var result = await target.TryCreate(sourceRepository, CancellationToken.None);
            HttpHandlerResourceV3 resource = (HttpHandlerResourceV3)result.Item2;
            HttpClientHandler clientHandler = resource.ClientHandler;

            // Assert
            clientHandler.ServerCertificateCustomValidationCallback.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_InvalidCertificateWithValidationEnabled_ClientHandlerThrowsAnException()
        {
            // Arrange
            TcpListenerServer server = new()
            {
                Mode = TestServerMode.InvalidTLSCertificate
            };

            await server.ExecuteAsync(async uri =>
            {
                Mock<IProxyCache> proxyCache = new();
                proxyCache.Setup(pc => pc.GetProxy(It.IsAny<Uri>())).Returns((IWebProxy)null);
                PackageSource packageSource = new(uri, "source");
                SourceRepository sourceRepository = new(packageSource, Array.Empty<INuGetResourceProvider>());
                HttpHandlerResourceV3Provider target = new(proxyCache.Object);
                var result = await target.TryCreate(sourceRepository, CancellationToken.None);
                HttpHandlerResourceV3 resource = (HttpHandlerResourceV3)result.Item2;
                HttpClientHandler clientHandler = resource.ClientHandler;
                var client = new HttpClient(clientHandler);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetAsync(uri));
                return 0;
            });
        }

        [Fact]
        public async Task GetAsync_InvalidCertificateWithValidationDisabled_ClientHandlerDoesNotThrowAnException()
        {
            // Arrange
            TcpListenerServer server = new()
            {
                Mode = TestServerMode.InvalidTLSCertificate
            };

            await server.ExecuteAsync(async uri =>
            {
                Mock<IProxyCache> proxyCache = new();
                proxyCache.Setup(pc => pc.GetProxy(It.IsAny<Uri>())).Returns((IWebProxy)null);
                PackageSource packageSource = new(uri, "source")
                {
                    DisableTLSCertificateValidation = true
                };
                SourceRepository sourceRepository = new(packageSource, Array.Empty<INuGetResourceProvider>());
                HttpHandlerResourceV3Provider target = new(proxyCache.Object);
                var result = await target.TryCreate(sourceRepository, CancellationToken.None);
                HttpHandlerResourceV3 resource = (HttpHandlerResourceV3)result.Item2;
                HttpClientHandler clientHandler = resource.ClientHandler;
                var client = new HttpClient(clientHandler);

                // Act
                var response = await client.GetAsync(uri);

                // Assert
                Assert.True(response.IsSuccessStatusCode);
                return 0;
            });
        }
    }
}
