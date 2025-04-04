// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
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
