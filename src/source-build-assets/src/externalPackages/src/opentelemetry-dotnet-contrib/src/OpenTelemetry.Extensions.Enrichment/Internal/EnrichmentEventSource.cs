// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Extensions.Enrichment;

[EventSource(Name = "OpenTelemetry-Extensions-Enrichment")]
internal sealed class EnrichmentEventSource : EventSource
{
    public static EnrichmentEventSource Log = new();

    private EnrichmentEventSource()
    {
    }

    [NonEvent]
    public void TraceEnricherException(string operationName, TraceEnricher enricher, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Warning, EventKeywords.All))
        {
            var enricherType = enricher.GetType();
            this.TraceEnricherException(operationName, enricherType.FullName ?? enricherType.Name, ex.ToInvariantString());
        }
    }

    [NonEvent]
    public void TraceEnrichmentActionException(Action<TraceEnrichmentBag> action, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Warning, EventKeywords.All))
        {
            var method = action.Method;
            var declaringType = method.DeclaringType;
            var actionName = declaringType is null
                ? method.Name
                : $"{declaringType.FullName ?? declaringType.Name}.{method.Name}";

            this.TraceEnrichmentActionException(actionName, ex.ToInvariantString());
        }
    }

    [Event(1, Message = "Trace enricher '{0}' threw during '{1}'. Trace processing will continue. Exception: '{2}'.", Level = EventLevel.Warning)]
    public void TraceEnricherException(string enricherType, string operationName, string exception)
    {
        this.WriteEvent(1, enricherType, operationName, exception);
    }

    [Event(2, Message = "Trace enrichment action '{0}' threw. Trace processing will continue. Exception: '{1}'.", Level = EventLevel.Warning)]
    public void TraceEnrichmentActionException(string actionName, string exception)
    {
        this.WriteEvent(2, actionName, exception);
    }
}
