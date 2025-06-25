// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Razor;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Xunit;

namespace Microsoft.VisualStudio.Extensibility.Testing;

internal partial class ShellInProcess
{
    public async Task<string> GetActiveDocumentFileNameAsync(CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var monitorSelection = await GetRequiredGlobalServiceAsync<SVsShellMonitorSelection, IVsMonitorSelection>(cancellationToken);
        ErrorHandler.ThrowOnFailure(monitorSelection.GetCurrentElementValue((uint)VSConstants.VSSELELEMID.SEID_WindowFrame, out var windowFrameObj));
        var windowFrame = (IVsWindowFrame)windowFrameObj;

        ErrorHandler.ThrowOnFailure(windowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_pszMkDocument, out var documentPathObj));
        var documentPath = (string)documentPathObj;
        return Path.GetFileName(documentPath);
    }

    public async Task SetInsertSpacesAsync(CancellationToken cancellationToken)
    {
        var textManager = await GetRequiredGlobalServiceAsync<SVsTextManager, IVsTextManager4>(cancellationToken);

        var langPrefs3 = new LANGPREFERENCES3[] { new LANGPREFERENCES3() { guidLang = RazorConstants.RazorLanguageServiceGuid } };
        Assert.Equal(VSConstants.S_OK, textManager.GetUserPreferences4(null, langPrefs3, null));

        langPrefs3[0].fInsertTabs = 0;

        Assert.Equal(VSConstants.S_OK, textManager.SetUserPreferences4(null, langPrefs3, null));
    }
}
