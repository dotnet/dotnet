// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    public static void Deconstruct(this Range range, out Position start, out Position end)
        => (start, end) = (range.Start, range.End);

    public static void Deconstruct(this Range range, out int startLine, out int startCharacter, out int endLine, out int endCharacter)
        => (startLine, startCharacter, endLine, endCharacter) = (range.Start.Line, range.Start.Character, range.End.Line, range.End.Character);

    public static LinePositionSpan ToLinePositionSpan(this Range range)
        => new(range.Start.ToLinePosition(), range.End.ToLinePosition());

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
        range.End.Line < other.Start.Line || (range.End.Line == other.Start.Line && range.End.Character < other.Start.Character);

    private static bool IsAfter(this Range range, Range other) =>
        other.End.Line < range.Start.Line || (other.End.Line == range.Start.Line && other.End.Character < range.Start.Character);

    public static bool OverlapsWith(this Range range, Range other)
    {
        return range.ToLinePositionSpan().OverlapsWith(other.ToLinePositionSpan());
    }

    public static bool LineOverlapsWith(this Range range, Range other)
    {
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

        return range.Start.CompareTo(other.Start) <= 0 && range.End.CompareTo(other.End) >= 0;
    }

    public static bool SpansMultipleLines(this Range range)
    {
        return range.Start.Line != range.End.Line;
    }

    public static bool IsSingleLine(this Range range)
    {
        return range.Start.Line == range.End.Line;
    }

    public static bool IsUndefined(this Range range)
    {
        return range == VsLspFactory.UndefinedRange;
    }

    public static bool IsZeroWidth(this Range range)
    {
        return range.Start == range.End;
    }

    public static int CompareTo(this Range range1, Range range2)
    {
        var result = range1.Start.CompareTo(range2.Start);

        if (result == 0)
        {
            result = range1.End.CompareTo(range2.End);
        }

        return result;
    }

    public static Range? Overlap(this Range range, Range other)
    {
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
        if (overlapStart.CompareTo(overlapEnd) < 0)
        {
            return VsLspFactory.CreateRange(overlapStart, overlapEnd);
        }

        return null;
    }

    public static string ToDisplayString(this Range range)
        => $"{range.Start.ToDisplayString()}-{range.End.ToDisplayString()}";
}
