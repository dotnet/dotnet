namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector
{
    /// <summary>
    /// Represents a request to collect a custom performance counter.
    /// </summary>
    public class PerformanceCounterCollectionRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterCollectionRequest"/> class.
        /// </summary>
        public PerformanceCounterCollectionRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterCollectionRequest"/> class.
        /// </summary>
        /// <param name="performanceCounter">Performance counter in a canonical format.</param>
        /// <param name="reportAs">Alias to report the counter under.</param>
        public PerformanceCounterCollectionRequest(string performanceCounter, string reportAs)
        {
            this.PerformanceCounter = performanceCounter;
            this.ReportAs = reportAs;
        }

        /// <summary>
        /// Gets or sets the performance counter description in a canonical format.
        /// </summary>
        public string PerformanceCounter { get; set; }

        /// <summary>
        /// Gets or sets an alias to report the counter under.
        /// </summary>
        public string ReportAs { get; set; }
    }
}
