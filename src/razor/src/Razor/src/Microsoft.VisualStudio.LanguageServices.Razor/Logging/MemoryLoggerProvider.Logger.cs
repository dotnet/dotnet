// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.VisualStudio.Razor.Logging;

internal partial class MemoryLoggerProvider
{
    private class Logger(Buffer buffer, string categoryName) : ILogger
    {
        private readonly Buffer _buffer = buffer;
        private readonly string _categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log(LogLevel logLevel, string message, Exception? exception)
        {
            _buffer.Append($"{DateTime.Now:h:mm:ss.fff} [{_categoryName}] {message}");
            if (exception is not null)
            {
                _buffer.Append(exception.ToString());
            }
        }
    }
}
