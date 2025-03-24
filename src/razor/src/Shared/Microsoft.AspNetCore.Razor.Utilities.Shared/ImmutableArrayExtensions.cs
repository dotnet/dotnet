// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;

namespace System.Collections.Immutable;

/// <summary>
/// <see cref="ImmutableArray{T}"/> extension methods
/// </summary>
internal static partial class ImmutableArrayExtensions
{
    /// <summary>
    /// Returns an empty array if the input array is null (default)
    /// </summary>
    public static ImmutableArray<T> NullToEmpty<T>(this ImmutableArray<T> array)
    {
        return array.IsDefault ? [] : array;
    }

    public static void SetCapacityIfLarger<T>(this ImmutableArray<T>.Builder builder, int newCapacity)
    {
        if (builder.Capacity < newCapacity)
        {
            builder.Capacity = newCapacity;
        }
    }

    public static ImmutableArray<TResult> SelectAsArray<T, TResult>(this ImmutableArray<T> source, Func<T, TResult> selector)
    {
        return source switch
        {
            [] => [],
            [var item] => [selector(item)],
            [var item1, var item2] => [selector(item1), selector(item2)],
            [var item1, var item2, var item3] => [selector(item1), selector(item2), selector(item3)],
            [var item1, var item2, var item3, var item4] => [selector(item1), selector(item2), selector(item3), selector(item4)],
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
            return [];
        }

        using var builder = new PooledArrayBuilder<TResult>(capacity: source.Count);
        foreach (var item in source)
        {
            builder.AddRange(selector(item));
        }

        return builder.DrainToImmutable();
    }

    public static ImmutableArray<T> WhereAsArray<T>(this ImmutableArray<T> source, Func<T, bool> predicate)
    {
        if (source is [])
        {
            return [];
        }

        using var builder = new PooledArrayBuilder<T>();

        foreach (var item in source)
        {
            if (predicate(item))
            {
                builder.Add(item);
            }
        }

        return builder.DrainToImmutable();
    }

    /// <summary>
    ///  Returns an <see cref="ImmutableArray{T}"/> that contains no duplicates from the <paramref name="source"/> array
    ///  and returns the most recent copy of each item.
    /// </summary>
    /// <param name="source">The array to process.</param>
    public static ImmutableArray<T> GetMostRecentUniqueItems<T>(this ImmutableArray<T> source)
    {
        if (source.IsEmpty)
        {
            return [];
        }

        using var _ = HashSetPool<T>.GetPooledObject(out var uniqueItems);

        return source.GetMostRecentUniqueItems(uniqueItems);
    }

    /// <summary>
    ///  Returns an <see cref="ImmutableArray{T}"/> that contains no duplicates from the <paramref name="source"/> array
    ///  and returns the most recent copy of each item.
    /// </summary>
    /// <param name="source">The array to process.</param>
    /// <param name="comparer">
    ///  A comparer to use for uniqueness.
    /// </param>
    public static ImmutableArray<T> GetMostRecentUniqueItems<T>(this ImmutableArray<T> source, IEqualityComparer<T> comparer)
    {
        if (source.IsEmpty)
        {
            return [];
        }

#if !NETSTANDARD2_0
        var uniqueItems = new HashSet<T>(capacity: source.Length, comparer);
#else
        var uniqueItems = new HashSet<T>(comparer);
#endif

        return source.GetMostRecentUniqueItems(uniqueItems);
    }

    /// <summary>
    ///  Returns an <see cref="ImmutableArray{T}"/> that contains no duplicates from the <paramref name="source"/> array
    ///  and returns the most recent copy of each item.
    /// </summary>
    /// <param name="source">The array to process.</param>
    /// <param name="uniqueItems">
    ///  An empty <see cref="HashSet{T}"/> to use for uniqueness.
    ///  Note that this may still contain items after processing.
    /// </param>
    public static ImmutableArray<T> GetMostRecentUniqueItems<T>(this ImmutableArray<T> source, HashSet<T> uniqueItems)
    {
        if (source.IsEmpty)
        {
            return [];
        }

        return GetMostRecentUniqueItemsCore(source, uniqueItems);
    }

    private static ImmutableArray<T> GetMostRecentUniqueItemsCore<T>(ImmutableArray<T> source, HashSet<T> uniqueItems)
    {
        Debug.Assert(uniqueItems.Count == 0, $"{nameof(uniqueItems)} should be empty!");

        using var stack = new PooledArrayBuilder<T>(capacity: source.Length);

        // Walk the next batch in reverse to identify unique items.
        // We push them on a stack so that we can pop them in order later
        for (var i = source.Length - 1; i >= 0; i--)
        {
            var item = source[i];

            if (uniqueItems.Add(item))
            {
                stack.Push(item);
            }
        }

        // Did we actually dedupe anything? If not, just return the original.
        if (stack.Count == source.Length)
        {
            return source;
        }

        using var result = new PooledArrayBuilder<T>(capacity: stack.Count);

        while (stack.Count > 0)
        {
            result.Add(stack.Pop());
        }

        return result.DrainToImmutable();
    }

