// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement
{
    public static class GatherExceptionHelpers
    {
        /// <summary>
        /// Throw if packages.config contains an AllowedVersions entry for the target, 
        /// and no packages outside of that range have been found.
        /// </summary>
        /// <param name="target">target package id</param>
        /// <param name="packagesConfig">entries from packages.config</param>
        /// <param name="availablePackages">gathered packages</param>
        public static void ThrowIfVersionIsDisallowedByPackagesConfig(string target,
            IEnumerable<Packaging.PackageReference> packagesConfig,
            IEnumerable<PackageDependencyInfo> availablePackages,
            ILogger logger)
        {
            ThrowIfVersionIsDisallowedByPackagesConfig(new string[] { target }, packagesConfig, availablePackages, logger);
        }

        /// <summary>
        /// Throw if packages.config contains an AllowedVersions entry for the target, 
        /// and no packages outside of that range have been found.
        /// </summary>
        /// <param name="targets">target package ids</param>
        /// <param name="packagesConfig">entries from packages.config</param>
        /// <param name="availablePackages">gathered packages</param>
        public static void ThrowIfVersionIsDisallowedByPackagesConfig(IEnumerable<string> targets,
            IEnumerable<Packaging.PackageReference> packagesConfig,
            IEnumerable<PackageDependencyInfo> availablePackages,
            ILogger logger)
        {
            foreach (var target in targets)
            {
                var configEntry = packagesConfig.FirstOrDefault(reference => reference.HasAllowedVersions
                    && StringComparer.OrdinalIgnoreCase.Equals(target, reference.PackageIdentity.Id));

                if (configEntry != null)
                {
                    logger.LogMinimal(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.PackagesConfigAllowedVersions,
                        configEntry.AllowedVersions.PrettyPrint(),
                        "packages.config"));

                    var packagesForId = availablePackages.Where(package => StringComparer.OrdinalIgnoreCase.Equals(target, package.Id));

                    // check if package versions exist, but none satisfy the allowed range
                    if (packagesForId.Any() && !packagesForId.Any(package => configEntry.AllowedVersions.Satisfies(package.Version)))
                    {
                        // Unable to resolve '{0}'. An additional constraint {1} defined in {2} prevents this operation.
                        throw new InvalidOperationException(
                            String.Format(CultureInfo.CurrentCulture,
                                Strings.PackagesConfigAllowedVersionConflict,
                                target,
                                configEntry.AllowedVersions.PrettyPrint(),
                                "packages.config"));
                    }
                }
            }
        }

        /// <summary>
        /// Throw if packages.config contains a newer version of the package already 
        /// </summary>
        /// <param name="target">target package id</param>
        /// <param name="packagesConfig">entries from packages.config</param>
        /// <param name="availablePackages">gathered packages</param>
        public static void ThrowIfNewerVersionAlreadyReferenced(string target,
            IEnumerable<Packaging.PackageReference> packagesConfig,
            IEnumerable<PackageDependencyInfo> availablePackages)
        {
            var configEntry = packagesConfig.FirstOrDefault(r => r.PackageIdentity.Id.Equals(target, StringComparison.OrdinalIgnoreCase));
            var availablePackage = availablePackages.FirstOrDefault(p => p.Id.Equals(target, StringComparison.OrdinalIgnoreCase));

            if (configEntry != null && availablePackage != null && configEntry.PackageIdentity.Version > availablePackage.Version)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.NewerVersionAlreadyReferenced, target));
            }
        }
    }
}
