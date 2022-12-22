// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public static class PackageSpecUtility
    {
        /// <summary>
        /// Apply a snapshot value.
        /// </summary>
        public static NuGetVersion SpecifySnapshot(string version, string snapshotValue)
        {
            // Snapshots should be in the form 1.0.0-*, 1.0.0-beta-*, or 1.0.0-rc.*
            // Snapshots may not contain metadata such as 1.0.0+5.* or be stable versions such as 1.0.*
            if (IsSnapshotVersion(version))
            {
                if (string.IsNullOrEmpty(snapshotValue))
                {
                    version = version.Substring(0, version.Length - 2);
                }
                else
                {
                    version = version.Substring(0, version.Length - 1) + snapshotValue;
                }
            }

            return NuGetVersion.Parse(version);
        }

        /// <summary>
        /// True if the string is a snapshot version.
        /// </summary>
        public static bool IsSnapshotVersion(string version)
        {
            if (version != null
                && version.EndsWith("*", StringComparison.Ordinal)
                && version.IndexOf("-", StringComparison.Ordinal) > -1
                && version.IndexOf("+", StringComparison.Ordinal) < 0
                && (version.EndsWith("-*", StringComparison.Ordinal)
                    || (version.EndsWith(".*", StringComparison.Ordinal))))
            {
                // Verify the version is valid
                NuGetVersion parsed = null;
                return NuGetVersion.TryParse(version.Substring(0, version.Length - 2), out parsed);
            }

            return false;
        }
    }
}
