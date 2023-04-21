// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.RoslynWorkspace;

public class RazorWorkspaceListener : IDisposable
{
    private static readonly TimeSpan s_debounceTime = TimeSpan.FromMilliseconds(100);

    private string? _projectRazorJsonFileName;
    private Workspace? _workspace;
    private readonly Dictionary<ProjectId, TaskDelayScheduler> _workQueues;
    private readonly object _gate = new();

    public RazorWorkspaceListener()
    {
        _workQueues = new Dictionary<ProjectId, TaskDelayScheduler>();
    }

    public void EnsureInitialized(Workspace workspace, string projectRazorJsonFileName)
    {
        // Make sure we don't hook up the event handler multiple times
        if (_projectRazorJsonFileName is not null)
        {
            return;
        }

        _projectRazorJsonFileName = projectRazorJsonFileName;
        _workspace = workspace;
        _workspace.WorkspaceChanged += Workspace_WorkspaceChanged;
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

        TaskDelayScheduler? scheduler;
        lock (_gate)
        {
            if (_workQueues.TryGetValue(project.Id, out scheduler))
            {
                _workQueues.Remove(project.Id);
            }
        }

        scheduler?.Dispose();
    }

    private void EnqueueUpdate(Project? project)
    {
        if (_projectRazorJsonFileName is null ||
            project is not
            {
                Language: LanguageNames.CSharp
            })
        {
            return;
        }

        TaskDelayScheduler? scheduler;
        lock (_gate)
        {
            if (!_workQueues.TryGetValue(project.Id, out scheduler))
            {
                scheduler = new TaskDelayScheduler(s_debounceTime, CancellationToken.None);
                _workQueues.Add(project.Id, scheduler);
            }
        }

        var projectId = project.Id;
        scheduler.ScheduleAsyncTask(ct => SerializeProjectAsync(projectId, ct), CancellationToken.None);
    }

    // Protected for testing
    protected virtual Task SerializeProjectAsync(ProjectId projectId, CancellationToken ct)
    {
        if (_projectRazorJsonFileName is null || _workspace is null)
        {
            return Task.CompletedTask;
        }

        var project = _workspace.CurrentSolution.GetProject(projectId);
        if (project is null)
        {
            return Task.CompletedTask;
        }

        return RazorProjectJsonSerializer.SerializeAsync(project, _projectRazorJsonFileName, ct);
    }

    public void Dispose()
    {
        lock (_gate)
        {
            foreach (var (_, value) in _workQueues)
            {
                value.Dispose();
            }
        }
    }
}
