// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal class RemoteTagHelperDeltaProvider
{
    private readonly TagHelperResultCache _resultCache = new();
    private readonly object _gate = new();
    private int _currentResultId;

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

        ImmutableArray<TagHelperDescriptor> added;
        ImmutableArray<TagHelperDescriptor> removed;

        if (cachedTagHelpers.Length < currentTagHelpers.Length)
        {
            added = TagHelperDelta.Compute(cachedTagHelpers, currentTagHelpers);

            // No need to call TagHelperDelta.Compute again if we know there aren't any removed
            var anyRemoved = currentTagHelpers.Length - cachedTagHelpers.Length != added.Length;
            removed = anyRemoved ? TagHelperDelta.Compute(currentTagHelpers, cachedTagHelpers) : ImmutableArray<TagHelperDescriptor>.Empty;
        }
        else
        {
            removed = TagHelperDelta.Compute(currentTagHelpers, cachedTagHelpers);

            // No need to call TagHelperDelta.Compute again if we know there aren't any added
            var anyAdded = cachedTagHelpers.Length - currentTagHelpers.Length != removed.Length;
            added = anyAdded ? TagHelperDelta.Compute(cachedTagHelpers, currentTagHelpers) : ImmutableArray<TagHelperDescriptor>.Empty;
        }

        lock (_gate)
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
}
