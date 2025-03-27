// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.Workspaces;

public abstract class WorkspaceTestBase(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private bool _initialized;
    private HostServices? _hostServices;
    private Workspace? _workspace;
    private IWorkspaceProvider? _workspaceProvider;
    private IProjectEngineFactoryProvider? _projectEngineFactoryProvider;
    private LanguageServerFeatureOptions? _languageServerFeatureOptions;

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

    private protected IWorkspaceProvider WorkspaceProvider
    {
        get
        {
            EnsureInitialized();
            return _workspaceProvider;
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

    private protected LanguageServerFeatureOptions LanguageServerFeatureOptions
    {
        get
        {
            EnsureInitialized();
            return _languageServerFeatureOptions;
        }
    }

    private protected RazorCompilerOptions CompilerOptions
        => LanguageServerFeatureOptions.ToCompilerOptions();

    private protected override TestProjectSnapshotManager CreateProjectSnapshotManager()
        => CreateProjectSnapshotManager(ProjectEngineFactoryProvider, LanguageServerFeatureOptions);

    private protected override TestProjectSnapshotManager CreateProjectSnapshotManager(IProjectEngineFactoryProvider projectEngineFactoryProvider)
        => CreateProjectSnapshotManager(projectEngineFactoryProvider, LanguageServerFeatureOptions);

    protected virtual void ConfigureWorkspace(AdhocWorkspace workspace)
    {
    }

    protected virtual void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
    }

    [MemberNotNull(
        nameof(_hostServices),
        nameof(_workspace),
        nameof(_workspaceProvider),
        nameof(_projectEngineFactoryProvider),
        nameof(_languageServerFeatureOptions))]
    private void EnsureInitialized()
    {
        if (_initialized)
        {
            _hostServices.AssumeNotNull();
            _workspace.AssumeNotNull();
            _workspaceProvider.AssumeNotNull();
            _projectEngineFactoryProvider.AssumeNotNull();
            _languageServerFeatureOptions.AssumeNotNull();
            return;
        }

        _projectEngineFactoryProvider = TestProjectEngineFactoryProvider.Instance.AddConfigure(ConfigureProjectEngine);

        _hostServices = MefHostServices.DefaultHost;
        _workspace = TestWorkspace.Create(_hostServices, ConfigureWorkspace);
        AddDisposable(_workspace);
        _workspaceProvider = new TestWorkspaceProvider(_workspace);
        _languageServerFeatureOptions = TestLanguageServerFeatureOptions.Instance;
        _initialized = true;
    }

    /// <summary>
    ///  Calls <see cref="Workspace.TryApplyChanges(Solution)"/> and waits for <see cref="Workspace.WorkspaceChanged"/>
    ///  to stop firing events.
    /// </summary>
    protected Task<bool> UpdateWorkspaceAsync(Solution solution)
    {
        return Task.Run(
            async () =>
            {
                var currentCount = 0;

                Workspace.WorkspaceChanged += OnWorkspaceChanged;

                if (!Workspace.TryApplyChanges(solution))
                {
                    return false;
                }

                int lastCount;

                do
                {
                    lastCount = currentCount;
                    await Task.Delay(50);
                }
                while (lastCount != currentCount);

                Workspace.WorkspaceChanged -= OnWorkspaceChanged;
                return true;

                void OnWorkspaceChanged(object? sender, WorkspaceChangeEventArgs e)
                {
                    currentCount++;
                }
            },
            DisposalToken);
    }

    private sealed class TestWorkspaceProvider(Workspace workspace) : IWorkspaceProvider
    {
        public Workspace GetWorkspace() => workspace;
    }
}
