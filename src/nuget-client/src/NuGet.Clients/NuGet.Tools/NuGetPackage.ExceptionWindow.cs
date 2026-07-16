// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.PackageManagement.UI;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using Resx = NuGet.PackageManagement.UI.Resources;

namespace NuGetVSExtension
{
    public sealed partial class NuGetPackage
    {
        // Editor type GUID used only to open the Package Manager error window. Because the doc view and
        // doc data are supplied directly to CreateDocumentWindow, no editor factory is registered or
        // invoked for this GUID; it merely needs to be a stable, non-empty value.
        private static readonly Guid ExceptionWindowEditorType = new Guid("5f6b9c3d-4a2e-4b1c-9d8f-2a7e6c5b4a30");

        /// <summary>
        /// Handles a failure to open the Package Manager UI (project or solution level). Posts a fault
        /// telemetry event and, unless the failure was a cancellation, shows the exception details to the
        /// user in a document window.
        /// </summary>
        private async Task OnPackageManagerOpenFailureAsync(Exception exception, string callerMemberName)
        {
            // Cancellations are expected for user-initiated actions and shouldn't surface an error window.
            if (exception is OperationCanceledException)
            {
                return;
            }

            await TelemetryUtility.PostFaultAsync(exception, nameof(NuGetPackage), callerMemberName);

            try
            {
                await ShowExceptionWindowAsync(exception);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception showException)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // Showing the error window itself failed. Make sure we don't lose the original failure.
                await TelemetryUtility.PostFaultAsync(showException, nameof(NuGetPackage), nameof(ShowExceptionWindowAsync));
                ExceptionHelper.WriteErrorToActivityLog(showException);
            }
        }

        /// <summary>
        /// Opens a document window that displays the message, stack trace, and inner exceptions of the
        /// given exception.
        /// </summary>
        private async Task ShowExceptionWindowAsync(Exception exception)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var uiShell = await this.GetServiceAsync<SVsUIShell, IVsUIShell>();
            Assumes.Present(uiShell);
            var solution = await this.GetServiceAsync<SVsSolution, IVsSolution>();
            Assumes.Present(solution);

            var windowPane = new ExceptionWindowPane(exception);
            // Although CreateDocumentWindow documents the doc data as nullable, VS rejects a null doc data
            // at runtime, so we always supply one.
            var docData = new ExceptionWindowDocData(Guid.Empty);
            // A non-empty editor type GUID is required; because we supply the doc view directly, VS never
            // invokes an editor factory, so this value only needs to be unique.
            var guidEditorType = ExceptionWindowEditorType;
            var guidCommandUI = Guid.Empty;
            var caption = Resx.Text_ErrorOccurred;

            // Use a unique document moniker so each failure opens its own window instead of reusing a
            // previously opened error window.
            var documentName = string.Format(CultureInfo.InvariantCulture, "{0} {1:N}", caption, Guid.NewGuid());

            var windowFlags =
                (uint)_VSRDTFLAGS.RDT_DontAddToMRU |
                (uint)_VSRDTFLAGS.RDT_DontSaveAs |
                (uint)_VSRDTFLAGS.RDT_CantSave;

            var ppunkDocView = IntPtr.Zero;
            var ppunkDocData = IntPtr.Zero;
            IVsWindowFrame? windowFrame = null;
            var hr = 0;

            try
            {
                ppunkDocView = Marshal.GetIUnknownForObject(windowPane);
                ppunkDocData = Marshal.GetIUnknownForObject(docData);
                hr = uiShell.CreateDocumentWindow(
                    windowFlags,
                    documentName,
                    (IVsUIHierarchy)solution,
                    (uint)VSConstants.VSITEMID.Root,
                    ppunkDocView,
                    ppunkDocData,
                    ref guidEditorType,
                    null,
                    ref guidCommandUI,
                    null,
                    caption,
                    string.Empty,
                    null,
                    out windowFrame);

                if (windowFrame != null)
                {
                    WindowFrameHelper.DisableWindowAutoReopen(windowFrame);
                }
            }
            finally
            {
                if (ppunkDocView != IntPtr.Zero)
                {
                    Marshal.Release(ppunkDocView);
                }

                if (ppunkDocData != IntPtr.Zero)
                {
                    Marshal.Release(ppunkDocData);
                }
            }

            if (ErrorHandler.Failed(hr))
            {
                // VS did not take ownership of the pane on failure, so dispose the object we created.
                windowPane.Dispose();
                ErrorHandler.ThrowOnFailure(hr);
            }

            windowFrame?.Show();
        }
    }
}
