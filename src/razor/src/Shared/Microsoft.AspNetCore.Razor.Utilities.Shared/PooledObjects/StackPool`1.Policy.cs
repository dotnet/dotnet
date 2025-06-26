// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class StackPool<T>
{
    private class Policy : IPooledObjectPolicy<Stack<T>>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public Stack<T> Create() => new();

        public bool Return(Stack<T> stack)
        {
            var count = stack.Count;

            stack.Clear();

            if (count > DefaultPool.MaximumObjectSize)
            {
                stack.TrimExcess();
            }

            return true;
        }
    }
}
