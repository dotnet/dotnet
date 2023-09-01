// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
///  Wraps a pooled <see cref="ImmutableArray{T}.Builder"/> but doesn't allocate it until
///  it's needed. Note: Dispose this to ensure that the pooled array builder is returned
///  to the pool.
/// </summary>
internal ref partial struct PooledArrayBuilder<T>
{
    private readonly ObjectPool<ImmutableArray<T>.Builder> _pool;
    private readonly int? _capacity;
    private ImmutableArray<T>.Builder? _builder;

    public PooledArrayBuilder()
        : this(null, null)
    {
    }

    public PooledArrayBuilder(ObjectPool<ImmutableArray<T>.Builder>? pool = null, int? capacity = null)
    {
        _pool = pool ?? ArrayBuilderPool<T>.Default;
        _capacity = capacity;
    }

    public readonly T this[int i]
    {
        get
        {
            if (_builder is null || Count <= i)
            {
                throw new IndexOutOfRangeException();
            }

            return _builder[i];
        }
        set
        {
            if (_builder is null || Count <= i)
            {
                throw new IndexOutOfRangeException();
            }

            _builder[i] = value;
        }
    }

    public void Dispose()
    {
        ClearAndFree();
    }

    public readonly int Count
        => _builder?.Count ?? 0;

    public void Add(T item)
    {
        _builder ??= GetBuilder();
        _builder.Add(item);
    }

    public void AddRange(ImmutableArray<T> items)
    {
        if (items.Length == 0)
        {
            return;
        }

        _builder ??= GetBuilder();
        _builder.AddRange(items);
    }

    public void AddRange(IReadOnlyList<T> items)
    {
        if (items.Count == 0)
        {
            return;
        }

        _builder ??= GetBuilder();
        _builder.AddRange(items);
    }

    public void AddRange(IEnumerable<T> items)
    {
        _builder ??= GetBuilder();
        _builder.AddRange(items);
    }

    public readonly Enumerator GetEnumerator()
        => _builder is { } builder
            ? new Enumerator(builder)
            : default;

    public void ClearAndFree()
    {
        if (_builder is { } builder)
        {
            _pool.Return(builder);
            _builder = null;
        }
    }

    public readonly void RemoveAt(int index)
    {
        if (_builder is null || Count <= index)
        {
            throw new IndexOutOfRangeException();
        }

        _builder.RemoveAt(index);
    }

    private readonly ImmutableArray<T>.Builder GetBuilder()
    {
        var result = _pool.Get();
        if (_capacity is int capacity)
        {
            result.SetCapacityIfLarger(capacity);
        }

        return result;
    }

    /// <summary>
    ///  Returns the current contents as an <see cref="ImmutableArray{T}"/> and sets
    ///  the collection to a zero length array.
    /// </summary>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity"/> equals <see cref="Count"/>, the
    ///  internal array will be extracted as an <see cref="ImmutableArray{T}"/> without copying
    ///  the contents. Otherwise, the contents will be copied into a new array. The collection
    ///  will then be set to a zero-length array.
    /// </remarks>
    /// <returns>An immutable array.</returns>
    public readonly ImmutableArray<T> DrainToImmutable()
        => _builder?.DrainToImmutable() ?? ImmutableArray<T>.Empty;

    public readonly ImmutableArray<T> ToImmutable()
        => _builder?.ToImmutable() ?? ImmutableArray<T>.Empty;

    public readonly T[] ToArray()
        => _builder?.ToArray() ?? Array.Empty<T>();

    public void Push(T item)
    {
        this.Add(item);
    }

    public readonly T Peek()
    {
        return this[^1];
    }

    public readonly T Pop()
    {
        var item = this[^1];
        RemoveAt(Count - 1);
        return item;
    }

    public readonly bool TryPop([MaybeNullWhen(false)] out T item)
    {
        if (Count == 0)
        {
            item = default;
            return false;
        }

        item = Pop();
        return true;
    }
}
