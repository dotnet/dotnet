// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NuGet.Shared;

namespace NuGet.Versioning
{
    /// <summary>
    /// Represents a range of versions and a preferred order.
    /// </summary>
    /// <remarks>
    /// Immutable, although subclasses may introduce mutable state.
    /// </remarks>
    public partial class VersionRange : VersionRangeBase, IFormattable
    {
        private readonly FloatRange? _floatRange;
        private readonly string? _originalString;

        /// <summary>
        /// Creates a range that is greater than or equal to the minVersion.
        /// </summary>
        /// <param name="minVersion">Lower bound of the version range.</param>
        public VersionRange(NuGetVersion minVersion)
            : this(minVersion, null)
        {
        }

        /// <summary>
        /// Creates a range that is greater than or equal to the minVersion with the given float behavior.
        /// </summary>
        /// <param name="minVersion">Lower bound of the version range.</param>
        /// <param name="floatRange">Floating behavior.</param>
        public VersionRange(NuGetVersion minVersion, FloatRange? floatRange)
            : this(
                  minVersion: minVersion,
                  includeMinVersion: true,
                  maxVersion: null,
                  includeMaxVersion: false,
                  originalString: null,
                  floatRange: floatRange)
        {
        }

        /// <summary>
        /// Clones a version range and applies a new float range.
        /// </summary>
        public VersionRange(VersionRange range, FloatRange floatRange)
            : this(range.MinVersion, range.IsMinInclusive, range.MaxVersion, range.IsMaxInclusive, floatRange)
        {
        }

        /// <summary>
        /// Creates a VersionRange with the given min and max.
        /// </summary>
        /// <param name="minVersion">Lower bound of the version range.</param>
        /// <param name="includeMinVersion">True if minVersion satisfies the condition.</param>
        /// <param name="maxVersion">Upper bound of the version range.</param>
        /// <param name="includeMaxVersion">True if maxVersion satisfies the condition.</param>
        /// <param name="floatRange">The floating range subset used to find the best version match.</param>
        /// <param name="originalString">The original string being parsed to this object.</param>
        public VersionRange(NuGetVersion? minVersion = null, bool includeMinVersion = true, NuGetVersion? maxVersion = null,
            bool includeMaxVersion = false, FloatRange? floatRange = null, string? originalString = null)
            : base(minVersion, includeMinVersion, maxVersion, includeMaxVersion)
        {
            if (floatRange != null && minVersion == null)
            {
                throw ResourcesFormatter.CannotBeNullWhenParameterIsNull(nameof(minVersion), nameof(floatRange));
            }

            _floatRange = floatRange;
            _originalString = originalString;
        }

        /// <summary>
        /// True if the range has a floating version above the min version.
        /// </summary>
        [MemberNotNullWhen(true, nameof(MinVersion))]
        [MemberNotNullWhen(true, nameof(Float))]
        [MemberNotNullWhen(true, nameof(_floatRange))]
        public bool IsFloating
        {
            get { return Float != null && Float.FloatBehavior != NuGetVersionFloatBehavior.None; }
        }

        /// <inheritdoc cref="VersionRangeBase.MinVersion"/>
        public new NuGetVersion? MinVersion => base.MinVersion;

        /// <inheritdoc cref="VersionRangeBase.MaxVersion"/>
        public new NuGetVersion? MaxVersion => base.MaxVersion;

        /// <inheritdoc cref="VersionRangeBase.HasLowerBound"/>
        [MemberNotNullWhen(true, nameof(MinVersion))]
        public new bool HasLowerBound => base.HasLowerBound;

        /// <inheritdoc cref="VersionRangeBase.IsMinInclusive"/>
        [MemberNotNullWhen(true, nameof(MinVersion))]
        public new bool IsMinInclusive => base.IsMinInclusive;

        /// <inheritdoc cref="VersionRangeBase.HasUpperBound"/>
        [MemberNotNullWhen(true, nameof(MaxVersion))]
        public new bool HasUpperBound => base.HasUpperBound;

        /// <inheritdoc cref="VersionRangeBase.IsMaxInclusive"/>
        [MemberNotNullWhen(true, nameof(MaxVersion))]
        public new bool IsMaxInclusive => base.IsMaxInclusive;

