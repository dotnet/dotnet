// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;

namespace NuGet.Protocol
{
    public class TokenStore
    {
        private readonly ConcurrentDictionary<Uri, string> _tokenCache = new ConcurrentDictionary<Uri, string>();

        public static TokenStore Instance { get; } = new TokenStore();

        public Guid Version { get; private set; } = Guid.NewGuid();

        public string GetToken(Uri sourceUri)
        {
            string token;
            if (_tokenCache.TryGetValue(sourceUri, out token))
            {
                return token;
            }

            var rootUri = GetRootUri(sourceUri);
            if (_tokenCache.TryGetValue(rootUri, out token))
            {
                return token;
            }

            return null;
        }

        public void AddToken(Uri sourceUri, string token)
        {
            StoreToken(sourceUri, token);

            var rootUri = GetRootUri(sourceUri);
            StoreToken(rootUri, token);
        }

        private void StoreToken(Uri uri, string token)
        {
            _tokenCache.AddOrUpdate(
                uri,
                addValueFactory: _ => { Version = Guid.NewGuid(); return token; },
                updateValueFactory: (_, __) => { Version = Guid.NewGuid(); return token; });
        }

        private static Uri GetRootUri(Uri uri)
        {
            return new Uri(uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped));
        }
    }
}
