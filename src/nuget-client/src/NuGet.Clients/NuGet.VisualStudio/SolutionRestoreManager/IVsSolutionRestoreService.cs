// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Represents a package restore service API for integration with a project system.
    /// </summary>
    [ComImport]
    [Guid("2b046428-ca39-40bb-8b4b-7dd1d96118cb")]
    public interface IVsSolutionRestoreService
    {
        /// <summary>
        /// A task providing last/current restore operation status.
        /// Could be null if restore has not started yet.
        /// </summary>
        /// <remarks>
        /// This task is a reflection of the current state of the current-restore-operation or
        /// recently-completed-restore. The usage of this property will be to continue,
        /// e.g. to build solution or something) on completion of this task.
        /// Also, on completion, if the task returns false then it means the restore failed and
        /// the build task will be terminated.
        /// </remarks>
        Task<bool> CurrentRestoreOperation { get; }

        /// <summary>
        /// An entry point used by CPS to indicate given project needs to be restored.
        /// </summary>
        /// <param name="projectUniqueName">
        /// Unique identifier of the project. Should be a full path to project file.
        /// </param>
        /// <param name="projectRestoreInfo">Metadata <see cref="IVsProjectRestoreInfo"/> needed for restoring the project.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>
        /// Returns a restore task corresponding to the nominated project request.
        /// NuGet will batch restore requests so it's possible the same restore task will be returned for multiple projects.
        /// When the requested restore operation for the given project completes the task will indicate operation success or failure.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="projectUniqueName" /> is not the path of a project file.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="projectRestoreInfo" /> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="token" /> is cancelled.</exception>
        Task<bool> NominateProjectAsync(string projectUniqueName, IVsProjectRestoreInfo projectRestoreInfo, CancellationToken token);
    }
}
