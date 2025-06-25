// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    public static VSProjectContext? GetProjectContext(this TextDocumentIdentifier textDocumentIdentifier)
        => textDocumentIdentifier is VSTextDocumentIdentifier vsIdentifier
            ? vsIdentifier.ProjectContext
            : null;

    /// <summary>
    /// Returns a copy of the passed in <see cref="TextDocumentIdentifier"/> with the passed in <see cref="Uri"/>.
    /// </summary>
    public static TextDocumentIdentifier WithUri(this TextDocumentIdentifier textDocumentIdentifier, Uri uri)
    {
        var documentUri = new DocumentUri(uri);
        if (textDocumentIdentifier is VSTextDocumentIdentifier vsTdi)
        {
            return new VSTextDocumentIdentifier
            {
                DocumentUri = documentUri,
                ProjectContext = vsTdi.ProjectContext
            };
        }

        return new TextDocumentIdentifier
        {
            DocumentUri = documentUri
        };
    }

    public static RazorTextDocumentIdentifier ToRazorTextDocumentIdentifier(this TextDocumentIdentifier textDocumentIdentifier)
        => new RazorTextDocumentIdentifier(textDocumentIdentifier.DocumentUri.GetRequiredParsedUri(), (textDocumentIdentifier as VSTextDocumentIdentifier)?.ProjectContext?.Id);
}
