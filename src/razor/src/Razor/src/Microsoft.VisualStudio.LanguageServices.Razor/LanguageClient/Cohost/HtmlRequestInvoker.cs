// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(IHtmlRequestInvoker))]
[method: ImportingConstructor]
internal sealed class HtmlRequestInvoker(
    LSPRequestInvoker requestInvoker,
    LSPDocumentManager documentManager,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    ITelemetryReporter telemetryReporter,
    ILoggerFactory loggerFactory) : IHtmlRequestInvoker
{
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly TrackingLSPDocumentManager _documentManager = documentManager as TrackingLSPDocumentManager ?? throw new InvalidOperationException("Expected TrackingLSPDocumentManager");
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<HtmlRequestInvoker>();

    public async Task<TResponse?> MakeHtmlLspRequestAsync<TRequest, TResponse>(TextDocument razorDocument, string method, TRequest request, TimeSpan threshold, Guid correlationId, CancellationToken cancellationToken) where TRequest : notnull
    {
        var syncResult = await _htmlDocumentSynchronizer.TrySynchronizeAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (!syncResult.Synchronized)
        {
            _logger.LogDebug($"Couldn't synchronize for {razorDocument.FilePath}");
            return default;
        }

        if (!_documentManager.TryGetDocument(razorDocument.CreateUri(), out var snapshot))
        {
            _logger.LogError($"Couldn't find document in LSPDocumentManager for {razorDocument.FilePath}");
            return default;
        }

        if (!snapshot.TryGetVirtualDocument<HtmlVirtualDocumentSnapshot>(out var htmlDocument))
        {
            _logger.LogError($"Couldn't find virtual document snapshot for {snapshot.Uri}");
            return default;
        }

        var existingChecksum = (ChecksumWrapper)htmlDocument.State.AssumeNotNull();
        if (!existingChecksum.Equals(syncResult.Checksum))
        {
            _logger.LogError($"Checksum for {snapshot.Uri}, {htmlDocument.State} doesn't match {syncResult.Checksum}.");
            return default;
        }

        // If the request is for a text document, we need to update the Uri to point to the Html document,
        // and most importantly set it back again before leaving the method in case a caller uses it.
        Uri? originalUri = null;
        var textDocumentRequest = request as ITextDocumentParams;
        if (textDocumentRequest is not null)
        {
            originalUri = textDocumentRequest.TextDocument.Uri;
            textDocumentRequest.TextDocument.Uri = htmlDocument.Uri;
        }

        try
        {
            _logger.LogDebug($"Making LSP request for {method} from {htmlDocument.Uri}{(request is ITextDocumentPositionParams positionParams ? $" at {positionParams.Position}" : "")}, checksum {syncResult.Checksum}.");

            // Passing Guid.Empty to this method will mean no tracking
            using var _ = _telemetryReporter.TrackLspRequest(Methods.TextDocumentCodeActionName, RazorLSPConstants.HtmlLanguageServerName, threshold, correlationId);

            var result = await _requestInvoker.ReinvokeRequestOnServerAsync<TRequest, TResponse?>(
                htmlDocument.Snapshot.TextBuffer,
                method,
                RazorLSPConstants.HtmlLanguageServerName,
                request,
                cancellationToken).ConfigureAwait(false);

            if (result is null)
            {
                return default;
            }

            return result.Response;
        }
        finally
        {
            Debug.Assert(
                (textDocumentRequest is null && originalUri is null) ||
                (textDocumentRequest is not null && originalUri is not null), "textDocumentRequest and originalUri should either both be null or both be non-null");

            // Reset the Uri if we changed it.
            if (textDocumentRequest is not null && originalUri is not null)
            {
                textDocumentRequest.TextDocument.Uri = originalUri;
            }
        }
    }
}
