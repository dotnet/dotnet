// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class MappingService(IRazorClientLanguageServerManager razorClientLanguageServerManager) : IRazorMappingService
{
    private const string RazorMapSpansEndpoint = "razor/mapSpans";
    private const string RazorMapTextChangesEndpoint = "razor/mapTextChanges";

    private readonly IRazorClientLanguageServerManager _razorClientLanguageServerManager = razorClientLanguageServerManager;

    public async Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(Document document, IEnumerable<TextSpan> spans, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(document.FilePath) ||
            (spans.TryGetNonEnumeratedCount(out var count) && count == 0))
        {
            return [];
        }

        var sourceText = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);

        var mapParams = new RazorMapSpansParams()
        {
            CSharpDocument = new()
            {
                DocumentUri = new(new Uri(document.FilePath))
            },
            Ranges = [.. spans.Select(sourceText.GetRange)]
        };

        var response = await _razorClientLanguageServerManager.SendRequestAsync<RazorMapSpansParams, RazorMapSpansResponse?>(
            RazorMapSpansEndpoint,
            mapParams,
            cancellationToken).ConfigureAwait(false);

        if (response is not { Spans.Length: > 0, Ranges.Length: > 0 })
        {
            return [];
        }

        Debug.Assert(response.Spans.Length == spans.Count(), "The number of mapped spans should match the number of input spans.");
        Debug.Assert(response.Ranges.Length == spans.Count(), "The number of mapped ranges should match the number of input spans.");

        using var builder = new PooledArrayBuilder<RazorMappedSpanResult>(response.Spans.Length);
        var filePath = response.RazorDocument.DocumentUri.GetRequiredParsedUri().GetDocumentFilePath();

        for (var i = 0; i < response.Spans.Length; i++)
        {
            var span = response.Spans[i];
            var range = response.Ranges[i];

            if (range.IsUndefined())
            {
                continue;
            }

            builder.Add(new RazorMappedSpanResult(filePath, range.ToLinePositionSpan(), span.ToTextSpan()));
        }

        return builder.ToImmutableAndClear();
    }

    public async Task<ImmutableArray<RazorMappedEditResult>> MapTextChangesAsync(Document oldDocument, Document newDocument, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(newDocument.FilePath))
        {
            return [];
        }

        var changes = await newDocument.GetTextChangesAsync(oldDocument, cancellationToken).ConfigureAwait(false);
        var textChanges = changes.Select(c => c.ToRazorTextChange()).ToArray();

        if (textChanges.Length == 0)
        {
            return [];
        }

        var mapParams = new RazorMapTextChangesParams()
        {
            CSharpDocument = new()
            {
                DocumentUri = new(new Uri(newDocument.FilePath))
            },
            TextChanges = textChanges
        };

        var response = await _razorClientLanguageServerManager.SendRequestAsync<RazorMapTextChangesParams, RazorMapTextChangesResponse?>(
            RazorMapTextChangesEndpoint,
            mapParams,
            cancellationToken).ConfigureAwait(false);

        if (response is not { MappedTextChanges.Length: > 0 })
        {
            return [];
        }

        Debug.Assert(response.MappedTextChanges.Length == changes.Count(), "The number of mapped text changes should match the number of input text changes.");
        var filePath = response.RazorDocument.DocumentUri.GetRequiredParsedUri().GetDocumentFilePath();
        var convertedChanges = Array.ConvertAll(response.MappedTextChanges, mappedChange => mappedChange.ToTextChange());
        var result = new RazorMappedEditResult(filePath, convertedChanges);
        return [result];
    }
}
