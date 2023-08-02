// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class RangeExtensions
{
    public static readonly Range UndefinedRange = new()
    {
        Start = new Position(-1, -1),
        End = new Position(-1, -1)
    };

    public static bool IntersectsOrTouches(this Range range, Range other)
    {
        if (range.IsBefore(other))
        {
            return false;
        }

        if (range.IsAfter(other))
        {
            return false;
        }

        return true;
    }

    private static bool IsBefore(this Range range, Range other) =>
        range.End.Line < other.Start.Line || range.End.Line == other.Start.Line && range.End.Character < other.Start.Character;

    private static bool IsAfter(this Range range, Range other) =>
        other.End.Line < range.Start.Line || other.End.Line == range.Start.Line && other.End.Character < range.Start.Character;

    public static bool OverlapsWith(this Range range, Range other)
    {
        if (range is null)
        {
            throw new ArgumentNullException(nameof(range));
        }

        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var overlapStart = range.Start;
        if (range.Start.CompareTo(other.Start) < 0)
        {
            overlapStart = other.Start;
        }

        var overlapEnd = range.End;
        if (range.End.CompareTo(other.End) > 0)
        {
            overlapEnd = other.End;
        }

        // Empty ranges do not overlap with any range.
        return overlapStart.CompareTo(overlapEnd) < 0;
    }

    public static bool LineOverlapsWith(this Range range, Range other)
    {
        if (range is null)
        {
            throw new ArgumentNullException(nameof(range));
        }

        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var overlapStart = range.Start.Line;
        if (range.Start.Line.CompareTo(other.Start.Line) < 0)
        {
            overlapStart = other.Start.Line;
        }

        var overlapEnd = range.End.Line;
        if (range.End.Line.CompareTo(other.End.Line) > 0)
        {
            overlapEnd = other.End.Line;
        }

        return overlapStart.CompareTo(overlapEnd) <= 0;
    }

    public static bool Contains(this Range range, Range other)
    {
        if (range is null)
        {
            throw new ArgumentNullException(nameof(range));
        }

        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        return range.Start.CompareTo(other.Start) <= 0 && range.End.CompareTo(other.End) >= 0;
    }

    public static bool SpansMultipleLines(this Range range)
    {
        if (range is null)
        {
            throw new ArgumentNullException(nameof(range));
        }

        return range.Start.Line != range.End.Line;
    }

    public static TextSpan AsTextSpan(this Range range, SourceText sourceText)
    {
        if (range is null)
        {
            throw new ArgumentNullException(nameof(range));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        if (range.Start.Line >= sourceText.Lines.Count)
        {
            throw new ArgumentOutOfRangeException($"Range start line {range.Start.Line} matches or exceeds SourceText boundary {sourceText.Lines.Count}.");
        }

        if (range.End.Line >= sourceText.Lines.Count)
        {
            throw new ArgumentOutOfRangeException($"Range end line {range.End.Line} matches or exceeds SourceText boundary {sourceText.Lines.Count}.");
        }

        var start = sourceText.Lines[range.Start.Line].Start + range.Start.Character;
        var end = sourceText.Lines[range.End.Line].Start + range.End.Character;

        var length = end - start;
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException($"{range} resolved to zero or negative length.");
        }

        return new TextSpan(start, length);
    }

    public static Language.Syntax.TextSpan AsRazorTextSpan(this Range range, SourceText sourceText)
    {
        if (range is null)
        {
            throw new ArgumentNullException(nameof(range));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        if (range.Start.Line >= sourceText.Lines.Count)
        {
            throw new ArgumentOutOfRangeException($"Range start line {range.Start.Line} matches or exceeds SourceText boundary {sourceText.Lines.Count}.");
        }

        if (range.End.Line >= sourceText.Lines.Count)
        {
            throw new ArgumentOutOfRangeException($"Range end line {range.End.Line} matches or exceeds SourceText boundary {sourceText.Lines.Count}.");
        }

        var start = sourceText.Lines[range.Start.Line].Start + range.Start.Character;
        var end = sourceText.Lines[range.End.Line].Start + range.End.Character;

        var length = end - start;
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException($"{range} resolved to zero or negative length.");
        }

        return new Language.Syntax.TextSpan(start, length);
    }

    public static bool IsUndefined(this Range range)
    {
        if (range is null)
        {
            throw new ArgumentNullException(nameof(range));
        }

        return range == UndefinedRange;
    }
}
