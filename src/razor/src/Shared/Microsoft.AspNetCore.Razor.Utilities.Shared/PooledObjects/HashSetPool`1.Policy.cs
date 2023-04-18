// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class HashSetPool<T>
{
    private class Policy : IPooledObjectPolicy<HashSet<T>>
    {
        public static readonly Policy Instance = new();

        private readonly IEqualityComparer<T>? _comparer;

        public Policy(IEqualityComparer<T>? comparer = null)
        {
            _comparer = comparer;
        }

        public HashSet<T> Create() => new(_comparer);

        public bool Return(HashSet<T> set)
        {
            var count = set.Count;

            set.Clear();

            if (count > DefaultPool.MaximumObjectSize)
            {
                set.TrimExcess();
            }

            return true;
        }
    }
}
