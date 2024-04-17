namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for DependencyCollectorDiagnosticListener.
    /// </summary>
    public partial class DependencyCollectorDiagnosticListenerTests
    {
        /// <summary>
        /// Tests that OnStartActivity injects headers.
        /// </summary>
        [TestMethod]
        public void OnActivityStartInjectsHeaders()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut");
            activity.AddBaggage("k", "v");
            activity.TraceStateString = "trace=state";
            activity.Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                // Request-Id and Correlation-Context are injected by HttpClient when W3C is off,
                // check W3C and legacy headers here
                var requestIds = requestMsg.Headers.GetValues(RequestResponseHeaders.RequestIdHeader).ToArray();
                Assert.AreEqual(1, requestIds.Length);
                Assert.AreEqual($"|{activity.TraceId.ToHexString()}.{activity.SpanId.ToHexString()}.", requestIds[0]);

                var traceparents = requestMsg.Headers.GetValues(W3C.W3CConstants.TraceParentHeader).ToArray();
                Assert.AreEqual(1, traceparents.Length);
                Assert.AreEqual(activity.Id, traceparents[0]);

                var tracestates = requestMsg.Headers.GetValues(W3C.W3CConstants.TraceStateHeader).ToArray();
                Assert.AreEqual(1, tracestates.Length);
                Assert.AreEqual("trace=state", tracestates[0]);

                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                Assert.AreEqual(this.testApplicationId1,
                    GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));
            }
        }

        /// <summary>
        /// Tests that OnStartActivity injects headers.
        /// </summary>
        [TestMethod]
        public void OnActivityStartInjectsHeadersRequestIdOff()
        {
            using (var listenerWithoutRequestId = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: false,
                HttpInstrumentationVersion.V2))
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut");
                activity.Start();

                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithoutRequestId.OnActivityStart(requestMsg);

                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestIdHeader));

                var traceparents = requestMsg.Headers.GetValues(W3C.W3CConstants.TraceParentHeader).ToArray();
                Assert.AreEqual(1, traceparents.Length);
                Assert.AreEqual(activity.Id, traceparents[0]);
            }
        }

        /// <summary>
        /// Tests that OnStartActivity injects headers.
        /// </summary>
        [TestMethod]
        public void OnActivityStartInjectsLegacyHeaders()
        {
            var listenerWithLegacyHeaders = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new[] { "excluded.host.com" },
                injectLegacyHeaders: true,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V2);

            using (listenerWithLegacyHeaders)
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut");
                activity.AddBaggage("k", "v");
                activity.Start();

                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithLegacyHeaders.OnActivityStart(requestMsg);

                // Request-Id and Correlation-Context are injected by HttpClient
                // check only legacy headers here
                Assert.AreEqual(activity.RootId,
                    requestMsg.Headers.GetValues(RequestResponseHeaders.StandardRootIdHeader).Single());
                Assert.AreEqual(activity.SpanId.ToHexString(), requestMsg.Headers.GetValues(RequestResponseHeaders.StandardParentIdHeader).Single());
                Assert.AreEqual(this.testApplicationId1, GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));
            }
        }

        /// <summary>
        /// Tests that OnStartActivity injects W3C headers.
        /// </summary>
        [TestMethod]
        public void OnActivityStartInjectsW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            using (var listenerWithoutW3CHeaders = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut").SetParentId("|guid.").Start();

                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithoutW3CHeaders.OnActivityStart(requestMsg);

                // Request-Id and Correlation-Context are injected by HttpClient
                // check only W3C headers here
                Assert.AreEqual(this.testApplicationId1, GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
            }
        }

        /// <summary>
        /// Tests that OnStopActivity tracks telemetry.
        /// </summary>
        [TestMethod]
        public async Task OnActivityStopTracksTelemetry()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut")
                .AddBaggage("k", "v")
                .Start();
            activity.TraceStateString = "state=some";

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);
                var approxStartTime = DateTime.UtcNow;
                activity = Activity.Current;

                await Task.Delay(10);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("POST /", telemetry.Name);
                Assert.AreEqual(RequestUrl, telemetry.Target);
                Assert.AreEqual(RemoteDependencyConstants.HTTP, telemetry.Type);
                Assert.AreEqual(RequestUrlWithScheme, telemetry.Data);
                Assert.AreEqual("200", telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                Assert.IsTrue(Math.Abs((telemetry.Timestamp - approxStartTime).TotalMilliseconds) < 100);
                Assert.IsTrue(telemetry.Duration.TotalMilliseconds > 10);

                Assert.AreEqual(activity.RootId, telemetry.Context.Operation.Id);
                Assert.IsNull(telemetry.Context.Operation.ParentId);

                Assert.AreEqual(activity.SpanId.ToHexString(), telemetry.Id);
                Assert.AreEqual("v", telemetry.Properties["k"]);

                Assert.AreEqual(32, telemetry.Context.Operation.Id.Length);
                Assert.IsTrue(Regex.Match(telemetry.Context.Operation.Id, @"[a-z][0-9]").Success);

                string expectedVersion =
                    SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule),
                        prefix: "rdddsc:");
                Assert.AreEqual(expectedVersion, telemetry.Context.GetInternalContext().SdkVersion);
                Assert.AreEqual("state=some", telemetry.Properties["tracestate"]);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }
        }

        /// <summary>
        /// Tests that OnStopActivity tracks telemetry.
        /// </summary>
        [TestMethod]
        public async Task OnActivityStopTracksTelemetryW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            var activity = new Activity("System.Net.Http.HttpRequestOut")
                .AddBaggage("k", "v")
                .Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                var approxStartTime = DateTime.UtcNow;
                activity = Activity.Current;

                await Task.Delay(10);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("POST /", telemetry.Name);
                Assert.AreEqual(RequestUrl, telemetry.Target);
                Assert.AreEqual(RemoteDependencyConstants.HTTP, telemetry.Type);
                Assert.AreEqual(RequestUrlWithScheme, telemetry.Data);
                Assert.AreEqual("200", telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                Assert.IsTrue(Math.Abs((telemetry.Timestamp - approxStartTime).TotalMilliseconds) < 100);
                Assert.IsTrue(telemetry.Duration.TotalMilliseconds > 10);

                Assert.AreEqual(activity.RootId, telemetry.Context.Operation.Id);
                Assert.IsNull(telemetry.Context.Operation.ParentId);

                Assert.AreEqual(activity.Id, telemetry.Id);
                Assert.AreEqual("v", telemetry.Properties["k"]);

                string expectedVersion =
                    SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule),
                        prefix: "rdddsc:");
                Assert.AreEqual(expectedVersion, telemetry.Context.GetInternalContext().SdkVersion);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }
        }

        /// <summary>
        /// Tests that activity wit parent id gets a new W3C compatible root id.
        /// </summary>
        [TestMethod]
        public void OnActivityWithParentId()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut")
                .SetParentId("00-0123456789abcdef0123456789abcdef-0123456789abcdef-01")
                .Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("0123456789abcdef0123456789abcdef", telemetry.Context.Operation.Id);
                Assert.AreEqual("0123456789abcdef",
                    telemetry.Context.Operation.ParentId);
            }
        }

        /// <summary>
        /// Tests that activity without parent does not get a new W3C compatible root id.
        /// </summary>
        [TestMethod]
        public void OnActivityWithParent()
        {
            var parent = new Activity("dummy").Start();
            new Activity("System.Net.Http.HttpRequestOut").Start();
 
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual(parent.RootId, telemetry.Context.Operation.Id);
                Assert.AreEqual(parent.SpanId.ToHexString(), telemetry.Context.Operation.ParentId);
            }
        }

        /// <summary>
        /// Tests that OnStopActivity tracks telemetry.
        /// </summary>
        [TestMethod]
        public void OnActivityStopWithParentTracksTelemetry()
        {
            var parent = new Activity("parent")
                .AddBaggage("k", "v")
                .Start();

            var activity = new Activity("System.Net.Http.HttpRequestOut").Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual(parent.RootId, telemetry.Context.Operation.Id);
                Assert.AreEqual(activity.ParentSpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(activity.SpanId.ToHexString(), telemetry.Id);
                Assert.AreEqual("v", telemetry.Properties["k"]);

                string expectedVersion =
                    SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule),
                        prefix: "rdddsc:");
                Assert.AreEqual(expectedVersion, telemetry.Context.GetInternalContext().SdkVersion);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }
        }

        /// <summary>
        /// Tests that OnStopActivity tracks cancelled request.
        /// </summary>
        [TestMethod]
        public void OnActivityStopTracksTelemetryForCanceledRequest()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut");
            activity.Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                listener.OnActivityStop(null, requestMsg, TaskStatus.Canceled);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Canceled", telemetry.ResultCode);
                Assert.AreEqual(false, telemetry.Success);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry, responseExpected: false);
            }
        }

        /// <summary>
        /// Tests that OnStopActivity tracks faulted request.
        /// </summary>
        [TestMethod]
        public void OnActivityStopTracksTelemetryForFaultedRequest()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut");
            activity.Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                listener.OnActivityStop(null, requestMsg, TaskStatus.Faulted);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Faulted", telemetry.ResultCode);
                Assert.AreEqual(false, telemetry.Success);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry, responseExpected: false);
            }
        }

        /// <summary>
        /// Tests HTTP dependencies and exceptions are NOT tracked for ApplicationInsights URL.
        /// </summary>
        [TestMethod]
        public void ApplicationInsightsUrlAreNotTracked()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut");
            activity.Start();

            var appInsightsUrl = TelemetryConfiguration.CreateDefault().TelemetryChannel.EndpointAddress;
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Get, appInsightsUrl);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.RequestContextCorrelationSourceKey));
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.RequestIdHeader));
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.StandardParentIdHeader));
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.StandardRootIdHeader));

                listener.OnActivityStop(null, requestMsg, TaskStatus.Faulted);

                Assert.IsFalse(this.sentTelemetry.Any());
            }
        }

        /// <summary>
        /// Call OnStartActivity() with uri that is in the excluded domain list.
        /// </summary>
        [TestMethod]
        public void OnStartActivityWithUriInExcludedDomainList()
        {
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, "http://excluded.host.com/path/to/file.html");
            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestContextHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.CorrelationContextHeader));
            }
        }

        [TestMethod]
        public void OnStartActivityWithUriInExcludedDomainListW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, "http://excluded.host.com/path/to/file.html");
            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestContextHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.CorrelationContextHeader));
            }
        }

        /// <summary>
        /// Tests that if OnStopActivity is called with null Activity, dependency is not tracked
        /// </summary>
        [TestMethod]
        public async Task OnActivityStopWithNullActivityDoesNotTrackDependency()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut")
                .AddBaggage("k", "v")
                .Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                await Task.Delay(10);

                activity = Activity.Current;
                activity.Stop();

                Assert.IsNull(Activity.Current);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                Assert.IsFalse(this.sentTelemetry.Any());
            }
        }

        [TestMethod]
        public void MultiHost_OnlyOneListenerTracksTelemetry()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var startEvent =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Start",
                    new { Request = request });
            var stopEvent =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Stop",
                    new { Request = request, Response = response, RequestTaskStatus = TaskStatus.RanToCompletion });

            using (var firstListener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            using (var secondListener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut").Start();

                firstListener.OnNext(startEvent);
                secondListener.OnNext(startEvent);

                firstListener.OnNext(stopEvent);
                secondListener.OnNext(stopEvent);

                Assert.AreEqual(1, this.sentTelemetry.Count(t => t is DependencyTelemetry));
            }
        }

        [TestMethod]
        public void MultiHost_TwoActiveAndOneIsDisposedStillTracksTelemetry()
        {
            var request1 = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
            var response1 = new HttpResponseMessage(HttpStatusCode.OK);

            var startEvent1 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Start",
                    new { Request = request1 });
            var stopEvent1 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Stop",
                    new { Request = request1, Response = response1, RequestTaskStatus = TaskStatus.RanToCompletion });

            var firstListener = this.CreateHttpListener(HttpInstrumentationVersion.V2);
            using (var secondListener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut").Start();
                firstListener.OnNext(startEvent1);
                secondListener.OnNext(startEvent1);

                firstListener.OnNext(stopEvent1);
                secondListener.OnNext(stopEvent1);

                Assert.AreEqual(1, this.sentTelemetry.Count(t => t is DependencyTelemetry));

                firstListener.Dispose();

                var request2 = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                var response2 = new HttpResponseMessage(HttpStatusCode.OK);

                var startEvent2 =
                    new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Start",
                        new { Request = request2 });
                var stopEvent2 =
                    new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Stop",
                        new { Request = request2, Response = response2, RequestTaskStatus = TaskStatus.RanToCompletion });

                activity = new Activity("System.Net.Http.HttpRequestOut").Start();
                secondListener.OnNext(startEvent2);
                secondListener.OnNext(stopEvent2);

                Assert.AreEqual(2, this.sentTelemetry.Count(t => t is DependencyTelemetry));
            }
        }

        [TestMethod]
        public void MultiHost_OneListenerThenAnotherTracksTelemetry()
        {
            var request1 = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
            var response1 = new HttpResponseMessage(HttpStatusCode.OK);

            var startEvent1 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Start",
                    new { Request = request1 });
            var stopEvent1 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Stop",
                    new { Request = request1, Response = response1, RequestTaskStatus = TaskStatus.RanToCompletion });

            var activity = new Activity("System.Net.Http.HttpRequestOut").Start();

            using (var firstListener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                firstListener.OnNext(startEvent1);
                firstListener.OnNext(stopEvent1);

                Assert.AreEqual(1, this.sentTelemetry.Count(t => t is DependencyTelemetry));

                firstListener.Dispose();
            }

            var request2 = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
            var response2 = new HttpResponseMessage(HttpStatusCode.OK);

            var startEvent2 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Start",
                    new { Request = request2 });
            var stopEvent2 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Stop",
                    new { Request = request2, Response = response2, RequestTaskStatus = TaskStatus.RanToCompletion });

            using (var secondListener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                activity = new Activity("System.Net.Http.HttpRequestOut").Start();
                secondListener.OnNext(startEvent2);
                secondListener.OnNext(stopEvent2);

                Assert.AreEqual(2, this.sentTelemetry.Count(t => t is DependencyTelemetry));
            }
        }

        private HttpCoreDiagnosticSourceListener CreateHttpListener(HttpInstrumentationVersion instrumentationVersion)
        {
            return new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[0],
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                instrumentationVersion);
        }
    }
}