// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.Logging;

internal sealed partial class EmptyLoggingFactory
{
    private sealed class EmptyLogger : ILogger
    {
        public static readonly EmptyLogger Instance = new();

        private EmptyLogger()
        {
        }

        public bool IsEnabled(LogLevel logLevel) => false;

        public void Log(LogLevel logLevel, string message, Exception? exception)
        {
        }
    }
}
