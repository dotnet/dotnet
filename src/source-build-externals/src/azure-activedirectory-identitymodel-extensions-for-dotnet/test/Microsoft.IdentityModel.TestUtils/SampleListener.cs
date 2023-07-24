// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.Tracing;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.TestUtils
{
    public class SampleListener : EventListener
    {
        public string TraceBuffer { get; set; }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData != null && eventData.Payload.Count > 0)
            {
                TraceBuffer += eventData.Payload[0] + "\n";
            }
        }

        public static SampleListener CreateLoggerListener(EventLevel eventLevel)
        {
            SampleListener listener = new SampleListener();
            IdentityModelEventSource.Logger.LogLevel = eventLevel;
            listener.EnableEvents(IdentityModelEventSource.Logger, eventLevel);
            return listener;
        }
    }
}
