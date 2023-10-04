// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class SourceSpanExtensions
{
    public static Range ToRange(this SourceSpan sourceSpan, SourceText sourceText)
    {
        sourceText.GetLinesAndOffsets(sourceSpan, out var startLine, out var startChar, out var endLine, out var endChar);

        return new Range
        {
            Start = new Position(startLine, startChar),
            End = new Position(endLine, endChar),
        };
    }

    public static LinePositionSpan ToLinePositionSpan(this SourceSpan sourceSpan, SourceText sourceText)
    {
        sourceText.GetLinesAndOffsets(sourceSpan, out var startLine, out var startChar, out var endLine, out var endChar);

        return new LinePositionSpan(new(startLine, startChar), new(endLine, endChar));
    }
}
