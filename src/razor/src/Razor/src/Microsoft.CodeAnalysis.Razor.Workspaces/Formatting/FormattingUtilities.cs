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
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal static class FormattingUtilities
{
    public const string Indent = "$$Indent$$";
    public const string InitialIndent = "$$InitialIndent$$";

    /// <summary>
    ///  Adds indenting to the method.
    /// </summary>
    /// <param name="method">
    ///  The method to add indenting to. The method should be marked with <see cref="Indent"/> where an indent is wanted
    /// </param>
    /// <param name="tabSize">
    ///  The indentation size
    /// </param>
    /// <param name="insertSpaces">
    ///  Use spaces for indentation.
    /// </param>
    /// <param name="startingIndent">
    ///  The size of the any existing indent.
    /// </param>
    /// <returns>The indented method.</returns>
    public static string AddIndentationToMethod(string method, int tabSize, bool insertSpaces, int startingIndent)
    {
        var initialIndent = GetIndentationString(startingIndent, insertSpaces, tabSize);
        var indent = GetIndentationString(tabSize, insertSpaces, tabSize);
        return method.Replace(InitialIndent, initialIndent).Replace(Indent, indent);
    }

    /// <summary>
    ///  Adds indenting to the method.
    /// </summary>
    /// <param name="method">
    ///  The method to add indenting to. The method should be marked with <see cref="Indent"/> where an indent is wanted
    ///  and <see cref="InitialIndent"/> where some initial indent is needed.
    /// </param>
    /// <param name="tabSize">
    ///  The indentation size
    /// </param>
    /// <param name="insertSpaces">
    /// Use spaces for indentation.
    /// </param>
    /// <param name="startAbsoluteIndex">
    ///  The absolute index of the beginning of the class in the C# file the method will be added to.
    /// </param>
    /// <param name="numCharacterBefore">
    ///  The number of characters on the line before where startAbsoluteIndex is in the source.
    /// </param>
    /// <param name="source">
    ///  The contents of the C# file.
    /// </param>
    /// <returns>The indented method.</returns>
    public static string AddIndentationToMethod(string method, int tabSize, bool insertSpaces, int startAbsoluteIndex, int numCharacterBefore, SourceText source)
    {
        var startingIndent = 0;
        for (var i = 1; i <= numCharacterBefore; i++)
        {
            if (source[startAbsoluteIndex - i] == '\t')
            {
                startingIndent += tabSize;
            }
            else
            {
                startingIndent++;
            }
        }

        return AddIndentationToMethod(method, tabSize, insertSpaces, startingIndent);
    }

    /// <summary>
    ///  Adds indenting to the method.
    /// </summary>
    /// <param name="method">
    ///  The method to add indenting to. The method should be marked with <see cref="Indent"/> where an indent is wanted
    ///  and <see cref="InitialIndent"/> where some initial indent is needed.
    /// </param>
    /// <param name="tabSize">
    ///  The indentation size
    /// </param>
    /// <param name="insertSpaces"></param>
    /// <param name="startAbsoluteIndex">
    ///  The absolute index of the beginning of the code block in the Razor file where the method will be added to.
    /// </param>
    /// <param name="numCharacterBefore">
    ///  The number of characters on the line before where startAbsoluteIndex is in the source.
    /// </param>
    /// <param name="source">
    ///  The <see cref="RazorSourceDocument"/> of the razor file the method is being added to.
    /// </param>
    /// <returns>The indented method.</returns>
    public static string AddIndentationToMethod(string method, int tabSize, bool insertSpaces, int startAbsoluteIndex, int numCharacterBefore, RazorSourceDocument source)
    {
        var startingIndent = 0;
        for (var i = 1; i <= numCharacterBefore; i++)
        {
            if (source.Text[startAbsoluteIndex - i] == '\t')
            {
                startingIndent += tabSize;
            }
            else
            {
                startingIndent++;
            }
        }

        return AddIndentationToMethod(method, tabSize, insertSpaces, startingIndent);
    }

    /// <summary>
    /// Counts the number of non-whitespace characters in a given span of text.
    /// </summary>
    /// <param name="text">The source text</param>
    /// <param name="start">Inclusive position for where to start counting</param>
    /// <param name="endExclusive">Exclusive for where to stop counting</param>
    public static int CountNonWhitespaceChars(SourceText text, int start, int endExclusive)
    {
        var count = 0;
        for (var i = start; i < endExclusive; i++)
        {
            if (!char.IsWhiteSpace(text[i]))
            {
                count++;
            }
        }

        return count;
    }

    public static int GetIndentationLevel(TextLine line, int firstNonWhitespaceCharacterPosition, bool insertSpaces, int tabSize, out string additionalIndentation)
    {
        if (firstNonWhitespaceCharacterPosition > line.End)
        {
            throw new ArgumentOutOfRangeException(nameof(firstNonWhitespaceCharacterPosition), "The first non-whitespace character position must be within the line.");
        }

        // For spaces, the actual indentation needs to be divided by the tab size to get the level, and additional is the remainder
        var currentIndentationWidth = firstNonWhitespaceCharacterPosition - line.Start;
        if (insertSpaces)
        {
            return GetIndentationLevel(currentIndentationWidth, tabSize, out additionalIndentation);
        }

        // For tabs, we just count the tabs, and additional is any spaces at the end.
        var tabCount = 0;
        var text = line.Text.AssumeNotNull();
        for (var i = line.Start; i < firstNonWhitespaceCharacterPosition; i++)
        {
            if (text[i] == '\t')
            {
                tabCount++;
            }
            else
            {
                Debug.Assert(text[i] == ' ');
                additionalIndentation = text.ToString(TextSpan.FromBounds(i, firstNonWhitespaceCharacterPosition));
                return tabCount;
            }
        }

        additionalIndentation = "";
        return tabCount;
    }

    public static int GetIndentationLevel(int length, int tabSize, out string additionalIndentation)
    {
        var indentationLevel = length / tabSize;
        var additionalIndentationLength = length % tabSize;
        additionalIndentation = additionalIndentationLength == 0
            ? ""
            : new string(' ', additionalIndentationLength);
        return indentationLevel;
    }

    /// <summary>
    /// Given a <paramref name="indentation"/> amount of characters, generate a string representing the configured indentation.
    /// </summary>
    /// <param name="indentation">An amount of characters to represent the indentation.</param>
    /// <param name="insertSpaces">Whether spaces are used for indentation.</param>
    /// <param name="tabSize">The size of a tab and indentation.</param>
    /// <returns>A whitespace string representation indentation.</returns>
    public static string GetIndentationString(int indentation, bool insertSpaces, int tabSize)
    {
        if (insertSpaces)
        {
            return new string(' ', indentation);
        }

        var tabs = indentation / tabSize;
        var tabPrefix = new string('\t', tabs);

        var spaces = indentation % tabSize;
        var spaceSuffix = new string(' ', spaces);

        var combined = string.Concat(tabPrefix, spaceSuffix);
        return combined;
    }

    /// <summary>
    /// Unindents a span of text with a few caveats:
    ///
    /// 1. This assumes consistency in tabs/spaces for starting whitespace per line
    /// 2. This doesn't format the text, just attempts to remove leading whitespace in a uniform way
    /// 3. It will never remove non-whitespace
    ///
    /// This was made with extracting code into a new file in mind because it's not trivial to format that text and make
    /// sure the indentation is right. Use with caution.
    /// </summary>
    public static void NaivelyUnindentSubstring(SourceText text, TextSpan extractionSpan, System.Text.StringBuilder builder)
    {
        var extractedText = text.ToString(extractionSpan);
        var range = text.GetRange(extractionSpan);
        if (range.Start.Line == range.End.Line)
        {
            builder.Append(extractedText);
            return;
        }

        var extractedTextSpan = extractedText.AsSpan();
        var indentation = GetNormalizedIndentation(text, extractionSpan);

        foreach (var lineRange in GetLineRanges(extractedText))
        {
            var lineSpan = extractedTextSpan[lineRange];
            lineSpan = UnindentLine(lineSpan, indentation);

            foreach (var c in lineSpan)
            {
                builder.Append(c);
            }
        }

        //
        // Local Methods
        //

        static ReadOnlySpan<char> UnindentLine(ReadOnlySpan<char> line, int indentation)
        {
            var startCharacter = 0;
            while (startCharacter < indentation && IsWhitespace(line[startCharacter]))
            {
                startCharacter++;
            }

            return line[startCharacter..];
        }

        // Gets the smallest indentation of all the lines in a given span
        static int GetNormalizedIndentation(SourceText sourceText, TextSpan span)
        {
            var indentation = int.MaxValue;
            foreach (var line in sourceText.Lines)
            {
                if (!span.OverlapsWith(line.Span))
                {
                    continue;
                }

                indentation = Math.Min(indentation, GetIndentation(line));
            }

            return indentation;
        }

        static int GetIndentation(TextLine line)
        {
            if (line.Text is null)
            {
                return 0;
            }

            var indentation = 0;
            for (var position = line.Span.Start; position < line.Span.End; position++)
            {
                var c = line.Text[position];
                if (!IsWhitespace(c))
                {
                    break;
                }

                indentation++;
            }

            return indentation;
        }

        static bool IsWhitespace(char c)
            => c == ' ' || c == '\t';

        static ImmutableArray<Range> GetLineRanges(string text)
        {
            using var builder = new PooledArrayBuilder<Range>();
            var start = 0;
            var end = text.IndexOf('\n');
            while (true)
            {
                if (end == -1)
                {
                    builder.Add(new(start, text.Length));
                    break;
                }

                // end + 1 to include the new line
                builder.Add(new(start, end + 1));
                start = end + 1;
                if (start == text.Length)
                {
                    break;
                }

                end = text.IndexOf('\n', start);
            }

            return builder.ToImmutableAndClear();
        }
    }

    /// <summary>
    /// Sometimes the Html language server will send back an edit that contains a tilde, because the generated
    /// document we send them has lots of tildes. In those cases, we need to do some extra work to compute the
    /// minimal text edits
    /// </summary>
    public static TextEdit[] FixHtmlTextEdits(SourceText htmlSourceText, TextEdit[] edits)
    {
        // Avoid computing a minimal diff if we don't need to
        if (!edits.Any(static e => e.NewText.Contains("~")))
            return edits;

        var changes = edits.SelectAsArray(htmlSourceText.GetTextChange);

        var fixedChanges = htmlSourceText.MinimizeTextChanges(changes);
        return [.. fixedChanges.Select(htmlSourceText.GetTextEdit)];
    }

    internal static SumType<TextEdit, AnnotatedTextEdit>[] FixHtmlTextEdits(SourceText htmlSourceText, SumType<TextEdit, AnnotatedTextEdit>[] edits)
    {
        // Avoid computing a minimal diff if we don't need to
        if (!edits.Any(static e => ((TextEdit)e).NewText.Contains("~")))
            return edits;

        var changes = edits.SelectAsArray(e => htmlSourceText.GetTextChange((TextEdit)e));

        var fixedChanges = htmlSourceText.MinimizeTextChanges(changes);
        return [.. fixedChanges.Select(htmlSourceText.GetTextEdit)];
    }

    public static void GetOriginalDocumentChangesFromLineInfo(FormattingContext context, SourceText originalText, ImmutableArray<LineInfo> formattedLineInfo, SourceText formattedText, ILogger logger, Func<int, bool>? shouldKeepInsertedNewlineAtPosition, ref PooledArrayBuilder<TextChange> formattingChanges, out int lastFormattedTextLine)
    {
        var iFormatted = 0;
        for (var iOriginal = 0; iOriginal < originalText.Lines.Count; iOriginal++, iFormatted++)
        {
            var lineInfo = formattedLineInfo[iOriginal];

            if (lineInfo.SkipPreviousLine)
            {
                iFormatted++;
            }

            if (iFormatted >= formattedText.Lines.Count)
            {
                break;
            }

            string? indentationString = null;

            var formattedLine = formattedText.Lines[iFormatted];
            if (formattedLine.GetFirstNonWhitespaceOffset() is { } formattedIndentation)
            {
                var originalLine = originalText.Lines[iOriginal];
                var originalLineOffset = originalLine.GetFirstNonWhitespaceOffset().GetValueOrDefault();
                var fixedIndentString = context.GetIndentationLevelString(lineInfo.FixedIndentLevel);

                if (lineInfo.ProcessIndentation)
                {
                    Debug.Assert(originalLine.GetFirstNonWhitespaceOffset().HasValue);

                    // First up, we take the indentation from the formatted file, and add on the fixed indentation level from the line info, and
                    // replace whatever was in the original file with it.
                    indentationString = formattedText.ToString(new TextSpan(formattedLine.Start, formattedIndentation))
                        + fixedIndentString
                        + lineInfo.AdditionalIndentation;
                    formattingChanges.Add(new TextChange(new TextSpan(originalLine.Start, originalLineOffset), indentationString));
                }

                // Now we handle the formatting, which is changes to the right of the first non-whitespace character.
                if (lineInfo.ProcessFormatting)
                {
                    // The offset and length properties of the line info are relative to the indented content in their respective documents.
                    // In other words, relative to the first non-whitespace character on the line.
                    var originalStart = originalLine.Start + originalLineOffset + lineInfo.OriginOffset;
                    var length = lineInfo.FormattedLength == 0
                        ? originalLine.End - originalStart
                        : lineInfo.FormattedLength;
                    var formattedStart = formattedLine.Start + formattedIndentation + lineInfo.FormattedOffset;
                    var formattedEnd = formattedLine.End - lineInfo.FormattedOffsetFromEndOfLine;
                    if (formattedEnd > formattedStart)
                    {
                        formattingChanges.Add(new TextChange(new TextSpan(originalStart, length), formattedText.ToString(TextSpan.FromBounds(formattedStart, formattedEnd))));
                    }

                    if (lineInfo.CheckForNewLines)
                    {
                        Debug.Assert(lineInfo.FormattedLength == 0, "Can't have a FormattedLength if we're looking for new lines. The logic is incompatible.");
                        Debug.Assert(lineInfo.FormattedOffsetFromEndOfLine == 0, "Can't have a FormattedOffsetFromEndOfLine if we're looking for new lines. The logic is incompatible.");

                        ConsumeNewLines(
                            context, originalText, formattedText, logger, shouldKeepInsertedNewlineAtPosition,
                            ref formattingChanges, ref iOriginal, ref iFormatted, ref originalLine, ref formattedLine,
                            originalStart, formattedStart, fixedIndentString);
                    }

                    // The above "CheckForNewLines" means new lines inserted in the middle of a line of the original text, but
                    // the formatter may have inserted a blank line after the current line too. In that case we need to make sure
                    // we advance the formatted line pointer past it, but also include it. This only applies if the line after the
                    // blank line matches the next original line and the next original line isn't blank (ie, an actual insertion)
                    if (iFormatted + 1 < formattedText.Lines.Count &&
                        formattedText.Lines[iFormatted + 1].Span.Length == 0 &&
                        iOriginal + 1 < originalText.Lines.Count &&
                        originalText.Lines[iOriginal + 1] is { } nextOriginalLine &&
                        nextOriginalLine.Span.Length != 0)
                    {
                        // Next line is blank, and next original line isn't. Now we check the line after next
                        if (iFormatted + 2 < formattedText.Lines.Count)
                        {
                            var lineAfterNext = formattedText.Lines[iFormatted + 2];
                            if (originalText.NonWhitespaceContentEquals(formattedText, nextOriginalLine.Start, nextOriginalLine.End, lineAfterNext.Start, lineAfterNext.End))
                            {
                                // Next line is blank, and line after next matches the next original line, so we skip the blank line
                                iFormatted++;
                                formattingChanges.Add(new TextChange(TextSpan.FromBounds(originalLine.End, originalLine.End), context.NewLineString));
                            }
                        }
                    }
                }
            }

            if (lineInfo.SkipNextLine)
            {
                iFormatted++;
            }
            else if (lineInfo.SkipNextLineIfBrace)
            {
                // If the next line is a brace, we skip it. This is used to skip the opening brace of a class
                // that we insert, but Roslyn settings might place on the same line as the class declaration,
                // or skip the opening brace of a lambda definition we insert, but Roslyn might place it on the
                // next line. In that case, we can't place it on the next line ourselves because Roslyn doesn't
                // adjust the indentation of opening braces of lambdas in that scenario.
                if (NextLineIsOnlyAnOpenBrace(formattedText, iFormatted))
                {
                    iFormatted++;
                }

                // On the other hand, we might insert the opening brace of a class, and Roslyn might collapse
                // it up to the previous line, so we would want to skip the next line in the original document
                // in that case. Fortunately its illegal to have `@code {\r\n {` in a Razor file, so there can't
                // be false positives here.
                if (NextLineIsOnlyAnOpenBrace(originalText, iOriginal))
                {
                    iOriginal++;

                    // We're skipping a line in the original document, because Roslyn brought it up to the previous
                    // line, but the fact is the opening brace was in the original document, and might need its indentation
                    // adjusted. Since we can't reason about this line in any way, because Roslyn has changed it, we just
                    // apply the indentation from the previous line.
                    //
                    // If we didn't adjust the indentation of the previous line, then we really have no information to go
                    // on at all, so hopefully the user is happy with where their open brace is.
                    if (indentationString is not null)
                    {
                        var originalLine = originalText.Lines[iOriginal];
                        var originalLineOffset = originalLine.GetFirstNonWhitespaceOffset().GetValueOrDefault();
                        formattingChanges.Add(new TextChange(new TextSpan(originalLine.Start, originalLineOffset), indentationString));
                    }
                }
            }
        }

        lastFormattedTextLine = iFormatted;
    }

    private static bool NextLineIsOnlyAnOpenBrace(SourceText text, int lineNumber)
        => lineNumber + 1 < text.Lines.Count &&
            text.Lines[lineNumber + 1] is { Span.Length: > 0 } nextLine &&
            nextLine.GetFirstNonWhitespaceOffset() is { } firstNonWhitespace &&
            nextLine.Start + firstNonWhitespace == nextLine.End - 1 &&
            nextLine.CharAt(firstNonWhitespace) == '{';

    /// <summary>
    /// Handles the case where the external formatter has changed the number of lines by inserting or removing newlines.
    /// The primary side (formatted lines when inserting, original lines when removing) is consumed first, and then
    /// the secondary side is consumed to keep content aligned if the formatter wrapped at a different point.
    /// </summary>
    private static void ConsumeNewLines(
        FormattingContext context,
        SourceText originalText,
        SourceText formattedText,
        ILogger logger,
        Func<int, bool>? shouldKeepInsertedNewlineAtPosition,
        ref PooledArrayBuilder<TextChange> formattingChanges,
        ref int iOriginal,
        ref int iFormatted,
        ref TextLine originalLine,
        ref TextLine formattedLine,
        int originalStart,
        int formattedStart,
        string fixedIndentString)
    {
        // We assume the external formatter won't change anything but whitespace, so we can just apply the
        // changes directly, but it could very well be adding whitespace in the form of newlines, for example
        // taking "if (true) {" and making it run over two lines, or even "string Prop { get" and making it
        // span three lines. Since we assume it won't change anything non-whitespace, we just keep inserting
        // the formatted lines of C# until we match the original line contents.
        // Of course, the formatter could just as easily remove whitespace, eg making a "class Goo\n{" into
        // "class Goo {", so whilst the same theory applies, instead of inserting formatted lines, we eat
        // the original lines.

        var originalNonWhitespace = CountNonWhitespaceChars(originalText, originalStart, originalLine.End);
        var formattedNonWhitespace = CountNonWhitespaceChars(formattedText, formattedStart, formattedLine.End);

        if (originalNonWhitespace == formattedNonWhitespace)
        {
            return;
        }

        var formatterInsertedNewLines = originalNonWhitespace > formattedNonWhitespace;

        // Before we start skipping formatted lines, we need the info to work out where exactly the newline is being added
        var originalPosition = originalStart + (formattedLine.End - formattedStart);
        var consumedFromSecondarySide = false;

        while (!originalText.NonWhitespaceContentEquals(formattedText, originalStart, originalLine.End, formattedStart, formattedLine.End))
        {
            // Consume from the primary side: formatted lines if the formatter inserted newlines, original lines if it removed them.
            var didAdvance = formatterInsertedNewLines
                ? TryAdvanceLine(context, logger, "formatted", formattedText, ref iFormatted, ref formattedLine, iOriginal, originalText.Lines.Count)
                : TryAdvanceLine(context, logger, "original", originalText, ref iOriginal, ref originalLine, iFormatted, formattedText.Lines.Count);

            if (!didAdvance)
            {
                break;
            }

            // After consuming from the primary side, the other side's content may now be insufficient
            // (e.g., the formatter wrapped at a different point). Consume from the secondary side to keep aligned.
            var originalNonWS = CountNonWhitespaceChars(originalText, originalStart, originalLine.End);
            var formattedNonWS = CountNonWhitespaceChars(formattedText, formattedStart, formattedLine.End);
            var secondaryNeedsAdvancing = formatterInsertedNewLines
                ? originalNonWS < formattedNonWS
                : originalNonWS > formattedNonWS;

            while (secondaryNeedsAdvancing)
            {
                didAdvance = formatterInsertedNewLines
                    ? TryAdvanceLine(context, logger, "original", originalText, ref iOriginal, ref originalLine, iFormatted, formattedText.Lines.Count)
                    : TryAdvanceLine(context, logger, "formatted", formattedText, ref iFormatted, ref formattedLine, iOriginal, originalText.Lines.Count);

                if (!didAdvance)
                {
                    break;
                }

                consumedFromSecondarySide = true;

                originalNonWS = CountNonWhitespaceChars(originalText, originalStart, originalLine.End);
                formattedNonWS = CountNonWhitespaceChars(formattedText, formattedStart, formattedLine.End);
                secondaryNeedsAdvancing = formatterInsertedNewLines
                    ? originalNonWS < formattedNonWS
                    : originalNonWS > formattedNonWS;
            }

            // When we haven't consumed from the secondary side, the formatter purely added or removed lines,
            // so we emit per-line text changes.
            if (!consumedFromSecondarySide)
            {
                if (formatterInsertedNewLines)
                {
                    // The current line has been split into multiple lines, but its up to whoever called us to decide if we're keeping that.
                    if (shouldKeepInsertedNewlineAtPosition?.Invoke(originalPosition) ?? true)
                    {
                        // If we're keeping it, we insert this newline after the original line, with the correct indentation.
                        formattingChanges.Add(new TextChange(new(originalLine.EndIncludingLineBreak, 0), fixedIndentString + formattedText.ToString(formattedLine.SpanIncludingLineBreak)));
                    }
                    else
                    {
                        // If we're not keeping the newline, we need to restore this line back to the original line it came from
                        formattingChanges.Add(new TextChange(new(originalLine.End, 0), formattedText.ToString(formattedLine.Span)));
                    }
                }
                else
                {
                    // The formatter has removed newlines, so we need to remove the original lines.
                    formattingChanges.Add(new TextChange(TextSpan.FromBounds(originalText.Lines[iOriginal - 1].End, originalLine.End), ""));
                }
            }
        }

        if (consumedFromSecondarySide)
        {
            // The formatter re-wrapped content at a different point, consuming lines from both sides.
            // Update the formatting change to cover the full range of consumed original and formatted lines.
            formattingChanges[formattingChanges.Count - 1] = new TextChange(
                TextSpan.FromBounds(originalStart, originalLine.End),
                formattedText.ToString(TextSpan.FromBounds(formattedStart, formattedLine.End)));
        }
    }

    private static bool TryAdvanceLine(
        FormattingContext context,
        ILogger logger,
        string label,
        SourceText text,
        ref int lineIndex,
        ref TextLine line,
        int otherLineIndex,
        int otherLineCount)
    {
        lineIndex++;
        if (lineIndex >= text.Lines.Count)
        {
            context.Logger?.LogMessage($"Ran out of {label} lines at index {lineIndex} of {text.Lines.Count} (other side: {otherLineIndex} of {otherLineCount})");
            logger.LogError($"Ran out of {label} lines while trying to process formatted changes. Abandoning further formatting to not corrupt the source file, please report this issue.");
            return false;
        }

        line = text.Lines[lineIndex];
        return true;
    }
}
