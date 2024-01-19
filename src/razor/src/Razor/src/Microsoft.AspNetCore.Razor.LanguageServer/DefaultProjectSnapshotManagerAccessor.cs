// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultProjectSnapshotManagerAccessor(
    IEnumerable<IProjectSnapshotChangeTrigger> changeTriggers,
    IOptionsMonitor<RazorLSPOptions> optionsMonitor,
    AdhocWorkspaceFactory workspaceFactory,
    ProjectSnapshotManagerDispatcher dispatcher,
    IErrorReporter errorReporter) : ProjectSnapshotManagerAccessor, IDisposable
{
    private readonly IEnumerable<IProjectSnapshotChangeTrigger> _changeTriggers = changeTriggers;
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor = optionsMonitor;
    private readonly AdhocWorkspaceFactory _workspaceFactory = workspaceFactory;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;
    private readonly IErrorReporter _errorReporter = errorReporter;
    private ProjectSnapshotManagerBase? _instance;
    private bool _disposed;

    public override ProjectSnapshotManagerBase Instance
    {
        get
        {
            if (_instance is null)
            {
                var workspace = _workspaceFactory.Create(
                    workspaceServices: new[]
                    {
                        new RemoteProjectSnapshotProjectEngineFactory(_optionsMonitor)
                    });
                _instance = new DefaultProjectSnapshotManager(
                    _errorReporter,
                    _changeTriggers,
                    workspace,
                    _dispatcher);
            }

            return _instance;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        _instance?.Workspace.Dispose();
    }
}
