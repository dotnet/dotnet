// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace Test.Utility
{
    public class TestHttpHandlerProvider : ResourceProvider
    {
        private Func<HttpClientHandler> _messageHandlerFactory;

        public TestHttpHandlerProvider(Func<HttpClientHandler> messageHandlerFactory)
            : base(typeof(HttpHandlerResource), "testhandler", NuGetResourceProviderPositions.First)
        {
            _messageHandlerFactory = messageHandlerFactory;
        }

        public override Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            var result = new Tuple<bool, INuGetResource>(true, new TestHttpHandler(_messageHandlerFactory()));
            return Task.FromResult(result);
        }
    }
}
