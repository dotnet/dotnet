// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement.Utility;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// NuGetPackageManager orchestrates a nuget package operation such as an install or uninstall
    /// It is to be called by various NuGet Clients including the custom third-party ones
    /// </summary>
    public class NuGetPackageManager
    {
        private IReadOnlyList<SourceRepository> _globalPackageFolderRepositories;

        private ISourceRepositoryProvider SourceRepositoryProvider { get; }

        private ISolutionManager SolutionManager { get; }

        private Configuration.ISettings Settings { get; }

        private HashSet<string> _buildIntegratedProjectsUpdateSet;

        private DependencyGraphSpec _buildIntegratedProjectsCache;

        private RestoreCommandProvidersCache _restoreProviderCache;

        public IDeleteOnRestartManager DeleteOnRestartManager { get; }

        public FolderNuGetProject PackagesFolderNuGetProject { get; set; }

        public SourceRepository PackagesFolderSourceRepository { get; set; }

        public IInstallationCompatibility InstallationCompatibility { get; set; }

        private IRestoreProgressReporter RestoreProgressReporter { get; }

        /// <summary>
        /// Event to be raised when batch processing of install/ uninstall packages starts at a project level
        /// </summary>
        public event EventHandler<PackageProjectEventArgs> BatchStart;

        /// <summary>
        /// Event to be raised when batch processing of install/ uninstall packages ends at a project level
        /// </summary>
        public event EventHandler<PackageProjectEventArgs> BatchEnd;

        /// <summary>
        /// To construct a NuGetPackageManager that does not need a SolutionManager like NuGet.exe
        /// </summary>
        public NuGetPackageManager(
                ISourceRepositoryProvider sourceRepositoryProvider,
                ISettings settings,
                string packagesFolderPath)
            : this(sourceRepositoryProvider, settings, packagesFolderPath, excludeVersion: false)
        {
        }

        public NuGetPackageManager(
            ISourceRepositoryProvider sourceRepositoryProvider,
            ISettings settings,
            string packagesFolderPath,
            bool excludeVersion)
        {
            if (packagesFolderPath == null)
            {
                throw new ArgumentNullException(nameof(packagesFolderPath));
            }

            SourceRepositoryProvider = sourceRepositoryProvider ?? throw new ArgumentNullException(nameof(sourceRepositoryProvider));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            InstallationCompatibility = PackageManagement.InstallationCompatibility.Instance;

            InitializePackagesFolderInfo(packagesFolderPath, excludeVersion);
        }

        /// <summary>
        /// To construct a NuGetPackageManager with a mandatory SolutionManager lke VS
        /// </summary>
        public NuGetPackageManager(
                ISourceRepositoryProvider sourceRepositoryProvider,
                ISettings settings,
                ISolutionManager solutionManager,
                IDeleteOnRestartManager deleteOnRestartManager)
        : this(sourceRepositoryProvider, settings, solutionManager, deleteOnRestartManager, excludeVersion: false)
        {
        }

        public NuGetPackageManager(
            ISourceRepositoryProvider sourceRepositoryProvider,
            ISettings settings,
            ISolutionManager solutionManager,
            IDeleteOnRestartManager deleteOnRestartManager,
            bool excludeVersion)
            : this(sourceRepositoryProvider, settings, solutionManager, deleteOnRestartManager, reporter: null, excludeVersion: excludeVersion)
        {
        }

        public NuGetPackageManager(
            ISourceRepositoryProvider sourceRepositoryProvider,
            ISettings settings,
            ISolutionManager solutionManager,
            IDeleteOnRestartManager deleteOnRestartManager,
            IRestoreProgressReporter reporter) :
            this(sourceRepositoryProvider, settings, solutionManager, deleteOnRestartManager, reporter, excludeVersion: false)
        {
            _ = reporter ?? throw new ArgumentNullException(nameof(reporter));
        }

        public NuGetPackageManager(
            ISourceRepositoryProvider sourceRepositoryProvider,
            ISettings settings,
            ISolutionManager solutionManager,
            IDeleteOnRestartManager deleteOnRestartManager,
            IRestoreProgressReporter reporter,
            bool excludeVersion)
        {
            SourceRepositoryProvider = sourceRepositoryProvider ?? throw new ArgumentNullException(nameof(sourceRepositoryProvider));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            SolutionManager = solutionManager ?? throw new ArgumentNullException(nameof(solutionManager));
            InstallationCompatibility = PackageManagement.InstallationCompatibility.Instance;
            InitializePackagesFolderInfo(PackagesFolderPathUtility.GetPackagesFolderPath(SolutionManager, Settings), excludeVersion);
            DeleteOnRestartManager = deleteOnRestartManager ?? throw new ArgumentNullException(nameof(deleteOnRestartManager));
            RestoreProgressReporter = reporter;
        }

        /// <summary>
        /// SourceRepositories for the user global package folder and all fallback package folders.
        /// </summary>
        public IReadOnlyList<SourceRepository> GlobalPackageFolderRepositories
        {
            get
            {
                if (_globalPackageFolderRepositories == null)
                {
                    var sources = new List<SourceRepository>();

                    // Read package folders from settings
                    var pathContext = NuGetPathContext.Create(Settings);

                    // count = FallbackPackageFolders.Count + 1 for UserPackageFolder
                    var count = (pathContext.FallbackPackageFolders?.Count() ?? 0) + 1;
                    var folders = new List<string>(count)
                    {
                        pathContext.UserPackageFolder
                    };

                    folders.AddRange(pathContext.FallbackPackageFolders);
                    foreach (var folder in folders)
                    {
                        // Create a repo for each folder
                        var source = SourceRepositoryProvider.CreateRepository(new PackageSource(folder), FeedType.FileSystemV3);

                        sources.Add(source);
                    }

                    _globalPackageFolderRepositories = sources;
                }

                return _globalPackageFolderRepositories;
            }
        }

        private void InitializePackagesFolderInfo(string packagesFolderPath, bool excludeVersion = false)
        {
            // FileSystemPackagesConfig supports id.version formats, if the version is excluded use the normal v2 format
            var feedType = excludeVersion ? FeedType.FileSystemV2 : FeedType.FileSystemPackagesConfig;
            var resolver = new PackagePathResolver(packagesFolderPath, !excludeVersion);

            PackagesFolderNuGetProject = new FolderNuGetProject(packagesFolderPath, resolver);
            // Capturing it locally is important since it allows for the instance to cache packages for the lifetime
            // of the closure \ NuGetPackageManager.
            PackagesFolderSourceRepository = SourceRepositoryProvider.CreateRepository(
                new PackageSource(packagesFolderPath),
                feedType);
        }

        /// <summary>
        /// Installs the latest version of the given <paramref name="packageId" /> to NuGetProject
        /// <paramref name="nuGetProject" /> <paramref name="resolutionContext" /> and
        /// <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public Task InstallPackageAsync(
            NuGetProject nuGetProject,
            string packageId,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            SourceRepository primarySourceRepository,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            if (resolutionContext == null)
            {
                throw new ArgumentNullException(nameof(resolutionContext));
            }
            var logger = new LoggerAdapter(nuGetProjectContext);

            var downloadContext = new PackageDownloadContext(resolutionContext.SourceCacheContext)
            {
                ParentId = nuGetProjectContext.OperationId,
                ClientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, logger)
            };

            return InstallPackageAsync(
                nuGetProject,
                packageId,
                resolutionContext,
                nuGetProjectContext,
                downloadContext,
                new List<SourceRepository> { primarySourceRepository },
                secondarySources,
                token);
        }

        /// <summary>
        /// Installs the latest version of the given <paramref name="packageId" /> to NuGetProject
        /// <paramref name="nuGetProject" /> <paramref name="resolutionContext" /> and
        /// <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public Task InstallPackageAsync(
            NuGetProject nuGetProject,
            string packageId,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext,
            SourceRepository primarySourceRepository,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            return InstallPackageAsync(
                nuGetProject,
                packageId,
                resolutionContext,
                nuGetProjectContext,
                downloadContext,
                new List<SourceRepository> { primarySourceRepository },
                secondarySources,
                token);
        }

        /// <summary>
        /// Installs the latest version of the given
        /// <paramref name="packageId" /> to NuGetProject <paramref name="nuGetProject" />
        /// <paramref name="resolutionContext" /> and <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public async Task InstallPackageAsync(NuGetProject nuGetProject, string packageId, ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext, IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources, CancellationToken token)
        {
            if (resolutionContext == null)
            {
                throw new ArgumentNullException(nameof(resolutionContext));
            }

            var logger = new LoggerAdapter(nuGetProjectContext);

            var downloadContext = new PackageDownloadContext(resolutionContext.SourceCacheContext)
            {
                ParentId = nuGetProjectContext.OperationId,
                ClientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, logger)
            };

            await InstallPackageAsync(
                nuGetProject,
                packageId,
                resolutionContext,
                nuGetProjectContext,
                downloadContext,
                primarySources,
                secondarySources,
                token);
        }

        /// <summary>
        /// Installs the latest version of the given
        /// <paramref name="packageId" /> to NuGetProject <paramref name="nuGetProject" />
        /// <paramref name="resolutionContext" /> and <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public async Task InstallPackageAsync(
            NuGetProject nuGetProject,
            string packageId,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            var log = new LoggerAdapter(nuGetProjectContext);

            // Step-1 : Get latest version for packageId
            var resolvedPackage = await GetLatestVersionAsync(
                packageId,
                nuGetProject,
                resolutionContext,
                primarySources,
                log,
                token);

            if (resolvedPackage == null || resolvedPackage.LatestVersion == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.NoLatestVersionFound, packageId));
            }

            // Step-2 : Call InstallPackageAsync(project, packageIdentity)
            await InstallPackageAsync(
                nuGetProject,
                new PackageIdentity(packageId, resolvedPackage.LatestVersion),
                resolutionContext,
                nuGetProjectContext,
                downloadContext,
                primarySources,
                secondarySources,
                token);
        }

        /// <summary>
        /// Installs given <paramref name="packageIdentity" /> to NuGetProject <paramref name="nuGetProject" />
        /// <paramref name="resolutionContext" /> and <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public Task InstallPackageAsync(
            NuGetProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            SourceRepository primarySourceRepository,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            return InstallPackageAsync(
                nuGetProject,
                packageIdentity,
                resolutionContext,
                nuGetProjectContext,
                new List<SourceRepository> { primarySourceRepository },
                secondarySources,
                token);
        }

        /// <summary>
        /// Installs given <paramref name="packageIdentity" /> to NuGetProject <paramref name="nuGetProject" />
        /// <paramref name="resolutionContext" /> and <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public Task InstallPackageAsync(
            NuGetProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext,
            SourceRepository primarySourceRepository,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            return InstallPackageAsync(
                nuGetProject,
                packageIdentity,
                resolutionContext,
                nuGetProjectContext,
                downloadContext,
                new List<SourceRepository> { primarySourceRepository },
                secondarySources,
                token);
        }

        /// <summary>
        /// Installs given <paramref name="packageIdentity" /> to NuGetProject <paramref name="nuGetProject" />
        /// <paramref name="resolutionContext" /> and <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public async Task InstallPackageAsync(
            NuGetProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            if (resolutionContext == null)
            {
                throw new ArgumentNullException(nameof(resolutionContext));
            }

            var logger = new LoggerAdapter(nuGetProjectContext);

            var downloadContext = new PackageDownloadContext(resolutionContext.SourceCacheContext)
            {
                ParentId = nuGetProjectContext.OperationId,
                ClientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, logger)
            };

            await InstallPackageAsync(
                nuGetProject,
                packageIdentity,
                resolutionContext,
                nuGetProjectContext,
                downloadContext,
                primarySources,
                secondarySources,
                token);
        }

        /// <summary>
        /// Installs given <paramref name="packageIdentity" /> to NuGetProject <paramref name="nuGetProject" />
        /// <paramref name="resolutionContext" /> and <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public async Task InstallPackageAsync(
            NuGetProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            ActivityCorrelationId.StartNew();

            // Step-1 : Call PreviewInstallPackageAsync to get all the nuGetProjectActions
            var nuGetProjectActions = await PreviewInstallPackageAsync(nuGetProject, packageIdentity, resolutionContext,
                nuGetProjectContext, primarySources, secondarySources, token);

            SetDirectInstall(packageIdentity, nuGetProjectContext);

            // Step-2 : Execute all the nuGetProjectActions
            await ExecuteNuGetProjectActionsAsync(
                nuGetProject,
                nuGetProjectActions,
                nuGetProjectContext,
                downloadContext,
                token);

            ClearDirectInstall(nuGetProjectContext);
        }

        public async Task UninstallPackageAsync(NuGetProject nuGetProject, string packageId, UninstallationContext uninstallationContext,
            INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            ActivityCorrelationId.StartNew();

            // Step-1 : Call PreviewUninstallPackagesAsync to get all the nuGetProjectActions
            var nuGetProjectActions = await PreviewUninstallPackageAsync(nuGetProject, packageId, uninstallationContext, nuGetProjectContext, token);

            // Step-2 : Execute all the nuGetProjectActions
            await ExecuteNuGetProjectActionsAsync(nuGetProject, nuGetProjectActions, nuGetProjectContext, NullSourceCacheContext.Instance, token);
        }

        /// <summary>
        /// Gives the preview as a list of NuGetProjectActions that will be performed to install
        /// <paramref name="packageId" /> into <paramref name="nuGetProject" /> <paramref name="resolutionContext" />
        /// and <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public Task<IEnumerable<NuGetProjectAction>> PreviewInstallPackageAsync(
            NuGetProject nuGetProject,
            string packageId,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            SourceRepository primarySourceRepository,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            return PreviewInstallPackageAsync(nuGetProject, packageId, resolutionContext, nuGetProjectContext, new[] { primarySourceRepository }, secondarySources, token);
        }

        /// <summary>
        /// Gives the preview as a list of NuGetProjectActions that will be performed to install
        /// <paramref name="packageId" /> into <paramref name="nuGetProject" /> <paramref name="resolutionContext" />
        /// and <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public async Task<IEnumerable<NuGetProjectAction>> PreviewInstallPackageAsync(
            NuGetProject nuGetProject,
            string packageId,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }

            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            if (resolutionContext == null)
            {
                throw new ArgumentNullException(nameof(resolutionContext));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            var log = new LoggerAdapter(nuGetProjectContext);

            // Step-1 : Get latest version for packageId
            var resolvedPackage = await GetLatestVersionAsync(
                packageId,
                nuGetProject,
                resolutionContext,
                primarySources,
                log,
                token);

            if (resolvedPackage == null || resolvedPackage.LatestVersion == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.UnknownPackage, packageId));
            }

            var projectInstalledPackageReferences = await nuGetProject.GetInstalledPackagesAsync(token);
            var installedPackageReference = projectInstalledPackageReferences.Where(pr => StringComparer.OrdinalIgnoreCase.Equals(pr.PackageIdentity.Id, packageId)).FirstOrDefault();
            if (installedPackageReference != null
                && installedPackageReference.PackageIdentity.Version > resolvedPackage.LatestVersion)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.NewerVersionAlreadyReferenced, packageId));
            }

            // Step-2 : Call InstallPackage(project, packageIdentity)
            return await PreviewInstallPackageAsync(nuGetProject, new PackageIdentity(packageId, resolvedPackage.LatestVersion), resolutionContext,
                nuGetProjectContext, primarySources, secondarySources, token);
        }

        public Task<IEnumerable<NuGetProjectAction>> PreviewUpdatePackagesAsync(
            IEnumerable<NuGetProject> nuGetProjects,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            return PreviewUpdatePackagesAsync(
                packageId: null,
                packageIdentities: new List<PackageIdentity>(),
                nuGetProjects: nuGetProjects,
                resolutionContext: resolutionContext,
                nuGetProjectContext: nuGetProjectContext,
                primarySources: primarySources,
                secondarySources: secondarySources,
                token: token);
        }

        public Task<IEnumerable<NuGetProjectAction>> PreviewUpdatePackagesAsync(
            string packageId,
            IEnumerable<NuGetProject> nuGetProjects,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            return PreviewUpdatePackagesAsync(
                packageId: packageId,
                packageIdentities: new List<PackageIdentity>(),
                nuGetProjects: nuGetProjects,
                resolutionContext: resolutionContext,
                nuGetProjectContext: nuGetProjectContext,
                primarySources: primarySources,
                secondarySources: secondarySources,
                token: token);
        }

        public Task<IEnumerable<NuGetProjectAction>> PreviewUpdatePackagesAsync(
            PackageIdentity packageIdentity,
            IEnumerable<NuGetProject> nuGetProjects,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            return PreviewUpdatePackagesAsync(
                packageId: null,
                packageIdentities: new List<PackageIdentity> { packageIdentity },
                nuGetProjects: nuGetProjects,
                resolutionContext: resolutionContext,
                nuGetProjectContext: nuGetProjectContext,
                primarySources: primarySources,
                secondarySources: secondarySources,
                token: token);
        }

        public Task<IEnumerable<NuGetProjectAction>> PreviewUpdatePackagesAsync(
            List<PackageIdentity> packageIdentities,
            IEnumerable<NuGetProject> nuGetProjects,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            return PreviewUpdatePackagesAsync(
                packageId: null,
                packageIdentities: packageIdentities,
                nuGetProjects: nuGetProjects,
                resolutionContext: resolutionContext,
                nuGetProjectContext: nuGetProjectContext,
                primarySources: primarySources,
                secondarySources: secondarySources,
                token: token);
        }

        private async Task<IEnumerable<NuGetProjectAction>> PreviewUpdatePackagesAsync(
                string packageId,
                List<PackageIdentity> packageIdentities,
                IEnumerable<NuGetProject> nuGetProjects,
                ResolutionContext resolutionContext,
                INuGetProjectContext nuGetProjectContext,
                IEnumerable<SourceRepository> primarySources,
                IEnumerable<SourceRepository> secondarySources,
                CancellationToken token)
        {
            if (packageIdentities == null)
            {
                throw new ArgumentNullException(nameof(packageIdentities));
            }

            if (nuGetProjects == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjects));
            }

            if (resolutionContext == null)
            {
                throw new ArgumentNullException(nameof(resolutionContext));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            if (primarySources == null)
            {
                throw new ArgumentNullException(nameof(primarySources));
            }

            if (secondarySources == null)
            {
                throw new ArgumentNullException(nameof(secondarySources));
            }

            var maxTasks = 4;
            var tasks = new List<Task<IEnumerable<NuGetProjectAction>>>(maxTasks);
            var nugetActions = new List<NuGetProjectAction>();

            var nuGetProjectList = nuGetProjects.ToList();
            var buildIntegratedProjects = nuGetProjectList.OfType<BuildIntegratedNuGetProject>().ToList();
            var nonBuildIntegratedProjects = nuGetProjectList.Except(buildIntegratedProjects).ToList();

            var shouldFilterProjectsForUpdate = false;
            if (packageIdentities.Count > 0)
            {
                shouldFilterProjectsForUpdate = true;
            }

            // Update http source cache context MaxAge so that it can always go online to fetch
            // latest version of packages.
            resolutionContext.SourceCacheContext.MaxAge = DateTimeOffset.UtcNow;

            // add tasks for all build integrated projects
            foreach (var project in buildIntegratedProjects)
            {
                var packagesToUpdateInProject = packageIdentities;
                var updatedResolutionContext = resolutionContext;

                if (shouldFilterProjectsForUpdate)
                {
                    packagesToUpdateInProject = await GetPackagesToUpdateInProjectAsync(project, packageIdentities, token);
                    if (packagesToUpdateInProject.Count > 0)
                    {
                        var includePrerelease = packagesToUpdateInProject.Any(
                        package => package.Version.IsPrerelease) || resolutionContext.IncludePrerelease;

                        updatedResolutionContext = new ResolutionContext(
                            dependencyBehavior: resolutionContext.DependencyBehavior,
                            includePrelease: includePrerelease,
                            includeUnlisted: resolutionContext.IncludeUnlisted,
                            versionConstraints: resolutionContext.VersionConstraints,
                            gatherCache: resolutionContext.GatherCache,
                            sourceCacheContext: resolutionContext.SourceCacheContext);
                    }
                    else
                    {
                        // skip running update preview for this project, since it doesn't have any package installed
                        // which is being updated.
                        continue;
                    }
                }

                // if tasks count reachs max then wait until an existing task is completed
                if (tasks.Count >= maxTasks)
                {
                    var actions = await CompleteTaskAsync(tasks);
                    nugetActions.AddRange(actions);
                }

                tasks.Add(Task.Run(async ()
                    => await PreviewUpdatePackagesForBuildIntegratedAsync(
                            packageId,
                            packagesToUpdateInProject,
                            project,
                            updatedResolutionContext,
                            nuGetProjectContext,
                            primarySources,
                            token)));
            }

            // Wait for all restores to finish
            var allActions = await Task.WhenAll(tasks);
            nugetActions.AddRange(allActions.SelectMany(action => action));

            foreach (var project in nonBuildIntegratedProjects)
            {
                var packagesToUpdateInProject = packageIdentities;
                var updatedResolutionContext = resolutionContext;

                if (shouldFilterProjectsForUpdate)
                {
                    packagesToUpdateInProject = await GetPackagesToUpdateInProjectAsync(project, packageIdentities, token);
                    if (packagesToUpdateInProject.Count > 0)
                    {
                        var includePrerelease = packagesToUpdateInProject.Any(
                        package => package.HasVersion && package.Version.IsPrerelease) || resolutionContext.IncludePrerelease;

                        updatedResolutionContext = new ResolutionContext(
                            dependencyBehavior: resolutionContext.DependencyBehavior,
                            includePrelease: includePrerelease,
                            includeUnlisted: resolutionContext.IncludeUnlisted,
                            versionConstraints: resolutionContext.VersionConstraints,
                            gatherCache: resolutionContext.GatherCache,
                            sourceCacheContext: resolutionContext.SourceCacheContext);
                    }
                    else
                    {
                        // skip running update preview for this project, since it doesn't have any package installed
                        // which is being updated.
                        continue;
                    }
                }

                // packages.config based projects are handled here
                nugetActions.AddRange(await PreviewUpdatePackagesForClassicAsync(
                packageId,
                packagesToUpdateInProject,
                project,
                updatedResolutionContext,
                nuGetProjectContext,
                primarySources,
                secondarySources,
                token));
            }

            return nugetActions;
        }

        private async Task<List<PackageIdentity>> GetPackagesToUpdateInProjectAsync(
            NuGetProject project,
            List<PackageIdentity> packages,
            CancellationToken token)
        {
            var installedPackages = await project.GetInstalledPackagesAsync(token);

            var packageIds = new HashSet<string>(
                installedPackages.Select(p => p.PackageIdentity.Id), StringComparer.OrdinalIgnoreCase);

            // We need to filter out packages from packagesToUpdate that are not installed
            // in the current project. Otherwise, we'll incorrectly install a
            // package that is not installed before.
            var packagesToUpdateInProject = packages.Where(
                package => packageIds.Contains(package.Id)).ToList();

            return packagesToUpdateInProject;
        }

        private async Task<IEnumerable<T>> CompleteTaskAsync<T>(
            List<Task<IEnumerable<T>>> updateTasks)
        {
            var doneTask = await Task.WhenAny(updateTasks);
            updateTasks.Remove(doneTask);
            return await doneTask;
        }

        /// <summary>
        /// Update Package logic specific to build integrated style NuGet projects
        /// </summary>
        private async Task<IEnumerable<NuGetProjectAction>> PreviewUpdatePackagesForBuildIntegratedAsync(
            string packageId,
            IReadOnlyList<PackageIdentity> packageIdentities,
            NuGetProject nuGetProject,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> primarySources,
            CancellationToken token)
        {
            var projectInstalledPackageReferences = await nuGetProject.GetInstalledPackagesAsync(token);

            if (!nuGetProject.TryGetMetadata(NuGetProjectMetadataKeys.TargetFramework, out NuGetFramework framework))
            {
                // Default to the any framework if the project does not specify a framework.
                framework = NuGetFramework.AnyFramework;
            }

            var log = new LoggerAdapter(nuGetProjectContext);

            var actions = new List<NuGetProjectAction>();

            if (packageIdentities.Count == 0 && packageId == null)
            {
                // Update All
                var lowLevelActions = new List<NuGetProjectAction>();

                foreach (var installedPackage in projectInstalledPackageReferences)
                {
                    // Skip auto referenced packages during update all.
                    var autoReferenced = IsPackageReferenceAutoReferenced(installedPackage);

                    if (!autoReferenced)
                    {
                        var resolvedPackage = await GetLatestVersionAsync(
                            installedPackage,
                            framework,
                            resolutionContext,
                            primarySources,
                            log,
                            token);

                        if (resolvedPackage != null && resolvedPackage.LatestVersion != null && resolvedPackage.LatestVersion > installedPackage.PackageIdentity.Version)
                        {
                            lowLevelActions.Add(NuGetProjectAction.CreateInstallProjectAction(
                                new PackageIdentity(installedPackage.PackageIdentity.Id, resolvedPackage.LatestVersion),
                                primarySources.FirstOrDefault(),
                                nuGetProject));
                        }
                    }
                }

                // If the update operation is a no-op there will be no project actions.
                if (lowLevelActions.Any())
                {
                    var buildIntegratedProject = nuGetProject as BuildIntegratedNuGetProject;

                    if (buildIntegratedProject != null)
                    {
                        // Create a build integrated action
                        var buildIntegratedAction =
                            await PreviewBuildIntegratedProjectActionsAsync(buildIntegratedProject, lowLevelActions, nuGetProjectContext, token);

                        actions.Add(buildIntegratedAction);
                    }
                    else
                    {
                        // Use the low level actions for projectK
                        actions = lowLevelActions;
                    }
                }
            }
            else
            {
                // either we have a packageId or a list of specific PackageIdentities to work with

                // first lets normalize this input so we are just dealing with a list

                if (packageIdentities.Count == 0)
                {
                    var installedPackageReference = projectInstalledPackageReferences
                        .FirstOrDefault(pr => StringComparer.OrdinalIgnoreCase.Equals(pr.PackageIdentity.Id, packageId));
                    // Skip autoreferenced update when we have only a package ID.
                    var autoReferenced = IsPackageReferenceAutoReferenced(installedPackageReference);

                    if (installedPackageReference != null && !autoReferenced)
                    {
                        var resolvedPackage = await GetLatestVersionAsync(
                            installedPackageReference,
                            framework,
                            resolutionContext,
                            primarySources,
                            log,
                            token);

                        if (resolvedPackage == null || !resolvedPackage.Exists)
                        {
                            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.UnknownPackage, packageId));
                        }
                        else if (resolvedPackage.LatestVersion != null)
                        {
                            if (installedPackageReference.PackageIdentity.Version > resolvedPackage.LatestVersion)
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                    Strings.NewerVersionAlreadyReferenced, packageId));
                            }
                            else if (installedPackageReference.PackageIdentity.Version < resolvedPackage.LatestVersion)
                            {
                                // The same instance of packageIdentities might be used by multiple tasks in parallel,
                                // so it's unsafe to modify, hence which it's IReadOnlyList and not just List.
                                // Therefore, treat the list as immutable (need a new instance with changes).
                                packageIdentities = packageIdentities.Concat(new[] { new PackageIdentity(packageId, resolvedPackage.LatestVersion) }).ToList();
                            }
                        }
                    }
                }

                // process the list of PackageIdentities

                var lowLevelActions = new List<NuGetProjectAction>();

                foreach (var packageIdentity in packageIdentities)
                {
                    var installed = projectInstalledPackageReferences
                        .Where(pr => StringComparer.OrdinalIgnoreCase.Equals(pr.PackageIdentity.Id, packageIdentity.Id))
                        .FirstOrDefault();
                    var autoReferenced = IsPackageReferenceAutoReferenced(installed);

                    //  if the package is not currently installed, or the installed one is auto referenced ignore it
                    if (installed != null && !autoReferenced)
                    {
                        lowLevelActions.Add(NuGetProjectAction.CreateInstallProjectAction(packageIdentity,
                            primarySources.FirstOrDefault(), nuGetProject));
                    }
                }

                if (lowLevelActions.Count > 0)
                {
                    var buildIntegratedProject = nuGetProject as BuildIntegratedNuGetProject;

                    if (buildIntegratedProject != null)
                    {
                        // Create a build integrated action
                        var buildIntegratedAction = await PreviewBuildIntegratedProjectActionsAsync(
                            buildIntegratedProject,
                            lowLevelActions,
                            nuGetProjectContext,
                            token);

                        actions.Add(buildIntegratedAction);
                    }
                    else
                    {
                        // Use the low level actions for projectK
                        actions.AddRange(lowLevelActions);
                    }
                }
            }

            if (actions.Count == 0)
            {
                var projectName = NuGetProject.GetUniqueNameOrName(nuGetProject);
                nuGetProjectContext.Log(MessageLevel.Info, Strings.NoPackageUpdates, projectName);
            }

            return actions;
        }

        private static bool IsPackageReferenceAutoReferenced(PackageReference package)
        {
            var buildPackageReference = package as BuildIntegratedPackageReference;
            return buildPackageReference?.Dependency?.AutoReferenced == true;
        }

        /// <summary>
        /// Update Package logic specific to classic style NuGet projects
        /// </summary>
        private async Task<IEnumerable<NuGetProjectAction>> PreviewUpdatePackagesForClassicAsync(
                string packageId,
                IReadOnlyList<PackageIdentity> packageIdentities,
                NuGetProject nuGetProject,
                ResolutionContext resolutionContext,
                INuGetProjectContext nuGetProjectContext,
                IEnumerable<SourceRepository> primarySources,
                IEnumerable<SourceRepository> secondarySources,
                CancellationToken token)
        {
            var log = new LoggerAdapter(nuGetProjectContext);
            var stopWatch = Stopwatch.StartNew();

            var projectInstalledPackageReferences = await nuGetProject.GetInstalledPackagesAsync(token);
            var oldListOfInstalledPackages = projectInstalledPackageReferences.Select(p => p.PackageIdentity);

            var isUpdateAll = (packageId == null && packageIdentities.Count == 0);

            var preferredVersions = new Dictionary<string, PackageIdentity>(StringComparer.OrdinalIgnoreCase);

            // By default we start by preferring everything we already have installed
            foreach (var installedPackage in oldListOfInstalledPackages)
            {
                preferredVersions[installedPackage.Id] = installedPackage;
            }

            var primaryTargetIds = new List<string>();
            var primaryTargets = new List<PackageIdentity>();

            // We have been given the exact PackageIdentities (id and version) to update to e.g. from PMC update-package -Id <id> -Version <version>
            if (packageIdentities.Count > 0)
            {
                // If we have been given explicit PackageIdentities to install then we will naturally prefer that
                foreach (var packageIdentity in packageIdentities)
                {
                    // Just a check to make sure the preferredVersions created from the existing package list actually contains the target
                    if (preferredVersions.ContainsKey(packageIdentity.Id))
                    {
                        primaryTargetIds.Add(packageIdentity.Id);

                        // If there was a version specified we will prefer that version
                        if (packageIdentity.HasVersion)
                        {
                            preferredVersions[packageIdentity.Id] = packageIdentity;
                            ((List<PackageIdentity>)primaryTargets).Add(packageIdentity);
                        }
                        // Otherwise we just have the Id and so we wil explicitly not prefer the one currently installed
                        else
                        {
                            preferredVersions.Remove(packageIdentity.Id);
                        }
                    }
                }
            }
            // We have just been given the package id, in which case we will look for the highest version and attempt to move to that
            else if (packageId != null)
            {
                if (preferredVersions.ContainsKey(packageId))
                {
                    if (PrunePackageTree.IsExactVersion(resolutionContext.VersionConstraints))
                    {
                        primaryTargets = new List<PackageIdentity> { preferredVersions[packageId] };
                    }
                    else
                    {
                        primaryTargetIds = new List<string> { packageId };

                        // If we have been given just a package Id we certainly don't want the one installed - pruning will be significant
                        preferredVersions.Remove(packageId);
                    }
                }
                else
                {
                    // This is the scenario where a specific package is being updated in a project with a -reinstall flag and the -projectname has not been specified.
                    // In this case, NuGet should bail out if the package does not exist in this project instead of re-installing all packages of the project.
                    // Bug: https://github.com/NuGet/Home/issues/737
                    return Enumerable.Empty<NuGetProjectAction>();
                }
            }
            // We are apply update logic to the complete project - attempting to resolver all updates together
            else
            {
                primaryTargetIds = projectInstalledPackageReferences.Select(p => p.PackageIdentity.Id).ToList();

                // We are performing a global project-wide update - nothing is preferred - again pruning will be significant
                preferredVersions.Clear();
            }

            // Note: resolver needs all the installed packages as targets too. And, metadata should be gathered for the installed packages as well
            var packageTargetIdsForResolver = new HashSet<string>(oldListOfInstalledPackages.Select(p => p.Id), StringComparer.OrdinalIgnoreCase);
            foreach (var packageIdToInstall in primaryTargetIds)
            {
                packageTargetIdsForResolver.Add(packageIdToInstall);
            }

            var projectName = NuGetProject.GetUniqueNameOrName(nuGetProject);
            var nuGetProjectActions = new List<NuGetProjectAction>();
            if (!packageTargetIdsForResolver.Any())
            {
                nuGetProjectContext.Log(NuGet.ProjectManagement.MessageLevel.Info, Strings.NoPackagesInProject, projectName);
                return nuGetProjectActions;
            }

            try
            {
                // If any targets are prerelease we should gather with prerelease on and filter afterwards
                var includePrereleaseInGather = resolutionContext.IncludePrerelease || (projectInstalledPackageReferences.Any(p => (p.PackageIdentity.HasVersion && p.PackageIdentity.Version.IsPrerelease)));

                // Create a modified resolution cache. This should include the same gather cache for multi-project
                // operations.
                var contextForGather = new ResolutionContext(
                    resolutionContext.DependencyBehavior,
                    includePrereleaseInGather,
                    resolutionContext.IncludeUnlisted,
                    VersionConstraints.None,
                    resolutionContext.GatherCache,
                    resolutionContext.SourceCacheContext);

                // Step-1 : Get metadata resources using gatherer
                var targetFramework = nuGetProject.GetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework);
                nuGetProjectContext.Log(MessageLevel.Info, Environment.NewLine);
                nuGetProjectContext.Log(MessageLevel.Info, Strings.AttemptingToGatherDependencyInfoForMultiplePackages, projectName, targetFramework);

                var allSources = new List<SourceRepository>(primarySources);
                var primarySourcesSet = new HashSet<string>(primarySources.Select(s => s.PackageSource.Source));
                foreach (var secondarySource in secondarySources)
                {
                    if (!primarySourcesSet.Contains(secondarySource.PackageSource.Source))
                    {
                        allSources.Add(secondarySource);
                    }
                }

                foreach (SourceRepository enabledSource in allSources)
                {
                    PackageSource source = enabledSource.PackageSource;
                    if (source.IsHttp && !source.IsHttps && !source.AllowInsecureConnections)
                    {
                        nuGetProjectContext.Log(MessageLevel.Warning, Strings.Warning_HttpServerUsage, "update", source.Source);
                    }
                }

                // Unless the packageIdentity was explicitly asked for we should remove any potential downgrades
                var allowDowngrades = false;
                if (packageIdentities.Count == 1)
                {
                    // Get installed package version
                    var packageTargetsForResolver = new HashSet<PackageIdentity>(oldListOfInstalledPackages, PackageIdentity.Comparer);
                    var installedPackageWithSameId = packageTargetsForResolver.Where(p => p.Id.Equals(packageIdentities[0].Id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (installedPackageWithSameId != null)
                    {
                        if (installedPackageWithSameId.Version > packageIdentities[0].Version)
                        {
                            // Looks like the installed package is of higher version than one being installed. So, we take it that downgrade is allowed
                            allowDowngrades = true;
                        }
                    }
                }

                var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(Settings);

                var gatherContext = new GatherContext(packageSourceMapping)
                {
                    InstalledPackages = oldListOfInstalledPackages.ToList(),
                    PrimaryTargetIds = primaryTargetIds.ToList(),
                    PrimaryTargets = primaryTargets.ToList(),
                    TargetFramework = targetFramework,
                    PrimarySources = primarySources.ToList(),
                    AllSources = allSources.ToList(),
                    PackagesFolderSource = PackagesFolderSourceRepository,
                    ResolutionContext = resolutionContext,
                    AllowDowngrades = allowDowngrades,
                    ProjectContext = nuGetProjectContext,
                    IsUpdateAll = isUpdateAll
                };

                var availablePackageDependencyInfoWithSourceSet = await ResolverGather.GatherAsync(gatherContext, token);

                // emit gather dependency telemetry event and restart timer
                stopWatch.Stop();

                var gatherTelemetryEvent = new ActionTelemetryStepEvent(
                    nuGetProjectContext.OperationId.ToString(),
                    TelemetryConstants.GatherDependencyStepName,
                    stopWatch.Elapsed.TotalSeconds);

                TelemetryActivity.EmitTelemetryEvent(gatherTelemetryEvent);
                stopWatch.Restart();

                if (!availablePackageDependencyInfoWithSourceSet.Any())
                {
                    throw new InvalidOperationException(Strings.UnableToGatherDependencyInfoForMultiplePackages);
                }

                // Update-Package ALL packages scenarios must always include the packages in the current project
                // Scenarios include: (1) a package havign been deleted from a feed (2) a source being removed from nuget config (3) an explicitly specified source
                if (isUpdateAll)
                {
                    // BUG #1181 VS2015 : Updating from one feed fails for packages from different feed.

                    var packagesFolderResource = await PackagesFolderSourceRepository.GetResourceAsync<DependencyInfoResource>(token);
                    var packages = new List<SourcePackageDependencyInfo>();
                    foreach (var installedPackage in projectInstalledPackageReferences)
                    {
                        var packageInfo = await packagesFolderResource.ResolvePackage(installedPackage.PackageIdentity, targetFramework, resolutionContext.SourceCacheContext, log, token);
                        if (packageInfo != null)
                        {
                            availablePackageDependencyInfoWithSourceSet.Add(packageInfo);
                        }
                    }
                }

                // Prune the results down to only what we would allow to be installed
                IEnumerable<SourcePackageDependencyInfo> prunedAvailablePackages = availablePackageDependencyInfoWithSourceSet;

                if (!resolutionContext.IncludePrerelease)
                {
                    prunedAvailablePackages = PrunePackageTree.PrunePrereleaseExceptAllowed(
                        prunedAvailablePackages,
                        oldListOfInstalledPackages,
                        isUpdateAll);
                }

                // Prune unlisted versions if IncludeUnlisted flag is not set.
                if (!resolutionContext.IncludeUnlisted)
                {
                    prunedAvailablePackages = prunedAvailablePackages.Where(p => p.Listed);
                }

                // Remove packages that do not meet the constraints specified in the UpdateConstrainst
                prunedAvailablePackages = PrunePackageTree.PruneByUpdateConstraints(prunedAvailablePackages, projectInstalledPackageReferences, resolutionContext.VersionConstraints);

                // Verify that the target is allowed by packages.config
                GatherExceptionHelpers.ThrowIfVersionIsDisallowedByPackagesConfig(primaryTargetIds, projectInstalledPackageReferences, prunedAvailablePackages, log);

                // Remove versions that do not satisfy 'allowedVersions' attribute in packages.config, if any
                prunedAvailablePackages = PrunePackageTree.PruneDisallowedVersions(prunedAvailablePackages, projectInstalledPackageReferences);

                // Remove all but the highest packages that are of the same Id as a specified packageId
                if (packageId != null)
                {
                    prunedAvailablePackages = PrunePackageTree.PruneAllButHighest(prunedAvailablePackages, packageId);

                    // And then verify that the installed package is not already of a higher version - this check here ensures the user get's the right error message
                    GatherExceptionHelpers.ThrowIfNewerVersionAlreadyReferenced(packageId, projectInstalledPackageReferences, prunedAvailablePackages);
                }

                // Remove packages that are of the same Id but different version than the primartTargets
                prunedAvailablePackages = PrunePackageTree.PruneByPrimaryTargets(prunedAvailablePackages, primaryTargets);

                // Unless the packageIdentity was explicitly asked for we should remove any potential downgrades
                if (!allowDowngrades)
                {
                    prunedAvailablePackages = PrunePackageTree.PruneDowngrades(prunedAvailablePackages, projectInstalledPackageReferences);
                }

                // Step-2 : Call PackageResolver.Resolve to get new list of installed packages
                var packageResolver = new PackageResolver();
                var packageResolverContext = new PackageResolverContext(
                    resolutionContext.DependencyBehavior,
                    primaryTargetIds,
                    packageTargetIdsForResolver,
                    projectInstalledPackageReferences,
                    preferredVersions.Values,
                    prunedAvailablePackages,
                    SourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                    log);

                nuGetProjectContext.Log(NuGet.ProjectManagement.MessageLevel.Info, Strings.AttemptingToResolveDependenciesForMultiplePackages);
                var newListOfInstalledPackages = packageResolver.Resolve(packageResolverContext, token);

                // emit resolve dependency telemetry event and restart timer
                stopWatch.Stop();

                var resolveTelemetryEvent = new ActionTelemetryStepEvent(
                    nuGetProjectContext.OperationId.ToString(),
                    TelemetryConstants.ResolveDependencyStepName,
                    stopWatch.Elapsed.TotalSeconds);

                TelemetryActivity.EmitTelemetryEvent(resolveTelemetryEvent);
                stopWatch.Restart();

                if (newListOfInstalledPackages == null)
                {
                    throw new InvalidOperationException(Strings.UnableToResolveDependencyInfoForMultiplePackages);
                }

                // if we have been asked for exact versions of packages then we should also force the uninstall/install of those packages (this corresponds to a -Reinstall)
                var isReinstall = PrunePackageTree.IsExactVersion(resolutionContext.VersionConstraints);

                var targetIds = Enumerable.Empty<string>();
                if (!isUpdateAll)
                {
                    targetIds = (isReinstall ? primaryTargets.Select(p => p.Id) : primaryTargetIds);
                }

                var installedPackagesInDependencyOrder = await GetInstalledPackagesInDependencyOrder(nuGetProject, token);

                var isDependencyBehaviorIgnore = resolutionContext.DependencyBehavior == DependencyBehavior.Ignore;

                nuGetProjectActions = GetProjectActionsForUpdate(
                    nuGetProject,
                    newListOfInstalledPackages,
                    installedPackagesInDependencyOrder,
                    prunedAvailablePackages,
                    nuGetProjectContext,
                    isReinstall,
                    targetIds,
                    isDependencyBehaviorIgnore);

                // emit resolve actions telemetry event
                stopWatch.Stop();

                var actionTelemetryEvent = new ActionTelemetryStepEvent(
                    nuGetProjectContext.OperationId.ToString(),
                    TelemetryConstants.ResolvedActionsStepName,
                    stopWatch.Elapsed.TotalSeconds);

                TelemetryActivity.EmitTelemetryEvent(actionTelemetryEvent);

                if (nuGetProjectActions.Count == 0)
                {
                    nuGetProjectContext.Log(MessageLevel.Info, Strings.ResolutionSuccessfulNoAction);
                    nuGetProjectContext.Log(MessageLevel.Info, Strings.NoUpdatesAvailable);
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (AggregateException aggregateEx)
            {
                throw new InvalidOperationException(aggregateEx.Message, aggregateEx);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(ex.Message))
                {
                    throw new InvalidOperationException(Strings.PackagesCouldNotBeInstalled, ex);
                }
                throw new InvalidOperationException(ex.Message, ex);
            }

            if (nuGetProjectActions.Count == 0)
            {
                nuGetProjectContext.Log(MessageLevel.Info, Strings.NoPackageUpdates, projectName);
            }

            return nuGetProjectActions;
        }

        /// <summary>
        /// The package dependency info for the given project. The project needs to be packages.config, otherwise returns an empty list.
        /// </summary>
        /// <param name="nuGetProject">The project is question</param>
        /// <param name="token">cancellation token</param>
        /// <param name="includeUnresolved">Whether to include the unresolved packages. The unresolved packages include packages that are not restored and cannot be found on disk.</param>
        /// <returns></returns>
        public async Task<IEnumerable<PackageDependencyInfo>> GetInstalledPackagesDependencyInfo(NuGetProject nuGetProject, CancellationToken token, bool includeUnresolved = false)
        {
            if (nuGetProject is MSBuildNuGetProject)
            {
                var targetFramework = nuGetProject.GetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework);
                var installedPackageIdentities = (await nuGetProject.GetInstalledPackagesAsync(token)).Select(pr => pr.PackageIdentity);
                return await GetDependencyInfoFromPackagesFolderAsync(installedPackageIdentities, targetFramework, includeUnresolved);
            }
            else
            {
                return Enumerable.Empty<PackageDependencyInfo>();
            }
        }

        /// <summary>
        /// Returns all installed packages in order of dependency. Packages with no dependencies come first.
        /// </summary>
        /// <remarks>Packages with unresolved dependencies are NOT returned since they are not valid.</remarks>
        public async Task<IEnumerable<PackageIdentity>> GetInstalledPackagesInDependencyOrder(NuGetProject nuGetProject,
            CancellationToken token)
        {
            var targetFramework = nuGetProject.GetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework);
            var installedPackages = await nuGetProject.GetInstalledPackagesAsync(token);
            var installedPackageIdentities = installedPackages.Select(pr => pr.PackageIdentity);
            var dependencyInfoFromPackagesFolder = await GetDependencyInfoFromPackagesFolderAsync(installedPackageIdentities,
                targetFramework);

            // dependencyInfoFromPackagesFolder can be null when NuGetProtocolException is thrown
            var resolverPackages = dependencyInfoFromPackagesFolder?.Select(package =>
                    new ResolverPackage(package.Id, package.Version, package.Dependencies, true, false));

            // Use the resolver sort to find the order. Packages with no dependencies
            // come first, then each package that has satisfied dependencies.
            // Packages with missing dependencies will not be returned.
            if (resolverPackages != null)
            {
                return ResolverUtility.TopologicalSort(resolverPackages);
            }

            return Enumerable.Empty<PackageIdentity>();
        }

        private static List<NuGetProjectAction> GetProjectActionsForUpdate(
            NuGetProject project,
            IEnumerable<PackageIdentity> newListOfInstalledPackages,
            IEnumerable<PackageIdentity> oldListOfInstalledPackages,
            IEnumerable<SourcePackageDependencyInfo> availablePackageDependencyInfoWithSourceSet,
            INuGetProjectContext nuGetProjectContext,
            bool isReinstall,
            IEnumerable<string> targetIds,
            bool isDependencyBehaviorIgnore)
        {
            // Step-3 : Get the list of nuGetProjectActions to perform, install/uninstall on the nugetproject
            // based on newPackages obtained in Step-2 and project.GetInstalledPackages
            var nuGetProjectActions = new List<NuGetProjectAction>();
            nuGetProjectContext.Log(MessageLevel.Info, Strings.ResolvingActionsToInstallOrUpdateMultiplePackages);

            // we are reinstalling everything so we just take the ordering directly from the Resolver
            var newPackagesToUninstall = oldListOfInstalledPackages;
            var newPackagesToInstall = newListOfInstalledPackages;

            // we are doing a reinstall of a specific package - we will also want to generate Project Actions for the dependencies
            if (isReinstall && targetIds.Any())
            {
                var packageIdsToReinstall = new HashSet<string>(targetIds, StringComparer.OrdinalIgnoreCase);

                // Avoid getting dependencies if dependencyBehavior is set to ignore
                if (!isDependencyBehaviorIgnore)
                {
                    packageIdsToReinstall = GetDependencies(targetIds, newListOfInstalledPackages, availablePackageDependencyInfoWithSourceSet);
                }

                newPackagesToUninstall = oldListOfInstalledPackages.Where(p => packageIdsToReinstall.Contains(p.Id));
                newPackagesToInstall = newListOfInstalledPackages.Where(p => packageIdsToReinstall.Contains(p.Id));
            }

            if (!isReinstall)
            {
                if (targetIds.Any())
                {
                    // we are targeting a particular package - there is no need therefore to alter other aspects of the project
                    // specifically an unrelated package may have been force removed in which case we should be happy to leave things that way

                    // It will get the list of packages which are being uninstalled to get a new version
                    newPackagesToUninstall = oldListOfInstalledPackages.Where(oldPackage =>
                        newListOfInstalledPackages.Any(newPackage =>
                        StringComparer.OrdinalIgnoreCase.Equals(oldPackage.Id, newPackage.Id) && !oldPackage.Version.Equals(newPackage.Version)));

                    // this will be the new set of target ids which includes current target ids as well as packages which are being updated
                    //It fixes the issue where we were only getting dependencies for target ids ignoring other packages which are also being updated. #2724
                    var newTargetIds = new HashSet<string>(newPackagesToUninstall.Select(p => p.Id), StringComparer.OrdinalIgnoreCase);
                    newTargetIds.AddRange(targetIds);

                    var allowed = newTargetIds;

                    // Avoid getting dependencies if dependencyBehavior is set to ignore
                    if (!isDependencyBehaviorIgnore)
                    {
                        // first, we will allow all the dependencies of the package(s) beging targeted
                        allowed = GetDependencies(newTargetIds, newListOfInstalledPackages, availablePackageDependencyInfoWithSourceSet);
                    }

                    // second, any package that is currently in the solution will also be allowed to change
                    // (note this logically doesn't include packages that have been force uninstalled from the project
                    // because we wouldn't want to just add those back in)
                    foreach (var p in oldListOfInstalledPackages)
                    {
                        allowed.Add(p.Id);
                    }

                    newListOfInstalledPackages = newListOfInstalledPackages.Where(p => allowed.Contains(p.Id));
                    newPackagesToInstall = newListOfInstalledPackages.Where(p => !oldListOfInstalledPackages.Contains(p));
                }
                else
                {
                    newPackagesToUninstall = oldListOfInstalledPackages.Where(p => !newListOfInstalledPackages.Contains(p));
                    newPackagesToInstall = newListOfInstalledPackages.Where(p => !oldListOfInstalledPackages.Contains(p));
                }
            }

            foreach (var newPackageToUninstall in newPackagesToUninstall.Reverse())
            {
                nuGetProjectActions.Add(NuGetProjectAction.CreateUninstallProjectAction(newPackageToUninstall, project));
            }

            foreach (var newPackageToInstall in newPackagesToInstall)
            {
                // find the package match based on identity
                var sourceDepInfo = availablePackageDependencyInfoWithSourceSet.Where(p => PackageIdentity.Comparer.Equals(p, newPackageToInstall)).SingleOrDefault();

                if (sourceDepInfo == null)
                {
                    // this really should never happen
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.PackageNotFound, newPackageToInstall));
                }

                nuGetProjectActions.Add(NuGetProjectAction.CreateInstallProjectAction(newPackageToInstall, sourceDepInfo.Source, project));
            }

            return nuGetProjectActions;
        }

        /// <summary>
        /// Filter down the reinstall list to just the ones we need to reinstall (i.e. the dependencies)
        /// </summary>
        private static HashSet<string> GetDependencies(IEnumerable<string> targetIds, IEnumerable<PackageIdentity> newListOfInstalledPackages, IEnumerable<SourcePackageDependencyInfo> available)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var targetId in targetIds)
            {
                CollectDependencies(result, targetId, newListOfInstalledPackages, available, 0);
            }
            return result;
        }

        /// <summary>
        /// A walk through the dependencies to collect the additional package identities that are involved in the current set of packages to be installed
        /// </summary>
        private static void CollectDependencies(HashSet<string> result,
            string id,
            IEnumerable<PackageIdentity> packages,
            IEnumerable<SourcePackageDependencyInfo> available, int depth)
        {
            // check if we've already added dependencies for current id
            if (!result.Add(id))
            {
                return;
            }

            // we want the exact PackageIdentity for this id
            var packageIdentity = packages.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (packageIdentity == null)
            {
                throw new ArgumentException(
                    message: string.Format(CultureInfo.CurrentCulture, Strings.PackageNotFound, id),
                    paramName: nameof(packages));
            }

            // now look up the dependencies of this exact package identity
            var sourceDepInfo = available.SingleOrDefault(p => packageIdentity.Equals(p));
            if (sourceDepInfo == null)
            {
                throw new ArgumentException(
                    message: string.Format(CultureInfo.CurrentCulture, Strings.PackageNotFound, id),
                    paramName: nameof(available));
            }

            // iterate through all the dependencies and call recursively to collect dependencies
            foreach (var dependency in sourceDepInfo.Dependencies)
            {
                // check we don't fall into an infinite loop caused by bad dependency data in the packages
                if (depth < packages.Count())
                {
                    CollectDependencies(result, dependency.Id, packages, available, depth + 1);
                }
            }
        }

        /// <summary>
        /// Gives the preview as a list of NuGetProjectActions that will be performed to install
        /// <paramref name="packageIdentity" /> into <paramref name="nuGetProject" />
        /// <paramref name="resolutionContext" /> and <paramref name="nuGetProjectContext" /> are used in the process.
        /// </summary>
        public async Task<IEnumerable<NuGetProjectAction>> PreviewInstallPackageAsync(NuGetProject nuGetProject, PackageIdentity packageIdentity,
            ResolutionContext resolutionContext, INuGetProjectContext nuGetProjectContext,
            SourceRepository primarySourceRepository, IEnumerable<SourceRepository> secondarySources, CancellationToken token)
        {
            if (nuGetProject is INuGetIntegratedProject)
            {
                var action = NuGetProjectAction.CreateInstallProjectAction(packageIdentity, primarySourceRepository, nuGetProject);
                var actions = new[] { action };

                var buildIntegratedProject = nuGetProject as BuildIntegratedNuGetProject;

                if (buildIntegratedProject != null)
                {
                    actions = new[] {
                        await PreviewBuildIntegratedProjectActionsAsync(buildIntegratedProject, actions, nuGetProjectContext, token)
                    };
                }

                return actions;
            }

            var primarySources = new List<SourceRepository> { primarySourceRepository };
            return await PreviewInstallPackageAsync(nuGetProject, packageIdentity, resolutionContext,
                nuGetProjectContext, primarySources, secondarySources, token);
        }

        public async Task<IEnumerable<ResolvedAction>> PreviewProjectsInstallPackageAsync(
            IReadOnlyCollection<NuGetProject> nuGetProjects,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IReadOnlyCollection<SourceRepository> activeSources,
            CancellationToken token)
        {
            return await PreviewProjectsInstallPackageAsync(nuGetProjects, packageIdentity, resolutionContext, nuGetProjectContext, activeSources, versionRange: null, token);
        }

        public async Task<IEnumerable<ResolvedAction>> PreviewProjectsInstallPackageAsync(
            IReadOnlyCollection<NuGetProject> nuGetProjects,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IReadOnlyCollection<SourceRepository> activeSources,
            VersionRange versionRange,
            CancellationToken token)
        {
            return await PreviewProjectsInstallPackageAsync(
                nuGetProjects,
                packageIdentity,
                resolutionContext,
                nuGetProjectContext,
                activeSources,
                versionRange,
                newMappingID: null,
                newMappingSource: null,
                token);
        }

        // Preview and return ResolvedActions for many NuGetProjects.
        public async Task<IEnumerable<ResolvedAction>> PreviewProjectsInstallPackageAsync(
            IReadOnlyCollection<NuGetProject> nuGetProjects,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IReadOnlyCollection<SourceRepository> activeSources,
            VersionRange versionRange,
            string newMappingID,
            string newMappingSource,
            CancellationToken token)
        {
            if (nuGetProjects == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjects));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (resolutionContext == null)
            {
                throw new ArgumentNullException(nameof(resolutionContext));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            if (activeSources == null)
            {
                throw new ArgumentNullException(nameof(activeSources));
            }

            if (activeSources.Count == 0)
            {
                throw new ArgumentException("At least 1 item expected for " + nameof(activeSources));
            }

            if (packageIdentity.Version == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            var results = new List<ResolvedAction>();

            // BuildIntegratedNuGetProject type projects are now supports parallel preview action for faster performance.
            var buildIntegratedProjectsToUpdate = new List<BuildIntegratedNuGetProject>();
            // Currently packages.config projects are not supported are supports parallel previews.
            // Here is follow up issue to address it https://github.com/NuGet/Home/issues/9906
            var otherTargetProjectsToUpdate = new List<NuGetProject>();

            foreach (var proj in nuGetProjects)
            {
                if (proj is BuildIntegratedNuGetProject buildIntegratedNuGetProject)
                {
                    buildIntegratedProjectsToUpdate.Add(buildIntegratedNuGetProject);
                }
                else
                {
                    otherTargetProjectsToUpdate.Add(proj);
                }
            }

            if (buildIntegratedProjectsToUpdate.Count != 0)
            {
                // Only automatically create Source Mappings when there's exclusively BuildIntegratedProjects.
                if (otherTargetProjectsToUpdate.Count > 0)
                {
                    newMappingID = null;
                    newMappingSource = null;
                }

                // Run build integrated project preview for all projects at the same time
                var resolvedActions = await PreviewBuildIntegratedProjectsActionsAsync(
                    buildIntegratedProjectsToUpdate,
                    nugetProjectActionsLookup: null, // no nugetProjectActionsLookup so it'll be derived from packageIdentity and activeSources
                    packageIdentity,
                    activeSources,
                    nuGetProjectContext,
                    versionRange,
                    newMappingID,
                    newMappingSource,
                    token);
                results.AddRange(resolvedActions);
            }

            foreach (var target in otherTargetProjectsToUpdate)
            {
                var actions = await PreviewInstallPackageAsync(
                    target,
                    packageIdentity,
                    resolutionContext,
                    nuGetProjectContext,
                    activeSources,
                    null,
                    token);
                results.AddRange(actions.Select(a => new ResolvedAction(target, a)));
            }

            return results;
        }

        public async Task<IEnumerable<NuGetProjectAction>> PreviewInstallPackageAsync(
            NuGetProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            CancellationToken token)
        {
            return await PreviewInstallPackageAsync(nuGetProject, packageIdentity, resolutionContext, nuGetProjectContext, primarySources, secondarySources, versionRange: null, token);
        }

        public async Task<IEnumerable<NuGetProjectAction>> PreviewInstallPackageAsync(
            NuGetProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> primarySources,
            IEnumerable<SourceRepository> secondarySources,
            VersionRange versionRange,
            CancellationToken token)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (resolutionContext == null)
            {
                throw new ArgumentNullException(nameof(resolutionContext));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            if (primarySources == null)
            {
                throw new ArgumentNullException(nameof(primarySources));
            }

            if (secondarySources == null)
            {
                secondarySources = SourceRepositoryProvider.GetRepositories().Where(e => e.PackageSource.IsEnabled);
            }

            if (!primarySources.Any())
            {
                throw new ArgumentException(
                    message: Strings.Argument_Cannot_Be_Null_Or_Empty,
                    paramName: nameof(primarySources));
            }

            if (packageIdentity.Version == null)
            {
                throw new ArgumentException(
                    message: string.Format(CultureInfo.CurrentCulture, Strings.PropertyCannotBeNull, nameof(packageIdentity.Version)),
                    paramName: nameof(packageIdentity));
            }

            if (nuGetProject is INuGetIntegratedProject)
            {
                var action = NuGetProjectAction.CreateInstallProjectAction(packageIdentity, primarySources.First(), nuGetProject, versionRange);
                var actions = new[] { action };

                var buildIntegratedProject = nuGetProject as BuildIntegratedNuGetProject;

                if (buildIntegratedProject != null)
                {
                    actions = new[] {
                        await PreviewBuildIntegratedProjectActionsAsync(buildIntegratedProject, actions, nuGetProjectContext, token)
                    };
                }

                return actions;
            }

            var projectName = NuGetProject.GetUniqueNameOrName(nuGetProject);
            var stopWatch = Stopwatch.StartNew();

            var projectInstalledPackageReferences = await nuGetProject.GetInstalledPackagesAsync(token);
            var oldListOfInstalledPackages = projectInstalledPackageReferences.Select(p => p.PackageIdentity);
            if (oldListOfInstalledPackages.Any(p => p.Equals(packageIdentity)))
            {
                var alreadyInstalledMessage = string.Format(CultureInfo.CurrentCulture, Strings.PackageAlreadyExistsInProject, packageIdentity, projectName ?? string.Empty);
                throw new InvalidOperationException(alreadyInstalledMessage, new PackageAlreadyInstalledException(alreadyInstalledMessage));
            }

            var nuGetProjectActions = new List<NuGetProjectAction>();

            var effectiveSources = GetEffectiveSources(primarySources, secondarySources);

            foreach (SourceRepository enabledSource in effectiveSources)
            {
                PackageSource source = enabledSource.PackageSource;
                if (source.IsHttp && !source.IsHttps && !source.AllowInsecureConnections)
                {
                    nuGetProjectContext.Log(MessageLevel.Warning, Strings.Warning_HttpServerUsage, "install", source.Source);
                }
            }

            if (resolutionContext.DependencyBehavior != DependencyBehavior.Ignore)
            {
                try
                {
                    var downgradeAllowed = false;
                    var packageTargetsForResolver = new HashSet<PackageIdentity>(oldListOfInstalledPackages, PackageIdentity.Comparer);
                    // Note: resolver needs all the installed packages as targets too. And, metadata should be gathered for the installed packages as well
                    var installedPackageWithSameId = packageTargetsForResolver.Where(p => p.Id.Equals(packageIdentity.Id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (installedPackageWithSameId != null)
                    {
                        packageTargetsForResolver.Remove(installedPackageWithSameId);
                        if (installedPackageWithSameId.Version > packageIdentity.Version)
                        {
                            // Looks like the installed package is of higher version than one being installed. So, we take it that downgrade is allowed
                            downgradeAllowed = true;
                        }
                    }
                    packageTargetsForResolver.Add(packageIdentity);

                    // Step-1 : Get metadata resources using gatherer
                    var targetFramework = nuGetProject.GetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework);
                    nuGetProjectContext.Log(NuGet.ProjectManagement.MessageLevel.Info, Environment.NewLine);
                    nuGetProjectContext.Log(ProjectManagement.MessageLevel.Info, Strings.AttemptingToGatherDependencyInfo, packageIdentity, projectName, targetFramework);

                    var primaryPackages = new List<PackageIdentity> { packageIdentity };

                    HashSet<SourcePackageDependencyInfo> availablePackageDependencyInfoWithSourceSet = null;
                    var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(Settings);

                    var gatherContext = new GatherContext(packageSourceMapping)
                    {
                        InstalledPackages = oldListOfInstalledPackages.ToList(),
                        PrimaryTargets = primaryPackages,
                        TargetFramework = targetFramework,
                        PrimarySources = primarySources.ToList(),
                        AllSources = effectiveSources.ToList(),
                        PackagesFolderSource = PackagesFolderSourceRepository,
                        ResolutionContext = resolutionContext,
                        AllowDowngrades = downgradeAllowed,
                        ProjectContext = nuGetProjectContext
                    };

                    availablePackageDependencyInfoWithSourceSet = await ResolverGather.GatherAsync(gatherContext, token);

                    // emit gather dependency telemetry event and restart timer
                    stopWatch.Stop();
                    var gatherTelemetryEvent = new ActionTelemetryStepEvent(
                        nuGetProjectContext.OperationId.ToString(),
                        TelemetryConstants.GatherDependencyStepName,
                        stopWatch.Elapsed.TotalSeconds);

                    TelemetryActivity.EmitTelemetryEvent(gatherTelemetryEvent);

                    stopWatch.Restart();

                    if (!availablePackageDependencyInfoWithSourceSet.Any())
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.UnableToGatherDependencyInfo, packageIdentity));
                    }

                    // Prune the results down to only what we would allow to be installed

                    // Keep only the target package we are trying to install for that Id
                    var prunedAvailablePackages = PrunePackageTree.RemoveAllVersionsForIdExcept(availablePackageDependencyInfoWithSourceSet, packageIdentity);

                    if (!downgradeAllowed)
                    {
                        prunedAvailablePackages = PrunePackageTree.PruneDowngrades(prunedAvailablePackages, projectInstalledPackageReferences);
                    }

                    if (!resolutionContext.IncludePrerelease)
                    {
                        prunedAvailablePackages = PrunePackageTree.PrunePreleaseForStableTargets(
                            prunedAvailablePackages,
                            packageTargetsForResolver,
                            new[] { packageIdentity });
                    }

                    var log = new LoggerAdapter(nuGetProjectContext);

                    // Verify that the target is allowed by packages.config
                    GatherExceptionHelpers.ThrowIfVersionIsDisallowedByPackagesConfig(packageIdentity.Id, projectInstalledPackageReferences, prunedAvailablePackages, log);

                    // Remove versions that do not satisfy 'allowedVersions' attribute in packages.config, if any
                    prunedAvailablePackages = PrunePackageTree.PruneDisallowedVersions(prunedAvailablePackages, projectInstalledPackageReferences);

                    // Step-2 : Call PackageResolver.Resolve to get new list of installed packages

                    // Note: resolver prefers installed package versions if the satisfy the dependency version constraints
                    // So, since we want an exact version of a package, create a new list of installed packages where the packageIdentity being installed
                    // is present after removing the one with the same id
                    var preferredPackageReferences = new List<PackageReference>(projectInstalledPackageReferences.Where(pr =>
                        !pr.PackageIdentity.Id.Equals(packageIdentity.Id, StringComparison.OrdinalIgnoreCase)))
                    {
                        new PackageReference(packageIdentity, targetFramework)
                    };

                    var packageResolverContext = new PackageResolverContext(resolutionContext.DependencyBehavior,
                        new string[] { packageIdentity.Id },
                        oldListOfInstalledPackages.Select(package => package.Id),
                        projectInstalledPackageReferences,
                        preferredPackageReferences.Select(package => package.PackageIdentity),
                        prunedAvailablePackages,
                        SourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                        log);

                    nuGetProjectContext.Log(MessageLevel.Info, Strings.AttemptingToResolveDependencies, packageIdentity, resolutionContext.DependencyBehavior);

                    var packageResolver = new PackageResolver();

                    var newListOfInstalledPackages = packageResolver.Resolve(packageResolverContext, token);

                    // emit resolve dependency telemetry event and restart timer
                    stopWatch.Stop();

                    var resolveTelemetryEvent = new ActionTelemetryStepEvent(
                        nuGetProjectContext.OperationId.ToString(),
                        TelemetryConstants.ResolveDependencyStepName,
                        stopWatch.Elapsed.TotalSeconds);

                    TelemetryActivity.EmitTelemetryEvent(resolveTelemetryEvent);

                    stopWatch.Restart();

                    if (newListOfInstalledPackages == null)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.UnableToResolveDependencyInfo, packageIdentity, resolutionContext.DependencyBehavior));
                    }

                    // Step-3 : Get the list of nuGetProjectActions to perform, install/uninstall on the nugetproject
                    // based on newPackages obtained in Step-2 and project.GetInstalledPackages

                    nuGetProjectContext.Log(MessageLevel.Info, Strings.ResolvingActionsToInstallPackage, packageIdentity);
                    var newPackagesToUninstall = new List<PackageIdentity>();
                    foreach (var oldInstalledPackage in oldListOfInstalledPackages)
                    {
                        var newPackageWithSameId = newListOfInstalledPackages
                            .FirstOrDefault(np =>
                                oldInstalledPackage.Id.Equals(np.Id, StringComparison.OrdinalIgnoreCase) &&
                                !oldInstalledPackage.Version.Equals(np.Version));

                        if (newPackageWithSameId != null)
                        {
                            newPackagesToUninstall.Add(oldInstalledPackage);
                        }
                    }
                    var newPackagesToInstall = newListOfInstalledPackages.Where(p => !oldListOfInstalledPackages.Contains(p));

                    foreach (var newPackageToUninstall in newPackagesToUninstall)
                    {
                        nuGetProjectActions.Add(NuGetProjectAction.CreateUninstallProjectAction(newPackageToUninstall, nuGetProject));
                    }

                    // created hashset of packageIds we are OK with touching
                    // the scenario here is that the user might have done an uninstall-package -Force on a particular package

                    // this will be the new set of target ids which includes current target ids as well as packages which are being updated
                    //It fixes the issue where we were only getting dependencies for target ids ignoring other packages which are also being updated. #2724
                    var newTargetIds = new HashSet<string>(newPackagesToUninstall.Select(p => p.Id), StringComparer.OrdinalIgnoreCase);
                    newTargetIds.Add(packageIdentity.Id);

                    // get all dependencies of new target ids so that we can have all the required install actions.
                    var allowed = GetDependencies(newTargetIds, newListOfInstalledPackages, prunedAvailablePackages);

                    foreach (var newPackageToInstall in newPackagesToInstall)
                    {
                        // we should limit actions to just packages that are in the dependency set of the target we are installing
                        if (allowed.Contains(newPackageToInstall.Id))
                        {
                            // find the package match based on identity
                            var sourceDepInfo = prunedAvailablePackages.SingleOrDefault(p => PackageIdentity.Comparer.Equals(p, newPackageToInstall));

                            if (sourceDepInfo == null)
                            {
                                // this really should never happen
                                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.PackageNotFound, packageIdentity));
                            }

                            nuGetProjectActions.Add(NuGetProjectAction.CreateInstallProjectAction(sourceDepInfo, sourceDepInfo.Source, nuGetProject));
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                catch (AggregateException aggregateEx)
                {
                    throw new InvalidOperationException(aggregateEx.Message, aggregateEx);
                }
                catch (Exception ex)
                {
                    if (string.IsNullOrEmpty(ex.Message))
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.PackageCouldNotBeInstalled, packageIdentity), ex);
                    }
                    throw new InvalidOperationException(ex.Message, ex);
                }
            }
            else
            {
                var logger = new ProjectContextLogger(nuGetProjectContext);
                var sourceRepository = await GetSourceRepository(packageIdentity, effectiveSources, resolutionContext.SourceCacheContext, logger);
                nuGetProjectActions.Add(NuGetProjectAction.CreateInstallProjectAction(packageIdentity, sourceRepository, nuGetProject));
            }

            // emit resolve actions telemetry event
            stopWatch.Stop();

            var actionTelemetryEvent = new ActionTelemetryStepEvent(
                nuGetProjectContext.OperationId.ToString(),
                TelemetryConstants.ResolvedActionsStepName,
                stopWatch.Elapsed.TotalSeconds);

            TelemetryActivity.EmitTelemetryEvent(actionTelemetryEvent);

            nuGetProjectContext.Log(MessageLevel.Info, Strings.ResolvedActionsToInstallPackage, packageIdentity);
            return nuGetProjectActions;
        }

        /// <summary>
        /// Check all sources in parallel to see if the package exists while respecting the order of the list.
        /// This is only used by PreviewInstall with DependencyBehavior.Ignore.
        /// Since, resolver gather is not used when dependencies are not used,
        /// we simply get the source repository using MetadataResource.Exists
        /// </summary>
        private static async Task<SourceRepository> GetSourceRepository(PackageIdentity packageIdentity,
            IEnumerable<SourceRepository> sourceRepositories,
            SourceCacheContext sourceCacheContext,
            ILogger logger)
        {
            SourceRepository source = null;

            // TODO: move this timeout to a better place
            // TODO: what should the timeout be?
            // Give up after 5 minutes
            using var tokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(5));

            var results = new Queue<KeyValuePair<SourceRepository, Task<bool>>>();

            foreach (var sourceRepository in sourceRepositories)
            {
                // TODO: fetch the resource in parallel also
                var metadataResource = await sourceRepository.GetResourceAsync<MetadataResource>();
                if (metadataResource != null)
                {
                    var task = Task.Run(() => metadataResource.Exists(packageIdentity, sourceCacheContext, logger, tokenSource.Token), tokenSource.Token);
                    results.Enqueue(new KeyValuePair<SourceRepository, Task<bool>>(sourceRepository, task));
                }
            }

            while (results.Count > 0)
            {
                var pair = results.Dequeue();

                try
                {
                    var exists = await pair.Value;

                    // take only the first true result, but continue waiting for the remaining cancelled
                    // tasks to keep things from getting out of control.
                    if (source == null && exists)
                    {
                        source = pair.Key;

                        // there is no need to finish trying the others
                        tokenSource.Cancel();
                    }
                }
                catch (OperationCanceledException)
                {
                    // ignore these
                }
                catch (Exception ex)
                {
                    logger.LogWarning(
                        string.Format(CultureInfo.CurrentCulture,
                            Strings.Warning_ErrorFindingRepository,
                            pair.Key.PackageSource.Source,
                            ExceptionUtilities.DisplayMessage(ex)));
                }
            }

            if (source == null)
            {
                // no matches were found
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    Strings.UnknownPackageSpecificVersion, packageIdentity.Id, packageIdentity.Version));
            }

            return source;
        }

        /// <summary>
        /// Gives the preview as a list of NuGetProjectActions that will be performed to uninstall for many NuGetProjects.
        /// </summary>
        /// <param name="nuGetProjects"></param>
        /// <param name="packageId"></param>
        /// <param name="uninstallationContext"></param>
        /// <param name="nuGetProjectContext"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NuGetProjectAction>> PreviewProjectsUninstallPackageAsync(
            IReadOnlyCollection<NuGetProject> nuGetProjects,
            string packageId,
            UninstallationContext uninstallationContext,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            if (nuGetProjects == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjects));
            }

            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            if (uninstallationContext == null)
            {
                throw new ArgumentNullException(nameof(uninstallationContext));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            token.ThrowIfCancellationRequested();

            var results = new List<NuGetProjectAction>();

            var buildIntegratedProjectsToUpdate = new List<BuildIntegratedNuGetProject>();
            var otherTargetProjectsToUpdate = new List<NuGetProject>();

            foreach (var project in nuGetProjects)
            {
                if (project == null)
                {
                    throw new ArgumentException(
                        message: string.Format(CultureInfo.CurrentCulture, Strings.PropertyCannotBeNull, nameof(project)),
                        paramName: nameof(nuGetProjects));
                }

                if (project is BuildIntegratedNuGetProject buildIntegratedNuGetProject)
                {
                    buildIntegratedProjectsToUpdate.Add(buildIntegratedNuGetProject);
                }
                else
                {
                    otherTargetProjectsToUpdate.Add(project);
                }
            }

            if (buildIntegratedProjectsToUpdate.Count != 0)
            {
                // Run build integrated project preview for all projects at the same time
                // There we can do evaluation DependencyGraphRestoreUtility.PreviewRestoreProjectsAsync in batch projects instead of one by one projects.
                // So for BuildIntegratedNuGetProject type we share same method 'PreviewBuildIntegratedProjectsActionsAsync' for both install and uninstall actions.
                var uninstallActions = await PreviewBuildIntegratedNuGetProjectsUninstallPackageInternalAsync(
                buildIntegratedProjectsToUpdate,
                packageId,
                nuGetProjectContext,
                token);

                results.AddRange(uninstallActions);
            }

            foreach (var project in otherTargetProjectsToUpdate)
            {
                if (project == null)
                {
                    throw new ArgumentException(
                        message: string.Format(CultureInfo.CurrentCulture, Strings.PropertyCannotBeNull, nameof(project)),
                        paramName: nameof(nuGetProjects));
                }

                // Step-1: Get the packageIdentity corresponding to packageId and check if it exists to be uninstalled
                var installedPackages = await project.GetInstalledPackagesAsync(token);
                var packageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase));
                if (packageReference?.PackageIdentity == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.PackageToBeUninstalledCouldNotBeFound,
                        packageId, project.GetMetadata<string>(NuGetProjectMetadataKeys.Name)));
                }

                IEnumerable<NuGetProjectAction> uninstallActions = await PreviewUninstallPackageInternalAsync(project, packageReference, uninstallationContext, nuGetProjectContext, token);

                results.AddRange(uninstallActions);
            }

            return results;
        }

        private async Task<IEnumerable<NuGetProjectAction>> PreviewBuildIntegratedNuGetProjectsUninstallPackageInternalAsync(
            IReadOnlyList<BuildIntegratedNuGetProject> buildIntegratedProjects,
            string packageId,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            if (SolutionManager == null)
            {
                throw new InvalidOperationException(Strings.SolutionManagerNotAvailableForUninstall);
            }

            var nugetProjectActionsLookup = new Dictionary<string, NuGetProjectAction[]>(PathUtility.GetStringComparerBasedOnOS());

            foreach (BuildIntegratedNuGetProject buildIntegratedProject in buildIntegratedProjects)
            {
                // Get the packageIdentity corresponding to packageId and check if it exists to be uninstalled
                var installedPackages = await buildIntegratedProject.GetInstalledPackagesAsync(token);
                var packageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase));
                if (packageReference?.PackageIdentity == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.PackageToBeUninstalledCouldNotBeFound,
                        packageId, buildIntegratedProject.GetMetadata<string>(NuGetProjectMetadataKeys.Name)));
                }

                NuGetProjectAction action = NuGetProjectAction.CreateUninstallProjectAction(packageReference.PackageIdentity, buildIntegratedProject);
                NuGetProjectAction[] actions = new[] { action };

                nugetProjectActionsLookup[buildIntegratedProject.MSBuildProjectPath] = actions;
            }

            IEnumerable<ResolvedAction> resolvedActions = await PreviewBuildIntegratedProjectsActionsAsync(
                buildIntegratedProjects,
                nugetProjectActionsLookup,
                packageIdentity: null, // since we have nuGetProjectActions no need packageIdentity
                primarySources: null, // since we have nuGetProjectActions no need primarySources
                nuGetProjectContext,
                versionRange: null,
                newMappingID: null,
                newMappingSource: null,
                token);

            return resolvedActions.Select(r => r.Action as BuildIntegratedProjectAction);
        }

        /// <summary>
        /// Gives the preview as a list of NuGetProjectActions that will be performed to uninstall
        /// <paramref name="packageId" /> into <paramref name="nuGetProject" />
        /// <paramref name="uninstallationContext" /> and <paramref name="nuGetProjectContext" /> are used in the
        /// process.
        /// </summary>
        public async Task<IEnumerable<NuGetProjectAction>> PreviewUninstallPackageAsync(NuGetProject nuGetProject, string packageId,
            UninstallationContext uninstallationContext, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }

            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            if (uninstallationContext == null)
            {
                throw new ArgumentNullException(nameof(uninstallationContext));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            // Step-1: Get the packageIdentity corresponding to packageId and check if it exists to be uninstalled
            var installedPackages = await nuGetProject.GetInstalledPackagesAsync(token);
            var packageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase));
            if (packageReference?.PackageIdentity == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.PackageToBeUninstalledCouldNotBeFound,
                    packageId, nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.Name)));
            }

            return await PreviewUninstallPackageInternalAsync(nuGetProject, packageReference, uninstallationContext, nuGetProjectContext, token);
        }

        /// <summary>
        /// Gives the preview as a list of <see cref="NuGetProjectAction" /> that will be performed to uninstall
        /// <paramref name="packageIdentity" /> into <paramref name="nuGetProject" />
        /// <paramref name="uninstallationContext" /> and <paramref name="nuGetProjectContext" /> are used in the
        /// process.
        /// </summary>
        public async Task<IEnumerable<NuGetProjectAction>> PreviewUninstallPackageAsync(NuGetProject nuGetProject, PackageIdentity packageIdentity,
            UninstallationContext uninstallationContext, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (uninstallationContext == null)
            {
                throw new ArgumentNullException(nameof(uninstallationContext));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            // Step-1: Get the packageIdentity corresponding to packageId and check if it exists to be uninstalled
            var installedPackages = await nuGetProject.GetInstalledPackagesAsync(token);
            var packageReference = installedPackages.FirstOrDefault(pr => pr.PackageIdentity.Equals(packageIdentity));
            if (packageReference?.PackageIdentity == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.PackageToBeUninstalledCouldNotBeFound,
                    packageIdentity.Id, nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.Name)));
            }

            return await PreviewUninstallPackageInternalAsync(nuGetProject, packageReference, uninstallationContext, nuGetProjectContext, token);
        }

        private async Task<IEnumerable<NuGetProjectAction>> PreviewUninstallPackageInternalAsync(NuGetProject nuGetProject, Packaging.PackageReference packageReference,
            UninstallationContext uninstallationContext, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            if (SolutionManager == null)
            {
                throw new InvalidOperationException(Strings.SolutionManagerNotAvailableForUninstall);
            }

            if (nuGetProject is INuGetIntegratedProject)
            {
                var action = NuGetProjectAction.CreateUninstallProjectAction(packageReference.PackageIdentity, nuGetProject);
                var actions = new[] { action };

                var buildIntegratedProject = nuGetProject as BuildIntegratedNuGetProject;

                if (buildIntegratedProject != null)
                {
                    actions = new[] {
                        await PreviewBuildIntegratedProjectActionsAsync(buildIntegratedProject, actions, nuGetProjectContext, token)
                    };
                }

                return actions;
            }

            // Step-1 : Get the metadata resources from "packages" folder or custom repository path
            var packageIdentity = packageReference.PackageIdentity;
            var projectName = NuGetProject.GetUniqueNameOrName(nuGetProject);
            var packageReferenceTargetFramework = packageReference.TargetFramework;
            nuGetProjectContext.Log(NuGet.ProjectManagement.MessageLevel.Info, Environment.NewLine);
            nuGetProjectContext.Log(ProjectManagement.MessageLevel.Info, Strings.AttemptingToGatherDependencyInfo, packageIdentity, projectName, packageReferenceTargetFramework);

            var log = new LoggerAdapter(nuGetProjectContext);
            var installedPackageIdentities = (await nuGetProject.GetInstalledPackagesAsync(token)).Select(pr => pr.PackageIdentity);
            var dependencyInfoFromPackagesFolder = await GetDependencyInfoFromPackagesFolderAsync(installedPackageIdentities,
                packageReferenceTargetFramework);

            nuGetProjectContext.Log(ProjectManagement.MessageLevel.Info, Strings.ResolvingActionsToUninstallPackage, packageIdentity);
            // Step-2 : Determine if the package can be uninstalled based on the metadata resources
            var packagesToBeUninstalled = UninstallResolver.GetPackagesToBeUninstalled(packageIdentity, dependencyInfoFromPackagesFolder, installedPackageIdentities, uninstallationContext);

            var nuGetProjectActions =
                packagesToBeUninstalled.Select(
                    package => NuGetProjectAction.CreateUninstallProjectAction(package, nuGetProject));

            nuGetProjectContext.Log(MessageLevel.Info, Strings.ResolvedActionsToUninstallPackage, packageIdentity);
            return nuGetProjectActions;
        }

        private async Task<IEnumerable<PackageDependencyInfo>> GetDependencyInfoFromPackagesFolderAsync(IEnumerable<PackageIdentity> packageIdentities,
            NuGetFramework nuGetFramework,
            bool includeUnresolved = false)
        {
            var dependencyInfoResource = await PackagesFolderSourceRepository.GetResourceAsync<DependencyInfoResource>();
            return await PackageGraphAnalysisUtilities.GetDependencyInfoForPackageIdentitiesAsync(packageIdentities, nuGetFramework, dependencyInfoResource, NullSourceCacheContext.Instance, includeUnresolved, NullLogger.Instance, CancellationToken.None);
        }

        /// <summary>
        /// Executes the list of <paramref name="nuGetProjectActions" /> on list of <paramref name="nuGetProjects" /> , which is
        /// likely obtained by calling into
        /// <see
        ///     cref="PreviewInstallPackageAsync(IEnumerable{NuGetProject},string,ResolutionContext,INuGetProjectContext,SourceRepository,IEnumerable{SourceRepository},CancellationToken)" />
        /// <paramref name="nuGetProjectContext" /> is used in the process.
        /// </summary>
        public async Task ExecuteNuGetProjectActionsAsync(IEnumerable<NuGetProject> nuGetProjects,
            IEnumerable<NuGetProjectAction> nuGetProjectActions,
            INuGetProjectContext nuGetProjectContext,
            SourceCacheContext sourceCacheContext,
            CancellationToken token)
        {
            var projects = nuGetProjects.ToList();

            // find out build integrated projects so that we can arrange them in reverse dependency order
            var buildIntegratedProjectsToUpdate = projects.OfType<BuildIntegratedNuGetProject>().ToList();

            // order won't matter for other type of projects so just add rest of the projects in result
            var sortedProjectsToUpdate = projects.Except(buildIntegratedProjectsToUpdate).ToList();

            if (buildIntegratedProjectsToUpdate.Count > 0)
            {
                var logger = new ProjectContextLogger(nuGetProjectContext);
                var referenceContext = new DependencyGraphCacheContext(logger, Settings);
                _buildIntegratedProjectsUpdateSet = new HashSet<string>(PathUtility.GetStringComparerBasedOnOS());

                var projectUniqueNamesForBuildIntToUpdate
                    = buildIntegratedProjectsToUpdate.ToDictionary((project) => project.MSBuildProjectPath);

                var dgFile = await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(SolutionManager, referenceContext);
                _buildIntegratedProjectsCache = dgFile;
                var allSortedProjects = DependencyGraphSpec.SortPackagesByDependencyOrder(dgFile.Projects);

                // cache these already evaluated(without commit) buildIntegratedProjects project ids which will be used to avoid duplicate restore as part of parent projects
                _buildIntegratedProjectsUpdateSet.AddRange(
                    buildIntegratedProjectsToUpdate.Select(child => child.MSBuildProjectPath));

                foreach (var projectUniqueName in allSortedProjects.Select(e => e.RestoreMetadata.ProjectUniqueName))
                {
                    BuildIntegratedNuGetProject project;
                    if (projectUniqueNamesForBuildIntToUpdate.TryGetValue(projectUniqueName, out project))
                    {
                        sortedProjectsToUpdate.Add(project);
                    }
                }
            }

            // execute all nuget project actions
            foreach (var project in sortedProjectsToUpdate)
            {
                var nugetActions = nuGetProjectActions.Where(action => action.Project.Equals(project));
                await ExecuteNuGetProjectActionsAsync(project, nugetActions, nuGetProjectContext, sourceCacheContext, token);
            }

            // clear cache which could temper with other updates
            _buildIntegratedProjectsUpdateSet?.Clear();
            _buildIntegratedProjectsCache = null;
            _restoreProviderCache = null;
        }

        /// <summary>
        /// Executes the list of <paramref name="nuGetProjectActions" /> on <paramref name="nuGetProject" /> , which is
        /// likely obtained by calling into
        /// <see
        ///     cref="PreviewInstallPackageAsync(NuGetProject,string,ResolutionContext,INuGetProjectContext,SourceRepository,IEnumerable{SourceRepository},CancellationToken)" />
        /// <paramref name="nuGetProjectContext" /> is used in the process.
        /// </summary>
        public async Task ExecuteNuGetProjectActionsAsync(NuGetProject nuGetProject,
            IEnumerable<NuGetProjectAction> nuGetProjectActions,
            INuGetProjectContext nuGetProjectContext,
            SourceCacheContext sourceCacheContext,
            CancellationToken token)
        {
            var logger = new LoggerAdapter(nuGetProjectContext);

            var downloadContext = new PackageDownloadContext(sourceCacheContext)
            {
                ParentId = nuGetProjectContext.OperationId,
                ClientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, logger)
            };

            await ExecuteNuGetProjectActionsAsync(nuGetProject,
                nuGetProjectActions,
                nuGetProjectContext,
                downloadContext,
                token);
        }

        /// <summary>
        /// Executes the list of <paramref name="nuGetProjectActions" /> on <paramref name="nuGetProject" /> , which is
        /// likely obtained by calling into
        /// <see
        ///     cref="PreviewInstallPackageAsync(NuGetProject,string,ResolutionContext,INuGetProjectContext,SourceRepository,IEnumerable{SourceRepository},CancellationToken)" />
        /// <paramref name="nuGetProjectContext" /> is used in the process.
        /// </summary>
        public async Task ExecuteNuGetProjectActionsAsync(NuGetProject nuGetProject,
            IEnumerable<NuGetProjectAction> nuGetProjectActions,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext,
            CancellationToken token)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }

            if (nuGetProjectActions == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectActions));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            var stopWatch = Stopwatch.StartNew();

            ExceptionDispatchInfo exceptionInfo = null;

            // DNU: Find the closure before executing the actions
            var buildIntegratedProject = nuGetProject as BuildIntegratedNuGetProject;
            if (buildIntegratedProject != null)
            {
                await ExecuteBuildIntegratedProjectActionsAsync(buildIntegratedProject,
                    nuGetProjectActions,
                    nuGetProjectContext,
                    token);
            }
            else
            {
                // Set the original packages config if it exists
                var msbuildProject = nuGetProject as MSBuildNuGetProject;
                if (msbuildProject != null)
                {
                    nuGetProjectContext.OriginalPackagesConfig =
                        msbuildProject.PackagesConfigNuGetProject?.GetPackagesConfig();
                }

                var executedNuGetProjectActions = new Stack<NuGetProjectAction>();
                var packageWithDirectoriesToBeDeleted = new HashSet<PackageIdentity>(PackageIdentity.Comparer);
                var ideExecutionContext = nuGetProjectContext.ExecutionContext as IDEExecutionContext;
                if (ideExecutionContext != null)
                {
                    await ideExecutionContext.SaveExpandedNodeStates(SolutionManager);
                }

                var logger = new ProjectContextLogger(nuGetProjectContext);
                Dictionary<PackageIdentity, PackagePreFetcherResult> downloadTasks = null;
                CancellationTokenSource downloadTokenSource = null;

                // batch events argument object
                PackageProjectEventArgs packageProjectEventArgs = null;

                try
                {
                    // PreProcess projects
                    await nuGetProject.PreProcessAsync(nuGetProjectContext, token);

                    var actionsList = nuGetProjectActions.ToList();

                    var hasInstalls = actionsList.Any(action =>
                        action.NuGetProjectActionType == NuGetProjectActionType.Install);

                    if (hasInstalls)
                    {
                        // Make this independently cancelable.
                        downloadTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

                        // Download all packages up front in parallel
                        downloadTasks = await PackagePreFetcher.GetPackagesAsync(
                            actionsList,
                            PackagesFolderNuGetProject,
                            downloadContext,
                            SettingsUtility.GetGlobalPackagesFolder(Settings),
                            logger,
                            downloadTokenSource.Token);

                        // Log download information
                        PackagePreFetcher.LogFetchMessages(
                            downloadTasks.Values,
                            PackagesFolderNuGetProject.Root,
                            logger);
                    }

                    // raise Nuget batch start event
                    var batchId = Guid.NewGuid().ToString();
                    string name;
                    nuGetProject.TryGetMetadata(NuGetProjectMetadataKeys.Name, out name);
                    var projectPath = msbuildProject?.MSBuildProjectPath;
                    packageProjectEventArgs = new PackageProjectEventArgs(batchId, name, projectPath);
                    BatchStart?.Invoke(this, packageProjectEventArgs);
                    PackageProjectEventsProvider.Instance.NotifyBatchStart(packageProjectEventArgs);

                    try
                    {
                        if (msbuildProject != null)
                        {
                            //start batch processing for msbuild
                            await msbuildProject.ProjectSystem.BeginProcessingAsync();
                        }

                        foreach (var nuGetProjectAction in actionsList)
                        {
                            if (nuGetProjectAction.NuGetProjectActionType == NuGetProjectActionType.Uninstall)
                            {
                                executedNuGetProjectActions.Push(nuGetProjectAction);

                                await ExecuteUninstallAsync(nuGetProject,
                                    nuGetProjectAction.PackageIdentity,
                                    packageWithDirectoriesToBeDeleted,
                                    nuGetProjectContext, token);

                                nuGetProjectContext.Log(
                                    ProjectManagement.MessageLevel.Info,
                                    Strings.SuccessfullyUninstalled,
                                    nuGetProjectAction.PackageIdentity,
                                    nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.Name));
                            }
                        }
                    }
                    finally
                    {
                        if (msbuildProject != null)
                        {
                            // end batch for msbuild and let it save everything.
                            // always calls it before PostProcessAsync or binding redirects
                            await msbuildProject.ProjectSystem.EndProcessingAsync();
                        }
                    }

                    try
                    {
                        if (msbuildProject != null)
                        {
                            //start batch processing for msbuild
                            await msbuildProject.ProjectSystem.BeginProcessingAsync();
                        }

                        foreach (var nuGetProjectAction in actionsList)
                        {
                            if (nuGetProjectAction.NuGetProjectActionType == NuGetProjectActionType.Install)
                            {
                                executedNuGetProjectActions.Push(nuGetProjectAction);

                                // Retrieve the downloaded package
                                // This will wait on the package if it is still downloading
                                var preFetchResult = downloadTasks[nuGetProjectAction.PackageIdentity];
                                using (var downloadPackageResult = await preFetchResult.GetResultAsync())
                                {
                                    // use the version exactly as specified in the nuspec file
                                    var packageIdentity = await downloadPackageResult.PackageReader.GetIdentityAsync(token);

                                    await ExecuteInstallAsync(
                                        nuGetProject,
                                        packageIdentity,
                                        downloadPackageResult,
                                        packageWithDirectoriesToBeDeleted,
                                        nuGetProjectContext,
                                        token);
                                }

                                var identityString = string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                                    nuGetProjectAction.PackageIdentity.Id,
                                    nuGetProjectAction.PackageIdentity.Version.ToNormalizedString());

                                preFetchResult.EmitTelemetryEvent(nuGetProjectContext.OperationId);

                                nuGetProjectContext.Log(
                                    ProjectManagement.MessageLevel.Info,
                                    Strings.SuccessfullyInstalled,
                                    identityString,
                                    nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.Name));
                            }
                        }
                    }
                    finally
                    {
                        if (msbuildProject != null)
                        {
                            // end batch for msbuild and let it save everything.
                            // always calls it before PostProcessAsync or binding redirects
                            await msbuildProject.ProjectSystem.EndProcessingAsync();
                        }
                    }

                    PackagesConfigLockFileUtility.UpdateLockFile(msbuildProject,
                        actionsList,
                        token);

                    // Post process
                    await nuGetProject.PostProcessAsync(nuGetProjectContext, token);

                    // Open readme file
                    await OpenReadmeFile(nuGetProject, nuGetProjectContext, token);
                }
                catch (SignatureException ex)
                {
                    var errors = ex.Results.SelectMany(r => r.GetErrorIssues());
                    var warnings = ex.Results.SelectMany(r => r.GetWarningIssues());
                    SignatureException unwrappedException = null;

                    if (errors.Count() == 1)
                    {
                        // In case of one error, throw it as the exception
                        var error = errors.First();
                        unwrappedException = new SignatureException(error.Code, error.Message, ex.PackageIdentity);
                    }
                    else
                    {
                        // In case of multiple errors, wrap them in a general NU3000 error
                        var errorMessage = string.Format(CultureInfo.CurrentCulture,
                            Strings.SignatureVerificationMultiple,
                            $"{Environment.NewLine}{string.Join(Environment.NewLine, errors.Select(e => e.FormatWithCode()))}");

                        unwrappedException = new SignatureException(NuGetLogCode.NU3000, errorMessage, ex.PackageIdentity);
                    }

                    foreach (var warning in warnings)
                    {
                        nuGetProjectContext.Log(warning);
                    }

                    exceptionInfo = ExceptionDispatchInfo.Capture(unwrappedException);
                }
                catch (Exception ex)
                {
                    exceptionInfo = ExceptionDispatchInfo.Capture(ex);
                }
                finally
                {
                    if (downloadTasks != null)
                    {
                        // Wait for all downloads to cancel and dispose
                        downloadTokenSource.Cancel();

                        foreach (var result in downloadTasks.Values)
                        {
                            await result.EnsureResultAsync();
                            result.Dispose();
                        }
                    }

                    downloadTokenSource?.Dispose();

                    if (msbuildProject != null)
                    {
                        // raise nuget batch end event
                        if (packageProjectEventArgs != null)
                        {
                            BatchEnd?.Invoke(this, packageProjectEventArgs);
                            PackageProjectEventsProvider.Instance.NotifyBatchEnd(packageProjectEventArgs);
                        }
                    }
                }

                if (exceptionInfo != null)
                {
                    await RollbackAsync(nuGetProject, executedNuGetProjectActions, packageWithDirectoriesToBeDeleted, nuGetProjectContext, token);
                }

                if (ideExecutionContext != null)
                {
                    await ideExecutionContext.CollapseAllNodes(SolutionManager);
                }

                // Delete the package directories as the last step, so that, if an uninstall had to be rolled back, we can just use the package file on the directory
                // Also, always perform deletion of package directories, even in a rollback, so that there are no stale package directories
                foreach (var packageWithDirectoryToBeDeleted in packageWithDirectoriesToBeDeleted)
                {
                    var packageFolderPath = PackagesFolderNuGetProject.GetInstalledPath(packageWithDirectoryToBeDeleted);
                    try
                    {
                        await DeletePackageAsync(packageWithDirectoryToBeDeleted, nuGetProjectContext, token);
                    }
                    finally
                    {
                        if (DeleteOnRestartManager != null)
                        {
                            if (Directory.Exists(packageFolderPath))
                            {
                                DeleteOnRestartManager.MarkPackageDirectoryForDeletion(
                                    packageWithDirectoryToBeDeleted,
                                    packageFolderPath,
                                    nuGetProjectContext);

                                // Raise the event to notify listners to update the UI etc.
                                DeleteOnRestartManager.CheckAndRaisePackageDirectoriesMarkedForDeletion();
                            }
                        }
                    }
                }

                // Save project
                await nuGetProject.SaveAsync(token);

                // Clear direct install
                SetDirectInstall(null, nuGetProjectContext);
            }


            // calculate total time taken to execute all nuget actions
            stopWatch.Stop();
            nuGetProjectContext.Log(
                MessageLevel.Info, Strings.NugetActionsTotalTime,
                DatetimeUtility.ToReadableTimeFormat(stopWatch.Elapsed));

            // emit resolve actions telemetry event
            var actionTelemetryEvent = new ActionTelemetryStepEvent(
                nuGetProjectContext.OperationId.ToString(),
                TelemetryConstants.ExecuteActionStepName, stopWatch.Elapsed.TotalSeconds);

            TelemetryActivity.EmitTelemetryEvent(actionTelemetryEvent);

            if (exceptionInfo != null)
            {
                exceptionInfo.Throw();
            }

        }

        /// <summary>
        /// Run project actions for a build integrated project.
        /// </summary>
        public async Task<BuildIntegratedProjectAction> PreviewBuildIntegratedProjectActionsAsync(
            BuildIntegratedNuGetProject buildIntegratedProject,
            IEnumerable<NuGetProjectAction> nuGetProjectActions,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            if (nuGetProjectActions == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectActions));
            }

            if (buildIntegratedProject == null)
            {
                throw new ArgumentNullException(nameof(buildIntegratedProject));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            if (!nuGetProjectActions.Any())
            {
                // Return null if there are no actions.
                return null;
            }

            var resolvedAction = await PreviewBuildIntegratedProjectsActionsAsync(
                new List<BuildIntegratedNuGetProject>() { buildIntegratedProject },
                new Dictionary<string, NuGetProjectAction[]>(PathUtility.GetStringComparerBasedOnOS())
                {
                    { buildIntegratedProject.MSBuildProjectPath, nuGetProjectActions.ToArray()}
                },
                packageIdentity: null, // since we have nuGetProjectActions no need packageIdentity
                primarySources: null, // since we have nuGetProjectActions no need primarySources
                nuGetProjectContext,
                versionRange: null,
                newMappingID: null,
                newMappingSource: null,
                token);

            return resolvedAction.FirstOrDefault(r => r.Project == buildIntegratedProject)?.Action as BuildIntegratedProjectAction;
        }

        /// <summary>
        /// Run project actions for build integrated many projects.
        /// </summary>
        internal async Task<IEnumerable<ResolvedAction>> PreviewBuildIntegratedProjectsActionsAsync(
            IReadOnlyCollection<BuildIntegratedNuGetProject> buildIntegratedProjects,
            Dictionary<string, NuGetProjectAction[]> nugetProjectActionsLookup,
            PackageIdentity packageIdentity,
            IReadOnlyCollection<SourceRepository> primarySources,
            INuGetProjectContext nuGetProjectContext,
            VersionRange versionRange,
            string newMappingID,
            string newMappingSource,
            CancellationToken token)
        {
            if (nugetProjectActionsLookup == null)
            {
                nugetProjectActionsLookup = new Dictionary<string, NuGetProjectAction[]>(PathUtility.GetStringComparerBasedOnOS());
            }

            if (buildIntegratedProjects == null)
            {
                throw new ArgumentNullException(nameof(buildIntegratedProjects));
            }

            if (buildIntegratedProjects.Count == 0)
            {
                // Return empty if there are no buildIntegratedProjects.
                return Enumerable.Empty<ResolvedAction>();
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            if (nugetProjectActionsLookup.Count == 0 && packageIdentity == null)
            {
                // Return empty if there are neither actions nor packageIdentity.
                return Enumerable.Empty<ResolvedAction>();
            }

            var stopWatch = Stopwatch.StartNew();
            var logger = new ProjectContextLogger(nuGetProjectContext);
            var result = new List<ResolvedAction>();

            var lockFileLookup = new Dictionary<string, LockFile>(PathUtility.GetStringComparerBasedOnOS());
            PackageSourceMappingProvider packageSourceMappingProvider = null;
            IReadOnlyList<PackageSourceMappingSourceItem> originalPackageSourceMappings = null;

            if (newMappingID != null && newMappingSource != null)
            {
                packageSourceMappingProvider = new PackageSourceMappingProvider(Settings, shouldSkipSave: true);
                originalPackageSourceMappings = packageSourceMappingProvider.GetPackageSourceMappingItems();
                AddNewPackageSourceMappingToSettings(newMappingID, newMappingSource, packageSourceMappingProvider);
            }

            var dependencyGraphContext = new DependencyGraphCacheContext(logger, Settings);
            var pathContext = NuGetPathContext.Create(Settings);
            var providerCache = new RestoreCommandProvidersCache();
            var updatedNugetPackageSpecLookup = new Dictionary<string, PackageSpec>(PathUtility.GetStringComparerBasedOnOS());
            var originalNugetPackageSpecLookup = new Dictionary<string, PackageSpec>(PathUtility.GetStringComparerBasedOnOS());
            var nuGetProjectSourceLookup = new Dictionary<string, HashSet<SourceRepository>>(PathUtility.GetStringComparerBasedOnOS());

            // For installs only use cache entries newer than the current time.
            // This is needed for scenarios where a new package shows up in search
            // but a previous cache entry does not yet have it.
            // So we want to capture the time once here, then pass it down to the two
            // restores happening in this flow.
            var now = DateTimeOffset.UtcNow;
            void cacheModifier(SourceCacheContext cache) => cache.MaxAge = now;

            // Add all enabled sources for the existing projects
            var enabledSources = SourceRepositoryProvider.GetRepositories();
            var allSources = new HashSet<SourceRepository>(enabledSources, SourceRepositoryComparer.Instance);

            foreach (var buildIntegratedProject in buildIntegratedProjects)
            {
                NuGetProjectAction[] nuGetProjectActions;

                if (packageIdentity != null)
                {
                    if (primarySources == null || primarySources.Count == 0)
                    {
                        throw new ArgumentNullException(nameof(primarySources), $"Should have value in {nameof(primarySources)} if there is value for {nameof(packageIdentity)}");
                    }

                    var nugetAction = NuGetProjectAction.CreateInstallProjectAction(packageIdentity, primarySources.First(), buildIntegratedProject, versionRange);
                    nuGetProjectActions = new[] { nugetAction };
                    nugetProjectActionsLookup[buildIntegratedProject.MSBuildProjectPath] = nuGetProjectActions;
                }
                else
                {
                    if (!nugetProjectActionsLookup.ContainsKey(buildIntegratedProject.MSBuildProjectPath))
                    {
                        throw new ArgumentException(
                            message: string.Format(CultureInfo.CurrentCulture, Strings.UnableToFindPathInLookupOrList, nameof(nugetProjectActionsLookup), buildIntegratedProject.MSBuildProjectPath, nameof(packageIdentity), nameof(primarySources)),
                            paramName: nameof(nugetProjectActionsLookup));
                    }

                    nuGetProjectActions = nugetProjectActionsLookup[buildIntegratedProject.MSBuildProjectPath];

                    if (nuGetProjectActions.Length == 0)
                    {
                        // Continue to next project if there are no actions for current project.
                        continue;
                    }
                }

                // Find all sources used in the project actions
                var sources = new HashSet<SourceRepository>(
                    nuGetProjectActions.Where(action => action.SourceRepository != null)
                        .Select(action => action.SourceRepository),
                        SourceRepositoryComparer.Instance);

                allSources.UnionWith(sources);
                sources.UnionWith(enabledSources);
                nuGetProjectSourceLookup[buildIntegratedProject.MSBuildProjectPath] = sources;

                // Read the current lock file if it exists
                LockFile originalLockFile = null;
                var lockFileFormat = new LockFileFormat();

                var lockFilePath = await buildIntegratedProject.GetAssetsFilePathAsync();

                if (File.Exists(lockFilePath))
                {
                    originalLockFile = lockFileFormat.Read(lockFilePath);
                }

                // Get Package Spec as json object
                var originalPackageSpec = await DependencyGraphRestoreUtility.GetProjectSpec(buildIntegratedProject, dependencyGraphContext);
                originalNugetPackageSpecLookup[buildIntegratedProject.MSBuildProjectPath] = originalPackageSpec;

                // Create a copy to avoid modifying the original spec which may be shared.
                var updatedPackageSpec = originalPackageSpec.Clone();

                // If the lock file does not exist, restore before starting the operations
                if (originalLockFile == null)
                {
                    var originalRestoreResult = await DependencyGraphRestoreUtility.PreviewRestoreAsync(
                        SolutionManager,
                        buildIntegratedProject,
                        originalPackageSpec,
                        dependencyGraphContext,
                        providerCache,
                        cacheModifier,
                        sources,
                        nuGetProjectContext.OperationId,
                        token);

                    originalLockFile = originalRestoreResult.Result.LockFile;
                }

                lockFileLookup[buildIntegratedProject.MSBuildProjectPath] = originalLockFile;

                foreach (var action in nuGetProjectActions)
                {
                    if (action.NuGetProjectActionType == NuGetProjectActionType.Uninstall)
                    {
                        // Remove the package from all frameworks and dependencies section.
                        PackageSpecOperations.RemoveDependency(updatedPackageSpec, action.PackageIdentity.Id);
                    }
                    else if (action.NuGetProjectActionType == NuGetProjectActionType.Install)
                    {
                        if (updatedPackageSpec.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference)
                        {
                            var packageDependency = new PackageDependency(action.PackageIdentity.Id, action.VersionRange ?? new VersionRange(action.PackageIdentity.Version));
                            PackageSpecOperations.AddOrUpdateDependency(updatedPackageSpec, packageDependency);
                        }
                        else
                        {
                            PackageSpecOperations.AddOrUpdateDependency(updatedPackageSpec, action.PackageIdentity);
                        }
                    }

                    updatedNugetPackageSpecLookup[buildIntegratedProject.MSBuildProjectPath] = updatedPackageSpec;
                    dependencyGraphContext.PackageSpecCache[buildIntegratedProject.MSBuildProjectPath] = updatedPackageSpec;
                }
            }

            // Restore based on the modified package specs for many projects. This operation does not write the lock files to disk.
            var restoreResults = await DependencyGraphRestoreUtility.PreviewRestoreProjectsAsync(
                SolutionManager,
                buildIntegratedProjects,
                updatedNugetPackageSpecLookup.Values,
                dependencyGraphContext,
                providerCache,
                cacheModifier,
                allSources,
                nuGetProjectContext.OperationId,
                logger,
                token);

            foreach (var buildIntegratedProject in buildIntegratedProjects)
            {
                NuGetProjectAction[] nuGetProjectActions = nugetProjectActionsLookup[buildIntegratedProject.MSBuildProjectPath];
                var updatedPackageSpec = updatedNugetPackageSpecLookup[buildIntegratedProject.MSBuildProjectPath];
                var originalPackageSpec = originalNugetPackageSpecLookup[buildIntegratedProject.MSBuildProjectPath];
                var originalLockFile = lockFileLookup[buildIntegratedProject.MSBuildProjectPath];
                var sources = nuGetProjectSourceLookup[buildIntegratedProject.MSBuildProjectPath];

                var allFrameworks = updatedPackageSpec
                    .TargetFrameworks
                    .Select(t => t.FrameworkName)
                    .Distinct()
                    .ToList();

                var restoreResult = restoreResults.Single(r =>
                    string.Equals(
                        r.SummaryRequest.Request.Project.RestoreMetadata.ProjectPath,
                        buildIntegratedProject.MSBuildProjectPath,
                        StringComparison.OrdinalIgnoreCase));

                var unsuccessfulFrameworks = restoreResult
                    .Result
                    .CompatibilityCheckResults
                    .Where(t => !t.Success)
                    .Select(t => t.Graph.Framework)
                    .Distinct()
                    .ToList();

                var originalFrameworks = updatedPackageSpec
                    .TargetFrameworks
                    .ToDictionary(x => x.FrameworkName, x => x.TargetAlias);

                List<(NuGetProjectAction, BuildIntegratedInstallationContext)> projectActionsAndInstallationContexts = new(nuGetProjectActions.Length);

                foreach (var action in nuGetProjectActions)
                {
                    PackageSpec referencePackageSpec = null;
                    referencePackageSpec = action.NuGetProjectActionType switch
                    {
                        NuGetProjectActionType.Install or NuGetProjectActionType.Update => restoreResult.Result.LockFile.PackageSpec,
                        NuGetProjectActionType.Uninstall => originalPackageSpec,
                        _ => throw new InvalidOperationException("Unknown NuGetProjectActionType"),
                    };
                    BuildIntegratedInstallationContext installationContext = CreateInstallationContextForPackageId(action.PackageIdentity.Id, referencePackageSpec, unsuccessfulFrameworks, originalFrameworks);
                    projectActionsAndInstallationContexts.Add((action, installationContext));
                }

                var firstInstallationAndProjectContext = projectActionsAndInstallationContexts[0];
                var firstAction = firstInstallationAndProjectContext.Item1;
                var successfulFrameworks = firstInstallationAndProjectContext.Item2.SuccessfulFrameworks.AsList();

                // If the restore failed and this was a single package install, try to install the package to a subset of
                // the target frameworks.
                if (nuGetProjectActions.Length == 1 &&
                    firstAction.NuGetProjectActionType == NuGetProjectActionType.Install &&
                    !restoreResult.Result.Success &&
                    successfulFrameworks.Count > 0 &&
                    unsuccessfulFrameworks.Count > 0 &&
                    !PackageSpecOperations.HasPackage(originalPackageSpec, firstAction.PackageIdentity.Id))
                {
                    updatedPackageSpec = originalPackageSpec.Clone();

                    PackageSpecOperations.AddOrUpdateDependency(
                        updatedPackageSpec,
                        firstAction.PackageIdentity,
                        successfulFrameworks);

                    restoreResult = await DependencyGraphRestoreUtility.PreviewRestoreAsync(
                        SolutionManager,
                        buildIntegratedProject,
                        updatedPackageSpec,
                        dependencyGraphContext,
                        providerCache,
                        cacheModifier,
                        sources,
                        nuGetProjectContext.OperationId,
                        token);
                }

                // If HideWarningsAndErrors is true then restore will not display the warnings and errors.
                // Further, replay errors and warnings only if restore failed because the assets file will not be committed.
                // If there were only warnings then those are written to assets file and committed. The design time build will replay them.
                if (updatedPackageSpec.RestoreSettings.HideWarningsAndErrors &&
                    !restoreResult.Result.Success)
                {
                    await MSBuildRestoreUtility.ReplayWarningsAndErrorsAsync(restoreResult.Result.LockFile?.LogMessages, logger);
                }

                InstallationCompatibility.EnsurePackageCompatibility(
                    buildIntegratedProject,
                    pathContext,
                    nuGetProjectActions,
                    restoreResult.Result);

                // If this build integrated project action represents only uninstalls, mark the entire operation
                // as an uninstall. Otherwise, mark it as an install. This is important because install operations
                // are a bit more sensitive to errors (thus resulting in rollbacks).
                var actionType = NuGetProjectActionType.Install;
                if (nuGetProjectActions.All(x => x.NuGetProjectActionType == NuGetProjectActionType.Uninstall))
                {
                    actionType = NuGetProjectActionType.Uninstall;
                }

                var nugetProjectAction = new BuildIntegratedProjectAction(
                    buildIntegratedProject,
                    nuGetProjectActions.First().PackageIdentity,
                    actionType,
                    originalLockFile,
                    restoreResult,
                    sources.ToList(),
                    projectActionsAndInstallationContexts,
                    versionRange);

                result.Add(new ResolvedAction(buildIntegratedProject, nugetProjectAction));
            }

            // Put back the Package Source Mappings that existed prior to this Preview.
            if (originalPackageSourceMappings != null && packageSourceMappingProvider != null)
            {
                packageSourceMappingProvider.SavePackageSourceMappings(originalPackageSourceMappings);
            }

            stopWatch.Stop();
            var actionTelemetryEvent = new ActionTelemetryStepEvent(
                nuGetProjectContext.OperationId.ToString(),
                TelemetryConstants.PreviewBuildIntegratedStepName, stopWatch.Elapsed.TotalSeconds);

            TelemetryActivity.EmitTelemetryEvent(actionTelemetryEvent);

            return result;
        }

        /// <summary>
        /// Build a context for package given package id.
        /// The "successful" frameworks are the ones that contain the package and are not part of the failed list.
        /// The "unsuccessful" frameworks are the ones that never had the package or are part of the unsuccessful list.
        /// </summary>>
        internal static BuildIntegratedInstallationContext CreateInstallationContextForPackageId(string packageIdentityId, PackageSpec packageSpec, List<NuGetFramework> unsuccessfulFrameworks, Dictionary<NuGetFramework, string> originalFrameworks)
        {
            var frameworksWithResultingPackage = packageSpec
                                .TargetFrameworks
                                .Where(e => e.Dependencies.Any(a => a.Name == packageIdentityId))
                                .Select(e => e.FrameworkName)
                                .Distinct();

            var frameworksWithoutResultingPackage = packageSpec
                .TargetFrameworks
                .Where(e => !e.Dependencies.Any(a => a.Name == packageIdentityId))
                .Select(e => e.FrameworkName)
                .Distinct();

            var successfulFrameworksWithPackage = frameworksWithResultingPackage
                .Except(unsuccessfulFrameworks)
                .ToList();

            return new BuildIntegratedInstallationContext(
                successfulFrameworksWithPackage,
                unsuccessfulFrameworks.Union(frameworksWithoutResultingPackage).Distinct(),
                originalFrameworks);
        }

        /// <summary>
        /// Reads existing Package Source Mappings from settings and appends a new mapping for the <paramref name="newMappingID"/> and a glob "*" pattern
        /// for the <paramref name="newMappingSource"/>.
        /// The intention is that Preview Restore can run and expect all newly installed packages to be source mapped to the new source.
        /// </summary>
        /// <returns>If a new mapping was provided, returns all persisted mappings appended with the new mapping. Otherwise, null.</returns>
        private void AddNewPackageSourceMappingToSettings(string newMappingID, string newMappingSource, PackageSourceMappingProvider mappingProvider)
        {
            List<PackagePatternItem> newPatternItems = new()
            {
                new PackagePatternItem(newMappingID),
                new PackagePatternItem("*")
            };

            List<PackageSourceMappingSourceItem> newAndExistingPackageSourceMappingItems = mappingProvider.GetPackageSourceMappingItems().ToList();
            newAndExistingPackageSourceMappingItems.Add(new PackageSourceMappingSourceItem(newMappingSource, newPatternItems));
            mappingProvider.SavePackageSourceMappings(newAndExistingPackageSourceMappingItems);
        }

        /// <summary>
        /// Run project actions for build integrated projects.
        /// </summary>
        public async Task ExecuteBuildIntegratedProjectActionsAsync(
            BuildIntegratedNuGetProject buildIntegratedProject,
            IEnumerable<NuGetProjectAction> nuGetProjectActions,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            if (buildIntegratedProject == null)
            {
                throw new ArgumentNullException(nameof(buildIntegratedProject));
            }

            BuildIntegratedProjectAction projectAction = null;

            if (nuGetProjectActions.Count() == 1
                && nuGetProjectActions.All(action => action is BuildIntegratedProjectAction))
            {
                projectAction = nuGetProjectActions.Single() as BuildIntegratedProjectAction;
            }
            else if (nuGetProjectActions.Any())
            {
                projectAction = await PreviewBuildIntegratedProjectActionsAsync(
                    buildIntegratedProject,
                    nuGetProjectActions,
                    nuGetProjectContext,
                    token);
            }
            else
            {
                // There are no actions, this is a no-op
                return;
            }

            var actions = projectAction.GetProjectActions();

            // Check if all actions are uninstalls
            var uninstallOnly = projectAction.NuGetProjectActionType == NuGetProjectActionType.Uninstall
                && actions.All(action => action.NuGetProjectActionType == NuGetProjectActionType.Uninstall);

            var restoreResult = projectAction.RestoreResult;

            // Avoid committing the changes if the restore did not succeed
            // For uninstalls continue even if the restore failed to avoid blocking the user
            if (restoreResult.Success || uninstallOnly)
            {
                // Get all install actions
                var ignoreActions = new HashSet<NuGetProjectAction>();
                var installedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach ((var action, _) in projectAction.ActionAndContextList.Reverse())
                {
                    if (action.NuGetProjectActionType == NuGetProjectActionType.Install)
                    {
                        installedIds.Add(action.PackageIdentity.Id);
                    }
                    else if (installedIds.Contains(action.PackageIdentity.Id))
                    {
                        ignoreActions.Add(action);
                    }
                }

                var pathResolver = new FallbackPackagePathResolver(
                    projectAction.RestoreResult.LockFile.PackageSpec.RestoreMetadata.PackagesPath,
                    projectAction.RestoreResult.LockFile.PackageSpec.RestoreMetadata.FallbackFolders);

                foreach ((var originalAction, var installationContext) in projectAction.ActionAndContextList.Where(e => !ignoreActions.Contains(e.Item1)))
                {
                    // This is where we calculate what goes into CPSPackageReferenceProject
                    if (originalAction.NuGetProjectActionType == NuGetProjectActionType.Install)
                    {
                        if (buildIntegratedProject.ProjectStyle == ProjectStyle.PackageReference)
                        {
                            BuildIntegratedRestoreUtility.UpdatePackageReferenceMetadata(
                                projectAction.RestoreResult.LockFile,
                                pathResolver,
                                originalAction.PackageIdentity);

                            var framework = installationContext.SuccessfulFrameworks.FirstOrDefault();
                            var resolvedAction = projectAction.RestoreResult.LockFile.PackageSpec.TargetFrameworks.FirstOrDefault(fm => fm.FrameworkName.Equals(framework))
                                .Dependencies.First(dependency => dependency.Name.Equals(originalAction.PackageIdentity.Id, StringComparison.OrdinalIgnoreCase));

                            installationContext.SuppressParent = resolvedAction.SuppressParent;
                            installationContext.IncludeType = resolvedAction.IncludeType;
                        }

                        // Install the package to the project
                        await buildIntegratedProject.InstallPackageAsync(
                            originalAction.PackageIdentity.Id,
                            originalAction.VersionRange ?? new VersionRange(originalAction.PackageIdentity.Version),
                            nuGetProjectContext,
                            installationContext,
                            token: token);
                    }
                    else if (originalAction.NuGetProjectActionType == NuGetProjectActionType.Uninstall)
                    {
                        await buildIntegratedProject.UninstallPackageAsync(
                            originalAction.PackageIdentity,
                            nuGetProjectContext: nuGetProjectContext,
                            token: token);
                    }
                }

                var logger = new ProjectContextLogger(nuGetProjectContext);
                var referenceContext = new DependencyGraphCacheContext(logger, Settings);

                var now = DateTime.UtcNow;
                void cacheContextModifier(SourceCacheContext c) => c.MaxAge = now;

                // Write out the lock file, now no need bubbling re-evaluating of parent projects when you restore from PM UI.
                // We already taken account of that concern in PreviewBuildIntegratedProjectsActionsAsync method.

                bool isNoOp = projectAction.RestoreResultPair.Result is NoOpRestoreResult;
                IReadOnlyList<string> filesToBeUpdated = isNoOp ? null : GetFilesToBeUpdated(projectAction.RestoreResultPair);
                if (!isNoOp)
                {
                    RestoreProgressReporter?.StartProjectUpdate(projectAction.RestoreResultPair.SummaryRequest.Request.Project.FilePath, filesToBeUpdated);
                }
                try
                {
                    await RestoreRunner.CommitAsync(projectAction.RestoreResultPair, token);
                }
                finally
                {
                    if (!isNoOp)
                    {
                        RestoreProgressReporter?.EndProjectUpdate(projectAction.RestoreResultPair.SummaryRequest.Request.Project.FilePath, filesToBeUpdated);
                    }
                }

                // add packages lock file into project
                if (PackagesLockFileUtilities.IsNuGetLockFileEnabled(projectAction.RestoreResult.LockFile.PackageSpec))
                {
                    var lockFilePath = PackagesLockFileUtilities.GetNuGetLockFilePath(projectAction.RestoreResult.LockFile.PackageSpec);

                    await buildIntegratedProject.AddFileToProjectAsync(lockFilePath);
                }

                // Write out a message for each action
                foreach (var action in actions)
                {
                    var identityString = string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                        action.PackageIdentity.Id,
                        action.PackageIdentity.Version.ToNormalizedString());

                    if (action.NuGetProjectActionType == NuGetProjectActionType.Install)
                    {
                        nuGetProjectContext.Log(
                            MessageLevel.Info,
                            Strings.SuccessfullyInstalled,
                            identityString,
                            buildIntegratedProject.GetMetadata<string>(NuGetProjectMetadataKeys.Name));
                    }
                    else
                    {
                        // uninstall
                        nuGetProjectContext.Log(
                            MessageLevel.Info,
                            Strings.SuccessfullyUninstalled,
                            identityString,
                            buildIntegratedProject.GetMetadata<string>(NuGetProjectMetadataKeys.Name));
                    }
                }

                // Run init.ps1 scripts
                var addedPackages = BuildIntegratedRestoreUtility.GetAddedPackages(
                    projectAction.OriginalLockFile,
                    restoreResult.LockFile);
                await BuildIntegratedRestoreUtility.ExecuteInitPs1ScriptsAsync(
                    buildIntegratedProject,
                    addedPackages,
                    pathResolver,
                    nuGetProjectContext,
                    token);

                // find list of buildintegrated projects
                var projects = (await SolutionManager.GetNuGetProjectsAsync()).OfType<BuildIntegratedNuGetProject>().ToList();

                // build reference cache if not done already
                if (_buildIntegratedProjectsCache == null)
                {
                    _buildIntegratedProjectsCache = await
                        DependencyGraphRestoreUtility.GetSolutionRestoreSpec(SolutionManager, referenceContext);
                }

                // Restore parent projects. These will be updated to include the transitive changes.
                var parents = BuildIntegratedRestoreUtility.GetParentProjectsInClosure(
                    projects,
                    buildIntegratedProject,
                    _buildIntegratedProjectsCache);
                // The settings contained in the context are applied to the dg spec.
                var dgSpecForParents = await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(SolutionManager, referenceContext);
                dgSpecForParents = dgSpecForParents.WithoutRestores();

                foreach (var parent in parents)
                {
                    // We only evaluate unseen parents.
                    if (_buildIntegratedProjectsUpdateSet == null ||
                        !_buildIntegratedProjectsUpdateSet.Contains(parent.MSBuildProjectPath))
                    {
                        // Mark project for restore
                        dgSpecForParents.AddRestore(parent.MSBuildProjectPath);
                        _buildIntegratedProjectsUpdateSet?.Add(parent.MSBuildProjectPath);
                    }
                }

                if (dgSpecForParents.Restore.Count > 0)
                {
                    // Restore and commit the lock file to disk regardless of the result
                    // This will restore all parents in a single restore 
                    await DependencyGraphRestoreUtility.RestoreAsync(
                        dgSpecForParents,
                        referenceContext,
                        GetRestoreProviderCache(),
                        cacheContextModifier,
                        projectAction.Sources,
                        nuGetProjectContext.OperationId,
                        forceRestore: false, // No need to force restore as the inputs would've changed here anyways
                        isRestoreOriginalAction: false, // not an explicit restore request instead being done as part of install or update
                        additionalMessages: null,
                        progressReporter: RestoreProgressReporter,
                        log: logger,
                        token: token);
                }
            }
            else
            {
                // Fail and display a rollback message to let the user know they have returned to the original state
                var message = string.Format(
                        CultureInfo.InvariantCulture,
                        Strings.RestoreFailedRollingBack,
                        buildIntegratedProject.ProjectName);

                // Read additional errors from the lock file if one exists
                var logMessages = restoreResult.LockFile?
                    .LogMessages
                    .Where(e => e.Level == LogLevel.Error || e.Level == LogLevel.Warning)
                    .Select(e => e.AsRestoreLogMessage())
                  ?? Enumerable.Empty<ILogMessage>();

                // Throw an exception containing all errors, these will be displayed in the error list
                throw new PackageReferenceRollbackException(message, logMessages);
            }

            await OpenReadmeFile(buildIntegratedProject, nuGetProjectContext, token);
        }

        private static IReadOnlyList<string> GetFilesToBeUpdated(RestoreResultPair result)
        {
            List<string> filesToBeUpdated = new(3); // We know that we have 3 files.
            filesToBeUpdated.Add(result.Result.LockFilePath);

            foreach (MSBuildOutputFile msbuildOutputFile in result.Result.MSBuildOutputFiles)
            {
                filesToBeUpdated.Add(msbuildOutputFile.Path);
            }

            return filesToBeUpdated.AsReadOnly();
        }

        private async Task RollbackAsync(
            NuGetProject nuGetProject,
            Stack<NuGetProjectAction> executedNuGetProjectActions,
            HashSet<PackageIdentity> packageWithDirectoriesToBeDeleted,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            if (executedNuGetProjectActions.Count > 0)
            {
                // Only print the rollback warning if we have something to rollback
                nuGetProjectContext.Log(MessageLevel.Warning, Strings.Warning_RollingBack);
            }

            while (executedNuGetProjectActions.Count > 0)
            {
                var nuGetProjectAction = executedNuGetProjectActions.Pop();
                try
                {
                    if (nuGetProjectAction.NuGetProjectActionType == NuGetProjectActionType.Install)
                    {
                        // Rolling back an install would be to uninstall the package
                        await ExecuteUninstallAsync(nuGetProject, nuGetProjectAction.PackageIdentity, packageWithDirectoriesToBeDeleted, nuGetProjectContext, token);
                    }
                    else
                    {
                        packageWithDirectoriesToBeDeleted.Remove(nuGetProjectAction.PackageIdentity);
                        var packagePath = PackagesFolderNuGetProject.GetInstalledPackageFilePath(nuGetProjectAction.PackageIdentity);
                        if (File.Exists(packagePath))
                        {
                            using (var downloadResourceResult = new DownloadResourceResult(File.OpenRead(packagePath), nuGetProjectAction.SourceRepository?.PackageSource?.Source))
                            {
                                await ExecuteInstallAsync(nuGetProject, nuGetProjectAction.PackageIdentity, downloadResourceResult, packageWithDirectoriesToBeDeleted, nuGetProjectContext, token);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // TODO: We are ignoring exceptions on rollback. Is this OK?
                }
            }
        }

        private Task OpenReadmeFile(NuGetProject nuGetProject, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            var executionContext = nuGetProjectContext.ExecutionContext;
            if (executionContext != null
                && executionContext.DirectInstall != null)
            {
                //packagesPath is different for project.json vs Packages.config scenarios. So check if the project is a build-integrated project
                var buildIntegratedProject = nuGetProject as BuildIntegratedNuGetProject;
                var readmeFilePath = string.Empty;

                if (buildIntegratedProject != null)
                {
                    var pathContext = NuGetPathContext.Create(Settings);
                    var pathResolver = new FallbackPackagePathResolver(pathContext);
                    var identity = nuGetProjectContext.ExecutionContext.DirectInstall;
                    var packageFolderPath = pathResolver.GetPackageDirectory(identity.Id, identity.Version);

                    if (!string.IsNullOrEmpty(packageFolderPath))
                    {
                        readmeFilePath = Path.Combine(packageFolderPath, Constants.ReadmeFileName);
                    }
                }
                else
                {
                    var packagePath = PackagesFolderNuGetProject.GetInstalledPackageFilePath(executionContext.DirectInstall);

                    if (!string.IsNullOrEmpty(packagePath))
                    {
                        readmeFilePath = Path.Combine(Path.GetDirectoryName(packagePath), Constants.ReadmeFileName);
                    }
                }

                if (!token.IsCancellationRequested &&
                    !string.IsNullOrEmpty(readmeFilePath) &&
                    File.Exists(readmeFilePath))
                {
                    return executionContext.OpenFile(readmeFilePath);
                }
            }

            return TaskResult.False;
        }

        /// <summary>
        /// RestorePackage is only allowed on a folderNuGetProject. In most cases, one will simply use the
        /// packagesFolderPath from NuGetPackageManager
        /// to create a folderNuGetProject before calling into this method
        /// </summary>
        public async Task<bool> RestorePackageAsync(
            PackageIdentity packageIdentity,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext,
            IEnumerable<SourceRepository> sourceRepositories,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (PackageExistsInPackagesFolder(packageIdentity, nuGetProjectContext.PackageExtractionContext.PackageSaveMode))
            {
                return false;
            }

            token.ThrowIfCancellationRequested();
            nuGetProjectContext.Log(MessageLevel.Info, string.Format(CultureInfo.CurrentCulture, Strings.RestoringPackage, packageIdentity));
            var enabledSources = (sourceRepositories != null && sourceRepositories.Any()) ? sourceRepositories :
                SourceRepositoryProvider.GetRepositories().Where(e => e.PackageSource.IsEnabled);

            token.ThrowIfCancellationRequested();

            using (var downloadResult = await PackageDownloader.GetDownloadResourceResultAsync(
                enabledSources,
                packageIdentity,
                downloadContext,
                SettingsUtility.GetGlobalPackagesFolder(Settings),
                new ProjectContextLogger(nuGetProjectContext),
                token))
            {
                // Install package whether returned from the cache or a direct download
                await PackagesFolderNuGetProject.InstallPackageAsync(packageIdentity, downloadResult, nuGetProjectContext, token);
            }

            return true;
        }

        public Task<bool> CopySatelliteFilesAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            return PackagesFolderNuGetProject.CopySatelliteFilesAsync(packageIdentity, nuGetProjectContext, token);
        }

        /// <summary>
        /// Checks whether package exists in packages folder and verifies that nupkg and nuspec are present as specified by packageSaveMode
        /// </summary>
        public bool PackageExistsInPackagesFolder(PackageIdentity packageIdentity, PackageSaveMode packageSaveMode)
        {
            return PackagesFolderNuGetProject.PackageExists(packageIdentity, packageSaveMode);
        }

        public bool PackageExistsInPackagesFolder(PackageIdentity packageIdentity)
        {
            return PackagesFolderNuGetProject.PackageExists(packageIdentity);
        }

        private async Task ExecuteInstallAsync(
            NuGetProject nuGetProject,
            PackageIdentity packageIdentity,
            DownloadResourceResult resourceResult,
            HashSet<PackageIdentity> packageWithDirectoriesToBeDeleted,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            // TODO: EnsurePackageCompatibility check should be performed in preview. Can easily avoid a lot of rollback
            await InstallationCompatibility.EnsurePackageCompatibilityAsync(nuGetProject, packageIdentity, resourceResult, token);

            packageWithDirectoriesToBeDeleted.Remove(packageIdentity);

            await nuGetProject.InstallPackageAsync(packageIdentity, resourceResult, nuGetProjectContext, token);
        }

        private async Task ExecuteUninstallAsync(NuGetProject nuGetProject, PackageIdentity packageIdentity, HashSet<PackageIdentity> packageWithDirectoriesToBeDeleted,
            INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            // Step-1: Call nuGetProject.UninstallPackage
            await nuGetProject.UninstallPackageAsync(packageIdentity, nuGetProjectContext, token);

            // Step-2: Check if the package directory could be deleted
            if (!(nuGetProject is INuGetIntegratedProject)
                && !await PackageExistsInAnotherNuGetProject(nuGetProject, packageIdentity, SolutionManager, token, excludeIntegrated: true))
            {
                packageWithDirectoriesToBeDeleted.Add(packageIdentity);
            }
        }

        /// <summary>
        /// Checks if package <paramref name="packageIdentity" /> that is installed in
        /// project <paramref name="nuGetProject" /> is also installed in any
        /// other projects in the solution.
        /// </summary>
        public static async Task<bool> PackageExistsInAnotherNuGetProject(NuGetProject nuGetProject, PackageIdentity packageIdentity, ISolutionManager solutionManager, CancellationToken token, bool excludeIntegrated = false)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (solutionManager == null)
            {
                // If the solution manager is null, simply assume that the
                // package exists on another nuget project to not delete it
                return true;
            }

            var nuGetProjectName = NuGetProject.GetUniqueNameOrName(nuGetProject);
            foreach (var otherNuGetProject in (await solutionManager.GetNuGetProjectsAsync()))
            {
                if (excludeIntegrated && otherNuGetProject is INuGetIntegratedProject)
                {
                    continue;
                }
                var otherNuGetProjectName = NuGetProject.GetUniqueNameOrName(otherNuGetProject);
                if (otherNuGetProjectName.Equals(nuGetProjectName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                var packageExistsInAnotherNuGetProject = (await otherNuGetProject.GetInstalledPackagesAsync(token)).Any(pr => pr.PackageIdentity.Equals(packageIdentity));
                if (packageExistsInAnotherNuGetProject)
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> DeletePackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            // 1. Check if the Package exists at root, if not, return false
            if (!PackagesFolderNuGetProject.PackageExists(packageIdentity))
            {
                nuGetProjectContext.Log(ProjectManagement.MessageLevel.Warning, Strings.PackageDoesNotExistInFolder, packageIdentity, PackagesFolderNuGetProject.Root);
                return false;
            }

            nuGetProjectContext.Log(ProjectManagement.MessageLevel.Info, Strings.RemovingPackageFromFolder, packageIdentity, PackagesFolderNuGetProject.Root);
            // 2. Delete the package folder and files from the root directory of this FileSystemNuGetProject
            // Remember that the following code may throw System.UnauthorizedAccessException
            await PackagesFolderNuGetProject.DeletePackage(packageIdentity, nuGetProjectContext, token);
            nuGetProjectContext.Log(ProjectManagement.MessageLevel.Info, Strings.RemovedPackageFromFolder, packageIdentity, PackagesFolderNuGetProject.Root);
            return true;
        }

        public static Task<ResolvedPackage> GetLatestVersionAsync(
            string packageId,
            NuGetFramework framework,
            ResolutionContext resolutionContext,
            SourceRepository primarySourceRepository,
            Common.ILogger log,
            CancellationToken token)
        {
            return GetLatestVersionAsync(
                packageId,
                framework,
                resolutionContext,
                new List<SourceRepository> { primarySourceRepository },
                log,
                token);
        }

        public static Task<ResolvedPackage> GetLatestVersionAsync(
            string packageId,
            NuGetProject project,
            ResolutionContext resolutionContext,
            SourceRepository primarySourceRepository,
            Common.ILogger log,
            CancellationToken token)
        {
            NuGetFramework framework;
            if (!project.TryGetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework, out framework))
            {
                // Default to the any framework if the project does not specify a framework.
                framework = NuGetFramework.AnyFramework;
            }

            return GetLatestVersionAsync(
                packageId,
                framework,
                resolutionContext,
                new List<SourceRepository> { primarySourceRepository },
                log,
                token);
        }

        public static async Task<ResolvedPackage> GetLatestVersionAsync(
            string packageId,
            NuGetProject project,
            ResolutionContext resolutionContext,
            IEnumerable<SourceRepository> sources,
            Common.ILogger log,
            CancellationToken token)
        {
            var tasks = new List<Task<NuGetVersion>>();

            NuGetFramework framework;
            if (!project.TryGetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework, out framework))
            {
                // Default to the any framework if the project does not specify a framework.
                framework = NuGetFramework.AnyFramework;
            }

            return await GetLatestVersionAsync(packageId, framework, resolutionContext, sources, log, token);
        }

        public static async Task<ResolvedPackage> GetLatestVersionAsync(
            string packageId,
            NuGetFramework framework,
            ResolutionContext resolutionContext,
            IEnumerable<SourceRepository> sources,
            Common.ILogger log,
            CancellationToken token)
        {
            var tasks = new List<Task<ResolvedPackage>>();
            NuGetVersion version = null;
            foreach (var source in sources)
            {
                tasks.Add(Task.Run(async ()
                    => await GetLatestVersionCoreAsync(packageId, version, framework, resolutionContext, source, log, token)));
            }

            var resolvedPackages = await Task.WhenAll(tasks);
            var latestVersion = resolvedPackages.Select(v => v.LatestVersion).Where(v => v != null).Max();

            return new ResolvedPackage(latestVersion, resolvedPackages.Any(p => p.Exists));
        }

        public static async Task<ResolvedPackage> GetLatestVersionAsync(
            PackageReference package,
            NuGetFramework framework,
            ResolutionContext resolutionContext,
            IEnumerable<SourceRepository> sources,
            Common.ILogger log,
            CancellationToken token)
        {
            var tasks = new List<Task<ResolvedPackage>>();
            foreach (var source in sources)
            {
                tasks.Add(Task.Run(async ()
                    => await GetLatestVersionCoreAsync(package.PackageIdentity.Id, package.PackageIdentity.Version, framework, resolutionContext, source, log, token)));
            }

            var resolvedPackages = await Task.WhenAll(tasks);

            var latestVersion = resolvedPackages
                .Select(v => v.LatestVersion)
                .Where(v => v != null && (package.AllowedVersions == null || package.AllowedVersions.Satisfies(v))).Max();

            return new ResolvedPackage(latestVersion, resolvedPackages.Any(p => p.Exists));
        }

        private static async Task<ResolvedPackage> GetLatestVersionCoreAsync(
            string packageId,
            NuGetVersion version,
            NuGetFramework framework,
            ResolutionContext resolutionContext,
            SourceRepository source,
            Common.ILogger log,
            CancellationToken token)
        {
            var dependencyInfoResource = await source.GetResourceAsync<DependencyInfoResource>();

            // Resolve the package for the project framework and cache the results in the
            // resolution context for the gather to use during the next step.
            // Using the metadata resource will result in multiple calls to the same url during an install.
            var packages = (await dependencyInfoResource.ResolvePackages(packageId, framework, resolutionContext.SourceCacheContext, log, token)).ToList();

            Debug.Assert(resolutionContext.GatherCache != null);

            // Cache the results, even if the package was not found.
            resolutionContext.GatherCache.AddAllPackagesForId(
                source.PackageSource,
                packageId,
                framework,
                packages);

            if (version != null)
            {
                packages = PrunePackageTree.PruneByUpdateConstraints(packages, version, resolutionContext.VersionConstraints).ToList();
            }

            // Find the latest version
            var latestVersion = packages.Where(package => (package.Listed || resolutionContext.IncludeUnlisted)
                && (resolutionContext.IncludePrerelease || !package.Version.IsPrerelease))
                .OrderByDescending(package => package.Version, VersionComparer.Default)
                .Select(package => package.Version)
                .FirstOrDefault();

            return new ResolvedPackage(latestVersion, packages.Count > 0);
        }

        private IEnumerable<SourceRepository> GetEffectiveSources(IEnumerable<SourceRepository> primarySources, IEnumerable<SourceRepository> secondarySources)
        {
            // Always have to add the packages folder as the primary repository so that
            // dependency info for an installed package that is unlisted from the server is still available :(

            // count = primarySources.Count + secondarySources.Count + 1 for PackagesFolderSourceRepository
            var count = (primarySources?.Count() ?? 0) +
                (secondarySources?.Count() ?? 0)
                + 1;

            var effectiveSources = new List<SourceRepository>(count)
            {
                PackagesFolderSourceRepository
            };

            if (primarySources != null)
            {
                effectiveSources.AddRange(primarySources);
            }
            if (secondarySources != null)
            {
                effectiveSources.AddRange(secondarySources);
            }

            return new HashSet<SourceRepository>(effectiveSources, SourceRepositoryComparer.Instance);
        }

        public static void SetDirectInstall(PackageIdentity directInstall,
            INuGetProjectContext nuGetProjectContext)
        {
            if (directInstall != null
                && nuGetProjectContext != null
                && nuGetProjectContext.ExecutionContext != null)
            {
                var ideExecutionContext = nuGetProjectContext.ExecutionContext as IDEExecutionContext;
                if (ideExecutionContext != null)
                {
                    ideExecutionContext.IDEDirectInstall = directInstall;
                }
            }
        }

        public static void ClearDirectInstall(INuGetProjectContext nuGetProjectContext)
        {
            if (nuGetProjectContext != null
                && nuGetProjectContext.ExecutionContext != null)
            {
                var ideExecutionContext = nuGetProjectContext.ExecutionContext as IDEExecutionContext;
                if (ideExecutionContext != null)
                {
                    ideExecutionContext.IDEDirectInstall = null;
                }
            }
        }

        private RestoreCommandProvidersCache GetRestoreProviderCache()
        {
            if (_restoreProviderCache == null)
            {
                _restoreProviderCache = new RestoreCommandProvidersCache();
            }

            return _restoreProviderCache;
        }
    }
}
