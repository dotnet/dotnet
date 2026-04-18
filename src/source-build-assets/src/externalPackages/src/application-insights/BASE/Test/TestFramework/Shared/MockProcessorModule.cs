namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Mock class that implements both ITelemetryProcessor and ITelemetryModule.
    /// </summary>
    internal class MockProcessorModule : ITelemetryProcessor, ITelemetryModule
    {
        public bool ModuleInitialized { get; private set; } = false;

        public void Initialize(TelemetryConfiguration configuration) => this.ModuleInitialized = true;

        public void Process(ITelemetry item) => throw new NotImplementedException();
    }
}
