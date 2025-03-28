// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Microsoft.VisualStudioCode.RazorExtension;

internal abstract class RazorWorkspaceListenerBase : IDisposable
{
    private static readonly TimeSpan s_debounceTime = TimeSpan.FromMilliseconds(500);
    private readonly CancellationTokenSource _disposeTokenSource = new();

    private readonly ILogger _logger;
    private readonly AsyncBatchingWorkQueue<Work> _workQueue;

    // Use an immutable dictionary for ImmutableInterlocked operations. The value isn't checked, just
    // the existance of the key so work is only done for projects with dynamic files.
    private ImmutableDictionary<ProjectId, bool> _projectsWithDynamicFile = ImmutableDictionary<ProjectId, bool>.Empty;

    private Stream? _stream;
    private Workspace? _workspace;
    private bool _disposed;

    internal record Work(ProjectId ProjectId);
    internal record UpdateWork(ProjectId ProjectId) : Work(ProjectId);
    internal record RemovalWork(ProjectId ProjectId, string IntermediateOutputPath) : Work(ProjectId);

    protected RazorWorkspaceListenerBase(ILogger logger)
    {
        _logger = logger;
        _workQueue = new(s_debounceTime, ProcessWorkAsync, EqualityComparer<Work>.Default, _disposeTokenSource.Token);
    }

    private protected abstract Task CheckConnectionAsync(Stream stream, CancellationToken cancellationToken);

    public void Dispose()
    {
        if (_workspace is not null)
        {
            _workspace.WorkspaceChanged -= Workspace_WorkspaceChanged;
        }

        if (_disposed)
        {
            _logger.LogInformation("Disposal was called twice");
            return;
        }

        _disposed = true;
        _logger.LogInformation("Tearing down named pipe for pid {pid}", Process.GetCurrentProcess().Id);

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();

        _stream?.Dispose();
        _stream = null;
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

        // Other modifications of projects happen on Workspace.Changed events, which are not
        // assumed to be on the same thread as dynamic file notification.
        ImmutableInterlocked.GetOrAdd(ref _projectsWithDynamicFile, projectId, static (_) => true);

        // Schedule a task, in case adding a dynamic file is the last thing that happens
        _logger.LogTrace("{projectId} scheduling task due to dynamic file", projectId);
        _workQueue.AddWork(new UpdateWork(projectId));
    }

    /// <summary>
    /// Initializes the workspace and begins hooking up to workspace events. This is not thread safe
    /// and intended to be called only once.
    /// </summary>
    private protected void EnsureInitialized(Workspace workspace, Func<Stream> createStream)
    {
        // Early exit check. Initialization should only happen once. Handle as safely as possible but
        if (_workspace is not null)
        {
            _logger.LogInformation("EnsureInitialized was called multiple times when it shouldn't have been.");
            return;
        }

        // Early check for disposal just to reduce any work further
        if (_disposed)
        {
            return;
        }

        _workspace = workspace;
        _workspace.WorkspaceChanged += Workspace_WorkspaceChanged;
        _stream = createStream();
    }

    private void Workspace_WorkspaceChanged(object? sender, WorkspaceChangeEventArgs e)
    {
        switch (e.Kind)
        {
            case WorkspaceChangeKind.SolutionChanged:
            case WorkspaceChangeKind.SolutionReloaded:
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

            case WorkspaceChangeKind.ProjectReloaded:
                EnqueueUpdate(e.NewSolution.GetProject(e.ProjectId));
                break;

            case WorkspaceChangeKind.ProjectRemoved:
                RemoveProject(e.OldSolution.GetProject(e.ProjectId.AssumeNotNull()).AssumeNotNull());
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

        //
        // Local functions
        //
        void EnqueueUpdate(Project? project)
        {
            if (_disposed ||
                project is not
                {
                    Language: LanguageNames.CSharp
                })
            {
                return;
            }

            // Don't queue work for projects that don't have a dynamic file
            if (!_projectsWithDynamicFile.TryGetValue(project.Id, out var _))
            {
                return;
            }

            _workQueue.AddWork(new UpdateWork(project.Id));
        }

        void RemoveProject(Project project)
        {
            // Remove project is called from Workspace.Changed, while other notifications of _projectsWithDynamicFile
            // are handled with NotifyDynamicFile. Use ImmutableInterlocked here to be sure the updates happen
            // in a thread safe manner since those are not assumed to be the same thread.
            if (ImmutableInterlocked.TryRemove(ref _projectsWithDynamicFile, project.Id, out var _))
            {
                var intermediateOutputPath = Path.GetDirectoryName(project.CompilationOutputInfo.AssemblyPath);
                if (intermediateOutputPath is null)
                {
                    _logger.LogTrace("intermediatePath is null, skipping notification of removal for {projectId}", project.Id);
                    return;
                }

                _workQueue.AddWork(new RemovalWork(project.Id, intermediateOutputPath));
            }
        }
    }

    /// <summary>
    /// Work controlled by the <see cref="_workQueue"/>. Cancellation of that work queue propagates
    /// to cancellation of this thread and should be handled accordingly
    /// </summary>
    /// <remarks>
    /// private protected virtual for testing
    /// </remarks>
    private protected async virtual ValueTask ProcessWorkAsync(ImmutableArray<Work> work, CancellationToken cancellationToken)
    {
        // Capture as locals here. Cancellation of the work queue still need to propogate. The cancellation
        // token itself represents the work queue halting, but this will help avoid any assumptions about nullability of locals
        // through the use in this function.
        var stream = _stream;
        var solution = _workspace?.CurrentSolution;

        cancellationToken.ThrowIfCancellationRequested();

        // Early bail check for if we are disposed or somewhere in the middle of disposal
        if (_disposed || stream is null || solution is null)
        {
            _logger.LogTrace("Skipping work due to disposal");
            return;
        }

        await CheckConnectionAsync(stream, cancellationToken).ConfigureAwait(false);
        await ProcessWorkCoreAsync(work, stream, solution, _logger, cancellationToken).ConfigureAwait(false);
    }

    private static async Task ProcessWorkCoreAsync(ImmutableArray<Work> work, Stream stream, Solution solution, ILogger logger, CancellationToken cancellationToken)
    {
        foreach (var unit in work)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (unit is RemovalWork removalWork)
                {
                    await ReportRemovalAsync(stream, removalWork, logger, cancellationToken).ConfigureAwait(false);
                }

                var project = solution.GetProject(unit.ProjectId);
                if (project is null)
                {
                    logger.LogTrace("Project {projectId} is not in workspace", unit.ProjectId);
                    continue;
                }

                await ReportUpdateProjectAsync(stream, project, logger, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Encountered exception while processing unit: {message}", ex.Message);
            }
        }

        try
        {
            await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Encountered error flusingh stream");
        }
    }

    private static async Task ReportUpdateProjectAsync(Stream stream, Project project, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogTrace("Serializing information for {projectId}", project.Id);
        var result = await RazorProjectInfoFactory.ConvertAsync(project, cancellationToken).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            logger.LogTrace("Skipped writing data for {projectId} because of '{reason}'", project.Id, result.Reason);
            return;
        }

        stream.WriteProjectInfoAction(ProjectInfoAction.Update);
        await stream.WriteProjectInfoAsync(result.ProjectInfo, cancellationToken).ConfigureAwait(false);
    }

    private static Task ReportRemovalAsync(Stream stream, RemovalWork unit, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogTrace("Reporting removal of {projectId}", unit.ProjectId);
        return stream.WriteProjectInfoRemovalAsync(unit.IntermediateOutputPath, cancellationToken);
    }
}
