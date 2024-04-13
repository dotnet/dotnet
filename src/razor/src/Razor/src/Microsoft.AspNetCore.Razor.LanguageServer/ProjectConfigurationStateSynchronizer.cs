// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class ProjectConfigurationStateSynchronizer : IProjectConfigurationFileChangeListener
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly IRazorProjectService _projectService;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ILogger _logger;
    private readonly Dictionary<string, ProjectKey> _configurationToProjectMap;
    internal readonly Dictionary<ProjectKey, DelayedProjectInfo> ProjectInfoMap;

    public ProjectConfigurationStateSynchronizer(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        IRazorProjectService projectService,
        ILoggerFactory loggerFactory,
        LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _projectService = projectService;
        _languageServerFeatureOptions = languageServerFeatureOptions;
        _logger = loggerFactory.GetOrCreateLogger<ProjectConfigurationStateSynchronizer>();
        _configurationToProjectMap = new Dictionary<string, ProjectKey>(FilePathComparer.Instance);
        ProjectInfoMap = new Dictionary<ProjectKey, DelayedProjectInfo>();
    }

    internal int EnqueueDelay { get; set; } = 250;

    public void ProjectConfigurationFileChanged(ProjectConfigurationFileChangeEventArgs args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        _projectSnapshotManagerDispatcher.AssertRunningOnDispatcher();

        switch (args.Kind)
        {
            case RazorFileChangeKind.Changed:
                {
                    var configurationFilePath = FilePathNormalizer.Normalize(args.ConfigurationFilePath);
                    if (!args.TryDeserialize(_languageServerFeatureOptions, out var projectInfo))
                    {
                        if (!_configurationToProjectMap.TryGetValue(configurationFilePath, out var lastAssociatedProjectKey))
                        {
                            // Could not resolve an associated project file, noop.
                            _logger.LogWarning($"Failed to deserialize configuration file after change for an unknown project. Configuration file path: '{configurationFilePath}'");
                            return;
                        }
                        else
                        {
                            _logger.LogWarning($"Failed to deserialize configuration file after change for project '{lastAssociatedProjectKey.Id}': '{configurationFilePath}'");
                        }

                        // We found the last associated project file for the configuration file. Reset the project since we can't
                        // accurately determine its configurations.

                        EnqueueUpdateProject(lastAssociatedProjectKey, projectInfo: null, CancellationToken.None);
                        return;
                    }

                    if (!_configurationToProjectMap.TryGetValue(configurationFilePath, out var associatedProjectKey))
                    {
                        _logger.LogWarning($"Found no project key for configuration file. Assuming new project. Configuration file path: '{configurationFilePath}'");

                        AddProjectAsync(configurationFilePath, projectInfo, CancellationToken.None).Forget();
                        return;
                    }

                    _logger.LogInformation($"Project configuration file changed for project '{associatedProjectKey.Id}': '{configurationFilePath}'");

                    EnqueueUpdateProject(associatedProjectKey, projectInfo, CancellationToken.None);
                    break;
                }
            case RazorFileChangeKind.Added:
                {
                    var configurationFilePath = FilePathNormalizer.Normalize(args.ConfigurationFilePath);
                    if (!args.TryDeserialize(_languageServerFeatureOptions, out var projectInfo))
                    {
                        // Given that this is the first time we're seeing this configuration file if we can't deserialize it
                        // then we have to noop.
                        _logger.LogWarning($"Failed to deserialize configuration file on configuration added event. Configuration file path: '{configurationFilePath}'");
                        return;
                    }

                    AddProjectAsync(configurationFilePath, projectInfo, CancellationToken.None).Forget();
                    break;
                }
            case RazorFileChangeKind.Removed:
                {
                    var configurationFilePath = FilePathNormalizer.Normalize(args.ConfigurationFilePath);
                    if (!_configurationToProjectMap.TryGetValue(configurationFilePath, out var projectFilePath))
                    {
                        // Failed to deserialize the initial project configuration file on add so we can't remove the configuration file because it doesn't exist in the list.
                        _logger.LogWarning($"Failed to resolve associated project on configuration removed event. Configuration file path: '{configurationFilePath}'");
                        return;
                    }

                    _configurationToProjectMap.Remove(configurationFilePath);

                    _logger.LogInformation($"Project configuration file removed for project '{projectFilePath}': '{configurationFilePath}'");

                    EnqueueUpdateProject(projectFilePath, projectInfo: null, CancellationToken.None);
                    break;
                }
        }

        async Task AddProjectAsync(string configurationFilePath, RazorProjectInfo projectInfo, CancellationToken cancellationToken)
        {
            try
            {
                var projectFilePath = FilePathNormalizer.Normalize(projectInfo.FilePath);
                var intermediateOutputPath = Path.GetDirectoryName(configurationFilePath).AssumeNotNull();
                var rootNamespace = projectInfo.RootNamespace;

                var projectKey = await _projectService
                    .AddProjectAsync(
                        projectFilePath,
                        intermediateOutputPath,
                        projectInfo.Configuration,
                        rootNamespace,
                        projectInfo.DisplayName,
                        cancellationToken)
                    .ConfigureAwait(false);
                _configurationToProjectMap[configurationFilePath] = projectKey;

                _logger.LogInformation($"Project configuration file added for project '{projectFilePath}': '{configurationFilePath}'");
                EnqueueUpdateProject(projectKey, projectInfo, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding project: {projectInfo.FilePath}");
            }
        }

        Task UpdateProjectAsync(ProjectKey projectKey, RazorProjectInfo? projectInfo, CancellationToken cancellationToken)
        {
            if (projectInfo is null)
            {
                return ResetProjectAsync(projectKey, cancellationToken);
            }

            _logger.LogInformation($"Actually updating {projectKey} with a real projectInfo");

            var projectWorkspaceState = projectInfo.ProjectWorkspaceState ?? ProjectWorkspaceState.Default;
            var documents = projectInfo.Documents;
            return _projectService.UpdateProjectAsync(
                projectKey,
                projectInfo.Configuration,
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                projectWorkspaceState,
                documents,
                cancellationToken);
        }

        async Task UpdateAfterDelayAsync(ProjectKey projectKey, CancellationToken cancellationToken)
        {
            await Task.Delay(EnqueueDelay).ConfigureAwait(true);

            var delayedProjectInfo = ProjectInfoMap[projectKey];
            await UpdateProjectAsync(projectKey, delayedProjectInfo.ProjectInfo, cancellationToken).ConfigureAwait(false);
        }

        void EnqueueUpdateProject(ProjectKey projectKey, RazorProjectInfo? projectInfo, CancellationToken cancellationToken)
        {
            if (!ProjectInfoMap.ContainsKey(projectKey))
            {
                ProjectInfoMap[projectKey] = new DelayedProjectInfo();
            }

            var delayedProjectInfo = ProjectInfoMap[projectKey];
            delayedProjectInfo.ProjectInfo = projectInfo;

            if (delayedProjectInfo.ProjectUpdateTask is null || delayedProjectInfo.ProjectUpdateTask.IsCompleted)
            {
                delayedProjectInfo.ProjectUpdateTask = UpdateAfterDelayAsync(projectKey, cancellationToken);
            }
        }

        Task ResetProjectAsync(ProjectKey projectKey, CancellationToken cancellationToken)
        {
            return _projectService.UpdateProjectAsync(
                projectKey,
                configuration: null,
                rootNamespace: null,
                displayName: "",
                ProjectWorkspaceState.Default,
                documents: [],
                cancellationToken);
        }
    }

    internal class DelayedProjectInfo
    {
        public Task? ProjectUpdateTask { get; set; }

        public RazorProjectInfo? ProjectInfo { get; set; }
    }
}
