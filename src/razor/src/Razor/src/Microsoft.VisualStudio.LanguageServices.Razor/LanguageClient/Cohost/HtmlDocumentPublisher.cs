// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(IHtmlDocumentPublisher))]
[method: ImportingConstructor]
internal sealed class HtmlDocumentPublisher(
    IRemoteServiceProvider remoteServiceProvider,
    LSPDocumentManager documentManager,
    JoinableTaskContext joinableTaskContext,
    ILoggerFactory loggerFactory) : IHtmlDocumentPublisher
{
    private readonly IRemoteServiceProvider _remoteServiceProvider = remoteServiceProvider;
    private readonly JoinableTaskContext _joinableTaskContext = joinableTaskContext;
    private readonly TrackingLSPDocumentManager _documentManager = documentManager as TrackingLSPDocumentManager ?? throw new InvalidOperationException("Expected TrackingLSPDocumentManager");
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<HtmlDocumentPublisher>();

    public Task<string?> GetHtmlSourceFromOOPAsync(TextDocument document, CancellationToken cancellationToken)
    {
        return _remoteServiceProvider.TryInvokeAsync<IRemoteHtmlDocumentService, string?>(document.Project.Solution,
            (service, solutionInfo, ct) => service.GetHtmlDocumentTextAsync(solutionInfo, document.Id, ct),
            cancellationToken).AsTask();
    }

    public async Task PublishAsync(TextDocument document, string htmlText, CancellationToken cancellationToken)
    {
        // TODO: Eventually, for VS Code, the following piece of logic needs to make an LSP call rather than directly update the
        // buffer, but the assembly this code currently lives in doesn't ship in VS Code, so we need to solve a few other things
        // before we get there.

        var uri = document.CreateUri();
        if (!_documentManager.TryGetDocument(uri, out var documentSnapshot) ||
            !documentSnapshot.TryGetVirtualDocument<HtmlVirtualDocumentSnapshot>(out var htmlDocument))
        {
            Debug.Fail("Got an LSP text document update before getting informed of the VS buffer. Create on demand?");
            _logger.LogError($"Couldn't get Html text for {document.FilePath}. Html document contents will be stale");
            return;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        _logger.LogDebug($"The html document for {document.FilePath} is {uri}");

        await _joinableTaskContext.Factory.SwitchToMainThreadAsync(cancellationToken);

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        VisualStudioTextChange[] changes = [new(0, htmlDocument.Snapshot.Length, htmlText)];
        _documentManager.UpdateVirtualDocument<HtmlVirtualDocument>(uri, changes, documentSnapshot.Version, state: null);

        _logger.LogDebug($"Finished Html document generation for {document.FilePath} (into {uri})");
    }
}
