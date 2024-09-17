// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET472_OR_GREATER

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;


namespace Microsoft.DotNet.Build.Tasks.Feed;

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
        public required string Claims { get; init; }
        public required string TenantId { get; init; }
        public required bool IsCaeEnabled { get; init; }
    }

    private class CachedToken
    {
        private AccessToken? token { get; set; }
        private DateTime shortTimeCacheExpiresOn = DateTime.UtcNow.AddMinutes(CacheExpirationMinutes);
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async ValueTask<AccessToken> GetToken(TokenRequestContext requestContext, Func<ValueTask<AccessToken>> getFreshToken)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (token != null && shortTimeCacheExpiresOn > DateTime.UtcNow)
                {
                    return token.Value;
                }

                token = await getFreshToken();
                shortTimeCacheExpiresOn = DateTime.UtcNow.AddMinutes(CacheExpirationMinutes);
                return token.Value;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}

#endif
