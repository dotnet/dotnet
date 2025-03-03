// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal sealed record RazorLSPOptions(
    FormattingFlags Formatting,
    bool AutoClosingTags,
    bool InsertSpaces,
    int TabSize,
    bool AutoShowCompletion,
    bool AutoListParams,
    bool AutoInsertAttributeQuotes,
    bool ColorBackground,
    bool CodeBlockBraceOnNextLine,
    bool CommitElementsWithSpace,
    ImmutableArray<string> TaskListDescriptors)
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
                                                         CommitElementsWithSpace: true,
                                                         TaskListDescriptors: []);

    public ImmutableArray<string> TaskListDescriptors
    {
        get;
        init => field = value.NullToEmpty();

    } = TaskListDescriptors.NullToEmpty();

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
              settings.AdvancedSettings.CommitElementsWithSpace,
              settings.AdvancedSettings.TaskListDescriptors);

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

    public bool Equals(RazorLSPOptions? other)
    {
        return other is not null &&
            Formatting == other.Formatting &&
            AutoClosingTags == other.AutoClosingTags &&
            InsertSpaces == other.InsertSpaces &&
            TabSize == other.TabSize &&
            AutoShowCompletion == other.AutoShowCompletion &&
            AutoListParams == other.AutoListParams &&
            AutoInsertAttributeQuotes == other.AutoInsertAttributeQuotes &&
            ColorBackground == other.ColorBackground &&
            CodeBlockBraceOnNextLine == other.CodeBlockBraceOnNextLine &&
            CommitElementsWithSpace == other.CommitElementsWithSpace &&
            TaskListDescriptors.SequenceEqual(other.TaskListDescriptors);
    }

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();
        hash.Add(Formatting);
        hash.Add(AutoClosingTags);
        hash.Add(InsertSpaces);
        hash.Add(TabSize);
        hash.Add(AutoShowCompletion);
        hash.Add(AutoListParams);
        hash.Add(AutoInsertAttributeQuotes);
        hash.Add(ColorBackground);
        hash.Add(CodeBlockBraceOnNextLine);
        hash.Add(CommitElementsWithSpace);
        hash.Add(TaskListDescriptors);
        return hash;
    }
}
