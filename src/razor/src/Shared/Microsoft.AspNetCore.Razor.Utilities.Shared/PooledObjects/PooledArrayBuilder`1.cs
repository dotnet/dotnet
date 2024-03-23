// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
///  Wraps a pooled <see cref="ImmutableArray{T}.Builder"/> but doesn't allocate it until
///  it's needed. Note: Dispose this to ensure that the pooled array builder is returned
///  to the pool.
///
///  There is significant effort to avoid retrieving the <see cref="ImmutableArray{T}.Builder"/>.
///  For very small arrays of length 4 or less, the elements will be stored on the stack. If the array
///  grows larger than 4 elements, a builder will be employed. Afterward, the build will
///  continue to be used, even if the arrays shrinks and has fewer than 4 elements.
/// </summary>
[NonCopyable]
[CollectionBuilder(typeof(PooledArrayBuilder), nameof(PooledArrayBuilder.Create))]
internal partial struct PooledArrayBuilder<T> : IDisposable
{
    public static PooledArrayBuilder<T> Empty => default;

    /// <summary>
    ///  The number of items that can be stored inline.
    /// </summary>
    private const int InlineCapacity = 4;

    /// <summary>
    ///  A builder to be used as storage after the first time that the number
    ///  of items exceeds <see cref="InlineCapacity"/>. Once the builder is used,
    ///  it is still used even if the number of items shrinks below <see cref="InlineCapacity"/>.
    ///  Essentially, if this field is non-null, it will be used as storage.
    /// </summary>
    private ImmutableArray<T>.Builder? _builder;

    /// <summary>
    ///  An optional initial capacity for the builder.
    /// </summary>
    private int? _capacity;

    private T _element0;
    private T _element1;
    private T _element2;
    private T _element3;

    /// <summary>
    ///  The number of inline elements. Note that this value is only used when <see cref="_builder"/> is <see langword="null"/>.
    /// </summary>
    private int _inlineCount;

    public PooledArrayBuilder(int? capacity = null)
    {
        _capacity = capacity is > InlineCapacity ? capacity : null;
        _element0 = default!;
        _element1 = default!;
        _element2 = default!;
        _element3 = default!;
        _inlineCount = 0;
    }

    private PooledArrayBuilder(in PooledArrayBuilder<T> builder)
    {
        // This is an intentional copy used to create an Enumerator.
#pragma warning disable RS0042
        this = builder;
#pragma warning restore RS0042
    }

    public void Dispose()
    {
        // Return _builder to the pool if necessary. Note that we don't need to clear the inline elements here
        // because this type is intended to be allocated on the stack and the GC can reclaim objects from the
        // stack after the last use of a reference to them.
        if (_builder is { } builder)
        {
            ArrayBuilderPool<T>.Default.Return(builder);
            _builder = null;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ClearInlineElement(int index)
    {
        Debug.Assert(_inlineCount <= InlineCapacity);

        // Clearing out an item makes it potentially available for garbage collection.
        // Note: On .NET Core, we can be a bit more judicious and only zero-out
        // fields that contain references to heap-allocated objects.

#if NETCOREAPP
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
#endif
        {
            SetInlineElement(index, default!);
        }
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get
        {
            if (_builder is { } builder)
            {
                return builder[index];
            }

            if (index >= _inlineCount)
            {
                ThrowIndexOutOfRangeException();
            }

            return GetInlineElement(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (_builder is { } builder)
            {
                builder[index] = value;
                return;
            }

            if (index >= _inlineCount)
            {
                ThrowIndexOutOfRangeException();
            }

            SetInlineElement(index, value);
        }
    }

    public T this[Index index]
    {
        readonly get => this[index.GetOffset(Count)];
        set => this[index.GetOffset(Count)] = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly T GetInlineElement(int index)
    {
        Debug.Assert(_inlineCount <= InlineCapacity);

        return index switch
        {
            0 => _element0,
            1 => _element1,
            2 => _element2,
            3 => _element3,
            _ => Assumed.Unreachable<T>()
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetInlineElement(int index, T value)
    {
        Debug.Assert(_inlineCount <= InlineCapacity);

        switch (index)
        {
            case 0:
                _element0 = value;
                break;

            case 1:
                _element1 = value;
                break;

            case 2:
                _element2 = value;
                break;

            case 3:
                _element3 = value;
                break;

            default:
                Assumed.Unreachable();
                break;
        }
    }

    public readonly int Count
        => _builder?.Count ?? _inlineCount;

    public void Add(T item)
    {
        if (_builder is { } builder)
        {
            builder.Add(item);
        }
        else if (_inlineCount < InlineCapacity)
        {
            SetInlineElement(_inlineCount, item);
            _inlineCount++;
        }
        else
        {
            Debug.Assert(_inlineCount == InlineCapacity);
            MoveInlineItemsToBuilder();
            _builder.Add(item);
        }
    }

    public void AddRange(ImmutableArray<T> items)
    {
        AddRange(items.AsSpan());
    }

    // Necessary to avoid conflict with AddRange(IEnumerable<T>) and AddRange(ReadOnlySpan<T>).
    public void AddRange(T[] items)
    {
        AddRange(items.AsSpan());
    }

    public void AddRange(ReadOnlySpan<T> items)
    {
        if (items.IsEmpty)
        {
            return;
        }

        if (_builder is { } builder)
        {
            builder.AddRange(items);
        }
        else if (_inlineCount + items.Length <= InlineCapacity)
        {
            foreach (var item in items)
            {
                SetInlineElement(_inlineCount, item);
                _inlineCount++;
            }
        }
        else
        {
            MoveInlineItemsToBuilder();
            _builder.AddRange(items);
        }
    }

    public void AddRange(IEnumerable<T> items)
    {
        if (_builder is { } builder)
        {
            builder.AddRange(items);
            return;
        }

        if (!items.TryGetCount(out var count))
        {
            // We couldn't retrieve a count, so we have to enumerate the elements.
            foreach (var item in items)
            {
                Add(item);
            }

            return;
        }

        if (count == 0)
        {
            // No items, so there's nothing to do.
            return;
        }

        if (_inlineCount + count <= InlineCapacity)
        {
            // The items can fit into our inline elements.
            foreach (var item in items)
            {
                SetInlineElement(_inlineCount, item);
                _inlineCount++;
            }
        }
        else
        {
            // The items can't fit into our inline elements, so we switch to a builder.
            MoveInlineItemsToBuilder();
            _builder.AddRange(items);
        }
    }

    public void Clear()
    {
        if (_builder is { } builder)
        {
            // Keep using a real builder to avoid churn in the object pool.
            builder.Clear();
        }
        else
        {
            var oldCapacity = _capacity;
            this = Empty;
            _capacity = oldCapacity;
        }
    }

    public readonly Enumerator GetEnumerator()
        => new(in this);

    public void RemoveAt(int index)
    {
        if (_builder is { } builder)
        {
            builder.RemoveAt(index);
            return;
        }

        if (index < 0 || index >= _inlineCount)
        {
            ThrowIndexOutOfRangeException();
        }

        // Copy inline elements depending on the index to be removed.
        switch (index)
        {
            case 0:
                _element0 = _element1;

                if (_inlineCount > 1)
                {
                    _element1 = _element2;
                }

                if (_inlineCount > 2)
                {
                    _element2 = _element3;
                }

                break;

            case 1:
                _element1 = _element2;

                if (_inlineCount > 2)
                {
                    _element2 = _element3;
                }

                break;

            case 2:
                _element2 = _element3;
                break;
        }

        // Clear the last element if necessary.
        if (_inlineCount > 3)
        {
            ClearInlineElement(3);
        }

        // Decrement the count.
        _inlineCount--;
    }

    public void RemoveAt(Index index)
        => RemoveAt(index.GetOffset(Count));

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
    public ImmutableArray<T> DrainToImmutable()
    {
        if (_builder is { } builder)
        {
            return builder.DrainToImmutable();
        }

        var inlineArray = InlineItemsToImmutableArray();

        var oldCapacity = _capacity;
        this = Empty;
        _capacity = oldCapacity;

        return inlineArray;
    }

    public readonly ImmutableArray<T> ToImmutable()
    {
        if (_builder is { } builder)
        {
            return builder.ToImmutable();
        }

        return InlineItemsToImmutableArray();
    }

    private readonly ImmutableArray<T> InlineItemsToImmutableArray()
    {
        Debug.Assert(_inlineCount <= InlineCapacity);

        return _inlineCount switch
        {
            0 => ImmutableArray<T>.Empty,
            1 => ImmutableArray.Create(_element0),
            2 => ImmutableArray.Create(_element0, _element1),
            3 => ImmutableArray.Create(_element0, _element1, _element2),
            _ => ImmutableArray.Create(_element0, _element1, _element2, _element3)
        };
    }

    public readonly T[] ToArray()
    {
        if (_builder is { } builder)
        {
            return builder.ToArray();
        }

        return _inlineCount switch
        {
            0 => [],
            1 => new[] { _element0 },
            2 => new[] { _element0, _element1 },
            3 => new[] { _element0, _element1, _element2 },
            _ => new[] { _element0, _element1, _element2, _element3 }
        };
    }

    public bool Any() => Count > 0;

    public void Push(T item)
    {
        Add(item);
    }

    public readonly T Peek()
    {
        return this[^1];
    }

    public T Pop()
    {
        var index = ^1;
        var item = this[index];
        RemoveAt(index);

        return item;
    }

    public bool TryPop([MaybeNullWhen(false)] out T item)
    {
        if (Count == 0)
        {
            item = default;
            return false;
        }

        item = Pop();
        return true;
    }

    /// <summary>
    ///  This is present to help the JIT inline methods that need to throw
    ///  a <see cref="IndexOutOfRangeException"/>.
    /// </summary>
    [DoesNotReturn]
    private static void ThrowIndexOutOfRangeException()
        => throw new IndexOutOfRangeException();

    [MemberNotNull(nameof(_builder))]
    private void MoveInlineItemsToBuilder()
    {
        Debug.Assert(_builder is null);

        var builder = ArrayBuilderPool<T>.Default.Get();

        if (_capacity is int capacity)
        {
            builder.SetCapacityIfLarger(capacity);
        }

        _builder = builder;

        // Add the inline items and clear their field values.
        for (var i = 0; i < _inlineCount; i++)
        {
            builder.Add(GetInlineElement(i));
            ClearInlineElement(i);
        }

        // Since _inlineCount tracks the number of inline items used, we zero it out here.
        _inlineCount = 0;
    }
}
