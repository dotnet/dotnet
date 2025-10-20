// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.UnifiedBuild.Tasks
{
    /// <summary>
    /// Reads a Version.Details.xml file and returns the list of non-pinned dependency package names.
    /// </summary>
    public class GetDependencyNamesFromVersionDetails : Microsoft.Build.Utilities.Task
    {
        private const string PinnedAttributeName = "Pinned";
        private const string DependencyAttributeName = "Dependency";
        private const string NameAttributeName = "Name";

        /// <summary>
        /// Path to the Version.Details.xml file.
        /// </summary>
        [Required]
        public string VersionDetailsPath { get; set; }

        /// <summary>
        /// Output: Array of dependency package names found in the Version.Details.xml file.
        /// Excludes pinned dependencies.
        /// </summary>
        [Output]
        public string[] DependencyNames { get; set; }

        public override bool Execute()
        {
            if (string.IsNullOrEmpty(VersionDetailsPath) || !File.Exists(VersionDetailsPath))
            {
                Log.LogError($"The VersionDetailsPath must point to a valid Version.Details.xml file. " +
                    $"Provided file path '{VersionDetailsPath}' does not exist.");
                return false;
            }

            HashSet<string> dependencies = VersionDetailsHelper.GetDependencies(VersionDetailsPath, Log);

            if (Log.HasLoggedErrors)
            {
                return false;
            }

            DependencyNames = dependencies
                .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return true;
        }
    }
}
