// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Frameworks;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    /// <summary>
    /// Replaces package dependencies with meta-package dependencies where appropriate.
    /// </summary>
    public class ApplyMetaPackages : BuildTask
    {
        /// <summary>
        /// Need the package id to ensure we don't add a meta-package reference for a package that depends on itself.
        /// </summary>
        [Required]
        public string PackageId { get; set; }
        /// <summary>
        /// Original dependencies
        /// </summary>
        [Required]
        public ITaskItem[] OriginalDependencies { get; set; }

        /// <summary>
        /// Package index files used to meta-package mappings
        /// </summary>
        public ITaskItem[] PackageIndexes { get; set; }

        /// <summary>
        /// List of package IDs which should be suppressed from remapping. You can pass in
        /// TargetFramework metadata on the item if we only need to supress the metapackage on
        /// specific TFMs.
        /// </summary>
        public ITaskItem[] SuppressMetaPackages { get; set; }

        /// <summary>
        /// Set to true to apply the meta-package remapping
        /// </summary>
        public bool Apply { get; set; }
        
        [Output]
        public ITaskItem[] UpdatedDependencies { get; set; }
        
        public override bool Execute()
        {
            if (!Apply)
            {
                UpdatedDependencies = OriginalDependencies;
                return true;
            }

            List<ITaskItem> updatedDependencies = new List<ITaskItem>();

            var suppressMetaPackages = new Dictionary<string, HashSet<string>>();

            if (SuppressMetaPackages != null)
            {
                foreach (ITaskItem metapackage in SuppressMetaPackages)
                {
                    if (!suppressMetaPackages.TryGetValue(metapackage.ItemSpec, out var value))
                    {
                        value = new HashSet<string>();
                        suppressMetaPackages.Add(metapackage.ItemSpec, value);
                    }
                    var tfmSpecificSupression = metapackage.GetMetadata("TargetFramework");
                    if (string.IsNullOrEmpty(tfmSpecificSupression))
                    {
                        // If the supression doesn't specify a TargetFramework, then it applies to all.
                        value.Add("All");
                    }
                    else
                    {
                        var fx = NuGetFramework.Parse(tfmSpecificSupression);
                        value.Add(fx.DotNetFrameworkName);
                    }
                }
            }

            PackageIndex index = PackageIndexes != null && PackageIndexes.Length > 0 ?
                PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath"))) :
                null;

            // We cannot add a dependency to a meta-package from a package that itself is part of the meta-package otherwise we create a cycle
            var metaPackageThisPackageIsIn = index?.MetaPackages?.GetMetaPackageId(PackageId);
            if (metaPackageThisPackageIsIn != null)
            {
                suppressMetaPackages.Add(metaPackageThisPackageIsIn, new HashSet<string> { "All" } );
            }

            // keep track of meta-package dependencies to add by framework so that we only add them once per framework.
            Dictionary<string, HashSet<NuGetFramework>> metaPackagesToAdd = new Dictionary<string, HashSet<NuGetFramework>>();

            foreach (var originalDependency in OriginalDependencies)
            {
                var metaPackage = index?.MetaPackages?.GetMetaPackageId(originalDependency.ItemSpec);

                // convert to meta-package dependency
                var tfm = originalDependency.GetMetadata("TargetFramework");
                var fx = NuGetFramework.Parse(tfm);

                if (metaPackage != null && !ShouldSuppressMetapackage(suppressMetaPackages, metaPackage, fx))
                {
                    HashSet<NuGetFramework> metaPackageFrameworks;

                    if (!metaPackagesToAdd.TryGetValue(metaPackage, out metaPackageFrameworks))
                    {
                        metaPackagesToAdd[metaPackage] = metaPackageFrameworks = new HashSet<NuGetFramework>();
                    }

                    metaPackageFrameworks.Add(fx);
                }
                else
                {
                    updatedDependencies.Add(originalDependency);
                }
            }

            updatedDependencies.AddRange(metaPackagesToAdd.SelectMany(pair => pair.Value.Select(tfm => CreateDependency(pair.Key, tfm))));

            UpdatedDependencies = updatedDependencies.ToArray();

            return !Log.HasLoggedErrors;
        }

        private bool ShouldSuppressMetapackage(Dictionary<string, HashSet<string>> suppressedMetaPackages, string metaPackage, NuGetFramework tfm) =>
            suppressedMetaPackages.TryGetValue(metaPackage, out var value) &&
                (value.Contains("All") || value.Contains(tfm.DotNetFrameworkName));

        private ITaskItem CreateDependency(string id, NuGetFramework targetFramework)
        {
            var item = new TaskItem(id);
            item.SetMetadata("TargetFramework", targetFramework.GetShortFolderName());
            return item;
        }
        
    }
}
