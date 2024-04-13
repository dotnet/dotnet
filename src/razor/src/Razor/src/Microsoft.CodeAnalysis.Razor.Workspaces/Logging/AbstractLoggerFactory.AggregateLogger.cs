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

        public void Log(LogLevel logLevel, string message, Exception? exception)
        {
            foreach (var logger in _loggers)
            {
                if (logger.IsEnabled(logLevel))
                {
                    logger.Log(logLevel, message, exception);
                }
            }
        }

        internal void AddLogger(ILogger logger)
        {
            ImmutableInterlocked.Update(ref _loggers, (set, l) => set.Add(l), logger);
        }
    }
}
