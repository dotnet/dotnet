// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

/// <summary>
/// Providers a bridge from CLaSP, which uses ILspLogger, to our logging infrastructure, which uses ILogger
/// </summary>
internal class ClaspLoggingBridge : ILspLogger
{
    public const string LogStartContextMarker = "[StartContext]";
    public const string LogEndContextMarker = "[EndContext]";

    private readonly ILogger _logger;
    private readonly ITelemetryReporter? _telemetryReporter;

    public ClaspLoggingBridge(IRazorLoggerFactory loggerFactory, ITelemetryReporter? telemetryReporter = null)
    {
        // We're creating this on behalf of CLaSP, because it doesn't know how to use IRazorLoggerFactory, so using that as the category name.
        _logger = loggerFactory.CreateLogger("CLaSP");
        _telemetryReporter = telemetryReporter;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return _logger.BeginScope<TState>(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public void LogStartContext(string message, params object[] @params)
    {
        // This is a special log message formatted so that the LogHub logger can detect it, and trigger the right trace event
        _logger.LogInformation(LogStartContextMarker + " {message}", message);
    }

    public void LogEndContext(string message, params object[] @params)
    {
        // This is a special log message formatted so that the LogHub logger can detect it, and trigger the right trace event
        _logger.LogInformation(LogEndContextMarker + " {message}", message);
    }

    public void LogError(string message, params object[] @params)
    {
#pragma warning disable CA2254 // Template should be a static expression
        if (_logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError(message, @params);
        }

        if (_telemetryReporter is not null)
        {
            var properties = new Property[@params.Length + 1];

            for (var i = 0; i < @params.Length; i++)
            {
                properties[i] = new("param" + i, @params[i]);
            }

            properties[^1] = new("message", message);

            _telemetryReporter.ReportEvent("lsperror", Severity.High, properties);
        }
    }

    public void LogException(Exception exception, string? message = null, params object[] @params)
    {
        if (_logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError(exception, message, @params);
        }

        _telemetryReporter?.ReportFault(exception, message, @params);
    }

    public void LogInformation(string message, params object[] @params)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(message, @params);
        }
    }

    public void LogDebug(string message, params object[] @params)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(message, @params);
        }
    }

    public void LogWarning(string message, params object[] @params)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning(message, @params);
        }
#pragma warning restore CA2254 // Template should be a static expression
    }
}
