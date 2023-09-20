// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class LinePositionSpanExtensions
{
    public static Range AsRange(this LinePositionSpan linePositionSpan)
    {
        var range = new Range
        {
            Start = new Position { Line = linePositionSpan.Start.Line, Character = linePositionSpan.Start.Character },
            End = new Position { Line = linePositionSpan.End.Line, Character = linePositionSpan.End.Character }
        };

        return range;
    }
}
