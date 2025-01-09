// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence;

internal static class CollectionExtensions
{
#if NETFRAMEWORK
    internal static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
            return true;
        }

        return false;
    }
#endif

    [return: NotNullIfNotNull(nameof(list))]
    internal static IReadOnlyList<T>? IReadOnlyList<T>(this IReadOnlyList<T>? list) => list;

    internal static void AddIfNotNull<T>(this List<T> list, T? item)
    {
        if (item is not null)
        {
            list.Add(item);
        }
    }

    internal static T AddAndReturn<T>(this List<T> list, T item)
    {
        list.Add(item);
        return item;
    }

    internal static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IReadOnlyCollection<T>? collection)
    {
        return collection is null || collection.Count == 0;
    }

    internal static ReadOnlyListStructEnumerable<T> GetStructEnumerable<T>(this IReadOnlyList<T>? list) => new ReadOnlyListStructEnumerable<T>(list);

    internal static ReadOnlyListStructReverseEnumerable<T> GetStructReverseEnumerable<T>(this IReadOnlyList<T>? list) => new ReadOnlyListStructReverseEnumerable<T>(list);

    /// <summary>
    /// Creates an array from a <see cref="IReadOnlyCollection{T}"/> with a selector to transform the items.
    /// </summary>
    /// <typeparam name="TSource">The item type of the input collection.</typeparam>
    /// <typeparam name="TResult">The item type of the new array.</typeparam>
    /// <param name="collection">The input collection.</param>
    /// <param name="selector">A way to convert TSource to TResult.</param>
    /// <returns>An array of the new items.</returns>
    internal static List<TResult> ToList<TSource, TResult>(this IReadOnlyCollection<TSource> collection, Func<TSource, TResult> selector)
    {
        List<TResult> list = new List<TResult>(collection.Count);
        foreach (TSource item in collection)
        {
            list.Add(selector(item));
        }

        return list;
    }

    /// <summary>
    /// Creates an array from a <see cref="IReadOnlyCollection{T}"/> with a selector to transform the items.
    /// </summary>
    /// <typeparam name="TSource">The item type of the input collection.</typeparam>
    /// <typeparam name="TResult">The item type of the new array.</typeparam>
    /// <typeparam name="TState">The type of the state to pass to the predicate and selector.</typeparam>
    /// <param name="collection">The input collection.</param>
    /// <param name="predicate">A way to filter the items.</param>
    /// <param name="selector">A way to convert TSource to TResult.</param>
    /// <param name="state">The state to pass to the predicate and selector.</param>
    /// <returns>An array of the new items.</returns>
    internal static List<TResult> WhereToList<TSource, TResult, TState>(
        this IReadOnlyCollection<TSource> collection,
        Func<TSource, TState, bool> predicate,
        Func<TSource, TState, TResult> selector,
        TState state)
    {
        List<TResult> list = new List<TResult>(collection.Count);
        foreach (TSource item in collection)
        {
            if (predicate(item, state))
            {
                list.Add(selector(item, state));
            }
        }

        return list;
    }
}
