// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

[LanguageServerEndpoint(LanguageServerConstants.RazorLanguageQueryEndpoint)]
internal sealed class RazorLanguageQueryEndpoint(IRazorDocumentMappingService documentMappingService, IRazorLoggerFactory loggerFactory)
    : IRazorRequestHandler<RazorLanguageQueryParams, RazorLanguageQueryResponse>
{
    private readonly IRazorDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ILogger _logger = loggerFactory.CreateLogger<RazorLanguageQueryEndpoint>();

    public bool MutatesSolutionState { get; } = false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(RazorLanguageQueryParams request)
    {
        return new TextDocumentIdentifier
        {
            Uri = request.Uri
        };
    }

    public async Task<RazorLanguageQueryResponse> HandleRequestAsync(RazorLanguageQueryParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();

        var documentSnapshot = documentContext.Snapshot;
        var documentVersion = documentContext.Version;

        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        var sourceText = await documentSnapshot.GetTextAsync().ConfigureAwait(false);
        var linePosition = new LinePosition(request.Position.Line, request.Position.Character);
        var hostDocumentIndex = sourceText.Lines.GetPosition(linePosition);
        var responsePosition = request.Position;

        if (codeDocument.IsUnsupported())
        {
            // All language queries on unsupported documents return Html. This is equivalent to what pre-VSCode Razor was capable of.
            return new RazorLanguageQueryResponse()
            {
                Kind = RazorLanguageKind.Html,
                Position = responsePosition,
                PositionIndex = hostDocumentIndex,
                HostDocumentVersion = documentVersion,
            };
        }

        var responsePositionIndex = hostDocumentIndex;

        var languageKind = _documentMappingService.GetLanguageKind(codeDocument, hostDocumentIndex, rightAssociative: false);
        if (languageKind == RazorLanguageKind.CSharp)
        {
            if (_documentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), hostDocumentIndex, out Position? projectedPosition, out var projectedIndex))
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

        _logger.LogInformation("Language query request for ({requestPositionLine}, {requestPositionCharacter}) = {languageKind} at ({responsePositionLine}, {responsePositionCharacter})",
            request.Position.Line, request.Position.Character, languageKind, responsePosition.Line, responsePosition.Character);

        return new RazorLanguageQueryResponse()
        {
            Kind = languageKind,
            Position = responsePosition,
            PositionIndex = responsePositionIndex,
            HostDocumentVersion = documentVersion
        };
    }
}
