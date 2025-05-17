// <copyright file="ILoggerIntegrationTests.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

using Microsoft.ApplicationInsights.Channel;

namespace Microsoft.ApplicationInsights
{
    internal sealed class TestTelemetryChannel : ITelemetryChannel
    {
        public TestTelemetryChannel()
        {
        }

        public bool? DeveloperMode { get; set; }
        public string EndpointAddress { get; set; }

        public void Dispose()
        {
        }

        public void Flush() => FlushCount++;

        public void Send(ITelemetry item) => SendCount++;

        public int FlushCount { get; private set; }

        public int SendCount { get; private set; }
    }
}