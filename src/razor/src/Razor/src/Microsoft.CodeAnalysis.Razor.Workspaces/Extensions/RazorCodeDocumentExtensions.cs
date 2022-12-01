// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;

internal static class RazorCodeDocumentExtensions
{
    private static readonly object s_sourceTextKey = new();
    private static readonly object s_cSharpSourceTextKey = new();
    private static readonly object s_htmlSourceTextKey = new();

    public static SourceText GetSourceText(this RazorCodeDocument document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var sourceTextObj = document.Items[s_sourceTextKey];
        if (sourceTextObj is null)
        {
            var source = document.Source;
            var charBuffer = new char[source.Length];
            source.CopyTo(0, charBuffer, 0, source.Length);
            var sourceText = SourceText.From(new string(charBuffer));
            document.Items[s_sourceTextKey] = sourceText;

            return sourceText;
        }

        return (SourceText)sourceTextObj;
    }

    public static SourceText GetCSharpSourceText(this RazorCodeDocument document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var sourceTextObj = document.Items[s_cSharpSourceTextKey];
        if (sourceTextObj is null)
        {
            var csharpDocument = document.GetCSharpDocument();
            var sourceText = SourceText.From(csharpDocument.GeneratedCode);
            document.Items[s_cSharpSourceTextKey] = sourceText;

            return sourceText;
        }

        return (SourceText)sourceTextObj;
    }

    public static SourceText GetHtmlSourceText(this RazorCodeDocument document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var sourceTextObj = document.Items[s_htmlSourceTextKey];
        if (sourceTextObj is null)
        {
            var htmlDocument = document.GetHtmlDocument();
            var sourceText = SourceText.From(htmlDocument.GeneratedHtml);
            document.Items[s_htmlSourceTextKey] = sourceText;

            return sourceText;
        }

        return (SourceText)sourceTextObj;
    }
}
