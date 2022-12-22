// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Wrapper class for tracing.
///     - Shortcut-methods for Error, Warning, Info, Verbose.
///     - Adds additional information to the trace: calling process name, PID, ThreadID, Time.
///     - Uses custom switch <c>EqtTraceLevel</c> from .config file.
///     - By default tracing if OFF.
///     - Our build environment always sets the /d:TRACE so this class is always enabled,
///       the Debug class is enabled only in debug builds (/d:DEBUG).
///     - We ignore exceptions thrown by underlying TraceSwitch (e.g. due to config file error).
///       We log ignored exceptions to system Application log.
///       We pass through exceptions thrown due to incorrect arguments to <c>EqtTrace</c> methods.
/// Usage: <c>EqtTrace.Info("Here's how to trace info");</c>
/// </summary>
public static class EqtTrace
{
    private static readonly IPlatformEqtTrace TraceImpl = new PlatformEqtTrace();

#if NETFRAMEWORK
    public static void SetupRemoteEqtTraceListeners(AppDomain? childDomain)
    {
        TraceImpl.SetupRemoteEqtTraceListeners(childDomain);
    }

    public static void SetupListener(TraceListener? listener)
    {
        TraceImpl.SetupListener(listener);
    }

    public static TraceLevel TraceLevel
    {
        get
        {
            return (TraceLevel)TraceImpl.GetTraceLevel();
        }
        set
        {
            TraceImpl.SetTraceLevel((PlatformTraceLevel)value);
        }
    }

#endif

#if NETSTANDARD || NET || NETCOREAPP3_1
    public static PlatformTraceLevel TraceLevel
    {
        get
        {
            return TraceImpl.GetTraceLevel();
        }
        set
        {
            TraceImpl.SetTraceLevel(value);
        }
    }
#endif

    public static string? LogFile
    {
        get
        {
            return TraceImpl.GetLogFile();
        }
    }

    // There is a typo on this property but it's part of the public API so we cannot change it.
    public static bool DoNotInitailize
    {
        get
        {
            return TraceImpl.DoNotInitialize;
        }
        set
        {
            TraceImpl.DoNotInitialize = value;
        }
    }

    public static string? ErrorOnInitialization
    {
        get;
        set;
    }

    /// <summary>
    /// Gets a value indicating whether tracing error statements is enabled.
    /// </summary>
    public static bool IsErrorEnabled
    {
        get
        {
            return TraceImpl.ShouldTrace(PlatformTraceLevel.Error);
        }
    }

    /// <summary>
    /// Gets a value indicating whether tracing info statements is enabled.
    /// </summary>
    public static bool IsInfoEnabled
    {
        get
        {
            return TraceImpl.ShouldTrace(PlatformTraceLevel.Info);
        }
    }

    /// <summary>
    /// Gets a value indicating whether tracing verbose statements is enabled.
    /// </summary>
    public static bool IsVerboseEnabled
    {
        get
        {
            return TraceImpl.ShouldTrace(PlatformTraceLevel.Verbose);
        }
    }

    /// <summary>
    /// Gets a value indicating whether tracing warning statements is enabled.
    /// </summary>
    public static bool IsWarningEnabled
    {
        get
        {
            return TraceImpl.ShouldTrace(PlatformTraceLevel.Warning);
        }
    }

    /// <summary>
    /// Initializes the verbose tracing with custom log file
    /// And overrides if any trace is set before
    /// </summary>
    /// <param name="customLogFile">
    /// A custom log file for trace messages.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool InitializeVerboseTrace(string? customLogFile)
    {
        return InitializeTrace(customLogFile, PlatformTraceLevel.Verbose);
    }

