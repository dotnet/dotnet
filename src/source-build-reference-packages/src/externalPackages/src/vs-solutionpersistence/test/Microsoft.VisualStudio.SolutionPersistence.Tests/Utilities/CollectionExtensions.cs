// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Utilities;

internal static class CollectionExtensions
{
    /// <summary>
    /// Creates an array from a <see cref="IReadOnlyCollection{T}"/> with a selector to transform the items.
    /// </summary>
    /// <typeparam name="TSource">The item type of the input collection.</typeparam>
    /// <typeparam name="TResult">The item type of the new array.</typeparam>
    /// <param name="collection">The input collection.</param>
    /// <param name="selector">A way to convert TSource to TResult.</param>
    /// <returns>An array of the new items.</returns>
    public static TResult[] ToArray<TSource, TResult>(this IReadOnlyCollection<TSource> collection, Func<TSource, TResult> selector)
    {
        Argument.ThrowIfNull(collection, nameof(collection));

        TResult[] array = new TResult[collection.Count];
        int index = 0;
        foreach (TSource item in collection)
        {
            array[index] = selector(item);
            index++;
        }

        return array;
    }

    public static int IndexOf<T>(this IReadOnlyList<T> list, T item)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (EqualityComparer<T>.Default.Equals(list[i], item))
            {
                return i;
            }
        }

        return -1;
    }
}
