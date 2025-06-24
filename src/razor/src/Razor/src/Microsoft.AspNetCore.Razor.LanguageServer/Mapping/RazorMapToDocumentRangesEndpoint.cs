// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

[RazorLanguageServerEndpoint(LanguageServerConstants.RazorMapToDocumentRangesEndpoint)]
internal sealed class RazorMapToDocumentRangesEndpoint :
    IRazorDocumentlessRequestHandler<RazorMapToDocumentRangesParams, RazorMapToDocumentRangesResponse?>,
    ITextDocumentIdentifierHandler<RazorMapToDocumentRangesParams, Uri>
{
    private readonly IDocumentMappingService _documentMappingService;

    public RazorMapToDocumentRangesEndpoint(IDocumentMappingService documentMappingService)
    {
        _documentMappingService = documentMappingService;
    }

    public bool MutatesSolutionState { get; } = false;

    public Uri GetTextDocumentIdentifier(RazorMapToDocumentRangesParams request)
    {
        return request.RazorDocumentUri;
    }

    public async Task<RazorMapToDocumentRangesResponse?> HandleRequestAsync(RazorMapToDocumentRangesParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (request.Kind == RazorLanguageKind.Razor)
        {
            return null;
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        IRazorGeneratedDocument? generatedDocument = request.Kind switch
        {
            RazorLanguageKind.CSharp => codeDocument.GetRequiredCSharpDocument(),
            RazorLanguageKind.Html => codeDocument.GetHtmlDocument(),
            _ => throw new NotSupportedException($"Unsupported language kind '{request.Kind}'."),
        };

        if (generatedDocument is null)
        {
            return null;
        }

        var ranges = new LspRange[request.ProjectedRanges.Length];
        var spans = new RazorTextSpan[request.ProjectedRanges.Length];

        for (var i = 0; i < request.ProjectedRanges.Length; i++)
        {
            var projectedRange = request.ProjectedRanges[i];
            if (!_documentMappingService.TryMapToHostDocumentRange(generatedDocument, projectedRange, request.MappingBehavior, out var originalRange))
            {
                // All language queries on unsupported documents return Html. This is equivalent to what pre-VSCode Razor was capable of.
                ranges[i] = LspFactory.UndefinedRange;
                continue;
            }

            ranges[i] = originalRange;
            spans[i] = originalRange.ToRazorTextSpan(codeDocument.Source.Text);
        }

        return new RazorMapToDocumentRangesResponse()
        {
            Ranges = ranges,
            HostDocumentVersion = documentContext.Snapshot.Version,
            Spans = spans,
        };
    }
}
