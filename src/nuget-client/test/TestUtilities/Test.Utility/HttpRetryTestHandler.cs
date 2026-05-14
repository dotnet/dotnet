// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Utility
{
    public class HttpRetryTestHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

        public HttpRetryTestHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
        {
            _handler = (request, token) => Task.FromResult(handler(request));
        }

        public HttpRetryTestHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
        {
            _handler = handler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return await _handler(request, cancellationToken);
        }
    }
}
