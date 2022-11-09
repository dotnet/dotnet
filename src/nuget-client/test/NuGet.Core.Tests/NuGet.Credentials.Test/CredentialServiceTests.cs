// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using Xunit;
using WebProxy = System.Net.WebProxy;

namespace NuGet.Credentials.Test
{
    public class CredentialServiceTests
    {
        private static int _lockTestConcurrencyCount = 0;

        private readonly Mock<ICredentialProvider> _mockProvider;

        public CredentialServiceTests()
        {
            _mockProvider = new Mock<ICredentialProvider>();
            _mockProvider
                .Setup(x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CredentialResponse(CredentialStatus.ProviderNotApplicable)));
            _mockProvider.Setup(x => x.Id).Returns("1");
        }

        [Fact]
        public void Constructor_ThrowsForNullProviders()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new CredentialService(providers: null, nonInteractive: true, handlesDefaultCredentials: true));

            Assert.Equal("providers", exception.ParamName);
        }

        [Fact]
        public async Task GetCredentialsAsync_ThrowsForNullUri()
        {
            var service = new CredentialService(new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(Enumerable.Empty<ICredentialProvider>())), nonInteractive: true, handlesDefaultCredentials: true);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => service.GetCredentialsAsync(
                    uri: null,
                    proxy: Mock.Of<IWebProxy>(),
                    type: CredentialRequestType.Unauthorized,
                    message: "a",
                    cancellationToken: CancellationToken.None));

            Assert.Equal("uri", exception.ParamName);
        }

        [Fact]
        public async Task GetCredentialsAsync_PassesAllParametersToProviders()
        {
            IEnumerable<ICredentialProvider> providers = new[] { _mockProvider.Object };
            // Arrange
            var service = new CredentialService(
                new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)),
                nonInteractive: true,
                handlesDefaultCredentials: true);
            var webProxy = new WebProxy();
            var uri = new Uri("http://uri");

            // Act
            await service.GetCredentialsAsync(
                uri,
                webProxy,
                CredentialRequestType.Proxy,
                message: "A",
                cancellationToken: CancellationToken.None);

            // Assert
            _mockProvider.Verify(x => x.GetAsync(
                uri,
                webProxy,
                /*type*/ CredentialRequestType.Proxy,
                /*message*/ "A",
                /*isRetry*/ It.IsAny<bool>(),
                /*nonInteractive*/ true,
                CancellationToken.None));
        }

        [Fact]
        public async Task GetCredentialsAsync_FirstCallHasRetryFalse()
        {
            // Arrange
            IEnumerable<ICredentialProvider> providers = new[] { _mockProvider.Object };

            var service = new CredentialService(
                new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)),
                nonInteractive: false,
                handlesDefaultCredentials: true);
            _mockProvider.Setup(
                x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CredentialResponse(new NetworkCredential())));
            var uri1 = new Uri("http://uri1");
            var uri2 = new Uri("http://uri2");

            // Act
            var result1 = await service.GetCredentialsAsync(
                uri1,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: "A",
                cancellationToken: CancellationToken.None);
            var result2 = await service.GetCredentialsAsync(
                uri2,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: "B",
                cancellationToken: CancellationToken.None);

            // Assert
            _mockProvider.Verify(x => x.GetAsync(
                uri1,
                /*webProxy*/ null,
                /*type*/ CredentialRequestType.Unauthorized,
                /*message*/ "A",
                /*isRetry*/ false,
                /*nonInteractive*/ false,
                CancellationToken.None));

            _mockProvider.Verify(x => x.GetAsync(
                uri2,
                /*webProxy*/ null,
                /*type*/ CredentialRequestType.Unauthorized,
                /*message*/ "B",
                /*isRetry*/ false,
                /*nonInteractive*/ false,
                CancellationToken.None));
        }

        [Fact]
        public async Task GetCredentialsAsync_SecondCallHasRetryTrue()
        {
            // Arrange
            IEnumerable<ICredentialProvider> providers = new[] { _mockProvider.Object };
            var service = new CredentialService(
                new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)),
                nonInteractive: false,
                handlesDefaultCredentials: true);
            _mockProvider.Setup(
                x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CredentialResponse(new NetworkCredential())));
            var uri1 = new Uri("http://uri1");
            var webProxy = new WebProxy();

            // Act
            await service.GetCredentialsAsync(
                uri1,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);
            await service.GetCredentialsAsync(
                uri1,
                webProxy,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);

            // Assert
            _mockProvider.Verify(x => x.GetAsync(
                uri1,
                null,
                /*type*/ CredentialRequestType.Unauthorized,
                /*message*/ null,
                /*isRetry*/ false,
                /*nonInteractive*/ false,
                CancellationToken.None));
            _mockProvider.Verify(x => x.GetAsync(
                uri1,
                webProxy,
                /*type*/ CredentialRequestType.Unauthorized,
                /*message*/ null,
                /*isRetry*/ true,
                /*nonInteractive*/ false,
                CancellationToken.None));
        }

        [Fact]
        public void GetCredentialsAsync_SingleThreadedAccessToEachProvider()
        {
            // Arrange
            IEnumerable<ICredentialProvider> providers = new[] { _mockProvider.Object };
            var service = new CredentialService(
                new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)),
                nonInteractive: true,
                handlesDefaultCredentials: true);
            var webProxy = new WebProxy();
            var uri = new Uri("http://uri");
            _mockProvider
                .Setup(x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(() =>
                {
                    _lockTestConcurrencyCount++;
                    Assert.Equal(1, _lockTestConcurrencyCount);
                    _lockTestConcurrencyCount--;
                    return Task.FromResult(new CredentialResponse(new NetworkCredential()));
                });
            var tasks = new Task[10];

            // Act
            for (var x = 0; x < 10; x++)
            {
                tasks[x] = service.GetCredentialsAsync(
                    uri,
                    webProxy,
                    type: CredentialRequestType.Unauthorized,
                    message: null,
                    cancellationToken: CancellationToken.None);
            }
            Task.WaitAll(tasks);

            // Assert
            // in this case, assert is done during provider access
        }

        [Fact]
        public async Task GetCredentialsAsync_WhenUriHasSameAuthority_ThenReturnsCachedCredential()
        {
            // Arrange
            IEnumerable<ICredentialProvider> providers = new[] { _mockProvider.Object };
            var service = new CredentialService(
                new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)),
                nonInteractive: false,
                handlesDefaultCredentials: true);
            _mockProvider
                .Setup(x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new CredentialResponse(new NetworkCredential())));
            var uri1 = new Uri("http://host/some/path");
            var uri2 = new Uri("http://host/some2/path2");

            // Act
            var result1 = await service.GetCredentialsAsync(
                uri1,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);
            var result2 = await service.GetCredentialsAsync(
                uri2,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.Same(result1, result2);
            _mockProvider.Verify(
                x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCredentialsAsync_NullResponsesAreCached()
        {
            // Arrange
            IEnumerable<ICredentialProvider> providers = new[] { _mockProvider.Object };
            var service = new CredentialService(
                new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)),
                nonInteractive: false,
                handlesDefaultCredentials: true);
            _mockProvider
                .Setup(x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new CredentialResponse(CredentialStatus.ProviderNotApplicable)));
            var uri1 = new Uri("http://host/some/path");
            var uri2 = new Uri("http://host/some2/path2");

            // Act
            var result1 = await service.GetCredentialsAsync(
                uri1,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);
            var result2 = await service.GetCredentialsAsync(
                uri2,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.Null(result1);
            Assert.Null(result2);
            _mockProvider.Verify(
                x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCredentialsAsync_TriesAllProviders_EvenWhenSameType()
        {
            // Arrange
            var mockProvider1 = new Mock<ICredentialProvider>();
            mockProvider1
                .Setup(x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CredentialResponse(CredentialStatus.ProviderNotApplicable)));
            mockProvider1.Setup(x => x.Id).Returns("1");
            var mockProvider2 = new Mock<ICredentialProvider>();
            mockProvider2
                .Setup(x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(
                    Task.FromResult(new CredentialResponse(CredentialStatus.ProviderNotApplicable)));
            mockProvider2.Setup(x => x.Id).Returns("2");
            IEnumerable<ICredentialProvider> providers = new[] { mockProvider1.Object, mockProvider2.Object };

            var service = new CredentialService(
                new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)),
                nonInteractive: false,
                handlesDefaultCredentials: true);
            var uri1 = new Uri("http://host/some/path");

            // Act
            var result1 = await service.GetCredentialsAsync(
                uri1,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.Null(result1);
            mockProvider1.Verify(
                x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            mockProvider2.Verify(
                x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCredentialsAsync_WhenRetry_ThenDoesNotReturnCachedCredential()
        {
            // Arrange
            IEnumerable<ICredentialProvider> providers = new[] { _mockProvider.Object };
            var service = new CredentialService(
                new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)),
                nonInteractive: false,
                handlesDefaultCredentials: true);
            _mockProvider
                .Setup(x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new CredentialResponse(new NetworkCredential())));
            var uri1 = new Uri("http://uri1");

            // Act
            var result1 = await service.GetCredentialsAsync(
                uri1,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);
            var result2 = await service.GetCredentialsAsync(
                uri1,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.NotSame(result1, result2);
            _mockProvider.Verify(
                x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()),
                Times.Exactly(2));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TryGetLastKnownGoodCredentialsFromCache_ThrowsForNullUri(bool isProxy)
        {
            var service = new CredentialService(new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(Enumerable.Empty<ICredentialProvider>())), nonInteractive: true, handlesDefaultCredentials: true);
            ICredentials credentials;

            var exception = Assert.Throws<ArgumentNullException>(
                () => service.TryGetLastKnownGoodCredentialsFromCache(
                    uri: null,
                    isProxy: isProxy,
                    credentials: out credentials));

            Assert.Equal("uri", exception.ParamName);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TryGetLastKnownGoodCredentialsFromCache_ReturnsFalseForCacheMiss(bool isProxy)
        {
            var service = new CredentialService(new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(Enumerable.Empty<ICredentialProvider>())), nonInteractive: true, handlesDefaultCredentials: true);
            ICredentials credentials;

            var wasCacheHit = service.TryGetLastKnownGoodCredentialsFromCache(
                new Uri("https://unit.test"),
                isProxy,
                out credentials);

            Assert.False(wasCacheHit);
        }

        [Theory]
        [InlineData(CredentialStatus.ProviderNotApplicable, true)]
        [InlineData(CredentialStatus.ProviderNotApplicable, false)]
        [InlineData(CredentialStatus.UserCanceled, true)]
        [InlineData(CredentialStatus.UserCanceled, false)]
        public async Task TryGetLastKnownGoodCredentialsFromCache_DoesNotReturnUnsuccessfulCredentials(
            CredentialStatus credentialStatus,
            bool isProxy)
        {
            var provider = new Mock<ICredentialProvider>(MockBehavior.Strict);

            provider.Setup(x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new CredentialResponse(credentialStatus)));
            provider.Setup(x => x.Id).Returns("a");
            IEnumerable<ICredentialProvider> providers = new[] { provider.Object };
            var service = new CredentialService(new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)), nonInteractive: false, handlesDefaultCredentials: true);
            var uri = new Uri("https://unit.test");
            var type = isProxy ? CredentialRequestType.Proxy : CredentialRequestType.Unauthorized;
            var credentials = await service.GetCredentialsAsync(
                uri,
                proxy: null,
                type: type,
                message: null,
                cancellationToken: CancellationToken.None);

            ICredentials cachedCredentials;
            var wasCacheHit = service.TryGetLastKnownGoodCredentialsFromCache(uri, isProxy, out cachedCredentials);

            Assert.False(wasCacheHit);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task TryGetLastKnownGoodCredentialsFromCache_ReturnsCredentialsForCacheHit(bool isProxy)
        {
            var networkCredential = new NetworkCredential();
            var provider = new Mock<ICredentialProvider>(MockBehavior.Strict);

            provider.Setup(x => x.GetAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<IWebProxy>(),
                    It.IsAny<CredentialRequestType>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new CredentialResponse(networkCredential)));
            provider.Setup(x => x.Id).Returns("a");
            IEnumerable<ICredentialProvider> providers = new[] { provider.Object };

            var service = new CredentialService(new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult(providers)), nonInteractive: false, handlesDefaultCredentials: true);
            var uri = new Uri("https://unit.test");
            var type = isProxy ? CredentialRequestType.Proxy : CredentialRequestType.Unauthorized;
            var credentials = await service.GetCredentialsAsync(
                uri,
                proxy: null,
                type: type,
                message: null,
                cancellationToken: CancellationToken.None);

            ICredentials cachedCredentials;
            var wasCacheHit = service.TryGetLastKnownGoodCredentialsFromCache(uri, isProxy, out cachedCredentials);

            Assert.True(wasCacheHit);
            Assert.Same(networkCredential, credentials);
            Assert.Same(credentials, cachedCredentials);
        }

        [Fact]
        public async Task GetCredentialProvidersExecutedOnlyOnce()
        {
            var counter = new CallCounter();
            var service = new CredentialService(new AsyncLazy<IEnumerable<ICredentialProvider>>(() => counter.GetProviders()), nonInteractive: true, handlesDefaultCredentials: true);

            var uri1 = new Uri("http://uri1");

            // Act
            var result1 = await service.GetCredentialsAsync(
                uri1,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);
            var result2 = await service.GetCredentialsAsync(
                uri1,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                cancellationToken: CancellationToken.None);

            Assert.Equal(1, counter.CallCount);
        }


        private class CallCounter
        {
            public int CallCount { get; private set; }

            public Task<IEnumerable<ICredentialProvider>> GetProviders()
            {
                CallCount++;
                return Task.FromResult(Enumerable.Empty<ICredentialProvider>());
            }
        }
    }
}
