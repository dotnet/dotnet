// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// Pooled <see cref="Dictionary{TKey, TValue}"/> instances when the key is of type <see cref="string"/>.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class StringDictionaryPool<TValue>
{
    public static readonly ObjectPool<Dictionary<string, TValue>> Ordinal = DictionaryPool<string, TValue>.Create(StringComparer.Ordinal);
    public static readonly ObjectPool<Dictionary<string, TValue>> OrdinalIgnoreCase = DictionaryPool<string, TValue>.Create(StringComparer.OrdinalIgnoreCase);

    public static PooledObject<Dictionary<string, TValue>> GetPooledObject()
        => Ordinal.GetPooledObject();

    public static PooledObject<Dictionary<string, TValue>> GetPooledObject(out Dictionary<string, TValue> map)
        => Ordinal.GetPooledObject(out map);

    public static PooledObject<Dictionary<string, TValue>> GetPooledObject(bool ignoreCase)
        => ignoreCase
            ? OrdinalIgnoreCase.GetPooledObject()
            : Ordinal.GetPooledObject();

    public static PooledObject<Dictionary<string, TValue>> GetPooledObject(bool ignoreCase, out Dictionary<string, TValue> map)
        => ignoreCase
            ? OrdinalIgnoreCase.GetPooledObject(out map)
            : Ordinal.GetPooledObject(out map);
}
