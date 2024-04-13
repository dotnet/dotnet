// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;

namespace Microsoft.CodeAnalysis.Razor.Logging;

internal static class ILoggerExtensions
{
    public static void Log(this ILogger logger, LogLevel logLevel, [InterpolatedStringHandlerArgument(nameof(logger), "logLevel")] ref LogMessageInterpolatedStringHandler message)
    {
        if (message.IsEnabled)
        {
            logger.Log(logLevel, message.ToString(), exception: null);
        }
    }

    public static void LogTrace(this ILogger logger, [InterpolatedStringHandlerArgument(nameof(logger))] ref TraceLogMessageInterpolatedStringHandler message)
    {
        if (message.IsEnabled)
        {
            logger.Log(LogLevel.Trace, message.ToString(), exception: null);
        }
    }

    public static void LogDebug(this ILogger logger, [InterpolatedStringHandlerArgument(nameof(logger))] ref DebugLogMessageInterpolatedStringHandler message)
    {
        if (message.IsEnabled)
        {
            logger.Log(LogLevel.Debug, message.ToString(), exception: null);
        }
    }

    public static void LogInformation(this ILogger logger, [InterpolatedStringHandlerArgument(nameof(logger))] ref InformationLogMessageInterpolatedStringHandler message)
    {
        if (message.IsEnabled)
        {
            logger.Log(LogLevel.Information, message.ToString(), exception: null);
        }
    }

    public static void LogWarning(this ILogger logger, [InterpolatedStringHandlerArgument(nameof(logger))] ref WarningLogMessageInterpolatedStringHandler message)
    {
        if (message.IsEnabled)
        {
            logger.Log(LogLevel.Warning, message.ToString(), exception: null);
        }
    }

    public static void LogWarning(this ILogger logger, Exception? exception, [InterpolatedStringHandlerArgument(nameof(logger))] ref WarningLogMessageInterpolatedStringHandler message)
    {
        if (message.IsEnabled)
        {
            logger.Log(LogLevel.Warning, message.ToString(), exception);
        }
    }

    public static void LogError(this ILogger logger, [InterpolatedStringHandlerArgument(nameof(logger))] ref ErrorLogMessageInterpolatedStringHandler message)
    {
        if (message.IsEnabled)
        {
            logger.Log(LogLevel.Error, message.ToString(), exception: null);
        }
    }

    public static void LogError(this ILogger logger, Exception? exception, [InterpolatedStringHandlerArgument(nameof(logger))] ref ErrorLogMessageInterpolatedStringHandler message)
    {
        if (message.IsEnabled)
        {
            logger.Log(LogLevel.Error, message.ToString(), exception);
        }
    }

    public static void LogCritical(this ILogger logger, Exception? exception, [InterpolatedStringHandlerArgument(nameof(logger))] ref CriticalLogMessageInterpolatedStringHandler message)
    {
        if (message.IsEnabled)
        {
            logger.Log(LogLevel.Critical, message.ToString(), exception);
        }
    }
}
