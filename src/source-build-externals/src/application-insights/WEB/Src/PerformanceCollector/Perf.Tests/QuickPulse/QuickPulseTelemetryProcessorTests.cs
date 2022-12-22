namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Common.Internal;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Tests.Helpers;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuickPulseTelemetryProcessorTests
    {
        private const int MaxFieldLength = 32768;

        private static readonly CollectionConfiguration EmptyCollectionConfiguration =
            new CollectionConfiguration(
                new CollectionConfigurationInfo() { ETag = string.Empty, Metrics = new CalculatedMetricInfo[0] },
                out errors,
                new ClockMock());

        private static CollectionConfigurationError[] errors;

        [TestInitialize]
        public void TestInitialize()
        {
            QuickPulseTestHelper.ClearEnvironment();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuickPulseTelemetryProcessorThrowsIfNextIsNull()
        {
            new QuickPulseTelemetryProcessor(null);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorRegistersWithModule()
        {
            // ARRANGE
            var module = new QuickPulseTelemetryModule(null, null, null, null, null, null);

            TelemetryModules.Instance.Modules.Add(module);

            // ACT
            var spy = new SimpleTelemetryProcessorSpy();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(spy);
            telemetryProcessor.Initialize(new TelemetryConfiguration());

            // ASSERT
            Assert.AreEqual(telemetryProcessor, module.TelemetryProcessors.Single());
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorCallsNext()
        {
            // ARRANGE
            var spy = new SimpleTelemetryProcessorSpy();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(spy);

            // ACT
            telemetryProcessor.Process(new RequestTelemetry() { Context = { InstrumentationKey = "some ikey" } });

            // ASSERT
            Assert.AreEqual(1, spy.ReceivedCalls);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorKeepsAccurateCountOfRequests()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // ACT
            telemetryProcessor.Process(
                new RequestTelemetry()
                {
                    Success = false,
                    ResponseCode = "200",
                    Duration = TimeSpan.FromSeconds(1),
                    Context = { InstrumentationKey = "some ikey" }
                });
            telemetryProcessor.Process(
                new RequestTelemetry()
                {
                    Success = true,
                    ResponseCode = "200",
                    Duration = TimeSpan.FromSeconds(2),
                    Context = { InstrumentationKey = "some ikey" }
                });
            telemetryProcessor.Process(
                new RequestTelemetry()
                {
                    Success = false,
                    ResponseCode = string.Empty,
                    Duration = TimeSpan.FromSeconds(3),
                    Context = { InstrumentationKey = "some ikey" }
                });
            telemetryProcessor.Process(
                new RequestTelemetry()
                {
                    Success = null,
                    ResponseCode = string.Empty,
                    Duration = TimeSpan.FromSeconds(4),
                    Context = { InstrumentationKey = "some ikey" }
                });
            telemetryProcessor.Process(
                new RequestTelemetry()
                {
                    Success = true,
                    ResponseCode = string.Empty,
                    Duration = TimeSpan.FromSeconds(5),
                    Context = { InstrumentationKey = "some ikey" }
                });
            telemetryProcessor.Process(
                new RequestTelemetry()
                {
                    Success = null,
                    ResponseCode = "404",
                    Duration = TimeSpan.FromSeconds(6),
                    Context = { InstrumentationKey = "some ikey" }
                });

            // ASSERT
            Assert.AreEqual(6, accumulatorManager.CurrentDataAccumulator.AIRequestCount);
            Assert.AreEqual(
                1 + 2 + 3 + 4 + 5 + 6,
                TimeSpan.FromTicks(accumulatorManager.CurrentDataAccumulator.AIRequestDurationInTicks).TotalSeconds);
            Assert.AreEqual(4, accumulatorManager.CurrentDataAccumulator.AIRequestSuccessCount);
            Assert.AreEqual(2, accumulatorManager.CurrentDataAccumulator.AIRequestFailureCount);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorKeepsAccurateCountOfDependencies()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // ACT
            telemetryProcessor.Process(
                new DependencyTelemetry() { Success = true, Duration = TimeSpan.FromSeconds(1), Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(
                new DependencyTelemetry() { Success = true, Duration = TimeSpan.FromSeconds(1), Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(
                new DependencyTelemetry() { Success = false, Duration = TimeSpan.FromSeconds(2), Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(
                new DependencyTelemetry() { Success = null, Duration = TimeSpan.FromSeconds(3), Context = { InstrumentationKey = "some ikey" } });

            // ASSERT
            Assert.AreEqual(4, accumulatorManager.CurrentDataAccumulator.AIDependencyCallCount);
            Assert.AreEqual(1 + 1 + 2 + 3, TimeSpan.FromTicks(accumulatorManager.CurrentDataAccumulator.AIDependencyCallDurationInTicks).TotalSeconds);
            Assert.AreEqual(2, accumulatorManager.CurrentDataAccumulator.AIDependencyCallSuccessCount);
            Assert.AreEqual(1, accumulatorManager.CurrentDataAccumulator.AIDependencyCallFailureCount);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorKeepsAccurateCountOfExceptions()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // ACT
            telemetryProcessor.Process(new ExceptionTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new ExceptionTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new ExceptionTelemetry() { Context = { InstrumentationKey = "some ikey" } });

            // ASSERT
            Assert.AreEqual(3, accumulatorManager.CurrentDataAccumulator.AIExceptionCount);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorStopsCollection()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var endpoint = new Uri("http://microsoft.com");
            var config = new TelemetryConfiguration() { InstrumentationKey = "some ikey" };

            // ACT
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(accumulatorManager, endpoint, config);
            telemetryProcessor.Process(new RequestTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StopCollection();
            telemetryProcessor.Process(new DependencyTelemetry() { Context = { InstrumentationKey = "some ikey" } });

            // ASSERT
            Assert.AreEqual(1, accumulatorManager.CurrentDataAccumulator.AIRequestCount);
            Assert.AreEqual(0, accumulatorManager.CurrentDataAccumulator.AIDependencyCallCount);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorIgnoresUnrelatedTelemetryItems()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // ACT
            telemetryProcessor.Process(new EventTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new ExceptionTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new MetricTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new PageViewTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new TraceTelemetry() { Context = { InstrumentationKey = "some ikey" } });

            // ASSERT
            Assert.AreEqual(0, accumulatorManager.CurrentDataAccumulator.AIRequestCount);
            Assert.AreEqual(0, accumulatorManager.CurrentDataAccumulator.AIDependencyCallCount);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorIgnoresTelemetryItemsToDifferentInstrumentationKeys()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());

            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // ACT
            telemetryProcessor.Process(new RequestTelemetry() { Context = { InstrumentationKey = "some other ikey" } });
            telemetryProcessor.Process(new RequestTelemetry() { Context = { InstrumentationKey = "some ikey" } });

            // ASSERT
            Assert.AreEqual(1, accumulatorManager.CurrentDataAccumulator.AIRequestCount);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorHandlesMultipleThreadsCorrectly()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());

            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            // expected data loss if threading is misimplemented is around 10% (established through experiment)
            int taskCount = 10000;
            var tasks = new List<Task>(taskCount);

            for (int i = 0; i < taskCount; i++)
            {
                var requestTelemetry = new RequestTelemetry()
                {
                    ResponseCode = (i % 2 == 0) ? "200" : "500",
                    Duration = TimeSpan.FromMilliseconds(i),
                    Context = { InstrumentationKey = "some ikey" }
                };

                var task = new Task(() => telemetryProcessor.Process(requestTelemetry));
                tasks.Add(task);
            }

            // ACT
            tasks.ForEach(task => task.Start());

            Task.WaitAll(tasks.ToArray());

            // ASSERT
            Assert.AreEqual(taskCount, accumulatorManager.CurrentDataAccumulator.AIRequestCount);
            Assert.AreEqual(taskCount / 2, accumulatorManager.CurrentDataAccumulator.AIRequestSuccessCount);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorSwitchesBetweenMultipleAccumulatorManagers()
        {
            // ARRANGE
            var accumulatorManager1 = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var accumulatorManager2 = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());

            // ACT
            var serviceEndpoint = new Uri("http://microsoft.com");
            var config = new TelemetryConfiguration() { InstrumentationKey = "some ikey" };
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(accumulatorManager1, serviceEndpoint, config);
            telemetryProcessor.Process(new RequestTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StopCollection();

            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(accumulatorManager2, serviceEndpoint, config);
            telemetryProcessor.Process(new DependencyTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StopCollection();

            // ASSERT
            Assert.AreEqual(1, accumulatorManager1.CurrentDataAccumulator.AIRequestCount);
            Assert.AreEqual(0, accumulatorManager1.CurrentDataAccumulator.AIDependencyCallCount);

            Assert.AreEqual(0, accumulatorManager2.CurrentDataAccumulator.AIRequestCount);
            Assert.AreEqual(1, accumulatorManager2.CurrentDataAccumulator.AIDependencyCallCount);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QuickPulseTelemetryProcessorMustBeStoppedBeforeReceivingStartCommand()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());

            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://test.com"),
                new TelemetryConfiguration());

            // ACT
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://test.com"),
                new TelemetryConfiguration());

            // ASSERT
            // must throw
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorFiltersOutDependencyCallsToQuickPulseServiceDuringCollection()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var simpleTelemetryProcessorSpy = new SimpleTelemetryProcessorSpy();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(simpleTelemetryProcessorSpy);
            var config = new TelemetryConfiguration() { InstrumentationKey = "some ikey" };

            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("https://qps.cloudapp.net/endpoint.svc"),
                config);

            // ACT
            telemetryProcessor.Process(
                new DependencyTelemetry() { Target = "microsoft.ru", Context = { InstrumentationKey = config.InstrumentationKey } });
            telemetryProcessor.Process(
                new DependencyTelemetry() { Target = "qps.cloudapp.net", Context = { InstrumentationKey = config.InstrumentationKey } });
            telemetryProcessor.Process(
               new DependencyTelemetry() { Target = "qps.cloudapp.net | I5UqryrMvK", Context = { InstrumentationKey = config.InstrumentationKey } });
            telemetryProcessor.Process(
                new DependencyTelemetry() { Target = "bing.com", Context = { InstrumentationKey = config.InstrumentationKey } });

            // ASSERT
            Assert.AreEqual(2, simpleTelemetryProcessorSpy.ReceivedCalls);
            Assert.AreEqual("microsoft.ru", (simpleTelemetryProcessorSpy.ReceivedItems[0] as DependencyTelemetry).Target);
            Assert.AreEqual("bing.com", (simpleTelemetryProcessorSpy.ReceivedItems[1] as DependencyTelemetry).Target);
            Assert.AreEqual(2, accumulatorManager.CurrentDataAccumulator.AIDependencyCallCount);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorFiltersOutDependencyCallsToDefaultQuickPulseServiceEndpointInIdleMode()
        {
            // ARRANGE
            var simpleTelemetryProcessorSpy = new SimpleTelemetryProcessorSpy();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(simpleTelemetryProcessorSpy);

            // ACT
            telemetryProcessor.Process(new DependencyTelemetry() { Target = "microsoft.ru" });
            telemetryProcessor.Process(new DependencyTelemetry() { Target = "rt.services.visualstudio.com" });
            telemetryProcessor.Process(new DependencyTelemetry() { Target = "rt.services.visualstudio.com | I5UqryrMvK" });
            telemetryProcessor.Process(new DependencyTelemetry() { Target = "bing.com" });

            // ASSERT
            Assert.AreEqual(2, simpleTelemetryProcessorSpy.ReceivedCalls);
            Assert.AreEqual("microsoft.ru", (simpleTelemetryProcessorSpy.ReceivedItems[0] as DependencyTelemetry).Target);
            Assert.AreEqual("bing.com", (simpleTelemetryProcessorSpy.ReceivedItems[1] as DependencyTelemetry).Target);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorFiltersOutDependencyCallsToQuickPulseServiceDuringCollectionOnceRedirected()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var simpleTelemetryProcessorSpy = new SimpleTelemetryProcessorSpy();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(simpleTelemetryProcessorSpy);
            var config = new TelemetryConfiguration() { InstrumentationKey = "some ikey" };

            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("https://qps.cloudapp.net/endpoint.svc"),
                config);

            // ACT
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).ServiceEndpoint = new Uri("https://bing.com");
            telemetryProcessor.Process(
                new DependencyTelemetry() { Target = "microsoft.ru", Context = { InstrumentationKey = config.InstrumentationKey } });
            telemetryProcessor.Process(
                new DependencyTelemetry() { Target = "qps.cloudapp.net", Context = { InstrumentationKey = config.InstrumentationKey } });
            telemetryProcessor.Process(
                new DependencyTelemetry() { Target = "bing.com", Context = { InstrumentationKey = config.InstrumentationKey } });
            
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).ServiceEndpoint = new Uri("https://microsoft.ru");
            telemetryProcessor.Process(
                new DependencyTelemetry() { Target = "microsoft.ru", Context = { InstrumentationKey = config.InstrumentationKey } });
            telemetryProcessor.Process(
                new DependencyTelemetry() { Target = "qps.cloudapp.net", Context = { InstrumentationKey = config.InstrumentationKey } });
            telemetryProcessor.Process(
                new DependencyTelemetry() { Target = "bing.com", Context = { InstrumentationKey = config.InstrumentationKey } });
            
            // ASSERT
            Assert.AreEqual(4, simpleTelemetryProcessorSpy.ReceivedCalls);
            Assert.AreEqual("microsoft.ru", (simpleTelemetryProcessorSpy.ReceivedItems[0] as DependencyTelemetry).Target);
            Assert.AreEqual("qps.cloudapp.net", (simpleTelemetryProcessorSpy.ReceivedItems[1] as DependencyTelemetry).Target);
            Assert.AreEqual("qps.cloudapp.net", (simpleTelemetryProcessorSpy.ReceivedItems[2] as DependencyTelemetry).Target);
            Assert.AreEqual("bing.com", (simpleTelemetryProcessorSpy.ReceivedItems[3] as DependencyTelemetry).Target);

            Assert.AreEqual(4, accumulatorManager.CurrentDataAccumulator.AIDependencyCallCount);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorFiltersOutDependencyCallsToQuickPulseServiceEndpointInIdleModeOnceRedirected()
        {
            // ARRANGE
            var simpleTelemetryProcessorSpy = new SimpleTelemetryProcessorSpy();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(simpleTelemetryProcessorSpy);
            var config = new TelemetryConfiguration() {InstrumentationKey = "some ikey"};

            // ACT
            ((IQuickPulseTelemetryProcessor) telemetryProcessor).ServiceEndpoint = new Uri("https://bing.com");
            telemetryProcessor.Process(
                new DependencyTelemetry() {Target = "microsoft.ru", Context = {InstrumentationKey = config.InstrumentationKey}});
            telemetryProcessor.Process(
                new DependencyTelemetry() {Target = "qps.cloudapp.net", Context = {InstrumentationKey = config.InstrumentationKey}});
            telemetryProcessor.Process(
                new DependencyTelemetry() {Target = "bing.com", Context = {InstrumentationKey = config.InstrumentationKey}});

            ((IQuickPulseTelemetryProcessor) telemetryProcessor).ServiceEndpoint = new Uri("https://microsoft.ru");
            telemetryProcessor.Process(
                new DependencyTelemetry() {Target = "microsoft.ru", Context = {InstrumentationKey = config.InstrumentationKey}});
            telemetryProcessor.Process(
                new DependencyTelemetry() {Target = "qps.cloudapp.net", Context = {InstrumentationKey = config.InstrumentationKey}});
            telemetryProcessor.Process(
                new DependencyTelemetry() {Target = "bing.com", Context = {InstrumentationKey = config.InstrumentationKey}});

            // ASSERT
            Assert.AreEqual(4, simpleTelemetryProcessorSpy.ReceivedCalls);
            Assert.AreEqual("microsoft.ru", (simpleTelemetryProcessorSpy.ReceivedItems[0] as DependencyTelemetry).Target);
            Assert.AreEqual("qps.cloudapp.net", (simpleTelemetryProcessorSpy.ReceivedItems[1] as DependencyTelemetry).Target);
            Assert.AreEqual("qps.cloudapp.net", (simpleTelemetryProcessorSpy.ReceivedItems[2] as DependencyTelemetry).Target);
            Assert.AreEqual("bing.com", (simpleTelemetryProcessorSpy.ReceivedItems[3] as DependencyTelemetry).Target);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorCollectsFullTelemetryItemsAndDistributesThemAmongDocumentStreamsCorrectly()
        {
            // ARRANGE
            var requestsAndDependenciesDocumentStreamInfo = new DocumentStreamInfo()
            {
                Id = "StreamRequestsAndDependenciesAndExceptions",
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters =
                                new FilterConjunctionGroupInfo
                                {
                                    Filters =
                                        new[]
                                        {
                                            new FilterInfo { FieldName = "ResponseCode", Predicate = Predicate.Equal, Comparand = "500" },
                                            new FilterInfo { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "0" }
                                        }
                                }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Dependency,
                            Filters =
                                new FilterConjunctionGroupInfo
                                {
                                    Filters = new[] { new FilterInfo { FieldName = "Duration", Predicate = Predicate.Equal, Comparand = "0.00:00:01" } }
                                }
                        },
                         new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters =
                                new FilterConjunctionGroupInfo
                                {
                                    Filters =
                                        new[]
                                        {
                                            new FilterInfo { FieldName = "CustomDimensions.Prop1", Predicate = Predicate.Equal, Comparand = "Val1" },
                                            new FilterInfo { FieldName = "CustomDimensions.Prop2", Predicate = Predicate.Equal, Comparand = "Val2" }
                                        }
                                }
                        },
                    }
            };

            var exceptionsEventsTracesDocumentStreamInfo = new DocumentStreamInfo()
            {
                Id = "StreamExceptionsEventsTraces",
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters =
                                new FilterConjunctionGroupInfo
                                {
                                    Filters =
                                        new[]
                                        {
                                            new FilterInfo { FieldName = "CustomDimensions.Prop1", Predicate = Predicate.Equal, Comparand = "Val1" },
                                            new FilterInfo { FieldName = "CustomDimensions.Prop2", Predicate = Predicate.Equal, Comparand = "Val2" }
                                        }
                                }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Event,
                            Filters =
                                new FilterConjunctionGroupInfo
                                {
                                    Filters = new[] { new FilterInfo { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Event1" } }
                                }
                        },
                         new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Trace,
                            Filters =
                                new FilterConjunctionGroupInfo
                                {
                                    Filters = new[] { new FilterInfo { FieldName = "Message", Predicate = Predicate.Equal, Comparand = "Trace1" } }
                                }
                        }
                    }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { requestsAndDependenciesDocumentStreamInfo, exceptionsEventsTracesDocumentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var request = new RequestTelemetry()
            {
                Name = Guid.NewGuid().ToString(),
                Success = false,
                ResponseCode = "500",
                Duration = TimeSpan.FromSeconds(1),
                Properties =
                {
                    {"Prop1", "Val1"}, {"Prop5", "Val5"}, {"Prop10", "Val10"}, {"Prop8", "Val8"}, {"Prop3", "Val3"}, {"Prop7", "Val7"}, {"Prop6", "Val6"}, {"Prop9", "Val9"},
                    {"Prop4", "Val4"}, {"Prop11", "Val11"}, {"Prop2", "Val2"}
                },
                Context = { InstrumentationKey = instrumentationKey }
            };

            var dependency = new DependencyTelemetry()
            {
                Name = Guid.NewGuid().ToString(),
                Success = false,
                Duration = TimeSpan.FromSeconds(1),
                Properties =
                {
                    {"Prop1", "Val1"}, {"Prop5", "Val5"}, {"Prop10", "Val10"}, {"Prop8", "Val8"}, {"ErrorMessage", "EMValue"}, {"Prop3", "Val3"}, {"Prop7", "Val7"},
                    {"Prop6", "Val6"}, {"Prop9", "Val9"},
                    {"Prop4", "Val4"}, {"Prop11", "Val11"}, {"Prop2", "Val2"}
                },
                Context = { InstrumentationKey = instrumentationKey }
            };

            var exception = new ExceptionTelemetry(new ArgumentNullException())
            {
                Properties =
                {
                    {"Prop1", "Val1"}, {"Prop5", "Val5"}, {"Prop10", "Val10"}, {"Prop8", "Val8"}, {"Prop3", "Val3"}, {"Prop7", "Val7"}, {"Prop6", "Val6"}, {"Prop9", "Val9"},
                    {"Prop4", "Val4"}, {"Prop11", "Val11"}, {"Prop2", "Val2"}
                },
                Context = { InstrumentationKey = instrumentationKey }
            };

            var @event = new EventTelemetry()
            {
                Name = "Event1",
                Properties =
                {
                    {"Prop1", "Val1"}, {"Prop5", "Val5"}, {"Prop10", "Val10"}, {"Prop8", "Val8"}, {"Prop3", "Val3"}, {"Prop7", "Val7"}, {"Prop6", "Val6"}, {"Prop9", "Val9"},
                    {"Prop4", "Val4"}, {"Prop11", "Val11"}, {"Prop2", "Val2"}
                },
                Context = { InstrumentationKey = instrumentationKey }
            };

            var trace = new TraceTelemetry()
            {
                Message = "Trace1",
                Properties =
                {
                    {"Prop1", "Val1"}, {"Prop5", "Val5"}, {"Prop10", "Val10"}, {"Prop8", "Val8"}, {"Prop3", "Val3"}, {"Prop7", "Val7"}, {"Prop6", "Val6"}, {"Prop9", "Val9"},
                    {"Prop4", "Val4"}, {"Prop11", "Val11"}, {"Prop2", "Val2"}
                },
                Context = { InstrumentationKey = instrumentationKey }
            };

            telemetryProcessor.Process(request);
            telemetryProcessor.Process(dependency);
            telemetryProcessor.Process(exception);
            telemetryProcessor.Process(@event);
            telemetryProcessor.Process(trace);

            // ASSERT
            var collectedTelemetry = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.ToArray().Reverse().ToArray();

            var expectedProperties =
                new Dictionary<string, string>()
                {
                    {"Prop1", "Val1"}, {"Prop10", "Val10"}, {"Prop11", "Val11"}, {"Prop2", "Val2"}, {"Prop3", "Val3"}, {"Prop4", "Val4"}, {"Prop5", "Val5"}, {"Prop6", "Val6"},
                    {"Prop7", "Val7"}, {"Prop8", "Val8"}
                }.ToArray();

            Assert.IsFalse(accumulatorManager.CurrentDataAccumulator.GlobalDocumentQuotaReached);

            Assert.AreEqual(5, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);

            Assert.AreEqual(TelemetryDocumentType.Request, Enum.Parse(typeof(TelemetryDocumentType), collectedTelemetry[0].DocumentType));
            Assert.AreEqual(request.Name, ((RequestTelemetryDocument)collectedTelemetry[0]).Name);
            Assert.AreEqual(10, collectedTelemetry[0].Properties.Length);
            CollectionAssert.AreEqual(expectedProperties, collectedTelemetry[0].Properties);
            Assert.AreEqual("StreamRequestsAndDependenciesAndExceptions", collectedTelemetry[0].DocumentStreamIds.Single());
            Assert.IsTrue(collectedTelemetry[0].Properties.ToList().TrueForAll(pair => pair.Key.Contains("Prop") && pair.Value.Contains("Val")));

            Assert.AreEqual(TelemetryDocumentType.RemoteDependency, Enum.Parse(typeof(TelemetryDocumentType), collectedTelemetry[1].DocumentType));
            Assert.AreEqual(dependency.Name, ((DependencyTelemetryDocument)collectedTelemetry[1]).Name);
            Assert.AreEqual(10 + 1, collectedTelemetry[1].Properties.Length);
            CollectionAssert.AreEqual(expectedProperties.Concat(new[] {new KeyValuePair<string, string>("ErrorMessage", "EMValue")}).ToArray(), collectedTelemetry[1].Properties);
            Assert.AreEqual("StreamRequestsAndDependenciesAndExceptions", collectedTelemetry[1].DocumentStreamIds.Single());
            Assert.IsTrue(
                collectedTelemetry[1].Properties.ToList()
                    .TrueForAll(
                        pair => (pair.Key.Contains("Prop") && pair.Value.Contains("Val")) || (pair.Key == "ErrorMessage" && pair.Value == "EMValue")));

            Assert.AreEqual(TelemetryDocumentType.Exception, Enum.Parse(typeof(TelemetryDocumentType), collectedTelemetry[2].DocumentType));
            Assert.AreEqual(exception.Exception.ToString(), ((ExceptionTelemetryDocument)collectedTelemetry[2]).Exception);
            Assert.AreEqual(10, collectedTelemetry[2].Properties.Length);
            CollectionAssert.AreEqual(expectedProperties, collectedTelemetry[2].Properties);
            Assert.AreEqual(2, collectedTelemetry[2].DocumentStreamIds.Length);
            Assert.AreEqual("StreamRequestsAndDependenciesAndExceptions", collectedTelemetry[2].DocumentStreamIds.First());
            Assert.AreEqual("StreamExceptionsEventsTraces", collectedTelemetry[2].DocumentStreamIds.Last());
            Assert.IsTrue(collectedTelemetry[2].Properties.ToList().TrueForAll(pair => (pair.Key.Contains("Prop") && pair.Value.Contains("Val"))));

            Assert.AreEqual(TelemetryDocumentType.Event, Enum.Parse(typeof(TelemetryDocumentType), collectedTelemetry[3].DocumentType));
            Assert.AreEqual(@event.Name, ((EventTelemetryDocument)collectedTelemetry[3]).Name);
            Assert.AreEqual(10, collectedTelemetry[3].Properties.Length);
            CollectionAssert.AreEqual(expectedProperties, collectedTelemetry[3].Properties);
            Assert.AreEqual("StreamExceptionsEventsTraces", collectedTelemetry[3].DocumentStreamIds.Single());
            Assert.IsTrue(collectedTelemetry[3].Properties.ToList().TrueForAll(pair => (pair.Key.Contains("Prop") && pair.Value.Contains("Val"))));

            Assert.AreEqual(TelemetryDocumentType.Trace, Enum.Parse(typeof(TelemetryDocumentType), collectedTelemetry[4].DocumentType));
            Assert.AreEqual(trace.Message, ((TraceTelemetryDocument)collectedTelemetry[4]).Message);
            Assert.AreEqual(10, collectedTelemetry[4].Properties.Length);
            CollectionAssert.AreEqual(expectedProperties, collectedTelemetry[4].Properties);
            Assert.AreEqual("StreamExceptionsEventsTraces", collectedTelemetry[4].DocumentStreamIds.Single());
            Assert.IsTrue(collectedTelemetry[4].Properties.ToList().TrueForAll(pair => (pair.Key.Contains("Prop") && pair.Value.Contains("Val"))));
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorHandlesRequestSuccessSpecialCaseCorrectly()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters =
                                new FilterConjunctionGroupInfo
                                {
                                    Filters =
                                        new[]
                                        {
                                            new FilterInfo { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "True" }
                                        }
                                }
                        }
                    }
            };

            var filterInfo = new FilterInfo() { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "True" };
            var metricInfo = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "Metric1",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Count()",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfo } } }
                }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                Metrics = metricInfo,
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var request = new RequestTelemetry()
            {
                Name = Guid.NewGuid().ToString(),
                Success = false,
                ResponseCode = string.Empty,
                Context = { InstrumentationKey = instrumentationKey }
            };
          
            telemetryProcessor.Process(request);

            // ASSERT
            // even though Success is set to false, since ResponseCode is empty the special case logic must have turned it into true
            var collectedTelemetry = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.ToArray().Reverse().ToArray().Single();
            double metricValue = accumulatorManager.CurrentDataAccumulator.CollectionConfigurationAccumulator.MetricAccumulators["Metric1"].CalculateAggregation(out long count);

            Assert.AreEqual(1, count);
            Assert.AreEqual(true, ((RequestTelemetryDocument)collectedTelemetry).Success);
            Assert.AreEqual(1, metricValue);

            // the value must have been restored
            Assert.AreEqual(false, request.Success);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorDoesNotCollectFullTelemetryItemsIfTypeIsNotMentionedInDocumentStream()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Dependency,
                            Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                        }
                    }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { DocumentStreams = new[] { documentStreamInfo }, ETag = "ETag1" };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var request = new RequestTelemetry()
            {
                Context = { InstrumentationKey = instrumentationKey }
            };

            var dependency = new DependencyTelemetry()
            {
                Context = { InstrumentationKey = instrumentationKey }
            };

            telemetryProcessor.Process(request);
            telemetryProcessor.Process(dependency);
            
            // ASSERT
            Assert.AreEqual(TelemetryDocumentType.RemoteDependency.ToString(), accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.ToArray().Reverse().ToArray().Single().DocumentType);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorDoesNotCollectFullRequestTelemetryItemsOnceQuotaIsExhaustedIndependentlyPerDocumentStream()
        {
            // ARRANGE
            var documentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "StreamAll",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Request,
                                Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "StreamSuccessOnly",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Request,
                                Filters =
                                    new FilterConjunctionGroupInfo
                                    {
                                        Filters = new[] { new FilterInfo() { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "true" } }
                                    }
                            }
                        }
                }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag1", DocumentStreams = documentStreamInfos };
            
            var timeProvider = new ClockMock();
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, timeProvider);
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            int counter = 0;
            for (int i = 0; i < 100; i++)
            {
                var request = new RequestTelemetry()
                {
                    Success = i == 0,
                    ResponseCode = "200",
                    Duration = TimeSpan.FromSeconds(counter++),
                    Context = { InstrumentationKey = instrumentationKey }
                };

                telemetryProcessor.Process(request);
            }

            timeProvider.FastForward(TimeSpan.FromSeconds(30));

            for (int i = 0; i < 100; i++)
            {
                var request = new RequestTelemetry()
                {
                    Success = i < 20,
                    ResponseCode = "200",
                    Duration = TimeSpan.FromSeconds(counter++),
                    Context = { InstrumentationKey = instrumentationKey }
                };

                telemetryProcessor.Process(request);
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            var collectedTelemetryStreamAll =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(document => document.DocumentStreamIds.Contains("StreamAll"))
                    .ToArray()
                    .Reverse()
                    .Cast<RequestTelemetryDocument>()
                    .ToArray();

            var collectedTelemetryStreamSuccessOnly =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(
                    document => document.DocumentStreamIds.Contains("StreamSuccessOnly"))
                    .ToArray()
                    .Reverse()
                    .Cast<RequestTelemetryDocument>()
                    .ToArray();

            // the quota is 3 initially, then 0.5 every second (but not more than 30)

            // StreamAll has collected the initial quota of the first 100, then the additional accrued quota from the second 100
            Assert.AreEqual(3 + 15, collectedTelemetryStreamAll.Length);

            // out of the first 100 items we expect to see the initial quota of 3
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(i, collectedTelemetryStreamAll[i].Duration.TotalSeconds);
            }

            // out of the second 100 items we expect to see items 100 through 114 (the new quota for 30 seconds is 15)
            for (int i = 0; i < 15; i++)
            {
                Assert.AreEqual(100 + i, collectedTelemetryStreamAll[3 + i].Duration.TotalSeconds);
            }

            // StreamSuccessOnly never hit the quota during the first 100. It got 1 and had 2 quota left at the end of it. 
            // Out of the second 100, it got 2 that were left over in the quota + the newly accrued quota of 15
            Assert.AreEqual(1 + 17, collectedTelemetryStreamSuccessOnly.Length);

            // just one item of the first 100
            Assert.AreEqual(0, collectedTelemetryStreamSuccessOnly[0].Duration.TotalSeconds);

            // 17 (15 accrued quota + 2 left over quota) from the second 100
            for (int i = 0; i < 17; i++)
            {
                Assert.AreEqual(100 + i, collectedTelemetryStreamSuccessOnly[1 + i].Duration.TotalSeconds);
            }
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorDoesNotCollectFullDependencyTelemetryItemsOnceQuotaIsExhaustedIndependentlyPerDocumentStream()
        {
            // ARRANGE
            var documentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "StreamAll",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Dependency,
                                Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "StreamSuccessOnly",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Dependency,
                                Filters =
                                    new FilterConjunctionGroupInfo
                                    {
                                        Filters = new[] { new FilterInfo() { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "true" } }
                                    }
                            }
                        }
                }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag1", DocumentStreams = documentStreamInfos };

            var timeProvider = new ClockMock();
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, timeProvider);
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            int counter = 0;
            for (int i = 0; i < 100; i++)
            {
                var request = new DependencyTelemetry()
                {
                    Success = i == 0,
                    Duration = TimeSpan.FromSeconds(counter++),
                    Context = { InstrumentationKey = instrumentationKey }
                };

                telemetryProcessor.Process(request);
            }

            timeProvider.FastForward(TimeSpan.FromSeconds(30));

            for (int i = 0; i < 100; i++)
            {
                var request = new DependencyTelemetry()
                {
                    Success = i < 20,
                    Duration = TimeSpan.FromSeconds(counter++),
                    Context = { InstrumentationKey = instrumentationKey }
                };

                telemetryProcessor.Process(request);
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            var collectedTelemetryStreamAll =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(document => document.DocumentStreamIds.Contains("StreamAll"))
                    .ToArray()
                    .Reverse()
                    .Cast<DependencyTelemetryDocument>()
                    .ToArray();

            var collectedTelemetryStreamSuccessOnly =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(
                    document => document.DocumentStreamIds.Contains("StreamSuccessOnly"))
                    .ToArray()
                    .Reverse()
                    .Cast<DependencyTelemetryDocument>()
                    .ToArray();

            // the quota is 3 initially, then 0.5 every second (but not more than 30)

            // StreamAll has collected the initial quota of the first 100, then the additional accrued quota from the second 100
            Assert.AreEqual(3 + 15, collectedTelemetryStreamAll.Length);

            // out of the first 100 items we expect to see the initial quota of 3
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(i, collectedTelemetryStreamAll[i].Duration.TotalSeconds);
            }

            // out of the second 100 items we expect to see items 100 through 114 (the new quota for 30 seconds is 15)
            for (int i = 0; i < 15; i++)
            {
                Assert.AreEqual(100 + i, collectedTelemetryStreamAll[3 + i].Duration.TotalSeconds);
            }

            // StreamSuccessOnly never hit the quota during the first 100. It got 1 and had 2 quota left at the end of it. 
            // Out of the second 100, it got 2 that were left over in the quota + the newly accrued quota of 15
            Assert.AreEqual(1 + 17, collectedTelemetryStreamSuccessOnly.Length);

            // just one item of the first 100
            Assert.AreEqual(0, collectedTelemetryStreamSuccessOnly[0].Duration.TotalSeconds);

            // 17 (15 accrued quota + 2 left over quota) from the second 100
            for (int i = 0; i < 17; i++)
            {
                Assert.AreEqual(100 + i, collectedTelemetryStreamSuccessOnly[1 + i].Duration.TotalSeconds);
            }
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorDoesNotCollectFullExceptionTelemetryItemsOnceQuotaIsExhaustedIndependentlyPerDocumentStream()
        {
            // ARRANGE
            var documentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "StreamAll",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Exception,
                                Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "StreamSuccessOnly",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Exception,
                                Filters =
                                    new FilterConjunctionGroupInfo
                                    {
                                        Filters =
                                            new[] { new FilterInfo() { FieldName = "Message", Predicate = Predicate.Equal, Comparand = "true" } }
                                    }
                            }
                        }
                }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag1", DocumentStreams = documentStreamInfos };

            var timeProvider = new ClockMock();
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, timeProvider);
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            int counter = 0;
            for (int i = 0; i < 100; i++)
            {
                var request = new ExceptionTelemetry()
                {
                    Exception = new Exception(i == 0 ? "true" : "false"),
                    Message = i == 0 ? "true" : "false",
                    Context = { InstrumentationKey = instrumentationKey, Operation = { Id = counter ++.ToString(CultureInfo.InvariantCulture) } }
                };

                telemetryProcessor.Process(request);
            }

            timeProvider.FastForward(TimeSpan.FromSeconds(30));

            for (int i = 0; i < 100; i++)
            {
                var request = new ExceptionTelemetry()
                {
                    Exception = new Exception(i < 20 ? "true" : "false"),
                    Message = i < 20 ? "true" : "false",
                    Context = { InstrumentationKey = instrumentationKey, Operation = { Id = counter ++.ToString(CultureInfo.InvariantCulture) } }
                };

                telemetryProcessor.Process(request);
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            var collectedTelemetryStreamAll =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(document => document.DocumentStreamIds.Contains("StreamAll"))
                    .ToArray()
                    .Reverse()
                    .Cast<ExceptionTelemetryDocument>()
                    .ToArray();

            var collectedTelemetryStreamSuccessOnly =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(
                    document => document.DocumentStreamIds.Contains("StreamSuccessOnly"))
                    .ToArray()
                    .Reverse()
                    .Cast<ExceptionTelemetryDocument>()
                    .ToArray();

            // the quota is 3 initially, then 0.5 every second (but not more than 30)

            // StreamAll has collected the initial quota of the first 100, then the additional accrued quota from the second 100
            Assert.AreEqual(3 + 15, collectedTelemetryStreamAll.Length);

            // out of the first 100 items we expect to see the initial quota of 3
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(i, int.Parse(collectedTelemetryStreamAll[i].OperationId, CultureInfo.InvariantCulture));
            }

            // out of the second 100 items we expect to see items 100 through 114 (the new quota for 30 seconds is 15)
            for (int i = 0; i < 15; i++)
            {
                Assert.AreEqual(100 + i, int.Parse(collectedTelemetryStreamAll[3 + i].OperationId, CultureInfo.InvariantCulture));
            }

            // StreamSuccessOnly never hit the quota during the first 100. It got 1 and had 2 quota left at the end of it. 
            // Out of the second 100, it got 2 that were left over in the quota + the newly accrued quota of 15
            Assert.AreEqual(1 + 17, collectedTelemetryStreamSuccessOnly.Length);

            // just one item of the first 100
            Assert.AreEqual(0, int.Parse(collectedTelemetryStreamSuccessOnly[0].OperationId, CultureInfo.InvariantCulture));

            // 17 (15 accrued quota + 2 left over quota) from the second 100
            for (int i = 0; i < 17; i++)
            {
                Assert.AreEqual(100 + i, int.Parse(collectedTelemetryStreamSuccessOnly[1 + i].OperationId, CultureInfo.InvariantCulture));
            }
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorDoesNotCollectFullEventTelemetryItemsOnceQuotaIsExhaustedIndependentlyPerDocumentStream()
        {
            // ARRANGE
            var documentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "StreamAll",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Event,
                                Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "StreamSuccessOnly",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Event,
                                Filters =
                                    new FilterConjunctionGroupInfo
                                    {
                                        Filters =
                                            new[] { new FilterInfo() { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "true" } }
                                    }
                            }
                        }
                }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag1", DocumentStreams = documentStreamInfos };

            var timeProvider = new ClockMock();
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, timeProvider);
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            int counter = 0;
            for (int i = 0; i < 100; i++)
            {
                var request = new EventTelemetry()
                {
                    Name = string.Format(CultureInfo.InvariantCulture, "{0}#{1}", i == 0 ? "true" : "false", counter++),
                    Context = { InstrumentationKey = instrumentationKey },
                };

                telemetryProcessor.Process(request);
            }

            timeProvider.FastForward(TimeSpan.FromSeconds(30));

            for (int i = 0; i < 100; i++)
            {
                var request = new EventTelemetry()
                {
                    Name = string.Format(CultureInfo.InvariantCulture, "{0}#{1}", i < 20 ? "true" : "false", counter++),
                    Context = { InstrumentationKey = instrumentationKey }
                };

                telemetryProcessor.Process(request);
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            var collectedTelemetryStreamAll =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(document => document.DocumentStreamIds.Contains("StreamAll"))
                    .ToArray()
                    .Reverse()
                    .Cast<EventTelemetryDocument>()
                    .ToArray();

            var collectedTelemetryStreamSuccessOnly =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(
                    document => document.DocumentStreamIds.Contains("StreamSuccessOnly")).ToArray().Reverse().Cast<EventTelemetryDocument>().ToArray();

            // the quota is 3 initially, then 0.5 every second (but not more than 30)

            // StreamAll has collected the initial quota of the first 100, then the additional accrued quota from the second 100
            Assert.AreEqual(3 + 15, collectedTelemetryStreamAll.Length);

            // out of the first 100 items we expect to see the initial quota of 3
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(i, int.Parse(collectedTelemetryStreamAll[i].Name.Split('#')[1], CultureInfo.InvariantCulture));
            }

            // out of the second 100 items we expect to see items 100 through 114 (the new quota for 30 seconds is 15)
            for (int i = 0; i < 15; i++)
            {
                Assert.AreEqual(100 + i, int.Parse(collectedTelemetryStreamAll[3 + i].Name.Split('#')[1], CultureInfo.InvariantCulture));
            }

            // StreamSuccessOnly never hit the quota during the first 100. It got 1 and had 2 quota left at the end of it. 
            // Out of the second 100, it got 2 that were left over in the quota + the newly accrued quota of 15
            Assert.AreEqual(1 + 17, collectedTelemetryStreamSuccessOnly.Length);

            // just one item of the first 100
            Assert.AreEqual(0, int.Parse(collectedTelemetryStreamSuccessOnly[0].Name.Split('#')[1], CultureInfo.InvariantCulture));

            // 17 (15 accrued quota + 2 left over quota) from the second 100
            for (int i = 0; i < 17; i++)
            {
                Assert.AreEqual(100 + i, int.Parse(collectedTelemetryStreamSuccessOnly[1 + i].Name.Split('#')[1], CultureInfo.InvariantCulture));
            }
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorDoesNotCollectFullTraceTelemetryItemsOnceQuotaIsExhaustedIndependentlyPerDocumentStream()
        {
            // ARRANGE
            var documentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "StreamAll",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Trace,
                                Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "StreamSuccessOnly",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Trace,
                                Filters =
                                    new FilterConjunctionGroupInfo
                                    {
                                        Filters = new[] { new FilterInfo() { FieldName = "Message", Predicate = Predicate.Contains, Comparand = "true" } }
                                    }
                            }
                        }
                }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag1", DocumentStreams = documentStreamInfos };

            var timeProvider = new ClockMock();
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, timeProvider);
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            int counter = 0;
            for (int i = 0; i < 100; i++)
            {
                var request = new TraceTelemetry()
                {
                    Message = string.Format(CultureInfo.InvariantCulture, "{0}#{1}", i == 0 ? "true" : "false", counter++),
                    Context = { InstrumentationKey = instrumentationKey },
                };

                telemetryProcessor.Process(request);
            }

            timeProvider.FastForward(TimeSpan.FromSeconds(30));

            for (int i = 0; i < 100; i++)
            {
                var request = new TraceTelemetry()
                {
                    Message = string.Format(CultureInfo.InvariantCulture, "{0}#{1}", i < 20 ? "true" : "false", counter++),
                    Context = { InstrumentationKey = instrumentationKey }
                };

                telemetryProcessor.Process(request);
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            var collectedTelemetryStreamAll =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(document => document.DocumentStreamIds.Contains("StreamAll"))
                    .ToArray()
                    .Reverse()
                    .Cast<TraceTelemetryDocument>()
                    .ToArray();

            var collectedTelemetryStreamSuccessOnly =
                accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(
                    document => document.DocumentStreamIds.Contains("StreamSuccessOnly"))
                    .ToArray()
                    .Reverse()
                    .Cast<TraceTelemetryDocument>()
                    .ToArray();

            // the quota is 3 initially, then 0.5 every second (but not more than 30)

            // StreamAll has collected the initial quota of the first 100, then the additional accrued quota from the second 100
            Assert.AreEqual(3 + 15, collectedTelemetryStreamAll.Length);

            // out of the first 100 items we expect to see the initial quota of 3
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(i, int.Parse(collectedTelemetryStreamAll[i].Message.Split('#')[1], CultureInfo.InvariantCulture));
            }

            // out of the second 100 items we expect to see items 100 through 114 (the new quota for 30 seconds is 15)
            for (int i = 0; i < 15; i++)
            {
                Assert.AreEqual(100 + i, int.Parse(collectedTelemetryStreamAll[3 + i].Message.Split('#')[1], CultureInfo.InvariantCulture));
            }

            // StreamSuccessOnly never hit the quota during the first 100. It got 1 and had 2 quota left at the end of it. 
            // Out of the second 100, it got 2 that were left over in the quota + the newly accrued quota of 15
            Assert.AreEqual(1 + 17, collectedTelemetryStreamSuccessOnly.Length);

            // just one item of the first 100
            Assert.AreEqual(0, int.Parse(collectedTelemetryStreamSuccessOnly[0].Message.Split('#')[1], CultureInfo.InvariantCulture));

            // 17 (15 accrued quota + 2 left over quota) from the second 100
            for (int i = 0; i < 17; i++)
            {
                Assert.AreEqual(100 + i, int.Parse(collectedTelemetryStreamSuccessOnly[1 + i].Message.Split('#')[1], CultureInfo.InvariantCulture));
            }
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorDoesNotCollectFullTelemetryItemsOnceGlobalQuotaIsExhausted()
        {
            // ARRANGE
            var documentStreamInfos = new List<DocumentStreamInfo>();

            // we have 15 streams (global quota is 10 * 30 documents per minute (5 documents per second), which is 10x the per-stream quota
            var streamCount = 15;
            for (int i = 0; i < streamCount; i++)
            {
                documentStreamInfos.Add(
                    new DocumentStreamInfo()
                    {
                        Id = string.Format(CultureInfo.InvariantCulture, "Stream{0}#", i),
                        DocumentFilterGroups =
                            new[]
                            {
                                new DocumentFilterConjunctionGroupInfo()
                                {
                                    TelemetryType = TelemetryType.Request,
                                    Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                                }
                            }
                    });
            }

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag1", DocumentStreams = documentStreamInfos.ToArray() };

            var timeProvider = new ClockMock();
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, timeProvider);
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);

            float maxGlobalTelemetryQuota = 6;
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy(), timeProvider, maxGlobalTelemetryQuota, 0);
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            // accrue the full quota (6 per minute for the purpose of this test, which is 0.1 per second)
            timeProvider.FastForward(TimeSpan.FromHours(1));

            // push 10 items to each stream
            for (int i = 0; i < 10; i++)
            {
                telemetryProcessor.Process(new RequestTelemetry() { Name = i.ToString(CultureInfo.InvariantCulture), Context = { InstrumentationKey = "some ikey" } });
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            Assert.IsTrue(accumulatorManager.CurrentDataAccumulator.GlobalDocumentQuotaReached);

            // we expect to see the first 6 documents in each stream, which is the global quota
            Assert.AreEqual(maxGlobalTelemetryQuota, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);

            for (int i = 0; i < streamCount; i++)
            {
                var streamId = string.Format(CultureInfo.InvariantCulture, "Stream{0}#", i);
                var collectedTelemetryForStream =
                    accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Where(document => document.DocumentStreamIds.Contains(streamId))
                        .ToArray()
                        .Reverse()
                        .Cast<RequestTelemetryDocument>()
                        .ToArray();

                Assert.AreEqual(maxGlobalTelemetryQuota, collectedTelemetryForStream.Length);

                for (int j = 0; j < collectedTelemetryForStream.Length; j++)
                {
                    Assert.AreEqual(j, int.Parse(collectedTelemetryForStream[j].Name, CultureInfo.InvariantCulture));
                }
            }
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorDoesNotCollectFullTelemetryItemsWhenSwitchIsOff()
        {
            // ARRANGE
            var accumulatorManager = new QuickPulseDataAccumulatorManager(EmptyCollectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey },
                disableFullTelemetryItems: true);

            // ACT
            var request = new RequestTelemetry()
            {
                Success = false,
                ResponseCode = "500",
                Duration = TimeSpan.FromSeconds(1),
                Context = { InstrumentationKey = instrumentationKey }
            };

            var dependency = new DependencyTelemetry()
            {
                Success = false,
                Duration = TimeSpan.FromSeconds(1),
                Context = { InstrumentationKey = instrumentationKey }
            };

            var exception = new ExceptionTelemetry(new ArgumentException("bla")) { Context = { InstrumentationKey = instrumentationKey } };

            var @event = new EventTelemetry() { Context = { InstrumentationKey = instrumentationKey } };

            telemetryProcessor.Process(request);
            telemetryProcessor.Process(dependency);
            telemetryProcessor.Process(exception);
            telemetryProcessor.Process(@event);

            // ASSERT
            Assert.AreEqual(0, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorTruncatesLongFullRequestTelemetryItemName()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var requestShort = new RequestTelemetry(new string('r', MaxFieldLength), DateTimeOffset.Now, TimeSpan.FromSeconds(1), "500", false)
            {
                Context = { InstrumentationKey = instrumentationKey }
            };
            var requestLong = new RequestTelemetry(new string('r', MaxFieldLength + 1), DateTimeOffset.Now, TimeSpan.FromSeconds(1), "500", false)
            {
                Context = { InstrumentationKey = instrumentationKey }
            };

            // process in the opposite order to allow for an easier validation order
            telemetryProcessor.Process(requestLong);
            telemetryProcessor.Process(requestShort);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<RequestTelemetryDocument>().ToList();

            Assert.AreEqual(telemetryDocuments[0].Name, requestShort.Name);
            Assert.AreEqual(telemetryDocuments[1].Name, requestShort.Name);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorTruncatesLongFullRequestTelemetryItemProperties()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var requestShort = new RequestTelemetry("requestShort", DateTimeOffset.Now, TimeSpan.FromSeconds(1), "500", false)
            {
                Properties = { { new string('p', MaxFieldLength), new string('v', MaxFieldLength) } },
                Context = { InstrumentationKey = instrumentationKey }
            };

            var requestLong = new RequestTelemetry("requestLong", DateTimeOffset.Now, TimeSpan.FromSeconds(1), "500", false)
            {
                Properties = { { new string('p', MaxFieldLength + 1), new string('v', MaxFieldLength + 1) } },
                Context = { InstrumentationKey = instrumentationKey }
            };

            // process in the opposite order to allow for an easier validation order
            telemetryProcessor.Process(requestLong);
            telemetryProcessor.Process(requestShort);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<RequestTelemetryDocument>().ToList();

            var actual = telemetryDocuments[0].Properties.First();
            var expected = requestShort.Properties.First();
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);

            actual = telemetryDocuments[1].Properties.First();
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorTruncatesLongFullDependencyTelemetryItemCommandName()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Dependency,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var dependencyShort = new DependencyTelemetry(
                new string('c', MaxFieldLength),
                new string('c', MaxFieldLength),
                new string('c', MaxFieldLength),
                new string('c', MaxFieldLength),
                DateTimeOffset.Now,
                TimeSpan.FromSeconds(1),
                new string('c', MaxFieldLength),
                false) { Context = { InstrumentationKey = instrumentationKey } };

            var dependencyLong = new DependencyTelemetry(
                new string('c', MaxFieldLength + 1),
                new string('c', MaxFieldLength + 1),
                new string('c', MaxFieldLength + 1),
                new string('c', MaxFieldLength + 1),
                DateTimeOffset.Now,
                TimeSpan.FromSeconds(1),
                new string('c', MaxFieldLength + 1),
                false) { Context = { InstrumentationKey = instrumentationKey } };

            // process in the opposite order to allow for an easier validation order
            telemetryProcessor.Process(dependencyLong);
            telemetryProcessor.Process(dependencyShort);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<DependencyTelemetryDocument>().ToList();

            Assert.AreEqual(telemetryDocuments[0].CommandName, dependencyShort.Data);
            Assert.AreEqual(telemetryDocuments[1].CommandName, dependencyLong.Data.Substring(0, MaxFieldLength));
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorTruncatesLongFullDependencyTelemetryItemName()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Dependency,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var dependencyShort = new DependencyTelemetry(
                new string('c', MaxFieldLength),
                new string('c', MaxFieldLength),
                new string('c', MaxFieldLength),
                new string('c', MaxFieldLength),
                DateTimeOffset.Now,
                TimeSpan.FromSeconds(1),
                new string('c', MaxFieldLength),
                false) { Context = { InstrumentationKey = instrumentationKey } };

            var dependencyLong = new DependencyTelemetry(
                new string('c', MaxFieldLength + 1),
                new string('c', MaxFieldLength + 1),
                new string('c', MaxFieldLength + 1),
                new string('c', MaxFieldLength + 1),
                DateTimeOffset.Now,
                TimeSpan.FromSeconds(1),
                new string('c', MaxFieldLength + 1),
                false) { Context = { InstrumentationKey = instrumentationKey } };

            // process in the opposite order to allow for an easier validation order
            telemetryProcessor.Process(dependencyLong);
            telemetryProcessor.Process(dependencyShort);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<DependencyTelemetryDocument>().ToList();

            Assert.AreEqual(telemetryDocuments[0].Name, dependencyShort.Name);
            Assert.AreEqual(telemetryDocuments[1].Name, dependencyLong.Name.Substring(0, MaxFieldLength));
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorTruncatesLongFullDependencyTelemetryItemProperties()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Dependency,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var dependencyShort = new DependencyTelemetry(
                "dependencyShort",
                "dependencyShort",
                "dependencyShort",
                "dependencyShort",
                DateTimeOffset.Now,
                TimeSpan.FromSeconds(1),
                "dependencyShort",
                false)
            {
                Properties = { { new string('p', MaxFieldLength), new string('v', MaxFieldLength) } },
                Context = { InstrumentationKey = instrumentationKey }
            };

            var dependencyLong = new DependencyTelemetry(
                "dependencyLong",
                "dependencyLong",
                "dependencyLong",
                "dependencyLong",
                DateTimeOffset.Now,
                TimeSpan.FromSeconds(1),
                "dependencyLong",
                false)
            {
                Properties = { { new string('p', MaxFieldLength + 1), new string('v', MaxFieldLength + 1) } },
                Context = { InstrumentationKey = instrumentationKey }
            };

            // process in the opposite order to allow for an easier validation order
            telemetryProcessor.Process(dependencyLong);
            telemetryProcessor.Process(dependencyShort);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<DependencyTelemetryDocument>().ToList();

            var expected = dependencyShort.Properties.First();
            var actual = telemetryDocuments[0].Properties.First();
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);

            actual = telemetryDocuments[1].Properties.First();
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorTruncatesLongFullExceptionTelemetryItemMessage()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var exceptionShort = new ExceptionTelemetry(new ArgumentException(new string('m', MaxFieldLength)))
            {
                Context = { InstrumentationKey = instrumentationKey }
            };

            var exceptionLong = new ExceptionTelemetry(new ArgumentException(new string('m', MaxFieldLength + 1)))
            {
                Context = { InstrumentationKey = instrumentationKey }
            };

            // process in the opposite order to allow for an easier validation order
            telemetryProcessor.Process(exceptionLong);
            telemetryProcessor.Process(exceptionShort);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<ExceptionTelemetryDocument>().ToList();

            Assert.AreEqual(telemetryDocuments[0].ExceptionMessage, exceptionShort.Exception.Message);
            Assert.AreEqual(telemetryDocuments[1].ExceptionMessage, exceptionLong.Exception.Message.Substring(0, MaxFieldLength));
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorTruncatesLongFullExceptionTelemetryItemProperties()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var exceptionShort = new ExceptionTelemetry(new ArgumentException())
            {
                Properties = { { new string('p', MaxFieldLength), new string('v', MaxFieldLength) } },
                Message = new string('m', MaxFieldLength),
                Context = { InstrumentationKey = instrumentationKey }
            };

            var exceptionLong = new ExceptionTelemetry(new ArgumentException())
            {
                Properties = { { new string('p', MaxFieldLength + 1), new string('v', MaxFieldLength + 1) } },
                Message = new string('m', MaxFieldLength),
                Context = { InstrumentationKey = instrumentationKey }
            };

            // process in the opposite order to allow for an easier validation order
            telemetryProcessor.Process(exceptionLong);
            telemetryProcessor.Process(exceptionShort);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<ExceptionTelemetryDocument>().ToList();

            var expected = exceptionShort.Properties.First();
            var actual = telemetryDocuments[0].Properties.First();
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);

            actual = telemetryDocuments[1].Properties.First();
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorHandlesDuplicatePropertyNamesDueToTruncation()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var exception = new ExceptionTelemetry(new ArgumentException())
            {
                Properties = { { new string('p', MaxFieldLength + 1), "Val1" }, { new string('p', MaxFieldLength + 2), "Val2" } },
                Message = "Message",
                Context = { InstrumentationKey = instrumentationKey }
            };

            telemetryProcessor.Process(exception);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<ExceptionTelemetryDocument>().ToList();

            Assert.AreEqual(1, telemetryDocuments[0].Properties.Length);
            Assert.AreEqual(new string('p', MaxFieldLength), telemetryDocuments[0].Properties.First().Key);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorExpandsAggregateExceptionMessage()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            var exception1 = new Exception("Exception 1");
            var exception2 = new Exception("Exception 2");
            var exception3 = new AggregateException("Exception 3", new Exception("Exception 4"), new Exception("Exception 5"));

            var aggregateException = new AggregateException("Top level message", exception1, exception2, exception3);

            // ACT
            var exceptionTelemetry = new ExceptionTelemetry(aggregateException) { Context = { InstrumentationKey = instrumentationKey } };

            telemetryProcessor.Process(exceptionTelemetry);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<ExceptionTelemetryDocument>().ToList();

            Assert.AreEqual("Exception 1 <--- Exception 2 <--- Exception 4 <--- Exception 5", telemetryDocuments[0].ExceptionMessage);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorExpandsAggregateExceptionMessageWhenEmpty()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            var exception = new AggregateException(string.Empty);

            // ACT
            var exceptionTelemetry = new ExceptionTelemetry(exception) { Context = { InstrumentationKey = instrumentationKey } };

            telemetryProcessor.Process(exceptionTelemetry);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<ExceptionTelemetryDocument>().ToList();

            Assert.AreEqual(string.Empty, telemetryDocuments[0].ExceptionMessage);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorExpandsExceptionMessageWhenSingleInnerException()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            var exception = new Exception("Exception 1", new Exception("Exception 2"));

            // ACT
            var exceptionTelemetry = new ExceptionTelemetry(exception) { Context = { InstrumentationKey = instrumentationKey } };

            telemetryProcessor.Process(exceptionTelemetry);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<ExceptionTelemetryDocument>().ToList();

            Assert.AreEqual("Exception 1 <--- Exception 2", telemetryDocuments[0].ExceptionMessage);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorExpandsExceptionMessageWhenNoInnerExceptions()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            var exception = new Exception("Exception 1");

            // ACT
            var exceptionTelemetry = new ExceptionTelemetry(exception) { Context = { InstrumentationKey = instrumentationKey } };

            telemetryProcessor.Process(exceptionTelemetry);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<ExceptionTelemetryDocument>().ToList();

            Assert.AreEqual("Exception 1", telemetryDocuments[0].ExceptionMessage);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorExpandsExceptionMessageWhenMultipleInnerExceptions()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            var exception = new Exception("Exception 1", new Exception("Exception 2", new Exception("Exception 3")));

            // ACT
            var exceptionTelemetry = new ExceptionTelemetry(exception) { Context = { InstrumentationKey = instrumentationKey } };

            telemetryProcessor.Process(exceptionTelemetry);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<ExceptionTelemetryDocument>().ToList();

            Assert.AreEqual("Exception 1 <--- Exception 2 <--- Exception 3", telemetryDocuments[0].ExceptionMessage);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorExpandsExceptionMessagesAndDedupesThem()
        {
            // ARRANGE
            var documentStreamInfo = new DocumentStreamInfo()
            {
                Id = "Stream1",
                DocumentFilterGroups =
                  new[]
                  {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new FilterInfo[0] }
                        },
                  }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { documentStreamInfo },
                ETag = "ETag1"
            };

            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            var exception = new AggregateException(
                "Exception 1",
                new Exception("Exception 1", new Exception("Exception 1")),
                new Exception("Exception 1"));

            // ACT
            var exceptionTelemetry = new ExceptionTelemetry(exception) { Context = { InstrumentationKey = instrumentationKey } };

            telemetryProcessor.Process(exceptionTelemetry);

            // ASSERT
            var telemetryDocuments = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Cast<ExceptionTelemetryDocument>().ToList();

            Assert.AreEqual("Exception 1", telemetryDocuments[0].ExceptionMessage);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorCalculatesCalculatedMetricsForRequests()
        {
            // ARRANGE
            var filterInfoResponseCodeGreaterThanOrEqualTo500 = new FilterInfo()
            {
                FieldName = "ResponseCode",
                Predicate = Predicate.GreaterThanOrEqual,
                Comparand = "500"
            };
            var filterInfoResponseCode200 = new FilterInfo() { FieldName = "ResponseCode", Predicate = Predicate.Equal, Comparand = "201" };
            var filterInfoSuccessful = new FilterInfo() { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "true" };
            var filterInfoFailed = new FilterInfo() { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "false" };

            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "AverageIdOfFailedRequestsGreaterThanOrEqualTo500",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Avg,
                    FilterGroups =
                        new[]
                        {
                            new FilterConjunctionGroupInfo()
                            {
                                Filters = new[] { filterInfoResponseCodeGreaterThanOrEqualTo500, filterInfoFailed }
                            }
                        }
                },
                new CalculatedMetricInfo()
                {
                    Id = "SumIdsOfSuccessfulRequestsEqualTo201",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Sum,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoResponseCode200, filterInfoSuccessful } } }
                }
            };

            var collectionConfiguration = new CollectionConfiguration(new CollectionConfigurationInfo() { Metrics = metrics }, out errors, new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var requests = new[]
            {
                new RequestTelemetry() { Id = "1", Success = true, ResponseCode = "500" },
                new RequestTelemetry() { Id = "2", Success = false, ResponseCode = "500" },
                new RequestTelemetry() { Id = "3", Success = true, ResponseCode = "501" },
                new RequestTelemetry() { Id = "4", Success = false, ResponseCode = "501" },
                new RequestTelemetry() { Id = "5", Success = true, ResponseCode = "499" },
                new RequestTelemetry() { Id = "6", Success = false, ResponseCode = "499" },
                new RequestTelemetry() { Id = "7", Success = true, ResponseCode = "201" },
                new RequestTelemetry() { Id = "8", Success = false, ResponseCode = "201" },
                new RequestTelemetry() { Id = "9", Success = true, ResponseCode = "blah" },
                new RequestTelemetry() { Id = "10", Success = false, ResponseCode = "blah" },
            };

            ArrayHelpers.ForEach(requests, r => r.Context.InstrumentationKey = instrumentationKey);

            ArrayHelpers.ForEach(requests, telemetryProcessor.Process);

            // ASSERT
            Dictionary<string, AccumulatedValues> calculatedMetrics =
                accumulatorManager.CurrentDataAccumulator.CollectionConfigurationAccumulator.MetricAccumulators;

            Assert.AreEqual(2, calculatedMetrics.Count);

            // 2, 4
            Assert.AreEqual(3d, calculatedMetrics["AverageIdOfFailedRequestsGreaterThanOrEqualTo500"].CalculateAggregation(out long count));
            Assert.AreEqual(2, count);

            // 7
            Assert.AreEqual(7, calculatedMetrics["SumIdsOfSuccessfulRequestsEqualTo201"].CalculateAggregation(out count));
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorCalculatesCalculatedMetricsForDependencies()
        {
            // ARRANGE
            var filterInfoDataGreaterThanOrEqualTo500 = new FilterInfo()
            {
                FieldName = "Data",
                Predicate = Predicate.GreaterThanOrEqual,
                Comparand = "500"
            };
            var filterInfoData200 = new FilterInfo() { FieldName = "Data", Predicate = Predicate.Equal, Comparand = "201" };
            var filterInfoSuccessful = new FilterInfo() { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "true" };
            var filterInfoFailed = new FilterInfo() { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "false" };

            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "AverageIdOfFailedDependenciesGreaterThanOrEqualTo500",
                    TelemetryType = TelemetryType.Dependency,
                    Projection = "Id",
                    Aggregation = AggregationType.Avg,
                    FilterGroups =
                        new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoDataGreaterThanOrEqualTo500, filterInfoFailed } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "SumIdsOfSuccessfulDependenciesEqualTo201",
                    TelemetryType = TelemetryType.Dependency,
                    Projection = "Id",
                    Aggregation = AggregationType.Sum,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoData200, filterInfoSuccessful } } }
                }
            };

            var collectionConfiguration = new CollectionConfiguration(new CollectionConfigurationInfo() { Metrics = metrics }, out errors, new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var dependencies = new[]
            {
                new DependencyTelemetry() { Id = "1", Success = true, Data = "500" },
                new DependencyTelemetry() { Id = "2", Success = false, Data = "500" },
                new DependencyTelemetry() { Id = "3", Success = true, Data = "501" },
                new DependencyTelemetry() { Id = "4", Success = false, Data = "501" },
                new DependencyTelemetry() { Id = "5", Success = true, Data = "499" },
                new DependencyTelemetry() { Id = "6", Success = false, Data = "499" },
                new DependencyTelemetry() { Id = "7", Success = true, Data = "201" },
                new DependencyTelemetry() { Id = "8", Success = false, Data = "201" },
                new DependencyTelemetry() { Id = "9", Success = true, Data = "blah" },
                new DependencyTelemetry() { Id = "10", Success = false, Data = "blah" },
            };

            ArrayHelpers.ForEach(dependencies, d => d.Context.InstrumentationKey = instrumentationKey);

            ArrayHelpers.ForEach(dependencies, telemetryProcessor.Process);

            // ASSERT
            Dictionary<string, AccumulatedValues> calculatedMetrics =
                accumulatorManager.CurrentDataAccumulator.CollectionConfigurationAccumulator.MetricAccumulators;

            Assert.AreEqual(2, calculatedMetrics.Count);

            // 2, 4
            Assert.AreEqual(3d, calculatedMetrics["AverageIdOfFailedDependenciesGreaterThanOrEqualTo500"].CalculateAggregation(out long count));
            Assert.AreEqual(2, count);

            // 7
            Assert.AreEqual(7, calculatedMetrics["SumIdsOfSuccessfulDependenciesEqualTo201"].CalculateAggregation(out count));
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorCalculatesCalculatedMetricsForExceptions()
        {
            // ARRANGE
            var filterInfoMessageGreaterThanOrEqualTo500 = new FilterInfo()
            {
                FieldName = "Message",
                Predicate = Predicate.GreaterThanOrEqual,
                Comparand = "500"
            };
            var filterInfoMessage200 = new FilterInfo() { FieldName = "Message", Predicate = Predicate.Equal, Comparand = "201" };
            var filterInfoSuccessful = new FilterInfo() { FieldName = "Sequence", Predicate = Predicate.Equal, Comparand = "true" };
            var filterInfoFailed = new FilterInfo() { FieldName = "Sequence", Predicate = Predicate.Equal, Comparand = "false" };

            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "AverageIdOfFailedMessageGreaterThanOrEqualTo500",
                    TelemetryType = TelemetryType.Exception,
                    Projection = "Message",
                    Aggregation = AggregationType.Avg,
                    FilterGroups =
                        new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoMessageGreaterThanOrEqualTo500, filterInfoFailed } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "SumIdsOfSuccessfulMessageEqualTo201",
                    TelemetryType = TelemetryType.Exception,
                    Projection = "Message",
                    Aggregation = AggregationType.Sum,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoMessage200, filterInfoSuccessful } } }
                }
            };

            var collectionConfiguration = new CollectionConfiguration(new CollectionConfigurationInfo() { Metrics = metrics }, out errors, new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var exceptions = new[]
            {
                new ExceptionTelemetry() { Sequence = "true", Message = "500" }, new ExceptionTelemetry() { Sequence = "false", Message = "500" },
                new ExceptionTelemetry() { Sequence = "true", Message = "501" }, new ExceptionTelemetry() { Sequence = "false", Message = "501" },
                new ExceptionTelemetry() { Sequence = "true", Message = "499" }, new ExceptionTelemetry() { Sequence = "false", Message = "499" },
                new ExceptionTelemetry() { Sequence = "true", Message = "201" }, new ExceptionTelemetry() { Sequence = "false", Message = "201" },
                new ExceptionTelemetry() { Sequence = "true", Message = "blah" }, new ExceptionTelemetry() { Sequence = "false", Message = "blah" },
            };

            ArrayHelpers.ForEach(exceptions, e => e.Context.InstrumentationKey = instrumentationKey);

            ArrayHelpers.ForEach(exceptions, telemetryProcessor.Process);

            // ASSERT
            Dictionary<string, AccumulatedValues> calculatedMetrics =
                accumulatorManager.CurrentDataAccumulator.CollectionConfigurationAccumulator.MetricAccumulators;

            Assert.AreEqual(2, calculatedMetrics.Count);

            // 500, 501
            Assert.AreEqual(500.5d, calculatedMetrics["AverageIdOfFailedMessageGreaterThanOrEqualTo500"].CalculateAggregation(out long count));
            Assert.AreEqual(2, count);

            // 201
            Assert.AreEqual(201d, calculatedMetrics["SumIdsOfSuccessfulMessageEqualTo201"].CalculateAggregation(out count));
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorCalculatesCalculatedMetricsForInfoBasedExceptions()
        {
            // ARRANGE
            var filterInfoMessageGreaterThanOrEqualTo500 = new FilterInfo()
            {
                FieldName = "Message",
                Predicate = Predicate.GreaterThanOrEqual,
                Comparand = "500"
            };
            var filterInfoMessage200 = new FilterInfo() { FieldName = "Message", Predicate = Predicate.Equal, Comparand = "201" };
            var filterInfoSuccessful = new FilterInfo() { FieldName = "Sequence", Predicate = Predicate.Equal, Comparand = "true" };
            var filterInfoFailed = new FilterInfo() { FieldName = "Sequence", Predicate = Predicate.Equal, Comparand = "false" };

            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "AverageIdOfFailedMessageGreaterThanOrEqualTo500",
                    TelemetryType = TelemetryType.Exception,
                    Projection = "Message",
                    Aggregation = AggregationType.Avg,
                    FilterGroups =
                        new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoMessageGreaterThanOrEqualTo500, filterInfoFailed } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "SumIdsOfSuccessfulMessageEqualTo201",
                    TelemetryType = TelemetryType.Exception,
                    Projection = "Message",
                    Aggregation = AggregationType.Sum,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoMessage200, filterInfoSuccessful } } }
                }
            };

            var collectionConfiguration = new CollectionConfiguration(new CollectionConfigurationInfo() { Metrics = metrics }, out errors, new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var emptyProperties = new Dictionary<string, string>();
            var emptyMeasurements = new Dictionary<string, double>();

            var exceptions = new[]
            {
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "500", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "true" },
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "500", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "false" },
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "501", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "true" },
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "501", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "false" },
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "499", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "true" },
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "499", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "false" },
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "201", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "true" },
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "201", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "false" },
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "blah", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "true" },
                new ExceptionTelemetry(new[] { new ExceptionDetailsInfo(1, -1, "SomeTypeException", "blah", true, "stack", new[] { new StackFrame("assm", "fileName", 1, 1, "method") }) }, SeverityLevel.Information, "problemId", emptyProperties, emptyMeasurements) { Sequence = "false" }
            };

            ArrayHelpers.ForEach(exceptions, e => e.Context.InstrumentationKey = instrumentationKey);

            ArrayHelpers.ForEach(exceptions, telemetryProcessor.Process);

            // ASSERT
            Dictionary<string, AccumulatedValues> calculatedMetrics =
                accumulatorManager.CurrentDataAccumulator.CollectionConfigurationAccumulator.MetricAccumulators;

            Assert.AreEqual(2, calculatedMetrics.Count);

            // 500, 501
            Assert.AreEqual(500.5d, calculatedMetrics["AverageIdOfFailedMessageGreaterThanOrEqualTo500"].CalculateAggregation(out long count));
            Assert.AreEqual(2, count);

            // 201
            Assert.AreEqual(201d, calculatedMetrics["SumIdsOfSuccessfulMessageEqualTo201"].CalculateAggregation(out count));
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorCalculatesCalculatedMetricsForEvents()
        {
            // ARRANGE
            var filterInfoNameGreaterThanOrEqualTo500 = new FilterInfo()
            {
                FieldName = "Name",
                Predicate = Predicate.GreaterThanOrEqual,
                Comparand = "500"
            };
            var filterInfoResponseCode200 = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "201" };
            var filterInfoSuccessful = new FilterInfo() { FieldName = "Sequence", Predicate = Predicate.Equal, Comparand = "true" };
            var filterInfoFailed = new FilterInfo() { FieldName = "Sequence", Predicate = Predicate.Equal, Comparand = "false" };

            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "AverageIdOfFailedEventsGreaterThanOrEqualTo500",
                    TelemetryType = TelemetryType.Event,
                    Projection = "Name",
                    Aggregation = AggregationType.Avg,
                    FilterGroups =
                        new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoNameGreaterThanOrEqualTo500, filterInfoFailed } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "SumIdsOfSuccessfulEventsEqualTo201",
                    TelemetryType = TelemetryType.Event,
                    Projection = "Name",
                    Aggregation = AggregationType.Sum,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoResponseCode200, filterInfoSuccessful } } }
                }
            };

            var collectionConfiguration = new CollectionConfiguration(new CollectionConfigurationInfo() { Metrics = metrics }, out errors, new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var events = new[]
            {
                new EventTelemetry() { Sequence = "true", Name = "500" }, new EventTelemetry() { Sequence = "false", Name = "500" },
                new EventTelemetry() { Sequence = "true", Name = "501" }, new EventTelemetry() { Sequence = "false", Name = "501" },
                new EventTelemetry() { Sequence = "true", Name = "499" }, new EventTelemetry() { Sequence = "false", Name = "499" },
                new EventTelemetry() { Sequence = "true", Name = "201" }, new EventTelemetry() { Sequence = "false", Name = "201" },
                new EventTelemetry() { Sequence = "true", Name = "blah" }, new EventTelemetry() { Sequence = "false", Name = "blah" },
            };

            ArrayHelpers.ForEach(events, e => e.Context.InstrumentationKey = instrumentationKey);

            ArrayHelpers.ForEach(events, telemetryProcessor.Process);

            // ASSERT
            Dictionary<string, AccumulatedValues> calculatedMetrics =
                accumulatorManager.CurrentDataAccumulator.CollectionConfigurationAccumulator.MetricAccumulators;

            Assert.AreEqual(2, calculatedMetrics.Count);

            // 500, 501
            Assert.AreEqual(500.5d, calculatedMetrics["AverageIdOfFailedEventsGreaterThanOrEqualTo500"].CalculateAggregation(out long count));
            Assert.AreEqual(2, count);

            // 201
            Assert.AreEqual(201, calculatedMetrics["SumIdsOfSuccessfulEventsEqualTo201"].CalculateAggregation(out count));
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorCalculatesCalculatedMetricsForTraces()
        {
            // ARRANGE
            var filterInfoNameGreaterThanOrEqualTo500 = new FilterInfo()
            {
                FieldName = "Message",
                Predicate = Predicate.GreaterThanOrEqual,
                Comparand = "500"
            };
            var filterInfoResponseCode200 = new FilterInfo() { FieldName = "Message", Predicate = Predicate.Equal, Comparand = "201" };
            var filterInfoSuccessful = new FilterInfo() { FieldName = "Sequence", Predicate = Predicate.Equal, Comparand = "true" };
            var filterInfoFailed = new FilterInfo() { FieldName = "Sequence", Predicate = Predicate.Equal, Comparand = "false" };

            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "AverageIdOfFailedTracesGreaterThanOrEqualTo500",
                    TelemetryType = TelemetryType.Trace,
                    Projection = "Message",
                    Aggregation = AggregationType.Avg,
                    FilterGroups =
                        new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoNameGreaterThanOrEqualTo500, filterInfoFailed } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "SumIdsOfSuccessfulTracesEqualTo201",
                    TelemetryType = TelemetryType.Trace,
                    Projection = "Message",
                    Aggregation = AggregationType.Sum,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoResponseCode200, filterInfoSuccessful } } }
                }
            };

            var collectionConfiguration = new CollectionConfiguration(new CollectionConfigurationInfo() { Metrics = metrics }, out errors, new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var traces = new[]
            {
                new TraceTelemetry() { Sequence = "true", Message = "500" }, new TraceTelemetry() { Sequence = "false", Message = "500" },
                new TraceTelemetry() { Sequence = "true", Message = "501" }, new TraceTelemetry() { Sequence = "false", Message = "501" },
                new TraceTelemetry() { Sequence = "true", Message = "499" }, new TraceTelemetry() { Sequence = "false", Message = "499" },
                new TraceTelemetry() { Sequence = "true", Message = "201" }, new TraceTelemetry() { Sequence = "false", Message = "201" },
                new TraceTelemetry() { Sequence = "true", Message = "blah" }, new TraceTelemetry() { Sequence = "false", Message = "blah" },
            };

            ArrayHelpers.ForEach(traces, t => t.Context.InstrumentationKey = instrumentationKey);

            ArrayHelpers.ForEach(traces, telemetryProcessor.Process);

            // ASSERT
            Dictionary<string, AccumulatedValues> calculatedMetrics =
                accumulatorManager.CurrentDataAccumulator.CollectionConfigurationAccumulator.MetricAccumulators;

            Assert.AreEqual(2, calculatedMetrics.Count);

            // 500, 501
            Assert.AreEqual(500.5d, calculatedMetrics["AverageIdOfFailedTracesGreaterThanOrEqualTo500"].CalculateAggregation(out long count));
            Assert.AreEqual(2, count);

            // 201
            Assert.AreEqual(201, calculatedMetrics["SumIdsOfSuccessfulTracesEqualTo201"].CalculateAggregation(out count));
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void QuickPulseTelemetryProcessorCalculatedMetricsIgnoresTelemetryWhereProjectionIsNotDouble()
        {
            // ARRANGE
            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "Metric1",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new FilterConjunctionGroupInfo[0]
                }
            };

            var collectionConfiguration = new CollectionConfiguration(new CollectionConfigurationInfo() { Metrics = metrics }, out errors, new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());
            var instrumentationKey = "some ikey";
            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = instrumentationKey });

            // ACT
            var requests = new[]
            {
                new RequestTelemetry() { Id = "1", Success = true, ResponseCode = "500" },
                new RequestTelemetry() { Id = "Not even a number...", Success = false, ResponseCode = "500" }
            };

            ArrayHelpers.ForEach(requests, r => r.Context.InstrumentationKey = instrumentationKey);

            ArrayHelpers.ForEach(requests, telemetryProcessor.Process);

            // ASSERT
            Dictionary<string, AccumulatedValues> calculatedMetrics =
                accumulatorManager.CurrentDataAccumulator.CollectionConfigurationAccumulator.MetricAccumulators;

            Assert.AreEqual(1, calculatedMetrics.Count);
            Assert.AreEqual(1.0d, calculatedMetrics["Metric1"].CalculateAggregation(out long count));
            Assert.AreEqual(1, count);
        }
        
        [TestMethod]
        public void QuickPulseTelemetryProcessorHandlesCalculatedMetricsInThreadSafeManner()
        {
            // ARRANGE
            var filterInfoAll200 = new FilterInfo() { FieldName = "ResponseCode", Predicate = Predicate.Equal, Comparand = "200" };
            var filterInfoAll500 = new FilterInfo() { FieldName = "ResponseCode", Predicate = Predicate.Equal, Comparand = "500" };
            var filterInfoAllSuccessful = new FilterInfo() { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "true" };
            var filterInfoAllFailed = new FilterInfo() { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "false" };
            var filterInfoAllFast = new FilterInfo() { FieldName = "Duration", Predicate = Predicate.LessThan, Comparand = "5000" };
            var filterInfoAllSlow = new FilterInfo() { FieldName = "Duration", Predicate = Predicate.GreaterThanOrEqual, Comparand = "5000" };

            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "AllGoodMin",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Min,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoAll200, filterInfoAllSuccessful } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "AllGoodMax",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Max,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoAll200, filterInfoAllSuccessful } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "AllBadMin",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Min,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoAll500, filterInfoAllFailed } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "AllBadMax",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Max,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoAll500, filterInfoAllFailed } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "AllGoodFastMin",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Min,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoAll200, filterInfoAllSuccessful, filterInfoAllFast } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "AllGoodFastMax",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Max,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoAll200, filterInfoAllSuccessful, filterInfoAllFast } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "AllBadSlowMin",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Min,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoAll500, filterInfoAllFailed, filterInfoAllSlow } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "AllBadSlowMax",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Max,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfoAll500, filterInfoAllFailed, filterInfoAllSlow } } }
                }
            };

            var collectionConfiguration = new CollectionConfiguration(new CollectionConfigurationInfo() { Metrics = metrics }, out errors, new ClockMock());
            
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            var telemetryProcessor = new QuickPulseTelemetryProcessor(new SimpleTelemetryProcessorSpy());

            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                new TelemetryConfiguration() { InstrumentationKey = "some ikey" });

            int taskCount = 10000;
            int swapTaskCount = 100;
            var tasks = new List<Task>(taskCount);

            for (int i = 0; i < taskCount; i++)
            {
                var requestTelemetry = new RequestTelemetry()
                {
                    Id = i.ToString(CultureInfo.InvariantCulture),
                    ResponseCode = (i % 2 == 0) ? "200" : "500",
                    Success = i % 2 == 0,
                    Duration = TimeSpan.FromDays(i),
                    Context = { InstrumentationKey = "some ikey" }
                };

                var task = new Task(() => telemetryProcessor.Process(requestTelemetry));
                tasks.Add(task);
            }

            // shuffle in a bunch of accumulator swapping operations
            var accumulators = new List<QuickPulseDataAccumulator>();
            for (int i = 0; i < swapTaskCount; i++)
            {
                var swapTask = new Task(
                    () =>
                        {
                            lock (accumulators)
                            {
                                accumulators.Add(accumulatorManager.CompleteCurrentDataAccumulator(collectionConfiguration));
                            }
                        });

                tasks.Insert((int)((double)taskCount / swapTaskCount * i), swapTask);
            }

            // ACT
            tasks.ForEach(task => task.Start());

            var taskArray = tasks.ToArray();
            Task.WaitAll(taskArray);

            // swap the last accumulator
            accumulators.Add(accumulatorManager.CompleteCurrentDataAccumulator(null));

            // ASSERT
            Assert.IsTrue(taskArray.All(task => task.Status == TaskStatus.RanToCompletion));

            // validate that all accumulators add up to the correct totals
            long allGoodMinCount = 0;
            long allBadMinCount = 0;
            long allGoodFastMinCount = 0;
            long allBadSlowMinCount = 0;
            long allGoodMaxCount = 0;
            long allBadMaxCount = 0;
            long allGoodFastMaxCount = 0;
            long allBadSlowMaxCount = 0;

            double allGoodMinValue = long.MaxValue;
            double allBadMinValue = long.MaxValue;
            double allGoodFastMinValue = long.MaxValue;
            double allBadSlowMinValue = long.MaxValue;
            double allGoodMaxValue = long.MinValue;
            double allBadMaxValue = long.MinValue;
            double allGoodFastMaxValue = long.MinValue;
            double allBadSlowMaxValue = long.MinValue;

            foreach (var accumulator in accumulators)
            {
                Dictionary<string, AccumulatedValues> metricsValues = accumulator.CollectionConfigurationAccumulator.MetricAccumulators;

                long count;
                double value = metricsValues["AllGoodMin"].CalculateAggregation(out count);
                allGoodMinValue = count != 0 ? Math.Min(allGoodMinValue, value) : allGoodMinValue;
                allGoodMinCount += count;
                value = metricsValues["AllGoodMax"].CalculateAggregation(out count);
                allGoodMaxValue = count != 0 ? Math.Max(allGoodMaxValue, value) : allGoodMaxValue;
                allGoodMaxCount += count;

                value = metricsValues["AllBadMin"].CalculateAggregation(out count);
                allBadMinValue = count != 0 ? Math.Min(allBadMinValue, value) : allBadMinValue;
                allBadMinCount += count;
                value = metricsValues["AllBadMax"].CalculateAggregation(out count);
                allBadMaxValue = count != 0 ? Math.Max(allBadMaxValue, value) : allBadMaxValue;
                allBadMaxCount += count;

                value = metricsValues["AllGoodFastMin"].CalculateAggregation(out count);
                allGoodFastMinValue = count != 0 ? Math.Min(allGoodFastMinValue, value) : allGoodFastMinValue;
                allGoodFastMinCount += count;
                value = metricsValues["AllGoodFastMax"].CalculateAggregation(out count);
                allGoodFastMaxValue = count != 0 ? Math.Max(allGoodFastMaxValue, value) : allGoodFastMaxValue;
                allGoodFastMaxCount += count;

                value = metricsValues["AllBadSlowMin"].CalculateAggregation(out count);
                allBadSlowMinValue = count != 0 ? Math.Min(allBadSlowMinValue, value) : allBadSlowMinValue;
                allBadSlowMinCount += count;
                value = metricsValues["AllBadSlowMax"].CalculateAggregation(out count);
                allBadSlowMaxValue = count != 0 ? Math.Max(allBadSlowMaxValue, value) : allBadSlowMaxValue;
                allBadSlowMaxCount += count;
            }

            allGoodMinValue = allGoodMinValue == long.MaxValue ? -1 : allGoodMinValue;
            allBadMinValue = allBadMinValue == long.MaxValue ? -1 : allBadMinValue;
            allGoodFastMinValue = allGoodFastMinValue == long.MaxValue ? -1 : allGoodFastMinValue;
            allBadSlowMinValue = allBadSlowMinValue == long.MaxValue ? -1 : allBadSlowMinValue;
            allBadMaxValue = allBadMaxValue == long.MinValue ? -1 : allBadMaxValue;
            allGoodFastMaxValue = allGoodFastMaxValue == long.MinValue ? -1 : allGoodFastMaxValue;
            allBadSlowMaxValue = allBadSlowMaxValue == long.MinValue ? -1 : allBadSlowMaxValue;
            
            // min and max metrics must have the same item count
            Assert.AreEqual(allGoodMinCount, allGoodMaxCount);
            Assert.AreEqual(allBadMinCount, allBadMaxCount);
            Assert.AreEqual(allGoodFastMinCount, allGoodFastMaxCount);
            Assert.AreEqual(allBadSlowMinCount, allBadSlowMaxCount);

            Assert.AreEqual(taskCount / 2, allGoodMinCount);
            Assert.AreEqual(0, allGoodMinValue);
            Assert.AreEqual(taskCount - 2, allGoodMaxValue);
            
            Assert.AreEqual(taskCount / 2, allBadMinCount);
            Assert.AreEqual(1, allBadMinValue);
            Assert.AreEqual(taskCount - 1, allBadMaxValue);
            
            Assert.AreEqual(taskCount / 4, allGoodFastMinCount);
            Assert.AreEqual(0, allGoodFastMinValue);
            Assert.AreEqual((taskCount / 2) - 2, allGoodFastMaxValue);

            Assert.AreEqual(taskCount / 4, allBadSlowMinCount);
            Assert.AreEqual((taskCount / 2) + 1, allBadSlowMinValue);
            Assert.AreEqual(taskCount - 1, allBadSlowMaxValue);
        }

