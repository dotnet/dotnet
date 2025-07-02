#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;

    using Common;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class ProfilerHttpProcessingTest : IDisposable
    {
        #region Fields
        private const int TimeAccuracyMilliseconds = 150; // this may be big number when under debugger
        private const string TestInstrumentationKey = nameof(TestInstrumentationKey);
        private const string TestApplicationId = "cid-v1:" + nameof(TestApplicationId);
        private readonly OperationDetailsInitializer operationDetailsInitializer = new OperationDetailsInitializer();
        private TelemetryConfiguration configuration;
        private Uri testUrl = new Uri("http://www.microsoft.com/");
        private Uri testUrlNonStandardPort = new Uri("http://www.microsoft.com:911/");
        private List<ITelemetry> sendItems;
        private int sleepTimeMsecBetweenBeginAndEnd = 100;
        private Exception ex = new Exception();
        private ProfilerHttpProcessing httpProcessingProfiler;
        #endregion //Fields

        #region TestInitialize

        [TestInitialize]
        public void TestInitialize()
        {
            this.sendItems = new List<ITelemetry>();

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            this.configuration = new TelemetryConfiguration()
            {
                TelemetryChannel = new StubTelemetryChannel
                {
                    OnSend = telemetry => this.sendItems.Add(telemetry)
                },
                InstrumentationKey = TestInstrumentationKey,
                ApplicationIdProvider = new MockApplicationIdProvider(TestInstrumentationKey, TestApplicationId)
            };

            this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.configuration.TelemetryInitializers.Add(this.operationDetailsInitializer);

            this.httpProcessingProfiler = new ProfilerHttpProcessing(
                this.configuration,
                null,
                new ObjectInstanceBasedOperationHolder<DependencyTelemetry>(),
                setCorrelationHeaders: true,
                correlationDomainExclusionList: new List<string>(),
                injectLegacyHeaders: false,
                injectW3CHeaders: false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }
        #endregion //TestInitialize

        #region GetResponse

        /// <summary>
        /// Validates HttpProcessingProfiler returns correct operation for OnBeginForGetResponse.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler returns correct operation for OnBeginForGetResponse.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnBeginForGetResponse()
        {
            var request = WebRequest.Create(this.testUrl);
            DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForGetResponse(request);
            Assert.IsNull(operationReturned, "Operation returned should be null as all context is maintained internally");
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
        }

        /// <summary>
        /// Validates HttpProcessingProfiler sends correct telemetry on calling OnEndForGetResponse.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler sends correct telemetry on calling OnEndForGetResponse.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnEndForGetResponse()
        {
            var request = WebRequest.Create(this.testUrl);
            var returnObjectPassed = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);
            this.httpProcessingProfiler.OnBeginForGetResponse(request);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            var objectReturned = this.httpProcessingProfiler.OnEndForGetResponse(null, returnObjectPassed, request);
            stopwatch.Stop();

            Assert.AreSame(returnObjectPassed, objectReturned, "Object returned from OnEndForGetResponse processor is not the same as expected return object");
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, true, stopwatch.Elapsed.TotalMilliseconds, "200");
        }

        /// <summary>
        /// Validates if DependencyTelemetry sent contains the cross component correlation ID.
        /// </summary>
        [TestMethod]
        [Description("Validates if DependencyTelemetry sent contains the cross component correlation ID.")]
        public void RddTestHttpProcessingProfilerOnEndAddsAppIdToTargetField()
        {
            // Here is a sample App ID, since the test initialize method adds a random ikey and our mock getAppId method pretends that the appId for a given ikey is the same as the ikey.
            // This will not match the current component's App ID. Hence represents an external component.
            string ikey = "0935FC42-FE1A-4C67-975C-0C9D5CBDEE8E";
            string appId = ikey + "-appId";
            
            this.SimulateWebRequestResponseWithAppId(appId);

            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            Assert.AreEqual(this.testUrl.Host + " | " + appId, ((DependencyTelemetry)this.sendItems[0]).Target);
        }

        /// <summary>
        /// Validates that DependencyTelemetry sent does not contains the cross component correlation id when the caller and callee are the same component.
        /// </summary>
        [TestMethod]
        [Description("Validates DependencyTelemetry does not send correlation ID if the IKey is from the same component")]
        public void RddTestHttpProcessingProfilerOnEndDoesNotAddAppIdToTargetFieldForInternalComponents()
        {
            this.SimulateWebRequestResponseWithAppId(TestApplicationId);

            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");

            // As opposed to this.testUrl.Host + " | " + correlationId
            Assert.AreEqual(this.testUrl.Host, ((DependencyTelemetry)this.sendItems[0]).Target);
        }

        /// <summary>
        /// Ensures that the source request header is added when request is sent.
        /// </summary>
        [TestMethod]
        [Description("Ensures that the source request header is added when request is sent.")]
        public void RddTestHttpProcessingProfilerOnBeginAddsSourceHeader()
        {
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);

            this.httpProcessingProfiler.OnBeginForGetResponse(request);
            Assert.IsNotNull(request.Headers.GetNameValueHeaderValue(
                RequestResponseHeaders.RequestContextHeader, 
                RequestResponseHeaders.RequestContextCorrelationSourceKey));
        }

