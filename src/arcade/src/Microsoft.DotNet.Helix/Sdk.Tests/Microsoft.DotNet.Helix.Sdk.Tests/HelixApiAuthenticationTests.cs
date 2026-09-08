// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.DotNet.Helix.Client;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests;

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
}
