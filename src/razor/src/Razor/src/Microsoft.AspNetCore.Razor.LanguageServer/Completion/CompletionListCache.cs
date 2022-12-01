// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal sealed class CompletionListCache
{
    // Internal for testing
    internal static readonly int MaxCacheSize = 10;

    private readonly object _accessLock;
    private readonly List<CacheEntry> _resultIdToCacheEntry;
    private int _nextResultId;

    public CompletionListCache()
    {
        _accessLock = new object();
        _resultIdToCacheEntry = new List<CacheEntry>();
    }

    public int Set(VSInternalCompletionList completionList, object? context)
    {
        if (completionList is null)
        {
            throw new ArgumentNullException(nameof(completionList));
        }

        lock (_accessLock)
        {
            // If cache exceeds maximum size, remove the oldest list in the cache
            if (_resultIdToCacheEntry.Count >= MaxCacheSize)
            {
                _resultIdToCacheEntry.RemoveAt(0);
            }

            var resultId = _nextResultId++;
            var cacheEntry = new CacheEntry(resultId, completionList, context);
            _resultIdToCacheEntry.Add(cacheEntry);

            // Return generated resultId so completion list can later be retrieved from cache
            return resultId;
        }
    }

    public bool TryGet(int resultId, [NotNullWhen(returnValue: true)] out CacheEntry? cachedEntry)
    {
        lock (_accessLock)
        {
            // Search back -> front because the items in the back are the most recently added which are most frequently accessed.
            for (var i = _resultIdToCacheEntry.Count - 1; i >= 0; i--)
            {
                var entry = _resultIdToCacheEntry[i];
                if (entry.ResultId == resultId)
                {
                    cachedEntry = entry;
                    return true;
                }
            }

            // A cache entry associated with the given resultId was not found
            cachedEntry = null;
            return false;
        }
    }

    public record CacheEntry(int ResultId, VSInternalCompletionList CompletionList, object? Context);
}
