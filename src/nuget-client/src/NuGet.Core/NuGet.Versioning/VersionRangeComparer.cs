// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;

namespace NuGet.Versioning
{
    /// <summary>
    /// A version range comparer capable of using different VersionComparers to check if ranges are equivalent.
    /// </summary>
    public class VersionRangeComparer : IVersionRangeComparer
    {
        private readonly IVersionComparer _versionComparer;

        /// <summary>
        /// Default version range comparer.
        /// </summary>
        public VersionRangeComparer()
            : this(VersionComparer.Default)
        {
        }

        /// <summary>
        /// Compare versions with a specific VersionComparison
        /// </summary>
        public VersionRangeComparer(VersionComparison versionComparison)
            : this(VersionComparer.Get(versionComparison))
        {
        }

        /// <summary>
        /// Compare versions with a specific IVersionComparer
        /// </summary>
        public VersionRangeComparer(IVersionComparer versionComparer)
        {
            if (versionComparer == null)
            {
                throw new ArgumentNullException(nameof(versionComparer));
            }

            _versionComparer = versionComparer;
        }

        /// <summary>
        /// Default Version comparer
        /// </summary>
        public static IVersionRangeComparer Default { get; } = new VersionRangeComparer(VersionComparer.Default);

        internal static IVersionRangeComparer Version { get; } = new VersionRangeComparer(VersionComparer.Version);

        /// <summary>
        /// Compare versions using the Version and Release
        /// </summary>
        public static IVersionRangeComparer VersionRelease { get; } = new VersionRangeComparer(VersionComparer.VersionRelease);

        internal static IVersionRangeComparer VersionReleaseMetadata { get; } = new VersionRangeComparer(VersionComparer.VersionReleaseMetadata);

        public static IVersionRangeComparer Get(VersionComparison versionComparison)
        {
            return versionComparison switch
            {
                VersionComparison.Default => Default,
                VersionComparison.Version => Version,
                VersionComparison.VersionRelease => VersionRelease,
                VersionComparison.VersionReleaseMetadata => VersionReleaseMetadata,
                _ => new VersionRangeComparer(versionComparison)
            };
        }

        /// <summary>
        /// Checks if two version ranges are equivalent. This follows the rules of the version comparer
        /// when checking the bounds.
        /// </summary>
        public bool Equals(VersionRangeBase? x, VersionRangeBase? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(y, null)
                || ReferenceEquals(x, null))
            {
                return false;
            }

            return x.IsMinInclusive == y.IsMinInclusive
                && y.IsMaxInclusive == x.IsMaxInclusive
#pragma warning disable CS8604 // Possible null reference argument.
                // BCL missing nullable annotations on IEqualityComparer<T> before .NET 5
                && _versionComparer.Equals(y.MinVersion, x.MinVersion)
                && _versionComparer.Equals(y.MaxVersion, x.MaxVersion);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        /// <summary>
        /// Creates a hash code based on all properties of the range. This follows the rules of the
        /// version comparer when comparing the version bounds.
        /// </summary>
        public int GetHashCode(VersionRangeBase obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            var combiner = new HashCodeCombiner();

            combiner.AddObject(obj.IsMinInclusive);
            combiner.AddObject(obj.IsMaxInclusive);
            if (obj.HasLowerBound)
            {
                combiner.AddObject(_versionComparer.GetHashCode(obj.MinVersion));
            }
            if (obj.HasUpperBound)
            {
                combiner.AddObject(_versionComparer.GetHashCode(obj.MaxVersion));
            }

            return combiner.CombinedHash;
        }
    }
}
