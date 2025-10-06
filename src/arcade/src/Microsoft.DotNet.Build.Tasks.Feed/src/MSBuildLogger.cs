// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class MSBuildLogger : Extensions.Logging.ILogger
    {
        private readonly TaskLoggingHelper _log;

        public MSBuildLogger(TaskLoggingHelper log)
        {
            _log = log;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            switch (logLevel)
            {
                case LogLevel.Critical:
                    _log.LogCriticalMessage(null, null, null, null, 0, 0, 0, 0, message);
                    break;
                case LogLevel.Error:
                    _log.LogError(message);
                    break;
                case LogLevel.Warning:
                    _log.LogWarning(message);
                    break;
                case LogLevel.Information:
                    _log.LogMessage(MessageImportance.High, message);
                    break;
                case LogLevel.Debug:
                    _log.LogMessage(MessageImportance.Normal, message);
                    break;
                case LogLevel.Trace:
                    _log.LogMessage(MessageImportance.Low, message);
                    break;
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }
    }

    /// <summary>
    /// An empty scope without any logic
    /// </summary>
    internal class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
