// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.VisualStudio.Razor.LanguageClient.Options;

internal static class SettingsNames
{
    public const string UnifiedCollection = "languages.razor.advanced";

    public static readonly string FormatOnType = UnifiedCollection + ".formatOnType";
    public static readonly string AutoClosingTags = UnifiedCollection + ".autoClosingTags";
    public static readonly string AutoInsertAttributeQuotes = UnifiedCollection + ".autoInsertAttributeQuotes";
    public static readonly string ColorBackground = UnifiedCollection + ".colorBackground";
    public static readonly string CodeBlockBraceOnNextLine = UnifiedCollection + ".codeBlockBraceOnNextLine";
    public static readonly string AttributeIndentStyle = UnifiedCollection + ".attributeIndentStyle";
    public static readonly string CommitElementsWithSpace = UnifiedCollection + ".commitElementsWithSpace";
    public static readonly string Snippets = UnifiedCollection + ".snippets";
    public static readonly string LogLevel = UnifiedCollection + ".logLevel";
    public static readonly string FormatOnPaste = UnifiedCollection + ".formatOnPaste";

    public static readonly string[] AllSettings =
    [
        FormatOnType,
        AutoClosingTags,
        AutoInsertAttributeQuotes,
        ColorBackground,
        CodeBlockBraceOnNextLine,
        AttributeIndentStyle,
        CommitElementsWithSpace,
        Snippets,
        LogLevel,
        FormatOnPaste,
    ];
}
