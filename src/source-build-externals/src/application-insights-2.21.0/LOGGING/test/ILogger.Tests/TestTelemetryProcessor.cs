using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Microsoft.ApplicationInsights
{
    /// <summary>
    /// Test telemetry processor which gives access to the telemetry items as it passes through the pipeline.
    /// </summary>
    internal class TestTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor nextTelemetryProcessor;
        private readonly Action<ITelemetry, ITelemetryProcessor> telemetryActionCallback;

        /// <summary>
        /// Initializes a new instances of the <see cref="TestTelemetryProcessor"/> class.
        /// </summary>
        /// <param name="nextTelemetryProcessor">Next telemetry processor to invoke.</param>
        /// <param name="telemetryActionCallback">Action to invoke when the telemetry item is received.</param>
        public TestTelemetryProcessor(ITelemetryProcessor nextTelemetryProcessor, Action<ITelemetry, ITelemetryProcessor> telemetryActionCallback)
        {
            this.nextTelemetryProcessor = nextTelemetryProcessor;
            this.telemetryActionCallback = telemetryActionCallback;
        }

        /// <summary>
        /// Invokes the callback registered by the user.
        /// </summary>
        /// <param name="item">Telemetry item.</param>
        public void Process(ITelemetry item)
        {
            telemetryActionCallback.Invoke(item, this.nextTelemetryProcessor);
        }
    }
}
