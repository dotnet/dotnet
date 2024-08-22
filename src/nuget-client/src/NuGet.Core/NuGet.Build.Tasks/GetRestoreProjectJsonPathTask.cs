// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using NuGet.Common;

namespace NuGet.Build.Tasks
{
    public class GetRestoreProjectJsonPathTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Full path to the msbuild project.
        /// </summary>
        [Required]
        public string ProjectPath { get; set; }

        /// <summary>
        /// Output path to project.json if it exists.
        /// </summary>
        [Output]
        public string ProjectJsonPath { get; set; }

        public override bool Execute()
        {
            var directory = Path.GetDirectoryName(ProjectPath);
            var projectName = Path.GetFileNameWithoutExtension(ProjectPath);

            // Allow project.json or projectName.project.json
            var path = ProjectJsonPathUtilities.GetProjectConfigPath(directory, projectName);

            if (File.Exists(path))
            {
                ProjectJsonPath = path;
            }

            return true;
        }
    }
}
