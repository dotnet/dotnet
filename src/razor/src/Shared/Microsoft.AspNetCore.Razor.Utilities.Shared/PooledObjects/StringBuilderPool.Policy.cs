// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
