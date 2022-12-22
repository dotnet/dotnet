// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Commands;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// An up to date checker for a solution.
    /// </summary>
    public interface ISolutionRestoreChecker
    {
        /// <summary>
        /// Given the current dependency graph spec, perform a fast up to date check and return the dirty projects.
        /// The checker itself caches the DependencyGraphSpec it is provided and the last restore status, reported through <see cref="ReportStatus(IReadOnlyList{RestoreSummary})"/>.
        /// Accounts for changes in the PackageSpec and marks all the parent projects as dirty as well.
        /// Additionally, ensures that the expected output files have the same timestamps as the last reported status
        /// <see cref="ReportStatus(IReadOnlyList{RestoreSummary})"/>
        /// Finally we will use the logger provided to replay any warnings necessary. Whether warnings are replayed in conditional on the <see cref="ProjectRestoreSettings"/> in the <see cref="PackageSpec"/>.
        /// </summary>
        /// <param name="dependencyGraphSpec">The current dependency graph spec.</param>
        /// <param name="logger">A logger that will be used to replay warnings for projects that no-op if necessary.</param>
        /// <returns>Unique ids of the dirty projects</returns>
        /// <remarks>Note that this call is stateful. This method may end up caching the dependency graph spec, so do not invoke multiple times. 
        /// Ideally each <see cref="PerformUpToDateCheck(DependencyGraphSpec, ILogger)"/> call should be followed by a <see cref="ReportStatus(IReadOnlyList{RestoreSummary})"/> call.
        /// </remarks>
        IEnumerable<string> PerformUpToDateCheck(DependencyGraphSpec dependencyGraphSpec, ILogger logger);

        /// <summary>
        /// Report the status of all the projects restored. 
        /// </summary>
        /// <param name="restoreSummaries"></param>
        /// <remarks>Note that this call is stateful. This method may end up caching the dependency graph spec, so do not invoke multiple times.
        ///  Ideally <see cref="PerformUpToDateCheck(DependencyGraphSpec)"/> call should be followed by a <see cref="ReportStatus(IReadOnlyList{RestoreSummary})"/> call.
        /// </remarks>
        void SaveRestoreStatus(IReadOnlyList<RestoreSummary> restoreSummaries);

        /// <summary>
        /// Clears any cached values. This is meant to mimic restores that overwrite the incremental restore optimizations.
        /// </summary>
        void CleanCache();
    }
}
