// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using ThreadHelper = Microsoft.VisualStudio.Shell.ThreadHelper;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Attaching to one solution load event requires implementing all methods of two interfaces.
    /// This helper class is designed to reduce the verbosity of the boilerplate code.
    /// </summary>
    public abstract class SolutionEventsListener : IVsSolutionEvents, IVsSolutionLoadEvents
    {
        private IVsSolution _vsSolution;
        private uint _cookie;

        protected void Advise(IVsSolution vsSolution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Assumes.Present(vsSolution);

            _vsSolution = vsSolution;
            var hr = _vsSolution.AdviseSolutionEvents(this, out _cookie);
            ErrorHandler.ThrowOnFailure(hr);
        }

        protected void Unadvise()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_cookie != 0)
            {
                _vsSolution?.UnadviseSolutionEvents(_cookie);
                _vsSolution = null;
            }
        }

        #region IVsSolutionEvents

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded) => VSConstants.S_OK;

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel) => VSConstants.S_OK;

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved) => VSConstants.S_OK;

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy) => VSConstants.S_OK;

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel) => VSConstants.S_OK;

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy) => VSConstants.S_OK;

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution) => VSConstants.S_OK;

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel) => VSConstants.S_OK;

        public int OnBeforeCloseSolution(object pUnkReserved) => VSConstants.S_OK;

        public int OnAfterCloseSolution(object pUnkReserved) => VSConstants.S_OK;

        #endregion IVsSolutionEvents

        #region IVsSolutionLoadEvents

        public virtual int OnBeforeOpenSolution(string pszSolutionFilename) => VSConstants.S_OK;

        public virtual int OnBeforeBackgroundSolutionLoadBegins() => VSConstants.S_OK;

        public virtual int OnQueryBackgroundLoadProjectBatch(out bool pfShouldDelayLoadToNextIdle)
        {
            pfShouldDelayLoadToNextIdle = false;
            return VSConstants.S_OK;
        }

        public virtual int OnBeforeLoadProjectBatch(bool fIsBackgroundIdleBatch) => VSConstants.S_OK;

        public virtual int OnAfterLoadProjectBatch(bool fIsBackgroundIdleBatch) => VSConstants.S_OK;

        public virtual int OnAfterBackgroundSolutionLoadComplete() => VSConstants.S_OK;

        #endregion IVsSolutionLoadEvents

    }
}
