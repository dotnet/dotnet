// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Tests.Utility
{
    public class UsingSemaphore : IDisposable
    {
        private SemaphoreSlim _semaphore;

        private UsingSemaphore(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public static async Task<IDisposable> WaitAsync(SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            return new UsingSemaphore(semaphore);
        }

        public void Dispose()
        {
            _semaphore?.Release();
            _semaphore = null;
        }
    }
}
