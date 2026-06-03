// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Contrib.Instrumentation.Remoting.Implementation;

[EventSource(Name = "OpenTelemetry-Instrumentation-Remoting")]
internal class RemotingInstrumentationEventSource : EventSource
{
    public static readonly RemotingInstrumentationEventSource Log = new();

    [NonEvent]
    public void MessageFilterException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
        {
            this.MessageFilterException(ex.ToInvariantString());
        }
    }

    [Event(1, Message = "InstrumentationFilter threw an exception. Message will not be collected. Exception {0}.", Level = EventLevel.Error)]
    public void MessageFilterException(string exception)
    {
        this.WriteEvent(1, exception);
    }

    [NonEvent]
    public void DynamicSinkException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
        {
            this.DynamicSinkException(ex.ToInvariantString());
        }
    }

    [Event(2, Message = "DynamicSink message processor threw an exception. Remoting activity will not be recorded. Exception {0}.", Level = EventLevel.Error)]
    public void DynamicSinkException(string exception)
    {
        this.WriteEvent(2, exception);
    }

    [NonEvent]
    public void MethodMessageEnrichmentException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
        {
            this.MethodMessageEnrichmentException(ex.ToInvariantString());
        }
    }

    [Event(3, Message = "EnrichWithMethodMessage threw exception. Exception {0}.", Level = EventLevel.Error)]
    public void MethodMessageEnrichmentException(string exception) => this.WriteEvent(3, exception);

    [NonEvent]
    public void MethodReturnMessageEnrichmentException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
        {
            this.MethodReturnMessageEnrichmentException(ex.ToInvariantString());
        }
    }

    [Event(4, Message = "EnrichWithMethodReturnMessage threw exception. Exception {0}.", Level = EventLevel.Error)]
    public void MethodReturnMessageEnrichmentException(string exception) => this.WriteEvent(4, exception);
}
