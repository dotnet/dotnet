namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    internal class MockTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetryItem)
        {
            telemetryItem.Context.Session.Id = "SessionID";
            telemetryItem.Context.User.Id = "UserID";
        }
    }
}
