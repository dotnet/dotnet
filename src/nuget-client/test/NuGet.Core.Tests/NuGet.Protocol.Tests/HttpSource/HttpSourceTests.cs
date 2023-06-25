// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Server;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class HttpSourceTests
    {
        /// <summary>
        /// We need a lock whenever we set static properties on <see cref="HttpHandlerResourceV3"/>.
        /// </summary>
        private static readonly SemaphoreSlim HttpHandlerResourceV3Lock = new SemaphoreSlim(1);
        private const string FakeSource = "https://fake.server/users.json";

        [Fact]
        public async Task HttpSource_GetAsync_ThrottlesRequests()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);

                // Act
                await tc.HttpSource.GetAsync(
                    new HttpSourceCachedRequest(tc.Url, tc.CacheKey, tc.CacheContext),
                    result => TaskResult.True,
                    tc.Logger,
                    token: CancellationToken.None);

                // Assert
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Once);
                tc.Throttle.Verify(x => x.Release(), Times.Once);
            }
        }

        [Fact]
        public async Task HttpSource_GetJObjectAsync_ThrottlesRequests()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);

                tc.SetResponseSequence(new[]
                {
                    new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{}") },
                });

                // Act
                await tc.HttpSource.GetJObjectAsync(
                    new HttpSourceRequest(tc.Url, tc.Logger),
                    tc.Logger,
                    token: CancellationToken.None);

                // Assert
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Once);
                tc.Throttle.Verify(x => x.Release(), Times.Once);
            }
        }

        [Fact]
        public async Task HttpSource_ProcessStreamAsync_ThrottlesRequests()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);

                // Act
                await tc.HttpSource.ProcessStreamAsync(
                    new HttpSourceRequest(tc.Url, tc.Logger),
                    stream => TaskResult.True,
                    tc.Logger,
                    token: CancellationToken.None);

                // Assert
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Once);
                tc.Throttle.Verify(x => x.Release(), Times.Once);
            }
        }

        [Fact]
        public async Task HttpSource_ProcessResponseAsync_ThrottlesRequests()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);

                // Act
                await tc.HttpSource.ProcessResponseAsync(
                    new HttpSourceRequest(tc.Url, tc.Logger),
                    stream => TaskResult.True,
                    tc.Logger,
                    token: CancellationToken.None);

                // Assert
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Once);
                tc.Throttle.Verify(x => x.Release(), Times.Once);
            }
        }

        [Fact]
        public async Task HttpSource_GetNoContent()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);

                tc.SetResponseSequence(new[]
                {
                    new HttpResponseMessage(HttpStatusCode.NoContent),
                });

                // Act
                var actual = await tc.HttpSource.GetAsync(
                    new HttpSourceCachedRequest(
                        tc.Url,
                        tc.CacheKey,
                        tc.CacheContext)
                    {
                        EnsureValidContents = tc.GetStreamValidator(validCache: true, validNetwork: true)
                    },
                    result => Task.FromResult(result.Status),
                    tc.Logger,
                    token: CancellationToken.None);

                // Assert
                Assert.Equal(HttpSourceResultStatus.NoContent, actual);
            }
        }

        [Fact]
        public async Task HttpSource_HasDefaultResponseTimeout()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);
                HttpRetryHandlerRequest actualRequest = null;
                tc.SetResponseFactory(request =>
                {
                    actualRequest = request;
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    return Task.FromResult(response);
                });

                // Act
                await tc.HttpSource.ProcessResponseAsync(
                    new HttpSourceRequest(() => new HttpRequestMessage(HttpMethod.Get, tc.Url)),
                    response =>
                    {
                        return Task.FromResult(0);
                    },
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.NotNull(actualRequest);
                Assert.Equal(TimeSpan.FromSeconds(100), actualRequest.RequestTimeout);
            }
        }

        [Fact]
        public async Task HttpSource_AllowsCustomResponseTimeout()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);
                HttpRetryHandlerRequest actualRequest = null;
                tc.SetResponseFactory(request =>
                {
                    actualRequest = request;
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    return Task.FromResult(response);
                });
                var timeout = TimeSpan.FromMinutes(5);

                // Act
                await tc.HttpSource.ProcessResponseAsync(
                    new HttpSourceRequest(() => new HttpRequestMessage(HttpMethod.Get, tc.Url))
                    {
                        RequestTimeout = timeout,
                    },
                    response =>
                    {
                        return Task.FromResult(0);
                    },
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.NotNull(actualRequest);
                Assert.Equal(timeout, actualRequest.RequestTimeout);
            }
        }

        [Fact]
        public async Task HttpSource_TimesOutDownload()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var expectedMilliseconds = 250;
                var server = new TcpListenerServer
                {
                    Mode = TestServerMode.SlowResponseBody,
                    SleepDuration = TimeSpan.FromSeconds(10)
                };
                var packageSource = new PackageSource(FakeSource);
                var handler = new HttpClientHandler();
                var handlerResource = new HttpHandlerResourceV3(handler, handler);
                var httpSource = new HttpSource(
                    packageSource,
                    () => Task.FromResult((HttpHandlerResource)handlerResource),
                    NullThrottle.Instance)
                {
                    HttpCacheDirectory = td
                };
                var logger = new TestLogger();

                // Act & Assert
                var actual = await Assert.ThrowsAsync<IOException>(() =>
                    server.ExecuteAsync(uri => httpSource.GetJObjectAsync(
                        new HttpSourceRequest(uri, logger)
                        {
                            DownloadTimeout = TimeSpan.FromMilliseconds(expectedMilliseconds)
                        },
                        logger,
                        CancellationToken.None)));
                Assert.IsType<TimeoutException>(actual.InnerException);
                Assert.EndsWith(
                    $"timed out because no data was received for {expectedMilliseconds}ms.",
                    actual.Message);
            }
        }

        [Fact]
        public async Task HttpSource_DoesNotPopulateCacheWithDirectDownload()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td)
                {
                    DirectDownload = true
                };

                // Act
                var actual = await tc.HttpSource.GetAsync(
                    new HttpSourceCachedRequest(
                        tc.Url,
                        tc.CacheKey,
                        tc.CacheContext)
                    {
                        EnsureValidContents = tc.GetStreamValidator(validCache: true, validNetwork: true)
                    },
                    result => Task.FromResult(tc.ReadStream(result.Stream)),
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.False(tc.ValidatedCacheContent, "The cache content should not have been cached at all.");
                Assert.False(tc.ValidatedNetworkContent, "The network content should not been validated.");
                Assert.Equal(tc.NetworkContent, actual);
                Assert.Equal(0, Directory.EnumerateFileSystemEntries(td).Count());
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Once);
                tc.Throttle.Verify(x => x.Release(), Times.Once);
            }
        }

        [Fact]
        public async Task HttpSource_ReadsFromTheCacheWithDirectDownload()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td)
                {
                    DirectDownload = true
                };
                await tc.WriteToCacheAsync(tc.CacheKey, tc.CacheContent);

                // Act
                var actual = await tc.HttpSource.GetAsync(
                    new HttpSourceCachedRequest(
                        tc.Url,
                        tc.CacheKey,
                        tc.CacheContext)
                    {
                        EnsureValidContents = tc.GetStreamValidator(validCache: true, validNetwork: true)
                    },
                    result => Task.FromResult(tc.ReadStream(result.Stream)),
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.True(tc.ValidatedCacheContent, "The cache content have been validated.");
                Assert.False(tc.ValidatedNetworkContent, "The network content should not been validated.");
                Assert.Equal(tc.CacheContent, actual);
                Assert.Equal(1, Directory.EnumerateFileSystemEntries(td).Count());
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Never);
                tc.Throttle.Verify(x => x.Release(), Times.Never);
            }
        }

        [Fact]
        public async Task HttpSource_ValidatesValidNetworkContent()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);

                // Act
                var actual = await tc.HttpSource.GetAsync(
                    new HttpSourceCachedRequest(
                        tc.Url,
                        tc.CacheKey,
                        tc.CacheContext)
                    {
                        EnsureValidContents = tc.GetStreamValidator(validCache: true, validNetwork: true)
                    },
                    result => Task.FromResult(tc.ReadStream(result.Stream)),
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.False(tc.ValidatedCacheContent, "The cache content should not have been cached at all.");
                Assert.True(tc.ValidatedNetworkContent, "The network content should have been validated.");
                Assert.Equal(tc.NetworkContent, actual);
                Assert.Equal(1, Directory.EnumerateFileSystemEntries(td).Count());
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Once);
                tc.Throttle.Verify(x => x.Release(), Times.Once);
            }
        }

        [Fact]
        public async Task HttpSource_ValidatesInvalidNetworkContent()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => tc.HttpSource.GetAsync(
                    new HttpSourceCachedRequest(
                        tc.Url,
                        tc.CacheKey,
                        tc.CacheContext)
                    {
                        EnsureValidContents = tc.GetStreamValidator(validCache: true, validNetwork: false)
                    },
                    result => TaskResult.True,
                    tc.Logger,
                    CancellationToken.None));

                // Assert
                Assert.Same(tc.NetworkValidationException, exception);
                Assert.False(tc.ValidatedCacheContent, "The cache content should not have been cached at all.");
                Assert.True(tc.ValidatedNetworkContent, "The network content should have been validated.");
                Assert.Equal(1, Directory.EnumerateFileSystemEntries(td).Count());
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Once);
                tc.Throttle.Verify(x => x.Release(), Times.Once);
            }
        }

        [Fact]
        public async Task HttpSource_ValidatesValidCachedContent()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);
                await tc.WriteToCacheAsync(tc.CacheKey, tc.CacheContent);

                // Act
                var actual = await tc.HttpSource.GetAsync(
                    new HttpSourceCachedRequest(
                        tc.Url,
                        tc.CacheKey,
                        tc.CacheContext)
                    {
                        EnsureValidContents = tc.GetStreamValidator(validCache: true, validNetwork: true)
                    },
                    result => Task.FromResult(tc.ReadStream(result.Stream)),
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.True(tc.ValidatedCacheContent, "The cache content should have been validated.");
                Assert.False(tc.ValidatedNetworkContent, "The network should not have been queried at all.");
                Assert.Equal(tc.CacheContent, actual);
                Assert.Equal(1, Directory.EnumerateFileSystemEntries(td).Count());
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Never);
                tc.Throttle.Verify(x => x.Release(), Times.Never);
            }
        }

        [Fact]
        public async Task HttpSource_ValidatesInvalidCachedContent()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td);
                await tc.WriteToCacheAsync(tc.CacheKey, tc.CacheContent);

                // Act
                var actual = await tc.HttpSource.GetAsync(
                    new HttpSourceCachedRequest(
                        tc.Url,
                        tc.CacheKey,
                        tc.CacheContext)
                    {
                        EnsureValidContents = tc.GetStreamValidator(validCache: false, validNetwork: true)
                    },
                    result => Task.FromResult(tc.ReadStream(result.Stream)),
                    tc.Logger,
                    CancellationToken.None);

                // Assert
                Assert.True(tc.ValidatedCacheContent, "The cache content should have been validated.");
                Assert.True(tc.ValidatedNetworkContent, "The network content should have been validated.");
                Assert.Equal(tc.NetworkContent, actual);
                Assert.Equal(1, Directory.EnumerateFileSystemEntries(td).Count());
                tc.Throttle.Verify(x => x.WaitAsync(), Times.Once);
                tc.Throttle.Verify(x => x.Release(), Times.Once);
            }
        }

        [Fact]
        public async Task HttpSource_ThrottlesRequestsFromSettings()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var tc = new TestContext(td, SemaphoreSlimThrottle.CreateSemaphoreThrottle(initialCount: 4));

                tc.MessageHandler.WaitTimeInMs = 100;
                var tasks = new List<Task>();

                // Act

                for (var i = 0; i < 18; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        return tc.HttpSource.ProcessStreamAsync(
                            new HttpSourceRequest(tc.Url, tc.Logger),
                            stream => TaskResult.True,
                            tc.Logger,
                            token: CancellationToken.None);
                    }));
                }

                await Task.WhenAll(tasks);

                // Assert
                Assert.True(tc.MessageHandler.MaxConcurrencyRequest <= 4, $"MaxConcurrencyRequest is {tc.MessageHandler.MaxConcurrencyRequest}");
            }
        }

        private class TestContext
        {
            public TestContext(TestDirectory testDirectory, IThrottle throttle = null)
            {
                // data
                var source = FakeSource;
                CacheValidationException = new Exception();
                NetworkValidationException = new Exception();
                CacheContent = "cache";
                NetworkContent = "network";
                CacheKey = "CacheKey";
                Url = "https://fake.server/foo/bar/something.json";
                Credentials = new NetworkCredential("foo", "bar");
                Throttle = new Mock<IThrottle>();

                // dependencies
                var packageSource = new PackageSource(source);
                var networkResponses = new Dictionary<string, string> { { Url, NetworkContent } };
                MessageHandler = new TestMessageHandler(networkResponses, string.Empty);
                var handlerResource = new TestHttpHandler(MessageHandler);

                Logger = new TestLogger();
                TestDirectory = testDirectory;
                RetryHandlerMock = new Mock<IHttpRetryHandler>();

                // target
                HttpSource = new HttpSource(packageSource, () => Task.FromResult((HttpHandlerResource)handlerResource), throttle ?? Throttle.Object)
                {
                    HttpCacheDirectory = TestDirectory
                };
            }

            public Exception CacheValidationException { get; }

            public TestDirectory TestDirectory { get; }

            public string Url { get; }

            public string CacheKey { get; }

            public string NetworkContent { get; }

            public string CacheContent { get; }

            public TestLogger Logger { get; }

            public TestMessageHandler MessageHandler { get; }

            public HttpSourceCacheContext CacheContext
            {
                get
                {
                    using (var cacheContext = new SourceCacheContext())
                    {
                        cacheContext.DirectDownload = DirectDownload;
                        return HttpSourceCacheContext.Create(cacheContext, retryCount: 0);
                    }
                }
            }

            public HttpSource HttpSource { get; set; }

            public Exception NetworkValidationException { get; }

            public bool ValidatedNetworkContent { get; set; }

            public bool ValidatedCacheContent { get; set; }

            public Mock<IHttpRetryHandler> RetryHandlerMock { get; }

            public ICredentials Credentials { get; }

            public Mock<IThrottle> Throttle { get; private set; }

            public bool DirectDownload { get; set; }

            public async Task WriteToCacheAsync(string cacheKey, string content)
            {
                var response = new HttpResponseMessage
                {
                    Content = new StringContent(content, Encoding.UTF8)
                };

                using (response)
                using (var sourceCacheContext = new SourceCacheContext())
                {
                    var httpSourceCacheContext = HttpSourceCacheContext.Create(sourceCacheContext, retryCount: 0);

                    var result = HttpCacheUtility.InitializeHttpCacheResult(
                        TestDirectory,
                        new Uri(FakeSource),
                        cacheKey,
                        httpSourceCacheContext);

                    await HttpCacheUtility.CreateCacheFileAsync(
                        result,
                        response,
                        stream => { },
                        CancellationToken.None);

                    result.Stream.Dispose();
                }
            }

            public string ReadStream(Stream stream)
            {
                return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            }

            public void SetResponseSequence(HttpResponseMessage[] responses)
            {
                HttpSource.RetryHandler = RetryHandlerMock.Object;
                int index = 0;
                RetryHandlerMock
                    .Setup(x => x.SendAsync(
                        It.IsAny<HttpRetryHandlerRequest>(),
                        It.IsAny<string>(),
                        It.IsAny<ILogger>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(() => Task.FromResult(responses[index++ % responses.Length]));
            }

            public void SetResponseFactory(Func<HttpRetryHandlerRequest, Task<HttpResponseMessage>> responseFactory)
            {
                HttpSource.RetryHandler = RetryHandlerMock.Object;
                RetryHandlerMock
                    .Setup(x => x.SendAsync(
                        It.IsAny<HttpRetryHandlerRequest>(),
                        It.IsAny<string>(),
                        It.IsAny<ILogger>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<HttpRetryHandlerRequest, string, ILogger, CancellationToken>((r, _, __, ___) => responseFactory(r));
            }

            public Action<Stream> GetStreamValidator(bool validCache, bool validNetwork)
            {
                return stream =>
                {
                    var content = ReadStream(stream);

                    if (content == CacheContent)
                    {
                        ValidatedCacheContent = true;
                        if (!validCache)
                        {
                            throw CacheValidationException;
                        }
                    }

                    if (content == NetworkContent)
                    {
                        ValidatedNetworkContent = true;
                        if (!validNetwork)
                        {
                            throw NetworkValidationException;
                        }
                    }
                };
            }
        }
    }
}
