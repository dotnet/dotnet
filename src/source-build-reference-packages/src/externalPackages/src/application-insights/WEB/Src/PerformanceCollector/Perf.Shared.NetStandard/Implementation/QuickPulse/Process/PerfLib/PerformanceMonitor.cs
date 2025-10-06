namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Ignore Warning. This is a stub class for NetCore")]
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Ignore Warning. This is a stub class for NetCore")]
    internal class PerformanceMonitor
    {
        private static readonly byte[] emptyResult = System.Array.Empty<byte>();

        public void Close()
        {
        }

        public byte[] GetData(string categoryIndex)
        {
            return PerformanceMonitor.emptyResult;
        }
    }
}
