// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class LinePositionSpanExtensions
{
    public static Range ToRange(this LinePositionSpan linePositionSpan)
        => new Range
        {
            Start = linePositionSpan.Start.ToPosition(),
            End = linePositionSpan.End.ToPosition()
        };

    public static TextSpan ToTextSpan(this LinePositionSpan linePositionSpan, SourceText sourceText)
        => sourceText.GetTextSpan(linePositionSpan.Start.Line, linePositionSpan.Start.Character, linePositionSpan.End.Line, linePositionSpan.End.Character);

    public static bool OverlapsWith(this LinePositionSpan range, LinePositionSpan other)
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
        return overlapStart.CompareTo(overlapEnd) < 0;
    }
}
