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
    public class GetRestorePackageReferencesTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Full path to the msbuild project.
        /// </summary>
        [Required]
        public string ProjectUniqueName { get; set; }

        [Required]
        public ITaskItem[] PackageReferences { get; set; }

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

            foreach (var msbuildItem in PackageReferences)
            {
                var packageId = msbuildItem.ItemSpec;

                if (string.IsNullOrEmpty(packageId) || !seenIds.Add(packageId))
                {
                    // Skip empty or already processed ids
                    continue;
                }

                var properties = new Dictionary<string, string>();
                properties.Add("ProjectUniqueName", ProjectUniqueName);
                properties.Add("Type", "Dependency");
                properties.Add("Id", packageId);
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "Version", "VersionRange");
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "VersionOverride");

                if (!string.IsNullOrEmpty(TargetFrameworks))
                {
                    properties.Add("TargetFrameworks", TargetFrameworks);
                }

                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "IncludeAssets");
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "ExcludeAssets");
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "PrivateAssets");
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "NoWarn");
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "IsImplicitlyDefined");
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "GeneratePathProperty");
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "Aliases");

                entries.Add(new TaskItem(Guid.NewGuid().ToString(), properties));
            }

            RestoreGraphItems = entries.ToArray();

            return true;
        }
    }
}
