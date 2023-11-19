// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;

namespace NuGet.Build.Tasks
{
    public class GetCentralPackageVersionsTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Full path to the msbuild project.
        /// </summary>
        [Required]
        public string ProjectUniqueName { get; set; }

        /// <summary>
        /// Central package versions.
        /// </summary>
        [Required]
        public ITaskItem[] CentralPackageVersions { get; set; }

        /// <summary>
        /// Target frameworks to apply this for. If empty this applies to all.
        /// </summary>
        public string TargetFrameworks { get; set; }

        /// <summary>
        /// Output items
        /// </summary>
        [Output]
        public ITaskItem[] RestoreGraphItems { get; set; }

        public override bool Execute()
        {
            var entries = new List<ITaskItem>();
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var msbuildItem in CentralPackageVersions)
            {
                string packageId = msbuildItem.ItemSpec;

                if (string.IsNullOrEmpty(packageId) || !seenIds.Add(packageId))
                {
                    // Skip empty or already processed ids
                    continue;
                }

                var properties = new Dictionary<string, string>();
                properties.Add("ProjectUniqueName", ProjectUniqueName);
                properties.Add("Type", "CentralPackageVersion");
                properties.Add("Id", packageId);
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "Version", "VersionRange");

                if (!string.IsNullOrEmpty(TargetFrameworks))
                {
                    properties.Add("TargetFrameworks", TargetFrameworks);
                }

                entries.Add(new TaskItem(Guid.NewGuid().ToString(), properties));
            }

            RestoreGraphItems = entries.ToArray();

            return true;
        }
    }
}
