// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
