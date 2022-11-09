// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    internal static class PackageServiceUtilities
    {
        /// <summary>
        /// Checks whether the specified package and version are in the list.
        /// If the nugetVersion is null, then this method only checks whether given package id is in list.
        /// </summary>
        /// <param name="installedPackageReferences">installed package references</param>
        /// <param name="packageId">packageId to check, can't be null or empty.</param>
        /// <param name="nugetVersion">nuGetVersion to check, can be null.</param>
        /// <returns>Whether the package is in the list.</returns>
        /// <exception cref="ArgumentNullException"> if <paramref name="installedPackageReferences"/> is null</exception>
        /// <exception cref="ArgumentException"> if <paramref name="packageId"/> is null or empty</exception>
        internal static bool IsPackageInList(IEnumerable<PackageReference> installedPackageReferences, string packageId, NuGetVersion nugetVersion)
        {
            if (installedPackageReferences == null)
            {
                throw new ArgumentNullException(nameof(installedPackageReferences));
            }

            if (string.IsNullOrEmpty(packageId))
            {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof(packageId));
            }

            return installedPackageReferences.Any(p =>
                StringComparer.OrdinalIgnoreCase.Equals(p.PackageIdentity.Id, packageId) &&
                (nugetVersion != null ?
                    VersionComparer.VersionRelease.Equals(p.PackageIdentity.Version, nugetVersion) :
                    true));
        }
    }
}
