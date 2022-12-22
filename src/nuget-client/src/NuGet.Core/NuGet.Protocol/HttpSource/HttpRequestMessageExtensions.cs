// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NuGet.Protocol
{
    public static class HttpRequestMessageExtensions
    {
        private const string NuGetConfigurationKey = "NuGet_Configuration";

        /// <summary>
        /// Clones an <see cref="HttpRequestMessage" /> request.
        /// </summary>
        internal static HttpRequestMessage Clone(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version
            };

            if (request.Content != null)
            {
                clone.Content = new HttpContentWrapper(request.Content);
            }

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

#if NET5_0_OR_GREATER
            var clonedOptions = (IDictionary<string, object>)clone.Options;
            foreach (var option in request.Options)
            {
                clonedOptions.Add(option.Key, option.Value);
            }
#else
            foreach (var property in request.Properties)
            {
                clone.Properties.Add(property);
            }
#endif
            return clone;
        }

        // Wraps HttpContent but does not dispose it for cloning
        internal class HttpContentWrapper : HttpContent
        {
            private HttpContent _httpContent;

            public HttpContentWrapper(HttpContent httpContent)
            {
                _httpContent = httpContent;

                foreach (var header in _httpContent.Headers)
                {
                    Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return _httpContent.CopyToAsync(stream, context);
            }

            protected override bool TryComputeLength(out long length)
            {
                var contentLength = _httpContent.Headers.ContentLength;
                length = contentLength ?? 0;
                return contentLength != null;
            }

            protected override void Dispose(bool disposing)
            {
                _httpContent = null; // do not dispose!
            }
        }

        /// <summary>
        /// Retrieves the HTTP request configuration instance attached to the given message as custom property.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <returns>Configuration instance if exists, or a default instance otherwise.</returns>
        public static HttpRequestMessageConfiguration GetOrCreateConfiguration(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var foundInstance = request.GetProperty<HttpRequestMessageConfiguration>(NuGetConfigurationKey);

            return foundInstance ?? HttpRequestMessageConfiguration.Default;
        }

        /// <summary>
        /// Attaches an HTTP request configuration instance to the given message as custom property.
        /// If the configuration has already been set on the request message, the old configuration
        /// is replaced.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <param name="configuration">An HTTP request message configuration instance.</param>
        public static void SetConfiguration(this HttpRequestMessage request, HttpRequestMessageConfiguration configuration)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

#if NET5_0_OR_GREATER
            request.Options.Set(new HttpRequestOptionsKey<HttpRequestMessageConfiguration>(NuGetConfigurationKey), configuration);
#else
            request.Properties[NuGetConfigurationKey] = configuration;
#endif
        }

        private static T GetProperty<T>(this HttpRequestMessage request, string key)
        {

#if NET5_0_OR_GREATER
            if (request.Options.TryGetValue<T>(new HttpRequestOptionsKey<T>(key), out T result))
#else
            object result;
            if (request.Properties.TryGetValue(key, out result) && result is T)
#endif
            {
                return (T)result;
            }

            return default(T);
        }
    }
}
