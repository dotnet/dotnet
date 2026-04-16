namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;

    internal class TelemetrySerializer
    {
        internal readonly Transmitter Transmitter;
        private Uri endpoindAddress;

        public TelemetrySerializer(Transmitter transmitter) => this.Transmitter = transmitter ?? throw new ArgumentNullException(nameof(transmitter));

        protected TelemetrySerializer()
        {
            // for stubs and mocks
        }

        /// <summary>
        /// Gets or sets the endpoint address.  
        /// </summary>
        public Uri EndpointAddress
        {
            get { return this.endpoindAddress; }
            set { this.endpoindAddress = value ?? throw new ArgumentNullException(nameof(this.EndpointAddress)); }
        }

        /// <summary>
        /// Gets or Sets the subscriber to an event with Transmission and HttpWebResponseWrapper.
        /// </summary>
        public EventHandler<TransmissionStatusEventArgs> TransmissionStatusEvent { get; set; }

        public virtual void Serialize(ICollection<ITelemetry> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (!items.Any())
            {
                throw new ArgumentException("One or more telemetry item is expected", nameof(items));
            }

            if (this.EndpointAddress == null)
            {
                throw new Exception("TelemetrySerializer.EndpointAddress was not set.");
            }

            var transmission = new Transmission(this.EndpointAddress, items)
                                    { TransmissionStatusEvent = this.TransmissionStatusEvent };
            this.Transmitter.Enqueue(transmission);
        }

        public virtual Task<bool> SerializeAsync(ICollection<ITelemetry> items, CancellationToken cancellationToken)
        {
            if (this.EndpointAddress == null)
            {
                throw new Exception("TelemetrySerializer.EndpointAddress was not set.");
            }

            if (items == null || !items.Any())
            {
                return this.Transmitter.MoveTransmissionsAndWaitForSender(cancellationToken);
            }

            var transmission = new Transmission() { TransmissionStatusEvent = this.TransmissionStatusEvent };
            // serialize transmission and enqueue asynchronously
            return Task.Run(() => this.SerializeTransmissionAndEnqueue(transmission, items, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Serializes transmission and enqueue.
        /// </summary>
        private Task<bool> SerializeTransmissionAndEnqueue(Transmission transmission, ICollection<ITelemetry> items, CancellationToken cancellationToken)
        {
            transmission.Serialize(this.EndpointAddress, items);
            return this.Transmitter.FlushAsync(transmission, cancellationToken);
        }
    }
}
