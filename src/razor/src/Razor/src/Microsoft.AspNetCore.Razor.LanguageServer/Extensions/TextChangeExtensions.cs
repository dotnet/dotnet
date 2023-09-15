// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class TextChangeExtensions
{
    public static TextEdit ToTextEdit(this TextChange textChange, SourceText sourceText)
    {
        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        var range = textChange.Span.ToRange(sourceText);

        Assumes.NotNull(textChange.NewText);

        return new TextEdit()
        {
            NewText = textChange.NewText,
            Range = range
        };
    }
}
