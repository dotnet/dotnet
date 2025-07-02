﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ErrorReporting;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Shared.TestHooks;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.CodeAnalysis.Editor.Implementation.LanguageClient;

// unfortunately, we can't implement this on LanguageServerClient since this uses MEF v2 and
// ILanguageClient requires MEF v1 and two can't be mixed exported in 1 class.
[Export]
[ExportEventListener(WellKnownEventListeners.Workspace, WorkspaceKind.Host), Shared]
[method: ImportingConstructor]
[method: Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
internal sealed class AlwaysActiveLanguageClientEventListener(
    AlwaysActivateInProcLanguageClient languageClient,
    Lazy<ILanguageClientBroker> languageClientBroker,
    IAsynchronousOperationListenerProvider listenerProvider) : IEventListener
{
    private readonly AlwaysActivateInProcLanguageClient _languageClient = languageClient;
    private readonly Lazy<ILanguageClientBroker> _languageClientBroker = languageClientBroker;

    private readonly IAsynchronousOperationListener _asynchronousOperationListener = listenerProvider.GetListener(FeatureAttribute.LanguageServer);

    /// <summary>
    /// LSP clients do not necessarily know which language servers (and when) to activate as they are language
    /// agnostic.  We know we can provide <see cref="AlwaysActivateInProcLanguageClient"/> as soon as the
    /// workspace is started, so tell the <see cref="ILanguageClientBroker"/> to start loading it.
    /// </summary>
    public void StartListening(Workspace workspace)
    {
        _ = workspace.RegisterWorkspaceChangedHandler(Workspace_WorkspaceChanged);
    }

    private void Workspace_WorkspaceChanged(WorkspaceChangeEventArgs e)
    {
        if (e.Kind == WorkspaceChangeKind.SolutionAdded)
        {
            // Normally VS will load the language client when an editor window is created for one of our content types,
            // but we want to load it as soon as a solution is loaded so workspace diagnostics work, and so 3rd parties
            // like Razor can use dynamic registration.
            Load();
        }
    }

    public void StopListening(Workspace workspace)
    {
        // Nothing to do here.  There's no concept of unloading an ILanguageClient.
    }

    private void Load()
    {
        using var token = _asynchronousOperationListener.BeginAsyncOperation(nameof(Load));
        LoadAsync().ReportNonFatalErrorAsync().CompletesAsyncOperation(token);

        async Task LoadAsync()
        {
            // Explicitly switch to the bg so that if this causes any expensive work (like mef loads) it 
            // doesn't block the UI thread. Note, we always yield because sometimes our caller starts
            // on the threadpool thread but is indirectly blocked on by the UI thread.
            await TaskScheduler.Default.SwitchTo(alwaysYield: true);

            // Sometimes the editor can be slow to stop the old server instance when the old solution is closed, so we force it here.
            // This will no-op if the server hasn't been started yet.
            await _languageClient.StopServerAsync().ConfigureAwait(false);

            await _languageClientBroker.Value.LoadAsync(new LanguageClientMetadata(
            [
                ContentTypeNames.CSharpContentType,
                ContentTypeNames.VisualBasicContentType,
                ContentTypeNames.FSharpContentType
            ]), _languageClient).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// The <see cref="ILanguageClientBroker.LoadAsync(ILanguageClientMetadata, ILanguageClient)"/> 
    /// requires that we pass the <see cref="ILanguageClientMetadata"/> along with the language client instance.
    /// The implementation of <see cref="ILanguageClientMetadata"/> is not public, so have to re-implement.
    /// https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1043922 tracking to remove this.
    /// </summary>
    private sealed class LanguageClientMetadata(string[] contentTypes, string clientName = null) : ILanguageClientMetadata
    {
        public string ClientName { get; } = clientName;

        public IEnumerable<string> ContentTypes { get; } = contentTypes;
    }
}
