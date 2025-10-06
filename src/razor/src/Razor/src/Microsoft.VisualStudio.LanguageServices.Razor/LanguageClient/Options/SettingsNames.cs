// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.VisualStudio.Razor.LanguageClient.Options;

internal static class SettingsNames
{
    public record Setting(string LegacyName, string UnifiedName);

    public const string LegacyCollection = "Razor";
    public const string UnifiedCollection = "textEditor.razor.advanced";

    public static readonly Setting FormatOnType = new("FormatOnType", UnifiedCollection + ".formatOnType");
    public static readonly Setting AutoClosingTags = new("AutoClosingTags", UnifiedCollection + ".autoClosingTags");
    public static readonly Setting AutoInsertAttributeQuotes = new("AutoInsertAttributeQuotes", UnifiedCollection + ".autoInsertAttributeQuotes");
    public static readonly Setting ColorBackground = new("ColorBackground", UnifiedCollection + ".colorBackground");
    public static readonly Setting CodeBlockBraceOnNextLine = new("CodeBlockBraceOnNextLine", UnifiedCollection + ".codeBlockBraceOnNextLine");
    public static readonly Setting CommitElementsWithSpace = new("CommitElementsWithSpace", UnifiedCollection + ".commitCharactersWithSpace");
    public static readonly Setting Snippets = new("Snippets", UnifiedCollection + ".snippets");
    public static readonly Setting LogLevel = new("LogLevel", UnifiedCollection + ".logLevel");
    public static readonly Setting FormatOnPaste = new("FormatOnPaste", UnifiedCollection + ".formatOnPaste");

    public static readonly Setting[] AllSettings =
    [
        FormatOnType,
        AutoClosingTags,
        AutoInsertAttributeQuotes,
        ColorBackground,
        CodeBlockBraceOnNextLine,
        CommitElementsWithSpace,
        Snippets,
        LogLevel,
        FormatOnPaste,
    ];
}