#if !NET452
        /// <summary>
        /// Ensures that the source request header is added when request is sent.
        /// </summary>
        [TestMethod]
        public void RddTestHttpProcessingProfilerOnBeginAddsLegacyHeadersAreEnabled()
        {
            var request = WebRequest.Create(this.testUrl);
            Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);

            var httpProcessingLegacyHeaders = new ProfilerHttpProcessing(
                this.configuration,
                null,
                new ObjectInstanceBasedOperationHolder<DependencyTelemetry>(),
                setCorrelationHeaders: true,
                correlationDomainExclusionList: new List<string>(),
                injectLegacyHeaders: true,
                injectW3CHeaders: false);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                httpProcessingLegacyHeaders.OnBeginForGetResponse(request);

                var actualRootId = request.Headers[RequestResponseHeaders.StandardRootIdHeader];

                Assert.IsNotNull(actualRootId);
                Assert.AreEqual(op.Telemetry.Context.Operation.Id, actualRootId);

                var actualParentIdHeader = request.Headers[RequestResponseHeaders.StandardParentIdHeader];
                Assert.IsNotNull(actualParentIdHeader);
                Assert.AreNotEqual(op.Telemetry.Id, actualParentIdHeader);

                Assert.AreEqual(actualParentIdHeader, request.Headers[RequestResponseHeaders.RequestIdHeader]);
            }
        }
#endif

        /// <summary>
        /// Ensures that the parent id header is added when request is sent.
        /// </summary>
        [TestMethod]
        public void RddTestHttpProcessingProfilerOnBegin()
        {
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                this.httpProcessingProfiler.OnBeginForGetResponse(request);

                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];
                var actualTraceparentHeader = request.Headers[W3C.W3CConstants.TraceParentHeader];

                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.IsNull(request.Headers[W3C.W3CConstants.TraceStateHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.CorrelationContextHeader]);

                var parentActivity = Activity.Current;
                Assert.IsTrue(actualTraceparentHeader.StartsWith($"00-{parentActivity.TraceId.ToHexString()}-", StringComparison.Ordinal));
                var spanId = actualTraceparentHeader.Split('-')[2];
                Assert.AreEqual($"|{parentActivity.TraceId.ToHexString()}.{spanId}.", actualRequestIdHeader);

                Assert.AreNotEqual(parentActivity.Id, actualTraceparentHeader);
            }
        }

        [TestMethod]
        public void RddTestHttpProcessingProfilerOnBeginW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            var request = WebRequest.Create(this.testUrl);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                this.httpProcessingProfiler.OnBegin(request);

                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];

                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.IsNull(request.Headers[W3C.W3CConstants.TraceParentHeader]);
                Assert.IsNull(request.Headers[W3C.W3CConstants.TraceStateHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.CorrelationContextHeader]);

                var parentActivity = Activity.Current;
                Assert.IsTrue(actualRequestIdHeader.StartsWith(parentActivity.Id, StringComparison.Ordinal));
                Assert.AreNotEqual(parentActivity.Id, actualRequestIdHeader);
            }
        }

        [TestMethod]
        public void RddTestHttpProcessingProfilerOnBeginW3COnRequestIdOffIsIgnored()
        {
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                var httpDesktopProcessingFrameworkRequestIdOff = new DesktopDiagnosticSourceHttpProcessing(
                    this.configuration,
                    new CacheBasedOperationHolder("testCache", 100 * 1000),
                    setCorrelationHeaders: true,
                    correlationDomainExclusionList: new List<string>(),
                    injectLegacyHeaders: false,
                    injectRequestIdInW3cMode: false);

                httpDesktopProcessingFrameworkRequestIdOff.OnBegin(request);

                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];
                var actualTraceparentHeader = request.Headers[W3C.W3CConstants.TraceParentHeader];

                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.IsNull(request.Headers[W3C.W3CConstants.TraceStateHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.CorrelationContextHeader]);

                var parentActivity = Activity.Current;
                Assert.IsTrue(actualTraceparentHeader.StartsWith($"00-{parentActivity.TraceId.ToHexString()}-", StringComparison.Ordinal));
                var spanId = actualTraceparentHeader.Split('-')[2];
                Assert.AreEqual($"|{parentActivity.TraceId.ToHexString()}.{spanId}.", actualRequestIdHeader);

                Assert.AreNotEqual(parentActivity.Id, actualTraceparentHeader);
            }
        }

        /// <summary>
        /// Ensures that the correlation context header is added when request is sent.
        /// </summary>
        [TestMethod]
        public void RddTestHttpProcessingProfilerOnBeginAddsCorrelationContextHeader()
        {
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.CorrelationContextHeader]);
            var activity = new Activity("test").AddBaggage("Key1", "Value1").AddBaggage("Key2", "Value2").Start();
            this.httpProcessingProfiler.OnBeginForGetResponse(request);

            var actualCorrelationContextHeader = request.Headers[RequestResponseHeaders.CorrelationContextHeader];
            Assert.IsNotNull(actualCorrelationContextHeader);
            Assert.IsTrue(actualCorrelationContextHeader == "Key2=Value2,Key1=Value1" || actualCorrelationContextHeader == "Key1=Value1,Key2=Value2");
        }

