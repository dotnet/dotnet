// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Resources.Host;

[EventSource(Name = "OpenTelemetry-Resources-Host")]
internal class HostResourceEventSource : EventSource
{
    public static HostResourceEventSource Log = new();

    private const int EventIdFailedToExtractAttributes = 1;
    private const int EventIdProcessTimeout = 2;

    [NonEvent]
    public void ResourceAttributesExtractException(string format, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Warning, EventKeywords.All))
        {
            this.FailedToExtractResourceAttributes(format, ex.ToInvariantString());
        }
    }

    [Event(EventIdFailedToExtractAttributes, Message = "Failed to extract resource attributes in '{0}'.", Level = EventLevel.Warning)]
    public void FailedToExtractResourceAttributes(string format, string exception)
    {
        this.WriteEvent(EventIdFailedToExtractAttributes, format, exception);
    }

    [Event(EventIdProcessTimeout, Message = "Process timeout occurred: '{0}'", Level = EventLevel.Warning)]
    public void ProcessTimeout(string processName)
    {
        this.WriteEvent(EventIdProcessTimeout, processName);
    }
}
