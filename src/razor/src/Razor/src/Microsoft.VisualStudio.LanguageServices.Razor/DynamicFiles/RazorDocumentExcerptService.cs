// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal class RazorDocumentExcerptService(
    IDocumentSnapshot document,
    IRazorMappingService mappingService) : DocumentExcerptService
{
    private readonly IDocumentSnapshot _document = document;
    private readonly IRazorMappingService _mappingService = mappingService;

    internal override async Task<ExcerptResultInternal?> TryGetExcerptInternalAsync(
        Document document,
        TextSpan span,
        ExcerptModeInternal mode,
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
        var excerptSpan = ChooseExcerptSpan(razorDocumentText, razorDocumentSpan, mode);

        // Then we'll classify the spans based on the primary document, since that's the coordinate
        // space that our output mappings use.
        var output = await _document.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        var mappings = output.GetCSharpDocument().SourceMappings;
        var classifiedSpans = await ClassifyPreviewAsync(
            excerptSpan,
            generatedDocument,
            mappings,
            options,
            cancellationToken).ConfigureAwait(false);

        var excerptText = GetTranslatedExcerptText(razorDocumentText, ref razorDocumentSpan, ref excerptSpan, classifiedSpans);

        return new ExcerptResultInternal(excerptText, razorDocumentSpan, classifiedSpans.ToImmutable(), document, span);
    }

    private async Task<ImmutableArray<ClassifiedSpan>.Builder> ClassifyPreviewAsync(
        TextSpan excerptSpan,
        Document generatedDocument,
        ImmutableArray<SourceMapping> mappings,
        RazorClassificationOptionsWrapper options,
        CancellationToken cancellationToken)
    {
        var builder = ImmutableArray.CreateBuilder<ClassifiedSpan>();

        var sorted = mappings.Sort((x, y) => x.OriginalSpan.AbsoluteIndex.CompareTo(y.OriginalSpan.AbsoluteIndex));

        // The algorithm here is to iterate through the source mappings (sorted) and use the C# classifier
        // on the spans that are known to be C#. For the spans that are not known to be C# then
        // we just treat them as text since we'd don't currently have our own classifications.

        var remainingSpan = excerptSpan;
        foreach (var span in sorted)
        {
            if (excerptSpan.Length == 0)
            {
                break;
            }

            var primarySpan = span.OriginalSpan.AsTextSpan();
            if (primarySpan.Intersection(remainingSpan) is not TextSpan intersection)
            {
                // This span is outside the area we're interested in.
                continue;
            }

            // OK this span intersects with the excerpt span, so we will process it. Let's compute
            // the secondary span that matches the intersection.
            var secondarySpan = span.GeneratedSpan.AsTextSpan();
            secondarySpan = new TextSpan(secondarySpan.Start + intersection.Start - primarySpan.Start, intersection.Length);
            primarySpan = intersection;

            if (remainingSpan.Start < primarySpan.Start)
            {
                // The position is before the next C# span. Classify everything up to the C# start
                // as text.
                builder.Add(new ClassifiedSpan(ClassificationTypeNames.Text, new TextSpan(remainingSpan.Start, primarySpan.Start - remainingSpan.Start)));

                // Advance to the start of the C# span.
                remainingSpan = new TextSpan(primarySpan.Start, remainingSpan.Length - (primarySpan.Start - remainingSpan.Start));
            }

            // We should be able to process this whole span as C#, so classify it.
            //
            // However, we'll have to translate it to the the generated document's coordinates to do that.
            Debug.Assert(remainingSpan.Contains(primarySpan) && remainingSpan.Start == primarySpan.Start);
            var classifiedSecondarySpans = await RazorClassifierAccessor.GetClassifiedSpansAsync(
                generatedDocument,
                secondarySpan,
                options,
                cancellationToken).ConfigureAwait(false);

            // NOTE: The Classifier will only returns spans for things that it understands. That means
            // that whitespace is not classified. The preview expects us to provide contiguous spans,
            // so we are going to have to fill in the gaps.

            // Now we have to translate back to the primary document's coordinates.
            var offset = primarySpan.Start - secondarySpan.Start;
            foreach (var classifiedSecondarySpan in classifiedSecondarySpans)
            {
                // It's possible for the classified span to extend past our secondary span, so we cap it
                var classifiedSpan = classifiedSecondarySpan.TextSpan.End > secondarySpan.End
                    ? TextSpan.FromBounds(classifiedSecondarySpan.TextSpan.Start, secondarySpan.End)
                    : classifiedSecondarySpan.TextSpan;
                Debug.Assert(secondarySpan.Contains(classifiedSpan));

                var updated = new TextSpan(classifiedSpan.Start + offset, classifiedSpan.Length);
                Debug.Assert(primarySpan.Contains(updated));

                // Make sure that we're not introducing a gap. Remember, we need to fill in the whitespace.
                if (remainingSpan.Start < updated.Start)
                {
                    builder.Add(new ClassifiedSpan(
                        ClassificationTypeNames.Text,
                        new TextSpan(remainingSpan.Start, updated.Start - remainingSpan.Start)));
                    remainingSpan = new TextSpan(updated.Start, remainingSpan.Length - (updated.Start - remainingSpan.Start));
                }

                builder.Add(new ClassifiedSpan(classifiedSecondarySpan.ClassificationType, updated));
                remainingSpan = new TextSpan(updated.End, remainingSpan.Length - (updated.End - remainingSpan.Start));
            }

            // Make sure that we're not introducing a gap. Remember, we need to fill in the whitespace.
            if (remainingSpan.Start < primarySpan.End)
            {
                builder.Add(new ClassifiedSpan(
                    ClassificationTypeNames.Text,
                    new TextSpan(remainingSpan.Start, primarySpan.End - remainingSpan.Start)));
                remainingSpan = new TextSpan(primarySpan.End, remainingSpan.Length - (primarySpan.End - remainingSpan.Start));
            }
        }

        // Deal with residue
        if (remainingSpan.Length > 0)
        {
            // Trailing Razor/markup text.
            builder.Add(new ClassifiedSpan(ClassificationTypeNames.Text, remainingSpan));
        }

        return builder;
    }
}
