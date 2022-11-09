// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.LibraryModel;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// A service API providing methods of altering references 
    /// as exposed by the underlying project system.
    /// </summary>
    public interface IProjectSystemReferencesService
    {
        /// <summary>
        /// Adds a new package reference or updates the existing one.
        /// </summary>
        /// <param name="packageReference">A package reference with metadata.</param>
        /// <param name="token">A cancellation token.</param>
        /// <exception cref="NotSupportedException">Thrown when the project system doesn't support package references.</exception>
        /// <remarks>A caller should verify project system's capabilities before calling this method.</remarks>
        Task AddOrUpdatePackageReferenceAsync(
            LibraryDependency packageReference,
            CancellationToken token);

        /// <summary>
        /// Removes a package reference from a legacy CSProj project
        /// </summary>
        /// <param name="packageName">Name of a package to remove from project</param>
        /// <exception cref="NotSupportedException">Thrown when the project system doesn't support package references.</exception>
        /// <remarks>A caller should verify project system's capabilities before calling this method.</remarks>
        Task RemovePackageReferenceAsync(string packageName);
    }
}