    /// <summary>
    /// Initializes the tracing with custom log file and trace level.
    /// Overrides if any trace is set before.
    /// </summary>
    /// <param name="customLogFile">Custom log file for trace messages.</param>
    /// <param name="traceLevel">Trace level.</param>
    /// <returns>Trace initialized flag.</returns>
    public static bool InitializeTrace(string? customLogFile, PlatformTraceLevel traceLevel)
    {
        // Remove extra quotes if we get them passed on the parameter,
        // System.IO.File does not ignore them when checking the file existence.
        customLogFile = customLogFile?.Trim('"');
        if (!TraceImpl.InitializeTrace(customLogFile, traceLevel))
        {
            ErrorOnInitialization = PlatformEqtTrace.ErrorOnInitialization;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Prints an error message and prompts with a Debug dialog
    /// </summary>
    /// <param name="message">the error message</param>
    [Conditional("TRACE")]
    public static void Fail(string? message)
    {
        Error(message);
        FailDebugger(message);
    }

    /// <summary>
    /// Combines together <c>EqtTrace.Fail</c> and Debug.Fail:
    /// Prints an formatted error message and prompts with a Debug dialog.
    /// </summary>
    /// <param name="format">The formatted error message</param>
    /// <param name="args">Arguments to the format</param>
    [Conditional("TRACE")]
    public static void Fail(string format, params object?[] args)
    {
        string message = string.Format(CultureInfo.InvariantCulture, format, args);
        Fail(message);
    }

    /// <summary>
    /// Trace an error message.
    /// </summary>
    /// <param name="message">Error message.</param>
    [Conditional("TRACE")]
    public static void Error(string? message)
    {
        if (TraceImpl.ShouldTrace(PlatformTraceLevel.Error))
        {
            TraceImpl.WriteLine(PlatformTraceLevel.Error, message);
        }
    }

    /// <summary>
    /// Only prints the message if the condition is true
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="message">Trace error message.</param>
    [Conditional("TRACE")]
    public static void ErrorIf(bool condition, string? message)
    {
        if (condition)
        {
            Error(message);
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is false
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="message">Trace error message.</param>
    [Conditional("TRACE")]
    public static void ErrorUnless(bool condition, string? message)
    {
        ErrorIf(!condition, message);
    }

    /// <summary>
    /// Prints the message if the condition is false. If the condition is true,
    /// the message is instead printed at the specified trace level.
    /// </summary>
    /// <param name="condition">Condition for trace.</param>
    /// <param name="bumpLevel">Level for trace.</param>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void ErrorUnlessAlterTrace(bool condition, PlatformTraceLevel bumpLevel, string? message)
    {
        if (condition)
        {
            WriteAtLevel(bumpLevel, message);
        }
        else
        {
            Error(message);
        }
    }

    /// <summary>
    /// Trace an error message with formatting arguments.
    /// </summary>
    /// <param name="format">Format of error message.</param>
    /// <param name="args">Parameters for the error message.</param>
    [Conditional("TRACE")]
    public static void Error(string format, params object?[] args)
    {
        TPDebug.Assert(format != null, "format != null");

        // Check level before doing string.Format to avoid string creation if tracing is off.
        if (TraceImpl.ShouldTrace(PlatformTraceLevel.Error))
        {
            Error(string.Format(CultureInfo.InvariantCulture, format, args));
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is false
    /// </summary>
    /// <param name="condition">Condition for trace.</param>
    /// <param name="format">Message format.</param>
    /// <param name="args">Trace message format arguments.</param>
    [Conditional("TRACE")]
    public static void ErrorUnless(bool condition, string format, params object?[] args)
    {
        ErrorIf(!condition, format, args);
    }

    /// <summary>
    /// Prints the message if the condition is false. If the condition is true,
    /// the message is instead printed at the specified trace level.
    /// </summary>
    /// <param name="condition">Condition for trace.</param>
    /// <param name="bumpLevel">Level for trace.</param>
    /// <param name="format">Message format.</param>
    /// <param name="args">Trace message format arguments.</param>
    [Conditional("TRACE")]
    public static void ErrorUnlessAlterTrace(bool condition, PlatformTraceLevel bumpLevel, string format, params object?[] args)
    {
        if (condition)
        {
            WriteAtLevel(bumpLevel, format, args);
        }
        else
        {
            Error(format, args);
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is true
    /// </summary>
    /// <param name="condition">Condition for trace.</param>
    /// <param name="format">Message format.</param>
    /// <param name="args">Trace message format arguments.</param>
    [Conditional("TRACE")]
    public static void ErrorIf(bool condition, string format, params object?[] args)
    {
        if (condition)
        {
            Error(format, args);
        }
    }

    /// <summary>
    /// Error and Debug.Fail combined in one call.
    /// </summary>
    /// <param name="format">The message to send to Debug.Fail and Error.</param>
    /// <param name="args">Arguments to string.Format.</param>
    [Conditional("TRACE")]
    public static void ErrorAssert(string format, params object?[] args)
    {
        TPDebug.Assert(format != null, "format != null");
        var message = string.Format(CultureInfo.InvariantCulture, format, args);
        Error(message);
        FailDebugger(message);
    }

    /// <summary>
    /// Write a exception if tracing for error is enabled
    /// </summary>
    /// <param name="exceptionToTrace">The exception to write.</param>
    [Conditional("TRACE")]
    public static void Error(Exception exceptionToTrace)
    {
        TPDebug.Assert(exceptionToTrace != null, "exceptionToTrace != null");

        // Write only if tracing for error is enabled.
        // Done upfront to avoid performance hit.
        if (TraceImpl.ShouldTrace(PlatformTraceLevel.Error))
        {
            // Write at error level
            TraceImpl.WriteLine(PlatformTraceLevel.Error, FormatException(exceptionToTrace));
        }
    }

    /// <summary>
    /// Trace a warning message.
    /// </summary>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void Warning(string? message)
    {
        if (TraceImpl.ShouldTrace(PlatformTraceLevel.Warning))
        {
            TraceImpl.WriteLine(PlatformTraceLevel.Warning, message);
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is true
    /// </summary>
    /// <param name="condition">Condition to evaluate for tracing.</param>
    /// <param name="message">Message to trace.</param>
    [Conditional("TRACE")]
    public static void WarningIf(bool condition, string? message)
    {
        if (condition)
        {
            Warning(message);
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is false
    /// </summary>
    /// <param name="condition">Condition to evaluate for tracing.</param>
    /// <param name="message">Message to trace.</param>
    [Conditional("TRACE")]
    public static void WarningUnless(bool condition, string? message)
    {
        WarningIf(!condition, message);
    }

    /// <summary>
    /// Prints the message if the condition is false. If the condition is true,
    /// the message is instead printed at the specified trace level.
    /// </summary>
    /// <param name="condition">Condition to evaluate for tracing.</param>
    /// <param name="bumpLevel">Trace message level.</param>
    /// <param name="message">Message to trace.</param>
    [Conditional("TRACE")]
    public static void WarningUnlessAlterTrace(bool condition, PlatformTraceLevel bumpLevel, string? message)
    {
        if (condition)
        {
            WriteAtLevel(bumpLevel, message);
        }
        else
        {
            Warning(message);
        }
    }

    /// <summary>
    /// Trace a warning message.
    /// </summary>
    /// <param name="format">Format of the trace message.</param>
    /// <param name="args">Arguments for the trace message format.</param>
    [Conditional("TRACE")]
    public static void Warning(string format, params object?[] args)
    {
        TPDebug.Assert(format != null, "format != null");

        // Check level before doing string.Format to avoid string creation if tracing is off.
        if (TraceImpl.ShouldTrace(PlatformTraceLevel.Warning))
        {
            Warning(string.Format(CultureInfo.InvariantCulture, format, args));
        }
    }

    /// <summary>
    /// Trace a warning message based on a condition.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="format">Format of the trace message.</param>
    /// <param name="args">Arguments for the trace message.</param>
    [Conditional("TRACE")]
    public static void WarningIf(bool condition, string format, params object?[] args)
    {
        if (condition)
        {
            Warning(format, args);
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is false
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="format">Format of trace message.</param>
    /// <param name="args">Arguments for the trace message.</param>
    [Conditional("TRACE")]
    public static void WarningUnless(bool condition, string format, params object?[] args)
    {
        WarningIf(!condition, format, args);
    }

    /// <summary>
    /// Prints the message if the condition is false. If the condition is true,
    /// the message is instead printed at the specified trace level.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="bumpLevel">Level of trace message.</param>
    /// <param name="format">Format of the trace message.</param>
    /// <param name="args">Arguments for trace message.</param>
    [Conditional("TRACE")]
    public static void WarningUnlessAlterTrace(bool condition, PlatformTraceLevel bumpLevel, string format, params object?[] args)
    {
        if (condition)
        {
            WriteAtLevel(bumpLevel, format, args);
        }
        else
        {
            Warning(format, args);
        }
    }

    /// <summary>
    /// Trace an informational message.
    /// </summary>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void Info(string? message)
    {
        if (TraceImpl.ShouldTrace(PlatformTraceLevel.Info))
        {
            TraceImpl.WriteLine(PlatformTraceLevel.Info, message);
        }
    }

    /// <summary>
    /// Trace an informational message based on a condition.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void InfoIf(bool condition, string? message)
    {
        if (condition)
        {
            Info(message);
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is false
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void InfoUnless(bool condition, string? message)
    {
        InfoIf(!condition, message);
    }

    /// <summary>
    /// Prints the message if the condition is false. If the condition is true,
    /// the message is instead printed at the specified trace level.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="bumpLevel">Trace message level.</param>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void InfoUnlessAlterTrace(bool condition, PlatformTraceLevel bumpLevel, string? message)
    {
        if (condition)
        {
            WriteAtLevel(bumpLevel, message);
        }
        else
        {
            Info(message);
        }
    }

    /// <summary>
    /// Trace an informational message based on a format.
    /// </summary>
    /// <param name="format">Trace message format.</param>
    /// <param name="args">Arguments for trace format.</param>
    [Conditional("TRACE")]
    public static void Info(string format, params object?[] args)
    {
        TPDebug.Assert(format != null, "format != null");

        // Check level before doing string.Format to avoid string creation if tracing is off.
        if (TraceImpl.ShouldTrace(PlatformTraceLevel.Info))
        {
            Info(string.Format(CultureInfo.InvariantCulture, format, args));
        }
    }

    /// <summary>
    /// Trace an informational message based on a condition.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="format">Format of the trace message.</param>
    /// <param name="args">Arguments for the trace format.</param>
    [Conditional("TRACE")]
    public static void InfoIf(bool condition, string format, params object?[] args)
    {
        if (condition)
        {
            Info(format, args);
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is false
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="format">Trace message format.</param>
    /// <param name="args">Trace message format arguments.</param>
    [Conditional("TRACE")]
    public static void InfoUnless(bool condition, string format, params object?[] args)
    {
        InfoIf(!condition, format, args);
    }

    /// <summary>
    /// Prints the message if the condition is false. If the condition is true,
    /// the message is instead printed at the specified trace level.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="bumpLevel">Trace message level.</param>
    /// <param name="format">Trace message format.</param>
    /// <param name="args">Trace message arguments.</param>
    [Conditional("TRACE")]
    public static void InfoUnlessAlterTrace(bool condition, PlatformTraceLevel bumpLevel, string format, params object?[] args)
    {
        if (condition)
        {
            WriteAtLevel(bumpLevel, format, args);
        }
        else
        {
            Info(format, args);
        }
    }

    /// <summary>
    /// Trace a verbose message.
    /// </summary>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void Verbose(string? message)
    {
        if (TraceImpl.ShouldTrace(PlatformTraceLevel.Verbose))
        {
            TraceImpl.WriteLine(PlatformTraceLevel.Verbose, message);
        }
    }

    /// <summary>
    /// Trace a verbose message based on condition.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void VerboseIf(bool condition, string? message)
    {
        if (condition)
        {
            Verbose(message);
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is false
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void VerboseUnless(bool condition, string? message)
    {
        VerboseIf(!condition, message);
    }

    /// <summary>
    /// Prints the message if the condition is false. If the condition is true,
    /// the message is instead printed at the specified trace level.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="level">Trace message level.</param>
    /// <param name="message">Trace message.</param>
    [Conditional("TRACE")]
    public static void VerboseUnlessAlterTrace(bool condition, PlatformTraceLevel level, string? message)
    {
        if (condition)
        {
            WriteAtLevel(level, message);
        }
        else
        {
            Verbose(message);
        }
    }

    /// <summary>
    /// Trace a verbose message.
    /// </summary>
    /// <param name="format">Format of trace message.</param>
    /// <param name="args">Arguments for trace message.</param>
    [Conditional("TRACE")]
    public static void Verbose(string format, params object?[] args)
    {
        TPDebug.Assert(format != null, "format != null");

        // Check level before doing string.Format to avoid string creation if tracing is off.
        if (TraceImpl.ShouldTrace(PlatformTraceLevel.Verbose))
        {
            Verbose(string.Format(CultureInfo.InvariantCulture, format, args));
        }
    }

    /// <summary>
    /// Trace a verbose message based on a condition.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="format">Message format.</param>
    /// <param name="args">Arguments for trace message.</param>
    [Conditional("TRACE")]
    public static void VerboseIf(bool condition, string format, params object?[] args)
    {
        if (condition)
        {
            Verbose(format, args);
        }
    }

    /// <summary>
    /// Only prints the formatted message if the condition is false
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="format">Format for the trace message.</param>
    /// <param name="args">Trace message arguments.</param>
    [Conditional("TRACE")]
    public static void VerboseUnless(bool condition, string format, params object?[] args)
    {
        VerboseIf(!condition, format, args);
    }

    /// <summary>
    /// Prints the message if the condition is false. If the condition is true,
    /// the message is instead printed at the specified trace level.
    /// </summary>
    /// <param name="condition">Condition for tracing.</param>
    /// <param name="level">Trace message level.</param>
    /// <param name="format">Format of the trace message.</param>
    /// <param name="args">Arguments for the trace message format.</param>
    [Conditional("TRACE")]
    public static void VerboseUnlessAlterTrace(bool condition, PlatformTraceLevel level, string format, params object?[] args)
    {
        if (condition)
        {
            WriteAtLevel(level, format, args);
        }
        else
        {
            Verbose(format, args);
        }
    }

    /// <summary>
    /// Formats an exception into a nice looking message.
    /// </summary>
    /// <param name="exceptionToTrace">The exception to write.</param>
    /// <returns>The formatted string.</returns>
    private static string FormatException(Exception exceptionToTrace)
    {
        // Prefix for each line
        string prefix = Environment.NewLine + '\t';

        // Format this exception
        StringBuilder message = new();
        message.Append(
            string.Format(
                CultureInfo.InvariantCulture,
                "Exception: {0}{1}Message: {2}{3}Stack Trace: {4}",
                exceptionToTrace.GetType(),
                prefix,
                exceptionToTrace.Message,
                prefix,
                exceptionToTrace.StackTrace));

        // If there is base exception, add that to message
        if (exceptionToTrace.GetBaseException() != null)
        {
            message.Append(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}BaseExceptionMessage: {1}",
                    prefix,
                    exceptionToTrace.GetBaseException().Message));
        }

        // If there is inner exception, add that to message
        // We deliberately avoid recursive calls here.
        if (exceptionToTrace.InnerException != null)
        {
            // Format same as outer exception except
            // "InnerException" is prefixed to each line
            Exception inner = exceptionToTrace.InnerException;
            prefix += "InnerException";
            message.Append(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}: {1}{2} Message: {3}{4} Stack Trace: {5}",
                    prefix,
                    inner.GetType(),
                    prefix,
                    inner.Message,
                    prefix,
                    inner.StackTrace));

            if (inner.GetBaseException() != null)
            {
                message.Append(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}BaseExceptionMessage: {1}",
                        prefix,
                        inner.GetBaseException().Message));
            }
        }

        // Append a new line
        message.Append(Environment.NewLine);

        return message.ToString();
    }

    private static void WriteAtLevel(PlatformTraceLevel level, string? message)
    {
        switch (level)
        {
            case PlatformTraceLevel.Off:
                return;
            case PlatformTraceLevel.Error:
                Error(message);
                break;
            case PlatformTraceLevel.Warning:
                Warning(message);
                break;
            case PlatformTraceLevel.Info:
                Info(message);
                break;
            case PlatformTraceLevel.Verbose:
                Verbose(message);
                break;
            default:
                FailDebugger("We should never get here!");
                break;
        }
    }

    private static void WriteAtLevel(PlatformTraceLevel level, string format, params object?[] args)
    {
        TPDebug.Assert(format != null, "format != null");
        WriteAtLevel(level, string.Format(CultureInfo.InvariantCulture, format, args));
    }

    private static void FailDebugger(string? message)
    {
        Debug.Fail(message);
    }
}
