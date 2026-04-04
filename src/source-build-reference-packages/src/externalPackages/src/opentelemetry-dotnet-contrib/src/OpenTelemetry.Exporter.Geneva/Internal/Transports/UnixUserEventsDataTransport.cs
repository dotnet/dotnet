// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET

using System.Diagnostics.Tracing;
using Microsoft.LinuxTracepoints.Provider;

namespace OpenTelemetry.Exporter.Geneva.Transports;

internal sealed class UnixUserEventsDataTransport : IDisposable
{
    public const string EventHeaderDynamicProviderName = "MicrosoftOpenTelemetryLogs";
    public const EventLevel LogsTracepointEventLevel = EventLevel.Informational;  // TODO: find the correct event level
    public const ulong LogsTracepointKeyword = 1;  // TODO: This will be a constant in the future. The actual value is TBD.

    private readonly EventHeaderDynamicProvider eventHeaderDynamicProvider;

    private UnixUserEventsDataTransport()
    {
        this.eventHeaderDynamicProvider = new EventHeaderDynamicProvider(EventHeaderDynamicProviderName);
    }

    public static UnixUserEventsDataTransport Instance { get; } = new();

    public EventHeaderDynamicTracepoint RegisterUserEventProviderForLogs()
    {
        var logsTracepoint = this.eventHeaderDynamicProvider.Register(LogsTracepointEventLevel, LogsTracepointKeyword);
        if (logsTracepoint.RegisterResult != 0)
        {
            // ENOENT (2): No such file or directory
            if (logsTracepoint.RegisterResult == 2)
            {
                throw new NotSupportedException(
                    $"Tracepoint registration for 'geneva_logs' failed with result: '{logsTracepoint.RegisterResult}'. Verify your distribution/kernel supports user_events: https://docs.kernel.org/trace/user_events.html.");
            }

            ExporterEventSource.Log.TransportInformation(
                nameof(UnixUserEventsDataTransport),
                $"Tracepoint registration operation for 'geneva_logs' returned result '{logsTracepoint.RegisterResult}' which is considered recoverable. Entering running state.");
        }

        return logsTracepoint;
    }

    public void Dispose()
    {
        this.eventHeaderDynamicProvider.Dispose();
    }
}

#endif
