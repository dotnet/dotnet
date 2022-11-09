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
            if (!_firstSend  || _errorHttpResponseMessageOnFirstSend == null)
            {
                return await Task.FromResult(_httpResponseMessage).ConfigureAwait(false);
            }

            _firstSend = false;
            return await Task.FromResult(_errorHttpResponseMessageOnFirstSend).ConfigureAwait(false);
        }
    }
}
