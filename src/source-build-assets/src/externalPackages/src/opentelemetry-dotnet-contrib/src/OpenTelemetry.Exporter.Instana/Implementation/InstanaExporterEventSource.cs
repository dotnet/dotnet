// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Exporter.Instana.Implementation;

[EventSource(Name = "OpenTelemetry-Exporter-Instana")]
internal sealed class InstanaExporterEventSource : EventSource
{
    public static InstanaExporterEventSource Log = new();

    private InstanaExporterEventSource()
    {
    }

    [NonEvent]
    public void FailedExport(Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.FailedExport(ex.ToInvariantString());
        }
    }

    [Event(1, Message = "Failed to send spans: '{0}'", Level = EventLevel.Error)]
    public void FailedExport(string exception)
    {
        this.WriteEvent(1, exception);
    }
}
