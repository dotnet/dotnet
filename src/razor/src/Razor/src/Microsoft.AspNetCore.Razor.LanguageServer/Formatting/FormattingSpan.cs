// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class FormattingSpan
{
    public FormattingSpan(
        TextSpan span,
        TextSpan blockSpan,
        FormattingSpanKind spanKind,
        FormattingBlockKind blockKind,
        int razorIndentationLevel,
        int htmlIndentationLevel,
        bool isInGlobalNamespace,
        bool isInClassBody = false,
        int componentLambdaNestingLevel = 0)
    {
        Span = span;
        BlockSpan = blockSpan;
        Kind = spanKind;
        BlockKind = blockKind;
        RazorIndentationLevel = razorIndentationLevel;
        HtmlIndentationLevel = htmlIndentationLevel;
        IsInGlobalNamespace = isInGlobalNamespace;
        IsInClassBody = isInClassBody;
        ComponentLambdaNestingLevel = componentLambdaNestingLevel;
    }

    public TextSpan Span { get; }

    public TextSpan BlockSpan { get; }

    public FormattingBlockKind BlockKind { get; }

    public FormattingSpanKind Kind { get; }

    public int RazorIndentationLevel { get; }

    public int HtmlIndentationLevel { get; }

    public int IndentationLevel => RazorIndentationLevel + HtmlIndentationLevel;

    public bool IsInGlobalNamespace { get; }

    public bool IsInClassBody { get; }

    public int ComponentLambdaNestingLevel { get; }

    public int MinCSharpIndentLevel
    {
        get
        {
            var baseIndent = 1;

            if (!IsInGlobalNamespace)
            {
                baseIndent++;
            }

            if (!IsInClassBody)
            {
                baseIndent++;
            }

            return baseIndent + ComponentLambdaNestingLevel;
        }
    }
}
