// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.PackageManagement.VisualStudio
{
    public interface IPackageReferenceProject
    {
        /// <summary>
        /// Gets both the installed (top level) and transitive package references for this project, optionally including transitive origins for transitive packages.
        /// Returns the package reference as two separate lists (installed and transitive).
        /// </summary>
        /// <param name="includeTransitiveOrigins">Set it to <c>true</c> to get transitive origins in transitive packages list</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>A <see cref="ProjectPackages"/>A struct with two lists: installed and transitive packages</returns>
        public Task<ProjectPackages> GetInstalledAndTransitivePackagesAsync(bool includeTransitiveOrigins, CancellationToken token);

        /// <summary>
        /// Gets packageFolders section from assets file
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>A collection of strings representing all packageFolders found in assets file, or empty if not found</returns>
        public Task<IReadOnlyCollection<string>> GetPackageFoldersAsync(CancellationToken ct);
    }
}
