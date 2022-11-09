// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    /// <summary>
    /// Determines appropriate package version for AssemblyVersion and raises dependencies to a baseline version.
    /// Dependencies specified without a version will be raised to the highest permitted version.
    /// Dependencies with a version will be raised to the lowest baseline version that satisfies
    /// the requested version.
    /// </summary>
    public class ApplyBaseLine : BuildTask
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
        /// Package index files used to define baseline, and assembly to package version mapping.
        /// </summary>
        public ITaskItem[] PackageIndexes { get; set; }

        /// <summary>
        /// Set to true to apply the package baseline
        /// </summary>
        public bool Apply { get; set; }
        
        [Output]
        public ITaskItem[] BaseLinedDependencies { get; set; }
        
        public override bool Execute()
        {
            if (PackageIndexes != null && PackageIndexes.Length > 0)
            {
                GetBaseLinedDependenciesFromIndex();
            }
            else
            {
                GetBaseLinedDependenciesFromBaseLinePackages();
            }

            return !Log.HasLoggedErrors;
        }

        public void GetBaseLinedDependenciesFromBaseLinePackages()
        {
            Dictionary<string, SortedSet<Version>> baseLineVersions = new Dictionary<string, SortedSet<Version>>();
            foreach (var baseLinePackage in BaseLinePackages.NullAsEmpty())
            {
                SortedSet<Version> versions = null;
                if (!baseLineVersions.TryGetValue(baseLinePackage.ItemSpec, out versions))
                {
                    baseLineVersions[baseLinePackage.ItemSpec] = versions = new SortedSet<Version>();
                }
                versions.Add(new Version(baseLinePackage.GetMetadata("Version")));
            }

            List<ITaskItem> baseLinedDependencies = new List<ITaskItem>();

            foreach (var dependency in OriginalDependencies)
            {
                if (Apply)
                {
                    SortedSet<Version> dependencyBaseLineVersions = null;
                    Version requestedVersion = null;
                    Version.TryParse(dependency.GetMetadata("Version"), out requestedVersion);

                    if (baseLineVersions.TryGetValue(dependency.ItemSpec, out dependencyBaseLineVersions))
                    {
                        // if no version is requested, choose the highest.  Otherwise choose the first that is 
                        // greater than or equal to the version requested.
                        Version baseLineVersion = requestedVersion == null ?
                            dependencyBaseLineVersions.Last() :
                            dependencyBaseLineVersions.FirstOrDefault(v => v >= requestedVersion);

                        if (baseLineVersion != null)
                        {
                            dependency.SetMetadata("Version", baseLineVersion.ToString(3));
                        }
                    }
                }

                baseLinedDependencies.Add(dependency);
            }

            BaseLinedDependencies = baseLinedDependencies.ToArray();
        }

        public void GetBaseLinedDependenciesFromIndex()
        {
            var index = PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath")));

            List<ITaskItem> baseLinedDependencies = new List<ITaskItem>();

            foreach (var dependency in OriginalDependencies)
            {
                Version assemblyVersion = null, packageVersion = null, baseLineVersion = null;
                string packageId = dependency.ItemSpec;
                Version.TryParse(dependency.GetMetadata("Version"), out packageVersion);
                Version.TryParse(dependency.GetMetadata("AssemblyVersion"), out assemblyVersion);

                // if we have an assembly version see if we have a better package version
                if (assemblyVersion != null)
                {
                    packageVersion = index.GetPackageVersionForAssemblyVersion(packageId, assemblyVersion);
                }

                if (Apply &&
                    index.TryGetBaseLineVersion(packageId, out baseLineVersion) &&
                    (packageVersion == null || baseLineVersion > packageVersion))
                {
                    packageVersion = baseLineVersion;
                }

                if (packageVersion != assemblyVersion)
                {
                    dependency.SetMetadata("Version", packageVersion.ToString());
                }

                baseLinedDependencies.Add(dependency);
            }

            BaseLinedDependencies = baseLinedDependencies.ToArray();

        }
    }
}
