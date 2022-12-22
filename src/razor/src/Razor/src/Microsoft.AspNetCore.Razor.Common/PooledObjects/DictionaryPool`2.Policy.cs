// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class DictionaryPool<TKey, TValue>
    where TKey : notnull
{
    private class Policy : IPooledObjectPolicy<Dictionary<TKey, TValue>>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public Dictionary<TKey, TValue> Create() => new();

        public bool Return(Dictionary<TKey, TValue> map)
        {
            var count = map.Count;

            map.Clear();

            return count <= DefaultPool.MaximumObjectSize;
        }
    }
}
