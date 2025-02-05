// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.TextDifferencing;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Text;

internal static class SourceTextExtensions
{
    /// <summary>
    /// Gets the minimal range of text that changed between the two versions.
    /// </summary>
    public static TextChangeRange GetEncompassingTextChangeRange(this SourceText newText, SourceText oldText)
    {
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

    public static LinePosition GetLinePosition(this SourceText text, int position)
        => text.Lines.GetLinePosition(position);

    public static LinePositionSpan GetLinePositionSpan(this SourceText text, TextSpan span)
        => text.Lines.GetLinePositionSpan(span);

    public static LinePositionSpan GetLinePositionSpan(this SourceText text, SourceSpan span)
        => text.GetLinePositionSpan(span.ToTextSpan());

    public static LinePositionSpan GetLinePositionSpan(this SourceText text, int start, int end)
        => text.GetLinePositionSpan(TextSpan.FromBounds(start, end));

    public static int GetPosition(this SourceText text, LinePosition position)
        => text.GetRequiredAbsoluteIndex(position);

    public static int GetPosition(this SourceText text, int line, int character)
        => text.GetPosition(new LinePosition(line, character));

    public static string GetSubTextString(this SourceText text, TextSpan span)
    {
        using var _ = ArrayPool<char>.Shared.GetPooledArray(span.Length, out var charBuffer);

        text.CopyTo(span.Start, charBuffer, 0, span.Length);
        return new string(charBuffer, 0, span.Length);
    }

    public static bool NonWhitespaceContentEquals(this SourceText text, SourceText other)
        => NonWhitespaceContentEquals(text, other, 0, text.Length, 0, other.Length);

    public static bool NonWhitespaceContentEquals(
        this SourceText text,
        SourceText other,
        int textStart,
        int textEnd,
        int otherStart,
        int otherEnd)
    {
        var i = textStart;
        var j = otherStart;
        while (i < textEnd && j < otherEnd)
        {
            if (char.IsWhiteSpace(text[i]))
            {
                i++;
                continue;
            }
            else if (char.IsWhiteSpace(other[j]))
            {
                j++;
                continue;
            }
            else if (text[i] != other[j])
            {
                return false;
            }

            i++;
            j++;
        }

        while (i < textEnd && char.IsWhiteSpace(text[i]))
        {
            i++;
        }

        while (j < otherEnd && char.IsWhiteSpace(other[j]))
        {
            j++;
        }

        return i == textEnd && j == otherEnd;
    }

    public static bool TryGetFirstNonWhitespaceOffset(this SourceText text, out int offset)
        => text.TryGetFirstNonWhitespaceOffset(new TextSpan(0, text.Length), out offset);

    public static bool TryGetFirstNonWhitespaceOffset(this SourceText text, TextSpan span, out int offset)
    {
        for (var i = span.Start; i < span.End; i++)
        {
            if (!char.IsWhiteSpace(text[i]))
            {
                offset = i - span.Start;
                return true;
            }
        }

        offset = -1;
        return false;
    }

    public static bool TryGetFirstNonWhitespaceOffset(this SourceText text, TextSpan span, out int offset, out int newLineCount)
    {
        newLineCount = 0;

        for (var i = span.Start; i < span.End; i++)
        {
            if (!char.IsWhiteSpace(text[i]))
            {
                offset = i - span.Start;
                return true;
            }
            else if (text[i] == '\n')
            {
                newLineCount++;
            }
        }

        offset = -1;
        newLineCount = -1;
        return false;
    }

    /// <summary>
    ///  <para>
    ///   Given the source text and the current span, we start at the ending span location and iterate towards the start
    ///   until we've reached a non-whitespace character.
    ///  </para>
    ///  <para>
    ///   For instance, "  abcdef  " would have a last non-whitespace offset of 7 to correspond to the character 'f'.
    ///  </para>
    /// </summary>
    public static bool TryGetLastNonWhitespaceOffset(this SourceText text, TextSpan span, out int offset)
    {
        // If the span is at the end of the document, it's common for the "End" to represent 1 past the end of the source
        var indexableSpanEnd = Math.Min(span.End, text.Length - 1);

        for (var i = indexableSpanEnd; i >= span.Start; i--)
        {
            if (!char.IsWhiteSpace(text[i]))
            {
                offset = i - span.Start;
                return true;
            }
        }

        offset = -1;
        return false;
    }

    public static bool IsValidPosition(this SourceText text, LinePosition lspPosition)
        => text.TryGetAbsoluteIndex(lspPosition, out _);

    public static bool IsValidPosition(this SourceText text, (int line, int character) lspPosition)
        => text.TryGetAbsoluteIndex(lspPosition, out _);

    public static bool IsValidPosition(this SourceText text, int line, int character)
        => text.TryGetAbsoluteIndex(line, character, out _);

    public static bool TryGetAbsoluteIndex(this SourceText text, LinePosition position, out int absoluteIndex)
        => text.TryGetAbsoluteIndex(position.Line, position.Character, out absoluteIndex);

    public static bool TryGetAbsoluteIndex(this SourceText text, (int line, int character) position, out int absoluteIndex)
        => text.TryGetAbsoluteIndex(position.line, position.character, out absoluteIndex);

    public static bool TryGetAbsoluteIndex(this SourceText text, int line, int character, out int absoluteIndex)
    {
        absoluteIndex = 0;

        if (character < 0)
        {
            return false;
        }

        var lineCount = text.Lines.Count;

        if (line > lineCount)
        {
            return false;
        }

        // The LSP spec allows the end of a range to be after the last line at character 0.
        // SourceText doesn't allow that, so we adjust to SourceText.Length.
        if (line == lineCount)
        {
            if (character == 0)
            {
                absoluteIndex = text.Length;
                return true;
            }

            return false;
        }

        var textLine = text.Lines[line];

        if (character > textLine.SpanIncludingLineBreak.Length)
        {
            return false;
        }

        absoluteIndex = textLine.Start + character;
        return true;
    }

    public static int GetRequiredAbsoluteIndex(this SourceText text, LinePosition position)
        => text.GetRequiredAbsoluteIndex(position.Line, position.Character);

    public static int GetRequiredAbsoluteIndex(this SourceText text, (int line, int character) position)
        => text.GetRequiredAbsoluteIndex(position.line, position.character);

    public static int GetRequiredAbsoluteIndex(this SourceText text, int line, int character)
        => text.TryGetAbsoluteIndex(line, character, out var absolutionPosition)
            ? absolutionPosition
            : ThrowHelper.ThrowInvalidOperationException<int>($"({line},{character}) matches or exceeds SourceText boundary {text.Lines.Count}.");

    public static TextSpan GetTextSpan(this SourceText text, LinePosition start, LinePosition end)
        => text.GetTextSpan(start.Line, start.Character, end.Line, end.Character);

    public static TextSpan GetTextSpan(this SourceText text, LinePositionSpan span)
        => text.GetTextSpan(span.Start, span.End);

    public static TextSpan GetTextSpan(this SourceText text, int startLine, int startCharacter, int endLine, int endCharacter)
    {
        var start = GetAbsoluteIndex(text, startLine, startCharacter, "Start");
        var end = GetAbsoluteIndex(text, endLine, endCharacter, "End");

        var length = end - start;
        if (length < 0)
        {
            return ThrowHelper.ThrowInvalidOperationException<TextSpan>($"({startLine},{startCharacter})-({endLine},{endCharacter}) resolved to zero or negative length.");
        }

        return new TextSpan(start, length);

        static int GetAbsoluteIndex(SourceText text, int line, int character, string name)
        {
            return text.TryGetAbsoluteIndex(line, character, out var absolutePosition)
                ? absolutePosition
                : ThrowHelper.ThrowInvalidOperationException<int>($"{name}: ({line},{character}) matches or exceeds SourceText boundary {text.Lines.Count}.");
        }
    }

    public static bool TryGetSourceLocation(this SourceText text, LinePosition position, out SourceLocation location)
        => text.TryGetSourceLocation(position.Line, position.Character, out location);

    public static bool TryGetSourceLocation(this SourceText text, int line, int character, out SourceLocation location)
    {
        if (text.TryGetAbsoluteIndex(line, character, out var absoluteIndex))
        {
            location = new SourceLocation(absoluteIndex, line, character);
            return true;
        }

        location = default;
        return false;
    }

    /// <summary>
    /// Applies the set of edits specified, and returns the minimal set needed to make the same changes
    /// </summary>
    public static ImmutableArray<TextChange> MinimizeTextChanges(this SourceText text, ImmutableArray<TextChange> changes)
        => MinimizeTextChanges(text, changes, out _);

    /// <summary>
    /// Applies the set of edits specified, and returns the minimal set needed to make the same changes
    /// </summary>
    public static ImmutableArray<TextChange> MinimizeTextChanges(this SourceText text, ImmutableArray<TextChange> changes, out SourceText originalTextWithChanges)
    {
        originalTextWithChanges = text.WithChanges(changes);

        if (text.ContentEquals(originalTextWithChanges))
        {
            return [];
        }

        return SourceTextDiffer.GetMinimalTextChanges(text, originalTextWithChanges, DiffKind.Char);
    }

    /// <summary>
    /// Determines if the given <see cref="SourceText"/> has more LF line endings ('\n') than CRLF line endings ('\r\n').
    /// </summary>
    /// <param name="text">The <see cref="SourceText"/> to examine.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="SourceText"/> is deemed to use LF line endings; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasLFLineEndings(this SourceText text)
    {
        var crlfCount = 0;
        var lfCount = 0;

        foreach (var line in text.Lines)
        {
            var lineBreakSpan = TextSpan.FromBounds(line.End, line.EndIncludingLineBreak);
            var lineBreak = line.Text?.ToString(lineBreakSpan) ?? string.Empty;
            if (lineBreak == "\r\n")
            {
                crlfCount++;
            }
            else if (lineBreak == "\n")
            {
                lfCount++;
            }
        }

        return lfCount > crlfCount;
    }

    public static ImmutableArray<TextChange> GetTextChangesArray(this SourceText newText, SourceText oldText)
    {
        var list = newText.GetTextChanges(oldText);

        // Fast path for the common case. The base SourceText.GetTextChanges method returns an ImmutableArray
        if (list is ImmutableArray<TextChange> array)
        {
            return array;
        }

        return list.ToImmutableArray();
    }
}
