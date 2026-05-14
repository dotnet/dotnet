// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.RemoteExecutor;

public sealed class RemoteExecutionTimeoutException : Exception
{
    public RemoteExecutionTimeoutException(string message)
        : base(message)
    {
    }
}