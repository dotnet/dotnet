// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
