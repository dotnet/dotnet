// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal abstract partial class BaseMarkupEndTagSyntax
{
    private SyntaxNode? _lazyChildren;

    public SyntaxList<RazorSyntaxNode> LegacyChildren
    {
        get
        {
            var children = _lazyChildren ??
                InterlockedOperations.Initialize(ref _lazyChildren, this.ComputeEndTagLegacyChildren());

            return new SyntaxList<RazorSyntaxNode>(children);
        }
    }

    private SyntaxNode ComputeEndTagLegacyChildren()
    {
        // This method returns the children of this end tag in legacy format.
        // This is needed to generate the same classified spans as the legacy syntax tree.

        using PooledArrayBuilder<SyntaxNode> builder = [];
        using PooledArrayBuilder<SyntaxToken> tokensBuilder = [];

        // Take a ref to tokensBuilder here to avoid calling AsRef() multiple times below
        // for each call to ToListAndClear().
        ref var tokens = ref tokensBuilder.AsRef();

        var editHandler = this.GetEditHandler();
        var chunkGenerator = ChunkGenerator;

        if (OpenAngle is { IsMissing: false } openAngle)
        {
            tokens.Add(openAngle);
        }

        if (ForwardSlash is { IsMissing: false } forwardSlash)
        {
            tokens.Add(forwardSlash);
        }

        if (Bang is { IsMissing: false } bang)
        {
            SpanEditHandler? acceptsAnyHandler = null;
            SpanEditHandler? acceptsNoneHandler = null;

            if (editHandler != null)
            {
                acceptsAnyHandler = SpanEditHandler.GetDefault(AcceptedCharactersInternal.Any);
                acceptsNoneHandler = SpanEditHandler.GetDefault(AcceptedCharactersInternal.None);
            }

            // The prefix of an end tag(E.g '|</|!foo>') will have 'Any' accepted characters if a bang exists.
            builder.Add(MarkupTextLiteral(tokens.ToListAndClear(), chunkGenerator, acceptsAnyHandler));

            // We can skip adding bang to the tokens builder, since we just cleared it.
            builder.Add(RazorMetaCode(bang, chunkGenerator, acceptsNoneHandler));
        }

        if (Name is { IsMissing: false } name)
        {
            tokens.Add(name);
        }

        if (MiscAttributeContent?.Children is { Count: > 0 } children)
        {
            foreach (var content in children)
            {
                var literal = (MarkupTextLiteralSyntax)content;
                tokens.AddRange(literal.LiteralTokens);
            }
        }

        if (CloseAngle is { IsMissing: false } closeAngle)
        {
            tokens.Add(closeAngle);
        }

        builder.Add(MarkupTextLiteral(tokens.ToListAndClear(), chunkGenerator, editHandler));

        return builder.ToListNode(parent: this, Position)
            .AssumeNotNull($"ToListNode should not return null since builder was not empty.");


        static MarkupTextLiteralSyntax MarkupTextLiteral(
            SyntaxList<SyntaxToken> tokens, ISpanChunkGenerator? chunkGenerator, SpanEditHandler? editHandler)
        {
            var node = SyntaxFactory.MarkupTextLiteral(tokens, chunkGenerator);

            if (editHandler != null)
            {
                node = node.WithEditHandler(editHandler);
            }

            return node;
        }

        static RazorMetaCodeSyntax RazorMetaCode(
            SyntaxToken token, ISpanChunkGenerator? chunkGenerator, SpanEditHandler? editHandler)
        {
            var node = SyntaxFactory.RazorMetaCode(token, chunkGenerator);

            if (editHandler != null)
            {
                node = node.WithEditHandler(editHandler);
            }

            return node;
        }
    }
}
