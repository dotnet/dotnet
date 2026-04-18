// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry;

/// <summary>
/// EventSource implementation for OpenTelemetry SDK extensions implementation.
/// </summary>
[EventSource(Name = "OpenTelemetry-Extensions")]
internal sealed class OpenTelemetryExtensionsEventSource : EventSource
{
    public static OpenTelemetryExtensionsEventSource Log = new OpenTelemetryExtensionsEventSource();

    [NonEvent]
    public void LogProcessorException(string @event, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
        {
            this.LogProcessorException(@event, ex.ToInvariantString());
        }
    }

    [Event(1, Message = "Unknown error in LogProcessor event '{0}': '{1}'.", Level = EventLevel.Error)]
    public void LogProcessorException(string @event, string exception)
    {
        this.WriteEvent(1, @event, exception);
    }

    [NonEvent]
    public void LogRecordFilterException(string? categoryName, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Warning, (EventKeywords)(-1)))
        {
            this.LogRecordFilterException(categoryName, ex.ToInvariantString());
        }
    }

    [Event(2, Message = "Filter threw an exception, log record will not be attached to an activity, the log record would flow to its pipeline unaffected. CategoryName: '{0}', Exception: {1}.", Level = EventLevel.Warning)]
    public void LogRecordFilterException(string? categoryName, string exception)
    {
        this.WriteEvent(2, categoryName, exception);
    }

    [Event(3, Message = "Baggage key predicate threw exeption when trying to add baggage entry with key '{0}'. Baggage entry will not be added to the activity. Exception: '{1}'", Level = EventLevel.Warning)]
    public void BaggageKeyActivityPredicateException(string baggageKey, string exception)
    {
        this.WriteEvent(3, baggageKey, exception);
    }

    [Event(4, Message = "Baggage key predicate threw exeption when trying to add baggage entry with key '{0}'. Baggage entry will not be added to the log record. Exception: '{1}'", Level = EventLevel.Warning)]
    public void BaggageKeyLogRecordPredicateException(string baggageKey, string exception)
    {
        this.WriteEvent(4, baggageKey, exception);
    }
}
