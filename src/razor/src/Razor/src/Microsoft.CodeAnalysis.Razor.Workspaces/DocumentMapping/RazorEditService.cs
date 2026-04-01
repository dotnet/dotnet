// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.CodeAnalysis.Text;
using RoslynSyntaxNode = Microsoft.CodeAnalysis.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal partial class RazorEditService(
    IDocumentMappingService documentMappingService,
    IClientSettingsManager clientSettingsManager,
    ITelemetryReporter telemetryReporter) : IRazorEditService
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    public async Task<ImmutableArray<RazorTextChange>> MapCSharpEditsAsync(
        ImmutableArray<RazorTextChange> textChanges,
        IDocumentSnapshot snapshot,
        bool includeCSharpLanguageFeatureEdits,
        CancellationToken cancellationToken)
    {
        var codeDocument = await snapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        var originalRazorSourceText = codeDocument.Source.Text;

        using var edits = new PooledArrayBuilder<RazorTextChange>();
        AddDirectlyMappedEdits(ref edits.AsRef(), textChanges, codeDocument, cancellationToken, out var skippedEdits);

        if (includeCSharpLanguageFeatureEdits && skippedEdits.Length != 0)
        {
            // If there was something that didn't map, and the caller wants us to, we need to process the generated C# document
            // that Roslyn wanted to produce, and look for changes that we can translate into their Razor equivalents.
            var originalCSharpSyntaxTree = await snapshot.GetCSharpSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var originalCSharpSourceText = await originalCSharpSyntaxTree.GetTextAsync(cancellationToken).ConfigureAwait(false);
            var originalCSharpSyntaxRoot = await originalCSharpSyntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);

            // Important note: We're only applying the skipped edits to this file, and we're not applying the directly mapped edits to the Razor file
            // so the changes here are NOT complete. This isn't important for the scenario we're supporting, which is added or removed C# language
            // features that are outside of the mapped area, but if that changes, it's important to note.
            var newCSharpSourceText = originalCSharpSourceText.WithChanges(skippedEdits.Select(static c => c.ToTextChange()));
            var newCSharpSyntaxTree = originalCSharpSyntaxTree.WithChangedText(newCSharpSourceText);
            var newCSharpSyntaxRoot = await newCSharpSyntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);

            var options = _clientSettingsManager.GetClientSettings().ToRazorFormattingOptions();
            AddCSharpLanguageFeatureChanges(ref edits.AsRef(), codeDocument, originalCSharpSyntaxRoot, originalCSharpSourceText, newCSharpSyntaxRoot, newCSharpSourceText, options, cancellationToken);
        }

        return NormalizeEdits(edits.ToImmutableOrderedByAndClear(static e => e.Span.Start), cancellationToken);
    }

    private static void AddCSharpLanguageFeatureChanges(
        ref PooledArrayBuilder<RazorTextChange> edits,
        RazorCodeDocument codeDocument,
        RoslynSyntaxNode originalCSharpSyntaxRoot,
        SourceText originalCSharpSourceText,
        RoslynSyntaxNode newCSharpSyntaxRoot,
        SourceText newCSharpSourceText,
        RazorFormattingOptions options,
        CancellationToken cancellationToken)
    {
        var oldUsings = UsingDirectiveHelper.FindUsingDirectiveStrings(originalCSharpSyntaxRoot, originalCSharpSourceText);
        var newUsings = UsingDirectiveHelper.FindUsingDirectiveStrings(newCSharpSyntaxRoot, newCSharpSourceText);

        var addedUsings = Delta.Compute(oldUsings, newUsings);
        var removedUsings = Delta.Compute(newUsings, oldUsings);

        AddUsingsChanges(ref edits, codeDocument, addedUsings, removedUsings, cancellationToken);

        var oldMethods = FindMethods(originalCSharpSyntaxRoot, originalCSharpSourceText);
        var newMethods = FindMethods(newCSharpSyntaxRoot, newCSharpSourceText);
        var addedMethods = Delta.Compute(oldMethods, newMethods);

        AddMethodChanges(ref edits, codeDocument, addedMethods, options);
    }

    /// <summary>
    /// Go through edits and make sure a few things are true:
    ///
    /// <list type="number">
    /// <item>
    ///  No edit is added twice. This can happen if a rename happens.
    /// </item>
    /// <item>
    ///  No edit overlaps with another edit. If they do throw to capture logs but choose the first
    ///  edit to at least not completely fail. It's possible this will need to be tweaked later.
    /// </item>
    /// </list>
    /// </summary>
    private ImmutableArray<RazorTextChange> NormalizeEdits(ImmutableArray<RazorTextChange> changes, CancellationToken cancellationToken)
    {
        // Ensure that the changes are sorted by start position otherwise
        // the normalization logic will not work.
        Debug.Assert(changes.SequenceEqual(changes.OrderBy(static c => c.Span.Start)));

        using var normalizedChanges = new PooledArrayBuilder<RazorTextChange>(changes.Length);
        var remaining = changes.AsSpan();

        var droppedEdits = 0;
        while (remaining is not [])
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (remaining is [var edit, var nextEdit, ..])
            {
                var editSpan = edit.Span.ToTextSpan();
                var nextEditSpan = nextEdit.Span.ToTextSpan();

                if (editSpan == nextEditSpan)
                {
                    normalizedChanges.Add(nextEdit);
                    remaining = remaining[1..];

                    if (edit.NewText != nextEdit.NewText)
                    {
                        droppedEdits++;
                    }
                }
                else if (StrictlyContains(editSpan, nextEditSpan))
                {
                    // Cases where there was a removal and addition on the same
                    // line err to taking the addition. This can happen in the
                    // case of a namespace rename
                    if (editSpan.Start == nextEditSpan.Start)
                    {
                        if (string.IsNullOrEmpty(edit.NewText) && !string.IsNullOrEmpty(nextEdit.NewText))
                        {
                            // Don't count this as a dropped edit, it is expected
                            // in the case of a rename
                            normalizedChanges.Add(new RazorTextChange()
                            {
                                Span = edit.Span,
                                NewText = nextEdit.NewText
                            });
                            remaining = remaining[1..];
                        }
                        else
                        {
                            normalizedChanges.Add(edit);
                            remaining = remaining[1..];
                            droppedEdits++;
                        }
                    }
                    else
                    {
                        normalizedChanges.Add(edit);

                        remaining = remaining[1..];
                        droppedEdits++;
                    }
                }
                else if (StrictlyContains(nextEditSpan, editSpan))
                {
                    // Add the edit that is contained in the other edit
                    // and skip the next edit.
                    normalizedChanges.Add(nextEdit);
                    remaining = remaining[1..];
                    if (edit.NewText != nextEdit.NewText)
                    {
                        droppedEdits++;
                    }
                }
                else
                {
                    normalizedChanges.Add(edit);
                }
            }
            else
            {
                normalizedChanges.Add(remaining[0]);
            }

            remaining = remaining[1..];
        }

        if (droppedEdits > 0)
        {
            _telemetryReporter.ReportFault(
                new DroppedEditsException(),
                "Potentially dropped edits when trying to map",
                new Property("droppedEditCount", droppedEdits));
        }

        if (normalizedChanges.Count == changes.Length)
        {
            return changes;
        }

        return normalizedChanges.ToImmutable();
    }

    /// <summary>
    /// Checks whether <paramref name="outer"/> truly contains <paramref name="inner"/>,
    /// excluding the case where <paramref name="inner"/> is a zero-width insertion that sits
    /// exactly at the end of <paramref name="outer"/>. Roslyn's <see cref="TextSpan.Contains(TextSpan)"/>
    /// treats an empty span at the end boundary as contained (e.g. [24,24) inside [23,24)),
    /// but for edit normalization purposes those spans are adjacent, not overlapping, and both
    /// edits should be preserved.
    /// </summary>
    private static bool StrictlyContains(TextSpan outer, TextSpan inner)
        => outer.Contains(inner) && !(inner.IsEmpty && inner.Start == outer.End);

    private RazorTextChange? TryGetMappedEdit(
        RazorCSharpDocument csharpDocument,
        RazorTextChange change,
        ref int lastNewLineAddedToLine)
    {
        var spanStart = change.Span.Start;
        var spanEnd = spanStart + change.Span.Length;
        var newText = change.NewText ?? "";

        var csharpSourceText = csharpDocument.Text;

        // Deliberately doing a naive check to avoid telemetry for truly bad data
        if (spanStart <= 0 || spanStart >= csharpSourceText.Length || spanEnd <= 0 || spanEnd >= csharpSourceText.Length)
        {
            return null;
        }

        var (startLine, startChar) = csharpSourceText.GetLinePosition(spanStart);
        var (endLine, _) = csharpSourceText.GetLinePosition(spanEnd);

        var mappedStart = _documentMappingService.TryMapToRazorDocumentPosition(csharpDocument, spanStart, out _, out var hostStartIndex);
        var mappedEnd = _documentMappingService.TryMapToRazorDocumentPosition(csharpDocument, spanEnd, out _, out var hostEndIndex);

        // Ideal case, both start and end can be mapped so just return the edit
        if (mappedStart && mappedEnd)
        {
            // If the previous edit was on the same line, and added a newline, then we need to add a space
            // between this edit and the previous one, because the normalization will have swallowed it. See
            // below for a more info.
            return new RazorTextChange()
            {
                Span = RazorTextSpan.FromBounds(hostStartIndex, hostEndIndex),
                NewText = (lastNewLineAddedToLine == startLine ? " " : "") + newText
            };
        }

        // For the first line of a code block the C# formatter will often return an edit that starts
        // before our mapping, but ends within. In those cases, when the edit spans multiple lines
        // we just take the last line and try to use that.
        if (!mappedStart && mappedEnd && startLine != endLine)
        {
            // Construct a theoretical edit that is just for the last line of the edit that the C# formatter
            // gave us, and see if we can map that.
            // The +1 here skips the newline character that is found, but also protects from Substring throwing
            // if there are no newlines (which should be impossible anyway)
            var lastNewLine = newText.LastIndexOfAny(['\n', '\r']) + 1;

            // If we're only taking the last line, then we need to be careful about not dropping text that might be
            // important, so we make sure that the text before the last line is all whitespace. If not, we'll let the
            // skipped edit handling deal with this change. The C# formatter doesn't produce this sort of change due
            // to the other processes the edits go through before they get to us.
            if (lastNewLine != 0 && newText[..(lastNewLine - 1)].Any(static c => !char.IsWhiteSpace(c)))
            {
                return null;
            }

            var startSync = csharpSourceText.TryGetAbsoluteIndex((endLine, 0), out var startIndex);
            if (startSync is false)
            {
                return null;
            }

            mappedStart = _documentMappingService.TryMapToRazorDocumentPosition(csharpDocument, startIndex, out _, out hostStartIndex);

            if (mappedStart && mappedEnd)
            {
                return new RazorTextChange()
                {
                    Span = RazorTextSpan.FromBounds(hostStartIndex, hostEndIndex),
                    NewText = newText[lastNewLine..]
                };
            }
        }

        // The opposite case of the above: for the last line of a code block, the C# formatter might
        // return an edit that starts within our mapping, but ends after. In those cases, when the edit
        // spans multiple lines we just take the first line and try to use that.
        if (mappedStart && !mappedEnd && startLine != endLine)
        {
            // Construct a theoretical edit that is just for the first line of the edit that the C# formatter
            // gave us, and see if we can map that.
            if (!csharpSourceText.TryGetAbsoluteIndex(startLine, csharpSourceText.Lines[startLine].Span.Length, out var endIndex))
            {
                return null;
            }

            if (_documentMappingService.TryMapToRazorDocumentPosition(csharpDocument, endIndex, out _, out hostEndIndex))
            {
                // If there's a newline in the new text, only take the part before it
                var firstNewLine = newText.IndexOfAny(['\n', '\r']);
                return new RazorTextChange()
                {
                    Span = RazorTextSpan.FromBounds(hostStartIndex, hostEndIndex),
                    NewText = firstNewLine >= 0
                        ? newText[..firstNewLine]
                        : newText
                };
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
            if (string.IsNullOrWhiteSpace(newText))
            {
                return null;
            }

            var line = csharpSourceText.Lines[startLine];

            // If the line isn't blank, then this isn't a functions directive
            if (line.GetFirstNonWhitespaceOffset() is not null)
            {
                return null;
            }

            // Only do anything if the end of the line in question is a valid mapping point (ie, a transition)
            if (_documentMappingService.TryMapToRazorDocumentPosition(csharpDocument, line.Span.End, out _, out hostEndIndex))
            {
                if (startLine == lastNewLineAddedToLine)
                {
                    // If we already added a newline to this line, then we don't want to add another one, but
                    // we do need to add a space between this edit and the previous one, because the normalization
                    // will have swallowed it.
                    return new RazorTextChange()
                    {
                        Span = new RazorTextSpan
                        {
                            Start = hostEndIndex,
                            Length = 0
                        },
                        NewText = " " + newText
                    };
                }

                // Otherwise, add a newline and the real content, and remember where we added it
                lastNewLineAddedToLine = startLine;
                // Tab size is not used since we only want spaces, so passing 0 is fine.
                var indent = FormattingUtilities.GetIndentationString(startChar, insertSpaces: true, tabSize: 0);
                return new RazorTextChange()
                {
                    Span = new RazorTextSpan
                    {
                        Start = hostEndIndex,
                        Length = 0
                    },
                    NewText = " " + Environment.NewLine + indent + newText
                };
            }
        }

        return null;
    }

    /// <summary>
    /// For all edits that are not mapped to using directives, map them directly to the Razor document.
    /// Edits that don't map are skipped, and using directive changes are handled separately
    /// by <see cref="AddUsingsChanges"/>. The original unmappable C# edits are returned unchanged via
    /// <paramref name="skippedEdits"/>.
    /// </summary>
    private void AddDirectlyMappedEdits(
        ref PooledArrayBuilder<RazorTextChange> edits,
        ImmutableArray<RazorTextChange> csharpEdits,
        RazorCodeDocument codeDocument,
        CancellationToken cancellationToken,
        out ImmutableArray<RazorTextChange> skippedEdits)
    {
        var root = codeDocument.GetRequiredSyntaxRoot();
        var csharpDocument = codeDocument.GetRequiredCSharpDocument();
        var lastNewLineAddedToLine = 0;
        using var skipped = new PooledArrayBuilder<RazorTextChange>();

        foreach (var csharpEdit in csharpEdits)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // First try to map the edit directly from the generated C# document to the Razor document, as that means it can be applied
            // directly. There is some special handling in here for edits where only one end can be mapped, but in general if we can't
            // directly map the edit then we skip it and handle it later with more complex processing.
            if (TryGetMappedEdit(csharpDocument, csharpEdit, ref lastNewLineAddedToLine) is not { } mappedEdit)
            {
                skipped.Add(csharpEdit);
                continue;
            }

            var mappedSpan = mappedEdit.Span.ToTextSpan();
            var node = root.FindNode(mappedSpan, getInnermostNodeForTie: true);
            if (node is null)
            {
                skipped.Add(csharpEdit);
                continue;
            }

            if (RazorSyntaxFacts.IsInUsingDirective(node))
            {
                skipped.Add(csharpEdit);
                continue;
            }

            edits.Add(mappedEdit);

            if (node is BaseMarkupStartTagSyntax startTagSyntax &&
                startTagSyntax.GetEndTag() is { } endTag)
            {
                // We are changing a start tag, and so we have a matching end tag. We have to translate the edit over there too
                // as we only map the start tag, but if they got out of sync that would be bad.
                edits.Add(new RazorTextChange()
                {
                    Span = new RazorTextSpan()
                    {
                        Start = mappedSpan.Start + (endTag.Name.SpanStart - startTagSyntax.Name.SpanStart),
                        Length = mappedSpan.Length
                    },
                    NewText = mappedEdit.NewText
                });
            }
        }

        skippedEdits = skipped.ToImmutable();
    }

    private sealed class DroppedEditsException : Exception
    {
    }
}
