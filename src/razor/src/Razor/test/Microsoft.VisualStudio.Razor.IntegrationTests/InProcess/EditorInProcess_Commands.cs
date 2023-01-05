// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using static Microsoft.VisualStudio.VSConstants;

namespace Microsoft.VisualStudio.Extensibility.Testing;

internal partial class EditorInProcess
{
    public async Task InvokeFormatDocumentAsync(CancellationToken cancellationToken)
    {
        var commandGuid = typeof(VSStd2KCmdID).GUID;
        var commandId = VSStd2KCmdID.FORMATDOCUMENT;
        await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
    }

    public async Task InvokeGoToDefinitionAsync(CancellationToken cancellationToken)
    {
        var commandGuid = typeof(VSStd97CmdID).GUID;
        var commandId = VSStd97CmdID.GotoDefn;
        await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
    }

    public async Task InvokeFindAllReferencesAsync(CancellationToken cancellationToken)
    {
        var commandGuid = typeof(VSStd97CmdID).GUID;
        var commandId = VSStd97CmdID.FindReferences;
        await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
    }

    public async Task InvokeGoToImplementationAsync(CancellationToken cancellationToken)
    {
        var commandGuid = typeof(VSStd97CmdID).GUID;
        var commandId = VSStd97CmdID.GotoDecl;
        await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
    }

    public async Task InvokeRenameAsync(CancellationToken cancellationToken)
    {
        var commandGuid = typeof(VSStd2KCmdID).GUID;
        var commandId = VSStd2KCmdID.RENAME;
        await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
    }

    public async Task CloseDocumentWindowAsync(CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var monitorSelection = await GetRequiredGlobalServiceAsync<SVsShellMonitorSelection, IVsMonitorSelection>(cancellationToken);
        ErrorHandler.ThrowOnFailure(monitorSelection.GetCurrentElementValue((uint)VSSELELEMID.SEID_WindowFrame, out var windowFrameObj));
        var windowFrame = (IVsWindowFrame)windowFrameObj;

        ErrorHandler.ThrowOnFailure(windowFrame.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_NoSave));
    }

    private async Task ExecuteCommandAsync(Guid commandGuid, uint commandId, CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var dispatcher = await GetRequiredGlobalServiceAsync<SUIHostCommandDispatcher, IOleCommandTarget>(cancellationToken);
        ErrorHandler.ThrowOnFailure(dispatcher.Exec(commandGuid, commandId, (uint)OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, IntPtr.Zero, IntPtr.Zero));
    }
}
