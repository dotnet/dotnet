// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;

namespace NuGet.Commands
{
    /// <summary>
    /// In Memory dg file provider.
    /// </summary>
    public class DependencyGraphSpecRequestProvider : IPreLoadedRestoreRequestProvider
    {
        private const bool DefaultRestoreLegacyPackagesDirectory = false;

        private readonly DependencyGraphSpec _dgFile;
        private readonly RestoreCommandProvidersCache _providerCache;
        private readonly LockFileBuilderCache _lockFileBuilderCache;
        private readonly ISettings _settings;

        public DependencyGraphSpecRequestProvider(
            RestoreCommandProvidersCache providerCache,
            DependencyGraphSpec dgFile)
        {
            _dgFile = dgFile;
            _providerCache = providerCache;
            _lockFileBuilderCache = new LockFileBuilderCache();
        }

        public DependencyGraphSpecRequestProvider(
            RestoreCommandProvidersCache providerCache,
            DependencyGraphSpec dgFile,
            ISettings settings)
            : this(providerCache, dgFile)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public Task<IReadOnlyList<RestoreSummaryRequest>> CreateRequests(
            RestoreArgs restoreContext)
        {
            var requests = GetRequestsFromItems(restoreContext, _dgFile);

            return Task.FromResult(requests);
        }

        private IReadOnlyList<RestoreSummaryRequest> GetRequestsFromItems(RestoreArgs restoreContext, DependencyGraphSpec dgFile)
        {
            if (restoreContext == null)
            {
                throw new ArgumentNullException(nameof(restoreContext));
            }

            if (dgFile == null)
            {
                throw new ArgumentNullException(nameof(dgFile));
            }

            // Validate the dg file input, this throws if errors are found.
            var projectsWithErrors = new HashSet<string>();
            if (restoreContext.AdditionalMessages != null)
            {
                foreach (var projectPath in restoreContext.AdditionalMessages.Where(m => m.Level == Common.LogLevel.Error).Select(m => m.ProjectPath))
                {
                    projectsWithErrors.Add(projectPath);
                }
            }
            SpecValidationUtility.ValidateDependencySpec(dgFile, projectsWithErrors);

            // Create requests
            var requests = new ConcurrentBag<RestoreSummaryRequest>();
            var toolRequests = new ConcurrentBag<RestoreSummaryRequest>();

            var parallelOptions = new ParallelOptions
            {
                // By default, max degree of parallelism is -1 which means no upper bound.
                // Limiting to processor count reduces task context switching which is better
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            using (var settingsLoadingContext = new SettingsLoadingContext())
            {
                // Parallel.Foreach has an optimization for Arrays, so calling .ToArray() is better and adds almost no overhead
                Parallel.ForEach(dgFile.Restore.ToArray(), parallelOptions, projectNameToRestore =>
                {
                    IReadOnlyList<PackageSpec> closure = dgFile.GetClosure(projectNameToRestore);
                    DependencyGraphSpec projectDependencyGraphSpec = dgFile.CreateFromClosure(projectNameToRestore, closure);

                    var externalClosure = new HashSet<ExternalProjectReference>(closure.Select(GetExternalProject));

                    ExternalProjectReference rootProject = externalClosure.Single(p =>
                        StringComparer.Ordinal.Equals(projectNameToRestore, p.UniqueName));

                    RestoreSummaryRequest request = Create(
                        projectNameToRestore,
                        rootProject,
                        externalClosure,
                        restoreContext,
                        projectDependencyGraphSpec,
                        settingsLoadingContext);

                    if (request.Request.ProjectStyle == ProjectStyle.DotnetCliTool)
                    {
                        // Store tool requests to be filtered later
                        toolRequests.Add(request);
                    }
                    else
                    {
                        requests.Add(request);
                    }
                });
            }

            // Filter out duplicate tool restore requests
            foreach (RestoreSummaryRequest subSetRequest in ToolRestoreUtility.GetSubSetRequests(toolRequests))
            {
                requests.Add(subSetRequest);
            }

            return requests.ToArray();
        }

        public static IEnumerable<ExternalProjectReference> GetExternalClosure(DependencyGraphSpec dgFile, string projectNameToRestore)
        {
            IReadOnlyList<PackageSpec> closure = dgFile.GetClosure(projectNameToRestore);

            return closure.Select(GetExternalProject);
        }

        private static ExternalProjectReference GetExternalProject(PackageSpec rootProject)
        {
            var projectReferences = rootProject.RestoreMetadata?.TargetFrameworks.SelectMany(e => e.ProjectReferences)
                ?? new List<ProjectRestoreReference>();

            var type = rootProject.RestoreMetadata?.ProjectStyle ?? ProjectStyle.Unknown;

            var uniqueReferences = projectReferences
                .Select(p => p.ProjectUniqueName)
                .Distinct(StringComparer.OrdinalIgnoreCase);

            return new ExternalProjectReference(
                rootProject.RestoreMetadata.ProjectUniqueName,
                rootProject,
                rootProject.RestoreMetadata?.ProjectPath,
                uniqueReferences);
        }

        private RestoreSummaryRequest Create(
            string projectNameToRestore,
            ExternalProjectReference project,
            HashSet<ExternalProjectReference> projectReferenceClosure,
            RestoreArgs restoreArgs,
            DependencyGraphSpec projectDgSpec,
            SettingsLoadingContext settingsLoadingContext)
        {
            var projectPackageSpec = projectDgSpec.GetProjectSpec(projectNameToRestore);
            //fallback paths, global packages path and sources need to all be passed in the dg spec
            var fallbackPaths = projectPackageSpec.RestoreMetadata.FallbackFolders;
            var globalPath = GetPackagesPath(restoreArgs, projectPackageSpec);
            var settings = _settings ?? Settings.LoadImmutableSettingsGivenConfigPaths(projectPackageSpec.RestoreMetadata.ConfigFilePaths, settingsLoadingContext);
            var packageSources = restoreArgs.GetEffectiveSources(settings, projectPackageSpec.RestoreMetadata.Sources);
            var auditSources = GetAuditSources(restoreArgs.CachingSourceProvider);
            var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, restoreArgs.Log);
            var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(settings);
            var updateLastAccess = SettingsUtility.GetUpdatePackageLastAccessTimeEnabledStatus(settings);

            var sharedCache = _providerCache.GetOrCreate(
                globalPath,
                fallbackPaths.AsList(),
                packageSources,
                auditSources,
                restoreArgs.CacheContext,
                restoreArgs.Log,
                updateLastAccess);

            var rootPath = Path.GetDirectoryName(project.PackageSpec.FilePath);

            IReadOnlyList<IAssetsLogMessage> projectAdditionalMessages = GetMessagesForProject(restoreArgs.AdditionalMessages, project.PackageSpec.FilePath);

            // Create request
            var request = new RestoreRequest(
                project.PackageSpec,
                sharedCache,
                restoreArgs.CacheContext,
                clientPolicyContext,
                packageSourceMapping,
                restoreArgs.Log,
                _lockFileBuilderCache)
            {
                // Set properties from the restore metadata
                ProjectStyle = project.PackageSpec.RestoreMetadata.ProjectStyle,
                //  Project.json is special cased to put assets file and generated .props and targets in the project folder
                RestoreOutputPath = project.PackageSpec.RestoreMetadata.ProjectStyle == ProjectStyle.ProjectJson ? rootPath : project.PackageSpec.RestoreMetadata.OutputPath,
                DependencyGraphSpec = projectDgSpec,
                MSBuildProjectExtensionsPath = projectPackageSpec.RestoreMetadata.OutputPath,
                AdditionalMessages = projectAdditionalMessages,
                UpdatePackageLastAccessTime = updateLastAccess,
            };

            var restoreLegacyPackagesDirectory = project.PackageSpec?.RestoreMetadata?.LegacyPackagesDirectory
                ?? DefaultRestoreLegacyPackagesDirectory;
            request.IsLowercasePackagesDirectory = !restoreLegacyPackagesDirectory;

            // Standard properties
            restoreArgs.ApplyStandardProperties(request);

            // Add project references
            request.ExternalProjects = projectReferenceClosure.ToList();

            // The lock file is loaded later since this is an expensive operation
            var summaryRequest = new RestoreSummaryRequest(
                request,
                project.MSBuildProjectPath,
                settings.GetConfigFilePaths(),
                packageSources);

            return summaryRequest;
        }

