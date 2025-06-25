// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
