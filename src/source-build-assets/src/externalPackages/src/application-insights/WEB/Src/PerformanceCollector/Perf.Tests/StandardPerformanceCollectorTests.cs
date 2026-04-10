#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.StandardPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// StandardPerformanceCollector tests.
    /// </summary>
    [TestClass]
    public class StandardPerformanceCollectorTests : PerformanceCollectorTestBase
    {
        [TestMethod]
        [TestCategory("RequiresPerformanceCounters")]
        public void PerformanceCollectorSanityTest()
        {
           this.PerformanceCollectorSanityTest(new StandardPerformanceCollector(), @"\Processor(_Total)\% Processor Time", "Processor", "% Processor Time", "_Total");
        }

        [TestMethod]
        [TestCategory("RequiresPerformanceCounters")]
        public void PerformanceCollectorWithPlaceHolderSanityTest()
        {
           this.PerformanceCollectorSanityTest(new StandardPerformanceCollector(), @"\Process(??APP_WIN32_PROC??)\Thread Count", "Process", "Thread Count", null);
        }

        [TestMethod]
        [TestCategory("RequiresPerformanceCounters")]
        public void PerformanceCollectorRefreshCountersTest()
        {
            this.PerformanceCollectorRefreshCountersTest(new StandardPerformanceCollector());
        }

        [TestMethod]
        [TestCategory("RequiresPerformanceCounters")]
        public void PerformanceCollectorBadStateTest()
        {
            this.PerformanceCollectorBadStateTest(new StandardPerformanceCollector());
        }

        [TestMethod]
        [TestCategory("RequiresPerformanceCounters")]
        public void PerformanceCollectorAddRemoveCountersTest()
        {
            this.PerformanceCollectorAddRemoveCountersTest(new StandardPerformanceCollector());
        }

        [TestMethod]
        [TestCategory("RequiresPerformanceCounters")]
        public void PerformanceCollectorNormalizedCpuCounterTest()
        {
            this.PerformanceCollectorNormalizedCpuTest(new StandardPerformanceCollector());
        }
    }
}
#endif