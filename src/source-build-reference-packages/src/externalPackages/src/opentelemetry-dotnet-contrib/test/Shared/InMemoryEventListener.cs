// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Diagnostics.Tracing;

namespace OpenTelemetry.Tests;

internal class InMemoryEventListener : EventListener
{
    private readonly EventSource eventSource;

    public InMemoryEventListener(EventSource eventSource, EventLevel minLevel = EventLevel.Verbose)
    {
        this.eventSource = eventSource;

        this.EnableEvents(eventSource, minLevel);
    }

    public ConcurrentQueue<EventWrittenEventArgs> Events { get; } = new();

    public override void Dispose()
    {
        this.DisableEvents(this.eventSource);

        base.Dispose();
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        this.Events.Enqueue(eventData);
    }
}
