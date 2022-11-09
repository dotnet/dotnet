// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;

namespace NuGet.Protocol.Tests
{
    internal class TestProxy : IWebProxy
    {
        private readonly Uri _proxyAddress;

        public TestProxy(Uri proxyAddress)
        {
            _proxyAddress = proxyAddress;
        }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination) => _proxyAddress;

        public bool IsBypassed(Uri host) => false;
    }
}
