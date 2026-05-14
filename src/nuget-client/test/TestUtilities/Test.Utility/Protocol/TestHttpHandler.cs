// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net.Http;
using NuGet.Protocol.Core.Types;

namespace Test.Utility
{
    public class TestHttpHandler : HttpHandlerResource
    {
        private HttpClientHandler _messageHandler;

        public TestHttpHandler(HttpClientHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public override HttpClientHandler ClientHandler
        {
            get { return _messageHandler; }
        }

        public override HttpMessageHandler MessageHandler
        {
            get
            {
                return _messageHandler;
            }
        }
    }
}
