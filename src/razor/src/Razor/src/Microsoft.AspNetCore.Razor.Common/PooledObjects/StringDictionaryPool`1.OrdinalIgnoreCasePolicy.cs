// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class StringDictionaryPool<TValue>
{
    private class OrdinalIgnoreCasePolicy : IPooledObjectPolicy<Dictionary<string, TValue>>
    {
        public static readonly OrdinalIgnoreCasePolicy Instance = new();

        private OrdinalIgnoreCasePolicy()
        {
        }

        public Dictionary<string, TValue> Create() => new(StringComparer.OrdinalIgnoreCase);

        public bool Return(Dictionary<string, TValue> map)
        {
            var count = map.Count;

            map.Clear();

            // If the map grew too large, don't return it to the pool.
            return count <= DefaultPool.MaximumObjectSize;
        }
    }
}
