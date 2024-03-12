namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Web;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.AspNet.TelemetryCorrelation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AspNetDiagnosticTelemetryModuleTest : IDisposable
    {
        private FakeAspNetDiagnosticSource aspNetDiagnosticsSource;
        private TelemetryConfiguration configuration;
        private IList<ITelemetry> sendItems;
        private AspNetDiagnosticTelemetryModule module;

        [TestInitialize]
        public void TestInit()
        {
            this.aspNetDiagnosticsSource = new FakeAspNetDiagnosticSource();
            this.sendItems = new List<ITelemetry>();
            var stubTelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sendItems.Add(item) };

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            this.configuration = new TelemetryConfiguration
            {
                InstrumentationKey = Guid.NewGuid().ToString(),
                TelemetryChannel = stubTelemetryChannel
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.Dispose(true);
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void InitializeWithoutRequestAndExceptionModulesMakesModuleNoop()
        {
            this.module = new AspNetDiagnosticTelemetryModule();
            this.module.Initialize(this.configuration);

            this.aspNetDiagnosticsSource.StartActivity();
            Assert.AreEqual(0, this.sendItems.Count);
        }

        [TestMethod]
        public void IsEnabledWithNullActivityDoesNotThrow()
        {
            this.module = this.CreateModule();

            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName));
            Assert.AreEqual(0, this.sendItems.Count);
        }

        [TestMethod]
        public void BeginEndRequestReportsTelemetry()
        {
            this.module = this.CreateModule();

            Assert.IsTrue(this.aspNetDiagnosticsSource.StartActivity());
            this.aspNetDiagnosticsSource.StopActivity();
            Assert.AreEqual(1, this.sendItems.Count);
        }

        [TestMethod]
        public void ChildTelemetryIsReportedProperlyBetweenBeginEndRequest()
        {
            this.module = this.CreateModule();

            this.aspNetDiagnosticsSource.StartActivity();

            var trace = new TraceTelemetry();
            var client = new TelemetryClient(this.configuration);
            client.TrackTrace(trace);

            this.aspNetDiagnosticsSource.StopActivity();
            Assert.AreEqual(2, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry ?? this.sendItems[1] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);

            Assert.AreEqual(requestTelemetry.Id, trace.Context.Operation.ParentId);
            Assert.AreEqual(requestTelemetry.Context.Operation.Id, trace.Context.Operation.Id);
        }

        // When telemetry is reported before AspNetDiagnosticsSource gets Start event
        // we create an activity in App Insights.
        // If this activity is lost on the way to BeginRequest on TelemteryCorrelation module
        // there is no way to correlate before/after telemetry -
        // TelemetryCorrelation module must be first in the pipeline, otherwise correlation is not guaranteed
        // see https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1049
        [Ignore]
        [TestMethod]
        public async Task TelemetryReportedBeforeAndAfterOnBeginAndLostActivity()
        {
            this.module = this.CreateModule();
            var client = new TelemetryClient(this.configuration);

            var trace1 = new TraceTelemetry("test1");
            await Task.Run(() =>
            {
                HttpContext.Current = this.aspNetDiagnosticsSource.FakeContext;
                client.TrackTrace(trace1);
            });

            HttpContext.Current = this.aspNetDiagnosticsSource.FakeContext;

            this.aspNetDiagnosticsSource.StartActivity();

            var trace2 = new TraceTelemetry("test2");
            client.TrackTrace(trace2);

            this.aspNetDiagnosticsSource.StopActivity();
            Assert.AreEqual(3, this.sendItems.Count);

            var requestTelemetry = this.sendItems.OfType<RequestTelemetry>().SingleOrDefault();
            Assert.IsNotNull(requestTelemetry);

            Assert.AreEqual(requestTelemetry.Context.Operation.Id, trace1.Context.Operation.Id);
            Assert.AreEqual(requestTelemetry.Context.Operation.Id, trace2.Context.Operation.Id);

            Assert.AreEqual(requestTelemetry.Id, trace1.Context.Operation.ParentId);
            Assert.AreEqual(requestTelemetry.Id, trace2.Context.Operation.ParentId);
        }

        [TestMethod]
        public void IsEnabledIsFalseIfRequestTelemetryIsCreatedAndCurrentActivityIsFromTelemetryCorrelation()
        {
            this.module = this.CreateModule();

            this.aspNetDiagnosticsSource.FakeContext = HttpModuleHelper.GetFakeHttpContext();
            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            activity.Start();
            var request = this.aspNetDiagnosticsSource.FakeContext.CreateRequestTelemetryPrivate();
            Assert.AreEqual(activity, Activity.Current);

            var anotherActivity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsFalse(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, anotherActivity, null));
        }

        [TestMethod]
        public void DoubleBeginEndRequestReportsOneTelemetry()
        {
            this.module = this.CreateModule();

            Assert.IsTrue(this.aspNetDiagnosticsSource.StartActivity());
            var activity = Activity.Current;
            Assert.IsTrue(this.aspNetDiagnosticsSource.StartActivity());

            // second Activity is ignored
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.IsNull(Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity(activity);

            Assert.AreEqual(1, this.sendItems.Count);
            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.IsNull(requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
            Assert.AreEqual(activity.SpanId.ToHexString(), requestTelemetry.Id);
        }

        [TestMethod]
        public void RequestTelemetryRequestIdWinsOverLegacyW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Request-Id"] = "|guid2.",
                    ["x-ms-request-id"] = "guid1",
                    ["x-ms-request-root-id"] = "guid2"
                });

            this.module = this.CreateModule("x-ms-request-root-id", "x-ms-request-id");

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));

            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();
            Assert.AreEqual("|guid2.", activity.ParentId);
            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("guid2", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|guid2.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.Id, requestTelemetry.Id);
        }

        [TestMethod]
        public void RequestTelemetryCustomHeadersW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            this.module = this.CreateModule("rootHeaderName", "parentHeaderName");
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["parentHeaderName"] = "ParentId",
                    ["rootHeaderName"] = "RootId"
                });

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual("RootId", activity.ParentId);
            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("RootId", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("ParentId", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.Id, requestTelemetry.Id);
        }

        [TestMethod]
        public void StandardHeadersWinOverLegacyHeadersW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Request-Id"] = "|requestId.",
                    ["x-ms-request-id"] = "legacy-id",
                    ["x-ms-request-root-id"] = "legacy-root-id"
                });

            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));

            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("requestId", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|requestId.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.Id, requestTelemetry.Id);
        }

        [TestMethod]
        public void TestActivityIdGenerationWithEmptyHeaders()
        {
            this.module = this.CreateModule();

            this.aspNetDiagnosticsSource.StartActivity();
            var activity = Activity.Current;
            this.aspNetDiagnosticsSource.StopActivity();
            
            Assert.AreEqual(1, this.sendItems.Count);

            var request = (RequestTelemetry)this.sendItems[0];
            Assert.AreEqual(activity.TraceId.ToHexString(), request.Context.Operation.Id);
            Assert.IsNull(request.Context.Operation.ParentId);
            Assert.AreEqual(activity.SpanId.ToHexString(), request.Id);

            Assert.IsFalse(request.Properties.ContainsKey("ai_legacyRootId"));
        }

        [TestMethod]
        public void TestActivityIdGenerationWithW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            this.module = this.CreateModule();
            
            this.aspNetDiagnosticsSource.StartActivity();
            Activity activity = Activity.Current;

            this.aspNetDiagnosticsSource.StopActivity();

            var request = this.sendItems.OfType<RequestTelemetry>().Single();

            Assert.AreEqual(activity.RootId, request.Context.Operation.Id);
            Assert.AreEqual(activity.ParentId, request.Context.Operation.ParentId);
            Assert.AreEqual(activity.Id, request.Id);
        }

        [TestMethod]
        public void W3CHeadersWinOverLegacy()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["traceparent"] = "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01",
                    ["x-ms-request-id"] = "legacy-id",
                    ["x-ms-request-rooit-id"] = "legacy-root-id"
                });

            this.module = this.CreateModule("x-ms-request-root-id", "x-ms-request-id");

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", activity.TraceId.ToHexString());
            Assert.AreEqual("00f067aa0ba902b7", activity.ParentSpanId.ToHexString());

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("00f067aa0ba902b7", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.AreEqual(0, requestTelemetry.Properties.Count);
            Assert.IsFalse(requestTelemetry.Properties.ContainsKey("ai_legacyRootId"));
        }

        [TestMethod]
        public void W3CHeadersWinOverRequestId()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["traceparent"] = "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01",
                    ["Request-Id"] = "|requestId."
                });

            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", activity.TraceId.ToHexString());
            Assert.AreEqual("00f067aa0ba902b7", activity.ParentSpanId.ToHexString());

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("00f067aa0ba902b7", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.IsFalse(requestTelemetry.Properties.ContainsKey("ai_legacyRootId"));
            Assert.AreEqual(0, requestTelemetry.Properties.Count);
        }

        [TestMethod]
        public void RequestIdBecomesParentWhenThereAreNoW3CHeaders()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Request-Id"] = "|requestId."
                });
            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);

            activity.Extract(HttpContext.Current.Request.Headers);

            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual(activity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|requestId.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.AreEqual(1, requestTelemetry.Properties.Count);
            Assert.IsTrue(requestTelemetry.Properties.TryGetValue("ai_legacyRootId", out var aiLegacyRootId));
            Assert.AreEqual("requestId", aiLegacyRootId);
        }

        [TestMethod]
        public void RequestIdBecomesParentAndRootIfCompatibleWhenThereAreNoW3CHeaders_ExceptionInScopeOfRequest()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Request-Id"] = "|4bf92f3577b34da6a3ce929d0e0e4736.",
                });
            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);

            // Activity is overwritten to match new W3C-compatible id

            var appInsightsActivity = Activity.Current;
            Assert.AreNotEqual(activity, appInsightsActivity);

            // here is where OnError happens
            this.aspNetDiagnosticsSource.FakeContext.AddError(new ArgumentNullException());

            // and then activity ends
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(2, this.sendItems.Count);

            var requestTelemetry = this.sendItems.OfType<RequestTelemetry>().FirstOrDefault();
            var exceptionTelemetry = this.sendItems.OfType<ExceptionTelemetry>().FirstOrDefault();

            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|4bf92f3577b34da6a3ce929d0e0e4736.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(appInsightsActivity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.IsNotNull(exceptionTelemetry);
            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", exceptionTelemetry.Context.Operation.Id);
            Assert.AreEqual(requestTelemetry.Id, exceptionTelemetry.Context.Operation.ParentId);

            Assert.IsFalse(requestTelemetry.Properties.ContainsKey("ai_legacyRootId"));
        }

        [TestMethod]
        public void RequestIdBecomesParentAndRootIfCompatibleWhenThereAreNoW3CHeaders()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Request-Id"] = "|4bf92f3577b34da6a3ce929d0e0e4736.",
                    ["Correlation-Context"] = "k=v",
                });
            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            var currentActivity = Activity.Current;

            // Activity is overwritten to match new W3C-compatible id
            Assert.AreNotEqual(activity, currentActivity);

            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", currentActivity.TraceId.ToHexString());
            Assert.AreEqual("0000000000000000", currentActivity.ParentSpanId.ToHexString());

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|4bf92f3577b34da6a3ce929d0e0e4736.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(currentActivity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.IsFalse(requestTelemetry.Properties.ContainsKey("ai_legacyRootId"));
            Assert.AreEqual(1, requestTelemetry.Properties.Count);
            Assert.IsTrue(requestTelemetry.Properties.TryGetValue("k", out var v));
            Assert.AreEqual("v", v);
        }

        [TestMethod]
        public void CustomHeadersBecomeParentWhenThereAreNoW3CHeaders()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["rootHeaderName"] = "root",
                    ["parentHeaderName"] = "parent"
                });
            this.module = this.CreateModule("rootHeaderName", "parentHeaderName");

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            activity.Extract(HttpContext.Current.Request.Headers);

            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual(activity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("parent", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.IsTrue(requestTelemetry.Properties.TryGetValue("ai_legacyRootId", out var legacyRootId));
            Assert.AreEqual("root", legacyRootId);
            Assert.AreEqual(1, requestTelemetry.Properties.Count);
        }

        [TestMethod]
        public void CorrelationContextIsReadWithoutTraceparentAndRequestId()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Correlation-Context"] = "k=v",
                });
            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            var currentActivity = Activity.Current;

            Assert.AreEqual(1, activity.Baggage.Count());
            Assert.AreEqual("k", activity.Baggage.Single().Key);
            Assert.AreEqual("v", activity.Baggage.Single().Value);

            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual(currentActivity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
            Assert.IsNull(requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(currentActivity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.AreEqual(1, requestTelemetry.Properties.Count);
            Assert.IsTrue(requestTelemetry.Properties.TryGetValue("k", out var v));
            Assert.AreEqual("v", v);
        }

        [TestMethod]
        public void CorrelationContextIsReadWithoutTraceparentAndRequestIdW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Correlation-Context"] = "k=v",
                });
            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            var currentActivity = Activity.Current;

            Assert.AreEqual(1, activity.Baggage.Count());
            Assert.AreEqual("k", activity.Baggage.Single().Key);
            Assert.AreEqual("v", activity.Baggage.Single().Value);

            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual(currentActivity.RootId, requestTelemetry.Context.Operation.Id);
            Assert.IsNull(requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(currentActivity.Id, requestTelemetry.Id);

            Assert.AreEqual(1, requestTelemetry.Properties.Count);
            Assert.IsTrue(requestTelemetry.Properties.TryGetValue("k", out var v));
            Assert.AreEqual("v", v);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private AspNetDiagnosticTelemetryModule CreateModule(string rootIdHeaderName = null, string parentIdHeaderName = null)
        {
            var initializer = new Web.OperationCorrelationTelemetryInitializer();
            if (rootIdHeaderName != null)
            {
                initializer.RootOperationIdHeaderName = rootIdHeaderName;
            }

            if (parentIdHeaderName != null)
            {
                initializer.ParentOperationIdHeaderName = parentIdHeaderName;
            }

            this.configuration.TelemetryInitializers.Add(new Extensibility.OperationCorrelationTelemetryInitializer());

            AspNetDiagnosticTelemetryModule result = new AspNetDiagnosticTelemetryModule();

            var requestModule = new RequestTrackingTelemetryModule()
            {
                EnableChildRequestTrackingSuppression = false
            };

            var exceptionModule = new ExceptionTrackingTelemetryModule();
            requestModule.Initialize(this.configuration);
            exceptionModule.Initialize(this.configuration);

            TelemetryModules.Instance.Modules.Add(requestModule);
            TelemetryModules.Instance.Modules.Add(exceptionModule);

            result.Initialize(this.configuration);

            return result;
        }

        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                this.aspNetDiagnosticsSource.Dispose();
                this.configuration.Dispose();
                this.module.Dispose();
            }
        }

        private class FakeAspNetDiagnosticSource : IDisposable
        {
            public const string IncomingRequestEventName = "Microsoft.AspNet.HttpReqIn";
            private const string AspNetListenerName = "Microsoft.AspNet.TelemetryCorrelation";

            private readonly DiagnosticListener listener;
            private HttpContext fakeContext;

            public FakeAspNetDiagnosticSource()
            {
                this.listener = new DiagnosticListener(AspNetListenerName);
                this.fakeContext = HttpModuleHelper.GetFakeHttpContext();
                HttpContext.Current = this.fakeContext;
            }

            public HttpContext FakeContext
            {
                get => this.fakeContext;

                set
                {
                    this.fakeContext = value;
                    HttpContext.Current = value;
                }
            }

            public bool IsEnabled(string eventName, object arg1 = null, object arg2 = null)
            {
                return this.listener.IsEnabled(eventName, arg1, arg2);
            }

            public void StartActivityWithoutChecks(Activity activity)
            {
                Debug.Assert(activity != null, "Activity is null");

                activity.Extract(this.fakeContext.Request.Headers);
                this.listener.OnActivityImport(activity, null);
                this.listener.StartActivity(activity, new { });
                this.fakeContext.Items["__AspnetActivity__"] = activity;
            }

            public bool StartActivity(Activity activity = null)
            {
                if (this.listener.IsEnabled() && this.listener.IsEnabled(IncomingRequestEventName))
                {
                    if (activity == null)
                    {
                        activity = new Activity(IncomingRequestEventName);

                        activity.Extract(this.fakeContext.Request.Headers);

                        this.listener.OnActivityImport(activity, null);
                    }

                    if (this.listener.IsEnabled(IncomingRequestEventName, activity))
                    {
                        this.listener.StartActivity(activity, new { });
                        
                        // simulate TelemetryCorrelation module
                        this.fakeContext.Items["__AspnetActivity__"] = activity;
                    }

                    return true;
                }

                return false;
            }

            public void StopActivity(Activity activity = null)
            {
                // simulate TelemetryCorrelation module
                var contextActivity = (Activity)this.FakeContext.Items["__AspnetActivity__"];
                if (Activity.Current != contextActivity)
                {
                    Activity.Current = contextActivity;
                }

                if (activity == null)
                {
                    this.listener.StopActivity(Activity.Current, new { });
                    return;
                }

                this.listener.StopActivity(activity, new { });
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool dispose)
            {
                if (dispose)
                {
                    this.listener.Dispose();
                }
            }
        }
    }
}