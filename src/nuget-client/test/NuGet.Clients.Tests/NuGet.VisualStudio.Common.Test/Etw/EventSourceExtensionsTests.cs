// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using FluentAssertions;
using NuGet.VisualStudio.Etw;
using Xunit;

namespace NuGet.VisualStudio.Common.Test.Etw
{
    public class EventSourceExtensionsTests
    {
        [Fact]
        public void StartStopEvents_WithoutEventData_RaisesBothStartAndStopEvents()
        {
            var eventSourceName = "NuGet-Test-" + Guid.NewGuid();
            using var listener = new TestListener(eventSourceName);

            string eventName = "SomeEvent";

            var eventSource = new EventSource(eventSourceName);
            using (eventSource.StartStopEvent(eventName))
            {
            }

            var events = listener.Events;
            events.Should().HaveCount(2);
            events.Select(e => e.EventName).All(n => n == eventName).Should().BeTrue();
            events.Should().Contain(e => e.Opcode == EventOpcode.Start);
            events.Should().Contain(e => e.Opcode == EventOpcode.Stop);
        }

        [Fact]
        public void StartStopEvents_WithEventData_RaisesBothStartAndStopEvents()
        {
            var eventSourceName = "NuGet-Test-" + Guid.NewGuid();
            using var listener = new TestListener(eventSourceName);

            string eventName = "SomeEvent";

            var eventSource = new EventSource(eventSourceName);
            var eventData = new
            {
                SomeProperty = "SomeValue"
            };
            using (eventSource.StartStopEvent(eventName, eventData))
            {
            }

            var events = listener.Events;
            events.Should().HaveCount(2);
            events.Select(e => e.EventName).All(n => n == eventName).Should().BeTrue();
            events.Should().Contain(e => e.Opcode == EventOpcode.Start && e.PayloadNames.Count > 0);
            events.Should().Contain(e => e.Opcode == EventOpcode.Stop);
        }

        private class TestListener : EventListener
        {
            private string _eventSourceName;
            private List<EventWrittenEventArgs> _events;

            public TestListener(string eventSourceName)
            {
                _eventSourceName = eventSourceName ?? throw new ArgumentNullException(nameof(eventSourceName));
                _events = new List<EventWrittenEventArgs>();
            }

            public IReadOnlyList<EventWrittenEventArgs> Events => _events;

            protected override void OnEventSourceCreated(EventSource eventSource)
            {
                if (eventSource.Name == _eventSourceName)
                {
                    EnableEvents(eventSource, EventLevel.LogAlways);
                }
            }

            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                _events.Add(eventData);
            }
        }
    }
}
