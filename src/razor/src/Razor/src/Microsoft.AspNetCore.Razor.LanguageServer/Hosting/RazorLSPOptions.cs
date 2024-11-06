// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Settings;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal record RazorLSPOptions(
    FormattingFlags Formatting,
    bool AutoClosingTags,
    bool InsertSpaces,
    int TabSize,
    bool AutoShowCompletion,
    bool AutoListParams,
    bool AutoInsertAttributeQuotes,
    bool ColorBackground,
    bool CodeBlockBraceOnNextLine,
    bool CommitElementsWithSpace)
{
    public readonly static RazorLSPOptions Default = new(Formatting: FormattingFlags.All,
                                                         AutoClosingTags: true,
                                                         AutoListParams: true,
                                                         InsertSpaces: true,
                                                         TabSize: 4,
                                                         AutoShowCompletion: true,
                                                         AutoInsertAttributeQuotes: true,
                                                         ColorBackground: false,
                                                         CodeBlockBraceOnNextLine: false,
                                                         CommitElementsWithSpace: true);

    /// <summary>
    /// Initializes the LSP options with the settings from the passed in client settings, and default values for anything
    /// not defined in client settings.
    /// </summary>
    internal static RazorLSPOptions From(ClientSettings settings)
        => new(GetFormattingFlags(settings),
              settings.AdvancedSettings.AutoClosingTags,
              !settings.ClientSpaceSettings.IndentWithTabs,
              settings.ClientSpaceSettings.IndentSize,
              settings.ClientCompletionSettings.AutoShowCompletion,
              settings.ClientCompletionSettings.AutoListParams,
              settings.AdvancedSettings.AutoInsertAttributeQuotes,
              settings.AdvancedSettings.ColorBackground,
              settings.AdvancedSettings.CodeBlockBraceOnNextLine,
              settings.AdvancedSettings.CommitElementsWithSpace);

    private static FormattingFlags GetFormattingFlags(ClientSettings settings)
    {
        var flags = FormattingFlags.Enabled;
        if (settings.AdvancedSettings.FormatOnPaste)
        {
            flags |= FormattingFlags.OnPaste;
        }

        if (settings.AdvancedSettings.FormatOnType)
        {
            flags |= FormattingFlags.OnType;
        }

        return flags;
    }
}
