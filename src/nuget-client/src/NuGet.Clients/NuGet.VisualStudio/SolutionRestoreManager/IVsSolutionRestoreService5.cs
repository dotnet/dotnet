// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.SolutionRestoreManager
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IVsSolutionRestoreService5 : IVsSolutionRestoreService4
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// An entry point used by CPS to indicate given project needs to be restored.
        /// </summary>
        /// <param name="projectUniqueName">
        /// The full path to the project file. In the VS SDK's IVsSolution, this is also known as the unique name.
        /// </param>
        /// <param name="projectRestoreInfo">Metadata <see cref="IVsProjectRestoreInfo3"/> needed for restoring the project.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>
        /// Returns a restore task corresponding to the nominated project request.
        /// NuGet will batch restore requests so it's possible the same restore task will be returned for multiple projects.
        /// When the requested restore operation for the given project completes the task will indicate operation success or failure.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="projectUniqueName" /> is not the path of a project file,
        /// or if <paramref name="projectRestoreInfo"/> has some basic validation errors.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="projectRestoreInfo" /> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="token" /> is cancelled.</exception>
        Task<bool> NominateProjectAsync(string projectUniqueName, IVsProjectRestoreInfo3 projectRestoreInfo, CancellationToken token);
    }
}
