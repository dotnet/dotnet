namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// PerformanceCollectorModules tests.
    /// </summary>
    [TestClass]
    public class PerformanceCollectorModulesTests
    {
#if NETFRAMEWORK
        [TestMethod]
        [SuppressMessage(category: "Microsoft.Globalization", checkId: "CA1305:SpecifyIFormatProvider", Justification = "Don't care about invariant in unit tests.")]
        public void TimerTest()
        {
            var collector = CreatePerformanceCollector();
            var configuration = CreateTelemetryConfiguration();
            var telemetryChannel = configuration.TelemetryChannel as StubTelemetryChannel;

            Exception assertionsFailure = null;

            telemetryChannel.OnSend = telemetry =>
                {
                    // validate that a proper telemetry item is being sent
                    // module will swallow any exception that we throw here, so catch and rethrow later
                    try
                    {
                        Assert.AreEqual(configuration.InstrumentationKey, telemetry.Context.InstrumentationKey);

                        Assert.IsInstanceOfType(telemetry, typeof(MetricTelemetry));

                        var perfTelemetry = telemetry as MetricTelemetry;

                        Assert.AreEqual((double)perfTelemetry.Name.GetHashCode(), perfTelemetry.Sum);
                    }
                    catch (AssertFailedException e)
                    {
                        // race condition, but we don't care who wins
                        assertionsFailure = e;
                    }
                };

            using (var module = CreatePerformanceCollectionModule(collector))
            {
                // start the module
                module.Initialize(configuration);

                // wait 1s to let the module finish initializing
                Thread.Sleep(TimeSpan.FromSeconds(1));

                // now wait to let the module's timer run
                Thread.Sleep(TimeSpan.FromSeconds(1));

                lock (collector.Sync)
                {
                    Assert.IsTrue(collector.Counters.TrueForAll(c => c.Item2.Count > 0), $"Some of the counters have not been collected. Counter count: {collector.Counters.Count}, non-zero counter count: {collector.Counters.Count(c => c.Item2.Count > 0)}");
                }

                if (assertionsFailure != null)
                {
                    throw assertionsFailure;
                }
            }
        }

        [TestMethod]
        public void ConfigurationTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();
            configuration.InstrumentationKey = string.Empty;

            using (var module = CreatePerformanceCollectionModule(collector))
            {
                // start the module
                module.Initialize(configuration);

                // wait 1s to let the module finish initializing
                Thread.Sleep(TimeSpan.FromSeconds(1));

                // now wait to let the module's timer run
                Thread.Sleep(TimeSpan.FromSeconds(1));

                var privateModule = new PrivateObject(module);

                Assert.IsNotNull(privateModule.GetField("client"));
            }
        }

        [TestMethod]
        public void DefaultCountersTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();

            using (var module = CreatePerformanceCollectionModule(collector))
            {
                // start the module
                module.Initialize(configuration);

                // wait to let the module finish initializing
                Thread.Sleep(TimeSpan.FromSeconds(1));

                // now wait to let the module's timer run
                Thread.Sleep(TimeSpan.FromSeconds(3));

                lock (collector.Sync)
                {
                    // check that the default counter list has been registered
                    Assert.AreEqual(module.DefaultCounters.Count(), collector.Counters.Count);
                }
            }
        }

        [TestMethod]
        public void CustomCountersTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();

            var customCounters = new List<PerformanceCounterCollectionRequest>()
                                     {
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName5(InstanceName5)\CounterName5",
                                             "CounterFive"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\Process(??APP_WIN32_PROC??)\% Processor Time",
                                             "CounterTwo")
                                     };

            using (var module = CreatePerformanceCollectionModule(collector, customCounters))
            {
                // start the module
                module.Initialize(configuration);

                // wait 1s to let the module finish initializing
                Thread.Sleep(TimeSpan.FromSeconds(1));

                // now wait to let the module's timer run
                Thread.Sleep(TimeSpan.FromSeconds(3));

                lock (collector.Sync)
                {
                    // check that the configured counter list has been registered
                    var defaultCounterCount = module.DefaultCounters.Count();

                    Assert.AreEqual(customCounters.Count() + defaultCounterCount, collector.Counters.Count);
                }
            }
        }

        [TestMethod]
        public void CustomCountersDuplicatesTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();

            var customCounters = new List<PerformanceCounterCollectionRequest>()
                                     {
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName1\CounterName1",
                                             "CounterOne"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName2\CounterName2",
                                             "CounterTwo"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName2\CounterName2",
                                             "CounterX"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName4\CounterName4",
                                             "CounterThree"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName3\CounterName3",
                                             "CounterThree"),
                                     };

            using (var module = CreatePerformanceCollectionModule(collector, customCounters))
            {
                // start the module
                module.Initialize(configuration);

                // wait 1s to let the module finish initializing
                Thread.Sleep(TimeSpan.FromSeconds(1));

                // now wait to let the module's timer run
                Thread.Sleep(TimeSpan.FromSeconds(3));

                lock (collector.Sync)
                {
                    // check that the configured counter list has been registered
                    Assert.AreEqual(4, module.Counters.Count());

                    Assert.AreEqual(@"\CategoryName1\CounterName1", module.Counters[0].PerformanceCounter);
                    Assert.AreEqual("CounterOne", module.Counters[0].ReportAs);

                    Assert.AreEqual(@"\CategoryName2\CounterName2", module.Counters[1].PerformanceCounter);
                    Assert.AreEqual("CounterTwo", module.Counters[1].ReportAs);

                    Assert.AreEqual(@"\CategoryName4\CounterName4", module.Counters[2].PerformanceCounter);
                    Assert.AreEqual("CounterThree", module.Counters[2].ReportAs);

                    Assert.AreEqual(@"\CategoryName3\CounterName3", module.Counters[3].PerformanceCounter);
                    Assert.AreEqual("CounterThree", module.Counters[3].ReportAs);
                }
            }
        }

        [TestMethod]
        [Timeout(5000)]
        public void UnicodeSupportTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();

            var customCounters = new List<PerformanceCounterCollectionRequest>()
                                     {
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName1\CounterName1",
                                             "CounterOne"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryNameTwo\CounterNameTwo",
                                             string.Empty),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName3\CounterName3",
                                             null),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName4\CounterName4",
                                             " Counter 4"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName5\CounterName5",
                                             " Counter5"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\Категория6\Счетчик6",
                                             "Только юникод первый"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\Категория7\Счетчик7",
                                             "Только юникод второй"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryNameAnother8%\CounterNameAnother8%",
                                             null),
                                     };

            using (var module = CreatePerformanceCollectionModule(collector, customCounters))
            {
                // start the module
                module.Initialize(configuration);

                // wait 1s to let the module finish initializing
                // and wait to let the module's timer run
                while (collector.Counters.Count < 8)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }

                lock (collector.Sync)
                {
                    // check that the configured counter list has been registered
                    // check sanitization rules
                    Assert.AreEqual(@"\CategoryName1\CounterName1", collector.Counters[2].Item1.OriginalString);
                    Assert.AreEqual("CounterOne", collector.Counters[2].Item1.ReportAs);

                    Assert.AreEqual(@"\CategoryNameTwo\CounterNameTwo", collector.Counters[3].Item1.OriginalString);
                    Assert.AreEqual(@"CategoryNameTwo - CounterNameTwo", collector.Counters[3].Item1.ReportAs);

                    Assert.AreEqual(@"\CategoryName3\CounterName3", collector.Counters[4].Item1.OriginalString);
                    Assert.AreEqual(@"CategoryName3 - CounterName3", collector.Counters[4].Item1.ReportAs);

                    Assert.AreEqual(@"\CategoryName4\CounterName4", collector.Counters[5].Item1.OriginalString);
                    Assert.AreEqual(@" Counter 4", collector.Counters[5].Item1.ReportAs);

                    Assert.AreEqual(@"\CategoryName5\CounterName5", collector.Counters[6].Item1.OriginalString);
                    Assert.AreEqual(@" Counter5", collector.Counters[6].Item1.ReportAs);

                    // unicode-only reportAs values are converted to "random" strings
                    Assert.AreEqual(@"\Категория6\Счетчик6", collector.Counters[7].Item1.OriginalString);
                    Assert.AreEqual(@"Только юникод первый", collector.Counters[7].Item1.ReportAs);

                    Assert.AreEqual(@"\Категория7\Счетчик7", collector.Counters[8].Item1.OriginalString);
                    Assert.AreEqual(@"Только юникод второй", collector.Counters[8].Item1.ReportAs);

                    Assert.AreEqual(
                        @"\CategoryNameAnother8%\CounterNameAnother8%",
                        collector.Counters[9].Item1.OriginalString);
                    Assert.AreEqual(@"CategoryNameAnother8% - CounterNameAnother8%", collector.Counters[9].Item1.ReportAs);
                }
            }
        }

        [TestMethod]
        public void RebindingNoPrematureRebindingTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();

            var customCounters = new List<PerformanceCounterCollectionRequest>()
                                     {
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName1(InstanceName1)\CounterName1",
                                             null),
                                         new PerformanceCounterCollectionRequest(
                                             @"\Process(??APP_WIN32_PROC??)\% Processor Time",
                                             null)
                                     };

            using (var module = CreatePerformanceCollectionModule(collector, customCounters))
            {
                // start the module
                module.Initialize(configuration);

                // make the module think that initial binding has already happened and it's not time to rebind yet
                var privateObject = new PrivateObject(module);
                privateObject.SetField("lastRefreshTimestamp", DateTime.Now + TimeSpan.FromMinutes(1));

                // wait 1s to let the module finish initializing
                Thread.Sleep(TimeSpan.FromSeconds(1));

                // now wait to let the module's timer run
                Thread.Sleep(TimeSpan.FromSeconds(3));

                lock (collector.Sync)
                {
                    // nothing should have been registered
                    Assert.AreEqual(0, collector.Counters.Count);
                }
            }
        }

        [TestMethod]
        public void TelemetryModuleIsNotInitializedTwiceToPreventTimerBeingRecreated()
        {
            var module = new PerformanceCollectorModule();
            PrivateObject privateObject = new PrivateObject(module);

            module.Initialize(TelemetryConfiguration.CreateDefault());
            object config1 = privateObject.GetField("telemetryConfiguration");

            module.Initialize(TelemetryConfiguration.CreateDefault());
            object config2 = privateObject.GetField("telemetryConfiguration");

            Assert.AreSame(config1, config2);
        }

        private static TelemetryConfiguration CreateTelemetryConfiguration()
        {
            var configuration = new TelemetryConfiguration();

            configuration.InstrumentationKey = "56D500C1-0F6C-46D1-A1F2-250D65075E0F";
            configuration.TelemetryChannel = new StubTelemetryChannel();

            return configuration;
        }

        private static PerformanceCollectorMock CreatePerformanceCollector()
        {
            return new PerformanceCollectorMock();
        }

        private static PerformanceCollectorModule CreatePerformanceCollectionModule(IPerformanceCollector collector, List<PerformanceCounterCollectionRequest> customCounterList = null)
        {
            var module = new PerformanceCollectorModule(collector);

            if (customCounterList != null)
            {
                customCounterList.ForEach(module.Counters.Add);
            }

            module.CollectionPeriod = TimeSpan.FromMilliseconds(10);

            module.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\DefaultCategory1\DefaultCounter1", @"\DefaultCategory1\DefaultCounter1"));
            module.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\DefaultCategory2(Instance2)\DefaultCounter2", @"\DefaultCategory2(Instance2)\DefaultCounter2"));

            return module;
        }
