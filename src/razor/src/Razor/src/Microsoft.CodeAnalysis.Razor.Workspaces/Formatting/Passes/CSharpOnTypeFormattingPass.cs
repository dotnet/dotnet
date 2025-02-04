// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

/// <summary>
/// Gets edits in C# files, and returns edits to Razor files, with nicely formatted Html
/// </summary>
internal sealed class CSharpOnTypeFormattingPass(
    IDocumentMappingService documentMappingService,
    IHostServicesProvider hostServicesProvider,
    ILoggerFactory loggerFactory)
    : CSharpFormattingPassBase(documentMappingService, hostServicesProvider, isFormatOnType: true)
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CSharpOnTypeFormattingPass>();

    protected async override Task<ImmutableArray<TextChange>> ExecuteCoreAsync(FormattingContext context, RoslynWorkspaceHelper roslynWorkspaceHelper, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        // Normalize and re-map the C# edits.
        var codeDocument = context.CodeDocument;
        var csharpText = codeDocument.GetCSharpSourceText();

        if (changes.Length == 0)
        {
            if (!DocumentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), context.HostDocumentIndex, out _, out var projectedIndex))
            {
                _logger.LogWarning($"Failed to map to projected position for document {context.OriginalSnapshot.FilePath}.");
                return [];
            }

            // Ask C# for formatting changes.
            var autoFormattingOptions = new RazorAutoFormattingOptions(
                formatOnReturn: true, formatOnTyping: true, formatOnSemicolon: true, formatOnCloseBrace: true);

            var formattingChanges = await RazorCSharpFormattingInteractionService.GetFormattingChangesAsync(
                roslynWorkspaceHelper.CreateCSharpDocument(context.CodeDocument),
                typedChar: context.TriggerCharacter,
                projectedIndex,
                context.Options.ToIndentationOptions(),
                autoFormattingOptions,
                indentStyle: CodeAnalysis.Formatting.FormattingOptions.IndentStyle.Smart,
                cancellationToken).ConfigureAwait(false);

            if (formattingChanges.IsEmpty)
            {
                _logger.LogInformation($"Received no results.");
                return [];
            }

            changes = formattingChanges;
            _logger.LogInformation($"Received {changes.Length} results from C#.");
        }

        // Sometimes the C# document is out of sync with our document, so Roslyn can return edits to us that will throw when we try
        // to normalize them. Instead of having this flow up and log a NFW, we just capture it here. Since this only happens when typing
        // very quickly, it is a safe assumption that we'll get another chance to do on type formatting, since we know the user is typing.
        // The proper fix for this is https://github.com/dotnet/razor-tooling/issues/6650 at which point this can be removed
        foreach (var edit in changes)
        {
            var startPos = edit.Span.Start;
            var endPos = edit.Span.End;
            var count = csharpText.Length;
            if (startPos > count || endPos > count)
            {
                _logger.LogWarning($"Got a bad edit that couldn't be applied. Edit is {startPos}-{endPos} but there are only {count} characters in C#.");
                return [];
            }
        }

        _logger.LogTestOnly($"Original C#:\r\n{csharpText}");

        var normalizedChanges = csharpText.MinimizeTextChanges(changes, out var originalTextWithChanges);

        _logger.LogTestOnly($"Formatted C#:\r\n{originalTextWithChanges}");

        var mappedChanges = RemapTextChanges(codeDocument, normalizedChanges);
        var filteredChanges = FilterCSharpTextChanges(context, mappedChanges);
        if (filteredChanges.Length == 0)
        {
            // There are no C# edits for us to apply that could be mapped, but we might still need to check for using statements
            // because they are non mappable, but might be the only thing changed (eg from the Add Using code action)
            //
            // If there aren't any edits that are likely to contain using statement changes, this call will no-op.
            filteredChanges = await AddUsingStatementEditsIfNecessaryAsync(context, changes, originalTextWithChanges, filteredChanges, cancellationToken).ConfigureAwait(false);

            return filteredChanges;
        }

        // Find the lines that were affected by these edits.
        var originalText = codeDocument.Source.Text;
        _logger.LogTestOnly($"Original text:\r\n{originalText}");

        _logger.LogTestOnly($"Source Mappings:\r\n{RenderSourceMappings(context.CodeDocument)}");

        // Apply the format on type edits sent over by the client.
        var formattedText = ApplyChangesAndTrackChange(originalText, filteredChanges, out _, out var spanAfterFormatting);
        _logger.LogTestOnly($"After C# changes:\r\n{formattedText}");

        var changedContext = await context.WithTextAsync(formattedText, cancellationToken).ConfigureAwait(false);
        var linePositionSpanAfterFormatting = formattedText.GetLinePositionSpan(spanAfterFormatting);

        cancellationToken.ThrowIfCancellationRequested();

        // We make an optimistic attempt at fixing corner cases.
        var cleanupChanges = CleanupDocument(changedContext, linePositionSpanAfterFormatting);
        var cleanedText = formattedText.WithChanges(cleanupChanges);
        _logger.LogTestOnly($"After CleanupDocument:\r\n{cleanedText}");

        changedContext = await changedContext.WithTextAsync(cleanedText, cancellationToken).ConfigureAwait(false);

        // At this point we should have applied all edits that adds/removes newlines.
        // Let's now ensure the indentation of each of those lines is correct.

        // We only want to adjust the range that was affected.
        // We need to take into account the lines affected by formatting as well as cleanup.
        var lineDelta = LineDelta(formattedText, cleanupChanges, out var firstLine, out var lastLine);

        // Okay hear me out, I know this looks lazy, but it totally makes sense.
        // This method is called with edits that the C# formatter wants to make, and from those edits we work out which
        // other edits to apply etc. Fine, all good so far. BUT its totally possible that the user typed a closing brace
        // in the same position as the C# formatter thought it should be, on the line _after_ the code that the C# formatter
        // reformatted.
        //
        // For example, given:
        // if (true){
        //     }
        //
        // If the C# formatter is happy with the placement of that close brace then this method will get two edits:
        //  * On line 1 to indent the if by 4 spaces
        //  * On line 1 to add a newline and 4 spaces in front of the opening brace
        //
        // We'll happy format lines 1 and 2, and ignore the closing brace altogether. So, by looking one line further
        // we won't have that problem.
        if (linePositionSpanAfterFormatting.End.Line + lineDelta < cleanedText.Lines.Count - 1)
        {
            lineDelta++;
        }

        // Now we know how many lines were affected by the cleanup and formatting, but we don't know where those lines are. For example, given:
        //
        // @if (true)
        // {
        //      }
        // else
        // {
        // $$}
        //
        // When typing that close brace, the changes would fix the previous close brace, but the line delta would be 0, so
        // we'd format line 6 and call it a day, even though the formatter made an edit on line 3. To fix this we use the
        // first and last position of edits made above, and make sure our range encompasses them as well. For convenience
        // we calculate these positions in the LineDelta method called above.
        var startLine = Math.Min(firstLine, linePositionSpanAfterFormatting.Start.Line);
        var endLineInclusive = Math.Max(lastLine, linePositionSpanAfterFormatting.End.Line + lineDelta);

        Debug.Assert(cleanedText.Lines.Count > endLineInclusive, "Invalid range. This is unexpected.");

        var indentationChanges = await AdjustIndentationAsync(changedContext, startLine, endLineInclusive, roslynWorkspaceHelper.HostWorkspaceServices, _logger, cancellationToken).ConfigureAwait(false);
        if (indentationChanges.Length > 0)
        {
            // Apply the edits that modify indentation.
            cleanedText = cleanedText.WithChanges(indentationChanges);

            _logger.LogTestOnly($"After AdjustIndentationAsync:\r\n{cleanedText}");
        }

        // Now that we have made all the necessary changes to the document. Let's diff the original vs final version and return the diff.
        var finalChanges = cleanedText.GetTextChangesArray(originalText);

        finalChanges = await AddUsingStatementEditsIfNecessaryAsync(context, changes, originalTextWithChanges, finalChanges, cancellationToken).ConfigureAwait(false);

        return finalChanges;
    }

    private ImmutableArray<TextChange> RemapTextChanges(RazorCodeDocument codeDocument, ImmutableArray<TextChange> projectedTextChanges)
    {
        if (codeDocument.IsUnsupported())
        {
            return [];
        }

        var changes = DocumentMappingService.GetHostDocumentEdits(codeDocument.GetCSharpDocument(), projectedTextChanges);

        return changes.ToImmutableArray();
    }

    private static async Task<ImmutableArray<TextChange>> AddUsingStatementEditsIfNecessaryAsync(FormattingContext context, ImmutableArray<TextChange> changes, SourceText originalTextWithChanges, ImmutableArray<TextChange> finalChanges, CancellationToken cancellationToken)
    {
        if (context.AutomaticallyAddUsings)
        {
            // Because we need to parse the C# code twice for this operation, lets do a quick check to see if its even necessary
            if (changes.Any(static e => e.NewText is not null && e.NewText.IndexOf("using") != -1))
            {
                var usingStatementEdits = await AddUsingsHelper.GetUsingStatementEditsAsync(context.CodeDocument, originalTextWithChanges, cancellationToken).ConfigureAwait(false);
                var usingStatementChanges = usingStatementEdits.Select(context.CodeDocument.Source.Text.GetTextChange);
                finalChanges = [.. usingStatementChanges, .. finalChanges];
            }
        }

        return finalChanges;
    }

    // Returns the minimal TextSpan that encompasses all the differences between the old and the new text.
    private static SourceText ApplyChangesAndTrackChange(SourceText oldText, IEnumerable<TextChange> changes, out TextSpan spanBeforeChange, out TextSpan spanAfterChange)
    {
        var newText = oldText.WithChanges(changes);
        var affectedRange = newText.GetEncompassingTextChangeRange(oldText);

        spanBeforeChange = affectedRange.Span;
        spanAfterChange = new TextSpan(spanBeforeChange.Start, affectedRange.NewLength);

        return newText;
    }

    private static ImmutableArray<TextChange> FilterCSharpTextChanges(FormattingContext context, ImmutableArray<TextChange> changes)
    {
        var indent = context.GetIndentationLevelString(1);

        using var filteredChanges = new PooledArrayBuilder<TextChange>();

        foreach (var change in changes)
        {
            if (!ShouldFormat(context, change.Span, allowImplicitStatements: false))
            {
                continue;
            }

            // One extra bit of filtering we do here, is to guard against quirks in runtime code-gen, where source mappings
            // end after whitespace, rather than design time where they end before. This results in the C# formatter wanting
            // to insert an indent in what ends up being the middle of a line of Razor code. Since there is no reason to ever
            // insert anything but a single space in the middle of a line, it's easy to filter them out.
            if (change.Span.Length == 0 &&
                change.NewText == indent)
            {
                var linePosition = context.SourceText.GetLinePosition(change.Span.Start);
                var first = context.SourceText.Lines[linePosition.Line].GetFirstNonWhitespaceOffset();
                if (linePosition.Character > first)
                {
                    continue;
                }
            }

            filteredChanges.Add(change);
        }

        return filteredChanges.ToImmutable();
    }

    private static int LineDelta(SourceText text, IEnumerable<TextChange> changes, out int firstLine, out int lastLine)
    {
        firstLine = int.MaxValue;
        lastLine = 0;

        // Let's compute the number of newlines added/removed by the incoming changes.
        var delta = 0;

        foreach (var change in changes)
        {
            var newLineCount = change.NewText is null ? 0 : change.NewText.Split('\n').Length - 1;

            // For convenience, since we're already iterating through things, we also find the extremes
            // of the range of edits that were made.
            var range = text.GetLinePositionSpan(change.Span);
            firstLine = Math.Min(firstLine, range.Start.Line);
            lastLine = Math.Max(lastLine, range.End.Line);

            // The number of lines added/removed will be,
            // the number of lines added by the change  - the number of lines the change span represents
            delta += newLineCount - (range.End.Line - range.Start.Line);
        }

        return delta;
    }

    private static ImmutableArray<TextChange> CleanupDocument(FormattingContext context, LinePositionSpan spanAfterFormatting)
    {
        var text = context.SourceText;
        var csharpDocument = context.CodeDocument.GetCSharpDocument();

        using var changes = new PooledArrayBuilder<TextChange>();
        foreach (var mapping in csharpDocument.SourceMappings)
        {
            var mappingSpan = new TextSpan(mapping.OriginalSpan.AbsoluteIndex, mapping.OriginalSpan.Length);
            var mappingLinePositionSpan = text.GetLinePositionSpan(mappingSpan);
            if (!spanAfterFormatting.LineOverlapsWith(mappingLinePositionSpan))
            {
                // We don't care about this range. It didn't change.
                continue;
            }

            CleanupSourceMappingStart(context, mappingLinePositionSpan, ref changes.AsRef(), out var newLineAdded);

            CleanupSourceMappingEnd(context, mappingLinePositionSpan, ref changes.AsRef(), newLineAdded);
        }

        return changes.ToImmutable();
    }

    private static void CleanupSourceMappingStart(FormattingContext context, LinePositionSpan sourceMappingRange, ref PooledArrayBuilder<TextChange> changes, out bool newLineAdded)
    {
        newLineAdded = false;

        //
        // We look through every source mapping that intersects with the affected range and
        // bring the first line to its own line and adjust its indentation,
        //
        // E.g,
        //
        // @{   public int x = 0;
        // }
        //
        // becomes,
        //
        // @{
        //    public int x  = 0;
        // }
        //

        var text = context.SourceText;
        var sourceMappingSpan = text.GetTextSpan(sourceMappingRange);
        if (!ShouldFormat(context, sourceMappingSpan, allowImplicitStatements: false, out var owner))
        {
            // We don't want to run cleanup on this range.
            return;
        }

        if (owner is CSharpStatementLiteralSyntax &&
            owner.TryGetPreviousSibling(out var prevNode) &&
            prevNode.FirstAncestorOrSelf<RazorSyntaxNode>(static a => a is CSharpTemplateBlockSyntax) is { } template &&
            owner.SpanStart == template.Span.End &&
            IsOnSingleLine(template, text))
        {
            // Special case, we don't want to add a line break after a single line template
            return;
        }

        // Parent.Parent.Parent is because the tree is
        //  ExplicitExpression -> ExplicitExpressionBody -> CSharpCodeBlock -> CSharpExpressionLiteral
        if (owner is CSharpExpressionLiteralSyntax { Parent.Parent.Parent: CSharpExplicitExpressionSyntax explicitExpression } &&
            IsOnSingleLine(explicitExpression, text))
        {
            // Special case, we don't want to add line breaks inside a single line explicit expression (ie @( ... ))
            return;
        }

        if (sourceMappingRange.Start.Character == 0)
        {
            // It already starts on a fresh new line which doesn't need cleanup.
            // E.g, (The mapping starts at | in the below case)
            // @{
            //     @: Some html
            // |   var x = 123;
            // }
            //

            return;
        }

        // @{
        //     if (true)
        //     {
        //         <div></div>|
        //
        //              |}
        // }
        // We want to return the length of the range marked by |...|
        //
        if (!text.TryGetFirstNonWhitespaceOffset(sourceMappingSpan, out var whitespaceLength, out var newLineCount))
        {
            // There was no content after the start of this mapping. Meaning it already is clean.
            // E.g,
            // @{|
            //    ...
            // }

            return;
        }

        var spanToReplace = new TextSpan(sourceMappingSpan.Start, whitespaceLength);
        if (!context.TryGetIndentationLevel(spanToReplace.End, out var contentIndentLevel))
        {
            // Can't find the correct indentation for this content. Leave it alone.
            return;
        }

        if (newLineCount == 0)
        {
            newLineAdded = true;
            newLineCount = 1;
        }

        // At this point, `contentIndentLevel` should contain the correct indentation level for `}` in the above example.
        // Make sure to preserve the same number of blank lines as the original string had
        var replacement = PrependLines(context.GetIndentationLevelString(contentIndentLevel), context.NewLineString, newLineCount);

        // After the below change the above example should look like,
        // @{
        //     if (true)
        //     {
        //         <div></div>
        //     }
        // }
        var change = new TextChange(spanToReplace, replacement);
        changes.Add(change);
    }

    private static string PrependLines(string text, string newLine, int count)
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        builder.SetCapacityIfLarger((newLine.Length * count) + text.Length);

        for (var i = 0; i < count; i++)
        {
            builder.Append(newLine);
        }

        builder.Append(text);
        return builder.ToString();
    }

    private static void CleanupSourceMappingEnd(FormattingContext context, LinePositionSpan sourceMappingRange, ref PooledArrayBuilder<TextChange> changes, bool newLineWasAddedAtStart)
    {
        //
        // We look through every source mapping that intersects with the affected range and
        // bring the content after the last line to its own line and adjust its indentation,
        //
        // E.g,
        //
        // @{
        //     if (true)
        //     {  <div></div>
        //     }
        // }
        //
        // becomes,
        //
        // @{
        //    if (true)
        //    {
        //        </div></div>
        //    }
        // }
        //

        var text = context.SourceText;
        var sourceMappingSpan = text.GetTextSpan(sourceMappingRange);
        var mappingEndLineIndex = sourceMappingRange.End.Line;

        var indentations = context.GetIndentations();

        var startsInCSharpContext = indentations[mappingEndLineIndex].StartsInCSharpContext;

        // If the span is on a single line, and we added a line, then end point is now on a line that does start in a C# context.
        if (!startsInCSharpContext && newLineWasAddedAtStart && sourceMappingRange.Start.Line == mappingEndLineIndex)
        {
            startsInCSharpContext = true;
        }

        if (!startsInCSharpContext)
        {
            // For corner cases like (Position marked with |),
            // It is already in a separate line. It doesn't need cleaning up.
            // @{
            //     if (true}
            //     {
            //         |<div></div>
            //     }
            // }
            //
            return;
        }

        var endSpan = TextSpan.FromBounds(sourceMappingSpan.End, sourceMappingSpan.End);
        if (!ShouldFormat(context, endSpan, allowImplicitStatements: false, out var owner))
        {
            // We don't want to run cleanup on this range.
            return;
        }

        if (owner is CSharpStatementLiteralSyntax &&
            owner.NextSpan() is { } nextNode &&
            nextNode.FirstAncestorOrSelf<RazorSyntaxNode>(static a => a is CSharpTemplateBlockSyntax) is { } template &&
            template.SpanStart == owner.Span.End &&
            IsOnSingleLine(template, text))
        {
            // Special case, we don't want to add a line break in front of a single line template
            return;
        }

        if (owner is MarkupTagHelperAttributeSyntax { TagHelperAttributeInfo.Bound: true } or
            MarkupTagHelperDirectiveAttributeSyntax { TagHelperAttributeInfo.Bound: true } or
            MarkupMinimizedTagHelperAttributeSyntax { TagHelperAttributeInfo.Bound: true } or
            MarkupMinimizedTagHelperDirectiveAttributeSyntax { TagHelperAttributeInfo.Bound: true })
        {
            // Special case, we don't want to add a line break at the end of a component attribute. They are technically
            // C#, for features like GTD and FAR, but we consider them Html for formatting
            return;
        }

        var contentStartOffset = text.Lines[mappingEndLineIndex].GetFirstNonWhitespaceOffset(sourceMappingRange.End.Character);
        if (contentStartOffset is null)
        {
            // There is no content after the end of this source mapping. No need to clean up.
            return;
        }

        var spanToReplace = new TextSpan(sourceMappingSpan.End, 0);
        if (!context.TryGetIndentationLevel(spanToReplace.End, out var contentIndentLevel))
        {
            // Can't find the correct indentation for this content. Leave it alone.
            return;
        }

        // At this point, `contentIndentLevel` should contain the correct indentation level for `}` in the above example.
        var replacement = context.NewLineString + context.GetIndentationLevelString(contentIndentLevel);

        // After the below change the above example should look like,
        // @{
        //     if (true)
        //     {
        //         <div></div>
        //     }
        // }
        var change = new TextChange(spanToReplace, replacement);
        changes.Add(change);
    }

    private static bool IsOnSingleLine(RazorSyntaxNode node, SourceText text)
    {
        var linePositionSpan = text.GetLinePositionSpan(node.Span);

        return linePositionSpan.Start.Line == linePositionSpan.End.Line;
    }
}
