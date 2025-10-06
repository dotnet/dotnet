// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// GatherCache contains cached results from DependencyInfoResource providers
    /// This class is for internal use or testing only.
    /// </summary>
    public class GatherCache
    {
        // These are typically the primary targets or installed packages which perform the lookup on only a single version.
        private readonly ConcurrentDictionary<GatherSingleCacheKey, SourcePackageDependencyInfo> _singleVersion
            = new ConcurrentDictionary<GatherSingleCacheKey, SourcePackageDependencyInfo>();

        // This contains the full list of package versions for the package id
        private readonly ConcurrentDictionary<GatherAllCacheKey, List<SourcePackageDependencyInfo>> _allPackageVersions
            = new ConcurrentDictionary<GatherAllCacheKey, List<SourcePackageDependencyInfo>>();

        /// <summary>
        /// Add a single package
        /// </summary>
        public void AddPackageFromSingleVersionLookup(
            Configuration.PackageSource source,
            PackageIdentity identity,
            NuGetFramework framework,
            SourcePackageDependencyInfo package)
        {
            var key = new GatherSingleCacheKey(identity, source, framework);

            _singleVersion.TryAdd(key, package);
        }

        /// <summary>
        /// Add the full list of versions for a package
        /// </summary>
        public void AddAllPackagesForId(
            Configuration.PackageSource source,
            string packageId,
            NuGetFramework framework,
            List<SourcePackageDependencyInfo> packages)
        {
            var key = new GatherAllCacheKey(packageId, source, framework);

            _allPackageVersions.TryAdd(key, packages);
        }

        /// <summary>
        /// Retrieve an exact version of a package
        /// </summary>
        public GatherCacheResult GetPackage(
            Configuration.PackageSource source,
            PackageIdentity package,
            NuGetFramework framework)
        {
            var key = new GatherSingleCacheKey(package, source, framework);

            var hasEntry = false;

            SourcePackageDependencyInfo result;

            hasEntry = _singleVersion.TryGetValue(key, out result);

            if (!hasEntry)
            {
                // Try finding the packages from cached all packages results
                var allPackagesResult = GetPackages(source, package.Id, framework);

                if (allPackagesResult.HasEntry)
                {
                    hasEntry = true;

                    // Find the exact package version in the list. The result may be null
                    // if that version does not exist.
                    result = allPackagesResult.Packages
                        .FirstOrDefault(p => package.Equals(p));
                }
            }

            var packages = new List<SourcePackageDependencyInfo>(1);

            if (result != null)
            {
                packages.Add(result);
            }

            return new GatherCacheResult(hasEntry, packages);
        }

        /// <summary>
        /// Retrieve all versions of a package id
        /// </summary>
        public GatherCacheResult GetPackages(
            Configuration.PackageSource source,
            string packageId,
            NuGetFramework framework)
        {
            var key = new GatherAllCacheKey(packageId, source, framework);

            List<SourcePackageDependencyInfo> result;

            var hasEntry = _allPackageVersions.TryGetValue(key, out result);

            return new GatherCacheResult(hasEntry, result);
        }

        /// <summary>
        /// Cache key for a package id and version
        /// </summary>
        private class GatherSingleCacheKey : IEquatable<GatherSingleCacheKey>
        {
            public GatherSingleCacheKey(
                PackageIdentity package,
                Configuration.PackageSource source,
                NuGetFramework framework)
            {
                Package = package;
                Source = source;
                Framework = framework;
            }

            public NuGetFramework Framework { get; }

            public PackageIdentity Package { get; }

            public Configuration.PackageSource Source { get; }

            public bool Equals(GatherSingleCacheKey other)
            {
                if (other == null)
                {
                    return false;
                }

                if (Object.ReferenceEquals(this, other))
                {
                    return true;
                }

                return Package.Equals(other.Package)
                    && Source.Equals(other.Source)
                    && Framework.Equals(other.Framework);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as GatherSingleCacheKey);
            }

            public override int GetHashCode()
            {
                var combiner = new HashCodeCombiner();

                combiner.AddObject(Package);
                combiner.AddObject(Source);
                combiner.AddObject(Framework);

                return combiner.CombinedHash;
            }
        }

        /// <summary>
        /// Cache key for a package id and all versions
        /// </summary>
        private class GatherAllCacheKey : IEquatable<GatherAllCacheKey>
        {
            public GatherAllCacheKey(
                string packageId,
                Configuration.PackageSource source,
                NuGetFramework framework)
            {
                PackageId = packageId;
                Source = source;
                Framework = framework;
            }

            public NuGetFramework Framework { get; set; }
            public string PackageId { get; set; }
            public Configuration.PackageSource Source { get; set; }

            public bool Equals(GatherAllCacheKey other)
            {
                if (other == null)
                {
                    return false;
                }

                if (Object.ReferenceEquals(this, other))
                {
                    return true;
                }

                return string.Equals(PackageId, other.PackageId, StringComparison.OrdinalIgnoreCase)
                    && Source.Equals(other.Source)
                    && Framework.Equals(other.Framework);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as GatherAllCacheKey);
            }

            public override int GetHashCode()
            {
                var combiner = new HashCodeCombiner();

                combiner.AddObject(PackageId.ToUpperInvariant());
                combiner.AddObject(Source);
                combiner.AddObject(Framework);

                return combiner.CombinedHash;
            }
        }
    }
}
