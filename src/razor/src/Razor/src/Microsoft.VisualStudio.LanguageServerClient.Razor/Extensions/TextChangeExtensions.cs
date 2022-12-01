// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

internal static class TextChangeExtensions
{
    public static ITextChange ToVisualStudioTextChange(this TextChange roslynTextChange) =>
        new VisualStudioTextChange(roslynTextChange.Span.Start, roslynTextChange.Span.Length, roslynTextChange.NewText!);

    public static TextEdit AsTextEdit(this TextChange textChange, SourceText sourceText)
    {
        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        var range = textChange.Span.AsRange(sourceText);

        return new TextEdit()
        {
            NewText = textChange.NewText!,
            Range = range
        };
    }
}
