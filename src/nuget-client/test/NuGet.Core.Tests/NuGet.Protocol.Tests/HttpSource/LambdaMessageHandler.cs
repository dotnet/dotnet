// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Tests
{
    internal class LambdaMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _delegate;

        public LambdaMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> @delegate)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            _delegate = request => Task.FromResult(@delegate(request));
        }

        public LambdaMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> @delegate)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            _delegate = @delegate;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _delegate(request);
        }
    }
}
