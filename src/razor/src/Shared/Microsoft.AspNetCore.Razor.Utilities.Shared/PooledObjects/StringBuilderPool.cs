// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="StringBuilder"/> instances.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class StringBuilderPool
{
    public static readonly ObjectPool<StringBuilder> Default = DefaultPool.Create(Policy.Instance);

    public static PooledObject<StringBuilder> GetPooledObject()
        => Default.GetPooledObject();

    public static PooledObject<StringBuilder> GetPooledObject(out StringBuilder builder)
        => Default.GetPooledObject(out builder);
}