#if !NET452
        [TestMethod]
        public void RddTestHttpProcessingProfilerOnBeginOnEnd()
        {
            var request = WebRequest.Create(this.testUrl);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                Activity.Current.AddBaggage("k", "v");
                Activity.Current.TraceStateString = "some=state";
                this.httpProcessingProfiler.OnBeginForGetResponse(request);

                Assert.AreEqual("k=v", request.Headers[RequestResponseHeaders.CorrelationContextHeader]);
                Assert.AreEqual("some=state", request.Headers[W3C.W3CConstants.TraceStateHeader]);

                var returnObjectPassed = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);
                this.httpProcessingProfiler.OnEndForEndGetResponse(null, returnObjectPassed, request, null);
            }

            Assert.AreEqual(2, this.sendItems.Count);
            var dependencies = this.sendItems.OfType<DependencyTelemetry>().ToArray();

            Assert.AreEqual(1, dependencies.Length);
            var dependency = dependencies.Single();
            Assert.IsNotNull(dependency);

            var dependencyIdParts = dependency.Id.Split('.', '|');
            Assert.AreEqual(4, dependencyIdParts.Length);

            var traceParent = request.Headers[W3C.W3CConstants.TraceParentHeader];
            Assert.AreEqual($"00-{dependencyIdParts[1]}-{dependencyIdParts[2]}-00", traceParent);

            Assert.AreEqual(2, dependency.Properties.Count);
            Assert.IsTrue(dependency.Properties.TryGetValue("tracestate", out var tracestate));
            Assert.AreEqual("some=state", tracestate);

            Assert.IsTrue(dependency.Properties.TryGetValue("k", out var baggageK));
            Assert.AreEqual("v", baggageK);
        }
