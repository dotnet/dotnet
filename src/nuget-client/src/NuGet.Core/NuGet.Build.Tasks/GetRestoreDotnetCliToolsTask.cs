// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using NuGet.Commands;
using NuGet.ProjectModel;
using NuGet.Versioning;

namespace NuGet.Build.Tasks
{
    public class GetRestoreDotnetCliToolsTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Full path to the msbuild project.
        /// </summary>
        [Required]
        public string ProjectPath { get; set; }

        /// <summary>
        /// Output items
        /// </summary>
        [Output]
        public ITaskItem[] RestoreGraphItems { get; set; }

        /// <summary>
        /// Tool runtime framework where this will be executed.
        /// </summary>
        [Required]
        public string ToolFramework { get; set; }

        /// <summary>
        /// NuGet sources
        /// </summary>
        public string[] RestoreSources { get; set; }

        /// <summary>
        /// NuGet fallback folders
        /// </summary>
        public string[] RestoreFallbackFolders { get; set; }

        /// <summary>
        /// User packages folder
        /// </summary>
        public string RestorePackagesPath { get; set; }

        /// <summary>
        /// Config File Paths used
        /// </summary>
        public string[] RestoreConfigFilePaths { get; set; }

        [Required]
        public ITaskItem[] DotnetCliToolReferences { get; set; }

        public override bool Execute()
        {
            var entries = new List<ITaskItem>();

            foreach (var msbuildItem in DotnetCliToolReferences)
            {
                if (string.IsNullOrEmpty(msbuildItem.ItemSpec))
                {
                    throw new InvalidDataException($"Invalid DotnetCliToolReference in {ProjectPath}");
                }


                // Create top level project
                var properties = new Dictionary<string, string>();
                properties.Add("Type", "ProjectSpec");
                properties.Add("ProjectPath", ProjectPath);
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, properties, "Version");
                properties.TryGetValue("Version", out string value);
                var uniqueName = ToolRestoreUtility.GetUniqueName(msbuildItem.ItemSpec, ToolFramework, value != null ? VersionRange.Parse(value) : VersionRange.All);
                properties.Add("ProjectUniqueName", uniqueName);
                properties.Add("ProjectName", uniqueName);

                BuildTasksUtility.AddPropertyIfExists(properties, "Sources", RestoreSources);
                BuildTasksUtility.AddPropertyIfExists(properties, "FallbackFolders", RestoreFallbackFolders);
                BuildTasksUtility.AddPropertyIfExists(properties, "ConfigFilePaths", RestoreConfigFilePaths);
                BuildTasksUtility.AddPropertyIfExists(properties, "PackagesPath", RestorePackagesPath);
                properties.Add("TargetFrameworks", ToolFramework);
                properties.Add("ProjectStyle", ProjectStyle.DotnetCliTool.ToString());

                entries.Add(new TaskItem(Guid.NewGuid().ToString(), properties));

                // Add reference to package
                var packageProperties = new Dictionary<string, string>();
                packageProperties.Add("ProjectUniqueName", uniqueName);
                packageProperties.Add("Type", "Dependency");
                packageProperties.Add("Id", msbuildItem.ItemSpec);
                BuildTasksUtility.CopyPropertyIfExists(msbuildItem, packageProperties, "Version", "VersionRange");
                packageProperties.Add("TargetFrameworks", ToolFramework);

                entries.Add(new TaskItem(Guid.NewGuid().ToString(), packageProperties));

                // Add restore spec to ensure this is executed during restore
                var restoreProperties = new Dictionary<string, string>();
                restoreProperties.Add("ProjectUniqueName", uniqueName);
                restoreProperties.Add("Type", "RestoreSpec");

                entries.Add(new TaskItem(Guid.NewGuid().ToString(), restoreProperties));
            }

            RestoreGraphItems = entries.ToArray();

            return true;
        }
    }
}
