// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Core.FuncTest
{
    public class ConcurrencyUtilitiesTests
    {
        [Fact]
        public async Task ConcurrencyUtilities_LockStressSynchronous()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                // This is the path that uniquely identifies the system-wide mutex.
                var path = Path.Combine(testDirectory, "ConcurrencyUtilities_LockStress_Verification_Synchronous");

                // This is a semaphore use to verify the lock.
                var verificationSemaphore = new SemaphoreSlim(1);

                // Iterate a lot, to increase confidence.
                const int threads = 50;
                const int iterations = 10;

                // This is the action that is execute inside of the lock.
                Action lockedActionSync = () =>
                {
                    var acquired = verificationSemaphore.Wait(0);
                    Assert.True(acquired, "Unable to acquire the lock on the semaphore within the file lock");

                    // Hold the lock for a little bit.
                    Thread.Sleep(TimeSpan.FromMilliseconds(1));

                    verificationSemaphore.Release();
                };


                // Loop the same action, over and over.
                Func<int, Task<List<bool>>> loopAsync = async thread =>
                {
                    var loopResults = new List<bool>();
                    await Task.Run(() =>
                    {
                        foreach (var iteration in Enumerable.Range(0, iterations))
                        {
                            ConcurrencyUtilities.ExecuteWithFileLocked(
                                path,
                                lockedActionSync);
                            loopResults.Add(true);
                        }
                    });

                    return loopResults;
                };

                // Act
                var tasks = Enumerable.Range(0, threads).Select(loopAsync);
                var results = (await Task.WhenAll(tasks)).SelectMany(r => r).ToArray();

                // Assert
                Assert.Equal(threads * iterations, results.Length);
            }
        }

        [Fact]
        public async Task ConcurrencyUtilities_LockStress()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                // This is the path that uniquely identifies the system-wide mutex.
                var path = Path.Combine(testDirectory, "ConcurrencyUtilities_LockStress_Verification");

                // This is a semaphore use to verify the lock.
                var verificationSemaphore = new SemaphoreSlim(1);

                // Iterate a lot, to increase confidence.
                const int threads = 50;
                const int iterations = 10;

                // This is the action that is execute inside of the lock.
                Func<CancellationToken, Task<bool>> lockedActionAsync = async lockedToken =>
                {
                    var acquired = await verificationSemaphore.WaitAsync(0);
                    if (!acquired)
                    {
                        return false;
                    }

                    // Hold the lock for a little bit.
                    await Task.Delay(TimeSpan.FromMilliseconds(1));

                    verificationSemaphore.Release();
                    return true;
                };

                // Loop the same action, over and over.
                Func<int, Task<List<bool>>> loopAsync = async thread =>
                {
                    var loopResults = new List<bool>();
                    foreach (var iteration in Enumerable.Range(0, iterations))
                    {
                        var result = await ConcurrencyUtilities.ExecuteWithFileLockedAsync(
                            path,
                            lockedActionAsync,
                            CancellationToken.None);
                        loopResults.Add(result);
                    }

                    return loopResults;
                };

                // Act
                var tasks = Enumerable.Range(0, threads).Select(loopAsync);
                var results = (await Task.WhenAll(tasks)).SelectMany(r => r).ToArray();

                // Assert
                Assert.Equal(threads * iterations, results.Length);
                Assert.DoesNotContain(false, results);
            }
        }

        [Fact]
        public async Task ConcurrencyUtilities_LockAllCasings()
        {
            // Arrange
            var token = CancellationToken.None;
            var path1 = "/tmp/packageA/1.0.0";
            var path2 = "/tmp/packagea/1.0.0";
            var action1HitSem = new ManualResetEventSlim();
            var action1Sem = new ManualResetEventSlim();
            var action2Sem = new ManualResetEventSlim();

            Func<CancellationToken, Task<bool>> action1 = (ct) =>
            {
                action1HitSem.Set();
                action1Sem.Wait();
                return TaskResult.True;
            };

            Func<CancellationToken, Task<bool>> action2 = (ct) =>
            {
                action2Sem.Set();
                return TaskResult.True;
            };

            // Act
            var task1 = Task.Run(async () => await ConcurrencyUtilities.ExecuteWithFileLockedAsync<bool>(
                path1,
                action1,
                token));

            var task2Started = action1HitSem.Wait(60 * 1000 * 5, token);
            Assert.True(task2Started);

            var task2 = Task.Run(async () => await ConcurrencyUtilities.ExecuteWithFileLockedAsync<bool>(
                path2,
                action2,
                token));

            // Wait 1s to verify that task2 has not started
            await Task.Delay(1000);

            var task2blocked = !action2Sem.IsSet;
            action1Sem.Set();

            await task1;
            var result = await task2;

            // Assert
            Assert.True(task2blocked);
            Assert.True(result);
        }

        [Fact]
        public async Task ConcurrencyUtilities_NormalizePaths()
        {
            // Arrange
            var token = CancellationToken.None;
            var path1 = "/tmp/packageA/1.0.0";
            var path2 = "/tmp/sub/../packageA/1.0.0";
            var action1HitSem = new ManualResetEventSlim();
            var action1Sem = new ManualResetEventSlim();
            var action2Sem = new ManualResetEventSlim();

            Func<CancellationToken, Task<bool>> action1 = (ct) =>
            {
                action1HitSem.Set();
                action1Sem.Wait();
                return TaskResult.True;
            };

            Func<CancellationToken, Task<bool>> action2 = (ct) =>
            {
                action2Sem.Set();
                return TaskResult.True;
            };

            // Act
            var task1 = Task.Run(async () => await ConcurrencyUtilities.ExecuteWithFileLockedAsync<bool>(
                path1,
                action1,
                token));

            var task2Started = action1HitSem.Wait(60 * 1000 * 5, token);
            Assert.True(task2Started);

            var task2 = Task.Run(async () => await ConcurrencyUtilities.ExecuteWithFileLockedAsync<bool>(
                path2,
                action2,
                token));

            // Wait 1s to verify that task2 has not started
            await Task.Delay(1000);

            var task2blocked = !action2Sem.IsSet;
            action1Sem.Set();

            await task1;
            var result = await task2;

            // Assert
            Assert.True(task2blocked);
            Assert.True(result);
        }

        [Fact]
        public void ExecuteWithFileLocked_WhenFileStreamIsUnauthorized_ThrowsInvalidOperationException()
        {
            // Arrange
            using var testDirectory = TestDirectory.Create();

            // This is the path that uniquely identifies the system-wide mutex.
            var path = Path.Combine(testDirectory, nameof(ExecuteWithFileLocked_WhenFileStreamIsUnauthorized_ThrowsInvalidOperationException));

            // This is a semaphore use to verify the lock.
            var verificationSemaphore = new SemaphoreSlim(1);

            // This is the action that is execute inside of the lock.
            Action lockedActionSync = () =>
            {
                var acquired = verificationSemaphore.Wait(0);
                Assert.True(acquired, "Unable to acquire the lock on the semaphore within the file lock");

                // Hold the lock for a little bit.
                Thread.Sleep(TimeSpan.FromMilliseconds(1));

                verificationSemaphore.Release();
            };

            Assert.Throws<InvalidOperationException>(() =>
            {
                ConcurrencyUtilities.ExecuteWithFileLocked(
                    path,
                    lockedActionSync,
                    acquireFileStream: (s) => throw new UnauthorizedAccessException(),
                    numberOfRetries: 3);
            });
        }
    }
}
