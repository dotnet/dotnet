// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using NuGet.Common;

namespace NuGet.Protocol
{
    /// <summary>
    /// A non-cached HTTP request handled by <see cref="HttpSource"/>.
    /// </summary>
    public class HttpSourceRequest
    {
        public static readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(100);

        public HttpSourceRequest(string uri, ILogger log)
            : this(() => HttpRequestMessageFactory.Create(HttpMethod.Get, uri, log))
        {
        }

        public HttpSourceRequest(Uri uri, ILogger log)
            : this(() => HttpRequestMessageFactory.Create(HttpMethod.Get, uri, log))
        {
        }

        public HttpSourceRequest(Func<HttpRequestMessage> requestFactory)
        {
            if (requestFactory == null)
            {
                throw new ArgumentNullException(nameof(requestFactory));
            }

            RequestFactory = requestFactory;
        }

        /// <summary>
        /// A factory that can be called repeatedly to build the HTTP request message.
        /// </summary>
        public Func<HttpRequestMessage> RequestFactory { get; }

        /// <summary>
        /// When processing the <see cref="HttpResponseMessage"/>, this flag allows
        /// <code>404 Not Found</code> to be interpreted as a null response. This value defaults
        /// to <code>false</code>.
        /// </summary>
        public bool IgnoreNotFounds { get; set; }

        /// <summary>
        /// The timeout to use when fetching the <see cref="HttpResponseMessage"/>. Since
        /// <see cref="HttpSource"/> only uses <see cref="HttpCompletionOption.ResponseHeadersRead"/>,
        /// this means that we wait this amount of time for only the HTTP headers to be returned.
        /// Downloading the response body is not included in this timeout.
        /// </summary>
        public TimeSpan RequestTimeout { get; set; } = DefaultRequestTimeout;

        /// <summary>The maximum number of times to try the request. This value includes the initial attempt.</summary>
        public int MaxTries { get; set; } = HttpRetryHandlerRequest.DefaultMaxTries;

        /// <summary>The timeout to apply to <see cref="DownloadTimeoutStream"/> instances.</summary>
        public TimeSpan DownloadTimeout { get; set; } = HttpRetryHandlerRequest.DefaultDownloadTimeout;

        /// <summary>Boolean representing whether the first attempt of this request is already a retry.</summary>
        public bool IsRetry { get; set; }

        /// <summary>Boolean representing whether this retry request is the last for the URL.</summary>
        public bool IsLastAttempt { get; set; } = true;
    }
}
