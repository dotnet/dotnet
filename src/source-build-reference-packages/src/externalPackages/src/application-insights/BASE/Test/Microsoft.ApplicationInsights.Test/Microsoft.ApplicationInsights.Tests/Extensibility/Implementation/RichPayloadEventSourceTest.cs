namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
#if NETFRAMEWORK
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Reflection;
    using System.Collections.ObjectModel;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.Diagnostics.Tracing.Session;
    using System.Threading.Tasks;
    using Microsoft.ServiceProfiler.Agent.Utilities;

    /// <summary>
    /// Tests the rich payload event source tracking.
    /// </summary>
    [TestClass]
    [TestCategory("WindowsOnly")] // do not run these tests on linux builds
    public class RichPayloadEventSourceTest
    {
        public Dictionary<Type, string> ServiceProfilerNameContracts = new Dictionary<Type, string>
        {
            {typeof(AvailabilityTelemetry), "Availability" },
            {typeof(DependencyTelemetry), "RemoteDependency" },
            {typeof(EventTelemetry), "Event" },
            {typeof(ExceptionTelemetry), "Exception" },
            {typeof(MetricTelemetry), "Metric" },
            {typeof(PageViewPerformanceTelemetry), "PageViewPerformance" },
            {typeof(PageViewTelemetry), "PageView" },
#pragma warning disable 618
            {typeof(PerformanceCounterTelemetry), "Metric" },
            {typeof(SessionStateTelemetry), "Event" },
 #pragma warning restore 618
            {typeof(RequestTelemetry), "Request" },
            {typeof(TraceTelemetry), "Message" },
            {typeof(OperationTelemetry), "Operation" },
            {typeof(UnknownTelemetry), "Event" },
        };

        /// <summary>
        /// Tests tracking request telemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceRequestSentTest()
        {
            this.DoTracking(
                RichPayloadEventSource.Keywords.Requests,
                new RequestTelemetry("TestRequest", DateTimeOffset.Now, TimeSpan.FromMilliseconds(10), "200", true),
                typeof(External.RequestData),
                (client, item) => { client.TrackRequest((RequestTelemetry)item); });
        }

        /// <summary>
        /// RichPayloadEventSource does not copy GlobalProperties unless it is enabled.
        /// </summary>
        [TestMethod]
        [Ignore("Fails when run in parallel with other tests which enables RichPayloadEventSource for testing." +
                "Even though the listener is disposed by tests, the EventSource itself is not disposed, and IsEnabled is" +
                "only an approximation as per MSDN. Will disable this until we find a better way to test this. ")]
        public void A_RichPayloadEventSourceDoesNotCopyGlobalPropertiesInRequestUnlessEnabled()
        {
            ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(new RequestTelemetry());
        }

        /// <summary>
        /// Tests tracking trace telemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceTraceSentTest()
        {
            this.DoTracking(
                RichPayloadEventSource.Keywords.Traces,
                new TraceTelemetry("TestTrace", SeverityLevel.Information),
                typeof(External.MessageData),
                (client, item) => { client.TrackTrace((TraceTelemetry)item); });
        }


        /// <summary>
        /// RichPayloadEventSource does not copy GlobalProperties unless it is enabled.
        /// </summary>
        [TestMethod]
        [Ignore("Fails when run in parallel with other tests which enables RichPayloadEventSource for testing." +
                "Even though the listener is disposed by tests, the EventSource itself is not disposed, and IsEnabled is" +
                "only an approximation as per MSDN. Will disable this until we find a better way to test this. ")]
        public void A_RichPayloadEventSourceDoesNotCopyGlobalPropertiesInTraceUnlessEnabled()
        {
            ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(new TraceTelemetry());
        }

        /// <summary>
        /// Tests tracking event telemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceEventSentTest()
        {
            this.DoTracking(
                RichPayloadEventSource.Keywords.Events,
                new EventTelemetry("TestEvent"),
                typeof(External.EventData),
                (client, item) => { client.TrackEvent((EventTelemetry)item); });
        }

        /// <summary>
        /// Tests tracking unknown implementaiton of ITelemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceUnknownEventSentTest()
        {
            this.DoTracking(
                RichPayloadEventSource.Keywords.Events,
                new UnknownTelemetry() { Source = "source", Name = "name", ResponseCode = "200", Success = true }, // .NET 4.5 Event Source does not process empty values
                typeof(External.EventData),
                (client, item) => { client.Track((UnknownTelemetry)item); });
        }

        /// <summary>
        /// RichPayloadEventSource does not copy GlobalProperties unless it is enabled.
        /// </summary>
        [TestMethod]
        [Ignore("Fails when run in parallel with other tests which enables RichPayloadEventSource for testing." +
                "Even though the listener is disposed by tests, the EventSource itself is not disposed, and IsEnabled is" +
                "only an approximation as per MSDN. Will disable this until we find a better way to test this. ")]
        public void A_RichPayloadEventSourceDoesNotCopyGlobalPropertiesInEventUnlessEnabled()
        {
            ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(new EventTelemetry());
        }

        /// <summary>
        /// Tests tracking exception telemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceExceptionSentTest()
        {
            var exceptionTelemetry = new ExceptionTelemetry(new SystemException("Test"));
            exceptionTelemetry.Data.Data.exceptions[0].parsedStack = new External.StackFrame[] { new External.StackFrame() };

            this.DoTracking(
                RichPayloadEventSource.Keywords.Exceptions,
                exceptionTelemetry,
                typeof(External.ExceptionData),
                (client, item) => { client.TrackException((ExceptionTelemetry)item); });
        }

        /// <summary>
        /// RichPayloadEventSource does not copy GlobalProperties unless it is enabled.
        /// </summary>
        [TestMethod]
        [Ignore("Fails when run in parallel with other tests which enables RichPayloadEventSource for testing." +
                "Even though the listener is disposed by tests, the EventSource itself is not disposed, and IsEnabled is" +
                "only an approximation as per MSDN. Will disable this until we find a better way to test this. ")]
        public void A_RichPayloadEventSourceDoesNotCopyGlobalPropertiesInExceptionUnlessEnabled()
        {
            ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(new ExceptionTelemetry());
        }

        /// <summary>
        /// Tests tracking metric telemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceMetricSentTest()
        {
            this.DoTracking(
                RichPayloadEventSource.Keywords.Metrics,
                new MetricTelemetry("Test Metric Namespace", "Test Metric Name", 1, 42, 42, 42, 0),
                typeof(External.MetricData),
                (client, item) => { client.TrackMetric((MetricTelemetry)item); });
        }

        /// <summary>
        /// RichPayloadEventSource does not copy GlobalProperties unless it is enabled.
        /// </summary>
        [TestMethod]
        [Ignore("Fails when run in parallel with other tests which enables RichPayloadEventSource for testing." +
                "Even though the listener is disposed by tests, the EventSource itself is not disposed, and IsEnabled is" +
                "only an approximation as per MSDN. Will disable this until we find a better way to test this. ")]
        public void A_RichPayloadEventSourceDoesNotCopyGlobalPropertiesInMetricUnlessEnabled()
        {
            ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(new MetricTelemetry());
        }

        /// <summary>
        /// Tests tracking dependency telemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceDependencySentTest()
        {
            
            this.DoTracking(
                RichPayloadEventSource.Keywords.Dependencies,
                new DependencyTelemetry("Custom", "Target", "TestDependency", "TestCommand", DateTimeOffset.Now, TimeSpan.Zero, "200", true),
                typeof(External.RemoteDependencyData),
                (client, item) => { client.TrackDependency((DependencyTelemetry)item); });
        }

        /// <summary>
        /// RichPayloadEventSource does not copy GlobalProperties unless it is enabled.
        /// </summary>
        [TestMethod]
        [Ignore("Fails when run in parallel with other tests which enables RichPayloadEventSource for testing." +
                "Even though the listener is disposed by tests, the EventSource itself is not disposed, and IsEnabled is" +
                "only an approximation as per MSDN. Will disable this until we find a better way to test this. ")]
        public void A_RichPayloadEventSourceDoesNotCopyGlobalPropertiesInDependencyUnlessEnabled()
        {
            ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(new DependencyTelemetry());
        }

        /// <summary>
        /// Tests tracking page view telemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourcePageViewSentTest()
        {
            this.DoTracking(
                RichPayloadEventSource.Keywords.PageViews,
                new PageViewTelemetry("TestPage"),
                typeof(External.PageViewData),
                (client, item) => { client.TrackPageView((PageViewTelemetry)item); });
        }

        /// <summary>
        /// RichPayloadEventSource does not copy GlobalProperties unless it is enabled.
        /// </summary>
        [TestMethod]
        [Ignore("Fails when run in parallel with other tests which enables RichPayloadEventSource for testing." +
                "Even though the listener is disposed by tests, the EventSource itself is not disposed, and IsEnabled is" +
                "only an approximation as per MSDN. Will disable this until we find a better way to test this. ")]
        public void A_RichPayloadEventSourceDoesNotCopyGlobalPropertiesInPageviewUnlessEnabled()
        {
            ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(new PageViewTelemetry());
        }

        /// <summary>
        /// Tests tracking session state telemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceSessionStateSentTest()
        {
#pragma warning disable 618
            this.DoTracking(
                RichPayloadEventSource.Keywords.Events,
                new SessionStateTelemetry(new SessionState()),
                typeof(External.EventData),
                (client, item) => { client.Track((SessionStateTelemetry)item); });
#pragma warning restore 618
        }

        /// <summary>
        /// RichPayloadEventSource does not copy GlobalProperties unless it is enabled.
        /// </summary>
        [TestMethod]
        [Ignore("Fails when run in parallel with other tests which enables RichPayloadEventSource for testing." +
                "Even though the listener is disposed by tests, the EventSource itself is not disposed, and IsEnabled is" +
                "only an approximation as per MSDN. Will disable this until we find a better way to test this. ")]
        public void A_RichPayloadEventSourceDoesNotCopyGlobalPropertiesInSessionStateUnlessEnabled()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(new SessionStateTelemetry());
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Tests tracking session state telemetry.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceSessionPerformanceCounterTest()
        {
#pragma warning disable 618
            this.DoTracking(
                RichPayloadEventSource.Keywords.Metrics,
                new PerformanceCounterTelemetry("TestCategory", "TestCounter", "TestInstance", 1.0),
                typeof(External.MetricData),
                (client, item) => { client.Track((PerformanceCounterTelemetry)item); });
#pragma warning restore 618
        }

        /// <summary>
        /// RichPayloadEventSource does not copy GlobalProperties unless it is enabled.
        /// </summary>
        [TestMethod]
        [Ignore("Fails when run in parallel with other tests which enables RichPayloadEventSource for testing." +
                "Even though the listener is disposed by tests, the EventSource itself is not disposed, and IsEnabled is" +
                "only an approximation as per MSDN. Will disable this until we find a better way to test this. ")]
        public void A_RichPayloadEventSourceDoesNotCopyGlobalPropertiesInPerformanceCounterUnlessEnabled()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(new PerformanceCounterTelemetry());
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Tests start/stop events for Operations.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceOperationStartStopTest()
        {
            if (IsRunningOnEnvironmentSupportingRichPayloadEventSource())
            {
                var client = CreateTelemetryClient();

                using (var listener = new TestFramework.TestEventListener())
                {
                    listener.EnableEvents(RichPayloadEventSource.Log.EventSourceInternal, EventLevel.Informational, RichPayloadEventSource.Keywords.Operations);

                    // Simulate a Start/Stop request operation
                    var requestTelemetry = new RequestTelemetry { Name = "Request" };
                    using (client.StartOperation(requestTelemetry))
                    {
                    }

                    // Expect exactly two events (start and stop)
                    var actualEvents = listener.Messages.Where(m => m.Keywords.HasFlag(RichPayloadEventSource.Keywords.Operations)).Take(2).ToArray();

                    VerifyOperationEvent(requestTelemetry, ServiceProfilerNameContracts[requestTelemetry.GetType()], EventOpcode.Start, actualEvents[0]);
                    VerifyOperationEvent(requestTelemetry, ServiceProfilerNameContracts[requestTelemetry.GetType()], EventOpcode.Stop, actualEvents[1]);
                }
            }
            else
            {
                // 4.5 doesn't have RichPayload events
                Assert.IsNull(RichPayloadEventSource.Log.EventSourceInternal);
            }
        }

        /// <summary>
        /// Tests start/stop events for nested operations.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceNestedOperationStartStopTest()
        {
            if (IsRunningOnEnvironmentSupportingRichPayloadEventSource())
            {
                var client = CreateTelemetryClient();

                using (var listener = new TestFramework.TestEventListener())
                {
                    listener.EnableEvents(RichPayloadEventSource.Log.EventSourceInternal, EventLevel.Informational, RichPayloadEventSource.Keywords.Operations);

                    // Simulate a Start/Stop request operation
                    var requestTelemetry = new RequestTelemetry { Name = "Request" };
                    var nestedOperation = new DependencyTelemetry { Name = "Dependency" };
                    using (client.StartOperation(requestTelemetry))
                    {
                        using (client.StartOperation(nestedOperation))
                        {
                        }
                    }

                    // Expect exactly four events (start, start, stop, stop)
                    var actualEvents = listener.Messages.Where(m=>m.Keywords.HasFlag(RichPayloadEventSource.Keywords.Operations)).Take(4).ToArray();

                    VerifyOperationEvent(requestTelemetry, ServiceProfilerNameContracts[requestTelemetry.GetType()], EventOpcode.Start, actualEvents[0]);
                    VerifyOperationEvent(nestedOperation, ServiceProfilerNameContracts[typeof(OperationTelemetry)], EventOpcode.Start, actualEvents[1]);
                    VerifyOperationEvent(nestedOperation, ServiceProfilerNameContracts[typeof(OperationTelemetry)], EventOpcode.Stop, actualEvents[2]);
                    VerifyOperationEvent(requestTelemetry, ServiceProfilerNameContracts[requestTelemetry.GetType()], EventOpcode.Stop, actualEvents[3]);
                }
            }
            else
            {
                // 4.5 doesn't have RichPayload events
                Assert.IsNull(RichPayloadEventSource.Log.EventSourceInternal);
            }
        }

        /// <summary>
        /// Tests sanitizing telemetry event
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceSanitizeTest()
        {
            if (IsRunningOnEnvironmentSupportingRichPayloadEventSource())
            {
                var request = new RequestTelemetry()
                {
                    Name = new String('n', 5000),
                    Url = new Uri("https://www.bing.com/" + new String('u', 5000)),
                    ResponseCode = "200"
                };

                string propKeyNameToBeTrimmed = new String('a', Property.MaxDictionaryNameLength) + 1;
                string propValueToBeTrimmed = new String('b', Property.MaxValueLength) + 1;
                string globalPropKeyNameToBeTrimmed = new String('c', Property.MaxDictionaryNameLength) + 1;
                string globalPropValueToBeTrimmed = new String('d', Property.MaxValueLength) + 1;

                string propKeyNameAfterTrimmed = new String('a', Property.MaxDictionaryNameLength);
                string propValueAfterTrimmed = new String('b', Property.MaxValueLength);
                string globalPropKeyNameAfterTrimmed = new String('c', Property.MaxDictionaryNameLength);
                string globalPropValueAfterTrimmed = new String('d', Property.MaxValueLength);

                request.Properties.Add(propKeyNameToBeTrimmed, propValueToBeTrimmed);
                request.Properties.Add(globalPropKeyNameToBeTrimmed, globalPropValueToBeTrimmed);

                var client = CreateTelemetryClient();

                using (var listener = new Microsoft.ApplicationInsights.TestFramework.TestEventListener())
                {
                    listener.EnableEvents(RichPayloadEventSource.Log.EventSourceInternal, EventLevel.Verbose, RichPayloadEventSource.Keywords.Requests);

                    client.Track(request);

                    IDictionary<string, object> richPayload = (IDictionary<string, object>)listener.Messages.FirstOrDefault().Payload[2];

                    Assert.AreEqual(Property.MaxNameLength, richPayload["name"].ToString().Length);
                    Assert.AreEqual(Property.MaxUrlLength, richPayload["url"].ToString().Length);
                    Assert.AreEqual(true, richPayload["success"]);

                    // Validates sanitize is done on Properties and GlobalProperties.
                    var prop = ((object[])richPayload["properties"])[0];
                    var gblProp = ((object[])richPayload["properties"])[1];
                    ValidatePropertyDictionary((IDictionary<string, object>)prop, propKeyNameAfterTrimmed.Length, propValueAfterTrimmed.Length);
                    ValidatePropertyDictionary((IDictionary<string, object>)gblProp, propKeyNameAfterTrimmed.Length, propValueAfterTrimmed.Length);
                };
            }
        }

        /// <summary>
        /// This test verifies that the Application Insights Profiler agent can decode
        /// RequestTelemetry payloads when passed through the ETW pipeline.
        /// </summary>
        [TestMethod]
        public void RichPayloadEventSourceEtwPayloadSerializationTest()
        {
            if (IsRunningOnEnvironmentSupportingRichPayloadEventSource())
            {
                var request = new RequestTelemetry()
                {
                    Name = "TestRequest",
                    Url = new Uri("https://www.example.com/api/test&id=1234"),
                    ResponseCode = "200",
                    Success = true,
                    Duration = TimeSpan.FromTicks(314159)
                };

                request.Context.InstrumentationKey = Guid.NewGuid().ToString();
                request.Context.Operation.Name = "TestOperation";
                request.Context.Operation.Id = "ABC123";

                using (var eventSource = new RichPayloadEventSource($"Microsoft-ApplicationInsights-{nameof(RichPayloadEventSourceEtwPayloadSerializationTest)}"))
                using (var session = new TraceEventSession($"{nameof(RichPayloadEventSourceEtwPayloadSerializationTest)}"))
                {
                    session.EnableProvider(eventSource.EventSourceInternal.Guid);
                    session.Source.AllEvents += traceEvent =>
                    {
                        var payload = traceEvent.EventData();
                        var parsedPayload = PayloadParser.ParsePayload(payload);
                        Assert.AreEqual(request.Context.InstrumentationKey, parsedPayload.InstrumentationKey);
                        Assert.AreEqual(request.Context.Operation.Name, parsedPayload.OperationName);
                        Assert.AreEqual(request.Context.Operation.Id, parsedPayload.OperationId);
                        Assert.AreEqual(request.Data.ver, parsedPayload.Version);
                        Assert.AreEqual(request.Data.id, parsedPayload.RequestId);
                        Assert.AreEqual(request.Data.source, parsedPayload.Source);
                        Assert.AreEqual(request.Data.name, parsedPayload.Name);
                        Assert.AreEqual(request.Data.duration, parsedPayload.Duration);
                    };

                    Task.Run(() =>
                    {
                        eventSource.Process(request);
                        session.Stop();
                    });

                    session.Source.Process();
                }
            }
        }

        private void ValidatePropertyDictionary(IDictionary<string, object> props, int keyMax, int valuemax)
        {
            var dic = (IDictionary<string, object>)(props);
            var propKeyActual = (string)dic["Key"];
            var propValueActual = (string)dic["Value"];

            Assert.AreEqual(keyMax, propKeyActual.Length);
            Assert.AreEqual(valuemax, propValueActual.Length);
        }

        private TelemetryClient CreateTelemetryClient()
        {
            // The default InMemoryChannel creates a worker thread which, if left running, causes
            // System.AppDomainUnloadedException from the test runner.
            var channel = new TestFramework.StubTelemetryChannel();
            var configuration = new TelemetryConfiguration(Guid.NewGuid().ToString(), channel);
            var client = new TelemetryClient(configuration) { InstrumentationKey = configuration.InstrumentationKey };
            return client;
        }

        /// <summary>
        /// Helper method to setup shared context and call the desired tracking for testing.
        /// </summary>
        /// <param name="keywords">The event keywords to enable.</param>
        /// <param name="item">The telemetry item to track.</param>
        /// <param name="track">The tracking callback to execute.</param>
        private void DoTracking(EventKeywords keywords, ITelemetry item, Type dataType, Action<TelemetryClient, ITelemetry> track)
        {
            if (IsRunningOnEnvironmentSupportingRichPayloadEventSource())
            {
                var client = CreateTelemetryClient();

                using (var listener = new Microsoft.ApplicationInsights.TestFramework.TestEventListener())
                {
                    listener.EnableEvents(RichPayloadEventSource.Log.EventSourceInternal, EventLevel.Verbose, keywords);

#pragma warning disable CS0618 // Type or member is obsolete
                    item.Context.Properties.Add("property1", "value1");
#pragma warning restore CS0618 // Type or member is obsolete
                    (item as ISupportProperties)?.Properties.Add("itemprop1","itemvalue1");
                    item.Context.GlobalProperties.Add("globalproperty1", "globalvalue1");
                    item.Context.User.Id = "testUserId";
                    item.Context.Operation.Id = Guid.NewGuid().ToString();

                    item.Extension = new MyTestExtension { myIntField = 42, myStringField = "value" };

                    track(client, item);

                    var actualEvent = listener.Messages.FirstOrDefault();
#pragma warning disable CS0618 // Type or member is obsolete
                    if (!(item is UnknownTelemetry)) // Global properties are copied directly into output properties for unknown telemetry
                    {
                        Assert.IsTrue(item.Context.Properties.ContainsKey("globalproperty1"), "Item Properties should contain the globalproperties as its copied before serialization");
                    }
#pragma warning restore CS0618 // Type or member is obsolete

                    Assert.IsNotNull(actualEvent);
                    Assert.AreEqual(client.InstrumentationKey, actualEvent.Payload[0]);
#if !NET452
                    // adding logging to confirm what executable is being tested.
                    var sdkAssembly = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "Microsoft.ApplicationInsights");
                    var sdkVersion = sdkAssembly.GetName().Version.ToString();
                    Console.WriteLine($"SDK Assembly: {sdkAssembly.Location}");
                    Console.WriteLine($"SDK Version: {sdkVersion}");
                    Assert.AreEqual(ServiceProfilerNameContracts[item.GetType()], actualEvent.EventName, $"ItemType: '{item.GetType().Name}' ServiceProfilerName: '{ServiceProfilerNameContracts[item.GetType()]}' does not match EventName: '{actualEvent.EventName}'");
#endif

                    int keysFound = 0;
                    object[] tags = actualEvent.Payload[1] as object[];
                    foreach (object tagObject in tags)
                    {
                        Dictionary<string, object> tag = (Dictionary<string, object>)tagObject;
                        Assert.IsNotNull(tag);
                        string key = (string)tag["Key"];
                        object value = tag["Value"];
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            if (string.Equals(key, "ai.user.id", StringComparison.Ordinal))
                            {
                                Assert.AreEqual("testUserId", value);
                                ++keysFound;
                            }
                            else if (string.Equals(key, "ai.operation.id", StringComparison.Ordinal))
                            {
                                Assert.AreEqual(item.Context.Operation.Id, value);
                                ++keysFound;
                            }
                        }
                    }

                    Assert.AreEqual(2, keysFound);
                    Assert.IsNotNull(actualEvent.Payload[2]);

                    if(item is ISupportProperties)
                    {
                        object[] properties = (object[])((IDictionary<string, object>)actualEvent.Payload[2])["properties"];
#pragma warning disable CS0618 // Type or member is obsolete
                        if (item is PerformanceCounterTelemetry)
#pragma warning restore CS0618 // Type or member is obsolete
                        {
                            // There should be 6 entries in properties
                            // 1. from item's ISupportProperties.Properties
                            // 2. from item context.GlobalProperties
                            // 3. from item context.Properties        
                            // 4. from myInfField in item's Extension
                            // 5. from myStringField in item's Extension
                            // 6. PerfCounter name is a custom property.
                            Assert.AreEqual(6, properties.Length);                                                        
                        }
                        else if (item is UnknownTelemetry)
                        {
                            // There should be 11 entries in properties, all fields are flattened into properties
                            // 1. from item's ISupportProperties.Properties
                            // 2. from item context.GlobalProperties
                            // 3. from item context.Properties        
                            // 4. from myInfField in item's Extension
                            // 5. from myStringField in item's Extension
                            // 6. Unknown Telemetry name.
                            // 7. Unknown Telemetry id
                            // 8. Unknown Telemetry responseCode
                            // 9. Unknown Telemetry source
                            // 10. Unknown Telemetry duration
                            // 11. Unknown Telemetry success                            
                            Assert.AreEqual(11, properties.Length);
                        }
                        else
                        {
                            // There should be 5 entries in properties
                            // 1. from item's ISupportProperties.Properties
                            // 2. from item context.GlobalProperties
                            // 3. from item context.Properties        
                            // 4. from myInfField in item's Extension
                            // 5. from myStringField in item's Extension                            
                            Assert.AreEqual(5, properties.Length);
                        }
                    }

                    var expectedProperties = dataType.GetProperties().AsEnumerable();
                    var actualPropertiesPayload = (IDictionary<string, object>)actualEvent.Payload[2];
                    VerifyEventPayload(expectedProperties, actualPropertiesPayload);
                }
            }
            else
            {
                // 4.5 doesn't have RichPayload events
                Assert.IsNull(RichPayloadEventSource.Log.EventSourceInternal);
            }
        }

        private void ValidateGlobalTelemetryIsNotCopiedIfNotEnabled(ITelemetry item)
        {
            if (IsRunningOnEnvironmentSupportingRichPayloadEventSource())
            {
                var client = new TelemetryClient();

#pragma warning disable CS0618 // Type or member is obsolete
                item.Context.Properties.Add("property1", "value1");
#pragma warning restore CS0618 // Type or member is obsolete
                (item as ISupportProperties)?.Properties.Add("itemprop1", "itemvalue1");
                item.Context.GlobalProperties.Add("globalproperty1", "globalvalue1");
                
                client.Track(item);

#pragma warning disable CS0618 // Type or member is obsolete
                Assert.IsFalse(item.Context.Properties.ContainsKey("globalproperty1"),
                    "Item Properties should not contain the globalproperties as its copied before serialization which is done only if RichPayloadEventSource is enabled.");
                Assert.IsTrue(item.Context.Properties.ContainsKey("property1"),
                    "Item Properties should contain the values set.");
                if (item is ISupportProperties)
                {
                    Assert.IsTrue(item.Context.Properties.ContainsKey("itemprop1"),
                        "Item Properties should contain the values set via ISupportProperties.");
                }
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        private static void VerifyEventPayload(IEnumerable<PropertyInfo> expectedProperties, IDictionary<string, object> actualEventPayload)
        {
            var actualProperties = actualEventPayload.Keys.Select(k => new { Key = k, Value = actualEventPayload[k] });

            Assert.IsTrue(expectedProperties.Count() == actualProperties.Count());
            var expectedPropertiesEnumerator = expectedProperties.GetEnumerator();
            var actualPropertiesEnumerator = actualProperties.GetEnumerator();
            while (expectedPropertiesEnumerator.MoveNext() && actualPropertiesEnumerator.MoveNext())
            {
                var expectedProperty = expectedPropertiesEnumerator.Current;
                var actualProperty = actualPropertiesEnumerator.Current;
                Assert.AreEqual(expectedPropertiesEnumerator.Current.Name, actualPropertiesEnumerator.Current.Key);

                if (!expectedProperty.PropertyType.IsValueType
                    && !expectedProperty.PropertyType.IsPrimitive
                    && expectedProperty.PropertyType != typeof(string))
                {
                    if (expectedProperty.PropertyType.IsClass)
                    {
                        VerifyEventPayload(expectedProperty.PropertyType.GetProperties().AsEnumerable(), (IDictionary<string, object>)actualProperty.Value);
                    }
                    else
                    {
                        var enumerableType = GetEnumerableType(expectedProperty.PropertyType);
                        Assert.IsNotNull(enumerableType);
                        Assert.IsTrue(actualProperty.Value.GetType().IsArray);

                        if (enumerableType.IsClass)
                        {
                            VerifyEventPayload(enumerableType.GetProperties().AsEnumerable(), (IDictionary<string, object>)((object[])actualProperty.Value)[0]);
                        }
                    }
                }

            }
        }

        private static void VerifyOperationEvent(OperationTelemetry expectedOperation, string expectedName, EventOpcode expectedOpCode, EventWrittenEventArgs actualEvent)
        {
            Assert.AreEqual(expectedOpCode, actualEvent.Opcode);
#if !NET452
            Assert.AreEqual(expectedName, actualEvent.EventName);
#endif
            VerifyOperationPayload(expectedOperation, actualEvent.Payload);
        }

        private static void VerifyOperationPayload(OperationTelemetry expected, ReadOnlyCollection<object> actualPayload)
        {
            Assert.IsNotNull(actualPayload);
            Assert.AreEqual(4, actualPayload.Count);
            Assert.AreEqual(expected.Context.InstrumentationKey, actualPayload[0]);
            Assert.AreEqual(expected.Id, actualPayload[1]);
            Assert.AreEqual(expected.Name, actualPayload[2]);
            Assert.AreEqual(expected.Context.Operation.Id, actualPayload[3]);
        }

        private static Type GetEnumerableType(Type type)
        {
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return intType.GetGenericArguments()[0];
                }
            }
            return null;
        }

        private static bool IsRunningOnEnvironmentSupportingRichPayloadEventSource()
        {
            // Other versions depend on EventSource in .Net Framework 4.6+
            string productVersionString = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(object).Assembly.Location).ProductVersion;

            Version ver;
            if (!Version.TryParse(productVersionString, out ver))
            {
                Assert.Fail("Unable to determine .net framework version");
            }

            var ver46 = new Version(4, 6, 0, 0);
            return ver >= ver46;
        }
    }
#endif
    }