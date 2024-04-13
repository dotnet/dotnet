// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(IRazorCohostTextDocumentSyncHandler))]
[method: ImportingConstructor]
internal class CohostTextDocumentSyncHandler(
    IRemoteClientProvider remoteClientProvider,
    LSPDocumentManager documentManager,
    JoinableTaskContext joinableTaskContext,
    ILoggerFactory loggerFactory) : IRazorCohostTextDocumentSyncHandler
{
    private readonly IRemoteClientProvider _remoteClientProvider = remoteClientProvider;
    private readonly JoinableTaskContext _joinableTaskContext = joinableTaskContext;
    private readonly TrackingLSPDocumentManager _documentManager = documentManager as TrackingLSPDocumentManager ?? throw new InvalidOperationException("Expected TrackingLSPDocumentManager");
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostTextDocumentSyncHandler>();

    public async Task HandleAsync(int version, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        // For didClose, we don't need to do anything. We can't close the virtual document, because that requires the buffer
        // so we just no-op and let our VS components handle closure.
        if (context.Method == Methods.TextDocumentDidCloseName)
        {
            return;
        }

        var textDocument = context.TextDocument.AssumeNotNull();
        var textDocumentPath = context.TextDocument.FilePath.AssumeNotNull();

        _logger.LogDebug($"[Cohost] {context.Method} of '{textDocumentPath}' with version {version}.");   

        var client = await _remoteClientProvider.TryGetClientAsync(cancellationToken);
        if (client is null)
        {
            _logger.LogError($"[Cohost] Couldn't get remote client for {context.Method} of '{textDocumentPath}'. Html document contents will be stale");
            return;
        }

        var htmlText = await client.TryInvokeAsync<IRemoteHtmlDocumentService, string?>(textDocument.Project.Solution,
            (service, solutionInfo, ct) => service.GetHtmlDocumentTextAsync(solutionInfo, textDocument.Id, ct),
            cancellationToken).ConfigureAwait(false);

        if (!htmlText.HasValue || htmlText.Value is null)
        {
            _logger.LogError($"[Cohost] Couldn't get Html text for {context.Method} of '{textDocumentPath}'. Html document contents will be stale");
            return;
        }

        // Eventually, for VS Code, the following piece of logic needs to make an LSP call rather than directly update the buffer

        // Roslyn might have got changes from the LSP server, but we just get the actual source text, so we just construct one giant change
        // from that. Guaranteed no sync issues, though we are passing long strings around unfortunately.
        var uri = textDocument.CreateUri();
        if (!_documentManager.TryGetDocument(uri, out var documentSnapshot) ||
            !documentSnapshot.TryGetVirtualDocument<HtmlVirtualDocumentSnapshot>(out var htmlDocument))
        {
            Debug.Fail("Got an LSP text document update before getting informed of the VS buffer");
            _logger.LogError($"[Cohost] Couldn't get an Html document for {context.Method} of '{textDocumentPath}'. Html document contents will be stale (or non-existent?)");
            return;
        }

        await _joinableTaskContext.Factory.SwitchToMainThreadAsync(cancellationToken);

        VisualStudioTextChange[] changes = [new(0, htmlDocument.Snapshot.Length, htmlText.Value)];
        _documentManager.UpdateVirtualDocument<HtmlVirtualDocument>(uri, changes, version, state: null);

        _logger.LogDebug($"[Cohost] Exiting {context.Method} of '{textDocumentPath}' with version {version}.");
    }
}
