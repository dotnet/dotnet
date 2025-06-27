// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Razor.Settings;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(IClientSettingsChangedTrigger))]
[method: ImportingConstructor]
internal class WorkspaceConfigurationChangedListener(LSPRequestInvoker requestInvoker) : IClientSettingsChangedTrigger
{
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;

    public void Initialize(IClientSettingsManager editorSettingsManager)
    {
        editorSettingsManager.ClientSettingsChanged += OnClientSettingsChanged;
    }

    private void OnClientSettingsChanged(object sender, EventArgs args)
    {
        _ = OnClientSettingsChangedAsync();
    }

    private async Task OnClientSettingsChangedAsync()
    {
        // Make sure the server updates the settings on their side by sending a
        // workspace/didChangeConfiguration request. This notifies the server that the user's
        // settings have changed.
        //
        // NOTE: This flow uses polyfilling because VS doesn't yet support workspace configuration
        // updates. Once they do, we can get rid of this extra logic.
        await _requestInvoker.ReinvokeRequestOnServerAsync<DidChangeConfigurationParams, Unit>(
            Methods.WorkspaceDidChangeConfigurationName,
            RazorLSPConstants.RazorLanguageServerName,
            new DidChangeConfigurationParams(),
            CancellationToken.None);
    }

    private class Unit
    {
    }
}