#endif

        [TestMethod]
        public void RddTestHttpProcessingProfilerOnBeginOnEndW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            var request = WebRequest.Create(this.testUrl);

            Activity parentActivity = null;
            
            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                parentActivity = Activity.Current;
                parentActivity.AddBaggage("k", "v");
                this.httpProcessingProfiler.OnBeginForGetResponse(request);

                Assert.AreEqual("k=v", request.Headers[RequestResponseHeaders.CorrelationContextHeader]);

                var returnObjectPassed = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);
                this.httpProcessingProfiler.OnEndForEndGetResponse(null, returnObjectPassed, request, null);
            }

            Assert.AreEqual(2, this.sendItems.Count);
            var dependencies = this.sendItems.OfType<DependencyTelemetry>().ToArray();

            Assert.AreEqual(1, dependencies.Length);
            var dependency = dependencies.Single();
            Assert.IsNotNull(dependency);

            Assert.IsNull(request.Headers[W3C.W3CConstants.TraceParentHeader]);
            Assert.IsTrue(dependency.Id.StartsWith(parentActivity.Id));
            Assert.AreEqual(parentActivity.RootId, dependency.Context.Operation.Id);
            Assert.AreEqual(parentActivity.Id, dependency.Context.Operation.ParentId);
        }

        /// <summary>
        /// Ensures that the source request header is not added, as per the config, when request is sent.
        /// </summary>
        [TestMethod]
        [Description("Ensures that the source request header is not added when the config commands as such")]
        public void RddTestHttpProcessingProfilerOnBeginSkipsAddingSourceHeaderPerConfig()
        {
            string hostnamepart = "partofhostname";
            string url = string.Format(CultureInfo.InvariantCulture, "http://hostnamestart{0}hostnameend.com/path/to/something?param=1", hostnamepart);
            var request = WebRequest.Create(new Uri(url));

            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);
            Assert.AreEqual(0, request.Headers.Keys.Cast<string>().Where((x) => { return x.StartsWith("x-ms-", StringComparison.OrdinalIgnoreCase); }).Count());

            var httpProcessingProfiler = new ProfilerHttpProcessing(
                this.configuration, 
                null, 
                new ObjectInstanceBasedOperationHolder<DependencyTelemetry>(), 
                setCorrelationHeaders: false,
                correlationDomainExclusionList: new List<string>(),
                injectLegacyHeaders: true,
                injectW3CHeaders: false);
            httpProcessingProfiler.OnBeginForGetResponse(request);
            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);
            Assert.AreEqual(0, request.Headers.Keys.Cast<string>().Where((x) => { return x.StartsWith("x-ms-", StringComparison.OrdinalIgnoreCase); }).Count());

            ICollection<string> exclusionList = new SanitizedHostList() { "randomstringtoexclude", hostnamepart };
            httpProcessingProfiler = new ProfilerHttpProcessing(
                this.configuration, 
                null, 
                new ObjectInstanceBasedOperationHolder<DependencyTelemetry>(), 
                setCorrelationHeaders: true,
                    correlationDomainExclusionList: exclusionList,
                    injectLegacyHeaders: true,
                    injectW3CHeaders: false);
            httpProcessingProfiler.OnBeginForGetResponse(request);
            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);
            Assert.AreEqual(0, request.Headers.Keys.Cast<string>().Where((x) => { return x.StartsWith("x-ms-", StringComparison.OrdinalIgnoreCase); }).Count());
        }

        /// <summary>
        /// Ensures that the source request header is not overwritten if already provided by the user.
        /// </summary>
        [TestMethod]
        [Description("Ensures that the source request header is not overwritten if already provided by the user.")]
        public void RddTestHttpProcessingProfilerOnBeginDoesNotOverwriteExistingSource()
        {
            string sampleHeaderValueWithAppId = RequestResponseHeaders.RequestContextCorrelationSourceKey + "=HelloWorld";
            var request = WebRequest.Create(this.testUrl);

            request.Headers.Add(RequestResponseHeaders.RequestContextHeader, sampleHeaderValueWithAppId);

            this.httpProcessingProfiler.OnBeginForGetResponse(request);
            var actualHeaderValue = request.Headers[RequestResponseHeaders.RequestContextHeader];

            Assert.IsNotNull(actualHeaderValue);
            Assert.AreEqual(sampleHeaderValueWithAppId, actualHeaderValue);

            string sampleHeaderValueWithoutAppId = "helloWorld";
            request = WebRequest.Create(this.testUrl);

            request.Headers.Add(RequestResponseHeaders.RequestContextHeader, sampleHeaderValueWithoutAppId);

            this.httpProcessingProfiler.OnBeginForGetResponse(request);
            actualHeaderValue = request.Headers[RequestResponseHeaders.RequestContextHeader];

            Assert.IsNotNull(actualHeaderValue);
            Assert.AreNotEqual(sampleHeaderValueWithAppId, actualHeaderValue);
        }

        /// <summary>
        /// Validates HttpProcessingProfiler sends correct telemetry on calling OnExceptionForGetResponse.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler sends correct telemetry on calling OnExceptionForGetResponse.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnExceptionForGetResponse()
        {
            var request = WebRequest.Create(this.testUrl);
            var returnObjectPassed = new object();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.httpProcessingProfiler.OnBeginForGetResponse(request);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Exception exc = new Exception();
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            this.httpProcessingProfiler.OnExceptionForGetResponse(null, exc, request);
            stopwatch.Stop();

            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, false, stopwatch.Elapsed.TotalMilliseconds, string.Empty, responseExpected: false);
        }

        /// <summary>
        /// Validates HttpProcessingProfiler sends correct telemetry including response code on calling OnExceptionForGetResponse for WebException.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler sends correct telemetry including response code on calling OnExceptionForGetResponse for WebException.")]
        [Owner("mafletch")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnWebExceptionForGetResponse()
        {
            var request = WebRequest.Create(this.testUrl);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.httpProcessingProfiler.OnBeginForGetResponse(request);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            var returnObjectPassed = TestUtils.GenerateHttpWebResponse(HttpStatusCode.NotFound);
            Exception exc = new WebException("exception message", null, WebExceptionStatus.ProtocolError, returnObjectPassed);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            this.httpProcessingProfiler.OnExceptionForGetResponse(null, exc, request);
            stopwatch.Stop();

            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, false, stopwatch.Elapsed.TotalMilliseconds, "404");
        }

        /// <summary>
        /// Validates HttpProcessingProfiler OnBegin logs error into EventLog when passed invalid thisObject.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler OnBegin logs error into EventLog when passed invalid thisObject")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnBeginForGetResponseFailed()
        {
            using (var listener = new TestEventListener())
            {
                const long AllKeyword = -1;
                listener.EnableEvents(DependencyCollectorEventSource.Log, EventLevel.Verbose, (EventKeywords)AllKeyword);
                
                var request = WebRequest.Create(this.testUrl);
                DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForGetResponse(null);
                Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
                
                TestUtils.ValidateEventLogMessage(listener, "will not run for id", EventLevel.Warning);
            }
        }

        /// <summary>
        /// Validates HttpProcessingProfiler OnEnd logs error into EventLog when passed invalid thisObject.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler OnEnd logs error into EventLog when passed invalid thisObject")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnEndForGetResponseFailed()
        {
            using (var listener = new TestEventListener())
            {
                const long AllKeyword = -1;
                listener.EnableEvents(DependencyCollectorEventSource.Log, EventLevel.Warning, (EventKeywords)AllKeyword);
                
                var returnObjectPassed = new object();
                var request = WebRequest.Create(this.testUrl);
                DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForGetResponse(request);
                var objectReturned = this.httpProcessingProfiler.OnEndForGetResponse(null, returnObjectPassed, null);
                Assert.AreSame(returnObjectPassed, objectReturned, "Object returned from OnEndForGetResponse processor is not the same as expected return object");
                Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
                
                var message = listener.Messages.First(item => item.EventId == 14);
                Assert.IsNotNull(message);  
            }
        }

