namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    /// <summary>
    /// Payload stored and used to transmit Health Heartbeat properties with, allowing for user to push updates
    /// as they deem necessary.
    /// </summary>
    internal class HeartbeatPropertyPayload
    {
        private string payloadValue = string.Empty; // the current value of this property, ready for transmission
        private bool isHealthy = true; // is this a healthy value or not

        /// <summary>
        /// Gets or sets the payload value at the time the property item was added, as a string.
        /// </summary>
        public string PayloadValue
        {
            get => this.payloadValue;
            set
            {
                string safeVal = value ?? string.Empty; // ensure we are setting a non-null value
                if (!this.payloadValue.Equals(safeVal, System.StringComparison.Ordinal))
                {
                    this.IsUpdated = true;
                    this.payloadValue = safeVal;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this property is currently in a healthy or unhealthy state.
        /// </summary>
        public bool IsHealthy
        {
            get => this.isHealthy;
            set
            {
                this.IsUpdated = this.isHealthy != value;
                this.isHealthy = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this property payload has been updated since the last time it was delivered in a heartbeat.
        /// </summary>
        public bool IsUpdated { get; set; }
    }
}
