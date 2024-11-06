// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Hover;

internal readonly record struct HoverDisplayOptions(MarkupKind MarkupKind, bool SupportsVisualStudioExtensions)
{
    public static HoverDisplayOptions From(ClientCapabilities clientCapabilities)
    {
        var markupKind = MarkupKind.PlainText;

        // If MarkDown is supported, we'll use that.
        if (clientCapabilities.TextDocument?.Hover?.ContentFormat is MarkupKind[] contentFormat &&
            Array.IndexOf(contentFormat, MarkupKind.Markdown) >= 0)
        {
            markupKind = MarkupKind.Markdown;
        }

        var supportsVisualStudioExtensions = (clientCapabilities as VSInternalClientCapabilities)?.SupportsVisualStudioExtensions ?? false;

        return new(markupKind, supportsVisualStudioExtensions);
    }
}
