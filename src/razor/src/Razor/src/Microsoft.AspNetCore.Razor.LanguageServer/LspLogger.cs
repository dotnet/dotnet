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
    private readonly string _categoryName;
    private IClientConnection? _clientConnection;

    public LspLogger(string categoryName, LogLevel logLevel, IClientConnection? clientConnection)
    {
        _logLevel = logLevel;
        _categoryName = categoryName;
        _clientConnection = clientConnection;
    }

    public void Initialize(IClientConnection clientConnection)
    {
        _clientConnection = clientConnection;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new Disposable();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _logLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
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
            Message = $"[{_categoryName}] {message}",
        };

        if (_clientConnection is null)
        {
            throw new InvalidOperationException($"Tried to log before {nameof(IClientConnection)} was set.");
        }

        _ = _clientConnection.SendNotificationAsync(Methods.WindowLogMessageName, @params, CancellationToken.None);
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

    internal IClientConnection? GetClientConnection()
    {
        return _clientConnection;
    }
#pragma warning restore CA2254 // Template should be a static expression

    private class Disposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
