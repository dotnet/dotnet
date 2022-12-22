// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetPackageDestination : BuildTask
    {
        /// <summary>
        /// All target monikers that this project wants to support
        ///   Identity: Target moniker to package this project as
        ///   TargetRuntime: (optional) if present takes precedence over PackageTargetRuntime
        /// </summary>
        public ITaskItem[] PackageTargetFrameworks { get; set; }

        /// <summary>
        /// Target monikers to suppress from build/package compatibility check
        /// </summary>
        public string[] SuppressPackageTargetFrameworkCompatibility { get; set; }

        /// <summary>
        /// Target runtime for this project
        /// </summary>
        public string PackageTargetRuntime { get; set; }

        /// <summary>
        /// Target moniker used to restore assets when building this project
        /// </summary>
        [Required]
        public string NuGetTargetMoniker { get; set; }

        /// <summary>
        /// True if this project is a reference assembly
        /// </summary>
        public bool IsReferenceAssembly { get; set; }

        /// <summary>
        /// True if this project should also be packaged as a reference for any .NET desktop target monikers
        /// </summary>
        public bool PackageDesktopAsRef { get; set; }

        /// <summary>
        /// True if this project should also be packaged without a runtime for any .NET desktop target monikers
        /// </summary>
        public bool PackageDesktopAsLib { get; set; }

        /// <summary>
        /// Package destination paths.
        ///   Identity : path in the nupkg that this project will be packaged
        ///   TargetFramework : target moniker to use when harvesting dependencies of this project's files
        /// </summary>
        [Output]
        public ITaskItem[] PackageDestinations { get; set; }
        
        /// <summary>
        /// True if this project contained any package destinations under ref.
        /// </summary>
        [Output]
        public bool IsReferenceAsset { get; set; }

        private List<ITaskItem> _packageDestinations = new List<ITaskItem>();

        public override bool Execute()
        {
            IsReferenceAsset = IsReferenceAssembly;
            var nuGetFx = NuGetFramework.Parse(NuGetTargetMoniker);

            var suppressions = new HashSet<NuGetFramework>(
                SuppressPackageTargetFrameworkCompatibility.NullAsEmpty().Select(
                    s => NuGetFramework.Parse(s)),
                    NuGetFramework.Comparer);

            var compat = DefaultCompatibilityProvider.Instance;

            var packageTargetFrameworks = PackageTargetFrameworks.NullAsEmpty().Where(p => !String.IsNullOrEmpty(p.ItemSpec));
            foreach (var packageTargetFramework in packageTargetFrameworks)
            {
                var packageFx = NuGetFramework.Parse(packageTargetFramework.ItemSpec);

                if (!packageFx.Equals(nuGetFx) &&
                    !compat.IsCompatible(packageFx, nuGetFx) &&
                    !suppressions.Contains(packageFx))
                {
                    // we might be using a portable combination that doesn't directly map to a PCL profile
                    // break it apart to make sure every framework is supported by the build framework
                    if (packageFx.Framework == FrameworkConstants.FrameworkIdentifiers.Portable && packageFx.Profile.Contains("+"))
                    {
                        foreach (var portableFramework in packageFx.Profile.Split('+'))
                        {
                            var portableFx = NuGetFramework.Parse(portableFramework);

                            if (!compat.IsCompatible(portableFx, nuGetFx))
                            {
                                Log.LogError($"Project is built as {nuGetFx.GetShortFolderName()} but packaged as {packageFx.GetShortFolderName()} and {portableFramework} is not compatible with {nuGetFx.GetShortFolderName()}.  To suppress this error you can add <SuppressPackageTargetFrameworkCompatibility Include=\"{packageFx.GetShortFolderName()}\" /> to your project file.");
                            }
                        }
                    }
                    else
                    {
                        Log.LogError($"Project is built as {nuGetFx.GetShortFolderName()} but packaged as {packageFx.GetShortFolderName()} which are not compatible; A {nuGetFx.GetShortFolderName()} asset cannot be referenced by a {packageFx.GetShortFolderName()} project.  To suppress this error you can add <SuppressPackageTargetFrameworkCompatibility Include=\"{packageFx.GetShortFolderName()}\" /> to your project file.");
                    }
                }

                string runtime = PackageTargetRuntime;

                // MSBuild returns an empty string for both non-existent meta-data as well as existent, but empty metadata.
                // If someone has defined an empty string here, we want to honor that over PackageTargetRuntime.
                if (packageTargetFramework.MetadataNames.Cast<string>().Any(md => md.Equals("TargetRuntime")))
                {
                    runtime = packageTargetFramework.GetMetadata("TargetRuntime");
                }

                Add(packageFx, runtime);
            }

            if (_packageDestinations.Count == 0)
            {
                // If PackageTargetFrameworks is not set, just use the NuGetTargetMoniker
                Add(nuGetFx, PackageTargetRuntime);
            }

            PackageDestinations = _packageDestinations.ToArray();

            return !Log.HasLoggedErrors;
        }

        private void Add(NuGetFramework framework, string runtime)
        {
            var path = new StringBuilder();
            if (!String.IsNullOrEmpty(runtime) && !IsReferenceAssembly)
            {
                path.Append($"runtimes/{runtime}/");
            }

            string folder = IsReferenceAssembly ? "ref" : "lib";
            string fx = framework.GetShortFolderName();
            path.Append($"{folder}/{fx}");

            _packageDestinations.Add(CreatePackageDestination(path.ToString(), fx));

            // RID-specific desktop libraries should also be packaged without a RID to work in packages.config projects
            if (framework.Framework == FrameworkConstants.FrameworkIdentifiers.Net)
            {
                if (!String.IsNullOrEmpty(runtime) && PackageDesktopAsLib)
                {
                    _packageDestinations.Add(CreatePackageDestination($"lib/{fx}", fx));
                }

                if (PackageDesktopAsRef)
                {
                    _packageDestinations.Add(CreatePackageDestination($"ref/{fx}", fx));
                    IsReferenceAsset = true;
                }
            }
        }

        private ITaskItem CreatePackageDestination(string path, string framework)
        {
            var item = new TaskItem(path);
            if (!String.IsNullOrEmpty(framework))
            {
                item.SetMetadata("TargetFramework", framework);
            }

            return item;
        }
    }
}
