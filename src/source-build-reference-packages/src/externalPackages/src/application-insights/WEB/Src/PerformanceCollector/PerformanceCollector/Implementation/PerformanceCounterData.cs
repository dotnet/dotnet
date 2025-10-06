namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation
{
    internal class PerformanceCounterData
    {
        public PerformanceCounterData(
            string originalString,
            string reportAs,
            bool usesInstanceNamePlaceholder,
            bool isInBadState,
            string categoryName,
            string counterName,
            string instanceName)
        {
            this.OriginalString = originalString;
            this.ReportAs = reportAs;
            this.UsesInstanceNamePlaceholder = usesInstanceNamePlaceholder;
            this.IsInBadState = isInBadState;
            this.PerformanceCounter = new PerformanceCounterStructure(categoryName, counterName, instanceName);
        }

        public PerformanceCounterData(
            string originalString,
            string reportAs,
            bool usesInstanceNamePlaceholder,
            bool isInBadState,
            PerformanceCounterStructure counter)
        {
            this.OriginalString = originalString;
            this.ReportAs = reportAs;
            this.UsesInstanceNamePlaceholder = usesInstanceNamePlaceholder;
            this.IsInBadState = isInBadState;
            this.PerformanceCounter = counter;
        }

        public PerformanceCounterStructure PerformanceCounter { get; private set; }

        public string OriginalString { get; private set; }

        public string ReportAs { get; private set; }

        public bool UsesInstanceNamePlaceholder { get; private set; }

        public bool IsInBadState { get; private set; }
    }
}
