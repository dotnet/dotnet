// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit.Sdk;

namespace Microsoft.DotNet.RemoteExecutor
{
    public sealed class RemoteExecutionException : XunitException
    {
        public RemoteExecutionException(string stackTrace)
            : base("Remote process failed with an unhandled exception.", stackTrace)
        {
        }
    }
}
