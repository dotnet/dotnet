// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;

namespace NuGet.Protocol.FuncTest.Helpers
{
    internal abstract class MockServerHttpClientHandler : HttpClientHandler
    {
        protected MockServerHttpClientHandler(Uri baseAddress)
        {
            var scheme = baseAddress.Scheme;
            if (scheme != "http" && scheme != "https")
            {
                throw new ArgumentException(message: "This mock is intended for HTTP mocking only", paramName: nameof(baseAddress));
            }

            if (!baseAddress.Host.EndsWith(".test"))
            {
                throw new ArgumentException(message: "All hostnames should end in .test to make it obvious it's not making real HTTP requests", paramName: nameof(baseAddress));
            }

            BaseAddress = baseAddress;
        }

        public Uri BaseAddress { get; }
    }
}
