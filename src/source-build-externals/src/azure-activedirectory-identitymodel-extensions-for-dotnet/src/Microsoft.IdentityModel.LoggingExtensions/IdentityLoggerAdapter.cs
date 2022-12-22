//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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
                    case EventLogLevel.LogAlways:
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
                case EventLogLevel.LogAlways:
                    return LogLevel.Critical;

                case EventLogLevel.Error:
                    return LogLevel.Error;

                case EventLogLevel.Warning:
                    return LogLevel.Warning;

                case EventLogLevel.Informational:
                    return LogLevel.Information;

                case EventLogLevel.Verbose:
                default:
                    return LogLevel.Debug;
            }
        }
    }
}