    /// <summary>
    /// Executes a binary search over an array, but allows the caller to decide what constitutes a match
    /// </summary>
    /// <typeparam name="T">Type of the elements in the array</typeparam>
    /// <typeparam name="TArg">Type of the argument to pass to the comparer</typeparam>
    /// <param name="array">The array to search</param>
    /// <param name="arg">An argument to pass to the comparison function</param>
    /// <param name="comparer">A comparison function that evaluates an item in the array. Return 0 if the item is a match,
    /// or -1 if the item indicates a successful match will be found in the left branch, or 1 if the item indicates a successful
    /// match will be found in the right branch.</param>
    /// <returns>The index of the element found</returns>
    public static int BinarySearchBy<T, TArg>(this ImmutableArray<T> array, TArg arg, Func<T, TArg, int> comparer)
    {
        var min = 0;
        var max = array.Length - 1;

        while (min <= max)
        {
            var mid = (min + max) / 2;
            var comparison = comparer(array[mid], arg);
            if (comparison == 0)
            {
                return mid;
            }

            if (comparison < 0)
            {
                min = mid + 1;
            }
            else
            {
                max = mid - 1;
            }
        }

        return ~min;
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in ascending order.
    /// </returns>
    public static ImmutableArray<T> OrderAsArray<T>(this ImmutableArray<T> array)
    {
        var sortHelper = new SortHelper<T>(comparer: null, descending: false);
        return array.OrderAsArrayCore(in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in ascending order.
    /// </returns>
    public static ImmutableArray<T> OrderAsArray<T>(this ImmutableArray<T> array, IComparer<T> comparer)
    {
        var sortHelper = new SortHelper<T>(comparer, descending: false);
        return array.OrderAsArrayCore(in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="comparison">A <see cref="Comparison{T}"/> to compare elements.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in ascending order.
    /// </returns>
    public static ImmutableArray<T> OrderAsArray<T>(this ImmutableArray<T> array, Comparison<T> comparison)
    {
        var sortHelper = new SortHelper<T>(comparison, descending: false);
        return array.OrderAsArrayCore(in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in descending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in descending order.
    /// </returns>
    public static ImmutableArray<T> OrderDescendingAsArray<T>(this ImmutableArray<T> array)
    {
        var sortHelper = new SortHelper<T>(comparer: null, descending: true);
        return array.OrderAsArrayCore(in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in descending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in descending order.
    /// </returns>
    public static ImmutableArray<T> OrderDescendingAsArray<T>(this ImmutableArray<T> array, IComparer<T> comparer)
    {
        var sortHelper = new SortHelper<T>(comparer, descending: true);
        return array.OrderAsArrayCore(in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in descending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="comparison">A <see cref="Comparison{T}"/> to compare elements.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in descending order.
    /// </returns>
    public static ImmutableArray<T> OrderDescendingAsArray<T>(this ImmutableArray<T> array, Comparison<T> comparison)
    {
        var sortHelper = new SortHelper<T>(comparison, descending: true);
        return array.OrderAsArrayCore(in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in ascending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in ascending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> OrderByAsArray<TElement, TKey>(
        this ImmutableArray<TElement> array, Func<TElement, TKey> keySelector)
    {
        var sortHelper = new SortHelper<TKey>(comparer: null, descending: false);
        return array.OrderByAsArrayCore(keySelector, in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in ascending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in ascending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> OrderByAsArray<TElement, TKey>(
        this ImmutableArray<TElement> array, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
    {
        var sortHelper = new SortHelper<TKey>(comparer, descending: false);
        return array.OrderByAsArrayCore(keySelector, in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in ascending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparison">A <see cref="Comparison{T}"/> to compare keys.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in ascending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> OrderByAsArray<TElement, TKey>(
        this ImmutableArray<TElement> array, Func<TElement, TKey> keySelector, Comparison<TKey> comparison)
    {
        var sortHelper = new SortHelper<TKey>(comparison, descending: false);
        return array.OrderByAsArrayCore(keySelector, in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in descending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in descending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> OrderByDescendingAsArray<TElement, TKey>(
        this ImmutableArray<TElement> array, Func<TElement, TKey> keySelector)
    {
        var sortHelper = new SortHelper<TKey>(comparer: null, descending: true);
        return array.OrderByAsArrayCore(keySelector, in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in descending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in descending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> OrderByDescendingAsArray<TElement, TKey>(
        this ImmutableArray<TElement> array, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
    {
        var sortHelper = new SortHelper<TKey>(comparer, descending: true);
        return array.OrderByAsArrayCore(keySelector, in sortHelper);
    }

    /// <summary>
    ///  Sorts the elements of an <see cref="ImmutableArray{T}"/> in descending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="array"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="array">An array to ordered.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparison">A <see cref="Comparison{T}"/> to compare keys.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in descending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> OrderByDescendingAsArray<TElement, TKey>(
        this ImmutableArray<TElement> array, Func<TElement, TKey> keySelector, Comparison<TKey> comparison)
    {
        var sortHelper = new SortHelper<TKey>(comparison, descending: true);
        return array.OrderByAsArrayCore(keySelector, in sortHelper);
    }

    private static ImmutableArray<T> OrderAsArrayCore<T>(this ImmutableArray<T> array, ref readonly SortHelper<T> sortHelper)
        => array.OrderByAsArrayCore(SortHelper<T>.IdentityFunc, in sortHelper);

    private static ImmutableArray<TElement> OrderByAsArrayCore<TElement, TKey>(
        this ImmutableArray<TElement> array, Func<TElement, TKey> keySelector, ref readonly SortHelper<TKey> sortHelper)
    {
        if (array.Length <= 1)
        {
            return array;
        }

        var items = array.AsSpan();
        var length = items.Length;

        using var keys = SortKey<TKey>.GetPooledArray(minimumLength: length);

        if (sortHelper.ComputeKeys(items, keySelector, keys.Span))
        {
            // The keys are already ordered, so we don't need to create a new array and sort it.
            return array;
        }

        var newArray = new TElement[length];
        items.CopyTo(newArray);

        Array.Sort(keys.Array, newArray, 0, length, sortHelper.GetOrCreateComparer());

        return ImmutableCollectionsMarshal.AsImmutableArray(newArray);
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in ascending order.
    /// </returns>
    public static ImmutableArray<T> ToImmutableOrdered<T>(this ImmutableArray<T>.Builder builder)
    {
        var array = builder.ToImmutable();
        array.Unsafe().Order();
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in ascending order.
    /// </returns>
    public static ImmutableArray<T> ToImmutableOrdered<T>(this ImmutableArray<T>.Builder builder, IComparer<T> comparer)
    {
        var array = builder.ToImmutable();
        array.Unsafe().Order(comparer);
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="comparison">An <see cref="Comparison{T}"/> to compare elements.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in ascending order.
    /// </returns>
    public static ImmutableArray<T> ToImmutableOrdered<T>(this ImmutableArray<T>.Builder builder, Comparison<T> comparison)
    {
        var array = builder.ToImmutable();
        array.Unsafe().Order(comparison);
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in descending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in descending order.
    /// </returns>
    public static ImmutableArray<T> ToImmutableOrderedDescending<T>(this ImmutableArray<T>.Builder builder)
    {
        var array = builder.ToImmutable();
        array.Unsafe().OrderDescending();
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in descending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in descending order.
    /// </returns>
    public static ImmutableArray<T> ToImmutableOrderedDescending<T>(this ImmutableArray<T>.Builder builder, IComparer<T> comparer)
    {
        var array = builder.ToImmutable();
        array.Unsafe().OrderDescending(comparer);
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in descending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="comparison">An <see cref="Comparison{T}"/> to compare elements.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in descending order.
    /// </returns>
    public static ImmutableArray<T> ToImmutableOrderedDescending<T>(this ImmutableArray<T>.Builder builder, Comparison<T> comparison)
    {
        var array = builder.ToImmutable();
        array.Unsafe().OrderDescending(comparison);
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in ascending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in ascending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> ToImmutableOrderedBy<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector)
    {
        var array = builder.ToImmutable();
        array.Unsafe().OrderBy(keySelector);
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in ascending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in ascending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> ToImmutableOrderedBy<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
    {
        var array = builder.ToImmutable();
        array.Unsafe().OrderBy(keySelector, comparer);
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in ascending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparison">An <see cref="Comparison{T}"/> to compare keys.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in ascending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> ToImmutableOrderedBy<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector, Comparison<TKey> comparison)
    {
        var array = builder.ToImmutable();
        array.Unsafe().OrderBy(keySelector, comparison);
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in descending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in descending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> ToImmutableOrderedByDescending<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector)
    {
        var array = builder.ToImmutable();
        array.Unsafe().OrderByDescending(keySelector);
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in descending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in descending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> ToImmutableOrderedByDescending<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
    {
        var array = builder.ToImmutable();
        array.Unsafe().OrderByDescending(keySelector, comparer);
        return array;
    }

    /// <summary>
    ///  Returns an immutable array that contains the current contents of this
    ///  <see cref="ImmutableArray{T}.Builder"/> sorted in descending order according to a key.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparison">An <see cref="Comparison{T}"/> to compare keys.</param>
    /// <returns>
    ///  Returns a new <see cref="ImmutableArray{T}"/> whose elements are sorted in descending order according to a key.
    /// </returns>
    public static ImmutableArray<TElement> ToImmutableOrderedByDescending<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector, Comparison<TKey> comparison)
    {
        var array = builder.ToImmutable();
        array.Unsafe().OrderByDescending(keySelector, comparison);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in ascending order and sets the collection to a zero length array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in ascending order.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<T> DrainToImmutableOrdered<T>(this ImmutableArray<T>.Builder builder)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().Order();
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in ascending order and sets the collection to a zero length array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in ascending order.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<T> DrainToImmutableOrdered<T>(this ImmutableArray<T>.Builder builder, IComparer<T> comparer)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().Order(comparer);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in ascending order and sets the collection to a zero length array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="comparison">An <see cref="Comparison{T}"/> to compare elements.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in ascending order.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<T> DrainToImmutableOrdered<T>(this ImmutableArray<T>.Builder builder, Comparison<T> comparison)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().Order(comparison);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in descending order and sets the collection to a zero length array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in descending order.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<T> DrainToImmutableOrderedDescending<T>(this ImmutableArray<T>.Builder builder)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().OrderDescending();
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in descending order and sets the collection to a zero length array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in descending order.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<T> DrainToImmutableOrderedDescending<T>(this ImmutableArray<T>.Builder builder, IComparer<T> comparer)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().OrderDescending(comparer);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in descending order and sets the collection to a zero length array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="comparison">An <see cref="Comparison{T}"/> to compare elements.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in descending order.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<T> DrainToImmutableOrderedDescending<T>(this ImmutableArray<T>.Builder builder, Comparison<T> comparison)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().OrderDescending(comparison);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in ascending order according to a key and sets
    ///  the collection to a zero length array.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in ascending order according to a key.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<TElement> DrainToImmutableOrderedBy<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().OrderBy(keySelector);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in ascending order according to a key and sets
    ///  the collection to a zero length array.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in ascending order according to a key.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<TElement> DrainToImmutableOrderedBy<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().OrderBy(keySelector, comparer);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in ascending order according to a key and sets
    ///  the collection to a zero length array.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparison">An <see cref="Comparison{T}"/> to compare keys.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in ascending order according to a key.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<TElement> DrainToImmutableOrderedBy<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector, Comparison<TKey> comparison)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().OrderBy(keySelector, comparison);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in descending order according to a key and sets
    ///  the collection to a zero length array.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in descending order according to a key.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<TElement> DrainToImmutableOrderedByDescending<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().OrderByDescending(keySelector);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in descending order according to a key and sets
    ///  the collection to a zero length array.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in descending order according to a key.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<TElement> DrainToImmutableOrderedByDescending<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector, IComparer<TKey> comparer)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().OrderByDescending(keySelector, comparer);
        return array;
    }

    /// <summary>
    ///  Returns the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  as an immutable array sorted in descending order according to a key and sets
    ///  the collection to a zero length array.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in <paramref name="builder"/>.</typeparam>
    /// <typeparam name="TKey">The type of key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> whose contents will be sorted.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparison">An <see cref="Comparison{T}"/> to compare keys.</param>
    /// <returns>
    ///  An immutable array that contains the current contents of this <see cref="ImmutableArray{T}.Builder"/>
    ///  sorted in descending order according to a key.
    /// </returns>
    /// <remarks>
    ///  If <see cref="ImmutableArray{T}.Builder.Capacity">Capacity</see> equals
    ///  <see cref="ImmutableArray{T}.Builder.Count">Count</see>, the internal array will be extracted as an
    ///  <see cref="ImmutableArray{T}"/> without copying the contents. Otherwise, the contents will be copied
    ///  into a new array. The collection will then be set to a zero length array.
    /// </remarks>
    public static ImmutableArray<TElement> DrainToImmutableOrderedByDescending<TElement, TKey>(
        this ImmutableArray<TElement>.Builder builder, Func<TElement, TKey> keySelector, Comparison<TKey> comparison)
    {
        var array = builder.DrainToImmutable();
        array.Unsafe().OrderByDescending(keySelector, comparison);
        return array;
    }
}
