// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if !IS_CORECLR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NuGet.Configuration;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class StsAuthenticationHandlerTests
    {
        [Fact]
        public async Task SendAsync_WithUnauthenticatedSource_PassesThru()
        {
            var packageSource = new PackageSource("http://package.source.net");
            var tokenStore = new TokenStore();

            var handler = new StsAuthenticationHandler(packageSource, tokenStore)
            {
                InnerHandler = GetLambdaMessageHandler(HttpStatusCode.OK)
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_WithValidAcquiredToken_RetriesRequest()
        {
            var packageSource = new PackageSource("http://package.source.net");
            var tokenStore = new TokenStore();
            Func<string, string, string> tokenFactory = (endpoint, realm) => $"{realm}@{endpoint}";

            var handler = new StsAuthenticationHandler(packageSource, tokenStore, tokenFactory)
            {
                InnerHandler = new LambdaMessageHandler(
                    request =>
                    {
                        var decodedToken = GetStsTokenFromRequest(request);
                        if (decodedToken == null)
                        {
                            var authResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                            authResponse.Headers.Add(StsAuthenticationHandler.STSEndPointHeader, "http://TEST_ENDPOINT");
                            authResponse.Headers.Add(StsAuthenticationHandler.STSRealmHeader, "TEST-REALM");
                            return authResponse;
                        }

                        Assert.Equal("TEST-REALM@http://TEST_ENDPOINT", decodedToken);
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    })
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_WithNotSupportedProtocol_PassesThru()
        {
            var packageSource = new PackageSource("http://package.source.net");
            var tokenStore = new TokenStore();
            Func<string, string, string> tokenFactory = (endpoint, realm) =>
            {
                throw new InvalidOperationException("Should NOT mint new token.");
            };

            var handler = new StsAuthenticationHandler(packageSource, tokenStore, tokenFactory)
            {
                InnerHandler = new LambdaMessageHandler(
                    request =>
                    {
                        var authResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        authResponse.Headers.Add("WWW-Authenticate", "Basic realm=\"TEST-REALM\"");
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    })
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_WithValidCachedToken_Returns200()
        {
            var packageSource = new PackageSource("http://package.source.net");
            var tokenStore = new TokenStore();
            tokenStore.AddToken(packageSource.SourceUri, "TEST-TOKEN");
            Func<string, string, string> tokenFactory = (endpoint, realm) =>
            {
                throw new InvalidOperationException("Should NOT mint new token.");
            };

            var handler = new StsAuthenticationHandler(packageSource, tokenStore, tokenFactory)
            {
                InnerHandler = new LambdaMessageHandler(
                    request =>
                    {
                        var decodedToken = GetStsTokenFromRequest(request, decode: false);
                        Assert.NotNull(decodedToken);
                        Assert.Equal("TEST-TOKEN", decodedToken);
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    })
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_WithInvalidCachedToken_Returns401()
        {
            var packageSource = new PackageSource("http://package.source.net");
            var tokenStore = new TokenStore();
            tokenStore.AddToken(packageSource.SourceUri, "TEST-TOKEN");
            Func<string, string, string> tokenFactory = (endpoint, realm) =>
            {
                throw new InvalidOperationException("Should NOT mint new token.");
            };

            var handler = new StsAuthenticationHandler(packageSource, tokenStore, tokenFactory)
            {
                InnerHandler = new LambdaMessageHandler(
                    request =>
                    {
                        var decodedToken = GetStsTokenFromRequest(request, decode: false);
                        Assert.NotNull(decodedToken);
                        Assert.Equal("TEST-TOKEN", decodedToken);
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    })
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_RetryWithClonedRequest()
        {
            var packageSource = new PackageSource("http://package.source.net");
            var tokenStore = new TokenStore();
            tokenStore.AddToken(packageSource.SourceUri, "TEST-TOKEN");
            Func<string, string, string> tokenFactory = (endpoint, realm) =>
            {
                throw new InvalidOperationException("Should NOT mint new token.");
            };

            var requests = 0;
            var handler = new StsAuthenticationHandler(packageSource, tokenStore, tokenFactory)
            {
                InnerHandler = new LambdaMessageHandler(
                    request =>
                    {
                        Assert.Null(request.Headers.Authorization);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "TEST");
                        requests++;

                        tokenStore.AddToken(packageSource.SourceUri, "TEST-TOKEN"); // update version for retry

                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    })
            };

            var response = await SendAsync(handler);

            Assert.True(requests > 1, "No retries");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private static string GetStsTokenFromRequest(HttpRequestMessage request, bool decode = true)
        {
            IEnumerable<string> headerValues;
            if (!request.Headers.TryGetValues(StsAuthenticationHandler.STSTokenHeader, out headerValues))
            {
                return null;
            }

            var rawToken = headerValues.Single();
            if (!decode)
            {
                return rawToken;
            }

            var decodedToken = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(rawToken));
            return decodedToken;
        }

        private static LambdaMessageHandler GetLambdaMessageHandler(HttpStatusCode statusCode)
        {
            return new LambdaMessageHandler(
                _ => new HttpResponseMessage(statusCode));
        }

        private static LambdaMessageHandler GetLambdaMessageHandler(params HttpStatusCode[] statusCodes)
        {
            var responses = new Queue<HttpStatusCode>(statusCodes);
            return new LambdaMessageHandler(
                _ => new HttpResponseMessage(responses.Dequeue()));
        }

        private static async Task<HttpResponseMessage> SendAsync(HttpMessageHandler handler, HttpRequestMessage request = null)
        {
            using (var client = new HttpClient(handler))
            {
                return await client.SendAsync(request ?? new HttpRequestMessage(HttpMethod.Get, "http://foo"));
            }
        }
    }
}
#endif
