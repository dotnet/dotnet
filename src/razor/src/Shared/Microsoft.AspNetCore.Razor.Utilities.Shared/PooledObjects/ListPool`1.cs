// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="List{T}"/> instances.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class ListPool<T>
{
    public static readonly ObjectPool<List<T>> Default = DefaultPool.Create(Policy.Instance);

    public static ObjectPool<List<T>> Create(int size = 20)
        => DefaultPool.Create(Policy.Instance, size);

    public static PooledObject<List<T>> GetPooledObject()
        => Default.GetPooledObject();

    public static PooledObject<List<T>> GetPooledObject(out List<T> list)
        => Default.GetPooledObject(out list);
}
