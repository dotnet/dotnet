// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class TextSpanExtensions
{
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

    public static Range ToRange(this Language.Syntax.TextSpan span, SourceText sourceText)
        => new TextSpan(span.Start, span.Length).ToRange(sourceText);
}
