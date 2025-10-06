// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class StringBuilderPool
{
    private class Policy : IPooledObjectPolicy<StringBuilder>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public StringBuilder Create() => new();

        public bool Return(StringBuilder builder)
        {
            builder.Clear();

            if (builder.Capacity > DefaultPool.MaximumObjectSize)
            {
                builder.Capacity = DefaultPool.MaximumObjectSize;
            }

            return true;
        }
    }
}
