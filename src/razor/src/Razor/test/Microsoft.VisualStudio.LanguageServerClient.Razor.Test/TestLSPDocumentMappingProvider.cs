// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;
using RazorMapToDocumentRangesResponse = Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp.RazorMapToDocumentRangesResponse;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Test;

internal class TestLSPDocumentMappingProvider : LSPDocumentMappingProvider
{
    private readonly Dictionary<Uri, (int hostDocumentVersion, RazorCodeDocument codeDocument)> _uriToVersionAndCodeDocumentMap;
    private readonly DefaultRazorDocumentMappingService _documentMappingService;

    public TestLSPDocumentMappingProvider(ILoggerFactory loggerFactory)
    {
        _uriToVersionAndCodeDocumentMap = new();
        _documentMappingService = new DefaultRazorDocumentMappingService(TestLanguageServerFeatureOptions.Instance, new TestDocumentContextFactory(), loggerFactory);
    }

    public int TextEditRemapCount { get; set; } = 0;

    public TestLSPDocumentMappingProvider(Dictionary<Uri, (int, RazorCodeDocument)> uriToVersionAndCodeDocumentMap, ILoggerFactory loggerFactory)
    {
        _uriToVersionAndCodeDocumentMap = uriToVersionAndCodeDocumentMap;
        _documentMappingService = new DefaultRazorDocumentMappingService(TestLanguageServerFeatureOptions.Instance, new TestDocumentContextFactory(), loggerFactory);
    }

    public override Task<RazorMapToDocumentRangesResponse?> MapToDocumentRangesAsync(
        RazorLanguageKind languageKind,
        Uri razorDocumentUri,
        Range[] projectedRanges,
        CancellationToken cancellationToken)
            => MapToDocumentRangesAsync(languageKind, razorDocumentUri, projectedRanges, LanguageServerMappingBehavior.Strict, cancellationToken);

    public override Task<RazorMapToDocumentRangesResponse?> MapToDocumentRangesAsync(
        RazorLanguageKind languageKind,
        Uri razorDocumentUri,
        Range[] projectedRanges,
        LanguageServerMappingBehavior mappingBehavior,
        CancellationToken cancellationToken)
    {
        if (languageKind != RazorLanguageKind.CSharp)
        {
            throw new NotImplementedException();
        }

        if (!_uriToVersionAndCodeDocumentMap.TryGetValue(razorDocumentUri, out var result))
        {
            return Task.FromResult<RazorMapToDocumentRangesResponse?>(null);
        }

        var mappedMappingBehavior = mappingBehavior == LanguageServerMappingBehavior.Strict ? MappingBehavior.Strict : MappingBehavior.Inclusive;

        var ranges = new Range[projectedRanges.Length];
        for (var i = 0; i < projectedRanges.Length; i++)
        {
            var projectedRange = projectedRanges[i];
            if (result.codeDocument.IsUnsupported() ||
                !_documentMappingService.TryMapFromProjectedDocumentRange(result.codeDocument, projectedRange, mappedMappingBehavior, out var originalRange))
            {
                ranges[i] = RangeExtensions.UndefinedRange;
                continue;
            }

            ranges[i] = originalRange;
        }

        var response = new RazorMapToDocumentRangesResponse()
        {
            Ranges = ranges,
            HostDocumentVersion = result.hostDocumentVersion,
        };

        return Task.FromResult<RazorMapToDocumentRangesResponse?>(response);
    }

    public override Task<TextEdit[]> RemapFormattedTextEditsAsync(
        Uri uri,
        TextEdit[] edits,
        FormattingOptions options,
        bool containsSnippet,
        CancellationToken cancellationToken)
    {
        TextEditRemapCount++;
        return Task.FromResult(edits);
    }

    public override Task<Location[]> RemapLocationsAsync(Location[] locations, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<TextEdit[]> RemapTextEditsAsync(Uri uri, TextEdit[] edits, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<WorkspaceEdit> RemapWorkspaceEditAsync(WorkspaceEdit workspaceEdit, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
