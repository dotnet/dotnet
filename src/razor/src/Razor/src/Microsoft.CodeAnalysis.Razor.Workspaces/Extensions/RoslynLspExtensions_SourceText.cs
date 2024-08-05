// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class RoslynLspExtensions
{
    public static Range GetRange(this SourceText text, TextSpan span)
        => text.GetLinePositionSpan(span).ToRange();

    public static TextSpan GetTextSpan(this SourceText text, Range range)
        => text.GetTextSpan(range.Start.Line, range.Start.Character, range.End.Line, range.End.Character);

    public static TextChange GetTextChange(this SourceText text, TextEdit edit)
        => new(text.GetTextSpan(edit.Range), edit.NewText);

    public static TextEdit GetTextEdit(this SourceText text, TextChange change)
        => RoslynLspFactory.CreateTextEdit(text.GetRange(change.Span), change.NewText.AssumeNotNull());

}
