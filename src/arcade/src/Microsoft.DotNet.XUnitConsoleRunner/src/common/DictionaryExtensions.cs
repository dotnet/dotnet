// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

static class DictionaryExtensions
{
    public static void Add<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
    {
        dictionary.GetOrAdd(key).Add(value);
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TValue : new()
    {
        return dictionary.GetOrAdd<TKey, TValue>(key, () => new TValue());
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> newValue)
    {
        TValue result;

        if (!dictionary.TryGetValue(key, out result))
        {
            result = newValue();
            dictionary[key] = result;
        }

        return result;
    }

    public static Dictionary<TKey, TValue> ToDictionaryIgnoringDuplicateKeys<TInput, TKey, TValue>(this IEnumerable<TInput> inputValues,
                                                                                                   Func<TInput, TKey> keySelector,
                                                                                                   Func<TInput, TValue> valueSelector,
                                                                                                   IEqualityComparer<TKey> comparer = null)
    {
        var result = new Dictionary<TKey, TValue>(comparer);

        foreach (var inputValue in inputValues)
        {
            var key = keySelector(inputValue);
            if (!result.ContainsKey(key))
                result.Add(key, valueSelector(inputValue));
        }

        return result;
    }
}
