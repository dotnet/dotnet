// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.Logging;

internal interface ILogger
{
    void Log(LogLevel logLevel, string message, Exception? exception);

    bool IsEnabled(LogLevel logLevel);
}
