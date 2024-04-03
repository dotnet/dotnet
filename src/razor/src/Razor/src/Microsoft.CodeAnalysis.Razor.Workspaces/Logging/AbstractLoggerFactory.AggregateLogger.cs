// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;

namespace Microsoft.CodeAnalysis.Razor.Logging;

internal abstract partial class AbstractLoggerFactory
{
    private class AggregateLogger(ImmutableArray<ILogger> loggers) : ILogger
    {
        private ImmutableArray<ILogger> _loggers = loggers;

        public bool IsEnabled(LogLevel logLevel)
        {
            foreach (var logger in _loggers)
            {
                if (logger.IsEnabled(logLevel))
                {
                    return true;
                }
            }

            return false;
        }

        public void Log<TState>(LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            foreach (var logger in _loggers)
            {
                if (logger.IsEnabled(logLevel))
                {
                    logger.Log(logLevel, state, exception, formatter);
                }
            }
        }

        internal void AddLogger(ILogger logger)
        {
            ImmutableInterlocked.Update(ref _loggers, (set, l) => set.Add(l), logger);
        }
    }
}
