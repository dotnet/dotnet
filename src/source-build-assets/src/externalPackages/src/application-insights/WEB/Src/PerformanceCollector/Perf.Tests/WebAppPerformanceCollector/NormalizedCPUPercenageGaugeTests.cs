namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;    

    [TestClass]
    public class NormalizedCPUPercenageGaugeTests
    {
        [TestMethod]
        public void NormalizedCPUPercenageGaugeBasicValidation()
        {
            int initialProcessorsCount = int.Parse(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"), CultureInfo.InvariantCulture);
            NormalizedCPUPercentageGauge normalizedGauge = new NormalizedCPUPercentageGauge(
                "CPU",
                new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Normalized Private Bytes", "userTime", AzureWebApEnvironmentVariables.App, new CacheHelperTests()));

            CPUPercenageGauge gauge = new CPUPercenageGauge(
                "CPU",
                new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", "userTime", AzureWebApEnvironmentVariables.App, new CacheHelperTests()));

            double value1 = gauge.Collect();
            double normalizedValue1 = normalizedGauge.Collect();

            Assert.IsTrue(Math.Abs(value1) < 0.000001);
            Assert.IsTrue(Math.Abs(normalizedValue1) < 0.000001);

            Stopwatch sw = Stopwatch.StartNew();
            Thread.Sleep(TimeSpan.FromSeconds(10));
            long actualSleepTimeTicks = sw.Elapsed.Ticks;

            double value2 = gauge.Collect();
            double normalizedValue2 = normalizedGauge.Collect();

            Assert.IsTrue(
                Math.Abs(value2 - (normalizedValue2 * initialProcessorsCount)) < 0.005,
                string.Format(CultureInfo.InvariantCulture, "Actual: {0}, Expected: {1}", normalizedValue2, value2 / initialProcessorsCount));
        }
    }
}
