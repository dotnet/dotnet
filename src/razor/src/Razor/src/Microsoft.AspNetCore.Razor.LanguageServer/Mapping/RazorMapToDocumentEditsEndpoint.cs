// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

[RazorLanguageServerEndpoint(LanguageServerConstants.RazorMapToDocumentEditsEndpoint)]
internal partial class RazorMapToDocumentEditsEndpoint(IDocumentMappingService documentMappingService, ITelemetryReporter telemetryReporter, ILoggerFactory loggerFactory) :
    IRazorDocumentlessRequestHandler<RazorMapToDocumentEditsParams, RazorMapToDocumentEditsResponse?>,
    ITextDocumentIdentifierHandler<RazorMapToDocumentEditsParams, Uri>
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorMapToDocumentEditsEndpoint>();

    public bool MutatesSolutionState => false;

    public Uri GetTextDocumentIdentifier(RazorMapToDocumentEditsParams request)
    {
        return request.RazorDocumentUri;
    }

    public async Task<RazorMapToDocumentEditsResponse?> HandleRequestAsync(RazorMapToDocumentEditsParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        if (request.TextChanges.Length == 0)
        {
            return null;
        }

        if (request.Kind != RazorLanguageKind.CSharp)
        {
            // All other non-C# requests map directly to where they are in the document,
            // so the edits do as well
            return new RazorMapToDocumentEditsResponse()
            {
                TextChanges = request.TextChanges,
                HostDocumentVersion = documentContext.Snapshot.Version,
            };
        }

        var mappedEdits = await RazorEditHelper.MapCSharpEditsAsync(
            request.TextChanges.ToImmutableArray(),
            documentContext.Snapshot,
            _documentMappingService,
            _telemetryReporter,
            cancellationToken).ConfigureAwait(false);

        _logger.LogTrace($"""
            Before:
            {DisplayEdits(request.TextChanges)}

            After:
            {DisplayEdits(mappedEdits)}
            """);

        return new RazorMapToDocumentEditsResponse()
        {
            TextChanges = mappedEdits.ToArray(),
        };
    }

    private static string DisplayEdits(IEnumerable<RazorTextChange> changes)
        => string.Join(
            Environment.NewLine,
            changes.Select(e => $"{e.Span} => '{e.NewText}'"));
}
