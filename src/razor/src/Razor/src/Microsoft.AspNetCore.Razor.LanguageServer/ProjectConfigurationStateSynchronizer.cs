// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class ProjectConfigurationStateSynchronizer : IProjectConfigurationFileChangeListener, IDisposable
{
    private abstract record Work(string ConfigurationFilePath);
    private sealed record AddProject(string ConfigurationFilePath, RazorProjectInfo ProjectInfo) : Work(ConfigurationFilePath);
    private sealed record ResetProject(string ConfigurationFilePath, ProjectKey ProjectKey) : Work(ConfigurationFilePath);
    private sealed record UpdateProject(string ConfigurationFilePath, ProjectKey ProjectKey, RazorProjectInfo ProjectInfo) : Work(ConfigurationFilePath);

    private static readonly TimeSpan s_delay = TimeSpan.FromMilliseconds(250);

    private readonly IRazorProjectService _projectService;
    private readonly LanguageServerFeatureOptions _options;
    private readonly ILogger _logger;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue<Work> _workQueue;

    private ImmutableDictionary<string, ProjectKey> _filePathToProjectKeyMap =
        ImmutableDictionary<string, ProjectKey>.Empty.WithComparers(keyComparer: FilePathComparer.Instance);

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
            var itemTask = item switch
            {
                AddProject(var configurationFilePath, var projectInfo) => AddProjectAsync(configurationFilePath, projectInfo, token),
                ResetProject(_, var projectKey) => ResetProjectAsync(projectKey, token),
                UpdateProject(_, var projectKey, var projectInfo) => UpdateProjectAsync(projectKey, projectInfo, token),
                _ => Assumed.Unreachable<Task>()
            };

            await itemTask.ConfigureAwait(false);
        }

        async Task AddProjectAsync(string configurationFilePath, RazorProjectInfo projectInfo, CancellationToken token)
        {
            var projectFilePath = FilePathNormalizer.Normalize(projectInfo.FilePath);
            var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(configurationFilePath);

            var projectKey = await _projectService
                .AddProjectAsync(
                    projectFilePath,
                    intermediateOutputPath,
                    projectInfo.Configuration,
                    projectInfo.RootNamespace,
                    projectInfo.DisplayName,
                    token)
                .ConfigureAwait(false);

            _logger.LogInformation($"Added {projectKey.Id}.");

            ImmutableInterlocked.AddOrUpdate(ref _filePathToProjectKeyMap, configurationFilePath, projectKey, static (k, v) => v);
            _logger.LogInformation($"Project configuration file added for project '{projectFilePath}': '{configurationFilePath}'");

            await UpdateProjectAsync(projectKey, projectInfo, token).ConfigureAwait(false);
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
                .UpdateProjectAsync(
                    projectKey,
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
        var configurationFilePath = FilePathNormalizer.Normalize(args.ConfigurationFilePath);

        switch (args.Kind)
        {
            case RazorFileChangeKind.Changed:
                {
                    if (args.TryDeserialize(_options, out var projectInfo))
                    {
                        if (_filePathToProjectKeyMap.TryGetValue(configurationFilePath, out var projectKey))
                        {
                            _logger.LogInformation($"""
                                Configuration file changed for project '{projectKey.Id}'.
                                Configuration file path: '{configurationFilePath}'
                                """);

                            _workQueue.AddWork(new UpdateProject(configurationFilePath, projectKey, projectInfo));
                        }
                        else
                        {
                            _logger.LogWarning($"""
                                Adding project for previously unseen configuration file.
                                Configuration file path: '{configurationFilePath}'
                                """);

                            _workQueue.AddWork(new AddProject(configurationFilePath, projectInfo));
                        }
                    }
                    else
                    {
                        if (_filePathToProjectKeyMap.TryGetValue(configurationFilePath, out var projectKey))
                        {
                            _logger.LogWarning($"""
                                Failed to deserialize after change to configuration file for project '{projectKey.Id}'.
                                Configuration file path: '{configurationFilePath}'
                                """);

                            // We found the last associated project file for the configuration file. Reset the project since we can't
                            // accurately determine its configurations.

                            _workQueue.AddWork(new ResetProject(configurationFilePath, projectKey));
                        }
                        else
                        {
                            // Could not resolve an associated project file.
                            _logger.LogWarning($"""
                                Failed to deserialize after change to previously unseen configuration file.
                                Configuration file path: '{configurationFilePath}'
                                """);
                        }
                    }
                }

                break;

            case RazorFileChangeKind.Added:
                {
                    if (args.TryDeserialize(_options, out var projectInfo))
                    {
                        _workQueue.AddWork(new AddProject(configurationFilePath, projectInfo));
                    }
                    else
                    {
                        // This is the first time we've seen this configuration file, but we can't deserialize it.
                        // The only thing we can really do is issue a warning.
                        _logger.LogWarning($"""
                            Failed to deserialize previously unseen configuration file.
                            Configuration file path: '{configurationFilePath}'
                            """);
                    }
                }

                break;

            case RazorFileChangeKind.Removed:
                {
                    if (ImmutableInterlocked.TryRemove(ref _filePathToProjectKeyMap, configurationFilePath, out var projectKey))
                    {
                        _logger.LogInformation($"""
                            Configuration file removed for project '{projectKey}'.
                            Configuration file path: '{configurationFilePath}'
                            """);

                        _workQueue.AddWork(new ResetProject(configurationFilePath, projectKey));
                    }
                    else
                    {
                        _logger.LogWarning($"""
                            Failed to resolve associated project on configuration removed event.
                            Configuration file path: '{configurationFilePath}'
                            """);
                    }
                }

                break;
        }
    }
}
