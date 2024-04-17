// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal static class RazorCodeDocumentExtensions
{
    private static readonly object s_cSharpSourceTextKey = new();
    private static readonly object s_htmlSourceTextKey = new();

    public static SourceText GetSourceText(this RazorCodeDocument document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return document.Source.Text;
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
            var sourceText = SourceText.From(htmlDocument.GeneratedCode);
            document.Items[s_htmlSourceTextKey] = sourceText;

            return sourceText;
        }

        return (SourceText)sourceTextObj;
    }

    public static SourceText GetGeneratedSourceText(this RazorCodeDocument document, IRazorGeneratedDocument generatedDocument)
    {
        if (generatedDocument is RazorCSharpDocument)
        {
            return GetCSharpSourceText(document);
        }
        else if (generatedDocument is RazorHtmlDocument)
        {
            return GetHtmlSourceText(document);
        }

        throw new InvalidOperationException("Unknown generated document type");
    }

    public static IRazorGeneratedDocument GetGeneratedDocument(this RazorCodeDocument document, RazorLanguageKind languageKind)
      => languageKind switch
      {
          RazorLanguageKind.CSharp => document.GetCSharpDocument(),
          RazorLanguageKind.Html => document.GetHtmlDocument(),
          _ => throw new System.InvalidOperationException(),
      };

    public static bool TryGetMinimalCSharpRange(this RazorCodeDocument codeDocument, LinePositionSpan razorRange, out LinePositionSpan csharpRange)
    {
        SourceSpan? minGeneratedSpan = null;
        SourceSpan? maxGeneratedSpan = null;

        var sourceText = codeDocument.GetSourceText();
        var textSpan = razorRange.ToTextSpan(sourceText);
        var csharpDoc = codeDocument.GetCSharpDocument();

        // We want to find the min and max C# source mapping that corresponds with our Razor range.
        foreach (var mapping in csharpDoc.SourceMappings)
        {
            var mappedTextSpan = mapping.OriginalSpan.AsTextSpan();

            if (textSpan.OverlapsWith(mappedTextSpan))
            {
                if (minGeneratedSpan is null || mapping.GeneratedSpan.AbsoluteIndex < minGeneratedSpan.Value.AbsoluteIndex)
                {
                    minGeneratedSpan = mapping.GeneratedSpan;
                }

                var mappingEndIndex = mapping.GeneratedSpan.AbsoluteIndex + mapping.GeneratedSpan.Length;
                if (maxGeneratedSpan is null || mappingEndIndex > maxGeneratedSpan.Value.AbsoluteIndex + maxGeneratedSpan.Value.Length)
                {
                    maxGeneratedSpan = mapping.GeneratedSpan;
                }
            }
        }

        // Create a new projected range based on our calculated min/max source spans.
        if (minGeneratedSpan is not null && maxGeneratedSpan is not null)
        {
            var csharpSourceText = codeDocument.GetCSharpSourceText();
            var startRange = minGeneratedSpan.Value.ToLinePositionSpan(csharpSourceText);
            var endRange = maxGeneratedSpan.Value.ToLinePositionSpan(csharpSourceText);

            csharpRange = new LinePositionSpan(startRange.Start, endRange.End);
            Debug.Assert(csharpRange.Start.CompareTo(csharpRange.End) <= 0, "Range.Start should not be larger than Range.End");

            return true;
        }

        csharpRange = default;
        return false;
    }
}
