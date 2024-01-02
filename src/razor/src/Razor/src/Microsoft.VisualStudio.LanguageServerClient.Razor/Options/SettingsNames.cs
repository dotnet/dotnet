// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Options;

internal static class SettingsNames
{
    public record Setting(string LegacyName, string UnifiedName);

    public const string LegacyCollection = "Razor";
    public const string UnifiedCollection = "textEditor.razor.advanced";

    public static readonly Setting FormatOnType = new("FormatOnType", UnifiedCollection + ".formatOnType");
    public static readonly Setting AutoClosingTags = new("AutoClosingTags", UnifiedCollection + ".autoClosingTags");
    public static readonly Setting AutoInsertAttributeQuotes = new("AutoInsertAttributeQuotes", UnifiedCollection + ".autoInsertAttributeQuotes");
    public static readonly Setting ColorBackground = new("ColorBackground", UnifiedCollection + ".colorBackground");
    public static readonly Setting CommitElementsWithSpace = new("CommitElementsWithSpace", UnifiedCollection + ".commitCharactersWithSpace");
    public static readonly Setting Snippets = new("Snippets", UnifiedCollection + ".snippets");
    public static readonly Setting LogLevel = new("LogLevel", UnifiedCollection + ".logLevel");

    public static readonly Setting[] AllSettings =
    [
        FormatOnType,
        AutoClosingTags,
        AutoInsertAttributeQuotes,
        ColorBackground,
        CommitElementsWithSpace,
        Snippets,
        LogLevel,
    ];
}
