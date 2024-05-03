// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class ProjectConfigurationStateSynchronizer : IProjectConfigurationFileChangeListener, IDisposable
{
    private abstract record Work(ProjectKey ProjectKey);
    private sealed record ResetProject(ProjectKey ProjectKey) : Work(ProjectKey);
    private sealed record UpdateProject(ProjectKey ProjectKey, RazorProjectInfo ProjectInfo) : Work(ProjectKey);

    private static readonly TimeSpan s_delay = TimeSpan.FromMilliseconds(250);

    private readonly IRazorProjectService _projectService;
    private readonly LanguageServerFeatureOptions _options;
    private readonly ILogger _logger;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue<Work> _workQueue;

    private readonly Dictionary<ProjectKey, ResetProject> _resetProjectMap = new();

    public ProjectConfigurationStateSynchronizer(
        IRazorProjectService projectService,
        ILoggerFactory loggerFactory,
        LanguageServerFeatureOptions options)
        : this(projectService, loggerFactory, options, s_delay)
    {
    }

    protected ProjectConfigurationStateSynchronizer(
        IRazorProjectService projectService,
        ILoggerFactory loggerFactory,
        LanguageServerFeatureOptions options,
        TimeSpan delay)
    {
        _projectService = projectService;
        _options = options;
        _logger = loggerFactory.GetOrCreateLogger<ProjectConfigurationStateSynchronizer>();

        _disposeTokenSource = new();
        _workQueue = new(delay, ProcessBatchAsync, _disposeTokenSource.Token);
    }

    public void Dispose()
    {
        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    private async ValueTask ProcessBatchAsync(ImmutableArray<Work> items, CancellationToken token)
    {
        foreach (var item in items.GetMostRecentUniqueItems(Comparer.Instance))
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            var itemTask = item switch
            {
                ResetProject(var projectKey) => ResetProjectAsync(projectKey, token),
                UpdateProject(var projectKey, var projectInfo) => UpdateProjectAsync(projectKey, projectInfo, token),
                _ => Assumed.Unreachable<Task>()
            };

            await itemTask.ConfigureAwait(false);
        }

        Task ResetProjectAsync(ProjectKey projectKey, CancellationToken token)
        {
            _logger.LogInformation($"Resetting {projectKey.Id}.");

            return _projectService
                .UpdateProjectAsync(
                    projectKey,
                    configuration: null,
                    rootNamespace: null,
                    displayName: "",
                    ProjectWorkspaceState.Default,
                    documents: [],
                    token);
        }

        Task UpdateProjectAsync(ProjectKey projectKey, RazorProjectInfo projectInfo, CancellationToken token)
        {
            _logger.LogInformation($"Updating {projectKey.Id}.");

            return _projectService
                .AddOrUpdateProjectAsync(
                    projectKey,
                    projectInfo.FilePath,
                    projectInfo.Configuration,
                    projectInfo.RootNamespace,
                    projectInfo.DisplayName,
                    projectInfo.ProjectWorkspaceState,
                    projectInfo.Documents,
                    token);
        }
    }

    public void ProjectConfigurationFileChanged(ProjectConfigurationFileChangeEventArgs args)
    {
        switch (args.Kind)
        {
            case RazorFileChangeKind.Changed:
                {
                    if (args.TryDeserialize(_options, out var projectInfo))
                    {
                        var projectKey = ProjectKey.From(projectInfo);
                        _logger.LogInformation($"Configuration file changed for project '{projectKey.Id}'.");

                        _workQueue.AddWork(new UpdateProject(projectKey, projectInfo));
                    }
                    else
                    {
                        var projectKey = args.GetProjectKey();
                        _logger.LogWarning($"Failed to deserialize after change to configuration file for project '{projectKey.Id}'.");

                        // We found the last associated project file for the configuration file. Reset the project since we can't
                        // accurately determine its configurations.
                        _workQueue.AddWork(new ResetProject(projectKey));
                    }
                }

                break;

            case RazorFileChangeKind.Added:
                {
                    if (args.TryDeserialize(_options, out var projectInfo))
                    {
                        var projectKey = ProjectKey.From(projectInfo);
                        _logger.LogInformation($"Configuration file added for project '{projectKey.Id}'.");

                        // Update will add the project if it doesn't exist
                        _workQueue.AddWork(new UpdateProject(projectKey, projectInfo));
                    }
                    else
                    {
                        // This is the first time we've seen this configuration file, but we can't deserialize it.
                        // The only thing we can really do is issue a warning.
                        _logger.LogWarning($"Failed to deserialize previously unseen configuration file '{args.ConfigurationFilePath}'");
                    }
                }

                break;

            case RazorFileChangeKind.Removed:
                {
                    var projectKey = args.GetProjectKey();
                    _logger.LogInformation($"Configuration file removed for project '{projectKey}'.");

                    _workQueue.AddWork(new ResetProject(projectKey));
                }

                break;
        }
    }
}
