// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Commands;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.Build.Tasks.Pack
{
    /// <summary>
    /// Gets a list of project references from the assets file
    /// This list is then later traversed to determine the version
    /// of the project reference during pack.
    /// </summary>
    public class GetProjectReferencesFromAssetsFileTask : Microsoft.Build.Utilities.Task
    {
        public string RestoreOutputAbsolutePath { get; set; }

        public string ProjectAssetsFileAbsolutePath { get; set; }

        /// <summary>
        /// Output items
        /// </summary>
        [Output]
        public ITaskItem[] ProjectReferences { get; set; }

        public override bool Execute()
        {
            var assetsFilePath = string.Empty;
            if (!string.IsNullOrEmpty(ProjectAssetsFileAbsolutePath) && File.Exists(ProjectAssetsFileAbsolutePath))
            {
                assetsFilePath = ProjectAssetsFileAbsolutePath;
            }
            else
            {
                assetsFilePath = Path.Combine(RestoreOutputAbsolutePath, LockFileFormat.AssetsFileName);
            }

            if (!File.Exists(assetsFilePath))
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.AssetsFileNotFound,
                    assetsFilePath));
            }
            // The assets file is necessary for project and package references. Pack should not do any traversal,
            // so we leave that work up to restore (which produces the assets file).
            var lockFileFormat = new LockFileFormat();
            var assetsFile = lockFileFormat.Read(assetsFilePath);

            if (assetsFile.PackageSpec == null)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.AssetsFileDoesNotHaveValidPackageSpec,
                    assetsFilePath));
            }

            var projectDirectory = Path.GetDirectoryName(assetsFile.PackageSpec.RestoreMetadata.ProjectPath);
            // Using the libraries section of the assets file, the library name and version for the project path.
            var projectPathToLibraryIdentities = assetsFile
                .Libraries
                .Where(library => library.MSBuildProject != null)
                .Select(library => new TaskItem(Path.GetFullPath(Path.Combine(
                        projectDirectory,
                        PathUtility.GetPathWithDirectorySeparator(library.MSBuildProject)))));
            if (projectPathToLibraryIdentities != null)
            {
                ProjectReferences = projectPathToLibraryIdentities.ToArray();
            }
            else
            {
                ProjectReferences = Array.Empty<ITaskItem>();
            }
            return true;
        }
    }
}
