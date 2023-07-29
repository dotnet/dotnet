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
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Export(typeof(IProjectSnapshotChangeTrigger))]
internal class VsSolutionUpdatesProjectSnapshotChangeTrigger : IProjectSnapshotChangeTrigger, IVsUpdateSolutionEvents2, IDisposable
{
    private readonly IServiceProvider _services;
    private readonly TextBufferProjectService _projectService;
    private readonly ProjectWorkspaceStateGenerator _workspaceStateGenerator;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;
    private ProjectSnapshotManagerBase? _projectManager;
    private CancellationTokenSource? _activeSolutionCancellationTokenSource;
    private uint _updateCookie;
    private IVsSolutionBuildManager? _solutionBuildManager;

    [ImportingConstructor]
    public VsSolutionUpdatesProjectSnapshotChangeTrigger(
        [Import(typeof(SVsServiceProvider))] IServiceProvider services,
        TextBufferProjectService projectService,
        ProjectWorkspaceStateGenerator workspaceStateGenerator,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (projectService is null)
        {
            throw new ArgumentNullException(nameof(projectService));
        }

        if (workspaceStateGenerator is null)
        {
            throw new ArgumentNullException(nameof(workspaceStateGenerator));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        _services = services;
        _projectService = projectService;
        _workspaceStateGenerator = workspaceStateGenerator;
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _joinableTaskContext = joinableTaskContext;
        _activeSolutionCancellationTokenSource = new CancellationTokenSource();
    }

    internal Task? CurrentUpdateTaskForTests { get; private set; }

    public void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        _projectManager = projectManager;
        _projectManager.Changed += ProjectManager_Changed;

        _ = _joinableTaskContext.Factory.RunAsync(async () =>
        {
            await _joinableTaskContext.Factory.SwitchToMainThreadAsync();

            // Attach the event sink to solution update events.
            if (_services.GetService(typeof(SVsSolutionBuildManager)) is IVsSolutionBuildManager solutionBuildManager)
            {
                _solutionBuildManager = solutionBuildManager;

                // We expect this to be called only once. So we don't need to Unadvise.
                var hr = _solutionBuildManager.AdviseUpdateSolutionEvents(this, out _updateCookie);
                Marshal.ThrowExceptionForHR(hr);
            }
        });
    }

    public int UpdateSolution_Begin(ref int pfCancelUpdate) => VSConstants.S_OK;

    public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand) => VSConstants.S_OK;

    public int UpdateSolution_StartUpdate(ref int pfCancelUpdate) => VSConstants.S_OK;

    public int UpdateSolution_Cancel() => VSConstants.S_OK;

    public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy) => VSConstants.S_OK;

    public int UpdateProjectCfg_Begin(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, ref int pfCancel) => VSConstants.S_OK;

    // This gets called when the project has finished building.
    public int UpdateProjectCfg_Done(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, int fSuccess, int fCancel)
    {
        Debug.Assert(_activeSolutionCancellationTokenSource != null, "We should not get build events when there is no active solution");

        CurrentUpdateTaskForTests = OnProjectBuiltAsync(pHierProj, _activeSolutionCancellationTokenSource?.Token ?? CancellationToken.None);

        return VSConstants.S_OK;
    }

    public void Dispose()
    {
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

    // Internal for testing
    internal Task OnProjectBuiltAsync(IVsHierarchy projectHierarchy, CancellationToken cancellationToken)
    {
        var projectFilePath = _projectService.GetProjectPath(projectHierarchy);
        return _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(() =>
        {
            if (_projectManager is null)
            {
                return;
            }

            var projectKeys = _projectManager.GetAllProjectKeys(projectFilePath);
            foreach (var projectKey in projectKeys)
            {
                var projectSnapshot = _projectManager?.GetLoadedProject(projectKey);
                if (projectSnapshot is not null)
                {
                    var workspaceProject = _projectManager?.Workspace.CurrentSolution.Projects.FirstOrDefault(wp => ProjectKey.From(wp) == projectSnapshot.Key);
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
}
