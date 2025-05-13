#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PerformanceMonitorTests
    {
        [TestMethod]
        public void PerformanceMonitorGetsDataFromRegistry()
        {
            // ARRANGE
            var perfMon = new PerformanceMonitor();
            
            // ACT
            byte[] data = perfMon.GetData("230");
            perfMon.Close();

            // ASSERT
            Assert.IsTrue(data.Length > 0);
        }       
    }
}
#endif