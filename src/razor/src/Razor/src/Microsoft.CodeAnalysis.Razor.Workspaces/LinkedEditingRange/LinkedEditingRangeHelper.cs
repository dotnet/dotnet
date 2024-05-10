// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

using RazorSyntaxToken = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxToken;

namespace Microsoft.CodeAnalysis.Razor.LinkedEditingRange;

internal static class LinkedEditingRangeHelper
{
    // The regex below excludes characters that can never be valid in a TagHelper name.
    // This is loosely based off logic from the Razor compiler:
    // https://github.com/dotnet/aspnetcore/blob/9da42b9fab4c61fe46627ac0c6877905ec845d5a/src/Razor/Microsoft.AspNetCore.Razor.Language/src/Legacy/HtmlTokenizer.cs
    public static readonly string WordPattern = @"!?[^ <>!\/\?\[\]=""\\@" + Environment.NewLine + "]+";

    public static LinePositionSpan[]? GetLinkedSpans(LinePosition linePosition, RazorCodeDocument codeDocument, ILogger logger)
    {
        if (GetSourceLocation(linePosition, codeDocument, logger) is not { } validLocation)
        {
            return null;
        }

        var syntaxTree = codeDocument.GetSyntaxTree();

        // We only care if the user is within a TagHelper or HTML tag with a valid start and end tag.
        if (TryGetNearestMarkupNameTokens(syntaxTree, validLocation, out var startTagNameToken, out var endTagNameToken) &&
            (startTagNameToken.Span.Contains(validLocation.AbsoluteIndex) || endTagNameToken.Span.Contains(validLocation.AbsoluteIndex) ||
            startTagNameToken.Span.End == validLocation.AbsoluteIndex || endTagNameToken.Span.End == validLocation.AbsoluteIndex))
        {
            var startSpan = startTagNameToken.GetLinePositionSpan(codeDocument.Source);
            var endSpan = endTagNameToken.GetLinePositionSpan(codeDocument.Source);

            return [startSpan, endSpan];
        }

        return null;
    }

    private static SourceLocation? GetSourceLocation(
        LinePosition linePosition,
        RazorCodeDocument codeDocument,
        ILogger logger)
    {
        var sourceText = codeDocument.GetSourceText();

        if (linePosition.ToPosition().TryGetSourceLocation(sourceText, logger, out var location))
        {
            return location;
        }
        else
        {
            return null;
        }
    }

    private static bool TryGetNearestMarkupNameTokens(
        RazorSyntaxTree syntaxTree,
        SourceLocation location,
        [NotNullWhen(true)] out RazorSyntaxToken? startTagNameToken,
        [NotNullWhen(true)] out RazorSyntaxToken? endTagNameToken)
    {
        var owner = syntaxTree.Root.FindInnermostNode(location.AbsoluteIndex);
        var element = owner?.FirstAncestorOrSelf<MarkupSyntaxNode>(
            a => a.Kind is SyntaxKind.MarkupTagHelperElement || a.Kind is SyntaxKind.MarkupElement);

        if (element is null)
        {
            startTagNameToken = null;
            endTagNameToken = null;
            return false;
        }

        switch (element)
        {
            // Tag helper
            case MarkupTagHelperElementSyntax markupTagHelperElement:
                startTagNameToken = markupTagHelperElement.StartTag?.Name;
                endTagNameToken = markupTagHelperElement.EndTag?.Name;
                return startTagNameToken is not null && endTagNameToken is not null;
            // HTML
            case MarkupElementSyntax markupElement:
                startTagNameToken = markupElement.StartTag?.Name;
                endTagNameToken = markupElement.EndTag?.Name;
                return startTagNameToken is not null && endTagNameToken is not null;
            default:
                throw new InvalidOperationException("Element is expected to be a MarkupTagHelperElement or MarkupElement.");
        }
    }
}
