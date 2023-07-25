// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Threading;
using IAsyncDisposable = Microsoft.VisualStudio.Threading.IAsyncDisposable;

namespace Microsoft.VisualStudio.LiveShare.Razor.Guest;

internal class ProjectSnapshotSynchronizationService : ICollaborationService, IAsyncDisposable, System.IAsyncDisposable
{
    private readonly JoinableTaskFactory _joinableTaskFactory;
    private readonly CollaborationSession _sessionContext;
    private readonly IProjectSnapshotManagerProxy _hostProjectManagerProxy;
    private readonly ProjectSnapshotManagerBase _projectSnapshotManager;

    public ProjectSnapshotSynchronizationService(
        JoinableTaskFactory joinableTaskFactory,
        CollaborationSession sessionContext,
        IProjectSnapshotManagerProxy hostProjectManagerProxy,
        ProjectSnapshotManagerBase projectSnapshotManager)
    {
        if (joinableTaskFactory is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskFactory));
        }

        if (sessionContext is null)
        {
            throw new ArgumentNullException(nameof(sessionContext));
        }

        if (hostProjectManagerProxy is null)
        {
            throw new ArgumentNullException(nameof(hostProjectManagerProxy));
        }

        if (projectSnapshotManager is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManager));
        }

        _joinableTaskFactory = joinableTaskFactory;
        _sessionContext = sessionContext;
        _hostProjectManagerProxy = hostProjectManagerProxy;
        _projectSnapshotManager = projectSnapshotManager;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        // We wire the changed event up early because any changed events that fire will ensure we have the most
        // up-to-date state.
        _hostProjectManagerProxy.Changed += HostProxyProjectManager_Changed;

        var projectManagerState = await _hostProjectManagerProxy.GetProjectManagerStateAsync(cancellationToken);

        await InitializeGuestProjectManagerAsync(projectManagerState.ProjectHandles, cancellationToken);
    }

    public async Task DisposeAsync()
    {
        _hostProjectManagerProxy.Changed -= HostProxyProjectManager_Changed;

        await _joinableTaskFactory.SwitchToMainThreadAsync();

        var projects = _projectSnapshotManager.GetProjects();
        foreach (var project in projects)
        {
            try
            {
                _projectSnapshotManager.ProjectRemoved(project.Key);
            }
            catch (Exception ex)
            {
                _projectSnapshotManager.ReportError(ex, project);
            }
        }
    }

    // Internal for testing
    internal void UpdateGuestProjectManager(ProjectChangeEventProxyArgs args)
    {
        if (args.Kind == ProjectProxyChangeKind.ProjectAdded)
        {
            var guestPath = ResolveGuestPath(args.ProjectFilePath);
            var guestIntermediateOutputPath = ResolveGuestPath(args.IntermediateOutputPath);
            var hostProject = new HostProject(guestPath, guestIntermediateOutputPath, args.Newer!.Configuration, args.Newer.RootNamespace);
            _projectSnapshotManager.ProjectAdded(hostProject);

            if (args.Newer.ProjectWorkspaceState != null)
            {
                _projectSnapshotManager.ProjectWorkspaceStateChanged(hostProject.Key, args.Newer.ProjectWorkspaceState);
            }
        }
        else if (args.Kind == ProjectProxyChangeKind.ProjectRemoved)
        {
            var guestPath = ResolveGuestPath(args.ProjectFilePath);
            var projectKeys = _projectSnapshotManager.GetAllProjectKeys(guestPath);
            foreach (var projectKey in projectKeys)
            {
                _projectSnapshotManager.ProjectRemoved(projectKey);
            }
        }
        else if (args.Kind == ProjectProxyChangeKind.ProjectChanged)
        {
            if (!args.Older!.Configuration.Equals(args.Newer!.Configuration))
            {
                var guestPath = ResolveGuestPath(args.Newer.FilePath);
                var guestIntermediateOutputPath = ResolveGuestPath(args.Newer.IntermediateOutputPath);
                var hostProject = new HostProject(guestPath, guestIntermediateOutputPath, args.Newer.Configuration, args.Newer.RootNamespace);
                _projectSnapshotManager.ProjectConfigurationChanged(hostProject);
            }
            else if (args.Older.ProjectWorkspaceState != args.Newer.ProjectWorkspaceState ||
                args.Older.ProjectWorkspaceState?.Equals(args.Newer.ProjectWorkspaceState) == false)
            {
                var guestPath = ResolveGuestPath(args.Newer.FilePath);
                var projectKeys = _projectSnapshotManager.GetAllProjectKeys(guestPath);
                foreach (var projectKey in projectKeys)
                {
                    _projectSnapshotManager.ProjectWorkspaceStateChanged(projectKey, args.Newer.ProjectWorkspaceState);
                }
            }
        }
    }

    private async Task InitializeGuestProjectManagerAsync(
        IReadOnlyList<ProjectSnapshotHandleProxy> projectHandles,
        CancellationToken cancellationToken)
    {
        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        foreach (var projectHandle in projectHandles)
        {
            var guestPath = ResolveGuestPath(projectHandle.FilePath);
            var guestIntermediateOutputPath = ResolveGuestPath(projectHandle.IntermediateOutputPath);
            var hostProject = new HostProject(guestPath, guestIntermediateOutputPath, projectHandle.Configuration, projectHandle.RootNamespace);
            _projectSnapshotManager.ProjectAdded(hostProject);

            if (projectHandle.ProjectWorkspaceState is not null)
            {
                _projectSnapshotManager.ProjectWorkspaceStateChanged(hostProject.Key, projectHandle.ProjectWorkspaceState);
            }
        }
    }

    private void HostProxyProjectManager_Changed(object sender, ProjectChangeEventProxyArgs args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        _joinableTaskFactory.Run(async () =>
        {
            try
            {
                await _joinableTaskFactory.SwitchToMainThreadAsync();

                UpdateGuestProjectManager(args);
            }
            catch (Exception ex)
            {
                _projectSnapshotManager.ReportError(ex);
            }
        });
    }

    private string ResolveGuestPath(Uri filePath)
    {
        return _sessionContext.ConvertSharedUriToLocalPath(filePath);
    }

    ValueTask System.IAsyncDisposable.DisposeAsync()
    {
        return new ValueTask(DisposeAsync());
    }
}
