// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Diagnostics.Tracing;
using System.Linq;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    public class RazorEventListener : EventListener
    {
        private const string EventSourceName = "Microsoft-DotNet-SDK-Razor-SourceGenerator";

        private readonly ImmutableArray<RazorEvent>.Builder _events = ImmutableArray.CreateBuilder<RazorEvent>();

        protected override void OnEventSourceCreated(EventSource source)
        {
            if (source.Name == EventSourceName)
            {
                EnableEvents(source, EventLevel.Informational);
            }
        }

        public void Clear()
        {
            lock (_events)
            {
                _events.Clear();
            }
        }

        public ImmutableArray<RazorEvent> Events
        {
            get
            {
                lock (_events)
                {
                    return _events.ToImmutable();
                }
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData.EventSource.Name != EventSourceName)
            {
                return;
            }

            var @event = new RazorEvent
            {
                EventId = eventData.EventId,
                EventName = eventData.EventName,
                Payload = eventData.Payload.ToArray(),
            };

            lock (_events)
            {
                _events.Add(@event);
            }
        }

        public sealed class RazorEvent
        {
            public int EventId { get; init; }

            public string EventName { get; init; }

            public object[] Payload { get; init; }
        }
    }
}
