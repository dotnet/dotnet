namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    public class StubTelemetryInitializer : ITelemetryInitializer
    {
        public Action<ITelemetry> OnInitialize = item => { };

        public void Initialize(ITelemetry telemetry)
        {
            this.OnInitialize(telemetry);
        }
    }
}
