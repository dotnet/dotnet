// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static class ClientCapabilitiesExtensions
{
    public static MarkupKind GetMarkupKind(this ClientCapabilities clientCapabilities)
    {
        // If MarkDown is supported, we'll use that.
        if (clientCapabilities.TextDocument?.Hover?.ContentFormat is MarkupKind[] contentFormat &&
            Array.IndexOf(contentFormat, MarkupKind.Markdown) >= 0)
        {
            return MarkupKind.Markdown;
        }

        return MarkupKind.PlainText;
    }

    public static bool SupportsMarkdown(this ClientCapabilities clientCapabilities)
    {
        return clientCapabilities.GetMarkupKind() == MarkupKind.Markdown;
    }

    public static bool SupportsVisualStudioExtensions(this ClientCapabilities clientCapabilities)
    {
        return (clientCapabilities as VSInternalClientCapabilities)?.SupportsVisualStudioExtensions ?? false;
    }
}
