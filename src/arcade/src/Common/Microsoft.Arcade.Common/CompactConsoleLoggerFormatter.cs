// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

#nullable enable
namespace Microsoft.Arcade.Common;

/// <summary>
/// Copied over from SimpleConsoleFormatter. Leaves out the logger name and new line, turning
/// info: test[0]
///     Log message
///     Second line of the message
///
/// into
///
/// info: Log message
///       Second line of the message
///
/// Only using SimpleConsoleFormatterOptions.SingleLine didn't help because multi-line messages
/// were put together on a single line so things like stack traces of exceptions were unreadable.
///
/// See https://github.com/dotnet/runtime/blob/0817e748b7698bef1e812fd74c8a3558b7f86421/src/libraries/Microsoft.Extensions.Logging.Console/src/SimpleConsoleFormatter.cs
/// </summary>
public class CompactConsoleLoggerFormatter : ConsoleFormatter
{
    private const string LoglevelPadding = ": ";
    private const string DefaultForegroundColor = "\x1B[39m\x1B[22m"; // reset to default foreground color
    private const string DefaultBackgroundColor = "\x1B[49m"; // reset to the background color

    public const string FormatterName = "compact";

    private readonly SimpleConsoleFormatterOptions _options;
    private readonly string _messagePadding;
    private readonly string _newLineWithMessagePadding;

    public CompactConsoleLoggerFormatter(IOptionsMonitor<SimpleConsoleFormatterOptions> options)
        : base(FormatterName)
    {
        _options = options.CurrentValue;
        _messagePadding = new string(' ', GetLogLevelString(LogLevel.Information).Length + LoglevelPadding.Length + (_options.TimestampFormat?.Length ?? 0));
        _newLineWithMessagePadding = Environment.NewLine + _messagePadding;
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        if (logEntry.Formatter == null)
        {
            return;
        }

        var message = logEntry.Formatter(logEntry.State, logEntry.Exception);
        if (logEntry.Exception == null && message == null)
        {
            return;
        }

        LogLevel logLevel = logEntry.LogLevel;
        var logLevelColors = GetLogLevelConsoleColors(logLevel);
        var logLevelString = GetLogLevelString(logLevel);

        if (_options.TimestampFormat != null)
        {
            var timestamp = DateTimeOffset.Now.ToString(_options.TimestampFormat);
            textWriter.Write(timestamp);
        }

        WriteColoredMessage(textWriter, logLevelString, logLevelColors.Background, logLevelColors.Foreground);

        textWriter.Write(LoglevelPadding);

        WriteMessage(textWriter, message, false);

        // Example:
        // System.InvalidOperationException
        //    at Namespace.Class.Function() in File:line X
        if (logEntry.Exception != null)
        {
            // exception message
            WriteMessage(textWriter, logEntry.Exception.ToString());
        }
    }

    private void WriteMessage(TextWriter textWriter, string message, bool includePadding = true)
    {
        if (message == null)
        {
            return;
        }

        if (includePadding)
        {
            textWriter.Write(_messagePadding);
        }

        textWriter.WriteLine(message.Replace(Environment.NewLine, _newLineWithMessagePadding));
    }

    private static string GetLogLevelString(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "trce",
        LogLevel.Debug => "dbug",
        LogLevel.Information => "info",
        LogLevel.Warning => "warn",
        LogLevel.Error => "fail",
        LogLevel.Critical => "crit",
        _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
    };

    private (ConsoleColor? Foreground, ConsoleColor? Background) GetLogLevelConsoleColors(LogLevel logLevel)
    {
        if (_options.ColorBehavior == LoggerColorBehavior.Disabled)
        {
            return (null, null);
        }

        // We must explicitly set the background color if we are setting the foreground color,
        // since just setting one can look bad on the users console.
        return logLevel switch
        {
            LogLevel.Trace => (ConsoleColor.Gray, ConsoleColor.Black),
            LogLevel.Debug => (ConsoleColor.Gray, ConsoleColor.Black),
            LogLevel.Information => (ConsoleColor.DarkGreen, ConsoleColor.Black),
            LogLevel.Warning => (ConsoleColor.Yellow, ConsoleColor.Black),
            LogLevel.Error => (ConsoleColor.Black, ConsoleColor.DarkRed),
            LogLevel.Critical => (ConsoleColor.White, ConsoleColor.DarkRed),
            _ => (null, null)
        };
    }

    private static void WriteColoredMessage(TextWriter textWriter, string message, ConsoleColor? background, ConsoleColor? foreground)
    {
        // Order: backgroundcolor, foregroundcolor, Message, reset foregroundcolor, reset backgroundcolor
        if (background.HasValue)
        {
            textWriter.Write(GetBackgroundColorEscapeCode(background.Value));
        }

        if (foreground.HasValue)
        {
            textWriter.Write(GetForegroundColorEscapeCode(foreground.Value));
        }

        textWriter.Write(message);

        if (foreground.HasValue)
        {
            textWriter.Write(DefaultForegroundColor); // reset to default foreground color
        }

        if (background.HasValue)
        {
            textWriter.Write(DefaultBackgroundColor); // reset to the background color
        }
    }

    private static string GetForegroundColorEscapeCode(ConsoleColor color) => color switch
    {
        ConsoleColor.Black => "\x1B[30m",
        ConsoleColor.DarkRed => "\x1B[31m",
        ConsoleColor.DarkGreen => "\x1B[32m",
        ConsoleColor.DarkYellow => "\x1B[33m",
        ConsoleColor.DarkBlue => "\x1B[34m",
        ConsoleColor.DarkMagenta => "\x1B[35m",
        ConsoleColor.DarkCyan => "\x1B[36m",
        ConsoleColor.Gray => "\x1B[37m",
        ConsoleColor.Red => "\x1B[1m\x1B[31m",
        ConsoleColor.Green => "\x1B[1m\x1B[32m",
        ConsoleColor.Yellow => "\x1B[1m\x1B[33m",
        ConsoleColor.Blue => "\x1B[1m\x1B[34m",
        ConsoleColor.Magenta => "\x1B[1m\x1B[35m",
        ConsoleColor.Cyan => "\x1B[1m\x1B[36m",
        ConsoleColor.White => "\x1B[1m\x1B[37m",
        _ => DefaultForegroundColor // default foreground color
    };

    private static string GetBackgroundColorEscapeCode(ConsoleColor color) => color switch
    {
        ConsoleColor.Black => "\x1B[40m",
        ConsoleColor.DarkRed => "\x1B[41m",
        ConsoleColor.DarkGreen => "\x1B[42m",
        ConsoleColor.DarkYellow => "\x1B[43m",
        ConsoleColor.DarkBlue => "\x1B[44m",
        ConsoleColor.DarkMagenta => "\x1B[45m",
        ConsoleColor.DarkCyan => "\x1B[46m",
        ConsoleColor.Gray => "\x1B[47m",
        _ => DefaultBackgroundColor // Use default background color
    };
}
