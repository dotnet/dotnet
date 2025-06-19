namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.FileDiagnosticsModule
{
    using System;
    using System.Diagnostics.Tracing;

    /// <summary>
    /// EventListener to listen for Application Insights diagnostics messages.
    /// </summary>
    internal class DiagnosticsEventListener : EventListener
    {
#if REDFIELD
        private const string EventSourceNamePrefix = "Redfield-Microsoft-ApplicationInsights-";
#else
        private const string EventSourceNamePrefix = "Microsoft-ApplicationInsights-";
#endif
        private readonly EventKeywords keywords;

        private readonly EventLevel logLevel;

        private readonly IEventListener sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsEventListener" /> class.
        /// </summary>
        /// <param name="logLevel">Log level to subscribe to.</param>
        /// <param name="keywords">Keywords to subscribe to.</param>
        /// <param name="sender">Event listener that will be called on every new event.</param>
        internal DiagnosticsEventListener(EventLevel logLevel, EventKeywords keywords, IEventListener sender)
        {
            this.logLevel = logLevel;
            this.keywords = keywords;
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        /// <summary>
        /// Gets event log level.
        /// </summary>
        public EventLevel LogLevel
        {
            get
            {
                return this.logLevel;
            }
        }

        /// <summary>
        /// Log event.
        /// </summary>
        /// <param name="eventData">Event to trace.</param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.sender.OnEventWritten(eventData);
        }

        /// <summary>
        /// This method subscribes on Application Insights EventSource.
        /// </summary>
        /// <param name="eventSource">EventSource to subscribe to.</param>
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource.Name.StartsWith(EventSourceNamePrefix, StringComparison.Ordinal))
            {
                this.EnableEvents(eventSource, this.logLevel, this.keywords);
            }

            base.OnEventSourceCreated(eventSource);
        }
    }
}
