// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Serialization;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal class RemoteTagHelperDeltaProvider
{
    private readonly TagHelperResultCache _resultCache;
    private readonly object _resultIdLock = new();
    private int _currentResultId;

    public RemoteTagHelperDeltaProvider()
    {
        _resultCache = new TagHelperResultCache();
    }

    public TagHelperDeltaResult GetTagHelpersDelta(
        ProjectId projectId,
        int lastResultId,
        ImmutableArray<TagHelperDescriptor> currentTagHelpers)
    {
        var cacheHit = _resultCache.TryGet(projectId, lastResultId, out var cachedTagHelpers);
        if (!cacheHit)
        {
            cachedTagHelpers = ImmutableArray<TagHelperDescriptor>.Empty;
        }

        var added = GetAddedTagHelpers(currentTagHelpers, cachedTagHelpers!);
        var removed = GetRemovedTagHelpers(currentTagHelpers, cachedTagHelpers!);

        lock (_resultIdLock)
        {
            var resultId = _currentResultId;
            if (added.Length > 0 || removed.Length > 0)
            {
                // The result actually changed, lets generate & cache a new result
                resultId = ++_currentResultId;
                _resultCache.Set(projectId, resultId, currentTagHelpers);
            }
            else if (cacheHit)
            {
                // Re-use existing result ID if we've hit he cache so next time we get asked we hit again.
                resultId = lastResultId;
            }

            var result = new TagHelperDeltaResult(cacheHit, resultId, added, removed);
            return result;
        }
    }

    private static ImmutableArray<TagHelperDescriptor> GetAddedTagHelpers(ImmutableArray<TagHelperDescriptor> current, ImmutableArray<TagHelperDescriptor> old)
    {
        if (old.Length == 0)
        {
            // Everything is considered added when there is no collection to compare to.
            return current;
        }

        if (current.Length == 0)
        {
            // No new descriptors so can't possibly add any
            return ImmutableArray<TagHelperDescriptor>.Empty;
        }

        using var _ = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var added);

        foreach (var tagHelper in current)
        {
            if (!old.Contains(tagHelper))
            {
                added.Add(tagHelper);
            }
        }

        return added.ToImmutable();
    }

    private static ImmutableArray<TagHelperDescriptor> GetRemovedTagHelpers(ImmutableArray<TagHelperDescriptor> current, ImmutableArray<TagHelperDescriptor> old)
    {
        if (old.Length == 0)
        {
            // Can't have anything removed if there's nothing to compare to
            return ImmutableArray<TagHelperDescriptor>.Empty;
        }

        if (current.Length == 0)
        {
            // Current collection has nothing so anything in "old" must have been removed
            return old;
        }

        using var _ = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var removed);

        foreach (var tagHelper in old)
        {
            if (!current.Contains(tagHelper))
            {
                removed.Add(tagHelper);
            }
        }

        return removed.ToImmutable();
    }
}
