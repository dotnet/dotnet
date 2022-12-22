// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace NuGet.VisualStudio.SolutionExplorer
{
    [Export(typeof(FileOpener))]
    internal sealed class FileOpener
    {
        private readonly IServiceProvider _serviceProvider;

        [ImportingConstructor]
        public FileOpener([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OpenFile(string filePath, bool isReadOnly)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var rdt = new RunningDocumentTable(_serviceProvider);

            IVsWindowFrame? windowFrame = null;
            try
            {
                // Open the document.
                VsShellUtilities.OpenDocument(
                    _serviceProvider,
                    filePath,
                    VSConstants.LOGVIEWID_Primary,
                    out _,
                    out _,
                    out windowFrame);

                // Set it as read only if necessary.
                if (isReadOnly)
                {
                    RunningDocumentInfo rdtInfo = rdt.GetDocumentInfo(filePath);

                    // Set it as read only if necessary.
                    if (rdtInfo.DocData is IVsTextBuffer textBuffer)
                    {
                        textBuffer.GetStateFlags(out uint flags);
                        textBuffer.SetStateFlags(flags | (uint)BUFFERSTATEFLAGS.BSF_USER_READONLY);
                    }
                }

                // Show the document window
                if (windowFrame != null)
                {
                    ErrorHandler.ThrowOnFailure(windowFrame.Show());
                }
            }
            catch (Exception ex) when (!ex.IsCritical())
            {
                windowFrame?.CloseFrame(0);
            }
        }
    }
}
