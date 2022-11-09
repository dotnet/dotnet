// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Common package queries implementes as extension methods
    /// </summary>
    public static class PackageCollectionExtensions
    {
        public static NuGetVersion[] GetPackageVersions(this IEnumerable<PackageIdentity> packages, string packageId)
        {
            return packages
                .Where(p => StringComparer.OrdinalIgnoreCase.Equals(p.Id, packageId))
                .Select(p => p.Version)
                .ToArray();
        }

        public static IEnumerable<IGrouping<string, T>> GroupById<T>(this IEnumerable<T> packages) where T : PackageIdentity
        {
            return packages
                .GroupBy(p => p.Id, p => p, StringComparer.OrdinalIgnoreCase);
        }

        public static IEnumerable<PackageIdentity> GetLatest(this IEnumerable<PackageIdentity> packages)
        {
            return packages
                 .GroupById()
                 .Select(g => g.OrderByDescending(x => x.Version).First());
        }

        public static IEnumerable<PackageIdentity> GetEarliest(this IEnumerable<PackageIdentity> packages)
        {
            return packages
                .GroupById()
                .Select(g => g.OrderBy(x => x.Version).First());
        }

        /// <summary>
        /// True if any package reference is AutoReferenced=true.
        /// </summary>
        public static bool IsAutoReferenced(this IEnumerable<PackageCollectionItem> packages, string id)
        {
            return packages.Where(e => StringComparer.OrdinalIgnoreCase.Equals(id, e.Id))
                .Any(e => e.IsAutoReferenced());
        }
    }
}
