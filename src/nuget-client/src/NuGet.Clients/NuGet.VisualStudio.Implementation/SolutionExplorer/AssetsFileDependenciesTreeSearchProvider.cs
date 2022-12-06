// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using NuGet.VisualStudio.SolutionExplorer.Models;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Implementation of <see cref="IDependenciesTreeSearchProvider"/> that searches an <see cref="AssetsFileDependenciesSnapshot"/>
    /// for items that match the search string.
    /// </summary>
    [Export(typeof(IDependenciesTreeSearchProvider))]
    internal sealed class AssetsFileDependenciesTreeSearchProvider : IDependenciesTreeSearchProvider
    {
        private readonly FileOpener _fileOpener;
        private readonly IFileIconProvider _fileIconProvider;

        [ImportingConstructor]
        public AssetsFileDependenciesTreeSearchProvider(
            FileOpener fileOpener,
            IFileIconProvider fileIconProvider)
        {
            _fileOpener = fileOpener;
            _fileIconProvider = fileIconProvider;
        }

        public async Task SearchAsync(IDependenciesTreeProjectSearchContext context)
        {
            // get latest snapshot
            ExportProvider exportProvider = context.UnconfiguredProject.Services.ExportProvider;

            Lazy<IAssetsFileDependenciesDataSource, IAppliesToMetadataView> dataSource
                = exportProvider
                    .GetExports<IAssetsFileDependenciesDataSource, IAppliesToMetadataView>()
                    .SingleOrDefault(export => export.Metadata.AppliesTo(context.UnconfiguredProject.Capabilities));

            if (dataSource == null)
            {
                // dataSource will be null for shared projects, for example
                return;
            }

            IProjectDataSourceRegistry? dataSourceRegistry = context.UnconfiguredProject.Services.DataSourceRegistry;
            Assumes.Present(dataSourceRegistry);

            AssetsFileDependenciesSnapshot snapshot = (await dataSource.Value.GetLatestVersionAsync<AssetsFileDependenciesSnapshot>(dataSourceRegistry, cancellationToken: context.CancellationToken)).Value;

            if (context.UnconfiguredProject.Services.ExportProvider.GetExportedValue<IActiveConfigurationGroupService>() is not IActiveConfigurationGroupService3 activeConfigurationGroupService)
            {
                return;
            }

            IConfigurationGroup<ConfiguredProject> configuredProjects = await activeConfigurationGroupService.GetActiveLoadedConfiguredProjectGroupAsync();

            foreach ((_, AssetsFileTarget target) in snapshot.DataByTarget)
            {
                ConfiguredProject? configuredProject = await FindConfiguredProjectAsync(target.TargetFrameworkMoniker);

                if (configuredProject == null)
                {
                    continue;
                }

                IDependenciesTreeConfiguredProjectSearchContext? targetContext = await context.ForConfiguredProjectAsync(configuredProject);

                if (targetContext == null)
                {
                    continue;
                }

                foreach ((_, AssetsFileTargetLibrary library) in target.LibraryByName)
                {
                    if (context.CancellationToken.IsCancellationRequested)
                    {
                        // Search was cancelled
                        return;
                    }

                    if (targetContext.IsMatch(library.Name))
                    {
                        targetContext.SubmitResult(CreateLibraryItem(library));
                    }

                    SearchAssemblies(library, library.CompileTimeAssemblies, PackageAssemblyGroupType.CompileTime);
                    SearchAssemblies(library, library.FrameworkAssemblies, PackageAssemblyGroupType.Framework);
                    SearchContentFiles(library);
                    SearchBuildFiles(library, library.BuildFiles, PackageBuildFileGroupType.Build);
                    SearchBuildFiles(library, library.BuildMultiTargetingFiles, PackageBuildFileGroupType.BuildMultiTargeting);
                    SearchDocuments(library);
                }

                SearchLogMessages();

                continue;

                async Task<ConfiguredProject?> FindConfiguredProjectAsync(string tfm)
                {
                    foreach (ConfiguredProject configuredProject in configuredProjects)
                    {
                        if (configuredProject.Services.ProjectSubscription == null)
                        {
                            continue;
                        }

                        IProjectSubscriptionUpdate subscriptionUpdate = (await configuredProject.Services.ProjectSubscription.ProjectRuleSource.GetLatestVersionAsync(configuredProject, cancellationToken: context.CancellationToken)).Value;

                        if (subscriptionUpdate.CurrentState.TryGetValue(NuGetRestoreRule.SchemaName, out IProjectRuleSnapshot nuGetRestoreSnapshot) &&
                            nuGetRestoreSnapshot.Properties.TryGetValue(NuGetRestoreRule.NuGetTargetMonikerProperty, out string nuGetTargetMoniker) &&
                            StringComparer.OrdinalIgnoreCase.Equals(nuGetTargetMoniker, tfm))
                        {
                            // Assets file 'target' string matches the configured project's NuGetTargetMoniker property value
                            return configuredProject;
                        }

                        if (subscriptionUpdate.CurrentState.TryGetValue(ConfigurationGeneralRule.SchemaName, out IProjectRuleSnapshot configurationGeneralSnapshot))
                        {
                            if (configurationGeneralSnapshot.Properties.TryGetValue(ConfigurationGeneralRule.TargetFrameworkMonikerProperty, out string targetFrameworkMoniker) &&
                                StringComparer.OrdinalIgnoreCase.Equals(targetFrameworkMoniker, tfm))
                            {
                                // Assets file 'target' string matches the configured project's TargetFrameworkMoniker property value
                                return configuredProject;
                            }

                            if (configurationGeneralSnapshot.Properties.TryGetValue(ConfigurationGeneralRule.TargetFrameworkProperty, out string targetFramework) &&
                                StringComparer.OrdinalIgnoreCase.Equals(targetFramework, tfm))
                            {
                                // Assets file 'target' string matches the configured project's TargetFramework property value
                                return configuredProject;
                            }
                        }
                    }

                    // No project found
                    return null;
                }

                void SearchAssemblies(AssetsFileTargetLibrary library, ImmutableArray<string> assemblies, PackageAssemblyGroupType groupType)
                {
                    foreach (string assembly in assemblies)
                    {
                        if (targetContext.IsMatch(Path.GetFileName(assembly)))
                        {
                            targetContext.SubmitResult(new PackageAssemblyItem(target, library, assembly, groupType));
                        }
                    }
                }

                void SearchContentFiles(AssetsFileTargetLibrary library)
                {
                    foreach (AssetsFileTargetLibraryContentFile contentFile in library.ContentFiles)
                    {
                        if (targetContext.IsMatch(contentFile.Path))
                        {
                            targetContext.SubmitResult(new PackageContentFileItem(target, library, contentFile, _fileIconProvider));
                        }
                    }
                }

                void SearchBuildFiles(AssetsFileTargetLibrary library, ImmutableArray<string> buildFiles, PackageBuildFileGroupType groupType)
                {
                    foreach (string buildFile in buildFiles)
                    {
                        if (targetContext.IsMatch(Path.GetFileName(buildFile)))
                        {
                            targetContext.SubmitResult(new PackageBuildFileItem(target, library, buildFile, groupType, _fileOpener));
                        }
                    }
                }

                IRelatableItem CreateLibraryItem(AssetsFileTargetLibrary library)
                {
                    return library.Type switch
                    {
                        AssetsFileLibraryType.Package => new PackageReferenceItem(target, library),
                        AssetsFileLibraryType.Project => new ProjectReferenceItem(target, library),
                        _ => throw Assumes.NotReachable()
                    };
                }

                void SearchLogMessages()
                {
                    foreach (AssetsFileLogMessage log in target.Logs)
                    {
                        if (targetContext.IsMatch(log.Message))
                        {
                            targetContext.SubmitResult(CreateLogItem(log));
                        }
                    }

                    DiagnosticItem? CreateLogItem(AssetsFileLogMessage log)
                    {
                        if (target.LibraryByName.TryGetValue(log.LibraryName, out AssetsFileTargetLibrary? library))
                        {
                            return new DiagnosticItem(target, library, log);
                        }

                        return null;
                    }
                }

                void SearchDocuments(AssetsFileTargetLibrary library)
                {
                    foreach (string path in library.DocumentationFiles)
                    {
                        if (targetContext.IsMatch(path))
                        {
                            targetContext.SubmitResult(new PackageDocumentItem(target, library, path, _fileOpener, _fileIconProvider));
                        }
                    }
                }
            }
        }
    }
}
