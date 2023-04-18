// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
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
    private HashSet<T>? _set;

    public PooledHashSet()
        : this(HashSetPool<T>.Default)
    {
    }

    public PooledHashSet(ObjectPool<HashSet<T>> pool)
    {
        _pool = pool;
    }

    public void Dispose()
    {
        ClearAndFree();
    }

    public int Count
        => _set?.Count ?? 0;

    public bool Add(T item)
    {
        _set ??= _pool.Get();
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

    public void UnionWith(IList<T>? other)
    {
        if (other?.Count > 0)
        {
            _set ??= _pool.Get();
            _set.UnionWith(other);
        }
    }
}
