// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal sealed partial class RazorLanguageServerHost
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(RazorLanguageServerHost instance)
    {
        public RazorLanguageServer Server => instance._server;
    }
}
