//-----------------------------------------------------------------------
// <copyright file="OtherTestEventSource.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EventSourceListener.Tests
{
    using System.Diagnostics.Tracing;

    [EventSource(Name = OtherTestEventSource.ProviderName)]
    internal class OtherTestEventSource : EventSource
    {
        public const string ProviderName = "Microsoft-ApplicationInsights-Extensibility-EventSourceListener-Tests-Other";

        [Event(3, Level = EventLevel.Informational, Message = "{0}")]
        public void Message(string message)
        {
            WriteEvent(3, message);
        }
    }
}
