// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal readonly record struct RazorFormattingOptions
{
    public static readonly RazorFormattingOptions Default = new();

    public bool InsertSpaces { get; init; } = true;
    public int TabSize { get; init; } = 4;
    public bool CodeBlockBraceOnNextLine { get; init; } = false;

    public RazorFormattingOptions()
    {
    }

    public static RazorFormattingOptions From(FormattingOptions options, bool codeBlockBraceOnNextLine)
    {
        return new RazorFormattingOptions()
        {
            InsertSpaces = options.InsertSpaces,
            TabSize = options.TabSize,
            CodeBlockBraceOnNextLine = codeBlockBraceOnNextLine
        };
    }

    public RazorIndentationOptions ToIndentationOptions()
        => new(
            UseTabs: !InsertSpaces,
            TabSize: TabSize,
            IndentationSize: TabSize);
}
