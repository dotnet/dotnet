// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// Provides methods for resolving a package and its dependencies. This might change based on the new
    /// dependency resolver.
    /// </summary>
    public abstract class DependencyInfoResource : INuGetResource
    {
        /// <summary>
        /// Retrieve dependency info for a single package.
        /// </summary>
        /// <param name="package">package id and version</param>
        /// <param name="projectFramework">project target framework. This is used for finding the dependency group</param>
        /// <param name="token">cancellation token</param>
        /// <returns>
        /// Returns dependency info for the given package if it exists. If the package is not found null is
        /// returned.
        /// </returns>
        public abstract Task<SourcePackageDependencyInfo> ResolvePackage(PackageIdentity package,
            NuGetFramework projectFramework,
            SourceCacheContext cacheContext,
            ILogger log,
            CancellationToken token);

        /// <summary>
        /// Retrieve the available packages and their dependencies.
        /// </summary>
        /// <remarks>Includes prerelease packages</remarks>
        /// <param name="packageId">package Id to search</param>
        /// <param name="projectFramework">project target framework. This is used for finding the dependency group</param>
        /// <param name="token">cancellation token</param>
        /// <returns>available packages and their dependencies</returns>
        public abstract Task<IEnumerable<SourcePackageDependencyInfo>> ResolvePackages(string packageId,
            NuGetFramework projectFramework,
            SourceCacheContext cacheContext,
            ILogger log,
            CancellationToken token);

        /// <summary>
        /// Retrieve the available packages and their dependencies.
        /// </summary>
        /// <remarks>Includes prerelease packages</remarks>
        /// <param name="packageId">package Id to search</param>
        /// <param name="token">cancellation token</param>
        /// <returns>available packages and their dependencies</returns>
        public virtual Task<IEnumerable<RemoteSourceDependencyInfo>> ResolvePackages(string packageId,
            SourceCacheContext cacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            throw new NotSupportedException();
        }
    }
}
