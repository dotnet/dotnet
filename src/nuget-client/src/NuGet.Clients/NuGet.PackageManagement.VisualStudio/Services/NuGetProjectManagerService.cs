// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.PackageManagement.Telemetry;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using StreamJsonRpc;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class NuGetProjectManagerService : INuGetProjectManagerService
    {
        private readonly ServiceActivationOptions _options;
        private readonly IServiceBroker _serviceBroker;
        private readonly AuthorizationServiceClient _authorizationServiceClient;
        private readonly INuGetProjectManagerServiceState _state;
        private readonly ISharedServiceState _sharedState;
        private AsyncSemaphore.Releaser? _semaphoreReleaser;

        public NuGetProjectManagerService(
            ServiceActivationOptions options,
            IServiceBroker serviceBroker,
            AuthorizationServiceClient authorizationServiceClient,
            INuGetProjectManagerServiceState state,
            ISharedServiceState sharedServiceState)
        {
            Assumes.NotNull(serviceBroker);
            Assumes.NotNull(authorizationServiceClient);
            Assumes.NotNull(state);
            Assumes.NotNull(sharedServiceState);

            _options = options;
            _serviceBroker = serviceBroker;
            _authorizationServiceClient = authorizationServiceClient;
            _state = state;
            _sharedState = sharedServiceState;
        }

        public void Dispose()
        {
            _authorizationServiceClient.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask<IReadOnlyCollection<IProjectContextInfo>> GetProjectsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IVsSolutionManager? solutionManager = await _sharedState.SolutionManager.GetValueAsync(cancellationToken);
            Assumes.NotNull(solutionManager);

            NuGetProject[] projects = (await solutionManager.GetNuGetProjectsAsync()).ToArray();
            var projectContexts = new List<IProjectContextInfo>(projects.Length);

            foreach (NuGetProject nugetProject in projects)
            {
                IProjectContextInfo? projectContext = await ProjectContextInfo.CreateAsync(nugetProject, cancellationToken);

                projectContexts.Add(projectContext);
            }

            return projectContexts;
        }

        public async ValueTask<IProjectContextInfo> GetProjectAsync(string projectId, CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);

            cancellationToken.ThrowIfCancellationRequested();

            NuGetProject? project = await SolutionUtility.GetNuGetProjectAsync(
                _sharedState.SolutionManager,
                projectId,
                cancellationToken);

            Assumes.NotNull(project);

            return await ProjectContextInfo.CreateAsync(project, cancellationToken);
        }

        public async ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>> GetInstalledPackagesAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectIds);

            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<NuGetProject> projects = await GetProjectsAsync(projectIds, cancellationToken);

            List<Task<IEnumerable<PackageReference>>> tasks = projects
                .Select(project => project.GetInstalledPackagesAsync(cancellationToken))
                .ToList();
            IEnumerable<PackageReference>[] results = await Task.WhenAll(tasks);

            var installedPackages = new List<PackageReferenceContextInfo>();
            GetInstalledPackagesAsyncTelemetryEvent? telemetryEvent = null;

            for (var i = 0; i < results.Length; ++i)
            {
                IEnumerable<PackageReference> packageReferences = results[i];
                int totalCount = 0;
                int nullCount = 0;

                foreach (PackageReference? packageReference in packageReferences)
                {
                    ++totalCount;

                    if (packageReference is null)
                    {
                        ++nullCount;

                        continue;
                    }

                    PackageReferenceContextInfo installedPackage = PackageReferenceContextInfo.Create(packageReference);

                    installedPackages.Add(installedPackage);
                }

                if (nullCount > 0)
                {
                    telemetryEvent ??= new GetInstalledPackagesAsyncTelemetryEvent();

                    NuGetProject project = projects[i];

                    string projectId = project.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId);
                    NuGetProjectType projectType = VSTelemetryServiceUtility.GetProjectType(project);

                    telemetryEvent.AddProject(projectType, projectId, nullCount, totalCount);
                }
            }

            if (telemetryEvent is object)
            {
                TelemetryActivity.EmitTelemetryEvent(telemetryEvent);
            }

            return installedPackages;
        }

        public async ValueTask<IInstalledAndTransitivePackages> GetInstalledAndTransitivePackagesAsync(
            IReadOnlyCollection<string> projectIds,
            bool includeTransitiveOrigins,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectIds);

            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<NuGetProject> projects = await GetProjectsAsync(projectIds, cancellationToken);

            // If this is a PR-style project, get installed and transitive package references. Otherwise, just get installed package references.
            var prStyleTasks = new List<Task<ProjectPackages>>();
            var nonPrStyleTasks = new List<Task<IEnumerable<PackageReference>>>();
            foreach (NuGetProject? project in projects)
            {
                if (project is IPackageReferenceProject packageReferenceProject)
                {
                    prStyleTasks.Add(packageReferenceProject.GetInstalledAndTransitivePackagesAsync(includeTransitiveOrigins, cancellationToken));
                }
                else
                {
                    nonPrStyleTasks.Add(project.GetInstalledPackagesAsync(cancellationToken));
                }
            }
            ProjectPackages[] prStyleReferences = await Task.WhenAll(prStyleTasks);
            IEnumerable<PackageReference>[] nonPrStyleReferences = await Task.WhenAll(nonPrStyleTasks);

            // combine all of the installed package references
            IEnumerable<IEnumerable<PackageReference>> installedPackages = nonPrStyleReferences
                .Concat(prStyleReferences
                    .Select(p => p.InstalledPackages));

            PackageReferenceContextInfo[] installedPackagesContextInfos = installedPackages.SelectMany(e => e).Select(pr => PackageReferenceContextInfo.Create(pr)).ToArray();
            TransitivePackageReferenceContextInfo[] transitivePackageContextInfos = prStyleReferences.SelectMany(e => e.TransitivePackages).Select(pr => TransitivePackageReferenceContextInfo.Create(pr)).ToArray();
            return new InstalledAndTransitivePackages(installedPackagesContextInfos, transitivePackageContextInfos);
        }

        public async ValueTask<IInstalledAndTransitivePackages> GetInstalledAndTransitivePackagesAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken) => await GetInstalledAndTransitivePackagesAsync(projectIds, includeTransitiveOrigins: false, cancellationToken);

        public async ValueTask<IReadOnlyCollection<PackageDependencyInfo>> GetInstalledPackagesDependencyInfoAsync(
            string projectId,
            bool includeUnresolved,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);

            cancellationToken.ThrowIfCancellationRequested();

            NuGetPackageManager packageManager = await _sharedState.GetPackageManagerAsync(cancellationToken);

            NuGetProject? project = await SolutionUtility.GetNuGetProjectAsync(
                _sharedState.SolutionManager,
                projectId,
                cancellationToken);

            Assumes.NotNull(project);

            IEnumerable<PackageDependencyInfo>? results = await packageManager.GetInstalledPackagesDependencyInfo(
                project,
                cancellationToken,
                includeUnresolved);

            if (results == null)
            {
                return Array.Empty<PackageDependencyInfo>();
            }

            return results.ToArray();
        }

        public async ValueTask<IReadOnlyCollection<NuGetFramework>> GetTargetFrameworksAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectIds);

            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<NuGetProject> projects = await GetProjectsAsync(projectIds, cancellationToken);

            HashSet<NuGetFramework> targetFrameworks = new HashSet<NuGetFramework>();
            foreach (NuGetProject project in projects)
            {
                if (project is BuildIntegratedNuGetProject buildIntegratedProject)
                {
                    if (project is LegacyPackageReferenceProject legacyPackageReferenceProject)
                    {
                        targetFrameworks.Add(legacyPackageReferenceProject.TargetFramework);
                    }
                    else
                    {
                        var dgcContext = new DependencyGraphCacheContext();
                        IReadOnlyList<ProjectModel.PackageSpec>? packageSpecs = await buildIntegratedProject.GetPackageSpecsAsync(dgcContext);

                        IEnumerable<NuGetFramework>? frameworks = packageSpecs
                            .SelectMany(spec => spec.TargetFrameworks)
                            .Select(f => f.FrameworkName);

                        if (!(frameworks is null))
                        {
                            targetFrameworks.UnionWith(frameworks);
                        }
                    }
                }
            }

            return targetFrameworks;
        }

        public async ValueTask<IProjectMetadataContextInfo> GetMetadataAsync(string projectId, CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);

            cancellationToken.ThrowIfCancellationRequested();

            NuGetProject? project = await SolutionUtility.GetNuGetProjectAsync(
                _sharedState.SolutionManager,
                projectId,
                cancellationToken);

            Assumes.NotNull(project);

            return ProjectMetadataContextInfo.Create(project.Metadata);
        }

        public async ValueTask<(bool, string?)> TryGetInstalledPackageFilePathAsync(
            string projectId,
            PackageIdentity packageIdentity,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectId);
            Assumes.NotNull(packageIdentity);

            cancellationToken.ThrowIfCancellationRequested();

            NuGetProject? project = await SolutionUtility.GetNuGetProjectAsync(
                _sharedState.SolutionManager,
                projectId,
                cancellationToken);

            Assumes.NotNull(project);

            string? packageFilePath = null;

            if (project is MSBuildNuGetProject msBuildProject)
            {
                packageFilePath = msBuildProject.FolderNuGetProject.GetInstalledPackageFilePath(packageIdentity);
            }

            bool success = packageFilePath != null;

            return (success, packageFilePath);
        }

        public async ValueTask BeginOperationAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _semaphoreReleaser = await _state.AsyncSemaphore.EnterAsync(cancellationToken);

            _state.Reset();

            _state.SourceCacheContext = new SourceCacheContext();
        }

        public ValueTask EndOperationAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (AsyncSemaphore.Releaser? semaphoreReleaser = _semaphoreReleaser)
            {
                _state.Reset();

                _semaphoreReleaser = null;
            }

            return new ValueTask();
        }

        public async ValueTask ExecuteActionsAsync(IReadOnlyList<ProjectAction> actions, CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(actions);

            cancellationToken.ThrowIfCancellationRequested();

            await CatchAndRethrowExceptionAsync(async () =>
            {
                INuGetProjectContext? projectContext = null;

                try
                {
                    projectContext = await ServiceLocator.GetComponentModelServiceAsync<INuGetProjectContext>();

                    Assumes.NotNull(projectContext);

                    if (IsDirectInstall(actions))
                    {
                        NuGetPackageManager.SetDirectInstall(_state.PackageIdentity, projectContext);
                    }

                    var nugetProjectActions = new List<NuGetProjectAction>();

                    foreach (ProjectAction action in actions)
                    {
                        if (_state.ResolvedActions.TryGetValue(action.Id, out ResolvedAction resolvedAction))
                        {
                            nugetProjectActions.Add(resolvedAction.Action);
                        }
                    }

                    Assumes.NotNullOrEmpty(nugetProjectActions);

                    NuGetPackageManager packageManager = await _sharedState.GetPackageManagerAsync(cancellationToken);
                    IEnumerable<NuGetProject> projects = nugetProjectActions.Select(action => action.Project);

                    await packageManager.ExecuteNuGetProjectActionsAsync(
                        projects,
                        nugetProjectActions,
                        projectContext,
                        _state.SourceCacheContext,
                        cancellationToken);
                }
                finally
                {
                    if (projectContext != null)
                    {
                        NuGetPackageManager.ClearDirectInstall(projectContext);
                    }
                }
            });
        }

        public async ValueTask<IReadOnlyList<ProjectAction>> GetInstallActionsAsync(
            IReadOnlyCollection<string> projectIds,
            PackageIdentity packageIdentity,
            VersionConstraints versionConstraints,
            bool includePrerelease,
            DependencyBehavior dependencyBehavior,
            IReadOnlyList<string> packageSourceNames,
            CancellationToken cancellationToken)
        {
            return await GetInstallActionsAsync(projectIds, packageIdentity, versionConstraints, includePrerelease, dependencyBehavior, packageSourceNames, versionRange: null, cancellationToken);
        }

        public async ValueTask<IReadOnlyList<ProjectAction>> GetInstallActionsAsync(
            IReadOnlyCollection<string> projectIds,
            PackageIdentity packageIdentity,
            VersionConstraints versionConstraints,
            bool includePrerelease,
            DependencyBehavior dependencyBehavior,
            IReadOnlyList<string> packageSourceNames,
            VersionRange? versionRange,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectIds);
            Assumes.NotNull(packageIdentity);
            Assumes.NotNullOrEmpty(packageSourceNames);
            Assumes.Null(_state.PackageIdentity);
            Assumes.True(_state.ResolvedActions.Count == 0);
            Assumes.NotNull(_state.SourceCacheContext);

            cancellationToken.ThrowIfCancellationRequested();

            return await CatchAndRethrowExceptionAsync(async () =>
            {
                _state.PackageIdentity = packageIdentity;

                IReadOnlyList<SourceRepository> sourceRepositories = GetSourceRepositories(
                    packageSourceNames,
                    cancellationToken);

                Assumes.NotNullOrEmpty(sourceRepositories);

                INuGetProjectContext projectContext = await ServiceLocator.GetComponentModelServiceAsync<INuGetProjectContext>();
                IReadOnlyList<NuGetProject> projects = await GetProjectsAsync(projectIds, cancellationToken);

                var resolutionContext = new ResolutionContext(
                    dependencyBehavior,
                    includePrerelease,
                    includeUnlisted: false,
                    versionConstraints,
                    new GatherCache(),
                    _state.SourceCacheContext);

                NuGetPackageManager packageManager = await _sharedState.GetPackageManagerAsync(cancellationToken);
                IEnumerable<ResolvedAction> resolvedActions = await packageManager.PreviewProjectsInstallPackageAsync(
                    projects,
                    _state.PackageIdentity,
                    resolutionContext,
                    projectContext,
                    sourceRepositories,
                    versionRange,
                    cancellationToken);

                var projectActions = new List<ProjectAction>();

                foreach (ResolvedAction resolvedAction in resolvedActions)
                {
                    ProjectAction projectAction = CreateProjectAction(resolvedAction);

                    _state.ResolvedActions[projectAction.Id] = resolvedAction;

                    projectActions.Add(projectAction);
                }

                return projectActions;
            });
        }

        public async ValueTask<IReadOnlyList<ProjectAction>> GetUninstallActionsAsync(
            IReadOnlyCollection<string> projectIds,
            PackageIdentity packageIdentity,
            bool removeDependencies,
            bool forceRemove,
            CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectIds);
            Assumes.NotNull(packageIdentity);
            Assumes.False(packageIdentity.HasVersion);
            Assumes.NotNull(_state.SourceCacheContext);
            Assumes.Null(_state.PackageIdentity);
            Assumes.True(_state.ResolvedActions.Count == 0);

            cancellationToken.ThrowIfCancellationRequested();

            return await CatchAndRethrowExceptionAsync(async () =>
            {
                INuGetProjectContext projectContext = await ServiceLocator.GetComponentModelServiceAsync<INuGetProjectContext>();
                IReadOnlyList<NuGetProject> projects = await GetProjectsAsync(projectIds, cancellationToken);

                var projectActions = new List<ProjectAction>();
                var uninstallationContext = new UninstallationContext(removeDependencies, forceRemove);

                NuGetPackageManager packageManager = await _sharedState.GetPackageManagerAsync(cancellationToken);
                IEnumerable<NuGetProjectAction> projectsWithActions = await packageManager.PreviewProjectsUninstallPackageAsync(
                    projects,
                    packageIdentity.Id,
                    uninstallationContext,
                    projectContext,
                    cancellationToken);

                foreach (NuGetProjectAction projectWithActions in projectsWithActions)
                {
                    var resolvedAction = new ResolvedAction(projectWithActions.Project, projectWithActions);
                    var projectAction = new ProjectAction(
                        CreateProjectActionId(),
                        projectWithActions.Project.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId),
                        projectWithActions.PackageIdentity,
                        projectWithActions.NuGetProjectActionType,
                        implicitActions: null);

                    _state.ResolvedActions[projectAction.Id] = resolvedAction;

                    projectActions.Add(projectAction);
                }

                return projectActions;
            });
        }

        public async ValueTask<IReadOnlyList<ProjectAction>> GetUpdateActionsAsync(
           IReadOnlyCollection<string> projectIds,
           IReadOnlyCollection<PackageIdentity> packageIdentities,
           VersionConstraints versionConstraints,
           bool includePrelease,
           DependencyBehavior dependencyBehavior,
           IReadOnlyList<string> packageSourceNames,
           CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(projectIds);
            Assumes.NotNullOrEmpty(packageIdentities);
            Assumes.NotNullOrEmpty(packageSourceNames);
            Assumes.NotNull(_state.SourceCacheContext);
            Assumes.NotNull(_state.ResolvedActions);
            Assumes.Null(_state.PackageIdentity);

            return await CatchAndRethrowExceptionAsync(async () =>
            {
                var primarySources = new List<SourceRepository>();
                var secondarySources = new List<SourceRepository>();

                IEnumerable<SourceRepository> sourceRepositories = _sharedState.SourceRepositoryProvider.GetRepositories();
                var packageSourceNamesSet = new HashSet<string>(packageSourceNames, StringComparer.OrdinalIgnoreCase);

                foreach (SourceRepository sourceRepository in sourceRepositories)
                {
                    if (packageSourceNamesSet.Contains(sourceRepository.PackageSource.Name))
                    {
                        primarySources.Add(sourceRepository);
                    }

                    if (sourceRepository.PackageSource.IsEnabled)
                    {
                        secondarySources.Add(sourceRepository);
                    }
                }

                INuGetProjectContext projectContext = await ServiceLocator.GetComponentModelServiceAsync<INuGetProjectContext>();
                IReadOnlyList<NuGetProject> projects = await GetProjectsAsync(projectIds, cancellationToken);

                var resolutionContext = new ResolutionContext(
                    dependencyBehavior,
                    includePrelease,
                    includeUnlisted: true,
                    versionConstraints,
                    new GatherCache(),
                    _state.SourceCacheContext);

                NuGetPackageManager packageManager = await _sharedState.GetPackageManagerAsync(cancellationToken);
                IEnumerable<NuGetProjectAction> actions = await packageManager.PreviewUpdatePackagesAsync(
                    packageIdentities.ToList(),
                    projects,
                    resolutionContext,
                    projectContext,
                    primarySources,
                    secondarySources,
                    cancellationToken);

                var projectActions = new List<ProjectAction>();

                foreach (NuGetProjectAction action in actions)
                {
                    var resolvedAction = new ResolvedAction(action.Project, action);
                    ProjectAction projectAction = CreateProjectAction(resolvedAction);

                    _state.ResolvedActions[projectAction.Id] = resolvedAction;

                    projectActions.Add(projectAction);
                }

                return projectActions;
            });
        }

        /// <inheritdoc />
        public async ValueTask<IReadOnlyCollection<string>> GetPackageFoldersAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken)
        {
            if (projectIds == null)
            {
                throw new ArgumentNullException(nameof(projectIds));
            }

            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<NuGetProject> projects = await GetProjectsAsync(projectIds, cancellationToken);
            var prStyleTasks = new List<Task<IReadOnlyCollection<string>>>();
            foreach (NuGetProject? project in projects)
            {
                if (project is IPackageReferenceProject packageReferenceProject)
                {
                    prStyleTasks.Add(packageReferenceProject.GetPackageFoldersAsync(cancellationToken));
                }
            }

            IReadOnlyCollection<string>[] packageFolders = await Task.WhenAll(prStyleTasks);

            HashSet<string> packageFoldersUnique = new HashSet<string>();
            packageFolders.ForEach(folders => packageFoldersUnique.AddRange(folders));

            return packageFoldersUnique.ToList();
        }

        public async ValueTask<IReadOnlyCollection<IProjectContextInfo>> GetProjectsWithDeprecatedDotnetFrameworkAsync(CancellationToken cancellationToken)
        {
            Assumes.NotNullOrEmpty(_state.ResolvedActions);

            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<NuGetProject> affectedProjects = DotnetDeprecatedPrompt.GetAffectedProjects(_state.ResolvedActions.Values);

            IEnumerable<Task<IProjectContextInfo>> tasks = affectedProjects
                .Select(affectedProject => ProjectContextInfo.CreateAsync(affectedProject, cancellationToken).AsTask());

            return await Task.WhenAll(tasks);
        }

        private static ProjectAction CreateProjectAction(ResolvedAction resolvedAction)
        {
            List<ImplicitProjectAction>? implicitActions = null;

            if (resolvedAction.Action is BuildIntegratedProjectAction buildIntegratedAction)
            {
                implicitActions = new List<ImplicitProjectAction>();

                foreach (NuGetProjectAction buildAction in buildIntegratedAction.GetProjectActions())
                {
                    var implicitAction = new ImplicitProjectAction(
                        CreateProjectActionId(),
                        buildAction.PackageIdentity,
                        buildAction.NuGetProjectActionType);

                    implicitActions.Add(implicitAction);
                }
            }

            string projectId = resolvedAction.Project.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId);
            var projectAction = new ProjectAction(
                CreateProjectActionId(),
                projectId,
                resolvedAction.Action.PackageIdentity,
                resolvedAction.Action.NuGetProjectActionType,
                implicitActions);

            return projectAction;
        }

        private static string CreateProjectActionId()
        {
            return Guid.NewGuid().ToString("N", provider: null);
        }

        private async Task<IReadOnlyList<NuGetProject>> GetProjectsAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken)
        {
            IVsSolutionManager? solutionManager = await _sharedState.SolutionManager.GetValueAsync(cancellationToken);
            Assumes.NotNull(solutionManager);

            Dictionary<string, NuGetProject>? projects = (await solutionManager.GetNuGetProjectsAsync())
                .ToDictionary(project => project.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId), _ => _, StringComparer.OrdinalIgnoreCase);
            var matchingProjects = new List<NuGetProject>(capacity: projectIds.Count);

            foreach (string projectId in projectIds)
            {
                Assumes.NotNullOrEmpty(projectId);

                if (projects.TryGetValue(projectId, out NuGetProject project))
                {
                    Assumes.NotNull(project);
                    matchingProjects.Add(project);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, Strings.ProjectWithIdNotFound, projectId),
                        nameof(projectIds));
                }
            }

            return matchingProjects;
        }

        private IReadOnlyList<SourceRepository> GetSourceRepositories(
            IReadOnlyList<string> packageSourceNames,
            CancellationToken cancellationToken)
        {
            var sourceRepositories = new List<SourceRepository>();
            Dictionary<string, SourceRepository> allSourceRepositories = _sharedState.SourceRepositoryProvider.GetRepositories()
                .ToDictionary(sr => sr.PackageSource.Name, sr => sr);

            foreach (string packageSourceName in packageSourceNames)
            {
                if (allSourceRepositories.TryGetValue(packageSourceName, out SourceRepository sourceRepository))
                {
                    sourceRepositories.Add(sourceRepository);
                }
            }

            return sourceRepositories;
        }

        private bool IsDirectInstall(IReadOnlyList<ProjectAction> projectActions)
        {
            return _state.PackageIdentity != null
                && projectActions.Any(projectAction => projectAction.ProjectActionType == NuGetProjectActionType.Install);
        }

        private async ValueTask CatchAndRethrowExceptionAsync(Func<Task> taskFunc)
        {
            try
            {
                await taskFunc();
            }
            catch (Exception ex)
            {
                var exception = new LocalRpcException(ex.Message, ex)
                {
                    ErrorCode = (int)RemoteErrorCode.RemoteError,
                    ErrorData = RemoteErrorUtility.ToRemoteError(ex)
                };

                throw exception;
            }
        }

        private async ValueTask<T> CatchAndRethrowExceptionAsync<T>(Func<Task<T>> taskFunc)
        {
            try
            {
                return await taskFunc();
            }
            catch (Exception ex)
            {
                var exception = new LocalRpcException(ex.Message, ex)
                {
                    ErrorCode = (int)RemoteErrorCode.RemoteError,
                    ErrorData = RemoteErrorUtility.ToRemoteError(ex)
                };

                throw exception;
            }
        }
    }
}
