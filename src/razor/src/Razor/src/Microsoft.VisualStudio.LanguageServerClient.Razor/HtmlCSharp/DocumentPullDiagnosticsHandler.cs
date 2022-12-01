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
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[Shared]
[ExportLspMethod(VSInternalMethods.DocumentPullDiagnosticName)]
internal class DocumentPullDiagnosticsHandler :
    IRequestHandler<VSInternalDocumentDiagnosticsParams, IReadOnlyList<VSInternalDiagnosticReport>>
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly LSPDocumentManager _documentManager;
    private readonly LSPDocumentSynchronizer _documentSynchronizer;
    private readonly LSPDiagnosticsTranslator _diagnosticsProvider;
    private readonly ILogger _logger;

    [ImportingConstructor]
    public DocumentPullDiagnosticsHandler(
        LSPRequestInvoker requestInvoker,
        LSPDocumentManager documentManager,
        LSPDocumentSynchronizer documentSynchronizer,
        LSPDiagnosticsTranslator diagnosticsProvider,
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

        if (documentSynchronizer is null)
        {
            throw new ArgumentNullException(nameof(documentSynchronizer));
        }

        if (diagnosticsProvider is null)
        {
            throw new ArgumentNullException(nameof(diagnosticsProvider));
        }

        if (loggerProvider is null)
        {
            throw new ArgumentNullException(nameof(loggerProvider));
        }

        _requestInvoker = requestInvoker;
        _documentManager = documentManager;
        _documentSynchronizer = documentSynchronizer;
        _diagnosticsProvider = diagnosticsProvider;

        _logger = loggerProvider.CreateLogger(nameof(DocumentPullDiagnosticsHandler));
    }

    // Internal for testing
    public async Task<IReadOnlyList<VSInternalDiagnosticReport>?> HandleRequestAsync(VSInternalDocumentDiagnosticsParams request, ClientCapabilities clientCapabilities, CancellationToken cancellationToken)
    {
        if (request.TextDocument is null)
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
            _logger.LogInformation("Document {textDocumentUri} closed or deleted, clearing diagnostics.", request.TextDocument.Uri);

            var clearedDiagnosticReport = new VSInternalDiagnosticReport[]
            {
                new VSInternalDiagnosticReport()
                {
                    ResultId = null,
                    Diagnostics = null
                }
            };
            return clearedDiagnosticReport;
        }

        var (synchronized, csharpDoc)= await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
            documentSnapshot.Version,
            request.TextDocument.Uri,
            cancellationToken).ConfigureAwait(false);
        if (!synchronized)
        {
            _logger.LogInformation("Failed to synchronize document {hostDocument}.", request.TextDocument.Uri);

            // Could not synchronize, report nothing changed
            return new VSInternalDiagnosticReport[]
            {
                new VSInternalDiagnosticReport()
                {
                    ResultId = request.PreviousResultId,
                    Diagnostics = null
                }
            };
        }

        var referenceParams = new VSInternalDocumentDiagnosticsParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = csharpDoc.Uri
            },
            PreviousResultId = request.PreviousResultId
        };

        _logger.LogInformation("Requesting diagnostics for {csharpDocUri} with previous result Id of {previousResultId}.", csharpDoc.Uri, request.PreviousResultId);

        var textBuffer = csharpDoc.Snapshot.TextBuffer;
        var requests = _requestInvoker.ReinvokeRequestOnMultipleServersAsync<VSInternalDocumentDiagnosticsParams, VSInternalDiagnosticReport[]>(
            textBuffer,
            VSInternalMethods.DocumentPullDiagnosticName,
            referenceParams,
            cancellationToken).ConfigureAwait(false);

        var resultsFromAllLanguageServers = new List<VSInternalDiagnosticReport>();
        await foreach (var response in requests)
        {
            if (response.Response is not null)
            {
                resultsFromAllLanguageServers.AddRange(response.Response);
            }
        }

        _logger.LogInformation("Received {resultsFromAllLanguageServersCount} diagnostic reports.", resultsFromAllLanguageServers.Count);

        var processedResults = await RemapDocumentDiagnosticsAsync(
            resultsFromAllLanguageServers,
            request.TextDocument.Uri,
            cancellationToken).ConfigureAwait(false);

        // | ---------------------------------------------------------------------------------- |
        // |                       LSP Platform Expected Response Semantics                     |
        // | ---------------------------------------------------------------------------------- |
        // | DiagnosticReport.Diagnostics     | DiagnosticReport.ResultId | Meaning             |
        // | -------------------------------- | ------------------------- | ------------------- |
        // | `null`                           | `null`                    | document gone       |
        // | `null`                           | valid                     | nothing changed     |
        // | valid (non-null including empty) | valid                     | diagnostics changed |
        // | ---------------------------------------------------------------------------------- |
        return processedResults;
    }

    private async Task<IReadOnlyList<VSInternalDiagnosticReport>> RemapDocumentDiagnosticsAsync(
        IReadOnlyList<VSInternalDiagnosticReport> unmappedDiagnosticReports,
        Uri razorDocumentUri,
        CancellationToken cancellationToken)
    {
        if (unmappedDiagnosticReports.Any() != true)
        {
            return unmappedDiagnosticReports;
        }

        var mappedDiagnosticReports = new List<VSInternalDiagnosticReport>(unmappedDiagnosticReports.Count);

        foreach (var diagnosticReport in unmappedDiagnosticReports)
        {
            // Check if there are any diagnostics in this report
            if (diagnosticReport?.Diagnostics?.Any() != true)
            {
                _logger.LogInformation("Diagnostic report contained no diagnostics.");
                if (diagnosticReport is not null)
                {
                    mappedDiagnosticReports.Add(diagnosticReport);
                }

                continue;
            }

            _logger.LogInformation("Requesting processing of {diagnosticsLength} diagnostics.", diagnosticReport.Diagnostics.Length);

            var processedDiagnostics = await _diagnosticsProvider.TranslateAsync(
                RazorLanguageKind.CSharp,
                razorDocumentUri,
                diagnosticReport.Diagnostics,
                cancellationToken
            ).ConfigureAwait(false);

            if (processedDiagnostics is null || !_documentManager.TryGetDocument(razorDocumentUri, out var documentSnapshot) ||
                documentSnapshot.Version != processedDiagnostics.HostDocumentVersion)
            {
                _logger.LogInformation("Document version mismatch, discarding {diagnosticsLength} diagnostics.", diagnosticReport.Diagnostics.Length);

                // We choose to discard diagnostics in this case & report nothing changed.
                diagnosticReport.Diagnostics = null;
                mappedDiagnosticReports.Add(diagnosticReport);
                continue;
            }

            _logger.LogInformation("Returning {diagnosticsLength} diagnostics.", processedDiagnostics.Diagnostics?.Length ?? 0);
            diagnosticReport.Diagnostics = processedDiagnostics.Diagnostics;

            mappedDiagnosticReports.Add(diagnosticReport);
        }

        return mappedDiagnosticReports.ToArray();
    }
}
