// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// Compares the Id, Version, and Version release label. Version build metadata is ignored.
    /// </summary>
    public class PackageIdentityComparer : IPackageIdentityComparer
    {
        private readonly IVersionComparer _versionComparer;

        /// <summary>
        /// Default version range comparer.
        /// </summary>
        public PackageIdentityComparer()
            : this(new VersionComparer(VersionComparison.Default))
        {
        }

        /// <summary>
        /// Compare versions with a specific VersionComparison
        /// </summary>
        public PackageIdentityComparer(VersionComparison versionComparison)
            : this(new VersionComparer(versionComparison))
        {
        }

        /// <summary>
        /// Compare versions with a specific IVersionComparer
        /// </summary>
        public PackageIdentityComparer(IVersionComparer versionComparer)
        {
            if (versionComparer == null)
            {
                throw new ArgumentNullException(nameof(versionComparer));
            }

            _versionComparer = versionComparer;
        }

        /// <summary>
        /// Default comparer that compares on the id, version, and version release labels.
        /// </summary>
        public static PackageIdentityComparer Default { get; } = new PackageIdentityComparer();

        /// <summary>
        /// True if the package identities are the same when ignoring build metadata.
        /// </summary>
        public bool Equals(PackageIdentity x, PackageIdentity y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null)
                || ReferenceEquals(y, null))
            {
                return false;
            }

            return _versionComparer.Equals(x.Version, y.Version)
                && StringComparer.OrdinalIgnoreCase.Equals(x.Id, y.Id);
        }

        /// <summary>
        /// Hash code of the id and version
        /// </summary>
        public int GetHashCode(PackageIdentity obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            var combiner = new HashCodeCombiner();

            combiner.AddObject(obj.Id, StringComparer.OrdinalIgnoreCase);
            combiner.AddObject(_versionComparer.GetHashCode(obj.Version));

            return combiner.CombinedHash;
        }

        /// <summary>
        /// Compares on the Id first, then version
        /// </summary>
        public int Compare(PackageIdentity x, PackageIdentity y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (ReferenceEquals(x, null))
            {
                return -1;
            }

            if (ReferenceEquals(y, null))
            {
                return 1;
            }

            int result = StringComparer.OrdinalIgnoreCase.Compare(x.Id, y.Id);

            if (result == 0)
            {
                result = _versionComparer.Compare(x.Version, y.Version);
            }

            return result;
        }
    }
}
