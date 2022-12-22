// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace NuGet.Test.Server
{
    public enum TestServerMode
    {
        ConnectFailure,
        ServerProtocolViolation,
        NameResolutionFailure,
        SlowResponseBody
    }

    public interface ITestServer
    {
        Task<T> ExecuteAsync<T>(Func<string, Task<T>> action);

        TestServerMode Mode { get; set; }
    }
}
