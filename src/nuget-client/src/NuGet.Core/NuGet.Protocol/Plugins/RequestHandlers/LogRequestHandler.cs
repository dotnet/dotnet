// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request handler for logging.
    /// </summary>
    public sealed class LogRequestHandler : IRequestHandler
    {
        private ILogger _logger;
        private LogLevel _logLevel;

        /// <summary>
        /// Gets the <see cref="CancellationToken" /> for a request.
        /// </summary>
        public CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// Instantiates a new instance of the <see cref="LogRequestHandler" /> class.
        /// </summary>
        /// <param name="logger">A logger.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <see langword="null" />.</exception>
        public LogRequestHandler(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            SetLogger(logger);
        }

        /// <summary>
        /// Asynchronously handles responding to a request.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="request">A request message.</param>
        /// <param name="responseHandler">A response handler.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connection" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="responseHandler" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task HandleResponseAsync(
            IConnection connection,
            Message request,
            IResponseHandler responseHandler,
            CancellationToken cancellationToken)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (responseHandler == null)
            {
                throw new ArgumentNullException(nameof(responseHandler));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var logRequest = MessageUtilities.DeserializePayload<LogRequest>(request);
            MessageResponseCode responseCode;

            if (logRequest.LogLevel >= _logLevel)
            {
                Log(logRequest);

                responseCode = MessageResponseCode.Success;
            }
            else
            {
                responseCode = MessageResponseCode.Error;
            }

            var response = new LogResponse(responseCode);

            await responseHandler.SendResponseAsync(request, response, cancellationToken);
        }

        /// <summary>
        /// Sets the logger.
        /// </summary>
        /// <param name="logger">A logger.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <see langword="null" />.</exception>
        public void SetLogger(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (ReferenceEquals(_logger, logger))
            {
                return;
            }

            _logger = logger;
            _logLevel = GetLogLevel(logger);
        }

        /// <summary>
        /// Gets the log level of a logger.
        /// </summary>
        /// <param name="logger">A logger.</param>
        /// <returns>A log level.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <see langword="null" />.</exception>
        public static LogLevel GetLogLevel(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var loggerBase = logger as LoggerBase;

            if (loggerBase != null)
            {
                return loggerBase.VerbosityLevel;
            }

            return LogLevel.Information;
        }

        private void Log(LogRequest request)
        {
            switch (request.LogLevel)
            {
                case LogLevel.Debug:
                    _logger.LogDebug(request.Message);
                    break;

                case LogLevel.Verbose:
                    _logger.LogVerbose(request.Message);
                    break;

                case LogLevel.Information:
                    _logger.LogInformation(request.Message);
                    break;

                case LogLevel.Minimal:
                    _logger.LogMinimal(request.Message);
                    break;

                case LogLevel.Warning:
                    _logger.LogWarning(request.Message);
                    break;

                case LogLevel.Error:
                    _logger.LogError(request.Message);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
