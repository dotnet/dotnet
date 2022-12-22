// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    public static class PackageGraphAnalysisUtilities
    {
        /// <summary>
        /// Returns package dependency info for the given package identities in the given resource. It returns null if any protocol errors occur.
        /// For example, the feed is not accessible.
        /// </summary>
        /// <param name="packageIdentities">A collection of <see cref="PackageIdentity"/> to get info for.</param>
        /// <param name="nuGetFramework">Framework for determining the dependency groups of packages</param>
        /// <param name="dependencyInfoResource">The resource to fetch dependency info from. Could be http/file feed/global packages folder/solution packages folder.</param>
        /// <param name="sourceCacheContext">Caching context. Only really applicable when the dependency info resource is http based.</param>
        /// <param name="includeUnresolved">Whether to include unresolved packages in the list. If true, the unresolved packages will have an empty dependencies collection.</param>
        /// <param name="logger">logger</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>A collection of <see cref="PackageDependencyInfo"/>, null if a protocol exception happens.  </returns>
        public static async Task<IEnumerable<PackageDependencyInfo>> GetDependencyInfoForPackageIdentitiesAsync(IEnumerable<PackageIdentity> packageIdentities,
            NuGetFramework nuGetFramework,
            DependencyInfoResource dependencyInfoResource,
            SourceCacheContext sourceCacheContext,
            bool includeUnresolved,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var results = new HashSet<PackageDependencyInfo>(PackageIdentity.Comparer);
                foreach (var package in packageIdentities)
                {
                    var packageDependencyInfo = await dependencyInfoResource.ResolvePackage(
                        package,
                        nuGetFramework,
                        sourceCacheContext,
                        logger,
                        cancellationToken);

                    if (packageDependencyInfo != null)
                    {
                        results.Add(packageDependencyInfo);
                    }
                    else if (includeUnresolved)
                    {
                        results.Add(new PackageDependencyInfo(package, null));
                    }
                }

                return results;
            }
            catch (NuGetProtocolException e)
            {
                logger.LogWarning(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Return the packages from a list that have a dependency on a specific package (id and version).
        /// </summary>
        /// <param name="package"></param>
        /// <param name="packageDependencyInfos"></param>
        /// <returns></returns>
        public static IList<PackageDependencyInfo> GetDependantPackages(PackageDependencyInfo package, IList<PackageDependencyInfo> packageDependencyInfos)
        {
            var dependantPackages = new List<PackageDependencyInfo>();

            foreach (var packageDependencyInfo in packageDependencyInfos)
            {
                if (packageDependencyInfo.Dependencies.Any(d => package.Id == d.Id && package.Version == d.VersionRange.MinVersion))
                {
                    dependantPackages.Add(packageDependencyInfo);
                }
            }

            return dependantPackages;
        }

        /// <summary>
        /// Given <paramref name="packageDependencyInfos"/> generates a collection of <see cref="PackageWithDependants"/> with the dependants populated correctly.
        /// </summary>
        /// <returns>A collection of <see cref="PackageWithDependants"/></returns>
        public static IList<PackageWithDependants> GetPackagesWithDependants(IList<PackageDependencyInfo> packageDependencyInfos)
        {
            var packageWithDependants = new List<PackageWithDependants>();

            foreach (var package in packageDependencyInfos)
            {
                packageWithDependants.Add(new PackageWithDependants(package, GetDependantPackages(package, packageDependencyInfos).ToArray()));
            }
            return packageWithDependants;
        }
    }
}
