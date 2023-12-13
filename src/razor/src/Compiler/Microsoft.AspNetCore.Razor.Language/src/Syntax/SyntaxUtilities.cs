// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class SyntaxUtilities
{
    public static MarkupTextLiteralSyntax MergeTextLiterals(params MarkupTextLiteralSyntax[] literalSyntaxes)
    {
        SyntaxNode? parent = null;
        var position = 0;
        var seenFirstLiteral = false;
        var builder = InternalSyntax.SyntaxListBuilder.Create();

        foreach (var syntax in literalSyntaxes)
        {
            if (syntax == null)
            {
                continue;
            }
            else if (!seenFirstLiteral)
            {
                // Set the parent and position of the merged literal to the value of the first non-null literal.
                parent = syntax.Parent;
                position = syntax.Position;
                seenFirstLiteral = true;
            }

            foreach (var token in syntax.LiteralTokens)
            {
                builder.Add(token.Green);
            }
        }

        var mergedLiteralSyntax = InternalSyntax.SyntaxFactory.MarkupTextLiteral(
            builder.ToList<InternalSyntax.SyntaxToken>(), chunkGenerator: null);

        return (MarkupTextLiteralSyntax)mergedLiteralSyntax.CreateRed(parent, position);
    }

    internal static SyntaxNode GetStartTagLegacyChildren(
        SyntaxNode @this,
        SyntaxList<RazorSyntaxNode> attributes,
        SyntaxToken openAngle,
        SyntaxToken bang,
        SyntaxToken name,
        SyntaxToken forwardSlash,
        SyntaxToken closeAngle)
    {
        // This method returns the children of this start tag in legacy format.
        // This is needed to generate the same classified spans as the legacy syntax tree.
        using var _1 = SyntaxListBuilderPool.GetPooledBuilder(out var builder);
        using var _2 = SyntaxListBuilderPool.GetPooledBuilder<SyntaxToken>(out var tokens);

        SpanEditHandler? acceptsAnyHandler = null;
        var containsAttributesContent = false;
        var editHandler = @this.GetEditHandler();
        var chunkGenerator = @this.GetChunkGenerator();
        if (editHandler != null)
        {
            // We want to know if this tag contains non-whitespace attribute content to set the appropriate AcceptedCharacters.
            // The prefix of a start tag(E.g '|<foo| attr>') will have 'Any' accepted characters if non-whitespace attribute content exists.
            acceptsAnyHandler = SpanEditHandler.CreateDefault(AcceptedCharactersInternal.Any);
            foreach (var attribute in attributes)
            {
                if (!string.IsNullOrWhiteSpace(attribute.GetContent()))
                {
                    containsAttributesContent = true;
                    break;
                }
            }
        }

        if (!openAngle.IsMissing)
        {
            tokens.Add(openAngle);
        }

        if (bang != null)
        {
            builder.Add(SyntaxFactory.MarkupTextLiteral(tokens.Consume(), chunkGenerator).WithEditHandler(acceptsAnyHandler));

            tokens.Add(bang);
            var acceptsNoneHandler = editHandler == null ? null : SpanEditHandler.CreateDefault(AcceptedCharactersInternal.None);
            builder.Add(SyntaxFactory.RazorMetaCode(tokens.Consume(), chunkGenerator).WithEditHandler(acceptsNoneHandler));
        }

        if (!name.IsMissing)
        {
            tokens.Add(name);
        }

        builder.Add(SyntaxFactory.MarkupTextLiteral(tokens.Consume(), chunkGenerator).WithEditHandler(containsAttributesContent ? acceptsAnyHandler : editHandler));

        builder.AddRange(attributes);

        if (forwardSlash != null)
        {
            tokens.Add(forwardSlash);
        }

        if (!closeAngle.IsMissing)
        {
            tokens.Add(closeAngle);
        }

        if (tokens.Count > 0)
        {
            builder.Add(SyntaxFactory.MarkupTextLiteral(tokens.Consume(), chunkGenerator).WithEditHandler(editHandler));
        }

        return builder.ToListNode().AssumeNotNull().CreateRed(@this, @this.Position);
    }

    internal static SyntaxNode GetEndTagLegacyChildren(
        SyntaxNode @this,
        SyntaxToken openAngle,
        SyntaxToken forwardSlash,
        SyntaxToken bang,
        SyntaxToken name,
        MarkupMiscAttributeContentSyntax miscAttributeContent,
        SyntaxToken closeAngle)
    {
        // This method returns the children of this end tag in legacy format.
        // This is needed to generate the same classified spans as the legacy syntax tree.
        using var _1 = SyntaxListBuilderPool.GetPooledBuilder(out var builder);
        using var _2 = SyntaxListBuilderPool.GetPooledBuilder<SyntaxToken>(out var tokens);

        var editHandler = @this.GetEditHandler();
        var chunkGenerator = @this.GetChunkGenerator();

        if (!openAngle.IsMissing)
        {
            tokens.Add(openAngle);
        }

        if (!forwardSlash.IsMissing)
        {
            tokens.Add(forwardSlash);
        }

        if (bang != null)
        {
            SpanEditHandler? acceptsAnyHandler = null;
            SpanEditHandler? acceptsNoneHandler = null;
            if (editHandler != null)
            {
                acceptsAnyHandler = SpanEditHandler.CreateDefault(AcceptedCharactersInternal.Any);
                acceptsNoneHandler = SpanEditHandler.CreateDefault(AcceptedCharactersInternal.None);
            }

            // The prefix of an end tag(E.g '|</|!foo>') will have 'Any' accepted characters if a bang exists.
            builder.Add(SyntaxFactory.MarkupTextLiteral(tokens.Consume(), chunkGenerator).WithEditHandler(acceptsAnyHandler));

            tokens.Add(bang);
            builder.Add(SyntaxFactory.RazorMetaCode(tokens.Consume(), chunkGenerator).WithEditHandler(acceptsNoneHandler));
        }

        if (!name.IsMissing)
        {
            tokens.Add(name);
        }

        if (miscAttributeContent?.Children != null && miscAttributeContent.Children.Count > 0)
        {
            foreach (var content in miscAttributeContent.Children)
            {
                tokens.AddRange(((MarkupTextLiteralSyntax)content).LiteralTokens);
            }
        }

        if (!closeAngle.IsMissing)
        {
            tokens.Add(closeAngle);
        }

        builder.Add(SyntaxFactory.MarkupTextLiteral(tokens.Consume(), chunkGenerator).WithEditHandler(editHandler));

        return builder.ToListNode().AssumeNotNull().CreateRed(@this, @this.Position);
    }

    internal static SyntaxList<RazorSyntaxNode> GetRewrittenMarkupStartTagChildren(MarkupStartTagSyntax node, bool includeEditHandler = false)
    {
        // Rewrites the children of the start tag to look like the legacy syntax tree.
        if (node.IsMarkupTransition)
        {
            return GetRewrittenMarkupNodeChildren(node, node.ChunkGenerator, includeEditHandler);
        }

        SpanEditHandler? latestEditHandler = null;

        var children = node.LegacyChildren;
        using var _ = SyntaxListBuilderPool.GetPooledBuilder<RazorSyntaxNode>(out var newChildren);
        var literals = new List<MarkupTextLiteralSyntax>();
        foreach (var child in children)
        {
            if (child is MarkupTextLiteralSyntax literal)
            {
                literals.Add(literal);

                if (includeEditHandler)
                {
                    latestEditHandler = literal.GetEditHandler() ?? latestEditHandler;
                }
            }
            else if (child is MarkupMiscAttributeContentSyntax miscContent)
            {
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
                        AddLiteralIfExists();
                        newChildren.Add(contentChild);
                    }
                }
            }
            else
            {
                AddLiteralIfExists();
                newChildren.Add(child);
            }
        }

        AddLiteralIfExists();

        return newChildren.ToList(node);

        void AddLiteralIfExists()
        {
            if (literals.Count > 0)
            {
                var mergedLiteral = MergeTextLiterals(literals.ToArray());

                if (includeEditHandler)
                {
                    mergedLiteral = mergedLiteral.WithEditHandler(latestEditHandler);
                }

                literals.Clear();
                latestEditHandler = null;
                newChildren.Add(mergedLiteral);
            }
        }
    }

    internal static SyntaxList<RazorSyntaxNode> GetRewrittenMarkupEndTagChildren(MarkupEndTagSyntax node, bool includeEditHandler = false)
    {
        // Rewrites the children of the end tag to look like the legacy syntax tree.
        return node.IsMarkupTransition
            ? GetRewrittenMarkupNodeChildren(node, node.ChunkGenerator, includeEditHandler)
            : node.LegacyChildren;
    }

    internal static SyntaxList<RazorSyntaxNode> GetRewrittenMarkupNodeChildren(
        MarkupSyntaxNode node,
        ISpanChunkGenerator chunkGenerator,
        bool includeEditHandler = false)
    {
        var tokens = node.DescendantNodes().OfType<SyntaxToken>().Where(t => !t.IsMissing).ToArray();

        using var _ = SyntaxListBuilderPool.GetPooledBuilder<SyntaxToken>(out var builder);
        builder.AddRange(tokens, 0, tokens.Length);
        var transitionTokens = builder.ToList();

        var markupTransition = SyntaxFactory
            .MarkupTransition(transitionTokens, chunkGenerator).Green
            .CreateRed(node, node.Position);

        if (includeEditHandler && node.GetEditHandler() is { } editHandler)
        {
            markupTransition = markupTransition.WithEditHandler(editHandler);
        }

        return SyntaxList<RazorSyntaxNode>.Create(markupTransition, parent: node);
    }
}
