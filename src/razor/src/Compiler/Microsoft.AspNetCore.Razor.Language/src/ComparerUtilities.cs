// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class ComparerUtilities
{
    public static bool Equals<T>(IReadOnlyList<T>? first, IReadOnlyList<T>? second, IEqualityComparer<T>? comparer)
    {
        if (first == second)
        {
            return true;
        }

        if (first is null)
        {
            return second is null;
        }
        else if (second is null)
        {
            return false;
        }

        if (first.Count != second.Count)
        {
            return false;
        }

        comparer ??= EqualityComparer<T>.Default;

        for (var i = 0; i < first.Count; i++)
        {
            if (!comparer.Equals(first[i], second[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static void AddToHash<T>(ref HashCodeCombiner hash, IReadOnlyList<T> list, IEqualityComparer<T>? comparer)
    {
        comparer ??= EqualityComparer<T>.Default;

        for (var i = 0; i < list.Count; i++)
        {
            hash.Add(list[i], comparer);
        }
    }

    public static bool Equals<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? first, IReadOnlyDictionary<TKey, TValue>? second, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
        where TKey : notnull
    {
        if (first == second)
        {
            return true;
        }

        if (first is null)
        {
            return second is null;
        }
        else if (second is null)
        {
            return false;
        }

        if (first.Count != second.Count)
        {
            return false;
        }

        keyComparer ??= EqualityComparer<TKey>.Default;
        valueComparer ??= EqualityComparer<TValue>.Default;

        // 🐇 Avoid enumerator allocations for Dictionary<TKey, TValue>
        if (first is Dictionary<TKey, TValue> firstDictionary
            && second is Dictionary<TKey, TValue> secondDictionary)
        {
            using var firstEnumerator = firstDictionary.GetEnumerator();
            using var secondEnumerator = secondDictionary.GetEnumerator();
            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
            {
                if (!keyComparer.Equals(firstEnumerator.Current.Key, secondEnumerator.Current.Key)
                    || !valueComparer.Equals(firstEnumerator.Current.Value, secondEnumerator.Current.Value))
                {
                    return false;
                }
            }

            Debug.Assert(!firstEnumerator.MoveNext() && !secondEnumerator.MoveNext(), "We already know the collections have same count.");
            return true;
        }
        else
        {
            using var firstEnumerator = first.GetEnumerator();
            using var secondEnumerator = second.GetEnumerator();
            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
            {
                if (!keyComparer.Equals(firstEnumerator.Current.Key, secondEnumerator.Current.Key)
                    || !valueComparer.Equals(firstEnumerator.Current.Value, secondEnumerator.Current.Value))
                {
                    return false;
                }
            }

            Debug.Assert(!firstEnumerator.MoveNext() && !secondEnumerator.MoveNext(), "We already know the collections have same count.");
            return true;
        }
    }

    public static void AddToHash<TKey, TValue>(ref HashCodeCombiner hash, IReadOnlyDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
        where TKey : notnull
    {
        keyComparer ??= EqualityComparer<TKey>.Default;
        valueComparer ??= EqualityComparer<TValue>.Default;

        // 🐇 Avoid enumerator allocations for Dictionary<TKey, TValue>
        if (dictionary is Dictionary<TKey, TValue> typedDictionary)
        {
            foreach (var kvp in typedDictionary)
            {
                hash.Add(kvp.Key, keyComparer);
                hash.Add(kvp.Value, valueComparer);
            }
        }
        else
        {
            foreach (var kvp in dictionary)
            {
                hash.Add(kvp.Key, keyComparer);
                hash.Add(kvp.Value, valueComparer);
            }
        }
    }
}
