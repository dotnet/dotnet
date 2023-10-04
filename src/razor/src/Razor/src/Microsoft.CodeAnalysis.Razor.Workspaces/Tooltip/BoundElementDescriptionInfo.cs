// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Tooltip;

internal sealed record BoundElementDescriptionInfo(string TagHelperTypeName, string? Documentation = null)
{
    public static BoundElementDescriptionInfo From(TagHelperDescriptor tagHelper)
    {
        var tagHelperTypeName = tagHelper.GetTypeName();

        return new BoundElementDescriptionInfo(tagHelperTypeName, tagHelper.Documentation);
    }
}