        private static IReadOnlyList<SourceRepository> GetAuditSources(CachingSourceProvider cachingSourceProvider)
        {
            IReadOnlyList<PackageSource> auditSources = cachingSourceProvider.PackageSourceProvider.LoadAuditSources();

            if (auditSources is null || auditSources.Count == 0)
            {
                return Array.Empty<SourceRepository>();
            }

            var auditSourceRepositories = new List<SourceRepository>(auditSources.Count);
            for (int i = 0; i < auditSources.Count; i++)
            {
                PackageSource source = auditSources[i];
                if (source.IsEnabled)
                {
                    auditSourceRepositories.Add(cachingSourceProvider.CreateRepository(source));
                }
            }

            return auditSourceRepositories;
        }

        private string GetPackagesPath(RestoreArgs restoreArgs, PackageSpec project)
        {
            if (!string.IsNullOrEmpty(restoreArgs.GlobalPackagesFolder))
            {
                project.RestoreMetadata.PackagesPath = restoreArgs.GlobalPackagesFolder;
            }
            return project.RestoreMetadata.PackagesPath;
        }

        /// <summary>
        /// Return all references for a given project path.
        /// References is modified by this method.
        /// This includes the root project.
        /// </summary>
        private static void CollectReferences(
            ExternalProjectReference root,
            Dictionary<string, ExternalProjectReference> allProjects,
            HashSet<ExternalProjectReference> references)
        {
            if (references.Add(root))
            {
                foreach (var child in root.ExternalProjectReferences)
                {
                    ExternalProjectReference childProject;
                    if (!allProjects.TryGetValue(child, out childProject))
                    {
                        // Let the resolver handle this later
                        Debug.Fail($"Missing project {childProject}");
                    }

                    // Recurse down
                    CollectReferences(childProject, allProjects, references);
                }
            }
        }

        internal static IReadOnlyList<IAssetsLogMessage> GetMessagesForProject(IReadOnlyList<IAssetsLogMessage> allMessages, string projectPath)
        {
            List<IAssetsLogMessage> projectAdditionalMessages = null;

            if (allMessages != null)
            {
                foreach (var message in allMessages)
                {
                    if (message.ProjectPath == projectPath)
                    {
                        if (projectAdditionalMessages == null)
                        {
                            projectAdditionalMessages = new List<IAssetsLogMessage>();
                        }

                        projectAdditionalMessages.Add(message);
                    }
                }
            }

            return projectAdditionalMessages;
        }
    }
}
