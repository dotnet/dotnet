// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor.Logging;

internal static class ILoggerExtensions
{
    public static void LogTrace(this ILogger logger, string? message, params object?[] args)
    {
        Log(logger, LogLevel.Trace, exception: null, message, args);
    }

    public static void LogDebug(this ILogger logger, string? message, params object?[] args)
    {
        Log(logger, LogLevel.Debug, exception: null, message, args);
    }

    public static void LogInformation(this ILogger logger, string? message, params object?[] args)
    {
        Log(logger, LogLevel.Information, exception: null, message, args);
    }

    public static void LogWarning(this ILogger logger, string? message, params object?[] args)
    {
        Log(logger, LogLevel.Warning, exception: null, message, args);
    }

    public static void LogWarning(this ILogger logger, Exception exception, string? message, params object?[] args)
    {
        Log(logger, LogLevel.Warning, exception, message, args);
    }

    public static void LogError(this ILogger logger, string? message, params object?[] args)
    {
        Log(logger, LogLevel.Error, exception: null, message, args);
    }

    public static void LogError(this ILogger logger, Exception exception, string? message, params object?[] args)
    {
        Log(logger, LogLevel.Error, exception, message, args);
    }

    public static void LogCritical(this ILogger logger, Exception exception, string? message, params object?[] args)
    {
        Log(logger, LogLevel.Critical, exception, message, args);
    }

    private static void Log(ILogger logger, LogLevel level, Exception? exception, string? message, object?[] args)
    {
        logger.Log(level, (message, args), exception, FormatMessage);
    }

    private static string FormatMessage((string? message, object?[] args) s, Exception? exception)
    {
        var (format, args) = s;
        if (format is null)
        {
            return "[null]";
        }

        // from: https://github.com/dotnet/runtime/blob/ec4437be46d8b90bc9fa6740c556bd860d9fe5ab/src/libraries/Microsoft.Extensions.Logging.Abstractions/src/LogValuesFormatter.cs
        using var _ = StringBuilderPool.GetPooledObject(out var vsb);
        var scanIndex = 0;
        var endIndex = format.Length;

        var count = 0;

        while (scanIndex < endIndex)
        {
            var openBraceIndex = FindBraceIndex(format, '{', scanIndex, endIndex);
            if (scanIndex == 0 && openBraceIndex == endIndex)
            {
                // No holes found.
                return format;
            }

            var closeBraceIndex = FindBraceIndex(format, '}', openBraceIndex, endIndex);

            if (closeBraceIndex == endIndex)
            {
                vsb.Append(format, scanIndex, endIndex - scanIndex);
                scanIndex = endIndex;
            }
            else
            {
                // Format item syntax : { index[,alignment][ :formatString] }.
                var formatDelimiterIndex = format.AsSpan(openBraceIndex, closeBraceIndex - openBraceIndex).IndexOfAny(',', ':');
                formatDelimiterIndex = formatDelimiterIndex < 0 ? closeBraceIndex : formatDelimiterIndex + openBraceIndex;

                vsb.Append(format, scanIndex, openBraceIndex - scanIndex + 1);
                vsb.Append(count++);
                vsb.Append(format, formatDelimiterIndex, closeBraceIndex - formatDelimiterIndex + 1);

                scanIndex = closeBraceIndex + 1;
            }
        }

        return string.Format(vsb.ToString(), args);
    }

    private static int FindBraceIndex(string format, char brace, int startIndex, int endIndex)
    {
        // Example: {{prefix{{{Argument}}}suffix}}.
        var braceIndex = endIndex;
        var scanIndex = startIndex;
        var braceOccurrenceCount = 0;

        while (scanIndex < endIndex)
        {
            if (braceOccurrenceCount > 0 && format[scanIndex] != brace)
            {
                if (braceOccurrenceCount % 2 == 0)
                {
                    // Even number of '{' or '}' found. Proceed search with next occurrence of '{' or '}'.
                    braceOccurrenceCount = 0;
                    braceIndex = endIndex;
                }
                else
                {
                    // An unescaped '{' or '}' found.
                    break;
                }
            }
            else if (format[scanIndex] == brace)
            {
                if (brace == '}')
                {
                    if (braceOccurrenceCount == 0)
                    {
                        // For '}' pick the first occurrence.
                        braceIndex = scanIndex;
                    }
                }
                else
                {
                    // For '{' pick the last occurrence.
                    braceIndex = scanIndex;
                }

                braceOccurrenceCount++;
            }

            scanIndex++;
        }

        return braceIndex;
    }
}