#endregion //GetResponse

#region GetRequestStream

        /// <summary>
        /// Validates HttpProcessingProfiler returns correct operation for OnBeginForGetRequestStream.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler returns correct operation for OnBeginForGetRequestStream.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnBeginForGetRequestStream()
        {
            var request = WebRequest.Create(this.testUrl);
            DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForGetRequestStream(request, null);
            Assert.IsNull(operationReturned, "Operation returned should be null as all context is maintained internally");
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
        }
        
        /// <summary>
        /// Validates HttpProcessingProfiler sends correct telemetry on calling OnExceptionForGetRequestStream.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler sends correct telemetry on calling OnExceptionForGetRequestStream.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnExceptionForGetRequestStream()
        {
            var request = WebRequest.Create(this.testUrl);
            var returnObjectPassed = new object();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.httpProcessingProfiler.OnBeginForGetResponse(request);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            Exception exc = new Exception();
            this.httpProcessingProfiler.OnExceptionForGetResponse(null, exc, request);
            stopwatch.Stop();

            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, false, stopwatch.Elapsed.TotalMilliseconds, string.Empty, responseExpected: false);
        }

#endregion //GetRequestStream

#region BeginGetResponse-EndGetResponse

        /// <summary>
        /// Validates HttpProcessingProfiler returns correct operation for OnBeginForBeginGetResponse.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler returns correct operation for OnBeginForBeginGetResponse.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnBeginForBeginGetResponse()
        {
            var request = WebRequest.Create(this.testUrl);
            DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForBeginGetResponse(request, null, null);
            Assert.IsNull(operationReturned, "For async methods, operation returned should be null as correlation is done internally using WeakTables.");
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
        }

        /// <summary>
        /// Validates HttpProcessingProfiler sends correct telemetry on calling OnEndForGetResponse.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler sends correct telemetry on calling OnEndForEndGetResponse.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnEndForEndGetResponse()
        {
            var request = WebRequest.Create(this.testUrl);
            var returnObjectPassed = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForBeginGetResponse(request, null, null);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            var objectReturned = this.httpProcessingProfiler.OnEndForEndGetResponse(operationReturned, returnObjectPassed, request, null);
            stopwatch.Stop();

            Assert.AreSame(returnObjectPassed, objectReturned, "Object returned from OnEndForEndGetResponse processor is not the same as expected return object");
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, true, stopwatch.Elapsed.TotalMilliseconds, "200");
        }

        /// <summary>
        /// Validates HttpProcessingProfiler sends correct telemetry on calling OnEndForGetResponse when returned object has been disposed.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler sends correct telemetry on calling OnEndForEndGetResponse when returned object has been disposed.")]
        [Owner("mafletch")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnEndForEndGetResponseWithDisposedResponse()
        {
            var request = WebRequest.Create(this.testUrl);
            var returnObjectPassed = TestUtils.GenerateDisposedHttpWebResponse(HttpStatusCode.OK);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForBeginGetResponse(request, null, null);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            var objectReturned = this.httpProcessingProfiler.OnEndForEndGetResponse(operationReturned, returnObjectPassed, request, null);
            stopwatch.Stop();

            Assert.AreSame(returnObjectPassed, objectReturned, "Object returned from OnEndForEndGetResponse processor is not the same as expected return object");
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, false, stopwatch.Elapsed.TotalMilliseconds, string.Empty, responseExpected: false);
        }

        /// <summary>
        /// Validates HttpProcessingProfiler sends correct telemetry on calling OnExceptionForEndGetResponse.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler sends correct telemetry on calling OnExceptionForGetResponse.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnExceptionForEndGetResponse()
        {
            var request = WebRequest.Create(this.testUrl);
            var returnObjectPassed = new object();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForBeginGetResponse(request, null, null);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            Exception exc = new Exception();
            this.httpProcessingProfiler.OnExceptionForEndGetResponse(operationReturned, exc, request, null);
            stopwatch.Stop();

            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, false, stopwatch.Elapsed.TotalMilliseconds, string.Empty, responseExpected: false);
        }

