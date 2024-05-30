// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal sealed partial class RazorLanguageServerHost
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(RazorLanguageServerHost instance)
    {
        public RazorLanguageServer Server => instance._server;
    }
}
