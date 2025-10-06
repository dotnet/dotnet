// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;

namespace NuGet.Build.Tasks
{
    public class GetRestoreProjectReferencesTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Full path to the msbuild project.
        /// </summary>
        [Required]
        public string ProjectUniqueName { get; set; }

        [Required]
        public ITaskItem[] ProjectReferences { get; set; }

        /// <summary>
        /// Root project path used for resolving the absolute path.
        /// </summary>
        [Required]
        public string ParentProjectPath { get; set; }

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

            // Filter obvious duplicates without considering OS case sensitivity.
            // This will be filtered further when creating the spec.
            var seen = new HashSet<string>(StringComparer.Ordinal);

            var parentDirectory = Path.GetDirectoryName(ParentProjectPath);

            foreach (var project in ProjectReferences)
            {
                var refOutput = BuildTasksUtility.GetPropertyIfExists(project, "ReferenceOutputAssembly");

                // Match the same behavior as NuGet.targets
                // ReferenceOutputAssembly == '' OR ReferenceOutputAssembly == 'true'
                if (string.IsNullOrEmpty(refOutput)
                    || Boolean.TrueString.Equals(refOutput, StringComparison.OrdinalIgnoreCase))
                {
                    // Get the absolute path
                    var referencePath = Path.GetFullPath(Path.Combine(parentDirectory, project.ItemSpec));

                    if (!seen.Add(referencePath))
                    {
                        // Skip already processed projects
                        continue;
                    }

                    var properties = new Dictionary<string, string>();
                    properties.Add("ProjectUniqueName", ProjectUniqueName);
                    properties.Add("Type", "ProjectReference");
                    properties.Add("ProjectPath", referencePath);
                    properties.Add("ProjectReferenceUniqueName", referencePath);

                    if (!string.IsNullOrEmpty(TargetFrameworks))
                    {
                        properties.Add("TargetFrameworks", TargetFrameworks);
                    }

                    BuildTasksUtility.CopyPropertyIfExists(project, properties, "IncludeAssets");
                    BuildTasksUtility.CopyPropertyIfExists(project, properties, "ExcludeAssets");
                    BuildTasksUtility.CopyPropertyIfExists(project, properties, "PrivateAssets");

                    entries.Add(new TaskItem(Guid.NewGuid().ToString(), properties));
                }
            }

            RestoreGraphItems = entries.ToArray();

            return true;
        }
    }
}
