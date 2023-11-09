// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static class SyntaxUtilities
{
    public static MarkupTextLiteralSyntax? MergeTextLiterals(params MarkupTextLiteralSyntax[] literalSyntaxes)
    {
        if (literalSyntaxes == null || literalSyntaxes.Length == 0)
        {
            return null;
        }

        SyntaxNode? parent = null;
        var position = 0;
        var seenFirstLiteral = false;
        var builder = Syntax.InternalSyntax.SyntaxListBuilder.Create();

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

        var mergedLiteralSyntax = Syntax.InternalSyntax.SyntaxFactory.MarkupTextLiteral(
            builder.ToList<Syntax.InternalSyntax.SyntaxToken>(), chunkGenerator: null);

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
        var builder = new SyntaxListBuilder(5);
        var tokens = SyntaxListBuilder<SyntaxToken>.Create();
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

        return builder.ToListNode().CreateRed(@this, @this.Position);
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
        var builder = new SyntaxListBuilder(3);
        var tokens = SyntaxListBuilder<SyntaxToken>.Create();
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

        return builder.ToListNode().CreateRed(@this, @this.Position);
    }
}
