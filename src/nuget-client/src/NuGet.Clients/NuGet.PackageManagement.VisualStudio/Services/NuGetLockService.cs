// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using AsyncLocalInt = System.Threading.AsyncLocal<int>;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// MEF component providing the lock which guarantees non-overlapping execution of NuGet operations.
    /// </summary>
    [Export(typeof(INuGetLockService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class NuGetLockService : INuGetLockService, IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private readonly AsyncLocalInt _lockCount = new AsyncLocalInt();

        private readonly JoinableTaskCollection _joinableTaskCollection;

        private readonly JoinableTaskFactory _joinableTaskFactory;

        [ImportingConstructor]
        public NuGetLockService(JoinableTaskContext joinableTaskContext)
        {
            _joinableTaskCollection = joinableTaskContext.CreateCollection();
            _joinableTaskFactory = joinableTaskContext.CreateFactory(_joinableTaskCollection);
        }

        public bool IsLockHeld => _semaphore.CurrentCount == 0;

        public int LockCount => _lockCount.Value;

        /// <summary>
        /// This method guarantees that only one operation executes at a time globally;
        /// however, once an asynchronous call context has acquired the semaphore,
        /// reentrancy for that call context is allowed without having to wait again on the semaphore.
        /// </summary>
        public async Task<T> ExecuteNuGetOperationAsync<T>(Func<Task<T>> action, CancellationToken token)
        {
            if (_lockCount.Value == 0)
            {
                return await _joinableTaskFactory.RunAsync(async delegate
                {
                    using (_joinableTaskCollection.Join())
                    {
                        await _semaphore.WaitAsync(token);
                    }

                    // Once this thread acquired the lock then increment lockCount
                    _lockCount.Value++;

                    try
                    {
                        // Run it as part of CPS JTF so that it can be joined in Shell JTF collection
                        // and allows other tasks to proceed instead of deadlocking them.
                        return await NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                        {
                            return await action();
                        });
                    }
                    finally
                    {
                        try
                        {
                            _semaphore.Release();
                        }
                        catch (ObjectDisposedException) { }

                        _lockCount.Value--;
                    }
                });

            }
            else
            {
                return await NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    return await action();
                });
            }
        }

        public Task ExecuteNuGetOperationAsync(Func<Task> action, CancellationToken token)
        {
            return ExecuteNuGetOperationAsync(async () =>
            {
                await action();
                return true;
            }, token);
        }

        public void Dispose()
        {
            _semaphore.Dispose();
        }
    }
}
