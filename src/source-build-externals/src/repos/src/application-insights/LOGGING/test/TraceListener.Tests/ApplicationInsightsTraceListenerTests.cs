namespace Microsoft.ApplicationInsights.TraceListener.Tests
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Microsoft.ApplicationInsights.CommonTestShared;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Tracing.Tests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static System.Globalization.CultureInfo;

    [TestClass]
    public sealed class ApplicationInsightsTraceListenerTests : IDisposable
    {
        private readonly AdapterHelper adapterHelper = new AdapterHelper();

        public TestContext TestContext { get; set; }

        public void Dispose()
        {
            this.adapterHelper.Dispose();
        }
        
        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerInitializeDoesNotThrowWhenInstrumentationKeyIsNull()
        {
            var listener = new ApplicationInsightsTraceListener(null);
            listener.Dispose();
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerInitializeTDoesNoThrowWhenInstrumentationKeyIsEmpty()
        {
            var listener = new ApplicationInsightsTraceListener(string.Empty);
            listener.Dispose();
        }
        
        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerWriteUsedApplicationInsightsConfigInstrumentationKeyWhenUnspecifiedInstrumentationKey()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.InstrumentationKey = this.adapterHelper.InstrumentationKey;

            // Changing the channel to Mock channel to verify 
            // the Telemetry event is assigned with the InstrumentationKey from configuration
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

            using (var listener = new ApplicationInsightsTraceListener(null))
            {                
                this.VerifyMessagesInMockChannel(listener, this.adapterHelper.InstrumentationKey);
            }
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerWriteNotUsingAppliocationInsightsConfigInstrumentationKeyWhenspecifiedInstrumentationKey()
        {
            // Changing the channel to Mock channel to verify 
            // the Telemetry event is assigned with the InstrumentationKey from configuration
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete
            string instrumentationKey = Guid.NewGuid().ToString();

            using (var listener = new ApplicationInsightsTraceListener(instrumentationKey))
            {
                this.VerifyMessagesInMockChannel(listener, instrumentationKey);
            }
        }
        
        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerWriteLineUseApplicationEventSourceToLogMessage()
        {
            // Changing the channel to Mock channel to verify 
            // the Telemetry event is assigned with the InstrumentationKey from configuration
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

            using (var listener = new ApplicationInsightsTraceListener(Guid.NewGuid().ToString()))
            {
                var expectedMessage = "A Message to write line";
                listener.WriteLine(expectedMessage);

                TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.First();
                Assert.AreEqual(expectedMessage, telemetry.Message);
                Assert.IsFalse(telemetry.Properties.ContainsKey("EventId"));
                Assert.AreEqual(SeverityLevel.Verbose, telemetry.SeverityLevel);
            }
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void SdkVersionIsCorrect()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

            using (var listener = new ApplicationInsightsTraceListener(Guid.NewGuid().ToString()))
            {
                listener.WriteLine("A Message to write line");

                TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.First();

                string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(prefix: "sd:", loggerType: typeof(ApplicationInsightsTraceListener));
                Assert.AreEqual(expectedVersion, telemetry.Context.GetInternalContext().SdkVersion);
            }
        }

        [TestMethod]
        [Ignore]
        [TestCategory("TraceListener")]
        public void TelemetryIsAcceptedByValidateEndpoint()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

            using (var listener = new ApplicationInsightsTraceListener(Guid.NewGuid().ToString()))
            {
                listener.WriteLine("A Message to write line");

                TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.First();

                Assert.IsNull(TelemetrySender.ValidateEndpointSend(telemetry));
            }
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerTraceEventWithMessage()
        {
            TraceOptions options = TraceOptions.Timestamp;
            var expectedTraceEventType = TraceEventType.Information;
            var expectedMessage = "A simple message";
            var expectedEventId = 3;

            this.ValidateASingleMessageActionBased(
                (ApplicationInsightsTraceListener listener, TraceEventCache cache) =>
                listener.TraceEvent(cache, this.TestContext.TestName, expectedTraceEventType, expectedEventId, expectedMessage),
                this.adapterHelper.InstrumentationKey,
                options);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(expectedMessage, telemetry.Message);
            Assert.AreEqual(expectedEventId.ToString(InvariantCulture), telemetry.Properties["EventId"]);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerTraceEventWithFormat()
        {
            TraceOptions options = TraceOptions.Timestamp;
            var formatString = "{0} event";
            int arg0 = 1;
            var expectedTraceEventType = TraceEventType.Information;
            var expectedEventId = 3;

            this.ValidateASingleMessageActionBased(
                (ApplicationInsightsTraceListener listener, TraceEventCache cache) =>
                listener.TraceEvent(cache, this.TestContext.TestName, expectedTraceEventType, expectedEventId, formatString, arg0),
                this.adapterHelper.InstrumentationKey,
                options);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(string.Format(InvariantCulture, formatString, arg0), telemetry.Message);
            Assert.AreEqual(expectedEventId.ToString(InvariantCulture), telemetry.Properties["EventId"]);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceDataStoresDataConvertedToStringInTraceMessage()
        {
            object expectedData = new Tuple<int, double>(123, 123.456);
            TraceOptions options = TraceOptions.Timestamp;
            var expectedTraceEventType = TraceEventType.Information;
            var expectedEventId = 3;

            this.ValidateASingleMessageActionBased(
                (ApplicationInsightsTraceListener listener, TraceEventCache cache) =>
                listener.TraceData(cache, "hello", expectedTraceEventType, expectedEventId, expectedData),
                this.adapterHelper.InstrumentationKey,
                options);
            
            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual("(123, 123.456)", telemetry.Message);
            Assert.AreEqual(expectedEventId.ToString(InvariantCulture), telemetry.Properties["EventId"]);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerStoresDataItemsConvertedToStringInTraceMessage()
        {
            object[] expectedData = new object[] { new Tuple<int, double>(123, 123.456), new Uri("https://foobar") };
            TraceOptions options = TraceOptions.Timestamp;
            var expectedTraceEventType = TraceEventType.Information;
            var expectedEventId = 3;

            this.ValidateASingleMessageActionBased(
                (ApplicationInsightsTraceListener listener, TraceEventCache cache) =>
                listener.TraceData(cache, "hello", expectedTraceEventType, expectedEventId, expectedData),
                this.adapterHelper.InstrumentationKey,
                options);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual("(123, 123.456), https://foobar/", telemetry.Message);
            Assert.AreEqual(expectedEventId.ToString(InvariantCulture), telemetry.Properties["EventId"]);
        }

#if NET452
        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsResumeAsVerbose()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Resume, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Verbose, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsStartAsVerbose()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Start, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Verbose, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsStopAsVerbose()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Stop, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Verbose, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsSuspendAsVerbose()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Suspend, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Verbose, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsTransferAsVerbose()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Transfer, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Verbose, telemetry.SeverityLevel);
        }
#endif

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsVerboseAsVerbose()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Verbose, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Verbose, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsInformationAsInformation()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Information, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Information, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsWarningAsWarning()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Warning, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Warning, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsErrorAsError()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Error, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Error, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("TraceListener")]
        public void TraceListenerSendsCriticalAsCritical()
        {
            this.ValidateASingleMessageActionBased(
                (listener, cache) => listener.TraceData(cache, "hello", TraceEventType.Critical, 3),
                this.adapterHelper.InstrumentationKey,
                TraceOptions.Timestamp);

            TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
            Assert.AreEqual(SeverityLevel.Critical, telemetry.SeverityLevel);
        }
        
        [TestMethod]
        public void TraceEventDoesNotThrowArgumentNullExceptionWhenArgsIsNull()
        {
            var source = new TraceSource("Test", SourceLevels.All);
            source.Listeners.Add(new ApplicationInsightsTraceListener());
            source.TraceInformation("TestMessage");
        }

#if NET452
        [TestMethod]
        public void TraceEventDoesNotStoreLogicalOperationStackInTelemetryPropertiesBecauseLongValuesAreRejected()
        {
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
            var listener = new ApplicationInsightsTraceListener();
            listener.TraceOutputOptions = TraceOptions.LogicalOperationStack;            

            Trace.CorrelationManager.StartLogicalOperation(42);
            listener.TraceEvent(new TraceEventCache(), string.Empty, TraceEventType.Information, default(int));

            var telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.Single();
            Assert.IsFalse(telemetry.Properties.ContainsKey("LogicalOperationStack"));
        }

        [TestMethod]
        public void TraceEventDoesNotStoreCallStackInTelemetryPropertiesBecauseLongValuesAreRejected()
        {
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
            var listener = new ApplicationInsightsTraceListener();
            listener.TraceOutputOptions = TraceOptions.Callstack;

            listener.TraceEvent(new TraceEventCache(), string.Empty, TraceEventType.Information, default(int));

            var telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.Single();
            Assert.IsFalse(telemetry.Properties.ContainsKey("CallStack"));
        }
#endif

        [TestMethod]
        public void TraceListenerFlushesChannel()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

            using (var listener = new ApplicationInsightsTraceListener(Guid.NewGuid().ToString()))
            {
                try
                {
                    listener.Flush();
                    Assert.Fail();
                }
                catch (Exception ex)
                {
                    Assert.AreEqual("Flush called", ex.Message);
                }
            }
        }

        private void ValidateASingleMessageActionBased(
            Action<ApplicationInsightsTraceListener, TraceEventCache> callTraceAction,
            string instrumentationKey,
            TraceOptions options)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

            using (var listener = new ApplicationInsightsTraceListener(instrumentationKey))
            {
                listener.TraceOutputOptions = options;
                TraceEventCache traceEventCache = new TraceEventCache();
#if NET452
                PrivateObject privateObject = new PrivateObject(traceEventCache);
                privateObject.SetField("timeStamp", DateTime.Now.Ticks);
                privateObject.SetField("stackTrace", "Environment.StackTrace");
#else
                TypeInfo traceEventCacheType = traceEventCache.GetType().GetTypeInfo();
                traceEventCacheType.GetDeclaredField("_timeStamp").SetValue(traceEventCache, DateTime.Now.Ticks);
#endif
                callTraceAction(listener, traceEventCache);

                TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.FirstOrDefault();
                Assert.IsNotNull(telemetry, "didn't got the event trace to the inner channel");
                Assert.AreEqual(telemetry.Context.InstrumentationKey, instrumentationKey);
            }
        }
        
        private void VerifyMessagesInMockChannel(
            ApplicationInsightsTraceListener aiListener,
            string instrumentationKey)
        {
            aiListener.Write("Sample Write message");
            aiListener.WriteLine("Sample WriteLine message");
            aiListener.TraceEvent(new TraceEventCache(), "Sample TraceEvent", TraceEventType.Error, 0);
            aiListener.TraceData(new TraceEventCache(), "Sample warning message GUID:{0}", TraceEventType.Information, 0, Guid.NewGuid());

            AdapterHelper.ValidateChannel(this.adapterHelper, instrumentationKey, 4);
        }
    }
}
