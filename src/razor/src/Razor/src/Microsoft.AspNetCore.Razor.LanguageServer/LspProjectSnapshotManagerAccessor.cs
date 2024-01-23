// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class LspProjectSnapshotManagerAccessor(
    IEnumerable<IProjectSnapshotChangeTrigger> changeTriggers,
    IOptionsMonitor<RazorLSPOptions> optionsMonitor,
    IAdhocWorkspaceFactory workspaceFactory,
    ProjectSnapshotManagerDispatcher dispatcher,
    IErrorReporter errorReporter) : IProjectSnapshotManagerAccessor, IDisposable
{
    private readonly IEnumerable<IProjectSnapshotChangeTrigger> _changeTriggers = changeTriggers;
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor = optionsMonitor;
    private readonly IAdhocWorkspaceFactory _workspaceFactory = workspaceFactory;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;
    private readonly IErrorReporter _errorReporter = errorReporter;
    private ProjectSnapshotManagerBase? _instance;
    private bool _disposed;

    public ProjectSnapshotManagerBase Instance
    {
        get
        {
            if (_instance is null)
            {
                var projectEngineFactoryProvider = new LspProjectEngineFactoryProvider(_optionsMonitor);
                var workspace = _workspaceFactory.Create();

                _instance = new DefaultProjectSnapshotManager(
                    _errorReporter,
                    _changeTriggers,
                    workspace,
                    projectEngineFactoryProvider,
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
