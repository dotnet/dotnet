// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(IRazorStartupService))]
internal class CSharpVirtualDocumentManager : IRazorStartupService, IDisposable
{
    private readonly LSPDocumentManager _lspDocumentManager;

    private static readonly TimeSpan s_defaultDelay = TimeSpan.FromMilliseconds(200);
    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue _workQueue;

    [ImportingConstructor]
    public CSharpVirtualDocumentManager(
        LSPDocumentManager lspDocumentManager,
        IProjectSnapshotManager projectManager)
    {
        _lspDocumentManager = lspDocumentManager;

        _disposeTokenSource = new();
        _workQueue = new AsyncBatchingWorkQueue(s_defaultDelay, ProcessBatchAsync, _disposeTokenSource.Token);

        projectManager.Changed += ProjectManager_Changed;
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    private ValueTask ProcessBatchAsync(CancellationToken token)
    {
        if (!token.IsCancellationRequested)
        {
            _lspDocumentManager.RefreshVirtualDocuments();
        }

        return default;
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs e)
    {
        if (e.IsSolutionClosing)
        {
            return;
        }

        switch (e.Kind)
        {
            case ProjectChangeKind.DocumentAdded:
            case ProjectChangeKind.DocumentRemoved:
            case ProjectChangeKind.ProjectChanged:
            case ProjectChangeKind.ProjectAdded:
            case ProjectChangeKind.ProjectRemoved:
                _workQueue.AddWork();
                break;
        }
    }
}
