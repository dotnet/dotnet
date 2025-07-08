// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
internal sealed class RazorMapToDocumentRangesEndpoint(IDocumentMappingService documentMappingService)
    : IRazorDocumentlessRequestHandler<RazorMapToDocumentRangesParams, RazorMapToDocumentRangesResponse?>, ITextDocumentIdentifierHandler<RazorMapToDocumentRangesParams, Uri>
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;

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
        var csharpDocument = codeDocument.GetRequiredCSharpDocument();

        var ranges = new LspRange[request.ProjectedRanges.Length];
        var spans = new RazorTextSpan[request.ProjectedRanges.Length];

        for (var i = 0; i < request.ProjectedRanges.Length; i++)
        {
            var originalRange = request.ProjectedRanges[i];
            if (request.Kind is RazorLanguageKind.CSharp)
            {
                var projectedRange = request.ProjectedRanges[i];
                if (!_documentMappingService.TryMapToRazorDocumentRange(csharpDocument, projectedRange, request.MappingBehavior, out originalRange))
                {
                    // All language queries on unsupported documents return Html. This is equivalent to what pre-VSCode Razor was capable of.
                    ranges[i] = LspFactory.UndefinedRange;
                    continue;
                }
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
