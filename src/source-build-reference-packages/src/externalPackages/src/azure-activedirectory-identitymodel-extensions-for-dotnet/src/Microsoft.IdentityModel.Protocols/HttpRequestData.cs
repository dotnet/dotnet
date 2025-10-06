// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Structure that represents an incoming or an outgoing http request.
    /// </summary>
    public class HttpRequestData
    {
        private IDictionary<string, IEnumerable<string>> _headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);
        private X509Certificate2Collection _clientCertificates;

        /// <summary>
        /// Gets or sets the http request URI. 
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the http request method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the http request body.
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// Gets or sets the collection of http request headers.
        /// </summary>
        public IDictionary<string, IEnumerable<string>> Headers
        {
            get
            {
                return _headers;
            }
            set
            {
                _headers = value ?? throw new ArgumentNullException(nameof(Headers));
            }
        }

        /// <summary>
        /// Gets the certificate collection involved in authenticating the client against the server.
        /// </summary>
        public X509Certificate2Collection ClientCertificates => _clientCertificates ??
            Interlocked.CompareExchange(ref _clientCertificates, [], null) ??
            _clientCertificates;

        /// <summary>
        /// Gets or sets an <see cref="IDictionary{String, Object}"/> that enables custom extensibility scenarios.
        /// </summary>
        public IDictionary<string, object> PropertyBag { get; set; }

        /// <summary>
        /// A utility method that appends <paramref name="headers"/> to the <see cref="Headers"/>.
        /// </summary>
        /// <param name="headers">A collection of http request headers.</param>
        public void AppendHeaders(HttpHeaders headers)
        {
            if (headers == null)
                return;

            foreach (var header in headers)
            {
                if (Headers.ContainsKey(header.Key))
                    Headers[header.Key] = Headers[header.Key].Concat(header.Value);
                else
                    Headers.Add(header.Key, header.Value);
            }
        }
    }
}
