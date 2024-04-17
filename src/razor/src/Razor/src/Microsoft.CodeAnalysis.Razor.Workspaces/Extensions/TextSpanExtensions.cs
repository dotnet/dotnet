// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal static class TextSpanExtensions
{
    internal static TextSpan TrimLeadingWhitespace(this TextSpan span, SourceText text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        for (var i = 0; i < span.Length; ++i)
        {
            if (!char.IsWhiteSpace(text[span.Start + i]))
            {
                return new TextSpan(span.Start + i, span.Length - i);
            }
        }

        return span;
    }

    public static Range ToRange(this TextSpan span, SourceText sourceText)
    {
        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        sourceText.GetLinesAndOffsets(span, out var startLine, out var startChar, out var endLine, out var endChar);

        var range = new Range
        {
            Start = new Position(startLine, startChar),
            End = new Position(endLine, endChar)
        };

        return range;
    }
}
