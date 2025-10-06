// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet
{
    /// <summary>
    /// Provides a caching mechanism for async operations.
    /// </summary>
    /// <typeparam name="TKey">The key to use for storing the async operation.</typeparam>
    /// <typeparam name="TValue">The return type of the async operation.</typeparam>
    internal sealed class TaskResultCache<TKey, TValue>
        where TKey : notnull
    {
        /// <summary>
        /// Represents the cache of async operations.
        /// </summary>
        private readonly ConcurrentDictionary<TKey, Task<TValue>> _cache;

        /// <summary>
        /// Represents a dictionary of locks to synchronize access to individual async operations in the cache.
        /// </summary>
        private readonly ConcurrentDictionary<TKey, object> _perTaskLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskResultCache{TKey, TValue}" /> class with the specified key comparer.
        /// </summary>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}" /> to use when comparing keys.</param>
        public TaskResultCache(IEqualityComparer<TKey> comparer)
        {
            _cache = new(comparer);
            _perTaskLock = new(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskResultCache{TKey, TValue}" /> class with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The default capacity for the cache.</param>
        public TaskResultCache(int capacity)
        {
            _cache = new(concurrencyLevel: Environment.ProcessorCount, capacity);
            _perTaskLock = new(concurrencyLevel: Environment.ProcessorCount, capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskResultCache{TKey, TValue}" /> class.
        /// </summary>
        public TaskResultCache()
        {
            _cache = new();
            _perTaskLock = new();
        }

        /// <summary>
        /// Gets a collection containing the keys in the cache.
        /// </summary>
        public ICollection<TKey> Keys => _cache.Keys;

        /// <summary>
        /// Gets the cached async operation associated with the specified key, or runs the operation asynchronously and returns <see cref="Task{TValue}" /> that the caller can await.
        /// </summary>
        /// <param name="key">The key for the async operation to get or store in the cache.</param>
        /// <param name="valueFactory">A <see cref="Func{TResult}" /> to execute asynchronously if a cached operation does not exist.</param>
        /// <param name="state">A state object to pass to the value factory.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to use for signaling that an operation should be cancelled.</param>
        /// <returns>A <see cref="Task{TResult}" /> for the specified asynchronous operation from the cache if found, otherwise the scheduled asynchronous operation to await.</returns>
        public Task<TValue> GetOrAddAsync<TState>(TKey key, Func<TState, Task<TValue>> valueFactory, TState state, CancellationToken cancellationToken)
        {
            return GetOrAddAsync(key, refresh: false, valueFactory, state, cancellationToken);
        }

        /// <summary>
        /// Gets the cached async operation associated with the specified key, or runs the operation asynchronously and returns <see cref="Task{TValue}" /> that the caller can await, and optionally refreshes the cache.
        /// </summary>
        /// <param name="key">The key for the async operation to get or store in the cache.</param>
        /// <param name="refresh"><see langword="true" /> to force the specified asynchronous operation to be executed and stored in the cache even if a cached operation exists, otherwise <see langword="false" />.</param>
        /// <param name="valueFactory">A <see cref="Func{T1, TResult}" /> to execute asynchronously if a cached operation does not exist.</param>
        /// <param name="state">A state object to pass to the value factory.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to use for signaling that an operation should be cancelled.</param>
        /// <returns>A <see cref="Task{TResult}" /> for the specified asynchronous operation from the cache if found, otherwise the scheduled asynchronous operation to await.</returns>
        public Task<TValue> GetOrAddAsync<TState>(TKey key, bool refresh, Func<TState, Task<TValue>> valueFactory, TState state, CancellationToken cancellationToken)
        {
            if (!refresh && _cache.TryGetValue(key, out Task<TValue>? value))
            {
                return value;
            }

            // Get a lock object for this one single key which allows other asynchronous tasks to be added and retrieved at the same time
            // rather than locking the entire cache.
            // NOTE: Be very careful about which overload of GetOrAdd is called. There was previously a very subtle bug with this call:
            //
            // GetOrAdd(key, static () => new object());
            //
            // Which calls the `GetOrAdd(TKey key, TValue value)` overload rather than the `GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)`
            // overload. The consequence is that the same static delegate is cached and locked on rather than having one lock object per key.
            object lockObject = _perTaskLock.GetOrAdd(key, static (TKey _) => new object());

            lock (lockObject)
            {
                if (!refresh && _cache.TryGetValue(key, out value))
                {
                    return value;
                }

                return _cache[key] = valueFactory(state)
                    .ContinueWith(
                        static task => task.GetAwaiter().GetResult(),
                        cancellationToken,
                        TaskContinuationOptions.RunContinuationsAsynchronously,
                        TaskScheduler.Default);
            }
        }

        /// <summary>
        /// Gets the async operation associated with the specified key if one exists, otherwise throws a <see cref="KeyNotFoundException" />.
        /// </summary>
        /// <param name="key">The key for the async operation to get the value of.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">The specified key does not exist in the cache.</exception>
        public Task<TValue> GetValueAsync(TKey key)
        {
            if (TryGetValue(key, out Task<TValue>? value))
            {
                return value;
            }

            throw new KeyNotFoundException();
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)" />
        public bool TryGetValue(TKey key, [NotNullWhen(true)] out Task<TValue>? value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}
