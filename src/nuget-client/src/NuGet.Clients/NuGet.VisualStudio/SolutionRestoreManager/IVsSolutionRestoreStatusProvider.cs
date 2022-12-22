// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Provides the status of IVsSolutionRestore.
    /// </summary>
    [ComImport]
    [Guid("BEEE8F17-2174-4380-AE77-C428ACDF07E5")]
    public interface IVsSolutionRestoreStatusProvider
    {
        /// <summary>
        /// IsRestoreCompleteAsync indicates whether or not automatic package restore has pending work.
        /// Automatic package restore applies for both packages.config and PackageReference projects.
        ///
        /// Returns true if all projects in the solution that require nomination have been nominated for restore and all pending restores have completed.
        /// The result does not indicate that restore completed successfully, a failed restore will still return true.
        /// </summary>
        /// <remarks>
        /// Special cases:
        /// * An empty solution will return true.
        /// * If no solution is open this will true.
        /// * An invalid project that does not provide restore details will cause this to return false since restore will not run for that project.
        ///
        /// Restores running due to Install/Update/Uninstall operations are NOT included in this status. Status here is limited to IVsSolutionRestoreService.
        /// </remarks>
        Task<bool> IsRestoreCompleteAsync(CancellationToken token);
    }
}
