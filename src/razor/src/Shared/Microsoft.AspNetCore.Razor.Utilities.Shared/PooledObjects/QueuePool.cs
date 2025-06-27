// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class QueuePool<T>
{
    public static readonly ObjectPool<Queue<T>> Default = DefaultPool.Create(Policy.Instance);

    public static PooledObject<Queue<T>> GetPooledObject()
        => Default.GetPooledObject();

    public static PooledObject<Queue<T>> GetPooledObject(out Queue<T> queue)
        => Default.GetPooledObject(out queue);
}
