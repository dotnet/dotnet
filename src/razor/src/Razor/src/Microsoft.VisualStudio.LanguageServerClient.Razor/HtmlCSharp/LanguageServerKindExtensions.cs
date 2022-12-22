// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

internal static class LanguageServerKindExtensions
{
    public static string ToLanguageServerName(this LanguageServerKind languageServerKind)
    {
        return languageServerKind switch
        {
            LanguageServerKind.CSharp => RazorLSPConstants.RazorCSharpLanguageServerName,
            LanguageServerKind.Html => RazorLSPConstants.HtmlLanguageServerName,
            LanguageServerKind.Razor => RazorLSPConstants.RazorLanguageServerName,
            _ => throw new System.NotImplementedException(),
        };
    }

    public static string ToContentType(this LanguageServerKind languageServerKind)
    {
        return languageServerKind switch
        {
            LanguageServerKind.CSharp => RazorLSPConstants.CSharpContentTypeName,
            LanguageServerKind.Html => RazorLSPConstants.HtmlLSPDelegationContentTypeName,
            _ => RazorConstants.RazorLSPContentTypeName,
        };
    }

    public static ITextBuffer GetTextBuffer(this LanguageServerKind languageServerKind, LSPDocumentSnapshot documentSnapshot)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        switch (languageServerKind)
        {
            case LanguageServerKind.CSharp:
                if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var csharpVirtualDocumentSnapshot))
                {
                    throw new InvalidOperationException("Could not extract C# virtual document, this is unexpected");
                }

                return csharpVirtualDocumentSnapshot.Snapshot.TextBuffer;
            case LanguageServerKind.Html:
                if (!documentSnapshot.TryGetVirtualDocument<HtmlVirtualDocumentSnapshot>(out var htmlVirtualDocumentSnapshot))
                {
                    throw new InvalidOperationException("Could not extract HTML virtual document, this is unexpected");
                }

                return htmlVirtualDocumentSnapshot.Snapshot.TextBuffer;
            case LanguageServerKind.Razor:
                return documentSnapshot.Snapshot.TextBuffer;
            default:
                throw new InvalidOperationException("Unknown language server kind.");
        }
    }
}
