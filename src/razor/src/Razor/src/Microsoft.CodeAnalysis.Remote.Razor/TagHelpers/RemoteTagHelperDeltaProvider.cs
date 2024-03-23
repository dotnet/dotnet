// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Remote;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(RemoteTagHelperDeltaProvider)), Shared]
internal class RemoteTagHelperDeltaProvider
{
    private readonly TagHelperResultCache _resultCache = new();
    private readonly object _gate = new();
    private int _currentResultId;

    public TagHelperDeltaResult GetTagHelpersDelta(
        ProjectId projectId,
        int lastResultId,
        ImmutableArray<Checksum> currentChecksums)
    {
        var cacheHit = _resultCache.TryGet(projectId, lastResultId, out var cachedChecksums);

        if (!cacheHit)
        {
            cachedChecksums = ImmutableArray<Checksum>.Empty;
        }

        ImmutableArray<Checksum> added;
        ImmutableArray<Checksum> removed;

        if (cachedChecksums.Length < currentChecksums.Length)
        {
            added = Delta.Compute(cachedChecksums, currentChecksums);

            // No need to call TagHelperDelta.Compute again if we know there aren't any removed
            var anyRemoved = currentChecksums.Length - cachedChecksums.Length != added.Length;
            removed = anyRemoved ? Delta.Compute(currentChecksums, cachedChecksums) : ImmutableArray<Checksum>.Empty;
        }
        else
        {
            removed = Delta.Compute(currentChecksums, cachedChecksums);

            // No need to call TagHelperDelta.Compute again if we know there aren't any added
            var anyAdded = cachedChecksums.Length - currentChecksums.Length != removed.Length;
            added = anyAdded ? Delta.Compute(cachedChecksums, currentChecksums) : ImmutableArray<Checksum>.Empty;
        }

        lock (_gate)
        {
            var resultId = _currentResultId;
            if (added.Length > 0 || removed.Length > 0)
            {
                // The result actually changed, lets generate & cache a new result
                resultId = ++_currentResultId;
                _resultCache.Set(projectId, resultId, currentChecksums);
            }
            else if (cacheHit)
            {
                // Re-use existing result ID if we've hit he cache so next time we get asked we hit again.
                resultId = lastResultId;
            }

            return new TagHelperDeltaResult(cacheHit, resultId, added, removed);
        }
    }
}
