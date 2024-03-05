// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Settings;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal record RazorLSPOptions(
    bool EnableFormatting,
    bool AutoClosingTags,
    bool InsertSpaces,
    int TabSize,
    bool AutoShowCompletion,
    bool AutoListParams,
    bool FormatOnType,
    bool AutoInsertAttributeQuotes,
    bool ColorBackground,
    bool CodeBlockBraceOnNextLine,
    bool CommitElementsWithSpace)
{
    public readonly static RazorLSPOptions Default = new(EnableFormatting: true,
                                                         AutoClosingTags: true,
                                                         AutoListParams: true,
                                                         InsertSpaces: true,
                                                         TabSize: 4,
                                                         AutoShowCompletion: true,
                                                         FormatOnType: true,
                                                         AutoInsertAttributeQuotes: true,
                                                         ColorBackground: false,
                                                         CodeBlockBraceOnNextLine: false,
                                                         CommitElementsWithSpace: true);

    /// <summary>
    /// Initializes the LSP options with the settings from the passed in client settings, and default values for anything
    /// not defined in client settings.
    /// </summary>
    internal static RazorLSPOptions From(ClientSettings settings)
        => new(Default.EnableFormatting,
              settings.AdvancedSettings.AutoClosingTags,
              !settings.ClientSpaceSettings.IndentWithTabs,
              settings.ClientSpaceSettings.IndentSize,
              settings.ClientCompletionSettings.AutoShowCompletion,
              settings.ClientCompletionSettings.AutoListParams,
              settings.AdvancedSettings.FormatOnType,
              settings.AdvancedSettings.AutoInsertAttributeQuotes,
              settings.AdvancedSettings.ColorBackground,
              settings.AdvancedSettings.CodeBlockBraceOnNextLine,
              settings.AdvancedSettings.CommitElementsWithSpace);
}
