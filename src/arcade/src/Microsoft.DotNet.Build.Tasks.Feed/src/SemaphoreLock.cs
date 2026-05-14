// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public readonly struct SemaphoreLock : IDisposable
    {
        private readonly SemaphoreSlim _sem;

        private SemaphoreLock(SemaphoreSlim sem)
        {
            _sem = sem;
        }

        public void Dispose()
        {
            _sem?.Release();
        }

        public static ValueTask<SemaphoreLock> LockAsync(SemaphoreSlim sem)
        {
            if (sem == null)
            {
                return new ValueTask<SemaphoreLock>(new SemaphoreLock(null));
            }
            Task waitTask = sem.WaitAsync();
            if (waitTask.IsCompleted && !waitTask.IsFaulted && !waitTask.IsCanceled)
            {
                return new ValueTask<SemaphoreLock>(new SemaphoreLock(sem));
            }

            static async Task<SemaphoreLock> WaitForLock(Task waitTask, SemaphoreSlim sem)
            {
                await waitTask.ConfigureAwait(false);
                return new SemaphoreLock(sem);
            }

            return new ValueTask<SemaphoreLock>(WaitForLock(waitTask, sem));
        }
    }
}
