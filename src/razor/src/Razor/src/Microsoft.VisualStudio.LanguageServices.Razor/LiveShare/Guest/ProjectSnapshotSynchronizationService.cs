// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Threading;
using IAsyncDisposable = Microsoft.VisualStudio.Threading.IAsyncDisposable;

namespace Microsoft.VisualStudio.LiveShare.Razor.Guest;

internal class ProjectSnapshotSynchronizationService(
    CollaborationSession sessionContext,
    IProjectSnapshotManagerProxy hostProjectManagerProxy,
    ProjectSnapshotManagerBase projectManager,
    ProjectSnapshotManagerDispatcher dispatcher,
    IErrorReporter errorReporter,
    JoinableTaskFactory jtf) : ICollaborationService, IAsyncDisposable, System.IAsyncDisposable
{
    private readonly JoinableTaskFactory _jtf = jtf;
    private readonly CollaborationSession _sessionContext = sessionContext;
    private readonly IProjectSnapshotManagerProxy _hostProjectManagerProxy = hostProjectManagerProxy;
    private readonly ProjectSnapshotManagerBase _projectManager = projectManager;
    private readonly IErrorReporter _errorReporter = errorReporter;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        // We wire the changed event up early because any changed events that fire will ensure we have the most
        // up-to-date state.
        _hostProjectManagerProxy.Changed += HostProxyProjectManager_Changed;

        var projectManagerState = await _hostProjectManagerProxy.GetProjectManagerStateAsync(cancellationToken);

        await InitializeGuestProjectManagerAsync(projectManagerState.ProjectHandles);
    }

    public async Task DisposeAsync()
    {
        _hostProjectManagerProxy.Changed -= HostProxyProjectManager_Changed;

        await _dispatcher.Scheduler;

        var projects = _projectManager.GetProjects();
        foreach (var project in projects)
        {
            try
            {
                _projectManager.ProjectRemoved(project.Key);
            }
            catch (Exception ex)
            {
                _errorReporter.ReportError(ex, project);
            }
        }
    }

    ValueTask System.IAsyncDisposable.DisposeAsync()
    {
        return new ValueTask(DisposeAsync());
    }

    // Internal for testing
    internal async ValueTask UpdateGuestProjectManagerAsync(ProjectChangeEventProxyArgs args)
    {
        await _dispatcher.Scheduler;

        if (args.Kind == ProjectProxyChangeKind.ProjectAdded)
        {
            var guestPath = ResolveGuestPath(args.ProjectFilePath);
            var guestIntermediateOutputPath = ResolveGuestPath(args.IntermediateOutputPath);
            var hostProject = new HostProject(guestPath, guestIntermediateOutputPath, args.Newer!.Configuration, args.Newer.RootNamespace);
            _projectManager.ProjectAdded(hostProject);

            if (args.Newer.ProjectWorkspaceState != null)
            {
                _projectManager.ProjectWorkspaceStateChanged(hostProject.Key, args.Newer.ProjectWorkspaceState);
            }
        }
        else if (args.Kind == ProjectProxyChangeKind.ProjectRemoved)
        {
            var guestPath = ResolveGuestPath(args.ProjectFilePath);
            var projectKeys = _projectManager.GetAllProjectKeys(guestPath);
            foreach (var projectKey in projectKeys)
            {
                _projectManager.ProjectRemoved(projectKey);
            }
        }
        else if (args.Kind == ProjectProxyChangeKind.ProjectChanged)
        {
            if (!args.Older!.Configuration.Equals(args.Newer!.Configuration))
            {
                var guestPath = ResolveGuestPath(args.Newer.FilePath);
                var guestIntermediateOutputPath = ResolveGuestPath(args.Newer.IntermediateOutputPath);
                var hostProject = new HostProject(guestPath, guestIntermediateOutputPath, args.Newer.Configuration, args.Newer.RootNamespace);
                _projectManager.ProjectConfigurationChanged(hostProject);
            }
            else if (args.Older.ProjectWorkspaceState != args.Newer.ProjectWorkspaceState ||
                args.Older.ProjectWorkspaceState?.Equals(args.Newer.ProjectWorkspaceState) == false)
            {
                var guestPath = ResolveGuestPath(args.Newer.FilePath);
                var projectKeys = _projectManager.GetAllProjectKeys(guestPath);
                foreach (var projectKey in projectKeys)
                {
                    _projectManager.ProjectWorkspaceStateChanged(projectKey, args.Newer.ProjectWorkspaceState);
                }
            }
        }
    }

    private async Task InitializeGuestProjectManagerAsync(IReadOnlyList<ProjectSnapshotHandleProxy> projectHandles)
    {
        await _dispatcher.Scheduler;

        foreach (var projectHandle in projectHandles)
        {
            var guestPath = ResolveGuestPath(projectHandle.FilePath);
            var guestIntermediateOutputPath = ResolveGuestPath(projectHandle.IntermediateOutputPath);
            var hostProject = new HostProject(guestPath, guestIntermediateOutputPath, projectHandle.Configuration, projectHandle.RootNamespace);
            _projectManager.ProjectAdded(hostProject);

            if (projectHandle.ProjectWorkspaceState is not null)
            {
                _projectManager.ProjectWorkspaceStateChanged(hostProject.Key, projectHandle.ProjectWorkspaceState);
            }
        }
    }

    private void HostProxyProjectManager_Changed(object sender, ProjectChangeEventProxyArgs args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        _jtf.Run(async () =>
        {
            try
            {
                await UpdateGuestProjectManagerAsync(args);
            }
            catch (Exception ex)
            {
                _errorReporter.ReportError(ex);
            }
        });
    }

    private string ResolveGuestPath(Uri filePath)
    {
        return _sessionContext.ConvertSharedUriToLocalPath(filePath);
    }
}
