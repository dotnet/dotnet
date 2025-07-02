namespace Microsoft.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SumUpCountersGaugeTests
    {
        [TestMethod]
        public void RateCounterGaugeGetValueAndResetGetsTheValueFromJson()
        {
            SumUpCountersGauge twoTimesPrivateBytes = new SumUpCountersGauge(
                "twoTimesPrivateBytes", 
                new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes * 2", "privateBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests()), 
                new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", "privateBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests()));

            RawCounterGauge privateBytes = new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", "privateBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests());

            double expectedValue = privateBytes.Collect();
            double actualValue = twoTimesPrivateBytes.Collect();

            // twoTimesPrivateBytes is -greater than (privateBytes * 1.85) but lower than (privateBytes * 2.15).
            Assert.IsTrue((expectedValue * 1.85) < actualValue && (expectedValue * 2.15) > actualValue);
        }
    }
}
