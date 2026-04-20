using System;
using System.Collections.Generic;

using Microsoft.ApplicationInsights.Channel;


namespace Microsoft.ApplicationInsights.Metrics.TestUtility
{
    /// <summary>
    /// A stub of <see cref="ITelemetryChannel"/>.
    /// </summary>
    public sealed class StubApplicationInsightsTelemetryChannel : ITelemetryChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StubTelemetryChannel"/> class.
        /// </summary>
        public StubApplicationInsightsTelemetryChannel()
        {
            OnSend = telemetry => { };
            OnFlush = () => { };
            OnDispose = () => { };
            TelemetryItems = new List<ITelemetry>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this channel is in developer mode.
        /// </summary>
        public bool? DeveloperMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the channel's URI. To this URI the telemetry is expected to be sent.
        /// </summary>
        public string EndpointAddress { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked by the <see cref="Send"/> method.
        /// </summary>
        public Action<ITelemetry> OnSend { get; set; }

        /// <summary>
        /// </summary>
        public Action OnFlush { get; set; }

        /// <summary>
        /// </summary>
        public Action OnDispose { get; set; }

        /// <summary>
        /// </summary>
        public IList<ITelemetry> TelemetryItems { get; }

        /// <summary>
        /// Implements the <see cref="ITelemetryChannel.Send"/> method by invoking the <see cref="OnSend"/> callback.
        /// </summary>
        public void Send(ITelemetry item)
        {
            TelemetryItems.Add(item);
            OnSend(item);
        }

        /// <summary>
        /// Implements the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>
        /// Implements  the <see cref="ITelemetryChannel.Flush" /> method.
        /// </summary>
        public void Flush()
        {
            OnFlush();
        }
    }
}
