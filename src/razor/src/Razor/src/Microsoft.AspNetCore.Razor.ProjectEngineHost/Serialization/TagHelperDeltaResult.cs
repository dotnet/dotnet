// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.Extensions.Internal;

#if DEBUG
using System.Diagnostics;
#endif

namespace Microsoft.AspNetCore.Razor.Serialization;

internal sealed record TagHelperDeltaResult(
    bool IsDelta,
    int ResultId,
    ImmutableArray<Checksum> Added,
    ImmutableArray<Checksum> Removed)
{
    public ImmutableArray<Checksum> Apply(ImmutableArray<Checksum> baseChecksums)
    {
        if (Added.Length == 0 && Removed.Length == 0)
        {
            return baseChecksums;
        }

        using var _ = ArrayBuilderPool<Checksum>.GetPooledObject(out var result);
        result.SetCapacityIfLarger(baseChecksums.Length + Added.Length - Removed.Length);

        result.AddRange(Added);
        result.AddRange(Delta.Compute(Removed, baseChecksums));

#if DEBUG
        // Ensure that there are no duplicate tag helpers in the result.
        using var pooledSet = HashSetPool<Checksum>.GetPooledObject();
        var set = pooledSet.Object;

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

        return IsDelta == other.IsDelta &&
               ResultId == other.ResultId &&
               Added.SequenceEqual(other.Added) &&
               Removed.SequenceEqual(other.Removed);
    }

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();

        hash.Add(IsDelta);
        hash.Add(ResultId);
        hash.Add(Added);
        hash.Add(Removed);

        return hash.CombinedHash;
    }
}