#endregion //BeginGetResponse-EndGetResponse

#region BeginGetRequestStream-EndGetRequestStream

        /// <summary>
        /// Validates HttpProcessingProfiler returns correct operation for OnBeginForBeginGetRequestStream.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler returns correct operation for OnBeginForBeginGetRequestStream.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnBeginForBeginGetRequestStream()
        {
            var request = WebRequest.Create(this.testUrl);
            DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForBeginGetRequestStream(request, null, null);
            Assert.IsNull(operationReturned, "For async methods, operation returned should be null as correlation is done internally using WeakTables.");
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
        }

        /// <summary>
        /// Validates HttpProcessingProfiler sends correct telemetry on calling OnExceptionForEndGetRequestStream.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler sends correct telemetry on calling OnExceptionForEndGetRequestStream.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerOnExceptionForEndGetRequestStream()
        {
            var request = WebRequest.Create(this.testUrl);
            var returnObjectPassed = new object();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForBeginGetRequestStream(request, null, null);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            Exception exc = new Exception();
            this.httpProcessingProfiler.OnExceptionForEndGetRequestStream(operationReturned, exc, request, null, null);
            stopwatch.Stop();

            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, false, stopwatch.Elapsed.TotalMilliseconds, string.Empty, responseExpected: false);
        }

#endregion //BeginGetRequestStream-EndGetRequestStream

