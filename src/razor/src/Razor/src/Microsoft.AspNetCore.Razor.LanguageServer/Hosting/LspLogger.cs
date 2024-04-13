// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

/// <summary>
/// ILogger implementation that logs via the window/logMessage LSP method
/// </summary>
internal class LspLogger : ILogger
{
    private readonly LogLevel _logLevel;
    private readonly string _categoryName;
    private IClientConnection _clientConnection;

    public LspLogger(string categoryName, LogLevel logLevel, IClientConnection clientConnection)
    {
        _logLevel = logLevel;
        _categoryName = categoryName;
        _clientConnection = clientConnection;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _logLevel;
    }

    public void Log(LogLevel logLevel, string message, Exception? exception)
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
        if (exception is not null)
        {
            message += Environment.NewLine + exception.ToString();
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
}