#endif

        [TestMethod]
        public void PerformanceCollectorModuleDefaultContainsExpectedCountersNonWindows()
        {
#if NETCOREAPP
            PerformanceCounterUtility.isAzureWebApp = null;
            var original = PerformanceCounterUtility.IsWindows;
            PerformanceCounterUtility.IsWindows = false;
            var module = new PerformanceCollectorModule();
            
            try
            {                                          
                module.Initialize(new TelemetryConfiguration());

                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\Private Bytes"));
                Assert.AreEqual(3, module.DefaultCounters.Count);
            }
            finally
            {
                PerformanceCounterUtility.IsWindows = original;
                module.Dispose();
            }
#endif
        }

        [TestMethod]
        public void PerformanceCollectorModuleDefaultContainsExpectedCountersWebApps()
        {
            PerformanceCounterUtility.isAzureWebApp = null;
            Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", "something");
            var module = new PerformanceCollectorModule();
            try
            {
                module.Initialize(new TelemetryConfiguration());

                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\Private Bytes"));

                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Memory\Available Bytes"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\IO Data Bytes/sec"));

#if NETFRAMEWORK
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests/Sec"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue"));
                Assert.AreEqual(9, module.DefaultCounters.Count);
#else                
                Assert.AreEqual(5, module.DefaultCounters.Count);
