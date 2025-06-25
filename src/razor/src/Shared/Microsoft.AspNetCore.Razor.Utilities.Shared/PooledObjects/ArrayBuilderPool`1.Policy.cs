// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
