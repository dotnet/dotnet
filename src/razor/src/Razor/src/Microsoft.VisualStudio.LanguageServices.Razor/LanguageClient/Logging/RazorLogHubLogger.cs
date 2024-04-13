// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.Razor.Logging;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Logging;

internal sealed class RazorLogHubLogger : ILogger
{
    private string _categoryName;
    private RazorLogHubTraceProvider _traceProvider;

    public RazorLogHubLogger(string categoryName, RazorLogHubTraceProvider traceProvider)
    {
        _categoryName = categoryName;
        _traceProvider = traceProvider;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log(LogLevel logLevel, string message, Exception? exception)
    {
        var traceSource = _traceProvider.TryGetTraceSource();
        if (traceSource is null)
        {
            // We can't log if there is no trace source to log to
            return;
        }

        switch (logLevel)
        {
            // We separate out Information because we want to check for specific log messages set from CLaSP
            case LogLevel.Information:
                // The category for start and stop will only ever be "CLaSP" so no point logging it
                if (message.StartsWith(ClaspLoggingBridge.LogStartContextMarker))
                {
                    traceSource.TraceEvent(TraceEventType.Start, id: 0, "{0}", message);
                }
                else if (message.StartsWith(ClaspLoggingBridge.LogEndContextMarker))
                {
                    traceSource.TraceEvent(TraceEventType.Stop, id: 0, "{0}", message);
                }
                else
                {
                    traceSource.TraceEvent(TraceEventType.Information, id: 0, "[{0}] {1}", _categoryName, message);
                }

                break;

            case LogLevel.Trace:
            case LogLevel.Debug:
            case LogLevel.None:
                traceSource.TraceEvent(TraceEventType.Information, id: 0, "[{0}] {1}", _categoryName, message);
                break;
            case LogLevel.Warning:
                traceSource.TraceEvent(TraceEventType.Warning, id: 0, "[{0}] {1}", _categoryName, message);
                break;
            case LogLevel.Error:
            case LogLevel.Critical:
                traceSource.TraceEvent(TraceEventType.Error, id: 0, "[{0}] {1} {2}", _categoryName, message, exception!);
                break;
        }
    }
}
