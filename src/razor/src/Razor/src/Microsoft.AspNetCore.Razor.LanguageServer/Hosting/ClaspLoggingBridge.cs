// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

/// <summary>
/// Providers a bridge from CLaSP, which uses ILspLogger, to our logging infrastructure, which uses ILogger
/// </summary>
internal partial class ClaspLoggingBridge : ILspLogger
{
    public const string LogStartContextMarker = "[StartContext]";
    public const string LogEndContextMarker = "[EndContext]";

    private readonly ILogger _logger;
    private readonly ITelemetryReporter? _telemetryReporter;

    public ClaspLoggingBridge(ILoggerFactory loggerFactory, ITelemetryReporter? telemetryReporter = null)
    {
        // We're creating this on behalf of CLaSP, because it doesn't know how to use our ILoggerFactory, so using that as the category name.
        _logger = loggerFactory.GetOrCreateLogger("CLaSP");
        _telemetryReporter = telemetryReporter;
    }

    public void LogError(string message, params object[] @params)
    {
        _logger.LogError($"{message}: {string.Join(",", @params)}");

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
        _logger.LogError(exception, $"{message}: {string.Join(",", @params)}");

        _telemetryReporter?.ReportFault(exception, message, @params);
    }

    public void LogInformation(string message, params object[] @params)
    {
        _logger.LogInformation($"{message}: {string.Join(",", @params)}");
    }

    public void LogDebug(string message, params object[] @params)
    {
        _logger.LogDebug($"{message}: {string.Join(",", @params)}");
    }

    public void LogWarning(string message, params object[] @params)
    {
        _logger.LogWarning($"{message}: {string.Join(",", @params)}");
    }

    public IDisposable? CreateContext(string context)
    {
        return new LspLoggingScope(context, _logger);
    }

    public IDisposable? CreateLanguageContext(string? language)
    {
        // We don't support hosting other languages in our LSP server
        return null;
    }
}
