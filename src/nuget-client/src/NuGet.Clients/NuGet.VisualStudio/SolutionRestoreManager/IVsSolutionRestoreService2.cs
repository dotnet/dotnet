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
    [Guid("88BE06DF-97B7-47E8-8047-28279C02E401")]
    public interface IVsSolutionRestoreService2
    {
        /// <summary>
        /// An entry point which allows non-NETCore SDK based projects to indicate given project needs to be restored.
        /// </summary>
        /// <param name="projectUniqueName">
        /// Unique identifier of the project. Should be a full path to project file.
        /// </param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>
        /// Returns a restore task corresponding to the nominated project request.
        /// NuGet will batch restore requests so it's possible the same restore task will be returned for multiple projects.
        /// When the requested restore operation for the given project completes the task will indicate operation success or failure.
        /// </returns>
        Task<bool> NominateProjectAsync(string projectUniqueName, CancellationToken token);
    }
}
