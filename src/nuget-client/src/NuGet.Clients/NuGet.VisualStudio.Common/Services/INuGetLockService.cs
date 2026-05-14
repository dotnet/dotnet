// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Defines a contract for a service providing a locking mechanism which guarantees non-overlapping execution of NuGet operations.
    /// </summary>
    public interface INuGetLockService
    {
        /// <summary>
        /// Gets a value indicating whether any kind of lock is held.
        /// </summary>
        bool IsLockHeld { get; }

        /// <summary>
        /// Returns the total lock count for current async NuGet operation.
        /// </summary>
        int LockCount { get; }

        /// <summary>
        /// Obtains NuGet specific lock and execute action.
        /// </summary>
        /// <param name="action">NuGet action to be executed</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Awaitable Task</returns>
        Task ExecuteNuGetOperationAsync(Func<Task> action, CancellationToken token);

        /// <summary>
        /// Obtains NuGet specific lock and execute action. And return an awaitable task of T.
        /// </summary>
        /// <typeparam name="T">Return type template</typeparam>
        /// <param name="action">NuGet action to be executed</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Awaitable Task with T</returns>
        Task<T> ExecuteNuGetOperationAsync<T>(Func<Task<T>> action, CancellationToken token);

    }
}
