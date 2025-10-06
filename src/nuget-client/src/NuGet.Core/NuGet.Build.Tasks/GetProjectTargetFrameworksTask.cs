// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Build.Framework;
using Newtonsoft.Json;
using NuGet.Commands;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Determine the project's targetframework(s) based
    /// on the available properties.
    /// </summary>
    public class GetProjectTargetFrameworksTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Full path to the msbuild project.
        /// </summary>
        [Required]
        public string ProjectPath { get; set; }

        /// <summary>
        /// Optional TargetFrameworkMoniker property value.
        /// </summary>
        public string TargetFrameworkMoniker { get; set; }

        /// <summary>
        /// Optional TargetPlatformIdentifier property value.
        /// </summary>
        public string TargetPlatformIdentifier { get; set; }

        /// <summary>
        /// Optional TargetPlatformMinVersion property value.
        /// </summary>
        public string TargetPlatformMinVersion { get; set; }

        /// <summary>
        /// Optional TargetPlatformVersion property value.
        /// </summary>
        public string TargetPlatformVersion { get; set; }

        /// <summary>
        /// Optional TargetFrameworks property value.
        /// </summary>
        public string TargetFrameworks { get; set; }

        /// <summary>
        /// Optional TargetFrameworks property value.
        /// </summary>
        public string TargetFramework { get; set; }

        /// <summary>
        /// ; delimited list of target frameworks for the project.
        /// </summary>
        [Output]
        public string ProjectTargetFrameworks { get; set; }

        public override bool Execute()
        {
            // If no framework can be found this will return Unsupported.
            var frameworks = MSBuildProjectFrameworkUtility.GetProjectFrameworkStrings(
                projectFilePath: ProjectPath,
                targetFrameworks: TargetFrameworks,
                targetFramework: TargetFramework,
                targetFrameworkMoniker: TargetFrameworkMoniker,
                targetPlatformIdentifier: TargetPlatformIdentifier,
                targetPlatformVersion: TargetPlatformVersion,
                targetPlatformMinVersion: TargetPlatformMinVersion);

            ProjectTargetFrameworks = string.Join(";", frameworks);

            return true;
        }
    }
}
