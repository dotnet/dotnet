// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class SourceTextExtensions
{
    /// <summary>
    /// Gets the minimal range of text that changed between the two versions.
    /// </summary>
    public static TextChangeRange GetEncompassingTextChangeRange(this SourceText newText, SourceText oldText)
    {
        if (newText is null)
        {
            throw new ArgumentNullException(nameof(newText));
        }

        if (oldText is null)
        {
            throw new ArgumentNullException(nameof(oldText));
        }

        var ranges = newText.GetChangeRanges(oldText);
        if (ranges.Count == 0)
        {
            return default;
        }

        // simple case.
        if (ranges.Count == 1)
        {
            return ranges[0];
        }

        return TextChangeRange.Collapse(ranges);
    }

    public static void GetLineAndOffset(this SourceText source, int position, out int lineNumber, out int offset)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var line = source.Lines.GetLineFromPosition(position);

        lineNumber = line.LineNumber;
        offset = position - line.Start;
    }

    public static void GetLinesAndOffsets(
        this SourceText source,
        TextSpan textSpan,
        out int startLineNumber,
        out int startOffset,
        out int endLineNumber,
        out int endOffset)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        source.GetLineAndOffset(textSpan.Start, out startLineNumber, out startOffset);
        source.GetLineAndOffset(textSpan.End, out endLineNumber, out endOffset);
    }

    public static void GetLinesAndOffsets(
        this SourceText source,
        SourceSpan sourceSpan,
        out int startLineNumber,
        out int startOffset,
        out int endLineNumber,
        out int endOffset)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        source.GetLineAndOffset(sourceSpan.AbsoluteIndex, out startLineNumber, out startOffset);
        source.GetLineAndOffset(sourceSpan.AbsoluteIndex + sourceSpan.Length, out endLineNumber, out endOffset);
    }

    public static string GetSubTextString(this SourceText source, TextSpan span)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var charBuffer = new char[span.Length];
        source.CopyTo(span.Start, charBuffer, 0, span.Length);
        return new string(charBuffer);
    }

    public static bool NonWhitespaceContentEquals(this SourceText source, SourceText other)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var i = 0;
        var j = 0;
        while (i < source.Length && j < other.Length)
        {
            if (char.IsWhiteSpace(source[i]))
            {
                i++;
                continue;
            }
            else if (char.IsWhiteSpace(other[j]))
            {
                j++;
                continue;
            }
            else if (source[i] != other[j])
            {
                return false;
            }

            i++;
            j++;
        }

        while (i < source.Length && char.IsWhiteSpace(source[i]))
        {
            i++;
        }

        while (j < other.Length && char.IsWhiteSpace(other[j]))
        {
            j++;
        }

        return i == source.Length && j == other.Length;
    }

    public static int? GetFirstNonWhitespaceOffset(this SourceText source, TextSpan? span, out int newLineCount)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        span ??= new TextSpan(0, source.Length);
        newLineCount = 0;

        for (var i = span.Value.Start; i < span.Value.End; i++)
        {
            if (!char.IsWhiteSpace(source[i]))
            {
                return i - span.Value.Start;
            }
            else if (source[i] == '\n')
            {
                newLineCount++;
            }
        }

        return null;
    }

    // Given the source text and the current span, we start at the ending span location and iterate towards the start
    // until we've reached a non-whitespace character.
    // For instance "  abcdef  " would have a last non-whitespace offset of 7 to correspond to the charcter 'f'.
    public static int? GetLastNonWhitespaceOffset(this SourceText source, TextSpan? span, out int newLineCount)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        span ??= new TextSpan(0, source.Length);
        newLineCount = 0;

        // If the span is at the end of the document it's common for the "End" to represent 1 past the end of the source
        var indexableSpanEnd = Math.Min(span.Value.End, source.Length - 1);

        for (var i = indexableSpanEnd; i >= span.Value.Start; i--)
        {
            if (!char.IsWhiteSpace(source[i]))
            {
                return i - span.Value.Start;
            }
            else if (source[i] == '\n')
            {
                newLineCount++;
            }
        }

        return null;
    }

    public static bool TryGetAbsoluteIndex(this SourceText sourceText, int line, int character, out int absoluteIndex)
    {
        return sourceText.TryGetAbsoluteIndex(line, character, logger: null, out absoluteIndex);
    }

    public static bool TryGetAbsoluteIndex(this SourceText sourceText, int line, int character, ILogger? logger, out int absoluteIndex)
    {
        absoluteIndex = 0;
        var lineCount = sourceText.Lines.Count;
        if (line > lineCount ||
            (line == lineCount && character > 0))
        {
            if (logger != null)
            {
#pragma warning disable CA2254 // Template should be a static expression.
                // This is actually static, the compiler just doesn't know it.
                var errorMessage = SR.FormatPositionLine_Outside_Range(line, nameof(sourceText), sourceText.Lines.Count);
                logger?.LogError(errorMessage);
#pragma warning restore CA2254 // Template should be a static expression
                Debug.Fail(SR.FormatPositionLine_Outside_Range(line, nameof(sourceText), sourceText.Lines.Count));
            }

            return false;
        }

        // LSP spec allowed a Range to end one line past the end, and character 0. SourceText does not, so we adjust to the final char position
        if (line == lineCount)
        {
            absoluteIndex = sourceText.Length;
        }
        else
        {
            var sourceLine = sourceText.Lines[line];
            var lineLengthIncludingLineBreak = sourceLine.SpanIncludingLineBreak.Length;
            if (character > lineLengthIncludingLineBreak)
            {
                if (logger != null)
                {
#pragma warning disable CA2254 // Template should be a static expression.
                    // This is actually static, the compiler just doesn't know it.
                    var errorMessage = SR.FormatPositionCharacter_Outside_Range(character, nameof(sourceText), lineLengthIncludingLineBreak);
                    logger?.LogError(errorMessage);
                    Debug.Fail(errorMessage);
#pragma warning restore CA2254 // Template should be a static expression
                }

                return false;
            }

            absoluteIndex = sourceLine.Start + character;
        }

        return true;
    }

    public static int GetRequiredAbsoluteIndex(this SourceText sourceText, int line, int character, ILogger? logger = null)
    {
        if (!sourceText.TryGetAbsoluteIndex(line, character, logger, out var absolutePosition))
        {
            throw new ArgumentOutOfRangeException($"({line},{character}) matches or exceeds SourceText boundary {sourceText.Lines.Count}.");
        }

        return absolutePosition;
    }

    public static TextSpan GetTextSpan(this SourceText sourceText, int startLine, int startCharacter, int endLine, int endCharacter)
    {
        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        var start = GetAbsoluteIndex(startLine, startCharacter, sourceText, "Start");
        var end = GetAbsoluteIndex(endLine, endCharacter, sourceText, "End");

        var length = end - start;
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException($"({startLine},{startCharacter})-({endLine},{endCharacter}) resolved to zero or negative length.");
        }

        return new TextSpan(start, length);

        static int GetAbsoluteIndex(int line, int character, SourceText sourceText, string argName)
        {
            if (!sourceText.TryGetAbsoluteIndex(line, character, out var absolutePosition))
            {
                throw new ArgumentOutOfRangeException($"{argName} ({line},{character}) matches or exceeds SourceText boundary {sourceText.Lines.Count}.");
            }

            return absolutePosition;
        }
    }
}
