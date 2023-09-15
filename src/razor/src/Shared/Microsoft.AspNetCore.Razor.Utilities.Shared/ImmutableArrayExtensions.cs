// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace System.Collections.Immutable;

/// <summary>
/// <see cref="ImmutableArray{T}"/> extension methods
/// </summary>
internal static class ImmutableArrayExtensions
{
    /// <summary>
    /// Returns an empty array if the input array is null (default)
    /// </summary>
    public static ImmutableArray<T> NullToEmpty<T>(this ImmutableArray<T> array)
    {
        return array.IsDefault ? ImmutableArray<T>.Empty : array;
    }

    public static void SetCapacityIfLarger<T>(this ImmutableArray<T>.Builder builder, int newCapacity)
    {
        if (builder.Capacity < newCapacity)
        {
            builder.Capacity = newCapacity;
        }
    }

    /// <summary>
    ///  Returns the current contents as an <see cref="ImmutableArray{T}"/> and sets
    ///  the collection to a zero length array.
    /// </summary>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity"/> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count"/>, the internal array will be extracted
    ///  as an <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the
    ///  contents will be copied into a new array. The collection will then be set to a
    ///  zero-length array.
    /// </remarks>
    /// <returns>An immutable array.</returns>
    public static ImmutableArray<T> DrainToImmutable<T>(this ImmutableArray<T>.Builder builder)
    {
#if NET8_0_OR_GREATER
        return builder.DrainToImmutable();
#else
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

    public static ImmutableArray<TResult> SelectAsArray<T, TResult>(this ImmutableArray<T> source, Func<T, TResult> selector)
    {
        return source switch
        {
            [] => ImmutableArray<TResult>.Empty,
            [var item] => ImmutableArray.Create(selector(item)),
            [var item1, var item2] => ImmutableArray.Create(selector(item1), selector(item2)),
            [var item1, var item2, var item3] => ImmutableArray.Create(selector(item1), selector(item2), selector(item3)),
            [var item1, var item2, var item3, var item4] => ImmutableArray.Create(selector(item1), selector(item2), selector(item3), selector(item4)),
            var items => BuildResult(items, selector)
        };

        static ImmutableArray<TResult> BuildResult(ImmutableArray<T> items, Func<T, TResult> selector)
        {
            using var results = new PooledArrayBuilder<TResult>(capacity: items.Length);

            foreach (var item in items)
            {
                results.Add(selector(item));
            }

            return results.DrainToImmutable();
        }
    }

    public static ImmutableArray<TResult> SelectManyAsArray<TSource, TResult>(this IReadOnlyCollection<TSource>? source, Func<TSource, ImmutableArray<TResult>> selector)
    {
        if (source is null || source.Count == 0)
        {
            return ImmutableArray<TResult>.Empty;
        }

        using var builder = new PooledArrayBuilder<TResult>(capacity: source.Count);
        foreach (var item in source)
        {
            builder.AddRange(selector(item));
        }

        return builder.DrainToImmutable();
    }
}
