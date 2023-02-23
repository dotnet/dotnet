// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal partial class MarkupEndTagSyntax
{
    private SyntaxNode _lazyChildren;

    public bool IsMarkupTransition
        => ((InternalSyntax.MarkupEndTagSyntax)Green).IsMarkupTransition;

    public SyntaxList<RazorSyntaxNode> Children
    {
        get
        {
            var children = _lazyChildren ?? InterlockedOperations.Initialize(ref _lazyChildren, GetLegacyChildren());

            return new SyntaxList<RazorSyntaxNode>(children);
        }
    }

    public string GetTagNameWithOptionalBang()
    {
        return Name.IsMissing ? string.Empty : Bang?.Content + Name.Content;
    }

    private SyntaxNode GetLegacyChildren()
    {
        // This method returns the children of this end tag in legacy format.
        // This is needed to generate the same classified spans as the legacy syntax tree.
        var builder = new SyntaxListBuilder(3);
        var tokens = SyntaxListBuilder<SyntaxToken>.Create();
        var context = this.GetSpanContext();

        if (!OpenAngle.IsMissing)
        {
            tokens.Add(OpenAngle);
        }

        if (!ForwardSlash.IsMissing)
        {
            tokens.Add(ForwardSlash);
        }

        if (Bang != null)
        {
            // The prefix of an end tag(E.g '|</|!foo>') will have 'Any' accepted characters if a bang exists.
            var acceptsAnyContext = new SpanContext(context.ChunkGenerator, SpanEditHandler.CreateDefault(AcceptedCharactersInternal.Any));
            builder.Add(SyntaxFactory.MarkupTextLiteral(tokens.Consume()).WithSpanContext(acceptsAnyContext));

            tokens.Add(Bang);
            var acceptsNoneContext = new SpanContext(context.ChunkGenerator, SpanEditHandler.CreateDefault(AcceptedCharactersInternal.None));
            builder.Add(SyntaxFactory.RazorMetaCode(tokens.Consume()).WithSpanContext(acceptsNoneContext));
        }

        if (!Name.IsMissing)
        {
            tokens.Add(Name);
        }

        if (MiscAttributeContent?.Children != null && MiscAttributeContent.Children.Count > 0)
        {
            foreach (var content in MiscAttributeContent.Children)
            {
                tokens.AddRange(((MarkupTextLiteralSyntax)content).LiteralTokens);
            }
        }

        if (!CloseAngle.IsMissing)
        {
            tokens.Add(CloseAngle);
        }

        builder.Add(SyntaxFactory.MarkupTextLiteral(tokens.Consume()).WithSpanContext(context));

        return builder.ToListNode().CreateRed(this, Position);
    }
}
