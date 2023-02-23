// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class ListPool<T>
{
    private class Policy : IPooledObjectPolicy<List<T>>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public List<T> Create() => new();

        public bool Return(List<T> list)
        {
            var count = list.Count;

            list.Clear();

            if (count > DefaultPool.MaximumObjectSize)
            {
                list.TrimExcess();
            }

            return true;
        }
    }
}
