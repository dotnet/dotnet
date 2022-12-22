// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGet.Resolver
{
    public class PackageResolverContext
    {
        /// <summary>
        /// Resolver context
        /// </summary>
        /// <param name="dependencyBehavior">behavior for non-target packages</param>
        /// <param name="targetIds">packages to install or update</param>
        /// <param name="requiredPackageIds">packages required in the solution</param>
        /// <param name="packagesConfig">existing packages</param>
        /// <param name="preferredVersions">preferred package versions or the installed version of a package</param>
        /// <param name="availablePackages">all packages from the gather stage</param>
        public PackageResolverContext(DependencyBehavior dependencyBehavior,
            IEnumerable<string> targetIds,
            IEnumerable<string> requiredPackageIds,
            IEnumerable<Packaging.PackageReference> packagesConfig,
            IEnumerable<PackageIdentity> preferredVersions,
            IEnumerable<SourcePackageDependencyInfo> availablePackages,
            IEnumerable<PackageSource> packageSources,
            Common.ILogger log)
        {
            if (targetIds == null)
            {
                throw new ArgumentNullException(nameof(targetIds));
            }

            if (requiredPackageIds == null)
            {
                throw new ArgumentNullException(nameof(requiredPackageIds));
            }

            if (packagesConfig == null)
            {
                throw new ArgumentNullException(nameof(packagesConfig));
            }

            if (preferredVersions == null)
            {
                throw new ArgumentNullException(nameof(preferredVersions));
            }

            if (availablePackages == null)
            {
                throw new ArgumentNullException(nameof(availablePackages));
            }

            if (packageSources == null)
            {
                throw new ArgumentNullException(nameof(packageSources));
            }

            DependencyBehavior = dependencyBehavior;
            TargetIds = new HashSet<string>(targetIds, StringComparer.OrdinalIgnoreCase);

            RequiredPackageIds = new HashSet<string>(requiredPackageIds, StringComparer.OrdinalIgnoreCase);
            RequiredPackageIds.UnionWith(targetIds);

            PackagesConfig = packagesConfig;
            PreferredVersions = new HashSet<PackageIdentity>(preferredVersions, PackageIdentity.Comparer);
            AvailablePackages = availablePackages;
            PackageSources = packageSources;
            Log = log;
            Debug.Assert(PreferredVersions.GroupBy(p => p.Id, StringComparer.OrdinalIgnoreCase)
                .All(group => group.Count() == 1), "duplicate preferred ids");
        }

        /// <summary>
        /// New packages to install or update. These will prefer the highest version.
        /// </summary>
        public HashSet<string> TargetIds { get; }

        /// <summary>
        /// Existing packages that are required, and the target ids that are required.
        /// These packages are required for the solution.
        /// </summary>
        public HashSet<string> RequiredPackageIds { get; }

        /// <summary>
        /// The existing state of the project from packages.config
        /// </summary>
        public IEnumerable<Packaging.PackageReference> PackagesConfig { get; }

        /// <summary>
        /// Preferred versions of each package. If the package does not exist here
        /// it will use the dependency behavior, or if it is a target the highest
        /// version will be used.
        /// </summary>
        public HashSet<PackageIdentity> PreferredVersions { get; }

        /// <summary>
        /// All packages available to use in the solution.
        /// </summary>
        public IEnumerable<SourcePackageDependencyInfo> AvailablePackages { get; }

        /// <summary>
        /// Dependency behavior
        /// </summary>
        public DependencyBehavior DependencyBehavior { get; }

        /// <summary>
        /// Package Sources
        /// </summary>
        public IEnumerable<PackageSource> PackageSources { get; }

        /// <summary>
        /// Logging adapter
        /// </summary>
        public Common.ILogger Log { get; }

    }
}
