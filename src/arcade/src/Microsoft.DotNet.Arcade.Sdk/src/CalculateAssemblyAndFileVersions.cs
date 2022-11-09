// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Versioning;

namespace Microsoft.DotNet.Arcade.Sdk
{
    /// <summary>
    /// File version has 4 parts and need to increase every official build.This is especially important when building MSIs.
    /// See https://github.com/dotnet/arcade/blob/master/Documentation/CorePackages/Versioning.md#assembly-version.
    /// </summary>
    public class CalculateAssemblyAndFileVersions : Microsoft.Build.Utilities.Task
    {
        private const int MaxMinor = 654;
        private const int MaxBuild = 9999;

        [Required]
        public string VersionPrefix { get; set; }

        [Required]
        public string BuildNumber { get; set; }

        public int PatchNumber { get; set; }

        public bool AutoGenerateAssemblyVersion { get; set; }

        [Output]
        public string AssemblyVersion { get; private set; }

        [Output]
        public string FileVersion { get; private set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            if (!Version.TryParse(VersionPrefix, out var prefix) ||
                prefix.Major == -1 ||
                prefix.Minor == -1 ||
                prefix.Build == -1 ||
                prefix.Revision != -1)
            {
                Log.LogError($"VersionPrefix is not a valid 3-part version: {VersionPrefix}");
                return;
            }

            if (PatchNumber < 0)
            {
                Log.LogError("Invalid value of VersionBaseShortDate");
                return;
            }

            if (AutoGenerateAssemblyVersion)
            {
                int assemblyMajor = prefix.Major;
                int assemblyMinor = prefix.Minor;
                int assemblyPatch = PatchNumber / 50000;
                int assemblyRevision = PatchNumber % 50000;

                FileVersion = AssemblyVersion = $"{assemblyMajor}.{assemblyMinor}.{assemblyPatch}.{assemblyRevision}";
            }
            else
            {
                if (prefix.Minor > MaxMinor)
                {
                    Log.LogError($"The minor version specified in {nameof(VersionPrefix)} must be at most {MaxMinor}: {VersionPrefix}");
                    return;
                }

                if (prefix.Build > MaxBuild)
                {
                    Log.LogError($"The patch version specified in {nameof(VersionPrefix)} must be at most {MaxBuild}: {VersionPrefix}");
                    return;
                }

                int yy, mm, dd, r;
                try
                {
                    yy = int.Parse(BuildNumber.Substring(2, 2));
                    mm = int.Parse(BuildNumber.Substring(4, 2));
                    dd = int.Parse(BuildNumber.Substring(6, 2));
                    r = int.Parse(BuildNumber.Substring(9));
                }
                catch
                {
                    yy = mm = dd = r = -1;
                }

                if (yy < 0 || yy > 99 || mm < 1 || mm > 12 || dd < 1 || dd > 31 || r < 0 || r > 99)
                {
                    Log.LogError($"Invalid format of {nameof(BuildNumber)}: {BuildNumber}");
                    return;
                }

                int fileMajor = prefix.Major;
                int fileMinor = prefix.Minor * 100 + prefix.Build / 100;
                int filePatch = (prefix.Build % 100) * 100 + yy;
                int fileRevision = mm * 5000 + dd * 100 + r;

                FileVersion = $"{fileMajor}.{fileMinor}.{filePatch}.{fileRevision}";
                AssemblyVersion = $"{prefix}.0";
            }
        }
    }
}
