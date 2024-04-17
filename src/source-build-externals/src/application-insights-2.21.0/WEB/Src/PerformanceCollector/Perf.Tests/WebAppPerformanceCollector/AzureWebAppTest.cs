namespace Microsoft.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;    
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTestAzureWeb
    {
        [TestMethod]
        public void TestPerformanceCounterValuesAreCorrectlyRetrievedUsingRawCounterGauge()
        {
            RawCounterGauge gauge = new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", "privateBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests());
            double value = gauge.Collect();

            Assert.IsTrue(value > 0);
        }
    }
}
