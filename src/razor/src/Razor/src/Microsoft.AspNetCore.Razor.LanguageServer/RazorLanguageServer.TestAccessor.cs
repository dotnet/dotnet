// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class RazorLanguageServer
{
    internal new TestAccessor GetTestAccessor() => new(this);

    internal new readonly struct TestAccessor(RazorLanguageServer instance)
    {
        public AbstractHandlerProvider HandlerProvider => instance.HandlerProvider;

        public RazorRequestExecutionQueue GetRequestExecutionQueue()
        {
            return (RazorRequestExecutionQueue)instance.GetRequestExecutionQueue();
        }
    }
}
