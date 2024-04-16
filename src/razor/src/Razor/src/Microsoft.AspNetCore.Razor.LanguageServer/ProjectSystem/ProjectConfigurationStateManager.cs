// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

/// <summary>
/// Updates project system service with new project info.
/// </summary>
/// <remarks>
/// Used to figure out if the project system data is being added, updated or deleted
/// and acts accordingly to update project system service.
/// </remarks>
internal partial class ProjectConfigurationStateManager : IDisposable
{
    private readonly IRazorProjectService _projectService;
    private readonly IProjectSnapshotManager _projectManager;
    private readonly ILogger _logger;

    private readonly AsyncBatchingWorkQueue<(ProjectKey ProjectKey, RazorProjectInfo? ProjectInfo)> _workQueue;
    private readonly CancellationTokenSource _disposalTokenSource;
    private static readonly TimeSpan s_enqueueDelay = TimeSpan.FromMilliseconds(250);

    public ProjectConfigurationStateManager(
        IRazorProjectService projectService,
        ILoggerFactory loggerFactory,
        IProjectSnapshotManager projectManager)
        : this(projectService,
               loggerFactory,
               projectManager,
               s_enqueueDelay)
    {
    }

    // Provided for tests to specify enqueue delay
    public ProjectConfigurationStateManager(
        IRazorProjectService projectService,
        ILoggerFactory loggerFactory,
        IProjectSnapshotManager projectManager,
        TimeSpan enqueueDelay)
    {
        _projectService = projectService;
        _projectManager = projectManager;
        _logger = loggerFactory.GetOrCreateLogger<ProjectConfigurationStateManager>();

        _disposalTokenSource = new();
        _workQueue = new(
            enqueueDelay,
            ProcessBatchAsync,
            _disposalTokenSource.Token);
    }

    public void Dispose()
    {
        _disposalTokenSource.Cancel();
        _disposalTokenSource.Dispose();
    }

    private ValueTask ProcessBatchAsync(ImmutableArray<(ProjectKey ProjectKey, RazorProjectInfo? ProjectInfo)> workItems, CancellationToken cancellationToken)
    {
        foreach (var workItem in workItems.GetMostRecentUniqueItems(Comparer.Instance))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return default;
            }

            UpdateProjectAsync(workItem.ProjectKey, workItem.ProjectInfo, cancellationToken).Forget();
        }

        return default;
    }

    public async Task ProjectInfoUpdatedAsync(ProjectKey projectKey, RazorProjectInfo? projectInfo, CancellationToken cancellationToken)
    {
        if (!_projectManager.TryGetLoadedProject(projectKey, out _))
        {
            if (projectInfo is not null)
            {
                // For the new code path using endpoint, SerializedFilePath is IntermediateOutputPath.
                // We will rename it when we remove the old code path that actually uses serialized bin file.
                var intermediateOutputPath = FilePathNormalizer.Normalize(projectInfo.SerializedFilePath);
                _logger.LogInformation($"Found no existing project key for project key '{projectKey.Id}'. Assuming new project.");

                projectKey = await AddProjectAsync(intermediateOutputPath, projectInfo, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                _logger.LogWarning($"Found no existing project key '{projectKey.Id}' but projectInfo is null. Assuming no-op deletion.");
                return;
            }
        }
        else
        {
            _logger.LogInformation($"Project info changed for project '{projectKey.Id}'");
        }

        // projectInfo may be null, in which case we are enqueuing remove is "remove"
        EnqueueUpdateProject(projectKey, projectInfo);
    }

    private Task<ProjectKey> AddProjectAsync(
        string intermediateOutputPath,
        RazorProjectInfo projectInfo,
        CancellationToken cancellationToken)
    {
        var projectFilePath = FilePathNormalizer.Normalize(projectInfo.FilePath);
        var rootNamespace = projectInfo.RootNamespace;

        _logger.LogInformation($"Project configuration added for project '{projectFilePath}': '{intermediateOutputPath}'");

        return _projectService.AddProjectAsync(
            projectFilePath,
            intermediateOutputPath,
            projectInfo.Configuration,
            rootNamespace,
            projectInfo.DisplayName,
            cancellationToken);
    }

    private Task UpdateProjectAsync(ProjectKey projectKey, RazorProjectInfo? projectInfo, CancellationToken cancellationToken)
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

    private void EnqueueUpdateProject(ProjectKey projectKey, RazorProjectInfo? projectInfo)
    {
        _workQueue.AddWork((projectKey, projectInfo));
    }

    private Task ResetProjectAsync(ProjectKey projectKey, CancellationToken cancellationToken)
    {
        return _projectService.UpdateProjectAsync(
            projectKey,
            configuration: null,
            rootNamespace: null,
            displayName: "",
            ProjectWorkspaceState.Default,
            documents: ImmutableArray<DocumentSnapshotHandle>.Empty,
            cancellationToken);
    }
}
