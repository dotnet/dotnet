// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.ProjectSystem.VS;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(IRazorStartupService))]
internal class VisualStudioSolutionCloseChangeTrigger : IRazorStartupService, IVsSolutionEvents3, IDisposable
{
    private IVsSolution? _solution;
    private readonly IServiceProvider _serviceProvider;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly JoinableTaskContext _joinableTaskContext;

    private uint _cookie;

    [ImportingConstructor]
    public VisualStudioSolutionCloseChangeTrigger(
       [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
       ProjectSnapshotManager projectManager,
       JoinableTaskContext joinableTaskContext)
    {
        _serviceProvider = serviceProvider;
        _projectManager = projectManager;
        _joinableTaskContext = joinableTaskContext;

        _ = _joinableTaskContext.Factory.RunAsync(async () =>
        {
            await _joinableTaskContext.Factory.SwitchToMainThreadAsync();

            if (_serviceProvider.GetService(typeof(SVsSolution)) is IVsSolution solution)
            {
                _solution = solution;
                _solution.AdviseSolutionEvents(this, out _cookie);
            }
        });
    }

    public void Dispose()
    {
        _solution?.UnadviseSolutionEvents(_cookie);
    }

    public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
    {
        _projectManager.UpdateAsync(static updater => updater.SolutionOpened(), CancellationToken.None).Forget();
        return VSConstants.S_OK;
    }

    public int OnBeforeCloseSolution(object pUnkReserved)
    {
        _projectManager.UpdateAsync(static updater => updater.SolutionClosed(), CancellationToken.None).Forget();
        return VSConstants.S_OK;
    }

    public int OnAfterCloseSolution(object pUnkReserved)
    {
        _projectManager.UpdateAsync(static updater => updater.SolutionOpened(), CancellationToken.None).Forget();
        return VSConstants.S_OK;
    }

    #region Events we're not interested in
    public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
    {
        return HResult.NotImplemented;
    }

    public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
    {
        return HResult.NotImplemented;
    }

    public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
    {
        return HResult.NotImplemented;
    }

    public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
    {
        return HResult.NotImplemented;
    }

    public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
    {
        return HResult.NotImplemented;
    }

    public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
    {
        return HResult.NotImplemented;
    }

    public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
    {
        return HResult.NotImplemented;
    }

    public int OnAfterMergeSolution(object pUnkReserved)
    {
        return HResult.NotImplemented;
    }

    public int OnBeforeOpeningChildren(IVsHierarchy pHierarchy)
    {
        return HResult.NotImplemented;
    }

    public int OnAfterOpeningChildren(IVsHierarchy pHierarchy)
    {
        return HResult.NotImplemented;
    }

    public int OnBeforeClosingChildren(IVsHierarchy pHierarchy)
    {
        return HResult.NotImplemented;
    }

    public int OnAfterClosingChildren(IVsHierarchy pHierarchy)
    {
        return HResult.NotImplemented;
    }

    #endregion
}
