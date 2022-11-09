// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetPackageFromModule : BuildTask
    {
        /// <summary>
        /// Modules referenced that need to be mapped to packages
        /// </summary>
        [Required]
        public ITaskItem[] ModulesReferenced { get; set; }

        /// <summary>
        /// Permitted package baseline versions.
        ///   Identity: module name
        ///   Package: package which contains the module
        /// </summary>
        public ITaskItem[] ModulePackages { get; set; }

        /// <summary>
        /// Package index files used to define module to package mapping.
        /// </summary>
        public ITaskItem[] PackageIndexes { get; set; }

        /// <summary>
        /// Packages containing the referenced modules
        /// </summary>
        [Output]
        public ITaskItem[] PackagesReferenced { get; set; }

        public override bool Execute()
        {
            IDictionary<string, string> modulesToPackages;

            if (PackageIndexes != null && PackageIndexes.Length > 0)
            {
                var index = PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath")));

                modulesToPackages = index.ModulesToPackages;
            }
            else
            {
                modulesToPackages = new Dictionary<string, string>();

                foreach(var modulePackage in ModulePackages.NullAsEmpty())
                {
                    modulesToPackages.Add(modulePackage.ItemSpec, modulePackage.GetMetadata("Package"));
                }
            }

            List<ITaskItem> packagesReferenced = new List<ITaskItem>();

            foreach(var moduleReferenced in ModulesReferenced)
            {
                string moduleName = moduleReferenced.ItemSpec;
                string packageId;

                if (modulesToPackages.TryGetValue(moduleName, out packageId))
                {
                    var packageReferenced = new TaskItem(packageId);
                    packageReferenced.SetMetadata("NativeLibrary", moduleName);
                    moduleReferenced.CopyMetadataTo(packageReferenced);
                    packagesReferenced.Add(packageReferenced);
                }
            }

            PackagesReferenced = packagesReferenced.ToArray();

            return !Log.HasLoggedErrors;
        }
    }
}
