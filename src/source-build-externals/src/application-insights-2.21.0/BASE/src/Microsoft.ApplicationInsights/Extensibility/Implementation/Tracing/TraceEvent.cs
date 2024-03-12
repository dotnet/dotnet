namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Event Source event wrapper.
    /// Contains description information for trace event.
    /// </summary>
    internal class TraceEvent
    {
        /// <summary>
        /// Prefix for user-actionable traces.
        /// </summary>
        private const string AiPrefix = "AI: ";

        /// <summary>
        /// Prefix for non user-actionable traces. "AI Internal".
        /// </summary>
        private const string AiNonUserActionable = "AI (Internal): ";

        /// <summary>
        /// Gets or sets event metadata.
        /// </summary>
        public EventMetaData MetaData { get; set; }

        /// <summary>
        /// Gets or sets event parameters.
        /// </summary>
        public object[] Payload { get; set; }

        public override string ToString()
        {
            // Add "AI: " prefix (if keyword does not contain UserActionable = (EventKeywords)0x1, than prefix should be "AI (Internal):" )
            string message = this.MetaData.IsUserActionable()
                ? AiPrefix
                : AiNonUserActionable + '[' + this.MetaData.EventSourceName + "] ";

            message += this.Payload != null ?
                string.Format(CultureInfo.CurrentCulture, this.MetaData.MessageFormat, this.Payload.ToArray()) :
                this.MetaData.MessageFormat;

            return message;
        }
    }
}