        /// <inheritdoc cref="VersionRangeBase.HasLowerAndUpperBounds"/>
        [MemberNotNullWhen(true, nameof(MinVersion))]
        [MemberNotNullWhen(true, nameof(MaxVersion))]
        public new bool HasLowerAndUpperBounds => base.HasLowerAndUpperBounds;

        /// <summary>
        /// Optional floating range used to determine the best version match.
        /// </summary>
        public FloatRange? Float
        {
            get { return _floatRange; }
        }

        /// <summary>
        /// Original string being parsed to this object.
        /// </summary>
        public string? OriginalString
        {
            get { return _originalString; }
        }

        /// <summary>
        /// Normalized range string.
        /// </summary>
        public override string ToString()
        {
            return ToNormalizedString();
        }

        /// <summary>
        /// Normalized range string.
        /// </summary>
        public virtual string ToNormalizedString()
        {
            return ToString("N", VersionRangeFormatter.Instance);
        }

        /// <summary>
        /// A legacy version range compatible with NuGet 2.8.3
        /// </summary>
        public virtual string ToLegacyString()
        {
            return ToString("D", VersionRangeFormatter.Instance);
        }

        /// <summary>
        /// A short legacy version range compatible with NuGet 2.8.3.
        /// Ex: 1.0.0
        /// </summary>
        public virtual string ToLegacyShortString()
        {
            return ToString("T", VersionRangeFormatter.Instance);
        }

        /// <summary>
        /// Format the version range with an IFormatProvider
        /// </summary>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            string? formattedString = null;

            if (formatProvider == null
                || !TryFormatter(format, formatProvider, out formattedString))
            {
                formattedString = ToString();
            }

            return formattedString;
        }

        /// <summary>
        /// Format the range
        /// </summary>
        protected bool TryFormatter(string? format, IFormatProvider formatProvider, [NotNullWhen(true)] out string? formattedString)
        {
            var formatted = false;
            formattedString = null;

            if (formatProvider != null)
            {
                var formatter = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    formatted = true;
                    formattedString = formatter.Format(format, this, formatProvider);
                }
            }

