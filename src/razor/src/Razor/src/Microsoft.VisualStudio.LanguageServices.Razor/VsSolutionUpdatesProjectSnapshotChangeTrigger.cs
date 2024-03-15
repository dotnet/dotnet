// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Export(typeof(IRazorStartupService))]
internal class VsSolutionUpdatesProjectSnapshotChangeTrigger : IRazorStartupService, IVsUpdateSolutionEvents2, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IProjectSnapshotManager _projectManager;
    private readonly IProjectWorkspaceStateGenerator _workspaceStateGenerator;
    private readonly IWorkspaceProvider _workspaceProvider;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher;
    private readonly JoinableTaskFactory _jtf;
    private readonly JoinableTask _initializeTask;

    private CancellationTokenSource? _activeSolutionCancellationTokenSource = new();
    private uint _updateCookie;
    private IVsSolutionBuildManager? _solutionBuildManager;

    private Task? _projectBuiltTask;

    [ImportingConstructor]
    public VsSolutionUpdatesProjectSnapshotChangeTrigger(
        [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
        IProjectSnapshotManager projectManager,
        IProjectWorkspaceStateGenerator workspaceStateGenerator,
        IWorkspaceProvider workspaceProvider,
        ProjectSnapshotManagerDispatcher dispatcher,
        JoinableTaskContext joinableTaskContext)
    {
        _serviceProvider = serviceProvider;
        _projectManager = projectManager;
        _workspaceStateGenerator = workspaceStateGenerator;
        _workspaceProvider = workspaceProvider;
        _dispatcher = dispatcher;
        _jtf = joinableTaskContext.Factory;

        _projectManager.Changed += ProjectManager_Changed;

        _initializeTask = _jtf.RunAsync(async () =>
        {
            await _jtf.SwitchToMainThreadAsync();

            // Attach the event sink to solution update events.
            _solutionBuildManager = _serviceProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager;
            Assumes.Present(_solutionBuildManager);

            // We expect this to be called only once. So we don't need to Unadvise.
            var hr = _solutionBuildManager.AdviseUpdateSolutionEvents(this, out _updateCookie);
            Marshal.ThrowExceptionForHR(hr);
        });
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
        Debug.Assert(_activeSolutionCancellationTokenSource != null, "We should not get build events when there is no active solution");

        _projectBuiltTask = OnProjectBuiltAsync(pHierProj, _activeSolutionCancellationTokenSource?.Token ?? CancellationToken.None);

        return VSConstants.S_OK;
    }

    public void Dispose()
    {
        _jtf.AssertUIThread();

        _solutionBuildManager?.UnadviseUpdateSolutionEvents(_updateCookie);
        _activeSolutionCancellationTokenSource?.Cancel();
        _activeSolutionCancellationTokenSource?.Dispose();
        _activeSolutionCancellationTokenSource = null;
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs args)
    {
        if (args.SolutionIsClosing)
        {
            _activeSolutionCancellationTokenSource?.Cancel();
            _activeSolutionCancellationTokenSource?.Dispose();
            _activeSolutionCancellationTokenSource = null;
        }
        else
        {
            _activeSolutionCancellationTokenSource ??= new CancellationTokenSource();
        }
    }

    private async Task OnProjectBuiltAsync(IVsHierarchy projectHierarchy, CancellationToken cancellationToken)
    {
        var projectFilePath = await projectHierarchy.GetProjectFilePathAsync(_jtf, cancellationToken);
        if (projectFilePath is null)
        {
            return;
        }

        await _dispatcher.RunAsync(() =>
        {
            if (_projectManager is null)
            {
                return;
            }

            var projectKeys = _projectManager.GetAllProjectKeys(projectFilePath);
            foreach (var projectKey in projectKeys)
            {
                if (_projectManager.TryGetLoadedProject(projectKey, out var projectSnapshot))
                {
                    var workspace = _workspaceProvider.GetWorkspace();
                    var workspaceProject = workspace.CurrentSolution.Projects.FirstOrDefault(wp => ProjectKey.From(wp) == projectSnapshot.Key);
                    if (workspaceProject is not null)
                    {
                        // Trigger a tag helper update by forcing the project manager to see the workspace Project
                        // from the current solution.
                        _workspaceStateGenerator.Update(workspaceProject, projectSnapshot, cancellationToken);
                    }
                }
            }
        }, cancellationToken);
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(VsSolutionUpdatesProjectSnapshotChangeTrigger instance)
    {
        public JoinableTask InitializeTask => instance._initializeTask;
        public Task? OnProjectBuiltTask => instance._projectBuiltTask;

        public Task OnProjectBuiltAsync(IVsHierarchy projectHierarchy, CancellationToken cancellationToken)
            => instance.OnProjectBuiltAsync(projectHierarchy, cancellationToken);
    }
}
