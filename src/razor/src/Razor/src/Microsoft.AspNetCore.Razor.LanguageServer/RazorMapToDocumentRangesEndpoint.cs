// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorMapToDocumentRangesEndpoint : IRazorMapToDocumentRangesHandler
{
    private readonly RazorDocumentMappingService _documentMappingService;

    public RazorMapToDocumentRangesEndpoint(RazorDocumentMappingService documentMappingService)
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
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var documentContext = requestContext.GetRequiredDocumentContext();

        if (request.Kind != RazorLanguageKind.CSharp)
        {
            // All other non-C# requests map directly to where they are in the document.
            return new RazorMapToDocumentRangesResponse()
            {
                Ranges = request.ProjectedRanges,
                HostDocumentVersion = documentContext.Version,
            };
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken);
        var ranges = new Range[request.ProjectedRanges.Length];
        for (var i = 0; i < request.ProjectedRanges.Length; i++)
        {
            var projectedRange = request.ProjectedRanges[i];
            if (codeDocument.IsUnsupported() ||
                !_documentMappingService.TryMapFromProjectedDocumentRange(codeDocument, projectedRange, request.MappingBehavior, out var originalRange))
            {
                // All language queries on unsupported documents return Html. This is equivalent to what pre-VSCode Razor was capable of.
                ranges[i] = RangeExtensions.UndefinedRange;
                continue;
            }

            ranges[i] = originalRange;
        }

        return new RazorMapToDocumentRangesResponse()
        {
            Ranges = ranges,
            HostDocumentVersion = documentContext.Version,
        };
    }
}
