// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal record TagHelperDeltaResult(
    bool Delta,
    int ResultId,
    ImmutableArray<TagHelperDescriptor> Added,
    ImmutableArray<TagHelperDescriptor> Removed)
{
    public ImmutableArray<TagHelperDescriptor> Apply(ImmutableArray<TagHelperDescriptor> baseTagHelpers)
    {
        if (Added.Length == 0 && Removed.Length == 0)
        {
            return baseTagHelpers;
        }

        // We're specifically choosing to create a List here instead of an alternate type like HashSet because
        // results that are produced from `Apply` are typically fed back into two different systems:
        //
        // 1. This TagHelperDeltaResult.Apply where we don't iterate / Contains check the "base" collection.
        // 2. The rest of the Razor project system. Everything there is always indexed / iterated as a list.
        using var _ = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var newTagHelpers);
        newTagHelpers.SetCapacityIfLarger(baseTagHelpers.Length + Added.Length - Removed.Length);
        newTagHelpers.AddRange(Added);

        foreach (var existingTagHelper in baseTagHelpers)
        {
            if (!Removed.Contains(existingTagHelper))
            {
                newTagHelpers.Add(existingTagHelper);
            }
        }

        return newTagHelpers.ToImmutable();
    }
}
