// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal class WhitespaceRewriter : SyntaxRewriter
{
    public override SyntaxNode Visit(SyntaxNode node)
    {
        if (node == null)
        {
            return base.Visit(node);
        }

        var children = node.ChildNodesAndTokens();
        for (var i = 0; i < children.Count; i++)
        {
            var child = children[i];
            if (child.AsNode() is CSharpCodeBlockSyntax codeBlock &&
                TryRewriteWhitespace(codeBlock, out var rewritten, out var whitespaceLiteral))
            {
                // Replace the existing code block with the whitespace literal
                // followed by the rewritten code block (with the code whitespace removed).
                node = node.ReplaceNode(codeBlock, [whitespaceLiteral, rewritten]);

                // Since we replaced node, its children are different. Update our collection.
                children = node.ChildNodesAndTokens();
            }
        }

        return base.Visit(node);
    }

    private bool TryRewriteWhitespace(CSharpCodeBlockSyntax codeBlock, out CSharpCodeBlockSyntax rewritten, out SyntaxNode whitespaceLiteral)
    {
        // Rewrite any whitespace represented as code at the start of a line preceding an expression block.
        // We want it to be rendered as Markup.

        rewritten = null;
        whitespaceLiteral = null;

        if (codeBlock.Children is [CSharpStatementLiteralSyntax literal, CSharpExplicitExpressionSyntax or CSharpImplicitExpressionSyntax, ..])
        {
            var containsNonWhitespace = literal.DescendantTokens().Any(static t => !string.IsNullOrWhiteSpace(t.Content));

            if (!containsNonWhitespace)
            {
                // Literal node is all whitespace. Can rewrite.
                whitespaceLiteral = SyntaxFactory.MarkupTextLiteral(literal.LiteralTokens, chunkGenerator: null);
                rewritten = codeBlock.ReplaceNode(literal, newNode: null);
                return true;
            }
        }

        return false;
    }
}
