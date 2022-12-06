// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Shared;

namespace NuGet.Versioning
{
    /// <summary>
    /// The floating subset of a version range.
    /// </summary>
    public class FloatRange : IEquatable<FloatRange>
    {
        private readonly NuGetVersion _minVersion;
        private readonly NuGetVersionFloatBehavior _floatBehavior;
        private readonly string _releasePrefix;

        /// <summary>
        /// Create a floating range.
        /// </summary>
        /// <param name="floatBehavior">Section to float.</param>
        public FloatRange(NuGetVersionFloatBehavior floatBehavior)
            : this(floatBehavior, null, null)
        {
        }

        /// <summary>
        /// Create a floating range.
        /// </summary>
        /// <param name="floatBehavior">Section to float.</param>
        /// <param name="minVersion">Min version of the range.</param>
        public FloatRange(NuGetVersionFloatBehavior floatBehavior, NuGetVersion minVersion)
            : this(floatBehavior, minVersion, null)
        {
        }

        /// <summary>
        /// FloatRange
        /// </summary>
        /// <param name="floatBehavior">Section to float.</param>
        /// <param name="minVersion">Min version of the range.</param>
        /// <param name="releasePrefix">The original release label. Invalid labels are allowed here.</param>
        public FloatRange(NuGetVersionFloatBehavior floatBehavior, NuGetVersion minVersion, string releasePrefix)
        {
            _floatBehavior = floatBehavior;
            _minVersion = minVersion;
            _releasePrefix = releasePrefix;

            if (_releasePrefix == null
                && minVersion != null
                && minVersion.IsPrerelease)
            {
                // use the actual label if one was not given
                _releasePrefix = minVersion.Release;
            }
        }

        /// <summary>
        /// True if a min range exists.
        /// </summary>
        public bool HasMinVersion => _minVersion != null;
        /// <summary>
        /// The minimum version of the float range. This is null for cases such as *
        /// </summary>
        public NuGetVersion MinVersion => _minVersion;

        /// <summary>
        /// Defined float behavior
        /// </summary>
        public NuGetVersionFloatBehavior FloatBehavior => _floatBehavior;

        /// <summary>
        /// The original release label. Invalid labels are allowed here.
        /// </summary>
        public string OriginalReleasePrefix => _releasePrefix;

        /// <summary>
        /// True if the given version falls into the floating range.
        /// </summary>
        public bool Satisfies(NuGetVersion version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (_floatBehavior == NuGetVersionFloatBehavior.AbsoluteLatest)
            {
                return true;
            }

            if (_floatBehavior == NuGetVersionFloatBehavior.Major
                && !version.IsPrerelease)
            {
                return true;
            }

            if (_minVersion != null)
            {
                // everything beyond this point requires a version
                if (_floatBehavior == NuGetVersionFloatBehavior.PrereleaseRevision)
                {
                    // allow the stable version to match
                    return _minVersion.Major == version.Major
                       && _minVersion.Minor == version.Minor
                       && _minVersion.Patch == version.Patch
                       && ((version.IsPrerelease && version.Release.StartsWith(_releasePrefix, StringComparison.OrdinalIgnoreCase))
                           || !version.IsPrerelease);
                }
                else if (_floatBehavior == NuGetVersionFloatBehavior.PrereleasePatch)
                {
                    // allow the stable version to match
                    return _minVersion.Major == version.Major
                       && _minVersion.Minor == version.Minor
                       && ((version.IsPrerelease && version.Release.StartsWith(_releasePrefix, StringComparison.OrdinalIgnoreCase))
                           || !version.IsPrerelease);
                }
                else if (FloatBehavior == NuGetVersionFloatBehavior.PrereleaseMinor)
                {
                    // allow the stable version to match
                    return _minVersion.Major == version.Major
                       && ((version.IsPrerelease && version.Release.StartsWith(_releasePrefix, StringComparison.OrdinalIgnoreCase))
                           || !version.IsPrerelease);
                }
                else if (FloatBehavior == NuGetVersionFloatBehavior.PrereleaseMajor)
                {
                    // allow the stable version to match
                    return (version.IsPrerelease && version.Release.StartsWith(_releasePrefix, StringComparison.OrdinalIgnoreCase))
                           || !version.IsPrerelease;
                }
                else if (_floatBehavior == NuGetVersionFloatBehavior.Prerelease)
                {
                    // allow the stable version to match
                    return VersionComparer.Version.Equals(_minVersion, version)
                           && ((version.IsPrerelease && version.Release.StartsWith(_releasePrefix, StringComparison.OrdinalIgnoreCase))
                               || !version.IsPrerelease);
                }
                else if (_floatBehavior == NuGetVersionFloatBehavior.Revision)
                {
                    return _minVersion.Major == version.Major
                           && _minVersion.Minor == version.Minor
                           && _minVersion.Patch == version.Patch
                           && !version.IsPrerelease;
                }
                else if (_floatBehavior == NuGetVersionFloatBehavior.Patch)
                {
                    return _minVersion.Major == version.Major
                           && _minVersion.Minor == version.Minor
                           && !version.IsPrerelease;
                }
                else if (_floatBehavior == NuGetVersionFloatBehavior.Minor)
                {
                    return _minVersion.Major == version.Major
                           && !version.IsPrerelease;
                }
            }

            return false;
        }

