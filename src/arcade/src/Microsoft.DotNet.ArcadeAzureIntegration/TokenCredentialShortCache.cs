// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET472_OR_GREATER

#nullable enable

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Microsoft.DotNet.ArcadeAzureIntegration;


/// <summary>
/// TokenCredentialShortCache is a wrapper around TokenCredential that caches the token for the same scope and request parameters.
/// Cache time is short, 3 minutes only because we don't want to affect the expiration window that's still handled by the underlying TokenCredential implementation. 
/// It helps with reducing the number of requests to Entra or AzureCLI external process during heavy paralellized operations.
/// </summary>
public class TokenCredentialShortCache : TokenCredential
{
    private const int CacheExpirationMinutes = 3;

    public TokenCredentialShortCache(TokenCredential tokenCredential)
    {
        _tokenCredential = tokenCredential;
    }

    private TokenCredential _tokenCredential;
    private ConcurrentDictionary<CacheKey, CachedToken> _tokenCache = new ConcurrentDictionary<CacheKey, CachedToken>();

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return GetTokenAsync(requestContext, cancellationToken).Result;
    }

    public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        CacheKey cacheKey = new CacheKey
        {
            Scopes = string.Join(":", requestContext.Scopes),
            Claims = requestContext.Claims,
            TenantId = requestContext.TenantId,
            IsCaeEnabled = requestContext.IsCaeEnabled
        };

        CachedToken cachedToken = _tokenCache.GetOrAdd(cacheKey, _ => new CachedToken());
        var token = await cachedToken.GetToken(requestContext, () => {
            return _tokenCredential.GetTokenAsync(requestContext, cancellationToken);
        });

        return token;
    }

    private record struct CacheKey
    {
        public required string Scopes { get; init; }
        public required string? Claims { get; init; }
        public required string? TenantId { get; init; }
        public required bool IsCaeEnabled { get; init; }
    }

    private class CachedToken
    {
        private AccessToken? _token;
        private DateTime _shortTimeCacheExpiresOn = DateTime.UtcNow;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async ValueTask<AccessToken> GetToken(TokenRequestContext requestContext, Func<ValueTask<AccessToken>> getFreshToken)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_token != null && _shortTimeCacheExpiresOn > DateTime.UtcNow)
                {
                    return _token.Value;
                }

                _token = await getFreshToken();
                _shortTimeCacheExpiresOn = DateTime.UtcNow.AddMinutes(CacheExpirationMinutes);
                return _token.Value;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}

#endif
