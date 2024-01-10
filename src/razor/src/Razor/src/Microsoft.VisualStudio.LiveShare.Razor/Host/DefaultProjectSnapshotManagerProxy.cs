// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LiveShare.Razor.Host;

internal class DefaultProjectSnapshotManagerProxy : IProjectSnapshotManagerProxy, ICollaborationService, IDisposable
{
    private readonly CollaborationSession _session;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly ProjectSnapshotManager _projectSnapshotManager;
    private readonly JoinableTaskFactory _joinableTaskFactory;
    private readonly AsyncSemaphore _latestStateSemaphore;
    private bool _disposed;
    private ProjectSnapshotManagerProxyState? _latestState;

    // Internal for testing
    internal JoinableTask? _processingChangedEventTestTask;

    public DefaultProjectSnapshotManagerProxy(
        CollaborationSession session,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ProjectSnapshotManager projectSnapshotManager,
        JoinableTaskFactory joinableTaskFactory)
    {
        if (session is null)
        {
            throw new ArgumentNullException(nameof(session));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (projectSnapshotManager is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManager));
        }

        if (joinableTaskFactory is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskFactory));
        }

        _session = session;
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _projectSnapshotManager = projectSnapshotManager;
        _joinableTaskFactory = joinableTaskFactory;

        _latestStateSemaphore = new AsyncSemaphore(initialCount: 1);
        _projectSnapshotManager.Changed += ProjectSnapshotManager_Changed;
    }

    public event EventHandler<ProjectChangeEventProxyArgs>? Changed;

    public async Task<ProjectSnapshotManagerProxyState> GetProjectManagerStateAsync(CancellationToken cancellationToken)
    {
        using (await _latestStateSemaphore.EnterAsync(cancellationToken).ConfigureAwait(false))
        {
            if (_latestState is not null)
            {
                return _latestState;
            }
        }

        var projects = await GetLatestProjectsAsync();
        var state = await CalculateUpdatedStateAsync(projects);

        return state;
    }

    public void Dispose()
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        _projectSnapshotManager.Changed -= ProjectSnapshotManager_Changed;
        _latestStateSemaphore.Dispose();
        _disposed = true;
    }

    // Internal for testing
    internal async Task<IReadOnlyList<IProjectSnapshot>> GetLatestProjectsAsync()
    {
        if (!_joinableTaskFactory.Context.IsOnMainThread)
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync(CancellationToken.None);
        }

        return _projectSnapshotManager.GetProjects();
    }

    // Internal for testing
    internal async Task<ProjectSnapshotManagerProxyState> CalculateUpdatedStateAsync(IReadOnlyList<IProjectSnapshot> projects)
    {
        using (await _latestStateSemaphore.EnterAsync().ConfigureAwait(false))
        {
            var projectHandles = new List<ProjectSnapshotHandleProxy>();
            foreach (var project in projects)
            {
                var projectHandleProxy = ConvertToProxy(project);
                projectHandles.Add(projectHandleProxy);
            }

            _latestState = new ProjectSnapshotManagerProxyState(projectHandles);
            return _latestState;
        }
    }

    [return: NotNullIfNotNull(nameof(project))]
    private ProjectSnapshotHandleProxy? ConvertToProxy(IProjectSnapshot? project)
    {
        if (project is null)
        {
            return null;
        }

        var projectWorkspaceState = ProjectWorkspaceState.Create(project.TagHelpers, project.CSharpLanguageVersion);
        var projectFilePath = _session.ConvertLocalPathToSharedUri(project.FilePath);
        var intermediateOutputPath = _session.ConvertLocalPathToSharedUri(project.IntermediateOutputPath);
        var projectHandleProxy = new ProjectSnapshotHandleProxy(projectFilePath, intermediateOutputPath, project.Configuration, project.RootNamespace, projectWorkspaceState);
        return projectHandleProxy;
    }

    private void ProjectSnapshotManager_Changed(object sender, ProjectChangeEventArgs args)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_disposed)
        {
            return;
        }

        if (args.Kind == ProjectChangeKind.DocumentAdded ||
            args.Kind == ProjectChangeKind.DocumentRemoved ||
            args.Kind == ProjectChangeKind.DocumentChanged)
        {
            // Razor LiveShare doesn't currently support document based notifications over the wire.
            return;
        }

        _processingChangedEventTestTask = _joinableTaskFactory.RunAsync(async () =>
        {
            var projects = await GetLatestProjectsAsync();

            await _joinableTaskFactory.SwitchToMainThreadAsync();

            var oldProjectProxy = ConvertToProxy(args.Older);
            var newProjectProxy = ConvertToProxy(args.Newer);
            var remoteProjectChangeArgs = new ProjectChangeEventProxyArgs(oldProjectProxy, newProjectProxy, (ProjectProxyChangeKind)args.Kind);

            OnChanged(remoteProjectChangeArgs);
        });
    }

    private void OnChanged(ProjectChangeEventProxyArgs args)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_disposed)
        {
            return;
        }

        Changed?.Invoke(this, args);
    }
}
