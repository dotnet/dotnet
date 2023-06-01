// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal sealed class TagHelperResolutionResultComparer : IEqualityComparer<TagHelperResolutionResult?>
{
    internal static readonly TagHelperResolutionResultComparer Default = new();

    public bool Equals(TagHelperResolutionResult? x, TagHelperResolutionResult? y)
    {
        if (x is null)
        {
            return y is null;
        }
        else if (y is null)
        {
            return false;
        }

        return x.Descriptors.SequenceEqual(y.Descriptors, TagHelperDescriptorComparer.Default);
    }

    public int GetHashCode(TagHelperResolutionResult? obj)
    {
        if (obj is null)
        {
            return 0;
        }

        var hash = HashCodeCombiner.Start();

        foreach (var descriptor in obj.Descriptors)
        {
            hash.Add(descriptor);
        }

        return hash.CombinedHash;
    }
}
