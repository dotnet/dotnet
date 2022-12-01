// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class ILoggerExtensions
{
    public static bool TestOnlyLoggingEnabled = false;

    [Conditional("DEBUG")]
    public static void LogTestOnly(this ILogger logger, string message, params object?[] args)
    {
        if (TestOnlyLoggingEnabled)
        {
#pragma warning disable CA2254 // Template should be a static expression
            // This is test-only, so we don't mind losing structured logging for it.
            logger.LogDebug(message, args);
#pragma warning restore CA2254 // Template should be a static expression
        }
    }
}
