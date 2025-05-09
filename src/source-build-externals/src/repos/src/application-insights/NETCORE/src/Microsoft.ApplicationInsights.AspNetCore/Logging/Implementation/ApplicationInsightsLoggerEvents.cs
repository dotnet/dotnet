namespace Microsoft.ApplicationInsights.AspNetCore.Logging
{
    using System;

    /// <summary>
    /// Class to provide ApplicationInsights logger events.
    /// </summary>
    internal class ApplicationInsightsLoggerEvents
    {
        /// <summary>
        /// Event that is fired when new ApplicationInsights logger is added.
        /// </summary>
        public event Action LoggerAdded;

        /// <summary>
        /// Invokes LoggerAdded event.
        /// </summary>
        public void OnLoggerAdded()
        {
            this.LoggerAdded?.Invoke();
        }
    }
}
