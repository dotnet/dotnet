namespace Microsoft.ApplicationInsights.AspNetCore.Tests
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    // TODO: Remove FakeTelemetryInitializer when we can use a dynamic test isolation framework, like NSubstitute or Moq
    internal class FakeTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            throw new NotImplementedException();
        }
    }
}