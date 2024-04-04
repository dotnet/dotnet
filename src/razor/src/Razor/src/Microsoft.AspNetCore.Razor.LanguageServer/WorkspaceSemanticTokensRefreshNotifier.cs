// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class WorkspaceSemanticTokensRefreshNotifier : IWorkspaceSemanticTokensRefreshNotifier, IDisposable
{
    private static readonly TimeSpan s_delay = TimeSpan.FromMilliseconds(250);

    private readonly IClientCapabilitiesService _clientCapabilitiesService;
    private readonly IClientConnection _clientConnection;
    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly IDisposable _optionsChangeListener;

    private readonly object _gate = new();
    private bool? _supportsRefresh;
    private bool _waitingToRefresh;
    private Task _refreshTask = Task.CompletedTask;

    private bool _isColoringBackground;

    public WorkspaceSemanticTokensRefreshNotifier(
        IClientCapabilitiesService clientCapabilitiesService,
        IClientConnection clientConnection,
        RazorLSPOptionsMonitor optionsMonitor)
    {
        _clientCapabilitiesService = clientCapabilitiesService;
        _clientConnection = clientConnection;

        _disposeTokenSource = new();

        _isColoringBackground = optionsMonitor.CurrentValue.ColorBackground;
        _optionsChangeListener = optionsMonitor.OnChange(HandleOptionsChange);
    }

    public void Dispose()
    {
        _optionsChangeListener.Dispose();

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    private void HandleOptionsChange(RazorLSPOptions options, string _)
    {
        if (options.ColorBackground != _isColoringBackground)
        {
            _isColoringBackground = options.ColorBackground;
            NotifyWorkspaceSemanticTokensRefresh();
        }
    }

    public void NotifyWorkspaceSemanticTokensRefresh()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        lock (_gate)
        {
            if (_waitingToRefresh)
            {
                // We're going to refresh shortly.
                return;
            }

            _supportsRefresh ??= _clientCapabilitiesService.ClientCapabilities.Workspace?.SemanticTokens?.RefreshSupport ?? false;

            if (_supportsRefresh is false)
            {
                return;
            }

            _refreshTask = RefreshAfterDelayAsync();
            _waitingToRefresh = true;
        }

        async Task RefreshAfterDelayAsync()
        {
            await Task.Delay(s_delay, _disposeTokenSource.Token).ConfigureAwait(false);

            _clientConnection
                .SendNotificationAsync(Methods.WorkspaceSemanticTokensRefreshName, _disposeTokenSource.Token)
                .Forget();

            _waitingToRefresh = false;
        }
    }

    internal TestAccessor GetTestAccessor()
        => new(this);

    internal class TestAccessor(WorkspaceSemanticTokensRefreshNotifier instance)
    {
        public async Task WaitForNotificationAsync()
        {
            Task refreshTask;

            lock (instance._gate)
            {
                refreshTask = instance._refreshTask;
            }

            await refreshTask.ConfigureAwait(false);
        }
    }
}