#region SyncScenarios

        /// <summary>
        /// Validates HttpProcessingProfiler calculates startTime from the start of very first GetRequestStream
        /// 1.create request
        /// 2.request.GetRequestStream
        /// 3.request.GetRequestStream
        /// 4.request.GetRequestStream
        /// 5.request.GetResponse
        /// The expected time is the time between 2 and 5.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler calculates startTime from the start of very first GetRequestStream")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerStartTimeFromGetRequestStream()
        {
            var request = WebRequest.Create(this.testUrl);
            var returnObjectPassed = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.httpProcessingProfiler.OnBeginForGetRequestStream(request, null);
            this.httpProcessingProfiler.OnBeginForGetRequestStream(request, null);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            this.httpProcessingProfiler.OnBeginForGetRequestStream(request, null);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);

            this.httpProcessingProfiler.OnBeginForGetResponse(request);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            this.httpProcessingProfiler.OnEndForGetResponse(null, returnObjectPassed, request);
            stopwatch.Stop();

            // These times should not be calculated as dependency times
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);

            Assert.AreEqual(1, this.sendItems.Count, "Exactly one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, true, stopwatch.Elapsed.TotalMilliseconds, "200");
        }

        /// <summary>
        /// Validates that HttpProcessingProfiler will sent RDD telemetry when GetRequestStream fails and GetResponse is not invoked
        /// 1.create request
        /// 2.request.GetRequestStream  fails.
        /// </summary>
        [TestMethod]
        [Description("Validates that HttpProcessingProfiler will sent RDD telemetry when GetRequestStream fails and GetResponse is not invoked.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerFailedGetRequestStream()
        {
            var request = WebRequest.Create(this.testUrl);
            
            this.httpProcessingProfiler.OnBeginForGetRequestStream(request, null);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            this.httpProcessingProfiler.OnExceptionForGetRequestStream(null, this.ex, request, null);

            Assert.AreEqual(1, this.sendItems.Count, "Exactly one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, false, 0, string.Empty, responseExpected: false);
        }
#endregion //SyncScenarios

#region AsyncScenarios

        /// <summary>
        /// Validates HttpProcessingProfiler calculates startTime from the start of very first BeginGetRequestStream if any
        /// 1.create request
        /// 2.request.BeginGetRequestStream
        /// 3.request.BeginGetResponse
        /// 4.request.EndGetResponse        
        /// The expected time is the time between 2 and 4.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler calculates startTime from the start of very first BeginGetRequestStream if any")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerStartTimeFromGetRequestStreamAsync()
        {
            var request = WebRequest.Create(this.testUrl);
            var returnObjectPassed = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);

            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.httpProcessingProfiler.OnBeginForBeginGetRequestStream(request, null, null);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            this.httpProcessingProfiler.OnBeginForBeginGetResponse(request, null, null);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            this.httpProcessingProfiler.OnEndForEndGetResponse(null, returnObjectPassed, request, null);
            stopwatch.Stop();

            Assert.AreEqual(1, this.sendItems.Count, "Exactly one telemetry item should be sent");
            this.ValidateTelemetryPacket(this.sendItems[0] as DependencyTelemetry, this.testUrl, RemoteDependencyConstants.HTTP, true, stopwatch.Elapsed.TotalMilliseconds, "200");
        }

#endregion AsyncScenarios

#region ProfilerCorrectlyPreventsRecursion
        
        /// <summary>
        /// Validates HttpProcessingProfiler sends correct telemetry on calling OnEndForGetResponse.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler filters out custom ApplicationInsights resource.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerGetResponseIgnoreCustomAppInsightsUrl()
        {
            Uri specificEndpointAddress = new Uri("http://localhost:8989");
            var currentChannel = this.configuration.TelemetryChannel;
            string currentEndpointAddress = null;

            if (currentChannel is InMemoryChannel)
            {
                currentEndpointAddress = currentChannel.EndpointAddress;
                currentChannel.EndpointAddress = specificEndpointAddress.ToString();
            }
            else
            {
                this.configuration.TelemetryChannel = new InMemoryChannel
                {
                    EndpointAddress = specificEndpointAddress.ToString()
                };
            }

            try
            {
                var request = WebRequest.Create(specificEndpointAddress);
                var returnObjectPassed = new object();
                this.httpProcessingProfiler.OnBeginForGetResponse(request);
                Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
                var objectReturned = this.httpProcessingProfiler.OnEndForGetResponse(null, returnObjectPassed, request);

                Assert.AreSame(returnObjectPassed, objectReturned, "Object returned from OnEndForGetResponse processor is not the same as expected return object");
                Assert.AreEqual(0, this.sendItems.Count, "No RDD packets should be created for AI urls.");
            }
            finally
            {
                this.configuration.TelemetryChannel = currentChannel;
                if (currentEndpointAddress != null)
                {
                    this.configuration.TelemetryChannel.EndpointAddress = currentEndpointAddress;
                }
            }
        }

#endregion

#region Misc

        /// <summary>
        /// Validates HttpProcessingProfiler determines resource name correctly for simple url.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler determines resource name correctly for simple url.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerResourceNameTestForSimpleUrl()
        {
            var request = WebRequest.Create(this.testUrl);
            var expectedName = this.testUrl;
            var actualResourceName = HttpProcessing.GetUrl(request);
            Assert.AreEqual(expectedName, actualResourceName, "HttpProcessingProfiler returned incorrect resource name");

            Assert.AreEqual(null, HttpProcessing.GetUrl(null));
        }

        /// <summary>
        /// Validates HttpProcessingProfiler determines resource name correctly for url with query string.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler determines resource name correctly for url with query string.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerResourceNameTestForUrlWithQueryString()
        {
            UriBuilder ub = new UriBuilder(this.testUrl);
            ub.Query = "querystring=1";
            var request = WebRequest.Create(ub.Uri);
            var expectedName = ub.Uri.ToString();
            var actualResourceName = HttpProcessing.GetUrl(request);
            Assert.AreEqual(expectedName, actualResourceName.ToString(), "HttpProcessingProfiler returned incorrect resource name");
        }

        /// <summary>
        /// Validates HttpProcessingProfiler determines resource name correctly for url with paths.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler determines resource name correctly for url with path.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerResourceNameTestForUrlWithPaths()
        {
            UriBuilder ub = new UriBuilder(this.testUrl);
            ub.Path = "/rewards";
            var request = WebRequest.Create(ub.Uri);
            var expectedName = ub.Uri.ToString();
            var actualResourceName = HttpProcessing.GetUrl(request);
            Assert.AreEqual(expectedName, actualResourceName.ToString(), "HttpProcessingProfiler returned incorrect resource name");
        }

        /// <summary>
        /// Validates HttpProcessingProfiler determines target correctly for url with non standard port.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler determines target correctly for url with non standard port.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerSetTargetForNonStandardPort()
        {
            var request = WebRequest.Create(this.testUrlNonStandardPort);
            var returnObjectPassed = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);
                        
            this.httpProcessingProfiler.OnBeginForGetRequestStream(request, null);
            this.httpProcessingProfiler.OnEndForGetResponse(null, returnObjectPassed, request);
                        
            Assert.AreEqual(1, this.sendItems.Count, "Exactly one telemetry item should be sent");
            DependencyTelemetry receivedItem = (DependencyTelemetry)this.sendItems[0];
            string expectedTarget = this.testUrlNonStandardPort.Host + ":" + this.testUrlNonStandardPort.Port;
            Assert.AreEqual(expectedTarget, receivedItem.Target, "HttpProcessingProfiler returned incorrect target for non standard port.");
        }

#endregion //Misc

#region LoggingTests

        /// <summary>
        /// Validates HttpProcessingProfiler logs to event log when resource name is null or empty.
        /// </summary>
        [TestMethod]
        [Description("Validates HttpProcessingProfiler logs to event log when resource name is null or empty.")]
        [Owner("cithomas")]
        [TestCategory("CVT")]
        public void RddTestHttpProcessingProfilerLogsWhenResourceNameIsNullOrEmpty()
        {
            using (var listener = new TestEventListener())
            {
                const long AllKeyword = -1;
                listener.EnableEvents(DependencyCollectorEventSource.Log, EventLevel.Warning, (EventKeywords)AllKeyword);
                
                // pass any object other than WebRequest so that Processor will fail to extract any url/name
                var request = new object();
                DependencyTelemetry operationReturned = (DependencyTelemetry)this.httpProcessingProfiler.OnBeginForGetResponse(request);
                
                Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
                TestUtils.ValidateEventLogMessage(listener, "UnexpectedCallbackParameter", EventLevel.Warning);
            }
        }

#endregion //LoggingTests

#region Disposable
        public void Dispose()
        {
            this.configuration.Dispose();
            GC.SuppressFinalize(this);
        }
#endregion Disposable

#region Helpers

        private void ValidateTelemetryPacket(
            DependencyTelemetry remoteDependencyTelemetryActual,
            Uri uri,
            string type,
            bool success,
            double expectedValue,
            string resultCode,
            bool responseExpected = true)
        {
            Assert.AreEqual("GET " + uri.AbsolutePath, remoteDependencyTelemetryActual.Name, true, "Resource name in the sent telemetry is wrong");
            Assert.AreEqual(uri.Host, remoteDependencyTelemetryActual.Target, true, "Resource target in the sent telemetry is wrong");
            Assert.AreEqual(uri.OriginalString, remoteDependencyTelemetryActual.Data, true, "Resource data in the sent telemetry is wrong");
            Assert.AreEqual(type.ToString(), remoteDependencyTelemetryActual.Type, "DependencyKind in the sent telemetry is wrong");
            Assert.AreEqual(success, remoteDependencyTelemetryActual.Success, "Success in the sent telemetry is wrong");
            Assert.AreEqual(resultCode, remoteDependencyTelemetryActual.ResultCode, "ResultCode in the sent telemetry is wrong");

            this.operationDetailsInitializer.ValidateOperationDetailsDesktop(remoteDependencyTelemetryActual, responseExpected, headersExpected: false);

            var valueMinRelaxed = expectedValue - TimeAccuracyMilliseconds;
            Assert.IsTrue(
                remoteDependencyTelemetryActual.Duration >= TimeSpan.FromMilliseconds(valueMinRelaxed),
                string.Format(CultureInfo.InvariantCulture, "Value (dependency duration = {0}) in the sent telemetry should be equal or more than the time duration between start and end", remoteDependencyTelemetryActual.Duration));

            var valueMax = expectedValue + TimeAccuracyMilliseconds;
            Assert.IsTrue(
                remoteDependencyTelemetryActual.Duration <= TimeSpan.FromMilliseconds(valueMax),
                string.Format(CultureInfo.InvariantCulture, "Value (dependency duration = {0}) in the sent telemetry should not be significantly bigger than the time duration between start and end", remoteDependencyTelemetryActual.Duration));

            string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule), prefix: "rddp:");
            Assert.AreEqual(expectedVersion, remoteDependencyTelemetryActual.Context.GetInternalContext().SdkVersion);
        }

        private void SimulateWebRequestResponseWithAppId(string appId)
        {
            this.SimulateWebRequestWithGivenRequestContextHeaderValue(this.GetCorrelationIdHeaderValue(appId));
        }

        private void SimulateWebRequestWithGivenRequestContextHeaderValue(string headerValue)
        {
            var request = WebRequest.Create(this.testUrl);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(RequestResponseHeaders.RequestContextHeader, headerValue);

            var returnObjectPassed = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK, headers);

            this.httpProcessingProfiler.OnBeginForGetResponse(request);
            var objectReturned = this.httpProcessingProfiler.OnEndForGetResponse(null, returnObjectPassed, request);
        }
        
        private string GetCorrelationIdHeaderValue(string appId)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}={1}", RequestResponseHeaders.RequestContextCorrelationTargetKey, appId);
        }
#endregion Helpers
    }
}
#endif