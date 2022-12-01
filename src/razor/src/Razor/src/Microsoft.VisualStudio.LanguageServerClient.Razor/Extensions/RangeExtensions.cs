// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

internal static class RangeExtensions
{
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

        var start = sourceText.Lines[range.Start.Line].Start + range.Start.Character;
        var end = sourceText.Lines[range.End.Line].Start + range.End.Character;
        return new TextSpan(start, end - start);
    }

    // Internal for testing only
    internal static readonly Range UndefinedRange = new()
    {
        Start = new Position(-1, -1),
        End = new Position(-1, -1)
    };

    public static bool IsUndefined(this Range range)
    {
        if (range is null)
        {
            throw new ArgumentNullException(nameof(range));
        }

        return range == UndefinedRange;
    }

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

    public static Range? Overlap(this Range range, Range other)
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
        if (overlapStart.CompareTo(overlapEnd) < 0)
        {
            return new Range()
            {
                Start = overlapStart,
                End = overlapEnd,
            };
        }

        return null;
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
}
