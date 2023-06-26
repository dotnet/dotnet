// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Coordinates solution restore jobs execution.
    /// </summary>
    public interface ISolutionRestoreWorker
    {
        /// <summary>
        /// Returns currently running or last completed restore operation's result.
        /// </summary>
        Task<bool> CurrentRestoreOperation { get; }

        /// <summary>
        /// Returns true when it's executing a restore operation.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Returns true when restore is running or additional restores are pending.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Joinable task factory to synchronize with the worker.
        /// </summary>
        JoinableTaskFactory JoinableTaskFactory { get; }

        /// <summary>
        /// Schedules background restore operation.
        /// </summary>
        /// <param name="request">Restore request.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that completes when scheduled operation completes.</returns>
        Task<bool> ScheduleRestoreAsync(SolutionRestoreRequest request, CancellationToken token);

        /// <summary>
        /// Run solution restore job.
        /// </summary>
        /// <param name="request">Restore request.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that completes when restore completes.</returns>
        Task<bool> RestoreAsync(SolutionRestoreRequest request, CancellationToken token);

        /// <summary>
        /// Cleans incremental restore cache.
        /// </summary>
        Task CleanCacheAsync();
    }
}
