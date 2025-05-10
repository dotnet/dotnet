// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal abstract partial class BaseMarkupStartTagSyntax
{
    private SyntaxNode? _lazyChildren;

    public SyntaxList<RazorSyntaxNode> LegacyChildren
    {
        get
        {
            var children = _lazyChildren ??
                InterlockedOperations.Initialize(ref _lazyChildren, ComputeStartTagLegacyChildren());

            return new SyntaxList<RazorSyntaxNode>(children);
        }
    }

    /// <summary>
    ///  This method returns the children of this start tag in legacy format.
    ///  This is needed to generate the same classified spans as the legacy syntax tree.
    /// </summary>
    private SyntaxNode ComputeStartTagLegacyChildren()
    {
        using PooledArrayBuilder<SyntaxNode> builder = [];
        using PooledArrayBuilder<SyntaxToken> tokensBuilder = [];

        // Take a ref to tokensBuilder here to avoid calling AsRef() multiple times below
        // for each call to ToListAndClear().
        ref var tokens = ref tokensBuilder.AsRef();

        SpanEditHandler? acceptsAnyHandler = null;
        SpanEditHandler? acceptsNoneHandler = null;

        var containsAttributesContent = false;

        var editHandler = this.GetEditHandler();
        if (editHandler != null)
        {
            acceptsAnyHandler = SpanEditHandler.GetDefault(AcceptedCharactersInternal.Any);
            acceptsNoneHandler = SpanEditHandler.GetDefault(AcceptedCharactersInternal.None);

            // We want to know if this tag contains non-whitespace attribute content to set
            // the appropriate AcceptedCharacters. The prefix of a start tag(E.g '|<foo| attr>')
            // will have 'Any' accepted characters if non-whitespace attribute content exists.

            foreach (var attribute in Attributes)
            {
                foreach (var token in attribute.DescendantTokens())
                {
                    if (!string.IsNullOrWhiteSpace(token.Content))
                    {
                        containsAttributesContent = true;
                        break;
                    }
                }
            }
        }

        var chunkGenerator = ChunkGenerator;

        if (OpenAngle is { IsMissing: false } openAngle)
        {
            tokens.Add(openAngle);
        }

        if (Bang is { IsMissing: false } bang)
        {
            builder.Add(MarkupTextLiteral(tokens.ToListAndClear(), chunkGenerator, acceptsAnyHandler));

            // We can skip adding bang to the tokens builder, since we just cleared it.
            builder.Add(RazorMetaCode(bang, chunkGenerator, acceptsNoneHandler));
        }

        if (Name is { IsMissing: false } name)
        {
            tokens.Add(name);
        }

        builder.Add(MarkupTextLiteral(
            tokens.ToListAndClear(), chunkGenerator, containsAttributesContent ? acceptsAnyHandler : editHandler));

        builder.AddRange(Attributes);

        if (ForwardSlash is { IsMissing: false } forwardSlash)
        {
            tokens.Add(forwardSlash);
        }

        if (CloseAngle is { IsMissing: false } closeAngle)
        {
            tokens.Add(closeAngle);
        }

        if (tokens.Count > 0)
        {
            builder.Add(MarkupTextLiteral(tokens.ToListAndClear(), chunkGenerator, editHandler));
        }

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