        /// <summary>
        /// Parse a floating version into a FloatRange
        /// </summary>
        public static FloatRange Parse(string versionString)
        {
            _ = TryParse(versionString, out FloatRange range);

            return range;
        }

        /// <summary>
        /// Parse a floating version into a FloatRange
        /// </summary>
        public static bool TryParse(string versionString, out FloatRange range)
        {
            range = null;

            if (versionString != null && !string.IsNullOrWhiteSpace(versionString))
            {
                var firstStarPosition = versionString.IndexOf('*');
                var lastStarPosition = versionString.LastIndexOf('*');
                string releasePrefix = null;

                if (versionString.Length == 1
                    && firstStarPosition == 0)
                {
                    range = new FloatRange(NuGetVersionFloatBehavior.Major, new NuGetVersion(new Version(0, 0)));
                }
                else if (versionString.Equals("*-*", StringComparison.Ordinal))
                {
                    range = new FloatRange(NuGetVersionFloatBehavior.AbsoluteLatest, new NuGetVersion("0.0.0-0"), releasePrefix: string.Empty);
                }
                else if (firstStarPosition != lastStarPosition && lastStarPosition != -1 && versionString.IndexOf('+') == -1)
                {
                    var behavior = NuGetVersionFloatBehavior.None;
                    // 2 *s are only allowed in prerelease versions.
                    var dashPosition = versionString.IndexOf('-');
                    string actualVersion = null;

                    if (dashPosition != -1 &&
                        lastStarPosition == versionString.Length - 1 && // Last star is at the end of the full string
                        firstStarPosition == (dashPosition - 1) // First star is right before the first dash.
                        )
                    {
                        // Get the stable part.
                        var stablePart = versionString.Substring(0, dashPosition - 1); // Get the part without the *
                        stablePart += "0";
                        var versionParts = CalculateVersionParts(stablePart);
                        switch (versionParts)
                        {
                            case 1:
                                behavior = NuGetVersionFloatBehavior.PrereleaseMajor;
                                break;
                            case 2:
                                behavior = NuGetVersionFloatBehavior.PrereleaseMinor;
                                break;
                            case 3:
                                behavior = NuGetVersionFloatBehavior.PrereleasePatch;
                                break;
                            case 4:
                                behavior = NuGetVersionFloatBehavior.PrereleaseRevision;
                                break;
                            default:
                                break;
                        }

                        var releaseVersion = versionString.Substring(dashPosition + 1);
                        releasePrefix = releaseVersion.Substring(0, releaseVersion.Length - 1);
                        var releasePart = releasePrefix;
                        if (releasePrefix.Length == 0 || releasePrefix.EndsWith(".", StringComparison.Ordinal))
                        {
                            // 1.0.0-* scenario, an empty label is not a valid version.
                            releasePart += "0";
                        }

                        actualVersion = stablePart + "-" + releasePart;
                    }

                    if (NuGetVersion.TryParse(actualVersion, out NuGetVersion version))
                    {
                        range = new FloatRange(behavior, version, releasePrefix);
                    }
                }
                // A single * can only appear as the last char in the string. 
                // * cannot appear in the metadata section after the +
                else if (lastStarPosition == versionString.Length - 1 && versionString.IndexOf('+') == -1)
                {
                    var behavior = NuGetVersionFloatBehavior.None;

                    var actualVersion = versionString.Substring(0, versionString.Length - 1);

                    if (versionString.IndexOf('-') == -1)
                    {
                        // replace the * with a 0
                        actualVersion += "0";

                        var versionParts = CalculateVersionParts(actualVersion);

                        if (versionParts == 2)
                        {
                            behavior = NuGetVersionFloatBehavior.Minor;
                        }
                        else if (versionParts == 3)
                        {
                            behavior = NuGetVersionFloatBehavior.Patch;
                        }
                        else if (versionParts == 4)
                        {
                            behavior = NuGetVersionFloatBehavior.Revision;
                        }
                    }
                    else
                    {
                        behavior = NuGetVersionFloatBehavior.Prerelease;

                        // check for a prefix
                        if (versionString.IndexOf('-') == versionString.LastIndexOf('-'))
                        {
                            releasePrefix = actualVersion.Substring(versionString.LastIndexOf('-') + 1);

                            // For numeric labels 0 is the lowest. For alpha-numeric - is the lowest.
                            if (releasePrefix.Length == 0 || actualVersion.EndsWith(".", StringComparison.Ordinal))
                            {
                                // 1.0.0-* scenario, an empty label is not a valid version.
                                actualVersion += "0";
                            }
                            else if (actualVersion.EndsWith("-", StringComparison.Ordinal))
                            {
                                // Append a dash to allow floating on the next character.
                                actualVersion += "-";
                            }
                        }
                    }

                    NuGetVersion version = null;
                    if (NuGetVersion.TryParse(actualVersion, out version))
                    {
                        range = new FloatRange(behavior, version, releasePrefix);
                    }
                }
                else
                {
                    // normal version parse
                    NuGetVersion version = null;
                    if (NuGetVersion.TryParse(versionString, out version))
                    {
                        // there is no float range for this version
                        range = new FloatRange(NuGetVersionFloatBehavior.None, version);
                    }
                }
            }

            return range != null;
        }

