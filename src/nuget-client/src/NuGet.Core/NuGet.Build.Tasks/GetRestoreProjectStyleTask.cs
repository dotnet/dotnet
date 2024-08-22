// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Build.Framework;
using NuGet.ProjectModel;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Gets the project style.
    /// </summary>
    public sealed class GetRestoreProjectStyleTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the project has any PackageReference items.
        /// </summary>
        public bool HasPackageReferenceItems { get; set; }

        [Output]
        public bool IsPackageReferenceCompatibleProjectStyle { get; set; }

        /// <summary>
        /// Gets or sets the full path to the project directory.
        /// </summary>
        [Required]
        public string MSBuildProjectDirectory { get; set; }

        /// <summary>
        /// Gets or sets the name of the project file.
        /// </summary>
        [Required]
        public string MSBuildProjectName { get; set; }

        /// <summary>
        /// The path to a project.json file.
        /// </summary>
        public string ProjectJsonPath { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ProjectModel.ProjectStyle"/> of the project.
        /// </summary>
        [Output]
        public ProjectStyle ProjectStyle { get; set; }

        /// <summary>
        /// Gets or sets the user specified project style of the project.
        /// </summary>
        public string RestoreProjectStyle { get; set; }

        public override bool Execute()
        {
            var log = new MSBuildLogger(Log);

            var result = BuildTasksUtility.GetProjectRestoreStyle(RestoreProjectStyle, HasPackageReferenceItems, ProjectJsonPath, MSBuildProjectDirectory, MSBuildProjectName, log);

            IsPackageReferenceCompatibleProjectStyle = result.IsPackageReferenceCompatibleProjectStyle;
            ProjectStyle = result.ProjectStyle;

            return !Log.HasLoggedErrors;
        }
    }
}
