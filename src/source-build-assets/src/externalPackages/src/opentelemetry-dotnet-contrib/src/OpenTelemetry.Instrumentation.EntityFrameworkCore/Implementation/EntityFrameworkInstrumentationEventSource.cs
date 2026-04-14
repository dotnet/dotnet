// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.EntityFrameworkCore.Implementation;

[EventSource(Name = "OpenTelemetry-Instrumentation-EntityFrameworkCore")]
internal class EntityFrameworkInstrumentationEventSource : EventSource
{
    public static EntityFrameworkInstrumentationEventSource Log = new();

    [NonEvent]
    public void UnknownErrorProcessingEvent(string handlerName, string eventName, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
        {
            this.UnknownErrorProcessingEvent(handlerName, eventName, ex.ToInvariantString());
        }
    }

    [NonEvent]
    public void EnrichmentException(string eventName, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.EnrichmentException(eventName, ex.ToInvariantString());
        }
    }

    [NonEvent]
    public void CommandFilterException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.CommandFilterException(ex.ToInvariantString());
        }
    }

    [Event(1, Message = "Unknown error processing event '{1}' from handler '{0}', Exception: {2}", Level = EventLevel.Error)]
    public void UnknownErrorProcessingEvent(string handlerName, string eventName, string ex)
    {
        this.WriteEvent(1, handlerName, eventName, ex);
    }

    [Event(2, Message = "Current Activity is NULL the '{0}' callback. Span will not be recorded.", Level = EventLevel.Warning)]
    public void NullActivity(string eventName)
    {
        this.WriteEvent(2, eventName);
    }

    [Event(3, Message = "Payload is NULL in event '{1}' from handler '{0}', span will not be recorded.", Level = EventLevel.Warning)]
    public void NullPayload(string handlerName, string eventName)
    {
        this.WriteEvent(3, handlerName, eventName);
    }

    [Event(5, Message = "Enrichment threw exception. Exception {0}.", Level = EventLevel.Error)]
    public void EnrichmentException(string eventName, string exception)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.WriteEvent(5, eventName, exception);
        }
    }

    [Event(6, Message = "Command is filtered out. Activity {0}", Level = EventLevel.Verbose)]
    public void CommandIsFilteredOut(string activityName)
    {
        this.WriteEvent(6, activityName);
    }

    [Event(7, Message = "Command filter threw exception. Command will not be collected. Exception {0}.", Level = EventLevel.Error)]
    public void CommandFilterException(string exception)
    {
        this.WriteEvent(7, exception);
    }
}
