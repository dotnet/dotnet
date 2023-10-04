// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Utilities;

// We've created our own MemoryCache here, ideally we would use the one in Microsoft.Extensions.Caching.Memory,
// but until we update O# that causes an Assembly load problem.
internal class MemoryCache<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    private const int DefaultSizeLimit = 50;
    private const int DefaultConcurrencyLevel = 2;

    protected IDictionary<TKey, CacheEntry> _dict;

    private readonly object _compactLock;
    private readonly int _sizeLimit;

    public MemoryCache(int sizeLimit = DefaultSizeLimit, int concurrencyLevel = DefaultConcurrencyLevel)
    {
        _sizeLimit = sizeLimit;
        _dict = new ConcurrentDictionary<TKey, CacheEntry>(concurrencyLevel, capacity: _sizeLimit);
        _compactLock = new object();
    }

    public bool TryGetValue(TKey key, [NotNullWhen(returnValue: true)] out TValue? result)
    {
        if (_dict.TryGetValue(key, out var value))
        {
            value.LastAccess = DateTime.UtcNow;
            result = value.Value;
            return true;
        }

        result = default;
        return false;
    }

    public void Set(TKey key, TValue value)
    {
        lock (_compactLock)
        {
            if (_dict.Count >= _sizeLimit)
            {
                Compact();
            }
        }

        _dict[key] = new CacheEntry
        {
            LastAccess = DateTime.UtcNow,
            Value = value,
        };
    }

    public void Clear() => _dict.Clear();

    protected virtual void Compact()
    {
        var kvps = _dict.OrderBy(x => x.Value.LastAccess).ToArray();

        for (var i = 0; i < _sizeLimit / 2; i++)
        {
            _dict.Remove(kvps[i].Key);
        }
    }

    protected class CacheEntry
    {
        public required TValue Value { get; init; }

        public required DateTime LastAccess { get; set; }
    }
}
