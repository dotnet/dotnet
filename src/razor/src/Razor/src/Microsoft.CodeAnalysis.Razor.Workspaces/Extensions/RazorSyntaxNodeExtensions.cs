// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class RazorSyntaxNodeExtensions
{
    internal static bool IsUsingDirective(this SyntaxNode node, out SyntaxList<SyntaxNode> children)
    {
        // Using directives are weird, because the directive keyword ("using") is part of the C# statement it represents
        if (node is RazorDirectiveSyntax { DirectiveDescriptor: null, Body: RazorDirectiveBodySyntax body } &&
            body.Keyword is CSharpStatementLiteralSyntax { LiteralTokens: { Count: > 0 } literalTokens })
        {
            if (literalTokens[0] is { Kind: SyntaxKind.Keyword, Content: "using" })
            {
                children = literalTokens;
                return true;
            }
        }

        children = default;
        return false;
    }

    /// <summary>
    /// Walks up the tree through the <paramref name="owner"/>'s parents to find the outermost node that starts at the same position.
    /// </summary>
    internal static SyntaxNode? GetOutermostNode(this SyntaxNode owner)
    {
        var node = owner.Parent;
        if (node is null)
        {
            return owner;
        }

        var lastNode = node;
        while (node.SpanStart == owner.SpanStart)
        {
            lastNode = node;
            node = node.Parent;
            if (node is null)
            {
                break;
            }
        }

        return lastNode;
    }

    internal static bool TryGetPreviousSibling(this SyntaxNode node, [NotNullWhen(true)] out SyntaxNode? previousSibling)
    {
        previousSibling = null;

        var parent = node.Parent;
        if (parent is null)
        {
            return false;
        }

        foreach (var child in parent.ChildNodes())
        {
            if (ReferenceEquals(child, node))
            {
                return previousSibling is not null;
            }

            previousSibling = child;
        }

        Debug.Fail("How can we iterate node.Parent.ChildNodes() and not find node again?");

        previousSibling = null;
        return false;
    }

    public static bool ContainsOnlyWhitespace(this SyntaxNode node, bool includingNewLines = true)
    {
        foreach (var token in node.GetTokens())
        {
            var tokenKind = token.Kind;
            if (tokenKind != SyntaxKind.Whitespace && (!includingNewLines || tokenKind != SyntaxKind.NewLine))
            {
                return false;
            }
        }

        // All tokens were either whitespace or new-lines.
        return true;
    }

    public static LinePositionSpan GetLinePositionSpan(this SyntaxNode node, RazorSourceDocument sourceDocument)
    {
        var start = node.Position;
        var end = node.EndPosition;
        var sourceText = sourceDocument.Text;

        Debug.Assert(start <= sourceText.Length && end <= sourceText.Length, "Node position exceeds source length.");

        if (start == sourceText.Length && node.FullWidth == 0)
        {
            // Marker symbol at the end of the document.
            var location = node.GetSourceLocation(sourceDocument);

            return location.ToLinePosition().ToZeroWidthSpan();
        }

        return sourceText.GetLinePositionSpan(start, end);
    }

    /// <summary>
    /// Finds the innermost SyntaxNode for a given location in source, within a given node.
    /// </summary>
    /// <param name="node">The parent node to search inside.</param>
    /// <param name="index">The location to find the innermost node at.</param>
    /// <param name="includeWhitespace">Whether to include whitespace in the search.</param>
    /// <param name="walkMarkersBack">When true, if there are multiple <see cref="SyntaxKind.Marker"/> tokens in a single location, return the parent node of the
    /// first one in the tree.</param>
    public static SyntaxNode? FindInnermostNode(this SyntaxNode node, int index, bool includeWhitespace = false, bool walkMarkersBack = true)
    {
        var token = node.FindToken(index, includeWhitespace);

        // If the index is EOF but the node has index-1,
        // then try to get a token to the left of the index.
        // patterns like
        // <button></button>$$
        // should get the button node instead of the razor document (which is the parent
        // of the EOF token)
        if (token.Kind == SyntaxKind.EndOfFile && node.Span.Contains(index - 1))
        {
            token = token.GetPreviousToken(includeWhitespace);
        }

        var foundPosition = token.Position;

        if (walkMarkersBack && token.Kind == SyntaxKind.Marker)
        {
            while (true)
            {
                var previousToken = token.GetPreviousToken(includeWhitespace);

                if (previousToken.Kind != SyntaxKind.Marker || previousToken.Position != foundPosition)
                {
                    break;
                }

                token = previousToken;
            }
        }

        return token.Parent;
    }

    public static SyntaxNode? FindNode(this SyntaxNode @this, TextSpan span, bool includeWhitespace = false, bool getInnermostNodeForTie = false)
    {
        if (!@this.FullSpan.Contains(span))
        {
            return ThrowHelper.ThrowArgumentOutOfRangeException<SyntaxNode?>(nameof(span));
        }

        var node = @this.FindToken(span.Start, includeWhitespace)
            .Parent!
            .FirstAncestorOrSelf<SyntaxNode>(a => a.FullSpan.Contains(span));

        node.AssumeNotNull();

        // Tie-breaking.
        if (!getInnermostNodeForTie)
        {
            var cuRoot = node.Ancestors();

            // Only null if node is the original node is the root
            if (cuRoot is null)
            {
                return node;
            }

            while (true)
            {
                var parent = node.Parent;
                // NOTE: We care about FullSpan equality, but FullWidth is cheaper and equivalent.
                if (parent == null || parent.FullWidth != node.FullWidth)
                {
                    break;
                }

                // prefer child over compilation unit
                if (parent == cuRoot)
                {
                    break;
                }

                node = parent;
            }
        }

        return node;
    }

    public static bool ExistsOnTarget(this SyntaxNode node, SyntaxNode target)
    {
        // TODO: This looks like a potential allocation hotspot and performance bottleneck.

        var nodeString = node.RemoveEmptyNewLines().ToFullString();
        var matchingNode = target.DescendantNodesAndSelf()
            // Empty new lines can affect our comparison so we remove them since they're insignificant.
            .Where(n => n.RemoveEmptyNewLines().ToFullString() == nodeString)
            .FirstOrDefault();

        return matchingNode is not null;
    }

    public static SyntaxNode RemoveEmptyNewLines(this SyntaxNode node)
    {
        if (node is MarkupTextLiteralSyntax markupTextLiteral)
        {
            var literalTokensWithoutLines = markupTextLiteral.LiteralTokens.Where(static t => t.Kind != SyntaxKind.NewLine);
            return markupTextLiteral.WithLiteralTokens(literalTokensWithoutLines);
        }

        return node;
    }

    public static bool IsCSharpNode(this SyntaxNode node, [NotNullWhen(true)] out CSharpCodeBlockSyntax? csharpCodeBlock)
    {
        csharpCodeBlock = null;

        // Any piece of C# code can potentially be surrounded by a CSharpCodeBlockSyntax.
        switch (node)
        {
            case CSharpCodeBlockSyntax outerCSharpCodeBlock:
                var innerCSharpNode = outerCSharpCodeBlock.ChildNodes().FirstOrDefault(
                    static n => n is CSharpStatementSyntax or
                                     RazorDirectiveSyntax or
                                     CSharpExplicitExpressionSyntax or
                                     CSharpImplicitExpressionSyntax);

                if (innerCSharpNode is not null)
                {
                    return innerCSharpNode.IsCSharpNode(out csharpCodeBlock);
                }

                break;

            // @code {
            //    var foo = "bar";
            // }
            case RazorDirectiveSyntax { Body: RazorDirectiveBodySyntax body }:
                // code {
                //    var foo = "bar";
                // }
                var directive = body.Keyword.ToFullString();
                if (directive != "code")
                {
                    return false;
                }

                // {
                //    var foo = "bar";
                // }
                csharpCodeBlock = body.CSharpCode;

                // var foo = "bar";
                var innerCodeBlock = csharpCodeBlock.ChildNodes().FirstOrDefault(IsCSharpCodeBlockSyntax);
                if (innerCodeBlock is not null)
                {
                    csharpCodeBlock = innerCodeBlock as CSharpCodeBlockSyntax;
                }

                break;

            // @(x)
            // (x)
            case CSharpExplicitExpressionSyntax { Body: CSharpExplicitExpressionBodySyntax body }:
                // x
                csharpCodeBlock = body.CSharpCode;
                break;

            // @x
            case CSharpImplicitExpressionSyntax { Body: CSharpImplicitExpressionBodySyntax body }:
                // x
                csharpCodeBlock = body.CSharpCode;
                break;

            // @{
            //    var x = 1;
            // }
            case CSharpStatementSyntax csharpStatement:
                // {
                //    var x = 1;
                // }
                var csharpStatementBody = csharpStatement.Body;

                // var x = 1;
                csharpCodeBlock = csharpStatementBody.ChildNodes().FirstOrDefault(IsCSharpCodeBlockSyntax) as CSharpCodeBlockSyntax;
                break;
        }

        return csharpCodeBlock is not null;

        static bool IsCSharpCodeBlockSyntax(SyntaxNode node)
        {
            return node is CSharpCodeBlockSyntax;
        }
    }
}
