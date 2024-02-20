// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal sealed partial class MarkupTagHelperElementSyntax
{
    private static readonly string TagHelperInfoKey = typeof(TagHelperInfo).Name;

    public TagHelperInfo? TagHelperInfo
        => this.GetAnnotationValue(TagHelperInfoKey) as TagHelperInfo;

    public MarkupTagHelperElementSyntax WithTagHelperInfo(TagHelperInfo info)
    {
        var newGreen = Green.WithAnnotationsGreen([.. GetAnnotations(), new(TagHelperInfoKey, info)]);

        return (MarkupTagHelperElementSyntax)newGreen.CreateRed(Parent, Position);
    }
}
