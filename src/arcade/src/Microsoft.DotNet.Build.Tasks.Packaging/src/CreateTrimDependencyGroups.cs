// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class CreateTrimDependencyGroups : BuildTask
    {
        private const string PlaceHolderDependency = "_._";

        [Required]
        public ITaskItem[] Dependencies
        {
            get;
            set;
        }

        [Required]
        public ITaskItem[] Files
        {
            get;
            set;
        }

        /// <summary>
        /// Package index files used to define stable package list.
        /// </summary>
        [Required]
        public ITaskItem[] PackageIndexes
        {
            get;
            set;
        }


        [Output]
        public ITaskItem[] TrimmedDependencies
        {
            get;
            set;
        }

        /* Given a set of available frameworks ("InboxOnTargetFrameworks"), and a list of desired frameworks,
        reduce the set of frameworks to the minimum set of frameworks which is compatible (preferring inbox frameworks. */
        public override bool Execute()
        {
            if (Dependencies == null)
            {
                Log.LogError("Dependencies argument must be specified");
                return false;
            }
            if (PackageIndexes == null || PackageIndexes.Length == 0)
            {
                Log.LogError("PackageIndexes argument must be specified");
                return false;
            }

            var index = PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath")));

            // Retrieve the list of dependency group TFM's
            var dependencyGroups = Dependencies
                .Select(dependencyItem => new TaskItemPackageDependency(dependencyItem))
                .GroupBy(dependency => dependency.TargetFramework)
                .Select(dependencyGrouping => new TaskItemPackageDependencyGroup(dependencyGrouping.Key, dependencyGrouping))
                .ToArray();

            // Prepare a resolver for evaluating if candidate frameworks are actually supported by the package
            PackageItem[] packageItems = Files.Select(f => new PackageItem(f)).ToArray();
            var packagePaths = packageItems.Select(pi => pi.TargetPath);
            NuGetAssetResolver resolver = new NuGetAssetResolver(null, packagePaths);

            // Determine all inbox frameworks which are supported by this package
            var supportedInboxFrameworks = index.GetAlllInboxFrameworks().Where(fx => IsSupported(fx, resolver));

            var newDependencyGroups = new Queue<TaskItemPackageDependencyGroup>();
            // For each inbox framework determine its best compatible dependency group and create an explicit group, trimming out any inbox dependencies
            foreach(var supportedInboxFramework in supportedInboxFrameworks)
            {
                var nearestDependencyGroup = dependencyGroups.GetNearest(supportedInboxFramework);

                // We found a compatible dependency group that is not the same as this framework
                if (nearestDependencyGroup != null  && nearestDependencyGroup.TargetFramework != supportedInboxFramework)
                {
                    // remove all dependencies which are inbox on supportedInboxFramework
                    var filteredDependencies = nearestDependencyGroup.Packages.Where(d => !index.IsInbox(d.Id, supportedInboxFramework, d.AssemblyVersion)).ToArray();
                    
                    newDependencyGroups.Enqueue(new TaskItemPackageDependencyGroup(supportedInboxFramework, filteredDependencies));
                }
            }

            // Remove any redundant groups from the added set (EG: net45 and net46 with the same set of dependencies)
            int groupsToCheck = newDependencyGroups.Count;
            for(int i = 0; i < groupsToCheck; i++)
            {
                // to determine if this is a redundant group, we dequeue so that it won't be considered in the following check for nearest group.
                var group = newDependencyGroups.Dequeue();

                // of the remaining groups, find the most compatible one
                var nearestGroup = newDependencyGroups.Concat(dependencyGroups).GetNearest(group.TargetFramework);

                // either we found no compatible group, 
                // or the closest compatible group has different dependencies, 
                // or the closest compatible group is portable and this is not (Portable profiles have different framework precedence, https://github.com/NuGet/Home/issues/6483),
                // keep it in the set of additions
                if (nearestGroup == null || 
                    !group.Packages.SetEquals(nearestGroup.Packages) || 
                    FrameworkUtilities.IsPortableMoniker(group.TargetFramework) != FrameworkUtilities.IsPortableMoniker(nearestGroup.TargetFramework))
                {
                    // not redundant, keep it in the queue
                    newDependencyGroups.Enqueue(group);
                }
            }

            // Build the items representing added dependency groups.
            List<ITaskItem> trimmedDependencies = new List<ITaskItem>();
            foreach (var newDependencyGroup in newDependencyGroups)
            {
                if (newDependencyGroup.Packages.Count == 0)
                {
                    // no dependencies (all inbox), use a placeholder dependency.
                    var item = new TaskItem(PlaceHolderDependency);
                    item.SetMetadata("TargetFramework", newDependencyGroup.TargetFramework.GetShortFolderName());
                    trimmedDependencies.Add(item);
                }
                else
                {
                    foreach(var dependency in newDependencyGroup.Packages)
                    {
                        var item = new TaskItem(dependency.Item);
                        // emit CopiedFromTargetFramework to aide in debugging.
                        item.SetMetadata("CopiedFromTargetFramework", item.GetMetadata("TargetFramework"));
                        item.SetMetadata("TargetFramework", newDependencyGroup.TargetFramework.GetShortFolderName());
                        trimmedDependencies.Add(item);
                    }
                }
            }
            TrimmedDependencies = trimmedDependencies.ToArray();
            return !Log.HasLoggedErrors;
        }

        private bool IsSupported(NuGetFramework inboxFx, NuGetAssetResolver resolver)
        {
            var compileAssets = resolver.ResolveCompileAssets(inboxFx);

            // We assume that packages will only support inbox frameworks with lib/tfm assets and not runtime specific assets.
            // This effectively means we'll never reduce dependencies if a package happens to support an inbox framework with
            // a RID asset, but that is OK because RID assets can only be used by nuget3 + project.json
            // and we don't care about reducing dependencies for project.json because indirect dependencies are hidden.
            var runtimeAssets = resolver.ResolveRuntimeAssets(inboxFx, null);

            foreach (var compileAsset in compileAssets.Where(c => !NuGetAssetResolver.IsPlaceholder(c)))
            {
                string fileName = Path.GetFileName(compileAsset);

                if (!runtimeAssets.Any(r => Path.GetFileName(r).Equals(fileName, StringComparison.OrdinalIgnoreCase)))
                {
                    // ref with no matching lib
                    return false;
                }
            }

            // Either all compile assets had matching runtime assets, or all were placeholders, make sure we have at
            // least one runtime asset to cover the placeholder case
            return runtimeAssets.Any();
        }

        /// <summary>
        /// Similar to NuGet.Packaging.Core.PackageDependency but also allows for flowing the original ITaskItem.
        /// </summary>
        class TaskItemPackageDependency : PackageDependency
        {
            public TaskItemPackageDependency(ITaskItem item) : base(item.ItemSpec, TryParseVersionRange(item.GetMetadata("Version")))
            {
                Item = item;
                TargetFramework = NuGetFramework.Parse(item.GetMetadata(nameof(TargetFramework)));
                AssemblyVersion = GetAssemblyVersion(item);
            }

            private static VersionRange TryParseVersionRange(string versionString)
            {
                VersionRange value;

                return VersionRange.TryParse(versionString, out value) ? value : null;
            }

            private static Version GetAssemblyVersion(ITaskItem dependency)
            {
                // If we don't have the AssemblyVersion metadata (4 part version string), fall back and use Version (3 part version string)
                string versionString = dependency.GetMetadata("AssemblyVersion");
                if (string.IsNullOrEmpty(versionString))
                {
                    versionString = dependency.GetMetadata("Version");
                }

                return FrameworkUtilities.Ensure4PartVersion(versionString);
            }

            public ITaskItem Item { get; }
            public NuGetFramework TargetFramework { get; }
            public Version AssemblyVersion { get; }
        }

        /// <summary>
        /// An IFrameworkSpecific type that can be used with FrameworkUtilties.GetNearest.
        /// This differs from NuGet.Packaging.PackageDependencyGroup in that it exposes the package dependencies as an ISet which can
        /// undergo an unordered comparison with another ISet.
        /// </summary>
        class TaskItemPackageDependencyGroup : IFrameworkSpecific
        {
            public TaskItemPackageDependencyGroup(NuGetFramework targetFramework, IEnumerable<TaskItemPackageDependency> packages)
            {
                TargetFramework = targetFramework;
                Packages = new HashSet<TaskItemPackageDependency>(packages.Where(d => d.Id != PlaceHolderDependency));
            }

            public NuGetFramework TargetFramework { get; }

            public ISet<TaskItemPackageDependency> Packages { get; }

        }
    }
}
