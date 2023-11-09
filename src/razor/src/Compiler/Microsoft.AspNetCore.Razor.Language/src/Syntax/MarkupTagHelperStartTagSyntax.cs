// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal partial class MarkupTagHelperStartTagSyntax
{
    private SyntaxNode _lazyChildren;

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
}
