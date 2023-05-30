// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Versioning;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Represents a version that will be displayed on the UI.
    /// </summary>
    public class DisplayVersion
    {
        public readonly string AdditionalInfo;

        private readonly string _toString;

        public DisplayVersion(
            NuGetVersion version,
            string additionalInfo,
            bool isValidVersion = true,
            bool isCurrentInstalled = false,
            bool autoReferenced = false,
            bool isDeprecated = false,
            string versionFormat = "N")
            : this(GetRange(version), additionalInfo, isValidVersion, isCurrentInstalled, autoReferenced, isDeprecated, versionFormat)
        {
        }

        public DisplayVersion(
            VersionRange range,
            string additionalInfo,
            bool isValidVersion = true,
            bool isCurrentInstalled = false,
            bool autoReferenced = false,
            bool isDeprecated = false,
            string versionFormat = "N")
            : this(range, version: null, additionalInfo, isValidVersion, isCurrentInstalled, autoReferenced, isDeprecated, isVulnerable: false, versionFormat)
        {
        }

        public DisplayVersion(
            VersionRange range,
            NuGetVersion version,
            string additionalInfo,
            bool isValidVersion = true,
            bool isCurrentInstalled = false,
            bool autoReferenced = false,
            bool isDeprecated = false,
            bool isVulnerable = false,
            string versionFormat = "N")
        {
            if (versionFormat == null)
            {
                // default to normalized version
                versionFormat = "N";
            }

            Range = range;
            AdditionalInfo = additionalInfo;

            IsValidVersion = isValidVersion;

            Version = version ?? range.MinVersion;
            IsCurrentInstalled = isCurrentInstalled;
            AutoReferenced = autoReferenced;
            IsDeprecated = isDeprecated;
            IsVulnerable = isVulnerable;

            // Display a single version if the range is locked
            if (range.OriginalString == null && range.HasLowerAndUpperBounds && range.MinVersion == range.MaxVersion)
            {
                var formattedVersionString = Version.ToString(versionFormat, VersionFormatter.Instance);

                _toString = string.IsNullOrEmpty(AdditionalInfo) ?
                    formattedVersionString :
                    AdditionalInfo + " " + formattedVersionString;
            }
            else
            {
                // Display the range, use the original value for floating ranges
                _toString = string.IsNullOrEmpty(AdditionalInfo) ?
                    Range.OriginalString :
                    AdditionalInfo + " " + Range.OriginalString;
            }

            if (IsDeprecated && IsVulnerable)
            {
                _toString += string.Format(
                    CultureInfo.CurrentCulture,
                    "    ({0}, {1})",
                    Resources.Label_Vulnerable,
                    Resources.Label_Deprecated);
            }
            else if (IsDeprecated)
            {
                _toString += string.Format(
                    CultureInfo.CurrentCulture,
                    "    ({0})",
                    Resources.Label_Deprecated);
            }
            else if (IsVulnerable)
            {
                _toString += string.Format(
                    CultureInfo.CurrentCulture,
                    "    ({0})",
                    Resources.Label_Vulnerable);
            }
        }

        public bool IsCurrentInstalled { get; set; }

        public NuGetVersion Version { get; }

        public VersionRange Range { get; }

        public bool IsValidVersion { get; set; }

        public bool AutoReferenced { get; set; }

        public bool IsDeprecated { get; set; }

        public bool IsVulnerable { get; set; }

        public override string ToString()
        {
            return _toString;
        }

        public override bool Equals(object obj)
        {
            var other = obj as DisplayVersion;
            return other != null
                && other.Version == Version
                && string.Equals(other.AdditionalInfo, AdditionalInfo, StringComparison.Ordinal)
                && IsDeprecated == other.IsDeprecated
                && IsVulnerable == other.IsVulnerable;
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }

        private static VersionRange GetRange(NuGetVersion version)
        {
            return new VersionRange(minVersion: version, includeMinVersion: true, maxVersion: version, includeMaxVersion: true);
        }
    }
}
