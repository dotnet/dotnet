// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.LibraryModel;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class ProjectJsonToPackageRefMigrator
    {
        public static async Task MigrateAsync(
            BuildIntegratedNuGetProject project)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dteProjectFullName = project.MSBuildProjectPath;
            var projectJsonFilePath = ProjectJsonPathUtilities.GetProjectConfigPath(Path.GetDirectoryName(project.MSBuildProjectPath),
                Path.GetFileNameWithoutExtension(project.MSBuildProjectPath));

            if (!File.Exists(projectJsonFilePath))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Strings.Error_FileNotExists, projectJsonFilePath));
            }

            var packageSpec = JsonPackageSpecReader.GetPackageSpec(
                Path.GetFileNameWithoutExtension(project.MSBuildProjectPath),
                projectJsonFilePath);

            if (packageSpec == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Strings.Error_InvalidJson, projectJsonFilePath));
            }

            await MigrateDependenciesAsync(project, packageSpec);

            var buildProject = EnvDTEProjectUtility.AsMSBuildEvaluationProject(dteProjectFullName);

            MigrateRuntimes(packageSpec, buildProject);

            RemoveProjectJsonReference(buildProject, projectJsonFilePath);

            await CreateBackupAsync(project,
                projectJsonFilePath);
        }

        private static async Task MigrateDependenciesAsync(BuildIntegratedNuGetProject project, PackageSpec packageSpec)
        {
            if (packageSpec.TargetFrameworks.Count > 1)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Error_MultipleFrameworks,
                        project.MSBuildProjectPath));
            }

            var dependencies = new List<LibraryDependency>();
            foreach (var targetFramework in packageSpec.TargetFrameworks)
            {
                dependencies.AddRange(targetFramework.Dependencies);
            }

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            dependencies.AddRange(packageSpec.Dependencies);
            foreach (var dependency in dependencies)
            {
                await project.ProjectServices.References.AddOrUpdatePackageReferenceAsync(dependency, CancellationToken.None);
            }
        }

        private static void MigrateRuntimes(PackageSpec packageSpec, Microsoft.Build.Evaluation.Project buildProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var runtimes = packageSpec.RuntimeGraph.Runtimes;
            var supports = packageSpec.RuntimeGraph.Supports;
            var runtimeIdentifiers = new List<string>();
            var runtimeSupports = new List<string>();
            if (runtimes != null && runtimes.Count > 0)
            {
                runtimeIdentifiers.AddRange(runtimes.Keys);

            }

            if (supports != null && supports.Count > 0)
            {
                runtimeSupports.AddRange(supports.Keys);
            }

            var union = string.Join(";", runtimeIdentifiers.Union(runtimeSupports));
            buildProject.SetProperty("RuntimeIdentifiers", union);
        }

        private static void RemoveProjectJsonReference(Microsoft.Build.Evaluation.Project buildProject, string projectJsonFilePath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var projectJsonItem = buildProject
                .GetItems("None")
                .FirstOrDefault(t => string.Equals(t.EvaluatedInclude, Path.GetFileName(projectJsonFilePath), StringComparison.OrdinalIgnoreCase));

            if (projectJsonItem != null)
            {
                buildProject.RemoveItem(projectJsonItem);
            }
        }


        private static async Task CreateBackupAsync(
            BuildIntegratedNuGetProject project,
            string projectJsonFilePath)
        {
            try
            {
                await TaskScheduler.Default;
                var backupDirectory = Path.Combine(Path.GetDirectoryName(project.MSBuildProjectPath), "Backup");

                Directory.CreateDirectory(backupDirectory);

                var backupJsonFile = Path.Combine(backupDirectory, Path.GetFileName(projectJsonFilePath));
                FileUtility.Replace(projectJsonFilePath, backupJsonFile);
                var backupProjectFile = Path.Combine(backupDirectory, Path.GetFileName(project.MSBuildProjectPath));
                File.Copy(project.MSBuildProjectPath, backupProjectFile);
            }
            finally
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            }
        }
    }
}
