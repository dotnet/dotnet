namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.ServiceContract;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuickPulseTelemetryModuleTests
    {
#pragma warning disable 0162
        // TODO: Stabilize sleep-based tests
#if NETCOREAPP
        private const bool Ignored = true;
#else
        private const bool Ignored = false;
#endif

        [TestInitialize]
        public void TestInitialize()
        {
            QuickPulseTestHelper.ClearEnvironment();
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleIsInitializedBySdk()
        {
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var configuration = new TelemetryConfiguration();
            var builder = configuration.TelemetryProcessorChainBuilder;
            builder = builder.Use(current => telemetryProcessor);
            builder.Build();

            var qp = new QuickPulseTelemetryModule();
            qp.Initialize(configuration);
            qp.Dispose();
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleServerIdDefaultsToMachineName()
        {
            using (var qp = new QuickPulseTelemetryModule())
            {
                Assert.AreEqual(Environment.MachineName, qp.ServerId);
            }
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleServerIdCanBeChanged()
        {
            using (var qp = new QuickPulseTelemetryModule())
            {
                qp.ServerId = "my-server-name";
                qp.Initialize(new TelemetryConfiguration("foo"));
                Assert.AreEqual("my-server-name", qp.ServerId);
            }
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleDisposeWithoutInitialize()
        {
            var qp = new QuickPulseTelemetryModule();
            qp.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuickPulseTelemetryModuleDoesNotRegisterNullProcessor()
        {
            // ARRANGE
            var module = new QuickPulseTelemetryModule(null, null, null, null, null, null);

            // ACT
            module.RegisterTelemetryProcessor(null);

            // ASSERT
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleDoesNotRegisterSameProcessorMoreThanOnce()
        {
            // ARRANGE
            var module = new QuickPulseTelemetryModule(null, null, null, null, null, null);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());

            // ACT
            module.RegisterTelemetryProcessor(telemetryProcessor);
            module.RegisterTelemetryProcessor(telemetryProcessor);
            module.RegisterTelemetryProcessor(telemetryProcessor);

            // ASSERT
            Assert.AreEqual(telemetryProcessor, module.TelemetryProcessors.Single());
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleInitializesServiceClientFromConfiguration()
        {
            // ARRANGE
            var module = new QuickPulseTelemetryModule(null, null, null, null, null, null);

            module.QuickPulseServiceEndpoint = "https://test.com/api";

            // ACT
            module.Initialize(new TelemetryConfiguration());

            // ASSERT
            Assert.IsInstanceOfType(module.ServiceClient, typeof(QuickPulseServiceClient));
        }

        [TestMethod]
        [TestCategory("QuickPulseEndpoint")]
        public void QuickPulseTelemetryModuleInitializesServiceClient_FromCode_WithDefaults()
        {
            // ARRANGE
            var configuration = new TelemetryConfiguration();
            var expectedEndpoint = QuickPulseDefaults.QuickPulseServiceEndpoint;

            var module = new QuickPulseTelemetryModule(null, null, null, null, null, null);
            TelemetryModules.Instance.Modules.Add(module);
            var processor = (IQuickPulseTelemetryProcessor)new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            module.Initialize(configuration);

            // ASSERT
            Assert.IsInstanceOfType(module.ServiceClient, typeof(QuickPulseServiceClient));
            Assert.AreEqual(expectedEndpoint, module.ServiceClient.CurrentServiceUri, "module is invalid");
            Assert.AreEqual(expectedEndpoint, processor.ServiceEndpoint, "processor is invalid");
        }

        [TestMethod]
        [TestCategory("QuickPulseEndpoint")]
        public void QuickPulseTelemetryModuleInitializesServiceClient_FromConfigFile_WithDefaults()
        {
            // ARRANGE
            var configuration = new TelemetryConfiguration();
            var expectedEndpoint = QuickPulseDefaults.QuickPulseServiceEndpoint;

            string configFileContents = TelemetryConfigurationFactoryHelper.BuildConfiguration(
                module: @"<Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryModule, Microsoft.AI.PerfCounterCollector""/>",
                processor: @"<Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryProcessor, Microsoft.AI.PerfCounterCollector""/>"
                );
            TelemetryConfigurationFactoryHelper.Initialize(configuration, TelemetryModules.Instance, configFileContents);

            // ASSERT
            var module = TelemetryModules.Instance.Modules.OfType<QuickPulseTelemetryModule>().SingleOrDefault();
            Assert.IsNotNull(module, "module was not initialized");

            var processor = configuration.TelemetryProcessors.OfType<IQuickPulseTelemetryProcessor>().SingleOrDefault();
            Assert.IsNotNull(processor, "processor was not initialized");

            Assert.IsInstanceOfType(module.ServiceClient, typeof(QuickPulseServiceClient));
            Assert.AreEqual(expectedEndpoint, module.ServiceClient.CurrentServiceUri, "module is invalid");
            Assert.AreEqual(expectedEndpoint, processor.ServiceEndpoint, "processor is invalid");
        }

        [TestMethod]
        [TestCategory("QuickPulseEndpoint")]
        public void QuickPulseInitializeViaCode()
        {
            // Code sample from http://apmtips.com/blog/2017/02/13/enable-application-insights-live-metrics-from-code/

            var explicitEndpoint = "https://127.0.0.1/";
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;LiveEndpoint={explicitEndpoint}";
            var expectedEndpoint = $"{explicitEndpoint}QuickPulseService.svc";

            // ARANGE
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            configuration.ConnectionString = connectionString;

            QuickPulseTelemetryProcessor processor = null;

            configuration.TelemetryProcessorChainBuilder
                .Use((next) =>
                {
                    processor = new QuickPulseTelemetryProcessor(next);
                    return processor;
                })
                .Build();

            var module = new QuickPulseTelemetryModule();
            module.Initialize(configuration);
            module.RegisterTelemetryProcessor(processor); // module did not exist when Processor was created. Need to manually register.

            // ASSERT
            Assert.AreEqual(expectedEndpoint, module.ServiceClient.CurrentServiceUri.AbsoluteUri, "module endpoint is invalid");
            Assert.AreEqual(expectedEndpoint, ((IQuickPulseTelemetryProcessor)processor).ServiceEndpoint.AbsoluteUri, "processor endpoint is invalid");
        }

        [TestMethod]
        [TestCategory("QuickPulseEndpoint")]
        public void QuickPulseTelemetryModuleInitializesServiceClient_FromCode_WithCustomEndpoint()
        {
            // ARRANGE
            // Config module, 
            var configuration = new TelemetryConfiguration();
            var expectedEndpoint = "https://127.0.0.1/QuickPulseService.svc";

            var module = new QuickPulseTelemetryModule(null, null, null, null, null, null);
            module.QuickPulseServiceEndpoint = expectedEndpoint;
            TelemetryModules.Instance.Modules.Add(module);
            var processor = (IQuickPulseTelemetryProcessor)new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy()); // processor will register self with module within constructor.
            module.Initialize(configuration);

            // ASSERT
            Assert.IsInstanceOfType(module.ServiceClient, typeof(QuickPulseServiceClient));
            Assert.AreEqual(expectedEndpoint, module.ServiceClient.CurrentServiceUri.AbsoluteUri, "module is invalid");
            Assert.AreEqual(expectedEndpoint, processor.ServiceEndpoint.AbsoluteUri, "processor is invalid");
        }

        [TestMethod]
        [TestCategory("QuickPulseEndpoint")]
        public void QuickPulseTelemetryModuleInitializesServiceClient_FromCodeReversed_WithCustomEndpoint()
        {
            // ARRANGE
            var configuration = new TelemetryConfiguration();
            var expectedEndpoint = "https://127.0.0.1/QuickPulseService.svc";

            var module = new QuickPulseTelemetryModule(null, null, null, null, null, null);
            module.QuickPulseServiceEndpoint = expectedEndpoint;
            module.Initialize(configuration);
            TelemetryModules.Instance.Modules.Add(module);
            var processor = (IQuickPulseTelemetryProcessor)new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy()); // processor will register self with module within constructor

            // ASSERT
            Assert.IsInstanceOfType(module.ServiceClient, typeof(QuickPulseServiceClient));
            Assert.AreEqual(expectedEndpoint, module.ServiceClient.CurrentServiceUri.AbsoluteUri, "module is invalid");
            Assert.AreEqual(expectedEndpoint, processor.ServiceEndpoint.AbsoluteUri, "processor is invalid");
        }

        [TestMethod]
        [TestCategory("QuickPulseEndpoint")]
        public void QuickPulseTelemetryModuleInitializesServiceClient_FromConfigFile_WithCustomEndpoint()
        {
            // ARRANGE
            var configuration = new TelemetryConfiguration();
            var expectedEndpoint = "https://127.0.0.1/QuickPulseService.svc";

            string configFileContents = TelemetryConfigurationFactoryHelper.BuildConfiguration(
                module: @"<Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryModule, Microsoft.AI.PerfCounterCollector""><QuickPulseServiceEndpoint>https://127.0.0.1/QuickPulseService.svc</QuickPulseServiceEndpoint></Add>",
                processor: @"<Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryProcessor, Microsoft.AI.PerfCounterCollector""/>"
                );
            TelemetryConfigurationFactoryHelper.Initialize(configuration, TelemetryModules.Instance, configFileContents);

            // ASSERT
            var module = TelemetryModules.Instance.Modules.OfType<QuickPulseTelemetryModule>().SingleOrDefault();
            Assert.IsNotNull(module, "module was not initialized");

            var processor = configuration.TelemetryProcessors.OfType<IQuickPulseTelemetryProcessor>().SingleOrDefault();
            Assert.IsNotNull(processor, "processor was not initialized");

            Assert.IsInstanceOfType(module.ServiceClient, typeof(QuickPulseServiceClient));
            Assert.AreEqual(expectedEndpoint, module.ServiceClient.CurrentServiceUri.AbsoluteUri, "module is invalid");
            Assert.AreEqual(expectedEndpoint, processor.ServiceEndpoint.AbsoluteUri, "processor is invalid");
        }

        [TestMethod]
        [TestCategory("QuickPulseEndpoint")]
        [TestCategory("ConnectionString")]
        public void QuickPulseTelemetryModuleInitializesServiceClient_WithConnectionString()
        {
            // ARRANGE
            var explicitEndpoint = "https://127.0.0.1/";
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;LiveEndpoint={explicitEndpoint}";
            var expectedEndpoint = $"{explicitEndpoint}QuickPulseService.svc";

            var configuration = new TelemetryConfiguration
            {
                ConnectionString = connectionString
            };

            var module = new QuickPulseTelemetryModule(null, null, null, null, null, null);
            module.Initialize(configuration);
            TelemetryModules.Instance.Modules.Add(module);
            var processor = (IQuickPulseTelemetryProcessor)new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());

            // ASSERT
            Assert.IsInstanceOfType(module.ServiceClient, typeof(QuickPulseServiceClient));
            Assert.AreEqual(expectedEndpoint, module.ServiceClient.CurrentServiceUri.AbsoluteUri, "module is invalid");
            Assert.AreEqual(expectedEndpoint, processor.ServiceEndpoint.AbsoluteUri, "processor is invalid");
        }

        [TestMethod]
        [TestCategory("QuickPulseEndpoint")]
        [TestCategory("ConnectionString")]
        public void QuickPulseTelemetryModuleInitializesServiceClient_FromConfigFile_WithConnectionString()
        {
            // ARRANGE

            var explicitEndpoint = "https://127.0.0.1/";
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;LiveEndpoint={explicitEndpoint}";
            var expectedEndpoint = $"{explicitEndpoint}QuickPulseService.svc";

            var configuration = new TelemetryConfiguration();

            string configFileContents = TelemetryConfigurationFactoryHelper.BuildConfiguration(
                connectionString: connectionString,
                module: @"<Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryModule, Microsoft.AI.PerfCounterCollector""/>",
                processor: @"<Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryProcessor, Microsoft.AI.PerfCounterCollector""/>"
                );
            TelemetryConfigurationFactoryHelper.Initialize(configuration, TelemetryModules.Instance, configFileContents);

            // ASSERT
            var module = TelemetryModules.Instance.Modules.OfType<QuickPulseTelemetryModule>().SingleOrDefault();
            Assert.IsNotNull(module, "module was not initialized");

            var processor = configuration.TelemetryProcessors.OfType<IQuickPulseTelemetryProcessor>().SingleOrDefault();
            Assert.IsNotNull(processor, "processor was not initialized");

            Assert.IsInstanceOfType(module.ServiceClient, typeof(QuickPulseServiceClient));
            Assert.AreEqual(expectedEndpoint, module.ServiceClient.CurrentServiceUri.AbsoluteUri, "module is invalid");
            Assert.AreEqual(expectedEndpoint, processor.ServiceEndpoint.AbsoluteUri, "processor is invalid");
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleDoesNothingWithoutInstrumentationKey()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(null, null, serviceClient, performanceCollector, topCpuCollector, timings);

            module.Initialize(new TelemetryConfiguration());

            // ACT
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

            // ASSERT
            Assert.AreEqual(0, serviceClient.PingCount);
            Assert.AreEqual(0, serviceClient.SnappedSamples.Count);
        }

        [TestMethod]
        public void QuickPulseTelemetryModulePicksUpInstrumentationKeyAsItGoes()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            var config = new TelemetryConfiguration();
            module.Initialize(config);

            // ACT
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            config.InstrumentationKey = "some ikey";
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

            // ASSERT
            Assert.IsTrue(serviceClient.PingCount > 0);
            Assert.IsTrue(serviceClient.SnappedSamples.Count > 0);
        }

        [TestMethod]
        public void QuickPulseTelemetryModulePingsService()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = false, ReturnValueFromSubmitSample = false };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(null, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT
            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // ASSERT
            Thread.Sleep((int)(interval.TotalMilliseconds * 100));

            Assert.IsTrue(serviceClient.PingCount > 0);
            Assert.AreEqual(0, serviceClient.SnappedSamples.Count);
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleCollectsData()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pause = TimeSpan.FromSeconds(1);
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var filter1 = new[]
            {
                new FilterConjunctionGroupInfo()
                {
                    Filters = new[] { new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Request1" } }
                }
            };
            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "Metric0",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = filter1
                }
            };
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "1", Metrics = metrics };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock()
            {
                TopProcesses = new List<Tuple<string, int>>() { Tuple.Create("Process1", 25) }
            };

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            telemetryProcessor.Initialize(new TelemetryConfiguration());
            module.RegisterTelemetryProcessor(telemetryProcessor);

            // ACT
            var telemetryConfiguration = new TelemetryConfiguration() { InstrumentationKey = "some ikey" };
            module.Initialize(telemetryConfiguration);

            Thread.Sleep(pause);

            telemetryProcessor.Process(new RequestTelemetry() { Id = "1", Name = "Request1", Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new DependencyTelemetry() { Context = { InstrumentationKey = "some ikey" } });

            Thread.Sleep(pause);

            Assert.AreEqual(1, serviceClient.PingCount);

            // ASSERT
            serviceClient.ReturnValueFromPing = false;
            serviceClient.ReturnValueFromSubmitSample = false;

            Thread.Sleep(pause);
            Assert.IsTrue(serviceClient.SnappedSamples.Count > 0);

            Assert.IsTrue(serviceClient.SnappedSamples.Any(s => s.AIRequestsPerSecond > 0));
            Assert.IsTrue(serviceClient.SnappedSamples.Any(s => s.AIDependencyCallsPerSecond > 0));
            Assert.IsTrue(
                serviceClient.SnappedSamples.Any(s => Math.Abs(s.PerfCountersLookup[@"\Processor(_Total)\% Processor Time"]) > double.Epsilon));

            Assert.IsTrue(
                serviceClient.SnappedSamples.TrueForAll(s => s.TopCpuData.Single().Item1 == "Process1" && s.TopCpuData.Single().Item2 == 25));

            Assert.IsTrue(
                serviceClient.SnappedSamples.Any(
                    s =>
                    s.CollectionConfigurationAccumulator.MetricAccumulators.Any(
                        a => a.Value.MetricId == "Metric0" && a.Value.CalculateAggregation(out long count) == 1.0d && count == 1)));
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleDoesNotCollectTopCpuDataWhenSwitchedOff()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pause = TimeSpan.FromMilliseconds(100);
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock()
            {
                TopProcesses = new List<Tuple<string, int>>() { Tuple.Create("Process1", 25) }
            };

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);
            module.DisableTopCpuProcesses = true;

            // ACT
            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            Thread.Sleep(pause);

            Assert.AreEqual(1, serviceClient.PingCount);

            // ASSERT
            serviceClient.ReturnValueFromPing = false;
            serviceClient.ReturnValueFromSubmitSample = false;

            Thread.Sleep(pause);
            Assert.IsTrue(serviceClient.SnappedSamples.Count > 0);

            Assert.IsTrue(serviceClient.SnappedSamples.TrueForAll(s => !s.TopCpuData.Any()));
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleInteractsWithTelemetryProcessorCorrectlyWhenLoadedAtRuntime()
        {
            // ARRANGE
            TelemetryConfiguration config = new TelemetryConfiguration();
            QuickPulseTelemetryModule newModule = new QuickPulseTelemetryModule();
            newModule.Initialize(config);

            config.TelemetryProcessorChainBuilder.Use(next =>
            {
                QuickPulseTelemetryProcessor processor = new QuickPulseTelemetryProcessor(next);
                newModule.RegisterTelemetryProcessor(processor);
                return processor;
            });
            config.TelemetryProcessorChainBuilder.Build();

            TelemetryModules.Instance.Modules.Add(newModule);

            // ASSERT
            var module = TelemetryModules.Instance.Modules.OfType<QuickPulseTelemetryModule>().SingleOrDefault();
            Assert.IsNotNull(module);

            var telemetryProcessor = config.TelemetryProcessors.OfType<QuickPulseTelemetryProcessor>().SingleOrDefault();
            Assert.IsNotNull(telemetryProcessor);

            Assert.AreEqual(telemetryProcessor, module.TelemetryProcessors.SingleOrDefault());

            newModule.Dispose();
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleOnlyInitializesPerformanceCollectorAfterCollectionStarts()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = false, ReturnValueFromSubmitSample = false };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT & ASSERT
            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            Thread.Sleep((int)(interval.TotalMilliseconds * 100));

            Assert.IsFalse(performanceCollector.PerformanceCounters.Any());

            serviceClient.ReturnValueFromPing = true;

            Thread.Sleep((int)(interval.TotalMilliseconds * 100));

            Assert.IsTrue(performanceCollector.PerformanceCounters.Any());
            Assert.IsTrue(
                serviceClient.SnappedSamples.All(s => Math.Abs(s.PerfCountersLookup[@"\Processor(_Total)\% Processor Time"]) > double.Epsilon));
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleTimestampsDataSamples()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(null, null, serviceClient, performanceCollector, topCpuCollector, timings);

            var timestampStart = DateTimeOffset.UtcNow;

            // ACT
            module.Initialize(new TelemetryConfiguration());

            Thread.Sleep((int)(interval.TotalMilliseconds * 100));

            // ASSERT
            var timestampEnd = DateTimeOffset.UtcNow;
            Assert.IsTrue(serviceClient.SnappedSamples.All(s => s.StartTimestamp > timestampStart));
            Assert.IsTrue(serviceClient.SnappedSamples.All(s => s.StartTimestamp < timestampEnd));
            Assert.IsTrue(serviceClient.SnappedSamples.All(s => s.StartTimestamp <= s.EndTimestamp));
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleSupportsMultipleTelemetryProcessorsForMultipleConfigurations()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pollingInterval = TimeSpan.FromMilliseconds(1);
            var collectionInterval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(pollingInterval, collectionInterval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            CollectionConfigurationError[] errors;
            var accumulatorManager =
                new QuickPulseDataAccumulatorManager(
                    new CollectionConfiguration(
                        new CollectionConfigurationInfo() { ETag = string.Empty, Metrics = new CalculatedMetricInfo[0] },
                        out errors,
                        new ClockMock()));
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(
                collectionTimeSlotManager,
                accumulatorManager,
                serviceClient,
                performanceCollector,
                topCpuCollector,
                timings);

            const int TelemetryProcessorCount = 4;

            var ikey = "some ikey";
            var config = new TelemetryConfiguration { InstrumentationKey = ikey };

            // spawn a bunch of configurations, each one having an individual instance of QuickPulseTelemetryProcessor
            var telemetryProcessors = new List<QuickPulseTelemetryProcessor>();
            for (int i = 0; i < TelemetryProcessorCount; i++)
            {
                var configuration = new TelemetryConfiguration();
                var builder = configuration.TelemetryProcessorChainBuilder;
                builder = builder.Use(current => new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy()));
                builder.Build();

                telemetryProcessors.Add(configuration.TelemetryProcessors.OfType<QuickPulseTelemetryProcessor>().Single());
            }

            // ACT
            foreach (var telemetryProcessor in telemetryProcessors)
            {
                module.RegisterTelemetryProcessor(telemetryProcessor);
            }

            module.Initialize(config);

            Thread.Sleep(TimeSpan.FromMilliseconds(100));

            // send data to each instance of QuickPulseTelemetryProcessor
            var request = new RequestTelemetry() { ResponseCode = "200", Success = true, Context = { InstrumentationKey = ikey } };
            telemetryProcessors[0].Process(request);

            request = new RequestTelemetry() { ResponseCode = "500", Success = false, Context = { InstrumentationKey = ikey } };
            telemetryProcessors[1].Process(request);

            var dependency = new DependencyTelemetry() { Success = true, Context = { InstrumentationKey = ikey } };
            telemetryProcessors[2].Process(dependency);

            dependency = new DependencyTelemetry() { Success = false, Context = { InstrumentationKey = ikey } };
            telemetryProcessors[3].Process(dependency);

            Thread.Sleep(TimeSpan.FromMilliseconds(100));

            // ASSERT
            // verify that every telemetry processor has contributed to the accumulator
            int samplesWithSomeRequests = serviceClient.SnappedSamples.Count(s => s.AIRequestsPerSecond > 0);
            int samplesWithSomeDependencies = serviceClient.SnappedSamples.Count(s => s.AIDependencyCallsPerSecond > 0);

            Assert.AreEqual(TelemetryProcessorCount, module.TelemetryProcessors.Count);
            Assert.IsTrue(samplesWithSomeRequests > 0 && samplesWithSomeRequests <= 2);
            Assert.AreEqual(1, serviceClient.SnappedSamples.Count(s => s.AIRequestsFailedPerSecond > 0));
            Assert.IsTrue(samplesWithSomeDependencies > 0 && samplesWithSomeDependencies < 2);
            Assert.AreEqual(1, serviceClient.SnappedSamples.Count(s => s.AIDependencyCallsFailedPerSecond > 0));
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleManagesTimersCorrectly()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pollingInterval = TimeSpan.FromSeconds(1);
            var collectionInterval = TimeSpan.FromMilliseconds(400);
            var timings = new QuickPulseTimings(pollingInterval, collectionInterval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = false, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT & ASSERT
            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // initially, the module is in the polling state
            Thread.Sleep((int)(2.5 * pollingInterval.TotalMilliseconds));
            serviceClient.CountersEnabled = false;

            // 2.5 polling intervals have elapsed, we must have pinged the service 3 times (the first time immediately upon initialization), but no samples yet
            Assert.AreEqual(3, serviceClient.PingCount, "Ping count 1");
            Assert.AreEqual(0, serviceClient.SnappedSamples.Count, "Sample count 1");

            // now the service wants the data
            serviceClient.Reset();
            serviceClient.ReturnValueFromPing = true;
            serviceClient.ReturnValueFromSubmitSample = true;

            serviceClient.CountersEnabled = true;
            Thread.Sleep((int)(5 * collectionInterval.TotalMilliseconds));
            serviceClient.CountersEnabled = false;

            // a number of  collection intervals have elapsed, we must have pinged the service once, and then started sending samples
            Assert.AreEqual(1, serviceClient.PingCount, "Ping count 2");
            Assert.IsTrue(serviceClient.SnappedSamples.Count > 0, "Sample count 2");

            lock (serviceClient.ResponseLock)
            {
                // the service doesn't want the data anymore
                serviceClient.ReturnValueFromPing = false;
                serviceClient.ReturnValueFromSubmitSample = false;

                serviceClient.Reset();
                serviceClient.CountersEnabled = true;
            }

            Thread.Sleep((int)(2.9 * pollingInterval.TotalMilliseconds));
            serviceClient.CountersEnabled = false;

            // 2 polling intervals have elapsed, we must have submitted one batch of samples, stopped collecting and pinged the service twice afterwards
            Assert.AreEqual(1, serviceClient.SnappedSamples.Count / serviceClient.LastSampleBatchSize, "Sample count 3");
            Assert.AreEqual(2, serviceClient.PingCount, "Ping count 3");
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleUpdatesCollectionConfiguration()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pollingInterval = TimeSpan.FromSeconds(1);
            var collectionInterval = TimeSpan.FromMilliseconds(400);
            var timings = new QuickPulseTimings(pollingInterval, collectionInterval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT & ASSERT
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag1", QuotaInfo = new QuotaConfigurationInfo() { InitialQuota = 50, MaxQuota=60, QuotaAccrualRatePerSec=10 } };

            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            Thread.Sleep(pollingInterval);
            Thread.Sleep((int)(2.5 * collectionInterval.TotalMilliseconds));
            Assert.AreEqual("ETag1", serviceClient.SnappedSamples.Last().CollectionConfigurationAccumulator.CollectionConfiguration.ETag);

            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag2" };
            Thread.Sleep((int)(10 * collectionInterval.TotalMilliseconds));
            Assert.AreEqual("ETag2", serviceClient.SnappedSamples.Last().CollectionConfigurationAccumulator.CollectionConfiguration.ETag);
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleUpdatesCollectionConfigurationWithQuotaAccrualRate()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pollingInterval = TimeSpan.FromSeconds(1);
            var collectionInterval = TimeSpan.FromMilliseconds(400);
            var timings = new QuickPulseTimings(pollingInterval, collectionInterval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();
            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT & ASSERT
            CollectionConfigurationInfo collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                ETag = "ETag1",
                DocumentStreams = new[] { new DocumentStreamInfo() { Id = "wx3", DocumentFilterGroups = new[] { new DocumentFilterConjunctionGroupInfo() { TelemetryType = TelemetryType.Request, Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] } } } } },
                QuotaInfo = new QuotaConfigurationInfo()
                {
                    InitialQuota = 50,
                    QuotaAccrualRatePerSec = 10,
                    MaxQuota = 60
                }
            };
            serviceClient.CollectionConfigurationInfo = collectionConfigurationInfo;
            QuickPulseTelemetryProcessor telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            module.RegisterTelemetryProcessor(telemetryProcessor);
            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out _, new Clock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            for (int i=0;i<100;i++)
            {
                telemetryProcessor.Process(new RequestTelemetry() { Id = "1", Name = "Request1", Context = { InstrumentationKey = "some ikey" } });
            }

            Assert.AreEqual(50, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);
            Thread.Sleep(pollingInterval);

            for (int i = 0; i < 100; i++)
            {
                telemetryProcessor.Process(new RequestTelemetry() { Id = "1", Name = "Request1", Context = { InstrumentationKey = "some ikey" } });
            }

            Assert.AreEqual(60, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleUpdatesCollectionConfigurationWithMaxQuota()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pollingInterval = TimeSpan.FromSeconds(1);
            var collectionInterval = TimeSpan.FromMilliseconds(400);
            var timings = new QuickPulseTimings(pollingInterval, collectionInterval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();
            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT & ASSERT
            CollectionConfigurationInfo collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                ETag = "ETag1",
                DocumentStreams = new[] { new DocumentStreamInfo() { Id = "wx3", DocumentFilterGroups = new[] { new DocumentFilterConjunctionGroupInfo() { TelemetryType = TelemetryType.Request, Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] } } } } },
                QuotaInfo = new QuotaConfigurationInfo()
                {
                    InitialQuota = 50,
                    QuotaAccrualRatePerSec = 40,
                    MaxQuota = 60
                }
            };
            serviceClient.CollectionConfigurationInfo = collectionConfigurationInfo;
            QuickPulseTelemetryProcessor telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            module.RegisterTelemetryProcessor(telemetryProcessor);
            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out _, new Clock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            Thread.Sleep(pollingInterval);

            for (int i = 0; i < 100; i++)
            {
                telemetryProcessor.Process(new RequestTelemetry() { Id = "1", Name = "Request1", Context = { InstrumentationKey = "some ikey" } });
            }

            Assert.AreEqual(60, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleUpdatesGlobalCollectionConfigurationWithQuotaInfo()
        {
#if !NETCOREAPP
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pollingInterval = TimeSpan.FromSeconds(1);
            var collectionInterval = TimeSpan.FromMilliseconds(400);
            var timings = new QuickPulseTimings(pollingInterval, collectionInterval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();
            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT & ASSERT
            CollectionConfigurationInfo collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                ETag = "ETag1",
                DocumentStreams = new[] { new DocumentStreamInfo() { Id = "wx3", DocumentFilterGroups = new[] { new DocumentFilterConjunctionGroupInfo() { TelemetryType = TelemetryType.Request, Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] } } } } },
                QuotaInfo = new QuotaConfigurationInfo()
                {
                    InitialQuota = 50,
                    QuotaAccrualRatePerSec = 10,
                    MaxQuota = 60
                }
            };
            serviceClient.CollectionConfigurationInfo = collectionConfigurationInfo;
            QuickPulseTelemetryProcessor telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            module.RegisterTelemetryProcessor(telemetryProcessor);
            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out _, new Clock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            Thread.Sleep(pollingInterval);

            for (int i = 0; i < 100; i++)
            {
                telemetryProcessor.Process(new RequestTelemetry() { Id = "1", Name = "Request1", Context = { InstrumentationKey = "some ikey" } });
            }

            Assert.AreEqual(60, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);

            CollectionConfigurationInfo collectionConfigurationInfo2 = new CollectionConfigurationInfo()
            {
                ETag = "ETag2",
                DocumentStreams = new[] { new DocumentStreamInfo() { Id = "wx3", DocumentFilterGroups = new[] { new DocumentFilterConjunctionGroupInfo() { TelemetryType = TelemetryType.Request, Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] } } } } },
                QuotaInfo = new QuotaConfigurationInfo()
                {
                    InitialQuota = 0,
                    QuotaAccrualRatePerSec = 1,
                    MaxQuota = 5
                }
            };

            PrivateObject quickPulseTelemetryModuleTester = new PrivateObject(module);
            quickPulseTelemetryModuleTester.Invoke("OnUpdatedConfiguration", collectionConfigurationInfo2);

            Thread.Sleep(pollingInterval);

            for (int i = 0; i < 100; i++)
            {
                telemetryProcessor.Process(new RequestTelemetry() { Id = "1", Name = "Request1", Context = { InstrumentationKey = "some ikey" } });
            }

            Assert.IsTrue(accumulatorManager.CurrentDataAccumulator.GlobalDocumentQuotaReached);
            Assert.AreEqual(61, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);
