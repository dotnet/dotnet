// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

[DataContract]
internal readonly record struct RazorFormattingOptions
{
    [DataMember(Order = 0)]
    public bool InsertSpaces { get; init; } = true;
    [DataMember(Order = 1)]
    public int TabSize { get; init; } = 4;
    [DataMember(Order = 2)]
    public bool CodeBlockBraceOnNextLine { get; init; } = false;

    public RazorFormattingOptions()
    {
    }

    public static RazorFormattingOptions From(FormattingOptions options, bool codeBlockBraceOnNextLine)
        => new()
        {
            InsertSpaces = options.InsertSpaces,
            TabSize = options.TabSize,
            CodeBlockBraceOnNextLine = codeBlockBraceOnNextLine
        };

    public RazorIndentationOptions ToIndentationOptions()
        => new(
            UseTabs: !InsertSpaces,
            TabSize: TabSize,
            IndentationSize: TabSize);

    public FormattingOptions ToLspFormattingOptions()
        => new()
        {
            InsertSpaces = InsertSpaces,
            TabSize = TabSize,
        };
}
