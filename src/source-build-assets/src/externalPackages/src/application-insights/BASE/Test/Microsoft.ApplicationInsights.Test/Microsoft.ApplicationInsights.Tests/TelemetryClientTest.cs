namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.W3C;
    using Microsoft.ApplicationInsights.Metrics;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;
    using Microsoft.ApplicationInsights.Metrics.TestUtility;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class TelemetryClientTest
    {
        [TestMethod]
        public void IsEnabledReturnsTrueIfTelemetryTrackingIsEnabledInConfiguration()
        {
            var configuration = new TelemetryConfiguration { DisableTelemetry = false };
            var client = new TelemetryClient(configuration);

            Assert.IsTrue(client.IsEnabled());
        }

        [TestMethod]
        public void FlushDoesNotThrowIfConfigurationIsDisposed()
        {
            var channel = new InMemoryChannel();
            var configuration = new TelemetryConfiguration { TelemetryChannel = channel };
            var client = new TelemetryClient(configuration);

            configuration.Dispose();

            try
            {
                client.Flush();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        #region TrackEvent

        [TestMethod]
        public void TrackEventSendsEventTelemetryWithSpecifiedNameToProvideSimplestWayOfSendingEventTelemetry()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackEvent("TestEvent");

            var eventTelemetry = (EventTelemetry)sentTelemetry.Single();
            Assert.AreEqual("TestEvent", eventTelemetry.Name);
        }

        [TestMethod]
        public void TrackEventSendsEventTelemetryWithSpecifiedObjectTelemetry()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackEvent(new EventTelemetry("TestEvent"));

            var eventTelemetry = (EventTelemetry)sentTelemetry.Single();
            Assert.AreEqual("TestEvent", eventTelemetry.Name);
        }

        [TestMethod]
        public void TrackEventWillSendPropertiesIfProvidedInline()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackEvent("Test", new Dictionary<string, string> { { "blah", "yoyo" } });

            var eventTelemetry = (EventTelemetry)sentTelemetry.Single();
            Assert.AreEqual("yoyo", eventTelemetry.Properties["blah"]);
        }

        #endregion

        #region Initialize

        [TestMethod]
        public void InitializeSetsDateTime()
        {
            EventTelemetry telemetry = new EventTelemetry("TestEvent");

            new TelemetryClient(TelemetryConfiguration.CreateDefault()).Initialize(telemetry);

            Assert.IsTrue(telemetry.Timestamp != default(DateTimeOffset));
        }

        /// <summary>
        /// Tests the scenario if Initialize assigns current precise time to start time.
        /// </summary>
        [TestMethod]
        public void TimestampIsPrecise()
        {
            double[] timeStampDiff = new double[1000];
            DateTimeOffset prevTimestamp = DateTimeOffset.MinValue;
            for (int i = 0; i < timeStampDiff.Length; i++)
            {
                var telemetry = new DependencyTelemetry();
                new TelemetryClient(TelemetryConfiguration.CreateDefault()).Initialize(telemetry);

                if (i > 0)
                {
                    timeStampDiff[i] = telemetry.Timestamp.Subtract(prevTimestamp).TotalMilliseconds;
                    Debug.WriteLine(timeStampDiff[i]);

                    // if timestamp is NOT precise, we'll get precisely 0 which should not ever happen
                    Assert.IsTrue(timeStampDiff[i] != 0);
                }

                prevTimestamp = telemetry.Timestamp;

                // waste a bit of time, assert result to prevent any optimizations
                Assert.IsTrue(ComputeSomethingHeavy() > 0);
            }
        }

        [TestMethod]
        public void InitializeSetsRoleInstance()
        {
            PlatformSingleton.Current = new StubPlatform { OnGetMachineName = () => "TestMachine" };

            EventTelemetry telemetry = new EventTelemetry("TestEvent");
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).Initialize(telemetry);

            Assert.AreEqual("TestMachine", telemetry.Context.Cloud.RoleInstance);
            Assert.IsNull(telemetry.Context.Internal.NodeName);

            PlatformSingleton.Current = null;
        }

        [TestMethod]
        public void InitializeDoesNotOverrideRoleInstance()
        {
            PlatformSingleton.Current = new StubPlatform { OnGetMachineName = () => "TestMachine" };

            EventTelemetry telemetry = new EventTelemetry("TestEvent");
            telemetry.Context.Cloud.RoleInstance = "MyMachineImplementation";

            new TelemetryClient(TelemetryConfiguration.CreateDefault()).Initialize(telemetry);

            Assert.AreEqual("MyMachineImplementation", telemetry.Context.Cloud.RoleInstance);
            Assert.AreEqual("TestMachine", telemetry.Context.Internal.NodeName);

            PlatformSingleton.Current = null;
        }

        [TestMethod]
        public void InitializeDoesNotOverrideNodeName()
        {
            PlatformSingleton.Current = new StubPlatform { OnGetMachineName = () => "TestMachine" };

            EventTelemetry telemetry = new EventTelemetry("TestEvent");
            telemetry.Context.Internal.NodeName = "MyMachineImplementation";

            new TelemetryClient(TelemetryConfiguration.CreateDefault()).Initialize(telemetry);

            Assert.AreEqual("TestMachine", telemetry.Context.Cloud.RoleInstance);
            Assert.AreEqual("MyMachineImplementation", telemetry.Context.Internal.NodeName);

            PlatformSingleton.Current = null;
        }

        #endregion

        #region InitializeIKey

        [TestMethod]
        public void InitializeIKeySetsIkeyFromContext()
        {
            EventTelemetry telemetry = new EventTelemetry("TestEvent");

            var tc = new TelemetryClient(TelemetryConfiguration.CreateDefault());
            // Set ikey on Context
            tc.InstrumentationKey = "mykey";
            tc.InitializeInstrumentationKey(telemetry);

            Assert.AreEqual("mykey",telemetry.Context.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeIKeySetsIkeyFromCofig()
        {
            EventTelemetry telemetry = new EventTelemetry("TestEvent");

            // Set ikey on config
            var config = new TelemetryConfiguration("mykey");
            var tc = new TelemetryClient(config);
            tc.InitializeInstrumentationKey(telemetry);

            Assert.AreEqual("mykey", telemetry.Context.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeIKeySetsIkeyFromContextOverConfig()
        {
            EventTelemetry telemetry = new EventTelemetry("TestEvent");

            // Set ikey on config
            var config = new TelemetryConfiguration("mykeyonconfig");
            var tc = new TelemetryClient(config);
            // Set ikey on Context as well.
            tc.InstrumentationKey = "mykeyoncontext";
            tc.InitializeInstrumentationKey(telemetry);

            // ikey on Context takes priority.
            Assert.AreEqual("mykeyoncontext", telemetry.Context.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeIKeyDoesNotOverrideIKey()
        {
            EventTelemetry telemetry = new EventTelemetry("TestEvent");
            telemetry.Context.InstrumentationKey = "expectedIKey";

            var tc = new TelemetryClient(TelemetryConfiguration.CreateDefault());
            tc.InstrumentationKey = "mykey";
            tc.InitializeInstrumentationKey(telemetry);

            Assert.AreEqual("expectedIKey", telemetry.Context.InstrumentationKey);
        }

        #endregion

        #region TrackMetric

        [TestMethod]
        public void TrackMetricSendsSpecifiedAggregatedMetricTelemetry()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

#pragma warning disable CS0618 // Type or member is obsolete
            client.TrackMetric(
                new MetricTelemetry()
                {
                    Name = "Test Metric",
                    Count = 5,
                    Sum = 40,
                    Min = 3.0,
                    Max = 4.0,
                    StandardDeviation = 1.0
                });
#pragma warning restore CS0618 // Type or member is obsolete

            var metric = (MetricTelemetry)sentTelemetry.Single();

            Assert.AreEqual("Test Metric", metric.Name);
            Assert.AreEqual(5, metric.Count);
            Assert.AreEqual(40, metric.Sum);
            Assert.AreEqual(3.0, metric.Min);
            Assert.AreEqual(4.0, metric.Max);
            Assert.AreEqual(1.0, metric.StandardDeviation);
        }

        [TestMethod]
        public void TrackMetricSendsMetricTelemetryWithSpecifiedNameAndValue()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

#pragma warning disable CS0618
            client.TrackMetric("TestMetric", 42);
#pragma warning restore CS0618

            var metric = (MetricTelemetry)sentTelemetry.Single();

            Assert.AreEqual("TestMetric", metric.Name);

#pragma warning disable CS0618
            Assert.AreEqual(42, metric.Value);
#pragma warning restore CS0618
        }

        [TestMethod]
        public void TrackMetricSendsSpecifiedMetricTelemetry()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

#pragma warning disable CS0618
            client.TrackMetric(new MetricTelemetry("TestMetric", 42));
#pragma warning restore CS0618

            var metric = (MetricTelemetry)sentTelemetry.Single();

            Assert.AreEqual("TestMetric", metric.Name);

#pragma warning disable CS0618
            Assert.AreEqual(42, metric.Value);
#pragma warning restore CS0618
        }

        [TestMethod]
        public void TrackMetricSendsMetricTelemetryWithGivenNameValueAndProperties()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

#pragma warning disable CS0618
            client.TrackMetric("TestMetric", 4.2, new Dictionary<string, string> { { "blah", "yoyo" } });
#pragma warning restore CS0618

            var metric = (MetricTelemetry)sentTelemetry.Single();

            Assert.AreEqual("TestMetric", metric.Name);

#pragma warning disable CS0618
            Assert.AreEqual(4.2, metric.Value);
#pragma warning restore CS0618

            Assert.AreEqual("yoyo", metric.Properties["blah"]);
        }

        [TestMethod]
        public void TrackMetricIgnoresNullPropertiesArgumentToAvoidCrashingUserApp()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

#pragma warning disable CS0618
            client.TrackMetric("TestMetric", 4.2, null);
#pragma warning restore CS0618

            var metric = (MetricTelemetry)sentTelemetry.Single();

            Assert.AreEqual("TestMetric", metric.Name);
#pragma warning disable CS0618
            Assert.AreEqual(4.2, metric.Value);
#pragma warning restore CS0618
            AssertEx.IsEmpty(metric.Properties);
        }

        #endregion

        #region TrackTrace

        [TestMethod]
        public void TrackTraceSendsTraceTelemetryWithSpecifiedNameToProvideSimplestWayOfSendingTraceTelemetry()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackTrace("TestTrace");

            var trace = (TraceTelemetry)sentTelemetry.Single();
            Assert.AreEqual("TestTrace", trace.Message);
        }

        [TestMethod]
        public void TrackTraceSendsTraceTelemetryWithSpecifiedObjectTelemetry()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackTrace(new TraceTelemetry { Message = "TestTrace" });

            var trace = (TraceTelemetry)sentTelemetry.Single();
            Assert.AreEqual("TestTrace", trace.Message);
        }

        [TestMethod]
        public void TrackTraceWillSendSeverityLevelIfProvidedInline()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackTrace("Test", SeverityLevel.Error);

            var trace = (TraceTelemetry)sentTelemetry.Single();
            Assert.AreEqual(SeverityLevel.Error, trace.SeverityLevel);
        }

        [TestMethod]
        public void TrackTraceWillNotSetSeverityLevelIfCustomerProvidedOnlyName()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackTrace("Test");

            var trace = (TraceTelemetry)sentTelemetry.Single();
            Assert.AreEqual(null, trace.SeverityLevel);
        }

        #endregion

        #region TrackException

        [TestMethod]
        public void TrackExceptionSendsExceptionTelemetryWithSpecifiedNameToProvideSimplestWayOfSendingExceptionTelemetry()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            Exception ex = new Exception();
            client.TrackException(ex);

            var exceptionTelemetry = (ExceptionTelemetry)sentTelemetry.Single();
            Assert.AreSame(ex, exceptionTelemetry.Exception);
        }

        [TestMethod]
        public void TrackExceptionWillUseRequiredFieldAsTextForTheExceptionNameWhenTheExceptionNameIsEmptyToHideUserErrors()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackException((Exception)null);

            var exceptionTelemetry = (ExceptionTelemetry)sentTelemetry.Single();
            Assert.IsNotNull(exceptionTelemetry.Exception);
            Assert.AreEqual("n/a", exceptionTelemetry.Exception.Message);
        }

        [TestMethod]
        public void TrackExceptionSendsExceptionTelemetryWithSpecifiedObjectTelemetry()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            Exception ex = new Exception();
            client.TrackException(new ExceptionTelemetry(ex));

            var exceptionTelemetry = (ExceptionTelemetry)sentTelemetry.Single();
            Assert.AreSame(ex, exceptionTelemetry.Exception);
        }

        [TestMethod]
        public void TrackExceptionWillUseABlankObjectAsTheExceptionToHideUserErrors()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackException((ExceptionTelemetry)null);

            var exceptionTelemetry = (ExceptionTelemetry)sentTelemetry.Single();
            Assert.IsNotNull(exceptionTelemetry.Exception);
        }

        [TestMethod]
        public void TrackExceptionWillNotSetSeverityLevelIfOnlyExceptionProvided()
        {
            var sentTelemetry = new List<ITelemetry>();
            var client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackException(new Exception());

            var exceptionTelemetry = (ExceptionTelemetry)sentTelemetry.Single();
            Assert.AreEqual(null, exceptionTelemetry.SeverityLevel);
        }

        #endregion

        #region TrackPageView

        [TestMethod]
        public void TrackPageViewSendsPageViewTelemetryWithGivenNameToTelemetryChannel()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            client.TrackPageView("TestName");

            var pageView = (PageViewTelemetry)sentTelemetry.Single();
            Assert.AreEqual("TestName", pageView.Name);
        }

        [TestMethod]
        public void TrackPageViewSendsGivenPageViewTelemetryToTelemetryChannel()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var pageViewTelemetry = new PageViewTelemetry("TestName");
            client.TrackPageView(pageViewTelemetry);

            var channelPageView = (PageViewTelemetry)sentTelemetry.Single();
            Assert.AreSame(pageViewTelemetry, channelPageView);
        }

        #endregion

        #region TrackRequest

        [TestMethod]
        public void TrackRequestSendsRequestTelemetryWithGivenNameTimestampDurationAndSuccessToTelemetryChannel()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var timestamp = DateTimeOffset.Now;
            client.TrackRequest("name", timestamp, TimeSpan.FromSeconds(42), "500", false);

            var request = (RequestTelemetry)sentTelemetry.Single();

            Assert.AreEqual("name", request.Name);
            Assert.AreEqual(timestamp, request.Timestamp);
            Assert.AreEqual("500", request.ResponseCode);
            Assert.AreEqual(TimeSpan.FromSeconds(42), request.Duration);
            Assert.AreEqual(false, request.Success);
        }

        [TestMethod]
        public void TrackRequestSendsGivenRequestTelemetryToTelemetryChannel()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var clientRequest = new RequestTelemetry();
            client.TrackRequest(clientRequest);

            var channelRequest = (RequestTelemetry)sentTelemetry.Single();
            Assert.AreSame(clientRequest, channelRequest);
        }

        #endregion

        #region TrackDependency

        [TestMethod]
        public void ObsoleteTrackDependencySendsDependencyTelemetryWithGivenNameCommandnameTimestampDurationAndSuccessToTelemetryChannel()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var timestamp = DateTimeOffset.Now;
