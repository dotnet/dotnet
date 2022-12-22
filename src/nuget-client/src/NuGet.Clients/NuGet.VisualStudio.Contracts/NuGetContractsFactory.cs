// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.VisualStudio.Contracts
{
    /// <summary>Factory to create contract types</summary>
    /// <remarks>Trying to be forwards compatible with what C#9 records are going to be</remarks>
    public static class NuGetContractsFactory
    {
        /// <summary>Create a <see cref="NuGetInstalledPackage"/></summary>
        /// <param name="id">Package Id</param>
        /// <param name="requestedRange">The requested range</param>
        /// <param name="version">The installed version</param>
        /// <param name="installPath">The package install path</param>
        /// <param name="directDependency">The package a direct dependency of the project</param>
        /// <returns><see cref="NuGetInstalledPackage"/></returns>
        public static NuGetInstalledPackage CreateNuGetInstalledPackage(string id, string requestedRange, string version, string installPath, bool directDependency)
        {
            return new NuGetInstalledPackage(id, requestedRange, version, installPath, directDependency);
        }

        /// <summary>Create a <see cref="InstalledPackageResultStatus"/></summary>
        /// <param name="status"><see cref="InstalledPackageResultStatus"/></param>
        /// <param name="packages">Read-only collection of <see cref="NuGetInstalledPackage"/></param>
        /// <returns><see cref="InstalledPackagesResult"/></returns>
        public static InstalledPackagesResult CreateInstalledPackagesResult(InstalledPackageResultStatus status, IReadOnlyCollection<NuGetInstalledPackage> packages)
        {
            return new InstalledPackagesResult(status, packages);
        }
    }
}
