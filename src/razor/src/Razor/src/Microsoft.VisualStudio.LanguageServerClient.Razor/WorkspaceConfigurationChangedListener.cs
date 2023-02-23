// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Shared]
[Export(typeof(ClientSettingsChangedTrigger))]
internal class WorkspaceConfigurationChangedListener : ClientSettingsChangedTrigger
{
    private readonly LSPRequestInvoker _requestInvoker;

    [ImportingConstructor]
    public WorkspaceConfigurationChangedListener(LSPRequestInvoker requestInvoker)
    {
        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        _requestInvoker = requestInvoker;
    }

    public override void Initialize(IClientSettingsManager editorSettingsManager)
    {
        editorSettingsManager.ClientSettingsChanged += EditorSettingsManager_Changed;
    }

    private void EditorSettingsManager_Changed(object sender, ClientSettingsChangedEventArgs args)
    {
        _ = EditorSettingsManager_ChangedAsync();
    }

    private async Task EditorSettingsManager_ChangedAsync()
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
