// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class HttpRetryHandlerTests
    {
        private const int MaxTries = 5;
        private const string TestUrl = "https://test.local/test.json";
        private static readonly TimeSpan SmallTimeout = TimeSpan.FromMilliseconds(50);
        private static readonly TimeSpan LargeTimeout = TimeSpan.FromSeconds(5);

        // Add an additional header and verify that it was send in the request.
        [Fact]
        public async Task HttpRetryHandler_AddHeaders()
        {
            // Arrange
            var retryHandler = new HttpRetryHandler();
            var testHandler = new TestHandler();
            using (var httpClient = new HttpClient(testHandler))
            {
                var request = new HttpRetryHandlerRequest(
                    httpClient,
                    () => new HttpRequestMessage(HttpMethod.Get, TestUrl));

                var id = Guid.NewGuid().ToString();
                request.AddHeaders.Add(new KeyValuePair<string, IEnumerable<string>>(ProtocolConstants.SessionId, new[] { id }));
                var log = new TestLogger();

                // Act
                using (var actualResponse = await retryHandler.SendAsync(request, log, CancellationToken.None))
                {
                    // Assert
                    testHandler.LastRequest.Headers.GetValues(ProtocolConstants.SessionId)
                        .FirstOrDefault()
                        .Should()
                        .Be(id);
                }
            }
        }

        [Fact]
        public async Task HttpRetryHandler_ReturnsContentHeaders()
        {
            // Arrange
            Func<HttpRequestMessage, HttpResponseMessage> handler = requestMessage =>
            {
                var stringContent = new StringContent("Plain text document.", Encoding.UTF8, "text/plain");
                stringContent.Headers.TryAddWithoutValidation("X-Content-ID", "49f47c14-c21f-4c1d-9e13-4f5fcf5f8013");
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = stringContent,
                };
                response.Headers.TryAddWithoutValidation("X-Message-Header", "This isn't on the content.");
                return response;
            };

            var retryHandler = new HttpRetryHandler();
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(
                httpClient,
                () => new HttpRequestMessage(HttpMethod.Get, TestUrl));
            var log = new TestLogger();

            // Act
            var actualResponse = await retryHandler.SendAsync(request, log, CancellationToken.None);

            // Assert
            var actualHeaders = actualResponse
                .Content
                .Headers
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);
            Assert.Equal(
                new List<string> { "Content-Type", "X-Content-ID" },
                actualHeaders.Keys.OrderBy(x => x).ToList());
            Assert.Equal(actualHeaders["Content-Type"], new[] { "text/plain; charset=utf-8" });
            Assert.Equal(actualHeaders["X-Content-ID"], new[] { "49f47c14-c21f-4c1d-9e13-4f5fcf5f8013" });
        }

        [Fact]
        public async Task HttpRetryHandler_DifferentRequestInstanceEachTime()
        {
            // Arrange
            TimeSpan retryDelay = TimeSpan.Zero;

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = MaxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });

            var requests = new HashSet<HttpRequestMessage>();
            Func<HttpRequestMessage, HttpResponseMessage> handler = requestMessage =>
            {
                requests.Add(requestMessage);
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            };

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = MaxTries,
                RequestTimeout = Timeout.InfiniteTimeSpan,
                RetryDelay = retryDelay
            };
            var log = new TestLogger();

            // Act
            using (await retryHandler.SendAsync(request, log, CancellationToken.None))
            {
            }

            // Assert
            Assert.Equal(MaxTries, requests.Count);
        }

        [Fact]
        public async Task HttpRetryHandler_CancelsRequestAfterTimeout()
        {
            // Arrange
            TimeSpan retryDelay = TimeSpan.Zero;

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = MaxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });
            var requestToken = CancellationToken.None;
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler = async (requestMessage, token) =>
            {
                requestToken = token;
                await Task.Delay(LargeTimeout, token);
                return new HttpResponseMessage(HttpStatusCode.OK);
            };

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = 1,
                RequestTimeout = SmallTimeout,
                RetryDelay = retryDelay
            };

            // Act
            Func<Task> actionAsync = () => retryHandler.SendAsync(
                request,
                new TestLogger(),
                CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<TimeoutException>(actionAsync);
            Assert.True(requestToken.CanBeCanceled);
            Assert.True(requestToken.IsCancellationRequested);
        }

        [Fact]
        public async Task HttpRetryHandler_ThrowsTimeoutExceptionForTimeout()
        {
            // Arrange
            TimeSpan retryDelay = TimeSpan.Zero;

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = MaxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler = async (requestMessage, token) =>
            {
                await Task.Delay(LargeTimeout);
                return new HttpResponseMessage(HttpStatusCode.OK);
            };

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = 1,
                RequestTimeout = TimeSpan.Zero,
                RetryDelay = retryDelay
            };

            // Act
            Func<Task> actionAsync = () => retryHandler.SendAsync(
                request,
                new TestLogger(),
                CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<TimeoutException>(actionAsync);
        }

        [Fact]
        public async Task HttpRetryHandler_MultipleTriesTimed()
        {
            // Arrange
            TimeSpan retryDelay = SmallTimeout;

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = MaxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });
            Func<HttpRequestMessage, HttpResponseMessage> handler = requestMessage =>
            {
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            };

            var minTime = GetRetryMinTime(MaxTries, SmallTimeout);

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = MaxTries,
                RequestTimeout = Timeout.InfiniteTimeSpan,
                RetryDelay = retryDelay
            };
            var log = new TestLogger();

            // Act
            var timer = new Stopwatch();
            timer.Start();

            using (await retryHandler.SendAsync(request, log, CancellationToken.None))
            {
            }

            timer.Stop();

            // Assert
            Assert.True(
                timer.Elapsed >= minTime,
                $"Expected this to take at least: {minTime} But it finished in: {timer.Elapsed}");
        }

        [Fact]
        public async Task HttpRetryHandler_MultipleTriesNoSuccess()
        {
            // Arrange
            TimeSpan retryDelay = TimeSpan.Zero;

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = MaxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });
            var hits = 0;

            Func<HttpRequestMessage, HttpResponseMessage> handler = requestMessage =>
            {
                hits++;

                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            };

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = MaxTries,
                RequestTimeout = Timeout.InfiniteTimeSpan,
                RetryDelay = retryDelay
            };
            var log = new TestLogger();

            // Act
            using (await retryHandler.SendAsync(request, log, CancellationToken.None))
            {
            }

            // Assert
            Assert.Equal(MaxTries, hits);
        }

        [Fact]
        public async Task HttpRetryHandler_MultipleSuccessFirstVerifySingleHit()
        {
            // Arrange
            TimeSpan retryDelay = TimeSpan.Zero;

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = MaxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });
            var hits = 0;

            Func<HttpRequestMessage, HttpResponseMessage> handler = requestMessage =>
            {
                hits++;
                return new HttpResponseMessage(HttpStatusCode.OK);
            };

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = MaxTries,
                RequestTimeout = Timeout.InfiniteTimeSpan,
                RetryDelay = retryDelay
            };
            var log = new TestLogger();

            // Act
            using (var response = await retryHandler.SendAsync(request, log, CancellationToken.None))
            {
                // Assert
                Assert.Equal(1, hits);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task HttpRetryHandler_404VerifySingleHit()
        {
            // Arrange
            TimeSpan retryDelay = TimeSpan.Zero;

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = MaxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });
            var hits = 0;

            Func<HttpRequestMessage, HttpResponseMessage> handler = requestMessage =>
            {
                hits++;
                return new HttpResponseMessage(HttpStatusCode.OK);
            };

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = MaxTries,
                RequestTimeout = Timeout.InfiniteTimeSpan,
                RetryDelay = retryDelay
            };
            var log = new TestLogger();

            // Act
            using (var response = await retryHandler.SendAsync(request, log, CancellationToken.None))
            {
                // Assert
                Assert.Equal(1, hits);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/8392")]
        public async Task HttpRetryHandler_TimesOutDownload()
        {
            // Arrange
            var hits = 0;
            var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes("foobar"));
            var expectedMilliseconds = 50;
            Func<HttpRequestMessage, HttpResponseMessage> handler = requestMessage =>
            {
                hits++;
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(new SlowStream(memoryStream)
                    {
                        DelayPerByte = TimeSpan.FromSeconds(1)
                    })
                };
            };

            var retryHandler = new HttpRetryHandler();
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                DownloadTimeout = TimeSpan.FromMilliseconds(expectedMilliseconds)
            };
            var destinationStream = new MemoryStream();
            var log = new TestLogger();

            // Act
            using (var response = await retryHandler.SendAsync(request, log, CancellationToken.None))
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var actual = await Assert.ThrowsAsync<IOException>(() => stream.CopyToAsync(destinationStream));

                // Assert
                Assert.Equal(1, hits);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.IsType<TimeoutException>(actual.InnerException);
                Assert.EndsWith(
                    $"timed out because no data was received for {expectedMilliseconds}ms.",
                    actual.Message);
            }
        }

        [Fact]
        public async Task HttpRetryHandler_MultipleTriesUntilSuccess()
        {
            // Arrange
            TimeSpan retryDelay = TimeSpan.Zero;

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = MaxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });
            var tries = 0;
            var sent503 = false;

            Func<HttpRequestMessage, HttpResponseMessage> handler = requestMessage =>
            {
                tries++;

                // Return 503 for the first 2 tries
                if (tries > 2)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    sent503 = true;
                    return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                }
            };

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = MaxTries,
                RequestTimeout = Timeout.InfiniteTimeSpan,
                RetryDelay = retryDelay
            };
            var log = new TestLogger();

            // Act
            using (var response = await retryHandler.SendAsync(request, log, CancellationToken.None))
            {
                // Assert
                Assert.True(sent503);
                Assert.Equal(3, tries);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task HttpRetryHandler_EnhancedRetryAllowsSettingMoreRetries()
        {
            // Arrange
            var tries = 0;
            var sent503 = false;

            Func<HttpRequestMessage, HttpResponseMessage> handler = requestMessage =>
            {
                tries++;

                // Return 503 for the first 2 tries
                if (tries > 10)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    sent503 = true;
                    return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                }
            };

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                new Dictionary<string, string>()
                {
                    [EnhancedHttpRetryHelper.IsEnabledEnvironmentVariableName] = bool.TrueString,
                    [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = "11",
                    [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = "3"
                });

            EnhancedHttpRetryHelper helper = new EnhancedHttpRetryHelper(testEnvironmentVariableReader);
            Assert.Equal(helper.IsEnabled, true);
            // Enhanced retry mode causes a random 0-199 ms jitter so we can't time it in this test
            // but we can make sure the setting got through
            Assert.Equal(helper.DelayInMilliseconds, 3);
            Assert.Equal(helper.RetryCount, 11);

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = helper.RetryCount,
                RequestTimeout = Timeout.InfiniteTimeSpan,
                // HttpRetryHandler will override with values from NUGET_ENHANCED_NETWORK_RETRY_DELAY_MILLISECONDS
                // so set this to a value that will cause test timeout if the correct value is not honored.
                RetryDelay = TimeSpan.FromMilliseconds(int.MaxValue) // = about 24 days
            };
            var log = new TestLogger();

            // Act
            using (var response = await retryHandler.SendAsync(request, log, CancellationToken.None))
            {
                // Assert
                Assert.True(sent503);
                Assert.Equal(11, tries);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Theory]
        [InlineData(null, EnhancedHttpRetryHelper.DefaultEnabled)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("0", EnhancedHttpRetryHelper.DefaultEnabled)]
        [InlineData("something", EnhancedHttpRetryHelper.DefaultEnabled)]
        public void HttpRetryHandler_EnhancedRetryOnByDefault(string value, bool expectedValue)
        {
            // Arrange
            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                new Dictionary<string, string>()
                {
                    [EnhancedHttpRetryHelper.IsEnabledEnvironmentVariableName] = value,
                    [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = null,
                    [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = null
                });

            // Act
            EnhancedHttpRetryHelper helper = new EnhancedHttpRetryHelper(testEnvironmentVariableReader);

            Assert.Equal(helper.IsEnabled, expectedValue);
            Assert.Equal(helper.RetryCount, EnhancedHttpRetryHelper.DefaultRetryCount);
            Assert.Equal(helper.DelayInMilliseconds, EnhancedHttpRetryHelper.DefaultDelayMilliseconds);
        }

        private static TimeSpan GetRetryMinTime(int tries, TimeSpan retryDelay)
        {
            return TimeSpan.FromTicks((tries - 1) * retryDelay.Ticks);
        }

        private class TestHandler : DelegatingHandler
        {
            public HttpRequestMessage LastRequest { get; private set; }

            public HttpResponseMessage Response { get; set; } = new HttpResponseMessage(HttpStatusCode.Accepted);

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                return Task.FromResult(Response);
            }
        }
    }
}
