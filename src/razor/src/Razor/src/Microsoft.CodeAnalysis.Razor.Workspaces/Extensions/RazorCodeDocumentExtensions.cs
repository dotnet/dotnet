// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class RazorCodeDocumentExtensions
{
    private static readonly object s_csharpSourceTextKey = new();
    private static readonly object s_htmlSourceTextKey = new();

    public static SourceText GetCSharpSourceText(this RazorCodeDocument document)
    {
        if (!document.Items.TryGetValue(s_csharpSourceTextKey, out SourceText? sourceText))
        {
            var csharpDocument = document.GetCSharpDocument();
            sourceText = SourceText.From(csharpDocument.GeneratedCode);
            document.Items[s_csharpSourceTextKey] = sourceText;

            return sourceText;
        }

        return sourceText.AssumeNotNull();
    }

    public static SourceText GetHtmlSourceText(this RazorCodeDocument document)
    {
        if (!document.Items.TryGetValue(s_htmlSourceTextKey, out SourceText? sourceText))
        {
            var htmlDocument = document.GetHtmlDocument();
            sourceText = SourceText.From(htmlDocument.GeneratedCode);
            document.Items[s_htmlSourceTextKey] = sourceText;

            return sourceText;
        }

        return sourceText.AssumeNotNull();
    }

    public static bool TryGetGeneratedDocument(
        this RazorCodeDocument codeDocument,
        Uri generatedDocumentUri,
        IFilePathService filePathService,
        [NotNullWhen(true)] out IRazorGeneratedDocument? generatedDocument)
    {
        if (filePathService.IsVirtualCSharpFile(generatedDocumentUri))
        {
            generatedDocument = codeDocument.GetCSharpDocument();
            return true;
        }

        if (filePathService.IsVirtualHtmlFile(generatedDocumentUri))
        {
            generatedDocument = codeDocument.GetHtmlDocument();
            return true;
        }

        generatedDocument = null;
        return false;
    }

    public static SourceText GetGeneratedSourceText(this RazorCodeDocument document, IRazorGeneratedDocument generatedDocument)
        => generatedDocument switch
        {
            RazorCSharpDocument => document.GetCSharpSourceText(),
            RazorHtmlDocument => document.GetHtmlSourceText(),
            _ => ThrowHelper.ThrowInvalidOperationException<SourceText>("Unknown generated document type"),
        };

    public static IRazorGeneratedDocument GetGeneratedDocument(this RazorCodeDocument document, RazorLanguageKind languageKind)
        => languageKind switch
        {
            RazorLanguageKind.CSharp => document.GetCSharpDocument(),
            RazorLanguageKind.Html => document.GetHtmlDocument(),
            _ => ThrowHelper.ThrowInvalidOperationException<IRazorGeneratedDocument>($"Unexpected language kind: {languageKind}"),
        };

    public static bool TryGetMinimalCSharpRange(this RazorCodeDocument codeDocument, LinePositionSpan razorRange, out LinePositionSpan csharpRange)
    {
        SourceSpan? minGeneratedSpan = null;
        SourceSpan? maxGeneratedSpan = null;

        var sourceText = codeDocument.Source.Text;
        var textSpan = sourceText.GetTextSpan(razorRange);
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
            var startRange = csharpSourceText.GetLinePositionSpan(minGeneratedSpan.Value);
            var endRange = csharpSourceText.GetLinePositionSpan(maxGeneratedSpan.Value);

            csharpRange = new LinePositionSpan(startRange.Start, endRange.End);
            Debug.Assert(csharpRange.Start.CompareTo(csharpRange.End) <= 0, "Range.Start should not be larger than Range.End");

            return true;
        }

        csharpRange = default;
        return false;
    }

    public static bool ComponentNamespaceMatches(this RazorCodeDocument razorCodeDocument, string fullyQualifiedNamespace)
    {
        var namespaceNode = (NamespaceDeclarationIntermediateNode)razorCodeDocument
            .GetDocumentIntermediateNode()
            .FindDescendantNodes<IntermediateNode>()
            .First(static n => n is NamespaceDeclarationIntermediateNode);

        return namespaceNode.Content == fullyQualifiedNamespace;
    }
}
