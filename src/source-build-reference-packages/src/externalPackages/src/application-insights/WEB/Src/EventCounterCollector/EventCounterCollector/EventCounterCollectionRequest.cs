namespace Microsoft.ApplicationInsights.Extensibility.EventCounterCollector
{
    /// <summary>
    /// Represents a request to collect a EventCounter.
    /// </summary>
    public class EventCounterCollectionRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventCounterCollectionRequest"/> class.
        /// </summary>
        public EventCounterCollectionRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCounterCollectionRequest"/> class.
        /// </summary>
        /// <param name="eventSourceName">EventSourceName which publishes the counter.</param>
        /// <param name="eventCounterName">name of the counter.</param>
        public EventCounterCollectionRequest(string eventSourceName, string eventCounterName)
        {
            this.EventSourceName = eventSourceName;
            this.EventCounterName = eventCounterName;
        }

        /// <summary>
        /// Gets or sets the EventSourceName which publishes the counter.
        /// </summary>
        public string EventSourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the counter.
        /// </summary>
        public string EventCounterName { get; set; }
    }
}