// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public record RazorLSPOptions(
    Trace Trace,
    bool EnableFormatting,
    bool AutoClosingTags,
    bool InsertSpaces,
    int TabSize,
    bool FormatOnType)
{
    public RazorLSPOptions(Trace trace, bool enableFormatting, bool autoClosingTags, ClientSettings settings)
        : this(trace, enableFormatting, autoClosingTags, !settings.ClientSpaceSettings.IndentWithTabs, settings.ClientSpaceSettings.IndentSize, settings.AdvancedSettings.FormatOnType)
    {
    }

    public readonly static RazorLSPOptions Default = new(Trace: default, EnableFormatting: true, AutoClosingTags: true, InsertSpaces: true, TabSize: 4, FormatOnType: true);

    public LogLevel MinLogLevel => GetLogLevelForTrace(Trace);

    public static LogLevel GetLogLevelForTrace(Trace trace)
        => trace switch
        {
            Trace.Off => LogLevel.None,
            Trace.Messages => LogLevel.Information,
            Trace.Verbose => LogLevel.Trace,
            _ => LogLevel.None,
        };

    internal RazorLSPOptions With(ClientSettings clientSettings)
        => new(Trace, EnableFormatting, AutoClosingTags, clientSettings);
}
