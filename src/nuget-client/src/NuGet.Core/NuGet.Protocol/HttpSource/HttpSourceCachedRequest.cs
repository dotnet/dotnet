// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// A cached HTTP request handled by <see cref="HttpSource"/>.
    /// </summary>
    public class HttpSourceCachedRequest
    {
        public HttpSourceCachedRequest(string uri, string cacheKey, HttpSourceCacheContext cacheContext)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (cacheKey == null)
            {
                throw new ArgumentNullException(nameof(cacheKey));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            Uri = uri;
            CacheKey = cacheKey;
            CacheContext = cacheContext;
        }

        /// <summary>
        /// The URI to request with <code>GET</code>.
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// The cache key to use when fetching and storing the response from the HTTP cache. This
        /// cache key is scoped to the NuGet source. That is to say that each NuGet source has its
        /// own independent HTTP cache.
        /// </summary>
        public string CacheKey { get; }

        /// <summary>
        /// The cache context.
        /// </summary>
        public HttpSourceCacheContext CacheContext { get; }

        /// <summary>
        /// The header values to apply when building the <see cref="HttpRequestMessage"/>.
        /// </summary>
        public IList<MediaTypeWithQualityHeaderValue> AcceptHeaderValues { get; } = new List<MediaTypeWithQualityHeaderValue>();

        /// <summary>
        /// When processing the <see cref="HttpResponseMessage"/>, this flag allows
        /// <code>404 Not Found</code> to be interpreted as a null response. This value defaults
        /// to <code>false</code>.
        /// </summary>
        public bool IgnoreNotFounds { get; set; }

        /// <summary>The maximum number of times to try the request. This value includes the initial attempt.</summary>
        public int MaxTries { get; set; } = HttpRetryHandlerRequest.DefaultMaxTries;

        /// <summary>
        /// A method used to validate the response stream. This method should not
        /// dispose the stream and should throw an exception when the content is invalid.
        /// </summary>
        public Action<Stream> EnsureValidContents { get; set; }

        /// <summary>
        /// The timeout to use when fetching the <see cref="HttpResponseMessage"/>. Since
        /// <see cref="HttpSource"/> only uses <see cref="HttpCompletionOption.ResponseHeadersRead"/>,
        /// this means that we wait this amount of time for only the HTTP headers to be returned.
        /// Downloading the response body is not included in this timeout.
        /// </summary>
        public TimeSpan RequestTimeout { get; set; } = HttpSourceRequest.DefaultRequestTimeout;

        /// <summary>The timeout to apply to <see cref="DownloadTimeoutStream"/> instances.</summary>
        public TimeSpan DownloadTimeout { get; set; } = HttpRetryHandlerRequest.DefaultDownloadTimeout;

        /// <summary>Boolean representing whether the first attempt of this request is already a retry.</summary>
        public bool IsRetry { get; set; }

        /// <summary>Boolean representing whether this retry request is the last for the URL.</summary>
        public bool IsLastAttempt { get; set; } = true;
    }
}
