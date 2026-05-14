// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Common.Test
{
    public class KeyedLockTests : IDisposable
    {
        private const string Key = "a";
        private const string OtherKey = "b";

        private readonly KeyedLock _mutex;

        public KeyedLockTests()
        {
            _mutex = new KeyedLock();
        }

        public void Dispose()
        {
            _mutex.Dispose();
        }

        [Fact]
        public async Task EnterAsync_WhenKeyIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _mutex.EnterAsync(key: null!, CancellationToken.None));

            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public async Task EnterAsync_WhenCancellationTokenIsCancelled_Throws()
        {
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => _mutex.EnterAsync(Key, new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task EnterAsync_WhenDisposed_Throws()
        {
            _mutex.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(() => _mutex.EnterAsync(Key, CancellationToken.None));
        }

        [Fact]
        public async Task EnterAsync_WhenCompleted_LockIsAcquired()
        {
            await _mutex.EnterAsync(Key, CancellationToken.None);

            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(() => _mutex.EnterAsync(Key, cancellationTokenSource.Token));
            }

            await _mutex.ExitAsync(Key);
        }

        [Fact]
        public async Task EnterAsync_WithDifferentKeys_DoesNotBlock()
        {
            await _mutex.EnterAsync(Key, CancellationToken.None);

            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
            {
                await _mutex.EnterAsync(OtherKey, cancellationTokenSource.Token);
            }

            await _mutex.ExitAsync(Key);
            await _mutex.ExitAsync(OtherKey);
        }

        [Fact]
        public void Enter_WhenKeyIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _mutex.Enter(key: null!));

            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void Enter_WhenDisposed_Throws()
        {
            _mutex.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _mutex.Enter(Key));
        }

        [Fact]
        public async Task Enter_WhenCompleted_LockIsAcquired()
        {
            _mutex.Enter(Key);

            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => _mutex.EnterAsync(Key, cancellationTokenSource.Token));
            }

            _mutex.Exit(Key);
        }

        [Fact]
        public async Task Enter_WithDifferentKeys_DoesNotBlock()
        {
            _mutex.Enter(Key);

            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
            {
                await Task.Run(() => _mutex.Enter(OtherKey));
            }

            _mutex.Exit(Key);
            _mutex.Exit(OtherKey);
        }

        [Fact]
        public async Task ExitAsync_WhenKeyIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _mutex.ExitAsync(key: null!));

            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public async Task ExitAsync_WhenKeyIsNotFound_Throws()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _mutex.ExitAsync(Key));
        }

        [Fact]
        public async Task ExitAsync_WhenDisposed_Throws()
        {
            _mutex.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(() => _mutex.ExitAsync(Key));
        }

        [Fact]
        public async Task ExitAsync_WhenCompleted_LockIsReleased()
        {
            await _mutex.EnterAsync(Key, CancellationToken.None);
            await _mutex.ExitAsync(Key);

            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                await _mutex.EnterAsync(Key, cancellationTokenSource.Token);
            }

            await _mutex.ExitAsync(Key);
        }

        [Fact]
        public void Exit_WhenKeyIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _mutex.Enter(key: null!));

            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void Exit_WhenKeyIsNotFound_Throws()
        {
            Assert.Throws<KeyNotFoundException>(() => _mutex.Exit(Key));
        }

        [Fact]
        public void Exit_WhenDisposed_Throws()
        {
            _mutex.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _mutex.Exit(Key));
        }

        [Fact]
        public async Task Exit_WhenCompleted_LockIsReleased()
        {
            _mutex.Enter(Key);
            _mutex.Exit(Key);

            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                await _mutex.EnterAsync(Key, cancellationTokenSource.Token);
            }

            _mutex.Exit(Key);
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_IsIdempotent()
        {
            _mutex.Dispose();
            _mutex.Dispose();
        }
    }
}
