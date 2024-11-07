// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Hover;

internal readonly record struct HoverDisplayOptions(MarkupKind MarkupKind, bool SupportsVisualStudioExtensions)
{
    public static HoverDisplayOptions From(ClientCapabilities clientCapabilities)
    {
        var markupKind = clientCapabilities.GetMarkupKind();
        var supportsVisualStudioExtensions = clientCapabilities.SupportsVisualStudioExtensions();

        return new(markupKind, supportsVisualStudioExtensions);
    }
}
