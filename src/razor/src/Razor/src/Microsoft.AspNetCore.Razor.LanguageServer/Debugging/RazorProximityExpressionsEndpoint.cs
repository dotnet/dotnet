// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Debugging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Debugging;

[RazorLanguageServerEndpoint(LanguageServerConstants.RazorProximityExpressionsEndpoint)]
internal class RazorProximityExpressionsEndpoint(
    IDocumentMappingService documentMappingService,
    ILoggerFactory loggerFactory) : IRazorDocumentlessRequestHandler<RazorProximityExpressionsParams, RazorProximityExpressionsResponse?>, ITextDocumentIdentifierHandler<RazorProximityExpressionsParams, Uri>
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorBreakpointSpanEndpoint>();

    public bool MutatesSolutionState => false;

    public Uri GetTextDocumentIdentifier(RazorProximityExpressionsParams request)
        => request.Uri;

    public async Task<RazorProximityExpressionsResponse?> HandleRequestAsync(RazorProximityExpressionsParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        if (documentContext.Snapshot.Version != request.HostDocumentSyncVersion)
        {
            // Whether we are being asked about an old version of the C# document, or somehow a future one, we can't rely on the result.
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var hostDocumentIndex = sourceText.GetPosition(request.Position);

        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        var projectedIndex = hostDocumentIndex;
        var languageKind = codeDocument.GetLanguageKind(hostDocumentIndex, rightAssociative: false);
        // If we're in C#, then map to the right position in the generated document
        if (languageKind == RazorLanguageKind.CSharp &&
            !_documentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), hostDocumentIndex, out _, out projectedIndex))
        {
            return null;
        }
        // Otherwise see if there is more C# on the line to map to
        else if (languageKind == RazorLanguageKind.Html &&
            !_documentMappingService.TryMapToGeneratedDocumentOrNextCSharpPosition(codeDocument.GetCSharpDocument(), hostDocumentIndex, out _, out projectedIndex))
        {
            return null;
        }
        else if (languageKind == RazorLanguageKind.Razor)
        {
            return null;
        }

        // Now ask Roslyn to adjust the breakpoint to a valid location in the code
        var syntaxTree = codeDocument.GetOrParseCSharpSyntaxTree(cancellationToken);
        var expressions = RazorCSharpProximityExpressionResolverService.GetProximityExpressions(syntaxTree, projectedIndex, cancellationToken)?.ToList();
        if (expressions == null)
        {
            return null;
        }

        _logger.LogTrace($"Proximity expressions request for ({request.Position.Line}, {request.Position.Character}) yielded {expressions.Count} results.");

        return new RazorProximityExpressionsResponse
        {
            Expressions = expressions,
        };
    }
}
