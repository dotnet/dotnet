// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[Shared]
[ExportLspMethod(Methods.TextDocumentDocumentHighlightName)]
internal class DocumentHighlightHandler : IRequestHandler<DocumentHighlightParams, DocumentHighlight[]>
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly LSPDocumentManager _documentManager;
    private readonly LSPProjectionProvider _projectionProvider;
    private readonly LSPDocumentMappingProvider _documentMappingProvider;
    private readonly ILogger _logger;

    [ImportingConstructor]
    public DocumentHighlightHandler(
        LSPRequestInvoker requestInvoker,
        LSPDocumentManager documentManager,
        LSPProjectionProvider projectionProvider,
        LSPDocumentMappingProvider documentMappingProvider,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider)
    {
        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (projectionProvider is null)
        {
            throw new ArgumentNullException(nameof(projectionProvider));
        }

        if (documentMappingProvider is null)
        {
            throw new ArgumentNullException(nameof(documentMappingProvider));
        }

        if (loggerProvider is null)
        {
            throw new ArgumentNullException(nameof(loggerProvider));
        }

        _requestInvoker = requestInvoker;
        _documentManager = documentManager;
        _projectionProvider = projectionProvider;
        _documentMappingProvider = documentMappingProvider;

        _logger = loggerProvider.CreateLogger(nameof(DocumentHighlightHandler));
    }

    public async Task<DocumentHighlight[]?> HandleRequestAsync(DocumentHighlightParams request, ClientCapabilities clientCapabilities, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (clientCapabilities is null)
        {
            throw new ArgumentNullException(nameof(clientCapabilities));
        }

        _logger.LogInformation("Starting request for {textDocumentUri}.", request.TextDocument.Uri);

        if (!_documentManager.TryGetDocument(request.TextDocument.Uri, out var documentSnapshot))
        {
            _logger.LogWarning("Failed to find document {textDocumentUri}.", request.TextDocument.Uri);
            return null;
        }

        var projectionResult = await _projectionProvider.GetProjectionAsync(
            documentSnapshot,
            request.Position,
            cancellationToken).ConfigureAwait(false);
        if (projectionResult is null)
        {
            return null;
        }

        var serverKind = projectionResult.LanguageKind == RazorLanguageKind.CSharp ? LanguageServerKind.CSharp : LanguageServerKind.Html;

        cancellationToken.ThrowIfCancellationRequested();

        var documentHighlightParams = new DocumentHighlightParams()
        {
            Position = projectionResult.Position,
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = projectionResult.Uri
            }
        };

        _logger.LogInformation("Requesting highlights for {projectionUri} at ({projectionLine}, {projectionCharacter}).",
            projectionResult.Uri, projectionResult.Position?.Line, projectionResult.Position?.Character);

        var languageServerName = serverKind.ToLanguageServerName();
        var textBuffer = serverKind.GetTextBuffer(documentSnapshot);
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentHighlightParams, DocumentHighlight[]>(
            textBuffer,
            Methods.TextDocumentDocumentHighlightName,
            languageServerName,
            documentHighlightParams,
            cancellationToken).ConfigureAwait(false);

        if (!ReinvocationResponseHelper.TryExtractResultOrLog(response, _logger, languageServerName, out var highlights))
        {
            return null;
        }

        if (highlights.Length == 0)
        {
            _logger.LogInformation("Received no results.");
            return highlights;
        }

        _logger.LogInformation("Received {length} results, remapping.", highlights.Length);

        var remappedHighlights = new List<DocumentHighlight>();

        var rangesToMap = highlights.Select(r => r.Range).ToArray();
        var mappingResult = await _documentMappingProvider.MapToDocumentRangesAsync(
            projectionResult.LanguageKind,
            request.TextDocument.Uri,
            rangesToMap,
            cancellationToken).ConfigureAwait(false);

        if (mappingResult?.HostDocumentVersion != documentSnapshot.Version)
        {
            // Couldn't remap the range or the document changed in the meantime. Discard this highlight.
            _logger.LogInformation("Mapping failed. Versions: {documentVersion} -> {hostDocumentVersion}.",
                documentSnapshot.Version, mappingResult?.HostDocumentVersion);
            return Array.Empty<DocumentHighlight>();
        }

        for (var i = 0; i < highlights.Length; i++)
        {
            var highlight = highlights[i];
            var range = mappingResult.Ranges[i];
            if (range.IsUndefined())
            {
                // Couldn't remap the range correctly. Discard this range.
                continue;
            }

            var remappedHighlight = new DocumentHighlight()
            {
                Range = range,
                Kind = highlight.Kind
            };

            remappedHighlights.Add(remappedHighlight);
        }

        _logger.LogInformation("Returning {remappedHighlightsCount} highlights.", remappedHighlights.Count);
        return remappedHighlights.ToArray();
    }
}
