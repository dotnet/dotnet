// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Razor.IntegrationTests.InProcess;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using static Microsoft.VisualStudio.VSConstants;

namespace Microsoft.VisualStudio.Extensibility.Testing;

internal partial class EditorInProcess
{
    public async Task InvokeDeleteLineAsync(CancellationToken cancellationToken)
    {
        var commandGuid = typeof(VSStd2KCmdID).GUID;
        var commandId = VSStd2KCmdID.DELETELINE;
        await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
    }

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

        // Rename seems to be extra-succeptable to COM exceptions
        await Helper.RetryAsync<bool?>(async (cancellationToken) =>
        {
            await ExecuteCommandAsync(commandGuid, (uint)commandId, cancellationToken);
            return true;
        }, TimeSpan.FromSeconds(1), cancellationToken);
    }

    public async Task CloseCodeFileAsync(string projectName, string relativeFilePath, bool saveFile, CancellationToken cancellationToken)
    {
        await CloseFileAsync(projectName, relativeFilePath, VSConstants.LOGVIEWID.Code_guid, saveFile, cancellationToken);
    }

    public async Task CloseCurrentlyFocusedWindowAsync(CancellationToken cancellationToken, bool save = false)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var monitorSelection = await GetRequiredGlobalServiceAsync<SVsShellMonitorSelection, IVsMonitorSelection>(cancellationToken);
        ErrorHandler.ThrowOnFailure(monitorSelection.GetCurrentElementValue((uint)VSSELELEMID.SEID_WindowFrame, out var windowFrameObj));
        var windowFrame = (IVsWindowFrame)windowFrameObj;

        var closeFlags = save
            ? __FRAMECLOSE.FRAMECLOSE_SaveIfDirty
            : __FRAMECLOSE.FRAMECLOSE_NoSave;
        ErrorHandler.ThrowOnFailure(windowFrame.CloseFrame((uint)closeFlags));
    }

    private async Task ExecuteCommandAsync(Guid commandGuid, uint commandId, CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var dispatcher = await GetRequiredGlobalServiceAsync<SUIHostCommandDispatcher, IOleCommandTarget>(cancellationToken);
        ErrorHandler.ThrowOnFailure(dispatcher.Exec(commandGuid, commandId, (uint)OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, IntPtr.Zero, IntPtr.Zero));
    }

    private async Task CloseFileAsync(string projectName, string relativeFilePath, Guid logicalView, bool saveFile, CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var filePath = await GetAbsolutePathForProjectRelativeFilePathAsync(projectName, relativeFilePath, cancellationToken);
        if (!VsShellUtilities.IsDocumentOpen(ServiceProvider.GlobalProvider, filePath, logicalView, out _, out _, out var windowFrame))
        {
            throw new InvalidOperationException($"File '{filePath}' is not open in logical view '{logicalView}'");
        }

        var frameClose = saveFile ? __FRAMECLOSE.FRAMECLOSE_SaveIfDirty : __FRAMECLOSE.FRAMECLOSE_NoSave;
        ErrorHandler.ThrowOnFailure(windowFrame.CloseFrame((uint)frameClose));
    }

    private async Task<string> GetAbsolutePathForProjectRelativeFilePathAsync(string projectName, string relativeFilePath, CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var dte = await GetRequiredGlobalServiceAsync<SDTE, EnvDTE.DTE>(cancellationToken);
        var solution = dte.Solution;
        Assumes.Present(solution);

        var project = solution.Projects.Cast<EnvDTE.Project>().First(x => x.Name == projectName);
        var projectPath = Path.GetDirectoryName(project.FullName);
        return Path.Combine(projectPath, relativeFilePath);
    }
}
