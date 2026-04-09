// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.CodeAnalysis.Razor.NestedFiles;
using Microsoft.CodeAnalysis.Razor.Protocol.NestedFiles;
using Microsoft.VisualStudio.Razor.LanguageClient;
using Microsoft.VisualStudio.Razor.ProjectSystem;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.VisualStudio.RazorExtension.NestedFiles;

/// <summary>
/// Base class for handling Add/View nested file commands for Razor documents.
/// When the file doesn't exist, sends an LSP request to the Razor language server
/// to create it via workspace/applyEdit. When the file exists, just opens it.
/// </summary>
internal sealed class NestedFileCommandHandler(
    IServiceProvider serviceProvider,
    string fileExtension,
    NestedFileKind fileKind,
    Lazy<LSPRequestInvokerWrapper> requestInvoker)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly string _fileExtension = fileExtension;
    private readonly NestedFileKind _fileKind = fileKind;
    private readonly Lazy<LSPRequestInvokerWrapper> _requestInvoker = requestInvoker;

    /// <summary>
    /// Configures the command status and text based on whether the nested file exists.
    /// </summary>
    public void OnBeforeQueryStatus(object sender, EventArgs e)
    {
        if (sender is not OleMenuCommand command)
        {
            return;
        }

        // Check if the Razor file context is active before doing expensive hierarchy queries
        if (!IsRazorFileUIContextActive()
            || GetSelectedRazorFilePath() is not string razorFilePath)
        {
            command.Visible = false;
            return;
        }

        var nestedFilePath = GetNestedFilePath(razorFilePath);
        var nestedFileExists = File.Exists(nestedFilePath);
        var nestedFileName = Path.GetFileName(nestedFilePath);

        command.Visible = true;
        command.Enabled = true;
        command.Text = nestedFileExists ? Resources.FormatView_Nested_File(nestedFileName) : Resources.FormatAdd_Nested_File(nestedFileName);
    }

    private bool IsRazorFileUIContextActive()
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        var contextGuid = RazorPackage.GuidRazorFileContext;

        return _serviceProvider.GetService(typeof(SVsShellMonitorSelection)) is IVsMonitorSelection monitorSelection
            && monitorSelection.GetCmdUIContextCookie(ref contextGuid, out var cookie) == VSConstants.S_OK
            && monitorSelection.IsCmdUIContextActive(cookie, out var isActive) == VSConstants.S_OK
            && isActive != 0;
    }

    /// <summary>
    /// Executes the command - either opens an existing nested file or creates a new one
    /// via the LSP server and then opens it.
    /// </summary>
    public void Execute(object sender, EventArgs e)
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        if (GetSelectedRazorFilePath() is not string razorFilePath)
        {
            return;
        }

        var nestedFilePath = GetNestedFilePath(razorFilePath);

        if (File.Exists(nestedFilePath))
        {
            // View: just open the existing file
            VsShellUtilities.OpenDocument(_serviceProvider, nestedFilePath);
        }
        else
        {
            // Add: send LSP request to create the file, then open it.
            // FileAndForget ensures exceptions are reported to telemetry rather than silently swallowed.
#pragma warning disable VSSDK007 // Fire-and-forget from synchronous EventHandler is intentional
            ThreadHelper.JoinableTaskFactory.RunAsync(
                () => CreateAndOpenNestedFileAsync(razorFilePath, nestedFilePath, CancellationToken.None)).FileAndForget("NestedFileCommandHandler.Execute");
#pragma warning restore VSSDK007
        }
    }

    private async Task CreateAndOpenNestedFileAsync(
        string razorFilePath,
        string nestedFilePath,
        CancellationToken cancellationToken)
    {
        // The cohost endpoint will create the file via workspace/applyEdit.
        // By the time this returns, the file should exist on disk.
        await _requestInvoker.Value.ReinvokeRequestOnServerAsync<AddNestedFileParams, object?>(
            RazorLSPConstants.AddNestedFileName,
            RazorLSPConstants.RoslynLanguageServerName,
            AddNestedFileParams.Create(new Uri(razorFilePath), _fileKind),
            cancellationToken);

        if (File.Exists(nestedFilePath))
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            // The workspace/applyEdit creates the file and inserts content via TextDocumentEdit,
            // which leaves the buffer dirty. Save it so the user sees a clean document.
            VsShellUtilities.SaveFileIfDirty(_serviceProvider, nestedFilePath);
            VsShellUtilities.OpenDocument(_serviceProvider, nestedFilePath);
        }
    }

    /// <summary>
    /// Gets the path to the nested file based on the Razor file path.
    /// </summary>
    private string GetNestedFilePath(string razorFilePath)
    {
        Debug.Assert(_fileExtension.StartsWith('.'));
        return razorFilePath + _fileExtension;
    }

    /// <summary>
    /// Gets the file path of the currently selected Razor file in Solution Explorer.
    /// </summary>
    private string? GetSelectedRazorFilePath()
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        if (_serviceProvider.GetService(typeof(SVsShellMonitorSelection)) is not IVsMonitorSelection monitorSelection)
        {
            return null;
        }

        monitorSelection.GetCurrentSelection(out var hierarchyPtr, out var itemId, out _, out var selectionContainerPtr);

        try
        {
            if (itemId is VSConstants.VSITEMID_NIL or VSConstants.VSITEMID_ROOT or VSConstants.VSITEMID_SELECTION)
            {
                return null;
            }

            if (hierarchyPtr != IntPtr.Zero)
            {
                var hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;
                if (hierarchy is IVsProject project)
                {
                    project.GetMkDocument(itemId, out var filePath);

                    if (filePath is not null)
                    {
                        if (filePath.EndsWith(FileKinds.ComponentFileExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            // return all paths with extension .razor except for the special imports file
                            var fileName = Path.GetFileNameWithoutExtension(filePath);
                            if (!string.Equals(fileName, ComponentHelpers.ImportsFileName, StringComparison.OrdinalIgnoreCase))
                            {
                                return filePath;
                            }
                        }
                        else if (filePath.EndsWith(FileKinds.LegacyFileExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            // return all paths with extension .cshtml except for the special imports file
                            var fileName = Path.GetFileNameWithoutExtension(filePath);
                            if (!string.Equals(fileName, MvcImportProjectFeature.ImportsFileName, StringComparison.OrdinalIgnoreCase))
                            {
                                return filePath;
                            }
                        }
                    }
                }
            }

            return null;
        }
        finally
        {
            if (hierarchyPtr != IntPtr.Zero)
            {
                Marshal.Release(hierarchyPtr);
            }

            if (selectionContainerPtr != IntPtr.Zero)
            {
                Marshal.Release(selectionContainerPtr);
            }
        }
    }
}
