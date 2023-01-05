// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

// We unify the ILspLogger and ILogger systems here because the ILspLogger class does not match the ILogger class used by Razor,
// but we did not want to migrate them all at once
public class LoggerAdapter : IRazorLogger
{
    private readonly ILogger _logger;
    private readonly ITelemetryReporter? _telemetryReporter;

    public LoggerAdapter(ILogger logger, ITelemetryReporter? telemetryReporter)
    {
        _logger = logger;
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
        _logger.Log<TState>(logLevel, eventId, state, exception, formatter);
    }

    public void LogEndContext(string message, params object[] @params)
    {
        _logger.LogInformation("Exiting: {}", message);
    }

    public void LogError(string message, params object[] @params)
    {
#pragma warning disable CA2254 // Template should be a static expression
        _logger.LogError(message, @params);

        if (_telemetryReporter is not null)
        {
            using var _ = DictionaryPool<string, object>.GetPooledObject(out var props);

            var index = 0;
            foreach (var param in @params)
            {
                props.Add("param" + index++, param);
            }

            props.Add("message", message);
            _telemetryReporter.ReportEvent("lsperror", VisualStudio.Telemetry.TelemetrySeverity.High, props.ToImmutableDictionary());
        }
    }

    public void LogException(Exception exception, string? message = null, params object[] @params)
    {
        _logger.LogError(exception, message, @params);
        _telemetryReporter?.ReportFault(exception, message, @params);
    }

    public void LogInformation(string message, params object[] @params)
    {
        _logger.LogInformation(message, @params);
    }

    public void LogStartContext(string message, params object[] @params)
    {
        _logger.LogInformation("Entering: {}", message);
    }

    public void LogWarning(string message, params object[] @params)
    {
        _logger.LogWarning(message, @params);
#pragma warning restore CA2254 // Template should be a static expression
    }
}