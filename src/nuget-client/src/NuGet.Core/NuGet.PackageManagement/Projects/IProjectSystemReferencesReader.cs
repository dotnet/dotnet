// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Represents an API providing read-only access to references
    /// as exposed by the underlying project system.
    /// </summary>
    public interface IProjectSystemReferencesReader
    {
        /// <summary>
        /// Returns a collection of package references in associated project.
        /// </summary>
        /// <param name="targetFramework">Target framework for evaluation.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>Collection of package references as <see cref="LibraryDependency"/></returns>
        /// <exception cref="NotSupportedException">Thrown when the project system doesn't support package references.</exception>
        /// <remarks>A caller should verify project system's capabilities before calling this method.</remarks>
        Task<IEnumerable<LibraryDependency>> GetPackageReferencesAsync(
            NuGetFramework targetFramework,
            CancellationToken token);

        /// <summary>
        /// Returns a collection of project references in the associated project.
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns></returns>
        Task<IEnumerable<ProjectRestoreReference>> GetProjectReferencesAsync(
            Common.ILogger logger,
            CancellationToken token);
    }
}
