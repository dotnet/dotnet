// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[Shared]
[ExportLspMethod(Methods.TextDocumentDefinitionName)]
internal class GoToDefinitionHandler : IRequestHandler<TextDocumentPositionParams, Location[]>
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly LSPDocumentManager _documentManager;
    private readonly LSPProjectionProvider _projectionProvider;
    private readonly LSPDocumentMappingProvider _documentMappingProvider;
    private readonly ILogger _logger;

    [ImportingConstructor]
    public GoToDefinitionHandler(
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

        _logger = loggerProvider.CreateLogger(nameof(GoToDefinitionHandler));
    }

    public async Task<Location[]?> HandleRequestAsync(TextDocumentPositionParams request, ClientCapabilities clientCapabilities, CancellationToken cancellationToken)
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
            _logger.LogWarning("Projection result was null");
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var textDocumentPositionParams = new TextDocumentPositionParams()
        {
            Position = projectionResult.Position,
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = projectionResult.Uri
            }
        };

        _logger.LogInformation("Requesting GoToDef for {projectionResultUri}.", projectionResult.Uri);

        var serverKind = projectionResult.LanguageKind.ToLanguageServerKind();
        var languageServerName = serverKind.ToLanguageServerName();
        var textBuffer = serverKind.GetTextBuffer(documentSnapshot);
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<TextDocumentPositionParams, Location[]>(
            textBuffer,
            Methods.TextDocumentDefinitionName,
            languageServerName,
            textDocumentPositionParams,
            cancellationToken).ConfigureAwait(false);

        if (!ReinvocationResponseHelper.TryExtractResultOrLog(response, _logger, languageServerName, out var locations))
        {
            return null;
        }

        if (locations.Length == 0)
        {
            _logger.LogInformation("Received no results.");
            return locations;
        }

        _logger.LogInformation("Received {locationsLength} results, remapping.", locations.Length);

        cancellationToken.ThrowIfCancellationRequested();

        var remappedLocations = await _documentMappingProvider.RemapLocationsAsync(locations, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Returning {remappedLocationsLength} definitions.", remappedLocations?.Length);
        return remappedLocations;
    }
}
