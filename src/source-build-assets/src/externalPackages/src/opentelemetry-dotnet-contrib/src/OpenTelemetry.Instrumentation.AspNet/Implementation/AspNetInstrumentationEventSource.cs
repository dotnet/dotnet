// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.AspNet.Implementation;

/// <summary>
/// EventSource events emitted from the project.
/// </summary>
[EventSource(Name = "OpenTelemetry-Instrumentation-AspNet")]
internal sealed class AspNetInstrumentationEventSource : EventSource, IConfigurationExtensionsLogger
{
    public static AspNetInstrumentationEventSource Log = new();

    [NonEvent]
    public void RequestFilterException(string operationName, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.RequestFilterException(operationName, ex.ToInvariantString());
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

    [Event(1, Message = "Request is filtered out and will not be collected. Operation='{0}'", Level = EventLevel.Verbose)]
    public void RequestIsFilteredOut(string operationName)
    {
        this.WriteEvent(1, operationName);
    }

    [Event(2, Message = "Filter callback threw an exception. Request will not be collected. Operation='{0}': {1}", Level = EventLevel.Error)]
    public void RequestFilterException(string operationName, string exception)
    {
        this.WriteEvent(2, operationName, exception);
    }

    [Event(3, Message = "Enrich callback threw an exception. Event='{0}': {1}", Level = EventLevel.Error)]
    public void EnrichmentException(string eventName, string exception)
    {
        this.WriteEvent(3, eventName, exception);
    }

    [Event(4, Message = "Configuration key '{0}' has an invalid value: '{1}'", Level = EventLevel.Warning)]
    public void InvalidConfigurationValue(string key, string value)
    {
        this.WriteEvent(4, key, value);
    }

    [Event(5, Message = "Activity is null in '{0}'. Tracing is not available.", Level = EventLevel.Warning)]
    public void NullActivity(string methodName)
    {
        this.WriteEvent(5, methodName);
    }

    void IConfigurationExtensionsLogger.LogInvalidConfigurationValue(string key, string value)
    {
        this.InvalidConfigurationValue(key, value);
    }
}
