namespace Microsoft.ApplicationInsights.Tests
{
    using System.Globalization;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RateCounterTests
    {
        [TestMethod]
        public void RateCounterGaugeGetValueAndResetGetsTheValueFromJson()
        {
            RateCounterGauge privateBytesRate = new RateCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", "privateBytes", AzureWebApEnvironmentVariables.App, null, new CacheHelperTests());

            double value = privateBytesRate.Collect();

            // Initial read - so rate is expected to be zero. Also the actual raw value of the counter is 10000 from RemoteEnvironmentVariablesAllSampleOne.json
            Assert.IsTrue(value == 0);

            System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(7));

            // Second read, the actual raw value of the counter is 200000 from RemoteEnvironmentVariablesAllSampleTwo.json
            // Rate should be (200000-10000)/ 7 secs  = ~14000
            value = privateBytesRate.Collect();

            Assert.IsTrue(value >= 10000 && value <= 20000, string.Format(CultureInfo.InvariantCulture, "ActualRate:{0}, is not within expected range", value));
        }
    }
}
