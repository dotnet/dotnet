// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Linq;

namespace Microsoft.Build.Locator
{
    /// <summary>
    /// Converts string in its semantic version.
    /// The basic parser logic is taken from https://github.com/NuGetArchive/NuGet.Versioning/releases/tag/rc-preview1.
    /// </summary>
    internal static class SemanticVersionParser
    {
        /// <summary>
        /// Parse a version string
        /// </summary>
        /// <returns>false if the version wasn't parsed</returns>
        public static bool TryParse(string value, out SemanticVersion version)
        {
            version = null;

            if (value != null)
            {
                var (versionString, releaseLabels) = ParseSections(value);

                if (Version.TryParse(versionString, out Version systemVersion))
                {
                    // validate the version string
                    string[] parts = versionString.Split('.');

                    // versions must be 3 parts
                    if (parts.Length != 3)
                    {
                        return false;
                    }

                    foreach (var part in parts)
                    {
                        if (!IsValidPart(part, false))
                        {
                            // leading zeros are not allowed
                            return false;
                        }
                    }

                    if (releaseLabels != null && !releaseLabels.All(s => IsValidPart(s, false)))
                    {
                        return false;
                    }

                    version = new SemanticVersion(
                        NormalizeVersionValue(systemVersion),
                        releaseLabels,
                        value);

                    return true;
                }
            }

            return false;
        }

        private static bool IsLetterOrDigitOrDash(char c)
        {
            int x = (int)c;

            // "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-"
            return (x >= 48 && x <= 57) || (x >= 65 && x <= 90) || (x >= 97 && x <= 122) || x == 45;
        }

        private static bool IsValidPart(string s, bool allowLeadingZeros) => IsValidPart(s.ToCharArray(), allowLeadingZeros);

        private static bool IsValidPart(char[] chars, bool allowLeadingZeros)
        {
            bool result = true;

            if (chars.Length == 0)
            {
                // empty labels are not allowed
                result = false;
            }

            // 0 is fine, but 00 is not. 
            // 0A counts as an alpha numeric string where zeros are not counted
            if (!allowLeadingZeros && chars.Length > 1 && chars[0] == '0' && chars.All(c => Char.IsDigit(c)))
            {
                // no leading zeros in labels allowed
                result = false;
            }
            else
            {
                result &= chars.All(c => IsLetterOrDigitOrDash(c));
            }

            return result;
        }

        /// <summary>
        /// Parse the version string into version/release
        /// </summary>
        private static (string Version, string[] ReleaseLabels) ParseSections(string value)
        {
            string versionString = null;
            string[] releaseLabels = null;

            int dashPos = -1;
            int plusPos = -1;

            char[] chars = value.ToCharArray();

            bool end;
            for (int i = 0; i < chars.Length; i++)
            {
                end = (i == chars.Length - 1);

                if (dashPos < 0)
                {
                    if (end || chars[i] == '-' || chars[i] == '+')
                    {
                        int endPos = i + (end ? 1 : 0);
                        versionString = value.Substring(0, endPos);

                        dashPos = i;

                        if (chars[i] == '+')
                        {
                            plusPos = i;
                        }
                    }
                }
                else if (plusPos < 0)
                {
                    if (end || chars[i] == '+')
                    {
                        int start = dashPos + 1;
                        int endPos = i + (end ? 1 : 0);
                        string releaseLabel = value.Substring(start, endPos - start);

                        releaseLabels = releaseLabel.Split('.');

                        plusPos = i;
                    }
                }
            }

            return (versionString, releaseLabels);
        }

        private static Version NormalizeVersionValue(Version version)
        {
            Version normalized = version;

            if (version.Build < 0 || version.Revision < 0)
            {
                normalized = new Version(
                               version.Major,
                               version.Minor,
                               Math.Max(version.Build, 0),
                               Math.Max(version.Revision, 0));
            }

            return normalized;
        }
    }
}
#endif