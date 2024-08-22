// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.PackageManagement.UI
{
    public class PackageManagerWindowPane : WindowPane, IVsWindowFrameNotify3
    {
        private PackageManagerControl _content;

        /// <summary>
        /// Initializes a new instance of the EditorPane class.
        /// </summary>
        public PackageManagerWindowPane(PackageManagerControl control)
            : base(null)
        {
            _content = control;
        }

        public PackageManagerModel Model
        {
            get { return _content.Model; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// IVsWindowPane
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override object Content
        {
            get { return _content; }
        }

        private void CleanUp()
        {
            if (_content != null)
            {
                _content.Dispose();
                _content = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    CleanUp();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public int OnClose(ref uint pgrfSaveOptions)
        {
            _content.SaveSettings();
            _content.Model.Context.UserSettingsManager.PersistSettings();

            pgrfSaveOptions = (uint)__FRAMECLOSE.FRAMECLOSE_NoSave;

            return VSConstants.S_OK;
        }

        public int OnDockableChange(int fDockable, int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        public int OnMove(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        public int OnShow(int fShow)
        {
            return VSConstants.S_OK;
        }

        public int OnSize(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }
    }
}
