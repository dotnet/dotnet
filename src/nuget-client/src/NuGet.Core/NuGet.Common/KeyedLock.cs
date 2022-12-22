// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Common
{
    internal sealed class KeyedLock : IDisposable
    {
        private readonly Dictionary<string, LockState> _locks;
        private readonly SemaphoreSlim _dictionaryLock;

        internal KeyedLock()
        {
            _locks = new Dictionary<string, LockState>();
            _dictionaryLock = new SemaphoreSlim(initialCount: 1);
        }

        internal async Task EnterAsync(string key, CancellationToken token)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            LockState lockState;

            await _dictionaryLock.WaitAsync(token);
            try
            {
                lockState = GetOrCreate(key);
            }
            finally
            {
                _dictionaryLock.Release();
            }

            try
            {
                await lockState.Semaphore.WaitAsync(token);
            }
            catch
            {
                Interlocked.Decrement(ref lockState.Count);
                throw;
            }
        }

        internal void Enter(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            LockState lockState;

            _dictionaryLock.Wait(CancellationToken.None);
            try
            {
                lockState = GetOrCreate(key);
            }
            finally
            {
                _dictionaryLock.Release();
            }

            try
            {
                lockState.Semaphore.Wait(CancellationToken.None);
            }
            catch
            {
                Interlocked.Decrement(ref lockState.Count);
                throw;
            }
        }

        private LockState GetOrCreate(string key)
        {
            if (_locks.TryGetValue(key, out var lockState))
            {
                Interlocked.Increment(ref lockState.Count);
            }
            else
            {
                lockState = new LockState();
                lockState.Semaphore = new SemaphoreSlim(initialCount: 1);
                lockState.Count = 1;
                _locks[key] = lockState;
            }

            return lockState;
        }

        internal async Task ExitAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            await _dictionaryLock.WaitAsync();
            try
            {
                Cleanup(key);
            }
            finally
            {
                _dictionaryLock.Release();
            }
        }

        internal void Exit(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _dictionaryLock.Wait();
            try
            {
                Cleanup(key);
            }
            finally
            {
                _dictionaryLock.Release();
            }
        }

        private void Cleanup(string key)
        {
            var lockState = _locks[key];
            lockState.Semaphore.Release();
            var count = Interlocked.Decrement(ref lockState.Count);
            if (count == 0)
            {
                lockState.Semaphore.Dispose();
                _locks.Remove(key);
            }
        }

        private class LockState
        {
            public SemaphoreSlim Semaphore;
            public int Count;
        }

        public void Dispose()
        {
            _dictionaryLock.Dispose();

            foreach (var kvp in _locks)
            {
                kvp.Value.Semaphore.Dispose();
            }
            _locks.Clear();
        }
    }
}
