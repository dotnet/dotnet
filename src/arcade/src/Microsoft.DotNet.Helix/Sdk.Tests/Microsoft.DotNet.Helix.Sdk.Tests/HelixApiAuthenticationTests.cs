// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.DotNet.Helix.Client;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.Sdk;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class HelixApiAuthenticationTests
    {
        [Fact]
        public void AnonymousOptionsExposeAnonymousMode()
        {
            var options = new HelixApiOptions();

            Assert.Equal(HelixApiAuthenticationMode.Anonymous, options.AuthenticationMode);
            Assert.Empty(options.TokenScopes);
        }

        [Fact]
        public void PatCredentialPreservesLegacyAuthenticationMode()
        {
            var options = new HelixApiOptions(new HelixApiTokenCredential("legacy-token"));

            Assert.Equal(HelixApiAuthenticationMode.PersonalAccessToken, options.AuthenticationMode);
            Assert.Empty(options.TokenScopes);
        }

        [Fact]
        public void ProductionCredentialUsesProductionScope()
        {
            var options = new HelixApiOptions(new TestTokenCredential());

            Assert.Equal(HelixApiAuthenticationMode.EntraId, options.AuthenticationMode);
            Assert.Equal(new[] { HelixApiOptions.ProductionScope }, options.TokenScopes);
        }

        [Fact]
        public void StagingCredentialUsesStagingScope()
        {
            var options = new HelixApiOptions(
                new Uri("https://helix.int-dot.net/"),
                new TestTokenCredential());

            Assert.Equal(HelixApiAuthenticationMode.EntraId, options.AuthenticationMode);
            Assert.Equal(new[] { HelixApiOptions.StagingScope }, options.TokenScopes);
        }

        [Fact]
        public void CustomHostRequiresExplicitScope()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new HelixApiOptions(new Uri("https://localhost:5001/"), new TestTokenCredential()));

            Assert.Contains("explicit scopes", exception.Message);
        }

        [Fact]
        public void EntraCredentialRequiresBaseUri()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new HelixApiOptions(null, new TestTokenCredential()));
        }

        [Fact]
        public void EntraCredentialRequiresAbsoluteBaseUri()
        {
            Assert.Throws<ArgumentException>(() =>
                new HelixApiOptions(new Uri("relative", UriKind.Relative), new TestTokenCredential()));
        }

        [Fact]
        public void CustomHostUsesExplicitScope()
        {
            const string scope = "api://custom-helix/.default";
            var options = new HelixApiOptions(
                new Uri("https://localhost:5001/"),
                new TestTokenCredential(),
                new[] { scope });

            Assert.Equal(HelixApiAuthenticationMode.EntraId, options.AuthenticationMode);
            Assert.Equal(new[] { scope }, options.TokenScopes);
        }

        [Fact]
        public void ExplicitScopeCannotBeEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
                new HelixApiOptions(
                    new Uri("https://localhost:5001/"),
                    new TestTokenCredential(),
                    new[] { "" }));
        }

        [Fact]
        public void ExplicitScopeRequiresAbsoluteBaseUri()
        {
            Assert.Throws<ArgumentException>(() =>
                new HelixApiOptions(
                    new Uri("relative", UriKind.Relative),
                    new TestTokenCredential(),
                    new[] { "api://custom-helix/.default" }));
        }

        [Fact]
        public void ExplicitScopeRejectsPatCredential()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new HelixApiOptions(
                    new Uri("https://localhost:5001/"),
                    new HelixApiTokenCredential("legacy-token"),
                    new[] { "api://custom-helix/.default" }));

            Assert.Contains("without explicit scopes", exception.Message);
        }

        [Fact]
        public void CredentialBasedOptionsRejectHttpBaseUri()
        {
            var baseUri = new Uri("http://localhost:5001/");

            var patException = Assert.Throws<ArgumentException>(() =>
                new HelixApiOptions(baseUri, new HelixApiTokenCredential("legacy-token")));
            var entraException = Assert.Throws<ArgumentException>(() =>
                new HelixApiOptions(
                    baseUri,
                    new TestTokenCredential(),
                    new[] { "api://custom-helix/.default" }));

            Assert.Contains("HTTPS", patException.Message);
            Assert.Contains("HTTPS", entraException.Message);
        }

        [Fact]
        public void CredentialBasedOptionsAcceptHttpsBaseUri()
        {
            var baseUri = new Uri("https://localhost:5001/");

            var patOptions = new HelixApiOptions(
                baseUri,
                new HelixApiTokenCredential("legacy-token"));
            var entraOptions = new HelixApiOptions(
                baseUri,
                new TestTokenCredential(),
                new[] { "api://custom-helix/.default" });

            Assert.Equal(HelixApiAuthenticationMode.PersonalAccessToken, patOptions.AuthenticationMode);
            Assert.Equal(HelixApiAuthenticationMode.EntraId, entraOptions.AuthenticationMode);
        }

        [Fact]
        public void AnonymousOptionsAllowHttpBaseUri()
        {
            var baseUri = new Uri("http://localhost:5001/");

            var options = new HelixApiOptions(baseUri);

            Assert.Equal(HelixApiAuthenticationMode.Anonymous, options.AuthenticationMode);
            Assert.Equal(baseUri, options.BaseUri);
        }

        [Fact]
        public void EntraFactoryUsesProductionScope()
        {
            var api = Assert.IsType<HelixApi>(
                ApiFactory.GetAuthenticatedWithEntra(new TestTokenCredential()));

            Assert.Equal(HelixApiAuthenticationMode.EntraId, api.Options.AuthenticationMode);
            Assert.Equal(new[] { HelixApiOptions.ProductionScope }, api.Options.TokenScopes);
        }

        [Fact]
        public void EntraFactoryRequiresCredential()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ApiFactory.GetAuthenticatedWithEntra(null));
            Assert.Throws<ArgumentNullException>(() =>
                ApiFactory.GetAuthenticatedWithEntra("https://helix.dot.net/", null));
            Assert.Throws<ArgumentNullException>(() =>
                ApiFactory.GetAuthenticatedWithEntra(
                    "https://localhost:5001/",
                    null,
                    "api://custom-helix/.default"));
        }

        [Fact]
        public void EntraFactoryRejectsPatCredential()
        {
            var credential = new HelixApiTokenCredential("legacy-token");

            var productionException = Assert.Throws<ArgumentException>(() =>
                ApiFactory.GetAuthenticatedWithEntra(credential));
            var hostException = Assert.Throws<ArgumentException>(() =>
                ApiFactory.GetAuthenticatedWithEntra("https://helix.dot.net/", credential));
            var explicitScopeException = Assert.Throws<ArgumentException>(() =>
                ApiFactory.GetAuthenticatedWithEntra(
                    "https://localhost:5001/",
                    credential,
                    "api://custom-helix/.default"));

            Assert.Contains("GetAuthenticated", productionException.Message);
            Assert.Contains("GetAuthenticated", hostException.Message);
            Assert.Contains("GetAuthenticated", explicitScopeException.Message);
        }

        [Fact]
        public async Task EntraCredentialReacquiresTokenAfterExpiration()
        {
            TimeSpan tokenLifetime = TimeSpan.FromMilliseconds(200);
            TimeSpan expirationMargin = TimeSpan.FromMilliseconds(300);
            var credential = new ShortLivedTokenCredential(tokenLifetime);
            using var httpClient = new HttpClient(new SuccessfulHttpMessageHandler());
            var options = new HelixApiOptions(
                new Uri("https://helix.dot.net/"),
                credential)
            {
                Transport = new HttpClientTransport(httpClient),
            };
            var api = new HelixApi(options);

            using HttpMessage firstMessage = api.Pipeline.CreateMessage();
            firstMessage.Request.Method = RequestMethod.Get;
            firstMessage.Request.Uri.Reset(options.BaseUri);
            await api.Pipeline.SendAsync(firstMessage, CancellationToken.None);
            Assert.True(firstMessage.Request.Headers.TryGetValue("Authorization", out string firstAuthorization));

            await Task.Delay(tokenLifetime + expirationMargin);

            using HttpMessage secondMessage = api.Pipeline.CreateMessage();
            secondMessage.Request.Method = RequestMethod.Get;
            secondMessage.Request.Uri.Reset(options.BaseUri);
            await api.Pipeline.SendAsync(secondMessage, CancellationToken.None);
            Assert.True(secondMessage.Request.Headers.TryGetValue("Authorization", out string secondAuthorization));

            Assert.Equal(2, credential.CallCount);
            Assert.NotEqual(firstAuthorization, secondAuthorization);
        }

        [Theory]
        [InlineData(false, null, HelixApiAuthenticationMode.Anonymous)]
        [InlineData(false, "legacy-token", HelixApiAuthenticationMode.PersonalAccessToken)]
        [InlineData(true, null, HelixApiAuthenticationMode.EntraId)]
        [InlineData(true, "legacy-token", HelixApiAuthenticationMode.EntraId)]
        public void HelixTaskSelectsRequestedAuthenticationMode(
            bool useEntraAuthentication,
            string accessToken,
            HelixApiAuthenticationMode expectedMode)
        {
            var api = Assert.IsType<HelixApi>(
                HelixTask.CreateHelixApi(
                    "https://helix.dot.net/",
                    accessToken,
                    useEntraAuthentication,
                    () => ApiFactory.GetAuthenticatedWithEntra(new TestTokenCredential())));

            Assert.Equal(expectedMode, api.Options.AuthenticationMode);
        }

        [Theory]
        [InlineData(false, null, "https://storage/results.trx")]
        [InlineData(false, "legacy-token", "https://storage/results.trx?access_token=legacy-token")]
        [InlineData(true, null, "https://storage/results.trx")]
        [InlineData(true, "legacy-token", "https://storage/results.trx")]
        public void UploadedFileLinksOnlyContainTokenInPatMode(
            bool useEntraAuthentication,
            string accessToken,
            string expectedLink)
        {
            var files = ImmutableList.Create(new UploadedFile("results.trx", "https://storage/results.trx"));

            IImmutableList<UploadedFile> result = GetHelixWorkItems.AddAccessTokenToFileLinks(
                files,
                accessToken,
                useEntraAuthentication);

            Assert.Equal(expectedLink, Assert.Single(result).Link);
        }

        [Theory]
        [InlineData(false, null, false)]
        [InlineData(false, "legacy-token", true)]
        [InlineData(true, null, true)]
        [InlineData(true, "legacy-token", true)]
        public void CancellationRecognizesConfiguredAuthentication(
            bool useEntraAuthentication,
            string accessToken,
            bool expected)
        {
            Assert.Equal(
                expected,
                CancelHelixJobs.CanUseAuthenticatedCancellation(useEntraAuthentication, accessToken));
        }

        [Theory]
        [InlineData(true, null, null, null)]
        [InlineData(true, "legacy-token", null, null)]
        [InlineData(true, null, "creator", "Creator is forbidden when using authenticated access.")]
        [InlineData(false, "legacy-token", "creator", "Creator is forbidden when using authenticated access.")]
        [InlineData(false, null, null, "Creator is required when using anonymous access.")]
        [InlineData(false, null, "creator", null)]
        public void CreatorValidationRecognizesEntraAsAuthenticated(
            bool useEntraAuthentication,
            string accessToken,
            string creator,
            string expectedError)
        {
            Assert.Equal(
                expectedError,
                SendHelixJob.GetCreatorValidationError(
                    useEntraAuthentication,
                    accessToken,
                    creator));
        }

        private sealed class TestTokenCredential : TokenCredential
        {
            public override AccessToken GetToken(
                TokenRequestContext requestContext,
                CancellationToken cancellationToken)
            {
                return new AccessToken("test-token", DateTimeOffset.UtcNow.AddMinutes(30));
            }

            public override ValueTask<AccessToken> GetTokenAsync(
                TokenRequestContext requestContext,
                CancellationToken cancellationToken)
            {
                return new ValueTask<AccessToken>(GetToken(requestContext, cancellationToken));
            }
        }

        private sealed class SuccessfulHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
        }

        private sealed class ShortLivedTokenCredential : TokenCredential
        {
            private readonly TimeSpan _lifetime;

            public ShortLivedTokenCredential(TimeSpan lifetime)
            {
                _lifetime = lifetime;
            }

            public int CallCount { get; private set; }

            public override AccessToken GetToken(
                TokenRequestContext requestContext,
                CancellationToken cancellationToken)
            {
                return new AccessToken(
                    $"test-token-{++CallCount}",
                    DateTimeOffset.UtcNow.Add(_lifetime));
            }

            public override ValueTask<AccessToken> GetTokenAsync(
                TokenRequestContext requestContext,
                CancellationToken cancellationToken)
            {
                return new ValueTask<AccessToken>(GetToken(requestContext, cancellationToken));
            }
        }
    }
}
