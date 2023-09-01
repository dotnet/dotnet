// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.RoslynWorkspace;

public class RazorWorkspaceListener : IDisposable
{
    private static readonly TimeSpan s_debounceTime = TimeSpan.FromMilliseconds(500);

    private readonly ILogger _logger;

    private string? _projectInfoFileName;
    private Workspace? _workspace;
    private ImmutableDictionary<ProjectId, TaskDelayScheduler> _workQueues = ImmutableDictionary<ProjectId, TaskDelayScheduler>.Empty;

    public RazorWorkspaceListener(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(nameof(RazorWorkspaceListener));
    }

    public void EnsureInitialized(Workspace workspace, string projectInfoFileName)
    {
        // Make sure we don't hook up the event handler multiple times
        if (_projectInfoFileName is not null)
        {
            return;
        }

        _projectInfoFileName = projectInfoFileName;
        _workspace = workspace;
        _workspace.WorkspaceChanged += Workspace_WorkspaceChanged;
    }

    public void NotifyDynamicFile(ProjectId projectId)
    {
        // Since there is no "un-notify" API to indicate that callers no longer care about a project, it's entirely
        // possible that by the time we get notified, a project might have been removed from the workspace. Whilst
        // that wouldn't cause any issues we may as well avoid creating a task scheduler.
        if (_workspace is null || !_workspace.CurrentSolution.ContainsProject(projectId))
        {
            return;
        }

        // We expect this to be called multiple times per project so a no-op update operation seems like a better choice
        // than constructing a new TaskDelayScheduler each time, and using the TryAdd method, which doesn't support a
        // valueFactory argument.
        var scheduler = ImmutableInterlocked.AddOrUpdate(ref _workQueues, projectId, static _ => new TaskDelayScheduler(s_debounceTime, CancellationToken.None), static (_, val) => val);

        // Schedule a task, in case adding a dynamic file is the last thing that happens
        _logger.LogTrace("{projectId} scheduling task due to dynamic file", projectId);
        scheduler.ScheduleAsyncTask(ct => SerializeProjectAsync(projectId, ct), CancellationToken.None);
    }

    private void Workspace_WorkspaceChanged(object? sender, WorkspaceChangeEventArgs e)
    {
        switch (e.Kind)
        {
            case WorkspaceChangeKind.SolutionChanged:
            case WorkspaceChangeKind.SolutionReloaded:
                foreach (var project in e.OldSolution.Projects)
                {
                    RemoveProject(project);
                }

                foreach (var project in e.NewSolution.Projects)
                {
                    EnqueueUpdate(project);
                }

                break;

            case WorkspaceChangeKind.SolutionAdded:
                foreach (var project in e.NewSolution.Projects)
                {
                    EnqueueUpdate(project);
                }

                break;

            case WorkspaceChangeKind.ProjectRemoved:
                RemoveProject(e.OldSolution.GetProject(e.ProjectId));
                break;

            case WorkspaceChangeKind.ProjectReloaded:
                RemoveProject(e.OldSolution.GetProject(e.ProjectId));
                EnqueueUpdate(e.NewSolution.GetProject(e.ProjectId));
                break;

            case WorkspaceChangeKind.ProjectAdded:
            case WorkspaceChangeKind.ProjectChanged:
            case WorkspaceChangeKind.DocumentAdded:
            case WorkspaceChangeKind.DocumentRemoved:
            case WorkspaceChangeKind.DocumentReloaded:
            case WorkspaceChangeKind.DocumentChanged:
            case WorkspaceChangeKind.AdditionalDocumentAdded:
            case WorkspaceChangeKind.AdditionalDocumentRemoved:
            case WorkspaceChangeKind.AdditionalDocumentReloaded:
            case WorkspaceChangeKind.AdditionalDocumentChanged:
            case WorkspaceChangeKind.DocumentInfoChanged:
            case WorkspaceChangeKind.AnalyzerConfigDocumentAdded:
            case WorkspaceChangeKind.AnalyzerConfigDocumentRemoved:
            case WorkspaceChangeKind.AnalyzerConfigDocumentReloaded:
            case WorkspaceChangeKind.AnalyzerConfigDocumentChanged:
                var projectId = e.ProjectId ?? e.DocumentId?.ProjectId;
                if (projectId is not null)
                {
                    EnqueueUpdate(e.NewSolution.GetProject(projectId));
                }

                break;
            case WorkspaceChangeKind.SolutionCleared:
            case WorkspaceChangeKind.SolutionRemoved:
                foreach (var project in e.OldSolution.Projects)
                {
                    RemoveProject(project);
                }

                break;
            default:
                break;
        }
    }

    private void RemoveProject(Project? project)
    {
        if (project is null)
        {
            return;
        }

        if (ImmutableInterlocked.TryRemove(ref _workQueues, project.Id, out var scheduler))
        {
            scheduler.Dispose();
        }
    }

    private void EnqueueUpdate(Project? project)
    {
        if (_projectInfoFileName is null ||
            project is not
            {
                Language: LanguageNames.CSharp
            })
        {
            return;
        }

        var projectId = project.Id;
        if (_workQueues.TryGetValue(projectId, out var scheduler))
        {
            _logger.LogTrace("{projectId} scheduling task due to workspace event", projectId);

            scheduler.ScheduleAsyncTask(ct => SerializeProjectAsync(projectId, ct), CancellationToken.None);
        }
    }

    // Protected for testing
    protected virtual Task SerializeProjectAsync(ProjectId projectId, CancellationToken ct)
    {
        if (_projectInfoFileName is null || _workspace is null)
        {
            return Task.CompletedTask;
        }

        var project = _workspace.CurrentSolution.GetProject(projectId);
        if (project is null)
        {
            return Task.CompletedTask;
        }

        _logger.LogTrace("{projectId} writing json file", projectId);
        return RazorProjectInfoSerializer.SerializeAsync(project, _projectInfoFileName, ct);
    }

    public void Dispose()
    {
        if (_workspace is not null)
        {
            _workspace.WorkspaceChanged -= Workspace_WorkspaceChanged;
        }

        var queues = Interlocked.Exchange(ref _workQueues, ImmutableDictionary<ProjectId, TaskDelayScheduler>.Empty);
        foreach (var (_, value) in queues)
        {
            value.Dispose();
        }
    }
}
