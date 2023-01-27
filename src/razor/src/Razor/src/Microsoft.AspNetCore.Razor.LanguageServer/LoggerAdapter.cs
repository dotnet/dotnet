// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

// We unify the ILspLogger and ILogger systems here because the ILspLogger class does not match the ILogger class used by Razor,
// but we did not want to migrate them all at once
public class LoggerAdapter : IRazorLogger
{
    private readonly IEnumerable<ILogger> _loggers;
    private readonly ITelemetryReporter? _telemetryReporter;

    public LoggerAdapter(IEnumerable<ILogger> loggers, ITelemetryReporter? telemetryReporter)
    {
        _loggers = loggers;
        _telemetryReporter = telemetryReporter;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        var compositeDisposable = new CompositeDisposable();
        foreach (var logger in _loggers)
        {
            var disposable = logger.BeginScope(state);
            compositeDisposable.AddDisposable(disposable);
        }

        return compositeDisposable;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _loggers.Any(l => l.IsEnabled(logLevel));
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        foreach (var logger in _loggers)
        {
            if (logger.IsEnabled(logLevel))
            {
                logger.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }

    public void LogEndContext(string message, params object[] @params)
    {
        LogInformation("Exiting: {}", message);
    }

    public void LogError(string message, params object[] @params)
    {
#pragma warning disable CA2254 // Template should be a static expression
        foreach (var logger in _loggers)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(message, @params);
            }
        }

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
        foreach (var logger in _loggers)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(exception, message, @params);
            }
        }

        _telemetryReporter?.ReportFault(exception, message, @params);
    }

    public void LogInformation(string message, params object[] @params)
    {
        foreach (var logger in _loggers)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(message, @params);
            }
        }
    }

    public void LogDebug(string message, params object[] @params)
    {
        foreach (var logger in _loggers)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(message, @params);
            }
        }
    }

    public void LogStartContext(string message, params object[] @params)
    {
        LogInformation("Entering: {}", message);
    }

    public void LogWarning(string message, params object[] @params)
    {
        foreach (var logger in _loggers)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(message, @params);
            }
        }
#pragma warning restore CA2254 // Template should be a static expression
    }

    private class CompositeDisposable : IDisposable
    {
        private bool _disposed = false;
        private readonly IList<IDisposable> _disposables = new List<IDisposable>();

        public void AddDisposable(IDisposable disposable)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(CompositeDisposable));
            }

            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            _disposed = true;
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
