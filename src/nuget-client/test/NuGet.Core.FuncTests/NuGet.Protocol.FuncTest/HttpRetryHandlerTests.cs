// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Test.Server;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Core.FuncTest
{
    public class HttpRetryHandlerTests
    {
        private const string TestUrl = "https://local.test/test.json";

        [Fact]
        public async Task HttpRetryHandler_ReturnsContentHeaders()
        {
            // Arrange
            var retryHandler = new HttpRetryHandler();
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRetryHandlerRequest(
                    httpClient,
                    () => new HttpRequestMessage(HttpMethod.Get, "https://api.nuget.org/v3/index.json"));
                var log = new TestLogger();

                // Act
                using (var actualResponse = await retryHandler.SendAsync(request, log, CancellationToken.None))
                {
                    // Assert
                    Assert.NotEmpty(actualResponse.Content.Headers);
                }
            }
        }

        [Fact]
        public async Task HttpRetryHandler_AppliesTimeoutToRequestsIndividually()
        {
            // Arrange

            // 20 requests that take 250ms each for a total of 5 seconds (plus noise).
            var requestDuration = TimeSpan.FromMilliseconds(250);
            var maxTries = 20;
            var retryDelay = TimeSpan.Zero;

            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = maxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });

            // Make the request timeout longer than each request duration but less than the total
            // duration of all attempts.
            var requestTimeout = TimeSpan.FromMilliseconds(4000);

            var hits = 0;
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler = async (requestMessage, token) =>
            {
                hits++;
                await Task.Delay(requestDuration);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            };

            var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
            var testHandler = new HttpRetryTestHandler(handler);
            var httpClient = new HttpClient(testHandler);
            var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = maxTries,
                RequestTimeout = requestTimeout,
                RetryDelay = retryDelay
            };
            var log = new TestLogger();

            // Act
            using (await retryHandler.SendAsync(request, log, CancellationToken.None))
            {
            }

            // Assert
            Assert.Equal(maxTries, hits);
        }

        [Fact]
        public async Task HttpRetryHandler_HandlesFailureToConnect()
        {
            // Arrange
            var server = new NotListeningServer { Mode = TestServerMode.ConnectFailure };

            // Act & Assert
            var exception = await ThrowsException<HttpRequestException>(server);
#if IS_CORECLR
            Assert.NotNull(exception.InnerException);
            if (!RuntimeEnvironmentHelper.IsWindows)
            {
                Assert.Equal("Connection refused", exception.InnerException.Message);
            }
            else
            {
                Assert.Equal("No connection could be made because the target machine actively refused it.", exception.InnerException.Message);
            }
#else
            var innerException = Assert.IsType<WebException>(exception.InnerException);
            Assert.Equal(WebExceptionStatus.ConnectFailure, innerException.Status);
#endif
        }

        [Fact]
        public async Task HttpRetryHandler_HandlesInvalidProtocol()
        {
            // Arrange
            var server = new TcpListenerServer { Mode = TestServerMode.ServerProtocolViolation };

            // Act & Assert
            var exception = await ThrowsException<HttpRequestException>(server);
#if IS_CORECLR
            Assert.Null(exception.InnerException);
#if NETCOREAPP3_1_OR_GREATER
            Assert.Equal("Received an invalid status code: 'BAD'.", exception.Message);
#else
            Assert.Equal("The server returned an invalid or unrecognized response.", exception.Message);
#endif
#else
            var innerException = Assert.IsType<WebException>(exception.InnerException);
            Assert.Equal(WebExceptionStatus.ServerProtocolViolation, innerException.Status);
#endif
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/12191")]
        public async Task HttpRetryHandler_HandlesNameResolutionFailure()
        {
            // Arrange
            var server = new UnknownDnsServer { Mode = TestServerMode.NameResolutionFailure };

            // Act & Assert
            var exception = await ThrowsException<HttpRequestException>(server);
#if IS_CORECLR
            Assert.NotNull(exception.InnerException);

            if (RuntimeEnvironmentHelper.IsMacOSX)
            {
#if NETCOREAPP3_1_OR_GREATER
                Assert.Equal("nodename nor servname provided, or not known", exception.InnerException.Message);
#else
                Assert.Equal("Device not configured", exception.InnerException.Message);
#endif
            }
            else if (!RuntimeEnvironmentHelper.IsWindows)
            {
#if NETCOREAPP3_1_OR_GREATER
                Assert.Equal("Name or service not known", exception.InnerException.Message);
#else
                Assert.Equal("No such device or address", exception.InnerException.Message);
#endif
            }
            else
            {
#if NETCOREAPP3_1_OR_GREATER
                Assert.Equal("No such host is known.", exception.InnerException.Message);
#else
                Assert.Equal("No such host is known", exception.InnerException.Message);
#endif
            }
#else
            var innerException = Assert.IsType<WebException>(exception.InnerException);
            Assert.Equal(WebExceptionStatus.NameResolutionFailure, innerException.Status);
#endif
        }

        [Fact]
        public async Task HttpRetryHandler_429WithRetryAfterHeader_UsesRetryAfterValue()
        {
            // Arrange
            int attempt = 0;
            // .NET Framework don't have HttpStatusCode.TooManyRequests
            HttpStatusCode tooManyRequestsStatusCode = (HttpStatusCode)429;

            var busyServer = new HttpRetryTestHandler((req, cancellationToken) =>
            {
                attempt++;
                var response = new HttpResponseMessage(tooManyRequestsStatusCode);
                response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromMilliseconds(1));
                return Task.FromResult(response);
            });
            var client = new HttpClient(busyServer);

            // Use a very long timeout that will tell us that Retry-After wasn't being used.
            TimeSpan retryDelay = TimeSpan.FromHours(5);
            var httpRetryHandlerRequest = new HttpRetryHandlerRequest(client, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                RetryDelay = retryDelay
            };
            TestEnvironmentVariableReader environmentVariables = GetEnhancedHttpRetryEnvironmentVariables(retry429: true, observeRetryAfter: true, delayMilliseconds: (int)retryDelay.TotalMilliseconds);
            var httpRetryHandler = new HttpRetryHandler(environmentVariables);
            var logger = new TestLogger();

            // Act
            HttpResponseMessage response;
            int cancellationDelay = Debugger.IsAttached ? int.MaxValue : 60 * 1000;
            using (var cts = new CancellationTokenSource(millisecondsDelay: cancellationDelay))
            {
                try
                {
                    response = await httpRetryHandler.SendAsync(httpRetryHandlerRequest, logger, cts.Token);
                }
                catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
                {
                    throw new Exception("HTTP response Retry-After not used");
                }
            }

            // Assert
            response.StatusCode.Should().Be(tooManyRequestsStatusCode);
            attempt.Should().Be(3);
        }

        [Fact]
        public async Task HttpRetryHandler_LongRetryAfterDuration_UsesEnvVarSetting()
        {
            // Arrange
            int attempts = 0;
            // .NET Framework don't have HttpStatusCode.TooManyRequests
            HttpStatusCode tooManyRequestsStatusCode = (HttpStatusCode)429;

            var busyServer = new HttpRetryTestHandler((req, cancellationToken) =>
            {
                attempts++;
                var response = new HttpResponseMessage(tooManyRequestsStatusCode);
                response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromHours(1));
                return Task.FromResult(response);
            });
            var client = new HttpClient(busyServer);

            // Use a very long timeout that will tell us that Retry-After wasn't being used.
            TimeSpan retryDelay = TimeSpan.FromHours(1);
            var httpRetryHandlerRequest = new HttpRetryHandlerRequest(client, () => new HttpRequestMessage(HttpMethod.Get, TestUrl))
            {
                MaxTries = 2,
                RetryDelay = retryDelay
            };
            TestEnvironmentVariableReader environmentVariables =
                GetEnhancedHttpRetryEnvironmentVariables(retryCount: 2, retry429: true, observeRetryAfter: true, delayMilliseconds: (int)retryDelay.TotalMilliseconds, maxRetryAfterSeconds: 1);
            var httpRetryHandler = new HttpRetryHandler(environmentVariables);
            var logger = new TestLogger();

            // Act
            HttpResponseMessage response;
            int cancellationDelay = Debugger.IsAttached ? int.MaxValue : 60 * 1000;
            using (var cts = new CancellationTokenSource(millisecondsDelay: cancellationDelay))
            {
                try
                {
                    response = await httpRetryHandler.SendAsync(httpRetryHandlerRequest, logger, cts.Token);
                }
                catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
                {
                    throw new Exception("HTTP response Retry-After not used");
                }
            }

            // Assert
            response.StatusCode.Should().Be(tooManyRequestsStatusCode);
            attempts.Should().Be(2);
        }

        private static async Task<T> ThrowsException<T>(ITestServer server) where T : Exception
        {
            return await server.ExecuteAsync(async address =>
            {
                int maxTries = 2;
                TimeSpan retryDelay = TimeSpan.Zero;

                TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(
                 new Dictionary<string, string>()
                 {
                     [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = maxTries.ToString(),
                     [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = retryDelay.TotalMilliseconds.ToString()
                 });

                // Arrange
                var retryHandler = new HttpRetryHandler(testEnvironmentVariableReader);
                var countingHandler = new CountingHandler { InnerHandler = new HttpClientHandler() };
                var httpClient = new HttpClient(countingHandler);
                var request = new HttpRetryHandlerRequest(httpClient, () => new HttpRequestMessage(HttpMethod.Get, address))
                {
                    MaxTries = maxTries,
                    RetryDelay = retryDelay
                };

                // Act
                Func<Task> actionAsync = () => retryHandler.SendAsync(
                    request,
                    new TestLogger(),
                    CancellationToken.None);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<T>(actionAsync);
                Assert.Equal(2, countingHandler.Hits);
                return exception;
            });
        }

        private static TestEnvironmentVariableReader GetEnhancedHttpRetryEnvironmentVariables(
            bool? isEnabled = true,
            int? retryCount = 5,
            int? delayMilliseconds = 0,
            bool? retry429 = true,
            bool? observeRetryAfter = true,
            int? maxRetryAfterSeconds = null)
        => new TestEnvironmentVariableReader(
            new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.IsEnabledEnvironmentVariableName] = isEnabled?.ToString(),
                [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = retryCount?.ToString(),
                [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = delayMilliseconds?.ToString(),
                [EnhancedHttpRetryHelper.Retry429EnvironmentVariableName] = retry429?.ToString(),
                [EnhancedHttpRetryHelper.ObserveRetryAfterEnvironmentVariableName] = observeRetryAfter?.ToString(),
                [EnhancedHttpRetryHelper.MaximumRetryAfterDurationEnvironmentVariableName] = maxRetryAfterSeconds?.ToString(),
            });

        private class CountingHandler : DelegatingHandler
        {
            public int Hits { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Hits++;

                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
