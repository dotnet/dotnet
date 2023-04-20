// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
///  Wraps a pooled <see cref="ImmutableArray{T}.Builder"/> but doesn't allocate it until
///  it's needed. Note: Dispose this to ensure that the pooled array builder is returned
///  to the pool.
/// </summary>
internal ref struct PooledArrayBuilder<T>
{
    private readonly ObjectPool<ImmutableArray<T>.Builder> _pool;
    private ImmutableArray<T>.Builder? _builder;

    public PooledArrayBuilder()
        : this(ArrayBuilderPool<T>.Default)
    {
    }

    public PooledArrayBuilder(ObjectPool<ImmutableArray<T>.Builder> pool)
    {
        _pool = pool;
    }

    public void Dispose()
    {
        ClearAndFree();
    }

    public readonly int Count
        => _builder?.Count ?? 0;

    public void Add(T item)
    {
        _builder ??= _pool.Get();
        _builder.Add(item);
    }

    public void AddRange(IReadOnlyList<T> items)
    {
        if (items.Count == 0)
        {
            return;
        }

        _builder ??= _pool.Get();
        _builder.AddRange(items);
    }

    public void AddRange(IEnumerable<T> items)
    {
        _builder ??= _pool.Get();
        _builder.AddRange(items);
    }

    public void ClearAndFree()
    {
        if (_builder is { } builder)
        {
            _pool.Return(builder);
            _builder = null;
        }
    }

    /// <summary>
    ///  Returns the current contents as an <see cref="ImmutableArray{T}"/> and sets
    ///  the collection to a zero length array.
    /// </summary>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity"/> equals <see cref="Count"/>, the
    ///  internal array will be extracted as an <see cref="ImmutableArray{T}"/> without copying
    ///  the contents. Otherwise, the contents will be copied into a new array. The collection
    ///  will then be set to a zero length array.
    /// </remarks>
    /// <returns>An immutable array.</returns>
    public readonly ImmutableArray<T> DrainToImmutable()
    {
#if NET8_0_OR_GREATER
        return _builder?.DrainToImmutable() ?? ImmutableArray<T>.Empty;
#else
        if (_builder is not { } builder)
        {
            return ImmutableArray<T>.Empty;
        }

        if (builder.Count == 0)
        {
            return ImmutableArray<T>.Empty;
        }

        if (builder.Count == builder.Capacity)
        {
            return builder.MoveToImmutable();
        }

        var result = builder.ToImmutable();
        builder.Clear();
        return result;
#endif
    }

    public readonly ImmutableArray<T> ToImmutable()
        => _builder?.ToImmutable() ?? ImmutableArray<T>.Empty;

    public readonly T[] ToArray()
        => _builder?.ToArray() ?? Array.Empty<T>();
}
