// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace NuGet.Test.Server
{
    public class UnknownDnsServer : ITestServer
    {
        public async Task<T> ExecuteAsync<T>(Func<string, Task<T>> action)
        {
            if (Mode != TestServerMode.NameResolutionFailure)
            {
                throw new InvalidOperationException($"The mode {Mode} is not supported by this server.");
            }

            var address = $"http://{Guid.NewGuid()}.org/index.json";
            return await action(address);
        }

        public TestServerMode Mode { get; set; }
    }
}
