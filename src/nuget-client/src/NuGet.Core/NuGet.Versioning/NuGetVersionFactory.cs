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

                    if (TryGetNormalizedVersion(versionPart, out systemVersion))
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

                        version = new NuGetVersion(version: systemVersion,
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

        private static bool TryGetNormalizedVersion(string str, [NotNullWhen(true)] out Version? version)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                version = null;
                return false;
            }

            int minor = 0;
            int build = 0;
            int revision = 0;

            // Check for all the possible parts of the version string. If lastParsedPosition is less than the end of
            // the string, the input string was invalid (e.g. "1.2.3.4.5").
            bool success = ParseSection(str, 0, out int lastParsedPosition, out int major) &&
                ParseSection(str, lastParsedPosition, out lastParsedPosition, out minor) &&
                ParseSection(str, lastParsedPosition, out lastParsedPosition, out build) &&
                ParseSection(str, lastParsedPosition, out lastParsedPosition, out revision) &&
                lastParsedPosition == str.Length;

            if (success)
            {
                version = new Version(major, minor, build, revision);
                return true;
            }
            else
            {
                version = null;
                return false;
            }

            // Returns false if an invalid section was found while processing the string.
            static bool ParseSection(string str, int start, out int end, out int versionNumber)
            {
                // Section is empty.
                if (start == str.Length)
                {
                    end = start;
                    versionNumber = 0;
                    return true;
                }

                // Trim off leading whitespace
                for (end = start; end < str.Length; ++end)
                {
                    char currentChar = str[end];
                    if (!char.IsWhiteSpace(currentChar))
                    {
                        if (IsDigit(currentChar))
                        {
                            break;
                        }
                        else
                        {
                            // Found a non-whitespace non-digit character. Invalid string.
                            versionNumber = 0;
                            return false;
                        }
                    }
                }

                bool done = false;
                bool digitFound = false;
                long intermediateVersionNumber = 0;
                // Handle number portion.
                for (; end < str.Length; ++end)
                {
                    // Negative numbers are invalid for version strings so we only need to check for digits.
                    char currentChar = str[end];
                    if (IsDigit(currentChar))
                    {
                        // Parse the values digit by digit and multiplies by 10 to make space for the next digit.
                        // When parsing "123456", this method becomes 1 -> 10 + 2 -> 120 + 3 -> 1230 + 4 -> 12340 + 5 -> 123450 + 6 -> 123456
                        // We subtract off ASCII value of '0' from our current character to get the digit's value
                        // e.g. '3' - '0' == 51 - 48 == 3
                        digitFound = true;
                        intermediateVersionNumber = intermediateVersionNumber * 10 + currentChar - '0';

                        // Check for overflow. We can't get outside the bounds of intermediateVersionNumber, a long, before exceeding int.MaxValue
                        // Intentionally avoid usage of 'checked' statement to avoid exception
                        if (intermediateVersionNumber > int.MaxValue)
                        {
                            versionNumber = 0;
                            return false;
                        }
                    }
                    else if (currentChar == '.')
                    {
                        ++end;
                        // version string ended with '.'
                        if (end == str.Length)
                        {
                            versionNumber = 0;
                            return false;
                        }

                        done = true;
                        break;
                    }
                    else if (char.IsWhiteSpace(currentChar))
                    {
                        break;
                    }
                    else
                    {
                        versionNumber = 0;
                        return false;
                    }
                }

                // We failed to find a number in the section, so the string is invalid.
                if (!digitFound)
                {
                    versionNumber = 0;
                    return false;
                }

                if (end == str.Length)
                {
                    done = true;
                }

                if (!done)
                {
                    // trailing whitespace
                    for (; end < str.Length; ++end)
                    {
                        char currentChar = str[end];
                        if (!char.IsWhiteSpace(currentChar))
                        {
                            if (currentChar == '.')
                            {
                                ++end;
                                // version string ended with '.'
                                if (end == str.Length)
                                {
                                    versionNumber = 0;
                                    return false;
                                }

                                break;
                            }
                            else
                            {
                                versionNumber = 0;
                                return false;
                            }
                        }
                    }
                }

                // Previous checks guarantee returnValue <= int.MaxValue
                versionNumber = (int)intermediateVersionNumber;
                return true;
            }

            static bool IsDigit(char c)
            {
                return c >= '0' && c <= '9';
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
