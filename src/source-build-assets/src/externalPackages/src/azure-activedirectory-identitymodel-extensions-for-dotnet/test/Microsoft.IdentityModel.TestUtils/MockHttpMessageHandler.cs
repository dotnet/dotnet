// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// A mock <see cref="HttpMessageHandler"/>.
    /// </summary>
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _httpResponseMessage;
        private HttpResponseMessage _errorHttpResponseMessageOnFirstSend;
        private bool _firstSend = true;

        /// <summary>
        /// Creates a <see cref="MockHttpMessageHandler"/> that always returns <paramref name="httpResponseMessage"/>.
        /// </summary>
        /// <param name="httpResponseMessage">A <see cref="MockHttpMessageHandler"/> that always returns <paramref name="httpResponseMessage"/>.</param>
        public MockHttpMessageHandler(HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
        }

        /// <summary>
        /// Creates a <see cref="MockHttpMessageHandler"/> that returns <paramref name="failureMessage"/> on the first
        /// call to <see cref="SendAsync(HttpRequestMessage, CancellationToken)"/> and <paramref name="httpResponseMessage"/> on
        /// all other subsequent calls.
        /// </summary>
        /// <param name="httpResponseMessage">The <see cref="HttpResponseMessage"/> returned on every call to <see cref="SendAsync(HttpRequestMessage, CancellationToken)"/>
        /// except the first call.</param>
        /// <param name="failureMessage">The <see cref="HttpResponseMessage"/> returned on the first call to <see cref="SendAsync(HttpRequestMessage, CancellationToken)"/></param>
        public MockHttpMessageHandler(HttpResponseMessage httpResponseMessage, HttpResponseMessage failureMessage)
        {
            _httpResponseMessage = httpResponseMessage;
            _errorHttpResponseMessageOnFirstSend = failureMessage;
        }

        /// <summary>
        /// Mocks <see cref="HttpMessageHandler.SendAsync(HttpRequestMessage, CancellationToken)"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!_firstSend || _errorHttpResponseMessageOnFirstSend == null)
            {
                return await Task.FromResult(_httpResponseMessage).ConfigureAwait(false);
            }

            _firstSend = false;
            return await Task.FromResult(_errorHttpResponseMessageOnFirstSend).ConfigureAwait(false);
        }
    }
}
