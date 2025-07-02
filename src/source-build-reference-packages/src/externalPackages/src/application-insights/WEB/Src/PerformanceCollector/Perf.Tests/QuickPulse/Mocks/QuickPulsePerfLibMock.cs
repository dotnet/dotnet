namespace Microsoft.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib;

    internal class QuickPulsePerfLibMock : IQuickPulsePerfLib
    {
        public CategorySample CategorySample { get; set; }

        public CategorySample GetCategorySample(int categoryIndex, int counterIndex)
        {
            return this.CategorySample;
        }

        public void Initialize()
        {
        }

        public void Close()
        {
        }
    }
}