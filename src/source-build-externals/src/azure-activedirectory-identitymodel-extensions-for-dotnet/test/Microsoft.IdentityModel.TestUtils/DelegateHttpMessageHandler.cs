// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// A <see cref="HttpMessageHandler"/> which delegates sending the request to a callback.
    /// </summary>
    public class DelegateHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateHttpMessageHandler"/>.
        /// </summary>
        /// <param name="callback">The callback to invoke when HTTP request is being executed.</param>
        public DelegateHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> callback)
        {
            _callback = callback;
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await _callback(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
