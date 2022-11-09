// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Exposes methods which mark partially deleted packages and deletes them.
    /// </summary>
    public interface IDeleteOnRestartManager
    {
        /// <summary>
        /// Gets the list of package directories that are still need to be deleted in the
        /// local package repository.
        /// </summary>
        IReadOnlyList<string> GetPackageDirectoriesMarkedForDeletion();

        /// <summary>
        /// Checks for any package directories that are pending to be deleted and raises the
        /// <see cref="PackagesMarkedForDeletionFound"/> event.
        /// </summary>
        void CheckAndRaisePackageDirectoriesMarkedForDeletion();

        /// <summary>
        /// Marks package directory for future removal if it was not fully deleted during the normal uninstall process
        /// if the directory does not contain any added or modified files.
        /// </summary>
        void MarkPackageDirectoryForDeletion(
            PackageIdentity package,
            string packageDirectory,
            INuGetProjectContext projectContext);

        /// <summary>
        /// Attempts to remove marked package directories that were unable to be fully deleted during the original
        /// uninstall.
        /// </summary>
        Task DeleteMarkedPackageDirectoriesAsync(INuGetProjectContext projectContext);

        /// <summary>
        /// Occurs when it is detected that the one or more packages are marked for deletion in the current solution.
        /// </summary>
        event EventHandler<PackagesMarkedForDeletionEventArgs> PackagesMarkedForDeletionFound;
    }
}
