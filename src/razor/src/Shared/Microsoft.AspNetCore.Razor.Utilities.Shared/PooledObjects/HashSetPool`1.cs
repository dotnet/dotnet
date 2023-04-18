// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="HashSet{T}"/> instances that compares items using default equality.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class HashSetPool<T>
{
    public static readonly ObjectPool<HashSet<T>> Default = DefaultPool.Create(Policy.Instance);

    public static ObjectPool<HashSet<T>> Create(IEqualityComparer<T> comparer)
        => DefaultPool.Create(new Policy(comparer));

    public static PooledObject<HashSet<T>> GetPooledObject()
        => Default.GetPooledObject();

    public static PooledObject<HashSet<T>> GetPooledObject(out HashSet<T> set)
        => Default.GetPooledObject(out set);
}
