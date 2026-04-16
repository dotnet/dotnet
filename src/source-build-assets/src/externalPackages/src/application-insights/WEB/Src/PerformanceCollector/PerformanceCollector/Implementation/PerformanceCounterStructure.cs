namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation
{
    /// <summary>Represents basic performance counter structure.</summary>
    internal class PerformanceCounterStructure
    {
        /// <summary>
        /// Initializes a new instance of the PerformanceCounterStructure class.
        /// </summary>
        public PerformanceCounterStructure()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PerformanceCounterStructure class.
        /// </summary>
        /// <param name="categoryName">The counter category name.</param>
        /// <param name="counterName">The counter name.</param>
        /// <param name="instanceName">The counter instance name.</param>
        public PerformanceCounterStructure(           
            string categoryName,
            string counterName,
            string instanceName)
        {
            this.CategoryName = categoryName;
            this.CounterName = counterName;
            this.InstanceName = instanceName;
        }

        /// <summary>Gets or sets the counter category name.</summary>
        public string CategoryName { get; set; }

        /// <summary>Gets or sets the counter name.</summary>
        public string CounterName { get; set; }

        /// <summary>Gets or sets the counter instance.</summary>
        public string InstanceName { get; set; }
    }
}
