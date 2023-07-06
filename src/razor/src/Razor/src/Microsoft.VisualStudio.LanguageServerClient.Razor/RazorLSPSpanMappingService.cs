// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServerClient.Razor.DocumentMapping;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal sealed class RazorLSPSpanMappingService : IRazorSpanMappingService
{
    private readonly LSPDocumentMappingProvider _lspDocumentMappingProvider;

    private readonly ITextSnapshot _textSnapshot;
    private readonly LSPDocumentSnapshot _documentSnapshot;

    public RazorLSPSpanMappingService(
        LSPDocumentMappingProvider lspDocumentMappingProvider,
        LSPDocumentSnapshot documentSnapshot,
        ITextSnapshot textSnapshot)
    {
        if (lspDocumentMappingProvider is null)
        {
            throw new ArgumentNullException(nameof(lspDocumentMappingProvider));
        }

        if (textSnapshot is null)
        {
            throw new ArgumentNullException(nameof(textSnapshot));
        }

        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        _lspDocumentMappingProvider = lspDocumentMappingProvider;
        _textSnapshot = textSnapshot;
        _documentSnapshot = documentSnapshot;
    }

    public async Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(
        Document document,
        IEnumerable<TextSpan> spans,
        CancellationToken cancellationToken)
    {
        return await MapSpansAsync(spans, _textSnapshot.AsText(), _documentSnapshot.Snapshot.AsText(), cancellationToken).ConfigureAwait(false);
    }

    private async Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(
        IEnumerable<TextSpan> spans,
        SourceText sourceTextGenerated,
        SourceText sourceTextRazor,
        CancellationToken cancellationToken)
    {
        if (spans is null)
        {
            throw new ArgumentNullException(nameof(spans));
        }

        var projectedRanges = spans.Select(span => span.AsRange(sourceTextGenerated)).ToArray();

        var mappedResult = await _lspDocumentMappingProvider.MapToDocumentRangesAsync(
            RazorLanguageKind.CSharp,
            _documentSnapshot.Uri,
            projectedRanges,
            cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        var mappedSpanResults = GetMappedSpanResults(_documentSnapshot.Uri.LocalPath, sourceTextRazor, mappedResult);
        return mappedSpanResults;
    }

    // Internal for testing
    internal static ImmutableArray<RazorMappedSpanResult> GetMappedSpanResults(
        string localFilePath,
        SourceText sourceTextRazor,
        RazorMapToDocumentRangesResponse? mappedResult)
    {
        var results = ImmutableArray.CreateBuilder<RazorMappedSpanResult>();

        if (mappedResult is null)
        {
            return results.ToImmutable();
        }

        foreach (var mappedRange in mappedResult.Ranges)
        {
            if (RangeExtensions.IsUndefined(mappedRange))
            {
                // Couldn't remap the range correctly. Add default placeholder to indicate to C# that there were issues.
                results.Add(new RazorMappedSpanResult());
                continue;
            }

            var mappedSpan = mappedRange.AsTextSpan(sourceTextRazor);
            var linePositionSpan = sourceTextRazor.Lines.GetLinePositionSpan(mappedSpan);
            results.Add(new RazorMappedSpanResult(localFilePath, linePositionSpan, mappedSpan));
        }

        return results.ToImmutable();
    }

    // Internal for testing use only
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
    internal async Task<IEnumerable<(string filePath, LinePositionSpan linePositionSpan, TextSpan span)>> MapSpansAsyncTest(
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        IEnumerable<TextSpan> spans,
        SourceText sourceTextGenerated,
        SourceText sourceTextRazor)
    {
        var result = await MapSpansAsync(spans, sourceTextGenerated, sourceTextRazor, cancellationToken: default).ConfigureAwait(false);
        return result.Select(mappedResult => (mappedResult.FilePath, mappedResult.LinePositionSpan, mappedResult.Span));
    }
}
