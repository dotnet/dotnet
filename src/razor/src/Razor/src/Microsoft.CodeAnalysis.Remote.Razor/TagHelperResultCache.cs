// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal class TagHelperResultCache
{
    private record Entry(int ResultId, ImmutableArray<TagHelperDescriptor> Descriptors);

    private readonly MemoryCache<string, Entry> _projectResultCache;

    public TagHelperResultCache()
    {
        _projectResultCache = new MemoryCache<string, Entry>(sizeLimit: 50);
    }

    public bool TryGet(string projectFilePath, int resultId, out ImmutableArray<TagHelperDescriptor> cachedTagHelpers)
    {
        if (!_projectResultCache.TryGetValue(projectFilePath, out var cachedResult))
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

        cachedTagHelpers = cachedResult.Descriptors;
        return true;
    }

    public bool TryGetId(string projectFilePath, out int resultId)
    {
        if (!_projectResultCache.TryGetValue(projectFilePath, out var cachedResult))
        {
            resultId = -1;
            return false;
        }

        resultId = cachedResult.ResultId;
        return true;
    }

    public void Set(string projectFilePath, int resultId, ImmutableArray<TagHelperDescriptor> tagHelpers)
    {
        var cacheEntry = new Entry(resultId, tagHelpers);
        _projectResultCache.Set(projectFilePath, cacheEntry);
    }
}
