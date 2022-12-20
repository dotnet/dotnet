// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class ArrayBuilderPool<T>
{
    private class Policy : IPooledObjectPolicy<ImmutableArray<T>.Builder>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public ImmutableArray<T>.Builder Create() => ImmutableArray.CreateBuilder<T>();

        public bool Return(ImmutableArray<T>.Builder builder)
        {
            var count = builder.Count;

            builder.Clear();

            if (count > DefaultPool.MaximumObjectSize)
            {
                builder.Capacity = DefaultPool.MaximumObjectSize;
            }

            return true;
        }
    }
}
