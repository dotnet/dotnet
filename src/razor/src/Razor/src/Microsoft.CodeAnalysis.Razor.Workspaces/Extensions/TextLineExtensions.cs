// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.CodeAnalysis.Text;

internal static class TextLineExtensions
{
    public static string GetLeadingWhitespace(this TextLine line)
    {
        return line.ToString().GetLeadingWhitespace();
    }

    public static int GetIndentationSize(this TextLine line, long tabSize)
    {
        var text = line.Text.AssumeNotNull();

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
            return ThrowHelper.ThrowArgumentOutOfRangeException<int?>(nameof(startOffset), SR.Invalid_Offset);
        }

        return line.Text!.TryGetFirstNonWhitespaceOffset(TextSpan.FromBounds(line.Start + startOffset, line.EndIncludingLineBreak), out var offset)
            ? offset
            : null;
    }
}
