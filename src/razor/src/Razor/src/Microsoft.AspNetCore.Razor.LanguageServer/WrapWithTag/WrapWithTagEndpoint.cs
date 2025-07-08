// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;

[RazorLanguageServerEndpoint(LanguageServerConstants.RazorWrapWithTagEndpoint)]
internal class WrapWithTagEndpoint(IClientConnection clientConnection, ILoggerFactory loggerFactory) : IRazorRequestHandler<WrapWithTagParams, WrapWithTagResponse?>
{
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<WrapWithTagEndpoint>();

    public bool MutatesSolutionState => false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(WrapWithTagParams request)
    {
        return request.TextDocument;
    }

    public async Task<WrapWithTagResponse?> HandleRequestAsync(WrapWithTagParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            _logger.LogWarning($"Failed to find document {request.TextDocument.DocumentUri}.");
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (request.Range is null)
        {
            _logger.LogInformation($"WrapWithTag request for {request.TextDocument.DocumentUri} has a null range.");
            return null;
        }

        if (!WrapWithTagHelper.TryGetValidWrappingRange(codeDocument, request.Range.ToLinePositionSpan(), out var adjustedRange))
        {
            _logger.LogInformation($"Unsupported language at the requested range.");
            return null;
        }

        // Update the request range if it was adjusted
        request.Range = adjustedRange.ToRange();

        cancellationToken.ThrowIfCancellationRequested();

        var versioned = new VersionedTextDocumentIdentifier
        {
            DocumentUri = request.TextDocument.DocumentUri,
            Version = documentContext.Snapshot.Version,
        };
        var parameter = new DelegatedWrapWithTagParams(versioned, request);

        var htmlResponse = await _clientConnection.SendRequestAsync<DelegatedWrapWithTagParams, WrapWithTagResponse>(
            LanguageServerConstants.RazorWrapWithTagEndpoint,
            parameter,
            cancellationToken).ConfigureAwait(false);

        if (htmlResponse.TextEdits is not null)
        {
            var htmlSourceText = await documentContext.GetHtmlSourceTextAsync(cancellationToken).ConfigureAwait(false);
            htmlResponse.TextEdits = FormattingUtilities.FixHtmlTextEdits(htmlSourceText, htmlResponse.TextEdits);
        }

        return htmlResponse;
    }
}
