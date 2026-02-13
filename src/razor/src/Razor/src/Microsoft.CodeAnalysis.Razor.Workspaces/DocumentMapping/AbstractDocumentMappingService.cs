// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal abstract class AbstractDocumentMappingService(ILogger logger) : IDocumentMappingService
{
    protected readonly ILogger Logger = logger;

    public IEnumerable<TextChange> GetRazorDocumentEdits(RazorCSharpDocument csharpDocument, ImmutableArray<TextChange> csharpChanges)
    {
        var csharpSourceText = csharpDocument.Text;
        var lastNewLineAddedToLine = 0;

        foreach (var change in csharpChanges)
        {
            var span = change.Span;
            // Deliberately doing a naive check to avoid telemetry for truly bad data
            if (span.Start <= 0 || span.Start >= csharpSourceText.Length || span.End <= 0 || span.End >= csharpSourceText.Length)
            {
                continue;
            }

            var (startLine, startChar) = csharpSourceText.GetLinePosition(span.Start);
            var (endLine, _) = csharpSourceText.GetLinePosition(span.End);

            var mappedStart = this.TryMapToRazorDocumentPosition(csharpDocument, span.Start, out var hostDocumentStart, out var hostStartIndex);
            var mappedEnd = this.TryMapToRazorDocumentPosition(csharpDocument, span.End, out var hostDocumentEnd, out var hostEndIndex);

            // Ideal case, both start and end can be mapped so just return the edit
            if (mappedStart && mappedEnd)
            {
                // If the previous edit was on the same line, and added a newline, then we need to add a space
                // between this edit and the previous one, because the normalization will have swallowed it. See
                // below for a more info.
                var newText = (lastNewLineAddedToLine == startLine ? " " : "") + change.NewText;
                yield return new TextChange(TextSpan.FromBounds(hostStartIndex, hostEndIndex), newText);
                continue;
            }

            // For the first line of a code block the C# formatter will often return an edit that starts
            // before our mapping, but ends within. In those cases, when the edit spans multiple lines
            // we just take the last line and try to use that.
            //
            // eg in the C# document you might see:
            //
            //      protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
            //      {
            // #nullable restore
            // #line 1 "/path/to/Document.component"
            //
            //          var x = DateTime.Now;
            //
            // To indent the 'var x' line the formatter will return an edit that starts the line before,
            // with a NewText of '\n            '. The start of that edit is outside our mapping, but we
            // still want to know how to format the 'var x' line, so we have to break up the edit.
            if (!mappedStart && mappedEnd && startLine != endLine)
            {
                // Construct a theoretical edit that is just for the last line of the edit that the C# formatter
                // gave us, and see if we can map that.
                // The +1 here skips the newline character that is found, but also protects from Substring throwing
                // if there are no newlines (which should be impossible anyway)
                var lastNewLine = change.NewText.AssumeNotNull().LastIndexOfAny(['\n', '\r']) + 1;

                // Strictly speaking we could be dropping more lines than we need to, because our mapping point could be anywhere within the edit
                // but we know that the C# formatter will only be returning blank lines up until the first bit of content that needs to be indented
                // so we can ignore all but the last line. This assert ensures that is true, just in case something changes in Roslyn
                Debug.Assert(lastNewLine == 0 || change.NewText[..(lastNewLine - 1)].All(c => c == '\r' || c == '\n'), "We are throwing away part of an edit that has more than just empty lines!");

                var startSync = csharpSourceText.TryGetAbsoluteIndex((endLine, 0), out var startIndex);
                if (startSync is false)
                {
                    break;
                }

                mappedStart = this.TryMapToRazorDocumentPosition(csharpDocument, startIndex, out _, out hostStartIndex);

                if (mappedStart && mappedEnd)
                {
                    yield return new TextChange(TextSpan.FromBounds(hostStartIndex, hostEndIndex), change.NewText[lastNewLine..]);
                    continue;
                }
            }

            // The opposite case of the above: for the last line of a code block, the C# formatter might
            // return an edit that starts within our mapping, but ends after. In those cases, when the edit
            // spans multiple lines we just take the first line and try to use that.
            //
            // This can happen with code actions that remove content, where the edit starts inside the mapped
            // region but extends beyond it. For example, when removing an unused variable in a single-line
            // explicit statement block like `@{ var x = 1; }`, Roslyn may generate edits that span beyond
            // the mapped C# region.
            if (mappedStart && !mappedEnd && startLine != endLine)
            {
                // Construct a theoretical edit that is just for the first line of the edit that the C# formatter
                // gave us, and see if we can map that.

                // Get the end of the start line
                if (!csharpSourceText.TryGetAbsoluteIndex(startLine, csharpSourceText.Lines[startLine].Span.Length, out var endIndex))
                {
                    break;
                }

                if (this.TryMapToRazorDocumentPosition(csharpDocument, endIndex, out _, out hostEndIndex))
                {
                    // If there's a newline in the new text, only take the part before it
                    var firstNewLine = change.NewText.AssumeNotNull().IndexOfAny(['\n', '\r']);
                    var newText = firstNewLine >= 0 ? change.NewText[..firstNewLine] : change.NewText;
                    yield return new TextChange(TextSpan.FromBounds(hostStartIndex, hostEndIndex), newText);
                    continue;
                }
            }

            // If we couldn't map either the start or the end then we still might want to do something tricky.
            // When we have a block like this:
            //
            // @functions {
            //    class Goo
            //    {
            //    }
            // }
            //
            // The source mapping starts at char 13 on the "@functions" line (after the open brace). Unfortunately
            // and code that is needed on that line, say an attribute that the code action wants to insert, will
            // start at char 8 because of the desired indentation of that new code. This means it starts outside of the
            // mapping, so is thrown away, which results in data loss.
            //
            // To fix this we check and if the mapping would have been successful at the end of the line (char 13 above)
            // then we insert a newline, and enough indentation to get us back out to where the new code wanted to start (char 8)
            // and then we're good - we've left the @functions bit alone which razor needs, but we're still able to insert
            // new code above where the C# code is in the generated document.
            //
            // One last hurdle is that sometimes these edits come in as separate edits. So for example replacing "class Goo" above
            // with "public class Goo" would come in as one edit for "public", one for "class" and one for "Goo", all on the same line.
            // When we map the edit for "public" we will push everything down a line, so we don't want to do it for other edits
            // on that line.
            if (!mappedStart && !mappedEnd && startLine == endLine)
            {
                // If the new text doesn't have any content we don't care - throwing away invisible whitespace is fine
                if (string.IsNullOrWhiteSpace(change.NewText))
                {
                    continue;
                }

                var line = csharpSourceText.Lines[startLine];

                // If the line isn't blank, then this isn't a functions directive
                if (line.GetFirstNonWhitespaceOffset() is not null)
                {
                    continue;
                }

                // Only do anything if the end of the line in question is a valid mapping point (ie, a transition)
                if (this.TryMapToRazorDocumentPosition(csharpDocument, line.Span.End, out _, out hostEndIndex))
                {
                    if (startLine == lastNewLineAddedToLine)
                    {
                        // If we already added a newline to this line, then we don't want to add another one, but
                        // we do need to add a space between this edit and the previous one, because the normalization
                        // will have swallowed it.
                        yield return new TextChange(new TextSpan(hostEndIndex, 0), " " + change.NewText);
                    }
                    else
                    {
                        // Otherwise, add a newline and the real content, and remember where we added it
                        lastNewLineAddedToLine = startLine;
                        yield return new TextChange(new TextSpan(hostEndIndex, 0), " " + Environment.NewLine + new string(' ', startChar) + change.NewText);
                    }

                    continue;
                }
            }
        }
    }

    public bool TryMapToRazorDocumentRange(RazorCSharpDocument csharpDocument, LinePositionSpan csharpRange, MappingBehavior mappingBehavior, out LinePositionSpan razorRange)
    {
        if (mappingBehavior == MappingBehavior.Strict)
        {
            return TryMapToRazorDocumentRangeStrict(csharpDocument, csharpRange, out razorRange);
        }
        else if (mappingBehavior == MappingBehavior.Inclusive)
        {
            return TryMapToRazorDocumentRangeInclusive(csharpDocument, csharpRange, out razorRange);
        }
        else if (mappingBehavior == MappingBehavior.Inferred)
        {
            return TryMapToRazorDocumentRangeInferred(csharpDocument, csharpRange, out razorRange);
        }
        else
        {
            throw new InvalidOperationException(SR.Unknown_mapping_behavior);
        }
    }

    public bool TryMapToCSharpDocumentRange(RazorCSharpDocument csharpDocument, LinePositionSpan razorRange, out LinePositionSpan csharpRange)
    {
        csharpRange = default;

        if (razorRange.End.Line < razorRange.Start.Line ||
            (razorRange.End.Line == razorRange.Start.Line &&
             razorRange.End.Character < razorRange.Start.Character))
        {
            Logger.LogWarning($"RazorDocumentMappingService:TryMapToGeneratedDocumentRange original range end < start '{razorRange}'");
            Debug.Fail($"RazorDocumentMappingService:TryMapToGeneratedDocumentRange original range end < start '{razorRange}'");
            return false;
        }

        var sourceText = csharpDocument.CodeDocument.Source.Text;
        var range = razorRange;
        if (!IsSpanWithinDocument(range, sourceText))
        {
            return false;
        }

        if (!sourceText.TryGetAbsoluteIndex(range.Start, out var startIndex) ||
            !TryMapToCSharpDocumentPosition(csharpDocument, startIndex, out var generatedRangeStart, out _))
        {
            return false;
        }

        if (!sourceText.TryGetAbsoluteIndex(range.End, out var endIndex) ||
            !TryMapToCSharpDocumentPosition(csharpDocument, endIndex, out var generatedRangeEnd, out _))
        {
            return false;
        }

        // Ensures a valid range is returned.
        // As we're doing two separate TryMapToGeneratedDocumentPosition calls,
        // it's possible the generatedRangeStart and generatedRangeEnd positions are in completely
        // different places in the document, including the possibility that the
        // generatedRangeEnd position occurs before the generatedRangeStart position.
        // We explicitly disallow such ranges where the end < start.
        if (generatedRangeEnd < generatedRangeStart)
        {
            return false;
        }

        csharpRange = new LinePositionSpan(generatedRangeStart, generatedRangeEnd);

        return true;
    }

    public ImmutableArray<LinePositionSpan> GetCSharpSpansOverlappingRazorSpan(RazorCSharpDocument csharpDocument, LinePositionSpan razorSpan)
    {
        var sourceText = csharpDocument.CodeDocument.Source.Text;
        if (!IsSpanWithinDocument(razorSpan, sourceText))
        {
            return [];
        }

        using var builder = new PooledArrayBuilder<LinePositionSpan>();

        foreach (var mapping in csharpDocument.SourceMappingsSortedByOriginal)
        {
            var originalSpan = mapping.OriginalSpan.ToLinePositionSpan();

            if (razorSpan.OverlapsWith(originalSpan))
            {
                var generatedSpan = mapping.GeneratedSpan.ToLinePositionSpan();

                builder.Add(generatedSpan);
            }
            else if (originalSpan.Start > razorSpan.End)
            {
                // This span (and all following) are after the area we're interested in
                break;
            }
        }

        return builder.ToImmutableAndClear();
    }

    public bool TryMapToRazorDocumentPosition(RazorCSharpDocument csharpDocument, int csharpIndex, out LinePosition razorPosition, out int razorIndex)
    {
        var sourceMappings = csharpDocument.SourceMappingsSortedByGenerated;

        var index = sourceMappings.BinarySearchBy(csharpIndex, static (mapping, generatedDocumentIndex) =>
        {
            var generatedSpan = mapping.GeneratedSpan;
            var generatedAbsoluteIndex = generatedSpan.AbsoluteIndex;
            if (generatedAbsoluteIndex <= generatedDocumentIndex)
            {
                var distanceIntoGeneratedSpan = generatedDocumentIndex - generatedAbsoluteIndex;
                if (distanceIntoGeneratedSpan <= generatedSpan.Length)
                {
                    return 0;
                }

                return -1;
            }

            return 1;
        });

        if (index >= 0)
        {
            var mapping = sourceMappings[index];

            var generatedAbsoluteIndex = mapping.GeneratedSpan.AbsoluteIndex;
            var distanceIntoGeneratedSpan = csharpIndex - generatedAbsoluteIndex;

            razorIndex = mapping.OriginalSpan.AbsoluteIndex + distanceIntoGeneratedSpan;
            razorPosition = csharpDocument.CodeDocument.Source.Text.GetLinePosition(razorIndex);
            return true;
        }

        razorPosition = default;
        razorIndex = default;
        return false;
    }

    public bool TryMapToCSharpPositionOrNext(RazorCSharpDocument csharpDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex)
        => TryMapToCSharpDocumentPositionInternal(csharpDocument, hostDocumentIndex, nextCSharpPositionOnFailure: true, out generatedPosition, out generatedIndex);

    public bool TryMapToCSharpDocumentPosition(RazorCSharpDocument csharpDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex)
        => TryMapToCSharpDocumentPositionInternal(csharpDocument, hostDocumentIndex, nextCSharpPositionOnFailure: false, out generatedPosition, out generatedIndex);

    private static bool TryMapToCSharpDocumentPositionInternal(RazorCSharpDocument csharpDocument, int razorIndex, bool nextCSharpPositionOnFailure, out LinePosition csharpPosition, out int csharpIndex)
    {
        SourceMapping? nextCSharpMapping = null;

        var hostDocumentLine = csharpDocument.CodeDocument.Source.Text.GetLinePosition(razorIndex).Line;

        foreach (var mapping in csharpDocument.SourceMappingsSortedByOriginal)
        {
            var originalSpan = mapping.OriginalSpan;
            var originalAbsoluteIndex = originalSpan.AbsoluteIndex;
            if (originalAbsoluteIndex <= razorIndex)
            {
                // Treat the mapping as owning the edge at its end (hence <= originalSpan.Length),
                // otherwise we wouldn't handle the cursor being right after the final C# char
                var distanceIntoOriginalSpan = razorIndex - originalAbsoluteIndex;
                if (distanceIntoOriginalSpan <= originalSpan.Length)
                {
                    csharpIndex = mapping.GeneratedSpan.AbsoluteIndex + distanceIntoOriginalSpan;
                    csharpPosition = csharpDocument.Text.GetLinePosition(csharpIndex);
                    return true;
                }
            }
            else if (nextCSharpPositionOnFailure &&
                mapping.OriginalSpan.LineIndex == hostDocumentLine &&
                mapping.OriginalSpan.AbsoluteIndex >= razorIndex &&
                (nextCSharpMapping is null || mapping.OriginalSpan.AbsoluteIndex < nextCSharpMapping.OriginalSpan.AbsoluteIndex))
            {
                // The "next" C# location is only valid if it is on the same line in the source document
                // as the requested position, and before than any previous "next" C# position we have found,
                // comparing their original positions.  Due to source mappings being ordered by generated span,
                // not original span, its possible for things to be out of order.
                nextCSharpMapping = mapping;
            }
            else
            {
                // This span (and all following) are after the area we're interested in
                break;
            }
        }

        if (nextCSharpPositionOnFailure && nextCSharpMapping is not null)
        {
            csharpIndex = nextCSharpMapping.GeneratedSpan.AbsoluteIndex;
            csharpPosition = csharpDocument.Text.GetLinePosition(csharpIndex);
            return true;
        }

        csharpPosition = default;
        csharpIndex = default;
        return false;
    }

    private bool TryMapToRazorDocumentRangeStrict(RazorCSharpDocument csharpDocument, LinePositionSpan csharpRange, out LinePositionSpan razorRange)
    {
        razorRange = default;

        var csharpSourceText = csharpDocument.Text;
        var range = csharpRange;
        if (!IsSpanWithinDocument(range, csharpSourceText))
        {
            return false;
        }

        if (!csharpSourceText.TryGetAbsoluteIndex(range.Start, out var startIndex) ||
            !TryMapToRazorDocumentPosition(csharpDocument, startIndex, out var hostDocumentStart, out _))
        {
            return false;
        }

        if (!csharpSourceText.TryGetAbsoluteIndex(range.End, out var endIndex) ||
            !TryMapToRazorDocumentPosition(csharpDocument, endIndex, out var hostDocumentEnd, out _))
        {
            return false;
        }

        // Ensures a valid range is returned, as we're doing two separate TryMapToGeneratedDocumentPosition calls.
        if (hostDocumentEnd < hostDocumentStart)
        {
            return false;
        }

        razorRange = new LinePositionSpan(hostDocumentStart, hostDocumentEnd);

        return true;
    }

    private bool TryMapToRazorDocumentRangeInclusive(RazorCSharpDocument csharpDocument, LinePositionSpan csharpRange, out LinePositionSpan rangeRange)
    {
        rangeRange = default;

        var csharpSourceText = csharpDocument.Text;

        if (!IsSpanWithinDocument(csharpRange, csharpSourceText))
        {
            return false;
        }

        var startIndex = csharpSourceText.GetRequiredAbsoluteIndex(csharpRange.Start);
        var startMappedDirectly = TryMapToRazorDocumentPosition(csharpDocument, startIndex, out var hostDocumentStart, out _);

        var endIndex = csharpSourceText.GetRequiredAbsoluteIndex(csharpRange.End);
        var endMappedDirectly = TryMapToRazorDocumentPosition(csharpDocument, endIndex, out var hostDocumentEnd, out _);

        if (startMappedDirectly && endMappedDirectly && hostDocumentStart <= hostDocumentEnd)
        {
            // We strictly mapped the start/end of the generated range.
            rangeRange = new LinePositionSpan(hostDocumentStart, hostDocumentEnd);
            return true;
        }

        using var _1 = ListPool<SourceMapping>.GetPooledObject(out var candidateMappings);
        var sourceMappings = csharpDocument.SourceMappingsSortedByGenerated;
        if (startMappedDirectly)
        {
            // Start of generated range intersects with a mapping
            candidateMappings.AddRange(
                sourceMappings.Where(mapping => IntersectsWith(startIndex, mapping.GeneratedSpan)));
        }
        else if (endMappedDirectly)
        {
            // End of generated range intersects with a mapping
            candidateMappings.AddRange(
                sourceMappings.Where(mapping => IntersectsWith(endIndex, mapping.GeneratedSpan)));
        }
        else
        {
            // Our range does not intersect with any mapping; we should see if it overlaps generated locations
            candidateMappings.AddRange(
                sourceMappings
                    .Where(mapping => Overlaps(csharpSourceText.GetTextSpan(csharpRange), mapping.GeneratedSpan)));
        }

        if (candidateMappings.Count == 1)
        {
            // We're intersecting or overlapping a single mapping, lets choose that.

            var mapping = candidateMappings[0];
            rangeRange = csharpDocument.CodeDocument.Source.Text.GetLinePositionSpan(mapping.OriginalSpan);
            return true;
        }
        else
        {
            // More then 1 or exactly 0 intersecting/overlapping mappings
            return false;
        }

        bool Overlaps(TextSpan generatedRangeAsSpan, SourceSpan span)
        {
            var overlapStart = Math.Max(generatedRangeAsSpan.Start, span.AbsoluteIndex);
            var overlapEnd = Math.Min(generatedRangeAsSpan.End, span.AbsoluteIndex + span.Length);

            return overlapStart < overlapEnd;
        }

        bool IntersectsWith(int position, SourceSpan span)
        {
            return unchecked((uint)(position - span.AbsoluteIndex) <= (uint)span.Length);
        }
    }

    private bool TryMapToRazorDocumentRangeInferred(RazorCSharpDocument csharpDocument, LinePositionSpan csharpRange, out LinePositionSpan razorRange)
    {
        // Inferred mapping behavior is a superset of inclusive mapping behavior so if the range is "inclusive" lets use that mapping.
        if (TryMapToRazorDocumentRangeInclusive(csharpDocument, csharpRange, out razorRange))
        {
            return true;
        }

        // Doesn't map so lets try and infer some mappings

        razorRange = default;
        var csharpSourceText = csharpDocument.Text;

        if (!IsSpanWithinDocument(csharpRange, csharpSourceText))
        {
            return false;
        }

        var generatedRangeAsSpan = csharpSourceText.GetTextSpan(csharpRange);
        SourceMapping? mappingBeforeGeneratedRange = null;
        SourceMapping? mappingAfterGeneratedRange = null;
        var sourceMappings = csharpDocument.SourceMappingsSortedByGenerated;

        for (var i = sourceMappings.Length - 1; i >= 0; i--)
        {
            var sourceMapping = sourceMappings[i];
            var sourceMappingEnd = sourceMapping.GeneratedSpan.AbsoluteIndex + sourceMapping.GeneratedSpan.Length;
            if (generatedRangeAsSpan.Start >= sourceMappingEnd)
            {
                // This is the source mapping that's before us!
                mappingBeforeGeneratedRange = sourceMapping;

                if (i + 1 < sourceMappings.Length)
                {
                    // We're not at the end of the document there's another source mapping after us
                    mappingAfterGeneratedRange = sourceMappings[i + 1];
                }

                break;
            }
        }

        if (mappingBeforeGeneratedRange == null)
        {
            // Could not find a mapping before
            return false;
        }

        var sourceDocument = csharpDocument.CodeDocument.Source;
        var originalSpanBeforeGeneratedRange = mappingBeforeGeneratedRange.OriginalSpan;
        var originalEndBeforeGeneratedRange = originalSpanBeforeGeneratedRange.AbsoluteIndex + originalSpanBeforeGeneratedRange.Length;
        var inferredStartPosition = sourceDocument.Text.GetLinePosition(originalEndBeforeGeneratedRange);

        if (mappingAfterGeneratedRange != null)
        {
            // There's a mapping after the "generated range" lets use its start position as our inferred end position.

            var originalSpanAfterGeneratedRange = mappingAfterGeneratedRange.OriginalSpan;
            var originalStartPositionAfterGeneratedRange = sourceDocument.Text.GetLinePosition(originalSpanAfterGeneratedRange.AbsoluteIndex);

            // The mapping in the generated file is after the start, but when mapped back to the host file that may not be true
            if (originalStartPositionAfterGeneratedRange >= inferredStartPosition)
            {
                razorRange = new LinePositionSpan(inferredStartPosition, originalStartPositionAfterGeneratedRange);
                return true;
            }
        }

        // There was no projection after the "generated range". Therefore, lets fallback to the end-document location.

        Debug.Assert(sourceDocument.Text.Length > 0, "Source document length should be greater than 0 here because there's a mapping before us");

        var endOfDocumentPosition = sourceDocument.Text.GetLinePosition(sourceDocument.Text.Length);

        Debug.Assert(endOfDocumentPosition >= inferredStartPosition, "Some how we found a start position that is after the end of the document?");

        razorRange = new LinePositionSpan(inferredStartPosition, endOfDocumentPosition);
        return true;
    }

    private static bool s_haveAsserted = false;

    private bool IsSpanWithinDocument(LinePositionSpan span, SourceText sourceText)
    {
        // This might happen when the document that ranges were created against was not the same as the document we're consulting.
        var result = IsPositionWithinDocument(span.Start, sourceText) && IsPositionWithinDocument(span.End, sourceText);

        if (!s_haveAsserted && !result)
        {
            s_haveAsserted = true;
            var sourceTextLinesCount = sourceText.Lines.Count;
            Logger.LogWarning($"Attempted to map a range ({span.Start.Line},{span.Start.Character})-({span.End.Line},{span.End.Character}) outside of the Source (line count {sourceTextLinesCount}.) This could happen if the Roslyn and Razor LSP servers are not in sync.");
        }

        return result;

        static bool IsPositionWithinDocument(LinePosition linePosition, SourceText sourceText)
        {
            return sourceText.TryGetAbsoluteIndex(linePosition, out _);
        }
    }
}
