//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Structure that represents an incoming or an outgoing http request.
    /// </summary>
    public class HttpRequestData
    {
        private IDictionary<string, IEnumerable<string>> _headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);

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
