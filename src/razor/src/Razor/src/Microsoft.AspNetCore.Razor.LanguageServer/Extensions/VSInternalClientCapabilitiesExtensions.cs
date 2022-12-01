// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class VSInternalClientCapabilitiesExtensions
{
    internal static VSInternalClientCapabilities ToVSInternalClientCapabilities(this ClientCapabilities clientCapabilities)
    {
        if (clientCapabilities is VSInternalClientCapabilities vsInternalClientCapabilities)
        {
            return vsInternalClientCapabilities;
        }

        return new VSInternalClientCapabilities()
        {
            TextDocument = clientCapabilities.TextDocument,
            SupportsVisualStudioExtensions = false,
            Experimental = clientCapabilities.Experimental,
            Workspace = clientCapabilities.Workspace,
        };
    }
}
