// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.Workspaces;

public abstract class WorkspaceTestBase : ToolingTestBase
{
    private bool _initialized;
    private HostServices? _hostServices;
    private Workspace? _workspace;
    private IProjectEngineFactoryProvider? _projectEngineFactoryProvider;

    protected WorkspaceTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    protected HostServices HostServices
    {
        get
        {
            EnsureInitialized();
            return _hostServices;
        }
    }

    protected Workspace Workspace
    {
        get
        {
            EnsureInitialized();
            return _workspace;
        }
    }

    private protected IProjectEngineFactoryProvider ProjectEngineFactoryProvider
    {
        get
        {
            EnsureInitialized();
            return _projectEngineFactoryProvider;
        }
    }

    protected virtual void ConfigureWorkspaceServices(List<IWorkspaceService> services)
    {
    }

    protected virtual void ConfigureLanguageServices(List<ILanguageService> services)
    {
    }

    protected virtual void ConfigureWorkspace(AdhocWorkspace workspace)
    {
    }

    protected virtual void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
    }

    [MemberNotNull(nameof(_hostServices), nameof(_workspace), nameof(_projectEngineFactoryProvider))]
    private void EnsureInitialized()
    {
        if (_initialized)
        {
            _hostServices.AssumeNotNull();
            _workspace.AssumeNotNull();
            _projectEngineFactoryProvider.AssumeNotNull();
            return;
        }

        _projectEngineFactoryProvider = new TestProjectEngineFactoryProvider()
        {
            Configure = ConfigureProjectEngine,
        };

        var workspaceServices = new List<IWorkspaceService>();
        ConfigureWorkspaceServices(workspaceServices);

        var languageServices = new List<ILanguageService>();
        ConfigureLanguageServices(languageServices);

        _hostServices = TestServices.Create(workspaceServices, languageServices);
        _workspace = TestWorkspace.Create(_hostServices, ConfigureWorkspace);
        AddDisposable(_workspace);
        _initialized = true;
    }
}
