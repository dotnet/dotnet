using System;

namespace Microsoft.ApplicationInsights.TestFramework
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// A stub of <see cref="ITelemetryProcessor"/>.
    /// </summary>
    public sealed class StubTelemetryProcessor : ITelemetryProcessor, IDisposable
    {
        /// <summary>
        /// Made public for testing if the chain of processors is correctly created.
        /// </summary>
        public ITelemetryProcessor next;

        /// <summary>
        /// Initializes a new instance of the <see cref="StubTelemetryProcessor"/> class.
        /// </summary>
        public StubTelemetryProcessor(ITelemetryProcessor next)
        {
            this.next = next;
            this.OnDispose = () => { };
            this.OnProcess = (unusedTelemetry) => { };
        }

        /// <summary>
        /// Gets or sets the callback invoked by the <see cref="Process"/> method.
        /// </summary>
        public Action<ITelemetry> OnProcess { get; set; }

        public Action OnDispose { get; set; }

        /// <summary>
        /// Implements the <see cref="ITelemetryProcessor.Initialize"/> method by invoking the process method
        /// </summary>
        public void Process(ITelemetry telemetry)
        {
            this.OnProcess(telemetry);
            if (this.next != null)
            {
                this.next.Process(telemetry);
            }
        }

        public void Dispose()
        {
            this.OnDispose();
        }
    }
}
