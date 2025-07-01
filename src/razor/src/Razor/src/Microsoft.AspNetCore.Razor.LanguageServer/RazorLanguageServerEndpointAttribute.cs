// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.LanguageServer;

// Using inside the namespace to override the global using for LanguageServerConstants, which is what
// we want everywhere but this file.
using Microsoft.CommonLanguageServerProtocol.Framework;

internal class RazorLanguageServerEndpointAttribute : LanguageServerEndpointAttribute
{
    public RazorLanguageServerEndpointAttribute(string method)
        : base(method, LanguageServerConstants.DefaultLanguageName)
    {
    }
}
