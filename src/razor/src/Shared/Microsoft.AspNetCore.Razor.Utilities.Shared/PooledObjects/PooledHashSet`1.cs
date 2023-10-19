// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
///  Wraps a pooled <see cref="HashSet{T}"/> but doesn't allocate it until
///  it's needed. Note: Dispose this to ensure that the pooled set is returned
///  to the pool.
/// </summary>
internal ref struct PooledHashSet<T>
{
    private readonly ObjectPool<HashSet<T>> _pool;
    private readonly int? _capacity;
    private HashSet<T>? _set;

    public PooledHashSet()
        : this(pool: null, capacity: null)
    {
    }

    public PooledHashSet(ObjectPool<HashSet<T>> pool)
        : this(pool, capacity: null)
    {
    }

    public PooledHashSet(int capacity)
        : this(pool: null, capacity)
    {
    }

    public PooledHashSet(ObjectPool<HashSet<T>>? pool, int? capacity)
    {
        _pool = pool ?? HashSetPool<T>.Default;
        _capacity = capacity;
    }

    public void Dispose()
    {
        ClearAndFree();
    }

    public readonly int Count
        => _set?.Count ?? 0;

    public bool Add(T item)
    {
        _set ??= GetHashSet();
        return _set.Add(item);
    }

    public void ClearAndFree()
    {
        if (_set is { } set)
        {
            _pool.Return(set);
            _set = null;
        }
    }

    public readonly bool Contains(T item)
        => _set?.Contains(item) ?? false;

    public readonly T[] ToArray()
        => _set?.ToArray() ?? Array.Empty<T>();

    public readonly ImmutableArray<T> ToImmutableArray()
        => _set?.ToImmutableArray() ?? ImmutableArray<T>.Empty;

    public void UnionWith(IList<T>? other)
    {
        if (other?.Count > 0)
        {
            _set ??= GetHashSet();
            _set.UnionWith(other);
        }
    }

    private readonly HashSet<T> GetHashSet()
    {
        var result = _pool.Get();

#if NET
        if (_capacity is int capacity)
        {
            result.EnsureCapacity(capacity);
        }
#endif

        return result;
    }
}
