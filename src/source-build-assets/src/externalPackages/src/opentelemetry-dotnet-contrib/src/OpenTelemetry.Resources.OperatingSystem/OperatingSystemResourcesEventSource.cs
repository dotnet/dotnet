// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Resources.OperatingSystem;

[EventSource(Name = "OpenTelemetry-Resources-OperatingSystem")]
internal sealed class OperatingSystemResourcesEventSource : EventSource
{
    public static OperatingSystemResourcesEventSource Log = new();

    private const int EventIdFailedToExtractAttributes = 1;
    private const int EventIdFailedToValidateValue = 2;
    private const int EventIdFailedToFindFile = 3;

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

    [Event(EventIdFailedToValidateValue, Message = "Failed to validate value. Details: '{0}'", Level = EventLevel.Warning)]
    public void FailedToValidateValue(string error)
    {
        this.WriteEvent(EventIdFailedToValidateValue, error);
    }

    [Event(EventIdFailedToFindFile, Message = "Process timeout occurred: '{0}'", Level = EventLevel.Warning)]
    public void FailedToFindFile(string error)
    {
        this.WriteEvent(EventIdFailedToFindFile, error);
    }
}
