// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Resolver
{
    public class ResolverPackage : SourcePackageDependencyInfo, IEquatable<ResolverPackage>
    {
        /// <summary>
        /// An absent package represents that the package is not needed in the solution.
        /// </summary>
        public bool Absent { get; }
        private readonly SortedDictionary<string, VersionRange> _dependencyIds;
        private readonly int _hash;

        public ResolverPackage(string id)
            : this(id, null)
        {
        }

        public ResolverPackage(string id, NuGetVersion? version)
            : this(id, version, Enumerable.Empty<PackageDependency>(), true, false)
        {
        }

        public ResolverPackage(string id, NuGetVersion? version, IEnumerable<PackageDependency>? dependencies, bool listed, bool absent)
            : base(
                new PackageIdentity(id ?? throw new ArgumentNullException(nameof(id)), version),
                dependencies ?? Enumerable.Empty<PackageDependency>(),
                listed,
                source: null,
                downloadUri: null,
                packageHash: null)
        {
            Debug.Assert(!Absent || (version == null && dependencies == null), "Invalid absent package");

            Absent = absent;
            _dependencyIds = new SortedDictionary<string, VersionRange>(StringComparer.OrdinalIgnoreCase);

            // Create a dictionary to optimize dependency lookups
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    if (_dependencyIds.ContainsKey(dependency.Id))
                    {
                        throw new InvalidOperationException(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.DuplicateDependencyIdsError,
                            id,
                            version,
                            dependency.Id));
                    }

                    _dependencyIds.Add(dependency.Id, dependency.VersionRange == null ? VersionRange.All : dependency.VersionRange);
                }
            }

            // Calculate the hash once
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Absent);
            combiner.AddObject(base.GetHashCode());

            _hash = combiner.CombinedHash;
        }

        public ResolverPackage(PackageDependencyInfo info, bool listed, bool absent)
            : this(
                (info ?? throw new ArgumentNullException(nameof(info))).Id,
                info.Version,
                info.Dependencies,
                listed,
                absent)
        {
        }

        /// <summary>
        /// Find the version range for the given package. The package may not exist.
        /// </summary>
        public VersionRange? FindDependencyRange(string id)
        {
            _dependencyIds.TryGetValue(id, out VersionRange? range);
            return range;
        }

        public override string ToString()
        {
            if (Absent)
            {
                return String.Format(CultureInfo.InvariantCulture, "Absent {0}", Id);
            }
            else
            {
                return String.Format(CultureInfo.InvariantCulture, "{0} {1}", Id, Version);
            }
        }

        public bool Equals(ResolverPackage? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //if (ReferenceEquals(other, this))
            //{
            //    return true;
            //}

            return this.Absent == other.Absent && PackageIdentity.Comparer.Equals(other, this);
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ResolverPackage;

            if (other != null)
            {
                return Equals(other);
            }

            return false;
        }
    }
}
