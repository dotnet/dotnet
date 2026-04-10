// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Abstractions;

namespace Microsoft.IdentityModel.LoggingExtensions
{
    /// <summary>
    /// The default implementation of <see cref="IIdentityLogger"/> that provides a wrapper around <see cref="ILogger"/> instance.
    /// </summary>
    public class IdentityLoggerAdapter : IIdentityLogger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Instantiates <see cref="IdentityLoggerAdapter"/> using <paramref name="logger"/>.
        /// </summary>
        /// <param name="logger">An <see cref="ILogger"/> instance to which identity log messages are written.</param>

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public IdentityLoggerAdapter(ILogger logger)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public bool IsEnabled(EventLogLevel eventLogLevel)
        {
            return _logger.IsEnabled(ConvertToLogLevel(eventLogLevel));
        }

        /// <inheritdoc/>
        public void Log(LogEntry entry)
        {
            if (entry != null)
            {
                switch (entry.EventLogLevel)
                {
                    case EventLogLevel.Critical:
                        _logger.LogCritical(entry.Message);
                        break;

                    case EventLogLevel.Error:
                        _logger.LogError(entry.Message);
                        break;

                    case EventLogLevel.Warning:
                        _logger.LogWarning(entry.Message);
                        break;

                    case EventLogLevel.Informational:
                        _logger.LogInformation(entry.Message);
                        break;

                    case EventLogLevel.Verbose:
                        _logger.LogDebug(entry.Message);
                        break;

                    case EventLogLevel.LogAlways:
                        _logger.LogTrace(entry.Message);
                        break;

                    default:
                        break;
                }
            }
        }

        private static LogLevel ConvertToLogLevel(EventLogLevel eventLogLevel)
        {
            switch (eventLogLevel)
            {
                case EventLogLevel.Critical:
                    return LogLevel.Critical;

                case EventLogLevel.Error:
                    return LogLevel.Error;

                case EventLogLevel.Warning:
                    return LogLevel.Warning;

                case EventLogLevel.Informational:
                    return LogLevel.Information;

                case EventLogLevel.LogAlways:
                    return LogLevel.Trace;

                case EventLogLevel.Verbose:
                default:
                    return LogLevel.Debug;
            }
        }
    }
}
