// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Security.AntiSSRF
{
    internal class RedirectHandler : DelegatingHandler
    {
        private readonly AntiSSRFPolicy _policy;

        internal RedirectHandler(HttpMessageHandler innerHandler, AntiSSRFPolicy policy) : base(innerHandler)
        {
            _policy = policy;
        }

#if NET5_0_OR_GREATER
        internal HttpResponseMessage SendWrapper(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Send(request, cancellationToken);
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!_policy.IsHttpRequestAllowed(request.RequestUri?.Scheme, request.Headers))
                throw new AntiSSRFException("This request is not allowed per policy.");

            HttpResponseMessage response;
            try
            {
                response = base.Send(request, cancellationToken);
            }
            catch (HttpRequestException ex) when (ex.InnerException is not null)
            {
                throw ex.InnerException;
            }

            if (!AllowAutoRedirect)
                return response;

            Uri? redirectUri;
            for (int redirectCount = 0; redirectCount < MaxAutomaticRedirections && (redirectUri = GetRedirectUri(request.RequestUri!, response)) is not null; redirectCount++)
            {
                HttpStatusCode statusCode = response.StatusCode;
                response.Dispose();
                request.RequestUri = redirectUri;
                SetUpForRedirect(request, statusCode);

                if (!_policy.IsHttpRequestAllowed(request.RequestUri?.Scheme, request.Headers))
                    throw new AntiSSRFException("This request is not allowed per policy.");

                try
                {
                    response = base.Send(request, cancellationToken);
                }
                catch (HttpRequestException ex) when (ex.InnerException is not null)
                {
                    throw ex.InnerException;
                }
            }

            return response;
        }
#endif

        internal async Task<HttpResponseMessage> SendAsyncWrapper(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await SendAsync(request, cancellationToken);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!_policy.IsHttpRequestAllowed(request.RequestUri?.Scheme, request.Headers))
                throw new AntiSSRFException("This request is not allowed per policy.");

            HttpResponseMessage response;
            try
            {
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (HttpRequestException ex) when (ex.InnerException is not null)
            {
                throw ex.InnerException;
            }

            if (!AllowAutoRedirect)
                return response;

            Uri? redirectUri;
            for (int redirectCount = 0; redirectCount < MaxAutomaticRedirections && (redirectUri = GetRedirectUri(request.RequestUri!, response)) is not null; redirectCount++)
            {
                HttpStatusCode statusCode2 = response.StatusCode;
                response.Dispose();
                request.RequestUri = redirectUri;
                SetUpForRedirect(request, statusCode2);

                if (!_policy.IsHttpRequestAllowed(request.RequestUri?.Scheme, request.Headers))
                    throw new AntiSSRFException("This request is not allowed per policy.");
                
                try
                {
                    response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                }
                catch (HttpRequestException ex) when (ex.InnerException is not null)
                {
                    throw ex.InnerException;
                }
            }

            return response;
        }

        private readonly HttpStatusCode[] RedirectCodes = new HttpStatusCode[] { HttpStatusCode.Moved, HttpStatusCode.Found, HttpStatusCode.SeeOther, HttpStatusCode.TemporaryRedirect, HttpStatusCode.MultipleChoices, (HttpStatusCode)308 /*HttpStatusCode.PermanentRedirect*/ };

        private Uri? GetRedirectUri(Uri requestUri, HttpResponseMessage response)
        {
            if (!RedirectCodes.Contains(response.StatusCode))
                return null;

            Uri? location = response.Headers.Location;
            if (location is null)
                return null;

            // Ensure the redirect location is an absolute URI.
            if (!location.IsAbsoluteUri)
                location = new Uri(requestUri, location);

            // Per https://tools.ietf.org/html/rfc7231#section-7.1.2, a redirect location without a
            // fragment should inherit the fragment from the original URI.
            if (!string.IsNullOrEmpty(requestUri.Fragment) && string.IsNullOrEmpty(location.Fragment))
                location = new UriBuilder(location) { Fragment = requestUri.Fragment }.Uri;

            return location;
        }

        private static void SetUpForRedirect(HttpRequestMessage request, HttpStatusCode statusCode)
        {
            // Clear the authorization header.
            request.Headers.Authorization = null;

            // Switch to GET if required by the status code and original method
            if (
                (request.Method == HttpMethod.Post
                    && (statusCode == HttpStatusCode.Moved || statusCode == HttpStatusCode.Found || statusCode == HttpStatusCode.MultipleChoices))
                || (request.Method != HttpMethod.Get && request.Method != HttpMethod.Head
                    && statusCode == HttpStatusCode.SeeOther))
            {
                request.Method = HttpMethod.Get;
                request.Content = null;
                if (request.Headers.TransferEncodingChunked == true)
                    request.Headers.TransferEncodingChunked = false;
            }
        }

        // ----- Properties handled directly -----

        internal bool AllowAutoRedirect = true;
        internal int MaxAutomaticRedirections = 50;
    }
}
