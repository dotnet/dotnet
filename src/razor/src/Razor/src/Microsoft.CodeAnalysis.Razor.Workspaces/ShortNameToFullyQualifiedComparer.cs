// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal sealed class ShortNameToFullyQualifiedComparer : IEqualityComparer<TagHelperDescriptor>
{
    public static readonly ShortNameToFullyQualifiedComparer Instance = new();

    private ShortNameToFullyQualifiedComparer()
    {
    }

    public bool Equals(TagHelperDescriptor? x, TagHelperDescriptor? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        return x is not null &&
               y is not null &&
               x.Name == y.Name &&
               x.AssemblyName == y.AssemblyName;
    }

    public int GetHashCode(TagHelperDescriptor obj)
        => obj.Name.GetHashCode();
}
