// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Supporting methods for restoring sets of projects that implement <see cref="IDependencyGraphProject"/>. This
    /// code is used by Visual Studio to execute restores for solutions that have mixtures of UWP project.json,
    /// packages.config, and PackageReference-type projects.
    /// </summary>
    public static class DependencyGraphRestoreUtility
    {
        /// <summary>
        /// Restore a solution and cache the dg spec to context.
        /// </summary>
        public static Task<IReadOnlyList<RestoreSummary>> RestoreAsync(
            ISolutionManager solutionManager,
            DependencyGraphSpec dgSpec,
            DependencyGraphCacheContext context,
            RestoreCommandProvidersCache providerCache,
            Action<SourceCacheContext> cacheContextModifier,
            IEnumerable<SourceRepository> sources,
            Guid parentId,
            bool forceRestore,
            bool isRestoreOriginalAction,
            ILogger log,
            CancellationToken token)
        {
            return RestoreAsync(
                dgSpec,
                context,
                providerCache,
                cacheContextModifier,
                sources,
                parentId,
                forceRestore,
                isRestoreOriginalAction,
                additionalMessages: null,
                progressReporter: null,
                log,
                token);
        }

        /// <summary>
        /// Restore a solution and cache the dg spec to context.
        /// </summary>
        [Obsolete("This method will be removed in a future release. Use other one of the other RestoreAsync methods.")]
        public static Task<IReadOnlyList<RestoreSummary>> RestoreAsync(
            ISolutionManager solutionManager,
            DependencyGraphSpec dgSpec,
            DependencyGraphCacheContext context,
            RestoreCommandProvidersCache providerCache,
            Action<SourceCacheContext> cacheContextModifier,
            IEnumerable<SourceRepository> sources,
            Guid parentId,
            bool forceRestore,
            bool isRestoreOriginalAction,
            IReadOnlyList<IAssetsLogMessage> additionalMessages,
            ILogger log,
            CancellationToken token)
        {
            return RestoreAsync(
                dgSpec,
                context,
                providerCache,
                cacheContextModifier,
                sources,
                parentId,
                forceRestore,
                isRestoreOriginalAction,
                additionalMessages,
                progressReporter: null,
                log,
                token
                );
        }

        /// <summary>
        /// Restore a solution and cache the dg spec to context.
        /// </summary>
        public static async Task<IReadOnlyList<RestoreSummary>> RestoreAsync(
            DependencyGraphSpec dgSpec,
            DependencyGraphCacheContext context,
            RestoreCommandProvidersCache providerCache,
            Action<SourceCacheContext> cacheContextModifier,
            IEnumerable<SourceRepository> sources,
            Guid parentId,
            bool forceRestore,
            bool isRestoreOriginalAction,
            IReadOnlyList<IAssetsLogMessage> additionalMessages,
            IRestoreProgressReporter progressReporter,
            ILogger log,
            CancellationToken token)
        {
            // Check if there are actual projects to restore before running.
            if (dgSpec.Restore.Count > 0)
            {
                using (var sourceCacheContext = new SourceCacheContext())
                {
                    // Update cache context
                    cacheContextModifier(sourceCacheContext);

                    var restoreContext = GetRestoreArgs(
                        context,
                        providerCache,
                        sourceCacheContext,
                        sources,
                        dgSpec,
                        parentId,
                        forceRestore,
                        isRestoreOriginalAction,
                        restoreForceEvaluate: false,
                        additionalMessages,
                        progressReporter: progressReporter);

                    var restoreSummaries = await RestoreRunner.RunAsync(restoreContext, token);

                    RestoreSummary.Log(log, restoreSummaries);

                    return restoreSummaries;
                }
            }

            return new List<RestoreSummary>();
        }

        [Obsolete]
        public static string GetDefaultDGSpecFileName()
        {
            return Path.Combine(
                        NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp),
                        "nuget-dg",
                        "nugetSpec.dg");
        }

        /// <summary>
        /// Restore a project without writing the lock file
        /// </summary>
        internal static async Task<RestoreResultPair> PreviewRestoreAsync(
            ISolutionManager solutionManager,
            BuildIntegratedNuGetProject project,
            PackageSpec packageSpec,
            DependencyGraphCacheContext context,
            RestoreCommandProvidersCache providerCache,
            Action<SourceCacheContext> cacheContextModifier,
            IEnumerable<SourceRepository> sources,
            Guid parentId,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Add the new spec to the dg file and fill in the rest.
            var dgFile = await GetSolutionRestoreSpec(solutionManager, context);

            dgFile = dgFile.WithoutRestores()
                .WithReplacedSpec(packageSpec);

            dgFile.AddRestore(project.MSBuildProjectPath);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                // Update cache context
                cacheContextModifier(sourceCacheContext);

                // Settings passed here will be used to populate the restore requests.
                var restoreContext = GetRestoreArgs(
                    context,
                    providerCache,
                    sourceCacheContext,
                    sources,
                    dgFile,
                    parentId,
                    forceRestore: true,
                    isRestoreOriginalAction: false,
                    restoreForceEvaluate: true,
                    additionalMessasges: null,
                    progressReporter: null);

                var requests = await RestoreRunner.GetRequests(restoreContext);
                var results = await RestoreRunner.RunWithoutCommit(requests, restoreContext);
                return results.Single();
            }
        }

        /// <summary>
        /// Restore many projects without writing the lock file
        /// SourceRepositories(sources) is only used for the CachingSourceProvider, the project-specific sources will still be resolved in RestoreRunner.
        /// </summary>
        internal static async Task<IEnumerable<RestoreResultPair>> PreviewRestoreProjectsAsync(
            ISolutionManager solutionManager,
            IEnumerable<BuildIntegratedNuGetProject> projects,
            IEnumerable<PackageSpec> updatedNugetPackageSpecs,
            DependencyGraphCacheContext context,
            RestoreCommandProvidersCache providerCache,
            Action<SourceCacheContext> cacheContextModifier,
            IEnumerable<SourceRepository> sources,
            Guid parentId,
            ILogger log,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // Add the new spec to the dg file and fill in the rest.
            var dgFile = await GetSolutionRestoreSpec(solutionManager, context);

            dgFile = dgFile.WithoutRestores()
                .WithPackageSpecs(updatedNugetPackageSpecs);

            foreach (var project in projects)
            {
                dgFile.AddRestore(project.MSBuildProjectPath);
            }

            using (var sourceCacheContext = new SourceCacheContext())
            {
                // Update cache context
                cacheContextModifier(sourceCacheContext);

                // Settings passed here will be used to populate the restore requests.
                var restoreContext = GetRestoreArgs(
                    context,
                    providerCache,
                    sourceCacheContext,
                    sources,
                    dgFile,
                    parentId,
                    forceRestore: true,
                    isRestoreOriginalAction: false,
                    restoreForceEvaluate: true,
                    additionalMessasges: null,
                    progressReporter: null);

                var requests = await RestoreRunner.GetRequests(restoreContext);
                var results = await RestoreRunner.RunWithoutCommit(requests, restoreContext);
                return results;
            }
        }

        /// <summary>
        /// Restore a build integrated project(PackageReference and Project.Json only) and update the assets file
        /// </summary>
        [Obsolete("This is an unused method and will be removed in a future release.")]
        public static async Task<RestoreResult> RestoreProjectAsync(
            ISolutionManager solutionManager,
            BuildIntegratedNuGetProject project,
            DependencyGraphCacheContext context,
            RestoreCommandProvidersCache providerCache,
            Action<SourceCacheContext> cacheContextModifier,
            IEnumerable<SourceRepository> sources,
            Guid parentId,
            ILogger log,
            CancellationToken token)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            // Restore
            var specs = await project.GetPackageSpecsAsync(context);
            var spec = specs.Single(e => e.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference
                || e.RestoreMetadata.ProjectStyle == ProjectStyle.ProjectJson); // Do not restore global tools Project Style in VS. 

            var result = await PreviewRestoreAsync(
                solutionManager,
                project,
                spec,
                context,
                providerCache,
                cacheContextModifier,
                sources,
                parentId,
                token);

            // Throw before writing if this has been canceled
            token.ThrowIfCancellationRequested();

            // Write out the lock file and msbuild files
            var summary = await RestoreRunner.CommitAsync(result, token);

            RestoreSummary.Log(log, new[] { summary });

            return result.Result;
        }

        public static bool IsRestoreRequired(
            DependencyGraphSpec solutionDgSpec)
        {
            if (solutionDgSpec.Restore.Count < 1)
            {
                // Nothing to restore
                return false;
            }
            // NO Op will be checked in the restore command 
            return true;
        }

        public static async Task<PackageSpec> GetProjectSpec(IDependencyGraphProject project, DependencyGraphCacheContext context)
        {
            var specs = await project.GetPackageSpecsAsync(context);

            var projectSpec = specs
                .FirstOrDefault(e => e.RestoreMetadata.ProjectStyle != ProjectStyle.Standalone
                                     && e.RestoreMetadata.ProjectStyle != ProjectStyle.DotnetCliTool);

            return projectSpec;
        }

        public static async Task<DependencyGraphSpec> GetSolutionRestoreSpec(
            ISolutionManager solutionManager,
            DependencyGraphCacheContext context)
        {
            var (dgSpec, _) = await GetSolutionRestoreSpecAndAdditionalMessages(solutionManager, context);
            return dgSpec;
        }

        public static async Task<(DependencyGraphSpec dgSpec, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetSolutionRestoreSpecAndAdditionalMessages(
            ISolutionManager solutionManager,
            DependencyGraphCacheContext context)
        {
            var dgSpec = new DependencyGraphSpec();
            List<IAssetsLogMessage> allAdditionalMessages = null;

            var projects = (await solutionManager.GetNuGetProjectsAsync()).OfType<IDependencyGraphProject>().ToList();
            var knownProjects = new HashSet<string>(projects.Select(e => e.MSBuildProjectPath), PathUtility.GetStringComparerBasedOnOS());

            for (var i = 0; i < projects.Count; i++)
            {
                var (packageSpecs, projectAdditionalMessages) = await projects[i].GetPackageSpecsAndAdditionalMessagesAsync(context);

                if (projectAdditionalMessages != null && projectAdditionalMessages.Count > 0)
                {
                    if (allAdditionalMessages == null)
                    {
                        allAdditionalMessages = new List<IAssetsLogMessage>();
                    }

                    allAdditionalMessages.AddRange(projectAdditionalMessages);
                }

                foreach (var packageSpec in packageSpecs)
                {
                    dgSpec.AddProject(packageSpec);

                    if (packageSpec.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference ||
                        packageSpec.RestoreMetadata.ProjectStyle == ProjectStyle.ProjectJson ||
                        packageSpec.RestoreMetadata.ProjectStyle == ProjectStyle.DotnetCliTool ||
                        packageSpec.RestoreMetadata.ProjectStyle == ProjectStyle.Standalone) // Don't add global tools to restore specs for solutions
                    {
                        dgSpec.AddRestore(packageSpec.RestoreMetadata.ProjectUniqueName);

                        var projFileName = Path.GetFileName(packageSpec.RestoreMetadata.ProjectPath);
                        var dgFileName = DependencyGraphSpec.GetDGSpecFileName(projFileName);
                        var outputPath = packageSpec.RestoreMetadata.OutputPath;

                        if (!string.IsNullOrEmpty(outputPath))
                        {
                            for (int frameworkCount = 0; frameworkCount < packageSpec.RestoreMetadata.TargetFrameworks.Count; frameworkCount++)
                            {
                                for (var projectReferenceCount = 0; projectReferenceCount < packageSpec.RestoreMetadata.TargetFrameworks[frameworkCount].ProjectReferences.Count; projectReferenceCount++)
                                {
                                    if (!knownProjects.Contains(packageSpec.RestoreMetadata.TargetFrameworks[frameworkCount].ProjectReferences[projectReferenceCount].ProjectPath))
                                    {
                                        var persistedDGSpecPath = Path.Combine(outputPath, dgFileName);
                                        if (File.Exists(persistedDGSpecPath))
                                        {
                                            var persistedDGSpec = DependencyGraphSpec.Load(persistedDGSpecPath);
                                            foreach (var dependentPackageSpec in persistedDGSpec.Projects.Where(e => !knownProjects.Contains(e.RestoreMetadata.ProjectPath)))
                                            {
                                                // Include all the missing projects from the closure.
                                                // Figuring out exactly what we need would be too and an overkill. That will happen later in the DependencyGraphSpecRequestProvider
                                                knownProjects.Add(dependentPackageSpec.RestoreMetadata.ProjectPath);
                                                dgSpec.AddProject(dependentPackageSpec);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Return dg file
            return (dgSpec, allAdditionalMessages);
        }

        /// <summary>
        /// Create a restore args.
        /// </summary>
        private static RestoreArgs GetRestoreArgs(
            DependencyGraphCacheContext context,
            RestoreCommandProvidersCache providerCache,
            SourceCacheContext sourceCacheContext,
            IEnumerable<SourceRepository> sources,
            DependencyGraphSpec dgFile,
            Guid parentId,
            bool forceRestore,
            bool isRestoreOriginalAction,
            bool restoreForceEvaluate,
            IReadOnlyList<IAssetsLogMessage> additionalMessasges,
            IRestoreProgressReporter progressReporter)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var caching = new CachingSourceProvider(new PackageSourceProvider(context.Settings, enablePackageSourcesChangedEvent: false));
#pragma warning restore CS0618 // Type or member is obsolete
            foreach (var source in sources)
            {
                caching.AddSourceRepository(source);
            }

            var dgProvider = new DependencyGraphSpecRequestProvider(providerCache, dgFile, context.Settings);

            var restoreContext = new RestoreArgs()
            {
                CacheContext = sourceCacheContext,
                PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>() { dgProvider },
                Log = context.Logger,
                AllowNoOp = !forceRestore,
                CachingSourceProvider = caching,
                ParentId = parentId,
                IsRestoreOriginalAction = isRestoreOriginalAction,
                RestoreForceEvaluate = restoreForceEvaluate,
                AdditionalMessages = additionalMessasges,
                ProgressReporter = progressReporter,
            };

            return restoreContext;
        }
    }
}