#if NET452
        [TestMethod]
        public void VerifyInitializationWhenDeferredIsTrue()
        {
            var config = new TelemetryConfiguration();
            config.ExperimentalFeatures.Add(ExperimentalConstants.DeferRequestTrackingProperties);

            var spy = new SimpleTelemetryProcessorSpy();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(spy);
            telemetryProcessor.Initialize(config);

            Assert.IsTrue(telemetryProcessor.EvaluateDisabledTrackingProperties);
        }

        [TestMethod]
        public void VerifyInitializationWhenDeferredIsFalse()
        {
            var spy = new SimpleTelemetryProcessorSpy();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(spy);
            telemetryProcessor.Initialize(new TelemetryConfiguration());

            Assert.IsFalse(telemetryProcessor.EvaluateDisabledTrackingProperties);
        }

        [TestMethod]
        public void VerifyBehaviorWhenDeferredIsTrue()
        {
            // SETUP CONFIG
            var instrumentationKey = "some ikey";
            var config = new TelemetryConfiguration()
            {
                InstrumentationKey = instrumentationKey
            };
            config.ExperimentalFeatures.Add(ExperimentalConstants.DeferRequestTrackingProperties);

            // ARRANGE
            var accumulatorManager = GetAccumulationManager();
            var telemetryProcessor = GetQuickPulseTelemetryProcessor(accumulatorManager, config);

            // ASSERT QuickPulseTelemetryProcessor was Initialized
            Assert.IsTrue(telemetryProcessor.EvaluateDisabledTrackingProperties);

            // ACT
            var request = new RequestTelemetry()
            {
                Name = Guid.NewGuid().ToString(),
                Success = true,
                ResponseCode = "500",
                Context = { InstrumentationKey = instrumentationKey },
                Url = null, // THIS IS WHAT WE'RE TESTING
            };

            var httpContext = HttpContextHelper.SetFakeHttpContext(); // QuickPulseTelemetryProcessor should use the Url from the Current HttpContext.

            telemetryProcessor.Process(request);

            // ASSERT
            Assert.IsFalse(accumulatorManager.CurrentDataAccumulator.GlobalDocumentQuotaReached);
            Assert.AreEqual(1, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);
            var collectedTelemetry = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.ToArray().Reverse().ToArray();

            Assert.AreEqual(TelemetryDocumentType.Request, Enum.Parse(typeof(TelemetryDocumentType), collectedTelemetry[0].DocumentType));
            var requestTelemetryDocument = (RequestTelemetryDocument)collectedTelemetry[0];

            Assert.AreEqual(request.Name, requestTelemetryDocument.Name);

            // this is what we care about
            Assert.IsNotNull(requestTelemetryDocument.Url, "request url was not set");
            Assert.AreEqual(httpContext.Request.Url, requestTelemetryDocument.Url, "RequestTelemetryDocument should use the URL of the httpcontext");
        }

        [TestMethod]
        public void VerifyBehaviorWhenDeferredIsFalse()
        {
            // SETUP CONFIG
            var instrumentationKey = "some ikey";
            var config = new TelemetryConfiguration()
            {
                InstrumentationKey = instrumentationKey
            };

            // ARRANGE
            var accumulatorManager = GetAccumulationManager();
            var telemetryProcessor = GetQuickPulseTelemetryProcessor(accumulatorManager, config);

            // ASSERT QuickPulseTelemetryProcessor was Initialized
            Assert.IsFalse(telemetryProcessor.EvaluateDisabledTrackingProperties);

            // ACT
            var request = new RequestTelemetry()
            {
                Name = Guid.NewGuid().ToString(),
                Success = true,
                ResponseCode = "500",
                Context = { InstrumentationKey = instrumentationKey },
                Url = null, // THIS IS WHAT WE'RE TESTING
            };

            telemetryProcessor.Process(request);

            // ASSERT
            Assert.IsFalse(accumulatorManager.CurrentDataAccumulator.GlobalDocumentQuotaReached);
            Assert.AreEqual(1, accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.Count);
            var collectedTelemetry = accumulatorManager.CurrentDataAccumulator.TelemetryDocuments.ToArray().Reverse().ToArray();

            Assert.AreEqual(TelemetryDocumentType.Request, Enum.Parse(typeof(TelemetryDocumentType), collectedTelemetry[0].DocumentType));
            var requestTelemetryDocument = (RequestTelemetryDocument)collectedTelemetry[0];

            Assert.AreEqual(request.Name, requestTelemetryDocument.Name);

            // this is what we care about
            Assert.IsNull(requestTelemetryDocument.Url, "request url was not set");
        }
