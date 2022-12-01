// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class TextLineExtensions
{
    public static string GetLeadingWhitespace(this TextLine line)
    {
        return line.ToString().GetLeadingWhitespace();
    }

    public static int GetIndentationSize(this TextLine line, long tabSize)
    {
        var text = line.Text;

        Assumes.NotNull(text);

        var indentation = 0;
        for (var i = line.Start; i < line.End; i++)
        {
            var c = text[i];
            if (c == '\t')
            {
                indentation += (int)tabSize;
            }
            else if (char.IsWhiteSpace(c))
            {
                indentation++;
            }
            else
            {
                break;
            }
        }

        return indentation;
    }

    public static int? GetFirstNonWhitespacePosition(this TextLine line)
    {
        var firstNonWhitespaceOffset = line.GetFirstNonWhitespaceOffset();

        return firstNonWhitespaceOffset.HasValue
            ? firstNonWhitespaceOffset + line.Start
            : null;
    }

    public static int? GetFirstNonWhitespaceOffset(this TextLine line, int startOffset = 0)
    {
        if (startOffset > line.SpanIncludingLineBreak.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(startOffset), SR.Invalid_Offset);
        }

        return line.Text!.GetFirstNonWhitespaceOffset(TextSpan.FromBounds(line.Start + startOffset, line.EndIncludingLineBreak), out _);
    }
}
