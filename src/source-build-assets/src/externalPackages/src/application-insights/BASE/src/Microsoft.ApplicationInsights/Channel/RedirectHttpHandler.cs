namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal class RedirectHttpHandler : HttpClientHandler
    {
        internal const int MaxRedirect = 10;
        internal readonly TimeSpan DefaultCacheExpirationDuration = TimeSpan.FromHours(12);

        private readonly Cache<Uri> cache = new Cache<Uri>();

        public RedirectHttpHandler()
        {
            this.AllowAutoRedirect = false;
        }

        /// <summary>
        /// This method will handle all requests from HttpClient.SendAsync().
        /// This inspects <see cref="HttpResponseMessage"/> to see if we received a redirect from the Ingestion service.
        /// </summary>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.cache.TryRead(out Uri redirectUri))
            { 
                request.RequestUri = redirectUri;
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (IsRedirection(response.StatusCode))
            {
                return await this.HandleRedirectAsync(request, response, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                return response;
            }
        }

        private static bool IsRedirection(HttpStatusCode statusCode)
        {
            switch ((int)statusCode)
            {
                case 307: // StatusCodes.Status307TemporaryRedirect
                case 308: // StatusCodes.Status308PermanentRedirect
                    return true;
                default:
                    return false;
            }
        }

        private static bool TryGetRedirectUri(HttpResponseMessage httpResponseMessage, out Uri redirectUri)
        {
            redirectUri = httpResponseMessage?.Headers?.Location;
            return redirectUri != null && redirectUri.IsAbsoluteUri;
        }

        private static bool TryGetRedirectCacheTimeSpan(HttpResponseMessage httpResponseMessage, out TimeSpan cacheExpirationDuration)
        {
            cacheExpirationDuration = httpResponseMessage?.Headers?.CacheControl?.MaxAge ?? default;
            return cacheExpirationDuration != default;
        }

        /// <summary>
        /// This method handles redirection.
        /// This keeps extra var allocation out of the hot path.
        /// </summary>
        private async Task<HttpResponseMessage> HandleRedirectAsync(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            int redirectCount = 0;

            do
            {
                if (TryGetRedirectUri(response, out Uri newRedirectUri))
                {
                    if (!TryGetRedirectCacheTimeSpan(response, out TimeSpan cacheExpirationDuration))
                    {
                        // if failed to read cache, use default
                        CoreEventSource.Log.IngestionRedirectInformation($"Failed to parse redirect cache, using default.");
                        cacheExpirationDuration = this.DefaultCacheExpirationDuration;
                    }

                    this.cache.Set(newRedirectUri, cacheExpirationDuration);

                    CoreEventSource.Log.IngestionRedirectInformation($"New Ingestion Endpoint: {newRedirectUri.AbsoluteUri} Expires: {cacheExpirationDuration}");
                    request.RequestUri = newRedirectUri;
                }
                else
                {
                    CoreEventSource.Log.IngestionRedirectError($"Failed to parse redirect headers.");
                    break;
                }

                redirectCount++;
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            while (redirectCount < MaxRedirect && IsRedirection(response.StatusCode));

            return response;
        }

        /// <summary>
        /// Simple class to encapsulate redirect cache.
        /// </summary>
        private class Cache<T>
        {
            private readonly object lockObj = new object();

            private T cachedValue = default;

            private DateTimeOffset expiration = DateTimeOffset.MinValue;

            public bool TryRead(out T cachedValue)
            {
                if (DateTimeOffset.UtcNow < this.expiration)
                {
                    cachedValue = this.cachedValue;
                    return true;
                }
                else
                {
                    cachedValue = default;
                    return false;
                }
            }
        
            public void Set(T cachingValue, TimeSpan expire)
            {
                lock (this.lockObj)
                {
                    this.cachedValue = cachingValue;
                    this.expiration = DateTimeOffset.UtcNow.Add(expire);
                }
            }
        }
    }
}
