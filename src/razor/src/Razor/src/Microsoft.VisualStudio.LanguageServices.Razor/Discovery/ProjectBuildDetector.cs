// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.Razor.Discovery;

[Export(typeof(IRazorStartupService))]
internal sealed partial class ProjectBuildDetector : IRazorStartupService, IVsUpdateSolutionEvents2, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly IProjectStateUpdater _projectStateUpdater;
    private readonly IWorkspaceProvider _workspaceProvider;
    private readonly JoinableTaskFactory _jtf;
    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly JoinableTask _initializeTask;

    private uint _updateCookie;
    private IVsSolutionBuildManager? _solutionBuildManager;

    private Task? _projectBuiltTask;

    [ImportingConstructor]
    public ProjectBuildDetector(
        [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
        ProjectSnapshotManager projectManager,
        IProjectStateUpdater projectStateUpdater,
        IWorkspaceProvider workspaceProvider,
        JoinableTaskContext joinableTaskContext)
    {
        _serviceProvider = serviceProvider;
        _projectManager = projectManager;
        _projectStateUpdater = projectStateUpdater;
        _workspaceProvider = workspaceProvider;
        _jtf = joinableTaskContext.Factory;

        _disposeTokenSource = new();

        _projectManager.Changed += ProjectManager_Changed;

        _initializeTask = _jtf.RunAsync(async () =>
        {
            await _jtf.SwitchToMainThreadAsync(_disposeTokenSource.Token);

            // Attach the event sink to solution update events.
            _solutionBuildManager = _serviceProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager;
            Assumes.Present(_solutionBuildManager);

            // We expect this to be called only once. So we don't need to Unadvise.
            var hr = _solutionBuildManager.AdviseUpdateSolutionEvents(this, out _updateCookie);
            Marshal.ThrowExceptionForHR(hr);
        });
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();

        _jtf.AssertUIThread();

        _solutionBuildManager?.UnadviseUpdateSolutionEvents(_updateCookie);
    }

    public int UpdateSolution_Begin(ref int pfCancelUpdate)
        => VSConstants.S_OK;

    public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        => VSConstants.S_OK;

    public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        => VSConstants.S_OK;

    public int UpdateSolution_Cancel()
        => VSConstants.S_OK;

    public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        => VSConstants.S_OK;

    public int UpdateProjectCfg_Begin(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, ref int pfCancel)
        => VSConstants.S_OK;

    // This gets called when the project has finished building.
    public int UpdateProjectCfg_Done(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, int fSuccess, int fCancel)
    {
        _projectBuiltTask = OnProjectBuiltAsync(pHierProj, _disposeTokenSource.Token);

        return VSConstants.S_OK;
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs args)
    {
        if (args.IsSolutionClosing)
        {
            // If the solution is closing, cancel all existing updates.
            _projectStateUpdater.CancelUpdates();
        }
    }

    private async Task OnProjectBuiltAsync(IVsHierarchy projectHierarchy, CancellationToken cancellationToken)
    {
        var projectFilePath = await projectHierarchy.GetProjectFilePathAsync(_jtf, cancellationToken);
        if (projectFilePath is null)
        {
            return;
        }

        var projectKeys = _projectManager.GetProjectKeysWithFilePath(projectFilePath);
        if (projectKeys.IsEmpty)
        {
            return;
        }

        var workspace = _workspaceProvider.GetWorkspace();
        var solution = workspace.CurrentSolution;

        foreach (var projectKey in projectKeys)
        {
            if (solution.TryGetProject(projectKey, out var workspaceProject))
            {
                // Trigger a tag helper update by forcing the project manager to see the workspace Project
                // from the current solution.
                _projectStateUpdater.EnqueueUpdate(projectKey, workspaceProject.Id);
            }
        }
    }
}
