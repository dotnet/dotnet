namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// A stub of <see cref="ITelemetryInitializer"/>.
    /// </summary>
    public sealed class StubTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StubTelemetryInitializer"/> class.
        /// </summary>
        public StubTelemetryInitializer()
        {
            this.OnInitialize = telemetry => { };
        }

        /// <summary>
        /// Gets or sets the callback invoked by the <see cref="Initialize"/> method.
        /// </summary>
        public Action<ITelemetry> OnInitialize = item => { };

        /// <summary>
        /// Implements the <see cref="ITelemetryInitializer.Initialize"/> method by invoking the <see cref="OnInitialize"/> callback.
        /// </summary>
        public void Initialize(ITelemetry telemetry)
        {
            this.OnInitialize(telemetry);
        }
    }
}
