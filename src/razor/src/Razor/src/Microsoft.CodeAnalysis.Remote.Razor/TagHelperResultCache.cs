// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal class TagHelperResultCache
{
    private readonly MemoryCache<string, ProjectResultCacheEntry> _projectResultCache;

    public TagHelperResultCache()
    {
        _projectResultCache = new MemoryCache<string, ProjectResultCacheEntry>(sizeLimit: 50);
    }

    public bool TryGet(string projectFilePath, int resultId, [NotNullWhen(returnValue: true)] out IReadOnlyCollection<TagHelperDescriptor>? cachedTagHelpers)
    {
        if (!_projectResultCache.TryGetValue(projectFilePath, out var cachedResult))
        {
            cachedTagHelpers = null;
            return false;
        }
        else if (cachedResult.ResultId != resultId)
        {
            // We don't know about the result that's being requested. Fallback to uncached behavior.
            cachedTagHelpers = null;
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

    public void Set(string projectFilePath, int resultId, IReadOnlyCollection<TagHelperDescriptor> tagHelpers)
    {
        var cacheEntry = new ProjectResultCacheEntry(resultId, tagHelpers);
        _projectResultCache.Set(projectFilePath, cacheEntry);
    }

    private record ProjectResultCacheEntry(int ResultId, IReadOnlyCollection<TagHelperDescriptor> Descriptors);
}