#endif
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleUpdatesPerformanceCollectorWhenUpdatingCollectionConfiguration()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pollingInterval = TimeSpan.FromMilliseconds(200);
            var collectionInterval = TimeSpan.FromMilliseconds(80);
            var timings = new QuickPulseTimings(pollingInterval, collectionInterval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT & ASSERT
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                ETag = "ETag1",
                Metrics =
                    new[]
                    {
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter1",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"\Memory\Cache Bytes"
                        },
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter2",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"\Memory\Cache Bytes Peak"
                        },
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter3",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"\Processor(_Total)\% Processor Time"
                        }
                    }
            };

            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            Thread.Sleep(pollingInterval);
            Thread.Sleep((int)(2.5 * collectionInterval.TotalMilliseconds));

            Assert.AreEqual("ETag1", serviceClient.SnappedSamples.Last().CollectionConfigurationAccumulator.CollectionConfiguration.ETag);

            // 2 default + 3 configured
            Assert.AreEqual(5, performanceCollector.PerformanceCounters.Count());
            Assert.AreEqual(@"\Memory\Cache Bytes", performanceCollector.PerformanceCounters.Skip(0).First().OriginalString);
            Assert.AreEqual(@"PerformanceCounter1", performanceCollector.PerformanceCounters.Skip(0).First().ReportAs);
            Assert.AreEqual(@"\Memory\Cache Bytes Peak", performanceCollector.PerformanceCounters.Skip(1).First().OriginalString);
            Assert.AreEqual(@"PerformanceCounter2", performanceCollector.PerformanceCounters.Skip(1).First().ReportAs);
            Assert.AreEqual(@"\Processor(_Total)\% Processor Time", performanceCollector.PerformanceCounters.Skip(2).First().OriginalString);
            Assert.AreEqual(@"PerformanceCounter3", performanceCollector.PerformanceCounters.Skip(2).First().ReportAs);
            Assert.AreEqual(@"\Memory\Committed Bytes", performanceCollector.PerformanceCounters.Skip(3).First().OriginalString);
            Assert.AreEqual(@"\Memory\Committed Bytes", performanceCollector.PerformanceCounters.Skip(3).First().ReportAs);
            Assert.AreEqual(@"\Processor(_Total)\% Processor Time", performanceCollector.PerformanceCounters.Skip(4).First().OriginalString);
            Assert.AreEqual(@"\Processor(_Total)\% Processor Time", performanceCollector.PerformanceCounters.Skip(4).First().ReportAs);

            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                ETag = "ETag2",
                Metrics =
                    new[]
                    {
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter1",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"\MEMORY\Cache Bytes"
                        },
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter5",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"\Memory\Commit Limit"
                        }
                    }
            };
            Thread.Sleep((int)(10 * collectionInterval.TotalMilliseconds));

            Assert.AreEqual("ETag2", serviceClient.SnappedSamples.Last().CollectionConfigurationAccumulator.CollectionConfiguration.ETag);

            // 2 default + 1 configured remaining + 1 configured new
            Assert.AreEqual(4, performanceCollector.PerformanceCounters.Count());
            Assert.AreEqual(@"\Memory\Cache Bytes", performanceCollector.PerformanceCounters.Skip(0).First().OriginalString);
            Assert.AreEqual(@"PerformanceCounter1", performanceCollector.PerformanceCounters.Skip(0).First().ReportAs);
            Assert.AreEqual(@"\Memory\Committed Bytes", performanceCollector.PerformanceCounters.Skip(1).First().OriginalString);
            Assert.AreEqual(@"\Memory\Committed Bytes", performanceCollector.PerformanceCounters.Skip(1).First().ReportAs);
            Assert.AreEqual(@"\Processor(_Total)\% Processor Time", performanceCollector.PerformanceCounters.Skip(2).First().OriginalString);
            Assert.AreEqual(@"\Processor(_Total)\% Processor Time", performanceCollector.PerformanceCounters.Skip(2).First().ReportAs);
            Assert.AreEqual(@"\Memory\Commit Limit", performanceCollector.PerformanceCounters.Skip(3).First().OriginalString);
            Assert.AreEqual(@"PerformanceCounter5", performanceCollector.PerformanceCounters.Skip(3).First().ReportAs);
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleReportsErrorsFromPerformanceCollectorWhenUpdatingCollectionConfiguration()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var pollingInterval = TimeSpan.FromMilliseconds(200);
            var collectionInterval = TimeSpan.FromMilliseconds(80);
            var timings = new QuickPulseTimings(pollingInterval, collectionInterval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT & ASSERT
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                ETag = "ETag1",
                Metrics =
                    new[]
                    {
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter1",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"\SomeCategory(SomeInstance)\SomeCounter"
                        },
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter2",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"\Memory\Cache Bytes Peak"
                        },
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter3",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"NonParseable"
                        },
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter4",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"\SomeObject\SomeCounter"
                        },
                        new CalculatedMetricInfo()
                        {
                            Id = "PerformanceCounter4",
                            TelemetryType = TelemetryType.PerformanceCounter,
                            Projection = @"\SomeObject1\SomeCounter1"
                        }
                    }
            };

            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            Thread.Sleep(pollingInterval);
            Thread.Sleep((int)(2.5 * collectionInterval.TotalMilliseconds));

            Assert.AreEqual("ETag1", serviceClient.SnappedSamples.Last().CollectionConfigurationAccumulator.CollectionConfiguration.ETag);

            Assert.AreEqual(5, performanceCollector.PerformanceCounters.Count());
            Assert.AreEqual(@"\SomeCategory(SomeInstance)\SomeCounter", performanceCollector.PerformanceCounters.Skip(0).First().OriginalString);
            Assert.AreEqual(@"PerformanceCounter1", performanceCollector.PerformanceCounters.Skip(0).First().ReportAs);
            Assert.AreEqual(@"\Memory\Cache Bytes Peak", performanceCollector.PerformanceCounters.Skip(1).First().OriginalString);
            Assert.AreEqual(@"PerformanceCounter2", performanceCollector.PerformanceCounters.Skip(1).First().ReportAs);
            Assert.AreEqual(@"\SomeObject\SomeCounter", performanceCollector.PerformanceCounters.Skip(2).First().OriginalString);
            Assert.AreEqual(@"PerformanceCounter4", performanceCollector.PerformanceCounters.Skip(2).First().ReportAs);
            Assert.AreEqual(@"\Memory\Committed Bytes", performanceCollector.PerformanceCounters.Skip(3).First().OriginalString);
            Assert.AreEqual(@"\Memory\Committed Bytes", performanceCollector.PerformanceCounters.Skip(3).First().ReportAs);
            Assert.AreEqual(@"\Processor(_Total)\% Processor Time", performanceCollector.PerformanceCounters.Skip(4).First().OriginalString);
            Assert.AreEqual(@"\Processor(_Total)\% Processor Time", performanceCollector.PerformanceCounters.Skip(4).First().ReportAs);

            CollectionConfigurationError[] errors = serviceClient.CollectionConfigurationErrors;
            Assert.AreEqual(2, errors.Length);

            Assert.AreEqual(CollectionConfigurationErrorType.PerformanceCounterDuplicateIds, errors[0].ErrorType);
            Assert.AreEqual(@"Duplicate performance counter id 'PerformanceCounter4'", errors[0].Message);
            Assert.AreEqual(string.Empty, errors[0].FullException);
            Assert.AreEqual(2, errors[0].Data.Count);
            Assert.AreEqual("PerformanceCounter4", errors[0].Data["MetricId"]);
            Assert.AreEqual("ETag1", errors[0].Data["ETag"]);

            Assert.AreEqual(CollectionConfigurationErrorType.PerformanceCounterParsing, errors[1].ErrorType);
            string expected = string.Format(CultureInfo.InvariantCulture, "Error parsing performance counter: '(PerformanceCounter3, NonParseable)'. Invalid performance counter name format: NonParseable. Expected formats are \\category(instance)\\counter or \\category\\counter{0}Parameter name: performanceCounter", Environment.NewLine);
            Assert.AreEqual(expected, errors[1].Message);
            Assert.AreEqual(string.Empty, errors[1].FullException);
            Assert.AreEqual(1, errors[1].Data.Count);
            Assert.AreEqual("PerformanceCounter3", errors[1].Data["MetricId"]);
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleResendsFailedSamples()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = null };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // ACT
            // below timeout should be sufficient for the module to get to the maximum storage capacity
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // ASSERT
            Assert.AreEqual(10, serviceClient.LastSampleBatchSize);
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleHandlesUnexpectedExceptions()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval, interval, interval, interval, interval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock
            {
                AlwaysThrow = true,
                ReturnValueFromPing = false,
                ReturnValueFromSubmitSample = null
            };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // ACT
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // ASSERT
            // it shouldn't throw and must keep pinging
            int pingCount = serviceClient.PingCount;
            Assert.IsTrue(pingCount > 5, string.Format(CultureInfo.InvariantCulture, "PingCount is not high enough, the value is {0}", pingCount));
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleDisposesCorrectly()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval, interval, interval, interval, interval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // ACT
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

            // ASSERT
            module.Dispose();
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleDoesNotLeakThreads()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval, interval, interval, interval, interval);
            var collectionTimeSlotManager = new QuickPulseCollectionTimeSlotManagerMock(timings);

            // this will flip-flop between collection and no collection, creating and ending a collection thread each time
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = false };
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();

            var module = new QuickPulseTelemetryModule(collectionTimeSlotManager, null, serviceClient, performanceCollector, topCpuCollector, timings);

            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            int initialThreadCount = Process.GetCurrentProcess().Threads.Count;

            // ACT
            Thread.Sleep(TimeSpan.FromMilliseconds(300));

            // ASSERT
            // we don't expect to find many more threads, even though other components might be spinning new ones up and down
            var threadDelta = Process.GetCurrentProcess().Threads.Count - initialThreadCount;
            Assert.IsTrue(Math.Abs(threadDelta) < 5, threadDelta.ToString(CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleUpdatesTelemetryProcessorWithEndpointRedirectReceivedFromServiceClient()
        {
            if (QuickPulseTelemetryModuleTests.Ignored)
            {
                return;
            }

            // ARRANGE
            var interval = TimeSpan.FromMilliseconds(1);
            var timings = new QuickPulseTimings(interval, interval);
            Uri redirectedEndpoint = new Uri("https://redirected-qps.com");
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = false, ReturnValueFromSubmitSample = false, CurrentServiceUriMockValue = redirectedEndpoint};
            var performanceCollector = new PerformanceCollectorMock();
            var topCpuCollector = new QuickPulseTopCpuCollectorMock();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var module = new QuickPulseTelemetryModule(null, null, serviceClient, performanceCollector, topCpuCollector, timings);

            // ACT
            module.Initialize(new TelemetryConfiguration() { InstrumentationKey = "some ikey" });
            module.RegisterTelemetryProcessor(telemetryProcessor);

            // ASSERT
            Thread.Sleep((int)(interval.TotalMilliseconds * 100));

            Assert.AreEqual(redirectedEndpoint, (telemetryProcessor as IQuickPulseTelemetryProcessor).ServiceEndpoint);
        }
#pragma warning restore 0162
    }
}