// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LspLogger : IRazorLogger
{
    private readonly LogLevel _logLevel;
    private ClientNotifierServiceBase? _serviceBase;

    public LspLogger(Trace trace)
    {
        var logLevel = trace switch
        {
            Trace.Off => LogLevel.None,
            Trace.Messages => LogLevel.Information,
            Trace.Verbose => LogLevel.Trace,
            _ => throw new NotImplementedException(),
        };
        _logLevel = logLevel;
    }

    public void Initialize(ClientNotifierServiceBase serviceBase)
    {
        _serviceBase = serviceBase;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new Disposable();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (logLevel is LogLevel.None)
        {
            return;
        }

        var messageType = logLevel switch
        {
            LogLevel.Critical => MessageType.Error,
            LogLevel.Error => MessageType.Error,
            LogLevel.Warning => MessageType.Warning,
            LogLevel.Information => MessageType.Info,
            LogLevel.Debug => MessageType.Log,
            LogLevel.Trace => MessageType.Log,
            _ => throw new NotImplementedException(),
        };
        var message = formatter(state, exception);
        if (message == "[null]" && exception is not null)
        {
            message = exception.ToString();
        }

        var @params = new LogMessageParams
        {
            MessageType = messageType,
            Message = message,
        };

        if (_serviceBase is null)
        {
            throw new InvalidOperationException($"Tried to log before {nameof(ClientNotifierServiceBase)} was set.");
        }

        _ = _serviceBase.SendNotificationAsync(Methods.WindowLogMessageName, @params, CancellationToken.None);
    }

    // Not doing anything for now.
    public void LogStartContext(string message, params object[] @params)
    {
    }

    // Not doing anything for now.
    public void LogEndContext(string message, params object[] @params)
    {
    }

    public void LogError(string message, params object[] @params)
    {
#pragma warning disable CA2254 // Template should be a static expression
        ((ILogger)this).LogError(message, @params);
    }

    public void LogException(Exception exception, string? message = null, params object[] @params)
    {
        ((ILogger)this).LogError(exception, message, @params);
    }

    public void LogInformation(string message, params object[] @params)
    {
        ((ILogger)this).LogInformation(message, @params);
    }

    public void LogWarning(string message, params object[] @params)
    {
        ((ILogger)this).LogWarning(message, @params);
    }
#pragma warning restore CA2254 // Template should be a static expression

    private class Disposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
