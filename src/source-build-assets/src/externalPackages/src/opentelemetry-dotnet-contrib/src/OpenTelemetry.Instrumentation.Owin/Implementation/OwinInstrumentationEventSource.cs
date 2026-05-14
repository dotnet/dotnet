// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Owin;

/// <summary>
/// EventSource events emitted from the project.
/// </summary>
[EventSource(Name = "OpenTelemetry-Instrumentation-Owin")]
internal sealed class OwinInstrumentationEventSource : EventSource, IConfigurationExtensionsLogger
{
    public static OwinInstrumentationEventSource Log { get; } = new OwinInstrumentationEventSource();

    [NonEvent]
    public void RequestFilterException(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
        {
            this.RequestFilterException(ex.ToInvariantString());
        }
    }

    [Event(EventIds.RequestIsFilteredOut, Message = "Request is filtered out.", Level = EventLevel.Verbose)]
    public void RequestIsFilteredOut()
    {
        this.WriteEvent(EventIds.RequestIsFilteredOut);
    }

    [Event(EventIds.RequestFilterException, Message = "InstrumentationFilter threw exception. Request will not be collected. Exception {0}.", Level = EventLevel.Error)]
    public void RequestFilterException(string exception)
    {
        this.WriteEvent(EventIds.RequestFilterException, exception);
    }

    [NonEvent]
    public void EnrichmentException(Exception exception)
    {
        if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
        {
            this.EnrichmentException(exception.ToInvariantString());
        }
    }

    [Event(EventIds.EnrichmentException, Message = "Enrichment threw exception. Exception {0}.", Level = EventLevel.Error)]
    public void EnrichmentException(string exception)
    {
        this.WriteEvent(EventIds.EnrichmentException, exception);
    }

    [Event(EventIds.InvalidConfigurationValue, Message = "Configuration key '{0}' has an invalid value: '{1}'", Level = EventLevel.Warning)]
    public void InvalidConfigurationValue(string key, string value)
    {
        this.WriteEvent(EventIds.InvalidConfigurationValue, key, value);
    }

    void IConfigurationExtensionsLogger.LogInvalidConfigurationValue(string key, string value)
    {
        this.InvalidConfigurationValue(key, value);
    }

    private class EventIds
    {
        public const int RequestIsFilteredOut = 1;
        public const int RequestFilterException = 2;
        public const int EnrichmentException = 3;
        public const int InvalidConfigurationValue = 4;
    }
}
