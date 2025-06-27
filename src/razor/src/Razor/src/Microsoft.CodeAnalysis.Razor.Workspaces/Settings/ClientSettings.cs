// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Razor.Settings;

/// <summary>
/// Settings that are set and handled on the client, but needed by the LSP Server to function correctly. When these are
/// updated a workspace/didchangeconfiguration should be sent from client to the server. Then the server requests
/// workspace/configuration to get the latest settings. For VS, the razor protocol also handles this and serializes the
/// settings back to the server.
/// </summary>
/// <param name="ClientSpaceSettings"></param>
/// <param name="ClientCompletionSettings"></param>
/// <param name="AdvancedSettings"></param>
internal record ClientSettings(ClientSpaceSettings ClientSpaceSettings, ClientCompletionSettings ClientCompletionSettings, ClientAdvancedSettings AdvancedSettings)
{
    public static readonly ClientSettings Default = new(ClientSpaceSettings.Default, ClientCompletionSettings.Default, ClientAdvancedSettings.Default);
}

internal sealed record ClientCompletionSettings(bool AutoShowCompletion, bool AutoListParams)
{
    public static readonly ClientCompletionSettings Default = new(AutoShowCompletion: true, AutoListParams: true);
}

internal sealed record ClientSpaceSettings(bool IndentWithTabs, int IndentSize)
{
    public static readonly ClientSpaceSettings Default = new(IndentWithTabs: false, IndentSize: 4);

    public int IndentSize { get; } = IndentSize >= 0 ? IndentSize : throw new ArgumentOutOfRangeException(nameof(IndentSize));
}

internal sealed record ClientAdvancedSettings(bool FormatOnType,
                                              bool AutoClosingTags,
                                              bool AutoInsertAttributeQuotes,
                                              bool ColorBackground,
                                              bool CodeBlockBraceOnNextLine,
                                              bool CommitElementsWithSpace,
                                              SnippetSetting SnippetSetting,
                                              LogLevel LogLevel,
                                              bool FormatOnPaste,
                                              ImmutableArray<string> TaskListDescriptors)
{
    public static readonly ClientAdvancedSettings Default = new(FormatOnType: true,
                                                                AutoClosingTags: true,
                                                                AutoInsertAttributeQuotes: true,
                                                                ColorBackground: false,
                                                                CodeBlockBraceOnNextLine: false,
                                                                CommitElementsWithSpace: true,
                                                                SnippetSetting.All,
                                                                LogLevel.Warning,
                                                                FormatOnPaste: true,
                                                                TaskListDescriptors: []);
}
