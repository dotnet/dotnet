// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.Tracing;

namespace NuGet.VisualStudio.Etw
{
    public static class EventSourceExtensions
    {
        public static IDisposable StartStopEvent(this EventSource eventSource, string eventName)
        {
            return new EtwStartStopEvents(eventSource, eventName);
        }

        public static IDisposable StartStopEvent<T>(this EventSource eventSource, string eventName, T startEventData)
        {
            return new EtwStartStopEvents<T>(eventSource, eventName, startEventData);
        }

        private static EventSourceOptions StartEventOptions { get; } =
            new EventSourceOptions()
            {
                Opcode = EventOpcode.Start,
                Level = EventLevel.Informational,
                ActivityOptions = EventActivityOptions.Detachable
            };

        private static EventSourceOptions StopEventOptions { get; } =
            new EventSourceOptions()
            {
                Opcode = EventOpcode.Stop,
                Level = EventLevel.Informational,
                ActivityOptions = EventActivityOptions.Detachable
            };

        private struct EtwStartStopEvents : IDisposable
        {
            private readonly EventSource _eventSource;
            private readonly string _eventName;

            public EtwStartStopEvents(EventSource eventSource, string eventName)
            {
                _eventSource = eventSource ?? throw new ArgumentNullException(nameof(eventSource));
                _eventName = eventName ?? throw new ArgumentNullException(nameof(eventName));

                _eventSource.Write(_eventName, StartEventOptions);
            }

            public void Dispose()
            {
                _eventSource.Write(_eventName, StopEventOptions);
            }
        }

        private struct EtwStartStopEvents<T> : IDisposable
        {
            private readonly EventSource _eventSource;
            private readonly string _eventName;

            public EtwStartStopEvents(EventSource eventSource, string eventName, T startEventData)
            {
                _eventSource = eventSource ?? throw new ArgumentNullException(nameof(eventSource));
                _eventName = eventName ?? throw new ArgumentNullException(nameof(eventName));

                _eventSource.Write(_eventName, StartEventOptions, startEventData);
            }

            public void Dispose()
            {
                _eventSource.Write(_eventName, StopEventOptions);
            }
        }
    }
}
