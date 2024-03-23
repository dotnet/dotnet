// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal abstract class AbstractRazorDocumentMappingService(
    IFilePathService filePathService,
    IDocumentContextFactory documentContextFactory,
    ILogger logger)
    : IRazorDocumentMappingService
{
    private readonly IFilePathService _documentFilePathService = filePathService ?? throw new ArgumentNullException(nameof(filePathService));
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
    private readonly ILogger _logger = logger;

    public TextEdit[] GetHostDocumentEdits(IRazorGeneratedDocument generatedDocument, TextEdit[] generatedDocumentEdits)
    {
        using var _1 = ListPool<TextEdit>.GetPooledObject(out var hostDocumentEdits);
        var generatedDocumentSourceText = GetGeneratedSourceText(generatedDocument);
        var lastNewLineAddedToLine = 0;

        foreach (var edit in generatedDocumentEdits)
        {
            var range = edit.Range;
            if (!IsRangeWithinDocument(range, generatedDocumentSourceText))
            {
                continue;
            }

            var startSync = range.Start.TryGetAbsoluteIndex(generatedDocumentSourceText, _logger, out var startIndex);
            var endSync = range.End.TryGetAbsoluteIndex(generatedDocumentSourceText, _logger, out var endIndex);
            if (startSync is false || endSync is false)
            {
                break;
            }

            var mappedStart = this.TryMapToHostDocumentPosition(generatedDocument, startIndex, out Position? hostDocumentStart, out _);
            var mappedEnd = this.TryMapToHostDocumentPosition(generatedDocument, endIndex, out Position? hostDocumentEnd, out _);

            // Ideal case, both start and end can be mapped so just return the edit
            if (mappedStart && mappedEnd)
            {
                // If the previous edit was on the same line, and added a newline, then we need to add a space
                // between this edit and the previous one, because the normalization will have swallowed it. See
                // below for a more info.
                var newText = (lastNewLineAddedToLine == range.Start.Line ? " " : "") + edit.NewText;
                hostDocumentEdits.Add(new TextEdit()
                {
                    NewText = newText,
                    Range = new Range { Start = hostDocumentStart!, End = hostDocumentEnd! },
                });
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
            if (!mappedStart && mappedEnd && range.SpansMultipleLines())
            {
                // Construct a theoretical edit that is just for the last line of the edit that the C# formatter
                // gave us, and see if we can map that.
                // The +1 here skips the newline character that is found, but also protects from Substring throwing
                // if there are no newlines (which should be impossible anyway)
                var lastNewLine = edit.NewText.LastIndexOfAny(new char[] { '\n', '\r' }) + 1;

                // Strictly speaking we could be dropping more lines than we need to, because our mapping point could be anywhere within the edit
                // but we know that the C# formatter will only be returning blank lines up until the first bit of content that needs to be indented
                // so we can ignore all but the last line. This assert ensures that is true, just in case something changes in Roslyn
                Debug.Assert(lastNewLine == 0 || edit.NewText[..(lastNewLine - 1)].All(c => c == '\r' || c == '\n'), "We are throwing away part of an edit that has more than just empty lines!");

                var proposedRange = new Range { Start = new Position(range.End.Line, 0), End = new Position(range.End.Line, range.End.Character) };
                startSync = proposedRange.Start.TryGetAbsoluteIndex(generatedDocumentSourceText, _logger, out startIndex);
                endSync = proposedRange.End.TryGetAbsoluteIndex(generatedDocumentSourceText, _logger, out endIndex);
                if (startSync is false || endSync is false)
                {
                    break;
                }

                mappedStart = this.TryMapToHostDocumentPosition(generatedDocument, startIndex, out hostDocumentStart, out _);
                mappedEnd = this.TryMapToHostDocumentPosition(generatedDocument, endIndex, out hostDocumentEnd, out _);

                if (mappedStart && mappedEnd)
                {
                    hostDocumentEdits.Add(new TextEdit()
                    {
                        NewText = edit.NewText[lastNewLine..],
                        Range = new Range { Start = hostDocumentStart!, End = hostDocumentEnd! },
                    });
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
            if (!mappedStart && !mappedEnd && !range.SpansMultipleLines())
            {
                // If the new text doesn't have any content we don't care - throwing away invisible whitespace is fine
                if (string.IsNullOrWhiteSpace(edit.NewText))
                {
                    continue;
                }

                var line = generatedDocumentSourceText.Lines[range.Start.Line];

                // If the line isn't blank, then this isn't a functions directive
                if (line.GetFirstNonWhitespaceOffset() is not null)
                {
                    continue;
                }

                // Only do anything if the end of the line in question is a valid mapping point (ie, a transition)
                var endOfLine = line.Span.End;
                if (this.TryMapToHostDocumentPosition(generatedDocument, endOfLine, out Position? hostDocumentIndex, out _))
                {
                    if (range.Start.Line == lastNewLineAddedToLine)
                    {
                        // If we already added a newline to this line, then we don't want to add another one, but
                        // we do need to add a space between this edit and the previous one, because the normalization
                        // will have swallowed it.
                        hostDocumentEdits.Add(new TextEdit()
                        {
                            NewText = " " + edit.NewText,
                            Range = new Range { Start = hostDocumentIndex, End = hostDocumentIndex }
                        });
                    }
                    else
                    {
                        // Otherwise, add a newline and the real content, and remember where we added it
                        lastNewLineAddedToLine = range.Start.Line;
                        hostDocumentEdits.Add(new TextEdit()
                        {
                            NewText = Environment.NewLine + new string(' ', range.Start.Character) + edit.NewText,
                            Range = new Range { Start = hostDocumentIndex, End = hostDocumentIndex }
                        });
                    }

                    continue;
                }
            }
        }

        return hostDocumentEdits.ToArray();
    }

    public bool TryMapToHostDocumentRange(IRazorGeneratedDocument generatedDocument, LinePositionSpan generatedDocumentRange, MappingBehavior mappingBehavior, out LinePositionSpan hostDocumentRange)
    {
        if (generatedDocument is null)
        {
            throw new ArgumentNullException(nameof(generatedDocument));
        }

        if (mappingBehavior == MappingBehavior.Strict)
        {
            return TryMapToHostDocumentRangeStrict(generatedDocument, generatedDocumentRange, out hostDocumentRange);
        }
        else if (mappingBehavior == MappingBehavior.Inclusive)
        {
            return TryMapToHostDocumentRangeInclusive(generatedDocument, generatedDocumentRange, out hostDocumentRange);
        }
        else if (mappingBehavior == MappingBehavior.Inferred)
        {
            return TryMapToHostDocumentRangeInferred(generatedDocument, generatedDocumentRange, out hostDocumentRange);
        }
        else
        {
            throw new InvalidOperationException(SR.Unknown_mapping_behavior);
        }
    }

    public bool TryMapToGeneratedDocumentRange(IRazorGeneratedDocument generatedDocument, LinePositionSpan hostDocumentRange, out LinePositionSpan generatedDocumentRange)
    {
        if (generatedDocument is null)
        {
            throw new ArgumentNullException(nameof(generatedDocument));
        }

        if (generatedDocument.CodeDocument is not { } codeDocument)
        {
            throw new InvalidOperationException("Cannot use document mapping service on a generated document that has a null CodeDocument.");
        }

        generatedDocumentRange = default;

        if (hostDocumentRange.End.Line < hostDocumentRange.Start.Line ||
            hostDocumentRange.End.Line == hostDocumentRange.Start.Line &&
             hostDocumentRange.End.Character < hostDocumentRange.Start.Character)
        {
            _logger.LogWarning("RazorDocumentMappingService:TryMapToGeneratedDocumentRange original range end < start '{originalRange}'", hostDocumentRange);
            Debug.Fail($"RazorDocumentMappingService:TryMapToGeneratedDocumentRange original range end < start '{hostDocumentRange}'");
            return false;
        }

        var sourceText = codeDocument.GetSourceText();
        var range = hostDocumentRange;
        if (!IsRangeWithinDocument(range, sourceText))
        {
            return false;
        }

        if (!range.Start.TryGetAbsoluteIndex(sourceText, _logger, out var startIndex) ||
            !TryMapToGeneratedDocumentPosition(generatedDocument, startIndex, out var generatedRangeStart, out var _))
        {
            return false;
        }

        if (!range.End.TryGetAbsoluteIndex(sourceText, _logger, out var endIndex) ||
            !TryMapToGeneratedDocumentPosition(generatedDocument, endIndex, out var generatedRangeEnd, out var _))
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

        generatedDocumentRange = new LinePositionSpan(generatedRangeStart, generatedRangeEnd);

        return true;
    }

    public bool TryMapToHostDocumentPosition(IRazorGeneratedDocument generatedDocument, int generatedDocumentIndex, out LinePosition hostDocumentPosition, out int hostDocumentIndex)
    {
        if (generatedDocument is null)
        {
            throw new ArgumentNullException(nameof(generatedDocument));
        }

        if (generatedDocument.CodeDocument is not { } codeDocument)
        {
            throw new InvalidOperationException("Cannot use document mapping service on a generated document that has a null CodeDocument.");
        }

        var sourceMappings = generatedDocument.SourceMappings;

        // We expect source mappings to be ordered by their generated document absolute index, because that is how the compiler creates them: As it
        // outputs the generated file to the text write.
        Debug.Assert(sourceMappings.SequenceEqual(sourceMappings.OrderBy(s => s.GeneratedSpan.AbsoluteIndex)));

        var index = sourceMappings.BinarySearchBy(generatedDocumentIndex, static (mapping, generatedDocumentIndex) =>
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
            var distanceIntoGeneratedSpan = generatedDocumentIndex - generatedAbsoluteIndex;

            hostDocumentIndex = mapping.OriginalSpan.AbsoluteIndex + distanceIntoGeneratedSpan;
            var originalLocation = codeDocument.Source.Text.Lines.GetLinePosition(hostDocumentIndex);
            hostDocumentPosition = new LinePosition(originalLocation.Line, originalLocation.Character);
            return true;
        }

        hostDocumentPosition = default;
        hostDocumentIndex = default;
        return false;
    }

    public bool TryMapToGeneratedDocumentOrNextCSharpPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex)
        => TryMapToGeneratedDocumentPositionInternal(generatedDocument, hostDocumentIndex, nextCSharpPositionOnFailure: true, out generatedPosition, out generatedIndex);

    public bool TryMapToGeneratedDocumentPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex)
        => TryMapToGeneratedDocumentPositionInternal(generatedDocument, hostDocumentIndex, nextCSharpPositionOnFailure: false, out generatedPosition, out generatedIndex);

    private static bool TryMapToGeneratedDocumentPositionInternal(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, bool nextCSharpPositionOnFailure, out LinePosition generatedPosition, out int generatedIndex)
    {
        if (generatedDocument is null)
        {
            throw new ArgumentNullException(nameof(generatedDocument));
        }

        if (generatedDocument.CodeDocument is not { } codeDocument)
        {
            throw new InvalidOperationException("Cannot use document mapping service on a generated document that has a null CodeDocument.");
        }

        foreach (var mapping in generatedDocument.SourceMappings)
        {
            var originalSpan = mapping.OriginalSpan;
            var originalAbsoluteIndex = originalSpan.AbsoluteIndex;
            if (originalAbsoluteIndex <= hostDocumentIndex)
            {
                // Treat the mapping as owning the edge at its end (hence <= originalSpan.Length),
                // otherwise we wouldn't handle the cursor being right after the final C# char
                var distanceIntoOriginalSpan = hostDocumentIndex - originalAbsoluteIndex;
                if (distanceIntoOriginalSpan <= originalSpan.Length)
                {
                    generatedIndex = mapping.GeneratedSpan.AbsoluteIndex + distanceIntoOriginalSpan;
                    generatedPosition = GetGeneratedPosition(generatedDocument, generatedIndex);
                    return true;
                }
            }
            else if (nextCSharpPositionOnFailure)
            {
                Debug.Assert(generatedDocument is RazorCSharpDocument);

                // The "next" C# location is only valid if it is on the same line in the source document
                // as the requested position.
                codeDocument.GetSourceText().GetLineAndOffset(hostDocumentIndex, out var hostDocumentLine, out _);

                if (mapping.OriginalSpan.LineIndex == hostDocumentLine)
                {
                    generatedIndex = mapping.GeneratedSpan.AbsoluteIndex;
                    generatedPosition = GetGeneratedPosition(generatedDocument, generatedIndex);
                    return true;
                }

                break;
            }
        }

        generatedPosition = default;
        generatedIndex = default;
        return false;

        static LinePosition GetGeneratedPosition(IRazorGeneratedDocument generatedDocument, int generatedIndex)
        {
            var generatedSource = GetGeneratedSourceText(generatedDocument);
            return generatedSource.Lines.GetLinePosition(generatedIndex);
        }
    }

    public RazorLanguageKind GetLanguageKind(RazorCodeDocument codeDocument, int hostDocumentIndex, bool rightAssociative)
    {
        if (codeDocument is null)
        {
            throw new ArgumentNullException(nameof(codeDocument));
        }

        var classifiedSpans = GetClassifiedSpans(codeDocument);
        var tagHelperSpans = GetTagHelperSpans(codeDocument);
        var documentLength = codeDocument.Source.Text.Length;
        var languageKind = GetLanguageKindCore(classifiedSpans, tagHelperSpans, hostDocumentIndex, documentLength, rightAssociative);

        return languageKind;
    }

    public async Task<WorkspaceEdit> RemapWorkspaceEditAsync(WorkspaceEdit workspaceEdit, CancellationToken cancellationToken)
    {
        if (workspaceEdit.TryGetDocumentChanges(out var documentChanges))
        {
            // The LSP spec says, we should prefer `DocumentChanges` property over `Changes` if available.
            var remappedEdits = await RemapVersionedDocumentEditsAsync(documentChanges, cancellationToken).ConfigureAwait(false);
            return new WorkspaceEdit()
            {
                DocumentChanges = remappedEdits
            };
        }
        else if (workspaceEdit.Changes != null)
        {
            var remappedEdits = await RemapDocumentEditsAsync(workspaceEdit.Changes, cancellationToken).ConfigureAwait(false);
            return new WorkspaceEdit()
            {
                Changes = remappedEdits
            };
        }

        return workspaceEdit;
    }

    public async Task<(Uri MappedDocumentUri, LinePositionSpan MappedRange)> MapToHostDocumentUriAndRangeAsync(Uri generatedDocumentUri, LinePositionSpan generatedDocumentRange, CancellationToken cancellationToken)
    {
        var razorDocumentUri = _documentFilePathService.GetRazorDocumentUri(generatedDocumentUri);

        // For Html we just map the Uri, the range will be the same
        if (_documentFilePathService.IsVirtualHtmlFile(generatedDocumentUri))
        {
            return (razorDocumentUri, generatedDocumentRange);
        }

        // We only map from C# files
        if (!_documentFilePathService.IsVirtualCSharpFile(generatedDocumentUri))
        {
            return (generatedDocumentUri, generatedDocumentRange);
        }

        var documentContext = _documentContextFactory.TryCreate(razorDocumentUri);
        if (documentContext is null)
        {
            return (generatedDocumentUri, generatedDocumentRange);
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var generatedDocument = GetGeneratedDocumentFromGeneratedDocumentUri(generatedDocumentUri, codeDocument);

        // We already checked that the uri was for a generated document, above
        Assumes.NotNull(generatedDocument);

        if (TryMapToHostDocumentRange(generatedDocument, generatedDocumentRange, MappingBehavior.Strict, out var mappedRange))
        {
            return (razorDocumentUri, mappedRange);
        }

        return (generatedDocumentUri, generatedDocumentRange);
    }

    // Internal for testing
    internal static RazorLanguageKind GetLanguageKindCore(
        ImmutableArray<ClassifiedSpanInternal> classifiedSpans,
        ImmutableArray<TagHelperSpanInternal> tagHelperSpans,
        int hostDocumentIndex,
        int hostDocumentLength,
        bool rightAssociative)
    {
        var length = classifiedSpans.Length;
        for (var i = 0; i < length; i++)
        {
            var classifiedSpan = classifiedSpans[i];
            var span = classifiedSpan.Span;

            if (span.AbsoluteIndex <= hostDocumentIndex)
            {
                var end = span.AbsoluteIndex + span.Length;
                if (end >= hostDocumentIndex)
                {
                    if (end == hostDocumentIndex)
                    {
                        // We're at an edge.

                        if (classifiedSpan.SpanKind is SpanKindInternal.MetaCode or SpanKindInternal.Transition)
                        {
                            // If we're on an edge of a transition of some kind (MetaCode representing an open or closing piece of syntax such as <|,
                            // and Transition representing an explicit transition to/from razor syntax, such as @|), prefer to classify to the span
                            // to the right to better represent where the user clicks
                            continue;
                        }

                        // If we're right associative, then we don't want to use the classification that we're at the end
                        // of, if we're also at the start of the next one
                        if (rightAssociative)
                        {
                            if (i < classifiedSpans.Length - 1 && classifiedSpans[i + 1].Span.AbsoluteIndex == hostDocumentIndex)
                            {
                                // If we're at the start of the next span, then use that span
                                return GetLanguageFromClassifiedSpan(classifiedSpans[i + 1]);
                            }

                            // Otherwise, we did not find a match using right associativity, so check for tag helpers
                            break;
                        }
                    }

                    return GetLanguageFromClassifiedSpan(classifiedSpan);
                }
            }
        }

        foreach (var tagHelperSpan in tagHelperSpans)
        {
            var span = tagHelperSpan.Span;

            if (span.AbsoluteIndex <= hostDocumentIndex)
            {
                var end = span.AbsoluteIndex + span.Length;
                if (end >= hostDocumentIndex)
                {
                    if (end == hostDocumentIndex)
                    {
                        // We're at an edge. TagHelper spans never own their edge and aren't represented by marker spans
                        continue;
                    }

                    // Found intersection
                    return RazorLanguageKind.Html;
                }
            }
        }

        // Use the language of the last classified span if we're at the end
        // of the document.
        if (classifiedSpans.Length != 0 && hostDocumentIndex == hostDocumentLength)
        {
            var lastClassifiedSpan = classifiedSpans.Last();
            return GetLanguageFromClassifiedSpan(lastClassifiedSpan);
        }

        // Default to Razor
        return RazorLanguageKind.Razor;

        static RazorLanguageKind GetLanguageFromClassifiedSpan(ClassifiedSpanInternal classifiedSpan)
        {
            // Overlaps with request
            return classifiedSpan.SpanKind switch
            {
                SpanKindInternal.Markup => RazorLanguageKind.Html,
                SpanKindInternal.Code => RazorLanguageKind.CSharp,

                // Content type was non-C# or Html or we couldn't find a classified span overlapping the request position.
                // All other classified span kinds default back to Razor
                _ => RazorLanguageKind.Razor,
            };
        }
    }

    private bool TryMapToHostDocumentRangeStrict(IRazorGeneratedDocument generatedDocument, LinePositionSpan generatedDocumentRange, out LinePositionSpan hostDocumentRange)
    {
        hostDocumentRange = default;

        var generatedSourceText = GetGeneratedSourceText(generatedDocument);
        var range = generatedDocumentRange;
        if (!IsRangeWithinDocument(range, generatedSourceText))
        {
            return false;
        }

        if (!range.Start.TryGetAbsoluteIndex(generatedSourceText, _logger, out var startIndex) ||
            !TryMapToHostDocumentPosition(generatedDocument, startIndex, out var hostDocumentStart, out _))
        {
            return false;
        }

        if (!range.End.TryGetAbsoluteIndex(generatedSourceText, _logger, out var endIndex) ||
            !TryMapToHostDocumentPosition(generatedDocument, endIndex, out var hostDocumentEnd, out _))
        {
            return false;
        }

        // Ensures a valid range is returned, as we're doing two separate TryMapToGeneratedDocumentPosition calls.
        if (hostDocumentEnd < hostDocumentStart)
        {
            return false;
        }

        hostDocumentRange = new LinePositionSpan(hostDocumentStart, hostDocumentEnd);

        return true;
    }

    private bool TryMapToHostDocumentRangeInclusive(IRazorGeneratedDocument generatedDocument, LinePositionSpan generatedDocumentRange, out LinePositionSpan hostDocumentRange)
    {
        if (generatedDocument.CodeDocument is not { } codeDocument)
        {
            throw new InvalidOperationException("Cannot use document mapping service on a generated document that has a null CodeDocument.");
        }

        hostDocumentRange = default;

        var generatedSourceText = GetGeneratedSourceText(generatedDocument);

        if (!IsRangeWithinDocument(generatedDocumentRange, generatedSourceText))
        {
            return false;
        }

        var startIndex = generatedDocumentRange.Start.GetRequiredAbsoluteIndex(generatedSourceText);
        var startMappedDirectly = TryMapToHostDocumentPosition(generatedDocument, startIndex, out var hostDocumentStart, out _);

        var endIndex = generatedDocumentRange.End.GetRequiredAbsoluteIndex(generatedSourceText);
        var endMappedDirectly = TryMapToHostDocumentPosition(generatedDocument, endIndex, out var hostDocumentEnd, out _);

        if (startMappedDirectly && endMappedDirectly && hostDocumentStart <= hostDocumentEnd)
        {
            // We strictly mapped the start/end of the generated range.
            hostDocumentRange = new LinePositionSpan(hostDocumentStart, hostDocumentEnd);
            return true;
        }

        using var _1 = ListPool<SourceMapping>.GetPooledObject(out var candidateMappings);
        if (startMappedDirectly)
        {
            // Start of generated range intersects with a mapping
            candidateMappings.AddRange(generatedDocument.SourceMappings.Where(mapping => IntersectsWith(startIndex, mapping.GeneratedSpan)));
        }
        else if (endMappedDirectly)
        {
            // End of generated range intersects with a mapping
            candidateMappings.AddRange(generatedDocument.SourceMappings.Where(mapping => IntersectsWith(endIndex, mapping.GeneratedSpan)));
        }
        else
        {
            // Our range does not intersect with any mapping; we should see if it overlaps generated locations
            candidateMappings.AddRange(generatedDocument.SourceMappings.Where(mapping => Overlaps(generatedDocumentRange.ToTextSpan(generatedSourceText), mapping.GeneratedSpan)));
        }

        if (candidateMappings.Count == 1)
        {
            // We're intersecting or overlapping a single mapping, lets choose that.

            var mapping = candidateMappings[0];
            hostDocumentRange = ConvertMapping(codeDocument.Source, mapping);
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

        static LinePositionSpan ConvertMapping(RazorSourceDocument sourceDocument, SourceMapping mapping)
        {
            var startLocation = sourceDocument.Text.Lines.GetLinePosition(mapping.OriginalSpan.AbsoluteIndex);
            var endLocation = sourceDocument.Text.Lines.GetLinePosition(mapping.OriginalSpan.AbsoluteIndex + mapping.OriginalSpan.Length);
            var convertedRange = new LinePositionSpan(startLocation, endLocation);
            return convertedRange;
        }
    }

    private bool TryMapToHostDocumentRangeInferred(IRazorGeneratedDocument generatedDocument, LinePositionSpan generatedDocumentRange, out LinePositionSpan hostDocumentRange)
    {
        if (generatedDocument.CodeDocument is not { } codeDocument)
        {
            throw new InvalidOperationException("Cannot use document mapping service on a generated document that has a null CodeDocument.");
        }

        // Inferred mapping behavior is a superset of inclusive mapping behavior so if the range is "inclusive" lets use that mapping.
        if (TryMapToHostDocumentRangeInclusive(generatedDocument, generatedDocumentRange, out hostDocumentRange))
        {
            return true;
        }

        // Doesn't map so lets try and infer some mappings

        hostDocumentRange = default;
        var generatedSourceText = GetGeneratedSourceText(generatedDocument);

        if (!IsRangeWithinDocument(generatedDocumentRange, generatedSourceText))
        {
            return false;
        }

        var generatedRangeAsSpan = generatedDocumentRange.ToTextSpan(generatedSourceText);
        SourceMapping? mappingBeforeGeneratedRange = null;
        SourceMapping? mappingAfterGeneratedRange = null;

        for (var i = generatedDocument.SourceMappings.Length - 1; i >= 0; i--)
        {
            var sourceMapping = generatedDocument.SourceMappings[i];
            var sourceMappingEnd = sourceMapping.GeneratedSpan.AbsoluteIndex + sourceMapping.GeneratedSpan.Length;
            if (generatedRangeAsSpan.Start >= sourceMappingEnd)
            {
                // This is the source mapping that's before us!
                mappingBeforeGeneratedRange = sourceMapping;

                if (i + 1 < generatedDocument.SourceMappings.Length)
                {
                    // We're not at the end of the document there's another source mapping after us
                    mappingAfterGeneratedRange = generatedDocument.SourceMappings[i + 1];
                }

                break;
            }
        }

        if (mappingBeforeGeneratedRange == null)
        {
            // Could not find a mapping before
            return false;
        }

        var sourceDocument = codeDocument.Source;
        var originalSpanBeforeGeneratedRange = mappingBeforeGeneratedRange.OriginalSpan;
        var originalEndBeforeGeneratedRange = originalSpanBeforeGeneratedRange.AbsoluteIndex + originalSpanBeforeGeneratedRange.Length;
        var originalEndPositionBeforeGeneratedRange = sourceDocument.Text.Lines.GetLinePosition(originalEndBeforeGeneratedRange);
        var inferredStartPosition = new LinePosition(originalEndPositionBeforeGeneratedRange.Line, originalEndPositionBeforeGeneratedRange.Character);

        if (mappingAfterGeneratedRange != null)
        {
            // There's a mapping after the "generated range" lets use its start position as our inferred end position.

            var originalSpanAfterGeneratedRange = mappingAfterGeneratedRange.OriginalSpan;
            var originalStartPositionAfterGeneratedRange = sourceDocument.Text.Lines.GetLinePosition(originalSpanAfterGeneratedRange.AbsoluteIndex);

            // The mapping in the generated file is after the start, but when mapped back to the host file that may not be true
            if (originalStartPositionAfterGeneratedRange >= inferredStartPosition)
            {
                hostDocumentRange = new LinePositionSpan(inferredStartPosition, originalStartPositionAfterGeneratedRange);
                return true;
            }
        }

        // There was no projection after the "generated range". Therefore, lets fallback to the end-document location.

        Debug.Assert(sourceDocument.Text.Length > 0, "Source document length should be greater than 0 here because there's a mapping before us");

        var endOfDocumentPosition = sourceDocument.Text.Lines.GetLinePosition(sourceDocument.Text.Length);

        Debug.Assert(endOfDocumentPosition >= inferredStartPosition, "Some how we found a start position that is after the end of the document?");

        hostDocumentRange = new LinePositionSpan(inferredStartPosition, endOfDocumentPosition);
        return true;
    }

    private static bool s_haveAsserted = false;

    private bool IsRangeWithinDocument(Range range, SourceText sourceText)
        => IsRangeWithinDocument(range.ToLinePositionSpan(), sourceText);

    private bool IsRangeWithinDocument(LinePositionSpan range, SourceText sourceText)
    {
        // This might happen when the document that ranges were created against was not the same as the document we're consulting.
        var result = IsPositionWithinDocument(range.Start, sourceText) && IsPositionWithinDocument(range.End, sourceText);

        if (!s_haveAsserted && !result)
        {
            s_haveAsserted = true;
            var sourceTextLinesCount = sourceText.Lines.Count;
            _logger.LogWarning("Attempted to map a range ({startLine},{startChar})-({endLine},{endChar}) outside of the Source (line count {sourceTextLinesCount}.) This could happen if the Roslyn and Razor LSP servers are not in sync.", range.Start.Line, range.Start.Character, range.End.Line, range.End.Character, sourceTextLinesCount);
        }

        return result;

        static bool IsPositionWithinDocument(LinePosition linePosition, SourceText sourceText)
        {
            return sourceText.TryGetAbsoluteIndex(linePosition.Line, linePosition.Character, out _);
        }
    }

    private async Task<TextDocumentEdit[]> RemapVersionedDocumentEditsAsync(TextDocumentEdit[] documentEdits, CancellationToken cancellationToken)
    {
        using var _ = ListPool<TextDocumentEdit>.GetPooledObject(out var remappedDocumentEdits);
        foreach (var entry in documentEdits)
        {
            var generatedDocumentUri = entry.TextDocument.Uri;

            // Check if the edit is actually for a generated document, because if not we don't need to do anything
            if (!_documentFilePathService.IsVirtualDocumentUri(generatedDocumentUri))
            {
                // This location doesn't point to a background razor file. No need to remap.
                remappedDocumentEdits.Add(entry);
                continue;
            }

            var razorDocumentUri = _documentFilePathService.GetRazorDocumentUri(generatedDocumentUri);
            var documentContext = _documentContextFactory.TryCreateForOpenDocument(razorDocumentUri, entry.TextDocument.GetProjectContext());
            if (documentContext is null)
            {
                continue;
            }

            var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

            var remappedEdits = RemapTextEditsCore(generatedDocumentUri, codeDocument, entry.Edits);
            if (remappedEdits is null || remappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            remappedDocumentEdits.Add(new TextDocumentEdit()
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier()
                {
                    Uri = razorDocumentUri,
                    Version = documentContext.Version
                },
                Edits = remappedEdits
            });
        }

        return remappedDocumentEdits.ToArray();
    }

    private async Task<Dictionary<string, TextEdit[]>> RemapDocumentEditsAsync(Dictionary<string, TextEdit[]> changes, CancellationToken cancellationToken)
    {
        var remappedChanges = new Dictionary<string, TextEdit[]>();
        foreach (var entry in changes)
        {
            var uri = new Uri(entry.Key);
            var edits = entry.Value;

            // Check if the edit is actually for a generated document, because if not we don't need to do anything
            if (!_documentFilePathService.IsVirtualDocumentUri(uri))
            {
                remappedChanges[entry.Key] = entry.Value;
                continue;
            }

            var documentContext = _documentContextFactory.TryCreate(uri);
            if (documentContext is null)
            {
                continue;
            }

            var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
            var remappedEdits = RemapTextEditsCore(uri, codeDocument, edits);
            if (remappedEdits is null || remappedEdits.Length == 0)
            {
                // Nothing to do.
                continue;
            }

            var razorDocumentUri = _documentFilePathService.GetRazorDocumentUri(uri);
            remappedChanges[razorDocumentUri.AbsoluteUri] = remappedEdits;
        }

        return remappedChanges;
    }

    private TextEdit[] RemapTextEditsCore(Uri generatedDocumentUri, RazorCodeDocument codeDocument, TextEdit[] edits)
    {
        var generatedDocument = GetGeneratedDocumentFromGeneratedDocumentUri(generatedDocumentUri, codeDocument);
        if (generatedDocument is null)
        {
            return edits;
        }

        using var _ = ListPool<TextEdit>.GetPooledObject(out var remappedEdits);
        for (var i = 0; i < edits.Length; i++)
        {
            var generatedRange = edits[i].Range;
            if (!this.TryMapToHostDocumentRange(generatedDocument, generatedRange, MappingBehavior.Strict, out var originalRange))
            {
                // Can't map range. Discard this edit.
                continue;
            }

            var edit = new TextEdit()
            {
                Range = originalRange,
                NewText = edits[i].NewText
            };

            remappedEdits.Add(edit);
        }

        return remappedEdits.ToArray();
    }

    private IRazorGeneratedDocument? GetGeneratedDocumentFromGeneratedDocumentUri(Uri generatedDocumentUri, RazorCodeDocument codeDocument)
    {
        if (_documentFilePathService.IsVirtualCSharpFile(generatedDocumentUri))
        {
            return codeDocument.GetCSharpDocument();
        }
        else if (_documentFilePathService.IsVirtualHtmlFile(generatedDocumentUri))
        {
            return codeDocument.GetHtmlDocument();
        }
        else
        {
            return null;
        }
    }

    private static SourceText GetGeneratedSourceText(IRazorGeneratedDocument generatedDocument)
    {
        if (generatedDocument.CodeDocument is not { } codeDocument)
        {
            throw new InvalidOperationException("Cannot use document mapping service on a generated document that has a null CodeDocument.");
        }

        return codeDocument.GetGeneratedSourceText(generatedDocument);
    }

    private static ImmutableArray<ClassifiedSpanInternal> GetClassifiedSpans(RazorCodeDocument document)
    {
        // Since this service is called so often, we get a good performance improvement by caching these values
        // for this code document. If the document changes, as the user types, then the document instance will be
        // different, so we don't need to worry about invalidating the cache.
        if (!document.Items.TryGetValue(typeof(ClassifiedSpanInternal), out ImmutableArray<ClassifiedSpanInternal> classifiedSpans))
        {
            var syntaxTree = document.GetSyntaxTree();
            classifiedSpans = ClassifiedSpanVisitor.VisitRoot(syntaxTree);

            document.Items[typeof(ClassifiedSpanInternal)] = classifiedSpans;
        }

        return classifiedSpans;
    }

    private static ImmutableArray<TagHelperSpanInternal> GetTagHelperSpans(RazorCodeDocument document)
    {
        // Since this service is called so often, we get a good performance improvement by caching these values
        // for this code document. If the document changes, as the user types, then the document instance will be
        // different, so we don't need to worry about invalidating the cache.
        if (!document.Items.TryGetValue(typeof(TagHelperSpanInternal), out ImmutableArray<TagHelperSpanInternal> tagHelperSpans))
        {
            var syntaxTree = document.GetSyntaxTree();
            tagHelperSpans = TagHelperSpanVisitor.VisitRoot(syntaxTree);

            document.Items[typeof(TagHelperSpanInternal)] = tagHelperSpans;
        }

        return tagHelperSpans;
    }
}
