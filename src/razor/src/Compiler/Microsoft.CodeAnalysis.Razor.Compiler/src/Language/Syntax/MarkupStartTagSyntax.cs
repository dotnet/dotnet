// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal partial class MarkupStartTagSyntax
{
    public string GetTagNameWithOptionalBang()
    {
        return Name.IsMissing ? string.Empty : Bang.Content + Name.Content;
    }

    public bool IsSelfClosing()
    {
        return ForwardSlash.Kind != SyntaxKind.None &&
            !ForwardSlash.IsMissing &&
            !CloseAngle.IsMissing;
    }

    public bool IsVoidElement()
    {
        return ParserHelpers.VoidElements.Contains(Name.Content);
    }
}
