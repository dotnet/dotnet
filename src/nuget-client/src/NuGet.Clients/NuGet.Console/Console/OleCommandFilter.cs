// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;

namespace NuGetConsole.Implementation.Console
{
    internal class OleCommandFilter : IOleCommandTarget
    {
        public const int OLECMDERR_E_NOTSUPPORTED = (int)Constants.OLECMDERR_E_NOTSUPPORTED;
        public const int OLECMDERR_E_UNKNOWNGROUP = (int)Constants.OLECMDERR_E_UNKNOWNGROUP;

        protected IOleCommandTarget OldChain { get; private set; }

        public OleCommandFilter(IVsTextView vsTextView)
        {
            Debug.Assert(vsTextView != null);

            IOleCommandTarget _oldChain;
            ErrorHandler.ThrowOnFailure(vsTextView.AddCommandFilter(this, out _oldChain));
            Debug.Assert(_oldChain != null);

            OldChain = _oldChain;
        }

        protected virtual int InternalQueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return OLECMDERR_E_NOTSUPPORTED;
        }

        protected virtual int InternalExec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            return OLECMDERR_E_NOTSUPPORTED;
        }

        #region IOleCommandTarget

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            int hr = InternalQueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);

            if (hr == OLECMDERR_E_NOTSUPPORTED)
            {
                hr = OldChain.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
            }

            return hr;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            int hr = InternalExec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            if (hr == OLECMDERR_E_NOTSUPPORTED)
            {
                hr = OldChain.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            }

            return hr;
        }

        #endregion
    }
}
