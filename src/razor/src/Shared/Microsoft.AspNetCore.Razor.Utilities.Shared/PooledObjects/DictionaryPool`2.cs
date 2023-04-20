// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="Dictionary{TKey, TValue}"/> instances.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class DictionaryPool<TKey, TValue>
    where TKey : notnull
{
    public static readonly ObjectPool<Dictionary<TKey, TValue>> Default = DefaultPool.Create(Policy.Instance);

    public static ObjectPool<Dictionary<TKey, TValue>> Create(IEqualityComparer<TKey> comparer)
        => DefaultPool.Create(new Policy(comparer));

    public static PooledObject<Dictionary<TKey, TValue>> GetPooledObject()
        => Default.GetPooledObject();

    public static PooledObject<Dictionary<TKey, TValue>> GetPooledObject(out Dictionary<TKey, TValue> map)
        => Default.GetPooledObject(out map);
}
