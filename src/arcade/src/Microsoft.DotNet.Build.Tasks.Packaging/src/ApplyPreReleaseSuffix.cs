// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    /// <summary>
    /// This task will determine if a set of packages need to be stable based on another set.
    /// If not stable, it will append a pre-release suffix.  It will also standardize on 3-part versions.
    /// </summary>
    public class ApplyPreReleaseSuffix : BuildTask
    {
        private Dictionary<string, Version> _stablePackageVersions;
        private PackageIndex _index;
        /// <summary>
        /// Original dependencies without pre-release specifier.
        /// </summary>
        [Required]
        public ITaskItem[] OriginalPackages { get; set; }

        /// <summary>
        /// Pre-release suffix for this build.
        /// </summary>
        [Required]
        public string PreReleaseSuffix { get; set; }

        /// <summary>
        /// Package index files used to define stable package list.
        /// </summary>
        public ITaskItem[] PackageIndexes { get; set; }

        /// <summary>
        /// List of previously shipped packages.
        ///   Identity: Package ID
        ///   Version: Package version.
        /// </summary>
        public ITaskItem[] StablePackages { get; set; }

        /// <summary>
        /// Updated dependencies whit pre-release specifier where package version is not yet stable.
        /// </summary>
        [Output]
        public ITaskItem[] UpdatedPackages { get; set; }

        public override bool Execute()
        {
            if (null == OriginalPackages || OriginalPackages.Length == 0)
            {
                Log.LogError($"{nameof(OriginalPackages)} argument must be specified");
                return false;
            }

            if (String.IsNullOrEmpty(PreReleaseSuffix))
            {
                Log.LogError($"{nameof(PreReleaseSuffix)} argument must be specified");
                return false;
            }

            if (PackageIndexes != null && PackageIndexes.Length > 0)
            {
                _index = PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath")));
            }
            else
            {
                LoadStablePackages();
            }

            List<ITaskItem> updatedPackages = new List<ITaskItem>();

            foreach (var originalPackage in OriginalPackages)
            {
                string packageId = originalPackage.ItemSpec;

                if (packageId == "_._")
                {
                    updatedPackages.Add(originalPackage);
                    continue;
                }

                string packageVersionString = originalPackage.GetMetadata("Version");

                if (packageVersionString.Contains('-'))
                {
                    updatedPackages.Add(originalPackage);
                    continue;
                }

                TaskItem updatedPackage = new TaskItem(originalPackage);
                Version packageVersion = ParseAs3PartVersion(packageVersionString);

                if (!IsStable(packageId, packageVersion))
                {
                    // pre-release, set with suffix
                    updatedPackage.SetMetadata("Version", packageVersion.ToString() + GetSuffix(packageId));
                }
                else
                {
                    // stable, just set the 3 part version without suffix
                    updatedPackage.SetMetadata("Version", packageVersion.ToString());
                }

                updatedPackages.Add(updatedPackage);
            }

            UpdatedPackages = updatedPackages.ToArray();

            return !Log.HasLoggedErrors;
        }

        private void LoadStablePackages()
        {
            // build up a map of stable versions
            _stablePackageVersions = new Dictionary<string, Version>();

            foreach (var stablePackage in StablePackages.NullAsEmpty())
            {
                string stablePackageId = stablePackage.ItemSpec;
                Version newVersion = ParseAs3PartVersion(stablePackage.GetMetadata("Version"));
                Version existingVersion = null;

                // if we don't have a version or the new version is greater assign it
                if (!_stablePackageVersions.TryGetValue(stablePackageId, out existingVersion) ||
                    (newVersion > existingVersion))
                {
                    _stablePackageVersions[stablePackageId] = newVersion;
                }
            }
        }

        private bool IsStable(string packageId, Version packageVersion)
        {
            bool isStable;

            if (_stablePackageVersions != null)
            {
                Version stableVersion;
                isStable = _stablePackageVersions.TryGetValue(packageId, out stableVersion) && stableVersion >= packageVersion;
            }
            else
            {
                isStable = _index.IsStable(packageId, packageVersion);
            }

            return isStable;
        }

        private string GetSuffix(string packageId)
        {
            var suffix = _index?.GetPreRelease(packageId) ?? PreReleaseSuffix;

            if (!string.IsNullOrEmpty(suffix) && suffix[0] != '-')
            {
                suffix = "-" + suffix;
            }

            return suffix;
        }

        private static Version ParseAs3PartVersion(string versionString)
        {
            Version result = new Version(versionString);
            if (result.Revision != -1)
            {
                result = new Version(result.Major, result.Minor, result.Build);
            }
            return result;
        }
    }
}
