// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace NuGet.Versioning
{
    public partial class VersionRange
    {
        // Static factory methods for creating version range objects.

        // If dictionary exceeds this size, ParsedVersionRangeMapping will be cleared.
        private const int ParsedVersionRangeMappingMaxEntries = 500;

        // Cached mappings from (string value, bool allowFloating) => VersionRange. On cache hit, avoids allocations during TryParse.
        private static Dictionary<(string, bool), VersionRange> ParsedVersionRangeMapping = new Dictionary<(string, bool), VersionRange>(ParsedVersionRangeMappingMaxEntries);

        /// <summary>
        /// A range that accepts all versions, prerelease and stable.
        /// </summary>
        public static readonly VersionRange All = new VersionRange(null, true, null, true);

        private static readonly NuGetVersion V0 = new NuGetVersion(0, 0, 0);

        /// <summary>
        /// A range that accepts all versions, prerelease and stable, and floats to the highest.
        /// </summary>
        [Obsolete("Consider not using this VersionRange. The lack of a proper normalized version means that it is not round trippable in an assets file.")]
        public static readonly VersionRange AllFloating = new VersionRange(V0, true, null, true, new FloatRange(NuGetVersionFloatBehavior.AbsoluteLatest, V0, "*"));

        /// <summary>
        /// A range that accepts all stable versions
        /// </summary>
        public static readonly VersionRange AllStable = new VersionRange(null, true, null, true);

        /// <summary>
        /// A range that accepts all versions, prerelease and stable, and floats to the highest.
        /// </summary>
        [Obsolete("Consider not using this VersionRange. The lack of a proper normalized version means that it is not round trippable in an assets file.")]
        public static readonly VersionRange AllStableFloating = new VersionRange(V0, true, null, true, new FloatRange(NuGetVersionFloatBehavior.Major));

        /// <summary>
        /// A range that rejects all versions
        /// </summary>
        public static readonly VersionRange None = new VersionRange(new NuGetVersion(0, 0, 0), false, new NuGetVersion(0, 0, 0), false);

        /// <summary>
        /// The version string is either a simple version or an arithmetic range
        /// e.g.
        /// 1.0         --> 1.0 ≤ x
        /// (,1.0]      --> x ≤ 1.0
        /// (,1.0)      --> x &lt; 1.0
        /// [1.0]       --> x == 1.0
        /// (1.0,)      --> 1.0 &lt; x
        /// (1.0, 2.0)   --> 1.0 &lt; x &lt; 2.0
        /// [1.0, 2.0]   --> 1.0 ≤ x ≤ 2.0
        /// </summary>
        public static VersionRange Parse(string value)
        {
            return Parse(value, true);
        }

        /// <summary>
        /// Direct parse
        /// </summary>
        public static VersionRange Parse(string value, bool allowFloating)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            VersionRange? versionInfo;
            if (!TryParse(value, allowFloating, out versionInfo))
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture,
                        Resources.Invalidvalue, value));
            }

            return versionInfo;
        }

        /// <summary>
        /// Parses a VersionRange from its string representation.
        /// </summary>
        public static bool TryParse(string value, [NotNullWhen(true)] out VersionRange? versionRange)
        {
            return TryParse(value, true, out versionRange);
        }

        /// <summary>
        /// Parses a VersionRange from its string representation.
        /// </summary>
        public static bool TryParse(string value, bool allowFloating, [NotNullWhen(true)] out VersionRange? versionRange)
        {
            versionRange = null;

            if (value is null)
            {
                return false;
            }

            lock (ParsedVersionRangeMapping)
            {
                if (ParsedVersionRangeMapping.TryGetValue((value, allowFloating), out versionRange))
                {
                    return true;
                }
            }

            var trimmedValue = value.Trim();
            if (string.IsNullOrEmpty(trimmedValue))
            {
                return false;
            }

            var charArray = trimmedValue.ToCharArray();

            // * is the only 1 char range
            if (allowFloating
                && charArray.Length == 1
                && charArray[0] == '*')
            {
                versionRange = new VersionRange(new NuGetVersion(0, 0, 0), true, null, true, FloatRange.Parse(trimmedValue), originalString: value);

                UpdateCachedVersionRange(value, allowFloating, versionRange);

                return true;
            }

            string? minVersionString = null;
            string? maxVersionString = null;
            var isMinInclusive = false;
            var isMaxInclusive = false;
            NuGetVersion? minVersion = null;
            NuGetVersion? maxVersion = null;
            FloatRange? floatRange = null;

            if (charArray[0] == '('
                || charArray[0] == '[')
            {
                // The first character must be [ to (
                switch (charArray[0])
                {
                    case '[':
                        isMinInclusive = true;
                        break;
                    case '(':
                        isMinInclusive = false;
                        break;
                    default:
                        return false;
                }

                // The last character must be ] ot )
                switch (charArray[charArray.Length - 1])
                {
                    case ']':
                        isMaxInclusive = true;
                        break;
                    case ')':
                        isMaxInclusive = false;
                        break;
                    default:
                        return false;
                }

                // Get rid of the two brackets
                trimmedValue = trimmedValue.Substring(1, trimmedValue.Length - 2);

                // Split by comma, and make sure we don't get more than two pieces
                var parts = trimmedValue.Split(',');

                if (parts.Length > 2)
                {
                    return false;
                }
                else
                {
                    var allEmpty = true;

                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(parts[i]))
                        {
                            allEmpty = false;
                            break;
                        }
                    }

                    // If all parts are empty, then neither of upper or lower bounds were specified. Version spec is of the format (,]
                    if (allEmpty)
                    {
                        return false;
                    }
                }

                // (1.0.0] and [1.0.0),(1.0.0) are invalid.
                if (parts.Length == 1
                    && !(isMinInclusive && isMaxInclusive))
                {
                    return false;
                }

                // If there is only one piece, we use it for both min and max
                minVersionString = parts[0];
                maxVersionString = (parts.Length == 2) ? parts[1] : parts[0];
            }
            else
            {
                // default to min inclusive when there are no braces
                isMinInclusive = true;

                // use the entire value as the version
                minVersionString = trimmedValue;
            }

            if (!string.IsNullOrWhiteSpace(minVersionString))
            {
                // parse the min version string
#if NETCOREAPP2_1_OR_GREATER
                if (allowFloating && minVersionString.Contains('*', StringComparison.Ordinal))
#else
                if (allowFloating && minVersionString.Contains("*"))
#endif
                {
                    // single floating version
                    if (FloatRange.TryParse(minVersionString, out floatRange)
                        && floatRange.HasMinVersion)
                    {
                        minVersion = floatRange.MinVersion;
                    }
                    else
                    {
                        // invalid float
                        return false;
                    }
                }
                else
                {
                    // single non-floating version
                    if (!NuGetVersion.TryParse(minVersionString, out minVersion))
                    {
                        // invalid version
                        return false;
                    }
                }
            }

            // parse the max version string, the max cannot float
            if (!string.IsNullOrWhiteSpace(maxVersionString))
            {
                if (!NuGetVersion.TryParse(maxVersionString, out maxVersion))
                {
                    // invalid version
                    return false;
                }
            }

            if (minVersion != null && maxVersion != null)
            {
                int result = minVersion.CompareTo(maxVersion);

                // minVersion > maxVersion
                if (result > 0)
                {
                    return false;
                }

                // minVersion is equal to maxVersion (1.0.0, 1.0.0], [1.0.0, 1.0.0)
                if (result == 0
                    && (isMinInclusive ^ isMaxInclusive))
                {
                    return false;
                }
            }

            // Successful parse!
            versionRange = new VersionRange(
                minVersion: minVersion,
                includeMinVersion: isMinInclusive,
                maxVersion: maxVersion,
                includeMaxVersion: isMaxInclusive,
                floatRange: floatRange,
                originalString: value);

            UpdateCachedVersionRange(value, allowFloating, versionRange);

            return true;
        }

        private static void UpdateCachedVersionRange(string value, bool allowFloating, VersionRange versionRange)
        {
            lock (ParsedVersionRangeMapping)
            {
                if (ParsedVersionRangeMapping.Count >= ParsedVersionRangeMappingMaxEntries)
                {
                    ParsedVersionRangeMapping.Clear();
                }

                ParsedVersionRangeMapping[(value, allowFloating)] = versionRange;
            }
        }

        /// <summary>
        /// Returns the smallest range that includes all given versions.
        /// </summary>
        public static VersionRange Combine(IEnumerable<NuGetVersion> versions)
        {
            return Combine(versions, VersionComparer.Default);
        }

        /// <summary>
        /// Returns the smallest range that includes all given versions.
        /// </summary>
        public static VersionRange Combine(IEnumerable<NuGetVersion> versions, IVersionComparer comparer)
        {
            var result = None;

            if (versions.Any())
            {
                IEnumerable<NuGetVersion> ordered = versions.OrderBy(v => v, comparer);

                result = new VersionRange(ordered.FirstOrDefault(), true, ordered.LastOrDefault(), true);
            }

            return result;
        }

        /// <summary>
        /// Returns the smallest range that includes all given ranges.
        /// </summary>
        public static VersionRange Combine(IEnumerable<VersionRange> ranges)
        {
            return Combine(ranges, VersionComparer.Default);
        }

        /// <summary>
        /// Returns the smallest range that includes all given ranges.
        /// </summary>
        public static VersionRange Combine(IEnumerable<VersionRange> ranges, IVersionComparer comparer)
        {
            if (ranges == null)
            {
                throw new ArgumentNullException(nameof(ranges));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            // Default to None for empty lists
            var result = None;

            // Remove zero width ranges. Ex: (1.0.0, 1.0.0)
            // This includes VersionRange.None and any other ranges that satisfy zero versions
            ranges = ranges.Where(range => HasValidRange(range));

            if (ranges.Any())
            {
                // start with the first range in the list
                var first = ranges.First();

                var lowest = first.MinVersion;
                var highest = first.MaxVersion;

                // To keep things consistent set min/max inclusive to false when there is no boundary
                // It is possible to denote an inclusive range with no bounds, but it has no useful meaning for combine
                var includeLowest = first.IsMinInclusive && first.HasLowerBound;
                var includeHighest = first.IsMaxInclusive && first.HasUpperBound;

                // expand the range to inclue all other ranges
                foreach (var range in ranges.Skip(1))
                {
                    // once we have an unbounded lower we can stop checking
                    if (lowest != null)
                    {
                        if (range.HasLowerBound)
                        {
#pragma warning disable CS8604 // Possible null reference argument.
                            // The BCL is missing nullable annotations in IComparer<T> before net5.0
                            var lowerCompare = comparer.Compare(range.MinVersion, lowest);
#pragma warning restore CS8604 // Possible null reference argument.

                            if (lowerCompare < 0)
                            {
                                // A new lowest was found
                                lowest = range.MinVersion;
                                includeLowest = range.IsMinInclusive;
                            }
                            else if (lowerCompare == 0)
                            {
                                // The lower ends are identical, update the inclusiveness
                                includeLowest |= range.IsMinInclusive;
                            }
                            // lowerCompare > 0 falls into the current range, this is a no-op
                        }
                        else
                        {
                            // No lower bound
                            lowest = null;
                            includeLowest = false;
                        }
                    }

                    // null is the highest we can get, stop checking once it is hit
                    if (highest != null)
                    {
                        if (range.HasUpperBound)
                        {
#pragma warning disable CS8604 // Possible null reference argument.
                            // The BCL is missing nullable annotations in IComparer<T> before net5.0
                            var higherCompare = comparer.Compare(range.MaxVersion, highest);
#pragma warning restore CS8604 // Possible null reference argument.

                            if (higherCompare > 0)
                            {
                                // A new highest was found
                                highest = range.MaxVersion;
                                includeHighest = range.IsMaxInclusive;
                            }
                            else if (higherCompare == 0)
                            {
                                // The higher ends are identical, update the inclusiveness
                                includeHighest |= range.IsMaxInclusive;
                            }
                            // higherCompare < 0 falls into the current range, this is a no-op
                        }
                        else
                        {
                            // No higher bound
                            highest = null;
                            includeHighest = false;
                        }
                    }
                }

                // Create the new range using the maximums found
                result = new VersionRange(lowest, includeLowest, highest, includeHighest);
            }

            return result;
        }

        /// <summary>
        /// Returns the greatest common range that satisfies all given ranges.
        /// </summary>
        public static VersionRange CommonSubSet(IEnumerable<VersionRange> ranges)
        {
            return CommonSubSet(ranges, VersionComparer.Default);
        }

        /// <summary>
        /// Returns the greatest common range that satisfies all given ranges.
        /// </summary>
        public static VersionRange CommonSubSet(IEnumerable<VersionRange> ranges, IVersionComparer comparer)
        {
            if (ranges == null)
            {
                throw new ArgumentNullException(nameof(ranges));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            // return None in case of any invalid range like (1.0.0, 1.0.0)
            // This also includes VersionRange.None and any other ranges that satisfy zero versions
            var versionRanges = ranges as VersionRange[] ?? ranges.ToArray();
            if (!versionRanges.Any() || versionRanges.Any(range => !HasValidRange(range)))
            {
                return None;
            }

            // find out maximum lowest bound and minimum highest bound to form common subset
            var lowest = versionRanges.Where(range => range.HasLowerBound).Max(range => range.MinVersion);
            var highest = versionRanges.Where(range => range.HasUpperBound).Min((range => range.MaxVersion));

            // exclude this lowest if any range has this lowest as excluded, else include
            var excludeLowest = versionRanges.Any(range => range.HasLowerBound &&
#pragma warning disable CS8604 // Possible null reference argument.
                                                    // The BCL is missing nullable annotations in IComparer<T> before net5.0
                                                    comparer.Compare(range.MinVersion, lowest) == 0 &&
#pragma warning restore CS8604 // Possible null reference argument.
                                                    !range.IsMinInclusive);

            // exclude this highest if any range has this highest excluded, else include
            var excludeHighest = versionRanges.Any(range => range.HasUpperBound &&
#pragma warning disable CS8604 // Possible null reference argument.
                                                    // The BCL is missing nullable annotations in IComparer<T> before net5.0
                                                    comparer.Compare(range.MaxVersion, highest) == 0 &&
#pragma warning restore CS8604 // Possible null reference argument.
                                                    !range.IsMaxInclusive);

            // finally check the final lowest n highest versions
            if (lowest != null && highest != null)
            {
                var compare = comparer.Compare(lowest, highest);
                if (compare > 0)
                {
                    return None;
                }

                if (compare == 0)
                {
                    excludeHighest = excludeLowest |= excludeHighest;
                }
            }

            // Create the new range using the minimums found
            var result = new VersionRange(lowest, !excludeLowest, highest, !excludeHighest);

            return HasValidRange(result) ? result : None;
        }

        /// <summary>
        /// Verify the range has an actual width.
        /// Ex: no version can satisfy (3.0.0, 3.0.0)
        /// </summary>
        private static bool HasValidRange(VersionRange range)
        {
            // Verify that if both bounds exist, and neither are included, that the versions are not the same
            return !range.HasUpperBound
                   || !range.HasLowerBound
                   || range.IsMaxInclusive
                   || range.IsMinInclusive
                   || range.MinVersion != range.MaxVersion;
        }
    }
}
