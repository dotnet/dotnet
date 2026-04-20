namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;

    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// An <see cref="ITelemetryProcessor"/> that act as a proxy to the Transmission of telemetry"/>.
    /// The <see cref="ITelemetryChannel"/>, passed at construction time, will be used for transmission.
    /// This processor is always appended as the last processor in the chain.
    /// </summary>
    internal class TransmissionProcessor : ITelemetryProcessor
    {        
        private readonly TelemetrySink sink;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmissionProcessor"/> class.
        /// </summary>        
        /// <param name="sink">The <see cref="TelemetrySink"/> holding to the telemetry channel to use for sending telemetry.</param>
        internal TransmissionProcessor(TelemetrySink sink)
        {
            this.sink = sink ?? throw new ArgumentNullException(nameof(sink));
        }

        /// <summary>
        /// Process the given <see cref="ITelemetry"/> item. Here processing is sending the item through the channel/>.
        /// </summary>
        public void Process(ITelemetry item)
        {
            TelemetryDebugWriter.WriteTelemetry(item);

            this.sink.TelemetryChannel.Send(item);
        }
    }
}
