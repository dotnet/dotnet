// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class FilterUnknownPackages : BuildTask
    {
        /// <summary>
        /// Original dependencies
        /// </summary>
        [Required]
        public ITaskItem[] OriginalDependencies { get; set; }

        /// <summary>
        /// Permitted package baseline versions.
        ///   Identity: Package ID
        ///   Version: Package version.
        /// </summary>
        public ITaskItem[] BaseLinePackages { get; set; }

        /// <summary>
        /// Package index files used to define known packages.
        /// </summary>
        public ITaskItem[] PackageIndexes { get; set; }


        [Output]
        public ITaskItem[] FilteredDependencies { get; set; }

        public override bool Execute()
        {
            Func<string, bool> isKnownPackage;

            if (PackageIndexes != null && PackageIndexes.Length > 0)
            {
                var index = PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath")));
                isKnownPackage = packageId => index.Packages.ContainsKey(packageId);
            }
            else
            {
                var baseLinePackageIds = new HashSet<string>(BaseLinePackages.NullAsEmpty().Select(b => b.ItemSpec));
                isKnownPackage = packageId => baseLinePackageIds.Contains(packageId);
            }

            FilteredDependencies = OriginalDependencies.Where(
                dependency =>
                    !dependency.ItemSpec.StartsWith("System.Private") ||  // only apply filtering to System.Private dependencies
                    isKnownPackage(dependency.ItemSpec)
                ).ToArray();

            return !Log.HasLoggedErrors;
        }
    }
}
