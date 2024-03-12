namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RatioCounterTests
    {
        [TestMethod]
        public void RateCounterGaugeGetValueAndResetGetsTheValueFromJson()
        {
            RawCounterGauge readIoBytes = new RawCounterGauge("READ IO BYTES", "readIoBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests());
            RawCounterGauge writeIoBytes = new RawCounterGauge("WRITE IO BYTES", "writeIoBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests());
            RawCounterGauge otherIoBytes = new RawCounterGauge("OTHER IO BYTES", "otherIoBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests());

            RatioCounterGauge readIoBytesRate = new RatioCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", readIoBytes, writeIoBytes);

            double value1 = readIoBytesRate.Collect();
            Assert.IsTrue(value1 != 0);

           SumUpCountersGauge writeAndOtherBytes = new SumUpCountersGauge("Sum Up Bytes", writeIoBytes, otherIoBytes);
           RatioCounterGauge totalReadIoBytesRate = new RatioCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", readIoBytes, writeAndOtherBytes);
           double value2 = totalReadIoBytesRate.Collect();

            Assert.IsTrue(value2 != 0);
            Assert.IsTrue(value1 >= value2);

           RatioCounterGauge totalReadIoBytesPercentage = new RatioCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", readIoBytes, writeAndOtherBytes, 100);
           double percentage = totalReadIoBytesPercentage.Collect();
            Assert.IsTrue(percentage != 0);
            Assert.IsTrue(Math.Abs((value2 * 100) - percentage) < 1);
        }

        [TestMethod]
        public void RatioCounterGaugeDoesNotThrowWithNullGauges()
        {
            RatioCounterGauge readIoBytesRate = new RatioCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", null, null);
            readIoBytesRate.Collect();
        }
    }
}
