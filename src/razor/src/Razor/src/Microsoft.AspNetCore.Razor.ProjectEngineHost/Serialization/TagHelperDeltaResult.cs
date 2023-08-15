// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.Extensions.Internal;

#if DEBUG
using System.Collections.Generic;
using System.Diagnostics;
#endif

namespace Microsoft.AspNetCore.Razor.Serialization;

internal sealed record TagHelperDeltaResult(
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
        using var _ = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var result);
        result.SetCapacityIfLarger(baseTagHelpers.Length + Added.Length - Removed.Length);

        result.AddRange(Added);
        result.AddRange(TagHelperDelta.Compute(Removed, baseTagHelpers));

#if DEBUG
        // Ensure that there are no duplicate tag helpers in the result.
        var set = new HashSet<TagHelperDescriptor>(TagHelperChecksumComparer.Instance);

        foreach (var item in result)
        {
            Debug.Assert(set.Add(item), $"{nameof(TagHelperDeltaResult)}.{nameof(Apply)} should not contain any duplicates!");
        }
#endif

        return result.DrainToImmutable();
    }

    public bool Equals(TagHelperDeltaResult? other)
    {
        if (other is null)
        {
            return false;
        }

        return Delta == other.Delta &&
               ResultId == other.ResultId &&
               Added.SequenceEqual(other.Added, TagHelperChecksumComparer.Instance) &&
               Removed.SequenceEqual(other.Removed, TagHelperChecksumComparer.Instance);
    }

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();

        hash.Add(Delta);
        hash.Add(ResultId);
        hash.Add(Added, TagHelperChecksumComparer.Instance);
        hash.Add(Removed, TagHelperChecksumComparer.Instance);

        return hash.CombinedHash;
    }
}
