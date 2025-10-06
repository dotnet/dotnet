//-----------------------------------------------------------------------
// <copyright file="TestProvider.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwTelemetryCollector.Tests
{
    using System;
    using System.Diagnostics.Tracing;

    [EventSource(Name = TestProvider.ProviderName)]
    internal class TestProvider : EventSource
    {
        public const string ProviderName = "Microsoft-ApplicationInsights-Extensibility-Etw-Provider-Tests";
        public const int InfoEventId = 1;
        public const int WarningEventId = 2;
        public const int ComplexEventId = 4;
        public const int RequestStartEventId = 5;
        public const int RequestStopEventId = 6;
        public const int TrickyEventId = 7;

        public static readonly TestProvider Log = new TestProvider();

        [Event(InfoEventId, Level = EventLevel.Informational, Message = "{0}", Keywords = Keywords.Routine)]
        public void Info(string information)
        {
            WriteEvent(InfoEventId, information);
        }

        [Event(WarningEventId, Level = EventLevel.Warning, Message = "Warning!", Keywords = Keywords.NonRoutine)]
        public void Warning(int i1, int i2)
        {
            WriteEvent(WarningEventId, i1, i2);
        }

        [Event(ComplexEventId, Level = EventLevel.Verbose, Message = "Blah blah", Keywords = Keywords.Routine,
            Channel = EventChannel.Debug, Opcode = EventOpcode.Extension, Tags = (EventTags)17, Task = (EventTask)32)]
        public void Complex(Guid uniqueId)
        {
            WriteEvent(ComplexEventId, uniqueId);
        }

        [Event(TrickyEventId, Level = EventLevel.Informational, Message = "Manifest message")]
        public void Tricky(int EventId, string EventName, string Message)
        {
            WriteEvent(TrickyEventId, EventId, EventName, Message);
        }

        [Event(RequestStartEventId, Level = EventLevel.Informational, ActivityOptions = EventActivityOptions.Recursive)]
        public void RequestStart(int requestId)
        {
            WriteEvent(RequestStartEventId, requestId);
        }

        [Event(RequestStopEventId, Level = EventLevel.Informational, ActivityOptions = EventActivityOptions.Recursive)]
        public void RequestStop(int requestId)
        {
            WriteEvent(RequestStopEventId, requestId);
        }

        public class Keywords
        {
            public const EventKeywords Routine = (EventKeywords)0x01;
            public const EventKeywords NonRoutine = (EventKeywords)0x2;
        }
    }
}
