// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class WorkspaceDiagnosticsRefresher : IRazorStartupService, IDisposable
{
    private readonly AsyncBatchingWorkQueue _queue;
    private readonly ProjectSnapshotManager _projectSnapshotManager;
    private readonly IClientCapabilitiesService _clientCapabilitiesService;
    private readonly IClientConnection _clientConnection;
    private bool? _supported;
    private CancellationTokenSource _disposeTokenSource = new();

    public WorkspaceDiagnosticsRefresher(
        ProjectSnapshotManager projectSnapshotManager,
        IClientCapabilitiesService clientCapabilitiesService,
        IClientConnection clientConnection,
        TimeSpan? delay = null)
    {
        _clientConnection = clientConnection;
        _projectSnapshotManager = projectSnapshotManager;
        _clientCapabilitiesService = clientCapabilitiesService;
        _queue = new(
            delay ?? TimeSpan.FromMilliseconds(200),
            ProcessBatchAsync,
            _disposeTokenSource.Token);
        _projectSnapshotManager.Changed += ProjectSnapshotManager_Changed;
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _projectSnapshotManager.Changed -= ProjectSnapshotManager_Changed;
        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    private ValueTask ProcessBatchAsync(CancellationToken token)
    {
        _clientConnection
            .SendNotificationAsync(Methods.WorkspaceDiagnosticRefreshName, token)
            .Forget();

        return default;
    }

    private void ProjectSnapshotManager_Changed(object? sender, ProjectChangeEventArgs e)
    {
        if (e.IsSolutionClosing)
        {
            return;
        }

        _supported ??= GetSupported();

        if (_supported != true)
        {
            return;
        }

        if (e.Kind is not ProjectChangeKind.DocumentChanged)
        {
            _queue.AddWork();
        }
    }

    private bool? GetSupported()
    {
        if (!_clientCapabilitiesService.CanGetClientCapabilities)
        {
            return null;
        }

        return _clientCapabilitiesService.ClientCapabilities.Workspace?.Diagnostics?.RefreshSupport;
    }

    internal TestAccessor GetTestAccessor()
        => new(this);

    internal sealed class TestAccessor(WorkspaceDiagnosticsRefresher instance)
    {

        public Task WaitForRefreshAsync()
        {
            if (instance._disposeTokenSource.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            return instance._queue.WaitUntilCurrentBatchCompletesAsync();
        }
    }
}
