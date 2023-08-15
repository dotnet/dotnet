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

        var added = TagHelperDelta.Compute(cachedTagHelpers, currentTagHelpers);
        var removed = TagHelperDelta.Compute(currentTagHelpers, cachedTagHelpers);

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
