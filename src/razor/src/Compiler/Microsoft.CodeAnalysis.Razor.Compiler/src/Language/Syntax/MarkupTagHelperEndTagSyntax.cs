// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal partial class MarkupTagHelperEndTagSyntax
{
    public override BaseMarkupStartTagSyntax? GetStartTag()
        => (Parent as MarkupTagHelperElementSyntax)?.StartTag;
}