        private static int CalculateVersionParts(string line)
        {
            var count = 1;
            if (line != null)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    if (line[i] == '.')
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Create a floating version string in the format: 1.0.0-alpha-*
        /// </summary>
        public override string ToString()
        {
            var result = string.Empty;
            switch (_floatBehavior)
            {
                case NuGetVersionFloatBehavior.None:
                    result = MinVersion.ToNormalizedString();
                    break;
                case NuGetVersionFloatBehavior.Prerelease:
                    result = string.Format(VersionFormatter.Instance, "{0:V}-{1}*", MinVersion, _releasePrefix);
                    break;
                case NuGetVersionFloatBehavior.Revision:
                    result = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.*", MinVersion.Major, MinVersion.Minor, MinVersion.Patch);
                    break;
                case NuGetVersionFloatBehavior.Patch:
                    result = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.*", MinVersion.Major, MinVersion.Minor);
                    break;
                case NuGetVersionFloatBehavior.Minor:
                    result = string.Format(CultureInfo.InvariantCulture, "{0}.*", MinVersion.Major);
                    break;
                case NuGetVersionFloatBehavior.Major:
                    result = "*";
                    break;
                case NuGetVersionFloatBehavior.PrereleaseRevision:
                    result = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.*-{3}*", MinVersion.Major, MinVersion.Minor, MinVersion.Patch, _releasePrefix);
                    break;
                case NuGetVersionFloatBehavior.PrereleasePatch:
                    result = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.*-{2}*", MinVersion.Major, MinVersion.Minor, _releasePrefix);
                    break;
                case NuGetVersionFloatBehavior.PrereleaseMinor:
                    result = string.Format(CultureInfo.InvariantCulture, "{0}.*-{1}*", MinVersion.Major, _releasePrefix);
                    break;
                case NuGetVersionFloatBehavior.PrereleaseMajor:
                    result = string.Format(CultureInfo.InvariantCulture, "*-{1}*", MinVersion.Major, _releasePrefix);
                    break;
                case NuGetVersionFloatBehavior.AbsoluteLatest:
                    result = "*-*";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Equals
        /// </summary>
        public bool Equals(FloatRange other)
        {
            return FloatBehavior == other.FloatBehavior
                   && VersionComparer.Default.Equals(MinVersion, other.MinVersion);
        }

        /// <summary>
        /// Override Object.Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as FloatRange);
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStruct(FloatBehavior);
            combiner.AddObject(MinVersion);

            return combiner.CombinedHash;
        }
    }
}
