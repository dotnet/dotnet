// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Logging;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.Threading;
using Roslyn.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

/// <summary>
/// ILogger implementation that logs via the window/logMessage LSP method
/// </summary>
internal class LspLogger(string categoryName, LogLevelProvider logLevelProvider, IClientConnection clientConnection) : ILogger
{
    private LogLevel LogLevel => logLevelProvider.Current;
    private readonly string _categoryName = categoryName;
    private readonly IClientConnection _clientConnection = clientConnection;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel.IsAtLeast(LogLevel);
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
            LogLevel.Debug => MessageType.Debug,
            LogLevel.Trace => MessageType.Log,
            _ => throw new NotImplementedException(),
        };

        var formattedMessage = LogMessageFormatter.FormatMessage(message, _categoryName, exception, includeTimeStamp: false);

        var @params = new LogMessageParams
        {
            MessageType = messageType,
            Message = formattedMessage,
        };

        _clientConnection.SendNotificationAsync(Methods.WindowLogMessageName, @params, CancellationToken.None).Forget();
    }
}
