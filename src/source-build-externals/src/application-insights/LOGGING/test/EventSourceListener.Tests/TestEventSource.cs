//-----------------------------------------------------------------------
// <copyright file="TestEventSource.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EventSourceListener.Tests
{
    using System;
    using System.Diagnostics.Tracing;

    [EventSource(Name = TestEventSource.ProviderName)]
    internal class TestEventSource: EventSource
    {
        public const string ProviderName = "Microsoft-ApplicationInsights-Extensibility-EventSourceListener-Tests";

        public static readonly TestEventSource Default = new TestEventSource();

        public const int InfoEventId = 1;

        [Event(InfoEventId, Level = EventLevel.Informational, Message = "{0}", Keywords = Keywords.Routine)]
        public void InfoEvent(string information)
        {
            WriteEvent(InfoEventId, information);
        }

        public const int WarningEventId = 2;

        [Event(WarningEventId, Level = EventLevel.Warning, Message = "Warning!", Keywords = Keywords.NonRoutine)]
        public void WarningEvent(int i1, int i2)
        {
            WriteEvent(WarningEventId, i1, i2);
        }

        public const int ErrorEventId = 3;

        [Event(ErrorEventId, Level = EventLevel.Error, Message = "Error!", Keywords = Keywords.NonRoutine)]
        public void ErrorEvent(double value, string context)
        {
            WriteEvent(ErrorEventId, value, context);
        }

        public const int ComplexEventId = 4;

        [Event(ComplexEventId, Level = EventLevel.Verbose, Message = "Blah blah", Keywords = Keywords.Routine, 
            Channel = EventChannel.Debug, Opcode = EventOpcode.Extension, Tags = (EventTags)17, Task = (EventTask)32)]
        public void ComplexEvent(Guid uniqueId)
        {
            WriteEvent(ComplexEventId, uniqueId);
        }

        public const int RequestStartEventId = 5;

        [Event(RequestStartEventId, Level = EventLevel.Informational, ActivityOptions = EventActivityOptions.Recursive)]
        public void RequestStart(int requestId)
        {
            WriteEvent(RequestStartEventId, requestId);
        }

        public const int RequestStopEventId = 6;

        [Event(RequestStopEventId, Level = EventLevel.Informational, ActivityOptions = EventActivityOptions.Recursive)]
        public void RequestStop(int requestId)
        {
            WriteEvent(RequestStopEventId, requestId);
        }

        public const int TrickyEventId = 7;

        [Event(TrickyEventId, Level = EventLevel.Informational, Message = "Manifest message")]
        public void Tricky(int EventId, string EventName, string Message)
        {
            WriteEvent(TrickyEventId, EventId, EventName, Message);
        }

        public class Keywords
        {
            public const EventKeywords Routine = (EventKeywords)0x01;
            public const EventKeywords NonRoutine = (EventKeywords)0x2;
        }
    }
}
