// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    /// <summary>
    /// Examines all dependencies 
    /// </summary>
    public class SplitDependenciesBySupport : BuildTask
    {
        [Required]
        public ITaskItem[] OriginalDependencies { get; set; }

        [Output]
        public ITaskItem[] SplitDependencies { get; set; }
        
        public override bool Execute()
        {
            var dependencies = OriginalDependencies.Select(od => new Dependency(od)).ToArray();

            // preserve all of the TFM-specific dependencies that are not NETStandard.
            List<ITaskItem> splitDependencies = new List<ITaskItem>(dependencies.Where(d => d.TargetFramework != null && d.TargetFramework.Framework != FrameworkConstants.FrameworkIdentifiers.NetStandard).Select(d => d.OriginalItem));

            // for any dependency with unspecified TFM, get its minimum supported netstandard version
            // and treat it as targeting that.
            var unspecDeps = dependencies.Where(d => d.TargetFramework == null).ToArray();
            foreach (var unspecDep in unspecDeps)
            {
                unspecDep.TargetFramework = unspecDep.MinimumNETStandard;
            }

            // get all distinct netstandard TFMs
            var netStandardGroups = dependencies.Select(d => d.TargetFramework)
                                     .Where(fx => fx != null && fx.Framework == FrameworkConstants.FrameworkIdentifiers.NetStandard)
                                     .Distinct()
                                     .OrderBy(fx => fx.Version)
                                     .ToArray();

            // for every netstandard group include all dependencies that support that version of NETStandard or lower
            foreach (var netStandardGroup in netStandardGroups)
            {
                var applicableDependencies = dependencies.Where(d => d.TargetFramework != null &&
                                                              d.TargetFramework.Framework == FrameworkConstants.FrameworkIdentifiers.NetStandard &&
                                                              d.TargetFramework.Version <= netStandardGroup.Version);
                splitDependencies.AddRange(applicableDependencies.Select(d => d.GetItemWithTargetFramework(netStandardGroup)));
            }

            SplitDependencies = splitDependencies.ToArray();

            return !Log.HasLoggedErrors;
        }

        private class Dependency
        {
            public Dependency(ITaskItem originalItem)
            {
                OriginalItem = originalItem;
                Name = originalItem.ItemSpec;
                string fx = originalItem.GetMetadata("TargetFramework");
                if (!String.IsNullOrEmpty(fx))
                {
                    TargetFramework = NuGetFramework.Parse(fx);
                }
                else
                {
                    TargetFramework = null;
                }

                string minNSFx = originalItem.GetMetadata("MinimumNETStandard");
                if (!String.IsNullOrEmpty(minNSFx))
                {
                    MinimumNETStandard = NuGetFramework.Parse(minNSFx);
                }
                else
                {
                    MinimumNETStandard = null;
                }
            }

            public ITaskItem OriginalItem { get; }
            public string Name { get; }
            public NuGetFramework TargetFramework { get; set; }
            public NuGetFramework MinimumNETStandard { get; }

            public ITaskItem GetItemWithTargetFramework(NuGetFramework framework)
            {
                var newItem = new TaskItem(OriginalItem);
                newItem.SetMetadata("TargetFramework", framework.GetShortFolderName());
                return newItem;
            }
        }
    }
}
