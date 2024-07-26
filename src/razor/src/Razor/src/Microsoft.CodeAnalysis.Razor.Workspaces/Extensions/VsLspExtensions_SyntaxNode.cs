// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    public static Range GetRange(this SyntaxNode node, RazorSourceDocument source)
    {
        var linePositionSpan = node.GetLinePositionSpan(source);

        return VsLspFactory.CreateRange(linePositionSpan);
    }

    public static Range? GetRangeWithoutWhitespace(this SyntaxNode node, RazorSourceDocument source)
    {
        var tokens = node.GetTokens();

        SyntaxToken? firstToken = null;
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (!token.IsWhitespace())
            {
                firstToken = token;
                break;
            }
        }

        SyntaxToken? lastToken = null;
        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            var token = tokens[i];
            if (!token.IsWhitespace())
            {
                lastToken = token;
                break;
            }
        }

        if (firstToken is null && lastToken is null)
        {
            return null;
        }

        var startPositionSpan = GetLinePositionSpan(firstToken, source, node.SpanStart);
        var endPositionSpan = GetLinePositionSpan(lastToken, source, node.SpanStart);

        return VsLspFactory.CreateRange(startPositionSpan.Start, endPositionSpan.End);

        // This is needed because SyntaxToken positions taken from GetTokens
        // are relative to their parent node and not to the document.
        static LinePositionSpan GetLinePositionSpan(SyntaxNode? node, RazorSourceDocument source, int parentStart)
        {
            ArgHelper.ThrowIfNull(node);
            ArgHelper.ThrowIfNull(source);

            var sourceText = source.Text;

            var start = node.Position + parentStart;
            var end = node.EndPosition + parentStart;

            if (start == sourceText.Length && node.FullWidth == 0)
            {
                // Marker symbol at the end of the document.
                var location = node.GetSourceLocation(source);
                var position = location.ToLinePosition();
                return new LinePositionSpan(position, position);
            }

            return sourceText.GetLinePositionSpan(start, end);
        }
    }
}
