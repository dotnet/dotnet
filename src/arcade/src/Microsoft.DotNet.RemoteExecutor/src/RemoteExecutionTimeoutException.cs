
using System;

namespace Microsoft.DotNet.RemoteExecutor;

public sealed class RemoteExecutionTimeoutException : Exception
{
    public RemoteExecutionTimeoutException(string message)
        : base(message)
    {
    }
}