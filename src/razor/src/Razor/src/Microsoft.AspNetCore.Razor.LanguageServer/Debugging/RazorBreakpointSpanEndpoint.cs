// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Debugging;

[LanguageServerEndpoint(LanguageServerConstants.RazorBreakpointSpanEndpoint)]
internal class RazorBreakpointSpanEndpoint : IRazorDocumentlessRequestHandler<RazorBreakpointSpanParams, RazorBreakpointSpanResponse?>, ITextDocumentIdentifierHandler<RazorBreakpointSpanParams, Uri>
{
    private readonly IRazorDocumentMappingService _documentMappingService;
    private readonly ILogger _logger;

    public bool MutatesSolutionState => false;

    public RazorBreakpointSpanEndpoint(
        IRazorDocumentMappingService documentMappingService,
        IRazorLoggerFactory loggerFactory)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _logger = loggerFactory.CreateLogger<RazorBreakpointSpanEndpoint>();
    }

    public Uri GetTextDocumentIdentifier(RazorBreakpointSpanParams request)
    {
        return request.Uri;
    }

    public async Task<RazorBreakpointSpanResponse?> HandleRequestAsync(RazorBreakpointSpanParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var linePosition = new LinePosition(request.Position.Line, request.Position.Character);
        var hostDocumentIndex = sourceText.Lines.GetPosition(linePosition);

        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        var projectedIndex = hostDocumentIndex;
        var languageKind = _documentMappingService.GetLanguageKind(codeDocument, hostDocumentIndex, rightAssociative: false);
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
        var csharpDocument = codeDocument.GetCSharpDocument();
        var syntaxTree = CSharpSyntaxTree.ParseText(csharpDocument.GeneratedCode, cancellationToken: cancellationToken);
        if (!RazorBreakpointSpans.TryGetBreakpointSpan(syntaxTree, projectedIndex, cancellationToken, out var csharpBreakpointSpan))
        {
            return null;
        }

        var csharpText = codeDocument.GetCSharpSourceText();

        csharpText.GetLineAndOffset(csharpBreakpointSpan.Start, out var startLineIndex, out var startCharacterIndex);
        csharpText.GetLineAndOffset(csharpBreakpointSpan.End, out var endLineIndex, out var endCharacterIndex);

        var projectedRange = new Range()
        {
            Start = new Position(startLineIndex, startCharacterIndex),
            End = new Position(endLineIndex, endCharacterIndex),
        };

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

        _logger.LogTrace("Breakpoint span request for ({requestLine}, {requestCharacter}) = ({hostDocumentStartLine}, {hostDocumentStartCharacter}",
            request.Position.Line, request.Position.Character, hostDocumentRange.Start.Line, hostDocumentRange.Start.Character);

        return new RazorBreakpointSpanResponse()
        {
            Range = hostDocumentRange
        };
    }
}