            return formatted;
        }

        /// <summary>
        /// Format the version range in Pretty Print format.
        /// </summary>
        public string PrettyPrint()
        {
            return ToString("P", VersionRangeFormatter.Instance);
        }

        /// <summary>
        /// Return the version that best matches the range.
        /// </summary>
        public NuGetVersion? FindBestMatch(IEnumerable<NuGetVersion>? versions)
        {
            NuGetVersion? bestMatch = null;

            if (versions != null)
            {
                foreach (var version in versions)
                {
                    if (IsBetter(bestMatch, version))
                    {
                        bestMatch = version;
                    }
                }
            }

            return bestMatch;
        }

        /// <summary>
        /// Determines if a given version is better suited to the range than a current version.
        /// </summary>
        public bool IsBetter(NuGetVersion? current, NuGetVersion? considering)
        {
            if (ReferenceEquals(current, considering))
            {
                return false;
            }

            // null checks
            if (ReferenceEquals(considering, null))
            {
                return false;
            }

            // If the range contains only stable versions disallow prerelease versions
            if (!HasPrereleaseBounds
                && considering.IsPrerelease
                && _floatRange?.FloatBehavior != NuGetVersionFloatBehavior.Prerelease
                && _floatRange?.FloatBehavior != NuGetVersionFloatBehavior.PrereleaseMajor
                && _floatRange?.FloatBehavior != NuGetVersionFloatBehavior.PrereleaseMinor
                && _floatRange?.FloatBehavior != NuGetVersionFloatBehavior.PrereleasePatch
                && _floatRange?.FloatBehavior != NuGetVersionFloatBehavior.PrereleaseRevision
                && _floatRange?.FloatBehavior != NuGetVersionFloatBehavior.AbsoluteLatest)
            {
                return false;
            }

            if (!Satisfies(considering))
            {
                // keep null over a value outside of the range
                return false;
            }

            if (ReferenceEquals(current, null))
            {
                return true;
            }

            if (IsFloating)
            {
                // check if either version is in the floating range
                var curInRange = _floatRange.Satisfies(current);
                var conInRange = _floatRange.Satisfies(considering);

                if (curInRange && !conInRange)
                {
                    // take the version in the range
                    return false;
                }
                else if (conInRange && !curInRange)
                {
                    // take the version in the range
                    return true;
                }
                else if (curInRange && conInRange)
                {
                    // prefer the highest one if both are in the range
                    return current < considering;
                }
                else
                {
                    // neither are in range
                    var curToLower = current < _floatRange.MinVersion;
                    var conToLower = considering < _floatRange.MinVersion;

                    if (curToLower && !conToLower)
                    {
                        // favor the version above the range
                        return true;
                    }
                    else if (!curToLower && conToLower)
                    {
                        // favor the version above the range
                        return false;
                    }
                    else if (!curToLower
                             && !conToLower)
                    {
                        // favor the lower version if we are above the range
                        return current > considering;
                    }
                    else if (curToLower && conToLower)
                    {
                        // favor the higher version if we are below the range
                        return current < considering;
                    }
                }
            }

            // Favor lower versions
            return current > considering;
        }

        /// <summary>
        /// Removes the floating snapshot part of the minimum version if it exists.
        /// Ex: 1.0.0-* -> 1.0.0
        /// </summary>
        public VersionRange ToNonSnapshotRange()
        {
            // For non-floating versions there is no change
            var result = this;

            if (IsFloating)
            {
                var minVersion = MinVersion;

                if (Float.FloatBehavior == NuGetVersionFloatBehavior.Prerelease)
                {
                    minVersion = GetNonSnapshotVersion(minVersion);
                }

                // Drop the floating range from the new range regardless of the float type
                result = new VersionRange(
                    minVersion,
                    IsMinInclusive,
                    MaxVersion,
                    IsMaxInclusive);
            }

            return result;
        }

        /// <summary>
        /// Remove the snapshot version section of a version
        /// </summary>
        private static NuGetVersion GetNonSnapshotVersion(NuGetVersion version)
        {
            var nonSnapshotVersion = version;

            var lastLabel = version.ReleaseLabels.LastOrDefault() ?? string.Empty;

            var endsWithZero = lastLabel == "0";
            var endsWithDash = lastLabel.EndsWith("-", StringComparison.Ordinal);

            if (endsWithZero || endsWithDash)
            {
                var fixedReleaseLabel = string.Empty;

                if (endsWithDash)
                {
                    if (lastLabel.EndsWith("--", StringComparison.Ordinal))
                    {
                        // For labels such as rc1-* an additional - is added by nuget
                        fixedReleaseLabel = lastLabel.Substring(0, lastLabel.Length - 2);
                    }
                    else
                    {
                        // Remove the - for 1.0.0-* (1.0.0--)
                        fixedReleaseLabel = lastLabel.Substring(0, lastLabel.Length - 1);
                    }
                }

                // Remove the last label and add in the fixed label if one exists.
                var fixedLabels = version.ReleaseLabels.Take(version.ReleaseLabels.Count() - 1).ToList();

                if (!string.IsNullOrEmpty(fixedReleaseLabel))
                {
                    fixedLabels.Add(fixedReleaseLabel);
                }

                nonSnapshotVersion = new NuGetVersion(
                    version.Major,
                    version.Minor,
                    version.Patch,
                    version.Revision,
                    fixedLabels,
                    version.Metadata);
            }

            return nonSnapshotVersion;
        }

        /// <summary>
        /// ToLegacyShortString that also includes floating ranges
        /// </summary>
        public virtual string ToShortString()
        {
            return ToString("A", VersionRangeFormatter.Instance);
        }

        /// <summary>
        /// Equals implementation for VersionRange.
        /// </summary>
        public bool Equals(VersionRange? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) &&
                IsFloating == other.IsFloating &&
                EqualityUtility.EqualsWithNullCheck(Float, other.Float);
        }

        /// <summary>
        /// Compare the obj as VersionRange.
        /// </summary>
        public override bool Equals(object? obj)
        {
            return Equals(obj as VersionRange);
        }

        /// <summary>
        /// Returns the VersionRangeBase hashcode
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
