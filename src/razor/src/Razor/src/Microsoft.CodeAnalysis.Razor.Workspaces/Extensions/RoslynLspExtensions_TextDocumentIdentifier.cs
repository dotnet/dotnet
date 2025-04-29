// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class RoslynLspExtensions
{
    /// <summary>
    /// Returns a copy of the passed in <see cref="TextDocumentIdentifier"/> with the passed in <see cref="Uri"/>.
    /// </summary>
    public static TextDocumentIdentifier WithUri(this TextDocumentIdentifier textDocumentIdentifier, Uri uri)
    {
        if (textDocumentIdentifier is VSTextDocumentIdentifier vsTdi)
        {
            return new VSTextDocumentIdentifier
            {
                Uri = uri,
                ProjectContext = vsTdi.ProjectContext
            };
        }

        return new TextDocumentIdentifier
        {
            Uri = uri
        };
    }

    public static RazorTextDocumentIdentifier ToRazorTextDocumentIdentifier(this TextDocumentIdentifier textDocumentIdentifier)
        => new RazorTextDocumentIdentifier(textDocumentIdentifier.Uri, (textDocumentIdentifier as VSTextDocumentIdentifier)?.ProjectContext?.Id);
}
