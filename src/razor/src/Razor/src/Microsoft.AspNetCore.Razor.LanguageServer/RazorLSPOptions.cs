// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal record RazorLSPOptions(
    Trace Trace,
    bool EnableFormatting,
    bool AutoClosingTags,
    bool InsertSpaces,
    int TabSize,
    bool FormatOnType,
    bool AutoInsertAttributeQuotes,
    bool ColorBackground)
{
    public RazorLSPOptions(Trace trace, bool enableFormatting, bool autoClosingTags, ClientSettings settings)
        : this(trace, enableFormatting, autoClosingTags, !settings.ClientSpaceSettings.IndentWithTabs, settings.ClientSpaceSettings.IndentSize, settings.AdvancedSettings.FormatOnType, settings.AdvancedSettings.AutoInsertAttributeQuotes, settings.AdvancedSettings.ColorBackground)
    {
    }

    public readonly static RazorLSPOptions Default = new(Trace: default, EnableFormatting: true, AutoClosingTags: true, InsertSpaces: true, TabSize: 4, FormatOnType: true, AutoInsertAttributeQuotes: true, ColorBackground: false);

    public LogLevel MinLogLevel => GetLogLevelForTrace(Trace);

    public static LogLevel GetLogLevelForTrace(Trace trace)
        => trace switch
        {
            Trace.Off => LogLevel.None,
            Trace.Messages => LogLevel.Information,
            Trace.Verbose => LogLevel.Trace,
            _ => LogLevel.None,
        };

    /// <summary>
    /// Initializes the LSP options with the settings from the passed in client settings, and default values for anything
    /// not defined in client settings.
    /// </summary>
    internal static RazorLSPOptions From(ClientSettings clientSettings)
        => new(Default.Trace,
            Default.EnableFormatting,
            clientSettings.AdvancedSettings.AutoClosingTags,
            clientSettings);
}
