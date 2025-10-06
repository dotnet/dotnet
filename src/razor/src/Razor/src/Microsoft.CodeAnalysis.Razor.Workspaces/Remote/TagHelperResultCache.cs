// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal class TagHelperResultCache
{
    private record Entry(int ResultId, ImmutableArray<Checksum> Checksums);

    private readonly MemoryCache<ProjectId, Entry> _projectResultCache;

    public TagHelperResultCache()
    {
        _projectResultCache = new MemoryCache<ProjectId, Entry>(sizeLimit: 50);
    }

    public bool TryGet(ProjectId projectKey, int resultId, out ImmutableArray<Checksum> cachedTagHelpers)
    {
        if (!_projectResultCache.TryGetValue(projectKey, out var cachedResult))
        {
            cachedTagHelpers = default;
            return false;
        }
        else if (cachedResult.ResultId != resultId)
        {
            // We don't know about the result that's being requested. Fallback to uncached behavior.
            cachedTagHelpers = default;
            return false;
        }

        cachedTagHelpers = cachedResult.Checksums;
        return true;
    }

    public bool TryGetId(ProjectId projectKey, out int resultId)
    {
        if (!_projectResultCache.TryGetValue(projectKey, out var cachedResult))
        {
            resultId = -1;
            return false;
        }

        resultId = cachedResult.ResultId;
        return true;
    }

    public void Set(ProjectId projectKey, int resultId, ImmutableArray<Checksum> tagHelpers)
    {
        var cacheEntry = new Entry(resultId, tagHelpers);
        _projectResultCache.Set(projectKey, cacheEntry);
    }
}
