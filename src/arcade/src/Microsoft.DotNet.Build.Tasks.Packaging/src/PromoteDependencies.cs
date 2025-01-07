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
    /// Promotes dependencies from reference (ref) assembly TargetFramework to the implementation (lib) assembly 
    /// TargetFramework and vice versa.  
    /// NuGet only ever chooses a single dependencyGroup from a package.  Often the TFM of the implementation and 
    /// reference differ so in order to ensure the correct dependencies are applied we have to promote dependencies
    /// from a less specific ref to the more specific lib, and from a less specific lib to a more specific ref.
    /// </summary>
    public class PromoteDependencies : BuildTask
    {
        private const string TargetFrameworkMetadataName = "TargetFramework";

        private PackageIndex index;

        [Required]
        public ITaskItem[] Dependencies { get; set; }

        [Required]
        public ITaskItem[] PackageIndexes { get; set; }

        [Output]
        public ITaskItem[] PromotedDependencies { get; set; }
        
        public override bool Execute()
        {
            index = PackageIndexes != null && PackageIndexes.Length > 0 ?
                PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath"))) :
                null;

            List<ITaskItem> promotedDependencies = new List<ITaskItem>();

            var dependencies = Dependencies.Select(d => new Dependency(d)).ToArray();

            var refSets = dependencies.Where(d => d.Id != "_._").Where(d => d.IsReference).GroupBy(d => NuGetFramework.Parse(d.TargetFramework)).ToDictionary(g => g.Key, g => g.ToArray());
            var refFxs = refSets.Keys.ToArray();

            Log.LogMessage(LogImportance.Low, $"Ref frameworks {string.Join(";", refFxs.Select(f => f.ToString()))}");

            var libSets = dependencies.Where(d => !d.IsReference).GroupBy(d => NuGetFramework.Parse(d.TargetFramework)).ToDictionary(g => g.Key, g => g.ToArray());
            var libFxs = libSets.Keys.ToArray();

            Log.LogMessage(LogImportance.Low, $"Lib frameworks {string.Join(";", libFxs.Select(f => f.ToString()))}");

            if (libFxs.Length > 0)
            {
                foreach (var refFx in refFxs)
                {
                    // find best lib (if any)
                    var nearestLibFx = FrameworkUtilities.GetNearest(refFx, libFxs);

                    if (nearestLibFx != null && !nearestLibFx.Equals(refFx))
                    {
                        promotedDependencies.AddRange(CopyDependencies(libSets[nearestLibFx], refFx));
                    }
                }
            }

            if (refFxs.Length > 0)
            {
                foreach (var libFx in libFxs)
                {
                    // find best lib (if any)
                    var nearestRefFx = FrameworkUtilities.GetNearest(libFx, refFxs);

                    if (nearestRefFx != null && !nearestRefFx.Equals(libFx))
                    {
                        promotedDependencies.AddRange(CopyDependencies(refSets[nearestRefFx], libFx));
                    }
                }
            }

            PromotedDependencies = promotedDependencies.ToArray();

            return !Log.HasLoggedErrors;
        }

        private IEnumerable<ITaskItem> CopyDependencies(IEnumerable<Dependency> dependencies, NuGetFramework targetFramework)
        {
            foreach (var dependency in dependencies)
            {
                if (index == null || !index.IsInbox(dependency.Id, targetFramework, dependency.Version))
                {
                    var copiedDepenedency = new TaskItem(dependency.OriginalItem);
                    copiedDepenedency.SetMetadata(TargetFrameworkMetadataName, targetFramework.GetShortFolderName());
                    yield return copiedDepenedency;
                }
            }
        }

        private class Dependency
        {
            public Dependency(ITaskItem item)
            {
                Id = item.ItemSpec;
                Version = item.GetMetadata("Version");
                IsReference = item.GetMetadata("TargetPath").StartsWith("ref/", System.StringComparison.OrdinalIgnoreCase);
                TargetFramework = item.GetMetadata(TargetFrameworkMetadataName);
                OriginalItem = item;
            }

            public string Id { get; }
            public string Version { get; }

            public bool IsReference { get; }
            public string TargetFramework { get; }

            public ITaskItem OriginalItem { get; }
        }
    }
}
