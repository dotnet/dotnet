// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
