// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace NuGet.Versioning
{
    public partial class NuGetVersion
    {
        /// <summary>
        /// Creates a NuGetVersion from a string representing the semantic version.
        /// </summary>
        public new static NuGetVersion Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Argument_Cannot_Be_Null_Or_Empty, value), nameof(value));
            }

            if (!TryParse(value, out NuGetVersion? ver))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Invalidvalue, value), nameof(value));
            }

            return ver;
        }

        /// <summary>
        /// Parses a version string using loose semantic versioning rules that allows 2-4 version components followed
        /// by an optional special version.
        /// </summary>
        public static bool TryParse(string? value, [NotNullWhen(true)] out NuGetVersion? version)
        {
            version = null;

            if (value != null)
            {
                Version? systemVersion;

                // trim the value before passing it in since we not strict here
                ParseSections(value.Trim(), out string? versionString, out string[]? releaseLabels, out string? buildMetadata);

                // null indicates the string did not meet the rules
                if (!string.IsNullOrEmpty(versionString))
                {
                    string versionPart = versionString!;

                    if (IndexOf(versionPart, '.') < 0)
                    {
                        // System.Version requires at least a 2 part version to parse.
                        versionPart += ".0";
                    }

                    if (Version.TryParse(versionPart, out systemVersion))
                    {
                        // labels
                        if (releaseLabels != null)
                        {
                            for (int i = 0; i < releaseLabels.Length; i++)
                            {
                                if (!IsValidPart(releaseLabels[i], allowLeadingZeros: false))
                                {
                                    return false;
                                }
                            }
                        }

                        // build metadata
                        if (buildMetadata != null
                            && !IsValid(buildMetadata, allowLeadingZeros: true))
                        {
                            return false;
                        }

                        var ver = NormalizeVersionValue(systemVersion);

                        var originalVersion = value;

                        if (IndexOf(originalVersion, ' ') > -1)
                        {
                            originalVersion =
#if NETCOREAPP2_0_OR_GREATER
                                value.Replace(" ", string.Empty, StringComparison.Ordinal);
#else
                                value.Replace(" ", string.Empty);
#endif
                        }

                        version = new NuGetVersion(version: ver,
                            releaseLabels: releaseLabels,
                            metadata: buildMetadata ?? string.Empty,
                            originalVersion: originalVersion);

                        return true;
                    }
                }
            }

            return false;

            int IndexOf(string str, char c)
            {
#if NETCOREAPP2_1_OR_GREATER
                return str.IndexOf(c, StringComparison.Ordinal);
#else
                return str.IndexOf(c);
#endif
            }
        }

        /// <summary>
        /// Parses a version string using strict SemVer rules.
        /// </summary>
        public static bool TryParseStrict(string value, [NotNullWhen(true)] out NuGetVersion? version)
        {
            version = null;

            if (TryParse(value, out SemanticVersion? semVer))
            {
                version = new NuGetVersion(semVer.Major, semVer.Minor, semVer.Patch, 0, semVer.ReleaseLabels, semVer.Metadata);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a legacy version string using System.Version
        /// </summary>
        private static string GetLegacyString(Version version, IEnumerable<string>? releaseLabels, string? metadata)
        {
            var sb = new StringBuilder(version.ToString());

            if (releaseLabels != null)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "-{0}", string.Join(".", releaseLabels));
            }

            if (!string.IsNullOrEmpty(metadata))
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "+{0}", metadata);
            }

            return sb.ToString();
        }

        private static IEnumerable<string>? ParseReleaseLabels(string? releaseLabels)
        {
            if (!string.IsNullOrEmpty(releaseLabels))
            {
                return releaseLabels!.Split('.');
            }

            return null;
        }
    }
}
