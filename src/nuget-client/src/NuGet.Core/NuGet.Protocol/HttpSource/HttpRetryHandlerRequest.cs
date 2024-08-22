// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace NuGet.Protocol
{
    /// <summary>
    /// A request to be handled by <see cref="HttpRetryHandler"/>. This type should contain all
    /// of the knowledge necessary to make a request, while handling transient transport errors.
    /// </summary>
    public class HttpRetryHandlerRequest
    {
        public static readonly int DefaultMaxTries = 3;
        public static readonly TimeSpan DefaultDownloadTimeout = TimeSpan.FromSeconds(60);

        public HttpRetryHandlerRequest(HttpClient httpClient, Func<HttpRequestMessage> requestFactory)
        {
            HttpClient = httpClient;
            RequestFactory = requestFactory;
            CompletionOption = HttpCompletionOption.ResponseHeadersRead;
            MaxTries = DefaultMaxTries;
            RequestTimeout = TimeSpan.FromSeconds(100);
            RetryDelay = TimeSpan.FromMilliseconds(200);
            DownloadTimeout = DefaultDownloadTimeout;
        }

        /// <summary>The HTTP client to use for each request attempt.</summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// The factory that generates each request message. This factory is invoked for each attempt.
        /// </summary>
        public Func<HttpRequestMessage> RequestFactory { get; }

        /// <summary>The HTTP completion option to use for the next attempt.</summary>
        public HttpCompletionOption CompletionOption { get; set; }

        /// <summary>The maximum number of times to try the request. This value includes the initial attempt.</summary>
        public int MaxTries { get; set; }

        /// <summary>How long to wait on the request to come back with a response.</summary>
        public TimeSpan RequestTimeout { get; set; }

        /// <summary>How long to wait before trying again after a failed request.</summary>
        /// <summary>This API is intended only for testing purposes and should not be used in product code.</summary>
        public TimeSpan RetryDelay { get; set; }

        /// <summary>The timeout to apply to <see cref="DownloadTimeoutStream"/> instances.</summary>
        public TimeSpan DownloadTimeout { get; set; }

        /// <summary>
        /// Additional headers to add to the request.
        /// </summary>
        public IList<KeyValuePair<string, IEnumerable<string>>> AddHeaders { get; set; } = new List<KeyValuePair<string, IEnumerable<string>>>();

        /// <summary>Boolean representing whether the first attempt of this request is already a retry.</summary>
        public bool IsRetry { get; set; }

        /// <summary>Boolean representing whether this retry request is the last for the URL.</summary>
        public bool IsLastAttempt { get; set; } = true;
    }
}
