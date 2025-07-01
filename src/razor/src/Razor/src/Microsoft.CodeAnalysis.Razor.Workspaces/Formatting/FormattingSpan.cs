// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

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
