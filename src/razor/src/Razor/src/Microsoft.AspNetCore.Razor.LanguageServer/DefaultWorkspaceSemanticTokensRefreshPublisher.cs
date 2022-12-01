// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultWorkspaceSemanticTokensRefreshPublisher : WorkspaceSemanticTokensRefreshPublisher, IDisposable
{
    private const string WorkspaceSemanticTokensRefreshKey = "WorkspaceSemanticTokensRefresh";
    private readonly IInitializeManager<InitializeParams, InitializeResult> _settingsManager;
    private readonly ClientNotifierServiceBase _notifierService;
    private readonly BatchingWorkQueue _workQueue;
    private static readonly TimeSpan s_debounceTimeSpan = TimeSpan.FromMilliseconds(250);

    public DefaultWorkspaceSemanticTokensRefreshPublisher(IInitializeManager<InitializeParams, InitializeResult> settingsManager, ClientNotifierServiceBase clientNotifier, ErrorReporter errorReporter)
    {
        if (settingsManager is null)
        {
            throw new ArgumentNullException(nameof(settingsManager));
        }

        if (errorReporter is null)
        {
            throw new ArgumentNullException(nameof(errorReporter));
        }

        _settingsManager = settingsManager;
        _notifierService = clientNotifier;
        _workQueue = new BatchingWorkQueue(s_debounceTimeSpan, StringComparer.Ordinal, errorReporter: errorReporter);
    }

    public override void EnqueueWorkspaceSemanticTokensRefresh()
    {
        var clientSettings = _settingsManager.GetInitializeParams();
        var useWorkspaceRefresh = clientSettings.Capabilities?.Workspace is not null &&
            (clientSettings.Capabilities.Workspace.SemanticTokens?.RefreshSupport ?? false);

        if (useWorkspaceRefresh)
        {
            var workItem = new SemanticTokensRefreshWorkItem(_notifierService);
            _workQueue.Enqueue(WorkspaceSemanticTokensRefreshKey, workItem);
        }
    }

    public void Dispose()
    {
        _workQueue.Dispose();
    }

    private class SemanticTokensRefreshWorkItem : BatchableWorkItem
    {
        private readonly ClientNotifierServiceBase _languageServer;

        public SemanticTokensRefreshWorkItem(ClientNotifierServiceBase languageServer)
        {
            _languageServer = languageServer;
        }

        public override ValueTask ProcessAsync(CancellationToken cancellationToken)
        {
            var task = _languageServer.SendNotificationAsync(Methods.WorkspaceSemanticTokensRefreshName, cancellationToken);

            return new ValueTask(task);
        }
    }

    internal TestAccessor GetTestAccessor()
        => new(this);

    internal class TestAccessor
    {
        private readonly DefaultWorkspaceSemanticTokensRefreshPublisher _publisher;

        internal TestAccessor(DefaultWorkspaceSemanticTokensRefreshPublisher publisher)
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
