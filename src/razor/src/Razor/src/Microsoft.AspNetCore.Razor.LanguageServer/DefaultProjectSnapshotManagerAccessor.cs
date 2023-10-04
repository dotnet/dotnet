// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultProjectSnapshotManagerAccessor : ProjectSnapshotManagerAccessor, IDisposable
{
    private readonly IEnumerable<IProjectSnapshotChangeTrigger> _changeTriggers;
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor;
    private readonly AdhocWorkspaceFactory _workspaceFactory;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private ProjectSnapshotManagerBase? _instance;
    private bool _disposed;

    public DefaultProjectSnapshotManagerAccessor(
        IEnumerable<IProjectSnapshotChangeTrigger> changeTriggers,
        IOptionsMonitor<RazorLSPOptions> optionsMonitor,
        AdhocWorkspaceFactory workspaceFactory,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher)
    {
        _changeTriggers = changeTriggers ?? throw new ArgumentNullException(nameof(changeTriggers));
        _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
        _workspaceFactory = workspaceFactory ?? throw new ArgumentNullException(nameof(workspaceFactory));
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher ?? throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
    }

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
                    ErrorReporter.Instance,
                    _changeTriggers,
                    workspace,
                    _projectSnapshotManagerDispatcher);
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
