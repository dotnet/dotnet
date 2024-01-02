// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Editor;

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
    bool CommitElementsWithSpace)
{
    public RazorLSPOptions(bool enableFormatting, bool autoClosingTags, bool commitElementsWithSpace, ClientSettings settings)
        : this(enableFormatting,
              autoClosingTags,
              !settings.ClientSpaceSettings.IndentWithTabs,
              settings.ClientSpaceSettings.IndentSize,
              settings.ClientCompletionSettings.AutoShowCompletion,
              settings.ClientCompletionSettings.AutoListParams,
              settings.AdvancedSettings.FormatOnType,
              settings.AdvancedSettings.AutoInsertAttributeQuotes,
              settings.AdvancedSettings.ColorBackground,
              commitElementsWithSpace)
    {
    }

    public readonly static RazorLSPOptions Default = new(EnableFormatting: true,
                                                         AutoClosingTags: true,
                                                         AutoListParams: true,
                                                         InsertSpaces: true,
                                                         TabSize: 4,
                                                         AutoShowCompletion: true,
                                                         FormatOnType: true,
                                                         AutoInsertAttributeQuotes: true,
                                                         ColorBackground: false,
                                                         CommitElementsWithSpace: true);

    /// <summary>
    /// Initializes the LSP options with the settings from the passed in client settings, and default values for anything
    /// not defined in client settings.
    /// </summary>
    internal static RazorLSPOptions From(ClientSettings clientSettings)
        => new(Default.EnableFormatting,
            clientSettings.AdvancedSettings.AutoClosingTags,
            clientSettings.AdvancedSettings.CommitElementsWithSpace,
            clientSettings);
}
