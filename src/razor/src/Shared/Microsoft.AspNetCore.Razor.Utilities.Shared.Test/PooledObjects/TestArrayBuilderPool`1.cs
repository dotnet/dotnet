// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test.PooledObjects;

internal static class TestArrayBuilderPool<T>
{
    public static ObjectPool<ImmutableArray<T>.Builder> Create(
        IPooledObjectPolicy<ImmutableArray<T>.Builder>? policy = null, int size = 1)
        => DefaultPool.Create(policy ?? NoReturnPolicy.Instance, size);

    public sealed class NoReturnPolicy : IPooledObjectPolicy<ImmutableArray<T>.Builder>
    {
        public static readonly NoReturnPolicy Instance = new();

        private NoReturnPolicy()
        {
        }

        public ImmutableArray<T>.Builder Create()
            => ImmutableArray.CreateBuilder<T>();

        public bool Return(ImmutableArray<T>.Builder obj)
            => false;
    }
}
