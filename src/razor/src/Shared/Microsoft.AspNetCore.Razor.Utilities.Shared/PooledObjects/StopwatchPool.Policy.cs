// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
