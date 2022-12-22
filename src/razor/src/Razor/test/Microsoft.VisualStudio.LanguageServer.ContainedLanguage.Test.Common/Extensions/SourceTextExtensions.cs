// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage.Test.Common.Extensions;

internal static class SourceTextExtensions
{
    public static void GetLinesAndOffsets(
        this SourceText sourceText,
        Span span,
        out int startLineNumber,
        out int startOffset,
        out int endLineNumber,
        out int endOffset)
    {
        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        sourceText.GetLineAndOffset(span.Start, out startLineNumber, out startOffset);
        sourceText.GetLineAndOffset(span.End, out endLineNumber, out endOffset);
    }

    public static void GetLineAndOffset(this SourceText source, int position, out int lineNumber, out int offset)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var line = source.Lines.GetLineFromPosition(position);

        lineNumber = line.LineNumber;
        offset = position - line.Start;
    }
}
