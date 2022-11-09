// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using NuGet.Packaging.Core;

namespace NuGet.Packaging
{
    /// <summary>
    /// Core package resolver
    /// </summary>
    public interface IPackageResolver
    {
        /// <summary>
        /// Resolve a set of packages
        /// </summary>
        /// <param name="targets">Package or packages to install</param>
        /// <param name="availablePackages">All relevant packages. This list must include the target packages.</param>
        /// <returns>A set of packages meeting the package dependency requirements</returns>
        IEnumerable<PackageIdentity> Resolve(IEnumerable<PackageIdentity> targets, IEnumerable<PackageDependencyInfo> availablePackages, CancellationToken token);

        /// <summary>
        /// Resolve a set of packages
        /// </summary>
        /// <param name="targets">Package or packages to install</param>
        /// <param name="availablePackages">
        /// All relevant packages. This list must include the target packages and
        /// installed packages.
        /// </param>
        /// <param name="installedPackages">
        /// Packages already installed into the project. These will be favored as
        /// dependency options.
        /// </param>
        /// <returns>A set of packages meeting the package dependency requirements</returns>
        IEnumerable<PackageIdentity> Resolve(IEnumerable<PackageIdentity> targets, IEnumerable<PackageDependencyInfo> availablePackages, IEnumerable<PackageReference> installedPackages, CancellationToken token);

        /// <summary>
        /// Resolve a set of packages
        /// </summary>
        /// <param name="targets">Package or packages to install</param>
        /// <param name="availablePackages">All relevant packages. This list must include the target packages.</param>
        /// <returns>A set of packages meeting the package dependency requirements</returns>
        IEnumerable<PackageIdentity> Resolve(IEnumerable<string> targets, IEnumerable<PackageDependencyInfo> availablePackages, CancellationToken token);

        /// <summary>
        /// Resolve a set of packages
        /// </summary>
        /// <param name="targets">Package or packages to install</param>
        /// <param name="availablePackages">
        /// All relevant packages. This list must include the target packages and
        /// installed packages.
        /// </param>
        /// <param name="installedPackages">
        /// Packages already installed into the project. These will be favored as
        /// dependency options.
        /// </param>
        /// <returns>A set of packages meeting the package dependency requirements</returns>
        IEnumerable<PackageIdentity> Resolve(IEnumerable<string> targets, IEnumerable<PackageDependencyInfo> availablePackages, IEnumerable<PackageReference> installedPackages, CancellationToken token);
    }
}
