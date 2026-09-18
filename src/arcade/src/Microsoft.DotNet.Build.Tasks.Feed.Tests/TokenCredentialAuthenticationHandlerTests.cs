// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Azure.Core;
using Microsoft.DotNet.ArcadeAzureIntegration;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests;

public class TokenCredentialAuthenticationHandlerTests
{
    private class MockTokenCredential : TokenCredential
    {
        private readonly Func<string> _tokenGenerator;
        public int GetTokenCallCount { get; private set; }

        public MockTokenCredential(Func<string> tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public MockTokenCredential(string token)
            : this(() => token)
        {
        }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            GetTokenCallCount++;
            return new AccessToken(_tokenGenerator(), DateTimeOffset.UtcNow.AddHours(1));
        }

        public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            GetTokenCallCount++;
            return new ValueTask<AccessToken>(new AccessToken(_tokenGenerator(), DateTimeOffset.UtcNow.AddHours(1)));
        }
    }

    private class InspectableHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

    [Fact]
    public async Task DynamicTokenRefresh_AttachesBearerTokenPerRequest()
    {
        int callIndex = 0;
        var tokens = new[] { "token-1", "token-2", "token-3" };
        var credential = new MockTokenCredential(() => tokens[callIndex++]);
        var innerHandler = new InspectableHttpMessageHandler();

        using var authHandler = new TokenCredentialAuthenticationHandler(
            credential,
            TokenCredentialAuthenticationHandler.AzureDevOpsScopes,
            innerHandler);

        using var client = new HttpClient(authHandler);

        // First request
        await client.GetAsync("https://dev.azure.com/dnceng/test");
        innerHandler.LastRequest.Headers.Authorization.Should().NotBeNull();
        innerHandler.LastRequest.Headers.Authorization.Scheme.Should().Be("Bearer");
        innerHandler.LastRequest.Headers.Authorization.Parameter.Should().Be("token-1");

        // Second request gets the fresh/refreshed token
        await client.GetAsync("https://dev.azure.com/dnceng/test");
        innerHandler.LastRequest.Headers.Authorization.Should().NotBeNull();
        innerHandler.LastRequest.Headers.Authorization.Scheme.Should().Be("Bearer");
        innerHandler.LastRequest.Headers.Authorization.Parameter.Should().Be("token-2");

        // Third request gets the next refreshed token
        await client.GetAsync("https://dev.azure.com/dnceng/test");
        innerHandler.LastRequest.Headers.Authorization.Should().NotBeNull();
        innerHandler.LastRequest.Headers.Authorization.Scheme.Should().Be("Bearer");
        innerHandler.LastRequest.Headers.Authorization.Parameter.Should().Be("token-3");

        credential.GetTokenCallCount.Should().Be(3);
    }

    [Fact]
    public async Task PreservesExistingAuthorizationHeader()
    {
        var credential = new MockTokenCredential("entra-token");
        var innerHandler = new InspectableHttpMessageHandler();

        using var authHandler = new TokenCredentialAuthenticationHandler(
            credential,
            TokenCredentialAuthenticationHandler.AzureDevOpsScopes,
            innerHandler);

        using var client = new HttpClient(authHandler);

        using var customRequest = new HttpRequestMessage(HttpMethod.Get, "https://dev.azure.com/dnceng/test");
        customRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", "custom-auth");

        await client.SendAsync(customRequest);

        innerHandler.LastRequest.Headers.Authorization.Should().NotBeNull();
        innerHandler.LastRequest.Headers.Authorization.Scheme.Should().Be("Basic");
        innerHandler.LastRequest.Headers.Authorization.Parameter.Should().Be("custom-auth");
        credential.GetTokenCallCount.Should().Be(0);
    }
}
