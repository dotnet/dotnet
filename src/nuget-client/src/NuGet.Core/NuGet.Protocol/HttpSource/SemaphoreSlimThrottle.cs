// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol
{
    public class SemaphoreSlimThrottle : IThrottle
    {
        private readonly SemaphoreSlim _semaphore;

        /// <summary>
        /// The number of remaining threads that can enter the semaphore.
        /// </summary>
        public int CurrentCount => _semaphore.CurrentCount;

        public SemaphoreSlimThrottle(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        }

        public async Task WaitAsync()
        {
            await _semaphore.WaitAsync();
        }

        public void Release()
        {
            _semaphore.Release();
        }

        public static SemaphoreSlimThrottle CreateBinarySemaphore()
        {
            return CreateSemaphoreThrottle(initialCount: 1);
        }

        public static SemaphoreSlimThrottle CreateSemaphoreThrottle(int initialCount)
        {
            return new SemaphoreSlimThrottle(new SemaphoreSlim(initialCount));
        }
    }
}
