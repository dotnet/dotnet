// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal record TagHelperDeltaResult(
    bool Delta,
    int ResultId,
    IReadOnlyCollection<TagHelperDescriptor> Added,
    IReadOnlyCollection<TagHelperDescriptor> Removed)
{
    public IReadOnlyCollection<TagHelperDescriptor> Apply(IReadOnlyCollection<TagHelperDescriptor> baseTagHelpers)
    {
        if (Added.Count == 0 && Removed.Count == 0)
        {
            return baseTagHelpers;
        }

        // We're specifically choosing to create a List here instead of an alternate type like HashSet because
        // results that are produced from `Apply` are typically fed back into two different systems:
        //
        // 1. This TagHelperDeltaResult.Apply where we don't iterate / Contains check the "base" collection.
        // 2. The rest of the Razor project system. Everything there is always indexed / iterated as a list.
        var newTagHelpers = new List<TagHelperDescriptor>(baseTagHelpers.Count + Added.Count - Removed.Count);
        newTagHelpers.AddRange(Added);

        foreach (var existingTagHelper in baseTagHelpers)
        {
            if (!Removed.Contains(existingTagHelper))
            {
                newTagHelpers.Add(existingTagHelper);
            }
        }

        return newTagHelpers;
    }
}
