// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

[LanguageServerEndpoint(LanguageServerConstants.RazorTranslateDiagnosticsEndpoint)]
internal class RazorTranslateDiagnosticsEndpoint : IRazorRequestHandler<RazorDiagnosticsParams, RazorDiagnosticsResponse>
{
    private readonly ILogger _logger;
    private readonly RazorTranslateDiagnosticsService _translateDiagnosticsService;

    public bool MutatesSolutionState { get; } = false;

    public RazorTranslateDiagnosticsEndpoint(
        RazorTranslateDiagnosticsService translateDiagnosticsService,
        ILoggerFactory loggerFactory)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _translateDiagnosticsService = translateDiagnosticsService ?? throw new ArgumentNullException(nameof(translateDiagnosticsService));
        _logger = loggerFactory.CreateLogger<RazorTranslateDiagnosticsEndpoint>();
    }

    public async Task<RazorDiagnosticsResponse> HandleRequestAsync(RazorDiagnosticsParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        _logger.LogInformation("Received {requestKind} diagnostic request for {razorDocumentUri} with {diagnosticsLength} diagnostics.",
            request.Kind, request.RazorDocumentUri, request.Diagnostics.Length);

        cancellationToken.ThrowIfCancellationRequested();

        var documentContext = requestContext.DocumentContext;

        if (documentContext is null)
        {
            _logger.LogInformation("Failed to find document {razorDocumentUri}.", request.RazorDocumentUri);

            return new RazorDiagnosticsResponse()
            {
                Diagnostics = null,
                HostDocumentVersion = null
            };
        }

        var mappedDiagnostics = await _translateDiagnosticsService.TranslateAsync(request.Kind, request.Diagnostics, documentContext, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Returning {mappedDiagnosticsLength} mapped diagnostics.", mappedDiagnostics.Length);

        return new RazorDiagnosticsResponse()
        {
            Diagnostics = mappedDiagnostics.Select(ToVSDiagnostic).ToArray(),
            HostDocumentVersion = documentContext.Version,
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(RazorDiagnosticsParams request)
    {
        return new TextDocumentIdentifier
        {
            Uri = request.RazorDocumentUri,
        };
    }

    private static VSDiagnostic ToVSDiagnostic(Diagnostic diagnostic)
    {
        return new VSDiagnostic
        {
            Code = diagnostic.Code,
            CodeDescription = diagnostic.CodeDescription,
            Message = diagnostic.Message,
            Range = diagnostic.Range,
            Severity = diagnostic.Severity,
            Source = diagnostic.Source,
            Tags = diagnostic.Tags,
        };
    }
}
