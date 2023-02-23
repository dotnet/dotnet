// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class StopwatchPool
{
    private class Policy : IPooledObjectPolicy<Stopwatch>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public Stopwatch Create() => new();

        public bool Return(Stopwatch watch)
        {
            watch.Reset();
            return true;
        }
    }
}
