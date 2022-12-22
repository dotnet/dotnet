// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class IndentationContext
{
    public int Line { get; init; }

#if DEBUG
    public string? DebugOnly_LineText { get; init; }
#endif

    public int RazorIndentationLevel { get; init; }

    public int HtmlIndentationLevel { get; init; }

    public int IndentationLevel => RazorIndentationLevel + HtmlIndentationLevel;

    public int RelativeIndentationLevel { get; init; }

    /// <summary>
    /// The number of characters of indentation there are on this line
    /// </summary>
    public int ExistingIndentation { get; init; }

    public FormattingSpan FirstSpan { get; }

    public bool EmptyOrWhitespaceLine { get; init; }

    public bool StartsInHtmlContext => FirstSpan.Kind == FormattingSpanKind.Markup;

    public bool StartsInCSharpContext => FirstSpan.Kind == FormattingSpanKind.Code;

    public bool StartsInRazorContext => !StartsInHtmlContext && !StartsInCSharpContext;

    public int MinCSharpIndentLevel => FirstSpan.MinCSharpIndentLevel;

    /// <summary>
    /// The amount of visual indentation there is on this line, taking into account tab size
    /// </summary>
    public int ExistingIndentationSize { get; init; }

    public IndentationContext(FormattingSpan firstSpan)
    {
        FirstSpan = firstSpan;
    }

    public override string ToString()
    {
        return $"Line: {Line}, IndentationLevel: {IndentationLevel}, RelativeIndentationLevel: {RelativeIndentationLevel}, ExistingIndentation: {ExistingIndentation}";
    }
}
