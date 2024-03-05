// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class LspProjectSnapshotManagerAccessor(
    IEnumerable<IProjectSnapshotChangeTrigger> changeTriggers,
    IOptionsMonitor<RazorLSPOptions> optionsMonitor,
    ProjectSnapshotManagerDispatcher dispatcher,
    IErrorReporter errorReporter) : IProjectSnapshotManagerAccessor
{
    private readonly IEnumerable<IProjectSnapshotChangeTrigger> _changeTriggers = changeTriggers;
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor = optionsMonitor;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;
    private readonly IErrorReporter _errorReporter = errorReporter;
    private ProjectSnapshotManagerBase? _instance;

    public ProjectSnapshotManagerBase Instance
    {
        get
        {
            if (_instance is null)
            {
                var projectEngineFactoryProvider = new LspProjectEngineFactoryProvider(_optionsMonitor);

                _instance = new DefaultProjectSnapshotManager(
                    _changeTriggers,
                    projectEngineFactoryProvider,
                    _dispatcher,
                    _errorReporter);
            }

            return _instance;
        }
    }

    public bool TryGetInstance([NotNullWhen(true)] out ProjectSnapshotManagerBase? instance)
    {
        instance = _instance;
        return instance is not null;
    }
}
