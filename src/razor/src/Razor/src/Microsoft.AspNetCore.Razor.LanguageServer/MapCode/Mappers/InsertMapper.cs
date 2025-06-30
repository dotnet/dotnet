// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.MapCode.Mappers;

internal static class InsertMapper
{
    public static int? GetInsertionPoint(
        SyntaxNode documentRoot,
        SourceText sourceText,
        LspLocation focusArea)
    {
        // If there's an specific focus area, or caret provided, we should try to insert as close as possible.
        // As long as the focused area is not empty.
        if (TryGetFocusedInsertionPoint(documentRoot, sourceText, focusArea, out var focusedInsertionPoint))
        {
            return focusedInsertionPoint;
        }

        // Fallback: Attempt to infer the insertion point without a valid focus area.
        if (TryGetDefaultInsertionPoint(documentRoot, out var defaultInsertionPoint))
        {
            return defaultInsertionPoint;
        }

        return null;
    }

    private static bool TryGetFocusedInsertionPoint(
        SyntaxNode documentRoot,
        SourceText sourceText,
        LspLocation focusArea,
        out int insertionPoint)
    {
        // If there's an specific focus area, or caret provided, we should try to insert as close as possible.

        // We currently only support 0-length focus areas.
        if (focusArea.Range.Start != focusArea.Range.End)
        {
            insertionPoint = 0;
            return false;
        }

        // Verify that the focus area is within the document.
        if (!sourceText.IsValidPosition(focusArea.Range.Start))
        {
            insertionPoint = 0;
            return false;
        }

        // Ensure we don't insert in the middle of a node.
        var node = documentRoot.FindNode(sourceText.GetTextSpan(focusArea.Range), includeWhitespace: true);
        if (node is null)
        {
            insertionPoint = 0;
            return false;
        }

        // If the node is a MarkupTextLiteral, we can probably insert in the middle as long as we're
        // on a blank line.
        if (node is MarkupTextLiteralSyntax)
        {
            var line = sourceText.Lines[focusArea.Range.Start.Line];
            if (line.GetFirstNonWhitespaceOffset() is null)
            {
                insertionPoint = sourceText.GetTextSpan(focusArea.Range).Start;
                return true;
            }
        }

        insertionPoint = node.Span.End;
        return true;
    }

    private static bool TryGetDefaultInsertionPoint(SyntaxNode documentRoot, out int insertionPoint)
    {
        // Our default insertion point is at the end of the document.
        insertionPoint = documentRoot.EndPosition;
        return true;
    }
}
