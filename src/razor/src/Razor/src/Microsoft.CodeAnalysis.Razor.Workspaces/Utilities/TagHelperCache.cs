// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed class TagHelperCache
{
    public static readonly TagHelperCache Default = new();

    private readonly Dictionary<Checksum, WeakReference<TagHelperDescriptor>> _checksumToTagHelperMap = new();

    private const int CleanUpThreshold = 200;
    private int _addsSinceLastCleanUp;

    public TagHelperCache()
    {
    }

    public TagHelperDescriptor GetOrAdd(Checksum checksum, TagHelperDescriptor tagHelper)
    {
        lock (_checksumToTagHelperMap)
        {
            // Note: This returns null if tagHelper was added to the cache.
            return TryAddOrGet_NoLock(checksum, tagHelper) ?? tagHelper;
        }
    }

    public bool TryAdd(Checksum checksum, TagHelperDescriptor tagHelper)
    {
        lock (_checksumToTagHelperMap)
        {
            // Note: This returns null if tagHelper was added to the cache.
            return TryAddOrGet_NoLock(checksum, tagHelper) is null;
        }
    }

    /// <summary>
    ///  Try to add the given tag helper to the cache. If it already exists, return the cached instance.
    /// </summary>
    private TagHelperDescriptor? TryAddOrGet_NoLock(Checksum checksum, TagHelperDescriptor tagHelper)
    {
        if (++_addsSinceLastCleanUp >= CleanUpThreshold)
        {
            CleanUpDeadObjects_NoLock();
        }

        if (!_checksumToTagHelperMap.TryGetValue(checksum, out var weakRef))
        {
            _checksumToTagHelperMap.Add(checksum, new(tagHelper));
            return null;
        }

        if (!weakRef.TryGetTarget(out var cachedTagHelper))
        {
            weakRef.SetTarget(tagHelper);
            return null;
        }

        return cachedTagHelper;
    }

    public bool TryGet(Checksum checksum, [NotNullWhen(true)] out TagHelperDescriptor? tagHelper)
    {
        lock (_checksumToTagHelperMap)
        {
            if (_checksumToTagHelperMap.TryGetValue(checksum, out var weakRef) &&
                weakRef.TryGetTarget(out tagHelper))
            {
                return true;
            }

            tagHelper = null;
            return false;
        }
    }

    private void CleanUpDeadObjects_NoLock()
    {
        using var deadChecksums = new PooledArrayBuilder<Checksum>();

        foreach (var (checksum, weakRef) in _checksumToTagHelperMap)
        {
            if (!weakRef.TryGetTarget(out _))
            {
                deadChecksums.Add(checksum);
            }
        }

        foreach (var checksum in deadChecksums)
        {
            _checksumToTagHelperMap.Remove(checksum);
        }

        _addsSinceLastCleanUp = 0;
    }
}
