// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

[RazorLanguageServerEndpoint(LanguageServerConstants.RazorLanguageQueryEndpoint)]
internal sealed class RazorLanguageQueryEndpoint(IDocumentMappingService documentMappingService, ILoggerFactory loggerFactory)
    : IRazorRequestHandler<RazorLanguageQueryParams, RazorLanguageQueryResponse?>
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorLanguageQueryEndpoint>();

    public bool MutatesSolutionState { get; } = false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(RazorLanguageQueryParams request)
    {
        return new TextDocumentIdentifier
        {
            DocumentUri = new(request.Uri)
        };
    }

    public async Task<RazorLanguageQueryResponse?> HandleRequestAsync(RazorLanguageQueryParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var documentSnapshot = documentContext.Snapshot;
        var documentVersion = documentContext.Snapshot.Version;

        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = codeDocument.Source.Text;
        var hostDocumentIndex = sourceText.GetPosition(request.Position);
        var responsePosition = request.Position;

        var responsePositionIndex = hostDocumentIndex;

        var languageKind = codeDocument.GetLanguageKind(hostDocumentIndex, rightAssociative: false);
        if (languageKind == RazorLanguageKind.CSharp)
        {
            if (_documentMappingService.TryMapToCSharpDocumentPosition(codeDocument.GetRequiredCSharpDocument(), hostDocumentIndex, out Position? projectedPosition, out var projectedIndex))
            {
                // For C# locations, we attempt to return the corresponding position
                // within the projected document
                responsePosition = projectedPosition;
                responsePositionIndex = projectedIndex;
            }
            else
            {
                // It no longer makes sense to think of this location as C#, since it doesn't
                // correspond to any position in the projected document. This should not happen
                // since there should be source mappings for all the C# spans.
                languageKind = RazorLanguageKind.Razor;
                responsePositionIndex = hostDocumentIndex;
            }
        }

        _logger.LogInformation($"Language query request for ({request.Position.Line}, {request.Position.Character}) = {languageKind} at ({responsePosition.Line}, {responsePosition.Character})");

        return new RazorLanguageQueryResponse()
        {
            Kind = languageKind,
            Position = responsePosition,
            PositionIndex = responsePositionIndex,
            HostDocumentVersion = documentVersion
        };
    }
}
