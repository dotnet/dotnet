// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class SyntaxNodeExtensions
{
    internal static bool TryGetPreviousSibling(this SyntaxNode syntaxNode, [NotNullWhen(true)] out SyntaxNode? previousSibling)
    {
        var syntaxNodeParent = syntaxNode.Parent;
        if (syntaxNodeParent is null)
        {
            previousSibling = default;
            return false;
        }

        var nodes = syntaxNodeParent.ChildNodes();
        for (var i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] == syntaxNode)
            {
                if (i == 0)
                {
                    previousSibling = default;
                    return false;
                }
                else
                {
                    previousSibling = nodes[i - 1];
                    return true;
                }
            }
        }

        previousSibling = default;
        return false;
    }

    public static bool ContainsOnlyWhitespace(this SyntaxNode node, bool includingNewLines = true)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        var tokens = node.GetTokens();

        for (var i = 0; i < tokens.Count; i++)
        {
            var tokenKind = tokens[i].Kind;
            if (tokenKind != SyntaxKind.Whitespace && (tokenKind != SyntaxKind.NewLine || !includingNewLines))
            {
                return false;
            }
        }

        // All tokens were either whitespace or newlines.
        return true;
    }

    public static LinePositionSpan GetLinePositionSpan(this SyntaxNode node, RazorSourceDocument source)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var start = node.Position;
        var end = node.EndPosition;

        Debug.Assert(start <= source.Length && end <= source.Length, "Node position exceeds source length.");

        if (start == source.Length && node.FullWidth == 0)
        {
            // Marker symbol at the end of the document.
            var location = node.GetSourceLocation(source);
            var position = GetLinePosition(location);
            return new LinePositionSpan(position, position);
        }

        var startLocation = source.Lines.GetLocation(start);
        var endLocation = source.Lines.GetLocation(end);
        var startPosition = GetLinePosition(startLocation);
        var endPosition = GetLinePosition(endLocation);

        return new LinePositionSpan(startPosition, endPosition);

        static LinePosition GetLinePosition(SourceLocation location)
        {
            return new LinePosition(location.LineIndex, location.CharacterIndex);
        }
    }

    public static Range GetRange(this SyntaxNode node, RazorSourceDocument source)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var lineSpan = node.GetLinePositionSpan(source);
        var range = new Range
        {
            Start = new Position(lineSpan.Start.Line, lineSpan.Start.Character),
            End = new Position(lineSpan.End.Line, lineSpan.End.Character)
        };

        return range;
    }

    public static Range? GetRangeWithoutWhitespace(this SyntaxNode node, RazorSourceDocument source)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

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

        var range = new Range
        {
            Start = new Position(startPositionSpan.Start.Line, startPositionSpan.Start.Character),
            End = new Position(endPositionSpan.End.Line, endPositionSpan.End.Character)
        };

        return range;

        // This is needed because SyntaxToken positions taken from GetTokens
        // are relative to their parent node and not to the document.
        static LinePositionSpan GetLinePositionSpan(SyntaxNode? node, RazorSourceDocument source, int parentStart)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var start = node.Position + parentStart;
            var end = node.EndPosition + parentStart;

            if (start == source.Length && node.FullWidth == 0)
            {
                // Marker symbol at the end of the document.
                var location = node.GetSourceLocation(source);
                var position = GetLinePosition(location);
                return new LinePositionSpan(position, position);
            }

            var startLocation = source.Lines.GetLocation(start);
            var endLocation = source.Lines.GetLocation(end);
            var startPosition = GetLinePosition(startLocation);
            var endPosition = GetLinePosition(endLocation);

            return new LinePositionSpan(startPosition, endPosition);

            static LinePosition GetLinePosition(SourceLocation location)
            {
                return new LinePosition(location.LineIndex, location.CharacterIndex);
            }
        }
    }

    public static int GetLeadingWhitespaceLength(this SyntaxNode node, FormattingContext context)
    {
        var tokens = node.GetTokens();
        var whitespaceLength = 0;

        foreach (var token in tokens)
        {
            if (token.IsWhitespace())
            {
                if (token.Kind == SyntaxKind.NewLine)
                {
                    // We need to reset when we move to a new line.
                    whitespaceLength = 0;
                }
                else if (token.IsSpace())
                {
                    whitespaceLength++;
                }
                else if (token.IsTab())
                {
                    whitespaceLength += (int)context.Options.TabSize;
                }
            }
            else
            {
                break;
            }
        }

        return whitespaceLength;
    }

    public static int GetTrailingWhitespaceLength(this SyntaxNode node, FormattingContext context)
    {
        var tokens = node.GetTokens();
        var whitespaceLength = 0;

        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            var token = tokens[i];
            if (token.IsWhitespace())
            {
                if (token.Kind == SyntaxKind.NewLine)
                {
                    whitespaceLength = 0;
                }
                else if (token.IsSpace())
                {
                    whitespaceLength++;
                }
                else if (token.IsTab())
                {
                    whitespaceLength += (int)context.Options.TabSize;
                }
            }
            else
            {
                break;
            }
        }

        return whitespaceLength;
    }
}
