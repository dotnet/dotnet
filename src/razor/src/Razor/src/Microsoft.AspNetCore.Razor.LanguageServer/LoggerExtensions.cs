// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class LoggerExtensions
{
    public static IRazorLogger ToRazorLogger(this ILogger logger)
        => logger is IRazorLogger razorLogger
            ? razorLogger
            : new LoggerAdapter(logger);
}
