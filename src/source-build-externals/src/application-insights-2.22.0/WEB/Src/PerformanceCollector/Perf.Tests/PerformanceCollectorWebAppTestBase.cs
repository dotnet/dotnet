namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// PerformanceCollector test base.
    /// </summary>
    public class PerformanceCollectorWebAppTestBase
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

        internal void PerformanceCollectorAddRemoveCountersForWebAppTest(WebAppPerformanceCollector collector)
        {
            var counterRequests = new[]
                               {                                   
                                   new PerformanceCounterCollectionRequest(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time", @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time"),                                
                                   new PerformanceCounterCollectionRequest(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue", @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue")
                               };
            
            foreach (var counterRequest in counterRequests)
            {
                string error;
                collector.RegisterCounter(
                    counterRequest.PerformanceCounter,
                    counterRequest.ReportAs,
                    out error,
                    false);
            }

            var twoCounters = collector.PerformanceCounters.ToArray();
            
            collector.RemoveCounter(
                @"\ASP.NET APPLICATIONS(??APP_W3SVC_PROC??)\Request Execution Time",
                counterRequests[0].ReportAs);

            var oneCounter = collector.PerformanceCounters.ToArray();

            Assert.AreEqual(2, twoCounters.Count());
            Assert.AreEqual(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time", twoCounters[0].OriginalString);
            Assert.AreEqual(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue", twoCounters[1].OriginalString);

            Assert.AreEqual(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue", oneCounter.Single().OriginalString);
        }        
    }
}