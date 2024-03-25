// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Logging;

internal partial class MemoryLoggerProvider
{
    private class Logger(Buffer buffer, string categoryName) : ILogger
    {
        private readonly Buffer _buffer = buffer;
        private readonly string _categoryName = categoryName;

        public IDisposable BeginScope<TState>(TState state)
            => Scope.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _buffer.Append($"{DateTime.Now:h:mm:ss.fff} [{_categoryName}] {formatter(state, exception)}");
            if (exception is not null)
            {
                _buffer.Append(exception.ToString());
            }
        }

        private class Scope : IDisposable
        {
            public static Scope Instance = new();

            public void Dispose()
            {
            }
        }
    }
}
