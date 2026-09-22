// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Microsoft.DotNet.ArcadeAzureIntegration;

/// <summary>
/// An HttpMessageHandler that dynamically attaches and refreshes Bearer tokens
/// from a TokenCredential for outgoing HTTP requests.
/// </summary>
/// <remarks>
/// This handler calls <see cref="TokenCredential.GetTokenAsync"/>/<see cref="TokenCredential.GetToken"/>
/// on every request that doesn't already carry an Authorization header. It does not itself cache tokens;
/// it relies on the wrapped <see cref="TokenCredential"/> to return a cached token cheaply when the current
/// one is still valid, and to only perform a real token acquisition when a refresh is actually needed. For
/// example, <see cref="DefaultIdentityTokenCredential"/> wraps its underlying credentials in
/// <see cref="TokenCredentialShortCache"/>, and credential types like ManagedIdentityCredential cache
/// internally as well. This is what makes per-request token attachment safe for long-running, high-volume
/// operations (e.g. downloading many assets over 30+ minutes): each request gets whatever token is
/// currently valid instead of the single token captured when the HttpClient was created.
/// </remarks>
public class TokenCredentialAuthenticationHandler : DelegatingHandler
{
    public const string AzureDevOpsScope = "499b84ac-1321-427f-aa17-267ca6975798/.default";
    public static readonly string[] AzureDevOpsScopes = [AzureDevOpsScope];

    private readonly TokenCredential _credential;
    private readonly string[] _scopes;

    public TokenCredentialAuthenticationHandler(
        TokenCredential credential,
        string[] scopes,
        HttpMessageHandler? innerHandler = null)
        : base(innerHandler ?? new HttpClientHandler { CheckCertificateRevocationList = true })
    {
        _credential = credential ?? throw new ArgumentNullException(nameof(credential));
        _scopes = scopes ?? throw new ArgumentNullException(nameof(scopes));
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization == null)
        {
            var tokenRequestContext = new TokenRequestContext(_scopes);
            var token = await _credential.GetTokenAsync(tokenRequestContext, cancellationToken).ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    protected override HttpResponseMessage Send(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization == null)
        {
            var tokenRequestContext = new TokenRequestContext(_scopes);
            var token = _credential.GetToken(tokenRequestContext, cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        }

        return base.Send(request, cancellationToken);
    }
}
