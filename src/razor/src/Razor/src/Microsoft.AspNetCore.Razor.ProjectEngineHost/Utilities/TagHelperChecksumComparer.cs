// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed class TagHelperChecksumComparer : IEqualityComparer<TagHelperDescriptor>
{
    public static readonly TagHelperChecksumComparer Instance = new();

    private TagHelperChecksumComparer()
    {
    }

    public bool Equals(TagHelperDescriptor? x, TagHelperDescriptor? y)
        => EqualityComparer<Checksum?>.Default.Equals(x?.GetChecksum(), y?.GetChecksum());

    public int GetHashCode(TagHelperDescriptor obj)
        => obj.GetChecksum().GetHashCode();
}
