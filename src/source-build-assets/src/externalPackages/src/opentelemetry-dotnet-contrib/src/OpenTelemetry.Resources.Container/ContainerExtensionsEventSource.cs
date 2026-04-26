// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Resources.Container;

[EventSource(Name = "OpenTelemetry-Resources-Container")]
internal class ContainerExtensionsEventSource : EventSource
{
    public static ContainerExtensionsEventSource Log = new();

    [NonEvent]
    public void ExtractResourceAttributesException(string format, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
        {
            this.FailedToExtractResourceAttributes(format, ex.ToInvariantString());
        }
    }

    [Event(1, Message = "Failed to extract resource attributes in '{0}'.", Level = EventLevel.Error)]
    public void FailedToExtractResourceAttributes(string format, string exception)
    {
        this.WriteEvent(1, format, exception);
    }
}