#endif

        private static QuickPulseDataAccumulatorManager GetAccumulationManager()
        {
            var requestsDocumentStreamInfo = new DocumentStreamInfo()
            {
                Id = "StreamRequests",
                DocumentFilterGroups =
                    new[]
                    {
                        // TODO: SHOULD GET THESE FILTERS FROM THE PARAMATER SO OTHER TESTS CAN SHARE THIS METHOD
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters =
                                new FilterConjunctionGroupInfo
                                {
                                    Filters = new[] { new FilterInfo { FieldName = "Success", Predicate = Predicate.Equal, Comparand = "0" } }
                                }
                        },
                    }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo()
            {
                DocumentStreams = new[] { requestsDocumentStreamInfo },
            };
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);

            return accumulatorManager;
        }

        private static QuickPulseTelemetryProcessor GetQuickPulseTelemetryProcessor(QuickPulseDataAccumulatorManager accumulatorManager, TelemetryConfiguration configuration)
        {
            var spy = new SimpleTelemetryProcessorSpy();
            var telemetryProcessor = new QuickPulseTelemetryProcessor(spy);
            telemetryProcessor.Initialize(configuration);

            ((IQuickPulseTelemetryProcessor)telemetryProcessor).StartCollection(
                accumulatorManager,
                new Uri("http://microsoft.com"),
                configuration);

            return telemetryProcessor;
        }
    }
}