// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[Shared]
[Export(typeof(LSPDiagnosticsTranslator))]
internal class DefaultLSPDiagnosticsTranslator : LSPDiagnosticsTranslator
{
    private readonly LSPDocumentManager _documentManager;
    private readonly LSPRequestInvoker _requestInvoker;
    private ILogger? _logger;
    private readonly HTMLCSharpLanguageServerLogHubLoggerProvider _loggerProvider;

    [ImportingConstructor]
    public DefaultLSPDiagnosticsTranslator(
        LSPDocumentManager documentManager,
        LSPRequestInvoker requestInvoker,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider)
    {
        _documentManager = documentManager;
        _requestInvoker = requestInvoker;
        _loggerProvider = loggerProvider;
    }

    private async Task<ILogger> GetLoggerAsync(CancellationToken cancellationToken)
    {
        if (_logger is null)
        {
            _logger = await _loggerProvider.CreateLoggerAsync(nameof(DefaultLSPDiagnosticsTranslator), cancellationToken);
        }

        return _logger;
    }

    public override async Task<RazorDiagnosticsResponse?> TranslateAsync(
        RazorLanguageKind languageKind,
        Uri razorDocumentUri,
        Diagnostic[] diagnostics,
        CancellationToken cancellationToken)
    {
        if (!_documentManager.TryGetDocument(razorDocumentUri, out var documentSnapshot))
        {
            return new RazorDiagnosticsResponse()
            {
                Diagnostics = Array.Empty<Diagnostic>(),
            };
        }

        var diagnosticsParams = new RazorDiagnosticsParams()
        {
            Kind = languageKind,
            RazorDocumentUri = razorDocumentUri,
            Diagnostics = diagnostics
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorDiagnosticsParams, RazorDiagnosticsResponse>(
            documentSnapshot.Snapshot.TextBuffer,
            LanguageServerConstants.RazorTranslateDiagnosticsEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            diagnosticsParams,
            cancellationToken).ConfigureAwait(false);
        var logger = await GetLoggerAsync(cancellationToken).ConfigureAwait(false);
        if (!ReinvocationResponseHelper.TryExtractResultOrLog(response, logger, RazorLSPConstants.RazorLanguageServerName, out var result))
        {
            return null;
        }

        return result;
    }
}
