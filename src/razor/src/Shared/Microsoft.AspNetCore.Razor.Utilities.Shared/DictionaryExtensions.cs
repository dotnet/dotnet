// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.PooledObjects;

namespace System.Collections.Generic;

internal static class DictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value)
        where TKey : notnull
    {
        if (dictionary.TryGetValue(key, out var existingValue))
        {
            return existingValue;
        }
        else
        {
            dictionary.Add(key, value);
            return value;
        }
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this PooledDictionaryBuilder<TKey, TValue> dictionary,
        TKey key,
        TValue value)
        where TKey : notnull
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }
        else
        {
            dictionary.Add(key, value);
            return value;
        }
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> func)
        where TKey : notnull
    {
        if (dictionary.TryGetValue(key, out var existingValue))
        {
            return existingValue;
        }
        else
        {
            var value = func(key);
            dictionary.Add(key, value);
            return value;
        }
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this PooledDictionaryBuilder<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> func)
        where TKey : notnull
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }
        else
        {
            var value = func(key);
            dictionary.Add(key, value);
            return value;
        }
    }
}
