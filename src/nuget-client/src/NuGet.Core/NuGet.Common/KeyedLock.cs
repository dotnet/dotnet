// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Common
{
    /// <summary>A class that allows one thread at a time synchronization, but is fine grained per key.
    /// For example, the key can be a resource name that must only be accessed one at a time,
    /// but different resources can be accessed concurrently.</summary>
    internal sealed class KeyedLock : IDisposable
    {
        /// <summary>The dictionary that contains a <see cref="LockState"/> for each key.</summary>
        /// <remarks>Both reading and modifying this dictionary must be synchronized though <see cref="_dictionaryLock"/>.</remarks>
        private readonly Dictionary<string, LockState> _locks;

        /// <summary>A lock to synchronize reading and modifying <see cref="_locks"/>.</summary>
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

            // Get the dictionary lock, so no other call to Enter[Async] or Exit[Async] can modify
            // _locks while the current call does.
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
                // wait on the key-specific lock
                await lockState.Semaphore.WaitAsync(token);
            }
            catch
            {
                // GetOrCreate(key) increments the lock state counter. Since this task failed to obtain the lock
                // and is no longer waiting for it, decrease the count so that it will be eligible for cleanup
                // when the code holding the lock releases it.
                // If Exit[Async] ran for this key after the above WaitAsync failed, but before this catch block runs,
                // it means that the LockState won't be removed from the dictionary, so technically a memory leak.
                // But there isn't a multi-threaded correctness problem, the key still only allows one thread at a time
                // to access it.
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

            // Get the dictionary lock, so no other call to Enter[Async] or Exit[Async] can modify
            // _locks while the current call does.
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
                // wait on the key-specific lock
                lockState.Semaphore.Wait(CancellationToken.None);
            }
            catch
            {
                // GetOrCreate(key) increments the lock state counter. Since this task failed to obtain the lock
                // and is no longer waiting for it, decrease the count so that it will be eligible for cleanup
                // when the code holding the lock releases it.
                // If Exit[Async] ran for this key after the above WaitAsync failed, but before this catch block runs,
                // it means that the LockState won't be removed from the dictionary, so technically a memory leak.
                // But there isn't a multi-threaded correctness problem, the key still only allows one thread at a time
                // to access it.
                Interlocked.Decrement(ref lockState.Count);
                throw;
            }
        }

        private LockState GetOrCreate(string key)
        {
            // The caller holds the dictionary lock, so we know no other call to Enter[Async] or Exit[Async] can be
            // running at this instant.
            if (_locks.TryGetValue(key, out var lockState))
            {
                // LockState.Count is a reference counter for the number of threads/tasks that want access to this key's lock
                // Since the current thread wants access, increment the counter. Doing this while we hold the dictionary lock
                // ensures that Exit[Async] trying to run at the same time won't have timing issues with regards to changing
                // and reading the value at the same time.
                Interlocked.Increment(ref lockState.Count);
            }
            else
            {
                // Key doesn't yet exist, so create one and set the initial count to 1, indicating that the current thread
                // has interest in the key's LockState.
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

            // Get the dictionary lock, so no other call to Enter[Async] or Exit[Async] can modify
            // _locks while the current call does.
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

            // Get the dictionary lock, so no other call to Enter[Async] or Exit[Async] can modify
            // _locks while the current call does.
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
            // The caller holds the dictionary lock, so we know no other call to Enter[Async] or Exit[Async] can be
            // running at this instant.
            var lockState = _locks[key];

            // Release the per-key lock, allowing any Enter[Async] to now obtain the lock
            lockState.Semaphore.Release();

            // Decrement the counter while still holding the dictionary lock to ensure that no other Enter[Async]
            // or Exit[Async] can cause timing issues.
            var count = Interlocked.Decrement(ref lockState.Count);
            if (count == 0)
            {
                // count == 0 means that this was the only/last thread accessing this key's LockState. Therefore, we
                // can dispose of it and remove the key from the dictionary.
                lockState.Semaphore.Dispose();
                _locks.Remove(key);
            }
        }

        /// <summary>Nested class to hold the state of per-key locks.</summary>
        private class LockState
        {
            /// <summary>The synchronization object used to ensure only 1 thread at a time can obtain the key's lock.</summary>
            public SemaphoreSlim Semaphore;

            /// <summary>A counter of how many threads/tasks have interest in the key's lock. When this reaches zero,
            /// it means no more threads or tasks want access to the resource, and the dictionary can clear its memory
            /// of the key and this state.</summary>
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
