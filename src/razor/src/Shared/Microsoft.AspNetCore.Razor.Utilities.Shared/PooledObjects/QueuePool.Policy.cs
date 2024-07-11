// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class QueuePool<T>
{
    private class Policy : IPooledObjectPolicy<Queue<T>>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public Queue<T> Create() => new Queue<T>();

        public bool Return(Queue<T> queue)
        {
            var count = queue.Count;

            queue.Clear();

            if (count > DefaultPool.MaximumObjectSize)
            {
                queue.TrimExcess();
            }

            return true;
        }
    }
}
