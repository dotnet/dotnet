// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Extensions.AWS;

[EventSource(Name = "OpenTelemetry-AWS-XRay")]
internal class AWSXRayEventSource : EventSource
{
    public static AWSXRayEventSource Log = new();

    [NonEvent]
    public void ActivityContextExtractException(string format, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Warning, (EventKeywords)(-1)))
        {
            this.FailedToExtractActivityContext(format, ex.ToInvariantString());
        }
    }

    [Event(1, Message = "Failed to extract activity context in format: '{0}', context: '{1}'.", Level = EventLevel.Warning)]
    public void FailedToExtractActivityContext(string format, string exception)
    {
        this.WriteEvent(1, format, exception);
    }

    [Event(2, Message = "Failed to inject activity context in format: '{0}', context: '{1}'.", Level = EventLevel.Warning)]
    public void FailedToInjectActivityContext(string format, string error)
    {
        this.WriteEvent(2, format, error);
    }
}
