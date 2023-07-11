// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace NuGet.Versioning
{
    /// <summary>
    /// A base version range that handles ranges only and not any of the preferred version logic.
    /// </summary>
    public abstract class VersionRangeBase : IEquatable<VersionRangeBase>
    {
        private readonly bool _includeMinVersion;
        private readonly bool _includeMaxVersion;
        private readonly NuGetVersion? _minVersion;
        private readonly NuGetVersion? _maxVersion;

        /// <summary>
        /// Creates a VersionRange with the given min and max.
        /// </summary>
        /// <param name="minVersion">Lower bound of the version range.</param>
        /// <param name="includeMinVersion">True if minVersion satisfies the condition.</param>
        /// <param name="maxVersion">Upper bound of the version range.</param>
        /// <param name="includeMaxVersion">True if maxVersion satisfies the condition.</param>
        public VersionRangeBase(
            NuGetVersion? minVersion = null,
            bool includeMinVersion = true,
            NuGetVersion? maxVersion = null,
            bool includeMaxVersion = false)
        {
            _minVersion = minVersion;
            _maxVersion = maxVersion;
            _includeMinVersion = includeMinVersion;
            _includeMaxVersion = includeMaxVersion;
        }

        /// <summary>
        /// True if MinVersion exists;
        /// </summary>
        [MemberNotNullWhen(true, nameof(MinVersion))]
        public bool HasLowerBound
        {
            get { return _minVersion != null; }
        }

        /// <summary>
        /// True if MaxVersion exists.
        /// </summary>
        [MemberNotNullWhen(true, nameof(MaxVersion))]
        public bool HasUpperBound
        {
            get { return _maxVersion != null; }
        }

        /// <summary>
        /// True if both MinVersion and MaxVersion exist.
        /// </summary>
        [MemberNotNullWhen(true, nameof(MinVersion))]
        [MemberNotNullWhen(true, nameof(MaxVersion))]
        public bool HasLowerAndUpperBounds
        {
            get { return HasLowerBound && HasUpperBound; }
        }

        /// <summary>
        /// True if MinVersion exists and is included in the range.
        /// </summary>
        [MemberNotNullWhen(true, nameof(MaxVersion))]
        public bool IsMinInclusive
        {
            get { return HasLowerBound && _includeMinVersion; }
        }

        /// <summary>
        /// True if MaxVersion exists and is included in the range.
        /// </summary>
        [MemberNotNullWhen(true, nameof(MaxVersion))]
        public bool IsMaxInclusive
        {
            get { return HasUpperBound && _includeMaxVersion; }
        }

        /// <summary>
        /// Maximum version allowed by this range.
        /// </summary>
        public NuGetVersion? MaxVersion
        {
            get { return _maxVersion; }
        }

        /// <summary>
        /// Minimum version allowed by this range.
        /// </summary>
        public NuGetVersion? MinVersion
        {
            get { return _minVersion; }
        }

        /// <summary>
        /// Determines if an NuGetVersion meets the requirements.
        /// </summary>
        /// <param name="version">SemVer to compare</param>
        /// <returns>True if the given version meets the version requirements.</returns>
        public bool Satisfies(NuGetVersion version)
        {
            // ignore metadata by default when finding a range.
            return Satisfies(version, VersionComparer.VersionRelease);
        }

        /// <summary>
        /// Determines if an NuGetVersion meets the requirements using the given mode.
        /// </summary>
        /// <param name="version">SemVer to compare</param>
        /// <param name="versionComparison">VersionComparison mode used to determine the version range.</param>
        /// <returns>True if the given version meets the version requirements.</returns>
        public bool Satisfies(NuGetVersion version, VersionComparison versionComparison)
        {
            return Satisfies(version, VersionComparer.Get(versionComparison));
        }

        /// <summary>
        /// Determines if an NuGetVersion meets the requirements using the version comparer.
        /// </summary>
        /// <param name="version">SemVer to compare.</param>
        /// <param name="comparer">Version comparer used to determine if the version criteria is met.</param>
        /// <returns>True if the given version meets the version requirements.</returns>
        public bool Satisfies(NuGetVersion version, IVersionComparer comparer)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            // Determine if version is in the given range using the comparer.
            var condition = true;
            if (HasLowerBound)
            {
                if (IsMinInclusive)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    // BCL is missing nullable annotations before net5.0
                    condition &= comparer.Compare(MinVersion, version) <= 0;
                }
                else
                {
                    condition &= comparer.Compare(MinVersion, version) < 0;
                }
            }

            if (HasUpperBound)
            {
                if (IsMaxInclusive)
                {
                    condition &= comparer.Compare(MaxVersion, version) >= 0;
                }
                else
                {
                    condition &= comparer.Compare(MaxVersion, version) > 0;
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }

            return condition;
        }

        /// <summary>
        /// Compares the object as a VersionRange with the default comparer
        /// </summary>
        public override bool Equals(object? obj)
        {
            var range = obj as VersionRangeBase;

            if (range != null)
            {
                return VersionRangeComparer.Default.Equals(this, range);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code using the default comparer.
        /// </summary>
        public override int GetHashCode()
        {
            return VersionRangeComparer.Default.GetHashCode(this);
        }

        /// <summary>
        /// Default compare
        /// </summary>
        public bool Equals(VersionRangeBase? other)
        {
            return Equals(other, VersionRangeComparer.Default);
        }

        /// <summary>
        /// Use the VersionRangeComparer for equality checks
        /// </summary>
        public bool Equals(VersionRangeBase? other, IVersionRangeComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

#pragma warning disable CS8604 // Possible null reference argument.
            // IEqualityComparer<T>.Equals doesn't have nullable annotations before .NET 5.0
            return comparer.Equals(this, other);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        /// <summary>
        /// Use a specific VersionComparison for comparison
        /// </summary>
        public bool Equals(VersionRangeBase? other, VersionComparison versionComparison)
        {
            IVersionRangeComparer comparer = VersionRangeComparer.Get(versionComparison);
            return Equals(other, comparer);
        }

        /// <summary>
        /// Use a specific IVersionComparer for comparison
        /// </summary>
        public bool Equals(VersionRangeBase? other, IVersionComparer versionComparer)
        {
            IVersionRangeComparer comparer = new VersionRangeComparer(versionComparer);
            return Equals(other, comparer);
        }

        /// <summary>
        /// SubSet check
        /// </summary>
        public bool IsSubSetOrEqualTo(VersionRangeBase? possibleSuperSet)
        {
            return IsSubSetOrEqualTo(possibleSuperSet, VersionComparer.Default);
        }

        /// <summary>
        /// SubSet check
        /// </summary>
        public bool IsSubSetOrEqualTo(VersionRangeBase? possibleSuperSet, IVersionComparer comparer)
        {
            var rangeComparer = new VersionRangeComparer(comparer);

            var possibleSubSet = this;
            var target = possibleSuperSet;

            if (rangeComparer.Equals(possibleSubSet, VersionRange.None))
            {
                return true;
            }

#pragma warning disable CS8604 // Possible null reference argument.
            // BCL doesn't have nullable annotations for IEqualityComparer<T> before net5.0
            if (rangeComparer.Equals(target, VersionRange.None))
            {
                return false;
            }
#pragma warning restore CS8604 // Possible null reference argument.

            if (target == null)
            {
                target = VersionRange.All;
            }

            if (possibleSubSet == null)
            {
                possibleSubSet = VersionRange.All;
            }

            var result = true;

            if (possibleSubSet.HasLowerBound)
            {
                // normal check
                if (!target.Satisfies(possibleSubSet.MinVersion))
                {
                    // it's possible we didn't need that version, do a special non inclusive check
                    if (!possibleSubSet.IsMinInclusive
                        && !target.IsMinInclusive)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        // BCL missing annotations on IEqualityComparer<T> before .NET 5
                        result &= comparer.Equals(target.MinVersion, possibleSubSet.MinVersion);
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result &= !target.HasLowerBound;
            }

            if (possibleSubSet.HasUpperBound)
            {
                // normal check
                if (!target.Satisfies(possibleSubSet.MaxVersion))
                {
                    // it's possible we didn't need that version, do a special non inclusive check
                    if (!possibleSubSet.IsMaxInclusive
                        && !target.IsMaxInclusive)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        // BCL is missing nullable annotations for IEqualityComparer<T> before net5.0
                        result &= comparer.Equals(target.MaxVersion, possibleSubSet.MaxVersion);
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result &= !target.HasUpperBound;
            }

            return result;
        }

        /// <summary>
        /// Infer if the range should allow prerelease versions based on if the lower or upper bounds 
        /// contain prerelease labels.
        /// </summary>
        protected bool HasPrereleaseBounds
        {
            get
            {
                return IsPrerelease(_minVersion) == true
                    || IsPrerelease(_maxVersion) == true;
            }
        }

        private static bool? IsPrerelease(SemanticVersion? version)
        {
            bool? b = null;

            if (version != null)
            {
                b = version.IsPrerelease;
            }

            return b;
        }
    }
}
