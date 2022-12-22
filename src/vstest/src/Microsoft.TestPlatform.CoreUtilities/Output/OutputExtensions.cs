// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Resources;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

/// <summary>
/// Utility Methods for sending output to IOutput.
/// </summary>
public static class OutputExtensions
{
    private const string DefaultFormat = "{0}";

    /// <summary>
    /// Output an error message.
    /// </summary>
    /// <param name="output">Output instance the method is being invoked with.</param>
    /// <param name="appendPrefix">Bool to decide whether Verbose level should be added as prefix or not in log messages.</param>
    /// <param name="format">Format string for the error message.</param>
    /// <param name="args">Arguments to format into the format string.</param>
    public static void Error(this IOutput output, bool appendPrefix, string format, params object?[]? args)
    {
        SetColorForAction(ConsoleColor.Red, () => Output(output, OutputLevel.Error, appendPrefix ? Resources.CommandLineError : DefaultFormat, format, args));
    }

    /// <summary>
    /// Output a warning message.
    /// </summary>
    /// <param name="output">Output instance the method is being invoked with.</param>
    /// <param name="appendPrefix">Bool to decide whether Verbose level should be added as prefix or not in log messages.</param>
    /// <param name="format">Format string for the warning message.</param>
    /// <param name="args">Arguments to format into the format string.</param>
    public static void Warning(this IOutput output, bool appendPrefix, string format, params object?[]? args)
    {
        SetColorForAction(ConsoleColor.Yellow, () => Output(output, OutputLevel.Warning, appendPrefix ? Resources.CommandLineWarning : DefaultFormat, format, args));
    }

    /// <summary>
    /// Output a informational message.
    /// </summary>
    /// <param name="output">Output instance the method is being invoked with.</param>
    /// <param name="appendPrefix">Bool to decide whether Verbose level should be added as prefix or not in log messages.</param>
    /// <param name="format">Format string for the informational message.</param>
    /// <param name="args">Arguments to format into the format string.</param>
    public static void Information(this IOutput output, bool appendPrefix, string format, params object?[]? args)
    {
        Information(output, appendPrefix, Console.ForegroundColor, format, args);
    }

    /// <summary>
    /// Output a informational message.
    /// </summary>
    /// <param name="output">Output instance the method is being invoked with.</param>
    /// <param name="appendPrefix">Bool to decide whether Verbose level should be added as prefix or not in log messages.</param>
    /// <param name="foregroundColor">Color in which text prints.</param>
    /// <param name="format">Format string for the informational message.</param>
    /// <param name="args">Arguments to format into the format string.</param>
    public static void Information(this IOutput output, bool appendPrefix, ConsoleColor foregroundColor, string format, params object?[]? args)
    {
        SetColorForAction(foregroundColor, () => Output(output, OutputLevel.Information, appendPrefix ? Resources.CommandLineInformational : DefaultFormat, format, args));
    }

    /// <summary>
    /// Write string with a given console color
    /// </summary>
    /// <param name="output">Output instance the method is being invoked with.</param>
    /// <param name="message">Message to be written</param>
    /// <param name="level">OutputLevel</param>
    /// <param name="foregroundColor">Console color for the output message</param>
    public static void Write(this IOutput output, string message, OutputLevel level, ConsoleColor foregroundColor)
    {
        SetColorForAction(foregroundColor, () => output.Write(message, level));
    }

    /// <summary>
    /// Formats the message.
    /// </summary>
    /// <param name="output">An output instance to write the message.</param>
    /// <param name="level">Message level.</param>
    /// <param name="messageTypeFormat">Format string for the message type.</param>
    /// <param name="format">Format string for the error message.</param>
    /// <param name="args">Arguments to format into the format string.</param>
    private static void Output(IOutput output, OutputLevel level, string messageTypeFormat, string format, params object?[]? args)
    {
        ValidateArg.NotNull(output, nameof(output));
        output.WriteLine(Format(messageTypeFormat, format, args), level);
    }

    /// <summary>
    /// Formats the message.
    /// </summary>
    /// <param name="messageTypeFormat">Format string for the message type.</param>
    /// <param name="format">Format string for the error message.</param>
    /// <param name="args">Arguments to format into the format string.</param>
    /// <returns>Formatted message.</returns>
    private static string Format(string messageTypeFormat, string format, params object?[]? args)
    {
        if (format == null || string.IsNullOrEmpty(format.Trim()))
        {
            throw new ArgumentException(Resources.CannotBeNullOrEmpty, nameof(format));
        }

        string message = args != null && args.Length > 0 ? string.Format(CultureInfo.CurrentCulture, format, args) : format;
        return string.Format(CultureInfo.CurrentCulture, messageTypeFormat, message);
    }

    private static void SetColorForAction(ConsoleColor foregroundColor, Action action)
    {
        if (action == null)
        {
            return;
        }

        var previousForegroundColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = foregroundColor;
            action.Invoke();
        }
        finally
        {
            Console.ForegroundColor = previousForegroundColor;
        }
    }
}
