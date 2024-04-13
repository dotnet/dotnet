// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Remote.Razor.Logging;

internal partial class RemoteLoggerFactory
{
    private class Logger(string categoryName) : ILogger
    {
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log(LogLevel logLevel, string message, Exception? exception)
        {
            if (s_traceSource is null)
            {
                // We can't log if there is no trace source to log to
                return;
            }

            switch (logLevel)
            {
                case LogLevel.Information:
                    s_traceSource.TraceEvent(TraceEventType.Information, id: 0, "[{0}] {1}", categoryName, message);
                    break;
                case LogLevel.Trace:
                case LogLevel.Debug:
                    s_traceSource.TraceEvent(TraceEventType.Verbose, id: 0, "[{0}] {1}", categoryName, message);
                    break;
                case LogLevel.Warning:
                    s_traceSource.TraceEvent(TraceEventType.Warning, id: 0, "[{0}] {1}", categoryName, message);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    s_traceSource.TraceEvent(TraceEventType.Error, id: 0, "[{0}] {1} {2}", categoryName, message, exception!);
                    break;
            }
        }
    }
}
