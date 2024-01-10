// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

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
}
