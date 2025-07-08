// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="Stopwatch"/> instances.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class StopwatchPool
{
    public static readonly ObjectPool<Stopwatch> Default = DefaultPool.Create(Policy.Instance);

    public static PooledObject<Stopwatch> GetPooledObject()
        => Default.GetPooledObject();

    public static PooledObject<Stopwatch> GetPooledObject(out Stopwatch watch)
        => Default.GetPooledObject(out watch);
}
