// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static class DefaultPool
{
    public const int MaximumObjectSize = 512;

    public static ObjectPool<T> Create<T>(IPooledObjectPolicy<T> policy)
        where T : class
        => new DefaultObjectPool<T>(policy, 20);
}
