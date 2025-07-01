// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static class DefaultPool
{
    public const int MaximumObjectSize = 512;

    public static ObjectPool<T> Create<T>(IPooledObjectPolicy<T> policy, int size = 20)
        where T : class
        => new DefaultObjectPool<T>(policy, size);
}
