// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class WorkspaceSemanticTokensRefreshPublisher : IWorkspaceSemanticTokensRefreshPublisher, IDisposable
{
    private const string WorkspaceSemanticTokensRefreshKey = "WorkspaceSemanticTokensRefresh";
    private static readonly TimeSpan s_debounceTimeSpan = TimeSpan.FromMilliseconds(250);

    private readonly IClientCapabilitiesService _clientCapabilitiesService;
    private readonly IClientConnection _clientConnection;
    private readonly BatchingWorkQueue _workQueue;

    private bool _isColoringBackground;

    public WorkspaceSemanticTokensRefreshPublisher(
        IClientCapabilitiesService clientCapabilitiesService,
        IClientConnection clientConnection,
        IErrorReporter errorReporter,
        RazorLSPOptionsMonitor razorLSPOptionsMonitor)
    {
        _clientCapabilitiesService = clientCapabilitiesService;
        _clientConnection = clientConnection;
        _workQueue = new BatchingWorkQueue(s_debounceTimeSpan, StringComparer.Ordinal, errorReporter: errorReporter);

        _isColoringBackground = razorLSPOptionsMonitor.CurrentValue.ColorBackground;
        razorLSPOptionsMonitor.OnChange(HandleOptionsChange);
    }

    private void HandleOptionsChange(RazorLSPOptions options, string _)
    {
        if (options.ColorBackground != _isColoringBackground)
        {
            _isColoringBackground = options.ColorBackground;
            EnqueueWorkspaceSemanticTokensRefresh();
        }
    }

    public void EnqueueWorkspaceSemanticTokensRefresh()
    {
        var capabilities = _clientCapabilitiesService.ClientCapabilities;
        var useWorkspaceRefresh = capabilities?.Workspace?.SemanticTokens?.RefreshSupport ?? false;

        if (useWorkspaceRefresh)
        {
            var workItem = new SemanticTokensRefreshWorkItem(_clientConnection);
            _workQueue.Enqueue(WorkspaceSemanticTokensRefreshKey, workItem);
        }
    }

    public void Dispose()
    {
        _workQueue.Dispose();
    }

    private class SemanticTokensRefreshWorkItem : BatchableWorkItem
    {
        private readonly IClientConnection _clientConnection;

        public SemanticTokensRefreshWorkItem(IClientConnection clientConnection)
        {
            _clientConnection = clientConnection;
        }

        public override ValueTask ProcessAsync(CancellationToken cancellationToken)
        {
            var task = _clientConnection.SendNotificationAsync(Methods.WorkspaceSemanticTokensRefreshName, cancellationToken);

            return new ValueTask(task);
        }
    }

    internal TestAccessor GetTestAccessor()
        => new(this);

    internal class TestAccessor
    {
        private readonly WorkspaceSemanticTokensRefreshPublisher _publisher;

        internal TestAccessor(WorkspaceSemanticTokensRefreshPublisher publisher)
        {
            _publisher = publisher;
        }

        public void WaitForEmpty()
        {
            var workQueueTestAccessor = _publisher._workQueue.GetTestAccessor();
            workQueueTestAccessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);
            while (workQueueTestAccessor.IsScheduledOrRunning)
            {
                workQueueTestAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromMilliseconds(50));
            }
        }
    }
}
