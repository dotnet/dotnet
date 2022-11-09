// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Indexing
{
    public class PackageSearchMetadataSplicer : IPackageSearchMetadataSplicer
    {
        /// <summary>
        /// Picks a metadata of a higher package version as a base object. Appends a delayed merge versions task.
        /// </summary>
        /// <param name="lhs">A package metadata</param>
        /// <param name="rhs">Another package metadata with the same id</param>
        /// <returns>Unified package metadata object aggregating attributes from both input objects</returns>
        public IPackageSearchMetadata MergeEntries(IPackageSearchMetadata lhs, IPackageSearchMetadata rhs)
        {
            if (lhs == null)
            {
                throw new ArgumentNullException(nameof(lhs));
            }

            if (rhs == null)
            {
                throw new ArgumentNullException(nameof(rhs));
            }

            if (!string.Equals(lhs.Identity.Id, rhs.Identity.Id, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Cannot merge packages '{lhs.Identity}' and '{rhs.Identity}' because their ids are different.");
            }

            var newerEntry = (lhs.Identity.Version >= rhs.Identity.Version) ? lhs : rhs;
            return newerEntry.WithVersions(() => MergeVersionsAsync(lhs, rhs));
        }

        private static async Task<IEnumerable<VersionInfo>> MergeVersionsAsync(IPackageSearchMetadata lhs, IPackageSearchMetadata rhs)
        {
            var versions = await Task.WhenAll(lhs.GetVersionsAsync(), rhs.GetVersionsAsync());
            return versions
                .SelectMany(v => v) // flatten a list of two lists
                .GroupBy(v => v.Version) // group all by version
                .Select(group => group.First()) // select first VersionInfo for each version
                .ToArray(); // force execution
        }
    }
}
