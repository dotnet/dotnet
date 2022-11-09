// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using NuGet.Frameworks;
using NuGet.ProjectManagement;
using NuGet.Versioning;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    public static class PowerShellCmdletsUtility
    {
        /// <summary>
        /// Parse the NuGetVersion from string
        /// </summary>
        public static NuGetVersion GetNuGetVersionFromString(string version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            NuGetVersion nVersion;
            var success = NuGetVersion.TryParse(version, out nVersion);
            if (!success)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.CurrentCulture,
                        Resources.Cmdlet_FailToParseVersion, version));
            }
            return nVersion;
        }

        /// <summary>
        /// Get project's target frameworks
        /// </summary>
        public static IEnumerable<string> GetProjectTargetFrameworks(NuGetProject project)
        {
            var frameworks = new List<string>();
            var nugetFramework = project.GetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework);
            if (nugetFramework != null)
            {
                var framework = nugetFramework.ToString();
                frameworks.Add(framework);
            }
            return frameworks;
        }
    }
}
