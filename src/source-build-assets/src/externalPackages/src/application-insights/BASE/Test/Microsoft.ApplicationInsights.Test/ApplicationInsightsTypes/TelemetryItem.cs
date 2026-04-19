namespace AI
{
    /// <summary>
    /// Provides access to a complete telemetry item.
    /// </summary>
    /// <typeparam name="T">The part B telemetry item type.</typeparam>
    public class TelemetryItem<T> : Envelope
    {
        /// <summary>
        /// Gets the telemetry data.
        /// </summary>
        public new Data<T> data { get; set; }
    }
}
