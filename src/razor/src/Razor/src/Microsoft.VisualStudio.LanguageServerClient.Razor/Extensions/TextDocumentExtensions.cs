// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

internal static class TextDocumentExtensions
{
    /// <summary>
    /// Mutates the <see cref="TextDocumentIdentifier"/> by changing the <see cref="Uri"/>. Returns it again for convenience.
    /// </summary>
    public static TextDocumentIdentifier WithUri(this TextDocumentIdentifier textDocumentIdentifier, Uri uri)
    {
        textDocumentIdentifier.Uri = uri;
        return textDocumentIdentifier;
    }

    public static RazorTextDocumentIdentifier ToRazorTextDocumentIdentifier(this TextDocumentIdentifier textDocumentIdentifier)
        => new RazorTextDocumentIdentifier(textDocumentIdentifier.Uri, (textDocumentIdentifier as VSTextDocumentIdentifier)?.ProjectContext?.Id);
}
