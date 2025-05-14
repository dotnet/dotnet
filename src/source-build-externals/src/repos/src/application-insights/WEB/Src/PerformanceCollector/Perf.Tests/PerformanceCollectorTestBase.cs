#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.StandardPerfCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;    
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// PerformanceCollector test base.
    /// </summary>
    public class PerformanceCollectorTestBase
    {
        internal void PerformanceCollectorSanityTest(IPerformanceCollector collector, string counter, string categoryName, string counterName, string instanceName)
        {
            const int CounterCount = 3;

            for (int i = 0; i < CounterCount; i++)
            {
                string error;
                collector.RegisterCounter(
                    counter,
                    null,
                    out error,
                    false);
            }

            var results = collector.Collect().ToList();

            Assert.AreEqual(CounterCount, results.Count);

            foreach (var result in results)
            {
                var value = result.Item2;

                Assert.AreEqual(categoryName, result.Item1.PerformanceCounter.CategoryName);
                Assert.AreEqual(counterName, result.Item1.PerformanceCounter.CounterName);

                if (instanceName != null)
                {
                    Assert.AreEqual(instanceName, result.Item1.PerformanceCounter.InstanceName);
                }

                Assert.IsTrue(value >= 0 && value <= 100);
            }
        }

        internal void PerformanceCollectorRefreshCountersTest(IPerformanceCollector collector)
        {
            var counters = new PerformanceCounter[]
                               {
                                   new PerformanceCounter("Processor", "% Processor Time", "_Total123blabla"),
                                   new PerformanceCounter("Processor", "% Processor Time", "_Total"),
                                   new PerformanceCounter("Processor", "% Processor Time", "_Total123afadfdsdf"),
                               };

            foreach (var pc in counters)
            {
                try
                {
                    string error = null;
                    collector.RegisterCounter(
                        PerformanceCounterUtility.FormatPerformanceCounter(pc),
                        null,
                        out error,
                        false);
                }
                catch (Exception)
                {
                }
            }

            collector.RefreshCounters();

            // All bad state counters are removed and added later through register counter, and as a result, the order of the performance coutners is changed.
            Assert.AreEqual(collector.PerformanceCounters.First().PerformanceCounter.InstanceName, "_Total");
            Assert.AreEqual(collector.PerformanceCounters.Last().PerformanceCounter.InstanceName, "_Total123afadfdsdf");
        }

        internal void PerformanceCollectorBadStateTest(IPerformanceCollector collector)
        {
            var counters = new PerformanceCounter[]
                               {
                                   new PerformanceCounter("Processor", "% Processor Time", "_Total123blabla"),
                                   new PerformanceCounter("Processor", "% Processor Time", "_Total")
                               };

            foreach (var pc in counters)
            {
                try
                {
                    string error = null;
                    collector.RegisterCounter(
                        PerformanceCounterUtility.FormatPerformanceCounter(pc),
                        null,
                        out error,
                        false);
                }
                catch (Exception)
                {
                }
            }

            Assert.IsTrue(collector.PerformanceCounters.First().IsInBadState);
            Assert.IsFalse(collector.PerformanceCounters.Last().IsInBadState);
        }

        internal void PerformanceCollectorAddRemoveCountersTest(StandardPerformanceCollector collector)
        {
            var counters = new[]
                               {
                                   new PerformanceCounter("Processor", "% Processor Time", "_Total"),
                                   new PerformanceCounter("Memory", "Available Bytes", string.Empty)
                               };

            foreach (var pc in counters)
            {
                string error;
                collector.RegisterCounter(
                    PerformanceCounterUtility.FormatPerformanceCounter(pc),
                    pc.GetHashCode().ToString(CultureInfo.InvariantCulture),
                    out error,
                    false);
            }

            var twoCounters = collector.PerformanceCounters.ToArray();

            collector.RemoveCounter(@"\PROCESSOR(_Total)\% Processor Time", counters[0].GetHashCode().ToString(CultureInfo.InvariantCulture));

            var oneCounter = collector.PerformanceCounters.ToArray();

            Assert.AreEqual(2, twoCounters.Count());
            Assert.AreEqual(@"\Processor(_Total)\% Processor Time", twoCounters[0].OriginalString);
            Assert.AreEqual(@"\Memory\Available Bytes", twoCounters[1].OriginalString);

            Assert.AreEqual(@"\Memory\Available Bytes", oneCounter.Single().OriginalString);
        }

        internal void PerformanceCollectorAddRemoveCountersForWebAppTest(WebAppPerformanceCollector collector)
        {
            var counters = new[]
                               {
                                   new PerformanceCounter("ASP.NET Applications", "Request Execution Time", "??APP_W3SVC_PROC??"),
                                   new PerformanceCounter("ASP.NET Applications", "Requests In Application Queue", "??APP_W3SVC_PROC??")
                               };

            foreach (var pc in counters)
            {
                string error;
                collector.RegisterCounter(
                    PerformanceCounterUtility.FormatPerformanceCounter(pc),
                    pc.GetHashCode().ToString(CultureInfo.InvariantCulture),
                    out error,
                    false);
            }

            var twoCounters = collector.PerformanceCounters.ToArray();

            collector.RemoveCounter(
                @"\ASP.NET APPLICATIONS(??APP_W3SVC_PROC??)\Request Execution Time",
                counters[0].GetHashCode().ToString(CultureInfo.InvariantCulture));

            var oneCounter = collector.PerformanceCounters.ToArray();

            Assert.AreEqual(2, twoCounters.Count());
            Assert.AreEqual(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time", twoCounters[0].OriginalString);
            Assert.AreEqual(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue", twoCounters[1].OriginalString);

            Assert.AreEqual(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue", oneCounter.Single().OriginalString);
        }

        internal void PerformanceCollectorNormalizedCpuTest(IPerformanceCollector collector)
        {
            string error = null;
            collector.RegisterCounter(
                    @"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized",
                    null,
                    out error,
                    false);

            collector.RegisterCounter(
                    @"\Process(??APP_WIN32_PROC??)\% Processor Time",
                    null,
                    out error,
                    false);
                       
            var results = collector.Collect().ToList();
            Assert.AreEqual(2, results.Count);

            Assert.AreEqual("Process", results[0].Item1.PerformanceCounter.CategoryName);
            Assert.AreEqual("% Processor Time Normalized", results[0].Item1.PerformanceCounter.CounterName);

            Assert.AreEqual("Process", results[1].Item1.PerformanceCounter.CategoryName);
            Assert.AreEqual("% Processor Time", results[1].Item1.PerformanceCounter.CounterName);

            Assert.AreEqual(results[0].Item1.PerformanceCounter.InstanceName, results[1].Item1.PerformanceCounter.InstanceName);

            Assert.IsTrue(results[0].Item2 >= 0 && results[0].Item2 <= 100);           
        }
    }
}
#endif