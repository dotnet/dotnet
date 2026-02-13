// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentExcerpt;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal class RazorDocumentExcerptService(
    IDocumentSnapshot document,
    IRazorMappingService mappingService) : DocumentExcerptService
{
    private readonly IDocumentSnapshot _document = document;
    private readonly IRazorMappingService _mappingService = mappingService;

    internal override async Task<RazorExcerptResult?> TryGetExcerptInternalAsync(
        Document document,
        TextSpan span,
        RazorExcerptMode mode,
        RazorClassificationOptionsWrapper options,
        CancellationToken cancellationToken)
    {
        if (_document is null)
        {
            return null;
        }

        var mappedSpans = await _mappingService.MapSpansAsync(document, [span], cancellationToken).ConfigureAwait(false);
        if (mappedSpans.Length == 0 || mappedSpans[0].Equals(default(RazorMappedSpanResult)))
        {
            return null;
        }

        var project = _document.Project;
        if (!project.TryGetDocument(mappedSpans[0].FilePath, out var razorDocument))
        {
            return null;
        }

        var razorDocumentText = await razorDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);
        var razorDocumentSpan = razorDocumentText.Lines.GetTextSpan(mappedSpans[0].LinePositionSpan);

        var generatedDocument = document;

        // First compute the range of text we want to we to display relative to the primary document.
        var excerptSpan = DocumentExcerptHelper.ChooseExcerptSpan(razorDocumentText, razorDocumentSpan, mode);

        // Then we'll classify the spans based on the primary document, since that's the coordinate
        // space that our output mappings use.
        var output = await _document.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        var mappingsSortedByOriginal = output.GetRequiredCSharpDocument().SourceMappingsSortedByOriginal;
        var classifiedSpans = await DocumentExcerptHelper.ClassifyPreviewAsync(
            excerptSpan,
            generatedDocument,
            mappingsSortedByOriginal,
            options,
            cancellationToken).ConfigureAwait(false);

        var excerptText = DocumentExcerptHelper.GetTranslatedExcerptText(razorDocumentText, ref razorDocumentSpan, ref excerptSpan, classifiedSpans);

        return new RazorExcerptResult(excerptText, razorDocumentSpan, classifiedSpans.ToImmutable(), document, span);
    }
}