#pragma warning disable CS0612 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
            client.TrackDependency("name", "command name", timestamp, TimeSpan.FromSeconds(42), false);
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CS0612 // Type or member is obsolete

            var dependency = (DependencyTelemetry)sentTelemetry.Single();

            Assert.AreEqual("name", dependency.Name);
            Assert.AreEqual("command name", dependency.Data);
            Assert.AreEqual(timestamp, dependency.Timestamp);
            Assert.AreEqual(TimeSpan.FromSeconds(42), dependency.Duration);
            Assert.AreEqual(false, dependency.Success);
        }

        [TestMethod]
        public void TrackDependencySendsDependencyTelemetryWithGivenNameCommandnameTimestampDurationAndSuccessToTelemetryChannel()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var timestamp = DateTimeOffset.Now;
            client.TrackDependency("type name", "name", "command name", timestamp, TimeSpan.FromSeconds(42), false);

            var dependency = (DependencyTelemetry)sentTelemetry.Single();

            Assert.AreEqual("type name", dependency.Type);
            Assert.AreEqual("name", dependency.Name);
            Assert.AreEqual("command name", dependency.Data);
            Assert.AreEqual(timestamp, dependency.Timestamp);
            Assert.AreEqual(TimeSpan.FromSeconds(42), dependency.Duration);
            Assert.AreEqual(false, dependency.Success);
        }

        [TestMethod]
        public void TrackDependencySendsGivenDependencyTelemetryToTelemetryChannel()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var clientDependency = new DependencyTelemetry();
            client.TrackDependency(clientDependency);

            var channelDependency = (DependencyTelemetry)sentTelemetry.Single();
            Assert.AreSame(clientDependency, channelDependency);
        }

        #endregion

        #region TrackAvailability

        [TestMethod]
        public void TrackAvailabilitySendsAvailabilityTelemetryWithGivenNameRunlocationRunIdDurationResultAndMessageToTelemetryChannel()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var timestamp = DateTimeOffset.Now;
            client.TrackAvailability("test name", timestamp, TimeSpan.FromSeconds(42), "test location", true);

            var availability = (AvailabilityTelemetry)sentTelemetry.Single();

            Assert.AreEqual("test name", availability.Name);
            Assert.AreEqual("test location", availability.RunLocation);
            Assert.AreEqual(timestamp, availability.Timestamp);
            Assert.AreEqual(TimeSpan.FromSeconds(42), availability.Duration);
            Assert.AreEqual(true, availability.Success);
        }

        [TestMethod]
        public void TrackAvailabilityTracksCustomDimensions()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var timestamp = DateTimeOffset.Now;
            var customDimensions = new Dictionary<string,string>()
                {
                    ["Blah"] = "yoyo"
                };
            
            client.TrackAvailability("test name", timestamp, TimeSpan.FromSeconds(42), "test location", true, properties: customDimensions);

            var availability = (AvailabilityTelemetry)sentTelemetry.Single();

            Assert.AreEqual("yoyo", availability.Properties["Blah"]);
            Assert.AreEqual(0, availability.Metrics.Count);
        }

        [TestMethod]
        public void TrackAvailabilityTracksCustomMetrics()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var timestamp = DateTimeOffset.Now;

            var customMetrics = new Dictionary<string, double>()
            {
                ["QueueLength"] = 10
            };

            client.TrackAvailability("test name", timestamp, TimeSpan.FromSeconds(42), "test location", true, metrics: customMetrics);

            var availability = (AvailabilityTelemetry)sentTelemetry.Single();

            Assert.AreEqual(0, availability.Properties.Count);
            Assert.AreEqual(10, availability.Metrics["QueueLength"]);
        }

        [TestMethod]
        public void TrackAvailabilitySendsGivenAvailabilityTelemetryToTelemetryChannel()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryClient client = this.InitializeTelemetryClient(sentTelemetry);

            var clientAvailability = new AvailabilityTelemetry();
            client.TrackAvailability(clientAvailability);

            var channelAvailability = (AvailabilityTelemetry)sentTelemetry.Single();
            Assert.AreSame(clientAvailability, channelAvailability);
        }

        #endregion

        #region Track

        [TestMethod]
        public void TrackMethodIsPublicToAllowDefiningTelemetryTypesOutsideOfCore()
        {
            Assert.IsTrue(typeof(TelemetryClient).GetTypeInfo().GetDeclaredMethod("Track").IsPublic);
        }

        [TestMethod]
        public void TrackMethodIsInvisibleThroughIntelliSenseSoThatCustomersDontGetConfused()
        {
            var attribute = typeof(TelemetryClient).GetTypeInfo().GetDeclaredMethod("Track").GetCustomAttributes(false).OfType<EditorBrowsableAttribute>().Single();
            Assert.AreEqual(EditorBrowsableState.Never, attribute.State);
        }

        [TestMethod]
        public void DefaultChannelInConfigurationIsCreatedByConstructorWhenNotSpecified()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration(Guid.NewGuid().ToString());
            Assert.IsNotNull(configuration.TelemetryChannel);
        }

        [TestMethod]
        public void TrackUsesInstrumentationKeyFromClientContextIfSetInCodeFirst()
        {
            ClearActiveTelemetryConfiguration();
            PlatformSingleton.Current = new StubPlatform();
            string message = "Test Message";

            ITelemetry sentTelemetry = null;
            var channel = new StubTelemetryChannel { OnSend = telemetry => sentTelemetry = telemetry };
            var configuration = new TelemetryConfiguration(string.Empty, channel);
            var client = new TelemetryClient(configuration);

            string environmentKey = Guid.NewGuid().ToString();
            string expectedKey = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", environmentKey); // Set via the environment variable.

            client.Context.InstrumentationKey = expectedKey;
            //Assert.DoesNotThrow
            client.TrackTrace(message);
            Assert.AreEqual(expectedKey, sentTelemetry.Context.InstrumentationKey);

            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);

            PlatformSingleton.Current = null;
        }

        [TestMethod]
        public void TrackUsesInstrumentationKeyFromConfigIfEnvironmentVariableIsEmpty()
        {
            PlatformSingleton.Current = new StubPlatform();

            ITelemetry sentTelemetry = null;
            var channel = new StubTelemetryChannel { OnSend = telemetry => sentTelemetry = telemetry };
            var configuration = new TelemetryConfiguration(string.Empty, channel);
            var client = new TelemetryClient(configuration);

            string expectedKey = Guid.NewGuid().ToString();
            configuration.InstrumentationKey = expectedKey; // Set in config
            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null); // Not set via env. variable
            
            //Assert.DoesNotThrow
            client.TrackTrace("Test Message");

            Assert.AreEqual(expectedKey, sentTelemetry.Context.InstrumentationKey);

            PlatformSingleton.Current = null;
        }

        [TestMethod]
        public void TrackDoesNotInitializeInstrumentationKeyFromConfigWhenItWasSetExplicitly()
        {
            var configuration = new TelemetryConfiguration(Guid.NewGuid().ToString(), new StubTelemetryChannel());
            var client = new TelemetryClient(configuration);

            var expectedKey = Guid.NewGuid().ToString();
            client.Context.InstrumentationKey = expectedKey;
            client.TrackTrace("Test Message");

            Assert.AreEqual(expectedKey, client.Context.InstrumentationKey);
        }

        [TestMethod]
        public void TrackDoesNotSendDataWhenTelemetryIsDisabled()
        {
            var sentTelemetry = new List<ITelemetry>();
            var channel = new StubTelemetryChannel { OnSend = t => sentTelemetry.Add(t) };
            var configuration = new TelemetryConfiguration(string.Empty, channel) { DisableTelemetry = true };

            var client = new TelemetryClient(configuration) {};

            client.Track(new StubTelemetry());

            Assert.AreEqual(0, sentTelemetry.Count);
        }

        [TestMethod]
        public void TrackRespectsInstrumentaitonKeyOfTelemetryItem()
        {
            var sentTelemetry = new List<ITelemetry>();
            var channel = new StubTelemetryChannel { OnSend = t => sentTelemetry.Add(t) };

            // No instrumentation key set here.
            var configuration = new TelemetryConfiguration(string.Empty, channel);

            var initializedTelemetry = new List<ITelemetry>();
            var telemetryInitializer = new StubTelemetryInitializer();
            telemetryInitializer.OnInitialize = item => initializedTelemetry.Add(item);
            configuration.TelemetryInitializers.Add(telemetryInitializer);

            var client = new TelemetryClient(configuration);

            var telemetry = new StubTelemetry();
            telemetry.Context.InstrumentationKey = "Foo";
            client.Track( telemetry );

            Assert.AreEqual(1, sentTelemetry.Count);
            Assert.AreEqual(1, initializedTelemetry.Count);
        }

        [TestMethod]
        public void TrackRespectsInstrumentaitonKeySetByTelemetryInitializer()
        {
            var sentTelemetry = new List<ITelemetry>();
            var channel = new StubTelemetryChannel { OnSend = t => sentTelemetry.Add(t) };

            // No instrumentation key set here.
            var configuration = new TelemetryConfiguration(string.Empty, channel);

            var initializedTelemetry = new List<ITelemetry>();
            var telemetryInitializer = new StubTelemetryInitializer();
            telemetryInitializer.OnInitialize = item =>
            {
                item.Context.InstrumentationKey = "Foo";
                initializedTelemetry.Add(item);
            };

            configuration.TelemetryInitializers.Add(telemetryInitializer);

            var client = new TelemetryClient(configuration);

            var telemetry = new StubTelemetry();
            client.Track(telemetry);

            Assert.AreEqual(1, sentTelemetry.Count);
            Assert.AreEqual(1, initializedTelemetry.Count);
        }

        [TestMethod]
        public void TrackDoesNotThrowExceptionsDuringTelemetryIntializersInitialize()
        {
            var configuration = new TelemetryConfiguration("Test key", new StubTelemetryChannel());
            var telemetryInitializer = new StubTelemetryInitializer();
            telemetryInitializer.OnInitialize = item => { throw new Exception(); };
            configuration.TelemetryInitializers.Add(telemetryInitializer);
            var client = new TelemetryClient(configuration);
            //Assert.DoesNotThrow
            client.Track(new StubTelemetry());
        }

        [TestMethod]
        public void TrackLogsDiagnosticsMessageOnExceptionsDuringTelemetryIntializersInitialize()
        {
            using (var listener = new TestEventListener())
            {
                listener.EnableEvents(CoreEventSource.Log, EventLevel.Error);

                var configuration = new TelemetryConfiguration("Test key", new StubTelemetryChannel());
                var telemetryInitializer = new StubTelemetryInitializer();
                var exceptionMessage = "Test exception message";
                telemetryInitializer.OnInitialize = item => { throw new Exception(exceptionMessage); };
                configuration.TelemetryInitializers.Add(telemetryInitializer);

                var client = new TelemetryClient(configuration);
                client.Track(new StubTelemetry());

                var exceptionExplanation = "Exception while initializing " + typeof(StubTelemetryInitializer).FullName;
                var diagnosticsMessage = (string)listener.Messages.First().Payload[0];
                AssertEx.Contains(exceptionExplanation, diagnosticsMessage, StringComparison.OrdinalIgnoreCase);
                AssertEx.Contains(exceptionMessage, diagnosticsMessage, StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void TrackDoesNotAddDeveloperModeCustomPropertyIfDeveloperModeIsSetToFalse()
        {
            ITelemetry sentTelemetry = null;
            var channel = new StubTelemetryChannel
            {
                OnSend = telemetry => sentTelemetry = telemetry,
                DeveloperMode = false
            };
            var configuration = new TelemetryConfiguration("Test key", channel);
            var client = new TelemetryClient(configuration);

            client.Track(new StubTelemetry());

            Assert.IsFalse(((ISupportProperties)sentTelemetry).Properties.ContainsKey("DeveloperMode"));
        }

        [TestMethod]
        public void TrackAddsDeveloperModeCustomPropertyWhenDeveloperModeIsTrue()
        {
            ITelemetry sentTelemetry = null;
            var channel = new StubTelemetryChannel
            {
                OnSend = telemetry => sentTelemetry = telemetry,
                DeveloperMode = true
            };
            var configuration = new TelemetryConfiguration("Test key", channel);
            var client = new TelemetryClient(configuration);

            client.Track(new StubTelemetry());

            Assert.AreEqual("true", ((ISupportProperties)sentTelemetry).Properties["DeveloperMode"]);
        }

        [TestMethod]
        public void TrackDoesNotTryAddingDeveloperModeCustomPropertyWhenTelemetryDoesNotSupportCustomProperties()
        {
            var channel = new StubTelemetryChannel { DeveloperMode = true };
            var configuration = new TelemetryConfiguration("Test Key", channel);
            var client = new TelemetryClient(configuration);

#pragma warning disable 618
            //Assert.DoesNotThrow
            client.Track(new SessionStateTelemetry());
#pragma warning disable 618
        }

        [TestMethod]
        public void TrackAddsTimestampWhenMissing()
        {
            ITelemetry sentTelemetry = null;
            var channel = new StubTelemetryChannel
            {
                OnSend = telemetry => sentTelemetry = telemetry
            };
            var configuration = new TelemetryConfiguration("Test key", channel);
            var client = new TelemetryClient(configuration);

            client.Track(new StubTelemetry());

            Assert.AreNotEqual(DateTimeOffset.MinValue, sentTelemetry.Timestamp);
        }

        [TestMethod]
        public void TrackWritesTelemetryToDebugOutputIfIKeyEmpty()
        {
            ClearActiveTelemetryConfiguration();
            string actualMessage = null;
            var debugOutput = new StubDebugOutput
            {
                OnWriteLine = message =>
                {
                    System.Diagnostics.Debug.WriteLine("1");
                    actualMessage = message;
                },
                OnIsAttached = () => true,
            };

            PlatformSingleton.Current = new StubPlatform { OnGetDebugOutput = () => debugOutput };
            var channel = new StubTelemetryChannel { DeveloperMode = true };
            var configuration = new TelemetryConfiguration(string.Empty, channel);
            var client = new TelemetryClient(configuration);

            client.Track(new StubTelemetry());
            
            Assert.IsTrue(actualMessage.StartsWith("Application Insights Telemetry (unconfigured): "));
            PlatformSingleton.Current = null;
        }

        [TestMethod]
        public void TrackWritesTelemetryToDebugOutputIfIKeyNotEmpty()
        {
            string actualMessage = null;
            var debugOutput = new StubDebugOutput
            {
                OnWriteLine = message => actualMessage = message,
                OnIsAttached = () => true,
            };

            PlatformSingleton.Current = new StubPlatform { OnGetDebugOutput = () => debugOutput };
            var channel = new StubTelemetryChannel { DeveloperMode = true };
            var configuration = new TelemetryConfiguration("123", channel);
            var client = new TelemetryClient(configuration);

            client.Track(new StubTelemetry());
            
            Assert.IsTrue(actualMessage.StartsWith("Application Insights Telemetry: "));
            PlatformSingleton.Current = null;
        }


        [TestMethod]
        public void TrackDoesNotWriteTelemetryToDebugOutputIfNotInDeveloperMode()
        {
            ClearActiveTelemetryConfiguration();
            string actualMessage = null;
            var debugOutput = new StubDebugOutput { OnWriteLine = message => actualMessage = message };
            PlatformSingleton.Current = new StubPlatform { OnGetDebugOutput = () => debugOutput };
            var channel = new StubTelemetryChannel();
            var configuration = new TelemetryConfiguration("Test key", channel);
            var client = new TelemetryClient(configuration);

            client.Track(new StubTelemetry());
            PlatformSingleton.Current = null;
            Assert.IsNull(actualMessage);
        }

        [TestMethod]
        public void TrackCopiesPropertiesFromClientToTelemetry()
        {
            var configuration = new TelemetryConfiguration(string.Empty, new StubTelemetryChannel());
            var client = new TelemetryClient(configuration);
            client.Context.Properties["TestProperty"] = "TestValue";
            client.Context.GlobalProperties["TestGlobalProperty"] = "TestGlobalValue";
            client.Context.InstrumentationKey = "Test Key";

            var telemetry = new StubTelemetry();
            client.Track(telemetry);

            AssertEx.AreEqual(client.Context.Properties.ToArray(), telemetry.Context.Properties.ToArray());
            AssertEx.AreEqual(client.Context.GlobalProperties.ToArray(), telemetry.Context.GlobalProperties.ToArray());
        }

        [TestMethod]
        public void TrackDoesNotOverwriteTelemetryPropertiesWithClientPropertiesBecauseExplicitlySetValuesTakePrecedence()
        {
            var configuration = new TelemetryConfiguration(string.Empty, new StubTelemetryChannel());
            var client = new TelemetryClient(configuration);
            client.Context.Properties["TestProperty"] = "ClientValue";
            client.Context.GlobalProperties["TestProperty"] = "ClientValue";
            client.Context.InstrumentationKey = "Test Key";

            var telemetry = new StubTelemetry { Properties = { { "TestProperty", "TelemetryValue" } } };
            client.Track(telemetry);

            Assert.AreEqual("TelemetryValue", telemetry.Properties["TestProperty"]);
        }

        [TestMethod]
        public void TrackCopiesPropertiesFromClientToTelemetryBeforeInvokingInitializersBecauseExplicitlySetValuesTakePrecedence()
        {
            const string PropertyName = "TestProperty";
            const string PropertyNameGlobal = "TestGlobalProperty";

            string valueInInitializer = null;
            string globalValueInInitializer = null;
            var initializer = new StubTelemetryInitializer();
            initializer.OnInitialize =
                (telemetry) =>
                {
                    valueInInitializer = ((ISupportProperties)telemetry).Properties[PropertyName];
                    globalValueInInitializer = ((ISupportProperties)telemetry).Properties[PropertyNameGlobal];
                };

            var configuration = new TelemetryConfiguration(string.Empty, new StubTelemetryChannel()) { TelemetryInitializers = { initializer } };

            var client = new TelemetryClient(configuration);
            client.Context.Properties[PropertyName] = "ClientValue";
            client.Context.Properties[PropertyNameGlobal] = "ClientValue";
            client.Context.InstrumentationKey = "Test Key";

            client.Track(new StubTelemetry());

            Assert.AreEqual(client.Context.Properties[PropertyName], valueInInitializer);
            Assert.AreEqual(client.Context.Properties[PropertyNameGlobal], globalValueInInitializer);
        }

#if (!NETCOREAPP) // This constant is defined for all versions of NetCore https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries#how-to-multitarget
        [TestMethod]
        public void TrackAddsSdkVerionByDefault()
        {
            // split version by 4 numbers manually so we do not do the same as in the product code and actually test it
            string versonStr = Assembly.GetAssembly(typeof(TelemetryConfiguration)).GetCustomAttributes(false)
                    .OfType<AssemblyFileVersionAttribute>()
                    .First()
                    .Version;
            string[] versionParts = new Version(versonStr).ToString().Split('.');

            var configuration = new TelemetryConfiguration(Guid.NewGuid().ToString(), new StubTelemetryChannel());
            var client = new TelemetryClient(configuration);

            client.Context.InstrumentationKey = "Test";
            EventTelemetry eventTelemetry = new EventTelemetry("test");
            client.Track(eventTelemetry);

            var expected = "dotnet:" + string.Join(".", versionParts[0], versionParts[1], versionParts[2]) + "-" + versionParts[3];

            Assert.AreEqual(expected, eventTelemetry.Context.Internal.SdkVersion);
        }

#endif

        [TestMethod]
        public void TrackDoesNotOverrideSdkVersion()
        {
            var configuration = new TelemetryConfiguration(Guid.NewGuid().ToString(), new StubTelemetryChannel());
            var client = new TelemetryClient(configuration);

            client.Context.InstrumentationKey = "Test";
            EventTelemetry eventTelemetry = new EventTelemetry("test");
            eventTelemetry.Context.Internal.SdkVersion = "test";
            client.Track(eventTelemetry);

            Assert.AreEqual("test", eventTelemetry.Context.Internal.SdkVersion);
        }

        [TestMethod]
        public void TrackClearsTelemetryContextRawStorageTempAfterInitializersAreRun()
        {
            const string keyTemp = "fooTemp";
            const string detailTemp = "barTemp";
            const string keyPerm = "fooPerm";
            const string detailPerm = "barPerm";
            var sentTelemetry = new List<ITelemetry>();
            var channel = new StubTelemetryChannel
            {
                OnSend = t =>
                {
                    // Upon reaching TelemetryChannel temp object should not exist.
                    Assert.IsFalse(t.Context.TryGetRawObject(keyTemp, out object detailTmp));

                    // Upon reaching TelemetryChannel perm object should remain.
                    Assert.IsTrue(t.Context.TryGetRawObject(keyPerm, out object detailPrm));
                }
            };           
            var configuration = new TelemetryConfiguration(string.Empty, channel);

            configuration.TelemetryProcessorChainBuilder.Use((next) => new StubTelemetryProcessor(next)
            {
                OnProcess = t =>
                {
                
                // Upon reaching TelemetryProcessor temp object should not exist.
                Assert.IsFalse(t.Context.TryGetRawObject(keyTemp, out object detailTmp));

                // Upon reaching TelemetryProcessor perm object should remain.
                Assert.IsTrue(t.Context.TryGetRawObject(keyPerm, out object detailPrm));
                }
            });

            configuration.TelemetryProcessorChainBuilder.Build();
            
            var telemetryInitializer = new StubTelemetryInitializer();
            telemetryInitializer.OnInitialize = item =>
            {
                // TelemetryInitializer should be able to access both temp and perm objects.
                Assert.IsTrue(item.Context.TryGetRawObject(keyTemp, out object detailTmp));
                Assert.IsTrue(item.Context.TryGetRawObject(keyPerm, out object detailPrm));
            };

            configuration.TelemetryInitializers.Add(telemetryInitializer);

            var client = new TelemetryClient(configuration);
            var telemetry = new StubTelemetry();
            telemetry.Context.StoreRawObject(keyTemp, detailTemp, true);
            telemetry.Context.StoreRawObject(keyPerm, detailPerm, false);
            
            // Calling Track will in turn call all TelemetryInitializers followed by TelemetryProcessors
            // and Channel. Test here is to validate that temp RawObjects stored in TelemetryContext gets
            // cleared after Initialiers are run.
            client.Track(telemetry);
        }

        #endregion

        #region Sampling

        [TestMethod]
        public void AllTelemetryIsSentWithDefaultSamplingRate()
        {
            var sentTelemetry = new List<ITelemetry>();
            var channel = new StubTelemetryChannel { OnSend = t => sentTelemetry.Add(t) };
            var configuration = new TelemetryConfiguration("Test key", channel);            
            var client = new TelemetryClient(configuration);

            const int ItemsToGenerate = 100;

            for (int i = 0; i < ItemsToGenerate; i++)
            {
                client.TrackRequest(new RequestTelemetry());
            }

            Assert.AreEqual(ItemsToGenerate, sentTelemetry.Count);
        }

        [TestMethod]
        public void ProactivelySampledOutTelemetryIsNotInitialized()
        {
            var sentTelemetry = new List<ITelemetry>();
            var channel = new StubTelemetryChannel { OnSend = t => sentTelemetry.Add(t) };

            var configuration = new TelemetryConfiguration("Test key", channel);

            var initializedTelemetry = new List<ITelemetry>();
            var telemetryInitializer = new StubTelemetryInitializer();
            telemetryInitializer.OnInitialize = item =>
            {   
                initializedTelemetry.Add(item);
            };

            configuration.TelemetryInitializers.Add(telemetryInitializer);

            var client = new TelemetryClient(configuration);

            var telemetry = new RequestTelemetry();
            telemetry.ProactiveSamplingDecision = SamplingDecision.SampledOut;
            client.Track(telemetry);

            Assert.AreEqual(SamplingDecision.SampledOut, telemetry.ProactiveSamplingDecision);
            Assert.AreEqual(0, initializedTelemetry.Count);
            Assert.IsNull(telemetry.Context.Internal.SdkVersion);
            Assert.IsNull(telemetry.Context.Internal.NodeName);            
            Assert.AreEqual(1, sentTelemetry.Count);
        }
        #endregion

        #region ValidateEndpoint

        [TestMethod]
        public async System.Threading.Tasks.Task SendEventToValidateEndpointAsync()
        {
            string unicodeString = "русский\\#/\x0000\x0001\x0002\x0003\x0004\x0005\x0006\x0007\x0008\x009Farabicشلاؤيثبلاهتنمةىخحضقسفعشلاؤيصثبل c\n\r\t";

            EventTelemetry telemetry1 = new EventTelemetry(unicodeString);
            MetricTelemetry telemetry2 = new MetricTelemetry("name", 100);
            DependencyTelemetry telemetry3 = new DependencyTelemetry("name", "commandName", DateTimeOffset.UtcNow, TimeSpan.FromHours(3), true);
            ExceptionTelemetry telemetry4 = new ExceptionTelemetry(new ArgumentException("Test"));
            MetricTelemetry telemetry5 = new MetricTelemetry("name", 100);
            PageViewTelemetry telemetry6 = new PageViewTelemetry("name");
#pragma warning disable 618
            PerformanceCounterTelemetry telemetry7 = new PerformanceCounterTelemetry("category", "name", "instance", 100);
#pragma warning restore 618
            RequestTelemetry telemetry8 = new RequestTelemetry("name", DateTimeOffset.UtcNow, TimeSpan.FromHours(2), "200", true);
#pragma warning disable 618
            SessionStateTelemetry telemetry9 = new SessionStateTelemetry(SessionState.Start);
#pragma warning restore 618
            TraceTelemetry telemetry10 = new TraceTelemetry("text");
            AvailabilityTelemetry telemetry11 = new AvailabilityTelemetry("name", DateTimeOffset.UtcNow, TimeSpan.FromHours(10), "location", true, "message");

            var telemetryItems = new List<ITelemetry>
            {
                telemetry1,
                telemetry2,
                telemetry3,
                telemetry4,
                telemetry5,
                telemetry6,
                telemetry7,
                telemetry8,
                telemetry9,
                telemetry10,
                telemetry11
            };

            // ChuckNorrisTeamUnitTests resource in Prototypes1
            var config = new TelemetryConfiguration("687218b9-2250-4eaa-8946-2dd5cc35eff8");
            var telemetryClient = new TelemetryClient(config);
            telemetryClient.Context.GlobalProperties.Add(unicodeString, unicodeString);
            
            telemetryClient.Initialize(telemetry1);
            telemetryClient.Initialize(telemetry2);
            telemetryClient.Initialize(telemetry3);
            telemetryClient.Initialize(telemetry4);
            telemetryClient.Initialize(telemetry5);
            telemetryClient.Initialize(telemetry6);
            telemetryClient.Initialize(telemetry7);
            telemetryClient.Initialize(telemetry8);
            telemetryClient.Initialize(telemetry9);
            telemetryClient.Initialize(telemetry10);
            telemetryClient.Initialize(telemetry11);

            string json = JsonSerializer.SerializeAsString(telemetryItems);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            HttpClient client = new HttpClient();
            try
            {
                var result = await client.PostAsync(
                    "https://dc.services.visualstudio.com/v2/validate",
                    new ByteArrayContent(jsonBytes));
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    var response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Trace.WriteLine(response);
                }

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception occuring trying to send items to backend." + ex);
            }
        }

        [TestMethod]
        public void SerailizeRemovesEmptyPropertiesAndProducesValidJson()
        {
            var telemetryIn = new ExceptionTelemetry(new InvalidOperationException());
            telemetryIn.Properties.Add("MyKey", null);

            string json = JsonSerializer.SerializeAsString(telemetryIn);
            ExceptionTelemetry telemetryOut = Newtonsoft.Json.JsonConvert.DeserializeObject<ExceptionTelemetry>(json);
            Assert.AreEqual(0, telemetryOut.Properties.Count);
        }

        #endregion

        #region Preaggregated metrics

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetMetric_SendsData()
        {
            IList<ITelemetry> sentTelemetry;
            TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out sentTelemetry);
            TelemetryClient client = new TelemetryClient(telemetryPipeline);

            {
                Metric metric = client.GetMetric("CowsSold");
                Assert.IsNotNull(metric);
                Assert.AreEqual(0, metric.Identifier.DimensionsCount);
                Assert.AreEqual(String.Empty, metric.Identifier.MetricNamespace);
                Assert.AreEqual("CowsSold", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                metric.TrackValue(0.5);
                metric.TrackValue(0.6);
                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(1.5, "A"));
                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(2.5, "A", "X"));

                telemetryPipeline.GetMetricManager().Flush();
                Assert.AreEqual(1, sentTelemetry.Count);
                TestUtil.ValidateNumericAggregateValues(sentTelemetry[0], "", "CowsSold", 2, 1.1, 0.6, 0.5, 0.05);
                Assert.AreEqual(1, ((MetricTelemetry)sentTelemetry[0]).Properties.Count);                
                Assert.IsTrue(((MetricTelemetry)sentTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                sentTelemetry.Clear();

                metric.TrackValue(0.7);
                metric.TrackValue(0.8);

                telemetryPipeline.GetMetricManager().Flush();
                Assert.AreEqual(1, sentTelemetry.Count);
                TestUtil.ValidateNumericAggregateValues(sentTelemetry[0], "", "CowsSold", 2, 1.5, 0.8, 0.7, 0.05);
                Assert.AreEqual(1, ((MetricTelemetry)sentTelemetry[0]).Properties.Count);
                Assert.IsTrue(((MetricTelemetry)sentTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                sentTelemetry.Clear();
            }
            {
                Metric metric = client.GetMetric("CowsSold", "Color", MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(1, metric.Identifier.DimensionsCount);
                Assert.AreEqual(String.Empty, metric.Identifier.MetricNamespace);
                Assert.AreEqual("CowsSold", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                metric.TrackValue(0.5, "Purple");
                metric.TrackValue(0.6, "Purple");
                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(2.5, "A", "X"));

                telemetryPipeline.GetMetricManager().Flush();
                Assert.AreEqual(1, sentTelemetry.Count);
                TestUtil.ValidateNumericAggregateValues(sentTelemetry[0], "", "CowsSold", 2, 1.1, 0.6, 0.5, 0.05);
                Assert.AreEqual(2, ((MetricTelemetry)sentTelemetry[0]).Properties.Count);
                Assert.IsTrue(((MetricTelemetry)sentTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                Assert.AreEqual("Purple", ((MetricTelemetry)sentTelemetry[0]).Properties["Color"]);
                sentTelemetry.Clear();

                metric.TrackValue(0.7, "Purple");
                metric.TrackValue(0.8, "Purple");

                telemetryPipeline.GetMetricManager().Flush();
                Assert.AreEqual(1, sentTelemetry.Count);
                TestUtil.ValidateNumericAggregateValues(sentTelemetry[0], String.Empty, "CowsSold", 2, 1.5, 0.8, 0.7, 0.05);
                Assert.AreEqual(2, ((MetricTelemetry)sentTelemetry[0]).Properties.Count);
                Assert.IsTrue(((MetricTelemetry)sentTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                Assert.AreEqual("Purple", ((MetricTelemetry)sentTelemetry[0]).Properties["Color"]);
                sentTelemetry.Clear();
            }
            {
                Metric metric = client.GetMetric("CowsSold", "Color", "Size", MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(2, metric.Identifier.DimensionsCount);
                Assert.AreEqual(String.Empty, metric.Identifier.MetricNamespace);
                Assert.AreEqual("CowsSold", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                metric.TrackValue(0.5, "Purple", "Large");
                metric.TrackValue(0.6, "Purple", "Large");

                telemetryPipeline.GetMetricManager().Flush();
                Assert.AreEqual(1, sentTelemetry.Count);

                MetricTelemetry[] orderedTelemetry = sentTelemetry
                                                        .OrderByDescending((t) => ((MetricTelemetry) t).Count * 10000 + ((MetricTelemetry) t).Sum)
                                                        .Select((t) => (MetricTelemetry) t)
                                                        .ToArray();

                TestUtil.ValidateNumericAggregateValues(orderedTelemetry[0], String.Empty, "CowsSold", 2, 1.1, 0.6, 0.5, 0.05);
                Assert.AreEqual(3, ((MetricTelemetry)orderedTelemetry[0]).Properties.Count);
                Assert.IsTrue(((MetricTelemetry)orderedTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                Assert.AreEqual("Purple", ((MetricTelemetry)orderedTelemetry[0]).Properties["Color"]);
                Assert.AreEqual("Large", ((MetricTelemetry)orderedTelemetry[0]).Properties["Size"]);
                sentTelemetry.Clear();

                metric.TrackValue(0.7, "Purple", "Large");
                metric.TrackValue(0.8, "Purple", "Small");

                telemetryPipeline.GetMetricManager().Flush();
                Assert.AreEqual(2, sentTelemetry.Count);

                orderedTelemetry = sentTelemetry
                                            .OrderByDescending((t) => ((MetricTelemetry) t).Count * 10000 + ((MetricTelemetry) t).Sum)
                                            .Select((t) => (MetricTelemetry) t)
                                            .ToArray();

                TestUtil.ValidateNumericAggregateValues(orderedTelemetry[0], String.Empty, "CowsSold", 1, 0.8, 0.8, 0.8, 0);
                Assert.AreEqual(3, ((MetricTelemetry)orderedTelemetry[0]).Properties.Count);
                Assert.IsTrue(((MetricTelemetry)orderedTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                Assert.AreEqual("Purple", ((MetricTelemetry)orderedTelemetry[0]).Properties["Color"]);
                Assert.AreEqual("Small", ((MetricTelemetry)orderedTelemetry[0]).Properties["Size"]);

                TestUtil.ValidateNumericAggregateValues(orderedTelemetry[1], String.Empty, "CowsSold", 1, 0.7, 0.7, 0.7, 0);
                Assert.AreEqual(3, ((MetricTelemetry)orderedTelemetry[1]).Properties.Count);
                Assert.IsTrue(((MetricTelemetry)orderedTelemetry[1]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                Assert.AreEqual("Purple", ((MetricTelemetry)orderedTelemetry[1]).Properties["Color"]);
                Assert.AreEqual("Large", ((MetricTelemetry)orderedTelemetry[1]).Properties["Size"]);

                sentTelemetry.Clear();
            }
            {
                Metric metric = client.GetMetric(
                            new MetricIdentifier(
                                        "Test MetricNamespace", 
                                        "Test MetricId", 
                                        "Dim 1", 
                                        "Dim 2", 
                                        "Dim 3", 
                                        "Dim 4", 
                                        "Dim 5", 
                                        "Dim 6", 
                                        "Dim 7", 
                                        "Dim 8", 
                                        "Dim 9", 
                                        "Dim 10"),
                            MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(10, metric.Identifier.DimensionsCount);
                Assert.AreEqual("Test MetricNamespace", metric.Identifier.MetricNamespace);
                Assert.AreEqual("Test MetricId", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreEqual("Dim 1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("Dim 2", metric.Identifier.GetDimensionName(2));
                Assert.AreEqual("Dim 3", metric.Identifier.GetDimensionName(3));
                Assert.AreEqual("Dim 4", metric.Identifier.GetDimensionName(4));
                Assert.AreEqual("Dim 5", metric.Identifier.GetDimensionName(5));
                Assert.AreEqual("Dim 6", metric.Identifier.GetDimensionName(6));
                Assert.AreEqual("Dim 7", metric.Identifier.GetDimensionName(7));
                Assert.AreEqual("Dim 8", metric.Identifier.GetDimensionName(8));
                Assert.AreEqual("Dim 9", metric.Identifier.GetDimensionName(9));
                Assert.AreEqual("Dim 10", metric.Identifier.GetDimensionName(10));

                metric.TrackValue(0.5, "DV1", "DV2", "DV3", "DV4", "DV5", "DV6", "DV7", "DV8", "DV9", "DV10");
                metric.TrackValue(0.6, "DV1", "DV2", "DV3", "DV4", "DV5", "DV6", "DV7", "DV8", "DV9", "DV10");

                telemetryPipeline.GetMetricManager().Flush();
                Assert.AreEqual(1, sentTelemetry.Count);

                MetricTelemetry[] orderedTelemetry = sentTelemetry
                                        .OrderByDescending( (t) => ((MetricTelemetry) t).Count * 10000 + ((MetricTelemetry) t).Sum )
                                        .Select( (t) => (MetricTelemetry) t )
                                        .ToArray();

                TestUtil.ValidateNumericAggregateValues(orderedTelemetry[0], "Test MetricNamespace", "Test MetricId", 2, 1.1, 0.6, 0.5, 0.05);
                Assert.AreEqual(11, ((MetricTelemetry)orderedTelemetry[0]).Properties.Count);
                Assert.IsTrue(((MetricTelemetry)orderedTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                Assert.AreEqual("DV1", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 1"]);
                Assert.AreEqual("DV2", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 2"]);
                Assert.AreEqual("DV3", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 3"]);
                Assert.AreEqual("DV4", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 4"]);
                Assert.AreEqual("DV5", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 5"]);
                Assert.AreEqual("DV6", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 6"]);
                Assert.AreEqual("DV7", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 7"]);
                Assert.AreEqual("DV8", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 8"]);
                Assert.AreEqual("DV9", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 9"]);
                Assert.AreEqual("DV10", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 10"]);
                sentTelemetry.Clear();

                metric.TrackValue(0.7, "DV1", "DV2", "DV3", "DV4", "DV5", "DV6", "DV7", "DV8", "DV9", "DV10");
                metric.TrackValue(0.8, "DV1", "DV2", "DV3", "DV4", "DV5", "DV6a", "DV7", "DV8", "DV9", "DV10");

                telemetryPipeline.GetMetricManager().Flush();
                Assert.AreEqual(2, sentTelemetry.Count);

                orderedTelemetry = sentTelemetry
                                        .OrderByDescending( (t) => ((MetricTelemetry) t).Count * 10000 + ((MetricTelemetry) t).Sum )
                                        .Select( (t) => (MetricTelemetry) t )
                                        .ToArray();

                TestUtil.ValidateNumericAggregateValues(orderedTelemetry[0], "Test MetricNamespace", "Test MetricId", 1, 0.8, 0.8, 0.8, 0);
                Assert.AreEqual(11, ((MetricTelemetry)orderedTelemetry[0]).Properties.Count);
                Assert.IsTrue(((MetricTelemetry)orderedTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                Assert.AreEqual("DV1", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 1"]);
                Assert.AreEqual("DV2", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 2"]);
                Assert.AreEqual("DV3", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 3"]);
                Assert.AreEqual("DV4", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 4"]);
                Assert.AreEqual("DV5", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 5"]);
                Assert.AreEqual("DV6a", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 6"]);
                Assert.AreEqual("DV7", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 7"]);
                Assert.AreEqual("DV8", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 8"]);
                Assert.AreEqual("DV9", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 9"]);
                Assert.AreEqual("DV10", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim 10"]);

                TestUtil.ValidateNumericAggregateValues(orderedTelemetry[1], "Test MetricNamespace", "Test MetricId", 1, 0.7, 0.7, 0.7, 0);
                Assert.AreEqual(11, ((MetricTelemetry)orderedTelemetry[1]).Properties.Count);
                Assert.IsTrue(((MetricTelemetry)orderedTelemetry[1]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                Assert.AreEqual("DV1", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 1"]);
                Assert.AreEqual("DV2", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 2"]);
                Assert.AreEqual("DV3", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 3"]);
                Assert.AreEqual("DV4", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 4"]);
                Assert.AreEqual("DV5", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 5"]);
                Assert.AreEqual("DV6", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 6"]);
                Assert.AreEqual("DV7", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 7"]);
                Assert.AreEqual("DV8", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 8"]);
                Assert.AreEqual("DV9", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 9"]);
                Assert.AreEqual("DV10", ((MetricTelemetry)orderedTelemetry[1]).Properties["Dim 10"]);

                sentTelemetry.Clear();
            }

            TestUtil.CompleteDefaultAggregationCycle(telemetryPipeline.GetMetricManager());
            telemetryPipeline.Dispose();
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetMetric_RespectsMetricConfiguration()
        {
            IList<ITelemetry> sentTelemetry;
            TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out sentTelemetry);
            TelemetryClient client = new TelemetryClient(telemetryPipeline);

            {
                Metric metric = client.GetMetric("M1");
                Assert.IsNotNull(metric);
                Assert.AreEqual(0, metric.Identifier.DimensionsCount);
                Assert.AreEqual("M1", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("M2", MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(0, metric.Identifier.DimensionsCount);
                Assert.AreEqual("M2", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("M3", MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(0, metric.Identifier.DimensionsCount);
                Assert.AreEqual("M3", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
            }
            {
                MetricConfiguration config = new MetricConfiguration(10, 10, new MetricSeriesConfigurationForMeasurement(true));
                Metric metric = client.GetMetric("M4", config);
                Assert.IsNotNull(metric);
                Assert.AreEqual(0, metric.Identifier.DimensionsCount);
                Assert.AreEqual("M4", metric.Identifier.MetricId);
                Assert.AreEqual(config, metric.GetConfiguration());
                Assert.AreSame(config, metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(new MetricSeriesConfigurationForMeasurement(true), series.GetConfiguration());
                Assert.AreNotSame(new MetricSeriesConfigurationForMeasurement(true), series.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("M5", "Dim1");
                Assert.IsNotNull(metric);
                Assert.AreEqual(1, metric.Identifier.DimensionsCount);
                Assert.AreEqual("Dim1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("M5", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.IsTrue(metric.TryGetDataSeries(out series, "Dim1Val"));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("M6", "Dim1", MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(1, metric.Identifier.DimensionsCount);
                Assert.AreEqual("Dim1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("M6", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.IsTrue(metric.TryGetDataSeries(out series, "Dim1Val"));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("M7", "Dim1", MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(1, metric.Identifier.DimensionsCount);
                Assert.AreEqual("Dim1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("M7", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.IsTrue(metric.TryGetDataSeries(out series, "Dim1Val"));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
            }
            {
                MetricConfiguration config = new MetricConfiguration(10, 10, new MetricSeriesConfigurationForMeasurement(true));
                Metric metric = client.GetMetric("M8", "Dim1", config);
                Assert.IsNotNull(metric);
                Assert.AreEqual(1, metric.Identifier.DimensionsCount);
                Assert.AreEqual("Dim1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("M8", metric.Identifier.MetricId);
                Assert.AreEqual(config, metric.GetConfiguration());
                Assert.AreSame(config, metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(config.SeriesConfig, series.GetConfiguration());
                Assert.AreSame(config.SeriesConfig, series.GetConfiguration());
                Assert.IsTrue(metric.TryGetDataSeries(out series, "Dim1Val"));
                Assert.AreEqual(new MetricSeriesConfigurationForMeasurement(true), series.GetConfiguration());
                Assert.AreNotSame(new MetricSeriesConfigurationForMeasurement(true), series.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("M9", "Dim1", "Dim2");
                Assert.IsNotNull(metric);
                Assert.AreEqual(2, metric.Identifier.DimensionsCount);
                Assert.AreEqual("Dim1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("Dim2", metric.Identifier.GetDimensionName(2));
                Assert.AreEqual("M9", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.IsTrue(metric.TryGetDataSeries(out series, "Dim1Val", "Dim2val"));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("M10", "Dim1", "Dim2", MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(2, metric.Identifier.DimensionsCount);
                Assert.AreEqual("Dim1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("Dim2", metric.Identifier.GetDimensionName(2));
                Assert.AreEqual("M10", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.IsTrue(metric.TryGetDataSeries(out series, "Dim1Val", "Dim2val"));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("M11", "Dim1", "Dim2", MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(2, metric.Identifier.DimensionsCount);
                Assert.AreEqual("Dim1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("Dim2", metric.Identifier.GetDimensionName(2));
                Assert.AreEqual("M11", metric.Identifier.MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.IsTrue(metric.TryGetDataSeries(out series, "Dim1Val", "Dim2val"));
                Assert.AreEqual(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement().SeriesConfig, series.GetConfiguration());
            }
            {
                MetricConfiguration config = new MetricConfiguration(10, 10, new MetricSeriesConfigurationForMeasurement(true));
                Metric metric = client.GetMetric("M12", "Dim1", "Dim2", config);
                Assert.IsNotNull(metric);
                Assert.AreEqual(2, metric.Identifier.DimensionsCount);
                Assert.AreEqual("Dim1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("Dim2", metric.Identifier.GetDimensionName(2));
                Assert.AreEqual("M12", metric.Identifier.MetricId);
                Assert.AreEqual(config, metric.GetConfiguration());
                Assert.AreSame(config, metric.GetConfiguration());

                MetricSeries series;
                Assert.IsTrue(metric.TryGetDataSeries(out series));
                Assert.AreEqual(config.SeriesConfig, series.GetConfiguration());
                Assert.AreSame(config.SeriesConfig, series.GetConfiguration());
                Assert.IsTrue(metric.TryGetDataSeries(out series, "Dim1Val", "Dim2val"));
                Assert.AreEqual(new MetricSeriesConfigurationForMeasurement(true), series.GetConfiguration());
                Assert.AreNotSame(new MetricSeriesConfigurationForMeasurement(true), series.GetConfiguration());
                Assert.AreSame(config.SeriesConfig, series.GetConfiguration());
            }

            TestUtil.CompleteDefaultAggregationCycle(telemetryPipeline.GetMetricManager());
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetMetric_DetectsMetricConfigurationConflicts()
        {
            IList<ITelemetry> sentTelemetry;
            TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out sentTelemetry);
            TelemetryClient client = new TelemetryClient(telemetryPipeline);

            {
                Metric m1 = client.GetMetric("M01");
                Assert.IsNotNull(m1);

                Metric m2 = client.GetMetric("M01");
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M01 ");
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M01", MetricConfigurations.Common.Measurement());
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M01", metricConfiguration: null);
                Assert.AreSame(m1, m2);

                Assert.ThrowsException<ArgumentException>(() => client.GetMetric(
                                            "M01", new MetricConfiguration(1, 1, new MetricSeriesConfigurationForMeasurement(false))));

                MetricConfiguration config1 = new MetricConfiguration(10, 10, new MetricSeriesConfigurationForMeasurement(false));
                MetricConfiguration config2 = new MetricConfiguration(10, 10, new MetricSeriesConfigurationForMeasurement(false));
                Assert.AreEqual(config1, config2);
                Assert.AreNotSame(config1, config2);

                m1 = client.GetMetric("M02", config1);
                Assert.IsNotNull(m1);

                m2 = client.GetMetric("M02", config2);
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M02", metricConfiguration: null);
                Assert.AreSame(m1, m2);

                config2 = new MetricConfiguration(10, 101, new MetricSeriesConfigurationForMeasurement(false));
                Assert.AreNotEqual(config1, config2);
                Assert.AreNotSame(config1, config2);

                Assert.ThrowsException<ArgumentException>(() => client.GetMetric("M02", config2));
                Assert.ThrowsException<ArgumentException>(() => client.GetMetric("M02 ", config2));
            }
            {
                Metric m1 = client.GetMetric("M11", "Dim1");
                Assert.IsNotNull(m1);

                Metric m2 = client.GetMetric("M11", "Dim1");
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric(" M11", "Dim1");
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M11", " Dim1");
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric(" M11", " Dim1", MetricConfigurations.Common.Measurement());
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M11", "Dim1", metricConfiguration: null);
                Assert.AreSame(m1, m2);

                Assert.ThrowsException<ArgumentException>(() => client.GetMetric(
                                                                    "M11",
                                                                    "Dim1 ",
                                                                    new MetricConfiguration(1, 1, new MetricSeriesConfigurationForMeasurement(false))));

                MetricConfiguration config1 = new MetricConfiguration(10, 10, new MetricSeriesConfigurationForMeasurement(false));
                MetricConfiguration config2 = new MetricConfiguration(10, 10, new MetricSeriesConfigurationForMeasurement(false));
                Assert.AreEqual(config1, config2);
                Assert.AreNotSame(config1, config2);

                m1 = client.GetMetric("M12 ", "Dim1", config1);
                Assert.IsNotNull(m1);

                m2 = client.GetMetric("M12", "Dim1 ", config2);
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M12", "Dim1", metricConfiguration: null);
                Assert.AreSame(m1, m2);

                config2 = new MetricConfiguration(10, 101, new MetricSeriesConfigurationForMeasurement(false));
                Assert.AreNotEqual(config1, config2);
                Assert.AreNotSame(config1, config2);

                Assert.ThrowsException<ArgumentException>(() => client.GetMetric("M12", "Dim1", config2));
                Assert.ThrowsException<ArgumentException>(() => client.GetMetric("M12 ", "Dim1", config2));
            }
            {
                Metric m1 = client.GetMetric("M21", "Dim1", "Dim2");
                Assert.IsNotNull(m1);

                Metric m2 = client.GetMetric("M21", "Dim1", "Dim2");
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric(" M21", "Dim1", "Dim2");
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M21", " Dim1", "Dim2");
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M21", "Dim1", " Dim2");
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric(" M21", " Dim1", "Dim2", MetricConfigurations.Common.Measurement());
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M21", "Dim1", "Dim2 ", metricConfiguration: null);
                Assert.AreSame(m1, m2);

                Assert.ThrowsException<ArgumentException>(() => client.GetMetric(
                                                                    "M21", 
                                                                    "Dim1 ", 
                                                                    "Dim2", 
                                                                    new MetricConfiguration(1, 1, new MetricSeriesConfigurationForMeasurement(false))));

                MetricConfiguration config1 = new MetricConfiguration(10, 10, new MetricSeriesConfigurationForMeasurement(false));
                MetricConfiguration config2 = new MetricConfiguration(10, 10, new MetricSeriesConfigurationForMeasurement(false));
                Assert.AreEqual(config1, config2);
                Assert.AreNotSame(config1, config2);

                m1 = client.GetMetric("M22 ", "Dim1", "Dim2 ", config1);
                Assert.IsNotNull(m1);

                m2 = client.GetMetric("M22", "Dim1 ", "Dim2", config2);
                Assert.AreSame(m1, m2);

                m2 = client.GetMetric("M22", "Dim1", "Dim2", metricConfiguration: null);
                Assert.AreSame(m1, m2);

                config2 = new MetricConfiguration(10, 101, new MetricSeriesConfigurationForMeasurement(false));
                Assert.AreNotEqual(config1, config2);
                Assert.AreNotSame(config1, config2);

                Assert.ThrowsException<ArgumentException>(() => client.GetMetric("M22", "Dim1", "Dim2", config2));
                Assert.ThrowsException<ArgumentException>(() => client.GetMetric("M22 ", "Dim1", "Dim2", config2));
            }
            {
                Metric m0 = client.GetMetric("Xxx");
                Metric m1 = client.GetMetric("Xxx", "Dim1");
                Metric m2 = client.GetMetric("Xxx", "Dim1", "Dim2");

                Assert.IsNotNull(m0);
                Assert.IsNotNull(m1);
                Assert.IsNotNull(m2);

                Assert.AreNotSame(m0, m1);
                Assert.AreNotSame(m0, m2);
                Assert.AreNotSame(m1, m2);

                Assert.AreSame(m0.GetConfiguration(), m1.GetConfiguration());
                Assert.AreSame(m0.GetConfiguration(), m2.GetConfiguration());
                Assert.AreSame(m1.GetConfiguration(), m2.GetConfiguration());

                Assert.AreSame(MetricConfigurations.Common.Measurement(), m0.GetConfiguration());
            }

            TestUtil.CompleteDefaultAggregationCycle(telemetryPipeline.GetMetricManager());
            telemetryPipeline.Dispose();
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetMetric_RespectsAggregationScope()
        {
            IList<ITelemetry> sentTelemetry1, sentTelemetry2;
            TelemetryConfiguration telemetryPipeline1 = TestUtil.CreateAITelemetryConfig(out sentTelemetry1);
            TelemetryConfiguration telemetryPipeline2 = TestUtil.CreateAITelemetryConfig(out sentTelemetry2);
            TelemetryClient client11 = new TelemetryClient(telemetryPipeline1);
            TelemetryClient client12 = new TelemetryClient(telemetryPipeline1);
            TelemetryClient client21 = new TelemetryClient(telemetryPipeline2);

            Metric metricA111 = client11.GetMetric("Metric A", "Dim1", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryConfiguration);
            metricA111.TrackValue(101);
            metricA111.TrackValue(102);
            metricA111.TrackValue(111, "Val");
            metricA111.TrackValue(112, "Val");

            Metric metricA112 = client11.GetMetric("Metric A", "Dim1", MetricConfigurations.Common.Measurement());
            metricA112.TrackValue(103);
            metricA112.TrackValue(104);
            metricA112.TrackValue(113, "Val");
            metricA112.TrackValue(114, "Val");

            Metric metricA113 = client11.GetMetric("Metric A", "Dim1");
            metricA113.TrackValue(105);
            metricA113.TrackValue(106);
            metricA113.TrackValue(115, "Val");
            metricA113.TrackValue(116, "Val");

            Assert.AreSame(metricA111, metricA112);
            Assert.AreSame(metricA111, metricA113);
            Assert.AreSame(metricA112, metricA113);

            MetricSeries series1, series2;
            Assert.IsTrue(metricA111.TryGetDataSeries(out series1));
            Assert.IsTrue(metricA112.TryGetDataSeries(out series2));
            Assert.AreSame(series1, series2);
            Assert.IsTrue(metricA113.TryGetDataSeries(out series2));
            Assert.AreSame(series1, series2);
            Assert.IsTrue(metricA112.TryGetDataSeries(out series1));
            Assert.AreSame(series1, series2);

            Assert.IsTrue(metricA111.TryGetDataSeries(out series1, "Val"));
            Assert.IsTrue(metricA112.TryGetDataSeries(out series2, "Val"));
            Assert.AreSame(series1, series2);
            Assert.IsTrue(metricA113.TryGetDataSeries(out series2, "Val"));
            Assert.AreSame(series1, series2);
            Assert.IsTrue(metricA112.TryGetDataSeries(out series1, "Val"));
            Assert.AreSame(series1, series2);

            Metric metricA121 = client12.GetMetric("Metric A", "Dim1", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryConfiguration);
            metricA121.TrackValue(107);
            metricA121.TrackValue(108);
            metricA121.TrackValue(117, "Val");
            metricA121.TrackValue(118, "Val");

            Assert.AreSame(metricA111, metricA121);

            Metric metricA211 = client21.GetMetric("Metric A", "Dim1", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryConfiguration);
            metricA211.TrackValue(201);
            metricA211.TrackValue(202);
            metricA211.TrackValue(211, "Val");
            metricA211.TrackValue(212, "Val");

            Assert.AreNotSame(metricA111, metricA211);

            Metric metricA11c1 = client11.GetMetric("Metric A", "Dim1", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryClient);
            metricA11c1.TrackValue(301);
            metricA11c1.TrackValue(302);
            metricA11c1.TrackValue(311, "Val");
            metricA11c1.TrackValue(312, "Val");

            Metric metricA11c2 = client11.GetMetric("Metric A", "Dim1", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryClient);
            metricA11c2.TrackValue(303);
            metricA11c2.TrackValue(304);
            metricA11c2.TrackValue(313, "Val");
            metricA11c2.TrackValue(314, "Val");

            Assert.AreNotSame(metricA111, metricA11c1);
            Assert.AreSame(metricA11c1, metricA11c2);

            Assert.IsTrue(metricA11c1.TryGetDataSeries(out series1));
            Assert.IsTrue(metricA11c1.TryGetDataSeries(out series2));
            Assert.AreSame(series1, series2);

            Assert.IsTrue(metricA11c1.TryGetDataSeries(out series1, "Val"));
            Assert.IsTrue(metricA11c1.TryGetDataSeries(out series2, "Val"));
            Assert.AreSame(series1, series2);

            Metric metricA12c1 = client12.GetMetric("Metric A", "Dim1", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryClient);
            metricA12c1.TrackValue(305);
            metricA12c1.TrackValue(306);
            metricA12c1.TrackValue(315, "Val");
            metricA12c1.TrackValue(316, "Val");

            Assert.AreNotSame(metricA11c1, metricA12c1);

            client11.GetMetricManager(MetricAggregationScope.TelemetryClient).Flush();
            client12.GetMetricManager(MetricAggregationScope.TelemetryClient).Flush();
            client21.GetMetricManager(MetricAggregationScope.TelemetryClient).Flush();
            telemetryPipeline1.GetMetricManager().Flush();
            telemetryPipeline2.GetMetricManager().Flush();

            Assert.AreEqual(6, sentTelemetry1.Count);
            Assert.AreEqual(2, sentTelemetry2.Count);

            MetricTelemetry[] orderedTelemetry = sentTelemetry1
                                                        .OrderByDescending((t) => ((MetricTelemetry) t).Count * 10000 + ((MetricTelemetry) t).Sum)
                                                        .Select((t) => (MetricTelemetry) t)
                                                        .ToArray();

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[0], String.Empty, "Metric A", 8, 916, 118, 111, 2.29128784747792);
            Assert.AreEqual(2, ((MetricTelemetry)orderedTelemetry[0]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
            Assert.AreEqual("Val", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim1"]);

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[1], String.Empty, "Metric A", 8, 836, 108, 101, 2.29128784747792);
            Assert.AreEqual(1, ((MetricTelemetry)orderedTelemetry[1]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[1]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[2], String.Empty, "Metric A", 4, 1250, 314, 311, 1.11803398874989);
            Assert.AreEqual(2, ((MetricTelemetry)orderedTelemetry[2]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[2]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
            Assert.AreEqual("Val", ((MetricTelemetry)orderedTelemetry[2]).Properties["Dim1"]);

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[3], String.Empty, "Metric A", 4, 1210, 304, 301, 1.11803398874989);
            Assert.AreEqual(1, ((MetricTelemetry)orderedTelemetry[3]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[3]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[4], String.Empty, "Metric A", 2, 631, 316, 315, 0.5);
            Assert.AreEqual(2, ((MetricTelemetry)orderedTelemetry[4]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[4]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
            Assert.AreEqual("Val", ((MetricTelemetry)orderedTelemetry[4]).Properties["Dim1"]);

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[5], String.Empty, "Metric A", 2, 611, 306, 305, 0.5);
            Assert.AreEqual(1, ((MetricTelemetry)orderedTelemetry[5]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[5]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

            orderedTelemetry = sentTelemetry2
                                        .OrderByDescending((t) => ((MetricTelemetry) t).Count * 10000 + ((MetricTelemetry) t).Sum)
                                        .Select((t) => (MetricTelemetry) t)
                                        .ToArray();

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[0], String.Empty, "Metric A", 2, 423, 212, 211, 0.5);
            Assert.AreEqual(2, ((MetricTelemetry)orderedTelemetry[0]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
            Assert.AreEqual("Val", ((MetricTelemetry)orderedTelemetry[0]).Properties["Dim1"]);

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[1], String.Empty, "Metric A", 2, 403, 202, 201, 0.5);
            Assert.AreEqual(1, ((MetricTelemetry)orderedTelemetry[1]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[1]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));


            Metric metricB21c1 = client21.GetMetric("Metric B", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryClient);

            TelemetryClient client22 = new TelemetryClient(telemetryPipeline2);
            TelemetryClient client23 = new TelemetryClient(telemetryPipeline2);
            Assert.AreNotSame(metricB21c1, client22.GetMetric("Metric B", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryClient));
            Assert.AreSame(metricB21c1, client21.GetMetric("Metric B", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryClient));
            Assert.ThrowsException<ArgumentException>(() => client21.GetMetric(
                                                                        "Metric B",
                                                                        new MetricConfiguration(1, 1, new MetricSeriesConfigurationForMeasurement(false)),
                                                                        MetricAggregationScope.TelemetryClient));
            Assert.IsNotNull(client23.GetMetric("Metric B", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryClient));

            Metric metricB211 = client21.GetMetric("Metric B", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryConfiguration);

            TelemetryClient client24 = new TelemetryClient(telemetryPipeline2);
            TelemetryClient client25 = new TelemetryClient(telemetryPipeline2);
            Assert.AreSame(metricB211, client24.GetMetric("Metric B", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryConfiguration));
            Assert.AreSame(metricB211, client21.GetMetric("Metric B", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryConfiguration));
            Assert.ThrowsException<ArgumentException>(() => client21.GetMetric(
                                                                        "Metric B",
                                                                        new MetricConfiguration(1, 1, new MetricSeriesConfigurationForMeasurement(false)), 
                                                                        MetricAggregationScope.TelemetryConfiguration));
            Assert.ThrowsException<ArgumentException>(() => client25.GetMetric(
                                                                        "Metric B",
                                                                        new MetricConfiguration(1, 1, new MetricSeriesConfigurationForMeasurement(false)), 
                                                                        MetricAggregationScope.TelemetryConfiguration));

            Assert.ThrowsException<ArgumentException>(() => client11.GetMetric("Metric C", MetricConfigurations.Common.Measurement(), (MetricAggregationScope) 42));

            TestUtil.CompleteDefaultAggregationCycle(
                        client11.GetMetricManager(MetricAggregationScope.TelemetryClient),
                        client12.GetMetricManager(MetricAggregationScope.TelemetryClient),
                        client21.GetMetricManager(MetricAggregationScope.TelemetryClient),
                        client22.GetMetricManager(MetricAggregationScope.TelemetryClient),
                        client23.GetMetricManager(MetricAggregationScope.TelemetryClient),
                        client24.GetMetricManager(MetricAggregationScope.TelemetryClient),
                        client25.GetMetricManager(MetricAggregationScope.TelemetryClient),
                        telemetryPipeline2.GetMetricManager(),
                        telemetryPipeline1.GetMetricManager());

            telemetryPipeline1.Dispose();
            telemetryPipeline2.Dispose();
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetMetric_RespectsClientContext()
        {
            IList<ITelemetry> sentTelemetry;
            TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out sentTelemetry);
            telemetryPipeline.InstrumentationKey = "754DD89F-61D6-4539-90C7-D886449E12BC";
            TelemetryClient client = new TelemetryClient(telemetryPipeline);

            Metric animalsSold = client.GetMetric("AnimalsSold", "Species", MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryClient);
            animalsSold.TrackValue(10, "Cow");
            animalsSold.TrackValue(20, "Cow");
            client.GetMetricManager(MetricAggregationScope.TelemetryClient).Flush();

            animalsSold.TrackValue(100, "Rabbit");
            animalsSold.TrackValue(200, "Rabbit");

            client.Context.InstrumentationKey = "3A3C34B6-CA2D-4372-B772-3B015E1E83DC";
            client.Context.Device.Model = "Super-Fancy";
#pragma warning disable CS0618 // Type or member is obsolete
            client.Context.Properties["MyTag"] = "MyValue";
#pragma warning restore CS0618 // Type or member is obsolete

            animalsSold.TrackValue(30, "Cow");
            animalsSold.TrackValue(40, "Cow");
            animalsSold.TrackValue(300, "Rabbit");
            animalsSold.TrackValue(400, "Rabbit");
            client.GetMetricManager(MetricAggregationScope.TelemetryClient).Flush();

            Assert.AreEqual(3, sentTelemetry.Count);

            MetricTelemetry[] orderedTelemetry = sentTelemetry
                                                        .OrderByDescending((t) => ((MetricTelemetry) t).Count * 10000 + ((MetricTelemetry) t).Sum)
                                                        .Select((t) => (MetricTelemetry) t)
                                                        .ToArray();

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[0], String.Empty, "AnimalsSold", 4, 1000, 400, 100, 111.803398874989);
            Assert.AreEqual(3, ((MetricTelemetry)orderedTelemetry[0]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[0]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
            Assert.AreEqual("Rabbit", ((MetricTelemetry)orderedTelemetry[0]).Properties["Species"]);
            Assert.AreEqual("MyValue", ((MetricTelemetry)orderedTelemetry[0]).Properties["MyTag"]);            
            Assert.AreEqual("Super-Fancy", orderedTelemetry[0].Context.Device.Model);
            Assert.AreEqual("3A3C34B6-CA2D-4372-B772-3B015E1E83DC", orderedTelemetry[0].Context.InstrumentationKey);

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[1], String.Empty, "AnimalsSold", 2, 70, 40, 30, 5);
            Assert.AreEqual(3, ((MetricTelemetry)orderedTelemetry[1]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[1]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
            Assert.AreEqual("Cow", ((MetricTelemetry)orderedTelemetry[1]).Properties["Species"]);
            Assert.AreEqual("MyValue", ((MetricTelemetry)orderedTelemetry[1]).Properties["MyTag"]);
            Assert.AreEqual("Super-Fancy", orderedTelemetry[1].Context.Device.Model);
            Assert.AreEqual("3A3C34B6-CA2D-4372-B772-3B015E1E83DC", orderedTelemetry[1].Context.InstrumentationKey);

            TestUtil.ValidateNumericAggregateValues(orderedTelemetry[2], String.Empty, "AnimalsSold", 2, 30, 20, 10, 5);
            Assert.AreEqual(2, ((MetricTelemetry)orderedTelemetry[2]).Properties.Count);
            Assert.IsTrue(((MetricTelemetry)orderedTelemetry[2]).Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
            Assert.AreEqual("Cow", ((MetricTelemetry)orderedTelemetry[2]).Properties["Species"]);
            Assert.IsNull(orderedTelemetry[2].Context.Device.Model);
            Assert.AreEqual("754DD89F-61D6-4539-90C7-D886449E12BC", orderedTelemetry[2].Context.InstrumentationKey);

            TestUtil.CompleteDefaultAggregationCycle(client.GetMetricManager(MetricAggregationScope.TelemetryClient));
            telemetryPipeline.Dispose();
        }

        #endregion Preaggregated metrics

        #region Connection Strings

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifyEndpointConnectionString_DefaultScenario()
        {
#pragma warning disable CS0618 // This constructor calls TelemetryConfiguration.Active which will throw an Obsolete compiler warning in NetCore projects. I don't care because I'm only testing that the pipeline could set a default value.
            TelemetryConfiguration.Active = null; // Need to null this because other tests can cause side effects here.

            var telemetryClient = new TelemetryClient();
#pragma warning restore CS0618

            Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", telemetryClient.TelemetryConfiguration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
        }

        #endregion

        private TelemetryClient InitializeTelemetryClient(ICollection<ITelemetry> sentTelemetry)
        {
            var channel = new StubTelemetryChannel { OnSend = t => sentTelemetry.Add(t) };
            var telemetryConfiguration = new TelemetryConfiguration(Guid.NewGuid().ToString(), channel);
            var client = new TelemetryClient(telemetryConfiguration);
            return client;
        }

#pragma warning disable 612, 618  // obsolete TelemetryConfigration.Active
        /// <summary>
        /// Resets the TelemetryConfiguration.Active default instance to null so that the iKey auto population paths can be followed for testing.
        /// </summary>
        private void ClearActiveTelemetryConfiguration()
        {
            TelemetryConfiguration.Active = null;
        }
#pragma warning restore 612, 618  // obsolete TelemetryConfigration.Active

        private double ComputeSomethingHeavy()
        {
            var random = new Random();
            double res = 0;
            for (int i = 0; i < 10000; i++)
            {
                res += Math.Sqrt(random.NextDouble());
            }

            return res;
        }
    }
}