#endif
            }
            finally
            {
                PerformanceCounterUtility.isAzureWebApp = null;
                module.Dispose();
                Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", string.Empty);
                Task.Delay(1000).Wait();
            }
        }

        [TestMethod]
        public void PerformanceCollectorModuleDefaultContainsExpectedCountersWindows()
        {
            PerformanceCounterUtility.isAzureWebApp = null;
            var module = new PerformanceCollectorModule();
#if NETCOREAPP
            var original = PerformanceCounterUtility.IsWindows;
            PerformanceCounterUtility.IsWindows = true;
#endif
            try
            {
                module.Initialize(new TelemetryConfiguration());

                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\Private Bytes"));

                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Memory\Available Bytes"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\IO Data Bytes/sec"));

#if NETFRAMEWORK
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests/Sec"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue"));
#endif

                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Processor(_Total)\% Processor Time"));
#if NETFRAMEWORK
                Assert.AreEqual(10, module.DefaultCounters.Count);
#else
                Assert.AreEqual(6, module.DefaultCounters.Count);
#endif
            }
            finally
            {
                module.Dispose();
#if NETCOREAPP
            PerformanceCounterUtility.IsWindows = original;
#endif
            }
        }

        private bool ContainsPerfCounter(IList<PerformanceCounterCollectionRequest> counters, string name)
        {
            foreach (var counter in counters)
            {
                if (counter.PerformanceCounter.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

    }
}