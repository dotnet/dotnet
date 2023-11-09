// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal partial class MarkupStartTagSyntax
{
    private SyntaxNode _lazyChildren;

    public bool IsMarkupTransition
        => ((InternalSyntax.MarkupStartTagSyntax)Green).IsMarkupTransition;

    public SyntaxList<RazorSyntaxNode> LegacyChildren
    {
        get
        {
            var children = _lazyChildren ?? InterlockedOperations.Initialize(ref _lazyChildren, GetLegacyChildren());

            return new SyntaxList<RazorSyntaxNode>(children);

            SyntaxNode GetLegacyChildren()
            {
                return SyntaxUtilities.GetStartTagLegacyChildren(this, Attributes, OpenAngle, Bang, Name, ForwardSlash, CloseAngle);
            }
        }
    }

    public string GetTagNameWithOptionalBang()
    {
        return Name.IsMissing ? string.Empty : Bang?.Content + Name.Content;
    }

    public bool IsSelfClosing()
    {
        return ForwardSlash != null &&
            !ForwardSlash.IsMissing &&
            !CloseAngle.IsMissing;
    }

    public bool IsVoidElement()
    {
        return ParserHelpers.VoidElements.Contains(Name.Content);
    }
}
