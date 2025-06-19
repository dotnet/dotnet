using System;

namespace Microsoft.ServiceProfiler.Agent.Utilities
{
    /// <summary>
    /// This code is derived from the Application Insights Profiler agent. It is included in this repo
    /// in order to validate ETW payload serialization in RichPayloadEventSource.
    /// </summary>
    internal class ParsedPayload
    {
        public string InstrumentationKey;
        public string OperationName;
        public string OperationId;
        public int Version;
        public string RequestId;
        public string Source;
        public string Name;
        public TimeSpan Duration;
    }
}
