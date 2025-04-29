// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.DocumentMapping;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

internal sealed class RazorLSPMappingService(
    LSPDocumentMappingProvider lspDocumentMappingProvider,
    LSPDocumentSnapshot documentSnapshot,
    ITextSnapshot textSnapshot) : IRazorMappingService
{
    private readonly LSPDocumentMappingProvider _lspDocumentMappingProvider = lspDocumentMappingProvider;
    private readonly LSPDocumentSnapshot _documentSnapshot = documentSnapshot;
    private readonly ITextSnapshot _textSnapshot = textSnapshot;

    public Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(
        Document document,
        IEnumerable<TextSpan> spans,
        CancellationToken cancellationToken)
    {
        return MapSpansAsync(
            spans,
            _textSnapshot.AsText(),
            _documentSnapshot.Snapshot.AsText(),
            cancellationToken);
    }

    public async Task<ImmutableArray<RazorMappedEditResult>> MapTextChangesAsync(
        Document oldDocument,
        Document newDocument,
        CancellationToken cancellationToken)
    {
        var changes = await newDocument.GetTextChangesAsync(oldDocument, cancellationToken).ConfigureAwait(false);

        var mappedEdits = await _lspDocumentMappingProvider.MapToDocumentEditsAsync(
            RazorLanguageKind.CSharp,
            _documentSnapshot.Uri,
            [.. changes],
            cancellationToken);

        if (mappedEdits is null)
        {
            return [];
        }

        var sourceTextRazor = _documentSnapshot.Snapshot.AsText();
        var mappedChanges = mappedEdits.TextChanges.Select(e => e.ToTextChange()).ToArray();
        return [new RazorMappedEditResult(_documentSnapshot.Uri.AbsolutePath, mappedChanges)];
    }

    private async Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(
        IEnumerable<TextSpan> spans,
        SourceText sourceTextGenerated,
        SourceText sourceTextRazor,
        CancellationToken cancellationToken)
    {
        var projectedRanges = spans.Select(sourceTextGenerated.GetRange).ToArray();

        var mappedResult = await _lspDocumentMappingProvider.MapToDocumentRangesAsync(
            RazorLanguageKind.CSharp,
            _documentSnapshot.Uri,
            projectedRanges,
            cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        var mappedSpanResults = GetMappedSpanResults(_documentSnapshot.Uri.LocalPath, sourceTextRazor, mappedResult);
        return mappedSpanResults;
    }

    internal static ImmutableArray<RazorMappedSpanResult> GetMappedSpanResults(
        string localFilePath,
        SourceText sourceTextRazor,
        RazorMapToDocumentRangesResponse? mappedResult)
    {
        if (mappedResult is null)
        {
            return [];
        }

        var ranges = mappedResult.Ranges;
        using var results = new PooledArrayBuilder<RazorMappedSpanResult>(capacity: ranges.Length);

        foreach (var mappedRange in ranges)
        {
            if (mappedRange.IsUndefined())
            {
                // Couldn't remap the range correctly. Add default placeholder to indicate to C# that there were issues.
                results.Add(new RazorMappedSpanResult());
                continue;
            }

            var mappedSpan = sourceTextRazor.GetTextSpan(mappedRange);
            var linePositionSpan = sourceTextRazor.GetLinePositionSpan(mappedSpan);
            results.Add(new RazorMappedSpanResult(localFilePath, linePositionSpan, mappedSpan));
        }

        return results.DrainToImmutable();
    }

    public TestAccessor GetTestAccessor() => new(this);

    public readonly struct TestAccessor(RazorLSPMappingService instance)
    {
        public async Task<IEnumerable<(string filePath, LinePositionSpan linePositionSpan, TextSpan span)>> MapSpansAsync(
            IEnumerable<TextSpan> spans,
            SourceText sourceTextGenerated,
            SourceText sourceTextRazor,
            CancellationToken cancellationToken)
        {
            var result = await instance.MapSpansAsync(spans, sourceTextGenerated, sourceTextRazor, cancellationToken).ConfigureAwait(false);
            return result.Select(static mappedResult => (mappedResult.FilePath, mappedResult.LinePositionSpan, mappedResult.Span));
        }
    }
}
