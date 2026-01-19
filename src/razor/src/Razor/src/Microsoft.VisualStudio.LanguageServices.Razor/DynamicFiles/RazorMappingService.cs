// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal class RazorMappingService(IDocumentSnapshot document, ITelemetryReporter telemetryReporter, ILoggerFactory loggerFactory) : IRazorMappingService
{
    private readonly IDocumentSnapshot _document = document;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly IDocumentMappingService _documentMappingService = new DocumentMappingService(loggerFactory);
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorMappingService>();

    public async Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(Document document, IEnumerable<TextSpan> spans, CancellationToken cancellationToken)
    {
        // Called on an uninitialized document.
        if (_document is null)
        {
            return [];
        }

        var output = await _document.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        var source = output.Source.Text;

        var csharpDocument = output.GetRequiredCSharpDocument();
        var filePath = output.Source.FilePath.AssumeNotNull();

        using var results = new PooledArrayBuilder<RazorMappedSpanResult>();

        foreach (var span in spans)
        {
            if (RazorEditHelper.TryGetMappedSpan(span, source, csharpDocument, out var linePositionSpan, out var mappedSpan))
            {
                results.Add(new(filePath, linePositionSpan, mappedSpan));
            }
            else
            {
                results.Add(default);
            }
        }

        return results.ToImmutableAndClear();
    }

    public async Task<ImmutableArray<RazorMappedEditResult>> MapTextChangesAsync(Document oldDocument, Document newDocument, CancellationToken cancellationToken)
    {
        try
        {
            if (_document.FilePath is null)
            {
                return [];
            }

            var changes = await newDocument.GetTextChangesAsync(oldDocument, cancellationToken).ConfigureAwait(false);
            var csharpSource = await oldDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);
            var results = await RazorEditHelper.MapCSharpEditsAsync(
                changes.SelectAsArray(c => c.ToRazorTextChange()),
                _document,
                _documentMappingService,
                _telemetryReporter,
                cancellationToken);

            var razorCodeDocument = await _document.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
            var razorSource = razorCodeDocument.Source.Text;
            var textChanges = results.SelectAsArray(te => te.ToTextChange());

            _logger.LogTrace($"""
                Before:
                {DisplayEdits(changes)}

                After:
                {DisplayEdits(textChanges)}
                """);

            return [new RazorMappedEditResult() { FilePath = _document.FilePath, TextChanges = [.. textChanges] }];
        }
        catch (Exception ex)
        {
            _telemetryReporter.ReportFault(ex, "Failed to map edits");
            return [];
        }

        static string DisplayEdits(IEnumerable<TextChange> changes)
        {
            return string.Join(
                        Environment.NewLine,
                        changes.Select(e => $"{e.Span} => '{e.NewText}'"));
        }
    }

    private class DocumentMappingService(ILoggerFactory loggerFactory) : AbstractDocumentMappingService(loggerFactory.GetOrCreateLogger<DocumentMappingService>())
    {
    }
}
