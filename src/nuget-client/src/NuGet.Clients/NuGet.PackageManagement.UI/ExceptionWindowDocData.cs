// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Minimal document data for the <see cref="ExceptionWindowPane"/>. Although the CreateDocumentWindow
    /// documentation states the doc data can be null, Visual Studio actually rejects a null doc data at
    /// runtime ("A document must be initialized with a docdata object"), so a real object is required. The
    /// exception window is a read-only, transient view, so this adapter simply satisfies the
    /// <see cref="IVsPersistDocData"/> contract without supporting saving or reloading.
    /// </summary>
    public sealed class ExceptionWindowDocData : IVsPersistDocData
    {
        private readonly Guid _editorFactoryGuid;

        public ExceptionWindowDocData(Guid editorFactoryGuid)
        {
            _editorFactoryGuid = editorFactoryGuid;
        }

        public int Close()
        {
            return VSConstants.S_OK;
        }

        public int GetGuidEditorType(out Guid pClassID)
        {
            pClassID = _editorFactoryGuid;
            return VSConstants.S_OK;
        }

        public int IsDocDataDirty(out int pfDirty)
        {
            pfDirty = 0;
            return VSConstants.S_OK;
        }

        public int IsDocDataReloadable(out int pfReloadable)
        {
            pfReloadable = 0;
            return VSConstants.S_OK;
        }

        public int LoadDocData(string pszMkDocument)
        {
            return VSConstants.S_OK;
        }

        public int OnRegisterDocData(uint docCookie, IVsHierarchy pHierNew, uint itemidNew)
        {
            return VSConstants.S_OK;
        }

        public int ReloadDocData(uint grfFlags)
        {
            return VSConstants.S_OK;
        }

        public int RenameDocData(uint grfAttribs, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.E_NOTIMPL;
        }

        public int SaveDocData(VSSAVEFLAGS dwSave, out string pbstrMkDocumentNew, out int pfSaveCanceled)
        {
            pbstrMkDocumentNew = null!;
            pfSaveCanceled = 0;
            return VSConstants.S_OK;
        }

        public int SetUntitledDocPath(string pszDocDataPath)
        {
            return VSConstants.E_NOTIMPL;
        }
    }
}
