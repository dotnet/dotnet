// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
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

[RazorLanguageServerEndpoint(LanguageServerConstants.RazorBreakpointSpanEndpoint)]
internal class RazorBreakpointSpanEndpoint(
    IDocumentMappingService documentMappingService,
    ILoggerFactory loggerFactory) : IRazorDocumentlessRequestHandler<RazorBreakpointSpanParams, RazorBreakpointSpanResponse?>, ITextDocumentIdentifierHandler<RazorBreakpointSpanParams, Uri>
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorBreakpointSpanEndpoint>();

    public bool MutatesSolutionState => false;

    public Uri GetTextDocumentIdentifier(RazorBreakpointSpanParams request)
        => request.Uri;

    public async Task<RazorBreakpointSpanResponse?> HandleRequestAsync(RazorBreakpointSpanParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
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
        if (!RazorBreakpointSpans.TryGetBreakpointSpan(syntaxTree, projectedIndex, cancellationToken, out var csharpBreakpointSpan))
        {
            return null;
        }

        var csharpText = codeDocument.GetCSharpSourceText();
        var projectedRange = csharpText.GetRange(csharpBreakpointSpan);

        // Now map that new C# location back to the host document
        // Razor files generate code in a "loosely" debuggable way. For instance if you were to do the following in a razor or cshtml file:
        //
        //      @DateTime.Now
        //
        // This would render as:
        //
        //      #line 123 "C:/path/to/abc.cshtml"
        //      __o = DateTime.Now;
        //
        //      #line default
        //
        // This in turn results in a breakpoint span encompassing `|__o = DateTime.Now;|`. Problem is that if we're doing "strict" mapping
        // Razor only maps `DateTime.Now` so mapping would fail. Therefore we use inclusive mapping which allows C# mappings that intersect
        // to be acceptable mapping locations
        if (!_documentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), projectedRange, MappingBehavior.Inclusive, out var hostDocumentRange))
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogTrace($"Breakpoint span request for ({request.Position.Line}, {request.Position.Character}) = ({hostDocumentRange.Start.Line}, {hostDocumentRange.Start.Character}");

        return new RazorBreakpointSpanResponse()
        {
            Range = hostDocumentRange
        };
    }
}
