// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.TargetFramework
{
    public class ChooseBestTargetFrameworksTask : BuildTask
    {
        [Required]
        public ITaskItem[] BuildTargetFrameworks { get; set; }

        [Required]
        public string RuntimeGraph { get; set; }

        [Required]
        public string[] SupportedTargetFrameworks { get; set; }

        // Returns distinct items only. Compares the include values. Metadata is ignored.
        public bool Distinct { get; set; }

        [Output]
        public ITaskItem[] BestTargetFrameworks { get; set; }

        public override bool Execute()
        {
            var bestTargetFrameworkList = new List<ITaskItem>(BuildTargetFrameworks.Length);
            var targetframeworkResolver = new TargetFrameworkResolver(RuntimeGraph);
 
            foreach (ITaskItem buildTargetFramework in BuildTargetFrameworks)
            {
                string bestTargetFramework = targetframeworkResolver.GetBestSupportedTargetFramework(SupportedTargetFrameworks, buildTargetFramework.ItemSpec);
                if (bestTargetFramework != null && (!Distinct || !bestTargetFrameworkList.Any(b => b.ItemSpec == bestTargetFramework)))
                {
                    var item = new TaskItem(bestTargetFramework);
                    buildTargetFramework.CopyMetadataTo(item);
                    bestTargetFrameworkList.Add(item);
                }
            }

            BestTargetFrameworks = bestTargetFrameworkList.ToArray(); 
            return !Log.HasLoggedErrors;
        }
    }
}
