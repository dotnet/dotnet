// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Microsoft.VisualStudio.Threading;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class NuGetProjectUpgraderService : INuGetProjectUpgraderService
    {
        private readonly ServiceActivationOptions _options;
        private readonly IServiceBroker _serviceBroker;
        private readonly AuthorizationServiceClient _authorizationServiceClient;
        private readonly ISharedServiceState _state;

        public NuGetProjectUpgraderService(
            ServiceActivationOptions options,
            IServiceBroker serviceBroker,
            AuthorizationServiceClient authorizationServiceClient,
            ISharedServiceState state)
        {
            Assumes.NotNull(serviceBroker);
            Assumes.NotNull(authorizationServiceClient);
            Assumes.NotNull(state);

            _options = options;
            _serviceBroker = serviceBroker;
            _authorizationServiceClient = authorizationServiceClient;
            _state = state;
        }

        public void Dispose()
        {
            _authorizationServiceClient.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask<bool> IsProjectUpgradeableAsync(string projectId, CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);

            cancellationToken.ThrowIfCancellationRequested();

            NuGetProject? project = await SolutionUtility.GetNuGetProjectAsync(
                _state.SolutionManager,
                projectId,
                cancellationToken);

            Assumes.NotNull(project);

            return await NuGetProjectUpgradeUtility.IsNuGetProjectUpgradeableAsync(project);
        }

        public async ValueTask<IReadOnlyCollection<IProjectContextInfo>> GetUpgradeableProjectsAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectIds);

            cancellationToken.ThrowIfCancellationRequested();

            NuGetProject[] projects = await GetProjectsAsync(projectIds);

            var upgradeableProjects = new List<IProjectContextInfo>();

            IEnumerable<NuGetProject> capableProjects = projects
                .Where(project =>
                    project.ProjectStyle == ProjectModel.ProjectStyle.PackagesConfig &&
                    project.ProjectServices.Capabilities.SupportsPackageReferences);

            // get all packages.config based projects with no installed packages
            foreach (NuGetProject project in capableProjects)
            {
                IEnumerable<PackageReference> installedPackages = await project.GetInstalledPackagesAsync(cancellationToken);

                if (!installedPackages.Any())
                {
                    IProjectContextInfo upgradeableProject = await ProjectContextInfo.CreateAsync(project, cancellationToken);

                    upgradeableProjects.Add(upgradeableProject);
                }
            }

            return upgradeableProjects;
        }

        public async ValueTask<string> BackupProjectAsync(string projectId, CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);

            cancellationToken.ThrowIfCancellationRequested();

            IVsSolutionManager? solutionManager = await _state.SolutionManager.GetValueAsync(cancellationToken);
            Assumes.NotNull(solutionManager);

            string solutionDirectory = await solutionManager.GetSolutionDirectoryAsync();

            await TaskScheduler.Default;

            MSBuildNuGetProject project = await GetMsBuildNuGetProjectAsync(projectId, cancellationToken);

            return CreateBackup(project, solutionDirectory);
        }

        public async ValueTask SaveProjectAsync(string projectId, CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);

            cancellationToken.ThrowIfCancellationRequested();

            MSBuildNuGetProject project = await GetMsBuildNuGetProjectAsync(projectId, cancellationToken);

            await project.SaveAsync(cancellationToken);
        }

        public async ValueTask UninstallPackagesAsync(
            string projectId,
            IReadOnlyList<PackageIdentity> packageIdentities,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);
            Assumes.NotNullOrEmpty(packageIdentities);

            cancellationToken.ThrowIfCancellationRequested();

            MSBuildNuGetProject project = await GetMsBuildNuGetProjectAsync(projectId, cancellationToken);

            IEnumerable<NuGetProjectAction>? actions = packageIdentities
                .Select(packageIdentity => NuGetProjectAction.CreateUninstallProjectAction(packageIdentity, project));

            NuGetPackageManager packageManager = await _state.GetPackageManagerAsync(cancellationToken);
            Assumes.NotNull(packageManager);

            INuGetProjectContext projectContext = await ServiceLocator.GetComponentModelServiceAsync<INuGetProjectContext>();
            Assumes.NotNull(projectContext);

            await packageManager.ExecuteNuGetProjectActionsAsync(
                project,
                actions,
                projectContext,
                NullSourceCacheContext.Instance,
                CancellationToken.None);
        }

        public async ValueTask InstallPackagesAsync(
            string projectId,
            IReadOnlyList<PackageIdentity> packageIdentities,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);
            Assumes.NotNullOrEmpty(packageIdentities);

            cancellationToken.ThrowIfCancellationRequested();

            NuGetProject? project = await SolutionUtility.GetNuGetProjectAsync(
                _state.SolutionManager,
                projectId,
                cancellationToken);

            Assumes.NotNull(project);

            SourceRepository? sourceRepository = await GetSourceRepositoryAsync(cancellationToken);

            IEnumerable<NuGetProjectAction>? actions = packageIdentities
                .Select(packageIdentity => NuGetProjectAction.CreateInstallProjectAction(packageIdentity, sourceRepository, project));

            NuGetPackageManager packageManager = await _state.GetPackageManagerAsync(cancellationToken);
            Assumes.NotNull(packageManager);

            INuGetProjectContext projectContext = await ServiceLocator.GetComponentModelServiceAsync<INuGetProjectContext>();
            Assumes.NotNull(projectContext);

            await packageManager.ExecuteBuildIntegratedProjectActionsAsync(
                project as BuildIntegratedNuGetProject,
                actions,
                projectContext,
                cancellationToken);
        }

        public async ValueTask<IProjectContextInfo> UpgradeProjectToPackageReferenceAsync(
            string projectId,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);

            cancellationToken.ThrowIfCancellationRequested();

            IVsSolutionManager solutionManager = await _state.SolutionManager.GetValueAsync(cancellationToken);
            Assumes.NotNull(solutionManager);

            NuGetProject project = await GetMsBuildNuGetProjectAsync(projectId, cancellationToken);
            NuGetProject newProject = await solutionManager.UpgradeProjectToPackageReferenceAsync(project);

            return await ProjectContextInfo.CreateAsync(newProject, cancellationToken);
        }

        private async ValueTask<MSBuildNuGetProject> GetMsBuildNuGetProjectAsync(
            string projectId,
            CancellationToken cancellationToken)
        {
            NuGetProject? project = await SolutionUtility.GetNuGetProjectAsync(
                _state.SolutionManager,
                projectId,
                cancellationToken);

            Assumes.NotNull(project);

            var msBuildProject = project as MSBuildNuGetProject;

            Assumes.NotNull(msBuildProject);

            return msBuildProject;
        }

        private async Task<NuGetProject[]> GetProjectsAsync(IReadOnlyCollection<string> projectIds)
        {
            Assumes.NotNullOrEmpty(projectIds);

            IVsSolutionManager solutionManager = await _state.SolutionManager.GetValueAsync(CancellationToken.None);
            Assumes.NotNull(solutionManager);

            NuGetProject[] projects = (await solutionManager.GetNuGetProjectsAsync())
                .Where(p => projectIds.Contains(p.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId)))
                .ToArray();

            return projects;
        }

        private async ValueTask<SourceRepository?> GetSourceRepositoryAsync(CancellationToken cancellationToken)
        {
            var activeSources = new List<SourceRepository>();

            IReadOnlyCollection<PackageSourceMoniker> packageSourceMonikers = await PackageSourceMoniker.PopulateListAsync(
                _serviceBroker,
                cancellationToken);

            foreach (PackageSourceMoniker item in packageSourceMonikers)
            {
                var sources = await _state.GetRepositoriesAsync(item.PackageSources, cancellationToken);
                activeSources.AddRange(sources);
            }

            return activeSources.FirstOrDefault();
        }

        private static string CreateBackup(MSBuildNuGetProject msBuildNuGetProject, string solutionDirectory)
        {
            var guid = Guid.NewGuid().ToString().Split('-').First();
            var backupPath = Path.Combine(solutionDirectory, "MigrationBackup", guid, NuGetProject.GetUniqueNameOrName(msBuildNuGetProject));
            Directory.CreateDirectory(backupPath);

            // Backup packages.config
            var packagesConfigFullPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;
            var packagesConfigFileName = Path.GetFileName(packagesConfigFullPath);
            File.Copy(packagesConfigFullPath, Path.Combine(backupPath, packagesConfigFileName), overwrite: true);

            // Backup project file
            var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem;
            var projectFullPath = msBuildNuGetProjectSystem.ProjectFileFullPath;
            var projectFileName = Path.GetFileName(projectFullPath);
            File.Copy(projectFullPath, Path.Combine(backupPath, projectFileName), overwrite: true);

            return backupPath;
        }
    }
}
