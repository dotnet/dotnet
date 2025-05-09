namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;    

    [TestClass]    
    public class CPUPercenageGaugeTests
    {
        [TestMethod]
        public void BasicValidation()
        {
            CPUPercenageGauge gauge = new CPUPercenageGauge(
                "CPU",
                new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes * 2", "userTime", AzureWebApEnvironmentVariables.App, new CacheHelperTests()));
            
            double value1 = gauge.Collect();
            Assert.IsTrue(Math.Abs(value1) < 0.000001);
            Stopwatch sw = Stopwatch.StartNew();
            Thread.Sleep(TimeSpan.FromSeconds(10));
            long actualSleepTimeTicks = sw.Elapsed.Ticks;
            double value2 = gauge.Collect();
            Assert.IsTrue(
                Math.Abs(value2 - ((24843750 - 24062500.0) / actualSleepTimeTicks * 100.0)) < 0.005, 
                string.Format(CultureInfo.InvariantCulture, "Actual: {0}, Expected: {1}", value2, (24843750 - 24062500.0) / actualSleepTimeTicks));
        }
    }
}
