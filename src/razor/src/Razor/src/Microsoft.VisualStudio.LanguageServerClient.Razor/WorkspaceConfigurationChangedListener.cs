// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.VisualStudio.Editor.Razor.Settings;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(IClientSettingsChangedTrigger))]
[method: ImportingConstructor]
internal class WorkspaceConfigurationChangedListener(LSPRequestInvoker requestInvoker) : IClientSettingsChangedTrigger
{
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;

    public void Initialize(IClientSettingsManager editorSettingsManager)
    {
        editorSettingsManager.ClientSettingsChanged += OnClientSettingsChanged;
    }

    private void OnClientSettingsChanged(object sender, ClientSettingsChangedEventArgs args)
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
            CheckRazorServerCapability,
            new DidChangeConfigurationParams(),
            CancellationToken.None);
    }

    private static bool CheckRazorServerCapability(JToken token)
    {
        // We're talking cross-language servers here. Given the workspace/didChangeConfiguration is a normal LSP message this will only fail
        // if the Razor language server is not running. Typically this would be OK from a platform perspective; however VS will explode if
        // there's not a corresponding language server to accept the message. To protect ourselves from this scenario we can utilize capabilities
        // and just lookup generic Razor language server specific capabilities. If they exist we can succeed.
        var isRazorLanguageServer = RazorLanguageServerCapability.TryGet(token, out _);
        return isRazorLanguageServer;
    }

    private class Unit
    {
    }
}
