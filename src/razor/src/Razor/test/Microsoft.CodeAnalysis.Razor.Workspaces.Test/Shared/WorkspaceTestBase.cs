// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor;

public abstract class WorkspaceTestBase : TestBase
{
    private bool _initialized;
    private HostServices _hostServices;
    private Workspace _workspace;

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

    private void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        var workspaceServices = new List<IWorkspaceService>()
        {
            new TestProjectSnapshotProjectEngineFactory()
            {
                Configure = ConfigureProjectEngine,
            },
        };
        ConfigureWorkspaceServices(workspaceServices);

        var languageServices = new List<ILanguageService>();
        ConfigureLanguageServices(languageServices);

        _hostServices = TestServices.Create(workspaceServices, languageServices);
        _workspace = TestWorkspace.Create(_hostServices, ConfigureWorkspace);
        AddDisposable(_workspace);
        _initialized = true;
    }
}
