// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Text;

internal static class TextSpanExtensions
{
    internal static TextSpan TrimLeadingWhitespace(this TextSpan span, SourceText text)
    {
        for (var i = 0; i < span.Length; ++i)
        {
            if (!char.IsWhiteSpace(text[span.Start + i]))
            {
                return new TextSpan(span.Start + i, span.Length - i);
            }
        }

        return span;
    }
}
