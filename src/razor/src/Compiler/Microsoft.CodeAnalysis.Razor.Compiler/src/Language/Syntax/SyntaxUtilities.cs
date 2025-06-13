// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class SyntaxUtilities
{
    public static MarkupTextLiteralSyntax MergeTextLiterals(params ReadOnlySpan<MarkupTextLiteralSyntax?> literals)
    {
        SyntaxNode? parent = null;
        var position = 0;
        var seenFirstLiteral = false;

        using PooledArrayBuilder<SyntaxToken> builder = [];

        foreach (var literal in literals)
        {
            if (literal == null)
            {
                continue;
            }

            if (!seenFirstLiteral)
            {
                // Set the parent and position of the merged literal to the value of the first non-null literal.
                parent = literal.Parent;
                position = literal.Position;
                seenFirstLiteral = true;
            }

            builder.AddRange(literal.LiteralTokens);
        }

        return (MarkupTextLiteralSyntax)InternalSyntax.SyntaxFactory
            .MarkupTextLiteral(
                literalTokens: builder.ToGreenListNode().ToGreenList<InternalSyntax.SyntaxToken>(),
                chunkGenerator: null)
            .CreateRed(parent, position);
    }

    internal static SyntaxList<RazorSyntaxNode> GetRewrittenMarkupStartTagChildren(
        MarkupStartTagSyntax node, bool includeEditHandler = false)
    {
        // Rewrites the children of the start tag to look like the legacy syntax tree.
        if (node.IsMarkupTransition)
        {
            return GetRewrittenMarkupNodeChildren(node, node.ChunkGenerator, includeEditHandler);
        }

        using PooledArrayBuilder<RazorSyntaxNode> newChildren = [];
        using PooledArrayBuilder<MarkupTextLiteralSyntax> literals = [];

        SpanEditHandler? latestEditHandler = null;

        foreach (var child in node.LegacyChildren)
        {
            switch (child)
            {
                case MarkupTextLiteralSyntax literal:
                    literals.Add(literal);

                    if (includeEditHandler)
                    {
                        latestEditHandler = literal.GetEditHandler() ?? latestEditHandler;
                    }

                    break;

                case MarkupMiscAttributeContentSyntax miscContent:
                    foreach (var contentChild in miscContent.Children)
                    {
                        if (contentChild is MarkupTextLiteralSyntax contentLiteral)
                        {
                            literals.Add(contentLiteral);

                            if (includeEditHandler)
                            {
                                latestEditHandler = contentLiteral.GetEditHandler() ?? latestEditHandler;
                            }
                        }
                        else
                        {
                            // Pop stack
                            AddLiteralIsIfNeeded();
                            newChildren.Add(contentChild);
                        }
                    }

                    break;

                default:
                    AddLiteralIsIfNeeded();
                    newChildren.Add(child);
                    break;
            }
        }

        AddLiteralIsIfNeeded();

        return newChildren.ToList(node);

        void AddLiteralIsIfNeeded()
        {
            if (literals.Count > 0)
            {
                var mergedLiteral = MergeTextLiterals(literals.ToArrayAndClear());

                if (includeEditHandler)
                {
                    mergedLiteral = mergedLiteral.WithEditHandler(latestEditHandler);
                }

                latestEditHandler = null;
                newChildren.Add(mergedLiteral);
            }
        }
    }

    internal static SyntaxList<RazorSyntaxNode> GetRewrittenMarkupEndTagChildren(
        MarkupEndTagSyntax node, bool includeEditHandler = false)
    {
        // Rewrites the children of the end tag to look like the legacy syntax tree.
        return node.IsMarkupTransition
            ? GetRewrittenMarkupNodeChildren(node, node.ChunkGenerator, includeEditHandler)
            : node.LegacyChildren;
    }

    internal static SyntaxList<RazorSyntaxNode> GetRewrittenMarkupNodeChildren(
        MarkupSyntaxNode node, ISpanChunkGenerator chunkGenerator, bool includeEditHandler = false)
    {
        using PooledArrayBuilder<SyntaxToken> builder = [];

        foreach (var token in node.DescendantTokens())
        {
            if (!token.IsMissing)
            {
                builder.Add(token);
            }
        }

        var markupTransition = InternalSyntax.SyntaxFactory
            .MarkupTransition(
                builder.ToGreenListNode().ToGreenList<InternalSyntax.SyntaxToken>(),
                chunkGenerator)
            .CreateRed(node, node.Position);

        if (includeEditHandler && node.GetEditHandler() is { } editHandler)
        {
            markupTransition = markupTransition.WithEditHandler(editHandler);
        }

        return new(markupTransition);
    }
}
