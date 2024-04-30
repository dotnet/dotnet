// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Logging;

internal static class LogLevelExtensions
{
    public static bool IsAtLeast(this LogLevel target, LogLevel logLevel)
    {
        return target >= logLevel && target != LogLevel.None;
    }

    public static bool IsAtMost(this LogLevel target, LogLevel logLevel)
    {
        return target <= logLevel || target == LogLevel.None;
    }
}
