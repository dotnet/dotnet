// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Exporter.OneCollector;

[EventSource(Name = "OpenTelemetry-Exporter-OneCollector")]
internal sealed class OneCollectorExporterEventSource : EventSource
{
    public static OneCollectorExporterEventSource Log { get; } = new OneCollectorExporterEventSource();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsInformationalLoggingEnabled() => this.IsEnabled(EventLevel.Informational, EventKeywords.All);

    [NonEvent]
    public void WriteExportExceptionThrownEventIfEnabled(string itemType, Exception exception)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.ExportExceptionThrown(itemType, exception.ToInvariantString());
        }
    }

    [NonEvent]
    public void WriteSinkDataWrittenEventIfEnabled(string itemType, int numberOfRecords, string sinkDescription)
    {
        if (this.IsInformationalLoggingEnabled())
        {
            this.SinkDataWritten(itemType, numberOfRecords, sinkDescription);
        }
    }

    [NonEvent]
    public void WriteTransportExceptionThrownEventIfEnabled(string transportType, Exception exception)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.TransportExceptionThrown(transportType, exception.ToInvariantString());
        }
    }

    [NonEvent]
    public void WriteHttpTransportErrorResponseReceivedEventIfEnabled(string transportType, int statusCode, IEnumerable<string>? errorMessages, string? errorDetails)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.HttpTransportErrorResponseReceived(transportType, statusCode, errorMessages?.FirstOrDefault() ?? string.Empty, errorDetails ?? string.Empty);
        }
    }

    [NonEvent]
    public void WriteExceptionThrownFromUserCodeEventIfEnabled(string userCodeType, Exception exception)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.ExceptionThrownFromUserCode(userCodeType, exception.ToInvariantString());
        }
    }

    [Event(1, Message = "Exception thrown exporting '{0}' batch: {1}.", Level = EventLevel.Error)]
    public void ExportExceptionThrown(string itemType, string exception)
    {
        this.WriteEvent(1, itemType, exception);
    }

    [Event(2, Message = "Sent '{0}' batch of {1} item(s) to '{2}' transport.", Level = EventLevel.Informational)]
#if NET
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Parameters passed to WriteEvent are all primitive values.")]
#endif
    public void TransportDataSent(string itemType, int numberOfRecords, string transportDescription)
    {
        this.WriteEvent(2, itemType, numberOfRecords, transportDescription);
    }

    [Event(3, Message = "Wrote '{0}' batch of {1} item(s) to '{2}' sink.", Level = EventLevel.Informational)]
#if NET
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Parameters passed to WriteEvent are all primitive values.")]
#endif
    public void SinkDataWritten(string itemType, int numberOfRecords, string sinkDescription)
    {
        this.WriteEvent(3, itemType, numberOfRecords, sinkDescription);
    }

    [Event(4, Message = "Dropped {1} '{0}' item(s). {2} item(s) dropped during serialization. {3} item(s) dropped due to transmission failure.", Level = EventLevel.Warning)]
#if NET
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Parameters passed to WriteEvent are all primitive values.")]
#endif
    public void DataDropped(string itemType, int numberOfRecords, int numberOfRecordsDroppedDuringSerialization, int numberOfRecordsDroppedDuringTransmission)
    {
        this.WriteEvent(4, itemType, numberOfRecords, numberOfRecordsDroppedDuringSerialization, numberOfRecordsDroppedDuringTransmission);
    }

    [Event(5, Message = "Exception thrown by '{0}' transport: {1}", Level = EventLevel.Error)]
    public void TransportExceptionThrown(string transportType, string exception)
    {
        this.WriteEvent(5, transportType, exception);
    }

    [Event(6, Message = "Error response received by '{0}' transport. StatusCode: {1}, ErrorMessage: '{2}', ErrorDetails: '{3}'", Level = EventLevel.Error)]
#if NET
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Parameters passed to WriteEvent are all primitive values.")]
#endif
    public void HttpTransportErrorResponseReceived(string transportType, int statusCode, string errorMessage, string errorDetails)
    {
        this.WriteEvent(6, transportType, statusCode, errorMessage, errorDetails);
    }

    [Event(7, Message = "Event full name discarded. EventNamespace: '{0}', EventName: '{1}'", Level = EventLevel.Warning)]
    public void EventFullNameDiscarded(string eventNamespace, string eventName)
    {
        this.WriteEvent(7, eventNamespace, eventName);
    }

    [Event(8, Message = "Event namespace invalid. EventNamespace: '{0}'", Level = EventLevel.Warning)]
    public void EventNamespaceInvalid(string eventNamespace)
    {
        this.WriteEvent(8, eventNamespace);
    }

    [Event(9, Message = "Event name invalid. EventName: '{0}'", Level = EventLevel.Warning)]
    public void EventNameInvalid(string eventName)
    {
        this.WriteEvent(9, eventName);
    }

    [Event(10, Message = "Exception thrown by '{0}' user code: {1}", Level = EventLevel.Error)]
    public void ExceptionThrownFromUserCode(string userCodeType, string exception)
    {
        this.WriteEvent(10, userCodeType, exception);
    }

    [Event(11, Message = "Dropped {0} attribute '{1}': {2}", Level = EventLevel.Warning)]
    public void AttributeDropped(string itemType, string name, string reason)
    {
        this.WriteEvent(11, itemType, name, reason);
    }
}
